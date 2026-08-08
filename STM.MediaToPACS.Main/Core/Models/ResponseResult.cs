using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models
{
    public class ResponseResult<T>
    {
        public T data { get; set; }
        public int statusCode { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
    }
}
