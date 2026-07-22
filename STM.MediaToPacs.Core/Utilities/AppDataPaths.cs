using System;
using System.IO;

namespace MediaToPacs.Core.Utilities
{
    /// <summary>
    /// Thư mục lưu cấu hình/dữ liệu ứng dụng ở tầng ProgramData - ổn định qua các lần
    /// cập nhật/cài lại ứng dụng.
    /// </summary>
    public static class AppDataPaths
    {
        private const string AppFolderName = "STM.MediaToPACS";

        public static string GetAppDataBasePath()
        {
            string basePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                AppFolderName);

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            return basePath;
        }
    }
}
