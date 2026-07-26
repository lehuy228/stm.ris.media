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
        private Guid? GetCurrentDiagnosticReportId()
        {
            if (_risV1OrderItem == null || _risV1OrderItem.report == null)
                return null;

            return _risV1OrderItem.report.id == Guid.Empty
                ? (Guid?)null
                : _risV1OrderItem.report.id;
        }

        private async Task LoadReportAttachmentsSafeAsync()
        {
            try
            {
                var reportId = GetCurrentDiagnosticReportId();
                if (!reportId.HasValue)
                    return;

                var attachments = await ServiceLocator.RisService2
                    .GetDiagnosticReportAttachmentsAsync(reportId.Value);

                if (attachments == null || attachments.Count == 0)
                    return;

                foreach (var attachment in attachments)
                    await AddServerAttachmentThumbnailAsync(reportId.Value, attachment);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không load được danh sách ảnh attachment của phiếu kết luận");
            }
        }

        private async Task AddServerAttachmentThumbnailAsync(
            Guid reportId,
            DiagnosticReportAttachmentDto attachment)
        {
            if (attachment == null || attachment.id == Guid.Empty)
                return;

            var localPath = await EnsureAttachmentPreviewFileAsync(reportId, attachment);
            if (string.IsNullOrWhiteSpace(localPath))
                return;

            var documentDestination = attachment.destinations?.FirstOrDefault(d =>
                string.Equals(d.destinationType, DiagnosticReportAttachmentDestinationTypes.Document, StringComparison.OrdinalIgnoreCase));

            var pacsDestination = attachment.destinations?.FirstOrDefault(d =>
                string.Equals(d.destinationType, DiagnosticReportAttachmentDestinationTypes.Pacs, StringComparison.OrdinalIgnoreCase));

            ImageThumbnailList.ThumbnailItem item;
            if (!_thumbnailList.TryAddImage(localPath, out item, documentDestination != null))
                return;

            _thumbnailList.SetAttachmentMetadata(
                item,
                attachment.id,
                attachment.fileName,
                attachment.contentType);

            if (documentDestination != null)
                _thumbnailList.SetDocumentSelected(item, true);

            if (pacsDestination != null)
            {
                _thumbnailList.SetPacsSelected(item, true);
                _thumbnailList.SetPacsStatus(item, pacsDestination.status, pacsDestination.errorDetail);
            }
        }

        private async Task<string> EnsureAttachmentPreviewFileAsync(
            Guid reportId,
            DiagnosticReportAttachmentDto attachment)
        {
            var extension = GuessExtension(attachment.contentType, attachment.fileName);
            var folder = Path.Combine(_baseFolder, "BenhNhan", _machidinh, "Attachments");
            Directory.CreateDirectory(folder);

            var fileName = attachment.id.ToString("D") + extension;
            var localPath = Path.Combine(folder, fileName);

            if (File.Exists(localPath) && new FileInfo(localPath).Length > 0)
                return localPath;

            using (var stream = await ServiceLocator.RisService2
                       .StreamDiagnosticReportAttachmentAsync(reportId, attachment.id))
            using (var file = File.Create(localPath))
            {
                await stream.CopyToAsync(file);
            }

            return localPath;
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
                var reportId = GetCurrentDiagnosticReportId();
                if (!reportId.HasValue || item == null)
                    return;

                await UploadPendingAttachmentsAndSyncDocumentSelectionAsync();
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không upload được ảnh snapshot lên attachment ngay sau khi chụp");
            }
        }

        private async Task UploadPendingAttachmentsAndSyncDocumentSelectionAsync()
        {
            var reportId = GetCurrentDiagnosticReportId();
            if (!reportId.HasValue)
                return;

            await UploadPendingAttachmentsAsync(reportId.Value);
            await SyncDocumentAttachmentSelectionAsync(reportId.Value);
            await SyncPacsAttachmentSelectionIfAnyAsync(reportId.Value);
        }

        private async Task UploadPendingAttachmentsAndSyncDocumentSelectionSafeAsync()
        {
            try
            {
                await UploadPendingAttachmentsAndSyncDocumentSelectionAsync();
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không đồng bộ được ảnh attachment/document-selection sau khi lưu kết luận");
            }
        }

        private async Task UploadPendingAttachmentsAsync(Guid reportId)
        {
            var pendingItems = _thumbnailList.GetPendingUploadItems();
            if (pendingItems.Count == 0)
                return;

            var uploadItems = pendingItems
                .Where(i => File.Exists(i.FilePath))
                .ToList();

            if (uploadItems.Count == 0)
                return;

            var filePaths = uploadItems.Select(i => i.FilePath).ToList();

            var uploaded = await ServiceLocator.RisService2
                .UploadDiagnosticReportAttachmentsAsync(reportId, filePaths);

            if (uploaded == null || uploaded.Count == 0)
                return;

            for (var i = 0; i < uploadItems.Count && i < uploaded.Count; i++)
            {
                var item = uploadItems[i];
                var attachment = uploaded[i];
                _thumbnailList.SetAttachmentMetadata(
                    item,
                    attachment.id,
                    attachment.fileName,
                    attachment.contentType);
            }
        }

        private async Task SyncDocumentAttachmentSelectionAsync(Guid reportId)
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
                .UpdateDocumentAttachmentSelectionAsync(reportId, selections);
        }

        private async Task SyncPacsAttachmentSelectionIfAnyAsync(Guid reportId)
        {
            var pacsAttachmentIds = _thumbnailList.GetPacsSelectedAttachmentIds();
            if (pacsAttachmentIds.Count == 0)
                return;

            await ServiceLocator.RisService2
                .UpdatePacsAttachmentSelectionAsync(reportId, pacsAttachmentIds);
        }

        private async Task PushSelectedPacsAttachmentsAsync()
        {
            var reportId = GetCurrentDiagnosticReportId();
            if (!reportId.HasValue)
            {
                MessageBox.Show(
                    this,
                    "Vui lòng lưu nháp kết luận trước khi đẩy ảnh PACS.",
                    "Chưa có phiếu kết luận",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            await UploadPendingAttachmentsAsync(reportId.Value);

            var pacsAttachmentIds = _thumbnailList.GetPacsSelectedAttachmentIds();
            if (pacsAttachmentIds.Count == 0)
            {
                MessageBox.Show(
                    this,
                    "Chưa có ảnh JPEG nào được chọn để đẩy PACS. Nhấn chuột phải trên thumbnail và chọn PACS.",
                    "Chưa chọn ảnh PACS",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            await ServiceLocator.RisService2
                .UpdatePacsAttachmentSelectionAsync(reportId.Value, pacsAttachmentIds);

            var result = await ServiceLocator.RisService2
                .PushDiagnosticReportAttachmentsToPacsAsync(reportId.Value);

            await RefreshAttachmentStatusesAsync(reportId.Value);

            var message = result == null
                ? "Đã gửi yêu cầu đẩy PACS."
                : $"Đẩy PACS hoàn tất.\nThành công: {result.pushed}\nĐã hoàn thành từ trước: {result.alreadyCompleted}\nThất bại: {result.failed}";

            MessageBox.Show(
                this,
                message,
                "Đẩy PACS",
                MessageBoxButtons.OK,
                result != null && result.failed > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
        }

        private async Task RefreshAttachmentStatusesAsync(Guid reportId)
        {
            var attachments = await ServiceLocator.RisService2
                .GetDiagnosticReportAttachmentsAsync(reportId);

            if (attachments == null || attachments.Count == 0)
                return;

            foreach (var item in _thumbnailList.Items)
            {
                if (!item.AttachmentId.HasValue)
                    continue;

                var attachment = attachments.FirstOrDefault(a => a.id == item.AttachmentId.Value);
                var pacsDestination = attachment?.destinations?.FirstOrDefault(d =>
                    string.Equals(d.destinationType, DiagnosticReportAttachmentDestinationTypes.Pacs, StringComparison.OrdinalIgnoreCase));

                if (pacsDestination == null)
                    continue;

                _thumbnailList.SetPacsSelected(item, true);
                _thumbnailList.SetPacsStatus(item, pacsDestination.status, pacsDestination.errorDetail);
            }
        }
    }
}
