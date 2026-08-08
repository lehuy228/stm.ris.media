using System;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaToPacs.Core.Enums;
using MediaToPacs.Core.Models.Order;
using MediaToPacs.Core.Models.ServiceCatalog;
using MediaToPacs.Core.Models.Conclusion;
using MediaToPacs.Core.Models.Suggestion;
using MediaToPacs.Core.Models.Template;
using MediaToPacs.Core.Models.Device;
using MediaToPacs.Core.Models.Signature;
using STM.MediaToPACS.Main.UI.Configurations;
using STM.MediaToPACS.Main.Utilities;
using Serilog;

namespace STM.MediaToPACS.Main.UI.DiagnosticReports
{
    /// <summary>
    /// Tải dữ liệu chỉ định/kết luận cho DiagnosticReportConclusionControl - chuyển thể từ FrmMain.Loading.cs,
    /// bỏ hoàn toàn phần liên quan DICOM (SetupExcludedTags, InitTranfer/InitTranferRIS worklist...).
    /// </summary>
    public partial class DiagnosticReportConclusionControl
    {
        private async Task InitConclusionDataAsync()
        {
            SetupButtonTexts();
            InitCbbPrinters();

            await InitThongTinChiDinhAsync();

            // Phải lấy kết quả chẩn đoán trước: nếu đã kết luận thành công thì staffCode để tra
            // KTV/y tá cùng khoa sẽ lấy theo bác sĩ đã kết luận (MaBacSiKetLuan), không phải bác sĩ
            // đang đăng nhập.
            await InitCheckKetQuaChanDoanAsync();

            var loadKTVTask = InitDanhSachKTVAsync();
            // Tra y lệnh bên RIS mới (best-effort) để sync/khôi phục bảng chỉ số
            var resolveRisV1Task = ResolveRisV1OrderItemAsync();
            // Lịch sử khám bệnh nhân cho sidebar (best-effort, không ảnh hưởng luồng chính)
            var loadHistoryTask = LoadPatientHistorySafeAsync();
            // Danh sách viewer PACS được phép, để đổi text nút "Xem ảnh PACS" nếu chỉ có 1 viewer
            var loadViewerAccessesTask = LoadViewerAccessesBestEffortAsync();

            await Task.WhenAll(loadKTVTask, resolveRisV1Task, loadHistoryTask, loadViewerAccessesTask);

            ApplyThietBiVaKTVSelectionFromResult();
            await LoadReportAttachmentsSafeAsync();
            // Khôi phục form chỉ số từ snapshot đã lưu bên RIS mới (nếu có) - fire-and-forget
            _ = RestoreParamFormFromRisV1Async();
        }

        private void SetupButtonTexts()
        {
            var keys = ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys;
            _btnCancel.Text = $"Thoát ({keys.Exit})";
            _btnPreviewMain.Text = $"Xem trước ({keys.Preview})";
            _btnPrint.Text = $"In ({keys.Print})";
            _btnSave.Text = $"Lưu nháp ({keys.Draft})";
            _btnSignature.Text = $"Ký số ({keys.Sign})";
            _btnSnapshot.Text = $"Chụp nhanh ({keys.Snapshot})";
            _btnLinkCamera.Text = $"Liên kết ({keys.LinkCamera})";
            _btnStop.Text = $"Dừng ({keys.Stop})";
            _btnPushPacs.Text = "Đẩy PACS";
        }

        private void InitCbbPrinters()
        {
            _cbbPrinters.Properties.Items.Clear();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                _cbbPrinters.Properties.Items.Add(printer);
            }
            if (_cbbPrinters.Properties.Items.Count > 0)
            {
                if (string.IsNullOrEmpty(ServiceLocator.ShortcutAndFontSetting.PrintSettings.Printer))
                    _cbbPrinters.SelectedIndex = 0;
                else
                    _cbbPrinters.Text = ServiceLocator.ShortcutAndFontSetting.PrintSettings.Printer;
            }
        }

