using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using VisioForge.Core.MediaPlayer;      // VisioForge đã gỡ - chức năng phát video đã tắt
//using VisioForge.Core.Types.Events;
//using VisioForge.Core.Types.MediaPlayer;
//using VisioForge.Core.Types;

namespace STM.MediaToPACS.Main.UI.CameraUI
{
    /// <summary>
    /// Chức năng phát video đã tắt cùng với chức năng quay video
    /// (VisioForge đã thay bằng FlashCap - chỉ dùng camera preview + snapshot).
    /// Control được giữ lại làm vỏ để không phải sửa layout của FrmMain;
    /// code phát video cũ được comment ở cuối file.
    /// </summary>
    public partial class MediaPlayerControl : DevExpress.XtraEditors.XtraUserControl
    {
        private string FilePathVideo;

        public string SnapshotMedia { get; private set; }
        public event EventHandler SnapshotMedia_Click;
        public event EventHandler BackCamera_Click;

        public MediaPlayerControl(string FilePathVideo)
        {
            InitializeComponent();
            this.FilePathVideo = FilePathVideo;
        }

        public MediaPlayerControl()
        {
            InitializeComponent();
            UpdateToolbarButtons(1);
        }

        private void VideoEditor_Load(object sender, EventArgs e)
        {
        }

        private void UpdateToolbarButtons(int i)
        {
            bool enabled = i != 1;
            toolStripButtonPreviousFrame.Enabled = enabled;
            toolStripButtonNextFrame.Enabled = enabled;
            toolStripButtonPreviousKeyFrame.Enabled = enabled;
            toolStripButtonNextKeyFrame.Enabled = enabled;
            toolStripButtonFirstFrame.Enabled = enabled;
            toolStripButtonLastFrame.Enabled = enabled;
            toolStripButtonSnapshot.Enabled = enabled;
        }

        private void toolStripButtonPlay_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng phát video đã được tắt.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tbTimeline_Scroll(object sender, EventArgs e)
        {
        }

        private void toolStripButtonNextFrame_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButtonPreviousFrame_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButtonSnapshot_Click(object sender, EventArgs e)
        {
        }

        private void tbSpeed_Scroll(object sender, EventArgs e)
        {
        }

        private void toolStripButtonPreviousKeyFrame_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButtonNextKeyFrame_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButtonFirstFrame_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButtonLastFrame_Click(object sender, EventArgs e)
        {
        }

        public void SetFilePathMedia(string filePath)
        {
            FilePathVideo = filePath;
        }

        private void MediaPlayerControl_ControlRemoved(object sender, ControlEventArgs e)
        {
        }

