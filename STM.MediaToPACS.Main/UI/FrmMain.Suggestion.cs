using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;
using Leadtools;
using Leadtools.Demos;
using Leadtools.Forms.DocumentWriters;
using Leadtools.Codecs;
using Leadtools.Dicom;
using System.Net;
using System.Threading;
using Leadtools.Dicom.Common.Extensions;
using Leadtools.Dicom.Common.Editing;
using Leadtools.Dicom.Scu.Common;
using Leadtools.Dicom.Scu;
using System.Diagnostics;
using Leadtools.Dicom.Common.DataTypes.Modality;
using STM.MediaToPACS.Main.UI;
using Leadtools.DicomDemos;
using System.Collections.Generic;
using System.Collections;
using System.Management;
using Leadtools.WinForms.CommonDialogs.File;
using System.Reflection;
using Leadtools.Dicom.Common.Editing.Converters;
using Leadtools.ImageProcessing;
using Leadtools.Drawing;
using Leadtools.ImageProcessing.Effects;
using STM.MediaToPACS.Main.UI.CameraUI;
using Leadtools.Medical.Worklist.DataAccessLayer;
using Leadtools.Medical.DataAccessLayer;
using Leadtools.Medical.DataAccessLayer.Configuration;
using Leadtools.Medical.Worklist.DataAccessLayer.Configuration;
using Leadtools.Medical.Winforms;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
//using VisioForge.Core.VideoEdit; // VisioForge đã gỡ (thay bằng FlashCap)
using MediaToPacs.Core.Models;
using STM.MediaToPACS.Main.Utilities;
using DevExpress.XtraPdfViewer;
using System.Drawing.Printing;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraEditors;
using DevExpress.Pdf;
using MediaToPacs.Core.Models.Ketluan;
using DevExpress.XtraReports.UI;
using System.Text;
using MediaToPacs.Core.Enums;
using System.Xml.Serialization;
using Serilog;
using System.Configuration;
using System.Runtime.InteropServices;
using STM.MediaToPACS.Main.UI.Configurations;

namespace STM.MediaToPACS.Main
{
    public partial class FrmMain
    {
        // Luồng gợi ý cũ (API goi-y-ketluan) - giữ làm fallback khi API risv1 chưa sẵn sàng
        private List<GoiYKetLuanResponse> _listGoiYKetLuan { get; set; }

        // Luồng gợi ý mới (API risv1 quick-suggestions)
        private List<QuickSuggestionListItemDto> _listQuickSuggestions;
        private SuggestionPresenter _suggestionPresenter;

        // Form chỉ số động cho suggestion Structured (tạo lazy, host trong tab "Tham số siêu âm"
        // của sidebar trái FrmMain - xem UI\PatientSidebar\PatientSidebarControl)
        private ParamFormControl _paramFormControl;
        // Khối text đã sinh lần trước - dùng để thay thế đúng khối đó trong ô Mô tả,
        // không ghi đè phần bác sĩ gõ tay bên ngoài khối
        private string _lastGeneratedParamText = "";

        // Y lệnh tương ứng bên RIS mới (tra theo _machidinh 1 lần lúc load form) -
        // null nghĩa là RIS mới chưa có y lệnh/không kết nối được -> bỏ qua sync, luồng cũ không ảnh hưởng
        private RisV1OrderItemDetailDto _risV1OrderItem;

        // Chi tiết suggestion Structured đang mở form chỉ số - dùng build snapshot khi lưu
        private QuickSuggestionPublicDetailDto _currentStructuredDetail;

        private SuggestionPresenter GetSuggestionPresenter()
        {
            if (_suggestionPresenter == null)
                _suggestionPresenter = new SuggestionPresenter(ServiceLocator.RisService2);
            return _suggestionPresenter;
        }

