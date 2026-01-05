using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models.Ketluan
{
    public class ReportTemplateResponse
    {
        public string Name { get; set; }
        public string Modality { get; set; }
        public string ProcedureCode { get; set; }
        public string Content { get; set; }
        public List<string> Placeholders { get; set; }
        public bool IsEmergency { get; set; }
        public string XmlTemplate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Id { get; set; }
    }
}
