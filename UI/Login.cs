using PrintToPACSDemo.AnPhat.Data;
using PrintToPACSDemo.UI.Conclusion;
using System;
using System.Windows.Forms;

namespace PrintToPACSDemo.UI
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        private ErrorProvider errorProvider = new ErrorProvider();
        private Account account;

        private void checkboxShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowPass.Checked)
            {
                textBoxPassword.PasswordChar = '\0';
            }
            else
            {
                textBoxPassword.PasswordChar = '*';
            }
        }

        private bool CheckUserAndPassword()
        {
            errorProvider.Clear();

            if (string.IsNullOrWhiteSpace(textBoxUsername.Text))
            {
                errorProvider.SetError(textBoxUsername, "Invalid User Name!");
                textBoxUsername.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBoxPassword.Text))
            {
                errorProvider.SetError(textBoxPassword, "Invalid Password!");
                textBoxPassword.Focus();
                return false;
            }
            return true;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (!CheckUserAndPassword())
            {
                return;
            }

            account = Account.UserVerify(textBoxUsername.Text.Trim(), textBoxPassword.Text.Trim());
            if (account == null)
            {
                MessageBox.Show("Login is incorrect!");
            }
            else
            {
                PacsSettings.Account = account;
                WorkListTable workListTable = new WorkListTable();
                workListTable.Show();
                this.Hide();
            }
        }
    }
}
