using MediaToPacs.Infrastructure.Auths;
using MediaToPacs.Infrastructure.Services;
using STM.MediaToPACS.Main.UI;
using Serilog;
using STM.MediaToPacs.Connection.AuthSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STM.MediaToPACS.Main.Utilities
{
    public class AppContextWithAuth : ApplicationContext
    {
        private readonly SynchronizationContext _uiContext;

        public AppContextWithAuth()
        {
            _uiContext = SynchronizationContext.Current ?? new WindowsFormsSynchronizationContext();
            GetTokenInBackground();
        }

        private async void GetTokenInBackground()
        {
            if (await Updater.CheckAndUpdate())
            {
                return;
            }

            Task.Run(async () =>
            {
                try
                {
                    var token = await Token.GetToken(30);

                    if (!string.IsNullOrEmpty(token?.access_token))
                    {
                        ServiceLocator.KeycloakUserInfo = await KeycloakService.GetUserInfoFromToken(token?.access_token);

                        ServiceLocator.SessionService = new SessionService();
                        ServiceLocator.SessionService.SetToken(
                            token.access_token,
                            token.refresh_token,
                            DateTime.Now.AddSeconds(token.expires_in)
                        );

                        _uiContext.Post(_ =>
                        {
                            try
                            {
                                var mainForm = new WorkListTable();
                                mainForm.FormClosed += (s, e) => ExitThread();
                                MainForm = mainForm;
                                mainForm.Show();
                            }
                            catch (Exception ex)
                            {
                                Log.Fatal(ex.Message);
                                MessageBox.Show("Lỗi khi mở MainForm: " + ex.Message, "Lỗi UI",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                ExitThread();
                            }
                        }, null);
                    }
                    else
                    {
                        ShowMessageAndExit("Không lấy được token. Hãy thử lại.", "Lỗi xác thực");
                    }
                }
                catch (OperationCanceledException)
                {
                    ShowMessageAndExit("Đăng nhập đã bị hủy.", "Thông báo");
                }
                catch (Exception ex)
                {
                    ShowMessageAndExit("Đăng nhập thất bại: " + ex.Message, "Lỗi");
                }
                finally
                {
                    Token.Cancel();
                }
            });
        }

        private void ShowMessageAndExit(string message, string title)
        {
            _uiContext.Post(_ =>
            {
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ExitThread();
            }, null);
        }
    }
}
