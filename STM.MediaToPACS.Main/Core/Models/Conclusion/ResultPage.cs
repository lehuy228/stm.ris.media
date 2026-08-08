using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models
{
    public class ResultPage<T>
    {
        public List<T> data { get; set; }
        public int total { get; set; }
    }

    public class CancelSignedFileResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public int Status { get; set; }
    }
}
