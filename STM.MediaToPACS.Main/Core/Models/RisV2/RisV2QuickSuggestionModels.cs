using System.Collections.Generic;

namespace MediaToPacs.Core.Models
{
    /// <summary>
    /// Item danh sách gợi ý nhanh — GET /risv1/quick-suggestions.
    /// Response trả trực tiếp mảng, không bọc ApiResponse, không kèm nội dung văn bản
    /// (description/conclusion/notes) — gọi endpoint chi tiết khi cần đầy đủ.
    /// </summary>
    public class QuickSuggestionListItemDto
    {
        public long id { get; set; }

        /// <summary>ID dịch vụ CĐHA (ImagingService.Id) gắn với suggestion</summary>
        public long serviceId { get; set; }

        /// <summary>Tiêu đề hiển thị</summary>
        public string title { get; set; }

        /// <summary>Unknown | Male | Female | Other</summary>
        public string gender { get; set; }

        public List<string> tags { get; set; }

        public int sortOrder { get; set; }

        /// <summary>Số lần đã được bác sĩ áp dụng</summary>
        public int usageCount { get; set; }

        public bool isActive { get; set; }

        /// <summary>Khác null = suggestion "Structured" (có form chỉ số); null = "Text" thuần</summary>
        public long? reportParamId { get; set; }

        /// <summary>Hiển thị title trong combobox</summary>
        public override string ToString()
        {
            return title;
        }
    }

    /// <summary>
    /// Chi tiết đầy đủ 1 gợi ý nhanh — GET /risv1/quick-suggestions/{id}.
    /// Nhúng kèm toàn bộ cấu trúc ReportParam (paramGroups) vì client không xác thực
    /// không gọi được các endpoint param-form riêng của api/v1.
    /// </summary>
    public class QuickSuggestionPublicDetailDto
    {
        public long id { get; set; }

        public long serviceId { get; set; }

        public string title { get; set; }

        /// <summary>Plain | Html — cách render field description</summary>
        public string descriptionMode { get; set; }

        /// <summary>Unknown | Male | Female | Other</summary>
        public string gender { get; set; }

        /// <summary>Nội dung mô tả</summary>
        public string description { get; set; }

        /// <summary>Kết luận</summary>
        public string conclusion { get; set; }

        /// <summary>Khuyến nghị</summary>
        public string notes { get; set; }

        public List<string> tags { get; set; }

        public int sortOrder { get; set; }

        public int usageCount { get; set; }

        public bool isActive { get; set; }

        /// <summary>Khác null = "Structured", null = "Text" thuần</summary>
        public long? reportParamId { get; set; }

        /// <summary>Mã ReportParam gắn với suggestion, null nếu Text thuần</summary>
        public string reportParamCode { get; set; }

        /// <summary>Tên hiển thị ReportParam, null nếu Text thuần</summary>
        public string reportParamName { get; set; }

        /// <summary>Cấu trúc đầy đủ groups/params/ranges; null nếu Text thuần</summary>
        public List<ParamGroupFormDto> paramGroups { get; set; }
    }

    /// <summary>Nhóm chỉ số trong form động của suggestion "Structured"</summary>
    public class ParamGroupFormDto
    {
        public string groupCode { get; set; }

        public string groupName { get; set; }

        public int sortOrder { get; set; }

        /// <summary>Số cột chiếm trong lưới 4 cột (1-4)</summary>
        public int colSpan { get; set; }

        /// <summary>Số chỉ số/hàng trong nhóm (1 hoặc 2)</summary>
        public int paramColumns { get; set; }

        public bool forceNewRow { get; set; }

        // "params" là keyword C# nên dùng verbatim identifier - serialize/deserialize vẫn ra đúng tên "params"
        public List<ParamFieldDto> @params { get; set; }
    }

    /// <summary>Một chỉ số trong nhóm của form động</summary>
    public class ParamFieldDto
    {
        public string paramCode { get; set; }

        public string label { get; set; }

        public string unit { get; set; }

        /// <summary>number | text | boolean | date</summary>
        public string dataType { get; set; }

        /// <summary>value | checkbox | checkbox_value</summary>
        public string inputMode { get; set; }

        /// <summary>Dải ngưỡng tô màu, null = không tô màu</summary>
        public List<ReportParamRangeMeta> ranges { get; set; }

        /// <summary>Giá trị mẫu gợi ý</summary>
        public string presetValue { get; set; }

        /// <summary>Nhãn hiển thị mặc định (dùng cho checkbox)</summary>
        public string presetLabel { get; set; }

        public bool isHighlighted { get; set; }

        public int sortOrder { get; set; }
    }

    /// <summary>Dải ngưỡng tô màu của một chỉ số</summary>
    public class ReportParamRangeMeta
    {
        public double? min { get; set; }

        public double? max { get; set; }

        /// <summary>critical_low | abnormal_low | normal | abnormal_high | critical_high</summary>
        public string severity { get; set; }

        public string label { get; set; }

        public string color { get; set; }
    }

    /// <summary>Hằng số gender của API risv1 (backend tự gộp Unknown khi lọc)</summary>
    public static class RisV2Gender
    {
        public const int Unknown = 0;
        public const int Male = 1;
        public const int Female = 2;
        public const int Other = 3;
    }
}
