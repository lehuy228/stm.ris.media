using MediaToPacs.Core.Interfaces;
using MediaToPacs.Core.Models;
using MediaToPacs.Core.Models.Ketluan;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MediaToPacs.Infrastructure.Services
{
    public class SignatureService : ISignatureService
    {
        private HttpClient _httpClient;

        public SignatureService()
        {
            _httpClient = new HttpClient();
        }

        public void LoadClient(HttpClient client)
        {
            _httpClient = client;
        }

        #region HisUserKySo CRUD

        public async Task<List<HisUserKySoResponse>> GetAllHisUserKySoAsync()
        {
            var response = await _httpClient.GetAsync(ApiEndpoints.Signature.HisUsers);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<HisUserKySoResponse>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<HisUserKySoResponse> GetByIdHisUserKySoAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{ApiEndpoints.Signature.HisUsers}/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<HisUserKySoResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<HisUserKySoResponse> GetByHisUserKySoIdAsync(string hisUserId)
        {
            var response = await _httpClient.GetAsync($"{ApiEndpoints.Signature.HisUsersByHisId}/{hisUserId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<HisUserKySoResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<HisUserKySoResponse> CreateHisUserKySoAsync(HisUserKySoRequest input)
        {
            var json = JsonSerializer.Serialize(input, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(ApiEndpoints.Signature.HisUsers, content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<HisUserKySoResponse>(result,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<HisUserKySoResponse> UpdateHisUserKySoAsync(Guid id, HisUserKySoRequest input)
        {
            var json = JsonSerializer.Serialize(input, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{ApiEndpoints.Signature.HisUsers}/{id}", content);
            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<HisUserKySoResponse>(result,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> DeleteHisUserKySoAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiEndpoints.Signature.HisUsers}/{id}");
            return response.IsSuccessStatusCode;
        }

        #endregion

        public async Task<Dictionary<string, CertBO>> GetCertificateInfosAsync(string userId)
        {
            var requestObj = new { UserId = userId };
            var content = new StringContent(JsonSerializer.Serialize(requestObj), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(ApiEndpoints.Signature.CertificateInfos, content);
            response.EnsureSuccessStatusCode();

            var responseStr = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<Dictionary<string, CertBO>>(responseStr, options);
        }

        public async Task<LoginResponseBO> GetToken(string userId)
        {
            var request = new { UserId = userId };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(ApiEndpoints.Signature.Login, content);
            response.EnsureSuccessStatusCode();

            var responseStr = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LoginResponseBO>(responseStr,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<UserDto> GetUserCert(string cccd)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{ApiEndpoints.Signature.UsersByUserId}/{cccd}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<UserDto>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw new Exception($"Lỗi khi gọi API: {response.StatusCode}");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi gọi API GetUserCert", ex);
            }
        }


        public async Task<string> SignHashPdf(SignhashRequest request)
        {
            var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(ApiEndpoints.Signature.SignHashPdf, content);
            response.EnsureSuccessStatusCode();

            var responseStr = await response.Content.ReadAsStringAsync();
            var pdfResponse = JsonSerializer.Deserialize<string>(responseStr, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return pdfResponse;
        }

        public async Task<string> SignHashPdfV2(SignhashRequestV2 request)
        {
            var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(ApiEndpoints.Signature.SignHashPdfV2, content);
            response.EnsureSuccessStatusCode();

            var responseStr = await response.Content.ReadAsStringAsync();
            var pdfResponse = JsonSerializer.Deserialize<string>(responseStr, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return pdfResponse;
        }

        public async Task<UserDto> UploadCertToUser(CreateUserRequest input)
        {
            var json = JsonSerializer.Serialize(input, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(ApiEndpoints.Signature.Users, content);
            response.EnsureSuccessStatusCode();
            var responseStr = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<UserDto>(responseStr, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result;
        }
    }
}
