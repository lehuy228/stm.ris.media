using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
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

        public static async Task<TokenData> GetToken(string authUrl, string clientId, string redirectUri)
        {
            // Generate PKCE code verifier & challenge
            string codeVerifier = GenerateCodeVerifier();
            string codeChallenge = GenerateCodeChallenge(codeVerifier);

            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(redirectUri + "/");
            _httpListener.Start();

            var url = $"{authUrl}/realms/master/protocol/openid-connect/auth" +
                      $"?client_id={clientId}" +
                      $"&response_type=code" +
                      $"&scope=openid%20profile" +
                      $"&redirect_uri={redirectUri}" +
                      $"&code_challenge={codeChallenge}" +
                      $"&code_challenge_method=S256";

            System.Diagnostics.Process.Start(url);

            HttpListenerContext context;
            try
            {
                context = await _httpListener.GetContextAsync();
            }
            catch (Exception ex)
            {
                Cancel(); // Ensure cleanup
                throw new OperationCanceledException("Listener bị hủy hoặc gặp lỗi: " + ex.Message);
            }

            string code = context.Request.QueryString["code"];

            // Trả lời trình duyệt
            var responseString = "<html><body>Đăng nhập thành công! Bạn có thể đóng tab này.</body></html>";
            var buffer = Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentLength64 = buffer.Length;
            await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();

            Cancel(); // stop listener sau khi xong

            // Đổi code sang token
            var http = new HttpClient();
            var tokenResponse = await http.PostAsync($"{authUrl}/realms/master/protocol/openid-connect/token",
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
