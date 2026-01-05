using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models.Ketluan
{
    public class ReportTemplateGridViewModel
    {
        public string Name { get; set; }
        public string Modality { get; set; }
        public bool IsEmergency { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Id { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
