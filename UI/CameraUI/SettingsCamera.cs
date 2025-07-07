using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Core.Types;
using VisioForge.Core.Types.VideoCapture;
using VisioForge.Core.Types.VideoEffects;
using VisioForge.Core.UI.WinForms.Dialogs.OutputFormats;
using VisioForge.Libs.DirectShowLib;

namespace PrintToPACSDemo.UI.CameraUI
{
    public partial class SettingsCamera : DevExpress.XtraEditors.XtraForm
    {
        private CameraControl _cameraControl;
        private MP4SettingsDialog mp4SettingsDialog;
        private AVISettingsDialog aviSettingsDialog;
        private WMVSettingsDialog wmvSettingsDialog;
        private DVSettingsDialog dvSettingsDialog;
        private PCMSettingsDialog pcmSettingsDialog;
        private MP3SettingsDialog mp3SettingsDialog;
        private WebMSettingsDialog webmSettingsDialog;
        private FFMPEGSettingsDialog ffmpegSettingsDialog;
        private FFMPEGEXESettingsDialog ffmpegEXESettingsDialog;
        private FLACSettingsDialog flacSettingsDialog;
        private M4ASettingsDialog m4aSettingsDialog;
        private OggVorbisSettingsDialog oggVorbisSettingsDialog;
        private SpeexSettingsDialog speexSettingsDialog;
        private CustomFormatSettingsDialog customFormatSettingsDialog;
        private HWEncodersOutputSettingsDialog mp4HWSettingsDialog;
        private GIFSettingsDialog gifSettingsDialog;
        private HWEncodersOutputSettingsDialog mpegTSSettingsDialog;
        private HWEncodersOutputSettingsDialog movSettingsDialog;

        public SettingsCamera(CameraControl cameraControl)
        {
            InitializeComponent();
            _cameraControl = cameraControl;
        }

        private void SettingsCamera_Load(object sender, EventArgs e)
        {
            InitUI();
        }
        private async void InitUI()
        {
            cbOutputFormat.SelectedIndex = 22;
            cbFlipX.Checked = _cameraControl.IsCheckFlipX;
            cbFlipY.Checked = _cameraControl.IsCheckFlipY;
            cbGreyscale.Checked = _cameraControl.IsCheckGreyscale;
            cbInvert.Checked = _cameraControl.IsCheckInvert;
            cbZoom.Checked = _cameraControl.IsCheckZoom;
            cbPan.Checked = _cameraControl.IsCheckPan;
            cbLiveRotation.Checked = _cameraControl.IsCheckRotation;
            foreach (var device in _cameraControl.VideoCapture1.Video_CaptureDevices())
            {
                cbVideoInputDevice.Properties.Items.Add(device.Name);
            }

            int indexVideo = FindStringExact(cbVideoInputDevice, _cameraControl.VideoInputDevice);  
            if (indexVideo != -1)
            {
                cbVideoInputDevice.SelectedIndex = indexVideo;
            }
            else
            {
                cbVideoInputDevice.SelectedIndex = 0;
            }

            foreach (var device in _cameraControl.VideoCapture1.Audio_CaptureDevices())
            {
                cbAudioInputDevice.Properties.Items.Add(device.Name);
                //cbAdditionalAudioSource.Items.Add(device.Name);
            }

            int indexAudio = FindStringExact(cbAudioInputDevice, _cameraControl.AudioInputDevice); 
            if (indexAudio != -1)
            {
                cbAudioInputDevice.SelectedIndex = indexAudio;
            }
            else
            {
                cbAudioInputDevice.SelectedIndex = 0;
            }

            if (!string.IsNullOrEmpty(cbAudioInputDevice.Text))
            {
                var audioInputDevice =
                    _cameraControl.VideoCapture1.Audio_CaptureDevices().FirstOrDefault(device => device.Name == cbAudioInputDevice.Text);
                if (audioInputDevice != null)
                {
                    foreach (string line in audioInputDevice.Lines)
                    {
                        cbAudioInputLine.Properties.Items.Add(line);
                    }

                    int indexAudioLine = FindStringExact(cbAudioInputLine, _cameraControl.AudioInputLine); 
                    if (indexAudioLine != -1)
                    {
                        cbAudioInputLine.SelectedIndex = indexAudioLine;
                    }
                    else
                    {
                        cbAudioInputLine.SelectedIndex = 0;
                    }
                }
            }
        }

