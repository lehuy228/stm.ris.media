using MediaToPacs.Core.Auths;
using MediaToPacs.Core.Interfaces;
using MediaToPacs.Core.Models;
using MediaToPacs.Core.Utilities;
using MediaToPacs.Infrastructure.Auths;
using MediaToPacs.Infrastructure.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace STM.MediaToPACS.Main.Utilities
{
    public static class ServiceLocator
    {
        // ===================== STATE 
        public static bool IsInitialized { get; private set; }

        // ===================== CONFIG =====================
        public const string DefaultUrlSystemUpdate = "https://github.com/lehuy228/stm.ris.media";
        public static SystemConfig SystemConfig { get; set; }

        private static readonly object _configReloadSync = new object();
        private static FileSystemWatcher _systemConfigWatcher;
        private static Timer _systemConfigReloadTimer;
        private static string _systemConfigPath;
        private static int _isReloadingSystemConfig;

        // ===================== SERVICES =====================
        // internal: chỉ ServiceLocator (khởi tạo/cấu hình) và CompositionRoot (đăng ký DI) được
        // truy cập trực tiếp. Toàn bộ code UI phải nhận service qua constructor injection.
        internal static ISessionService SessionService { get; private set; }
        internal static IStudyService StudyService { get; private set; }
        internal static IRisService RisService { get; private set; }
        internal static IRisService2 RisService2 { get; private set; }
        internal static ISignatureService SignatureService { get; private set; }
        internal static IHisService HisService { get; private set; }

        // ===================== USER STATE =====================
        public static KeycloakUserInfo KeycloakUserInfo { get; set; }
        public static UserDto UserInfo { get; set; }
        public static string SelectedOrganizationCode { get; set; }

        // ===================== CACHE =====================
        public static ResultPage<ReportTemplate> ReportTemplates { get; set; }
        public static Dictionary<string, string> ReportCache { get; private set; }
        public static ShortcutAndFontSettings ShortcutAndFontSetting { get; set; }

        // ===================== HTTP =====================
        private static HttpClient _orthancClient;
        private static HttpClient _signatureClient;

        public static CameraSettings CameraSettingConfig { get; set; }

        // ===================== APP DATA PATH =====================

        /// <summary>
        /// Thư mục lưu cấu hình ứng dụng ở tầng ProgramData - ổn định qua các lần
        /// cập nhật/cài lại ứng dụng, không phụ thuộc ổ D (File:BasePath).
        /// </summary>
        public static string GetAppDataBasePath()
        {
            return AppDataPaths.GetAppDataBasePath();
        }

        /// <summary>
        /// Thư mục lưu ảnh/video chụp của bệnh nhân. Ưu tiên đường dẫn admin cấu hình trong
        /// SystemConfig.FileStoragePath (màn hình Cấu hình hệ thống); nếu chưa cấu hình hoặc
        /// không truy cập được thì dùng thư mục mặc định trong ProgramData.
        /// </summary>
        public static string GetMediaStorageBasePath()
        {
            string configuredPath = SystemConfig?.FileStoragePath;
            if (!string.IsNullOrWhiteSpace(configuredPath))
            {
                try
                {
                    if (!Directory.Exists(configuredPath))
                        Directory.CreateDirectory(configuredPath);

                    return configuredPath;
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Không thể sử dụng FileStoragePath đã cấu hình: {Path}", configuredPath);
                }
            }

            return GetAppDataBasePath();
        }

        // ===================== INIT =====================
        public static void Initialize()
        {
            if (IsInitialized)
                return;

            string basePath = GetAppDataBasePath();
            string configFile = FileStorageSettingsProvider.Current.SystemConfigFile;

            string fullPath = Path.Combine(basePath, configFile);
            _systemConfigPath = fullPath;

            // Luôn đảm bảo SystemConfig khác null (chưa cấu hình thì để rỗng, không phải null)
            // để tránh NullReferenceException ở những nơi gọi ServiceLocator.SystemConfig.Xyz
            // trước khi admin mở Settings và lưu lần đầu.
            SystemConfig = XmlSettingsHelper.LoadEncrypted<SystemConfig>(fullPath);
            if (SystemConfig == null)
            {
                SystemConfig = new SystemConfig
                {
                    UrlApiRis = "http://10.12.8.16:5006",          // URL API RIS
                    UrlRisAuthen = null,                            // URL RIS Authen
                    UrlApiRisV2 = "http://10.12.8.16:7002",        // URL API RIS V2
                    UrlGateway = "https://gateway.benhvien.vn",   // Gateway chung
                    UrlPacsServer = "http://10.12.8.16:8042",      // Máy chủ PACS
                    PacsUser = "stmadmin",
                    PacsPassword = "Anphat123!",
                    UrlViewerPacs = null,
                    UrlPacsPublic = "http://10.12.8.16:6038/MedicalViewer",
                    UrlSystemUpdate = DefaultUrlSystemUpdate,
                    SystemUpdateUser = null,
                    SystemUpdateToken = null,
                    UrlTokenPacs = "",
                    SystemUpdatePassword = null,
                    CheckThanhToan = "http://117.5.149.75:25117/api/KiemTraTien",
                    UrlSignatureMysign = "http://10.12.8.16:10005"
                };

                SaveDefaultSystemConfig(fullPath, "Không thể khởi tạo file SystemConfig mặc định");
            }
            else if (ApplySystemConfigDefaults(SystemConfig))
            {
                SaveDefaultSystemConfig(fullPath, "Không thể cập nhật giá trị mặc định cho SystemConfig");
            }

            InitializeServices();
            InitializeCaches();
            StartSystemConfigWatcher();

            IsInitialized = true;
        }

        private static void StartSystemConfigWatcher()
        {
            if (string.IsNullOrWhiteSpace(_systemConfigPath))
                return;

            string directory = Path.GetDirectoryName(_systemConfigPath);
            string fileName = Path.GetFileName(_systemConfigPath);
            if (string.IsNullOrWhiteSpace(directory) || string.IsNullOrWhiteSpace(fileName))
                return;

            Directory.CreateDirectory(directory);

            _systemConfigWatcher?.Dispose();
            _systemConfigReloadTimer?.Dispose();

            _systemConfigReloadTimer = new Timer(
                ReloadSystemConfigFromFile,
                null,
                Timeout.Infinite,
                Timeout.Infinite);

            _systemConfigWatcher = new FileSystemWatcher(directory, fileName)
            {
                NotifyFilter = NotifyFilters.LastWrite
                    | NotifyFilters.Size
                    | NotifyFilters.FileName
                    | NotifyFilters.CreationTime,
                IncludeSubdirectories = false,
                EnableRaisingEvents = true
            };

            _systemConfigWatcher.Changed += SystemConfigFileChanged;
            _systemConfigWatcher.Created += SystemConfigFileChanged;
            _systemConfigWatcher.Renamed += SystemConfigFileRenamed;
            _systemConfigWatcher.Error += SystemConfigWatcherError;
        }

        private static void SystemConfigFileChanged(object sender, FileSystemEventArgs e)
        {
            ScheduleSystemConfigReload();
        }

        private static void SystemConfigFileRenamed(object sender, RenamedEventArgs e)
        {
            ScheduleSystemConfigReload();
        }

        private static void SystemConfigWatcherError(object sender, ErrorEventArgs e)
        {
            Log.Warning(e.GetException(), "Lỗi theo dõi file SystemConfig; đang khởi tạo lại watcher");
            StartSystemConfigWatcher();
        }

        private static void ScheduleSystemConfigReload()
        {
            lock (_configReloadSync)
            {
                // Một lần lưu có thể phát sinh nhiều Changed event. Chờ file ghi ổn định rồi mới đọc.
                _systemConfigReloadTimer?.Change(600, Timeout.Infinite);
            }
        }

        private static void ReloadSystemConfigFromFile(object state)
        {
            if (Interlocked.Exchange(ref _isReloadingSystemConfig, 1) != 0)
                return;

            try
            {
                SystemConfig reloadedConfig = null;

                // Phần mềm khác có thể đang thay thế/ghi file. Thử lại ngắn trước khi bỏ qua lần reload.
                for (int attempt = 0; attempt < 3 && reloadedConfig == null; attempt++)
                {
                    reloadedConfig = XmlSettingsHelper.LoadEncrypted<SystemConfig>(_systemConfigPath);
                    if (reloadedConfig == null)
                        Thread.Sleep(250);
                }

                if (reloadedConfig == null)
                {
                    Log.Warning("Không thể đọc lại SystemConfig sau khi file thay đổi");
                    return;
                }

                ApplySystemConfigDefaults(reloadedConfig);
                SystemConfig = reloadedConfig;

                var warnings = InitializeOptionalServices();
                if (warnings.Count > 0)
                    Log.Warning("Đã nạp lại SystemConfig với cảnh báo: {Warnings}", string.Join("; ", warnings));
                else
                    Log.Information("Đã tự động nạp lại SystemConfig và cập nhật các service Gateway");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi tự động nạp lại SystemConfig");
            }
            finally
            {
                Interlocked.Exchange(ref _isReloadingSystemConfig, 0);
            }
        }




        public static bool ApplySystemConfigDefaults(SystemConfig config)
        {
            if (config == null)
                return false;

            var changed = false;
            if (string.IsNullOrWhiteSpace(config.UrlSystemUpdate))
            {
                config.UrlSystemUpdate = DefaultUrlSystemUpdate;
                changed = true;
            }

            if (string.IsNullOrWhiteSpace(config.UrlGateway))
            {
                config.UrlGateway = "https://gateway.benhvien.vn";
                changed = true;
            }

            return changed;
        }

        private static void SaveDefaultSystemConfig(string fullPath, string warningMessage)
        {
            try
            {
                XmlSettingsHelper.SaveEncrypted(fullPath, SystemConfig);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, warningMessage);
            }
        }

        private static void InitializeServices()
        {
            // Khởi tạo sớm để CompositionRoot có instance đăng ký vào DI container;
            // token thực sự được gán sau khi đăng nhập qua SessionService.SetToken(...).
            SessionService = new SessionService();
            StudyService = new StudyService();
            RisService = new RisService();
            RisService2 = new RisService2();
            SignatureService = new SignatureService();
            HisService = new HisService();
            ShortcutAndFontSetting = ShortcutSettingsManager.LoadOrCreateSettings();
            CameraSettingConfig = XmlSettingsHelper.Load<CameraSettings>(Path.Combine(
                GetAppDataBasePath(),
                FileStorageSettingsProvider.Current.CameraConfig));
        }

        public static List<string> ValidateSystemConfig()
        {
            var warnings = new List<string>();

            if (SystemConfig == null)
            {
                warnings.Add("Hệ thống chưa được cấu hình.");
                return warnings;
            }

            // ===== Gateway =====
            if (string.IsNullOrWhiteSpace(SystemConfig.UrlGateway))
                warnings.Add("Chưa cấu hình địa chỉ Gateway.");

            // ===== Thanh toán =====
            if (string.IsNullOrWhiteSpace(SystemConfig.CheckThanhToan))
                warnings.Add("Chưa cấu hình API kiểm tra thanh toán.");

            // ===== Update =====
            if (string.IsNullOrWhiteSpace(SystemConfig.UrlSystemUpdate))
                warnings.Add("Chưa cấu hình hệ thống cập nhật.");

            return warnings;
        }

        private static string BuildGatewayUrl(string routePrefix)
        {
            if (string.IsNullOrWhiteSpace(SystemConfig?.UrlGateway))
                throw new InvalidOperationException("Gateway chưa được cấu hình.");

            return SystemConfig.UrlGateway.Trim().TrimEnd('/') + "/" + routePrefix.TrimStart('/');
        }

        private static Uri BuildGatewayUri(string routePrefix)
        {
            return new Uri(BuildGatewayUrl(routePrefix) + "/", UriKind.Absolute);
        }

        private static string GetGatewayAccessToken()
        {
            return SessionService?.GetCurrentUser()?.AccessToken;
        }

        /// <summary>
        /// Khởi tạo tối thiểu RIS V2 để kiểm tra update trước khi đăng nhập.
        /// Endpoint update phải cho phép anonymous hoặc app-level authentication.
        /// </summary>
        public static void InitializeUpdateService()
        {
            if (!(RisService2 is RisService2 ris2))
                throw new InvalidOperationException("RIS V2 service chưa được khởi tạo.");

            ris2.Configure(BuildGatewayUrl(ApiEndpoints.Gateway.RisV2));
            ris2.ConfigureDirectFallback(SystemConfig?.UrlApiRisV2);
        }

        private static void ApplyGatewayAuthorization(HttpClient client)
        {
            string accessToken = GetGatewayAccessToken();
            client.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(accessToken)
                ? null
                : new AuthenticationHeaderValue("Bearer", accessToken);
        }

        private static bool TryInitializeOrthancClient(out string errorMessage)
        {
            errorMessage = null;

            try
            {
                // Chỉ check cái thực sự bắt buộc cho PACS
                if (string.IsNullOrWhiteSpace(SystemConfig?.UrlGateway))
                {
                    errorMessage = "PACS Server chưa được cấu hình.";
                    return false;
                }

                _orthancClient?.Dispose();

                _orthancClient = new HttpClient
                {
                    BaseAddress = BuildGatewayUri(ApiEndpoints.Gateway.Pacs)
                };
                ApplyGatewayAuthorization(_orthancClient);

                StudyService.LoadClient(_orthancClient);

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Không khởi tạo được kết nối PACS: " + ex.Message;
                return false;
            }
        }

        private static bool TryInitializeSignatureClient(out string errorMessage)
        {
            errorMessage = null;

            try
            {
                // Chỉ check cái thực sự bắt buộc cho PACS
                if (string.IsNullOrWhiteSpace(SystemConfig?.UrlGateway))
                {
                    errorMessage = "Server ký số chưa được cấu hình.";
                    return false;
                }

                _signatureClient?.Dispose();

                _signatureClient = new HttpClient
                {
                    BaseAddress = BuildGatewayUri(ApiEndpoints.Gateway.Signature)
                };
                ApplyGatewayAuthorization(_signatureClient);

                SignatureService.LoadClient(_signatureClient);
                LoadSignatureUserAsync();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Không khởi tạo được kết nối Signature qua Gateway: " + ex.Message;
                return false;
            }
        }

        public static async Task LoadSignatureUserAsync()
        {
            if (SignatureService == null || KeycloakUserInfo == null)
                return;

            try
            {
                UserInfo = await SignatureService.GetUserCert(
                    KeycloakUserInfo.CCCD
                );
            }
            catch (Exception ex)
            {
                // log warning, KHÔNG throw
            }
        }

        private static bool TryInitializeRisService(out string errorMessage)
        {
            errorMessage = null;

            try
            {
                if (RisService == null)
                    return false;

                if (string.IsNullOrWhiteSpace(SystemConfig?.UrlGateway))
                {
                    errorMessage = "Chưa cấu hình API RIS.";
                    return false;
                }

                if (RisService is RisService ris)
                {
                    ris.Configure(
                        BuildGatewayUrl(ApiEndpoints.Gateway.Ris),
                        GetGatewayAccessToken());
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                errorMessage = "Không khởi tạo được RIS: " + ex.Message;
                return false;
            }
        }

        private static bool TryInitializeRisService2(out string errorMessage)
        {
            errorMessage = null;

            try
            {
                if (RisService2 == null)
                    return false;

                if (string.IsNullOrWhiteSpace(SystemConfig?.UrlGateway))
                {
                    errorMessage = "Chưa cấu hình API RIS V2.";
                    return false;
                }

                if (RisService2 is RisService2 ris2)
                {
                    ris2.Configure(
                        BuildGatewayUrl(ApiEndpoints.Gateway.RisV2),
                        GetGatewayAccessToken());
                    ris2.ConfigureDirectFallback(SystemConfig.UrlApiRisV2);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                errorMessage = "Không khởi tạo được RIS V2: " + ex.Message;
                return false;
            }
        }

        public static List<string> InitializeOptionalServices()
        {
            var warnings = new List<string>();

            // ===== PACS / Orthanc =====
            if (!TryInitializeOrthancClient(out var pacsError))
            {
                if (!string.IsNullOrWhiteSpace(pacsError))
                    warnings.Add(pacsError);
            }

            // ===== RIS =====
            if (!TryInitializeRisService(out var risError))
            {
                if (!string.IsNullOrWhiteSpace(risError))
                    warnings.Add(risError);
            }

            // ===== RIS V2 =====
            if (!TryInitializeRisService2(out var risV2Error))
            {
                if (!string.IsNullOrWhiteSpace(risV2Error))
                    warnings.Add(risV2Error);
            }

            // ===== RIS =====
            if (!TryInitializeSignatureClient(out var sigError))
            {
                if (!string.IsNullOrWhiteSpace(risError))
                    warnings.Add(sigError);
            }


            // ===== HIS (nếu có sau này) =====
            // if (!TryInitializeHisService(out var hisError))
            // {
            //     if (!string.IsNullOrWhiteSpace(hisError))
            //         warnings.Add(hisError);
            // }

            return warnings;
        }

        private static void InitializeCaches()
        {
            ReportCache = new Dictionary<string, string>();
        }

        // ===================== SHUTDOWN =====================
        public static void Shutdown()
        {
            _orthancClient?.Dispose();
            _orthancClient = null;
            IsInitialized = false;
        }
    }
}
