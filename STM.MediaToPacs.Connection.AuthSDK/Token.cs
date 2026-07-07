using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
        private static readonly object _lockObject = new object();
        private static CancellationTokenSource _currentCts;

        #region Public Methods

        /// <summary>
        /// Hủy bỏ HttpListener hiện tại một cách an toàn
        /// </summary>
        public static void Cancel()
        {
            lock (_lockObject)
            {
                try
                {
                    // Hủy CancellationTokenSource nếu đang tồn tại
                    _currentCts?.Cancel();
                    _currentCts?.Dispose();
                    _currentCts = null;

                    if (_httpListener == null) return;

                    if (_httpListener.IsListening)
                    {
                        _httpListener.Stop();
                    }

                    _httpListener.Close();
                }
                catch (ObjectDisposedException ex)
                {
                    Debug.WriteLine($"Listener đã bị dispose: {ex.Message}");
                }
                catch (HttpListenerException ex)
                {
                    Debug.WriteLine($"HttpListener lỗi: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Lỗi không xác định khi hủy listener: {ex.Message}");
                }
                finally
                {
                    _httpListener = null;
                }
            }
        }

        /// <summary>
        /// Đăng xuất khỏi Keycloak
        /// </summary>
        public static async Task<bool> Logout(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new ArgumentException("Refresh token không được để trống", nameof(refreshToken));
            }

            var config = GetAuthConfiguration();

            try
            {
                using (var http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) })
                {
                    var logoutUrl = $"{config.AuthUrl}/realms/{config.Realm}/protocol/openid-connect/logout";

                    var content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "client_id", config.ClientId },
                        { "refresh_token", refreshToken }
                    });

                    var response = await http.PostAsync(logoutUrl, content);

                    // Mở trình duyệt để xóa cookie phía Keycloak
                    var browserLogoutUrl = $"{config.AuthUrl}/realms/{config.Realm}/protocol/openid-connect/logout?redirect_uri={config.RedirectUri}";
                    Process.Start(new ProcessStartInfo(browserLogoutUrl) { UseShellExecute = true });

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi đăng xuất: {ex.Message}");
                return false;
            }
            finally
            {
                Cancel();
            }
        }

        /// <summary>
        /// Lấy token từ Keycloak thông qua Authorization Code Flow với PKCE
        /// </summary>
        public static async Task<TokenData> GetToken(int timeoutSeconds = 120)
        {
            if (timeoutSeconds <= 0)
            {
                throw new ArgumentException("Timeout phải lớn hơn 0", nameof(timeoutSeconds));
            }

            var config = GetAuthConfiguration();
            var pkce = GeneratePKCE();

            try
            {
                InitializeHttpListener(config.RedirectUri);
                OpenAuthorizationBrowser(config, pkce.CodeChallenge);

                var authorizationCode = await WaitForAuthorizationCode(timeoutSeconds);
                await SendSuccessResponse();

                return await ExchangeCodeForToken(config, authorizationCode, pkce.CodeVerifier);
            }
            catch (TimeoutException)
            {
                throw new TimeoutException("Người dùng không hoàn tất đăng nhập trong thời gian cho phép.");
            }
            catch (Exception ex)
            {
                throw new OperationCanceledException($"Đăng nhập bị hủy hoặc lỗi: {ex.Message}", ex);
            }
            finally
            {
                Cancel();
            }
        }

        #endregion

        #region Private Helper Methods

        private static AuthConfiguration GetAuthConfiguration()
        {
            return new AuthConfiguration
            {
                AuthUrl = ConfigurationManager.AppSettings["Auth:URL"]
                    ?? throw new ConfigurationErrorsException("Auth:URL không được cấu hình"),
                Realm = ConfigurationManager.AppSettings["Auth:REALM"]
                    ?? throw new ConfigurationErrorsException("Auth:REALM không được cấu hình"),
                RedirectUri = ConfigurationManager.AppSettings["Auth:REDIRECTURI"]
                    ?? throw new ConfigurationErrorsException("Auth:REDIRECTURI không được cấu hình"),
                ClientId = ConfigurationManager.AppSettings["Auth:CLIENTID"]
                    ?? throw new ConfigurationErrorsException("Auth:CLIENTID không được cấu hình")
            };
        }

        private static void InitializeHttpListener(string redirectUri)
        {
            lock (_lockObject)
            {
                // Hủy listener cũ nếu tồn tại
                Cancel();

                _httpListener = new HttpListener();
                _httpListener.Prefixes.Add(redirectUri.TrimEnd('/') + "/");

                try
                {
                    _httpListener.Start();
                }
                catch (HttpListenerException ex)
                {
                    throw new InvalidOperationException(
                        $"Không thể khởi động HTTP Listener. Đảm bảo ứng dụng chạy với quyền Administrator hoặc port đã được đăng ký. Chi tiết: {ex.Message}",
                        ex);
                }
            }
        }

        private static void OpenAuthorizationBrowser(AuthConfiguration config, string codeChallenge)
        {
            var authUrl = $"{config.AuthUrl}/realms/{config.Realm}/protocol/openid-connect/auth" +
                          $"?client_id={Uri.EscapeDataString(config.ClientId)}" +
                          $"&response_type=code" +
                          $"&scope=openid%20profile%20email" +
                          $"&redirect_uri={Uri.EscapeDataString(config.RedirectUri)}" +
                          $"&code_challenge={codeChallenge}" +
                          $"&code_challenge_method=S256";

            try
            {
                Process.Start(new ProcessStartInfo(authUrl) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Không thể mở trình duyệt: {ex.Message}", ex);
            }
        }

        private static async Task<string> WaitForAuthorizationCode(int timeoutSeconds)
        {
            lock (_lockObject)
            {
                _currentCts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));
            }

            try
            {
                var context = await _httpListener.GetContextAsync().WaitAsync(_currentCts.Token);

                var code = context.Request.QueryString["code"];
                var error = context.Request.QueryString["error"];
                var errorDescription = context.Request.QueryString["error_description"];

                if (!string.IsNullOrEmpty(error))
                {
                    throw new InvalidOperationException(
                        $"Lỗi xác thực: {error}. {errorDescription ?? "Không có mô tả lỗi"}");
                }

                if (string.IsNullOrEmpty(code))
                {
                    throw new InvalidOperationException("Không nhận được authorization code từ Keycloak");
                }

                // Lưu context để gửi response sau
                _currentContext = context;
                return code;
            }
            catch (OperationCanceledException)
            {
                throw new TimeoutException($"Hết thời gian chờ đăng nhập ({timeoutSeconds}s)");
            }
        }

        private static HttpListenerContext _currentContext;

        private static async Task SendSuccessResponse()
        {
            if (_currentContext == null) return;

            try
            {
                var responseHtml = @"<!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Đăng nhập thành công</title>
                    <style>
                        body {
                            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                            display: flex;
                            justify-content: center;
                            align-items: center;
                            height: 100vh;
                            margin: 0;
                            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                        }
                        .container {
                            background: white;
                            padding: 40px;
                            border-radius: 10px;
                            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
                            text-align: center;
                        }
                        .success-icon {
                            color: #4CAF50;
                            font-size: 60px;
                            margin-bottom: 20px;
                        }
                        h1 {
                            color: #333;
                            margin-bottom: 10px;
                        }
                        p {
                            color: #666;
                            margin-bottom: 20px;
                        }
                        .close-info {
                            color: #999;
                            font-size: 14px;
                        }
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='success-icon'>✓</div>
                        <h1>Đăng nhập thành công!</h1>
                        <p>Bạn đã đăng nhập thành công vào hệ thống.</p>
                        <p class='close-info'>Bạn có thể đóng tab này và quay lại ứng dụng.</p>
                    </div>
                    <script>
                        // Tự động đóng tab sau 3 giây
                        setTimeout(() => {
                            window.close();
                        }, 3000);
                    </script>
                </body>
                </html>";

                var buffer = Encoding.UTF8.GetBytes(responseHtml);
                _currentContext.Response.ContentType = "text/html; charset=utf-8";
                _currentContext.Response.ContentLength64 = buffer.Length;
                _currentContext.Response.StatusCode = 200;

                await _currentContext.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                _currentContext.Response.OutputStream.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi gửi response: {ex.Message}");
            }
            finally
            {
                _currentContext = null;
            }
        }

        private static async Task<TokenData> ExchangeCodeForToken(
            AuthConfiguration config,
            string authorizationCode,
            string codeVerifier)
        {
            using (var http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) })
            {
                var tokenUrl = $"{config.AuthUrl}/realms/{config.Realm}/protocol/openid-connect/token";

                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", "authorization_code" },
                    { "code", authorizationCode },
                    { "redirect_uri", config.RedirectUri },
                    { "client_id", config.ClientId },
                    { "code_verifier", codeVerifier }
                });

                var response = await http.PostAsync(tokenUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new InvalidOperationException(
                        $"Không thể lấy token. Status: {response.StatusCode}. Chi tiết: {errorContent}");
                }

                var json = await response.Content.ReadAsStringAsync();
                var tokenData = JsonConvert.DeserializeObject<TokenData>(json);

                if (tokenData == null || string.IsNullOrEmpty(tokenData.access_token))
                {
                    throw new InvalidOperationException("Token data không hợp lệ");
                }

                return tokenData;
            }
        }

        #endregion

        #region PKCE Generation

        private static PKCEData GeneratePKCE()
        {
            var codeVerifier = GenerateCodeVerifier();
            var codeChallenge = GenerateCodeChallenge(codeVerifier);

            return new PKCEData
            {
                CodeVerifier = codeVerifier,
                CodeChallenge = codeChallenge
            };
        }

        private static string GenerateCodeVerifier()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[32];
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes)
                    .TrimEnd('=')
                    .Replace('+', '-')
                    .Replace('/', '_');
            }
        }

        private static string GenerateCodeChallenge(string codeVerifier)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.ASCII.GetBytes(codeVerifier));
                return Convert.ToBase64String(hash)
                    .TrimEnd('=')
                    .Replace('+', '-')
                    .Replace('/', '_');
            }
        }

        #endregion

        #region Helper Classes

        private class AuthConfiguration
        {
            public string AuthUrl { get; set; }
            public string Realm { get; set; }
            public string RedirectUri { get; set; }
            public string ClientId { get; set; }
        }

        private class PKCEData
        {
            public string CodeVerifier { get; set; }
            public string CodeChallenge { get; set; }
        }

        #endregion
    }

    #region Extension Methods

    public static class TaskExtensions
    {
        public static async Task<T> WaitAsync<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task))
                {
                    throw new OperationCanceledException(cancellationToken);
                }
            }
            return await task;
        }
    }

    #endregion
}