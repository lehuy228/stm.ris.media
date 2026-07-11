using MediaToPacs.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Interfaces
{
    /// <summary>
    /// Module RIS V2 — tích hợp nội bộ, không xác thực (base URL: /api/risv1).
    /// Xem docs/api/ris-v1.md.
    /// </summary>
    public interface IRisService2
    {
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
        /// serviceId hiện chưa dùng được từ client (client chỉ có mã dịch vụ HIS dạng string) — để sẵn cho tương lai.
        /// </summary>
        Task<List<QuickSuggestionListItemDto>> GetQuickSuggestionsAsync(
            long? serviceId = null,
            int? gender = null,
            bool? hasReportParam = null,
            string modalityCode = null,
            string search = null,
            bool? activeOnly = null);

        /// <summary>
        /// Chi tiết đầy đủ 1 gợi ý nhanh (kèm paramGroups nếu là dạng Structured).
        /// Trả về null nếu không tìm thấy (404).
        /// </summary>
        Task<QuickSuggestionPublicDetailDto> GetQuickSuggestionByIdAsync(long id);
    }
}
