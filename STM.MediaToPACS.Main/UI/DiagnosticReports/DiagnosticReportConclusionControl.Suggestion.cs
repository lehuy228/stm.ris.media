using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaToPacs.Core.Models;
using MediaToPacs.Core.Models.Ketluan;
using STM.MediaToPACS.Main.UI;
using STM.MediaToPACS.Main.Utilities;
using Serilog;

namespace STM.MediaToPACS.Main.UI.DiagnosticReports
{
    /// <summary>
    /// Gợi ý kết luận + form chỉ số động - chuyển thể nguyên vẹn từ FrmMain.Suggestion.cs.
    /// ParamFormControl host vào _patientSidebar.ParamsHostPanel (tab "Tham số siêu âm").
    /// </summary>
    public partial class DiagnosticReportConclusionControl
    {
        private List<GoiYKetLuanResponse> _listGoiYKetLuan;
        private List<QuickSuggestionListItemDto> _listQuickSuggestions;
        private SuggestionPresenter _suggestionPresenter;

        private ParamFormControl _paramFormControl;
        private string _lastGeneratedParamText = "";

        private RisV1OrderItemDetailDto _risV1OrderItem;
        private QuickSuggestionPublicDetailDto _currentStructuredDetail;

        private SuggestionPresenter GetSuggestionPresenter()
        {
            if (_suggestionPresenter == null)
                _suggestionPresenter = new SuggestionPresenter(ServiceLocator.RisService2);
            return _suggestionPresenter;
        }

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
                    "Gọi quick-suggestions (risv1): modalityCode={Modality}, serviceCode={ServiceCode}, gender={Gender}",
                    modality, serviceCode, genderApi);

