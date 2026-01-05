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
        public string mabenhnhan { get; set; } = "";

        [Category("Thông tin bệnh nhân")]
        [DisplayName("Tên Bệnh Nhân")]
        public string tenbenhnhan { get; set; } = "";

        [Category("Thông tin bệnh nhân")]
        [DisplayName("Năm sinh")]
        public DateTime? namsinh { get; set; }

        [Category("Thông tin bệnh nhân")]
        [DisplayName("Giới tính")]
        public Genders? gioitinh { get; set; }

        [Category("Thông tin bệnh nhân")]
        [DisplayName("Địa chỉ")]
        public string diachi { get; set; } = "";

        [Category("Thông tin bệnh nhân")]
        [DisplayName("Dân tộc")]
        public string danToc { get; set; } = "";

        [Category("Thông tin bệnh nhân")]
        [DisplayName("Từ ngày bảo hiểm y tế")]
        public DateTime tungaybhyt { get; set; }

        [Category("Thông tin bệnh nhân")]
        [DisplayName("Đến ngày bảo hiểm y tế")]
        public DateTime denngaybhyt { get; set; }

        [Category("Thông tin bệnh nhân")]
        [DisplayName("Mã bảo hiểm y tế")]
        public string mabhyt { get; set; } = "";






        [Category("Thông tin chỉ định")]
        [DisplayName("Bác sĩ chỉ định")]
        public string bacsichidinh { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Đối tượng BN")]
        public string doituongbn { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Chẩn đoán sơ bộ")]
        public string chandoansobo { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Phương thức chụp")]
        public string modality { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Ngày chỉ định")]
        public DateTime ngaychidinh { get; set; }

        [Category("Thông tin chỉ định")]
        [DisplayName("Loại chỉ định")]
        public string loaichidinh { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Bác sĩ chỉ định")]
        public string tenbacsithuchienchidinh { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Khoa diều trị")]
        public string khoadieutri { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Mức độ chỉ định")]
        public string mucdochidinh { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Phòng")]
        public string phong { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Trạng thái thanh toán")]
        public string trangthaithanhtoanchidinh { get; set; } = "";




        [Category("Thông tin chỉ định")]
        [DisplayName("Loại chỉ định")]
        public string admissionType { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Số vào viện")]
        public string Sovaovien { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Mã chỉ định")]
        public string machidinh { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Số phiếu")]
        public string sophieu { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Mã bác sĩ chỉ định")]
        public string MaBacSiChiDinh { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Tên bác sĩ chỉ định")]
        public string TenBacSiChiDinh { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Chẩn đoán sơ bộ")]
        public string ChanDoanSoBo { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Thời gian thực hiện")]
        public DateTime Thoigianthuchien { get; set; }

        [Category("Thông tin chỉ định")]
        [DisplayName("Khoa điều trị")]
        public string KhoaDieuTri { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Phòng")]
        public string Phong { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Mã nơi chỉ định")]
        public string MaNoiChiDinh { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Tên nơi chỉ định")]
        public string TenNoiChiDinh { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Trạng thái")]
        public string TrangThai { get; set; } = "";

        [Category("Thông tin chỉ định")]
        [DisplayName("Thời gian tạo")]
        public DateTime CreateAt { get; set; }

        [Category("Thông tin dịch vụ")]
        [DisplayName("Mã dịch vụ")]
        public string MaDichVu { get; set; } = "";

        [Category("Thông tin dịch vụ")]
        [DisplayName("Tên dịch vụ")]
        public string TenDichVu { get; set; } = "";

        [Category("Thông tin dịch vụ")]
        [DisplayName("Phương thức chụp")]
        public string Modality { get; set; } = "";

        //public int SoLuong { get; set; }
    }
}
