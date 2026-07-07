using MediaToPacs.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace STM.MediaToPACS.Main.Utilities
{
    public static class ShortcutSettingsManager
    {
        public static string SettingsPath =>
            Path.Combine(
                ServiceLocator.GetAppDataBasePath(),
                ConfigurationManager.AppSettings["File:ShortcutSettingsFile"]
            );

        public static ShortcutAndFontSettings LoadOrCreateSettings()
        {
            ShortcutAndFontSettings settings = null;

            // Create serializer
            XmlSerializer serializer = new XmlSerializer(typeof(ShortcutAndFontSettings));

            // Nếu file chưa tồn tại → tạo mặc định
            if (!File.Exists(SettingsPath))
            {
                settings = CreateDefaultSettings();
                SaveSettings(settings);
                return settings;
            }

            // Nếu có file → đọc lên
            try
            {
                using (FileStream fs = new FileStream(SettingsPath, FileMode.Open))
                {
                    settings = (ShortcutAndFontSettings)serializer.Deserialize(fs);
                }
            }
            catch
            {
                // File lỗi → tạo file mới
                settings = CreateDefaultSettings();
                SaveSettings(settings);
                return settings;
            }

            // Đảm bảo không bị thiếu trường
            EnsureDefaults(settings);

            // Ghi lại file đã chuẩn hóa
            SaveSettings(settings);

            return settings;
        }
        public static void SaveSettings(ShortcutAndFontSettings settings)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ShortcutAndFontSettings));

            Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath));

            using (FileStream fs = new FileStream(SettingsPath, FileMode.Create))
            {
                serializer.Serialize(fs, settings);
            }
        }

        private static void EnsureDefaults(ShortcutAndFontSettings s)
        {
            var printers = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Cast<string>().ToList();
            if (s.AssignedKeys == null)
                s.AssignedKeys = new AssignedKeys();

            if (string.IsNullOrWhiteSpace(s.AssignedKeys.Search))
                s.AssignedKeys.Search = "F5";


            if (s.ConclusionScreenKeys == null)
                s.ConclusionScreenKeys = new ConclusionScreenKeys();

            if (string.IsNullOrWhiteSpace(s.ConclusionScreenKeys.Sign))
                s.ConclusionScreenKeys.Sign = "F1";

            if (string.IsNullOrWhiteSpace(s.ConclusionScreenKeys.Print))
                s.ConclusionScreenKeys.Print = "F2";

            if (string.IsNullOrWhiteSpace(s.ConclusionScreenKeys.Draft))
                s.ConclusionScreenKeys.Draft = "F3";

            if (string.IsNullOrWhiteSpace(s.ConclusionScreenKeys.Exit))
                s.ConclusionScreenKeys.Exit = "Escape";

            if (string.IsNullOrWhiteSpace(s.ConclusionScreenKeys.CaptureImage))
                s.ConclusionScreenKeys.CaptureImage = "F4";

            if (string.IsNullOrWhiteSpace(s.ConclusionScreenKeys.Preview))
                s.ConclusionScreenKeys.Preview = "F5";
            if (string.IsNullOrWhiteSpace(s.ConclusionScreenKeys.LinkCamera))
                s.ConclusionScreenKeys.Preview = "F6";
            if (string.IsNullOrWhiteSpace(s.ConclusionScreenKeys.Snapshot))
                s.ConclusionScreenKeys.Preview = "F7";
            if (string.IsNullOrWhiteSpace(s.ConclusionScreenKeys.Stop))
                s.ConclusionScreenKeys.Preview = "F8";


            if (s.FontSettings == null)
                s.FontSettings = new FontSettings();

            if (string.IsNullOrWhiteSpace(s.FontSettings.FontFamily))
                s.FontSettings.FontFamily = "Arial";

            if (string.IsNullOrWhiteSpace(s.PrintSettings.Printer))
                s.FontSettings.FontFamily = printers.Count > 0 ? printers[0] : string.Empty;

            if (s.FontSettings.FontSize <= 0)
                s.FontSettings.FontSize = 12;
        }



        private static ShortcutAndFontSettings CreateDefaultSettings()
        {
            var printers = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Cast<string>().ToList();


            return new ShortcutAndFontSettings
            {
                AssignedKeys = new AssignedKeys
                {
                    Search = "F5"
                },

                ConclusionScreenKeys = new ConclusionScreenKeys
                {
                    Sign = "F1",
                    Print = "F2",
                    Draft = "F3",
                    Exit = "Escape",
                    CaptureImage = "F4",
                    Preview = "F5",
                    LinkCamera = "F6",
                    Snapshot = "F7",
                    Stop = "F8"
                },

                FontSettings = new FontSettings
                {
                    FontFamily = "Arial",
                    FontSize = 12
                },
                PrintSettings = new PrintSettings
                {
                    Printer = printers.Count > 0 ? printers[0] : string.Empty
                }
            };
        }

    }
}
