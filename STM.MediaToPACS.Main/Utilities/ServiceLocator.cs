using MediaToPacs.Core.Auths;
using MediaToPacs.Core.Interfaces;
using MediaToPacs.Core.Models;
using MediaToPacs.Infrastructure.Auths;
using MediaToPacs.Infrastructure.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace STM.MediaToPACS.Main.Utilities
{
    public static class ServiceLocator
    {
        // ===================== STATE 
        public static bool IsInitialized { get; private set; }

        // ===================== CONFIG =====================
        public static SystemConfig SystemConfig { get; set; }

        // ===================== SERVICES =====================
        public static ISessionService SessionService { get; set; }
        public static IStudyService StudyService { get; private set; }
        public static IRisService RisService { get; private set; }
        public static IRisService2 RisService2 { get; private set; }
        public static ISignatureService SignatureService { get; private set; }
        public static IHisService HisService { get; private set; }

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
        private const string AppFolderName = "STM.MediaToPACS";

        /// <summary>
        /// Thư mục lưu cấu hình ứng dụng ở tầng ProgramData - ổn định qua các lần
        /// cập nhật/cài lại ứng dụng, không phụ thuộc ổ D (File:BasePath).
        /// </summary>
        public static string GetAppDataBasePath()
        {
            string basePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                AppFolderName);

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            return basePath;
        }

        /// <summary>
        /// Di chuyển các file cấu hình cũ (lưu ở File:BasePath, thường là ổ D) sang thư mục
        /// ProgramData mới nếu có, để không mất cấu hình đã lưu trước đó.
        /// </summary>
        private static void MigrateLegacyConfigIfNeeded(string basePath, params string[] fileNames)
        {
            try
            {
                string legacyBasePath = ConfigurationManager.AppSettings["File:BasePath"];
                if (string.IsNullOrWhiteSpace(legacyBasePath))
                    return;

                foreach (var fileName in fileNames)
                {
                    if (string.IsNullOrWhiteSpace(fileName))
                        continue;

                    string newPath = Path.Combine(basePath, fileName);
                    string legacyPath = Path.Combine(legacyBasePath, fileName);
                    if (!File.Exists(newPath) && File.Exists(legacyPath))
                    {
                        File.Copy(legacyPath, newPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không thể di chuyển cấu hình cũ từ File:BasePath");
            }
        }

        // ===================== INIT =====================
        public static void Initialize()
        {
            if (IsInitialized)
                return;

            string basePath = GetAppDataBasePath();
            string configFile = ConfigurationManager.AppSettings["SystemConfigFile"] ?? "SystemConfig.xml";
            string modalityFile = ConfigurationManager.AppSettings["Modality"] ?? "Modalities.xml";
            string cameraConfigFile = ConfigurationManager.AppSettings["File:CameraConfig"] ?? "CameraSettingConfig.xml";
            string shortcutFile = ConfigurationManager.AppSettings["File:ShortcutSettingsFile"] ?? "ShortcutSettingsFile.xml";

            MigrateLegacyConfigIfNeeded(basePath, configFile, modalityFile, cameraConfigFile, shortcutFile);

            string fullPath = Path.Combine(basePath, configFile);

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
                    UrlPacsServer = "http://10.12.8.16:8042",      // Máy chủ PACS
                    PacsUser = "stmadmin",
                    PacsPassword = "Anphat123!",
                    UrlViewerPacs = null,
                    UrlPacsPublic = "http://10.12.8.16:6038/MedicalViewer",
                    UrlSystemUpdate = "https://github.com/lehuy228/stm.ris.media",
                    SystemUpdateUser = null,
                    SystemUpdateToken = null,
                    UrlTokenPacs = "",
                    SystemUpdatePassword = null,
                    CheckThanhToan = "http://117.5.149.75:25117/api/KiemTraTien",
                    UrlSignatureMysign = "http://10.12.8.16:10005"
                };

                // Khởi tạo luôn file mặc định trên đĩa (không chỉ trong bộ nhớ),
                // để lần chạy sau/máy khác đọc thấy file đã tồn tại thay vì thiếu file.
                try
                {
                    XmlSettingsHelper.SaveEncrypted(fullPath, SystemConfig);
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Không thể khởi tạo file SystemConfig mặc định");
                }
            }

            InitializeServices();
            InitializeCaches();

            IsInitialized = true;
        }




        private static void InitializeServices()
        {
            //SessionService = new SessionService();
            StudyService = new StudyService();
            RisService = new RisService();
            RisService2 = new RisService2();
            SignatureService = new SignatureService();
            HisService = new HisService();
            ShortcutAndFontSetting = ShortcutSettingsManager.LoadOrCreateSettings();
            CameraSettingConfig = XmlSettingsHelper.Load<CameraSettings>(Path.Combine(
                GetAppDataBasePath(),
                ConfigurationManager.AppSettings["File:CameraConfig"]));
        }

        public static List<string> ValidateSystemConfig()
        {
            var warnings = new List<string>();

            if (SystemConfig == null)
            {
                warnings.Add("Hệ thống chưa được cấu hình.");
                return warnings;
            }

            // ===== PACS =====
            if (string.IsNullOrWhiteSpace(SystemConfig.UrlPacsServer))
                warnings.Add("Chưa cấu hình địa chỉ PACS Server.");

            if (string.IsNullOrWhiteSpace(SystemConfig.PacsUser))
                warnings.Add("Chưa cấu hình tài khoản PACS.");

            // ===== RIS =====
            if (string.IsNullOrWhiteSpace(SystemConfig.UrlApiRis))
                warnings.Add("Chưa cấu hình API RIS.");

            // ===== Thanh toán =====
            if (string.IsNullOrWhiteSpace(SystemConfig.CheckThanhToan))
                warnings.Add("Chưa cấu hình API kiểm tra thanh toán.");

            // ===== Update =====
            if (string.IsNullOrWhiteSpace(SystemConfig.UrlSystemUpdate))
                warnings.Add("Chưa cấu hình hệ thống cập nhật.");

            return warnings;
        }

        private static bool TryInitializeOrthancClient(out string errorMessage)
        {
            errorMessage = null;

            try
            {
                // Chỉ check cái thực sự bắt buộc cho PACS
                if (string.IsNullOrWhiteSpace(SystemConfig?.UrlPacsServer))
                {
                    errorMessage = "PACS Server chưa được cấu hình.";
                    return false;
                }

                _orthancClient?.Dispose();

                _orthancClient = new HttpClient
                {
                    BaseAddress = new Uri(SystemConfig.UrlPacsServer)
                };

                if (!string.IsNullOrWhiteSpace(SystemConfig.PacsUser))
                {
                    var auth = Convert.ToBase64String(
                        Encoding.ASCII.GetBytes($"{SystemConfig.PacsUser}:{SystemConfig.PacsPassword}")
                    );

                    _orthancClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Basic", auth);
                }

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
                if (string.IsNullOrWhiteSpace(SystemConfig?.UrlPacsServer))
                {
                    errorMessage = "Server ký số chưa được cấu hình.";
                    return false;
                }

                _signatureClient?.Dispose();

                _signatureClient = new HttpClient
                {
                    BaseAddress = new Uri(SystemConfig.UrlSignatureMysign)
                };

                SignatureService.LoadClient(_signatureClient);
                LoadSignatureUserAsync();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Không khởi tạo được kết nối PACS: " + ex.Message;
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

                if (string.IsNullOrWhiteSpace(SystemConfig?.UrlApiRis))
                {
                    errorMessage = "Chưa cấu hình API RIS.";
                    return false;
                }

                if (RisService is RisService ris)
                {
                    ris.Configure(SystemConfig.UrlApiRis);
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

                if (string.IsNullOrWhiteSpace(SystemConfig?.UrlApiRisV2))
                {
                    errorMessage = "Chưa cấu hình API RIS V2.";
                    return false;
                }

                if (RisService2 is RisService2 ris2)
                {
                    ris2.Configure(SystemConfig.UrlApiRisV2);
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
