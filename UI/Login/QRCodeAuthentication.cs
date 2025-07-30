using System;
using System.Windows.Forms;

namespace PrintToPACSDemo.UI.Login
{
    public partial class QRCodeAuthentication : DevExpress.XtraEditors.XtraForm
    {
        private const string AppName = "An Phat Digital Signature";
        public QRCodeAuthentication()
        {
            InitializeComponent();
            InitQRCode();
        }

        private void InitQRCode()
        {
           
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void buttonVertify_Click(object sender, EventArgs e)
        {
           
        }
    }
}
