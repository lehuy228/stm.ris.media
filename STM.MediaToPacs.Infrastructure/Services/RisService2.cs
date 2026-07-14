using MediaToPacs.Core.Interfaces;
using MediaToPacs.Core.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
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

        public async Task<SystemUpdateConfig> GetSystemUpdateConfigAsync()
        {
            var url = _risV2Url + ApiEndpoints.RisV2.SystemUpdateConfig;
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResponseResult<SystemUpdateConfig>>(json, _jsonOptions);
            return result != null && result.success ? result.data : null;
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
    }
}
