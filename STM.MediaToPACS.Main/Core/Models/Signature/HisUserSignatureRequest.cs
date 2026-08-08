using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models.Signature
{
    public class HisUserSignatureRequest
    {
        public string CanCuocNhanVien { get; set; }
        public string AnhChuKy { get; set; }
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
