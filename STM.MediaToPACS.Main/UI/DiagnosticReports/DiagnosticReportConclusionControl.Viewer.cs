using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MediaToPacs.Core.Models;
using STM.MediaToPACS.Main.UI;
using STM.MediaToPACS.Main.Utilities;
using Serilog;

namespace STM.MediaToPACS.Main.UI.DiagnosticReports
{
    /// <summary>
    /// Danh sách nút "Xem ảnh PACS" - mỗi viewer bác sĩ được phép dùng
    /// (GET /risv1/staff/viewer-accesses) hiện thành 1 nút riêng, đặt liền nhau; bấm nút nào thì
    /// gọi POST /risv1/order-items/by-placer-code/{code}/viewer-link (theo mã chỉ định _machidinh +
    /// staffCode, không cần đăng nhập/orderItemId nội bộ) lấy link mở ảnh cho đúng viewer đó rồi
    /// mở trong FormViewerBrowser. Không viewer nào được gán → không hiện nút nào cả.
    /// Xem docs/api/ris-v1.md §9d.
    /// </summary>
    public partial class DiagnosticReportConclusionControl
    {
        private readonly List<SimpleButton> _viewerButtons = new List<SimpleButton>();

        /// <summary>Tải danh sách viewer khi mở phiếu (best-effort) rồi dựng nút tương ứng.</summary>
        private async Task LoadViewerAccessesBestEffortAsync()
        {
            List<PractitionerViewerAccessDto> viewers;
            try
            {
                var staffCode = ServiceLocator.KeycloakUserInfo?.HISCode;
                viewers = await ServiceLocator.RisService2.GetViewerAccessesAsync(staffCode);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không tải được danh sách viewer PACS được phép");
                viewers = null;
            }

            RenderViewerButtons(viewers);
        }

        /// <summary>Xoá hết nút viewer cũ rồi dựng lại đúng theo danh sách server trả về - không có thì thôi.</summary>
        private void RenderViewerButtons(List<PractitionerViewerAccessDto> viewers)
        {
            foreach (var oldButton in _viewerButtons)
            {
                _patientActionButtons.Controls.Remove(oldButton);
                oldButton.Dispose();
            }
            _viewerButtons.Clear();

            if (viewers == null || viewers.Count == 0)
                return;

            // Chèn ngay sau _btnEditPatient trong Controls collection để các nút viewer nằm liền
            // nhau, đúng vị trí cũ của nút "Xem ảnh PACS" tĩnh trước đây.
            var insertIndex = _patientActionButtons.Controls.GetChildIndex(_btnEditPatient) + 1;

            foreach (var viewer in viewers)
            {
                var viewerName = viewer.viewerName;
                var button = new SimpleButton
                {
                    Text = viewerName,
                    AutoSize = true,
                    Height = 26,
                    Margin = new Padding(3, 1, 3, 1)
                };
                button.Appearance.Font = new Font("Tahoma", 8F, FontStyle.Regular);
                button.Appearance.Options.UseFont = true;
                button.LookAndFeel.UseDefaultLookAndFeel = true;
                button.Click += async (s, e) => await ViewerButton_ClickAsync(button, viewerName);

                _patientActionButtons.Controls.Add(button);
                _patientActionButtons.Controls.SetChildIndex(button, insertIndex);
                _viewerButtons.Add(button);
            }
        }

        private async Task ViewerButton_ClickAsync(SimpleButton button, string viewerName)
        {
            try
            {
                button.Enabled = false;
                await OpenViewerLinkAsync(viewerName);
            }
            finally
            {
                button.Enabled = true;
            }
        }

        /// <summary>
        /// Gọi API lấy link viewer theo mã chỉ định (_machidinh) + staffCode + tên viewer cụ thể,
        /// rồi mở trong FormViewerBrowser (WebView2, nhúng ngay trong app) - không modal, để bác sĩ
        /// vẫn thao tác được form kết luận song song.
        /// </summary>
        private async Task OpenViewerLinkAsync(string viewerName)
        {
            try
            {
                var staffCode = ServiceLocator.KeycloakUserInfo?.HISCode;
                if (string.IsNullOrWhiteSpace(staffCode))
                {
                    MessageBox.Show(
                        this,
                        "Không xác định được mã bác sĩ đang đăng nhập để chọn quyền viewer.",
                        "Chưa xem được ảnh PACS",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var launchUrl = await ServiceLocator.RisService2
                    .GetViewerLinkByPlacerCodeAsync(_machidinh, staffCode, viewerName);
                if (string.IsNullOrWhiteSpace(launchUrl))
                {
                    MessageBox.Show(
                        this,
                        "Không lấy được link mở ảnh PACS cho chỉ định này.",
                        "Chưa xem được ảnh PACS",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var title = string.IsNullOrWhiteSpace(viewerName) ? "Xem ảnh PACS" : $"Xem ảnh PACS - {viewerName}";
                var viewerForm = new FormViewerBrowser(title, launchUrl);
                viewerForm.Show(FindForm());
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi lấy/mở link viewer PACS. MaChiDinh={MaChiDinh} ViewerName={ViewerName}", _machidinh, viewerName);
                MessageBox.Show(this, $"Lỗi khi xem ảnh PACS: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