        /// <summary>
        /// Load danh sách gợi ý: thử API mới (risv1 quick-suggestions, lọc gender + modality server-side),
        /// nếu lỗi hoặc rỗng thì fallback về API cũ (goi-y-ketluan, lọc gender client-side).
        /// </summary>
        private async Task LoadSuggestionsSafeAsync(int? hisGioiTinh)
        {
            _listQuickSuggestions = null;
            _listGoiYKetLuan = null;

            try
            {
                string modality = _chiDinhDichVuResponse?.Modality;
                string serviceCode = _chiDinhDichVuResponse?.MaDichVu;
                int genderApi = SuggestionPresenter.MapHisGenderToApi(hisGioiTinh);
                Log.Information(
                    "Gọi quick-suggestions (risv1): baseUrl={BaseUrl}, modalityCode={Modality}, serviceCode={ServiceCode}, gender={Gender}",
                    ServiceLocator.SystemConfig?.UrlApiRisV2, modality, serviceCode, genderApi);

                _listQuickSuggestions = await GetSuggestionPresenter().LoadSuggestionsAsync(modality, hisGioiTinh, serviceCode);

                Log.Information("quick-suggestions trả về {Count} item",
                    _listQuickSuggestions != null ? _listQuickSuggestions.Count : 0);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không gọi được API quick-suggestions (risv1), fallback sang API gợi ý cũ");
            }

            if (_listQuickSuggestions == null || _listQuickSuggestions.Count == 0)
            {
                // TODO(legacy): gỡ nhánh fallback sau khi backend risv1 triển khai đủ dữ liệu ở các bệnh viện
                Log.Information("Fallback sang API gợi ý kết luận cũ (goi-y-ketluan)");
                var allGoiY = await ServiceLocator.RisService.GetDanhSachGoiYKetLuanResponseAsync(
                    madichvu: _chiDinhDichVuResponse.MaDichVu);
                _listGoiYKetLuan = FilterGoiYKetLuanByGender(allGoiY?.data, hisGioiTinh ?? -1);
            }
        }

        private void InitComboxMauGoiY()
        {
            _cbbMauGoiY.Properties.Items.Clear();

            if (_listQuickSuggestions != null && _listQuickSuggestions.Count > 0)
            {
                foreach (var item in _listQuickSuggestions)
                {
                    _cbbMauGoiY.Properties.Items.Add(item);
                }
            }
            else if (_listGoiYKetLuan != null)
            {
                foreach (var item in _listGoiYKetLuan)
                {
                    _cbbMauGoiY.Properties.Items.Add(item);
                }
            }

            if (_cbbMauGoiY.Properties.Items.Count > 0)
            {
                if (_kqChanDoanResponse == null)
                    _cbbMauGoiY.SelectedIndex = 0;
            }
        }
        private List<GoiYKetLuanResponse> FilterGoiYKetLuanByGender(
            List<GoiYKetLuanResponse> allGoiY, int gioiTinh)
        {
            if (allGoiY == null || allGoiY.Count == 0)
                return new List<GoiYKetLuanResponse>();

            return allGoiY.Where(x =>
            {
                if (string.IsNullOrWhiteSpace(x.gioitinh))
                    return true;

                var g = x.gioitinh.Trim().ToLower();

                if (gioiTinh == 0)          // Nam
                    return g == "nam";

                if (gioiTinh == 1)          // Nữ
                    return g == "nữ" || g == "nu";

                return true;                // Không xác định
            }).ToList();
        }
        private void _cbbReportTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Luồng mới: item là QuickSuggestionListItemDto - phải gọi API chi tiết để lấy nội dung
            if (_cbbMauGoiY.SelectedItem is QuickSuggestionListItemDto quickItem)
            {
                ApplyQuickSuggestionAsync(quickItem.id);
                return;
            }

            // Luồng fallback (API cũ): giữ nguyên hành vi, không có form chỉ số
            if (_cbbMauGoiY.SelectedItem is GoiYKetLuanResponse selected)
            {
                HideParamForm();
                _rtMoTa.Text = selected.kqcls_mota;
                _rtKhuyenNghi.Text = selected.kqcls_denghi;
                _rtKetLuan.Text = selected.kqcls_ketluan;

                var tb = _listThietBi?.FirstOrDefault(x => x.code == selected.mathietbi);
                if (tb != null)
                {
                    _cbbDSThietBi.EditValue = tb.id;
                }
            }
        }

