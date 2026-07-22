namespace MediaToPacs.Infrastructure.Services
{
    /// <summary>
    /// Danh mục route API dùng bởi tầng Infrastructure.
    /// Base URL vẫn được lấy từ cấu hình tại từng service.
    /// </summary>
    public static class ApiEndpoints
    {
        public static class Gateway
        {
            public const string Ris = "/ris";
            public const string RisV2 = "/ris-v2";
            public const string Signature = "/signature";
            public const string Pacs = "/pacs";
        }

        internal static class Ris
        {
            public const string Schedules = "/lich-chup";
            public const string DiagnosticResults = "/ket-qua-chan-doan";
            public const string SendResultToHis = "/send-to-his";
            public const string DownloadResultBuffer = "/download-buffer";
            public const string UploadSignedFile = "/upload-signed-file";
            public const string CancelSignedFile = "/cancel-signed-file";
            public const string ReportTemplates = "/report-template";
            public const string OrderServices = "/chi-dinh-dich-vu";
            public const string ConclusionSuggestions = "/goi-y-ketluan";
            public const string ServiceCatalog = "/danhmucdichvu";
            public const string Devices = "/thiet-bi";
            public const string HisUserMapper = "/his-user-mapper";
            public const string Patients = "/benh-nhan";
        }

        internal static class RisV2
        {
            public const string Root = "/api/risv1";
            public const string StaffColleagues = Root + "/staff/colleagues";
            public const string Devices = Root + "/devices";
            public const string Departments = Root + "/organizations/departments";
            public const string QuickSuggestions = Root + "/quick-suggestions";
            public const string OrderItems = Root + "/order-items";
            public const string OrderItemsByPlacerCode = OrderItems + "/by-placer-code";
            public const string Conclusion = "/conclusion";
            public const string SystemUpdateConfig = "/api/v1/system-configs/system-update";
        }

        internal static class Signature
        {
            public const string HisUsers = "api/hisuserkyso";
            public const string HisUsersByHisId = HisUsers + "/byhisid";
            public const string CertificateInfos = "api/signatures/certificate-infos";
            public const string Login = "api/signatures/login";
            public const string Users = "api/users";
            public const string UsersByUserId = Users + "/by-userid";
            public const string SignHashPdf = "api/signatures/signHash-pdf";
            public const string SignHashPdfV2 = "api/signatures/signHash-pdf-v2";
        }

        internal static class Orthanc
        {
            // Không bắt đầu bằng '/' để HttpClient giữ lại prefix /pacs/ trong BaseAddress.
            public const string Find = "tools/find";
            public const string Studies = "studies";
            public const string Patients = "patients";
            public const string Series = "series";
            public const string Instances = "instances";
            public const string Modify = "/modify";
            public const string StudySeries = "/series";
            public const string SimplifiedTags = "/tags?simplify";
            public const string InstanceFile = "/file";
        }

        internal static class Keycloak
        {
            public const string MasterToken = "/realms/master/protocol/openid-connect/token";
            public const string AdminRealms = "/admin/realms";
            public const string Users = "/users";
        }
    }
}
