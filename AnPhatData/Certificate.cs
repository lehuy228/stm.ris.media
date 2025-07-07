using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace PrintToPACSDemo.AnPhat.Data
{
    public class Certificate
    {
        public const string PrimaryKey = "ID";
        public int ID { get; set; }
        public string StaffCode { get; set; }
        public string Username { get; set; }
        public string CN { get; set; }
        public string Password { get; set; }
        public X509Certificate2 CertificateData { get; set; }
        public byte[] ImageSigner { get; set; }
    }
}
