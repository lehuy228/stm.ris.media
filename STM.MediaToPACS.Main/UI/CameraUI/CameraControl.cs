using FlashCap;
using MediaToPacs.Core.Models;
using STM.MediaToPACS.Main.Utilities;
using Serilog;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STM.MediaToPACS.Main.UI.CameraUI
{
    public partial class CameraControl : UserControl
    {
        private CaptureDevice _captureDevice;
        private volatile bool _rendering;
        private Timer timer;
        private int seconds;

        public CameraControl(string VideoInputDevice)
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;

            seconds = 0;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            seconds++;
            labelTime.Text = TimeSpan.FromSeconds(seconds).ToString(@"hh\:mm\:ss");
        }

        private static readonly Random _random = new Random();

        private string CreateFilePath(bool isCheckCapture, string relativePath)
        {
            string baseFolder = ServiceLocator.GetMediaStorageBasePath();
            string appName = Application.ProductName;
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string folderPath = Path.Combine(baseFolder, "Patient", relativePath);

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

            return filePath;
        }

        #region FlashCap: liệt kê thiết bị / chọn format

        /// <summary>
        /// Liệt kê các camera khả dụng (chỉ lấy thiết bị có ít nhất 1 format).
        /// </summary>
        public static CaptureDeviceDescriptor[] GetVideoDevices()
        {
            return new CaptureDevices()
                .EnumerateDescriptors()
                .Where(d => d.Characteristics.Length > 0)
                .ToArray();
        }

        /// <summary>
        /// Tên hiển thị của một format (lưu vào CameraSettings.InphutFormat).
        /// </summary>
        public static string GetFormatName(VideoCharacteristics characteristics)
        {
            return $"{characteristics.Width}x{characteristics.Height} {characteristics.PixelFormat}";
        }

        public static CaptureDeviceDescriptor FindDescriptor(string deviceName)
        {
            var devices = GetVideoDevices();
            return devices.FirstOrDefault(d => d.Name == deviceName) ?? devices.FirstOrDefault();
        }

        /// <summary>
        /// Tìm format khớp với cấu hình đã lưu; nếu không khớp thì lấy độ phân giải cao nhất.
        /// </summary>
        public static VideoCharacteristics FindCharacteristics(CaptureDeviceDescriptor descriptor, string formatName, string frameRate)
        {
            var candidates = descriptor.Characteristics
                .Where(c => c.PixelFormat != PixelFormats.Unknown)
                .ToList();
            if (candidates.Count == 0)
            {
                return descriptor.Characteristics.FirstOrDefault();
            }

            var matched = candidates.Where(c => GetFormatName(c) == formatName).ToList();
            if (matched.Count == 0)
            {
                return candidates.OrderByDescending(c => c.Width * c.Height).First();
            }

            if (double.TryParse(frameRate, NumberStyles.Any, CultureInfo.CurrentCulture, out double fps))
            {
                return matched.OrderBy(c => Math.Abs((double)c.FramesPerSecond - fps)).First();
            }

            return matched.OrderByDescending(c => (double)c.FramesPerSecond).First();
        }

        #endregion

        #region FlashCap: preview / snapshot

        public async Task StopCaptureAsync()
        {
            timer.Stop();
            seconds = 0;
            labelTime.Text = "00:00:00";

            if (_captureDevice != null)
            {
                try
                {
                    await _captureDevice.StopAsync();
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Lỗi khi dừng camera");
                }
                _captureDevice.Dispose();
                _captureDevice = null;
            }
        }

        public async Task SetVideoCapturePreviewAsync()
        {
            await StopCaptureAsync();

            var config = ServiceLocator.CameraSettingConfig;
            var descriptor = FindDescriptor(config?.VideoInputDevice);
            if (descriptor == null)
            {
                Log.Warning("Không tìm thấy thiết bị camera nào");
                return;
            }

            var characteristics = FindCharacteristics(descriptor, config?.InphutFormat, config?.FrameRate);
            if (characteristics == null)
            {
                Log.Warning("Thiết bị {Device} không có format video hợp lệ", descriptor.Name);
                return;
            }

            Log.Information("Mở camera {Device} với format {Format} @{Fps}fps",
                descriptor.Name, GetFormatName(characteristics), (double)characteristics.FramesPerSecond);

            _captureDevice = await descriptor.OpenAsync(characteristics, TranscodeFormats.Auto, OnFrameArrived);
            await _captureDevice.StartAsync();
        }

        public async Task PreviewCaptureAsync()
        {
            await SetVideoCapturePreviewAsync();
        }

        /// <summary>
        /// Nhận khung hình từ FlashCap (chạy trên worker thread), áp hiệu ứng rồi đưa lên PictureBox.
        /// Bỏ qua khung hình mới nếu khung trước chưa vẽ xong để không dồn hàng đợi.
        /// </summary>
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
                    frame = ApplyEffects(decoded, ServiceLocator.CameraSettingConfig);
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
                Log.Warning(ex, "Lỗi xử lý khung hình camera");
            }
        }

        public async Task<string> SnapshotAsync(string subFolder)
        {
            // Ảnh hiển thị chỉ được thay trên UI thread nên đọc trực tiếp là an toàn
            var image = videoView1.Image;
            if (image == null)
            {
                Log.Warning("Chưa có khung hình camera để chụp");
                return null;
            }

            // Clone ngay trên UI thread vì frame mới có thể dispose ảnh đang hiển thị
            var snapshot = (Bitmap)((Bitmap)image).Clone();
            string filePath = CreateFilePath(false, subFolder);
            await Task.Run(() =>
            {
                using (snapshot)
                using (var parameters = new EncoderParameters(1))
                {
                    var encoder = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                    parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L);
                    snapshot.Save(filePath, encoder, parameters);
                }
            });
            return filePath;
        }

        #endregion

        #region Hiệu ứng (GDI+, thay cho Video_Effects của VisioForge)

        internal static Bitmap ApplyEffects(Bitmap source, CameraSettings cfg)
        {
            // Zoom: cắt vùng giữa (dịch theo ShiftX/ShiftY) rồi phóng to
            var srcRect = new Rectangle(0, 0, source.Width, source.Height);
            if (cfg != null && cfg.EnableZoom && cfg.Zoom > 1.0)
            {
                int w = (int)(source.Width / cfg.Zoom);
                int h = (int)(source.Height / cfg.Zoom);
                int x = (source.Width - w) / 2 + cfg.ZoomShiftX;
                int y = (source.Height - h) / 2 - cfg.ZoomShiftY;
                x = Math.Max(0, Math.Min(x, source.Width - w));
                y = Math.Max(0, Math.Min(y, source.Height - h));
                srcRect = new Rectangle(x, y, w, h);
            }

            var result = new Bitmap(source.Width, source.Height, PixelFormat.Format24bppRgb);
            using (var g = Graphics.FromImage(result))
            {
                g.InterpolationMode = InterpolationMode.Bilinear;

                if (cfg != null && cfg.EnableLiveRotation && cfg.LiveRotationAngle != 0)
                {
                    g.TranslateTransform(result.Width / 2f, result.Height / 2f);
                    g.RotateTransform(cfg.LiveRotationAngle);
                    g.TranslateTransform(-result.Width / 2f, -result.Height / 2f);
                }

                var colorMatrix = BuildColorMatrix(cfg);
                var destRect = new Rectangle(0, 0, result.Width, result.Height);
                if (colorMatrix != null)
                {
                    using (var attrs = new ImageAttributes())
                    {
                        attrs.SetColorMatrix(colorMatrix);
                        g.DrawImage(source, destRect,
                            srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height,
                            GraphicsUnit.Pixel, attrs);
                    }
                }
                else
                {
                    g.DrawImage(source, destRect, srcRect, GraphicsUnit.Pixel);
                }
            }

            if (cfg != null)
            {
                if (cfg.FlipX && cfg.FlipY) result.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                else if (cfg.FlipX) result.RotateFlip(RotateFlipType.RotateNoneFlipX);
                else if (cfg.FlipY) result.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }

            return result;
        }

        private static ColorMatrix BuildColorMatrix(CameraSettings cfg)
        {
            bool grey = cfg != null && cfg.Greyscale;
            bool invert = cfg != null && cfg.Invert;
            if (!grey && !invert) return null;

            if (grey && invert)
            {
                // 1 - grayscale
                return new ColorMatrix(new[]
                {
                    new float[] {-0.299f, -0.299f, -0.299f, 0, 0},
                    new float[] {-0.587f, -0.587f, -0.587f, 0, 0},
                    new float[] {-0.114f, -0.114f, -0.114f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {1, 1, 1, 0, 1}
                });
            }

            if (grey)
            {
                return new ColorMatrix(new[]
                {
                    new float[] {0.299f, 0.299f, 0.299f, 0, 0},
                    new float[] {0.587f, 0.587f, 0.587f, 0, 0},
                    new float[] {0.114f, 0.114f, 0.114f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });
            }

            // invert
            return new ColorMatrix(new[]
            {
                new float[] {-1, 0, 0, 0, 0},
                new float[] {0, -1, 0, 0, 0},
                new float[] {0, 0, -1, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {1, 1, 1, 0, 1}
            });
        }

        #endregion

        #region Code VisioForge cũ (đã thay bằng FlashCap - chỉ dùng preview + snapshot, bỏ quay video)
        /*
        public VideoCaptureCore VideoCapture1 { get; private set; }
        private MP4SettingsDialog mp4SettingsDialog;

        private async void InitUI()
        {
            VideoCapture1 = await VideoCaptureCore.CreateAsync(videoView1 as IVideoView);
            VideoCapture1.SetLicenseKey("...", "...", "...");
        }

        private void SetMP4Output(ref MP4Output mp4Output)
        {
            if (this.mp4SettingsDialog == null)
            {
                this.mp4SettingsDialog = new MP4SettingsDialog();
            }

            this.mp4SettingsDialog.SaveSettings(ref mp4Output);
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

        // Quay video MP4 - đã bỏ (ứng dụng chỉ dùng preview + snapshot)
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
        */
        #endregion
    }
}
