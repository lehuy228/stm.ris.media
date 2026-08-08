using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaToPacs.Core.Models.Order;

namespace MediaToPacs.Core.Models.Conclusion
{

    public class RegularReportConclusionInfo
    {
        public ServiceOrderResponse ChiDinhDichVu { get; set; }
        public string TGBacSiChiDinh { get; set; }
        public string TGBacSiKetLuan { get; set; }
        public string MoTa { get; set; }
        public string KetLuan { get; set; }
        public string KhuyenNghi { get; set; }
        public string GioiTinhConvert { get; set; }
        public string TenBacSiKetLuan { get; set; }
        public string AnhChuKy { get; set; }
        public string Image1 { get; set; } = null;
        public string Image2 { get; set; } = null;
        public string Image3 { get; set; } = null;
        public string Image4 { get; set; } = null;

        /// <summary>
        /// B?ng ch? s? d?ng (suggestion Structured) - template c� DetailReportBand
        /// bind DataMember n�y s? render b?ng; template cu kh�ng c� band v?n ch?y b�nh thu?ng.
        /// </summary>
        public List<ReportParameter> DanhSachChiSo { get; set; } = new List<ReportParameter>();

        /// <summary>
        /// Tra c?u gi� tr? ch? s? theo M� param (vd "MV_VMAX") � d�ng cho script bind trong repx:
        /// m?i � gi� tr? d?t Tag = m�, script g?i GetChiSo(Tag) d? l?y gi� tr?.
        /// Mapping m�?� n?m trong repx n�n d?i template kh�ng c?n build l?i app.
        /// </summary>
        public Dictionary<string, string> ChiSoTheoMa { get; set; } = new Dictionary<string, string>();

        /// <summary>Tr? gi� tr? ch? s? theo m�; r?ng n?u kh�ng c�. Script trong repx g?i h�m n�y.</summary>
        public string GetChiSo(string ma)
        {

            if (string.IsNullOrEmpty(ma) || ChiSoTheoMa == null)
                return string.Empty;
            string value;
            return ChiSoTheoMa.TryGetValue(ma, out value) ? (value ?? string.Empty) : string.Empty;
        }

        /// <summary>
        /// Tr?ng th�i t�ch theo M� param � d�nh cho XRCheckBox trong repx:
        /// XRCheckBox d?t Tag = m�, script g?i GetChiSoCheck(Tag) g�n v�o Checked.
        /// Ch? ch?a param d?ng checkbox/checkbox_value; param d?ng value kh�ng c� trong dictionary.
        /// </summary>
        public Dictionary<string, bool> ChiSoCheckTheoMa { get; set; } = new Dictionary<string, bool>();

        /// <summary>Tr? tr?ng th�i t�ch c?a checkbox theo m�; false n?u kh�ng c�/kh�ng t�ch.</summary>
        public bool GetChiSoCheck(string ma)
        {
            if (string.IsNullOrEmpty(ma) || ChiSoCheckTheoMa == null)
                return false;
            bool isChecked;
            return ChiSoCheckTheoMa.TryGetValue(ma, out isChecked) && isChecked;
        }
    }
}
