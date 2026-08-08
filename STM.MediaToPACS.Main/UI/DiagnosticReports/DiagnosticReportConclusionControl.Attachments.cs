using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaToPacs.Core.Models;
using Serilog;
using STM.MediaToPACS.Main.Utilities;

namespace STM.MediaToPACS.Main.UI.DiagnosticReports
{
    public partial class DiagnosticReportConclusionControl
    {
        private Guid? GetCurrentRisV1OrderItemId()
        {
            return _risV1OrderItem != null && _risV1OrderItem.id != Guid.Empty
                ? (Guid?)_risV1OrderItem.id
                : null;
        }

        private async Task LoadReportAttachmentsSafeAsync()
        {
            try
            {
                SetAttachmentLoadStatus(true);
                _attachmentManifest = await Task.Run(() =>
                    DiagnosticReportAttachmentManifest.Load(_patientOrderFolder, _machidinh));
                var loadedFromServer = false;

                var orderItemId = GetCurrentRisV1OrderItemId();
                if (orderItemId.HasValue)
                {
                    var attachments = await ServiceLocator.RisService2
                        .GetDiagnosticReportAttachmentsAsync(orderItemId.Value);

                    if (attachments != null && attachments.Count > 0)
                    {
                        ClearAttachmentThumbnailsForReload();
                        _suppressAttachmentManifestSave = true;
                        try
                        {
                            foreach (var attachment in attachments)
                                await AddServerAttachmentThumbnailAsync(orderItemId.Value, attachment);
                        }
                        finally
                        {
                            _suppressAttachmentManifestSave = false;
                        }

                        LoadLocalAttachmentManifestThumbnails(includeUploaded: false);
                        SaveAttachmentManifestFromThumbnails();
                        loadedFromServer = true;
                    }
                }

                if (!loadedFromServer)
                {
                    ClearAttachmentThumbnailsForReload();
                    LoadLocalAttachmentManifestThumbnails(includeUploaded: true);
                    SaveAttachmentManifestFromThumbnails();
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không load được danh sách ảnh attachment của phiếu kết luận");
            }
            finally
            {
                if (_thumbnailList.Items.Count == 0)
                {
                    ClearAttachmentThumbnailsForReload();
                    LoadLocalAttachmentManifestThumbnails(includeUploaded: true);
                    SaveAttachmentManifestFromThumbnails();
                }

                SetAttachmentLoadStatus(false);
            }
        }

        private void ClearAttachmentThumbnailsForReload()
        {
            _suppressAttachmentManifestSave = true;
            try
            {
                _thumbnailList.ClearImages();
            }
            finally
            {
                _suppressAttachmentManifestSave = false;
            }
        }

        private void LoadLocalAttachmentManifestThumbnails(bool includeUploaded)
        {
            if (_attachmentManifest == null || _attachmentManifest.items == null)
                return;

            _suppressAttachmentManifestSave = true;
            try
            {
                var localItems = _attachmentManifest.items.OrderBy(i => i.createdAt).ToList();
                foreach (var manifestItem in localItems)
                {
                    if (!includeUploaded && manifestItem.attachmentId.HasValue)
                        continue;

                    if (string.IsNullOrWhiteSpace(manifestItem.filePath) || !File.Exists(manifestItem.filePath))
                        continue;

                    // scrollToEnd:false - thêm hàng loạt từ manifest, tự cuộn từng ảnh giữa chừng
                    // gây giật/lệch vị trí cuộn cuối cùng; cuộn 1 lần duy nhất sau khi nạp xong.
                    ImageThumbnailList.ThumbnailItem item;
                    if (!_thumbnailList.TryAddImage(manifestItem.filePath, out item, manifestItem.documentSelected, scrollToEnd: false))
                        continue;

                    if (manifestItem.attachmentId.HasValue)
                    {
                        _thumbnailList.SetAttachmentMetadata(
                            item,
                            manifestItem.attachmentId.Value,
                            manifestItem.fileName,
                            manifestItem.contentType);
                    }

                    _thumbnailList.SetDocumentSelected(item, manifestItem.documentSelected);
                    _thumbnailList.SetDocPushed(item, manifestItem.docPushed);
                    _thumbnailList.SetPacsSelected(item, manifestItem.pacsSelected);
                    _thumbnailList.SetPacsStatus(item, manifestItem.pacsStatus, manifestItem.errorDetail);
                }

                _thumbnailList.ScrollToLastItem();
            }
            finally
            {
                _suppressAttachmentManifestSave = false;
            }
        }

        private async Task ReloadAttachmentManifestFromDiskSafeAsync()
        {
            try
            {
                var manifest = await Task.Run(() =>
                    DiagnosticReportAttachmentManifest.Load(_patientOrderFolder, _machidinh));

                _attachmentManifest = manifest;
                ClearAttachmentThumbnailsForReload();
                LoadLocalAttachmentManifestThumbnails(includeUploaded: true);
                SaveAttachmentManifestFromThumbnails();
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Khong quet lai duoc manifest attachment local");
            }
        }

        private void SaveAttachmentManifestFromThumbnails()
        {
            if (_suppressAttachmentManifestSave)
                return;

            if (_attachmentManifest == null)
                _attachmentManifest = new DiagnosticReportAttachmentManifest { maChiDinh = _machidinh };

            var nextItems = new List<DiagnosticReportAttachmentManifestItem>();
            foreach (var thumbnail in _thumbnailList.Items)
            {
                if (string.IsNullOrWhiteSpace(thumbnail.FilePath))
                    continue;

                var existing = _attachmentManifest.FindByFilePath(thumbnail.FilePath)
                               ?? (thumbnail.AttachmentId.HasValue
                                   ? _attachmentManifest.FindByAttachmentId(thumbnail.AttachmentId.Value)
                                   : null);

                var manifestItem = existing ?? new DiagnosticReportAttachmentManifestItem
                {
                    localId = Guid.NewGuid(),
                    createdAt = DateTimeOffset.Now
                };

                manifestItem.filePath = thumbnail.FilePath;
                manifestItem.fileName = thumbnail.FileName;
                manifestItem.contentType = thumbnail.ContentType;
                manifestItem.attachmentId = thumbnail.AttachmentId;
                manifestItem.documentSelected = thumbnail.DocumentSelected || thumbnail.Checked;
                manifestItem.docPushed = thumbnail.DocPushed;
                manifestItem.pacsSelected = thumbnail.PacsSelected;
                manifestItem.uploaded = thumbnail.AttachmentId.HasValue;
                manifestItem.pacsStatus = thumbnail.PacsStatus;
                manifestItem.errorDetail = thumbnail.ErrorDetail;
                manifestItem.uploadStatus = thumbnail.AttachmentId.HasValue
                    ? LocalAttachmentUploadStatuses.Uploaded
                    : (string.IsNullOrWhiteSpace(manifestItem.uploadStatus)
                        ? LocalAttachmentUploadStatuses.Pending
                        : manifestItem.uploadStatus);

                if (thumbnail.AttachmentId.HasValue && !manifestItem.uploadedAt.HasValue)
                    manifestItem.uploadedAt = DateTimeOffset.Now;

                nextItems.Add(manifestItem);
            }

            _attachmentManifest.maChiDinh = _machidinh;
            _attachmentManifest.items = nextItems;
            _attachmentManifest.Save(_patientOrderFolder);
        }

        private async void ThumbnailList_DeleteRequested(object sender, ImageThumbnailList.ThumbnailItem item)
        {
            if (!CanEditConclusion() || item == null)
                return;

            var fileName = string.IsNullOrWhiteSpace(item.FileName)
                ? Path.GetFileName(item.FilePath)
                : item.FileName;
            var confirm = MessageBox.Show(
                this,
                $"Anh '{fileName}' se bi xoa vinh vien khoi may nay. Ban co muon xoa khong?",
                "Xac nhan xoa anh",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                var attachmentId = item.AttachmentId;
                await DeleteServerAttachmentIfAnyAsync(attachmentId);

                var filePath = item.FilePath;
                if (_thumbnailList.RemoveItem(item))
                {
                    DeleteLocalAttachmentFileSafe(filePath);
                    SaveAttachmentManifestFromThumbnails();
                    await SyncAttachmentSelectionsAfterDeleteSafeAsync(attachmentId);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Loi khi xoa anh attachment");
                MessageBox.Show(this, $"Khong xoa duoc anh: {ex.Message}", "Loi xoa anh",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ThumbnailList_DeleteAllRequested(object sender, EventArgs e)
        {
            if (!CanEditConclusion() || _thumbnailList.Items.Count == 0)
                return;

            var confirm = MessageBox.Show(
                this,
                "Tat ca anh dang hien thi se bi xoa vinh vien khoi may nay. Ban co muon xoa khong?",
                "Xac nhan xoa tat ca anh",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                var attachmentIds = _thumbnailList.Items
                    .Where(item => item.AttachmentId.HasValue)
                    .Select(item => item.AttachmentId.Value)
                    .ToList();
                var filePaths = _thumbnailList.Items
                    .Select(item => item.FilePath)
                    .Where(path => !string.IsNullOrWhiteSpace(path))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                foreach (var attachmentId in attachmentIds)
                    await DeleteServerAttachmentIfAnyAsync(attachmentId);

                _thumbnailList.ClearAll();
                foreach (var filePath in filePaths)
                    DeleteLocalAttachmentFileSafe(filePath);

                SaveAttachmentManifestFromThumbnails();
                await SyncAttachmentSelectionsAfterDeleteSafeAsync(null);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Loi khi xoa tat ca anh attachment");
                MessageBox.Show(this, $"Khong xoa duoc tat ca anh: {ex.Message}", "Loi xoa anh",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteLocalAttachmentFileSafe(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return;

            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Khong xoa duoc file anh local: {FilePath}", filePath);
            }
        }

        private async Task SyncAttachmentSelectionsAfterDeleteSafeAsync(Guid? deletedAttachmentId)
        {
            try
            {
                var orderItemId = GetCurrentRisV1OrderItemId();
                if (!orderItemId.HasValue)
                    return;

                await SyncDocumentAttachmentSelectionAsync(orderItemId.Value);
                await ServiceLocator.RisService2
                    .UpdatePacsAttachmentSelectionAsync(orderItemId.Value, _thumbnailList.GetPacsSelectedAttachmentIds());
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Khong dong bo duoc lua chon attachment sau khi xoa anh: {AttachmentId}", deletedAttachmentId);
            }
        }

        private async Task DeleteServerAttachmentIfAnyAsync(Guid? attachmentId)
        {
            if (!attachmentId.HasValue)
                return;

            var orderItemId = GetCurrentRisV1OrderItemId();
            if (!orderItemId.HasValue)
                return;

            await ServiceLocator.RisService2
                .DeleteDiagnosticReportAttachmentAsync(orderItemId.Value, attachmentId.Value);
        }

        private void RequestPendingAttachmentUploadQueue()
        {
            _attachmentQueueRequested = true;
            if (_isAttachmentQueueProcessing)
                return;

            _ = ProcessPendingAttachmentUploadQueueSafeAsync();
        }

        private async Task ProcessPendingAttachmentUploadQueueSafeAsync()
        {
            if (_isAttachmentQueueProcessing)
                return;

            _isAttachmentQueueProcessing = true;
            try
            {
                do
                {
                    _attachmentQueueRequested = false;

                    await ReloadAttachmentManifestFromDiskSafeAsync();

                    var orderItemId = await EnsureCurrentOrderItemIdForAttachmentSyncAsync();
                    if (!orderItemId.HasValue)
                    {
                        Log.Information("Chua co orderItemId RIS moi, giu anh attachment local o trang thai pending.");
                        return;
                    }

                    await UploadPendingAttachmentsAsync(orderItemId.Value);
                    await SyncDocumentAttachmentSelectionAsync(orderItemId.Value);
                    await SyncPacsAttachmentSelectionIfAnyAsync(orderItemId.Value);
                }
                while (_attachmentQueueRequested);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Xu ly queue upload attachment pending that bai");
            }
            finally
            {
                _isAttachmentQueueProcessing = false;
            }
        }

        private async Task AddServerAttachmentThumbnailAsync(
            Guid orderItemId,
            DiagnosticReportAttachmentDto attachment)
        {
            if (attachment == null || attachment.id == Guid.Empty)
                return;

            var existingItem = FindExistingThumbnailForServerAttachment(orderItemId, attachment);
            if (existingItem != null)
            {
                _thumbnailList.SetAttachmentMetadata(
                    existingItem,
                    attachment.id,
                    attachment.fileName,
                    attachment.contentType);
                ApplyAttachmentSelectionState(existingItem, attachment);
                SaveAttachmentManifestFromThumbnails();
                return;
            }

            var localPath = await EnsureAttachmentPreviewFileAsync(orderItemId, attachment);
            if (string.IsNullOrWhiteSpace(localPath))
                return;

            // scrollToEnd:false - hàm này được gọi lặp cho từng attachment trong LoadReportAttachmentsSafeAsync,
            // tự cuộn giữa chừng mỗi lần gây giật/lệch cuộn cuối cùng.
            ImageThumbnailList.ThumbnailItem item;
            if (!_thumbnailList.TryAddImage(localPath, out item, HasDocumentDestination(attachment), scrollToEnd: false))
                return;

            _thumbnailList.SetAttachmentMetadata(
                item,
                attachment.id,
                attachment.fileName,
                attachment.contentType);

            ApplyAttachmentSelectionState(item, attachment);
            SaveAttachmentManifestFromThumbnails();
        }

        private ImageThumbnailList.ThumbnailItem FindExistingThumbnailForServerAttachment(
            Guid orderItemId,
            DiagnosticReportAttachmentDto attachment)
        {
            var byAttachmentId = _thumbnailList.FindByAttachmentId(attachment.id);
            if (byAttachmentId != null)
                return byAttachmentId;

            var manifestItem = FindManifestItemForServerAttachment(orderItemId, attachment);
            if (manifestItem != null &&
                !string.IsNullOrWhiteSpace(manifestItem.filePath) &&
                File.Exists(manifestItem.filePath))
            {
                return _thumbnailList.Items.FirstOrDefault(item =>
                    string.Equals(NormalizePath(item.FilePath), NormalizePath(manifestItem.filePath), StringComparison.OrdinalIgnoreCase));
            }

            var previewPath = GetAttachmentPreviewFilePath(attachment);
            var byPreviewPath = _thumbnailList.Items.FirstOrDefault(item =>
                string.Equals(NormalizePath(item.FilePath), NormalizePath(previewPath), StringComparison.OrdinalIgnoreCase));
            if (byPreviewPath != null)
                return byPreviewPath;

            if (!string.IsNullOrWhiteSpace(attachment.fileName))
            {
                return _thumbnailList.Items.FirstOrDefault(item =>
                    string.Equals(item.FileName, attachment.fileName, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(Path.GetFileName(item.FilePath), attachment.fileName, StringComparison.OrdinalIgnoreCase));
            }

            return null;
        }

        private DiagnosticReportAttachmentManifestItem FindManifestItemForServerAttachment(
            Guid orderItemId,
            DiagnosticReportAttachmentDto attachment)
        {
            if (_attachmentManifest == null || _attachmentManifest.items == null || attachment == null)
                return null;

            var byAttachmentId = _attachmentManifest.FindByAttachmentId(attachment.id);
            if (byAttachmentId != null)
                return byAttachmentId;

            var previewPath = GetAttachmentPreviewFilePath(attachment);
            var byPreviewPath = _attachmentManifest.FindByFilePath(previewPath);
            if (byPreviewPath != null)
                return byPreviewPath;

            if (!string.IsNullOrWhiteSpace(attachment.fileName))
            {
                return _attachmentManifest.items.FirstOrDefault(item =>
                    string.Equals(item.fileName, attachment.fileName, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(Path.GetFileName(item.filePath), attachment.fileName, StringComparison.OrdinalIgnoreCase));
            }

            return null;
        }

        // Trạng thái Document/PACS của ảnh đã upload lấy hoàn toàn theo API (bật/tắt đúng theo
        // destination trả về) - không giữ lại trạng thái local cũ. Nơi gọi hàm này phải đảm bảo lựa
        // chọn Document/PACS hiện tại đã được đồng bộ lên server trước đó (xem SaveAndPushSelectedPacsAttachmentsAsync).
        private void ApplyAttachmentSelectionState(
            ImageThumbnailList.ThumbnailItem item,
            DiagnosticReportAttachmentDto attachment)
        {
            if (item == null)
                return;

            var documentDestination = attachment?.destinations?.FirstOrDefault(d =>
                string.Equals(d.destinationType, DiagnosticReportAttachmentDestinationTypes.Document, StringComparison.OrdinalIgnoreCase));

            _thumbnailList.SetDocumentSelected(item, documentDestination != null);
            _thumbnailList.SetDocPushed(item, documentDestination != null);

            var pacsDestination = attachment?.destinations?.FirstOrDefault(d =>
                string.Equals(d.destinationType, DiagnosticReportAttachmentDestinationTypes.Pacs, StringComparison.OrdinalIgnoreCase));

            _thumbnailList.SetPacsSelected(item, pacsDestination != null);
            _thumbnailList.SetPacsStatus(item, pacsDestination?.status, pacsDestination?.ErrorMessage);
        }

        private static bool HasDocumentDestination(DiagnosticReportAttachmentDto attachment)
        {
            return attachment != null &&
                   attachment.destinations != null &&
                   attachment.destinations.Any(d =>
                       string.Equals(d.destinationType, DiagnosticReportAttachmentDestinationTypes.Document, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<string> EnsureAttachmentPreviewFileAsync(
            Guid orderItemId,
            DiagnosticReportAttachmentDto attachment)
        {
            var localPath = GetAttachmentPreviewFilePath(attachment);
            Directory.CreateDirectory(Path.GetDirectoryName(localPath));

            if (File.Exists(localPath) && new FileInfo(localPath).Length > 0)
                return localPath;

            using (var stream = await ServiceLocator.RisService2
                       .StreamDiagnosticReportAttachmentAsync(orderItemId, attachment.id))
            using (var file = File.Create(localPath))
            {
                await stream.CopyToAsync(file);
            }

            return localPath;
        }

        private string GetAttachmentPreviewFilePath(DiagnosticReportAttachmentDto attachment)
        {
            var extension = GuessExtension(attachment.contentType, attachment.fileName);
            var folder = Path.Combine(_baseFolder, "Patient", _machidinh, "Attachments");
            var fileName = attachment.id.ToString("D") + extension;
            return Path.Combine(folder, fileName);
        }

        private static string NormalizePath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return string.Empty;

            try
            {
                return Path.GetFullPath(filePath);
            }
            catch
            {
                return filePath;
            }
        }

        private static string GuessExtension(string contentType, string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var ext = Path.GetExtension(fileName);
                if (!string.IsNullOrWhiteSpace(ext))
                    return ext;
            }

            switch ((contentType ?? string.Empty).ToLowerInvariant())
            {
                case "image/jpeg":
                    return ".jpg";
                case "image/png":
                    return ".png";
                case "image/gif":
                    return ".gif";
                case "image/bmp":
                    return ".bmp";
                case "image/webp":
                    return ".webp";
                case "video/mp4":
                    return ".mp4";
                case "video/mpeg":
                    return ".mpeg";
                default:
                    return ".bin";
            }
        }

        private async Task TryUploadSnapshotAttachmentAsync(ImageThumbnailList.ThumbnailItem item)
        {
            try
            {
                var orderItemId = GetCurrentRisV1OrderItemId();
                if (!orderItemId.HasValue || item == null)
                    return;

                await UploadAttachmentItemAsync(orderItemId.Value, item);
                await SyncDocumentAttachmentSelectionAsync(orderItemId.Value);
                await SyncPacsAttachmentSelectionIfAnyAsync(orderItemId.Value);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không upload được ảnh snapshot lên attachment ngay sau khi chụp");
            }
        }

        private async Task UploadPendingAttachmentsAndSyncDocumentSelectionAsync()
        {
            var orderItemId = await EnsureCurrentOrderItemIdForAttachmentSyncAsync();
            if (!orderItemId.HasValue)
                throw new InvalidOperationException("Chua co orderItemId RIS moi de upload attachment.");

            await UploadPendingAttachmentsAsync(orderItemId.Value);
            await SyncDocumentAttachmentSelectionAsync(orderItemId.Value);
            await SyncPacsAttachmentSelectionIfAnyAsync(orderItemId.Value);
        }

        private async Task<bool> UploadPendingAttachmentsAndSyncDocumentSelectionSafeAsync()
        {
            try
            {
                await UploadPendingAttachmentsAndSyncDocumentSelectionAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không đồng bộ được ảnh attachment/document-selection sau khi lưu kết luận");
            }
            return false;
        }

        private async Task<Guid?> EnsureCurrentOrderItemIdForAttachmentSyncAsync()
        {
            var orderItemId = GetCurrentRisV1OrderItemId();
            if (orderItemId.HasValue)
                return orderItemId;

            await ResolveRisV1OrderItemAsync();
            return GetCurrentRisV1OrderItemId();
        }

        private async Task UploadPendingAttachmentsAsync(Guid orderItemId)
        {
            var pendingItems = _thumbnailList.GetPendingUploadItems();
            if (pendingItems.Count == 0)
                return;

            var uploadItems = pendingItems
                .Where(i => File.Exists(i.FilePath))
                .ToList();

            if (uploadItems.Count == 0)
                return;

            var failedCount = 0;
            foreach (var item in uploadItems)
            {
                try
                {
                    await UploadAttachmentItemAsync(orderItemId, item);
                }
                catch (Exception ex)
                {
                    failedCount++;
                    Log.Warning(ex, "Upload attachment pending that bai: {FilePath}", item.FilePath);
                }
            }

            if (failedCount > 0)
                throw new InvalidOperationException($"Con {failedCount} anh chua upload duoc len RIS.");
        }

        private async Task UploadAttachmentItemAsync(
            Guid orderItemId,
            ImageThumbnailList.ThumbnailItem item)
        {
            if (item == null || item.AttachmentId.HasValue)
                return;

            if (string.IsNullOrWhiteSpace(item.FilePath) || !File.Exists(item.FilePath))
                return;

            var manifestItem = _attachmentManifest?.FindByFilePath(item.FilePath);
            if (manifestItem != null)
            {
                manifestItem.uploaded = false;
                manifestItem.uploadStatus = LocalAttachmentUploadStatuses.Uploading;
                manifestItem.lastAttemptAt = DateTimeOffset.Now;
                manifestItem.errorDetail = null;
                _attachmentManifest.Save(_patientOrderFolder);
            }

            try
            {
                var uploaded = await ServiceLocator.RisService2
                    .UploadDiagnosticReportAttachmentsAsync(orderItemId, new[] { item.FilePath });

                var attachment = uploaded?.FirstOrDefault();
                if (attachment == null || attachment.id == Guid.Empty)
                {
                    manifestItem = _attachmentManifest?.FindByFilePath(item.FilePath);
                    if (manifestItem != null)
                    {
                        manifestItem.uploaded = false;
                        manifestItem.uploadStatus = LocalAttachmentUploadStatuses.Failed;
                        manifestItem.errorDetail = "API khong tra ve attachment id";
                        _attachmentManifest.Save(_patientOrderFolder);
                    }
                    throw new InvalidOperationException("API khong tra ve attachment id.");
                }

                _thumbnailList.SetAttachmentMetadata(
                    item,
                    attachment.id,
                    attachment.fileName,
                    attachment.contentType);
                SaveAttachmentManifestFromThumbnails();

                manifestItem = _attachmentManifest?.FindByFilePath(item.FilePath);
                if (manifestItem != null)
                {
                    manifestItem.attachmentId = attachment.id;
                    manifestItem.fileName = attachment.fileName;
                    manifestItem.contentType = attachment.contentType;
                    manifestItem.uploaded = true;
                    manifestItem.uploadStatus = LocalAttachmentUploadStatuses.Uploaded;
                    manifestItem.uploadedAt = DateTimeOffset.Now;
                    manifestItem.errorDetail = null;
                    _attachmentManifest.Save(_patientOrderFolder);
                }
            }
            catch (Exception ex)
            {
                manifestItem = _attachmentManifest?.FindByFilePath(item.FilePath);
                if (manifestItem != null)
                {
                    manifestItem.uploadStatus = LocalAttachmentUploadStatuses.Failed;
                    manifestItem.uploaded = false;
                    manifestItem.lastAttemptAt = DateTimeOffset.Now;
                    manifestItem.errorDetail = ex.Message;
                    _attachmentManifest.Save(_patientOrderFolder);
                }

                throw;
            }
        }

        private async Task SyncDocumentAttachmentSelectionAsync(Guid orderItemId)
        {
            var selections = _thumbnailList.GetDocumentSelectedItems()
                .Where(i => i.AttachmentId.HasValue)
                .Select((item, index) => new DocumentAttachmentSelectionItem
                {
                    attachmentId = item.AttachmentId.Value,
                    sortOrder = index
                })
                .ToList();

            await ServiceLocator.RisService2
                .UpdateDocumentAttachmentSelectionAsync(orderItemId, selections);

            // Đồng bộ thành công mới dán badge "D" - không tự ăn theo checkbox đang tick.
            _thumbnailList.ApplyDocPushResult(selections.Select(s => s.attachmentId));
        }

        private async Task SyncPacsAttachmentSelectionIfAnyAsync(Guid orderItemId)
        {
            var pacsAttachmentIds = _thumbnailList.GetPacsSelectedAttachmentIds();
            if (pacsAttachmentIds.Count == 0)
                return;

            await ServiceLocator.RisService2
                .UpdatePacsAttachmentSelectionAsync(orderItemId, pacsAttachmentIds);
        }

        /// <summary>"Đẩy PACS": gán PACS theo đúng ảnh JPEG đang tích chọn TẠI THỜI ĐIỂM BẤM (không
        /// tính lịch sử tick trước đó), upload ảnh còn thiếu, lưu danh sách lên server rồi gửi thật
        /// sang hệ thống PACS - gộp chung 1 bước, không còn tách "Lưu PACS" riêng.</summary>
        private async void _btnPushPacs_Click(object sender, EventArgs e)
        {
            if (!CanEditConclusion())
                return;

            try
            {
                _btnPushPacs.Enabled = false;
                await SaveAndPushSelectedPacsAttachmentsAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi đẩy ảnh PACS");
                MessageBox.Show(this, $"Lỗi khi đẩy PACS: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ApplyConclusionEditability();
            }
        }

        private async Task SaveAndPushSelectedPacsAttachmentsAsync()
        {
            await ReloadAttachmentManifestFromDiskSafeAsync();

            var orderItemId = await EnsureCurrentOrderItemIdForAttachmentSyncAsync();
            if (!orderItemId.HasValue)
            {
                // Không nêu tên hệ thống/API phía sau - người dùng chỉ cần biết chưa đẩy PACS được.
                Log.Warning("Chua tim thay y lenh RIS moi de day anh PACS. MaChiDinh={MaChiDinh}", _machidinh);
                MessageBox.Show(
                    this,
                    "Chưa đẩy được ảnh lên PACS cho chỉ định này. Vui lòng thử lại sau.",
                    "Chưa đẩy được PACS",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Danh sách PACS luôn đồng bộ đúng theo checkbox đang tick ngay lúc bấm (ảnh JPEG tick
            // thì vào, không tick thì loại ra), không cộng dồn từ trước.
            _thumbnailList.SyncPacsSelectionWithCheckedItems();

            await UploadPendingAttachmentsAsync(orderItemId.Value);

            var pacsAttachmentIds = _thumbnailList.GetPacsSelectedAttachmentIds();
            if (pacsAttachmentIds.Count == 0)
            {
                MessageBox.Show(
                    this,
                    "Chưa có ảnh JPEG nào được tick chọn cho PACS. Hãy tick chọn ảnh JPEG cần đẩy trước.",
                    "Chưa chọn ảnh PACS",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            await ServiceLocator.RisService2
                .UpdatePacsAttachmentSelectionAsync(orderItemId.Value, pacsAttachmentIds);

            var result = await ServiceLocator.RisService2
                .PushDiagnosticReportAttachmentsToPacsAsync(orderItemId.Value);

            // Chỉ đọc lại đúng phần PACS theo API - KHÔNG đụng tới Document, vì Document là lựa chọn
            // độc lập, chỉ đồng bộ lên server khi "Lưu Nháp", bấm PACS không được tự ý đẩy/đổi nó.
            await RefreshPacsAttachmentStatusesAsync(orderItemId.Value);

            var message = result == null
                ? $"Đã lưu và gửi yêu cầu đẩy PACS cho {pacsAttachmentIds.Count} ảnh."
                : $"Đẩy PACS hoàn tất.\nThành công: {result.pushed}\nĐã hoàn thành từ trước: {result.alreadyCompleted}\nThất bại: {result.failed}";

            MessageBox.Show(
                this,
                message,
                "Đẩy PACS",
                MessageBoxButtons.OK,
                result != null && result.failed > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
        }

        // Đọc lại đúng phần PACS (SetPacsSelected/SetPacsStatus) theo API sau khi Lưu/Đẩy PACS -
        // KHÔNG đụng tới Document, vì Document là lựa chọn độc lập, chỉ đồng bộ lên server lúc
        // "Lưu Nháp". attachments == null nghĩa là request thất bại nên giữ nguyên UI hiện tại.
        private async Task RefreshPacsAttachmentStatusesAsync(Guid orderItemId)
        {
            var attachments = await ServiceLocator.RisService2
                .GetDiagnosticReportAttachmentsAsync(orderItemId);

            if (attachments == null)
                return;

            foreach (var item in _thumbnailList.Items)
            {
                if (!item.AttachmentId.HasValue)
                    continue;

                var attachment = attachments.FirstOrDefault(a => a.id == item.AttachmentId.Value);
                var pacsDestination = attachment?.destinations?.FirstOrDefault(d =>
                    string.Equals(d.destinationType, DiagnosticReportAttachmentDestinationTypes.Pacs, StringComparison.OrdinalIgnoreCase));

                _thumbnailList.SetPacsSelected(item, pacsDestination != null);
                _thumbnailList.SetPacsStatus(item, pacsDestination?.status, pacsDestination?.ErrorMessage);
            }

            SaveAttachmentManifestFromThumbnails();
        }
    }
}
