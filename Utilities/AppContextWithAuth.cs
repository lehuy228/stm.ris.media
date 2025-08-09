using MediaToPacs.Infrastructure.Auths;
using PrintToPACSDemo.UI;
using STMMedicalConnection.AuthSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintToPACSDemo.Utilities
{
    public class AppContextWithAuth : ApplicationContext
    {
        private readonly SynchronizationContext _uiContext;

        public AppContextWithAuth()
        {
            // Tạo context chuẩn của WinForms UI
            _uiContext = new WindowsFormsSynchronizationContext();

            // Bắt đầu xử lý đăng nhập mà không chặn UI thread
            GetTokenInBackground();
        }

        private void GetTokenInBackground()
        {
            Task.Run(async () =>
            {
                try
                {
                    var token = await Token.GetToken("https://auth.stmjsc.com", "his-demo", "http://localhost:7890/callback");

                    if (!string.IsNullOrEmpty(token?.access_token))
                    {
                        ServiceLocator.SessionService = new SessionService();
                        ServiceLocator.SessionService.SetToken(
                            token.access_token,
                            token.refresh_token,
                            DateTime.Now.AddSeconds(token.expires_in)
                        );

                        _uiContext.Post(_ =>
                        {
                            var mainForm = new WorkListTable();
                            mainForm.FormClosed += (s, e) => ExitThread();
                            MainForm = mainForm;
                            mainForm.Show();
                        }, null);
                    }
                    else
                    {
                        _uiContext.Post(_ =>
                        {
                            MessageBox.Show("Không lấy được token. Hãy thử lại.", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ExitThread();
                        }, null);
                    }
                }
                catch (OperationCanceledException)
                {
                    _uiContext.Post(_ =>
                    {
                        MessageBox.Show("Đăng nhập đã bị hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ExitThread();
                    }, null);
                }
                catch (Exception ex)
                {
                    _uiContext.Post(_ =>
                    {
                        MessageBox.Show("Đăng nhập thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ExitThread();
                    }, null);
                }
                finally
                {
                    Token.Cancel();
                }
            });
        }
    }
}
