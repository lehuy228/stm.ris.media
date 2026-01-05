using MediaToPacs.Core.Models;
using STM.MediaToPACS.Main.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Core.Types;
using VisioForge.Core.Types.Events;
using VisioForge.Core.Types.VideoCapture;
using VisioForge.Core.Types.VideoEffects;
using VisioForge.Core.UI.WinForms.Dialogs.OutputFormats;
using VisioForge.Core.VideoCapture;

namespace STM.MediaToPACS.Main.UI.Configurations
{
    public partial class ConfigCamera : UserControl
    {
        public VideoCaptureCore VideoCapture1 { get; private set; }
        private VisioForge.Core.UI.WinForms.VideoView videoView1;

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

        public List<string> LinkVideos { get; private set; }
        public string LinkImageSnapshot { get; private set; }

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
            this.videoView1 = new VisioForge.Core.UI.WinForms.VideoView();
            // 
            // videoView1
            // 
            this.videoView1.BackColor = System.Drawing.Color.Black;
            this.videoView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoView1.Location = new System.Drawing.Point(0, 40);
            this.videoView1.Margin = new System.Windows.Forms.Padding(4);
            this.videoView1.Name = "videoView1";
            this.videoView1.Size = new System.Drawing.Size(912, 797);
            this.videoView1.StatusOverlay = null;
            this.videoView1.TabIndex = 9;
            _panelCamera.Controls.Add(videoView1);
        }

        private async void InitVideoCamera()
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

            await CreateEngineAsync();

            cbOutputFormat.SelectedIndex = 2;

            foreach (var device in VideoCapture1.Video_CaptureDevices())
            {
                cbVideoInputDevice.Properties.Items.Add(device.Name);
            }

            if (cbVideoInputDevice.Properties.Items.Count > 0)
            {
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
            }

            cbVideoInputDevice_SelectedIndexChanged(null, null);

            foreach (var device in VideoCapture1.Audio_CaptureDevices())
            {
                cbAudioInputDevice.Properties.Items.Add(device.Name);
            }

            if (cbAudioInputDevice.Properties.Items.Count > 0)
            {
                string selectedAudioDevice = ServiceLocator.CameraSettingConfig.AudioInputDevice?.ToString();

                if (!string.IsNullOrEmpty(selectedAudioDevice))
                {
                    int index = -1;
                    for (int i = 0; i < cbAudioInputDevice.Properties.Items.Count; i++)
                    {
                        if (cbAudioInputDevice.Properties.Items[i].ToString() == selectedAudioDevice)
                        {
                            index = i;
                            break;
                        }
                    }
                    cbAudioInputDevice.SelectedIndex = (index >= 0) ? index : 0;
                }
                else
                {
                    cbAudioInputDevice.SelectedIndex = 0;
                }
            }

            cbAudioInputLine.Properties.Items.Clear();

            if (!string.IsNullOrEmpty(cbAudioInputDevice.Text))
            {
                var deviceItem =
                    VideoCapture1.Audio_CaptureDevices().FirstOrDefault(device => device.Name == cbAudioInputDevice.Text);
                if (deviceItem != null)
                {
                    foreach (string line in deviceItem.Lines)
                    {
                        cbAudioInputLine.Properties.Items.Add(line);
                    }

                    if (cbAudioInputLine.Properties.Items.Count > 0)
                    {
                        cbAudioInputLine.SelectedIndex = 0;
                    }
                    if (cbAudioInputLine.Properties.Items.Count > 0)
                    {
                        string selectedAudioInputLine = ServiceLocator.CameraSettingConfig.AudioInputLine?.ToString();

                        if (!string.IsNullOrEmpty(selectedAudioInputLine))
                        {
                            int index = -1;
                            for (int i = 0; i < cbAudioInputLine.Properties.Items.Count; i++)
                            {
                                if (cbAudioInputLine.Properties.Items[i].ToString() == selectedAudioInputLine)
                                {
                                    index = i;
                                    break;
                                }
                            }
                            cbAudioInputLine.SelectedIndex = (index >= 0) ? index : 0;
                        }
                        else
                        {
                            cbAudioInputLine.SelectedIndex = 0;
                        }
                    }
                }
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

            VideoCapture1.Video_Renderer_SetAuto();
            cbZoom.Checked = IsCheckZoom;
            cbInvert.Checked = IsCheckInvert;
            cbGreyscale.Checked = IsCheckGreyscale;
            cbFlipX.Checked = IsCheckFlipX;
            cbFlipY.Checked = IsCheckFlipY;
        }

        private async Task CreateEngineAsync()
        {
            VideoCapture1 = await VideoCaptureCore.CreateAsync(videoView1 as IVideoView);
            VideoCapture1.SetLicenseKey("1E17-F8AA-BB54-D7A1-BD5F-446D", "STM TECHNOLOGY AND COMMERCIAL JOINT STOCK COMPANY", "linh@anphats.com");
            VideoCapture1.OnError += VideoCapture1_OnError;
        }

        private void VideoCapture1_OnError(object sender, ErrorsEventArgs e)
        {
        }

