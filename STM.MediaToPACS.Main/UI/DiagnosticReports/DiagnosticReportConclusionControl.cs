using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MediaToPacs.Core.Models;
using MediaToPacs.Core.Models.Order;
using MediaToPacs.Core.Models.ServiceCatalog;
using MediaToPacs.Core.Models.Conclusion;
using MediaToPacs.Core.Models.Suggestion;
using MediaToPacs.Core.Models.Template;
using MediaToPacs.Core.Models.Device;
using MediaToPacs.Core.Models.Signature;
using STM.MediaToPACS.Main.UI.CameraUI;
using STM.MediaToPACS.Main.UI.PatientSidebar;
using STM.MediaToPACS.Main.Utilities;
using Serilog;
using MediaToPacs.Core.Enums;
using MediaToPacs.Core.Interfaces;

namespace STM.MediaToPACS.Main.UI.DiagnosticReports
{
    /// <summary>
    /// Bản dựng tạm thời (không phụ thuộc Leadtools/DICOM) tái sử dụng logic Camera + Kết luận +
    /// Lịch sử bệnh nhân từ FrmMain.
    /// Bước 1: scaffold UserControl + camera + danh sách ảnh thumbnail (ImageThumbnailList thay
    /// cho ListImageBox vốn dựng trên Leadtools RasterImage).
    /// Bước 2: kết luận (gợi ý/lưu/ký số/in) - xem DiagnosticReportConclusionControl.Suggestion.cs, .SaveLoad.cs,
    /// .Signature.cs, .Print.cs, .Loading.cs.
    /// Lịch sử bệnh nhân (bước 3) sẽ thêm sau.
    /// </summary>
    public partial class DiagnosticReportConclusionControl : UserControl
    {
        public event EventHandler<OrderNavigationRequestedEventArgs> OrderNavigationRequested;
        public event EventHandler TabCaptionChanged;
        public string PatientTabCaption { get; private set; }

        private readonly IRisService _risService;
        private readonly IRisService2 _risService2;
        private readonly ISignatureService _signatureService;
        private readonly string _videoInputDevice;
        private readonly string _sophieu;
        private readonly string _machidinh;
        private readonly string _baseFolder;

        private CameraControl _cameraControl;

        // Dữ liệu chỉ định/kết luận - xem DiagnosticReportConclusionControl.Loading.cs
        private ServiceOrderResponse _ServiceOrderResponse;
        private DiagnosisResultResponse _kqChanDoanResponse;
        private List<DeviceDto> _listThietBi;
        private List<ReportTemplateGridViewModel> _listMauBaoCao;
        private List<PractitionerListDto> _listHisUser;
        private HisUserSignatureResponse _HisUserSignatureResponse;
        private readonly string _patientOrderFolder;
        private DiagnosticReportAttachmentManifest _attachmentManifest;
        private bool _suppressAttachmentManifestSave;
        private bool _isAttachmentQueueProcessing;
        private bool _attachmentQueueRequested;
        private Panel _attachmentLoadOverlay;
        private Label _lbImageLoadStatus;
        private ContextMenuStrip _richTextContextMenu;
        private bool _isLoadingAuditLogs;

