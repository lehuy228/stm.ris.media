using MediaToPacs.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models
{
    public class MedicalReport
    {
        [Category("Thông tin bệnh nhân")]
        [DisplayName("Mã Bệnh Nhân")]
        public string PatientCode { get; set; }

        [Category("Thông tin bệnh nhân")]
        [DisplayName("Tên Bệnh Nhân")]
        public string PatientName { get; set; }

        [Category("Thông tin bệnh nhân")]
        [DisplayName("Năm sinh")]
        public DateTime? BirthDate { get; set; }

        [Category("Thông tin bệnh nhân")]
        [DisplayName("Giới tính")]
        public Genders? Gender { get; set; }

        [Category("Thông tin bệnh nhân")]
        [DisplayName("Địa chỉ")]
        public string Address { get; set; }

        [Category("Thông tin chỉ định")]
        [DisplayName("Khoa phòng")]
        public string Department { get; set; }

        [Category("Thông tin chỉ định")]
        [DisplayName("Bác sĩ chỉ định")]
        public string ReferringPhysician { get; set; }

        [Category("Thông tin chỉ định")]
        [DisplayName("Chẩn đoán")]
        public string Diagnosis { get; set; }

        [Category("Thông tin dịch vụ")]
        [DisplayName("Mã Dịch Vụ")]
        public string ServiceCode { get; set; }

        [Category("Thông tin dịch vụ")]
        [DisplayName("Tên Dịch vụ")]
        public string ServiceName { get; set; }

        [Category("Thông tin Thực Hiện")]
        [DisplayName("Bác sĩ đọc")]
        public string ResultPhysician { get; set; }

        [Category("Thông tin Thực Hiện")]
        [DisplayName("Mã Kỹ Thuật Viên")]
        public string TechnicianCode { get; set; }

        [Category("Thông tin Thực Hiện")]
        [DisplayName("Tên Kỹ Thuật Viên")]
        public string Technician { get; set; }

        [Category("Thông tin Thực Hiện")]
        [DisplayName("Kỹ Thuật Viên phụ")]
        public string SubTechnician { get; set; }

        [Category("Thông tin Thực Hiện")]
        [DisplayName("Kỹ Thuật Viên phụ")]
        public string SubTechnicianName { get; set; }

        [Category("Thông tin Thực Hiện")]
        [DisplayName("Máy chụp")]
        public string Machine { get; set; }
        [Category("Thông tin kết quả")]
        [DisplayName("Thực hiện")]
        public DateTime? ExecutionTime { get; set; }

        [Category("Thông tin kết quả")]
        [DisplayName("Kết quả")]
        public DateTime? ResultTime { get; set; }


        public MedicalReport()
        {
            PatientCode = "";
            PatientName = "";
            BirthDate = null;
            Gender =  Genders.Other;
            Address = "";
            Department = "";
            ReferringPhysician = "";
            ServiceCode = "";
            ServiceName = "";
            ResultPhysician = "";
            TechnicianCode = "";
            Technician = "";
            SubTechnician = "";
            SubTechnicianName = "";
            Machine = "";
            Diagnosis = "";
            ExecutionTime = null;
            ResultTime = null;
        }

    }
}
