using MediaToPacs.Core.Models;
using System.IO;
using System.Xml.Serialization;

public static class AppSettingsLoader
{
    private static readonly string filePath = "app_settings.xml";
    private static AppSettings _cache;

    private static AppSettings GetSettings()
    {
        if (_cache == null)
        {
            _cache = Load();
        }

        return _cache;
    }

    public static AppSettings Load()
    {
        if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
        {
            var defaultSettings = new AppSettings();
            Save(defaultSettings);
            return defaultSettings;
        }

        try
        {
            using (var stream = File.OpenRead(filePath))
            {
                var serializer = new XmlSerializer(typeof(AppSettings));
                return (AppSettings)serializer.Deserialize(stream);
            }
        }
        catch
        {
            return new AppSettings();
        }
    }

    public static void Save(AppSettings settings)
    {
        _cache = settings;
        using (var stream = File.Create(filePath))
        {
            var serializer = new XmlSerializer(typeof(AppSettings));
            serializer.Serialize(stream, settings);
        }
    }

    public static CameraSettings GetCameraSettings()
    {
        return GetSettings().CameraSettings;
    }

    public static ShortcutSettings GetShortcutSettings()
    {
        return GetSettings().ShortcutSettings;
    }

    public static void SaveCameraSettings(CameraSettings settings)
    {
        var config = GetSettings();
        config.CameraSettings = settings;
        Save(config);
    }

    public static void SaveShortcutSettings(ShortcutSettings settings)
    {
        var config = GetSettings();
        config.ShortcutSettings = settings;
        Save(config);
    }
}
