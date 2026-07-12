using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;
using Leadtools;
using Leadtools.Demos;
using Leadtools.Forms.DocumentWriters;
using Leadtools.Codecs;
using Leadtools.Dicom;
using System.Net;
using System.Threading;
using Leadtools.Dicom.Common.Extensions;
using Leadtools.Dicom.Common.Editing;
using Leadtools.Dicom.Scu.Common;
using Leadtools.Dicom.Scu;
using System.Diagnostics;
using Leadtools.Dicom.Common.DataTypes.Modality;
using STM.MediaToPACS.Main.UI;
using Leadtools.DicomDemos;
using System.Collections.Generic;
using System.Collections;
using System.Management;
using Leadtools.WinForms.CommonDialogs.File;
using System.Reflection;
using Leadtools.Dicom.Common.Editing.Converters;
using Leadtools.ImageProcessing;
using Leadtools.Drawing;
using Leadtools.ImageProcessing.Effects;
using STM.MediaToPACS.Main.UI.CameraUI;
using Leadtools.Medical.Worklist.DataAccessLayer;
using Leadtools.Medical.DataAccessLayer;
using Leadtools.Medical.DataAccessLayer.Configuration;
using Leadtools.Medical.Worklist.DataAccessLayer.Configuration;
using Leadtools.Medical.Winforms;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
//using VisioForge.Core.VideoEdit; // VisioForge đã gỡ (thay bằng FlashCap)
using MediaToPacs.Core.Models;
using STM.MediaToPACS.Main.Utilities;
using DevExpress.XtraPdfViewer;
using System.Drawing.Printing;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraEditors;
using DevExpress.Pdf;
using MediaToPacs.Core.Models.Ketluan;
using DevExpress.XtraReports.UI;
using System.Text;
using MediaToPacs.Core.Enums;
using System.Xml.Serialization;
using Serilog;
using System.Configuration;
using System.Runtime.InteropServices;
using STM.MediaToPACS.Main.UI.Configurations;

namespace STM.MediaToPACS.Main
{
    public partial class FrmMain
    {
        #region Camera/Media Operations
        // Constants
        private const int VIDEO_FILE_MIN_SIZE = 1024; // 1KB
        private const int VIDEO_FILE_CHECK_RETRIES = 20;
        private const int VIDEO_FILE_CHECK_DELAY_MS = 100;
        private const int STOP_RECORD_DELAY_MS = 1000;
        private const int SNAPSHOT_DELAY_MS = 500;
        private const int THUMBNAIL_IMAGE_SIZE = 110;
        private const int THUMBNAIL_BORDER_THICKNESS = 2;
        private const int THUMBNAIL_MARGIN = 5;

