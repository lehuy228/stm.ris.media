//using MediaToPacs.Core.Auths;
//using MediaToPacs.Core.Interfaces;
//using MediaToPacs.Core.Models;
//using MediaToPacs.Infrastructure.Auths;
//using MediaToPacs.Infrastructure.Services;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace STM.MediaToPACS.Main.Utilities
//{
//    public static class ServiceLocator
//    {
//        public static ISessionService SessionService { get; set; }
//        public static IRisService RisService { get; set; }
//        public static ISignatureService SignatureService { get; set; }
//        public static KeycloakUserInfo KeycloakUserInfo { get; set; }
//        public static ResultPage<ReportTemplate> ReportTemplates { get; set; }
//        public static UserDto UserInfo { get; set; }

//        public static ShortcutAndFontSettings ShortcutAndFontSetting { get; set; }
//        public static Dictionary<string, string> ReportCache;
//        public static StorageServer StorageServer { get; set; }
//        public static CameraSettings CameraSettingConfig { get; set; }
//        public static void Initialize()
//        {
//            SessionService = new SessionService();
//            RisService = new RisService();
//            SignatureService = new SignatureService();
//            ReportCache = new Dictionary<string, string>();
//            GetReportTemplates();
//            ShortcutAndFontSetting = ShortcutSettingsManager.LoadOrCreateSettings();
//            StorageServer = XmlSettingsHelper.Load<List<StorageServer>>(Path.Combine(
//                ConfigurationManager.AppSettings["File:BasePath"],
//                ConfigurationManager.AppSettings["File:PacsServerConfig"]
//            )).FirstOrDefault();
//            CameraSettingConfig = XmlSettingsHelper.Load<CameraSettings>(Path.Combine(
//                ConfigurationManager.AppSettings["File:BasePath"],
//                ConfigurationManager.AppSettings["File:CameraConfig"]
//            ));
//        }

//        public async static void GetReportTemplates()
//        {
//            ReportTemplates = await RisService.GetDanhSachMauKLAsync(modality:"ES");
//        }
//    }
//}



//using STM.MedicalWorkstation.Core.Auths;
//using STM.MedicalWorkstation.Core.Interfaces;
//using STM.MedicalWorkstation.Core.Models;
//using STM.MedicalWorkstation.Core.Models.BO;
//using STM.MedicalWorkstation.Infrastructure.Services;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.IO;
//using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Serialization;

//namespace STM.MedicalWorkstation.Utils
//{
//    public static class ServiceLocator
//    {
//        public static SystemConfig SystemConfig { get; set; }

//        public static ISessionService SessionService { get; set; }
//        public static IStudyService StudyService { get; private set; }
//        public static ISeriesService SeriesService { get; private set; }

//        public static IRisService RisService { get; private set; }
//        public static ISignatureService SignatureService { get; private set; }

//        public static IHisService HisService { get; private set; } 

//        public static KeycloakUserInfo keycloakUserInfo { get; set; }
//        public static ResultPage<ReportTemplate> ReportTemplates { get; set; }
//        public static UserDto UserInfo { get; set; }
//        public static Dictionary<string, string> ReportCache;

//        public static ShortcutAndFontSettings ShortcutAndFontSetting { get; set; }

//        public static void Initialize()
//        {
//            SystemConfig = XmlSettingsHelper.Load<SystemConfig>(Path.Combine(ConfigurationManager.AppSettings["BasePath"], ConfigurationManager.AppSettings["SystemConfigFile"] ?? "SystemConfig.xml"));

//            StudyService = new StudyService();
//            SeriesService = new SeriesService();
//            RisService = new RisService();
//            SignatureService = new SignatureService();
//            ReportCache = new Dictionary<string, string>();
//            HisService = new HisService();
//            ShortcutAndFontSetting = ShortcutSettingsManager.LoadOrCreateSettings();

//            if (SystemConfig != null)
//            {
//                LoadOrthancServer(SystemConfig);
//            }
//        }

//        public static void LoadOrthancServer(SystemConfig systemConfig)
//        {
//            string baseUrl = systemConfig.UrlPacsServer;
//            string username = systemConfig.PacsUser;
//            string password = systemConfig.PacsPassword;

//            var Client = new HttpClient { BaseAddress = new Uri(baseUrl) };

//            // encode username:password thành Base64
//            var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
//            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
//                "Basic", Convert.ToBase64String(byteArray)
//                );

//            SeriesService.LoadClient(Client);
//            StudyService.LoadClient(Client);
//        }

//    }
//}


using MediaToPacs.Core.Auths;
using MediaToPacs.Core.Interfaces;
using MediaToPacs.Core.Models;
using MediaToPacs.Infrastructure.Auths;
using MediaToPacs.Infrastructure.Services;
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
        public static ISignatureService SignatureService { get; private set; }
        public static IHisService HisService { get; private set; }

        // ===================== USER STATE =====================
        public static KeycloakUserInfo KeycloakUserInfo { get; set; }
        public static UserDto UserInfo { get; set; }

        // ===================== CACHE =====================
        public static ResultPage<ReportTemplate> ReportTemplates { get; set; }
        public static Dictionary<string, string> ReportCache { get; private set; }
        public static ShortcutAndFontSettings ShortcutAndFontSetting { get; set; }

        // ===================== HTTP =====================
        private static HttpClient _orthancClient;
        private static HttpClient _signatureClient;

        public static CameraSettings CameraSettingConfig { get; set; }
        // ===================== INIT =====================
        public static void Initialize()
        {
            if (IsInitialized)
                return;

            string basePath = ConfigurationManager.AppSettings["File:BasePath"];
            string configFile = ConfigurationManager.AppSettings["SystemConfigFile"] ?? "SystemConfig.xml";
            string fullPath = Path.Combine(basePath, configFile);
            SystemConfig = XmlSettingsHelper.LoadEncrypted<SystemConfig>(fullPath);
           
            InitializeServices();
            InitializeCaches();

            IsInitialized = true;
        }




        private static void InitializeServices()
        {
            //SessionService = new SessionService();
            StudyService = new StudyService();
            RisService = new RisService();
            SignatureService = new SignatureService();
            HisService = new HisService();
            ShortcutAndFontSetting = ShortcutSettingsManager.LoadOrCreateSettings();
            CameraSettingConfig = XmlSettingsHelper.Load<CameraSettings>(Path.Combine(
                ConfigurationManager.AppSettings["File:BasePath"],
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
