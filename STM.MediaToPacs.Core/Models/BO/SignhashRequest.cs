using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models
{
    public class SignhashRequest
    {
        public string FileName { get; set; }
        public string FileBase64 { get; set; }
        public string UserID { get; set; }
        public int numberPageSign { get; set; }
        public float coorX { get; set; }
        public float coorY { get; set; }
        public float width { get; set; }
        public float height { get; set; }
    }
}