        /// <summary>
        /// Xử lý sự kiện Record button - Chức năng quay video đã tắt (VisioForge đã thay bằng FlashCap, chỉ dùng preview + snapshot)
        /// </summary>
        private void _btnRecord_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show(this, "Chức năng quay video đã được tắt.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #region Code quay video cũ (VisioForge - đã tắt, chỉ dùng preview + snapshot)
        /*
        private async void _btnRecord_Click(object sender, EventArgs e)
        {
            try
            {
                _btnRecord.Enabled = false;
                if (IsCheckRecord)
                {
                    await StopRecordingAsync();
                }
                else
                {
                    await StartRecordingAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi thao tác ghi video");
                XtraMessageBox.Show(this, $"Lỗi khi ghi video: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _btnRecord.Enabled = true;
            }
        }

        /// <summary>
        /// Bắt đầu ghi video
        /// </summary>
        private async Task StartRecordingAsync()
        {
            try
            {
                Log.Information("Bắt đầu ghi video");

                string videoPath = await _cameraControl.StartRecordAsync(_machidinh);
                if (string.IsNullOrEmpty(videoPath))
                {
                    Log.Warning("Đường dẫn video rỗng");
                    return;
                }

                _listPathVideoRecords.Add(videoPath);
                IsCheckRecord = true;

                // Cập nhật UI
                _btnRecord.Text = "Dừng";

                Log.Information($"Đã bắt đầu ghi video: {videoPath}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi bắt đầu ghi video");
                throw;
            }
        }

        /// <summary>
        /// Dừng ghi video và tạo thumbnail
        /// </summary>
        private async Task StopRecordingAsync()
        {
            try
            {
                Log.Information("Dừng ghi video");

                // Dừng capture
                await _cameraControl.StopCaptureAsync();
                await Task.Delay(STOP_RECORD_DELAY_MS);

                // Lấy đường dẫn video cuối cùng
                string lastVideoPath = _listPathVideoRecords.LastOrDefault();
                if (string.IsNullOrEmpty(lastVideoPath))
                {
                    Log.Warning("Không tìm thấy đường dẫn video để xử lý");
                    UpdateRecordButtonUI(false);
                    IsCheckRecord = false;
                    return;
                }

                // Kiểm tra file video đã được ghi xong chưa
                bool fileReady = await WaitForVideoFileReadyAsync(lastVideoPath);
                if (!fileReady)
                {
                    Log.Warning($"File video chưa sẵn sàng: {lastVideoPath}");
                }

                // Tạo thumbnail
                await CreateVideoThumbnailAsync(lastVideoPath);

                // Xóa khỏi danh sách
                _listPathVideoRecords.Remove(lastVideoPath);

                // Cập nhật UI
                UpdateRecordButtonUI(false);
                IsCheckRecord = false;

                Log.Information($"Đã dừng ghi video: {lastVideoPath}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi dừng ghi video");
                throw;
            }
        }

        /// <summary>
        /// Đợi file video sẵn sàng (đã được ghi xong)
        /// </summary>
        private async Task<bool> WaitForVideoFileReadyAsync(string videoPath)
        {
            for (int i = 0; i < VIDEO_FILE_CHECK_RETRIES; i++)
            {
                try
                {
                    if (!File.Exists(videoPath))
                    {
                        await Task.Delay(VIDEO_FILE_CHECK_DELAY_MS);
                        continue;
                    }

                    using (var fs = new FileStream(videoPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        if (fs.Length > VIDEO_FILE_MIN_SIZE)
                        {
                            return true;
                        }
                    }
                }
                catch (IOException)
                {
                    // File đang được ghi, đợi thêm
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, $"Lỗi khi kiểm tra file video (lần {i + 1}): {videoPath}");
                }

                await Task.Delay(VIDEO_FILE_CHECK_DELAY_MS);
            }

            return false;
        }

        /// <summary>
        /// GetThumbnail từ video
        /// </summary>
        public async Task<Image> GetVideoThumbnailAsync(string videoPath)
        {
            if (!File.Exists(videoPath)) return null;

            using (var videoEdit = new VideoEditCore())
            {
                try
                {
                    await Task.Delay(300);

                    var bitmap = videoEdit.Helpful_GetFrameFromFile(
                        videoPath,
                        TimeSpan.FromSeconds(1),
                        false,
                        VisioForge.Core.Types.MediaPlayer.MediaPlayerSourceMode.FFMPEG
                    );

                    return bitmap;
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Lỗi khi lấy thumbnail video");
                    return null;
                }
            }
        }

        /// <summary>
        /// Tạo thumbnail cho video và thêm vào UI
        /// </summary>
        private async Task CreateVideoThumbnailAsync(string videoPath)
        {
            try
            {
                Image thumbnail = await GetVideoThumbnailAsync(videoPath);
                if (thumbnail == null)
                {
                    Log.Warning($"Không thể tạo thumbnail cho video: {videoPath}");
                    return;
                }

                // Tạo panel chứa thumbnail
                Panel containerPanel = CreateThumbnailPanel(thumbnail, videoPath);

                // Thêm vào UI (nếu có panel container)
                // _fPLRoll?.Controls.Add(containerPanel);

                Log.Information($"Đã tạo thumbnail cho video: {videoPath}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Lỗi khi tạo thumbnail cho video: {videoPath}");
            }
        }
        */
        #endregion

        /// <summary>
        /// Tạo panel chứa thumbnail
        /// </summary>
        private Panel CreateThumbnailPanel(Image thumbnail, string videoPath)
        {
            int containerSize = THUMBNAIL_IMAGE_SIZE + THUMBNAIL_BORDER_THICKNESS * 2;

            Panel containerPanel = new Panel
            {
                Size = new Size(containerSize, containerSize),
                BackColor = Color.Transparent,
                Margin = new Padding(THUMBNAIL_MARGIN),
                Padding = new Padding(THUMBNAIL_BORDER_THICKNESS),
                Tag = videoPath
            };

            PictureBox pictureBox = new PictureBox
            {
                Image = thumbnail,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Black,
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.None,
                Tag = videoPath,
                Size = new Size(THUMBNAIL_IMAGE_SIZE, THUMBNAIL_IMAGE_SIZE),
                Dock = DockStyle.Fill
            };

            pictureBox.Click += PictureBoxRoll_Click;
            containerPanel.Click += PictureBoxRoll_Click;

            containerPanel.Controls.Add(pictureBox);
            return containerPanel;
        }

        /// <summary>
        /// Cập nhật UI của nút Record
        /// </summary>
        private void UpdateRecordButtonUI(bool isRecording)
        {
            if (isRecording)
            {
                _btnRecord.Text = "Dừng";
            }
            else
            {
                _btnRecord.Text = "Ghi lại";
            }
        }

        /// <summary>
        /// Chụp ảnh từ camera
        /// </summary>
        private async Task TakeSnapshotAsync()
        {
            try
            {
                Log.Information($"Bắt đầu chụp ảnh cho folder: {_machidinh}");

                string imagePath = await _cameraControl.SnapshotAsync(_machidinh);
                if (string.IsNullOrEmpty(imagePath))
                {
                    Log.Warning("Đường dẫn ảnh chụp rỗng");
                    return;
                }

                // Đợi một chút để đảm bảo file đã được ghi xong
                await Task.Delay(SNAPSHOT_DELAY_MS);

                LoadRasterImage(imagePath);
                Log.Information($"Đã chụp và load ảnh: {imagePath}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi chụp ảnh");
                throw; // Re-throw để caller có thể xử lý
            }
        }

        /// <summary>
        /// Xử lý sự kiện Snapshot button
        /// </summary>
        private async void _btnSnapshot_Click(object sender, EventArgs e)
        {
            try
            {
                await TakeSnapshotAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi xử lý snapshot");
                XtraMessageBox.Show(this, $"Lỗi khi chụp ảnh: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        /// <summary>
        /// Xử lý sự kiện Stop button - Dừng camera
        /// </summary>
        private async void _btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                _btnStop.Enabled = false;
                await _cameraControl.StopCaptureAsync();
                Log.Information("Đã dừng camera");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi dừng camera");
                XtraMessageBox.Show(this, $"Lỗi khi dừng camera: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _btnStop.Enabled = true;
            }
        }


        /// <summary>
        /// Xử lý sự kiện Preview button - Xem trước camera
        /// </summary>
        private async void _btnLinkCamera_Click(object sender, EventArgs e)
        {
            try
            {
                _btnLinkCamera.Enabled = false;
                await _cameraControl.PreviewCaptureAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi xem trước camera");
                XtraMessageBox.Show(this, $"Lỗi khi xem trước: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _btnLinkCamera.Enabled = true;
            }
        }
        #endregion
    }
}
