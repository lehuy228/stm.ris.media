using FlashCap;
using MediaToPacs.Core.Models;
using STM.MediaToPACS.Main.UI.CameraUI;
using STM.MediaToPACS.Main.Utilities;
using Serilog;
using System;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
//using VisioForge.Core.Types;                              // VisioForge đã thay bằng FlashCap
//using VisioForge.Core.Types.Events;
//using VisioForge.Core.Types.VideoCapture;
//using VisioForge.Core.Types.VideoEffects;
//using VisioForge.Core.UI.WinForms.Dialogs.OutputFormats;
//using VisioForge.Core.VideoCapture;

namespace STM.MediaToPACS.Main.UI.Configurations
{
    public partial class ConfigCamera : UserControl
    {
        //public VideoCaptureCore VideoCapture1 { get; private set; }          // VisioForge đã thay bằng FlashCap
        //private VisioForge.Core.UI.WinForms.VideoView videoView1;
        private CaptureDevice _captureDevice;
        private volatile bool _rendering;
        private PictureBox videoView1;

        //
        public string VideoInputDevice;
        public string VideoInputFormat;
        public string VideoInputFrameRate;
        public string AudioInputDevice;
        public string AudioInputFormat;
        public string AudioInputLine;
        public string OutputFormat;
        public int IndexOutputFormat = 22;

        public bool IsCheckGreyscale = false;
        public bool IsCheckInvert = false;
        public bool IsCheckFlipX = false;
        public bool IsCheckFlipY = false;
        public bool IsCheckZoom = false;
        public bool IsCheckPan = false;
        public bool IsCheckRotation = false;
        public double Zoom = 1.0;
        public int ZoomShiftX;
        public int ZoomShiftY;
        private int _liveRotationAngle;

        public ConfigCamera()
        {
            InitializeComponent();
        }

        private void ConfigCamera_Load(object sender, EventArgs e)
        {
            InitVideoView();
            InitVideoCamera();
        }

        private void InitVideoView()
        {
            //this.videoView1 = new VisioForge.Core.UI.WinForms.VideoView();   // VisioForge đã thay bằng FlashCap
            this.videoView1 = new PictureBox();
            //
            // videoView1
            //
            this.videoView1.BackColor = System.Drawing.Color.Black;
            this.videoView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoView1.Location = new System.Drawing.Point(0, 40);
            this.videoView1.Margin = new System.Windows.Forms.Padding(4);
            this.videoView1.Name = "videoView1";
            this.videoView1.Size = new System.Drawing.Size(912, 797);
            this.videoView1.SizeMode = PictureBoxSizeMode.StretchImage;
            //this.videoView1.StatusOverlay = null;                            // thuộc tính của VisioForge VideoView
            this.videoView1.TabIndex = 9;
            _panelCamera.Controls.Add(videoView1);
        }

