using MediaToPacs.Core.Models;
using MediaToPacs.Core.Models.Order;
using MediaToPacs.Core.Models.ServiceCatalog;
using MediaToPacs.Core.Models.Conclusion;
using MediaToPacs.Core.Models.Suggestion;
using MediaToPacs.Core.Models.Template;
using MediaToPacs.Core.Models.Device;
using MediaToPacs.Core.Models.Signature;
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
             string tenPatient = null,
             string maChiDinh = null,
             string modality = null,
             string studyInstanceUID = null,
             string tenBacSiChiDinh = null,
             DateTime? dateTimeFrom = null,
             DateTime? dateTimeTo = null);

        Task<ScheduleStepRIS> GetLichChupAsync(string machidinh);

        Task<ResultPage<ReportTemplate>> GetDanhSachMauKLAsync(int page = 1, int pageSize = 50, string modality = null);

        //Kết quả chẩn đoán
        Task<DiagnosisResultResponse> TaoKetQuaChanDoanAsync(DiagnosisResultRequest request);
        Task<byte[]> TaiFileKetQuaChanDoanAsync(string machidinh);
        Task<bool> HuyKetQuaChanDoanAsync(string machidinh, string lyDoHuy);

        Task<DiagnosisResultResponse> GetKetQuaChanDoanAsync(string maChiDinh);

        Task<bool> UploadSignedFileAsync(
                string machidinh,
                string fileType,
                string description,
                byte[] fileBuffer);

        Task<bool> SendKetQuaChanDoanToHisAsync(string maChiDinh);

        //Gợi ý kết luận
        Task<ResultPage<ConclusionSuggestionGridView>> GetDanhSachGoiYKetLuanAsync(int page = 1, int pageSize = 50, string madichvu = null);
        Task<ResultPage<ConclusionSuggestionResponse>> GetDanhSachConclusionSuggestionResponseAsync(int page = 1, int pageSize = 50, string madichvu = null);
        Task<ConclusionSuggestionResponse> TaoMoiGoiYKetLuanAsync(ConclusionSuggestionRequest request);
        Task<ConclusionSuggestionResponse> GetGoiYKetLuanById(string id);
        Task<ConclusionSuggestionResponse> CapNhatGoiYKetLuanAsync(ConclusionSuggestionRequest request, string id);
        Task<bool> XoaGoiYKetLuanAsync(string id);

        //Bệnh nhân
        Task<Patient> GetPatientAsync(string maPatient);

        //Danh sách chỉ định dịch vụ
        Task<ResultPage<ServiceOrderResponse>> GetDSChiDinhDichVuAsync(int page = 1, int pageSize = 50,
            string maPatient = null,
            string tenPatient = null,
            string maChiDinh = null,
            string modality = null,
            string trangThai = null,
            string tenBacSiChiDinh = null,
            DateTime? dateTimeFrom = null,
            DateTime? dateTimeTo = null);
        Task<ServiceOrderResponse> GetChiDinhDichVuAsync(string maChiDinh);

        Task<ServiceOrderResponse> UpdateChiDinhDichVu(string maChiDinh, Patient Patient);

        //ReportTemplate
        Task<bool> CreateReportTemplateAsync(ReportTemplateRequest request);
        Task<bool> UpdateReportTemplateAsync(ReportTemplateRequest request, string id);
        Task<ResultPage<ReportTemplateGridViewModel>> GetReportTemplateAsync(int page = 1, int pageSize = 50, string modality = null);
        Task<ReportTemplateResponse> GetReportTemplateByIdAsync(string id);
        Task<bool> XoaReportTemplateAsync(string id);

        //Danh sách dịch vụ
        Task<ResultPage<ServiceCatalogResponse>> GetDSDichVuAsync(int page = 1, int pageSize = 500, string modality = null);
        Task<List<ServiceCatalogGridView>> GetDanhSachDichVuAsync(int page = 1, int pageSize = 500, string modality = null, string tenDichVu = null, string maDichVu = null);
        Task<ServiceCatalogResponse> GetDichVuAsync(string maDichVu);

        //Danh Sach thiết bị
        Task<ResultPage<DeviceResponse>> GetDSThietBiAsync(int page = 1, int pageSize = 500, string loaiThietBi = null);

        //Danh Sach KTV
        Task<ResultPage<HisUserResponse>> GetDSNguoidungAsync(int page = 1, int pageSize = 500);


    }
}