        public DiagnosticReportConclusionControl(
            IRisService risService,
            IRisService2 risService2,
            ISignatureService signatureService,
            string videoInputDevice,
            string soPhieu,
            string maChiDinh)
        {
            _risService = risService;
            _risService2 = risService2;
            _signatureService = signatureService;
            _videoInputDevice = videoInputDevice;
            _sophieu = soPhieu;
            _machidinh = maChiDinh;
            _baseFolder = ServiceLocator.GetMediaStorageBasePath();
            if (!Directory.Exists(_baseFolder))
                Directory.CreateDirectory(_baseFolder);
            _patientOrderFolder = Path.Combine(_baseFolder, "Patient", _machidinh);
            _attachmentManifest = DiagnosticReportAttachmentManifest.Load(_patientOrderFolder, _machidinh);

            InitializeComponent();
            SetupRichTextEditors();
            _btnSyncHis.Enabled = false;
            InitAttachmentLoadStatus();
            InitCamera();
            InitThumbnailListCounter();
            _btnAddFile.Click += _btnAddFile_Click;
            _btnEditPatient.Click += _btnEditPatient_Click;
            _cbHoverPreview.CheckedChanged += CbHoverPreview_CheckedChanged;
            _thumbnailList.ItemAdded += (s, e) => SaveAttachmentManifestFromThumbnails();
            _thumbnailList.SelectionChanged += (s, e) => SaveAttachmentManifestFromThumbnails();
            _thumbnailList.DeleteRequested += ThumbnailList_DeleteRequested;
            _thumbnailList.DeleteAllRequested += ThumbnailList_DeleteAllRequested;
            _patientSidebar.OrderNavigationRequested += PatientSidebar_OrderNavigationRequested;
            _patientSidebar.LogRefreshRequested += PatientSidebar_LogRefreshRequested;

            this.Load += DiagnosticReportConclusionControl_Load;
        }

        private void PatientSidebar_OrderNavigationRequested(
            object sender, OrderNavigationRequestedEventArgs e)
        {
            OrderNavigationRequested?.Invoke(this, e);
        }

        private async void PatientSidebar_LogRefreshRequested(object sender, EventArgs e)
        {
            await LoadAuditLogsSafeAsync();
        }

        /// <summary>
        /// Nạp nhật ký hệ thống của chỉ định đang mở (audit-log lọc theo orderCode = mã chỉ định HIS),
        /// sắp xếp mới nhất trước. Lỗi chỉ hiển thị trong tab, không chặn luồng kết luận.
        /// </summary>
        private async System.Threading.Tasks.Task LoadAuditLogsSafeAsync()
        {
            if (_isLoadingAuditLogs)
                return;

            if (string.IsNullOrWhiteSpace(_machidinh))
            {
                _patientSidebar.ShowLogError("Không có mã chỉ định để tra nhật ký");
                return;
            }

            try
            {
                _isLoadingAuditLogs = true;
                _patientSidebar.ShowLogLoading();

                var logs = await _risService2.GetAuditLogsByOrderCodeAsync(_machidinh);
                _patientSidebar.ShowLogs(logs);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không tải được nhật ký cho mã chỉ định {MaChiDinh}", _machidinh);
                _patientSidebar.ShowLogError("Không tải được nhật ký: " + ex.Message);
            }
            finally
            {
                _isLoadingAuditLogs = false;
            }
        }

        private bool IsConclusionDraft()
        {
            return IsConclusionStatus(TrangThaiKetLuan.NHAP);
        }

        private bool IsConclusionCompleted()
        {
            return IsConclusionStatus(TrangThaiKetLuan.HOAN_THANH);
        }

        private bool IsConclusionRevoked()
        {
            return IsConclusionStatus(TrangThaiKetLuan.DA_THU_HOI);
        }

        private bool IsConclusionStatus(string expectedStatus)
        {
            return _kqChanDoanResponse != null &&
                   string.Equals(
                       (_kqChanDoanResponse.TrangThai ?? string.Empty).Trim(),
                       expectedStatus,
                       StringComparison.OrdinalIgnoreCase);
        }

        private bool CanEditConclusion()
        {
            return _kqChanDoanResponse == null || IsConclusionDraft() || IsConclusionRevoked();
        }