        #region Code phát video cũ (VisioForge - đã tắt)
        /*
        private MediaPlayerCore MediaPlayer1;
        private Timer timer1;
        private int i = 1;

        private void VideoEditor_Load(object sender, EventArgs e)
        {
            MediaPlayer1 = new MediaPlayerCore(videoView1 as IVideoView);
            MediaPlayer1.SetLicenseKey("...", "...", "...");
            MediaPlayer1.OnError += MediaPlayer1_OnError;
            MediaPlayer1.OnStop += MediaPlayer1_OnStop;

            timer1 = new Timer();
            timer1.Interval = 1000;
            timer1.Tick += Timer_TickAsync;
        }

        private async void Timer_TickAsync(object sender, EventArgs e)
        {
            timer1.Tag = 1;
            tbTimeline.Maximum = (int)(await MediaPlayer1.Duration_TimeAsync()).TotalSeconds;

            int value = (int)(await MediaPlayer1.Position_Get_TimeAsync()).TotalSeconds;
            if ((value > 0) && (value < tbTimeline.Maximum))
            {
                tbTimeline.Value = value;
            }

            lbTimeline.Text = MediaPlayer1.Helpful_SecondsToTimeFormatted(tbTimeline.Value) + "/" + MediaPlayer1.Helpful_SecondsToTimeFormatted(tbTimeline.Maximum);

            timer1.Tag = 0;
        }

        private void MediaPlayer1_OnError(object sender, ErrorsEventArgs e)
        {
            Invoke((Action)(() =>
            {
                MessageBox.Show(e.Message + Environment.NewLine);
            }));
        }

        private void MediaPlayer1_OnStop(object sender, StopEventArgs e)
        {
        }

        private void toolStripButtonPlay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FilePathVideo) || !File.Exists(FilePathVideo))
            {
                MessageBox.Show("Vui lòng chọn video trước khi phát.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetupPlayVideoAsync();
        }

        private async Task SetupPlayVideoAsync()
        {
            if (i == 1)
            {
                MediaPlayer1.Source_Mode = MediaPlayerSourceMode.LAV;

                MediaPlayer1.Playlist_Clear();
                MediaPlayer1.Playlist_Add(FilePathVideo);

                MediaPlayer1.Loop = true;
                MediaPlayer1.Audio_PlayAudio = true;
                MediaPlayer1.Info_UseLibMediaInfo = true;
                MediaPlayer1.Audio_OutputDevice = "Default DirectSound Device";

                MediaPlayer1.Video_Renderer_SetAuto();

                MediaPlayer1.Debug_Mode = false;

                await MediaPlayer1.PlayAsync();

                timer1.Start();
                i = 2;
            }
            else if (i == 2)
            {
                i = 3;
                await MediaPlayer1.PauseAsync();
            }
            else if (i == 3)
            {
                i = 2;
                await MediaPlayer1.ResumeAsync();
            }
            UpdateToolbarButtons(i);
        }

        private async void tbTimeline_Scroll(object sender, EventArgs e)
        {
            if (Convert.ToInt32(timer1.Tag) == 0)
            {
                await MediaPlayer1.Position_Set_TimeAsync(TimeSpan.FromSeconds(tbTimeline.Value));
            }
        }

        private void toolStripButtonNextFrame_Click(object sender, EventArgs e)
        {
            MediaPlayer1.NextFrame();
            i = 3;
            UpdateToolbarButtons(i);
        }

        private void toolStripButtonPreviousFrame_Click(object sender, EventArgs e)
        {
            MediaPlayer1.PreviousFrame();
            i = 3;
            UpdateToolbarButtons(i);
        }

        private async void toolStripButtonSnapshot_Click(object sender, EventArgs e)
        {
            string filePath = $"D:\\Patient1\\{Guid.NewGuid()}.jpg";
            string directoryPath = Path.GetDirectoryName(filePath);
            SnapshotMedia = filePath;

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            await MediaPlayer1.Frame_SaveAsync(filePath, ImageFormat.Jpeg, 90);

            if (SnapshotMedia_Click != null)
            {
                SnapshotMedia_Click(this, EventArgs.Empty);
            }
        }

        private async void tbSpeed_Scroll(object sender, EventArgs e)
        {
            await MediaPlayer1.SetSpeedAsync(tbSpeed.Value / 4.0);
            lbSpeed.Text = (tbSpeed.Value / 4.0).ToString() + " x";
        }

        private int TimeToSeconds(string time)
        {
            string[] parts = time.Split(':');
            int hours = TimeToSeconds(parts[0]);
            int minutes = TimeToSeconds(parts[1]);
            int seconds = TimeToSeconds(parts[2]);

            return hours * 3600 + minutes * 60 + seconds;
        }

        private async void toolStripButtonPreviousKeyFrame_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(timer1.Tag) == 0)
            {
                if (tbTimeline.Value - tbTimeline.Maximum / 15 > 0)
                {
                    await MediaPlayer1.Position_Set_TimeAsync(TimeSpan.FromSeconds(tbTimeline.Value - tbTimeline.Maximum / 15));
                }
            }
        }

        private async void toolStripButtonNextKeyFrame_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(timer1.Tag) == 0)
            {
                if (tbTimeline.Value + tbTimeline.Maximum / 15 <= tbTimeline.Maximum)
                {
                    await MediaPlayer1.Position_Set_TimeAsync(TimeSpan.FromSeconds(tbTimeline.Value + tbTimeline.Maximum / 15));
                }
            }
        }

        private void DestroyEngine()
        {
            if (MediaPlayer1 != null)
            {
                MediaPlayer1.OnError -= MediaPlayer1_OnError;
                MediaPlayer1.OnStop -= MediaPlayer1_OnStop;

                MediaPlayer1.Dispose();
                MediaPlayer1 = null;
            }
        }

        private async Task StopAsync()
        {
            timer1.Stop();

            await MediaPlayer1.StopAsync();

            tbTimeline.Value = 0;
        }

        private async void VideoEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            await StopAsync();
            DestroyEngine();
        }
        */
        #endregion
    }
}
