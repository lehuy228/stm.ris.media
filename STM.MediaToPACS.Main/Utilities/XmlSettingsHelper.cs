using MediaToPacs.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace STM.MediaToPACS.Main.Utilities
{
    public static class XmlSettingsHelper
    {
        private static readonly byte[] _key;
        private static readonly byte[] _iv;

        // ================== INIT KEY ==================
        static XmlSettingsHelper()
        {
            using (var sha = SHA256.Create())
            {
                _key = sha.ComputeHash(
                    Encoding.UTF8.GetBytes("STM-CONFIG-KEY-2025")
                ); // 32 bytes
            }

            using (var md5 = MD5.Create())
            {
                _iv = md5.ComputeHash(
                    Encoding.UTF8.GetBytes("STM-CONFIG-IV")
                ); // 16 bytes
            }
        }

        ///
        /// Lưu object thành file XML
        ///
        public static void Save<T>(string filePath, T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fs, obj);
            }
        }

        /// <summary>
        /// Chỉ load object từ file XML
        /// Nếu file không tồn tại hoặc lỗi thì trả về null
        /// </summary>
        public static T Load<T>(string filePath) where T : class
        {
            if (!File.Exists(filePath))
                return null;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    return (T)serializer.Deserialize(fs);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary> /// 
        /// Kiểm tra file, nếu chưa có thì tạo mới và lưu /// 
        /// </summary>
        public static void EnsureFileExists<T>(string filePath, Func<T> factory) where T : class
        {
            if (!File.Exists(filePath))
            {
                if (factory == null)
                    throw new ArgumentNullException(nameof(factory));
                T obj = factory(); Save(filePath, obj);
            }
        }


        #region xml bảo mật

        // ================== SAVE (ENCRYPT) ==================
        public static void SaveEncrypted<T>(string filePath, T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            var serializer = new XmlSerializer(typeof(T));

            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;
                aes.Padding = PaddingMode.PKCS7;

                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                using (var cryptoStream = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    serializer.Serialize(cryptoStream, obj);
                }
            }
        }

        // ================== LOAD (DECRYPT) ==================
        public static T LoadEncrypted<T>(string filePath) where T : class
        {
            if (!File.Exists(filePath))
                return null;

            try
            {
                var serializer = new XmlSerializer(typeof(T));

                using (var aes = Aes.Create())
                {
                    aes.Key = _key;
                    aes.IV = _iv;
                    aes.Padding = PaddingMode.PKCS7;

                    using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    using (var cryptoStream = new CryptoStream(fs, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        return serializer.Deserialize(cryptoStream) as T;
                    }
                }
            }
            catch
            {
                // Sai key / file hỏng / xml lỗi
                return null;
            }
        }

        // ================== ENSURE FILE ==================
        public static void EnsureEncryptedFile<T>(string filePath, Func<T> factory)
            where T : class
        {
            if (!File.Exists(filePath))
            {
                if (factory == null)
                    throw new ArgumentNullException(nameof(factory));

                SaveEncrypted(filePath, factory());
            }
        }
        #endregion
    }
}
