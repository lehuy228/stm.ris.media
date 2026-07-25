using System;
using System.Linq;
using System.Windows.Forms;
using Serilog;
using STM.MediaToPACS.Main.UI.CameraUI;
using STM.MediaToPACS.Main.Utilities;

namespace STM.MediaToPACS.Main.UI.V2
{
    /// <summary>
    /// Host độc lập cho FormMainV2. Luồng MainForm sử dụng tab trực tiếp
    /// và không tạo tab lồng bên trong host này.
    /// </summary>
    public partial class FrmMainV2Host : Form
    {
        private readonly FormMainV2 _formMainV2;
        private bool _isCleanupDone;

        public FrmMainV2Host(string videoInputDevice, string soPhieu, string maChiDinh)
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(videoInputDevice))
                videoInputDevice = ServiceLocator.CameraSettingConfig?.VideoInputDevice
                    ?? CameraControl.GetVideoDevices().FirstOrDefault()?.Name;

            _formMainV2 = new FormMainV2(videoInputDevice, soPhieu, maChiDinh)
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(_formMainV2);
        }

        private async void FrmMainV2Host_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isCleanupDone || e.CloseReason == CloseReason.WindowsShutDown)
                return;

            e.Cancel = true;
            try
            {
                await _formMainV2.StopCameraAsync();
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Lỗi khi dừng camera lúc đóng FrmMainV2Host");
            }
            finally
            {
                _isCleanupDone = true;
                Close();
            }
        }
    }
}
