using MediaToPacs.Core.Interfaces;
using MediaToPacs.Core.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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

        public void Configure(string risV2Url)
        {
            _risV2Url = risV2Url;
        }

        public async Task<List<PractitionerListDto>> GetColleaguesAsync(string staffCode, List<string> titleCodes = null)
        {
            if (string.IsNullOrWhiteSpace(staffCode))
                throw new ArgumentException("staffCode không được để trống", nameof(staffCode));

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["staffCode"] = staffCode;

            if (titleCodes != null)
            {
                foreach (var titleCode in titleCodes)
                    query.Add("titleCodes", titleCode);
            }

            var url = $"{_risV2Url}/api/risv1/staff/colleagues?{query}";

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

            var url = $"{_risV2Url}/api/risv1/devices";
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
    }
}
