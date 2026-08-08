using System;
using System.IO;
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
            if (!_thumbnailList.TryAddImage(imagePath, out item, false))
            {
                Log.Warning("Không nạp được ảnh vào danh sách thumbnail: {ImagePath}", imagePath);
                return;
            }

            Log.Information("Đã chụp và nạp ảnh: {ImagePath}", imagePath);
            SaveAttachmentManifestFromThumbnails();
            RequestPendingAttachmentUploadQueue();
        }

        private async void _btnSnapshot_Click(object sender, EventArgs e)
        {
            if (!CanEditConclusion())
                return;

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
                ApplyConclusionEditability();
            }
        }

        private void _btnAddFile_Click(object sender, EventArgs e)
        {
            if (!CanEditConclusion())
                return;

            try
            {
                using (var dlgOpen = new OpenFileDialog())
                {
                    dlgOpen.Title = "Chon hinh anh";
                    dlgOpen.Multiselect = true;
                    dlgOpen.Filter =
                        "Image files|*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff|" +
                        "All files|*.*";

                    Directory.CreateDirectory(_patientOrderFolder);
                    dlgOpen.InitialDirectory = Path.GetDirectoryName(_patientOrderFolder);

                    if (dlgOpen.ShowDialog(this) != DialogResult.OK)
                        return;

                    var addedCount = 0;
                    foreach (var fileName in dlgOpen.FileNames)
                    {
                        var localPath = ImportAttachmentFileToPatientFolder(fileName);

                        // scrollToEnd:false - thêm nhiều file cùng lúc, tự cuộn từng cái gây giật;
                        // cuộn 1 lần sau khi thêm hết.
                        ImageThumbnailList.ThumbnailItem item;
                        if (_thumbnailList.TryAddImage(localPath, out item, false, scrollToEnd: false))
                            addedCount++;
                        else
                            Log.Warning("Khong nap duoc file anh vao danh sach thumbnail: {FilePath}", localPath);
                    }

                    if (addedCount > 0)
                    {
                        _thumbnailList.ScrollToLastItem();
                        SaveAttachmentManifestFromThumbnails();
                        RequestPendingAttachmentUploadQueue();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Loi khi them file anh vao ket luan");
                MessageBox.Show(this, $"Loi khi them file anh: {ex.Message}", "Loi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ImportAttachmentFileToPatientFolder(string sourceFilePath)
        {
            if (string.IsNullOrWhiteSpace(sourceFilePath))
                throw new ArgumentException("Duong dan file rong.", nameof(sourceFilePath));

            Directory.CreateDirectory(_patientOrderFolder);

            var sourceFullPath = Path.GetFullPath(sourceFilePath);
            var targetFullPath = Path.GetFullPath(Path.Combine(
                _patientOrderFolder,
                Path.GetFileName(sourceFilePath)));

            if (string.Equals(sourceFullPath, targetFullPath, StringComparison.OrdinalIgnoreCase))
                return targetFullPath;

            targetFullPath = GetAvailableImportFilePath(targetFullPath);
            File.Copy(sourceFullPath, targetFullPath, false);
            return targetFullPath;
        }

        private static string GetAvailableImportFilePath(string desiredPath)
        {
            if (!File.Exists(desiredPath))
                return desiredPath;

            var folder = Path.GetDirectoryName(desiredPath);
            var name = Path.GetFileNameWithoutExtension(desiredPath);
            var extension = Path.GetExtension(desiredPath);

            for (var index = 1; index < 10000; index++)
            {
                var candidate = Path.Combine(folder, $"{name}_{index}{extension}");
                if (!File.Exists(candidate))
                    return candidate;
            }

            return Path.Combine(folder, $"{name}_{Guid.NewGuid():N}{extension}");
        }

        private async void _btnStop_Click(object sender, EventArgs e)
        {
            if (!CanEditConclusion())
                return;

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
                ApplyConclusionEditability();
            }
        }

        private async void _btnLinkCamera_Click(object sender, EventArgs e)
        {
            if (!CanEditConclusion())
                return;

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
                ApplyConclusionEditability();
            }
        }

    }
}
