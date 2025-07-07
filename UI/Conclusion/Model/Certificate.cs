using PrintToPACSDemo.AnPhat.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PrintToPACSDemo.UI.Conclusion.Model
{
    public class Certificate
    {
        public int ID { get; set; }
        public string StaffID { get; set; }
        public string Username { get; set; }
        public string CN { get; set; }
        public string Password { get; set; }
        public X509Certificate2 CertificateData { get; set; }

        public byte[] ImageSigner { get; set; }

        public static Certificate GetCertificateInfo(string username)
        {
            Certificate certInfo = new Certificate();
            string query = @"SELECT [CertificateData], [Password], [ImageSigner], [StaffID], [Username], [CN] 
                     FROM [dbo].[Certificate] WHERE [Username] = @Username";

                SqlConnection conn = DataAccess.GetConnection();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var blob = new byte[reader.GetBytes(0, 0, null, 0, int.MaxValue)];
                    reader.GetBytes(0, 0, blob, 0, blob.Length);
                    certInfo.ImageSigner = new byte[reader.GetBytes(2, 0, null, 0, int.MaxValue)];
                    reader.GetBytes(2, 0, certInfo.ImageSigner, 0, certInfo.ImageSigner.Length);
                    certInfo.Password = reader.GetString(1);



                    certInfo.CertificateData = new X509Certificate2(blob, certInfo.Password, X509KeyStorageFlags.MachineKeySet);
                    certInfo.StaffID = reader.GetString(3);
                    certInfo.Username = reader.GetString(4);
                    certInfo.CN = reader.GetString(5);
                }

            return certInfo;
        }
    }
}
