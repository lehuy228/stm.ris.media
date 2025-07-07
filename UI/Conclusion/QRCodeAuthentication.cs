using OtpNet;
using PrintToPACSDemo.AnPhat.Data;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintToPACSDemo.UI.Conclusion
{
    public partial class QRCodeAuthentication : Form
    {
        public QRCodeAuthentication()
        {
            InitializeComponent();
        }
        private const string AppName = "An Phat Digital Signature";
        private Account _Account;
        private string Secret;
        private string userName;
        public QRCodeAuthentication(string userName)
        {
            InitializeComponent();
            this.userName = userName;
            _Account = Account.GetAccount(userName);
            InitQRCode();
        }

        private void InitQRCode()
        {
            if (_Account.Secret == null || String.IsNullOrEmpty(_Account.Secret))
            {
                Secret = Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(20));
                string totpUrl = $"otpauth://totp/{AppName}:{_Account.Email}?secret={Secret}&issuer={AppName}";

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(totpUrl, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                var qrCodeAsBitmap = qrCode.GetGraphic(20);
                pictureBoxQRCode.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBoxQRCode.Image = qrCodeAsBitmap;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidateOTP(string userOtp)
        {
            if (_Account.Secret != null && _Account.Secret.Length > 0)
            {
                Secret = _Account.Secret;
            }
            byte[] secretKeyBytes = Base32Encoding.ToBytes(Secret);
            Totp totp = new Totp(secretKeyBytes);
            string generatedOtp = totp.ComputeTotp();
            return userOtp == generatedOtp;
        }

        private void buttonVertify_Click(object sender, EventArgs e)
        {
            string userOtp = textBoxVertify.Text;

            if (ValidateOTP(userOtp))
            {
                if (_Account.Secret == null || String.IsNullOrEmpty(_Account.Secret))
                {
                    _Account.Secret = Secret;
                    Account.UpdateAccountSecret(_Account);
                }
                Program.IsAuthencation = true;
                this.Close();
            }
            else
            {
                Program.IsAuthencation = false;
                MessageBox.Show("OTP code is incorrect!");
            }
        }
    }
}
