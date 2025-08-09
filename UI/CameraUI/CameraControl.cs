using MediaToPacs.Core.Models;
using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Core.Helpers;
using VisioForge.Core.Types;
using VisioForge.Core.Types.VideoCapture;
using VisioForge.Core.Types.VideoEffects;
using VisioForge.Core.UI.WinForms.Dialogs.OutputFormats;
using VisioForge.Core.VideoCapture;
using MP4Output = VisioForge.Core.Types.Output.MP4Output;


namespace PrintToPACSDemo.UI.CameraUI
{
    public partial class CameraControl : UserControl
    {
        public VideoCaptureCore VideoCapture1 { get; private set; }

        //
        //public string VideoInputDevice;
        //public string VideoInputFormat;
        //public string VideoInputFrameRate;
        //public string AudioInputDevice;
        //public string AudioInputFormat;
        //public string AudioInputLine;
        //public string OutputFormat;
        //public int IndexOutputFormat = 22;

        //public bool IsCheckGreyscale = false;
        //public bool IsCheckInvert = false;
        //public bool IsCheckFlipX = false;
        //public bool IsCheckFlipY = false;
        //public bool IsCheckZoom = false;
        //public bool IsCheckPan = false;
        //public bool IsCheckRotation = false;
        //public double Zoom = 1.0;
        //public int ZoomShiftX;
        //public int ZoomShiftY;

        //private bool IsCheckVideo;
        //private bool IsCheckResume;
        private CameraSettings _cameraSettings;
        private Timer timer;
        private int seconds;
        private MP4SettingsDialog mp4SettingsDialog;

        public CameraControl(string VideoInputDevice)
        {
            InitializeComponent();
            InitUI(VideoInputDevice);
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;

            seconds = 0;
        }

        private async void InitUI(string VideoInputDevice)
        {
            _cameraSettings = AppSettingsLoader.GetCameraSettings();
            VideoCapture1 = await VideoCaptureCore.CreateAsync(videoView1 as IVideoView);
            VideoCapture1.SetLicenseKey("1E17-F8AA-BB54-D7A1-BD5F-446D", "STM TECHNOLOGY AND COMMERCIAL JOINT STOCK COMPANY", "linh@anphats.com");
            //this.VideoInputDevice = VideoInputDevice;

            //VideoInputFormat = VideoCapture1.Video_CaptureDevices()[0].VideoFormats[0].Name;
            //VideoInputFrameRate = VideoCapture1.Video_CaptureDevices()[0].VideoFormats[0].FrameRates[0].ToString(CultureInfo.CurrentCulture);
            //AudioInputDevice = VideoCapture1.Audio_CaptureDevices()[0].Name;
            //AudioInputFormat = VideoCapture1.Audio_CaptureDevices()[0].Formats[0];
            //AudioInputLine = VideoCapture1.Audio_CaptureDevices()[0].Lines[0];
            //OutputFormat = "FFMPEG (external exe)";

            VideoCapture1.Video_Filters_Clear();
            SettingCaptureDevice();
            VideoCapture1.Mode = VideoCaptureMode.VideoPreview;
            //await VideoCapture1.StopAsync();
            await VideoCapture1.StartAsync();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            seconds++;
            labelTime.Text = TimeSpan.FromSeconds(seconds).ToString(@"hh\:mm\:ss");
        }

        private void SetMP4Output(ref MP4Output mp4Output)
        {
            if (this.mp4SettingsDialog == null)
            {
                this.mp4SettingsDialog = new MP4SettingsDialog();
            }

            this.mp4SettingsDialog.SaveSettings(ref mp4Output);
        }

