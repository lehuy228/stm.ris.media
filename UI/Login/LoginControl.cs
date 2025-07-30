using Leadtools.Dicom.Common.DataTypes;
using PrintToPACSDemo.UI.Conclusion;
using System;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintToPACSDemo.UI.Login
{
    public partial class LoginControl : DevExpress.XtraEditors.XtraUserControl
    {
        private ErrorProvider errorProvider = new ErrorProvider();
        public event EventHandler ChangePasswordClick_Action;

        public LoginControl()
        {
            InitializeComponent();
        }

        private void checkboxShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (!_cbShowPass.Checked)
            {
                _txtPassword.Properties.PasswordChar = '\0';
            }
            else
            {
                _txtPassword.Properties.PasswordChar = '*';
            }
        }

        private bool CheckUserAndPassword()
        {
            errorProvider.Clear();

            if (string.IsNullOrWhiteSpace(_txtUsername.Text))
            {
                errorProvider.SetError(_txtUsername, "Điền thông tin tài khoản!");
                _txtUsername.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(_txtPassword.Text))
            {
                errorProvider.SetError(_txtPassword, "Điền thông tin mật khẩu!");
                _txtPassword.Focus();
                return false;
            }
            return true;
        }

        private async Task LoginAsync()
        {
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            LoginAsync();
        }

        private void QRCodeAuthentication_FormClosed(object sender, FormClosedEventArgs e)
        {
            WorkListTable workListTable = new WorkListTable();
            workListTable.FormClosed += WorkListTable_FormClosed;
            workListTable.Show();
            if (Program.IsAuthencation)
            {
                this.ParentForm.Hide();
            }
            else
            {
                this.ParentForm.Show();
            }
        }

        private void WorkListTable_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.IsAuthencation = false;
            this.ParentForm.Show();
        }

        public void ResetForm()
        {
            _txtUsername.Text = null;
            _txtPassword.Text = null;
        }

        private void _lbChangePassword_Click(object sender, EventArgs e)
        {
            if (ChangePasswordClick_Action != null)
            {
                ChangePasswordClick_Action?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
