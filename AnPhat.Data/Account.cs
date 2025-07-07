using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PrintToPACSDemo.AnPhat.Data
{
    public class Account : BaseEntity
    {
        public const string PrimaryKey = "ID";
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Name { get; set; }
        public string Identification { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public string StaffID { get; set; }
        public DateTime DOB { get; set; }
        public bool StatusConfirm { get; set; }
        public string Secret { get; set; }

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

        public void CreateUser(Account create)
        {
            string password = create.Password;
            byte[] saltBytes = GenerateSalt();
            // Hash the password with the salt
            string hashedPassword = HashPassword(password, saltBytes);
            string base64Salt = Convert.ToBase64String(saltBytes);

            create.Password = hashedPassword;
            create.Salt = base64Salt;
        }

        public static Account UserVerify(string Username, string Password)
        {
            var account = BaseEntity.getByField<Account>("Username", Username);
            if (string.IsNullOrEmpty(account.Username))
            {
                return null;
            }
            string storedHashedPassword = account.Password;// "hashed_password_from_database";
            //string storedSalt = user.Salt; //"salt_from_database";
            byte[] storedSaltBytes = Convert.FromBase64String(account.Salt);
            string enteredPassword = Password; //"user_entered_password";

            // Convert the stored salt and entered password to byte arrays
            // byte[] storedSaltBytes = Convert.FromBase64String(user.Salt);
            byte[] enteredPasswordBytes = Encoding.UTF8.GetBytes(enteredPassword);

            // Concatenate entered password and stored salt
            byte[] saltedPassword = new byte[enteredPasswordBytes.Length + storedSaltBytes.Length];
            Buffer.BlockCopy(enteredPasswordBytes, 0, saltedPassword, 0, enteredPasswordBytes.Length);
            Buffer.BlockCopy(storedSaltBytes, 0, saltedPassword, enteredPasswordBytes.Length, storedSaltBytes.Length);

            // Hash the concatenated value
            string enteredPasswordHash = HashPassword(enteredPassword, storedSaltBytes);

            // Compare the entered password hash with the stored hash
            if (enteredPasswordHash == storedHashedPassword)
            {
                return account;
            }
            else
            {
                return null;
            }
        }

        public static Account GetAccount(string username)
        {
            Account account = null;
            string query = "SELECT * FROM Account WHERE Username = @Username";

            SqlConnection conn = DataAccess.GetConnection();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Username", username);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                account = new Account
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    Username = reader["Username"].ToString(),
                    Password = reader["Password"].ToString(),
                    Salt = reader["Salt"].ToString(),
                    Name = reader["Name"].ToString(),
                    Identification = reader["Identification"].ToString(),
                    Email = reader["Email"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    Department = reader["Department"].ToString(),
                    StaffID = reader["StaffID"].ToString(),
                    DOB = Convert.ToDateTime(reader["DOB"]),
                    StatusConfirm = Convert.ToBoolean(reader["StatusConfirm"]),
                    Secret = reader["Secret"].ToString()
                };
            }

            reader.Close();

            return account;
        }

        public static bool UpdateAccountSecret(Account account)
        {
            string query = "UPDATE Account SET Secret = @Secret WHERE ID = @ID";

            SqlConnection conn = DataAccess.GetConnection();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Secret", account.Secret);
            cmd.Parameters.AddWithValue("@ID", account.ID);

            conn.Open();
            int rowsAffected = cmd.ExecuteNonQuery();

            // Kiểm tra nếu có hàng nào bị ảnh hưởng (tức là đã cập nhật thành công)
            return rowsAffected > 0;
        }
    }
}
