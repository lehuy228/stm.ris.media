using System;
using System.Security.Cryptography;
using System.Text;

namespace PrintToPACSDemo.AnPhat.Data
{
    public class Staff 
    {
        public int ID { get; set; }
        public string StaffDepartment { get; set; }
        public string StaffCode { get; set; }
        public string StaffName { get; set; }
        public DateTime StaffBirth { get; set; }
        public string StaffGender { get; set; }
        public string StaffPosition { get; set; }
        public string StaffTitle { get; set; }
        public string StaffPhone { get; set; }
        public string StaffEmail { get; set; }
        public string StaffLocation { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Secret { get; set; }
        public bool IsAdmin { get; set; }

        static string HashPassword(string password, byte[] salt)
        {
            using (var sha256 = new SHA256Managed())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];

                // Concatenate password and salt
                Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
                Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

                // Hash the concatenated password and salt
                byte[] hashedBytes = sha256.ComputeHash(saltedPassword);

                // Concatenate the salt and hashed password for storage
                byte[] hashedPasswordWithSalt = new byte[hashedBytes.Length + salt.Length];
                Buffer.BlockCopy(salt, 0, hashedPasswordWithSalt, 0, salt.Length);
                Buffer.BlockCopy(hashedBytes, 0, hashedPasswordWithSalt, salt.Length, hashedBytes.Length);

                return Convert.ToBase64String(hashedPasswordWithSalt);
            }
        }

        static byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16]; // Adjust the size based on your security requirements
                rng.GetBytes(salt);
                return salt;
            }
        }

        public void CreateUser(Staff staff)
        {
            string password = staff.Password;
            byte[] saltBytes = GenerateSalt();
            // Hash the password with the salt
            string hashedPassword = HashPassword(password, saltBytes);
            string base64Salt = Convert.ToBase64String(saltBytes);

            staff.Password = hashedPassword;
            staff.Salt = base64Salt;
        }
    }
}