        private int FindStringExact(DevExpress.XtraEditors.ComboBoxEdit comboBox, string searchKey)
        {
            int indexAudioLine = -1;
            for (int i = 0; i < comboBox.Properties.Items.Count; i++)
            {
                if (comboBox.Properties.Items[i].ToString() == searchKey)
                {
                    indexAudioLine = i;
                    break;
                }
            }
            return indexAudioLine;
        }

        private void cbVideoInputDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbVideoInputDevice.SelectedIndex != -1)
            {
                cbVideoInputFormat.Properties.Items.Clear();

                var deviceItem = _cameraControl.VideoCapture1.Video_CaptureDevices().FirstOrDefault(device => device.Name == cbVideoInputDevice.Text);
                if (deviceItem == null)
                {
                    return;
                }

                foreach (var format in deviceItem.VideoFormats)
                {
                    cbVideoInputFormat.Properties.Items.Add(format.Name);
                }

                int index = FindStringExact(cbAudioInputDevice, _cameraControl.AudioInputDevice); 
                if (index != -1)
                {
                    cbVideoInputFormat.SelectedIndex = index;
                }
                else
                {
                    cbVideoInputFormat.SelectedIndex = 0;
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
                var deviceItem = _cameraControl.VideoCapture1.Video_CaptureDevices().FirstOrDefault(device => device.Name == cbVideoInputDevice.Text);
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

                int index = FindStringExact(cbVideoInputFrameRate, _cameraControl.VideoInputFrameRate);
                if (index != -1)
                {
                    cbVideoInputFrameRate.SelectedIndex = index;
                }
                else
                {
                    cbVideoInputFrameRate.SelectedIndex = 0;
                }
            }

            _cameraControl.VideoInputDevice = cbAudioInputDevice.Text;
            _cameraControl.VideoInputFormat = cbVideoInputFormat.Text;
            _cameraControl.VideoInputDevice = cbVideoInputDevice.Text;
            _cameraControl.OutputFormat = cbOutputFormat.Text;
            _cameraControl.SetVideoCapturePreviewAsync();

            //if (_cameraControl.IsCheckFlipX)
            //{
            //    cbFlipX_CheckedChanged(null, null);
            //}
            //if (_cameraControl.IsCheckFlipY)
            //{
            //    cbFlipY_CheckedChanged(null, null);
            //}
            //if (_cameraControl.IsCheckGreyscale)
            //{
            //    cbGreyscale_CheckedChanged(null, null);
            //}
            //if (_cameraControl.IsCheckInvert)
            //{
            //    cbInvert_CheckedChanged(null, null);
            //}
            //if (_cameraControl.IsCheckZoom)
            //{
            //    cbZoom_CheckedChanged(null, null);
            //}
            //if (_cameraControl.IsCheckRotation)
            //{
            //    cbLiveRotation_CheckedChanged(null, null);
            //}
        }

        private void cbAudioInputDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbAudioInputFormat.Properties.Items.Clear();

