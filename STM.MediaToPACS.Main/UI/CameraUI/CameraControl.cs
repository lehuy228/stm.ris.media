using MediaToPacs.Core.Models;
using STM.MediaToPACS.Main.Utilities;
using Serilog;
using System;
using System.Configuration;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Core.Helpers;
using VisioForge.Core.Types;
using VisioForge.Core.Types.AudioEffects;
using VisioForge.Core.Types.VideoCapture;
using VisioForge.Core.Types.VideoEffects;
using VisioForge.Core.UI.WinForms.Dialogs.OutputFormats;
using VisioForge.Core.VideoCapture;
using MP4Output = VisioForge.Core.Types.Output.MP4Output;


namespace STM.MediaToPACS.Main.UI.CameraUI
{
    public partial class CameraControl : UserControl
    {
        public VideoCaptureCore VideoCapture1 { get; private set; }
        private Timer timer;
        private int seconds;
        private MP4SettingsDialog mp4SettingsDialog;

        public CameraControl(string VideoInputDevice)
        {
            InitializeComponent();
            InitUI();
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;

            seconds = 0;
        }

        private async void InitUI()
        {
            VideoCapture1 = await VideoCaptureCore.CreateAsync(videoView1 as IVideoView);
            VideoCapture1.SetLicenseKey("1E17-F8AA-BB54-D7A1-BD5F-446D", "STM TECHNOLOGY AND COMMERCIAL JOINT STOCK COMPANY", "linh@anphats.com");
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

        private static readonly Random _random = new Random();

        private string CreateFilePath(bool isCheckCapture, string relativePath)
        {
            string baseFolder = ServiceLocator.GetAppDataBasePath();
            string appName = Application.ProductName;
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string folderPath = Path.Combine(baseFolder, "BenhNhan", relativePath);

            // Đảm bảo folder tồn tại
            Directory.CreateDirectory(folderPath);

            // Sinh tên file
            string fileNameWithoutExt = $"{appName}_{timestamp}_{_random.Next(1, 1000)}";
            string extension = isCheckCapture ? ".tmp" : ".jpg";
            string filePath = Path.Combine(folderPath, fileNameWithoutExt + extension);

            // Ghi log chi tiết
            Log.Information(
                "Tạo file mới:\n" +
                "baseFolder = {BaseFolder}\n" +
                "appName = {AppName}\n" +
                "timestamp = {Timestamp}\n" +
                "folderPath = {FolderPath}\n" +
                "fileNameWithoutExt = {FileNameWithoutExt}\n" +
                "filePath = {FilePath}",
                baseFolder, appName, timestamp, folderPath, fileNameWithoutExt, filePath
            );

            // Nếu cần tạo file rỗng thật sự (tùy mục đích)
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath)) { }
            }

