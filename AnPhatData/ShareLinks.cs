using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;

namespace PrintToPACSDemo.AnPhat.Data
{
    [Table("share_links")]
    public class ShareLinks : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; }

        [Column("study_uid")]
        public string StudyUid { get; set; }

        [Column("role")]
        public string Role { get; set; }

        [Column("expiration")]
        public DateTime Expiration { get; set; }

        [Column("patient_id")]
        public string PatientID { get; set; }

        [Column("patient_name")]
        public string PatientName { get; set; }

        [Column("patient_gender")]
        public string PatientGender { get; set; }

        [Column("patient_doB")]
        public DateTime PatientDoB { get; set; }

        [Column("health_identification_code")]
        public string HealthIdentificationCode { get; set; }

        [Column("medical_imaging_create_at")]
        public DateTime MedicalImagingCreateAt { get; set; }

        [Column("medical_imaging_report_at")]
        public DateTime MedicalImagingReportedAt { get; set; }

        [Column("medical_imaging_code")]
        public string MedicalImagingCode { get; set; }

        [Column("ordering_physician")]
        public string OrderingPhysician { get; set; }

        [Column("radiologist")]
        public string Radiologist { get; set; }

        [Column("technicians")]
        public string Technicians { get; set; }

        [Column("devive_name")]
        public string DeviveName { get; set; }

        [Column("diagnose_info")]
        public string DiagnoseInfo { get; set; }

        [Column("diagnose_result")]
        public string DiagnoseResult { get; set; }

        [Column("diagnose_note")]
        public string DiagnoseNote { get; set; }

        [Column("imaging_service_name")]
        public string ImagingServiceName { get; set; }

        [Column("created_at")]
        public DateTime CreateAt { get; set; }

        [Column("images")]
        public List<String> Images { get; set; }
    }
}
