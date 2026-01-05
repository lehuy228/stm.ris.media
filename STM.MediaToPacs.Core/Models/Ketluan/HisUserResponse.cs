using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models.Ketluan
{
    public class HisUserResponse
    {
        public string his_id { get; set; }
        public string user_name { get; set; }
        public string full_name { get; set; }
        public bool is_synced { get; set; }
        public string id { get; set; }

        public override string ToString()
        {
            return full_name;
        }
    }
}
