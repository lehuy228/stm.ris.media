using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models
{
    public class ScheduleStepRIS
    {
        public string MaBenhNhan { get; set; }
        public string TenBenhNhan { get; set; }        
        public int GioiTinh { get; set; }            
        public DateTime NgaySinh { get; set; }          
        public string MaChiDinh { get; set; }         
        public string SoPhieuChiDinh { get; set; }    
        public DateTime ThoiGianDuKien { get; set; }   
        public string Modality { get; set; }            
        public string MaBacSiChiDinh { get; set; }    
        public string TenBacSiChiDinh { get; set; }     
        public string MaDichVu { get; set; }          
        public string TenDichVu { get; set; }           
        public string ScheduleID { get; set; }          
        public string TrangThaiWorklist { get; set; }    
        public string TrangThai { get; set; }          
        public int MucDoUuTien { get; set; }            
        public string Id { get; set; }
    }

    public class ScheduleStepRISResponse
    {
        public List<ScheduleStepRIS> Data { get; set; }
        public int Total { get; set; }
    }

    public class FilterItem
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("operator")]
        public string Operator { get; set; } = "eq";

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
