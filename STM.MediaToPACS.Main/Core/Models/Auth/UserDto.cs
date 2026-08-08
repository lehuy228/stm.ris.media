using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserID { get; set; }
        public string ImageUri { get; set; }
        public string imageData { get; set; }
        public CertificateDto Certificate { get; set; }
    }

    public class CertificateDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string CredentialId { get; set; }
        public string Status { get; set; }
        public string Certificates { get; set; }
        public string IssuerDN { get; set; }
        public string SerialNumber { get; set; }
        public string SubjectDN { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
    }
}
