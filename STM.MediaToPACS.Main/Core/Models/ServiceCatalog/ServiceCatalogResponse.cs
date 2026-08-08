using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models.ServiceCatalog
{
    public class ServiceCatalogResponse
    {
        public string version { get; set; }
        public string madichvu { get; set; }
        public string tendichvu { get; set; }
        public string modality { get; set; }
        public bool chibn100 { get; set; }
        public decimal gia_yeucau { get; set; }
        public DateTime createdAt { get; set; }
        public string id { get; set; }
    }
}
