using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace STM.MediaToPacs.Connection.AuthSDK
{
    public static class Token
    {
        private static HttpListener _httpListener;

        public static void Cancel()
        {
            if (_httpListener == null) return;

            try
            {
                if (_httpListener.IsListening)
                    _httpListener.Stop();

                _httpListener.Close();
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine("Listener đã bị dispose: " + ex.Message);
            }
            catch (HttpListenerException ex)
            {
                Console.WriteLine("HttpListener lỗi: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi không xác định khi huỷ listener: " + ex.Message);
            }
            finally
            {
                _httpListener = null;
            }
        }

        //public static async Task<bool> Logout(string refreshToken)
        //{
        //    string authUrl = ConfigurationManager.AppSettings["Auth:URL"];
        //    string realm = ConfigurationManager.AppSettings["Auth:REALM"];
        //    string redirectUri = ConfigurationManager.AppSettings["Auth:REDIRECTURI"];
        //    string clientId = ConfigurationManager.AppSettings["Auth:CLIENTID"];
        //    using (var http = new HttpClient())
        //    {
        //        var response = await http.PostAsync(
        //            $"{authUrl}/realms/{realm}/protocol/openid-connect/logout",
        //            new FormUrlEncodedContent(new Dictionary<string, string>
        //            {
        //                { "client_id", clientId },
        //                { "refresh_token", refreshToken }
        //        }));

        //        Cancel();

        //        return response.IsSuccessStatusCode;
        //    }
        //}

        public static async Task<bool> Logout(string refreshToken)
        {
            string authUrl = ConfigurationManager.AppSettings["Auth:URL"];
            string realm = ConfigurationManager.AppSettings["Auth:REALM"];
            string redirectUri = ConfigurationManager.AppSettings["Auth:REDIRECTURI"];
            string clientId = ConfigurationManager.AppSettings["Auth:CLIENTID"];

            using (var http = new HttpClient())
            {
                var response = await http.PostAsync(
                    $"{authUrl}/realms/{realm}/protocol/openid-connect/logout?redirect_uri={redirectUri}",
                    new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                { "client_id", clientId },
                { "refresh_token", refreshToken }
                    }));

                Cancel();

                // Sau khi logout xong, mở trình duyệt để xóa cookie phía Keycloak
                System.Diagnostics.Process.Start($"{authUrl}/realms/{realm}/protocol/openid-connect/logout?redirect_uri={redirectUri}");

                return response.IsSuccessStatusCode;
            }
        }
        public static async Task<TokenData> GetToken(int timeoutSeconds = 120)
        {
            string authUrl = ConfigurationManager.AppSettings["Auth:URL"];
            string realm = ConfigurationManager.AppSettings["Auth:REALM"];
            string redirectUri = ConfigurationManager.AppSettings["Auth:REDIRECTURI"];
            string clientId = ConfigurationManager.AppSettings["Auth:CLIENTID"];
            // PKCE
            string codeVerifier = GenerateCodeVerifier();
            string codeChallenge = GenerateCodeChallenge(codeVerifier);

            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(redirectUri + "/");
            _httpListener.Start();

            var url = $"{authUrl}/realms/{realm}/protocol/openid-connect/auth" +
                      $"?client_id={clientId}" +
                      $"&response_type=code" +
                      $"&scope=openid%20profile" +
                      $"&redirect_uri={redirectUri}" +
                      $"&code_challenge={codeChallenge}" +
                      $"&code_challenge_method=S256";

            // mở trình duyệt
            System.Diagnostics.Process.Start(url);

            try
            {
                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds)))
                {
                    var contextTask = _httpListener.GetContextAsync();
                    var completed = await Task.WhenAny(contextTask, Task.Delay(-1, cts.Token));

                    if (completed != contextTask)
                    {
                        Cancel();
                        throw new TimeoutException("Người dùng không hoàn tất đăng nhập trong thời gian cho phép.");
                    }

                    var context = contextTask.Result;
                    string code = context.Request.QueryString["code"];

                    var responseString = @"<!DOCTYPE html>
                        <html>
                        <head>
                            <meta charset=""UTF-8"">
                            <title>Đăng nhập</title>
                        </head>
                        <body>
                            Đăng nhập thành công! Bạn có thể đóng tab này.
                        </body>
                        </html>";
                    var buffer = Encoding.UTF8.GetBytes(responseString);
                    context.Response.ContentLength64 = buffer.Length;
                    await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                    context.Response.OutputStream.Close();

                    Cancel();

                    // Đổi code sang token
                    var http = new HttpClient();
                    var tokenResponse = await http.PostAsync($"{authUrl}/realms/{realm}/protocol/openid-connect/token",
                        new FormUrlEncodedContent(new Dictionary<string, string>
                        {
                            { "grant_type", "authorization_code" },
                            { "code", code },
                            { "redirect_uri", redirectUri },
                            { "client_id", clientId },
                            { "code_verifier", codeVerifier }
                        }));

                    var json = await tokenResponse.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TokenData>(json);
                }
            }
            catch (TimeoutException) { throw; }
            catch (Exception ex)
            {
                Cancel();
                throw new OperationCanceledException("Đăng nhập bị hủy hoặc lỗi: " + ex.Message);
            }
        }

        private static string GenerateCodeVerifier()
        {
            var bytes = new byte[32];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        private static string GenerateCodeChallenge(string codeVerifier)
        {
            var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.ASCII.GetBytes(codeVerifier));
            return Convert.ToBase64String(hash)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }
    }
}
