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
    }
}
