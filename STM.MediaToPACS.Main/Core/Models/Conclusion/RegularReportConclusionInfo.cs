using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaToPacs.Core.Models.Order;

namespace MediaToPacs.Core.Models.Conclusion
{

    public class RegularReportConclusionInfo
    {
        public ServiceOrderResponse ChiDinhDichVu { get; set; }
        public string TGBacSiChiDinh { get; set; }
        public string TGBacSiKetLuan { get; set; }
        public string MoTa { get; set; }
        public string KetLuan { get; set; }
        public string KhuyenNghi { get; set; }
        public string GioiTinhConvert { get; set; }
        public string TenBacSiKetLuan { get; set; }
        public string AnhChuKy { get; set; }
        public string Image1 { get; set; } = null;
        public string Image2 { get; set; } = null;
        public string Image3 { get; set; } = null;
        public string Image4 { get; set; } = null;

        /// <summary>
        /// Bảng chỉ số động (suggestion Structured) - template có DetailReportBand
        /// bind DataMember này sẽ render bảng; template cũ không có band vẫn chạy bình thường.
        /// </summary>
        public List<ReportParameter> DanhSachChiSo { get; set; } = new List<ReportParameter>();

        /// <summary>
        /// Tra cứu giá trị chỉ số theo Mã param (vd "MV_VMAX") - dùng cho script bind trong repx:
        /// mỗi ô giá trị đặt Tag = mã, script gọi GetChiSo(Tag) để lấy giá trị.
        /// Mapping mã-ô nằm trong repx nên đổi template không cần build lại app.
        /// </summary>
        public Dictionary<string, string> ChiSoTheoMa { get; set; } = new Dictionary<string, string>();

        /// <summary>Trả giá trị chỉ số theo mã; rỗng nếu không có. Script trong repx gọi hàm này.</summary>
        public string GetChiSo(string ma)
        {

            if (string.IsNullOrEmpty(ma) || ChiSoTheoMa == null)
                return string.Empty;
            string value;
            return ChiSoTheoMa.TryGetValue(ma, out value) ? (value ?? string.Empty) : string.Empty;
        }

        /// <summary>
        /// Trạng thái tích theo Mã param - dành cho XRCheckBox trong repx:
        /// XRCheckBox đặt Tag = mã, script gọi GetChiSoCheck(Tag) gán vào Checked.
        /// Chỉ chứa param dạng checkbox/checkbox_value; param dạng value không có trong dictionary.
        /// </summary>
        public Dictionary<string, bool> ChiSoCheckTheoMa { get; set; } = new Dictionary<string, bool>();

        /// <summary>Trả trạng thái tích của checkbox theo mã; false nếu không có/không tích.</summary>
        public bool GetChiSoCheck(string ma)
        {
            if (string.IsNullOrEmpty(ma) || ChiSoCheckTheoMa == null)
                return false;
            bool isChecked;
            return ChiSoCheckTheoMa.TryGetValue(ma, out isChecked) && isChecked;
        }
    }
}
