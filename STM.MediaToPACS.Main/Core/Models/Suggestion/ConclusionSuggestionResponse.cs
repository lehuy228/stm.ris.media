using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models.Suggestion
{
    public class ConclusionSuggestionResponse
    {
        public string name { get; set; }
        public string madichvu { get; set; }
        public string kqcls_denghi { get; set; }
        public string kqcls_mota { get; set; }
        public string kqcls_ketluan { get; set; }
        public DateTime createdAt { get; set; }
        public string gioitinh { get; set; }
        public string mathietbi { get; set; }
        public string id { get; set; }

        public override string ToString()
        {
            return name; // hiển thị name trong combobox
        }
    }
}
