using System;
using System.Threading;
using System.Windows.Forms;
using PrintToPACSDemo.UI;
using STMMedicalConnection.AuthSDK;

namespace PrintToPACSDemo
{
    public partial class SplashAuthForm : DevExpress.XtraEditors.XtraForm
    {
        private CancellationTokenSource _cts;

        public SplashAuthForm()
        {
            InitializeComponent();
            _progressPanel.Visible = false;
        }

        private async void _btnLogin_Click(object sender, EventArgs e)
        {
            _btnLogin.Enabled = false;
            _btnCancel.Enabled = true;
            _progressPanel.Visible = true;
            _progressPanel.Caption = "Đang xác thực...";
            _progressPanel.Description = "Vui lòng chờ đăng nhập từ trình duyệt...";

            _cts = new CancellationTokenSource();

            try
            {
                var token = await Token.GetToken(
                    "https://auth.stmjsc.com",
                    "his-demo",
                    "http://localhost:7890/callback"
                );

                if (_cts.IsCancellationRequested)
                    return;

                if (!string.IsNullOrEmpty(token?.access_token))
                {
                    var mainForm = new WorkListTable();
                    mainForm.FormClosed += (s, args) => this.Close();
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Không lấy được token. Hãy thử lại.", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Đăng nhập đã bị hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đăng nhập thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _btnLogin.Enabled = true;
                _btnCancel.Enabled = false;
                _progressPanel.Visible = false;
                _cts.Dispose();
                _cts = null;
            }
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            _cts?.Cancel();
            Token.Cancel();

            _btnCancel.Enabled = false;
            _btnLogin.Enabled = true;
            _progressPanel.Visible = false;
        }
    }
}
