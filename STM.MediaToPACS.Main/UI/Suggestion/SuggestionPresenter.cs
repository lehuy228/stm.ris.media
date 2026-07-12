using System.Threading.Tasks;
using System.Collections.Generic;
using MediaToPacs.Core.Interfaces;
using MediaToPacs.Core.Models;

namespace STM.MediaToPACS.Main.UI
{
    /// <summary>
    /// Nội dung gợi ý đã chuẩn hóa để đổ vào 3 ô rich text trên màn hình kết luận.
    /// Detail được giữ nguyên để phục vụ render form chỉ số động (giai đoạn sau).
    /// </summary>
    public class SuggestionContent
    {
        public string MoTa { get; set; }

        public string KhuyenNghi { get; set; }

        public string KetLuan { get; set; }

        /// <summary>true = suggestion "Structured" (có form chỉ số động qua paramGroups)</summary>
        public bool IsStructured { get; set; }

        public QuickSuggestionPublicDetailDto Detail { get; set; }
    }

    /// <summary>
    /// Lớp trung gian cho luồng gợi ý kết luận mới (API risv1 quick-suggestions):
    /// tách logic gọi API/chuẩn hóa dữ liệu ra khỏi FrmMain để dễ test và mở rộng form động sau này.
    /// </summary>
    public class SuggestionPresenter
    {
        private readonly IRisService2 _risService2;

        public SuggestionPresenter(IRisService2 risService2)
        {
            _risService2 = risService2;
        }

        /// <summary>
        /// Map giới tính theo quy ước HIS (0=Nam, 1=Nữ, khác=không xác định)
        /// sang gender của API risv1 (0=Unknown, 1=Male, 2=Female, 3=Other).
        /// Gửi Unknown khi không xác định để backend trả suggestion áp dụng chung.
        /// </summary>
        public static int MapHisGenderToApi(int? hisGioiTinh)
        {
            if (hisGioiTinh == 0)
                return RisV2Gender.Male;
            if (hisGioiTinh == 1)
                return RisV2Gender.Female;
            return RisV2Gender.Unknown;
        }

        /// <summary>
        /// Load danh sách gợi ý theo dịch vụ + modality (lọc server-side).
        /// serviceCode = mã dịch vụ HIS (maDichVu từ chỉ định) — backend map qua ImagingService.Code;
        /// không truyền (null/rỗng) thì lấy theo modality như trước.
        /// </summary>
        public Task<List<QuickSuggestionListItemDto>> LoadSuggestionsAsync(string modalityCode, int? hisGioiTinh, string serviceCode = null)
        {
            return _risService2.GetQuickSuggestionsAsync(
                modalityCode: modalityCode,
                serviceCode: serviceCode);
        }

        /// <summary>
        /// Lấy chi tiết suggestion và chuẩn hóa nội dung text.
        /// Trả về null nếu suggestion không còn tồn tại trên server.
        /// </summary>
        public async Task<SuggestionContent> GetContentAsync(long id)
        {
            var detail = await _risService2.GetQuickSuggestionByIdAsync(id);
            if (detail == null)
                return null;

            return new SuggestionContent
            {
                // TODO: descriptionMode == "Html" hiện vẫn đổ text thô, sẽ render HTML khi có form động
                MoTa = detail.description,
                KhuyenNghi = detail.notes,
                KetLuan = detail.conclusion,
                IsStructured = detail.reportParamId.HasValue,
                Detail = detail
            };
        }
    }
}
