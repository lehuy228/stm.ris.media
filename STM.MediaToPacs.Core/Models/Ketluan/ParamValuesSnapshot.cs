using System.Collections.Generic;

namespace MediaToPacs.Core.Models.Ketluan
{
    /// <summary>
    /// Bảng chỉ số gửi kèm kết luận lên RIS mới (field parameters của
    /// POST /risv1/order-items/{id}/conclusion) — cùng input shape với luồng v1 chuẩn:
    /// backend tự build "Schema B" (tính status/màu theo ranges) rồi lưu ParametersJson.
    /// </summary>
    public class RisV1ReportParamsInput
    {
        public long? suggestionId { get; set; }

        // "params" là keyword C# nên dùng verbatim identifier - serialize vẫn ra đúng tên "params"
        public List<RisV1ReportParamInputItem> @params { get; set; } = new List<RisV1ReportParamInputItem>();

        /// <summary>Mã người lưu (HISCode) — endpoint không xác thực nên client tự khai</summary>
        public string capturedBy { get; set; }

        public string capturedByName { get; set; }
    }

    /// <summary>Giá trị 1 chỉ số nhập từ form (input thô, chưa tính status)</summary>
    public class RisV1ReportParamInputItem
    {
        public string groupCode { get; set; }

        public string paramCode { get; set; }

        /// <summary>Giá trị nhập; null/rỗng với checkbox thuần</summary>
        public string value { get; set; }

        /// <summary>Nhãn hiển thị (checkbox dùng presetLabel); null nếu không có</summary>
        public string displayLabel { get; set; }

        /// <summary>Trạng thái tích với checkbox/checkbox_value; null với dạng value</summary>
        public bool? @checked { get; set; }
    }

    /// <summary>
    /// "Schema B" đọc về từ report.parameters (GET /risv1/order-items/...) — chỉ khai các field
    /// client cần để khôi phục form chỉ số; các field khác (status, ranges...) bỏ qua khi deserialize.
    /// </summary>
    public class RisV1ReportParamsSchema
    {
        public string schemaVersion { get; set; }

        public long? suggestionId { get; set; }

        public List<RisV1ReportParamGroupSnapshot> groups { get; set; }
    }

    public class RisV1ReportParamGroupSnapshot
    {
        public string groupCode { get; set; }

        public List<RisV1ReportParamSnapshotItem> @params { get; set; }
    }

    public class RisV1ReportParamSnapshotItem
    {
        public string paramCode { get; set; }

        public string value { get; set; }

        public bool? @checked { get; set; }
    }
}
