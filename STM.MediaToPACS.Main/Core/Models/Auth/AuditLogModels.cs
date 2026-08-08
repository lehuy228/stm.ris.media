using System;
using System.Collections.Generic;

namespace MediaToPacs.Core.Models
{
    /// <summary>
    /// 1 dòng nhật ký hệ thống (GET /api/v1/audit-logs). Item danh sách không kèm
    /// contextJson/beforeJson/afterJson/exceptionText - phải gọi /audit-logs/{id} nếu cần chi tiết.
    /// </summary>
    public class AuditLogListItemDto
    {
        public Guid id { get; set; }
        public DateTimeOffset timestampUtc { get; set; }

        /// <summary>Information | Warning | Error</summary>
        public string level { get; set; }

        /// <summary>Module phát sinh log, vd "DiagnosticReport.Approve"</summary>
        public string source { get; set; }

        /// <summary>Vd "APPROVE_DIAGNOSTIC_REPORT"</summary>
        public string action { get; set; }

        public string entityType { get; set; }
        public string entityId { get; set; }
        public string patientId { get; set; }
        public string visitId { get; set; }
        public string orderItemId { get; set; }

        /// <summary>Mã chỉ định phía HIS (PlacerOrderItemCode)</summary>
        public string orderCode { get; set; }

        /// <summary>
        /// Người thực hiện: luồng RisV1 là Practitioner.Id dạng số (string hoá), luồng khác có thể
        /// là username Keycloak hoặc "risv1".
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// Tên người thực hiện, backend tra thêm từ userId (chỉ khi userId là Practitioner.Id dạng số).
        /// null khi không tra được (vd "risv1", username Keycloak).
        /// </summary>
        public string userFullName { get; set; }

        public string message { get; set; }
        public long? durationMs { get; set; }
    }

    public class AuditLogPageDto
    {
        public List<AuditLogListItemDto> items { get; set; }
        public int total { get; set; }
        public int page { get; set; }
        public int limit { get; set; }
        public int totalPages { get; set; }
    }
}
