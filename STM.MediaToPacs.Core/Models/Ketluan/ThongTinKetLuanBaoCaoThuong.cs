using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models.Ketluan
{

    public class ThongTinKetLuanBaoCaoThuong
    {
        public ChiDinhDichVuResponse ChiDinhDichVu { get; set; }
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
        public List<ChiSoBaoCao> DanhSachChiSo { get; set; } = new List<ChiSoBaoCao>();

        /// <summary>
        /// Tra cứu giá trị chỉ số theo MÃ param (vd "MV_VMAX") — dùng cho script bind trong repx:
        /// mỗi ô giá trị đặt Tag = mã, script gọi GetChiSo(Tag) để lấy giá trị.
        /// Mapping mã→ô nằm trong repx nên đổi template không cần build lại app.
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
    }
}