        private string CreateFilePath(bool IsCheckCapture, string filePath)
        {
            string commonFolder = AppDomain.CurrentDomain.BaseDirectory;
            string appName = Application.ProductName;
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string folderPath = Path.Combine(commonFolder, "BenhNhan", filePath);
            string fileNameWithoutExtension = $"{appName}_{timestamp}";
            string filepath = Path.Combine(folderPath, $"{fileNameWithoutExtension}.");

            if (IsCheckCapture)
            {
                //switch (IndexOutputFormat)
                //{
                //    case 0:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".avi");
                //            break;
                //        }
                //    case 1:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".mkv");
                //            break;
                //        }
                //    case 2:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".wmv");
                //            break;
                //        }

                //    case 3:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".avi");
                //            break;
                //        }
                //    case 4:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".wav");
                //            break;
                //        }
                //    case 5:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".mp3");
                //            break;
                //        }
                //    case 6:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".m4a");
                //            break;
                //        }
                //    case 7:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".wma");
                //            break;
                //        }
                //    case 8:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".flac");
                //            break;
                //        }
                //    case 9:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".ogg");
                //            break;
                //        }
                //    case 10:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".ogg");
                //            break;
                //        }
                //    case 11:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".avi");
                //            break;
                //        }
                //    case 12:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".avi");
                //            break;
                //        }
                //    case 13:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".avi");
                //            break;
                //        }
                //    case 14:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".mpg");
                //            break;
                //        }
                //    case 15:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".mkv");
                //            break;
                //        }
                //    case 16:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".mp4");
                //            break;
                //        }
                //    case 17:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".mp4");
                //            break;
                //        }
                //    case 18:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".avi");
                //            break;
                //        }
                //    case 19:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".webm");
                //            break;
                //        }
                //    case 20:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".avi");
                //            break;
                //        }
                //    case 21:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".avi");
                //            break;
                //        }
                //    case 22:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".mp4");
                //            break;
                //        }
                //    case 23:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".mp4");
                //            break;
                //        }
                //    case 24:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".gif");
                //            break;
                //        }
                //    case 25:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".enc");
                //            break;
                //        }
                //    case 26:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".ts");
                //            break;
                //        }
                //    case 27:
                //        {
                //            filepath = FilenameHelper.ChangeFileExt(filepath, ".mov");
                //            break;
                //        }
                //}
            }
            else
            {
                filepath = FilenameHelper.ChangeFileExt(filepath, ".jpg");
            }
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            if (!File.Exists(filepath))
            {
                File.Create(filepath).Dispose();
            }
            return filepath;
        }

        private void SettingCaptureDevice()
        {
            VideoCapture1.Video_Effects_Enabled = false;
            VideoCapture1.Video_CaptureDevice = new VideoCaptureSource(_cameraSettings.VideoInputDevice)
            {
                Format_UseBest = false,
                Format = _cameraSettings.InphutFormat,
                FrameRate = new VideoFrameRate(Convert.ToDouble(_cameraSettings.FrameRate, CultureInfo.CurrentCulture))
            };

            VideoCapture1.Audio_RecordAudio = false;
            VideoCapture1.Audio_PlayAudio = false;

            VideoCapture1.Audio_CaptureDevice = new AudioCaptureSource(_cameraSettings.AudioInputDevice);
            VideoCapture1.Audio_CaptureDevice.Format = _cameraSettings.AudioInputFormat;
            VideoCapture1.Audio_CaptureDevice.Line = _cameraSettings.AudioInputLine;

            VideoCapture1.Video_Sample_Grabber_Enabled = true;
            VideoCapture1.Video_Renderer.Zoom_Ratio = 0;
            VideoCapture1.Video_Renderer.Zoom_ShiftX = 0;
            VideoCapture1.Video_Renderer.Zoom_ShiftY = 0;

            if (_cameraSettings.Greyscale) SetUpEffectGrayscale();
            if (_cameraSettings.Invert) SetUpEffectInvert();
            if (_cameraSettings.FlipX) SetUpEffectFlipRight();
            if (_cameraSettings.FlipY) SetUpEffectFlipDown();
            if (_cameraSettings.EnableLiveRotation) SetUpEffectRotation();
            if (_cameraSettings.EnableZoom) SetUpEffectZoom();

            SetUpEffectGrayscale();
            SetUpEffectInvert();
            SetUpEffectFlipDown();
            SetUpEffectFlipRight();
            SetUpEffectZoom();
            //SetUpEffectPan();
            SetUpEffectRotation();
        }

        private void SetUpEffectRotation()
        {
            IVideoEffectRotate rotate;
            var effect = VideoCapture1.Video_Effects_Get("Rotate");
            if (effect == null)
            {
                rotate = new VideoEffectRotate(_cameraSettings.EnableLiveRotation,  _cameraSettings.LiveRotationAngle, false);
                VideoCapture1.Video_Effects_Add(rotate);
            }
        }

