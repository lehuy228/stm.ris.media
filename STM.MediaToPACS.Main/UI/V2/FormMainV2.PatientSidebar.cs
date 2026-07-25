using System;
using System.IO;
using System.Threading.Tasks;
using STM.MediaToPACS.Main.Utilities;
using Serilog;

namespace STM.MediaToPACS.Main.UI.V2
{
    /// <summary>
    /// Wiring cho sidebar trái (Lịch sử khám bệnh nhân + Tham số siêu âm) - chuyển thể từ
    /// FrmMain.PatientSidebar.cs. FormMainV2 chỉ lo: đổ dữ liệu lịch sử, ẩn/hiện splitter kéo giãn
    /// theo trạng thái, và lưu/nạp bề rộng + trạng thái ghim vào UiLayoutSettings.xml.
    /// </summary>
    public partial class FormMainV2
    {
        private const float CameraColumnMinWidth = 420F;
        private const float ReportColumnMinWidth = 420F;
        private bool _cameraColumnDragging;
        private int _cameraColumnDragStartX;
        private float _cameraColumnDragStartWidth;

        private static string UiLayoutSettingsPath => Path.Combine(
            ServiceLocator.GetAppDataBasePath(), "UiLayoutSettings.xml");

        private void PatientSidebar_PinnedChanged(object sender, EventArgs e)
        {
            SaveUiLayout(settings => settings.PatientSidebarPinned = _patientSidebar.Pinned);
        }

        private void RestorePatientSidebarState()
        {
            try
            {
                var saved = XmlSettingsHelper.Load<UiLayoutSettings>(UiLayoutSettingsPath);

                if (saved != null && saved.ParamSidebarWidth > 0)
                {
                    int bodyWidth = _bodyPanel.ClientSize.Width > 0
                        ? _bodyPanel.ClientSize.Width
                        : this.ClientSize.Width;
                    int maxWidth = Math.Max(_patientSidebarSplitter.MinSize,
                        bodyWidth - _patientSidebarSplitter.MinExtra);
                    _patientSidebar.ExpandedWidth = Math.Max(
                        _patientSidebarSplitter.MinSize,
                        Math.Min(saved.ParamSidebarWidth, maxWidth));
                }

                // Đã ghim thì mở rộng sẵn (SetCollapsed(false) bên trong sẽ tự bắn CollapsedChanged
                // để hiện splitter) - gọi sau khi đã nạp ExpandedWidth ở trên.
                _patientSidebar.RestorePinned(saved != null && saved.PatientSidebarPinned);
                RestoreCameraColumnWidth(saved);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không nạp được trạng thái sidebar bệnh nhân đã lưu");
            }
        }

        private System.Windows.Forms.ColumnStyle CameraColumnStyle =>
            _contentTable != null && _contentTable.ColumnStyles.Count >= 3
                ? _contentTable.ColumnStyles[2]
                : null;

        private float ClampCameraColumnWidth(float width)
        {
            float availableWidth = _contentTable.ClientSize.Width
                                   - _contentTable.ColumnStyles[1].Width;
            float maxWidth = Math.Max(CameraColumnMinWidth, availableWidth - ReportColumnMinWidth);
            return Math.Max(CameraColumnMinWidth, Math.Min(width, maxWidth));
        }

        private void RestoreCameraColumnWidth(UiLayoutSettings settings)
        {
            var style = CameraColumnStyle;
            if (style == null)
                return;

            float requestedWidth = settings != null && settings.CameraColumnWidth > 0
                ? settings.CameraColumnWidth
                : Math.Max(CameraColumnMinWidth, _contentTable.ClientSize.Width * 0.46F);
            style.SizeType = System.Windows.Forms.SizeType.Absolute;
            style.Width = ClampCameraColumnWidth(requestedWidth);
        }

        private void CameraColumnSplitter_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var style = CameraColumnStyle;
            if (style == null || e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            _cameraColumnDragging = true;
            _cameraColumnDragStartX = System.Windows.Forms.Cursor.Position.X;
            _cameraColumnDragStartWidth = style.Width;
            _cameraColumnSplitter.BackColor = System.Drawing.Color.FromArgb(0, 120, 212);
            _cameraColumnSplitter.Capture = true;
        }

        private void CameraColumnSplitter_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!_cameraColumnDragging)
                return;

            var style = CameraColumnStyle;
            if (style == null)
                return;

            style.Width = ClampCameraColumnWidth(
                _cameraColumnDragStartWidth
                + _cameraColumnDragStartX
                - System.Windows.Forms.Cursor.Position.X);
        }

        private void CameraColumnSplitter_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!_cameraColumnDragging)
                return;

            _cameraColumnDragging = false;
            _cameraColumnSplitter.Capture = false;
            _cameraColumnSplitter.BackColor = System.Drawing.Color.FromArgb(218, 224, 232);

            var style = CameraColumnStyle;
            if (style != null)
                SaveUiLayout(settings => settings.CameraColumnWidth = style.Width);
        }

        private void PatientSidebarSplitter_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
        {
            if (_patientSidebar.Collapsed)
                return;

            _patientSidebar.ExpandedWidth = _patientSidebar.Width;
            SaveUiLayout(settings => settings.ParamSidebarWidth = _patientSidebar.Width);
            Log.Information("Đã lưu bề rộng sidebar bệnh nhân: {Width}px", _patientSidebar.Width);
        }

        /// <summary>
        /// Tải lịch sử khám bệnh nhân theo mã chỉ định hiện tại (best-effort, chạy nền lúc load form).
        /// Lỗi/không tìm thấy chỉ hiển thị trạng thái trong sidebar, không ảnh hưởng luồng chính.
        /// </summary>
        private async Task LoadPatientHistorySafeAsync()
        {
            if (string.IsNullOrWhiteSpace(_machidinh))
                return;

            _patientSidebar.ShowHistoryLoading();

            try
            {
                var history = await ServiceLocator.RisService2.GetPatientHistoryByOrderCodeAsync(_machidinh);
                _patientSidebar.ShowHistory(history, _machidinh);
                var patient = history?.patient;
                if (patient != null)
                {
                    PatientTabCaption = $"{patient.patientCode} - {patient.fullName}".Trim(' ', '-');
                    TabCaptionChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không tải được lịch sử khám bệnh nhân cho mã chỉ định {MaChiDinh}", _machidinh);
                _patientSidebar.ShowHistoryError("Không tải được lịch sử khám (lỗi kết nối RIS).");
            }
        }

        /// <summary>
        /// Cập nhật một phần UiLayoutSettings theo kiểu load-sửa-lưu để các giá trị layout
        /// khác trong file không bị ghi đè mất.
        /// </summary>
        private static void SaveUiLayout(Action<UiLayoutSettings> update)
        {
            try
            {
                var settings = XmlSettingsHelper.Load<UiLayoutSettings>(UiLayoutSettingsPath)
                               ?? new UiLayoutSettings();
                update(settings);
                XmlSettingsHelper.Save(UiLayoutSettingsPath, settings);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Lỗi khi lưu UiLayoutSettings");
            }
        }
    }
}
