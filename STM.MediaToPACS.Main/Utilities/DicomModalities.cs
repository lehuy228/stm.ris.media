using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STM.MediaToPACS.Main.Utilities
{
    public static class DicomModalities
    {
        public static readonly List<string> All = new List<string>
        {
            "ES",
            "CR",
            "CT",
            "DOC",
            "DR",
            "DX",
            "MG",
            "MR",
            "NM",
            "OT",
            "PT",
            "PX",
            "SEG",
            "SR",
            "XA",
            "US",
            "XC"
        };
    }
}
