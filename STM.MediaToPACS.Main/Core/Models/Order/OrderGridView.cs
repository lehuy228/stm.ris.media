using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models.Order
{
    public class ChiDinh
    {
        public string SoPhieu { get; set; }

        public string BacSiChiDinh { get; set; }

        public string MaBacSiChiDinh { get; set; }

        public string DoiTuongBN { get; set; }

        public string ChanDoanSoBo { get; set; }

        public List<ChiTietChiDinh> DanhSach { get; set; }

        public string KhoaDieuTri { get; set; }

        public string MucDoChiDinh { get; set; }

        public string NoiChiDinh { get; set; }

        public string Phong { get; set; }

        public string TrangThaiKySo { get; set; }

        public string TrangThaiPhieu { get; set; }

        public string TrangThaiThanhToanChiDinh { get; set; }

        public string TrangThaiDongBoWorklist { get; set; }
    }
    public class ChiTietChiDinh
    {
        public string MaChiDinh { get; set; }

        public string Modality { get; set; }

        public DateTime NgayChiDinh { get; set; }

        public string LoaiChiDinh { get; set; }

        public string LoaiMaChiDinh { get; set; }

        public string MaBacSiThucHienChiDinh { get; set; }

        public string TenBacSiThucHienChiDinh { get; set; }

        public int SoLuong { get; set; }

        public decimal ThanhTien { get; set; }
    }

    public class OrderGridView
    {
        public string MaBenhNhan { get; set; }
        public DateTime NgaySinh { get; set; }
        public int GioiTinh { get; set; }
        public string HoTen { get; set; }
        public DateTime TuNgayBHYT { get; set; }
        public DateTime DenNgayBHYT { get; set; }
        public string MaBHYT { get; set; }
        public string XaPhuong { get; set; }
        public string TinhThanh { get; set; }
        public string DanToc { get; set; }


        public string Sovaovien { get; set; }
        public string MaChiDinh { get; set; }
        public string SoPhieuChiDinh { get; set; }
        public string MaDichVu { get; set; }
        public string TenDichVu { get; set; }
        //public string SoLuong { get; set; }
        public string Modality { get; set; }
        public string MaBacSiChiDinh { get; set; }
        public string TenBacSiChiDinh { get; set; }
        public DateTime Thoigianthuchien { get; set; }
        public string MaNoiChiDinh { get; set; }
        public string TenNoiChiDinh { get; set; }
        public string TrangThai { get; set; }
        public DateTime CreateAt { get; set; }
        public string UpdatedAt { get; set; }
        public string Id { get; set; }
    }
}
