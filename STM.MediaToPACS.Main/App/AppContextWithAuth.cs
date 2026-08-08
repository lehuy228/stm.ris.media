using MediaToPacs.Core.Auths;
using MediaToPacs.Core.Interfaces;
using MediaToPacs.Infrastructure.Auths;
using MediaToPacs.Infrastructure.Services;
using STM.MediaToPACS.Main.UI;
using Serilog;
using STM.MediaToPacs.Connection.AuthSDK;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STM.MediaToPACS.Main.Utilities
{
    public class AppContextWithAuth : ApplicationContext
    {
        private readonly SynchronizationContext _uiContext;
        private readonly ISessionService _sessionService;
        private readonly IRisService _risService;
        private readonly IRisService2 _risService2;
        private readonly ISignatureService _signatureService;
        private readonly IHisService _hisService;
        private CancellationTokenSource _authCts;
        private bool _isAuthenticationInProgress;
        private SplashForm _splash;

        public AppContextWithAuth(
            ISessionService sessionService,
            IRisService risService,
            IRisService2 risService2,
            ISignatureService signatureService,
            IHisService hisService)
        {
            _sessionService = sessionService;
            _risService = risService;
            _risService2 = risService2;
            _signatureService = signatureService;
            _hisService = hisService;

            _uiContext = SynchronizationContext.Current ?? new WindowsFormsSynchronizationContext();

            _splash = new SplashForm();
            _splash.Show();

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            try
            {
                // Update chạy fail-open: mọi lỗi cấu hình/mạng/API đều không được chặn đăng nhập.
                try
                {
                    ServiceLocator.InitializeUpdateService();
                    if (await CheckForUpdatesAsync())
                        return;
                }
                catch (Exception updateException)
                {
                    Log.Warning(updateException,
                        "Không thể khởi tạo kiểm tra cập nhật; tiếp tục vào màn hình đăng nhập");
                    _splash.SetStatus("Không thể kiểm tra cập nhật, đang chuyển đến đăng nhập...");
                }

                // Bắt đầu quá trình xác thực
                await AuthenticateAndStartApplicationAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Lỗi nghiêm trọng khi khởi tạo ứng dụng");
                ShowErrorAndExit("Không thể khởi động ứng dụng. Vui lòng liên hệ quản trị viên.", "Lỗi khởi động");
            }
        }

        private async Task<bool> CheckForUpdatesAsync()
        {
            try
            {
                Log.Information("Kiểm tra cập nhật ứng dụng...");
                return await Updater.CheckAndUpdate(_risService2, _splash.SetStatus, _splash.SetProgress);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi kiểm tra cập nhật");
                // Không dừng ứng dụng nếu việc kiểm tra cập nhật thất bại
                return false;
            }
        }

        private async Task AuthenticateAndStartApplicationAsync()
        {
            if (_isAuthenticationInProgress)
            {
                Log.Warning("Xác thực đã đang trong quá trình thực hiện");
                return;
            }

            _isAuthenticationInProgress = true;
            _authCts = new CancellationTokenSource();

            try
            {
                Log.Information("Bắt đầu quá trình xác thực...");
                _splash.SetStatus("Đang xác thực người dùng...");

                var tokenData = await GetTokenWithRetryAsync(timeoutSeconds: 120, maxRetries: 2);

                if (tokenData == null || string.IsNullOrEmpty(tokenData.access_token))
                {
                    throw new InvalidOperationException("Không lấy được token hợp lệ");
                }

                _splash.SetStatus("Đang khởi tạo ứng dụng...");
                await InitializeUserSessionAsync(tokenData);
                // Cấu hình lại các client với Bearer token vừa lấy được (lần trước chưa có token).
                ServiceLocator.InitializeOptionalServices();

                _splash?.CloseSplash();
                _splash = null;

                bool confirmed = await ShowDoctorConfirmAsync();
                if (!confirmed)
                {
                    Log.Warning("Người dùng đã hủy tại màn hình xác nhận thông tin bác sĩ");
                    ShowMessageAndExit("Đã hủy đăng nhập.", "Thông báo");
                    return;
                }

                ShowMainForm();

                Log.Information("Đăng nhập thành công");
            }
            catch (OperationCanceledException)
            {
                Log.Warning("Người dùng đã hủy đăng nhập");
                ShowMessageAndExit("Đăng nhập đã bị hủy.", "Thông báo");
            }
            catch (TimeoutException ex)
            {
                Log.Warning(ex, "Hết thời gian chờ đăng nhập");
                ShowMessageAndExit("Hết thời gian chờ đăng nhập. Vui lòng thử lại.", "Timeout");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Đăng nhập thất bại");
                ShowErrorAndExit($"Đăng nhập thất bại: {ex.Message}", "Lỗi xác thực");
            }
            finally
            {
                _isAuthenticationInProgress = false;
                _authCts?.Dispose();
                _authCts = null;
                Token.Cancel();
            }
        }

        private async Task<TokenData> GetTokenWithRetryAsync(int timeoutSeconds, int maxRetries)
        {
            int retryCount = 0;
            Exception lastException = null;

            while (retryCount <= maxRetries)
            {
                try
                {
                    if (retryCount > 0)
                    {
                        Log.Information($"Thử lại lấy token lần {retryCount}/{maxRetries}");
                        await Task.Delay(1000); // Đợi 1 giây trước khi thử lại
                    }

                    return await Token.GetToken(timeoutSeconds);
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    retryCount++;

                    if (retryCount > maxRetries)
                    {
                        break;
                    }

                    Log.Warning(ex, $"Lần thử {retryCount} thất bại");
                }
            }

            throw lastException ?? new InvalidOperationException("Không thể lấy token sau nhiều lần thử");
        }

        private async Task InitializeUserSessionAsync(TokenData tokenData)
        {
            try
            {
                Log.Information("Đang lấy thông tin người dùng...");

                // Lấy thông tin người dùng từ token
                ServiceLocator.KeycloakUserInfo = await KeycloakService.GetUserInfoFromToken(tokenData.access_token);

                if (ServiceLocator.KeycloakUserInfo == null)
                {
                    throw new InvalidOperationException("Không thể lấy thông tin người dùng từ token");
                }

                Log.Information($"Đăng nhập thành công với user: {ServiceLocator.KeycloakUserInfo.Username}");

                // Gán token vào session service (instance đã được tạo sẵn ở ServiceLocator.Initialize())
                _sessionService.SetToken(
                    tokenData.access_token,
                    tokenData.refresh_token,
                    DateTime.Now.AddSeconds(tokenData.expires_in)
                );

                Log.Information("Session đã được khởi tạo thành công");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi khởi tạo user session");
                throw;
            }
        }

        private Task<bool> ShowDoctorConfirmAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            _uiContext.Post(_ =>
            {
                try
                {
                    using (var frm = new DoctorConfirmForm(_risService2))
                    {
                        var result = frm.ShowDialog();
                        tcs.SetResult(result == DialogResult.OK);
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }, null);

            return tcs.Task;
        }

        private void ShowMainForm()
        {
            _uiContext.Post(_ =>
            {
                try
                {
                    Log.Information("Đang mở MainForm...");

                    _splash?.CloseSplash();
                    _splash = null;

                    var mainForm = new MainForm(_sessionService, _risService, _risService2, _signatureService, _hisService);
                    mainForm.FormClosed += OnMainFormClosed;

                    MainForm = mainForm;
                    mainForm.Show();

                    Log.Information("MainForm đã được hiển thị");
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Lỗi khi mở MainForm");
                    ShowErrorAndExit($"Lỗi khi mở giao diện chính: {ex.Message}", "Lỗi UI");
                }
            }, null);
        }

        private void OnMainFormClosed(object sender, EventArgs e)
        {
            try
            {
                Log.Information("MainForm đã được đóng");

                // Cleanup resources
                CleanupResources();

                // Exit application
                ExitThread();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi đóng ứng dụng");
            }
        }

        private void CleanupResources()
        {
            try
            {
                // Hủy authentication nếu đang chạy
                _authCts?.Cancel();
                _authCts?.Dispose();

                // Hủy HTTP Listener
                Token.Cancel();

                //// Đăng xuất nếu có refresh token
                //if (!string.IsNullOrEmpty(ServiceLocator.SessionService?.RefreshToken))
                //{
                //    _ = Token.Logout(ServiceLocator.SessionService.RefreshToken);
                //}

                Log.Information("Cleanup resources thành công");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi cleanup resources");
            }
        }

        private void ShowMessageAndExit(string message, string title)
        {
            _uiContext.Post(_ =>
            {
                _splash?.CloseSplash();
                _splash = null;

                MessageBox.Show(
                    message,
                    title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                ExitThread();
            }, null);
        }

        private void ShowErrorAndExit(string message, string title)
        {
            _uiContext.Post(_ =>
            {
                _splash?.CloseSplash();
                _splash = null;

                MessageBox.Show(
                    message,
                    title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                ExitThread();
            }, null);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CleanupResources();
            }
            base.Dispose(disposing);
        }
    }
}
