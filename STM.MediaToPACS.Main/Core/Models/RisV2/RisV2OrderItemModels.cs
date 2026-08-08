using System;

namespace MediaToPacs.Core.Models
{
    public class RisV1OrderItemDetailDto
    {
        public Guid id { get; set; }
        public Guid orderSheetId { get; set; }
        public string serviceCode { get; set; }
        public string serviceName { get; set; }
        public string modalityCode { get; set; }
        public string status { get; set; }
        public string priority { get; set; }
        public string serviceNotes { get; set; }
        public string instruction { get; set; }
        public string bodySite { get; set; }
        public string bodySiteCode { get; set; }
        public string bodySiteLaterality { get; set; }
        public string reasonCode { get; set; }
        public string reasonCodeDisplay { get; set; }
        public DateTimeOffset? scheduledAt { get; set; }
        public DateTimeOffset? startedAt { get; set; }
        public DateTimeOffset? completedAt { get; set; }
        public bool hasImage { get; set; }
        public string dicomStudyUid { get; set; }
        public long? deviceId { get; set; }
        public long? locationId { get; set; }
        public string technologistCode { get; set; }
        public Guid? technologistId { get; set; }
        public RisV1PatientSummaryDto patient { get; set; }
        public RisV1VisitSummaryDto visit { get; set; }
        public RisV1OrderSheetSummaryDto orderSheet { get; set; }
        public RisV1DiagnosticReportDto report { get; set; }
    }

    public class RisV1PatientSummaryDto
    {
        public Guid id { get; set; }
        public string patientCode { get; set; }
        public string fullName { get; set; }
        public string gender { get; set; }
        public string dateOfBirth { get; set; }
        public string phoneNumber { get; set; }
        public string bloodType { get; set; }
    }

    public class RisV1VisitSummaryDto
    {
        public Guid id { get; set; }
        public string visitCode { get; set; }
        public string visitType { get; set; }
        public DateTimeOffset admissionTime { get; set; }
        public DateTimeOffset? dischargeTime { get; set; }
        public string admissionDiagnosis { get; set; }
        public string clinicalNotes { get; set; }
        public string referringDoctorName { get; set; }
        public short? heightCm { get; set; }
        public decimal? weightKg { get; set; }
    }

    public class RisV1OrderSheetSummaryDto
    {
        public Guid id { get; set; }
        public string orderCode { get; set; }
        public string clinicalDiagnosis { get; set; }
        public string clinicalNotes { get; set; }
        public string refPractitionerCode { get; set; }
        public string refPractitionerName { get; set; }
        public string refDepartmentName { get; set; }
        public string priority { get; set; }
        public string status { get; set; }
        public DateTimeOffset orderedAt { get; set; }
    }

    public class RisV1DiagnosticReportDto
    {
        public Guid id { get; set; }
        public string findings { get; set; }
        public string conclusion { get; set; }
        public string recommendation { get; set; }
        public string diagnosisCodesJson { get; set; }
        // Schema tham số thay đổi theo loại report; giữ object để client không khóa cứng cấu trúc.
        public object parameters { get; set; }
        public string status { get; set; }
        public long? deviceId { get; set; }
        public string deviceCode { get; set; }
        public string deviceName { get; set; }
        public Guid? templateId { get; set; }
        public string technologistPractitionerCode { get; set; }
        public string technologistPractitionerName { get; set; }
        public long? technologistPractitionerId { get; set; }
        public string doctorPractitionerCode { get; set; }
        public string doctorPractitionerName { get; set; }
        public long? doctorPractitionerId { get; set; }
        public DateTimeOffset? signedAt { get; set; }
        public DateTimeOffset? issuedAt { get; set; }
        public DateTimeOffset? readStartedAt { get; set; }
        public DateTimeOffset? readCompletedAt { get; set; }
        public string presentedFormUrl { get; set; }
        public string conclusionCode { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? modifiedAt { get; set; }
    }

    public class RisV1UpsertConclusionRequest
    {
        /// <summary>Luôn được áp phía backend — null sẽ XÓA kết luận trên report đang sửa được</summary>
        public string conclusion { get; set; }

        /// <summary>Mô tả/diễn giải. null = backend giữ nguyên giá trị hiện có</summary>
        public string findings { get; set; }

        /// <summary>Khuyến nghị. null = backend giữ nguyên giá trị hiện có</summary>
        public string recommendation { get; set; }

        /// <summary>Mã bác sĩ đọc/kết luận — client tự khai (endpoint không xác thực). null = backend giữ nguyên</summary>
        public string doctorPractitionerCode { get; set; }

        /// <summary>Tên bác sĩ đọc/kết luận. null = backend giữ nguyên giá trị hiện có</summary>
        public string doctorPractitionerName { get; set; }

        /// <summary>ID nội bộ bác sĩ nếu client biết. null = backend giữ nguyên giá trị hiện có</summary>
        public long? doctorPractitionerId { get; set; }

        /// <summary>Mã KTV thực hiện. null = backend giữ nguyên giá trị hiện có</summary>
        public string technologistPractitionerCode { get; set; }

        /// <summary>Tên KTV thực hiện. null = backend giữ nguyên giá trị hiện có</summary>
        public string technologistPractitionerName { get; set; }

        /// <summary>ID nội bộ KTV nếu client biết. null = backend giữ nguyên giá trị hiện có</summary>
        public long? technologistPractitionerId { get; set; }

        /// <summary>ID nội bộ thiết bị. null = backend giữ nguyên giá trị hiện có</summary>
        public long? deviceId { get; set; }

        /// <summary>Mã thiết bị. null = backend giữ nguyên giá trị hiện có</summary>
        public string deviceCode { get; set; }

        /// <summary>Tên thiết bị. null = backend giữ nguyên giá trị hiện có</summary>
        public string deviceName { get; set; }

        /// <summary>
        /// Thời điểm bắt đầu đọc. null = lần tạo report đầu tiên backend tự set giờ nhận request,
        /// các lần sau giữ nguyên giá trị đã có.
        /// </summary>
        public DateTimeOffset? readStartedAt { get; set; }

        /// <summary>
        /// Thời điểm kết thúc đọc. null = backend giữ nguyên (backend KHÔNG tự set theo giờ nhận request).
        /// </summary>
        public DateTimeOffset? readCompletedAt { get; set; }

        /// <summary>
        /// Bảng chỉ số (ParamValuesSnapshot) — serialize nguyên văn vào DiagnosticReport.parameters.
        /// null = backend giữ nguyên giá trị hiện có.
        /// </summary>
        public object parameters { get; set; }
    }

    public class RisV1DiagnosticReportDetailDto : RisV1DiagnosticReportDto
    {
        public Guid orderItemId { get; set; }
        public Guid patientId { get; set; }
        public string patientCode { get; set; }
        public Guid visitId { get; set; }
        public string visitCode { get; set; }
        public string serviceCode { get; set; }
        public string serviceName { get; set; }
        public string modality { get; set; }
        public string extensionJson { get; set; }
        public bool isDeleted { get; set; }
        public DateTime? deletedAt { get; set; }
        public string deletedBy { get; set; }
    }
}
