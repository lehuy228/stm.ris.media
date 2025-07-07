using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintToPACSDemo.UI.Conclusion
{
    public class ConclusionImage 
    {
        public const string PrimaryKey = "ID";
        public int ID { get; set; }
        public string StudyInstanceUID { get; set; }
        public string ReferencedFileLocal { get; set; }
    }
}
