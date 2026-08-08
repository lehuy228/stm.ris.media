using MediaToPacs.Core.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace MediaToPacs.Core.Utilities
{
    /// <summary>
    /// Load/save cấu hình tên file/đường dẫn từ AppFileSettings.json (ProgramData).
    /// Nếu file JSON chưa tồn tại, tạo mới với giá trị mặc định. Nếu đã tồn tại nhưng
    /// thiếu field (nâng cấp từ bản cũ hoặc bị sửa tay thiếu), tự điền giá trị mặc định
    /// cho các field null rồi lưu lại.
    /// </summary>
    public static class FileStorageSettingsProvider
    {
        private const string SettingsFileName = "AppFileSettings.json";
        private static readonly object _sync = new object();
        private static FileStorageSettings _current;

        public static FileStorageSettings Current
        {
            get
            {
                if (_current != null)
                    return _current;

                lock (_sync)
                {
                    if (_current == null)
                        _current = LoadOrCreateDefault();

                    return _current;
                }
            }
        }

        private static string SettingsPath => Path.Combine(AppDataPaths.GetAppDataBasePath(), SettingsFileName);

        private static FileStorageSettings LoadOrCreateDefault()
        {
            var settings = LoadFromDisk();
            if (settings == null)
            {
                settings = new FileStorageSettings();
                Save(settings);
                return settings;
            }

            if (ApplyDefaults(settings))
                Save(settings);

            return settings;
        }

        private static FileStorageSettings LoadFromDisk()
        {
            try
            {
                string path = SettingsPath;
                if (!File.Exists(path))
                    return null;

                return JsonConvert.DeserializeObject<FileStorageSettings>(File.ReadAllText(path));
            }
            catch (Exception ex)
            {
                Trace.TraceWarning("Không thể đọc AppFileSettings.json: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Điền giá trị mặc định cho các field null/rỗng. Trả về true nếu có thay đổi.
        /// </summary>
        private static bool ApplyDefaults(FileStorageSettings settings)
        {
            var defaults = new FileStorageSettings();
            var changed = false;

            if (string.IsNullOrWhiteSpace(settings.SystemConfigFile))
            {
                settings.SystemConfigFile = defaults.SystemConfigFile;
                changed = true;
            }

            if (string.IsNullOrWhiteSpace(settings.Modality))
            {
                settings.Modality = defaults.Modality;
                changed = true;
            }

            if (string.IsNullOrWhiteSpace(settings.ShortcutSettingsFile))
            {
                settings.ShortcutSettingsFile = defaults.ShortcutSettingsFile;
                changed = true;
            }

            if (string.IsNullOrWhiteSpace(settings.PacsServerConfig))
            {
                settings.PacsServerConfig = defaults.PacsServerConfig;
                changed = true;
            }

            if (string.IsNullOrWhiteSpace(settings.CameraConfig))
            {
                settings.CameraConfig = defaults.CameraConfig;
                changed = true;
            }

            return changed;
        }

        public static void Save(FileStorageSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            _current = settings;

            try
            {
                File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(settings, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Trace.TraceWarning("Không thể lưu AppFileSettings.json: " + ex.Message);
            }
        }
    }
}
