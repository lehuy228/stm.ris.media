using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintToPACSDemo.UI.Login
{
    public partial class AuthenticationForm : DevExpress.XtraEditors.XtraForm
    {
        private LoginControl login;
        private ResetPassword resetPassword;
        public AuthenticationForm()
        {
            InitializeComponent();
            InitUI();
        }

        private void InitUI()
        {
            InitLogin();
            InitResetPassword();
        }


        private void InitResetPassword()
        {
            resetPassword = new ResetPassword();
            resetPassword.Dock = DockStyle.Fill;
            resetPassword.Visible = false;
            resetPassword.LoginClick_Action += Login_Click;
            this.Controls.Add(resetPassword);

        }

        private void Login_Click(object sender, EventArgs e)
        {
            resetPassword.Visible = false;
            login.Visible = true;
            this.Height = 180;
            resetPassword.ResetTextbox();
        }

        private void InitLogin()
        {
            login = new LoginControl();
            login.Dock = DockStyle.Fill;
            login.Visible = true;
            login.ChangePasswordClick_Action += ChangePassword_Click;
            this.Controls.Add(login);
        }

        private void ChangePassword_Click(object sender, EventArgs e)
        {
            resetPassword.Visible = true;
            login.Visible = false;
            this.Height = 230;
            resetPassword.ResetTextbox();
        }
    }
}