using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serilog;

namespace STM.MediaToPACS.Main.UI.V2
{
    /// <summary>
    /// Xử lý camera cho FormMainV2 - chuyển thể từ FrmMain.Camera.cs, bỏ phần ghi video (VisioForge
    /// đã tắt trong bản gốc) và thay LoadRasterImage (Leadtools) bằng ImageThumbnailList.AddImage.
    /// </summary>
    public partial class FormMainV2
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

            // Đợi một chút để đảm bảo file đã được ghi xong
            await Task.Delay(SnapshotDelayMs);

            if (!_thumbnailList.AddImage(imagePath))
                Log.Warning("Không nạp được ảnh vào danh sách thumbnail: {ImagePath}", imagePath);
            else
                Log.Information("Đã chụp và nạp ảnh: {ImagePath}", imagePath);
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
    }
}
