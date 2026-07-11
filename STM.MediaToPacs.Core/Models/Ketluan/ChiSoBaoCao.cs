namespace MediaToPacs.Core.Models.Ketluan
{
    /// <summary>
    /// Một dòng chỉ số trong bảng chỉ số động của báo cáo kết luận (in PDF).
    /// Được đổ vào DetailReportBand (DataMember = DanhSachChiSo) của template XtraReport,
    /// group theo NhomChiSo; MauSac dùng cho expression binding ForeColor của cột giá trị.
    /// </summary>
    public class ChiSoBaoCao
    {
        /// <summary>Tên nhóm chỉ số (ví dụ "Các thông số đo 2D/TM")</summary>
        public string NhomChiSo { get; set; }

        /// <summary>Tên chỉ số (ví dụ "EF") hoặc câu mô tả hoàn chỉnh với dạng checkbox</summary>
        public string TenChiSo { get; set; }

        /// <summary>Giá trị bác sĩ nhập; rỗng với dạng checkbox thuần</summary>
        public string GiaTri { get; set; }

        /// <summary>Đơn vị đo (mm, %, cm/s...)</summary>
        public string DonVi { get; set; }

        /// <summary>Nhãn đánh giá theo dải ngưỡng (ví dụ "EF giảm nặng"), rỗng nếu không có ngưỡng</summary>
        public string DanhGia { get; set; }

        /// <summary>Mã màu hex theo dải ngưỡng (ví dụ "#cc1a1a"), rỗng nếu không có ngưỡng</summary>
        public string MauSac { get; set; }
    }
}
