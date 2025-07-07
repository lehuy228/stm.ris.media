using OtpNet;
using PrintToPACSDemo.AnPhat.Data;
using PrintToPACSDemo.AnPhatData;
using QRCoder;
using System;
using System.Windows.Forms;

namespace PrintToPACSDemo.UI.Login
{
    public partial class QRCodeAuthentication : DevExpress.XtraEditors.XtraForm
    {
        private const string AppName = "An Phat Digital Signature";
        private Staff _Staff;
        private string Secret;
        public QRCodeAuthentication(Staff Staff)
        {
            InitializeComponent();
            this._Staff = Staff;
            InitQRCode();
        }

        private void InitQRCode()
        {
            if (_Staff.Secret == null || String.IsNullOrEmpty(_Staff.Secret))
            {
                Secret = Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(20));
                string totpUrl = $"otpauth://totp/{AppName}:{_Staff.Username}?secret={Secret}&issuer={AppName}";

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(totpUrl, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                var qrCodeAsBitmap = qrCode.GetGraphic(20);
                pictureBoxQRCode.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
                pictureBoxQRCode.Image = qrCodeAsBitmap;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidateOTP(string userOtp)
        {
            if (_Staff.Secret != null && _Staff.Secret.Length > 0)
            {
                Secret = _Staff.Secret;
            }
            byte[] secretKeyBytes = Base32Encoding.ToBytes(Secret);
            Totp totp = new Totp(secretKeyBytes);
            string generatedOtp = totp.ComputeTotp();
            return userOtp == generatedOtp;
        }

        private async void buttonVertify_Click(object sender, EventArgs e)
        {
            string userOtp = textBoxVertify.Text;

            if (ValidateOTP(userOtp))
            {
                if (_Staff.Secret == null || String.IsNullOrEmpty(_Staff.Secret))
                {
                    _Staff.Secret = Secret;
                    await ClientAPI.Update<Staff>(_Staff.ID, _Staff);
                }
                Program.IsAuthencation = true;
                PacsSettings.Staff = _Staff;
                this.Close();
            }
            else
            {
                Program.IsAuthencation = false;
                MessageBox.Show("Mã OTP không chính xác!");
            }
        }
    }
}