        private void cbVideoInputDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbVideoInputDevice.SelectedIndex != -1)
            {
                cbVideoInputFormat.Properties.Items.Clear();
                var deviceItem = VideoCapture1.Video_CaptureDevices().FirstOrDefault(device => device.Name == cbVideoInputDevice.Text);
                if (deviceItem == null)
                {
                    return;
                }

                foreach (var format in deviceItem.VideoFormats)
                {
                    cbVideoInputFormat.Properties.Items.Add(format.Name);
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
                var deviceItem = VideoCapture1.Video_CaptureDevices().FirstOrDefault(device => device.Name == cbVideoInputDevice.Text);
                if (deviceItem == null)
                {
                    return;
                }

                var videoFormat = deviceItem.VideoFormats.Find(format => format.Name == cbVideoInputFormat.Text);
                if (videoFormat == null)
                {
                    return;
                }

                cbVideoInputFrameRate.Properties.Items.Clear();
                foreach (var frameRate in videoFormat.FrameRates)
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
            if (cbAudioInputDevice.SelectedIndex != -1)
            {
                cbAudioInputFormat.Properties.Items.Clear();

                var deviceItem = VideoCapture1.Audio_CaptureDevices().FirstOrDefault(device => device.Name == cbAudioInputDevice.Text);
                if (deviceItem == null)
                {
                    return;
                }

                var defaultValue = "PCM, 44100 Hz, 16 Bits, 2 Channels";
                var defaultValueExists = false;
                foreach (string format in deviceItem.Formats)
                {
                    cbAudioInputFormat.Properties.Items.Add(format);

                    if (defaultValue == format)
                    {
                        defaultValueExists = true;
                    }
                }

                if (cbAudioInputFormat.Properties.Items.Count > 0)
                {
                    cbAudioInputFormat.SelectedIndex = 0;

                    if (defaultValueExists)
                    {
                        cbAudioInputFormat.Text = defaultValue;
                    }
                }

                cbAudioInputLine.Properties.Items.Clear();

                foreach (string line in deviceItem.Lines)
                {
                    cbAudioInputLine.Properties.Items.Add(line);
                }

                if (cbAudioInputLine.Properties.Items.Count > 0)
                {
                    cbAudioInputLine.SelectedIndex = 0;
                }
            }
        }

        private void cbOutputFormat_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        private async void _btnPreview_Click(object sender, EventArgs e)
        {
            VideoCapture1.Video_Filters_Clear();
            await VideoCapture1.StopAsync();
            SettingCaptureDevice();
            VideoCapture1.Mode = VideoCaptureMode.VideoPreview;
            await VideoCapture1.StartAsync();
        }


        private void cbPan_CheckedChanged(object sender, EventArgs e)
        {
            //IVideoEffectPan pan;
            //var effect = VideoCapture1.Video_Effects_Get("Pan");
            //if (effect == null)
            //{
            //    pan = new VideoEffectPan(true);
            //    VideoCapture1.Video_Effects_Add(pan);
            //}
            //else
            //{
            //    pan = effect as IVideoEffectPan;
            //}

            //if (pan == null)
            //{
            //    MessageBox.Show(this, "Unable to configure pan effect.");
            //    return;
            //}

            //pan.Enabled = cbPan.Checked;
            //pan.StartTime = TimeSpan.FromMilliseconds(Convert.ToInt32(edPanStartTime.Text));
            //pan.StopTime = TimeSpan.FromMilliseconds(Convert.ToInt32(edPanStopTime.Text));
            //pan.StartX = Convert.ToInt32(edPanSourceLeft.Text);
            //pan.StartY = Convert.ToInt32(edPanSourceTop.Text);
            //pan.StartWidth = Convert.ToInt32(edPanSourceWidth.Text);
            //pan.StartHeight = Convert.ToInt32(edPanSourceHeight.Text);
            //pan.StopX = Convert.ToInt32(edPanDestLeft.Text);
            //pan.StopY = Convert.ToInt32(edPanDestTop.Text);
            //pan.StopWidth = Convert.ToInt32(edPanDestWidth.Text);
            //pan.StopHeight = Convert.ToInt32(edPanDestHeight.Text);
        }

        private void cbGreyscale_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckGreyscale = cbGreyscale.Checked;
            IVideoEffectGrayscale grayscale;
            var effect = VideoCapture1.Video_Effects_Get("Grayscale");
            if (effect == null)
            {
                grayscale = new VideoEffectGrayscale(cbGreyscale.Checked);
                VideoCapture1.Video_Effects_Add(grayscale);
            }
            else
            {
                grayscale = effect as IVideoEffectGrayscale;
                if (grayscale != null)
                {
                    grayscale.Enabled = cbGreyscale.Checked;
                }
            }
        }

        private async void cbInvert_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckInvert = cbInvert.Checked;
            IVideoEffectInvert invert;
            var effect = VideoCapture1.Video_Effects_Get("Invert");
            if (effect == null)
            {
                invert = new VideoEffectInvert(cbInvert.Checked);
                VideoCapture1.Video_Effects_Add(invert);
            }
            else
            {
                invert = effect as IVideoEffectInvert;
                if (invert != null)
                {
                    invert.Enabled = cbInvert.Checked;
                }
            }


        }

        private void cbFlipX_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckFlipX = cbFlipX.Checked;
            IVideoEffectFlipDown flip;
            var effect = VideoCapture1.Video_Effects_Get("FlipDown");
            if (effect == null)
            {
                flip = new VideoEffectFlipHorizontal(cbFlipX.Checked);
                VideoCapture1.Video_Effects_Add(flip);
            }
            else
            {
                flip = effect as IVideoEffectFlipDown;
                if (flip != null)
                {
                    flip.Enabled = cbFlipX.Checked;
                }
            }
        }

        private void cbFlipY_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckFlipY = cbFlipY.Checked;
            IVideoEffectFlipRight flip;
            var effect = VideoCapture1.Video_Effects_Get("FlipRight");
            if (effect == null)
            {
                flip = new VideoEffectFlipVertical(cbFlipY.Checked);
                VideoCapture1.Video_Effects_Add(flip);
            }
            else
            {
                flip = effect as IVideoEffectFlipRight;
                if (flip != null)
                {
                    flip.Enabled = cbFlipY.Checked;
                }
            }
        }
       

        private async void cbZoom_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckZoom = cbZoom.Checked;
            IVideoEffectZoom zoomEffect;
            var effect = VideoCapture1.Video_Effects_Get("Zoom");
            if (effect == null)
            {
                zoomEffect = new VideoEffectZoom(Zoom, Zoom, ZoomShiftX, ZoomShiftY, IsCheckZoom);
                VideoCapture1.Video_Effects_Add(zoomEffect);
            }
            else
            {
                zoomEffect = effect as IVideoEffectZoom;
            }

            if (zoomEffect == null)
            {
                MessageBox.Show(this, "Unable to configure zoom effect.");
                return;
            }

            zoomEffect.ZoomX = Zoom;
            zoomEffect.ZoomY = Zoom;
            zoomEffect.ShiftX = ZoomShiftX;
            zoomEffect.ShiftY = ZoomShiftY;
            zoomEffect.Enabled = IsCheckZoom;
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
            IVideoEffectRotate rotate;
            var effect = VideoCapture1.Video_Effects_Get("Rotate");
            if (effect == null)
            {
                rotate = new VideoEffectRotate(
                    IsCheckRotation,
                    tbLiveRotationAngle.Value, false);
                VideoCapture1.Video_Effects_Add(rotate);
            }
            else
            {
                rotate = effect as IVideoEffectRotate;
            }

            if (rotate == null)
            {
                MessageBox.Show(this, "Unable to configure rotate effect.");
                return;
            }

            rotate.Enabled = IsCheckRotation;
            rotate.Angle = tbLiveRotationAngle.Value;
        }

        private void tbLiveRotationAngle_Scroll(object sender, EventArgs e)
        {
            //cbLiveRotation_CheckedChanged(sender, e);
            //labelLiveRotationAngle.Text = tbLiveRotationAngle.Value.ToString();
        }

        public async void _btnStopCamera_Click(object sender, EventArgs e)
        {
            try
            {
                await VideoCapture1.StopAsync();
            }
            catch (Exception ex)
            {
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

            //var EnablePan = cbPan.Checked;
            //int PanStartTime = int.Parse(edPanStartTime.Text);
            //int PanStopTime = int.Parse(edPanStopTime.Text);
            //int PanSourceLeft = int.Parse(edPanSourceLeft.Text);
            //int PanSourceWidth = int.Parse(edPanSourceWidth.Text);
            //int PanSourceHeight = int.Parse(edPanSourceHeight.Text);
            //int PanSourceTop = int.Parse(edPanSourceTop.Text);
            //int PanDestLeft = int.Parse(edPanDestLeft.Text);
            //int PanDestWidth = int.Parse(edPanDestWidth.Text);
            //int PanDestHeight = int.Parse(edPanDestHeight.Text);
            //int PanDestTop = int.Parse(edPanDestTop.Text);

            //var EnableLiveRotation = cbLiveRotation.Checked;
            //int LiveRotationAngle = tbLiveRotationAngle.Value;

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

                //EnablePan = EnablePan,
                //PanStartTime = PanStartTime,
                //PanStopTime = PanStopTime,
                //PanSourceLeft = PanSourceLeft,
                //PanSourceWidth = PanSourceWidth,
                //PanSourceHeight = PanSourceHeight,
                //PanSourceTop = PanSourceTop,
                //PanDestLeft = PanDestLeft,
                //PanDestWidth = PanDestWidth,
                //PanDestHeight = PanDestHeight,
                //PanDestTop = PanDestTop,

                //EnableLiveRotation = EnableLiveRotation,
                //LiveRotationAngle = LiveRotationAngle,
            };
            ServiceLocator.CameraSettingConfig = cameraSettings;
            XmlSettingsHelper.Save<CameraSettings>(Path.Combine(
                ConfigurationManager.AppSettings["File:BasePath"],
                ConfigurationManager.AppSettings["File:CameraConfig"]), cameraSettings);
        }

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
    }
}