            return filePath;
        }

        private void SettingCaptureDevice()
        {
            
            VideoCapture1.Video_Effects_Enabled = true;
            VideoCapture1.Video_CaptureDevice = new VideoCaptureSource(ServiceLocator.CameraSettingConfig.VideoInputDevice)
            {
                Format_UseBest = false,
                Format = ServiceLocator.CameraSettingConfig.InphutFormat,
                FrameRate = new VideoFrameRate(Convert.ToDouble(ServiceLocator.CameraSettingConfig.FrameRate, CultureInfo.CurrentCulture))
            };

            VideoCapture1.Audio_RecordAudio = false;
            VideoCapture1.Audio_PlayAudio = false;
            VideoCapture1.Audio_CaptureDevice = new AudioCaptureSource(ServiceLocator.CameraSettingConfig.AudioInputDevice);
            VideoCapture1.Audio_CaptureDevice.Format = ServiceLocator.CameraSettingConfig.AudioInputFormat;
            VideoCapture1.Audio_CaptureDevice.Line = ServiceLocator.CameraSettingConfig.AudioInputLine;

            VideoCapture1.Video_Sample_Grabber_Enabled = true;

            VideoCapture1.Video_Renderer.Zoom_Ratio = 0;
            VideoCapture1.Video_Renderer.Zoom_ShiftX = 0;
            VideoCapture1.Video_Renderer.Zoom_ShiftY = 0;
            VideoCapture1.Video_Renderer.VideoRenderer = VideoRendererMode.Direct2D;

            VideoCapture1.Video_Renderer.StretchMode = VideoRendererStretchMode.Stretch;
            VideoCapture1.Video_Renderer.Flip_Horizontal = false;
            VideoCapture1.Video_Renderer.Flip_Vertical = false;
            VideoCapture1.Debug_Dir = Path.Combine(ServiceLocator.GetAppDataBasePath(), "Logs");

            VideoCapture1.Audio_OutputDevice = "Default DirectSound Device";

            VideoCapture1.Video_Effects_Clear();
            VideoCapture1.Audio_Effects_Clear(-1);
            VideoCapture1.Audio_Effects_Enabled = true;

            if (ServiceLocator.CameraSettingConfig.Greyscale) SetUpEffectGrayscale();
            if (ServiceLocator.CameraSettingConfig.Invert) SetUpEffectInvert();
            if (ServiceLocator.CameraSettingConfig.FlipX) SetUpEffectFlipRight();
            if (ServiceLocator.CameraSettingConfig.FlipY) SetUpEffectFlipDown();
            if (ServiceLocator.CameraSettingConfig.EnableLiveRotation) SetUpEffectRotation();
            if (ServiceLocator.CameraSettingConfig.EnableZoom) SetUpEffectZoom();

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
                rotate = new VideoEffectRotate(ServiceLocator.CameraSettingConfig.EnableLiveRotation,  ServiceLocator.CameraSettingConfig.LiveRotationAngle, false);
                VideoCapture1.Video_Effects_Add(rotate);
            }
        }

        private void SetUpEffectZoom()
        {
            IVideoEffectZoom zoomEffect;
            var effect = VideoCapture1.Video_Effects_Get("Zoom");
            if (effect == null)
            {
                zoomEffect = new VideoEffectZoom(ServiceLocator.CameraSettingConfig.Zoom, 
                    ServiceLocator.CameraSettingConfig.Zoom, 
                    ServiceLocator.CameraSettingConfig.ZoomShiftX, 
                    ServiceLocator.CameraSettingConfig.ZoomShiftY, 
                    ServiceLocator.CameraSettingConfig.EnableZoom);
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

            zoomEffect.ZoomX = ServiceLocator.CameraSettingConfig.Zoom;
            zoomEffect.ZoomY = ServiceLocator.CameraSettingConfig.Zoom;
            zoomEffect.ShiftX = ServiceLocator.CameraSettingConfig.ZoomShiftX;
            zoomEffect.ShiftY = ServiceLocator.CameraSettingConfig.ZoomShiftY;
            zoomEffect.Enabled = ServiceLocator.CameraSettingConfig.EnableZoom;
        }

        private void SetUpEffectFlipRight()
        {
            IVideoEffectFlipDown flip;
            var effect = VideoCapture1.Video_Effects_Get("FlipDown");
            if (effect == null)
            {
                flip = new VideoEffectFlipHorizontal(ServiceLocator.CameraSettingConfig.FlipY);
                VideoCapture1.Video_Effects_Add(flip);
            }
        }

        private void SetUpEffectFlipDown()
        {
            IVideoEffectFlipRight flip;
            var effect = VideoCapture1.Video_Effects_Get("FlipRight");
            if (effect == null)
            {
                flip = new VideoEffectFlipVertical(ServiceLocator.CameraSettingConfig.FlipX);
                VideoCapture1.Video_Effects_Add(flip);
            }

        }

        private void SetUpEffectInvert()
        {
            IVideoEffectInvert invert;
            var effect = VideoCapture1.Video_Effects_Get("Invert");
            if (effect == null)
            {
                invert = new VideoEffectInvert(ServiceLocator.CameraSettingConfig.Invert);
                VideoCapture1.Video_Effects_Add(invert);
            }
        }

        private void SetUpEffectGrayscale()
        {
            IVideoEffectGrayscale grayscale;
            var effect = VideoCapture1.Video_Effects_Get("Grayscale");
            if (effect == null)
            {
                grayscale = new VideoEffectGrayscale(ServiceLocator.CameraSettingConfig.Greyscale);
                VideoCapture1.Video_Effects_Add(grayscale);
            }
        }

        public async Task StopCaptureAsync()
        {
            timer.Stop();
            seconds = 0;
            labelTime.Text = "00:00:00";
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

        public async Task PreviewCaptureAsync()
        {
            await SetVideoCapturePreviewAsync();
        }

        public async Task<string> SnapshotAsync(string subFolder)
        {
            string filePath = CreateFilePath(false, subFolder);
            await VideoCapture1.Frame_SaveAsync(filePath, ImageFormat.Jpeg, 80);
            return filePath;
        }
    }
}