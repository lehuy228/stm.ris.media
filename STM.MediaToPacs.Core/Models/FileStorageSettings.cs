using System;

namespace MediaToPacs.Core.Models
{
    /// <summary>
    /// Cấu hình tên file/đường dẫn của ứng dụng, lưu tại AppFileSettings.json
    /// trong thư mục ProgramData (thay thế các key File:*/SystemConfigFile/Modality
    /// trong app.config).
    /// </summary>
    [Serializable]
    public class FileStorageSettings
    {
        public string SystemConfigFile { get; set; } = "SystemConfig.xml";

        public string Modality { get; set; } = "Modalities.xml";

        public string ShortcutSettingsFile { get; set; } = "ShortcutSettingsFile.xml";

        public string PacsServerConfig { get; set; } = "PacsServerConfig.xml";

        public string CameraConfig { get; set; } = "CameraSettingConfig.xml";
    }
}
