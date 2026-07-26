using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serilog;

namespace STM.MediaToPACS.Main.UI.DiagnosticReports
{
    public partial class DiagnosticReportConclusionControl
    {
        private const int SnapshotDelayMs = 500;

        private async Task TakeSnapshotAsync()
        {
            Log.Information("Bắt đầu chụp ảnh cho folder: {MaChiDinh}", _machidinh);

            string imagePath = await _cameraControl.SnapshotAsync(_machidinh);
            if (string.IsNullOrEmpty(imagePath))
            {
                Log.Warning("Đường dẫn ảnh chụp rỗng");
                return;
            }

            await Task.Delay(SnapshotDelayMs);

            ImageThumbnailList.ThumbnailItem item;
            if (!_thumbnailList.TryAddImage(imagePath, out item, true))
            {
                Log.Warning("Không nạp được ảnh vào danh sách thumbnail: {ImagePath}", imagePath);
                return;
            }

            Log.Information("Đã chụp và nạp ảnh: {ImagePath}", imagePath);
            await TryUploadSnapshotAttachmentAsync(item);
        }

        private async void _btnSnapshot_Click(object sender, EventArgs e)
        {
            try
            {
                _btnSnapshot.Enabled = false;
                await TakeSnapshotAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi xử lý snapshot");
                MessageBox.Show(this, $"Lỗi khi chụp ảnh: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _btnSnapshot.Enabled = true;
            }
        }

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
                MessageBox.Show(this, $"Lỗi khi dừng camera: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _btnStop.Enabled = true;
            }
        }

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
                MessageBox.Show(this, $"Lỗi khi xem trước: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _btnLinkCamera.Enabled = true;
            }
        }

        private async void _btnPushPacs_Click(object sender, EventArgs e)
        {
            try
            {
                _btnPushPacs.Enabled = false;
                await PushSelectedPacsAttachmentsAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi đẩy ảnh PACS");
                MessageBox.Show(this, $"Lỗi khi đẩy PACS: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _btnPushPacs.Enabled = true;
            }
        }
    }
}
