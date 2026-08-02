using MediaToPacs.Core.Interfaces;
using MediaToPacs.Core.Models;
using MediaToPacs.Core.Models.Ketluan;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace MediaToPacs.Infrastructure.Services
{
    public class RisService : IRisService
    {
        private string _risUrl;
        private readonly HttpClient _httpClient;

        public RisService()
        {
            _httpClient = new HttpClient();
        }

        public void Configure(string risUrl, string accessToken = null)
        {
            _risUrl = risUrl;
            _httpClient.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(accessToken)
                ? null
                : new AuthenticationHeaderValue("Bearer", accessToken);
        }
        public async Task<List<ScheduleStepRIS>> GetDanhSachLichChupAsync(
         int page = 1, int pageSize = 10,
         string tenBenhNhan = null,
         string maChiDinh = null,
         string modality = null,
         string studyInstanceUID = null,
         string tenBacSiChiDinh = null,
         DateTime? dateTimeFrom = null,
         DateTime? dateTimeTo = null)
        {
            var filters = new List<FilterItem>();

            if (!string.IsNullOrWhiteSpace(tenBenhNhan))
                filters.Add(new FilterItem { Field = "tenbenhnhan", Operator = "contains", Value = tenBenhNhan });

            if (!string.IsNullOrWhiteSpace(maChiDinh))
                filters.Add(new FilterItem { Field = "machidinh", Operator = "contains", Value = maChiDinh });

            if (!string.IsNullOrWhiteSpace(studyInstanceUID))
                filters.Add(new FilterItem { Field = "studyInstanceUID", Operator = "eq", Value = studyInstanceUID });


            if (dateTimeFrom.HasValue)
            {
                filters.Add(new FilterItem
                {
                    Field = "thoigianthuchien",
                    Operator = "gte",
                    Value = dateTimeFrom.Value.ToString("yyyy-MM-dd 00:00:00")
                });
            }

            if (dateTimeTo.HasValue)
            {
                filters.Add(new FilterItem
                {
                    Field = "thoigianthuchien",
                    Operator = "lte",
                    Value = dateTimeTo.Value.ToString("yyyy-MM-dd 23:59:59")
                });
            }

            if (!string.IsNullOrWhiteSpace(modality))
                filters.Add(new FilterItem { Field = "modality", Operator = "eq", Value = modality });

            if (!string.IsNullOrWhiteSpace(tenBacSiChiDinh))
                filters.Add(new FilterItem { Field = "tenbacsichidinh", Operator = "contains", Value = tenBacSiChiDinh });


            string filtersJson = JsonConvert.SerializeObject(filters);
            string encodedFilters = HttpUtility.UrlEncode(filtersJson);

            // Build URL
            var builder = new UriBuilder(_risUrl + ApiEndpoints.Ris.Schedules);
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["page"] = page.ToString();
            query["pageSize"] = pageSize.ToString();

            if (filters.Count > 0)
                query["filters"] = encodedFilters;

            builder.Query = query.ToString();
            string url = builder.ToString();

            // Gửi request
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<ScheduleStepRISResponse>(content);

            return responseObj?.Data ?? new List<ScheduleStepRIS>();
        }


        public async Task<bool> SendKetQuaChanDoanToHisAsync(string maChiDinh)
        {
            var response = await _httpClient.PostAsync(
                $"{_risUrl}{ApiEndpoints.Ris.DiagnosticResults}/{maChiDinh}{ApiEndpoints.Ris.SendResultToHis}",
                null
            );

            return response.IsSuccessStatusCode;
        }

        public async Task<ScheduleStepRIS> GetLichChupAsync(string machidinh)
        {
            if (string.IsNullOrEmpty(machidinh))
                throw new ArgumentException("Mã chỉ định không được để trống", nameof(machidinh));

            var url = $"{_risUrl}{ApiEndpoints.Ris.Schedules}/{machidinh}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = System.Text.Json.JsonSerializer.Deserialize<ScheduleStepRIS>(json, options);
            return result;
        }

        public async Task<ResultPage<ReportTemplate>> GetDanhSachMauKLAsync(int page = 1, int pageSize = 50, string modality = null)
        {
            var filters = new List<FilterItem>();

            if (!string.IsNullOrWhiteSpace(modality))
                filters.Add(new FilterItem { Field = "modality", Operator = "contains", Value = modality });



            string filtersJson = JsonConvert.SerializeObject(filters);
            string encodedFilters = HttpUtility.UrlEncode(filtersJson);

            // Build URL
            var builder = new UriBuilder(_risUrl + ApiEndpoints.Ris.ReportTemplates);
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["page"] = page.ToString();
            query["pageSize"] = pageSize.ToString();

            if (filters.Count > 0)
                query["filters"] = encodedFilters;

            builder.Query = query.ToString();
            string url = builder.ToString();

            // Gửi request
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<ResultPage<ReportTemplate>>(content);

            return responseObj ?? new ResultPage<ReportTemplate>();
        }

        public async Task<KetQuaChanDoanResponse> TaoKetQuaChanDoanAsync(KetQuaChanDoanRequest request)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_risUrl + ApiEndpoints.Ris.DiagnosticResults, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var apiResult = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<KetQuaChanDoanResponse>>(
                        responseContent,
                        new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    return apiResult?.Data;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Deserialize error: {ex.Message}");
                    throw;
                }
            }
            else
            {
                Console.WriteLine($"API error ({response.StatusCode}): {responseContent}");
                return null; // hoặc throw exception tuỳ bạn muốn xử lý thế nào
            }
        }


        public async Task<byte[]> TaiFileKetQuaChanDoanAsync(string machidinh)
        {
            var payload = new { machidinh };

            var json = System.Text.Json.JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(
                _risUrl + ApiEndpoints.Ris.DiagnosticResults + ApiEndpoints.Ris.DownloadResultBuffer, content);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsByteArrayAsync();
            return data;
        }

        public async Task<KetQuaChanDoanResponse> GetKetQuaChanDoanAsync(string maChiDinh)
        {
            var response = await _httpClient.GetAsync($"{_risUrl}{ApiEndpoints.Ris.DiagnosticResults}/{maChiDinh}");

            if (!response.IsSuccessStatusCode)
            {
                // Nếu server trả 404 hoặc 204 thì return null thay vì exception
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound ||
                    response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return null;
                }

                // Các lỗi khác thì throw như cũ
                response.EnsureSuccessStatusCode();
            }

            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
                return null;

            // Endpoint trả về { data: {...} } - 1 object, không phải mảng
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<KetQuaChanDoanResponse>>(json);

            return apiResponse?.Data;
        }

        public async Task<bool> UploadSignedFileAsync(string machidinh, string fileType, string description, byte[] fileBuffer)
        {
            var content = new MultipartFormDataContent();

            content.Add(new StringContent(machidinh), "machidinh");
            content.Add(new StringContent(fileType), "fileType");
            content.Add(new StringContent(description), "description");
            var fileContent = new ByteArrayContent(fileBuffer);

            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
            content.Add(fileContent, "file", "signed.pdf");

            var response = await _httpClient.PostAsync(
                _risUrl + ApiEndpoints.Ris.DiagnosticResults + ApiEndpoints.Ris.UploadSignedFile, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var err = await response.Content.ReadAsStringAsync();
                return false;
            }
        }

        #region ReportTemplate
        public async Task<bool> CreateReportTemplateAsync(ReportTemplateRequest request)
        {
            var formData = new MultipartFormDataContent();

            // Các field dạng text
            formData.Add(new StringContent(request.name ?? string.Empty), "name");
            formData.Add(new StringContent(request.modality ?? string.Empty), "modality");
            formData.Add(new StringContent(request.procedureCode ?? string.Empty), "procedureCode");
            formData.Add(new StringContent(request.content ?? string.Empty), "content");
            formData.Add(new StringContent(request.placeholders ?? string.Empty), "placeholders");
            formData.Add(new StringContent(request.xmlTemplate ?? string.Empty), "xmlTemplate");


            var response = await _httpClient.PostAsync(_risUrl + ApiEndpoints.Ris.ReportTemplates, formData);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API error: {error}");
            return false;
        }

        public async Task<bool> UpdateReportTemplateAsync(ReportTemplateRequest request, string id)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var requestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), $"{_risUrl}{ApiEndpoints.Ris.ReportTemplates}/{id}")
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Update thất bại: {response.StatusCode} - {error}");
        }

        public async Task<ResultPage<ReportTemplateGridViewModel>> GetReportTemplateAsync(int page = 1, int pageSize = 50, string modality = null)
        {
            var filters = new List<FilterItem>();

            if (!string.IsNullOrWhiteSpace(modality))
                filters.Add(new FilterItem { Field = "modality", Operator = "contains", Value = modality });



            string filtersJson = JsonConvert.SerializeObject(filters);
            string encodedFilters = HttpUtility.UrlEncode(filtersJson);

            // Build URL
            var builder = new UriBuilder(_risUrl + ApiEndpoints.Ris.ReportTemplates);
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["page"] = page.ToString();
            query["pageSize"] = pageSize.ToString();

            if (filters.Count > 0)
                query["filters"] = encodedFilters;

            builder.Query = query.ToString();
            string url = builder.ToString();

            // Gửi request
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                // Nếu server trả 404 hoặc 204 thì return null thay vì exception
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound ||
                    response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return null;
                }

                // Các lỗi khác thì throw như cũ
                response.EnsureSuccessStatusCode();
            }

            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
                return null;

            return System.Text.Json.JsonSerializer.Deserialize<ResultPage<ReportTemplateGridViewModel>>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        public async Task<ReportTemplateResponse> GetReportTemplateByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"{_risUrl}{ApiEndpoints.Ris.ReportTemplates}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                // Nếu server trả 404 hoặc 204 thì return null thay vì exception
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound ||
                    response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return null;
                }

                // Các lỗi khác thì throw như cũ
                response.EnsureSuccessStatusCode();
            }

            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
                return null;

            return System.Text.Json.JsonSerializer.Deserialize<ReportTemplateResponse>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        public async Task<bool> XoaReportTemplateAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"{_risUrl}{ApiEndpoints.Ris.ReportTemplates}/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            // Nếu cần log chi tiết lỗi
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Xoá gợi ý kết luận thất bại. Status: {response.StatusCode}, Message: {error}");
        }

        #endregion

        #region Danh sách chỉ định dịch vụ

        public async Task<ResultPage<ChiDinhDichVuResponse>> GetDSChiDinhDichVuAsync(int page = 1, int pageSize = 50,
           string mabenhnhan = null,
           string tenBenhNhan = null,
           string maChiDinh = null,
           string modality = null,
           string trangThai = null,
           string tenBacSiChiDinh = null,
           DateTime? dateTimeFrom = null,
           DateTime? dateTimeTo = null)
        {
            var filters = new List<FilterItem>();

            if (!string.IsNullOrWhiteSpace(mabenhnhan))
                filters.Add(new FilterItem { Field = "benhnhan.mabenhnhan", Operator = "contains", Value = mabenhnhan });

            if (!string.IsNullOrWhiteSpace(tenBenhNhan))
                filters.Add(new FilterItem { Field = "benhnhan.hoten", Operator = "contains", Value = tenBenhNhan });

            if (!string.IsNullOrWhiteSpace(maChiDinh))
                filters.Add(new FilterItem { Field = "chidinh.sophieu", Operator = "contains", Value = maChiDinh });


            if (dateTimeFrom.HasValue)
            {
                filters.Add(new FilterItem
                {
                    Field = "thoigianthuchien",
                    Operator = "gte",
                    Value = dateTimeFrom.Value.ToString("yyyy-MM-dd 00:00:00")
                });
            }

            if (dateTimeTo.HasValue)
            {
                filters.Add(new FilterItem
                {
                    Field = "thoigianthuchien",
                    Operator = "lte",
                    Value = dateTimeTo.Value.ToString("yyyy-MM-dd 23:59:59")
                });
            }


            if (!string.IsNullOrWhiteSpace(trangThai))
                filters.Add(new FilterItem { Field = "trangThai", Operator = "eq", Value = trangThai });

            if (!string.IsNullOrWhiteSpace(modality))
                filters.Add(new FilterItem { Field = "modality", Operator = "eq", Value = modality });

            if (!string.IsNullOrWhiteSpace(tenBacSiChiDinh))
                filters.Add(new FilterItem { Field = "tenBacSiChiDinh", Operator = "contains", Value = tenBacSiChiDinh });

            string filtersJson = JsonConvert.SerializeObject(filters);

            // Build query string thủ công (chuẩn UTF-8)
            var queryParams = new List<string>
            {
                $"page={page}",
                $"pageSize={pageSize}"
            };

            if (filters.Count > 0)
            {
                queryParams.Add("filters=" + Uri.EscapeDataString(filtersJson));
            }

            string queryString = string.Join("&", queryParams);
            string url = $"{_risUrl}{ApiEndpoints.Ris.OrderServices}?{queryString}";

            // Gửi request
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };

            string content = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<ResultPage<ChiDinhDichVuResponse>>(content, settings);

            return responseObj ?? new ResultPage<ChiDinhDichVuResponse>();
        }

        public async Task<ChiDinhDichVuResponse> GetChiDinhDichVuAsync(string maChiDinh)
        {
            var response = await _httpClient.GetAsync($"{_risUrl}{ApiEndpoints.Ris.OrderServices}/{maChiDinh}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

          
            var result = System.Text.Json.JsonSerializer.Deserialize<ChiDinhDichVuResponse>(
                content,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            return result;
        }
        #endregion

        #region Gợi ý kết luận
        public async Task<ResultPage<GoiYKetLuanDataGridView>> GetDanhSachGoiYKetLuanAsync(int page = 1, int pageSize = 50, string madichvu = null)
        {
            var filters = new List<FilterItem>();

            if (!string.IsNullOrWhiteSpace(madichvu))
                filters.Add(new FilterItem { Field = "madichvu", Operator = "eq", Value = madichvu });

            string filtersJson = JsonConvert.SerializeObject(filters);
            //string encodedFilters = HttpUtility.UrlEncode(filtersJson);

            // Build URL
            var builder = new UriBuilder(_risUrl + ApiEndpoints.Ris.ConclusionSuggestions);
            //builder.Port = -1;
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["page"] = page.ToString();
            query["pageSize"] = pageSize.ToString();

            if (filters.Count > 0)
                query["filters"] = filtersJson;

            builder.Query = query.ToString();
            string url = builder.ToString();

            // Gửi request
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };

            string content = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<ResultPage<GoiYKetLuanResponse>>(content, settings);

            var dataGridList = new List<GoiYKetLuanDataGridView>();

            foreach (var x in responseObj.data)
            {
                try
                {
                    dataGridList.Add(new GoiYKetLuanDataGridView
                    {
                        name = x.name,
                        createdAt = x.createdAt,
                        id = x.id,
                    });
                }
                catch (Exception ex)
                {

                }
            }

            return new ResultPage<GoiYKetLuanDataGridView>
            {
                total = responseObj.total,
                data = dataGridList
            };

        }

        public async Task<ResultPage<GoiYKetLuanResponse>> GetDanhSachGoiYKetLuanResponseAsync(int page = 1, int pageSize = 50, string madichvu = null)
        {
            var filters = new List<FilterItem>();

            if (!string.IsNullOrWhiteSpace(madichvu))
                filters.Add(new FilterItem { Field = "madichvu", Operator = "contains", Value = madichvu });

            string filtersJson = JsonConvert.SerializeObject(filters);
            //string encodedFilters = HttpUtility.UrlEncode(filtersJson);
            // Build URL
            var builder = new UriBuilder(_risUrl + ApiEndpoints.Ris.ConclusionSuggestions);
            //builder.Port = -1;
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["page"] = page.ToString();
            query["pageSize"] = pageSize.ToString();

            if (filters.Count > 0)
                query["filters"] = filtersJson;

            builder.Query = query.ToString();
            string url = builder.ToString();


            // Gửi request
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };

            string content = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<ResultPage<GoiYKetLuanResponse>>(content, settings);

            return responseObj ?? new ResultPage<GoiYKetLuanResponse>();
        }


        public async Task<GoiYKetLuanResponse> TaoMoiGoiYKetLuanAsync(GoiYKetLuanRequest request)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_risUrl + ApiEndpoints.Ris.ConclusionSuggestions, content);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = System.Text.Json.JsonSerializer.Deserialize<GoiYKetLuanResponse>(
                responseContent,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return result;
        }


        public async Task<GoiYKetLuanResponse> CapNhatGoiYKetLuanAsync(GoiYKetLuanRequest request, string id)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var requestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), $"{_risUrl}{ApiEndpoints.Ris.ConclusionSuggestions}/{id}")
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(requestMessage);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = System.Text.Json.JsonSerializer.Deserialize<GoiYKetLuanResponse>(
                responseContent,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return result;
        }

        public async Task<bool> XoaGoiYKetLuanAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"{_risUrl}{ApiEndpoints.Ris.ConclusionSuggestions}/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            // Nếu cần log chi tiết lỗi
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Xoá gợi ý kết luận thất bại. Status: {response.StatusCode}, Message: {error}");
        }


        public async Task<GoiYKetLuanResponse> GetGoiYKetLuanById(string id)
        {
            var response = await _httpClient.GetAsync($"{_risUrl}{ApiEndpoints.Ris.ConclusionSuggestions}/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var result = System.Text.Json.JsonSerializer.Deserialize<GoiYKetLuanResponse>(
                content,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return result;
        }
        #endregion

        #region Danh sách dich vụ
        //Danh sách dịch vụ
        public async Task<ResultPage<DanhMucDichVuResponse>> GetDSDichVuAsync(int page = 1, int pageSize = 500, string modality = null)
        {
            var filters = new List<FilterItem>();
            if (!string.IsNullOrWhiteSpace(modality))
                filters.Add(new FilterItem { Field = "modality", Operator = "eq", Value = modality });

            string filtersJson = JsonConvert.SerializeObject(filters);
            string encodedFilters = HttpUtility.UrlEncode(filtersJson);

            // Build URL
            var builder = new UriBuilder(_risUrl + ApiEndpoints.Ris.ServiceCatalog);
            //builder.Port = -1;
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["page"] = page.ToString();
            query["pageSize"] = pageSize.ToString();

            if (filters.Count > 0)
                query["filters"] = encodedFilters;

            builder.Query = query.ToString();
            string url = builder.ToString();

            // Gửi request
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };

            string content = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<ResultPage<DanhMucDichVuResponse>>(content, settings);

            return responseObj ?? new ResultPage<DanhMucDichVuResponse>();
        }

        public async Task<List<DanhMucDichVuGridView>> GetDanhSachDichVuAsync(int page = 1, int pageSize = 500, string modality = null, string tenDichVu = null, string maDichVu = null)
        {
            var filters = new List<FilterItem>();
            if (!string.IsNullOrWhiteSpace(modality))
                filters.Add(new FilterItem { Field = "modality", Operator = "contains", Value = modality });

            if (!string.IsNullOrWhiteSpace(maDichVu))
                filters.Add(new FilterItem { Field = "madichvu", Operator = "contains", Value = maDichVu });

            if (!string.IsNullOrWhiteSpace(tenDichVu))
                filters.Add(new FilterItem { Field = "tendichvu", Operator = "contains", Value = tenDichVu });

            string filtersJson = JsonConvert.SerializeObject(filters);
            string encodedFilters = HttpUtility.UrlEncode(filtersJson);

            // Build URL
            var builder = new UriBuilder(_risUrl + ApiEndpoints.Ris.ServiceCatalog);
            //builder.Port = -1;
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["page"] = page.ToString();
            query["pageSize"] = pageSize.ToString();

            if (filters.Count > 0)
                query["filters"] = encodedFilters;

            builder.Query = query.ToString();
            string url = builder.ToString();

            // Gửi request
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };

            string content = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<ResultPage<DanhMucDichVuResponse>>(content, settings);

            var dataGridList = new List<DanhMucDichVuGridView>();

            foreach (var x in responseObj.data)
            {
                try
                {
                    dataGridList.Add(new DanhMucDichVuGridView
                    {
                        madichvu = x.madichvu,
                        tendichvu = x.tendichvu,
                        id = x.id,
                    });
                }
                catch (Exception ex)
                {

                }
            }

            return dataGridList ?? new List<DanhMucDichVuGridView>();
        }

        public async Task<DanhMucDichVuResponse> GetDichVuAsync(string maDichVu)
        {
            var response = await _httpClient.GetAsync($"{_risUrl}{ApiEndpoints.Ris.ServiceCatalog}/{maDichVu}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var result = System.Text.Json.JsonSerializer.Deserialize<DanhMucDichVuResponse>(
                content,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return result;
        }
        #endregion

        #region Danh Sach thiết bị
        public async Task<ResultPage<ThietBiResponse>> GetDSThietBiAsync(
             int page = 1,
             int pageSize = 500,
             string loaiThietBi = null)
        {
            var filters = new List<FilterItem>();

            if (!string.IsNullOrWhiteSpace(loaiThietBi))
                filters.Add(new FilterItem
                {
                    Field = "loaiThietBi",
                    Operator = "eq",
                    Value = loaiThietBi
                });

            //// Serialize JSON filter
            //string filtersJson = JsonConvert.SerializeObject(filters);

            //// Build URL
            //var builder = new UriBuilder($"{_risUrl}/thiet-bi");
            //builder.Port = -1;

            //var query = HttpUtility.ParseQueryString(string.Empty);
            //query["page"] = page.ToString();
            //query["pageSize"] = pageSize.ToString();

            //if (filters.Count > 0)
            //    query["filters"] = filtersJson; 

            //builder.Query = query.ToString();
            //string url = builder.ToString();

            //var response = await _httpClient.GetAsync(url);
            //response.EnsureSuccessStatusCode();

            //var settings = new JsonSerializerSettings
            //{
            //    DateTimeZoneHandling = DateTimeZoneHandling.Local
            //};

            //string content = await response.Content.ReadAsStringAsync();
            //var responseObj = JsonConvert.DeserializeObject<ResultPage<ThietBiResponse>>(content, settings);

            //return responseObj;




            string filtersJson = JsonConvert.SerializeObject(filters);

            // Build query string thủ công (chuẩn UTF-8)
            var queryParams = new List<string>
            {
                $"page={page}",
                $"pageSize={pageSize}"
            };

            if (filters.Count > 0)
            {
                queryParams.Add("filters=" + Uri.EscapeDataString(filtersJson));
            }

            string queryString = string.Join("&", queryParams);
            string url = $"{_risUrl}{ApiEndpoints.Ris.Devices}?{queryString}";
            // Gửi request
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };

            string content = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<ResultPage<ThietBiResponse>>(content, settings);

            return responseObj ?? new ResultPage<ThietBiResponse>();
        }
        #endregion

        #region Danh Sach KTV
        public async Task<ResultPage<HisUserResponse>> GetDSNguoidungAsync(int page = 1, int pageSize = 500)
        {
            var filters = new List<FilterItem>();

            // Serialize JSON filter
            string filtersJson = JsonConvert.SerializeObject(filters);
            // Build URL
            var builder = new UriBuilder(_risUrl + ApiEndpoints.Ris.HisUserMapper);
            //builder.Port = -1;

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["page"] = page.ToString();
            query["pageSize"] = pageSize.ToString();

            if (filters.Count > 0)
                query["filters"] = filtersJson;

            builder.Query = query.ToString();
            string url = builder.ToString();

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };

            string content = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<ResultPage<HisUserResponse>>(content, settings);

            return responseObj;
        }

        public async Task<bool> HuyKetQuaChanDoanAsync(string machidinh, string lyDoHuy)
        {
            var body = new { lydoTrangThai = lyDoHuy };
            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var requestMessage = new HttpRequestMessage(
                new HttpMethod("PATCH"),
                $"{_risUrl}{ApiEndpoints.Ris.DiagnosticResults}/{machidinh}{ApiEndpoints.Ris.CancelSignedFile}")
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                // đọc JSON trả về (nếu cần message có thể log ra)
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CancelSignedFileResponse>(responseContent);
                return result.Success;
            }

            return false;
        }

        public async Task<BenhNhan> GetBenhNhanAsync(string mabenhnhan)
        {
            var response = await _httpClient.GetAsync($"{_risUrl}{ApiEndpoints.Ris.Patients}/{mabenhnhan}");

            if (!response.IsSuccessStatusCode)
            {
                // Nếu không có dữ liệu thì return null
                if (response.StatusCode == HttpStatusCode.NotFound ||
                    response.StatusCode == HttpStatusCode.NoContent)
                {
                    return null;
                }

                response.EnsureSuccessStatusCode();
            }

            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
                return null;

            var respon = System.Text.Json.JsonSerializer.Deserialize<ResponseResult<BenhNhan>>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            return respon.data;
        }



        #endregion

        public async Task<ChiDinhDichVuResponse> UpdateChiDinhDichVu(string maChiDinh, BenhNhan benhNhan)
        {
            // Build đúng body API cần
            var body = new
            {
                benhnhan = new
                {
                    mabenhnhan = benhNhan.MaBenhNhan,
                    ngaysinh = benhNhan.NgaySinh,
                    gioitinh = benhNhan.GioiTinh,
                    hoten = benhNhan.HoTen,
                    tungaybhyt = benhNhan.TuNgayBHYT,
                    denngaybhyt = benhNhan.DenNgayBHYT,
                    mabhyt = benhNhan.MaBHYT,
                    xaphuong = benhNhan.XaPhuong,
                    tinhthanh = benhNhan.TinhThanh,
                    dantoc = benhNhan.DanToc
                }
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var request = new HttpRequestMessage(
                new HttpMethod("PATCH"),
                $"{_risUrl}{ApiEndpoints.Ris.OrderServices}/{maChiDinh}"
            )
            {
                Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound ||
                    response.StatusCode == HttpStatusCode.NoContent)
                    return null;

                response.EnsureSuccessStatusCode();
            }

            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
                return null;

            var result =
                JsonConvert.DeserializeObject<ResponseResult<ChiDinhDichVuResponse>>(json);

            return result?.data;
        }


    }
}
// Class dùng để deserialize nhẹ
public class StudyLite
{
    public string ID { get; set; }
}
