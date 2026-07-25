using System;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaToPacs.Core.Enums;
using MediaToPacs.Core.Models.Ketluan;
using STM.MediaToPACS.Main.Utilities;
using Serilog;

namespace STM.MediaToPACS.Main.UI.V2
{
    /// <summary>
    /// Tải dữ liệu chỉ định/kết luận cho FormMainV2 - chuyển thể từ FrmMain.Loading.cs,
    /// bỏ hoàn toàn phần liên quan DICOM (SetupExcludedTags, InitTranfer/InitTranferRIS worklist...).
    /// </summary>
    public partial class FormMainV2
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
            var loadImageTask = Task.Run(() => LoadImageData());
            // Tra y lệnh bên RIS mới (best-effort) để sync/khôi phục bảng chỉ số
            var resolveRisV1Task = ResolveRisV1OrderItemAsync();
            // Lịch sử khám bệnh nhân cho sidebar (best-effort, không ảnh hưởng luồng chính)
            var loadHistoryTask = LoadPatientHistorySafeAsync();

            await Task.WhenAll(loadKTVTask, loadImageTask, resolveRisV1Task, loadHistoryTask);

            ApplyThietBiVaKTVSelectionFromResult();
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
                _chiDinhDichVuResponse = await ServiceLocator.RisService.GetChiDinhDichVuAsync(_machidinh);
                if (_chiDinhDichVuResponse == null)
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
            var bn = _chiDinhDichVuResponse.BenhNhan;

            await LoadSuggestionsSafeAsync(bn?.GioiTinh);

            await Task.WhenAll(
                InitDanhSachThietbiAsync(),
                InitLayoutMauAsync(_chiDinhDichVuResponse.Modality)
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
            var benhNhan = _chiDinhDichVuResponse.BenhNhan;
            if (benhNhan != null)
                PopulatePatientInfo(benhNhan);

            PopulateChiDinhInfo();
            LoadSignatureInfo();
        }

        private void PopulateChiDinhInfo()
        {
            _txMaChiDinh.Text = _chiDinhDichVuResponse.MaChiDinh ?? "";
            _dateNgayChiDinh.DateTime = _chiDinhDichVuResponse.Thoigianthuchien.AddHours(7);
            _txBSChiDinh.Text = _chiDinhDichVuResponse.TenBacSiChiDinh ?? "";
            _txDoiTuong.Text = string.IsNullOrWhiteSpace(_chiDinhDichVuResponse.BenhNhan.MaBHYT)
                ? "Viện phí"
                : "BHYT";
            _txMaBHYT.Text = _chiDinhDichVuResponse.BenhNhan.MaBHYT;
            _txDichVu.Text = _chiDinhDichVuResponse.TenDichVu ?? "";
            _txChanDoan.Text = _chiDinhDichVuResponse.ChanDoanSoBo ?? "";

            if (ServiceLocator.KeycloakUserInfo != null)
                _txBSDoc.Text = $"{ServiceLocator.KeycloakUserInfo.FirstName} {ServiceLocator.KeycloakUserInfo.LastName}";
        }

        private void PopulatePatientInfo(dynamic benhNhan)
        {
            _txMaBN.Text = benhNhan.MaBenhNhan ?? "";
            _txTenBN.Text = benhNhan.HoTen ?? "";
            _dateBN.DateTime = benhNhan.NgaySinh;
            _txPatientGender.Text =
                benhNhan.GioiTinh == 1 ? "Nữ" :
                benhNhan.GioiTinh == 0 ? "Nam" :
                "";
            _txQueQuan.Text = $"{benhNhan.XaPhuong ?? ""}-{benhNhan.TinhThanh ?? ""}";
        }

        private async void LoadSignatureInfo()
        {
            if (string.IsNullOrEmpty(_chiDinhDichVuResponse.MaBacSiChiDinh))
                return;

            try
            {
                _hisUserKySoResponse = await ServiceLocator.SignatureService.GetByHisUserKySoIdAsync(
                    _chiDinhDichVuResponse.MaBacSiChiDinh);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không thể load thông tin ký số cho: {MaBacSi}", _chiDinhDichVuResponse.MaBacSiChiDinh);
            }
        }

        private async Task InitDanhSachThietbiAsync()
        {
            try
            {
                _listThietBi = await ServiceLocator.RisService2.GetDevicesAsync(modality: _chiDinhDichVuResponse?.Modality);

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

                _rtMoTa.Text = _kqChanDoanResponse.Kqcls_MoTa ?? "";
                _rtKetLuan.Text = _kqChanDoanResponse.Kqcls_KetLuan ?? "";
                _rtKhuyenNghi.Text = _kqChanDoanResponse.Kqcls_DeNghi ?? "";

                bool isNhap = _kqChanDoanResponse.TrangThai != null && _kqChanDoanResponse.TrangThai.Equals(TrangThaiKetLuan.NHAP);
                _btnSave.Enabled = isNhap;
                _btnPrint.Enabled = true;
                _rtMoTa.Enabled = isNhap;
                _rtKetLuan.Enabled = isNhap;
                _rtKhuyenNghi.Enabled = isNhap;

                _btnSignature.Text = isNhap
                    ? $"Ký số ({ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.Sign})"
                    : $"Hủy ký số({ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.Sign})";
            }
            else
            {
                _dateTGThucHien.DateTime = DateTime.Now;
                _dateTGKetThuc.DateTime = DateTime.Now.AddMinutes(5);
                _btnPreviewMain.Enabled = false;
                _btnPrint.Enabled = false;
                _btnSignature.Enabled = false;
            }
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
                string path = Path.Combine($"{_baseFolder}\\BenhNhan\\{_machidinh}", FileNameXMLImage);
                XmlSettingsHelper.EnsureFileExists(path, () => new System.Collections.Generic.List<string>());
                listImageKeyLocal = XmlSettingsHelper.Load<System.Collections.Generic.List<string>>(path);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi load image data");
            }
        }
    }
}
