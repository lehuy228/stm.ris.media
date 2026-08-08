using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models
{
    public class StudyOr
    {
        public string ID { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public DateTime PatientDate { get; set; }
        public string PatientSex { get; set; }
        public string PatientDescription { get; set; }
        public DateTime StudyDate { get; set; }
        public string AccessionNumber { get; set; }
        public string ReferD { get; set; }
        public string ModalitiesInStudy { get; set; }
        public string StudyInstanceUID { get; set; }

    }
}
