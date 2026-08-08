using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models.Template
{
    public class ReportTemplateRequest
    {
        public string name { get; set; }
        public string modality { get; set; }
        public string procedureCode { get; set; }
        public string content { get; set; }
        public string placeholders { get; set; }
        public string xmlTemplate { get; set; }

        public byte[] FileBytes { get; set; }
        public string FileName { get; set; }
    }
}
