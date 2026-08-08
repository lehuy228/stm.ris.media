using MediaToPacs.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Interfaces
{
    /// <summary>
    /// Module RIS V2 — tích hợp nội bộ, không xác thực (base URL: /api/risv1).
    /// Xem docs/api/ris-v1.md.
    /// </summary>
    public interface IRisService2
    {
        Task<SystemUpdateConfig> GetSystemUpdateConfigAsync();

        /// <summary>
        /// Danh sách KTV, y tá cùng khoa (theo orgCode).
        /// titleCodes mặc định ["ktv", "y_ta"] nếu không truyền.
        /// </summary>
        Task<List<PractitionerListDto>> GetColleaguesAsync(string orgCode, List<string> titleCodes = null);

        /// <summary>
        /// Danh sách device, lọc theo modality nếu có truyền.
        /// </summary>
        Task<List<DeviceDto>> GetDevicesAsync(string modality = null);

        /// <summary>
        /// Danh sách khoa (Organization.OrgTypeCode = DEPARTMENT).
        /// </summary>
        Task<List<OrganizationDto>> GetDepartmentsAsync();

        /// <summary>
        /// Danh sách gợi ý nhanh (không kèm nội dung văn bản).
        /// gender: 0=Unknown, 1=Male, 2=Female, 3=Other — backend tự gộp suggestion gender=Unknown.
        /// serviceCode: mã dịch vụ HIS (ImagingService.Code) — dùng thay serviceId vì client
        /// chỉ có mã string từ chỉ định (maDichVu).
        /// </summary>
        Task<List<QuickSuggestionListItemDto>> GetQuickSuggestionsAsync(
            long? serviceId = null,
            int? gender = null,
            bool? hasReportParam = null,
            string modalityCode = null,
            string search = null,
            bool? activeOnly = null,
            string serviceCode = null);

        /// <summary>
        /// Chi tiết đầy đủ 1 gợi ý nhanh (kèm paramGroups nếu là dạng Structured).
        /// Trả về null nếu không tìm thấy (404).
        /// </summary>
        Task<QuickSuggestionPublicDetailDto> GetQuickSuggestionByIdAsync(long id);

        /// <summary>
        /// Lấy chi tiết y lệnh, bao gồm bệnh nhân, lượt khám, phiếu chỉ định và kết quả hiện tại.
        /// Trả về null nếu không tìm thấy y lệnh (404).
        /// </summary>
        Task<RisV1OrderItemDetailDto> GetOrderItemByIdAsync(Guid id);

        /// <summary>
        /// Tra y lệnh theo mã chỉ định phía HIS (OrderItem.PlacerOrderItemCode = maChiDinh).
        /// Trả về null nếu RIS mới chưa có y lệnh mang mã này (404).
        /// </summary>
        Task<RisV1OrderItemDetailDto> GetOrderItemByPlacerCodeAsync(string placerCode);

        /// <summary>
        /// Tạo mới hoặc cập nhật kết luận của y lệnh theo cơ chế upsert.
        /// Chỉ report draft/amended mới được phép cập nhật.
        /// </summary>
        Task<RisV1DiagnosticReportDetailDto> UpsertOrderItemConclusionAsync(
            Guid id,
            RisV1UpsertConclusionRequest request);

        /// <summary>
        /// Hủy ký số theo mã chỉ định HIS. <paramref name="userCode"/> (query `userCode`) là mã nhân
        /// viên thực hiện - backend resolve sang Practitioner.Id để ghi audit log; không truyền thì
        /// audit không xác định được người thực hiện.
        /// </summary>
        Task<RisV1DiagnosticReportDetailDto> VoidSignatureByPlacerCodeAsync(
            string placerCode, string userCode = null);

        /// <summary>
        /// Đánh dấu hoàn thành y lệnh theo mã chỉ định HIS. <paramref name="userCode"/> (query
        /// `userCode`) là mã nhân viên thực hiện - backend resolve sang Practitioner.Id để ghi audit log.
        /// </summary>
        Task<RisV1DiagnosticReportDetailDto> CompleteOrderItemByPlacerCodeAsync(
            string placerCode, string userCode = null);

        /// <summary>
        /// Gửi lại ORU^R01 sang HIS (đồng bộ). Trả kết quả kèm mã lỗi/nội dung lỗi HIS trả về
        /// (ACK ERR-5/ERR-7) để hiển thị cho người dùng; không bao giờ trả null.
        /// </summary>
        Task<OruResendResultDto> ResendOruToHisAsync(string orderItemCode);

        Task VoidDiagnosticReportAsync(Guid reportId);

        /// <summary>
        /// Nhật ký hệ thống của 1 chỉ định (GET /api/v1/audit-logs?orderCode=...), sắp xếp theo
        /// thời gian giảm dần. Trả danh sách rỗng nếu không có/lỗi đọc dữ liệu.
        /// </summary>
        Task<List<AuditLogListItemDto>> GetAuditLogsByOrderCodeAsync(string orderCode, int limit = 100);

        /// <summary>
        /// Tra bệnh nhân theo mã chỉ định, trả lịch sử tối đa 50 lượt khám gần nhất
        /// kèm chỉ định dịch vụ của từng lượt. Trả về null nếu không tìm thấy (404).
        /// </summary>
        Task<RisV1PatientOrderHistoryDto> GetPatientHistoryByOrderCodeAsync(string placerCode);

        /// <summary>
        /// Danh sách DICOM viewer được phép dùng của 1 bác sĩ, tra theo staffCode.
        /// Trả về danh sách rỗng nếu bác sĩ không tồn tại hoặc chưa được gán viewer nào.
        /// </summary>
        Task<List<PractitionerViewerAccessDto>> GetViewerAccessesAsync(string staffCode);

        /// <summary>
        /// Lấy link mở DICOM viewer theo mã chỉ định phía HIS (PlacerOrderItemCode) - endpoint
        /// risv1 không xác thực nên staffCode phải tự truyền (dùng để chọn quyền viewer thay cho
        /// danh tính đăng nhập). Yêu cầu y lệnh đã có DicomStudyUid. viewerName không truyền →
        /// server tự chọn viewer mặc định của staffCode (hoặc global config nếu chưa được gán viewer nào).
        /// </summary>
        Task<string> GetViewerLinkByPlacerCodeAsync(string placerCode, string staffCode, string viewerName = null);

        Task<List<DiagnosticReportAttachmentDto>> GetDiagnosticReportAttachmentsAsync(Guid orderItemId);

        Task<List<DiagnosticReportAttachmentDto>> UploadDiagnosticReportAttachmentsAsync(
            Guid orderItemId,
            IEnumerable<string> filePaths);

        Task<List<DiagnosticReportAttachmentDto>> UpdateDocumentAttachmentSelectionAsync(
            Guid orderItemId,
            List<DocumentAttachmentSelectionItem> selections);

        Task UpdatePacsAttachmentSelectionAsync(
            Guid orderItemId,
            List<Guid> attachmentIds);

        Task<PacsPushResult> PushDiagnosticReportAttachmentsToPacsAsync(
            Guid orderItemId,
            string targetServer = "MainStorage");

        Task<Stream> StreamDiagnosticReportAttachmentAsync(
            Guid orderItemId,
            Guid attachmentId);

        Task DeleteDiagnosticReportAttachmentAsync(
            Guid orderItemId,
            Guid attachmentId);
    }
}