        private void ApplyConclusionEditability()
        {
            var hasConclusion = _kqChanDoanResponse != null;
            var canEdit = CanEditConclusion();
            var isCompleted = IsConclusionCompleted();

            _rtMoTa.Enabled = canEdit;
            _rtKetLuan.Enabled = canEdit;
            _rtKhuyenNghi.Enabled = canEdit;
            _dateTGThucHien.Enabled = canEdit;
            _dateTGKetThuc.Enabled = canEdit;
            _btnSave.Enabled = canEdit;
            _btnSnapshot.Enabled = canEdit;
            _btnStop.Enabled = canEdit;
            _btnLinkCamera.Enabled = canEdit;
            _btnCameraSettings.Enabled = canEdit;
            _btnAddFile.Enabled = canEdit;
            _btnPushPacs.Enabled = canEdit;
            _cbbMauGoiY.Enabled = canEdit;
            _cbbHisUser.Enabled = canEdit;
            _cbbDSThietBi.Enabled = canEdit;
            _btnSignature.Enabled = hasConclusion;
            _btnPreviewMain.Enabled = hasConclusion;
            _btnPrint.Enabled = hasConclusion;
            _btnSyncHis.Enabled = hasConclusion;
            _thumbnailList.SetReadOnly(!canEdit);
            if (_paramFormControl != null)
                _paramFormControl.Enabled = canEdit;

            _btnSignature.Text = isCompleted
                ? $"Hủy ký số({ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.Sign})"
                : $"Ký số ({ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.Sign})";

            if (!canEdit)
                _ = _cameraControl?.StopCaptureAsync();
        }

        private void InitCamera()
        {
            _cameraControl = new CameraControl(_videoInputDevice)
            {
                Dock = DockStyle.Fill
            };
            _cameraViewport.Controls.Add(_cameraControl);
        }

        private void SetupRichTextEditors()
        {
            SetupRichTextContextMenu();
            SetupRichTextFonts();
        }

        private void SetupRichTextContextMenu()
        {
            _richTextContextMenu = new ContextMenuStrip();
            _richTextContextMenu.Items.Add("Sao chép", null, (s, e) => GetContextMenuRichTextBox()?.Copy());
            _richTextContextMenu.Items.Add("Dán", null, (s, e) => GetContextMenuRichTextBox()?.Paste());
            _richTextContextMenu.Items.Add("Cắt", null, (s, e) => GetContextMenuRichTextBox()?.Cut());
            _richTextContextMenu.Items.Add("Chọn tất cả", null, (s, e) => GetContextMenuRichTextBox()?.SelectAll());

            _rtKetLuan.ContextMenuStrip = _richTextContextMenu;
            _rtKhuyenNghi.ContextMenuStrip = _richTextContextMenu;
            _rtMoTa.ContextMenuStrip = _richTextContextMenu;
        }

        private RichTextBox GetContextMenuRichTextBox()
        {
            return _richTextContextMenu?.SourceControl as RichTextBox;
        }

        private void SetupRichTextFonts()
        {
            var fontSettings = ServiceLocator.ShortcutAndFontSetting.FontSettings;
            var font = new Font(fontSettings.FontFamily, fontSettings.FontSize);

            _rtKetLuan.Font = font;
            _rtKhuyenNghi.Font = font;
            _rtMoTa.Font = font;
        }

        private void InitAttachmentLoadStatus()
        {
            _attachmentLoadOverlay = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Visible = false
            };

            _lbImageLoadStatus = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Tahoma", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(107, 114, 128),
                Text = "Dang tai anh..."
            };

