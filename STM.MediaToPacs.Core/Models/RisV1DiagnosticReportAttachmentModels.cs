using System;
using System.Collections.Generic;

namespace MediaToPacs.Core.Models
{
    public static class DiagnosticReportAttachmentDestinationTypes
    {
        public const string Document = "document";
        public const string Pacs = "pacs";
    }

    public static class DiagnosticReportAttachmentDestinationStatuses
    {
        public const string Pending = "pending";
        public const string Processing = "processing";
        public const string Completed = "completed";
        public const string Failed = "failed";
        public const string AlreadyCompleted = "AlreadyCompleted";
    }

    public class DiagnosticReportAttachmentDto
    {
        public Guid id { get; set; }
        public string fileName { get; set; }
        public string contentType { get; set; }
        public long fileSize { get; set; }
        public string storageKey { get; set; }
        public List<DiagnosticReportAttachmentDestinationDto> destinations { get; set; }
        public DateTimeOffset createdAt { get; set; }
    }

    public class DiagnosticReportAttachmentDestinationDto
    {
        public Guid id { get; set; }
        public string destinationType { get; set; }
        public string status { get; set; }
        public int? sortOrder { get; set; }
        public DateTimeOffset? processedAt { get; set; }
        public string errorDetail { get; set; }
        public string instanceUid { get; set; }
    }

    public class DiagnosticReportAttachmentListResult
    {
        public List<DiagnosticReportAttachmentDto> items { get; set; }
    }

    public class UploadDiagnosticReportAttachmentsResult
    {
        public List<DiagnosticReportAttachmentDto> uploaded { get; set; }
    }

    public class UploadDiagnosticReportAttachmentsResponse
    {
        public object data { get; set; }
        public List<DiagnosticReportAttachmentDto> uploaded { get; set; }
        public List<DiagnosticReportAttachmentDto> items { get; set; }
    }

    public class DocumentAttachmentSelectionRequest
    {
        public List<DocumentAttachmentSelectionItem> selections { get; set; }
    }

    public class DocumentAttachmentSelectionItem
    {
        public Guid attachmentId { get; set; }
        public int sortOrder { get; set; }
    }

    public class PacsAttachmentSelectionRequest
    {
        public List<Guid> attachmentIds { get; set; }
    }

    public class PacsPushResponse
    {
        public PacsPushResult data { get; set; }
    }

    public class PacsPushResult
    {
        public int pushed { get; set; }
        public int alreadyCompleted { get; set; }
        public int failed { get; set; }
        public List<PacsPushItemResult> items { get; set; }
    }

    public class PacsPushItemResult
    {
        public Guid attachmentId { get; set; }
        public Guid destinationId { get; set; }
        public string status { get; set; }
        public string instanceUid { get; set; }
        public string errorDetail { get; set; }
    }
}