            if (cbAudioInputDevice.SelectedIndex != -1)
            {
                var deviceItem = _cameraControl.VideoCapture1.Audio_CaptureDevices().FirstOrDefault(device => device.Name == cbAudioInputDevice.Text);
                if (deviceItem != null)
                {
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
                }
            }
        }

        private void buttonSettingCam_Click(object sender, EventArgs e)
        {
            _cameraControl.SettingCam();
        }

        private void btOutputConfigure_Click(object sender, EventArgs e)
        {
           
                switch (cbOutputFormat.SelectedIndex)
                {
                    case 0:
                    case 1:
                        {
                            if (aviSettingsDialog == null)
                            {
                                aviSettingsDialog = new AVISettingsDialog(_cameraControl.VideoCapture1);
                            }

                            aviSettingsDialog.ShowDialog(this);

                            break;
                        }
                    case 2:
                        {
                            if (wmvSettingsDialog == null)
                            {
                                wmvSettingsDialog = new WMVSettingsDialog(_cameraControl.VideoCapture1);
                            }

                            wmvSettingsDialog.WMA = false;
                            wmvSettingsDialog.ShowDialog(this);

                            break;
                        }

                    case 3:
                        {
                            if (dvSettingsDialog == null)
                            {
                                dvSettingsDialog = new DVSettingsDialog();
                            }

                            dvSettingsDialog.ShowDialog(this);

                            break;
                        }
                    case 4:
                        {
                            if (pcmSettingsDialog == null)
                            {
                                pcmSettingsDialog = new PCMSettingsDialog(_cameraControl.VideoCapture1);
                            }

                            pcmSettingsDialog.ShowDialog(this);

                            break;
                        }
                    case 5:
                        {
                            if (mp3SettingsDialog == null)
                            {
                                mp3SettingsDialog = new MP3SettingsDialog();
                            }

                            mp3SettingsDialog.ShowDialog(this);

                            break;
                        }
                    case 6:
                        {
                            if (m4aSettingsDialog == null)
                            {
                                m4aSettingsDialog = new M4ASettingsDialog();
                            }

                            m4aSettingsDialog.ShowDialog(this);

                            break;
                        }
                    case 7:
                        {
                            if (wmvSettingsDialog == null)
                            {
                                wmvSettingsDialog = new WMVSettingsDialog(_cameraControl.VideoCapture1);
                            }

                            wmvSettingsDialog.WMA = true;
                            wmvSettingsDialog.ShowDialog(this);

                            break;
                        }

                    case 8:
                        {
                            if (flacSettingsDialog == null)
                            {
                                flacSettingsDialog = new FLACSettingsDialog();
                            }

                            flacSettingsDialog.ShowDialog(this);

                            break;
                        }
                    case 9:
                        {
                            if (oggVorbisSettingsDialog == null)
                            {
                                oggVorbisSettingsDialog = new OggVorbisSettingsDialog();
                            }

                            oggVorbisSettingsDialog.ShowDialog(this);

                            break;
                        }
                    case 10:
                        {
                            if (speexSettingsDialog == null)
                            {
                                speexSettingsDialog = new SpeexSettingsDialog();
                            }

                            speexSettingsDialog.ShowDialog(this);

                            break;
                        }
                    case 11:
                    case 16:
                    case 17:
                    case 18:
                        {
                            if (customFormatSettingsDialog == null)
                            {
                                customFormatSettingsDialog = new CustomFormatSettingsDialog(_cameraControl.VideoCapture1);
                            }

                            customFormatSettingsDialog.ShowDialog(this);

                            break;
                        }
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                        {
                            MessageBox.Show(this, "No settings available for selected output format.");

                            break;
                        }
                    case 19:
                        {
                            if (webmSettingsDialog == null)
                            {
                                webmSettingsDialog = new WebMSettingsDialog();
                            }

                            webmSettingsDialog.ShowDialog(this);

                            break;
                        }
                    case 20:
                        {
                            if (ffmpegSettingsDialog == null)
                            {
                                ffmpegSettingsDialog = new FFMPEGSettingsDialog();
                            }

                            ffmpegSettingsDialog.ShowDialog(this);

                            break;
                        }
                    case 21:
                        {
                            if (ffmpegEXESettingsDialog == null)
                            {
                                ffmpegEXESettingsDialog = new FFMPEGEXESettingsDialog();
                            }

                            ffmpegEXESettingsDialog.ShowDialog(this);

                            break;
                        }
                    case 22:
                    case 25:
                        {
                            if (mp4SettingsDialog == null)
                            {
                                mp4SettingsDialog = new MP4SettingsDialog();
                            }

                            mp4SettingsDialog.ShowDialog(this);

                            break;
                        }
                    case 23:
                        {
                            if (mp4HWSettingsDialog == null)
                            {
                                mp4HWSettingsDialog = new HWEncodersOutputSettingsDialog(HWSettingsDialogMode.MP4);
                            }

                            mp4HWSettingsDialog.ShowDialog(this);

                            break;
                        }
                    case 24:
                        {
                            if (gifSettingsDialog == null)
                            {
                                gifSettingsDialog = new GIFSettingsDialog();
                            }

                            gifSettingsDialog.ShowDialog(this);

                            break;
                        }
                    case 26:
                        {
                            if (mpegTSSettingsDialog == null)
                            {
                                mpegTSSettingsDialog = new HWEncodersOutputSettingsDialog(HWSettingsDialogMode.MPEGTS);
                            }

                            mpegTSSettingsDialog.ShowDialog(this);

                            break;
                        }
                    case 27:
                        {
                            if (movSettingsDialog == null)
                            {
                                movSettingsDialog = new HWEncodersOutputSettingsDialog(HWSettingsDialogMode.MOV);
                            }

                            movSettingsDialog.ShowDialog(this);

                            break;
                        }
                }
          
        }

        private void cbGreyscale_CheckedChanged(object sender, EventArgs e)
        {
            _cameraControl.IsCheckGreyscale = cbGreyscale.Checked;
            IVideoEffectGrayscale grayscale;
            var effect = _cameraControl.VideoCapture1.Video_Effects_Get("Grayscale");
            if (effect == null)
            {
                grayscale = new VideoEffectGrayscale(cbGreyscale.Checked);
                _cameraControl.VideoCapture1.Video_Effects_Add(grayscale);
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

        private void cbInvert_CheckedChanged(object sender, EventArgs e)
        {
            _cameraControl.IsCheckInvert = cbInvert.Checked;
            IVideoEffectInvert invert;
            var effect = _cameraControl.VideoCapture1.Video_Effects_Get("Invert");
            if (effect == null)
            {
                invert = new VideoEffectInvert(cbInvert.Checked);
                _cameraControl.VideoCapture1.Video_Effects_Add(invert);
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
            _cameraControl.IsCheckFlipX = cbFlipX.Checked;
            IVideoEffectFlipDown flip;
            var effect = _cameraControl.VideoCapture1.Video_Effects_Get("FlipDown");
            if (effect == null)
            {
                flip = new VideoEffectFlipHorizontal(cbFlipX.Checked);
                _cameraControl.VideoCapture1.Video_Effects_Add(flip);
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
            _cameraControl.IsCheckFlipY = cbFlipY.Checked;
            IVideoEffectFlipRight flip;
            var effect = _cameraControl.VideoCapture1.Video_Effects_Get("FlipRight");
            if (effect == null)
            {
                flip = new VideoEffectFlipVertical(cbFlipY.Checked);
                _cameraControl.VideoCapture1.Video_Effects_Add(flip);
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

        private void cbZoom_CheckedChanged(object sender, EventArgs e)
        {
            _cameraControl.IsCheckZoom = cbZoom.Checked;
            IVideoEffectZoom zoomEffect;
            var effect = _cameraControl.VideoCapture1.Video_Effects_Get("Zoom");
            if (effect == null)
            {
                zoomEffect = new VideoEffectZoom(_cameraControl.Zoom, _cameraControl.Zoom, _cameraControl.ZoomShiftX, _cameraControl.ZoomShiftY, _cameraControl.IsCheckZoom);
                _cameraControl.VideoCapture1.Video_Effects_Add(zoomEffect);
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

            zoomEffect.ZoomX = _cameraControl.Zoom;
            zoomEffect.ZoomY = _cameraControl.Zoom;
            zoomEffect.ShiftX = _cameraControl.ZoomShiftX;
            zoomEffect.ShiftY = _cameraControl.ZoomShiftY;
            zoomEffect.Enabled = _cameraControl.IsCheckZoom;
        }

        private void btEffZoomIn_Click(object sender, EventArgs e)
        {
            _cameraControl.Zoom += 0.1;
            _cameraControl.Zoom = Math.Min(_cameraControl.Zoom, 5);

            cbZoom_CheckedChanged(null, null);
        }

        private void btEffZoomOut_Click(object sender, EventArgs e)
        {
            _cameraControl.Zoom -= 0.1;
            _cameraControl.Zoom = Math.Min(_cameraControl.Zoom, 5);

            cbZoom_CheckedChanged(null, null);
        }

        private void btEffZoomUp_Click(object sender, EventArgs e)
        {
            _cameraControl.ZoomShiftY += 20;

            cbZoom_CheckedChanged(null, null);
        }

        private void btEffZoomDown_Click(object sender, EventArgs e)
        {
            _cameraControl.ZoomShiftY -= 20;

            cbZoom_CheckedChanged(null, null);
        }

        private void btEffZoomRight_Click(object sender, EventArgs e)
        {
            _cameraControl.ZoomShiftX += 20;

            cbZoom_CheckedChanged(null, null);
        }

        private void btEffZoomLeft_Click(object sender, EventArgs e)
        {
            _cameraControl.ZoomShiftX -= 20;

            cbZoom_CheckedChanged(null, null);
        }

        private void cbLiveRotation_CheckedChanged(object sender, EventArgs e)
        {
            _cameraControl.IsCheckRotation = cbLiveRotation.Checked;
            IVideoEffectRotate rotate;
            var effect = _cameraControl.VideoCapture1.Video_Effects_Get("Rotate");
            if (effect == null)
            {
                rotate = new VideoEffectRotate(
                    _cameraControl.IsCheckRotation,
                    tbLiveRotationAngle.Value, false);
                _cameraControl.VideoCapture1.Video_Effects_Add(rotate);
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

            rotate.Enabled = _cameraControl.IsCheckRotation;
            rotate.Angle = tbLiveRotationAngle.Value;
        }

        private void tbLiveRotationAngle_EditValueChanged(object sender, EventArgs e)
        {
            cbLiveRotation_CheckedChanged(sender, e);
            labelLiveRotationAngle.Text = tbLiveRotationAngle.Value.ToString();
        }

        private void cbOutputFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            _cameraControl.IndexOutputFormat = cbOutputFormat.SelectedIndex;
        }
    }
}