                _listQuickSuggestions = await GetSuggestionPresenter().LoadSuggestionsAsync(modality, hisGioiTinh, serviceCode);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không gọi được API quick-suggestions (risv1), fallback sang API gợi ý cũ");
            }

            if (_listQuickSuggestions == null || _listQuickSuggestions.Count == 0)
            {
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
                    _cbbMauGoiY.Properties.Items.Add(item);
            }
            else if (_listGoiYKetLuan != null)
            {
                foreach (var item in _listGoiYKetLuan)
                    _cbbMauGoiY.Properties.Items.Add(item);
            }

            if (_cbbMauGoiY.Properties.Items.Count > 0 && _kqChanDoanResponse == null)
                _cbbMauGoiY.SelectedIndex = 0;
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
                if (gioiTinh == 0) return g == "nam";
                if (gioiTinh == 1) return g == "nữ" || g == "nu";
                return true;
            }).ToList();
        }

        private void _cbbReportTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!CanEditConclusion())
                return;

            if (_cbbMauGoiY.SelectedItem is QuickSuggestionListItemDto quickItem)
            {
                ApplyQuickSuggestionAsync(quickItem.id);
                return;
            }

            if (_cbbMauGoiY.SelectedItem is GoiYKetLuanResponse selected)
            {
                HideParamForm();
                _rtMoTa.Text = selected.kqcls_mota;
                _rtKhuyenNghi.Text = selected.kqcls_denghi;
                _rtKetLuan.Text = selected.kqcls_ketluan;

                var tb = _listThietBi?.FirstOrDefault(x => x.code == selected.mathietbi);
                if (tb != null)
                    _cbbDSThietBi.EditValue = tb.id;
            }
        }

        private async void ApplyQuickSuggestionAsync(long id)
        {
            if (!CanEditConclusion())
                return;

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
                _lastGeneratedParamText = "";

                if (content.IsStructured)
                    OnStructuredSuggestionSelected(content.Detail);
                else
                    HideParamForm();
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Lỗi khi áp dụng gợi ý kết luận {Id}", id);
            }
        }

        private void OnStructuredSuggestionSelected(QuickSuggestionPublicDetailDto detail)
        {
            if (!CanEditConclusion())
                return;

            Log.Information("Suggestion Structured được chọn: {Title} (reportParam: {Code})",
                detail.title, detail.reportParamCode);

            EnsureParamFormControl();
            _paramFormControl.SetData(detail);
            _currentStructuredDetail = detail;
            _paramFormControl.Visible = true;
            _patientSidebar.SetParamsTabAvailable(true);
            _patientSidebar.ActivateParamsTab();

            SyncParamTextToMoTa();
        }

        /// <summary>
        /// Tạo form chỉ số (1 lần duy nhất) và gắn vào ParamsHostPanel của sidebar trái
        /// (tab "Tham số siêu âm") - xem UI/PatientSidebar/PatientSidebarControl.
        /// </summary>
        private void EnsureParamFormControl()
        {
            if (_paramFormControl != null)
                return;

            _paramFormControl = new ParamFormControl
            {
                Visible = false,
                Enabled = CanEditConclusion(),
                Dock = System.Windows.Forms.DockStyle.Fill
            };
            _paramFormControl.ParamValuesChanged += (s, e) => SyncParamTextToMoTa();

            _patientSidebar.ParamsHostPanel.Controls.Add(_paramFormControl);
            _paramFormControl.SetExpandedWidth(_patientSidebar.ParamsHostPanel.ClientSize.Width);
        }

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

        private void SyncParamTextToMoTa()
        {
            if (!CanEditConclusion())
                return;

            if (_paramFormControl == null || !_paramFormControl.Visible)
                return;

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

        #region Sync kết luận + chỉ số sang RIS mới (risv1) - best-effort

        private static bool IsRisV1ConclusionSyncEnabled()
        {
            var raw = System.Configuration.ConfigurationManager.AppSettings["Feature:RisV1ConclusionSync"];
            return !string.Equals(raw, "false", StringComparison.OrdinalIgnoreCase);
        }

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

        private async Task<RisV1DiagnosticReportDetailDto> SyncConclusionToRisV1Async(string moTa, string ketLuan, string khuyenNghi)
        {
            try
            {
                if (!IsRisV1ConclusionSyncEnabled())
                    return null;

                if (_risV1OrderItem == null)
                    await ResolveRisV1OrderItemAsync();
                if (_risV1OrderItem == null)
                    return null;

                var request = BuildRisV1ConclusionRequest(moTa, ketLuan, khuyenNghi);

                var report = await ServiceLocator.RisService2.UpsertOrderItemConclusionAsync(_risV1OrderItem.id, request);
                if (report != null)
                    _risV1OrderItem.report = report;
                Log.Information("Đã sync kết luận sang RIS mới (y lệnh {OrderItemId}, có chỉ số: {HasParams})",
                    _risV1OrderItem.id, request.parameters != null);
                return report;
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Sync kết luận sang RIS mới thất bại - luồng lưu chính không bị ảnh hưởng");
                return null;
            }
        }

        private RisV1UpsertConclusionRequest BuildRisV1ConclusionRequest(string moTa, string ketLuan, string khuyenNghi)
        {
            var request = new RisV1UpsertConclusionRequest
            {
                conclusion = ketLuan,
                findings = moTa,
                recommendation = khuyenNghi,
                parameters = BuildParamValuesSnapshot(),
                readStartedAt = ToOffsetOrNull(_dateTGThucHien.DateTime),
                readCompletedAt = ToOffsetOrNull(_dateTGKetThuc.DateTime)
            };

            // Bác sĩ kết luận: cùng danh tính gửi cho RIS cũ (mabacsiketluan/bacsiketluan). Endpoint risv1
            // không xác thực nên client tự khai; id nội bộ không có nên để null.
            var doctor = ServiceLocator.KeycloakUserInfo;
            if (doctor != null)
            {
                request.doctorPractitionerCode = doctor.HISCode;
                request.doctorPractitionerName = $"{doctor.FirstName} {doctor.LastName}".Trim();
            }

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

        /// <summary>
        /// DateEdit chưa nhập trả DateTime.MinValue - coi như không truyền để backend giữ nguyên
        /// giá trị hiện có thay vì ghi đè mốc thời gian rác.
        /// </summary>
        private static DateTimeOffset? ToOffsetOrNull(DateTime value)
        {
            if (value == DateTime.MinValue || value == default(DateTime))
                return null;

            return new DateTimeOffset(DateTime.SpecifyKind(value, DateTimeKind.Local));
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
                _paramFormControl.Enabled = CanEditConclusion();
                _patientSidebar.SetParamsTabAvailable(true);

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