        private void InitVideoCamera()
        {
            if (ServiceLocator.CameraSettingConfig == null)
            {
                ServiceLocator.CameraSettingConfig = new CameraSettings();
            }

            IsCheckInvert = ServiceLocator.CameraSettingConfig.Invert;
            IsCheckGreyscale = ServiceLocator.CameraSettingConfig.Greyscale;
            IsCheckFlipX = ServiceLocator.CameraSettingConfig.FlipX;
            IsCheckFlipY = ServiceLocator.CameraSettingConfig.FlipY;
            IsCheckZoom = ServiceLocator.CameraSettingConfig.EnableZoom;
            Zoom = ServiceLocator.CameraSettingConfig.Zoom;
            ZoomShiftX = ServiceLocator.CameraSettingConfig.ZoomShiftX;
            ZoomShiftY = ServiceLocator.CameraSettingConfig.ZoomShiftY;

            cbOutputFormat.SelectedIndex = 2;

            foreach (var device in CameraControl.GetVideoDevices())
            {
                cbVideoInputDevice.Properties.Items.Add(device.Name);
            }

            if (cbVideoInputDevice.Properties.Items.Count > 0)
            {
                string selectedDevice = ServiceLocator.CameraSettingConfig.VideoInputDevice?.ToString();

                if (!string.IsNullOrEmpty(selectedDevice))
                {
                    int index = -1;
                    for (int i = 0; i < cbVideoInputDevice.Properties.Items.Count; i++)
                    {
                        if (cbVideoInputDevice.Properties.Items[i].ToString() == selectedDevice)
                        {
                            index = i;
                            break;
                        }
                    }
                    cbVideoInputDevice.SelectedIndex = (index >= 0) ? index : 0;
                }
                else
                {
                    cbVideoInputDevice.SelectedIndex = 0;
                }
            }

            cbVideoInputDevice_SelectedIndexChanged(null, null);

            // Audio không còn dùng (đã bỏ chức năng quay video) - giữ nguyên giá trị đã lưu nếu có
            if (!string.IsNullOrEmpty(ServiceLocator.CameraSettingConfig.AudioInputDevice?.ToString()))
            {
                cbAudioInputDevice.Text = ServiceLocator.CameraSettingConfig.AudioInputDevice?.ToString();
            }
            if (!string.IsNullOrEmpty(ServiceLocator.CameraSettingConfig.AudioInputFormat?.ToString()))
            {
                cbAudioInputFormat.Text = ServiceLocator.CameraSettingConfig.AudioInputFormat?.ToString();
            }
            if (!string.IsNullOrEmpty(ServiceLocator.CameraSettingConfig.AudioInputLine?.ToString()))
            {
                cbAudioInputLine.Text = ServiceLocator.CameraSettingConfig.AudioInputLine?.ToString();
            }

            if (!string.IsNullOrEmpty(ServiceLocator.CameraSettingConfig.InphutFormat?.ToString()))
            {
                cbVideoInputFormat.Text = ServiceLocator.CameraSettingConfig.InphutFormat?.ToString();
            }

            if (!string.IsNullOrEmpty(ServiceLocator.CameraSettingConfig.FrameRate?.ToString()))
            {
                cbVideoInputFrameRate.Text = ServiceLocator.CameraSettingConfig.FrameRate?.ToString();
            }

            if (!string.IsNullOrEmpty(ServiceLocator.CameraSettingConfig.OutputFormat?.ToString()))
            {
                cbOutputFormat.Text = ServiceLocator.CameraSettingConfig.OutputFormat?.ToString();
            }

            cbGreyscale.Checked = ServiceLocator.CameraSettingConfig.Greyscale;
            cbInvert.Checked = ServiceLocator.CameraSettingConfig.Invert;
            cbFlipX.Checked = ServiceLocator.CameraSettingConfig.FlipX;
            cbFlipY.Checked = ServiceLocator.CameraSettingConfig.FlipY;

            cbPan.Checked = ServiceLocator.CameraSettingConfig.EnablePan;
            edPanStartTime.Text = ServiceLocator.CameraSettingConfig.PanStartTime.ToString();
            edPanStopTime.Text = ServiceLocator.CameraSettingConfig.PanStopTime.ToString();
            edPanSourceLeft.Text = ServiceLocator.CameraSettingConfig.PanSourceLeft.ToString();
            edPanSourceWidth.Text = ServiceLocator.CameraSettingConfig.PanSourceWidth.ToString();
            edPanSourceHeight.Text = ServiceLocator.CameraSettingConfig.PanSourceHeight.ToString();
            edPanSourceTop.Text = ServiceLocator.CameraSettingConfig.PanSourceTop.ToString();
            edPanDestLeft.Text = ServiceLocator.CameraSettingConfig.PanDestLeft.ToString();
            edPanDestWidth.Text = ServiceLocator.CameraSettingConfig.PanDestWidth.ToString();
            edPanDestHeight.Text = ServiceLocator.CameraSettingConfig.PanDestHeight.ToString();
            edPanDestTop.Text = ServiceLocator.CameraSettingConfig.PanDestTop.ToString();

            cbLiveRotation.Checked = ServiceLocator.CameraSettingConfig.EnableLiveRotation;
            tbLiveRotationAngle.Value = ServiceLocator.CameraSettingConfig.LiveRotationAngle;
            _liveRotationAngle = ServiceLocator.CameraSettingConfig.LiveRotationAngle;
            IsCheckRotation = ServiceLocator.CameraSettingConfig.EnableLiveRotation;

            cbZoom.Checked = IsCheckZoom;
            cbInvert.Checked = IsCheckInvert;
            cbGreyscale.Checked = IsCheckGreyscale;
            cbFlipX.Checked = IsCheckFlipX;
            cbFlipY.Checked = IsCheckFlipY;
        }

