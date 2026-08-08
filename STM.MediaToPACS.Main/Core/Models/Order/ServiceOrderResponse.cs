using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models.Order
{
    public class ServiceOrderResponse
    {
        public Patient BenhNhan { get; set; }

        public string admissionType { get; set; }
        public string Sovaovien { get; set; }
        public string MaChiDinh { get; set; }
        public string SoPhieuChiDinh { get; set; }
        public string MaDichVu { get; set; }
        public string TenDichVu { get; set; }
        public int SoLuong { get; set; }
        public string Modality { get; set; }
        public string MaBacSiChiDinh { get; set; }
        public string TenBacSiChiDinh { get; set; }
        public string ChanDoanSoBo { get; set; }
        public DateTime Thoigianthuchien { get; set; }
        public string KhoaDieuTri { get; set; }
        public string Phong { get; set; }
        public string MaNoiChiDinh { get; set; }
        public string TenNoiChiDinh { get; set; }
        public string TrangThai { get; set; }
        public DateTime CreateAt { get; set; }
        public string UpdatedAt { get; set; }
        public string Id { get; set; }
    }
}
