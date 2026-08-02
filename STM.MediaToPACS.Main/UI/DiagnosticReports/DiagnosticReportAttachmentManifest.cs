using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Serilog;

namespace STM.MediaToPACS.Main.UI.DiagnosticReports
{
    internal class DiagnosticReportAttachmentManifest
    {
        public const string FileName = "DiagnosticReportAttachments.dat";

        public string maChiDinh { get; set; }
        public List<DiagnosticReportAttachmentManifestItem> items { get; set; } =
            new List<DiagnosticReportAttachmentManifestItem>();

        public static DiagnosticReportAttachmentManifest Load(string folder, string maChiDinh)
        {
            Directory.CreateDirectory(folder);

            var path = GetManifestPath(folder);
            if (!File.Exists(path))
                return new DiagnosticReportAttachmentManifest { maChiDinh = maChiDinh };

            try
            {
                var json = File.ReadAllText(path);
                var manifest = JsonConvert.DeserializeObject<DiagnosticReportAttachmentManifest>(json)
                               ?? new DiagnosticReportAttachmentManifest();
                manifest.maChiDinh = string.IsNullOrWhiteSpace(manifest.maChiDinh)
                    ? maChiDinh
                    : manifest.maChiDinh;
                manifest.items = manifest.items ?? new List<DiagnosticReportAttachmentManifestItem>();
                foreach (var item in manifest.items)
                {
                    item.uploaded = item.attachmentId.HasValue ||
                                    string.Equals(item.uploadStatus, LocalAttachmentUploadStatuses.Uploaded, StringComparison.OrdinalIgnoreCase);
                    if (item.uploaded)
                        item.uploadStatus = LocalAttachmentUploadStatuses.Uploaded;
                }
                return manifest;
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Khong doc duoc manifest attachment local: {Path}", path);
                return new DiagnosticReportAttachmentManifest { maChiDinh = maChiDinh };
            }
        }

        public void Save(string folder)
        {
            Directory.CreateDirectory(folder);

            var path = GetManifestPath(folder);
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public DiagnosticReportAttachmentManifestItem UpsertLocalFile(
            string filePath,
            bool documentSelected,
            bool pacsSelected,
            string contentType)
        {
            var normalizedPath = NormalizePath(filePath);
            var item = items.FirstOrDefault(i =>
                string.Equals(NormalizePath(i.filePath), normalizedPath, StringComparison.OrdinalIgnoreCase));

            if (item == null)
            {
                item = new DiagnosticReportAttachmentManifestItem
                {
                    localId = Guid.NewGuid(),
                    filePath = normalizedPath,
                    fileName = Path.GetFileName(filePath),
                    createdAt = DateTimeOffset.Now,
                    uploadStatus = LocalAttachmentUploadStatuses.Pending
                };
                items.Add(item);
            }

            item.contentType = string.IsNullOrWhiteSpace(contentType) ? item.contentType : contentType;
            item.documentSelected = documentSelected;
            item.pacsSelected = pacsSelected;
            if (!item.attachmentId.HasValue)
            {
                item.uploaded = false;
                item.uploadStatus = LocalAttachmentUploadStatuses.Pending;
            }
            else
            {
                item.uploaded = true;
                item.uploadStatus = LocalAttachmentUploadStatuses.Uploaded;
            }
            return item;
        }

        public DiagnosticReportAttachmentManifestItem FindByFilePath(string filePath)
        {
            var normalizedPath = NormalizePath(filePath);
            return items.FirstOrDefault(i =>
                string.Equals(NormalizePath(i.filePath), normalizedPath, StringComparison.OrdinalIgnoreCase));
        }

        public DiagnosticReportAttachmentManifestItem FindByAttachmentId(Guid attachmentId)
        {
            if (attachmentId == Guid.Empty)
                return null;

            return items.FirstOrDefault(i => i.attachmentId.HasValue && i.attachmentId.Value == attachmentId);
        }

        private static string GetManifestPath(string folder)
        {
            return Path.Combine(folder, FileName);
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
    }

    internal class DiagnosticReportAttachmentManifestItem
    {
        public Guid localId { get; set; }
        public Guid? attachmentId { get; set; }
        public string filePath { get; set; }
        public string fileName { get; set; }
        public string contentType { get; set; }
        public bool documentSelected { get; set; }
        public bool pacsSelected { get; set; }
        public bool uploaded { get; set; }
        public string uploadStatus { get; set; }
        public string pacsStatus { get; set; }
        public string errorDetail { get; set; }
        public DateTimeOffset createdAt { get; set; }
        public DateTimeOffset? uploadedAt { get; set; }
        public DateTimeOffset? lastAttemptAt { get; set; }
    }

    internal static class LocalAttachmentUploadStatuses
    {
        public const string Pending = "pending";
        public const string Uploading = "uploading";
        public const string Uploaded = "uploaded";
        public const string Failed = "failed";
    }
}
