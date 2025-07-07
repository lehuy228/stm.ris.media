using System;
using System.Collections.Generic;

namespace PrintToPACSDemo.AnPhatData
{
    [Serializable]
    public class Conclusion
    {
        public string Id { get; set; }
        public string StudyInstanceUID { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string PatientGender { get; set; }
        public DateTime PatientDoB { get; set; }
        public string HealthIdentificationCode { get; set; }
        public DateTime MedicalImagingCreateAt { get; set; }
        public DateTime MedicalImagingReportedAt { get; set; }
        public string MedicalImagingCode { get; set; }
        public string OrderingPhysician { get; set; }
        public string Radiologist { get; set; }
        public string Technicians { get; set; }
        public string DeviveName { get; set; }
        public string DiagnoseInfo { get; set; }
        public string DiagnoseResult { get; set; }
        public string DiagnoseNote { get; set; }
        public string ImagingServiceCode { get; set; }
        public DateTime? CreateAt { get; set; }
        //public string StoreServerPacs { get; set; }
        //public string StoreServerRIS { get; set; }
        public int CountSeries { get; set; }
        public List<string> Images  { get; set; } = new List<string>();
            
    }
}
