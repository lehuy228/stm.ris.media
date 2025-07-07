using System.Data.SqlClient;
using System.Data;

namespace PrintToPACSDemo.AnPhat.Data
{
    public class DataAccess
    {
        #region Connection String
        private static string _ConnectionString = null;

        /// <summary>
        /// Lấy connection string từ web.config
        /// </summary>        
        public static SqlConnection GetConnection()
        {
            try
            {

                if (string.IsNullOrEmpty(_ConnectionString))
                    _ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["ConnStr"];

                SqlConnection connection = new SqlConnection(_ConnectionString);
                connection.Open();
                return connection;
            }
            catch { return null; }
        }
        #endregion

        #region General SQL Query
        /// <summary>
        /// Chạy các câu lệnh SQL truy vấn dữ liệu
        /// </summary>
        /// <param name="cmdText">Câu lệnh SQL</param>
        /// <returns>DataTable chứa dữ liệu truy vấn</returns>
        public static DataTable ExecuteQuery(string cmdText)
        {
            SqlConnection connection = DataAccess.GetConnection();
            if (connection == null)
                return null;
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmdText, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                connection.Close();
                return dt;
            }
            catch { return null; }
            finally { if (connection != null) connection.Close(); }
        }

        /// <summary>
        /// Chạy các câu lệnh SQL không truy vấn
        /// </summary>
        /// <param name="cmdText">Câu lệnh SQL</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        public static int ExecuteNonQuery(string cmdText)
        {
            SqlConnection connection = DataAccess.GetConnection();
            if (connection == null)
                return 0;
            try
            {
                SqlCommand cmd = new SqlCommand(cmdText, connection);
                int result = cmd.ExecuteNonQuery();
                connection.Close();
                return result;
            }
            catch { return 0; }
            finally { if (connection != null) connection.Close(); }
        }

        /// <summary>
        /// Chạy các câu lệnh SQL truy vấn 1 giá trị
        /// </summary>
        /// <param name="cmdText">Câu lệnh SQL</param>
        /// <returns>Giá trị trả về từ truy vấn</returns>
        public static object ExecuteScalar(string cmdText)
        {
            SqlConnection connection = DataAccess.GetConnection();
            if (connection == null)
                return null;
            try
            {
                SqlCommand cmd = new SqlCommand(cmdText, connection);
                object result = cmd.ExecuteScalar();
                connection.Close();
                return result;
            }
            catch { return null; }
            finally { if (connection != null) connection.Close(); }
        }

        /// <summary>
        /// Chạy Stored Procedure không tham số, không trả về kết quả
        /// </summary>
        /// <param name="spName">Tên Stored Procedure</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        public static int ExecuteStoredProcedure(string spName)
        {
            SqlConnection connection = DataAccess.GetConnection();
            if (connection == null)
                return 0;
            try
            {
                SqlCommand cmd = new SqlCommand(spName, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                int result = cmd.ExecuteNonQuery();
                connection.Close();
                return result;
            }
            catch { return 0; }
            finally { if (connection != null) connection.Close(); }
        }

        #endregion

        #region Transaction Support
        /// <summary>
        /// Chạy các câu lệnh SQL truy vấn dữ liệu, hỗ trợ transaction
        /// </summary>
        /// <param name="cmdText">Câu lệnh SQL</param>
        /// <param name="connection">Kết nối</param>
        /// <returns>DataTable chứa dữ liệu truy vấn</returns>
        public static DataTable ExecuteQuery(string cmdText, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(cmdText, connection, transaction);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// Chạy câu lệnh SQL không truy vấn, hỗ trợ transaction
        /// </summary>
        /// <param name="cmdText">Câu lệnh SQL</param>
        /// <param name="connection">Kết nối</param>
        /// <param name="transaction">Transaction</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        public static int ExecuteNonQuery(string cmdText, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(cmdText, connection, transaction);
                int i = cmd.ExecuteNonQuery();
                return i;
            }
            catch
            {
                return 0;
            }

        }

        /// <summary>
        /// Chạy câu lệnh SQL truy vấn 1 giá trị, hỗ trợ transaction
        /// </summary>
        /// <param name="cmdText">Câu lệnh SQL</param>
        /// <param name="connection">Kết nối</param>
        /// <param name="transaction">Transaction</param>
        /// <returns>Giá trị trả về từ truy vấn</returns>
        public static object ExecuteScalar(string cmdText, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(cmdText, connection, transaction);
                object o = cmd.ExecuteScalar();
                return o;
            }
            catch
            {
                return null;
            }

        }
        #endregion
    }
}
