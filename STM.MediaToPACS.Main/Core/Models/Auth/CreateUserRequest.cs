using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models
{
    public class CreateUserRequest
    {
        public CreateUpdateUserDto User { get; set; }
        public CertBO Certificate { get; set; }
        public string DataImage { get; set; }
        public string credentialId { get; set; }
    }

    public class CreateUpdateUserDto
    {
        public string UserID { get; set; }
    }
}
