using STM.MediaToPACS.Main.Utilities;
using Serilog;
using System;
using System.Threading.Tasks;

namespace STM.MediaToPACS.Main
{
    /// <summary>
    /// Wiring cho sidebar trái (Lịch sử khám bệnh nhân + Tham số siêu âm) - xem
    /// UI\PatientSidebar\PatientSidebarControl. Control tự quản lý thu gọn/mở rộng/ghim;
    /// FrmMain chỉ lo: đổ dữ liệu lịch sử, ẩn/hiện splitter kéo giãn theo trạng thái,
    /// và lưu/nạp bề rộng + trạng thái ghim vào UiLayoutSettings.xml.
    /// </summary>
    public partial class FrmMain
    {
        /// <summary>Gọi 1 lần sau InitializeComponent trong constructor.</summary>
        private void InitPatientSidebar()
        {
            _patientSidebar.CollapsedChanged += (s, e) =>
                _patientSidebarSplitter.Visible = !_patientSidebar.Collapsed;

            _patientSidebar.PinnedChanged += (s, e) =>
                SaveUiLayout(settings => settings.PatientSidebarPinned = _patientSidebar.Pinned);

            // Nạp bề rộng/trạng thái ghim đã lưu sau khi form có kích thước thật (tránh clamp
            // sai bằng kích thước lúc thiết kế) - cùng cách với ApplySavedCameraColumnWidth.
            this.Shown += (s, e) => RestorePatientSidebarState();
        }

        private void RestorePatientSidebarState()
        {
            try
            {
                var saved = XmlSettingsHelper.Load<UiLayoutSettings>(UiLayoutSettingsPath);

                if (saved != null && saved.ParamSidebarWidth > 0)
                {
                    int maxWidth = Math.Max(_patientSidebarSplitter.MinSize,
                        xtraTabPage1.ClientSize.Width - _patientSidebarSplitter.MinExtra);
                    _patientSidebar.ExpandedWidth = Math.Max(
                        _patientSidebarSplitter.MinSize,
                        Math.Min(saved.ParamSidebarWidth, maxWidth));
                }

                // Đã ghim thì mở rộng sẵn (SetCollapsed(false) bên trong sẽ tự bắn CollapsedChanged
                // để hiện splitter) - gọi sau khi đã nạp ExpandedWidth ở trên.
                _patientSidebar.RestorePinned(saved != null && saved.PatientSidebarPinned);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không nạp được trạng thái sidebar bệnh nhân đã lưu");
            }
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
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không tải được lịch sử khám bệnh nhân cho mã chỉ định {MaChiDinh}", _machidinh);
                _patientSidebar.ShowHistoryError("Không tải được lịch sử khám (lỗi kết nối RIS).");
            }
        }
    }
}
