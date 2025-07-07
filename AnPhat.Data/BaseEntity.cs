using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PrintToPACSDemo.AnPhat.Data
{
    public class BaseEntity
    {
        public void Update()
        {
            SqlConnection connection = DataAccess.GetConnection();
            if (connection == null)
                return;
            Update(connection, null);
            connection.Close();
        }

        public string Insert()
        {
            SqlConnection connection = DataAccess.GetConnection();
            if (connection == null)
                return "";

            string Id = Insert(connection, null);
            connection.Close();

            return Id;
        }

        public void Delete()
        {
            SqlConnection connection = DataAccess.GetConnection();
            if (connection == null)
                return;
            Delete(connection, null);
            connection.Close();
        }

        public static void Delete<T>(object id) where T : BaseEntity
        {
            SqlConnection connection = DataAccess.GetConnection();
            if (connection == null)
                return;
            Delete<T>(connection, null, id);
            connection.Close();
        }

        public static T getById<T>(object id) where T : new()
        {
            SqlConnection connection = DataAccess.GetConnection();
            if (connection == null)
                return default(T);

            return getById<T>(connection, null, id);
        }

        public static T getByField<T>(string byFieldName, object byFieldValue) where T : new()
        {
            SqlConnection connection = DataAccess.GetConnection();
            if (connection == null)
                return default(T);

            return GetObjByField<T>(connection, null, byFieldName, byFieldValue);
        }

        public static T getByFieldList<T>(SortedList listParam) where T : new()
        {
            SqlConnection connection = DataAccess.GetConnection();
            if (connection == null)
                return default(T);

            return getByFieldList<T>(connection, null, listParam);
        }

        public static object GetFieldById<T>(string fieldName, object id) where T : BaseEntity
        {
            SqlConnection connection = DataAccess.GetConnection();
            if (connection == null)
                return default(T);

            return GetFieldById<T>(connection, null, fieldName, id);
        }

        public static object GetFieldByField<T>(string getFieldName, string byFieldName, object byFieldValue) where T : BaseEntity
        {
            SqlConnection connection = DataAccess.GetConnection();
            if (connection == null)
                return default(T);

            return GetFieldByField<T>(connection, null, getFieldName, byFieldName, byFieldValue);
        }

        public static List<T> GetListByField<T>(string fieldName, object fieldValue, int top = 0) where T : new()
        {
            SqlConnection connection = DataAccess.GetConnection();
            if (connection == null)
                return null;

            return GetListByField<T>(connection, null, fieldName, fieldValue, top);
        }


        public static List<T> GetList<T>(int top = 0) where T : new()
        {
            SqlConnection connection = DataAccess.GetConnection();
            if (connection == null)
                return null;

            return GetList<T>(connection, null, "", "", top);
        }

        public static List<T> GetList<T>(string where, string orderBy, int top = 0) where T : new()
        {
            SqlConnection connection = DataAccess.GetConnection();
            if (connection == null)
                return null;

            return GetList<T>(connection, null, where, orderBy, top);
        }

        public static PagedList<T> GetPageList<T>(string where, string orderBy, int pageIndex = 0, int pageSize = 10) where T : new()
        {
            SqlConnection connection = DataAccess.GetConnection();
            if (connection == null)
                return null;

            return GetPageList<T>(connection, null, where, orderBy, pageIndex, pageSize);
        }

        public static void SetFieldValue<T>(string fieldName, object fieldValue, object id) where T : BaseEntity
        {
            SqlConnection connection = DataAccess.GetConnection();
            if (connection == null)
                return;

            SetFieldValue<T>(connection, null, fieldName, fieldValue, id);
        }

        #region Transaction Support
        public static void SetFieldValue<T>(SqlConnection connection, SqlTransaction transaction, string fieldName, object fieldValue, object id) where T : BaseEntity
        {
            try
            {
                string PrimaryKey = typeof(T).GetField("PrimaryKey").GetValue(null).ToString();
                string query = "UPDATE " + typeof(T).Name + " SET " + fieldName + "=@" + fieldName + " WHERE " + PrimaryKey + "=@Id";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                if (transaction != null)
                    cmd.Transaction = transaction;
                cmd.Prepare();

                cmd.Parameters.Add(GetSqlParameter("" + fieldName, fieldValue));
                cmd.Parameters.Add(GetSqlParameter("Id", id));
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string Insert(SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                string PrimaryKey = GetPrimaryKey();
                string query = "INSERT INTO " + this.GetType().Name + " (";
                string values = "";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                if (transaction != null)
                    cmd.Transaction = transaction;
                cmd.Prepare();
                foreach (System.Reflection.PropertyInfo pi in this.GetType().GetProperties())
                {
                    if (pi.Name != PrimaryKey)
                    {
                        object o = pi.GetValue(this, null);
                        if (o != null && o.ToString() != "")
                        {
                            query += pi.Name + ",";
                            values += "@" + pi.Name + ",";
                            cmd.Parameters.Add(GetSqlParameter("" + pi.Name, o));
                        }
                    }
                }
                query = query.Substring(0, query.Length - 1) + ") VALUES (" + values.Substring(0, values.Length - 1) + ")";
                cmd.CommandText = query + " SELECT SCOPE_IdENTITY()";


                int Id = int.Parse(cmd.ExecuteScalar().ToString());
                //string Id = cmd.ExecuteScalar().ToString();
                this.GetType().GetProperty(PrimaryKey).SetValue(this, Id, null);
                return Id.ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                string PrimaryKey = GetPrimaryKey();
                string query = "UPDATE " + this.GetType().Name + " SET ";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                if (transaction != null)
                    cmd.Transaction = transaction;
                cmd.Prepare();
                foreach (System.Reflection.PropertyInfo pi in this.GetType().GetProperties())
                {
                    if (pi.Name != PrimaryKey)
                    {
                        query += pi.Name + " = @" + pi.Name + ",";
                    }
                    cmd.Parameters.Add(GetSqlParameter("" + pi.Name, pi.GetValue(this, null)));
                }
                query = query.Substring(0, query.Length - 1);
                query += " WHERE " + PrimaryKey + "= @" + PrimaryKey;
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Delete(SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                string PrimaryKey = GetPrimaryKey();
                foreach (System.Reflection.PropertyInfo pi in this.GetType().GetProperties())
                {
                    if (pi.Name == PrimaryKey)
                    {
                        object o = pi.GetValue(this, null);
                        if (o != null)
                        {
                            if (connection != null)
                            {
                                SqlCommand cmd = new SqlCommand("DELETE FROM " + this.GetType().Name + " WHERE " + PrimaryKey + " = @Id");
                                cmd.Connection = connection;
                                if (transaction != null)
                                    cmd.Transaction = transaction;
                                cmd.Prepare();
                                cmd.Parameters.Add(GetSqlParameter("Id", o));
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public static void Delete<T>(SqlConnection connection, SqlTransaction transaction, object id) where T : BaseEntity
        {
            try
            {
                string PrimaryKey = typeof(T).GetField("PrimaryKey").GetValue(null).ToString();

                if (connection != null)
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM " + typeof(T).Name + " WHERE " + PrimaryKey + " = @Id");
                    cmd.Connection = connection;
                    if (transaction != null)
                        cmd.Transaction = transaction;
                    cmd.Prepare();
                    cmd.Parameters.Add(GetSqlParameter("Id", id));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static T getByFieldList<T>(SqlConnection connection, SqlTransaction transaction, SortedList listParam) where T : new()
        {
            try
            {
                T newT = new T();
                string PrimaryKey = newT.GetType().GetField("PrimaryKey").GetValue(null).ToString();
                string tbName = newT.GetType().Name;
                string query = "SELECT * FROM " + tbName + " WHERE (1=1) ";
                SqlCommand cmd = new SqlCommand();

                foreach (string key in listParam.Keys)
                {
                    query += " AND " + key + " = @" + key;
                    object value = listParam[key];
                    string type = value.GetType().Name;
                    if (type == "String")
                    {
                        SqlParameter para = new SqlParameter("@" + key, SqlDbType.NVarChar, value.ToString().Length);
                        para.Value = value;
                        cmd.Parameters.Add(para);
                    }
                    else
                        cmd.Parameters.Add(GetSqlParameter("" + key, value));
                }
                cmd.Connection = connection;
                cmd.CommandText = query;
                if (transaction != null)
                    cmd.Transaction = transaction;
                cmd.Prepare();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        foreach (System.Reflection.PropertyInfo pi in newT.GetType().GetProperties())
                        {
                            pi.SetValue(newT, ConvertToCsharpData(dr[pi.Name], pi.PropertyType), null);
                        }
                    }
                }
                connection.Close();
                return newT;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                connection.Close();
                return default(T);
            }
        }
        public static T getById<T>(SqlConnection connection, SqlTransaction transaction, object id) where T : new()
        {
            try
            {
                T newT = new T();
                string PrimaryKey = newT.GetType().GetField("PrimaryKey").GetValue(null).ToString();
                string tbName = newT.GetType().Name;
                SqlCommand cmd = new SqlCommand("SELECT * FROM " + tbName + " WHERE " + PrimaryKey + " = @Id", connection);
                cmd.Parameters.Add(GetSqlParameter("Id", id));
                if (transaction != null)
                    cmd.Transaction = transaction;
                cmd.Prepare();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        foreach (System.Reflection.PropertyInfo pi in newT.GetType().GetProperties())
                        {
                            pi.SetValue(newT, ConvertToCsharpData(dr[pi.Name], pi.PropertyType), null);
                        }
                    }
                }
                connection.Close();
                return newT;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                connection.Close();
                return default(T);
            }
        }

        public static object GetFieldById<T>(SqlConnection connection, SqlTransaction transaction, string fieldName, object id) where T : BaseEntity
        {
            try
            {
                string PrimaryKey = typeof(T).GetField("PrimaryKey").GetValue(null).ToString();
                string tbName = typeof(T).Name;
                SqlCommand cmd = new SqlCommand("SELECT " + fieldName + " FROM " + tbName + " WHERE " + PrimaryKey + " = @Id", connection);
                cmd.Parameters.Add(GetSqlParameter("Id", id));
                if (transaction != null)
                    cmd.Transaction = transaction;
                cmd.Prepare();
                object o = cmd.ExecuteScalar();
                connection.Close();

                return o;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                connection.Close();
                return null;
            }
        }

        public static object GetFieldByField<T>(SqlConnection connection, SqlTransaction transaction, string getFieldName, string byFieldName, object byFieldValue) where T : BaseEntity
        {
            try
            {
                string tbName = typeof(T).Name;
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 " + getFieldName + " FROM " + tbName + " WHERE " + byFieldName + " = @ByField", connection);
                cmd.Parameters.Add(GetSqlParameter("ByField", byFieldValue));
                if (transaction != null)
                    cmd.Transaction = transaction;
                cmd.Prepare();
                object o = cmd.ExecuteScalar();
                connection.Close();

                return o;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                connection.Close();
                return null;
            }
        }

        public static T GetObjByField<T>(SqlConnection connection, SqlTransaction transaction, string byFieldName, object byFieldValue) where T : new()
        {
            try
            {
                T newT = new T();
                string tbName = typeof(T).Name;
                SqlCommand cmd = new SqlCommand("SELECT * FROM " + tbName + " WHERE " + byFieldName + " = @ByField", connection);
                cmd.Parameters.Add(GetSqlParameter("ByField", byFieldValue));
                if (transaction != null)
                    cmd.Transaction = transaction;
                cmd.Prepare();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        foreach (System.Reflection.PropertyInfo pi in newT.GetType().GetProperties())
                        {
                            pi.SetValue(newT, ConvertToCsharpData(dr[pi.Name], pi.PropertyType), null);
                        }
                    }
                }
                connection.Close();
                return newT;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                connection.Close();
                return default(T);
            }
        }

        public static List<T> GetListByField<T>(SqlConnection connection, SqlTransaction transaction, string fieldName, object fieldValue, int top = 0) where T : new()
        {
            try
            {
                List<T> list = new List<T>();
                string tbName = typeof(T).Name;
                SqlCommand cmd = new SqlCommand("SELECT" + (top > 0 ? " TOP " + top : "") + " * FROM " + tbName +
                    " WHERE " + fieldName + " = @value", connection);
                cmd.Parameters.Add(GetSqlParameter("value", fieldValue));
                if (transaction != null)
                    cmd.Transaction = transaction;
                cmd.Prepare();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        T child = new T();
                        foreach (System.Reflection.PropertyInfo pi in child.GetType().GetProperties())
                        {
                            pi.SetValue(child, ConvertToCsharpData(dr[pi.Name], pi.PropertyType), null);
                        }
                        list.Add(child);
                    }
                }
                connection.Close();
                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                connection.Close();
                return null;
            }
        }

        public static List<T> GetList<T>(SqlConnection connection, SqlTransaction transaction, string where,
            string orderBy = "", int top = 0) where T : new()
        {
            try
            {
                List<T> list = new List<T>();
                string tbName = typeof(T).Name;
                SqlCommand cmd = new SqlCommand("SELECT" + (top > 0 ? " TOP " + top : "") + " * FROM " + tbName +
                    (where != "" ? " WHERE " + where : "") + (orderBy != "" ? " ORDER BY " + orderBy : ""), connection);
                if (transaction != null)
                    cmd.Transaction = transaction;
                cmd.Prepare();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        T child = new T();
                        foreach (System.Reflection.PropertyInfo pi in child.GetType().GetProperties())
                        {
                            pi.SetValue(child, ConvertToCsharpData(dr[pi.Name], pi.PropertyType), null);
                        }
                        list.Add(child);
                    }
                }
                connection.Close();
                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                connection.Close();
                return null;
            }
        }
        public static PagedList<T> GetPageList<T>(SqlConnection connection, SqlTransaction transaction,
            string where, string orderBy, int pageIndex = 0, int pageSize = 20) where T : new()
        {
            try
            {
                PagedList<T> plist = new PagedList<T>() { PageIndex = pageIndex, PageSize = pageSize };
                string tbName = typeof(T).Name;
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM " + tbName + (where != "" ? " WHERE " + where : ""), connection);
                if (transaction != null)
                    cmd.Transaction = transaction;
                cmd.Prepare();
                plist.TotalCount = Convert.ToInt16(cmd.ExecuteScalar());
                cmd.CommandText = "SELECT * FROM (SELECT *,ROW_NUMBER() OVER(ORDER BY " + orderBy + ") AS ROMS_NUMBER FROM " + tbName +
                    (where != "" ? " WHERE " + where : "") + ") AS A"
                    + " WHERE ROMS_NUMBER BETWEEN " + (pageIndex * pageSize + 1) + " AND " + (pageIndex + 1) * pageSize;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        T child = new T();
                        foreach (System.Reflection.PropertyInfo pi in child.GetType().GetProperties())
                        {
                            pi.SetValue(child, ConvertToCsharpData(dr[pi.Name], pi.PropertyType), null);
                        }
                        plist.Add(child);
                    }
                }
                connection.Close();
                return plist;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                connection.Close();
                return null;
            }
        }

        #endregion

        private static SqlParameter GetSqlParameter(string key, object value)
        {
            SqlParameter para;

            if (value != null)
            {
                Type type = value.GetType();
                if (type == typeof(string))
                    para = new SqlParameter("@" + key, SqlDbType.NVarChar, value.ToString().Length);
                else
                    para = new SqlParameter("@" + key, TypeConvertor.ToSqlDbType(type));
                para.Value = value;
            }
            else
            {
                para = new SqlParameter("@" + key, SqlDbType.NVarChar, 1);
                para.Value = "";
            }


            return para;
        }

        private string GetPrimaryKey()
        {
            try
            {
                var pi = this.GetType().GetField("PrimaryKey");
                return pi.GetValue(null).ToString();
            }
            catch { return ""; }
        }

        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        public static object ConvertToCsharpData(object data, Type type)
        {
            if (data == null || type == typeof(DBNull))
                return null;

            try
            {
                string dataStr = data.ToString();
                if (type == typeof(int) || type == typeof(int?))
                    return int.Parse(dataStr);
                if (type == typeof(string))
                    return dataStr;
                if (type == typeof(bool) || type == typeof(bool?))
                    return bool.Parse(dataStr);
                if (type == typeof(DateTime) || type == typeof(DateTime?))
                    return DateTime.Parse(dataStr);
                if (type == typeof(decimal) || type == typeof(decimal?))
                    return decimal.Parse(dataStr);
                if (type == typeof(double) || type == typeof(double?))
                    return double.Parse(dataStr);
                if (type == typeof(float) || type == typeof(float?))
                    return float.Parse(dataStr);
                if (type == typeof(byte[]))
                    return ObjectToByteArray(data);
                if (type == typeof(short) || type == typeof(short?))
                    return short.Parse(dataStr);
                if (type == typeof(byte) || type == typeof(byte?))
                    return byte.Parse(dataStr);
                if (type == typeof(long) || type == typeof(long?))
                    return long.Parse(dataStr);
                if (type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?))
                    return DateTimeOffset.Parse(dataStr);

                return data;
            }
            catch { return null; }
        }
    }
}
