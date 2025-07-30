
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintToPACSDemo.UI.Login
{
    public partial class ResetPassword : DevExpress.XtraEditors.XtraUserControl
    {
        private ErrorProvider errorProvider = new ErrorProvider();
        public EventHandler LoginClick_Action;
        public ResetPassword()
        {
            InitializeComponent();
        }

        private void checkBoxShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (_cbShowPass.Checked)
            {
                _txtNewPassword.Properties.PasswordChar = '\0';
                _txtPasswordCurrent.Properties.PasswordChar = '\0';
                _txtConfirmPassword.Properties.PasswordChar = '\0';
            }
            else
            {
                _txtNewPassword.Properties.PasswordChar = '*';
                _txtPasswordCurrent.Properties.PasswordChar = '*';
                _txtConfirmPassword.Properties.PasswordChar = '*';
            }
        }

        private async Task<bool> isPasswordCurent()
        {
            errorProvider.Clear();
            var loginData = new
            {
                username = _txtUsername.Text.Trim(),
                password = _txtPasswordCurrent.Text.Trim()
            };
            return true;
        }

        private bool isPasswordNew()
        {
            errorProvider.Clear();
            if (!_txtNewPassword.Text.Trim().Equals(_txtConfirmPassword.Text.Trim()))
            {
                errorProvider.SetError(_txtConfirmPassword, "Mật khẩu không trùng khớp!");
                _txtNewPassword.Focus();
                return false;
            }
            return true;
        }

        private bool isCheckEmpty()
        {
            errorProvider.Clear();
            if (string.IsNullOrWhiteSpace(_txtUsername.Text))
            {
                errorProvider.SetError(_txtUsername, "Điền thông tin tài khoản!");
                _txtUsername.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(_txtPasswordCurrent.Text))
            {
                errorProvider.SetError(_txtPasswordCurrent, "Điền thông tin mật khẩu hiện tại!");
                _txtPasswordCurrent.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(_txtNewPassword.Text))
            {
                errorProvider.SetError(_txtNewPassword, "Điền thông tin mật khẩu mới!");
                _txtNewPassword.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(_txtConfirmPassword.Text))
            {
                errorProvider.SetError(_txtConfirmPassword, "Điền thông tin xác nhận mật khẩu mới!");
                _txtConfirmPassword.Focus();
                return false;
            }

            return true;
        }

        private async void _btnResetPasswod_Click(object sender, EventArgs e)
        {
            if (!isCheckEmpty())
            {
                return;
            }
            if (!await isPasswordCurent())
            {
                return;
            }
            if (!isPasswordNew())
            {
                return;
            }
            SetChangePassword();

        }

        private void SetChangePassword()
        {
            MessageBox.Show("Cập nhật mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void _btnLogin_Click(object sender, EventArgs e)
        {
            if (LoginClick_Action != null)
            {
                LoginClick_Action?.Invoke(this, EventArgs.Empty);
            }
        }

        public void ResetTextbox()
        {
            _txtUsername.Text = string.Empty;
            _txtNewPassword.Text = string.Empty;
            _txtPasswordCurrent.Text = string.Empty;
            _txtConfirmPassword.Text = string.Empty;
        }
    }
}
