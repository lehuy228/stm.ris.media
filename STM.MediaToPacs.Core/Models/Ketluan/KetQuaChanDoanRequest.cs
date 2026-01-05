using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models
{
    public class KetQuaChanDoanRequest
    {
        public string sophieu { get; set; }
        public string machidinh { get; set; }
        public string mabacsiketluan { get; set; }
        public string bacsiketluan { get; set; }
        public string makythuatvien { get; set; }
        public string kythuatvien { get; set; }
        public string mathietbi { get; set; }
        public string tenthietbi { get; set; }
        public DateTime thoigianthuchien { get; set; }
        public DateTime thoigianketthuc { get; set; }
        public string kqcls_denghi { get; set; }
        public string kqcls_ketluan { get; set; }
        public string kqcls_mota { get; set; }
        public string trangthai { get; set; }
        public List<string> imageFileKeys { get; set; }
    }
}