            _attachmentLoadOverlay.Controls.Add(_lbImageLoadStatus);
            _panelImage.Controls.Add(_attachmentLoadOverlay);
            _attachmentLoadOverlay.BringToFront();
        }

        private void SetAttachmentLoadStatus(bool loading, string text = null)
        {
            if (_attachmentLoadOverlay == null || _lbImageLoadStatus == null)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => SetAttachmentLoadStatus(loading, text)));
                return;
            }

            _lbImageLoadStatus.Text = string.IsNullOrWhiteSpace(text) ? "Dang tai anh..." : text;
            _attachmentLoadOverlay.Visible = loading;
            if (loading)
                _attachmentLoadOverlay.BringToFront();
        }

        /// <summary>Cập nhật bộ đếm "đã chọn/tổng số" ảnh (_lbImageSelect) - tương đương UpdateToolBarState trong FrmMain.</summary>
        private void InitThumbnailListCounter()
        {
            void UpdateCounter(object s, EventArgs e) =>
                _lbImageSelect.Text = $"{_thumbnailList.GetCheckedFilePaths().Count}/{_thumbnailList.Items.Count}";

            _thumbnailList.ItemAdded += UpdateCounter;
            _thumbnailList.SelectionChanged += UpdateCounter;
        }

        /// <summary>Đóng form host chứa control này (tương đương _btnCancel_Click trong FrmMain).</summary>
        private void _btnCancel_Click(object sender, EventArgs e)
        {
            this.FindForm()?.Close();
        }

        private async void _btnSyncHis_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_machidinh))
            {
                MessageBox.Show(this, "Không tìm thấy mã chỉ định để gửi lại ORU.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _btnSyncHis.Enabled = false;
                var result = await _risService2.ResendOruToHisAsync(_machidinh);
                if (result == null || !result.success)
                {
                    var message = result != null
                        ? result.BuildFailureMessage()
                        : "Không gửi lại được kết quả sang HIS.";
                    Log.Warning("Gửi lại ORU thất bại. MaChiDinh={MaChiDinh}, ErrorCode={ErrorCode}, ErrorMessage={ErrorMessage}",
                        _machidinh, result?.errorCode, result?.errorMessage);
                    MessageBox.Show(this, message, "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show(this, "Đã gửi lại kết quả sang HIS thành công.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi gửi lại  sang HIS cho MaChiDinh: {MaChiDinh}", _machidinh);
                MessageBox.Show(this, $"Lỗi khi gửi lại kết quả: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ApplyConclusionEditability();
            }
        }

        private async void DiagnosticReportConclusionControl_Load(object sender, EventArgs e)
        {
            try
            {
                // Nạp bề rộng/trạng thái ghim sidebar đã lưu sau khi control đã có kích thước thật
                // (tránh clamp sai bằng kích thước lúc thiết kế).
                RestorePatientSidebarState();

                await InitConclusionDataAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi tải dữ liệu kết luận cho DiagnosticReportConclusionControl");
                MessageBox.Show(this, $"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Phải gọi từ FormClosing của Form host trước khi đóng, để dừng camera an toàn
        /// (giống FrmMain_FormClosing) - tránh dispose tài nguyên trong khi camera còn đang dừng.
        /// </summary>
        public async System.Threading.Tasks.Task StopCameraAsync()
        {
            try
            {
                if (_cameraControl != null)
                    await _cameraControl.StopCaptureAsync();
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Lỗi khi dừng camera lúc đóng DiagnosticReportConclusionControl");
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            var keys = ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys;
            if (keyData == ParseKey(keys.Preview))
            {
                _btnPreviewMain.PerformClick();
                return true;
            }
            if (keyData == ParseKey(keys.Sign))
            {
                _btnSignature.PerformClick();
                return true;
            }
            if (keyData == ParseKey(keys.Print))
            {
                _btnPrint.PerformClick();
                return true;
            }
            if (keyData == ParseKey(keys.Draft))
            {
                _btnSave.PerformClick();
                return true;
            }
            if (keyData == ParseKey(keys.Exit))
            {
                _btnCancel.PerformClick();
                return true;
            }
            if (keyData == ParseKey(keys.Snapshot))
            {
                _btnSnapshot.PerformClick();
                return true;
            }
            if (keyData == ParseKey(keys.Stop))
            {
                _btnStop.PerformClick();
                return true;
            }
            if (keyData == ParseKey(keys.LinkCamera))
            {
                _btnLinkCamera.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private static Keys ParseKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                return Keys.None;

            try
            {
                return (Keys)Enum.Parse(typeof(Keys), key, true);
            }
            catch
            {
                return Keys.None;
            }
        }
    }
}
