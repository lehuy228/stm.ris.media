using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MediaToPacs.Core.Models
{
    [XmlRoot("ShortcutAndFontSettings")]
    public class ShortcutAndFontSettings
    {
        [XmlElement("AssignedKeys")]
        public AssignedKeys AssignedKeys { get; set; } = new AssignedKeys();

        [XmlElement("ConclusionScreenKeys")]
        public ConclusionScreenKeys ConclusionScreenKeys { get; set; } = new ConclusionScreenKeys();

        [XmlElement("FontSettings")]
        public FontSettings FontSettings { get; set; } = new FontSettings();

        [XmlElement("PrintSettings")]
        public PrintSettings PrintSettings { get; set; } = new PrintSettings();
    }

    public class AssignedKeys
    {
        public string Search { get; set; } // Tìm kiếm
    }

    public class ConclusionScreenKeys
    {
        public string Sign { get; set; }            // Ký số
        public string Print { get; set; }           // In
        public string Draft { get; set; }           // Lưu nháp
        public string Exit { get; set; }            // Thoát
        public string CaptureImage { get; set; }    // Lấy ảnh
        public string Preview { get; set; }         // Xem trước
        public string LinkCamera { get; set; }
        public string Snapshot { get; set; }
        public string Stop { get; set; }
    }

    public class FontSettings
    {
        public string FontFamily { get; set; }      // Kiểu chữ
        public int FontSize { get; set; }           // Phông chữ
    }

    public class PrintSettings
    {
        public string Printer { get; set; }
    }
}