        private void SetUpEffectPan()
        {
            throw new NotImplementedException();
        }

        private void SetUpEffectZoom()
        {
            IVideoEffectZoom zoomEffect;
            var effect = VideoCapture1.Video_Effects_Get("Zoom");
            if (effect == null)
            {
                zoomEffect = new VideoEffectZoom(_cameraSettings.Zoom, _cameraSettings.Zoom, _cameraSettings.ZoomShiftX, _cameraSettings.ZoomShiftY, _cameraSettings.EnableZoom);
                VideoCapture1.Video_Effects_Add(zoomEffect);
            }
        }

        private void SetUpEffectFlipRight()
        {
            IVideoEffectFlipDown flip;
            var effect = VideoCapture1.Video_Effects_Get("FlipDown");
            if (effect == null)
            {
                flip = new VideoEffectFlipHorizontal(_cameraSettings.FlipY);
                VideoCapture1.Video_Effects_Add(flip);
            }
        }

        private void SetUpEffectFlipDown()
        {
            IVideoEffectFlipRight flip;
            var effect = VideoCapture1.Video_Effects_Get("FlipRight");
            if (effect == null)
            {
                flip = new VideoEffectFlipVertical(_cameraSettings.FlipX);
                VideoCapture1.Video_Effects_Add(flip);
            }

        }

        private void SetUpEffectInvert()
        {
            IVideoEffectInvert invert;
            var effect = VideoCapture1.Video_Effects_Get("Invert");
            if (effect == null)
            {
                invert = new VideoEffectInvert(_cameraSettings.Invert);
                VideoCapture1.Video_Effects_Add(invert);
            }
        }

        private void SetUpEffectGrayscale()
        {
            IVideoEffectGrayscale grayscale;
            var effect = VideoCapture1.Video_Effects_Get("Grayscale");
            if (effect == null)
            {
                grayscale = new VideoEffectGrayscale(_cameraSettings.Greyscale);
                VideoCapture1.Video_Effects_Add(grayscale);
            }
        }

        public void SettingCam()
        {
            VideoCapture1.Video_CaptureDevice_SettingsDialog_Show(IntPtr.Zero, _cameraSettings.VideoInputDevice);
        }


        public async void CameraControlRemoved()
        {
            await VideoCapture1.StopAsync();
        }

        public async Task SetVideoCapturePreviewAsync()
        {
            VideoCapture1.Video_Filters_Clear();
            await VideoCapture1.StopAsync();
            SettingCaptureDevice();
            VideoCapture1.Mode = VideoCaptureMode.VideoPreview;
            await VideoCapture1.StartAsync();
        }

        public async Task<string> StartRecordAsync(string folderPath)
        {
            VideoCapture1.Video_Filters_Clear();
            await VideoCapture1.StopAsync();
            labelTime.Text = "00:00:00";
            timer.Start();
            VideoCapture1.Mode = VideoCaptureMode.VideoCapture;

            SettingCaptureDevice();
            string filePath = CreateFilePath(true, folderPath);
            string directoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            VideoCapture1.Output_Filename = filePath;
            var mp4Output = new MP4Output();
                SetMP4Output(ref mp4Output);
                VideoCapture1.Output_Format = mp4Output;

            await VideoCapture1.StartAsync();
            return filePath;
        }

        public async Task StopCaptureAsync()
        {
            timer.Stop();
            seconds = 0;
            labelTime.Text = "00:00:00";
            await SetVideoCapturePreviewAsync();
        }

        public async Task SetPauseResumeCaptureAsync(bool isCheck)
        {
            if (isCheck)
            {
                timer.Start();
                await VideoCapture1.ResumeAsync();
            }
            else
            {
                timer.Stop();
                await VideoCapture1.PauseAsync();
            }
        }

        public async Task<string> SnapshotAsync(string subFolder)
        {
            string filePath = CreateFilePath(false, subFolder);
            await VideoCapture1.Frame_SaveAsync(filePath, ImageFormat.Jpeg, 80);
            return filePath;
        }
    }
}