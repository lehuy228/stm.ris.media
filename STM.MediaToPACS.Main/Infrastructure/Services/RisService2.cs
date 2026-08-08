using MediaToPacs.Core.Interfaces;
using MediaToPacs.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MediaToPacs.Infrastructure.Services
{
    /// <summary>
    /// Module RIS V2 — tích hợp nội bộ, không xác thực (base URL: /api/risv1).
    /// Response trả trực tiếp (không bọc ApiResponse), không phân trang.
    /// Xem docs/api/ris-v1.md.
    /// </summary>
    public class RisService2 : IRisService2
    {
        private string _risV2Url;
        private string _directRisV2Url;
        private readonly HttpClient _httpClient;

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public RisService2()
        {
            _httpClient = new HttpClient();
        }

        public void Configure(string risV2Url, string accessToken = null)
        {
            _risV2Url = NormalizeRisV2BaseUrl(risV2Url);
            _httpClient.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(accessToken)
                ? null
                : new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public void ConfigureDirectFallback(string directRisV2Url)
        {
            _directRisV2Url = NormalizeRisV2BaseUrl(directRisV2Url);
        }

        public async Task<SystemUpdateConfig> GetSystemUpdateConfigAsync()
        {
            using (var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                var url = _risV2Url + ApiEndpoints.RisV2.SystemUpdateConfig;
                Trace.TraceInformation("Kiểm tra cấu hình cập nhật qua Gateway: " + url);
                var response = await _httpClient.GetAsync(url, timeout.Token);

                if (response.StatusCode == HttpStatusCode.NotFound
                    && !string.IsNullOrWhiteSpace(_directRisV2Url))
                {
                    Trace.TraceWarning("Gateway trả 404 cho " + url + "; thử RIS V2 trực tiếp");
                    response.Dispose();
                    url = _directRisV2Url + ApiEndpoints.RisV2.SystemUpdateConfig;
                    response = await _httpClient.GetAsync(url, timeout.Token);
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    Trace.TraceWarning($"API cấu hình update lỗi. URL={url}, Status={(int)response.StatusCode}, Body={errorBody}");
                    response.EnsureSuccessStatusCode();
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ResponseResult<SystemUpdateConfig>>(json, _jsonOptions);
                return result != null && result.success ? result.data : null;
            }
        }

        private static string NormalizeRisV2BaseUrl(string risV2Url)
        {
            if (string.IsNullOrWhiteSpace(risV2Url))
                return risV2Url;

            var normalized = risV2Url.Trim().TrimEnd('/');
            const string risV1Path = ApiEndpoints.RisV2.Root;

            if (normalized.EndsWith(risV1Path, StringComparison.OrdinalIgnoreCase))
                normalized = normalized.Substring(0, normalized.Length - risV1Path.Length);

            return normalized.TrimEnd('/');
        }

        public async Task<List<PractitionerListDto>> GetColleaguesAsync(string orgCode, List<string> titleCodes = null)
        {
            if (string.IsNullOrWhiteSpace(orgCode))
                throw new ArgumentException("orgCode không được để trống", nameof(orgCode));

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["orgCode"] = orgCode;

            if (titleCodes != null)
            {
                foreach (var titleCode in titleCodes)
                    query.Add("titleCodes", titleCode);
            }

            var url = $"{_risV2Url}{ApiEndpoints.RisV2.StaffColleagues}?{query}";

            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return new List<PractitionerListDto>();

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
                return new List<PractitionerListDto>();

            return JsonSerializer.Deserialize<List<PractitionerListDto>>(json, _jsonOptions)
                ?? new List<PractitionerListDto>();
        }

        public async Task<List<PractitionerViewerAccessDto>> GetViewerAccessesAsync(string staffCode)
        {
            if (string.IsNullOrWhiteSpace(staffCode))
                return new List<PractitionerViewerAccessDto>();

            var url = $"{_risV2Url}{ApiEndpoints.RisV2.StaffViewerAccesses}?staffCode={Uri.EscapeDataString(staffCode)}";
            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return new List<PractitionerViewerAccessDto>();

            await EnsureSuccessWithBodyAsync(response);

            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
                return new List<PractitionerViewerAccessDto>();

            return JsonSerializer.Deserialize<List<PractitionerViewerAccessDto>>(json, _jsonOptions)
                ?? new List<PractitionerViewerAccessDto>();
        }

        public async Task<string> GetViewerLinkByPlacerCodeAsync(string placerCode, string staffCode, string viewerName = null)
        {
            if (string.IsNullOrWhiteSpace(placerCode))
                throw new ArgumentException("placerCode không được để trống", nameof(placerCode));
            if (string.IsNullOrWhiteSpace(staffCode))
                throw new ArgumentException("staffCode không được để trống", nameof(staffCode));

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["staffCode"] = staffCode;
            if (!string.IsNullOrWhiteSpace(viewerName))
                query["viewerName"] = viewerName;

            var url = $"{_risV2Url}{ApiEndpoints.RisV2.OrderItemsByPlacerCode}/{Uri.EscapeDataString(placerCode)}{ApiEndpoints.RisV2.ViewerLink}?{query}";

            using (var content = new StringContent(string.Empty))
            {
                var response = await _httpClient.PostAsync(url, content);
                await EnsureSuccessWithBodyAsync(response);

                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(json))
                    return null;

                // Response risv1 trả trực tiếp {"launchUrl": "..."} - không bọc "data" như bản api/v1.
                var result = JsonSerializer.Deserialize<ViewerLinkResult>(json, _jsonOptions);
                return result?.launchUrl;
            }
        }

        public async Task<List<DeviceDto>> GetDevicesAsync(string modality = null)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrWhiteSpace(modality))
                query["modality"] = modality;

            var url = _risV2Url + ApiEndpoints.RisV2.Devices;
            if (query.Count > 0)
                url += $"?{query}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
                return new List<DeviceDto>();

            return JsonSerializer.Deserialize<List<DeviceDto>>(json, _jsonOptions)
                ?? new List<DeviceDto>();
        }

        public async Task<List<OrganizationDto>> GetDepartmentsAsync()
        {
            var url = _risV2Url + ApiEndpoints.RisV2.Departments;

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
                return new List<OrganizationDto>();

            return JsonSerializer.Deserialize<List<OrganizationDto>>(json, _jsonOptions)
                ?? new List<OrganizationDto>();
        }

        public async Task<List<QuickSuggestionListItemDto>> GetQuickSuggestionsAsync(
            long? serviceId = null,
            int? gender = null,
            bool? hasReportParam = null,
            string modalityCode = null,
            string search = null,
            bool? activeOnly = null,
            string serviceCode = null)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);

            if (serviceId.HasValue)
                query["serviceId"] = serviceId.Value.ToString();
            if (!string.IsNullOrWhiteSpace(serviceCode))
                query["serviceCode"] = serviceCode;
            if (gender.HasValue)
                query["gender"] = gender.Value.ToString();
            if (hasReportParam.HasValue)
                query["hasReportParam"] = hasReportParam.Value ? "true" : "false";
            if (!string.IsNullOrWhiteSpace(modalityCode))
                query["modalityCode"] = modalityCode;
            if (!string.IsNullOrWhiteSpace(search))
                query["search"] = search;
            if (activeOnly.HasValue)
                query["activeOnly"] = activeOnly.Value ? "true" : "false";

            var url = _risV2Url + ApiEndpoints.RisV2.QuickSuggestions;
            if (query.Count > 0)
                url += $"?{query}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
                return new List<QuickSuggestionListItemDto>();

            return JsonSerializer.Deserialize<List<QuickSuggestionListItemDto>>(json, _jsonOptions)
                ?? new List<QuickSuggestionListItemDto>();
        }

        public async Task<QuickSuggestionPublicDetailDto> GetQuickSuggestionByIdAsync(long id)
        {
            var url = $"{_risV2Url}{ApiEndpoints.RisV2.QuickSuggestions}/{id}";

            var response = await _httpClient.GetAsync(url);

            // 404 = suggestion không tồn tại - trả null để caller tự xử lý
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
                return null;

            return JsonSerializer.Deserialize<QuickSuggestionPublicDetailDto>(json, _jsonOptions);
        }

        public async Task<RisV1OrderItemDetailDto> GetOrderItemByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("id y lệnh không hợp lệ", nameof(id));

            var url = $"{_risV2Url}{ApiEndpoints.RisV2.OrderItems}/{id:D}";
            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
                return null;

            return JsonSerializer.Deserialize<RisV1OrderItemDetailDto>(json, _jsonOptions);
        }

        public async Task<RisV1OrderItemDetailDto> GetOrderItemByPlacerCodeAsync(string placerCode)
        {
            if (string.IsNullOrWhiteSpace(placerCode))
                throw new ArgumentException("placerCode không được để trống", nameof(placerCode));

            var url = $"{_risV2Url}{ApiEndpoints.RisV2.OrderItemsByPlacerCode}/{Uri.EscapeDataString(placerCode.Trim())}";
            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
                return null;

            return JsonSerializer.Deserialize<RisV1OrderItemDetailDto>(json, _jsonOptions);
        }

        public async Task<RisV1DiagnosticReportDetailDto> UpsertOrderItemConclusionAsync(
            Guid id,
            RisV1UpsertConclusionRequest request)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("id y lệnh không hợp lệ", nameof(id));
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (request.conclusion != null && request.conclusion.Length > 100000)
                throw new ArgumentException("Kết luận không được vượt quá 100.000 ký tự", nameof(request));

            var url = $"{_risV2Url}{ApiEndpoints.RisV2.OrderItems}/{id:D}{ApiEndpoints.RisV2.Conclusion}";
            var payload = JsonSerializer.Serialize(request, _jsonOptions);

            using (var content = new StringContent(payload, Encoding.UTF8, "application/json"))
            {
                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(json))
                    return null;

                return JsonSerializer.Deserialize<RisV1DiagnosticReportDetailDto>(json, _jsonOptions);
            }
        }

        public async Task<RisV1DiagnosticReportDetailDto> VoidSignatureByPlacerCodeAsync(
            string placerCode, string userCode = null)
        {
            if (string.IsNullOrWhiteSpace(placerCode))
                throw new ArgumentException("placerCode khong duoc de trong", nameof(placerCode));

            var url = $"{_risV2Url}{ApiEndpoints.RisV2.OrderItemsByPlacerCode}/{Uri.EscapeDataString(placerCode.Trim())}{ApiEndpoints.RisV2.VoidSignature}";
            if (!string.IsNullOrWhiteSpace(userCode))
                url += $"?userCode={Uri.EscapeDataString(userCode.Trim())}";
            using (var content = new StringContent(string.Empty, Encoding.UTF8, "application/json"))
            {
                var response = await _httpClient.PostAsync(url, content);
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(json))
                    return null;

                return JsonSerializer.Deserialize<RisV1DiagnosticReportDetailDto>(json, _jsonOptions);
            }
        }

        public async Task<RisV1DiagnosticReportDetailDto> CompleteOrderItemByPlacerCodeAsync(
            string placerCode, string userCode = null)
        {
            if (string.IsNullOrWhiteSpace(placerCode))
                throw new ArgumentException("placerCode khong duoc de trong", nameof(placerCode));

            var url = $"{_risV2Url}{ApiEndpoints.RisV2.OrderItemsByPlacerCode}/{Uri.EscapeDataString(placerCode.Trim())}{ApiEndpoints.RisV2.Complete}";
            if (!string.IsNullOrWhiteSpace(userCode))
                url += $"?userCode={Uri.EscapeDataString(userCode.Trim())}";
            using (var content = new StringContent(string.Empty, Encoding.UTF8, "application/json"))
            {
                var response = await _httpClient.PostAsync(url, content);
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(json))
                    return null;

                return JsonSerializer.Deserialize<RisV1DiagnosticReportDetailDto>(json, _jsonOptions);
            }
        }

        public async Task<OruResendResultDto> ResendOruToHisAsync(string orderItemCode)
        {
            if (string.IsNullOrWhiteSpace(orderItemCode))
                throw new ArgumentException("orderItemCode khong duoc de trong", nameof(orderItemCode));

            var request = new { orderItemCode = orderItemCode.Trim() };
            var payload = JsonSerializer.Serialize(request, _jsonOptions);

            using (var content = new StringContent(payload, Encoding.UTF8, "application/json"))
            {
                var url = $"{_risV2Url}{ApiEndpoints.RisV2.Hl7OruResend}";
                var response = await _httpClient.PostAsync(url, content);
                var body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    // 404/409 đi qua ApiEnvelopeExceptionHandler chung: { success, error: { code, message } }
                    // - khác format với body 200 (OruResendResult).
                    Trace.TraceWarning($"API resend ORU loi. URL={url}, Status={(int)response.StatusCode}, Body={body}");
                    return ParseOruResendErrorEnvelope(body, response);
                }

                Trace.TraceInformation($"API resend ORU response. URL={url}, Body={body}");
                return ParseOruResendResult(body);
            }
        }

        /// <summary>
        /// Đọc body 200 (OruResendResult: orderCode/isSuccess/errorCode/errorMessage). Chấp nhận cả tên
        /// "success" phòng khi API đổi tên; không có cờ nào thì suy từ errorCode ("0"/rỗng = thành công).
        /// </summary>
        private static OruResendResultDto ParseOruResendResult(string json)
        {
            var result = new OruResendResultDto();

            if (string.IsNullOrWhiteSpace(json))
            {
                result.success = true;
                return result;
            }

            try
            {
                using (var doc = JsonDocument.Parse(json))
                {
                    var root = doc.RootElement;
                    if (root.ValueKind != JsonValueKind.Object)
                    {
                        result.success = root.ValueKind == JsonValueKind.True;
                        return result;
                    }

                    bool? success = ReadBool(root, "isSuccess") ?? ReadBool(root, "success");
                    result.orderCode = ReadString(root, "orderCode");
                    result.errorCode = ReadString(root, "errorCode");
                    result.errorMessage = ReadString(root, "errorMessage") ?? ReadString(root, "message");
                    result.ackBase64 = ReadString(root, "ackBase64");

                    result.success = success
                        ?? (string.IsNullOrWhiteSpace(result.errorCode) || result.errorCode.Trim() == "0");
                    return result;
                }
            }
            catch (JsonException ex)
            {
                Trace.TraceWarning($"Khong doc duoc response resend ORU: {ex.Message}. Body={json}");
                result.success = true;
                return result;
            }
        }

        /// <summary>
        /// Đọc body lỗi phía RIS (404 NOT_FOUND / 409 REPORT_NOT_FINAL) dạng envelope
        /// { success: false, error: { code, message } }; fallback về HTTP status nếu không parse được.
        /// </summary>
        private static OruResendResultDto ParseOruResendErrorEnvelope(string json, HttpResponseMessage response)
        {
            var result = new OruResendResultDto
            {
                success = false,
                errorMessage = $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}"
            };

            if (string.IsNullOrWhiteSpace(json))
                return result;

            try
            {
                using (var doc = JsonDocument.Parse(json))
                {
                    var root = doc.RootElement;
                    if (root.ValueKind != JsonValueKind.Object)
                        return result;

                    if (root.TryGetProperty("error", out var error) && error.ValueKind == JsonValueKind.Object)
                    {
                        result.errorCode = ReadString(error, "code");
                        result.errorMessage = ReadString(error, "message") ?? result.errorMessage;
                        return result;
                    }

                    result.errorCode = ReadString(root, "errorCode");
                    result.errorMessage = ReadString(root, "errorMessage")
                        ?? ReadString(root, "message")
                        ?? result.errorMessage;
                    return result;
                }
            }
            catch (JsonException ex)
            {
                Trace.TraceWarning($"Khong doc duoc body loi resend ORU: {ex.Message}. Body={json}");
                return result;
            }
        }

        private static bool? ReadBool(JsonElement root, string name)
        {
            if (!root.TryGetProperty(name, out var value))
                return null;

            if (value.ValueKind == JsonValueKind.True) return true;
            if (value.ValueKind == JsonValueKind.False) return false;
            return null;
        }

        private static string ReadString(JsonElement root, string name)
        {
            if (!root.TryGetProperty(name, out var value))
                return null;

            switch (value.ValueKind)
            {
                case JsonValueKind.String:
                    return value.GetString();
                case JsonValueKind.Number:
                    return value.ToString();
                default:
                    return null;
            }
        }

        public async Task VoidDiagnosticReportAsync(Guid reportId)
        {
            if (reportId == Guid.Empty)
                throw new ArgumentException("reportId khong hop le", nameof(reportId));

            var url = $"{_risV2Url}{ApiEndpoints.RisV2.DiagnosticReports}/{reportId:D}{ApiEndpoints.RisV2.Void}";
            using (var content = new StringContent(string.Empty, Encoding.UTF8, "application/json"))
            {
                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task<List<AuditLogListItemDto>> GetAuditLogsByOrderCodeAsync(string orderCode, int limit = 100)
        {
            if (string.IsNullOrWhiteSpace(orderCode))
                throw new ArgumentException("orderCode không được để trống", nameof(orderCode));

            // limit bị API clamp 1-100; clamp sẵn phía client cho khớp tài liệu.
            if (limit < 1) limit = 1;
            if (limit > 100) limit = 100;

            var url = $"{_risV2Url}{ApiEndpoints.RisV2.AuditLogs}" +
                      $"?orderCode={Uri.EscapeDataString(orderCode.Trim())}&limit={limit}";
            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return new List<AuditLogListItemDto>();

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
                return new List<AuditLogListItemDto>();

            var envelope = JsonSerializer.Deserialize<MediaToPacs.Core.Models.Conclusion.ApiResponse<AuditLogPageDto>>(
                json, _jsonOptions);
            var items = envelope?.Data?.items;
            if (items == null)
                return new List<AuditLogListItemDto>();

            // API không cam kết thứ tự - tự sắp mới nhất lên trước cho đúng nghĩa "theo thời gian".
            items.Sort((a, b) => b.timestampUtc.CompareTo(a.timestampUtc));
            return items;
        }

        public async Task<RisV1PatientOrderHistoryDto> GetPatientHistoryByOrderCodeAsync(string placerCode)
        {
            if (string.IsNullOrWhiteSpace(placerCode))
                throw new ArgumentException("placerCode không được để trống", nameof(placerCode));

            var url = $"{_risV2Url}{ApiEndpoints.RisV2.PatientHistoryByOrderCode}/{Uri.EscapeDataString(placerCode.Trim())}";
            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
                return null;

            return JsonSerializer.Deserialize<RisV1PatientOrderHistoryDto>(json, _jsonOptions);
        }

        public async Task<List<DiagnosticReportAttachmentDto>> GetDiagnosticReportAttachmentsAsync(Guid orderItemId)
        {
            EnsureOrderItemId(orderItemId);

            var url = GetDiagnosticReportAttachmentsUrl(orderItemId);
            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return new List<DiagnosticReportAttachmentDto>();

            await EnsureSuccessWithBodyAsync(response);

            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
                return new List<DiagnosticReportAttachmentDto>();

            return DeserializeAttachmentList(json);
        }

        public async Task<List<DiagnosticReportAttachmentDto>> UploadDiagnosticReportAttachmentsAsync(
            Guid orderItemId,
            IEnumerable<string> filePaths)
        {
            EnsureOrderItemId(orderItemId);
            if (filePaths == null)
                throw new ArgumentNullException(nameof(filePaths));

            var url = $"{GetDiagnosticReportAttachmentsUrl(orderItemId)}/batch";

            using (var form = new MultipartFormDataContent())
            {
                var streams = new List<FileStream>();
                try
                {
                    foreach (var filePath in filePaths)
                    {
                        var validatedPath = ValidateAttachmentFilePath(filePath);
                        var stream = File.OpenRead(validatedPath);
                        streams.Add(stream);

                        var content = new StreamContent(stream);
                        content.Headers.ContentType = new MediaTypeHeaderValue(GetContentType(validatedPath));
                        form.Add(content, "files", Path.GetFileName(validatedPath));
                    }

                    if (streams.Count == 0)
                        return new List<DiagnosticReportAttachmentDto>();

                    var response = await _httpClient.PostAsync(url, form);
                    await EnsureSuccessWithBodyAsync(response);

                    var json = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrWhiteSpace(json))
                        return new List<DiagnosticReportAttachmentDto>();

                    return DeserializeUploadAttachmentResult(json);
                }
                finally
                {
                    foreach (var stream in streams)
                        stream.Dispose();
                }
            }
        }

        private static List<DiagnosticReportAttachmentDto> DeserializeUploadAttachmentResult(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<DiagnosticReportAttachmentDto>();

            var trimmed = json.TrimStart();
            if (trimmed.StartsWith("["))
            {
                return JsonSerializer.Deserialize<List<DiagnosticReportAttachmentDto>>(json, _jsonOptions)
                    ?? new List<DiagnosticReportAttachmentDto>();
            }

            using (var doc = JsonDocument.Parse(json))
            {
                var root = doc.RootElement;

                if (TryDeserializeAttachmentArrayProperty(root, "uploaded", out var uploaded))
                    return uploaded;

                if (TryDeserializeAttachmentArrayProperty(root, "items", out var items))
                    return items;

                if (root.TryGetProperty("data", out var data))
                {
                    if (data.ValueKind == JsonValueKind.Array)
                    {
                        return JsonSerializer.Deserialize<List<DiagnosticReportAttachmentDto>>(
                                   data.GetRawText(),
                                   _jsonOptions)
                               ?? new List<DiagnosticReportAttachmentDto>();
                    }

                    if (data.ValueKind == JsonValueKind.Object)
                    {
                        if (TryDeserializeAttachmentArrayProperty(data, "uploaded", out var dataUploaded))
                            return dataUploaded;

                        if (TryDeserializeAttachmentArrayProperty(data, "items", out var dataItems))
                            return dataItems;
                    }
                }
            }

            return new List<DiagnosticReportAttachmentDto>();
        }

        private static bool TryDeserializeAttachmentArrayProperty(
            JsonElement element,
            string propertyName,
            out List<DiagnosticReportAttachmentDto> attachments)
        {
            attachments = null;

            if (!element.TryGetProperty(propertyName, out var property) ||
                property.ValueKind != JsonValueKind.Array)
                return false;

            attachments = JsonSerializer.Deserialize<List<DiagnosticReportAttachmentDto>>(
                              property.GetRawText(),
                              _jsonOptions)
                          ?? new List<DiagnosticReportAttachmentDto>();
            return true;
        }

        public async Task<List<DiagnosticReportAttachmentDto>> UpdateDocumentAttachmentSelectionAsync(
            Guid orderItemId,
            List<DocumentAttachmentSelectionItem> selections)
        {
            EnsureOrderItemId(orderItemId);

            var request = new DocumentAttachmentSelectionRequest
            {
                selections = selections ?? new List<DocumentAttachmentSelectionItem>()
            };

            var url = $"{GetDiagnosticReportAttachmentsUrl(orderItemId)}/document-selection";
            var payload = JsonSerializer.Serialize(request, _jsonOptions);

            using (var content = new StringContent(payload, Encoding.UTF8, "application/json"))
            {
                var response = await _httpClient.PutAsync(url, content);
                await EnsureSuccessWithBodyAsync(response);

                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(json))
                    return new List<DiagnosticReportAttachmentDto>();

                return DeserializeAttachmentList(json);
            }
        }

        public async Task UpdatePacsAttachmentSelectionAsync(
            Guid orderItemId,
            List<Guid> attachmentIds)
        {
            EnsureOrderItemId(orderItemId);

            var request = new PacsAttachmentSelectionRequest
            {
                attachmentIds = attachmentIds ?? new List<Guid>()
            };

            var url = $"{GetDiagnosticReportAttachmentsUrl(orderItemId)}/pacs-selection";
            var payload = JsonSerializer.Serialize(request, _jsonOptions);

            using (var content = new StringContent(payload, Encoding.UTF8, "application/json"))
            {
                var response = await _httpClient.PutAsync(url, content);
                await EnsureSuccessWithBodyAsync(response);
            }
        }

        public async Task<PacsPushResult> PushDiagnosticReportAttachmentsToPacsAsync(
            Guid orderItemId,
            string targetServer = "MainStorage")
        {
            EnsureOrderItemId(orderItemId);

            var server = string.IsNullOrWhiteSpace(targetServer)
                ? "MainStorage"
                : targetServer.Trim();

            var url = $"{GetDiagnosticReportAttachmentsUrl(orderItemId)}/pacs-push?targetServer={Uri.EscapeDataString(server)}";
            var response = await _httpClient.PostAsync(url, new StringContent(string.Empty));
            await EnsureSuccessWithBodyAsync(response);

            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
                return null;

            var wrapped = JsonSerializer.Deserialize<PacsPushResponse>(json, _jsonOptions);
            if (wrapped != null && wrapped.data != null)
                return wrapped.data;

            return JsonSerializer.Deserialize<PacsPushResult>(json, _jsonOptions);
        }

        public async Task<Stream> StreamDiagnosticReportAttachmentAsync(
            Guid orderItemId,
            Guid attachmentId)
        {
            EnsureOrderItemId(orderItemId);
            if (attachmentId == Guid.Empty)
                throw new ArgumentException("attachmentId không hợp lệ", nameof(attachmentId));

            var url = $"{GetDiagnosticReportAttachmentsUrl(orderItemId)}/{attachmentId:D}/stream";
            var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            await EnsureSuccessWithBodyAsync(response);

            return await response.Content.ReadAsStreamAsync();
        }

        public async Task DeleteDiagnosticReportAttachmentAsync(
            Guid orderItemId,
            Guid attachmentId)
        {
            EnsureOrderItemId(orderItemId);
            if (attachmentId == Guid.Empty)
                throw new ArgumentException("attachmentId khong hop le", nameof(attachmentId));

            var url = $"{GetDiagnosticReportAttachmentsUrl(orderItemId)}/{attachmentId:D}";
            var response = await _httpClient.DeleteAsync(url);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return;

            await EnsureSuccessWithBodyAsync(response);
        }

        private static void EnsureOrderItemId(Guid orderItemId)
        {
            if (orderItemId == Guid.Empty)
                throw new ArgumentException("orderItemId không hợp lệ", nameof(orderItemId));
        }

        private static string ValidateAttachmentFilePath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Đường dẫn file ảnh không được để trống", nameof(filePath));

            var fullPath = Path.GetFullPath(filePath);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException("File ảnh không tồn tại", fullPath);

            var info = new FileInfo(fullPath);
            if (info.Length <= 0)
                throw new InvalidOperationException("File ảnh rỗng: " + fullPath);

            GetContentType(fullPath);
            return fullPath;
        }

        private static string GetContentType(string filePath)
        {
            var ext = Path.GetExtension(filePath);
            if (string.IsNullOrWhiteSpace(ext))
                throw new InvalidOperationException("File không có phần mở rộng: " + filePath);

            switch (ext.ToLowerInvariant())
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".bmp":
                    return "image/bmp";
                case ".webp":
                    return "image/webp";
                case ".mp4":
                    return "video/mp4";
                case ".mpeg":
                case ".mpg":
                    return "video/mpeg";
                default:
                    throw new InvalidOperationException("Định dạng file không hợp lệ cho attachment: " + ext);
            }
        }

        private string GetDiagnosticReportAttachmentsUrl(Guid orderItemId)
        {
            return $"{_risV2Url}{ApiEndpoints.RisV2.DiagnosticReportOrderItems}/{orderItemId:D}/attachments";
        }

        private static List<DiagnosticReportAttachmentDto> DeserializeAttachmentList(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<DiagnosticReportAttachmentDto>();

            var trimmed = json.TrimStart();
            if (trimmed.StartsWith("["))
            {
                return JsonSerializer.Deserialize<List<DiagnosticReportAttachmentDto>>(json, _jsonOptions)
                    ?? new List<DiagnosticReportAttachmentDto>();
            }

            using (var doc = JsonDocument.Parse(json))
            {
                var root = doc.RootElement;

                if (TryDeserializeAttachmentArrayProperty(root, "items", out var items))
                    return items;

                // API bọc kết quả dạng {"success":true,"data":[...]} - "data" có thể là mảng
                // thẳng hoặc object lồng {"items"/"uploaded":[...]}.
                if (root.TryGetProperty("data", out var data))
                {
                    if (data.ValueKind == JsonValueKind.Array)
                    {
                        return JsonSerializer.Deserialize<List<DiagnosticReportAttachmentDto>>(
                                   data.GetRawText(),
                                   _jsonOptions)
                               ?? new List<DiagnosticReportAttachmentDto>();
                    }

                    if (data.ValueKind == JsonValueKind.Object)
                    {
                        if (TryDeserializeAttachmentArrayProperty(data, "items", out var dataItems))
                            return dataItems;

                        if (TryDeserializeAttachmentArrayProperty(data, "uploaded", out var dataUploaded))
                            return dataUploaded;
                    }
                }
            }

            return new List<DiagnosticReportAttachmentDto>();
        }

        private static async Task EnsureSuccessWithBodyAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            var body = response.Content == null
                ? null
                : await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(body))
                response.EnsureSuccessStatusCode();

            throw new HttpRequestException(
                $"API RIS lỗi. Status={(int)response.StatusCode} {response.ReasonPhrase}. Body={body}");
        }
    }
}
