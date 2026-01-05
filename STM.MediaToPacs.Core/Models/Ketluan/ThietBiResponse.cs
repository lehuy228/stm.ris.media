using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models.Ketluan
{
    public class ThietBiResponse
    {
        public string tenThietBi { get; set; }
        public string loaiThietBi { get; set; }
        public string trangThaiThietBi { get; set; }
        public string maThietBi { get; set; }
        public List<string> modalities { get; set; }
        public List<string> kyThuatVienPhuTrach { get; set; }
        public string id { get; set; }

        public override string ToString()
        {
            return tenThietBi; // hiển thị name trong combobox
        }
    }
}
