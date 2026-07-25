using System;
using System.Collections.Generic;

namespace MediaToPacs.Core.Models
{
    /// <summary>
    /// Response của GET /risv1/patients/history-by-order-code/{code}.
    /// Xem docs/api/ris-v1.md §11.
    /// </summary>
    public class RisV1PatientOrderHistoryDto
    {
        public RisV1PatientSummaryDto patient { get; set; }

        public List<RisV1VisitOrderHistoryDto> visits { get; set; }
    }

    public class RisV1VisitOrderHistoryDto
    {
        public Guid id { get; set; }
        public string visitCode { get; set; }
        public string visitType { get; set; }
        public DateTimeOffset admissionTime { get; set; }
        public DateTimeOffset? dischargeTime { get; set; }
        public string admissionDiagnosis { get; set; }
        public string referringDoctorName { get; set; }

        public List<RisV1OrderItemHistoryDto> orderItems { get; set; }
    }

    public class RisV1OrderItemHistoryDto
    {
        public Guid id { get; set; }
        public string orderCode { get; set; }
        public string placerOrderItemCode { get; set; }
        public string serviceCode { get; set; }
        public string serviceName { get; set; }
        public string modalityCode { get; set; }
        public string status { get; set; }
        public string priority { get; set; }
        public DateTimeOffset? scheduledAt { get; set; }
        public DateTimeOffset? startedAt { get; set; }
        public DateTimeOffset? completedAt { get; set; }
        public bool hasImage { get; set; }
        public string dicomStudyUid { get; set; }
        public Guid? reportId { get; set; }
        public string reportStatus { get; set; }
    }
}
