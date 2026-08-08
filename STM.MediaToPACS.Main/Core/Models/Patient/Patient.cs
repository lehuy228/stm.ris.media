using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models
{
    public class Patient
    {
        public string MaPatient { get; set; }

        public DateTime NgaySinh { get; set; }

        public int GioiTinh { get; set; }

        public string HoTen { get; set; }

        public DateTime TuNgayBHYT { get; set; }

        public DateTime DenNgayBHYT { get; set; }

        public string MaBHYT { get; set; }

        public string XaPhuong { get; set; }

        public string TinhThanh { get; set; }

        public string DanToc { get; set; }
    }
}