        private async Task InitThongTinChiDinhAsync()
        {
            try
            {
                _ServiceOrderResponse = await ServiceLocator.RisService.GetChiDinhDichVuAsync(_machidinh);
                if (_ServiceOrderResponse == null)
                {
                    Log.Warning("Không tìm thấy thông tin chỉ định cho MaChiDinh: {MaChiDinh}", _machidinh);
                    return;
                }

                await LoadDependentDataAsync();
                PopulateFormData();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi khởi tạo thông tin chỉ định");
                MessageBox.Show(this, $"Lỗi khi tải thông tin chỉ định: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadDependentDataAsync()
        {
            var bn = _ServiceOrderResponse.Patient;

            await LoadSuggestionsSafeAsync(bn?.GioiTinh);

            await Task.WhenAll(
                InitDanhSachThietbiAsync(),
                InitLayoutMauAsync(_ServiceOrderResponse.Modality)
            );
        }

        private async Task InitLayoutMauAsync(string modality)
        {
            if (string.IsNullOrWhiteSpace(modality))
                return;

            try
            {
                var response = await ServiceLocator.RisService.GetReportTemplateAsync(modality: modality);
                _listMauBaoCao = response?.data;

                if (_listMauBaoCao == null || _listMauBaoCao.Count == 0)
                {
                    Log.Warning("Không có mẫu báo cáo cho modality: {Modality}", modality);
                    return;
                }

                _cbbLayout.Properties.Items.Clear();
                foreach (var item in _listMauBaoCao)
                    _cbbLayout.Properties.Items.Add(item);

                SelectLayoutFromCache(modality);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi load layout mẫu cho modality: {Modality}", modality);
            }
        }

        private void SelectLayoutFromCache(string modality)
        {
            string lastSelectedId = null;
            if (ServiceLocator.ReportCache.TryGetValue(modality, out var cachedId))
                lastSelectedId = cachedId;

            if (!string.IsNullOrEmpty(lastSelectedId))
            {
                var matchedItem = _listMauBaoCao.FirstOrDefault(x => x.Id == lastSelectedId);
                if (matchedItem != null)
                {
                    _cbbLayout.SelectedItem = matchedItem;
                    return;
                }
            }

            if (_cbbLayout.Properties.Items.Count > 0)
            {
                _cbbLayout.SelectedIndex = 0;
                var layoutSelect = _cbbLayout.SelectedItem as ReportTemplateGridViewModel;
                if (layoutSelect != null)
                    ServiceLocator.ReportCache[modality] = layoutSelect.Id;
            }
        }

        private void PopulateFormData()
        {
            var Patient = _ServiceOrderResponse.Patient;
            if (Patient != null)
                PopulatePatientInfo(Patient);

            PopulateChiDinhInfo();
            LoadSignatureInfo();
        }

        private void PopulateChiDinhInfo()
        {
            _txMaChiDinh.Text = _ServiceOrderResponse.MaChiDinh ?? "";
            _dateNgayChiDinh.DateTime = _ServiceOrderResponse.Thoigianthuchien.AddHours(7);
            _txBSChiDinh.Text = _ServiceOrderResponse.TenBacSiChiDinh ?? "";
            _txDoiTuong.Text = string.IsNullOrWhiteSpace(_ServiceOrderResponse.Patient.MaBHYT)
                ? "Viện phí"
                : "BHYT";
            _txMaBHYT.Text = _ServiceOrderResponse.Patient.MaBHYT;
            _txDichVu.Text = _ServiceOrderResponse.TenDichVu ?? "";
            _txChanDoan.Text = _ServiceOrderResponse.ChanDoanSoBo ?? "";

            if (ServiceLocator.KeycloakUserInfo != null)
                _txBSDoc.Text = $"{ServiceLocator.KeycloakUserInfo.FirstName} {ServiceLocator.KeycloakUserInfo.LastName}";
        }

        // Mang từ FrmMain._tsmEditPatient_Click sang, gắn vào nút "Sửa thông tin" của màn kết luận.
        private void _btnEditPatient_Click(object sender, EventArgs e)
        {
            try
            {
                using (var patientForm = new PatientForm(
                    _ServiceOrderResponse.Patient,
                    _ServiceOrderResponse.MaChiDinh))
                {
                    if (patientForm.ShowDialog() == DialogResult.OK)
                    {
                        PopulatePatientInfo(_ServiceOrderResponse.Patient);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi sửa thông tin bệnh nhân");
                MessageBox.Show(this, $"Lỗi khi sửa thông tin bệnh nhân: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulatePatientInfo(dynamic Patient)
        {
            _txMaBN.Text = Patient.MaPatient ?? "";
            _txTenBN.Text = Patient.HoTen ?? "";
            _dateBN.DateTime = Patient.NgaySinh;
            _txPatientGender.Text =
                Patient.GioiTinh == 1 ? "Nữ" :
                Patient.GioiTinh == 0 ? "Nam" :
                "";
            _txQueQuan.Text = $"{Patient.XaPhuong ?? ""}-{Patient.TinhThanh ?? ""}";
        }

        private async void LoadSignatureInfo()
        {
            if (string.IsNullOrEmpty(_ServiceOrderResponse.MaBacSiChiDinh))
                return;

            try
            {
                _HisUserSignatureResponse = await ServiceLocator.SignatureService.GetByHisUserKySoIdAsync(
                    _ServiceOrderResponse.MaBacSiChiDinh);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không thể load thông tin ký số cho: {MaBacSi}", _ServiceOrderResponse.MaBacSiChiDinh);
            }
        }

        private async Task InitDanhSachThietbiAsync()
        {
            try
            {
                _listThietBi = await ServiceLocator.RisService2.GetDevicesAsync(modality: _ServiceOrderResponse?.Modality);

                if (_listThietBi == null || _listThietBi.Count == 0)
                {
                    Log.Warning("Danh sách thiết bị rỗng");
                    return;
                }

                ConfigureThietBiLookup();
                InitComboxMauGoiY();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi tải danh sách thiết bị");
                MessageBox.Show(this, $"Lỗi khi tải danh sách thiết bị: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureThietBiLookup()
        {
            _cbbDSThietBi.Properties.DataSource = _listThietBi;
            _cbbDSThietBi.Properties.DisplayMember = "name";
            _cbbDSThietBi.Properties.ValueMember = "id";
            _cbbDSThietBi.Properties.NullText = "Chọn thiết bị...";
            _cbbDSThietBi.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
            _cbbDSThietBi.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            _cbbDSThietBi.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            _cbbDSThietBi.Properties.PopulateColumns();
            foreach (DevExpress.XtraEditors.Controls.LookUpColumnInfo col in _cbbDSThietBi.Properties.Columns)
                col.Visible = col.FieldName == "code" || col.FieldName == "name";
            _cbbDSThietBi.Properties.Columns["code"].Caption = "Mã thiết bị";
            _cbbDSThietBi.Properties.Columns["name"].Caption = "Tên thiết bị";
        }

        private async Task InitDanhSachKTVAsync()
        {
            try
            {
                var orgCode = ServiceLocator.SelectedOrganizationCode;
                if (string.IsNullOrWhiteSpace(orgCode))
                {
                    Log.Warning("Không có mã khoa để tra danh sách KTV/Y tá cùng khoa");
                    return;
                }

                _listHisUser = await ServiceLocator.RisService2.GetColleaguesAsync(
                    orgCode,
                    titleCodes: new System.Collections.Generic.List<string> { "NURSE", "TECHNICIAN" });

                if (this.InvokeRequired)
                    this.Invoke((MethodInvoker)ConfigureKTVLookup);
                else
                    ConfigureKTVLookup();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi tải danh sách KTV");
            }
        }

        private void ConfigureKTVLookup()
        {
            _cbbHisUser.Properties.DataSource = _listHisUser;
            _cbbHisUser.Properties.DisplayMember = "fullName";
            _cbbHisUser.Properties.ValueMember = "id";
            _cbbHisUser.Properties.NullText = "Chọn KTV...";
            _cbbHisUser.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
            _cbbHisUser.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            _cbbHisUser.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            _cbbHisUser.Properties.PopulateColumns();
            foreach (DevExpress.XtraEditors.Controls.LookUpColumnInfo col in _cbbHisUser.Properties.Columns)
                col.Visible = col.FieldName == "staffCode" || col.FieldName == "fullName";
            _cbbHisUser.Properties.Columns["fullName"].Caption = "Tên KTV";
            _cbbHisUser.Properties.Columns["staffCode"].Caption = "Mã KTV";
        }

        private void ApplyThietBiVaKTVSelectionFromResult()
        {
            if (_listHisUser != null && _listHisUser.Count > 0)
            {
                var selectedUser = _kqChanDoanResponse != null
                    ? _listHisUser.FirstOrDefault(u => u.staffCode == _kqChanDoanResponse.MaKyThuatVien)
                    : null;
                _cbbHisUser.EditValue = selectedUser?.id;
            }

            if (_listThietBi != null && _listThietBi.Count > 0)
            {
                var selectedThietBi = _kqChanDoanResponse != null
                    ? _listThietBi.FirstOrDefault(x => x.code == _kqChanDoanResponse.MaThietBi)
                    : null;
                _cbbDSThietBi.EditValue = selectedThietBi?.id ?? 0;
            }
        }

        private async Task InitCheckKetQuaChanDoanAsync()
        {
            try
            {
                _kqChanDoanResponse = await ServiceLocator.RisService.GetKetQuaChanDoanAsync(_machidinh);

                if (this.InvokeRequired)
                    this.Invoke((MethodInvoker)UpdateUIFromKetQuaChanDoan);
                else
                    UpdateUIFromKetQuaChanDoan();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi load kết quả chẩn đoán");
                _dateTGThucHien.DateTime = DateTime.Now;
                _dateTGKetThuc.DateTime = DateTime.Now.AddMinutes(5);
            }
        }

        private void UpdateUIFromKetQuaChanDoan()
        {
            if (_kqChanDoanResponse != null)
            {
                _dateTGThucHien.DateTime = _kqChanDoanResponse.NgayKetQua.AddHours(7).AddMinutes(-2);
                _dateTGKetThuc.DateTime = _kqChanDoanResponse.NgayKetQua.AddHours(7);
                _txBSDoc.Text = _kqChanDoanResponse.BacSiKetLuan ?? "";

                _rtMoTa.Text = _kqChanDoanResponse.Kqcls_MoTa ?? "";
                _rtKetLuan.Text = _kqChanDoanResponse.Kqcls_KetLuan ?? "";
                _rtKhuyenNghi.Text = _kqChanDoanResponse.Kqcls_DeNghi ?? "";

            }
            else
            {
                _dateTGThucHien.DateTime = DateTime.Now;
                _dateTGKetThuc.DateTime = DateTime.Now.AddMinutes(5);
                _btnPreviewMain.Enabled = false;
                _btnPrint.Enabled = false;
                _btnSignature.Enabled = false;
            }

            ApplyConclusionEditability();
        }

        /// <summary>
        /// Load danh sách key ảnh đã chọn lần trước (từ XML lưu cùng BN) - KHÔNG tự nạp lại
        /// thumbnail ảnh cũ vào _thumbnailList (khác FrmMain gốc - nơi này cần LoadRasterImage
        /// của Leadtools để đọc lại ảnh từ ổ đĩa). Việc khôi phục thumbnail ảnh cũ để bổ sung sau.
        /// </summary>
        private void LoadImageData()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi load image data");
            }
        }
    }
}
