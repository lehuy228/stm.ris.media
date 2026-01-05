using MediaToPacs.Core.Models;
using MediaToPacs.Core.Models.Ketluan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Interfaces
{
    public interface IRisService
    {
        Task<List<ScheduleStepRIS>> GetDanhSachLichChupAsync(
             int page = 1, int pageSize = 10,
             string tenBenhNhan = null,
             string maChiDinh = null,
             string modality = null,
             string studyInstanceUID = null,
             string tenBacSiChiDinh = null,
             DateTime? dateTimeFrom = null,
             DateTime? dateTimeTo = null);

        Task<ScheduleStepRIS> GetLichChupAsync(string machidinh);

        Task<ResultPage<ReportTemplate>> GetDanhSachMauKLAsync(int page = 1, int pageSize = 50, string modality = null);

        //Kết quả chẩn đoán
        Task<KetQuaChanDoanResponse> TaoKetQuaChanDoanAsync(KetQuaChanDoanRequest request);
        Task<byte[]> TaiFileKetQuaChanDoanAsync(string machidinh);
        Task<bool> HuyKetQuaChanDoanAsync(string machidinh, string lyDoHuy);

        Task<KetQuaChanDoanResponse> GetKetQuaChanDoanAsync(string maChiDinh);

        Task<bool> UploadSignedFileAsync(
                string machidinh,
                string fileType,
                string description,
                byte[] fileBuffer);

        Task<bool> SendKetQuaChanDoanToHisAsync(string maChiDinh);

        //Gợi ý kết luận
        Task<ResultPage<GoiYKetLuanDataGridView>> GetDanhSachGoiYKetLuanAsync(int page = 1, int pageSize = 50, string madichvu = null);
        Task<ResultPage<GoiYKetLuanResponse>> GetDanhSachGoiYKetLuanResponseAsync(int page = 1, int pageSize = 50, string madichvu = null);
        Task<GoiYKetLuanResponse> TaoMoiGoiYKetLuanAsync(GoiYKetLuanRequest request);
        Task<GoiYKetLuanResponse> GetGoiYKetLuanById(string id);
        Task<GoiYKetLuanResponse> CapNhatGoiYKetLuanAsync(GoiYKetLuanRequest request, string id);
        Task<bool> XoaGoiYKetLuanAsync(string id);

        //Bệnh nhân
        Task<BenhNhan> GetBenhNhanAsync(string mabenhnhan);

        //Danh sách chỉ định dịch vụ
        Task<ResultPage<ChiDinhDichVuResponse>> GetDSChiDinhDichVuAsync(int page = 1, int pageSize = 50,
            string mabenhnhan = null,
            string tenBenhNhan = null,
            string maChiDinh = null,
            string modality = null,
            string trangThai = null,
            string tenBacSiChiDinh = null,
            DateTime? dateTimeFrom = null,
            DateTime? dateTimeTo = null);
        Task<ChiDinhDichVuResponse> GetChiDinhDichVuAsync(string maChiDinh);

        Task<ChiDinhDichVuResponse> UpdateChiDinhDichVu(string maChiDinh, BenhNhan benhNhan);

        //ReportTemplate
        Task<bool> CreateReportTemplateAsync(ReportTemplateRequest request);
        Task<bool> UpdateReportTemplateAsync(ReportTemplateRequest request, string id);
        Task<ResultPage<ReportTemplateGridViewModel>> GetReportTemplateAsync(int page = 1, int pageSize = 50, string modality = null);
        Task<ReportTemplateResponse> GetReportTemplateByIdAsync(string id);
        Task<bool> XoaReportTemplateAsync(string id);

        //Danh sách dịch vụ
        Task<ResultPage<DanhMucDichVuResponse>> GetDSDichVuAsync(int page = 1, int pageSize = 500, string modality = null);
        Task<List<DanhMucDichVuGridView>> GetDanhSachDichVuAsync(int page = 1, int pageSize = 500, string modality = null, string tenDichVu = null, string maDichVu = null);
        Task<DanhMucDichVuResponse> GetDichVuAsync(string maDichVu);

        //Danh Sach thiết bị
        Task<ResultPage<ThietBiResponse>> GetDSThietBiAsync(int page = 1, int pageSize = 500, string loaiThietBi = null);

        //Danh Sach KTV
        Task<ResultPage<HisUserResponse>> GetDSNguoidungAsync(int page = 1, int pageSize = 500);


    }
}
