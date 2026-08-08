using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MediaToPacs.Core.Models
{
    [XmlRoot("SystemConfig")]
    public class SystemConfig
    {
        #region API RIS Configuration
        [XmlElement("UrlApiRis")]
        public string UrlApiRis { get; set; }

        [XmlElement("UrlRisAuthen")]
        public string UrlRisAuthen { get; set; }

        [XmlElement("UrlSignatureMysign")]
        public string UrlSignatureMysign { get; set; }

        [XmlElement("UrlApiRisV2")]
        public string UrlApiRisV2 { get; set; }

        /// <summary>
        /// URL gateway tập trung, ví dụ https://gateway.benhvien.vn.
        /// API HIS (CheckThanhToan) vẫn gọi thẳng.
        /// </summary>
        [XmlElement("UrlGateway")]
        public string UrlGateway { get; set; }
        #endregion

        #region PACS Configuration
        [XmlElement("UrlViewerPacs")]
        public string UrlViewerPacs { get; set; }

        [XmlElement("UrlTokenPacs")]
        public string UrlTokenPacs { get; set; }

        [XmlElement("UrlPacsServer")]
        public string UrlPacsServer { get; set; }

        [XmlElement("PacsUser")]
        public string PacsUser { get; set; }

        [XmlElement("PacsPassword")]
        public string PacsPassword { get; set; }

        [XmlElement("UrlPacsPublic")]
        public string UrlPacsPublic { get; set; }
        #endregion

        #region System Update Configuration
        [XmlElement("UrlSystemUpdate")]
        public string UrlSystemUpdate { get; set; }

        [XmlElement("SystemUpdateUser")]
        public string SystemUpdateUser { get; set; }

        [XmlElement("SystemUpdatePassword")]
        public string SystemUpdatePassword { get; set; }

        [XmlElement("SystemUpdateToken")]
        public string SystemUpdateToken { get; set; }
        #endregion

        #region Other Settings
        [XmlElement("CheckThanhToan")]
        public string CheckThanhToan { get; set; }

        /// <summary>
        /// Thư mục gốc lưu ảnh/video chụp của bệnh nhân (thay thế App.config File:BasePath).
        /// Để trống sẽ dùng thư mục mặc định trong ProgramData.
        /// </summary>
        [XmlElement("FileStoragePath")]
        public string FileStoragePath { get; set; }
        #endregion
    }
}
