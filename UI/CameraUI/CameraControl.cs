using Leadtools.DicomDemos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Core.Helpers;
using VisioForge.Core.Types;
using VisioForge.Core.Types.Output;
using VisioForge.Core.Types.VideoCapture;
using VisioForge.Core.Types.VideoEffects;
using VisioForge.Core.Types.X.Output;
using VisioForge.Core.UI.WinForms.Dialogs.OutputFormats;
using VisioForge.Core.VideoCapture;
using MP4Output = VisioForge.Core.Types.Output.MP4Output;


namespace PrintToPACSDemo.UI.CameraUI
{
    public partial class CameraControl : UserControl
    {
        public VideoCaptureCore VideoCapture1 { get; private set; }

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
        private bool IsCheckVideo;
        private bool IsCheckResume;
        private Timer timer;
        private int seconds;
        private MP4SettingsDialog mp4SettingsDialog;

        public CameraControl(string VideoInputDevice)
        {
            InitializeComponent();
            VideoCapture1 = new VideoCaptureCore(videoView1);

            //VideoCapture1.SetLicenseKey("1E06-0EBA-44A5-7BFC-C5E1-0F56", "DownloadDevTools.com", "support@downloaddevtools.com");
            LinkVideos = new List<string>();
            this.VideoInputDevice = VideoInputDevice;

            InitUI();
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;

            seconds = 0;
        }

        private async void InitUI()
        {
            VideoInputFormat = VideoCapture1.Video_CaptureDevices()[0].VideoFormats[0].Name;
            VideoInputFrameRate = VideoCapture1.Video_CaptureDevices()[0].VideoFormats[0].FrameRates[0].ToString(CultureInfo.CurrentCulture);
            AudioInputDevice = VideoCapture1.Audio_CaptureDevices()[0].Name;
            AudioInputFormat = VideoCapture1.Audio_CaptureDevices()[0].Formats[0];
            AudioInputLine = VideoCapture1.Audio_CaptureDevices()[0].Lines[0];
            OutputFormat = "FFMPEG (external exe)";

            VideoCapture1.Video_Filters_Clear();
            SettingCaptureDevice(false);
            VideoCapture1.Mode = VideoCaptureMode.VideoPreview;

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

        private string CreateFilePath(bool IsCheckCapture)
        {
            string commonFolder = DicomDemoSettingsManager.GetFolderPath();
            string folderPath = Path.Combine(commonFolder, "BenhNhan");
            string filepath = Path.Combine(folderPath, $"{Guid.NewGuid()}.");
            if (IsCheckCapture)
            {
                switch (IndexOutputFormat)
                {
                    case 0:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".avi");
                            break;
                        }
                    case 1:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".mkv");
                            break;
                        }
                    case 2:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".wmv");
                            break;
                        }

                    case 3:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".avi");
                            break;
                        }
                    case 4:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".wav");
                            break;
                        }
                    case 5:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".mp3");
                            break;
                        }
                    case 6:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".m4a");
                            break;
                        }
                    case 7:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".wma");
                            break;
                        }
                    case 8:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".flac");
                            break;
                        }
                    case 9:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".ogg");
                            break;
                        }
                    case 10:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".ogg");
                            break;
                        }
                    case 11:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".avi");
                            break;
                        }
                    case 12:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".avi");
                            break;
                        }
                    case 13:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".avi");
                            break;
                        }
                    case 14:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".mpg");
                            break;
                        }
                    case 15:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".mkv");
                            break;
                        }
                    case 16:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".mp4");
                            break;
                        }
                    case 17:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".mp4");
                            break;
                        }
                    case 18:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".avi");
                            break;
                        }
                    case 19:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".webm");
                            break;
                        }
                    case 20:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".avi");
                            break;
                        }
                    case 21:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".avi");
                            break;
                        }
                    case 22:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".mp4");
                            break;
                        }
                    case 23:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".mp4");
                            break;
                        }
                    case 24:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".gif");
                            break;
                        }
                    case 25:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".enc");
                            break;
                        }
                    case 26:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".ts");
                            break;
                        }
                    case 27:
                        {
                            filepath = FilenameHelper.ChangeFileExt(filepath, ".mov");
                            break;
                        }
                }
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

        private void SetOutputFileName()
        {
            string filePath = CreateFilePath(true);
            string directoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            LinkVideos.Add(filePath);
            VideoCapture1.Output_Filename = filePath;
        }

        private void SettingCaptureDevice(bool isCheckCapture)
        {
            VideoCapture1.Video_Effects_Enabled = true;
            VideoCapture1.Video_CaptureDevice = new VideoCaptureSource(VideoInputDevice);
            VideoCapture1.Video_CaptureDevice.Format_UseBest = false;
            VideoCapture1.Video_CaptureDevice.Format = VideoInputFormat;
            VideoCapture1.Video_CaptureDevice.FrameRate = new VideoFrameRate(Convert.ToDouble(VideoInputFrameRate, CultureInfo.CurrentCulture));
            //if (cbVideoInputFrameRate.SelectedIndex != -1)
            //{
            //    VideoCapture1.Video_CaptureDevice.FrameRate = new VideoFrameRate(Convert.ToDouble(cbVideoInputFrameRate.Text, CultureInfo.CurrentCulture));
            //}

            VideoCapture1.Audio_CaptureDevice = new AudioCaptureSource(AudioInputDevice);
            VideoCapture1.Audio_CaptureDevice.Format = AudioInputFormat;
            VideoCapture1.Audio_CaptureDevice.Line = AudioInputLine;

            //VideoCapture1.Video_Renderer.Zoom_Ratio = 0;
            //VideoCapture1.Video_Renderer.Zoom_ShiftX = 0;
            //VideoCapture1.Video_Renderer.Zoom_ShiftY = 0;

            if (isCheckCapture)
            {
                SetOutputFileName();
                var mp4Output = new MP4Output();
                SetMP4Output(ref mp4Output);
                VideoCapture1.Output_Format = mp4Output;
            }

        }

        public void SettingCam()
        {
            VideoCapture1.Video_CaptureDevice_SettingsDialog_Show(IntPtr.Zero, VideoInputDevice);
        }


        public async void CameraControlRemoved()
        {
            await VideoCapture1.StopAsync();
        }

        public async Task SetVideoCapturePreviewAsync()
        {
            VideoCapture1.Video_Filters_Clear();
            await VideoCapture1.StopAsync();
            SettingCaptureDevice(false);
            VideoCapture1.Mode = VideoCaptureMode.VideoPreview;
            await VideoCapture1.StartAsync();
        }

        public async Task StartRecordAsync()
        {
            VideoCapture1.Video_Filters_Clear();
            await VideoCapture1.StopAsync();
            labelTime.Text = "00:00:00";
            timer.Start();
            VideoCapture1.Mode = VideoCaptureMode.VideoCapture;
            SettingCaptureDevice(true);
            VideoCapture1.StartAsync();
        }

        public async Task StopCaptureAsync()
        {
            timer.Stop();
            seconds = 0;
            labelTime.Text = "00:00:00";

            VideoCapture1.Video_Filters_Clear();
            SetVideoCapturePreviewAsync();
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

        public async void Snapshot()
        {
            string filePath = CreateFilePath(false);
            LinkImageSnapshot = filePath;
            await VideoCapture1.Frame_SaveAsync(filePath, ImageFormat.Jpeg, 80);
        }
    }
}
