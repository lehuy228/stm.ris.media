using MediaToPacs.Core.Interfaces;
using MediaToPacs.Core.Models.His;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MediaToPacs.Infrastructure.Services
{
    public class HisService : IHisService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public async Task<bool> KiemTraDuTienAsync(string url, string maThanhToanChiTiet)
        {
            var requestObj = new KiemTraTienRequest
            {
                Id = maThanhToanChiTiet
            };

            var json = JsonSerializer.Serialize(requestObj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<KiemTraTienResponse>(responseJson);

            // code = 0 → đủ tiền
            return result?.code == "0";
        }
    }
}
