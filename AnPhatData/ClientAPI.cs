using Newtonsoft.Json;
using PrintToPACSDemo.AnPhat.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintToPACSDemo.AnPhatData
{
    class ClientAPI
    {
        static readonly HttpClient HttpClient = new HttpClient();

        static string BaseUrl
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["baseURL"]; }
        }

        public static void Initialize()
        {
            HttpClient.BaseAddress = new Uri(BaseUrl);
        }

        public static async Task<T> Authencator<T>(object obj) where T : new()
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings[typeof(T).Name + "URL"] + "/login";
                HttpClient.DefaultRequestHeaders.Accept.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(obj);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await HttpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<T>(responseContent);
                    return result;
                }
                else
                {
                    return new T();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new T();
            }
        }

        public static async Task<T> GetByID<T>(int id) where T : new()
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings[typeof(T).Name + "URL"] + "/" + id.ToString();
                HttpClient.DefaultRequestHeaders.Accept.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await HttpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<T>(responseContent);
                    return result;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Đã xảy ra lỗi: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return new T();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new T();
            }
        }

        public static async Task<T> GetByField<T>(object obj) where T : new()
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings[typeof(T).Name + "URL"] + "/field";
                HttpClient.DefaultRequestHeaders.Accept.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(obj);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await HttpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<T>(responseContent);
                    return result;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Đã xảy ra lỗi: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return new T();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new T();
            }
        }

        public static async Task<List<T>> GetList<T>() where T : new()
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings[typeof(T).Name + "URL"] + "/list";
                List<T> list = new List<T>();
                HttpClient.DefaultRequestHeaders.Accept.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = HttpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<T>>(content);
                }
                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<List<T>> GetList<T>(object obj) where T : new()
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings[typeof(T).Name + "URL"] + "/list";
                List<T> list = new List<T>();
                HttpClient.DefaultRequestHeaders.Accept.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(obj);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await HttpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string res = await response.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<T>>(res);
                }
                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<List<T>> GetPageList<T>(object obj) where T : new()
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings[typeof(T).Name + "URL"] + "/pagelist";
                List<T> list = new List<T>();
                HttpClient.DefaultRequestHeaders.Accept.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(obj);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await HttpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string res = await response.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<T>>(res);
                }
                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<List<T>> GetListByField<T>(object obj) where T : new()
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings[typeof(T).Name + "URL"] + "/listfield";
                List<T> list = new List<T>();
                HttpClient.DefaultRequestHeaders.Accept.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(obj);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await HttpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string res = await response.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<T>>(res);
                }
                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<int> GetCount<T>() where T : new()
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings[typeof(T).Name + "URL"] + "/count";
                HttpClient.DefaultRequestHeaders.Accept.Clear();

                var response = await HttpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string res = await response.Content.ReadAsStringAsync();
                    return int.Parse(res);
                }
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }

        public static async Task<int> GetCount<T>(object obj) where T : new()
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings[typeof(T).Name + "URL"] + "/count";
                HttpClient.DefaultRequestHeaders.Accept.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(obj);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await HttpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string res = await response.Content.ReadAsStringAsync();
                    return int.Parse(res);
                }
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }

        public static async Task<int> GetCountByField<T>(object obj) where T : new()
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings[typeof(T).Name + "URL"] + "/countfield";
                HttpClient.DefaultRequestHeaders.Accept.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(obj);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await HttpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string res = await response.Content.ReadAsStringAsync();
                    return int.Parse(res);
                }
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }

        public static async Task<int?> Insert<T>(object o) where T : new()
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings[typeof(T).Name + "URL"] + "/insert";
                HttpClient.DefaultRequestHeaders.Accept.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(o);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await HttpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    int? newId = result?.id;
                    return newId;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Đã xảy ra lỗi: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<bool> Delete<T>(object id) where T : new()
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings[typeof(T).Name + "URL"] + "/" + id.ToString();
                HttpClient.DefaultRequestHeaders.Accept.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await HttpClient.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public static async Task DeleteAll<T>() where T : new()
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings[typeof(T).Name + "URL"] + "/deleteall";
                HttpClient.DefaultRequestHeaders.Accept.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await HttpClient.DeleteAsync(url);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static async Task<int?> Update<T>(int id, object o) where T : new()
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings[typeof(T).Name + "URL"] + "/update";
                HttpClient.DefaultRequestHeaders.Accept.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(o);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await HttpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    int? newId = result?.ID;
                    return newId;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Đã xảy ra lỗi: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<Certificate> GetCertificate(object obj)
        {
            string url = System.Configuration.ConfigurationManager.AppSettings["CertificateURL"] + "/get";
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string res = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Certificate>(res);
                return result;
            }
            return null;
        }

        public static async Task<Certificate> CreateCertificate(object obj)
        {
            string url = System.Configuration.ConfigurationManager.AppSettings["CertificateURL"];
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string res = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Certificate>(res);
                return result;
            }
            return null;
        }

        public static async Task DigitalSigninFile(string filePath, dynamic data, string FolderOuputPath)
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings["CertificateURL"] + "/digitalsignature";
                HttpClient.DefaultRequestHeaders.Accept.Clear();

                using (var form = new MultipartFormDataContent())
                {
                    string MIMEType = data.MIMEType;
                    string Username = data.Username;
                    form.Add(new StringContent(Username), "Username");
                    if (MIMEType.Equals("application/pdf"))
                    {
                        string DepartmentCode = data.DepartmentCode;
                        bool IsCheckSetting = data.IsCheckSetting;

                        form.Add(new StringContent(DepartmentCode), "DepartmentCode");
                        form.Add(new StringContent(IsCheckSetting.ToString()), "IsCheckSetting");

                        if (IsCheckSetting)
                        {
                            string ReasonText = data.ReasonText;
                            string LocationText = data.LocationText;
                            bool IsShowSignature = data.IsShowSignature;
                            int SignaturePage = data.SignaturePage;
                            bool IsGroupBasic = data.IsGroupBasic;

                            form.Add(new StringContent(ReasonText), "ReasonText");
                            form.Add(new StringContent(LocationText), "LocationText");
                            form.Add(new StringContent(IsShowSignature.ToString()), "IsShowSignature");
                            form.Add(new StringContent(SignaturePage.ToString()), "SignaturePage");
                            form.Add(new StringContent(IsGroupBasic.ToString()), "IsGroupBasic");

                            if (IsGroupBasic)
                            {
                                int AdvancedXCoord = data.AdvancedXCoord;
                                int AdvancedYCoord = data.AdvancedYCoord;
                                int AdvancedWidth = data.AdvancedWidth;
                                int AdvancedHeight = data.AdvancedHeight;

                                form.Add(new StringContent(AdvancedXCoord.ToString()), "AdvancedXCoord");
                                form.Add(new StringContent(AdvancedYCoord.ToString()), "AdvancedYCoord");
                                form.Add(new StringContent(AdvancedWidth.ToString()), "AdvancedWidth");
                                form.Add(new StringContent(AdvancedHeight.ToString()), "AdvancedHeight");
                            }
                            else
                            {
                                int BasicSignaturePosition = data.BasicSignaturePosition;
                                form.Add(new StringContent(BasicSignaturePosition.ToString()), "BasicSignaturePosition");
                            }
                        }
                    }
                    else if (MIMEType.Equals("application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
                    {

                    }
                    else if (MIMEType.Equals("application/xml"))
                    {

                    }


                    var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MIMEType);
                    form.Add(fileContent, "file", Path.GetFileName(filePath));

                    var response = await HttpClient.PostAsync(url, form);
                    if (response.IsSuccessStatusCode)
                    {
                        byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                        string fileName = response.Content.Headers.ContentDisposition.FileName;
                        File.WriteAllBytes(Path.Combine(FolderOuputPath, fileName), fileBytes);

                        MessageBox.Show($"Ký thành công nhận file tại: \"{Path.Combine(FolderOuputPath, fileName)}\"", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Ký không thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static bool IsFileType(string filename, string expectedExtension)
        {
            // Kiểm tra nếu tên file không null hoặc trống và có phần mở rộng như mong muốn
            return !string.IsNullOrEmpty(filename) && Path.GetExtension(filename).Equals(expectedExtension, StringComparison.OrdinalIgnoreCase);
        }

        public static async Task DigitalSigninFileTryAgain(int id, string FolderOuputPath)
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings["CertificateURL"] + "/digitalsignature" + "/" + id.ToString();
                HttpClient.DefaultRequestHeaders.Accept.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await HttpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                    string fileName = response.Content.Headers.ContentDisposition.FileName;

                    File.WriteAllBytes(Path.Combine(FolderOuputPath, fileName), fileBytes);

                    MessageBox.Show($"Ký thành công nhận file tại: \"{Path.Combine(FolderOuputPath, fileName)}\"", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Ký không thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static async Task DownloadFile(string folderPath, string fileName, string staffCode)
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings["CertificateURL"] + $"/download?fileName={fileName}&staffcode={staffCode}";
                HttpClient.DefaultRequestHeaders.Accept.Clear();

                var response = await HttpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                    File.WriteAllBytes(Path.Combine(folderPath, fileName), fileBytes);
                    MessageBox.Show($"Tải về thành công file : \"{Path.Combine(folderPath, fileName)}\"", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Tải về thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