        /// <summary>
        /// Lấy chi tiết suggestion mới và đổ vào 3 ô rich text.
        /// async void có chủ đích (fire-and-forget từ event handler) - mọi exception được nuốt tại chỗ kèm log.
        /// </summary>
        private async void ApplyQuickSuggestionAsync(long id)
        {
            try
            {
                var content = await GetSuggestionPresenter().GetContentAsync(id);
                if (content == null)
                {
                    Log.Warning("Suggestion {Id} không còn tồn tại trên server", id);
                    return;
                }

                _rtMoTa.Text = content.MoTa ?? "";
                _rtKhuyenNghi.Text = content.KhuyenNghi ?? "";
                _rtKetLuan.Text = content.KetLuan ?? "";
                // Mô tả vừa bị thay mới hoàn toàn nên khối text sinh cũ không còn
                _lastGeneratedParamText = "";

                if (content.IsStructured)
                {
                    OnStructuredSuggestionSelected(content.Detail);
                }
                else
                {
                    HideParamForm();
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Lỗi khi áp dụng gợi ý kết luận {Id}", id);
            }
        }

        /// <summary>
        /// Suggestion Structured: render form chỉ số động từ detail.paramGroups vào tab
        /// "Tham số siêu âm" của sidebar trái, và đồng bộ text sinh ra vào ô Mô tả.
        /// </summary>
        private void OnStructuredSuggestionSelected(QuickSuggestionPublicDetailDto detail)
        {
            Log.Information("Suggestion Structured được chọn: {Title} (reportParam: {Code})",
                detail.title, detail.reportParamCode);

            EnsureParamFormControl();
            _paramFormControl.SetData(detail);
            _currentStructuredDetail = detail;
            _paramFormControl.Visible = true;
            _patientSidebar.SetParamsTabAvailable(true);
            _patientSidebar.ActivateParamsTab();

            // Đồng bộ ngay lần đầu để các presetValue xuất hiện trong Mô tả
            SyncParamTextToMoTa();
        }

        /// <summary>
        /// Tạo form chỉ số (1 lần duy nhất) và gắn vào ParamsHostPanel của sidebar trái
        /// (tab "Tham số siêu âm") - xem UI\PatientSidebar\PatientSidebarControl.
        /// </summary>
        private void EnsureParamFormControl()
        {
            if (_paramFormControl != null)
                return;

            _paramFormControl = new ParamFormControl
            {
                Visible = false,
                Dock = DockStyle.Fill
            };
            _paramFormControl.ParamValuesChanged += (s, e) => SyncParamTextToMoTa();

            _patientSidebar.ParamsHostPanel.Controls.Add(_paramFormControl);
            _paramFormControl.SetExpandedWidth(_patientSidebar.ParamsHostPanel.ClientSize.Width);
        }

        /// <summary>
        /// Cắt khối text chỉ số đã sinh (live-sync) ra khỏi Mô tả khi tạo báo cáo PDF:
        /// trên màn hình và khi lưu về RIS vẫn giữ nguyên text, chỉ bản in dùng bảng chỉ số riêng.
        /// Nếu form chỉ số không mở hoặc không tìm thấy khối đã sinh thì trả về nguyên văn.
        /// </summary>
        private string StripGeneratedParamText(string moTa)
        {
            if (string.IsNullOrEmpty(moTa))
                return moTa;
            if (_paramFormControl == null || !_paramFormControl.Visible)
                return moTa;
            if (string.IsNullOrEmpty(_lastGeneratedParamText))
                return moTa;

            int idx = moTa.LastIndexOf(_lastGeneratedParamText, StringComparison.Ordinal);
            if (idx < 0)
                return moTa;

            return moTa.Remove(idx, _lastGeneratedParamText.Length).TrimEnd('\r', '\n', ' ');
        }

        /// <summary>Ẩn form chỉ số khi chọn suggestion Text/luồng cũ</summary>
        private void HideParamForm()
        {
            _currentStructuredDetail = null;
            // Mẫu đang chọn không có chỉ số - khoá tab để không mở được form rỗng
            _patientSidebar.SetParamsTabAvailable(false);
            if (_paramFormControl == null)
                return;
            _paramFormControl.Visible = false;
            _paramFormControl.ClearData();
            _lastGeneratedParamText = "";
        }

        /// <summary>
        /// Đồng bộ text sinh từ form chỉ số vào ô Mô tả theo cơ chế thay thế khối:
        /// tìm và thay đúng khối đã sinh lần trước, phần bác sĩ gõ tay bên ngoài khối được giữ nguyên.
        /// Nếu bác sĩ sửa tay vào giữa khối đã sinh thì khối mới sẽ được nối vào cuối.
        /// </summary>
        private void SyncParamTextToMoTa()
        {
            if (_paramFormControl == null || !_paramFormControl.Visible)
                return;

            // Không tự sửa Mô tả khi đã ký số (ô bị khóa)
            if (!_rtMoTa.Enabled)
                return;

            string newText = _paramFormControl.GenerateText();
            string current = _rtMoTa.Text ?? "";

            if (!string.IsNullOrEmpty(_lastGeneratedParamText) && current.Contains(_lastGeneratedParamText))
            {
                int idx = current.LastIndexOf(_lastGeneratedParamText, StringComparison.Ordinal);
                current = current.Remove(idx, _lastGeneratedParamText.Length).Insert(idx, newText);
            }
            else if (!string.IsNullOrEmpty(newText))
            {
                if (current.Length > 0 && !current.EndsWith("\n"))
                    current += "\n";
                current += newText;
            }

            _rtMoTa.Text = current;
            _lastGeneratedParamText = newText;
        }

        #region Sync kết luận + chỉ số sang RIS mới (risv1) - best-effort, không ảnh hưởng luồng lưu API cũ

        /// <summary>
        /// Cờ tắt khẩn cấp việc sync sang RIS mới (app.config: Feature:RisV1ConclusionSync = "false").
        /// Mặc định bật khi không khai báo key.
        /// </summary>
        private static bool IsRisV1ConclusionSyncEnabled()
        {
            var raw = ConfigurationManager.AppSettings["Feature:RisV1ConclusionSync"];
            return !string.Equals(raw, "false", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Tra y lệnh bên RIS mới theo mã chỉ định HIS (chạy nền lúc load form, kết quả cache
        /// vào _risV1OrderItem). Thất bại chỉ log warning - không ném exception ra ngoài.
        /// </summary>
        private async Task ResolveRisV1OrderItemAsync()
        {
            try
            {
                if (!IsRisV1ConclusionSyncEnabled() || string.IsNullOrWhiteSpace(_machidinh))
                    return;

                _risV1OrderItem = await ServiceLocator.RisService2.GetOrderItemByPlacerCodeAsync(_machidinh);
                if (_risV1OrderItem == null)
                    Log.Warning("RIS mới chưa có y lệnh với mã chỉ định {MaChiDinh} - sẽ bỏ qua sync kết luận", _machidinh);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không tra được y lệnh RIS mới theo mã chỉ định {MaChiDinh} - sẽ bỏ qua sync", _machidinh);
            }
        }

        /// <summary>
        /// Sync kết luận + bảng chỉ số sang RIS mới sau khi API cũ đã lưu THÀNH CÔNG.
        /// Best-effort: mọi lỗi chỉ ghi log, tuyệt đối không hiện popup/không ảnh hưởng luồng chính.
        /// </summary>
        private async Task SyncConclusionToRisV1Async(string moTa, string ketLuan, string khuyenNghi)
        {
            try
            {
                if (!IsRisV1ConclusionSyncEnabled())
                    return;

                // Lần load đầu chưa resolve được (RIS mới khởi động chậm/mạng chập chờn) thì thử lại 1 lần
                if (_risV1OrderItem == null)
                    await ResolveRisV1OrderItemAsync();
                if (_risV1OrderItem == null)
                    return;

                var request = BuildRisV1ConclusionRequest(moTa, ketLuan, khuyenNghi);

                await ServiceLocator.RisService2.UpsertOrderItemConclusionAsync(_risV1OrderItem.id, request);
                Log.Information("Đã sync kết luận sang RIS mới (y lệnh {OrderItemId}, có chỉ số: {HasParams})",
                    _risV1OrderItem.id, request.parameters != null);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Sync kết luận sang RIS mới thất bại - luồng lưu chính không bị ảnh hưởng");
            }
        }

        /// <summary>
        /// Gom giá trị form chỉ số hiện tại thành input gửi lên RIS mới
        /// (backend tự build Schema B); null nếu không mở form Structured.
        /// </summary>
        private RisV1UpsertConclusionRequest BuildRisV1ConclusionRequest(string moTa, string ketLuan, string khuyenNghi)
        {
            var request = new RisV1UpsertConclusionRequest
            {
                conclusion = ketLuan,
                findings = moTa,
                recommendation = khuyenNghi,
                parameters = BuildParamValuesSnapshot()
            };

            var selectedDevice = _cbbDSThietBi.Properties.GetDataSourceRowByKeyValue(_cbbDSThietBi.EditValue) as DeviceDto;
            if (selectedDevice != null)
            {
                request.deviceId = selectedDevice.id;
                request.deviceCode = selectedDevice.code;
                request.deviceName = selectedDevice.name;
            }

            var selectedTechnologist = _cbbHisUser.Properties.GetDataSourceRowByKeyValue(_cbbHisUser.EditValue) as PractitionerListDto;
            if (selectedTechnologist != null)
            {
                request.technologistPractitionerId = selectedTechnologist.id;
                request.technologistPractitionerCode = selectedTechnologist.staffCode;
                request.technologistPractitionerName = selectedTechnologist.fullName;
            }

            return request;
        }

        private RisV1ReportParamsInput BuildParamValuesSnapshot()
        {
            if (_paramFormControl == null || !_paramFormControl.Visible || _currentStructuredDetail == null)
                return null;

            return new RisV1ReportParamsInput
            {
                suggestionId = _currentStructuredDetail.id,
                @params = _paramFormControl.GetParamValues(),
                capturedBy = ServiceLocator.KeycloakUserInfo != null ? ServiceLocator.KeycloakUserInfo.HISCode : null,
                capturedByName = ServiceLocator.KeycloakUserInfo != null
                    ? $"{ServiceLocator.KeycloakUserInfo.FirstName} {ServiceLocator.KeycloakUserInfo.LastName}".Trim()
                    : null
            };
        }

        /// <summary>
        /// Khôi phục form chỉ số từ snapshot đã lưu bên RIS mới (report.parameters) khi mở lại chỉ định.
        /// KHÔNG đụng vào 3 ô text (đã nạp từ API cũ) - chỉ dựng lại form + ghi nhận khối text đã sinh
        /// để các lần sửa chỉ số sau thay đúng khối trong Mô tả. Mọi lỗi chỉ log, bỏ qua.
        /// </summary>
        private async Task RestoreParamFormFromRisV1Async()
        {
            try
            {
                var parametersObj = _risV1OrderItem != null && _risV1OrderItem.report != null
                    ? _risV1OrderItem.report.parameters
                    : null;

                string rawJson = null;
                if (parametersObj is System.Text.Json.JsonElement je &&
                    (je.ValueKind == System.Text.Json.JsonValueKind.Object || je.ValueKind == System.Text.Json.JsonValueKind.Array))
                {
                    rawJson = je.GetRawText();
                }
                if (string.IsNullOrEmpty(rawJson))
                    return;

                var snapshot = System.Text.Json.JsonSerializer.Deserialize<RisV1ReportParamsSchema>(
                    rawJson,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (snapshot == null || !snapshot.suggestionId.HasValue ||
                    snapshot.groups == null || snapshot.groups.Count == 0)
                    return;

                // Schema B lồng theo nhóm - trải phẳng để đổ vào form theo paramCode
                var savedValues = snapshot.groups
                    .Where(g => g.@params != null)
                    .SelectMany(g => g.@params)
                    .ToList();
                if (savedValues.Count == 0)
                    return;

                var content = await GetSuggestionPresenter().GetContentAsync(snapshot.suggestionId.Value);
                if (content == null || !content.IsStructured)
                {
                    Log.Warning("Suggestion {Id} trong snapshot chỉ số không còn/không phải Structured - bỏ qua khôi phục",
                        snapshot.suggestionId);
                    return;
                }

                EnsureParamFormControl();
                _paramFormControl.SetData(content.Detail);
                _paramFormControl.SetParamValues(savedValues);
                _currentStructuredDetail = content.Detail;
                _paramFormControl.Visible = true;
                _patientSidebar.SetParamsTabAvailable(true);

                // Khối text chỉ số đã nằm sẵn trong Mô tả lưu từ API cũ - chỉ ghi nhận lại
                // để lần sửa chỉ số tiếp theo thay đúng khối, không chèn trùng
                _lastGeneratedParamText = _paramFormControl.GenerateText();

                Log.Information("Đã khôi phục {Count} chỉ số từ RIS mới (suggestion {Id})",
                    savedValues.Count, snapshot.suggestionId);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không khôi phục được form chỉ số từ RIS mới - bỏ qua");
            }
        }

        #endregion
    }
}
