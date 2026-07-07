using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models
{
    public class KetQuaChanDoanResponse
    {
        public string SoPhieu { get; set; }
        public string MaChiDinh { get; set; }
        public string TenDichVu { get; set; }
        public string MaDichVu { get; set; }
        public DateTime NgayKetQua { get; set; }
        public string BacSiKetLuan { get; set; }
        public string MaBacSiKetLuan { get; set; }
        public string KyThuatVien { get; set; }
        public string MaKyThuatVien { get; set; }
        public string MaThietBi { get; set; }
        public string TenThietBi { get; set; }
        public string Kqcls_DeNghi { get; set; }
        public string Kqcls_KetLuan { get; set; }
        //public List<KqclsFile> Kqcls_Files { get; set; }
        public string Kqcls_MoTa { get; set; }
        public string TrangThaiKySo { get; set; }
        public string TrangThai { get; set; }

        [JsonProperty("keyimages")]
        public List<string> ImageFileKeys { get; set; }

        public string TemplateId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }
    }
}