        private void cbVideoInputDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbVideoInputDevice.SelectedIndex != -1)
            {
                cbVideoInputFormat.Properties.Items.Clear();
                var deviceItem = CameraControl.FindDescriptor(cbVideoInputDevice.Text);
                if (deviceItem == null)
                {
                    return;
                }

                foreach (var formatName in deviceItem.Characteristics
                    .Where(c => c.PixelFormat != PixelFormats.Unknown)
                    .Select(CameraControl.GetFormatName)
                    .Distinct())
                {
                    cbVideoInputFormat.Properties.Items.Add(formatName);
                }

                if (cbVideoInputFormat.Properties.Items.Count > 0)
                {
                    cbVideoInputFormat.SelectedIndex = 0;
                    cbVideoInputFormat_SelectedIndexChanged(null, null);
                }
            }
        }

        private void cbVideoInputFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbVideoInputFormat.Text))
            {
                return;
            }

            if (cbVideoInputDevice.SelectedIndex != -1)
            {
                var deviceItem = CameraControl.FindDescriptor(cbVideoInputDevice.Text);
                if (deviceItem == null)
                {
                    return;
                }

                cbVideoInputFrameRate.Properties.Items.Clear();
                foreach (var frameRate in deviceItem.Characteristics
                    .Where(c => CameraControl.GetFormatName(c) == cbVideoInputFormat.Text)
                    .Select(c => (double)c.FramesPerSecond)
                    .Distinct()
                    .OrderByDescending(fps => fps))
                {
                    cbVideoInputFrameRate.Properties.Items.Add(frameRate.ToString(CultureInfo.CurrentCulture));
                }

                if (cbVideoInputFrameRate.Properties.Items.Count > 0)
                {
                    cbVideoInputFrameRate.SelectedIndex = 0;
                }
            }
        }

        private void cbAudioInputDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Audio không còn dùng (đã bỏ chức năng quay video)
        }

        private void cbOutputFormat_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void _btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                await StopPreviewAsync();

                var descriptor = CameraControl.FindDescriptor(cbVideoInputDevice.Text);
                if (descriptor == null)
                {
                    MessageBox.Show(this, "Không tìm thấy thiết bị camera.");
                    return;
                }

                var characteristics = CameraControl.FindCharacteristics(descriptor, cbVideoInputFormat.Text, cbVideoInputFrameRate.Text);
                if (characteristics == null)
                {
                    MessageBox.Show(this, "Thiết bị không có format video hợp lệ.");
                    return;
                }

                _captureDevice = await descriptor.OpenAsync(characteristics, TranscodeFormats.Auto, OnFrameArrived);
                await _captureDevice.StartAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi xem trước camera trong cấu hình");
                MessageBox.Show(this, $"Lỗi khi xem trước camera: {ex.Message}");
            }
        }

        /// <summary>
        /// Cấu hình hiệu ứng tạm theo trạng thái UI hiện tại (chưa lưu).
        /// </summary>
        private CameraSettings BuildPreviewSettings()
        {
            return new CameraSettings
            {
                Greyscale = IsCheckGreyscale,
                Invert = IsCheckInvert,
                FlipX = IsCheckFlipX,
                FlipY = IsCheckFlipY,
                EnableZoom = IsCheckZoom,
                Zoom = Zoom,
                ZoomShiftX = ZoomShiftX,
                ZoomShiftY = ZoomShiftY,
                EnableLiveRotation = IsCheckRotation,
                LiveRotationAngle = _liveRotationAngle,
            };
        }

        private void OnFrameArrived(PixelBufferScope bufferScope)
        {
            if (_rendering)
            {
                return;
            }
            _rendering = true;

            try
            {
                byte[] imageData = bufferScope.Buffer.CopyImage();

                Bitmap frame;
                using (var ms = new MemoryStream(imageData))
                using (var decoded = new Bitmap(ms))
                {
                    frame = CameraControl.ApplyEffects(decoded, BuildPreviewSettings());
                }

                if (IsHandleCreated && !IsDisposed)
                {
                    BeginInvoke((Action)(() =>
                    {
                        try
                        {
                            var old = videoView1.Image;
                            videoView1.Image = frame;
                            old?.Dispose();
                        }
                        finally
                        {
                            _rendering = false;
                        }
                    }));
                }
                else
                {
                    frame.Dispose();
                    _rendering = false;
                }
            }
            catch (Exception ex)
            {
                _rendering = false;
                Log.Warning(ex, "Lỗi xử lý khung hình camera (cấu hình)");
            }
        }

        private async Task StopPreviewAsync()
        {
            if (_captureDevice != null)
            {
                try
                {
                    await _captureDevice.StopAsync();
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Lỗi khi dừng camera (cấu hình)");
                }
                _captureDevice.Dispose();
                _captureDevice = null;
            }
        }

        private void cbPan_CheckedChanged(object sender, EventArgs e)
        {
            // Pan chưa hỗ trợ (trước đây cũng đang comment ở bản VisioForge)
        }

        private void cbGreyscale_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckGreyscale = cbGreyscale.Checked;
        }

        private void cbInvert_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckInvert = cbInvert.Checked;
        }

        private void cbFlipX_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckFlipX = cbFlipX.Checked;
        }

        private void cbFlipY_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckFlipY = cbFlipY.Checked;
        }

        private void cbZoom_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckZoom = cbZoom.Checked;
        }

        private void btEffZoomIn_Click(object sender, EventArgs e)
        {
            Zoom += 0.1;
            Zoom = Math.Min(Zoom, 5);

            cbZoom_CheckedChanged(null, null);
        }

        private void btEffZoomOut_Click(object sender, EventArgs e)
        {
            Zoom -= 0.1;
            Zoom = Math.Max(Zoom, 1);

            cbZoom_CheckedChanged(null, null);
        }

        private void btEffZoomUp_Click(object sender, EventArgs e)
        {
            ZoomShiftY += 20;

            cbZoom_CheckedChanged(null, null);
        }

        private void btEffZoomDown_Click(object sender, EventArgs e)
        {
            ZoomShiftY -= 20;

            cbZoom_CheckedChanged(null, null);
        }

        private void btEffZoomRight_Click(object sender, EventArgs e)
        {
            ZoomShiftX += 20;

            cbZoom_CheckedChanged(null, null);
        }

        private void btEffZoomLeft_Click(object sender, EventArgs e)
        {
            ZoomShiftX -= 20;

            cbZoom_CheckedChanged(null, null);
        }

        private void cbLiveRotation_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckRotation = cbLiveRotation.Checked;
            _liveRotationAngle = tbLiveRotationAngle.Value;
        }

        private void tbLiveRotationAngle_Scroll(object sender, EventArgs e)
        {
            _liveRotationAngle = tbLiveRotationAngle.Value;
        }

        public async void _btnStopCamera_Click(object sender, EventArgs e)
        {
            try
            {
                await StopPreviewAsync();
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Lỗi khi dừng camera (cấu hình)");
            }
        }

        public void SaveSettingsCamera()
        {
            var VideoInputDevice = cbVideoInputDevice.Text;
            var VideoInputFormat = cbVideoInputFormat.Text;
            var VideoInputFrameRate = cbVideoInputFrameRate.Text;
            var OutputFormat = cbOutputFormat.Text;
            var AudioInputDevice = cbAudioInputDevice.Text;
            var AudioInputFormat = cbAudioInputFormat.Text;
            var AudioInputLine = cbAudioInputLine.Text;

            var Greyscale = cbGreyscale.Checked;
            var Invert = cbInvert.Checked;
            var FlipX = cbFlipX.Checked;
            var FlipY = cbFlipY.Checked;

            var EnableZoom = cbZoom.Checked;

            CameraSettings cameraSettings = new CameraSettings
            {
                VideoInputDevice = VideoInputDevice,
                InphutFormat = VideoInputFormat,
                FrameRate = VideoInputFrameRate,
                OutputFormat = OutputFormat,
                AudioInputDevice = AudioInputDevice,
                AudioInputFormat = AudioInputFormat,
                AudioInputLine = AudioInputLine,

                Greyscale = Greyscale,
                Invert = Invert,
                FlipX = FlipX,
                FlipY = FlipY,

                EnableZoom = EnableZoom,
                Zoom = Zoom,
                ZoomShiftX = ZoomShiftX,
                ZoomShiftY = ZoomShiftX,

                EnableLiveRotation = cbLiveRotation.Checked,
                LiveRotationAngle = tbLiveRotationAngle.Value,
            };
            ServiceLocator.CameraSettingConfig = cameraSettings;
            XmlSettingsHelper.Save<CameraSettings>(Path.Combine(
                ServiceLocator.GetAppDataBasePath(),
                ConfigurationManager.AppSettings["File:CameraConfig"]), cameraSettings);
        }

        #region Code VisioForge cũ (đã thay bằng FlashCap)
        /*
        private async Task CreateEngineAsync()
        {
            VideoCapture1 = await VideoCaptureCore.CreateAsync(videoView1 as IVideoView);
            VideoCapture1.SetLicenseKey("...", "...", "...");
            VideoCapture1.OnError += VideoCapture1_OnError;
        }

        private void VideoCapture1_OnError(object sender, ErrorsEventArgs e)
        {
        }

        // Liệt kê format/framerate qua VideoCapture1.Video_CaptureDevices(),
        // audio qua VideoCapture1.Audio_CaptureDevices() - xem lịch sử git để lấy bản đầy đủ.

        private void SettingCaptureDevice()
        {
            VideoCapture1.Video_Effects_Enabled = true;
            VideoCapture1.Video_CaptureDevice = new VideoCaptureSource(cbVideoInputDevice.Text)
            {
                Format_UseBest = false,
                Format = cbVideoInputFormat.Text,
                FrameRate = new VideoFrameRate(Convert.ToDouble(cbVideoInputFrameRate.Text, CultureInfo.CurrentCulture))
            };

            VideoCapture1.Audio_RecordAudio = false;
            VideoCapture1.Audio_PlayAudio = false;

            VideoCapture1.Video_Sample_Grabber_Enabled = true;
            VideoCapture1.Video_Renderer.Zoom_Ratio = 0;
            VideoCapture1.Video_Renderer.Zoom_ShiftX = 0;
            VideoCapture1.Video_Renderer.Zoom_ShiftY = 0;
            VideoCapture1.Video_Renderer.VideoRenderer = VideoRendererMode.Direct2D;
        }

        // Các handler hiệu ứng cũ (VideoEffectGrayscale/Invert/Flip/Zoom/Rotate)
        // nay chỉ cần cập nhật cờ IsCheck*, hiệu ứng áp trong CameraControl.ApplyEffects().
        */
        #endregion
    }
}
