using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Drawing;
using Application = System.Windows.Forms.Application;
using Font = System.Drawing.Font;
//using VisioForge.Core.VideoCapture; // VisioForge đã thay bằng FlashCap
using STM.MediaToPACS.Main.UI.CameraUI;
using DevExpress.XtraGrid.Views.Grid;
using STM.MediaToPACS.Main.Utilities;
using MediaToPacs.Core.Models;
using System.Linq;
using STM.MediaToPacs.Connection.AuthSDK;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using MediaToPacs.Core.Models.Order;
using MediaToPacs.Core.Models.ServiceCatalog;
using MediaToPacs.Core.Models.Conclusion;
using MediaToPacs.Core.Models.Suggestion;
using MediaToPacs.Core.Models.Template;
using MediaToPacs.Core.Models.Device;
using MediaToPacs.Core.Models.Signature;
using MediaToPacs.Core.Utilities;
using System.Threading.Tasks;
using STM.MediaToPACS.Main.UI.DiagnosticReports;
using STM.MediaToPACS.Main.UI.PatientSidebar;
using STM.MediaToPACS.Main.UI.Configurations;
using DevExpress.Utils;
using DevExpress.XtraTab;

namespace STM.MediaToPACS.Main.UI
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        private readonly Dictionary<string, XtraTabPage> _orderPages =
            new Dictionary<string, XtraTabPage>(StringComparer.OrdinalIgnoreCase);
        private bool _orderPagesCleanupDone;

        public MainForm()
        {
            try
            {
                InitializeComponent();
                xtraTabControl1.ClosePageButtonShowMode = ClosePageButtonShowMode.InAllTabPageHeaders;
                xtraTabPage1.ShowCloseButton = DefaultBoolean.False;
                xtraTabControl1.CloseButtonClick += OrderTabs_CloseButtonClick;
                InitPermissionControl();
                //foreach (var device in new VideoCaptureCore().Video_CaptureDevices()) // VisioForge đã thay bằng FlashCap
                foreach (var device in CameraControl.GetVideoDevices())
                {
                    _tsmCbbVideoCapture.Items.Add(device.Name);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
        
        private void InitPermissionControl()
        {
            var userInfo = ServiceLocator.SessionService.GetCurrentUser();
            string fullName = string.Join(" ", new[] { userInfo.LastName, userInfo.FirstName }
                .Where(s => !string.IsNullOrWhiteSpace(s)));

            _tSSLUserName.Text = $"{userInfo.Username}";
            _tssNguoiDung.Text = $"{fullName}";
            //SetControlPermission(_btnSettings, AppPermissions.Admin);
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            _gridViewChiDinh.CustomColumnDisplayText += (s, ex) =>
            {
                if (ex.Column.FieldName == "GioiTinh")
                {
                    if (ex.Value?.ToString() == "1") ex.DisplayText = "Nữ";
                    else if (ex.Value?.ToString() == "0") ex.DisplayText = "Nam";
                    else ex.DisplayText = "Khác";
                }
            };

            InitializeModalityComboBox();
            //if (new VideoCaptureCore().Video_CaptureDevices().Count>0) // VisioForge đã thay bằng FlashCap
            if (_tsmCbbVideoCapture.Items.Count > 0)
            {

                _tsmCbbVideoCapture.SelectedIndex = 0;
            }
            _dtDateFromRis.DateTime = DateTime.Today;
            _dtDateToRis.DateTime = DateTime.Today;
            var warnings = ServiceLocator.ValidateSystemConfig();
            if (warnings.Any())
            {
                MessageBox.Show(
                    string.Join("\n• ", warnings.Prepend("Cảnh báo cấu hình:")),
                    "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }

            // PACS có thể optional
            ServiceLocator.InitializeOptionalServices();
        }

        //Khởi tạo danh mục phương thức chụp cấu hình
        private void InitializeModalityComboBox()
        {
            _ccbModalities.Items.Clear();

            var defaultModalities = XmlSettingsHelper.Load<Modalities>(
                Path.Combine(
                    ServiceLocator.GetAppDataBasePath(),
                    FileStorageSettingsProvider.Current.Modality
                )
            );

            if (!string.IsNullOrWhiteSpace(defaultModalities?.ModalitiesList))
            {
                var modalities = defaultModalities.ModalitiesList
                    .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var modality in modalities)
                {
                    _ccbModalities.Items.Add(modality);
                }

                if (_ccbModalities.Items.Count > 0)
                    _ccbModalities.SelectedIndex = 0;
            }
        }



        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_orderPagesCleanupDone && e.CloseReason != CloseReason.WindowsShutDown)
            {
                var confirm = XtraMessageBox.Show(
                    this,
                    "Bạn có chắc chắn muốn đóng ứng dụng không?",
                    "Xác nhận đóng ứng dụng",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (!_orderPagesCleanupDone
                && e.CloseReason != CloseReason.WindowsShutDown
                && _orderPages.Count > 0)
            {
                e.Cancel = true;
                var controls = _orderPages.Values
                    .SelectMany(page => page.Controls.OfType<DiagnosticReportConclusionControl>())
                    .ToList();
                foreach (var control in controls)
                    await control.StopCameraAsync();

                _orderPagesCleanupDone = true;
                Close();
                return;
            }
        }


        private void _tsmLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                bool result = await Token.Logout(ServiceLocator.SessionService.GetCurrentUser().RefreshToken);
                Token.Cancel();
                Application.Exit();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                //Log.Error(ex.Message);
            }
        }


        private void _btnSearchRIS_Click(object sender, EventArgs e)
        {
            LoadDanhSachChiDinhAsync();
        }

        private void textEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadDanhSachChiDinhAsync();
                e.Handled = true;
            }
        }

        public static List<OrderGridView> ConvertToGridView(List<ServiceOrderResponse> dataList)
        {
            var result = new List<OrderGridView>();

            foreach (var item in dataList)
            {
                if (item == null) continue;

                var bn = item.Patient;

                var gridItem = new OrderGridView
                {
                    // ===== Bệnh nhân =====
                    MaPatient = bn?.MaPatient ?? "",
                    HoTen = bn?.HoTen ?? "",
                    NgaySinh = bn?.NgaySinh ?? DateTime.MinValue,
                    GioiTinh = bn?.GioiTinh ?? 0, // 1 = Nam, 0 = Nữ
                    TuNgayBHYT = bn?.TuNgayBHYT ?? DateTime.MinValue,
                    DenNgayBHYT = bn?.DenNgayBHYT ?? DateTime.MinValue,
                    MaBHYT = bn?.MaBHYT ?? "",
                    XaPhuong = bn?.XaPhuong ?? "",
                    TinhThanh = bn?.TinhThanh ?? "",
                    DanToc = bn?.DanToc ?? "",

                    // ===== Chỉ định =====
                    Sovaovien = item.Sovaovien ?? "",
                    MaChiDinh = item.MaChiDinh ?? "",
                    SoPhieuChiDinh = item.SoPhieuChiDinh ?? "",
                    MaDichVu = item.MaDichVu ?? "",
                    TenDichVu = item.TenDichVu ?? "",
                    Modality = item.Modality ?? "",
                    MaBacSiChiDinh = item.MaBacSiChiDinh ?? "",
                    TenBacSiChiDinh = item.TenBacSiChiDinh ?? "",
                    Thoigianthuchien = item.Thoigianthuchien,
                    MaNoiChiDinh = item.MaNoiChiDinh ?? "",
                    TenNoiChiDinh = item.TenNoiChiDinh ?? "",
                    TrangThai = item.TrangThai ?? "",
                    CreateAt = item.CreateAt,
                    UpdatedAt = item.UpdatedAt ?? "",
                    Id = item.Id ?? ""
                };

                result.Add(gridItem);
            }

            return result;
        }

        private async void LoadDanhSachChiDinhAsync()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitFormLoading), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu...");
                SplashScreenManager.Default.SetWaitFormDescription("Vui lòng chờ trong giây lát...");

                _gridControlChiDinh.DataSource = null;

                string maPatient = string.IsNullOrWhiteSpace(_txPatientCodeRis.Text) ? null : _txPatientCodeRis.Text;
                string tenPatient = string.IsNullOrWhiteSpace(_txPatientNameRis.Text) ? null : _txPatientNameRis.Text;
                string maChiDinh = string.IsNullOrWhiteSpace(_txMaCD.Text) ? null : _txMaCD.Text;
                string tenBacSiChiDinh = string.IsNullOrWhiteSpace(_txBSCDRis.Text) ? null : _txBSCDRis.Text;
                DateTime dateTimeFrom = _dtDateFromRis.DateTime;
                DateTime dateTo = _dtDateToRis.DateTime;
                string trangThai = string.IsNullOrWhiteSpace(_cbbTrangThai.Text) ? null : _cbbTrangThai.Text;
                var dsCD = await ServiceLocator.RisService.GetDSChiDinhDichVuAsync(
                        page: (int)_nudPage.Value,
                        pageSize: int.Parse(_cbPageSize.Text),
                        maPatient: maPatient,
                        maChiDinh: maChiDinh,
                        modality: _ccbModalities.Text,
                        tenBacSiChiDinh: tenBacSiChiDinh,
                        tenPatient: tenPatient,
                        dateTimeFrom: dateTimeFrom,
                        dateTimeTo: dateTo,
                        trangThai: trangThai
                    );

                var list = ConvertToGridView(dsCD.data);
                _lbSLCaChup.Text = list.Count.ToString();
                _gridControlChiDinh.DataSource = list;
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show(this, $"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private async void _gridViewChiDinhCT_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                Point pt = view.GridControl.PointToClient(Control.MousePosition);
                GridHitInfo info = view.CalcHitInfo(pt);

                if (info.InRowCell)
                {
                    // Lấy chi tiết chỉ định (con)
                    var obj = view.GetRow(info.RowHandle) as ChiTietChiDinh;
                    if (obj != null)
                    {
                        string maChiDinh = obj.MaChiDinh;
                        var parent = _gridViewChiDinh.GetFocusedRow() as OrderGridView;
                        if (parent != null)
                        {
                            SplashScreenManager.ShowForm(this, typeof(WaitFormLoading), true, true, false);
                            SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu...");
                            SplashScreenManager.Default.SetWaitFormDescription("Vui lòng chờ trong giây lát...");
                            string soPhieu = parent.SoPhieuChiDinh;
                            bool allowOpen = await CheckThanhToanHoiTiepAsync(maChiDinh);
                            if (!allowOpen)
                            {
                                SplashScreenManager.CloseForm(false);
                                return;
                            }
                            SplashScreenManager.CloseForm(false);
                            OpenOrderPage(soPhieu, maChiDinh, parent.HoTen);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Console.WriteLine(ex.Message);
            }
        }

        private async Task<bool> CheckThanhToanHoiTiepAsync(string maChiDinh)
        {
            try
            {
                bool duTien = await ServiceLocator.HisService
                    .KiemTraDuTienAsync(ServiceLocator.SystemConfig.CheckThanhToan, maChiDinh);

                // Đã thanh toán → đi tiếp luôn
                if (duTien)
                    return true;

                return AskContinue("Bệnh nhân chưa thanh toán đủ tiền.");
            }
            catch
            {
                // Lỗi API → cũng hỏi tiếp
                return AskContinue(
                     "Không thể kiểm tra trạng thái thanh toán.\n" +
                     "Nguyên nhân có thể do hệ thống chưa được cấu hình hoặc không kết nối được dịch vụ thanh toán."
                 );

            }
        }

        private bool AskContinue(string reason)
        {
            var result = XtraMessageBox.Show(
                $"{reason}\nBạn có muốn tiếp tục kết luận không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            return result == DialogResult.Yes;
        }

        private async void _gridViewChiDinh_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //try
            //{
            //    if (e.FocusedRowHandle >= 0)
            //    {
            //        var selected = _gridViewChiDinh.GetRow(e.FocusedRowHandle) as OrderGridView;
            //        if (selected != null)
            //        {
            //            string soPhieu = selected.SoPhieu;
            //            var chidinh = await ServiceLocator.RisService.GetChiDinhAsync(soPhieu);
            //            _gridControlChiDinhCT.DataSource = chidinh.Data.ChiDinh.DanhSach;
            //        }

            //    }
            //}catch(Exception ex)
            //{

            //}
        }

        private async void _gridViewChiDinh_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                Point pt = view.GridControl.PointToClient(Control.MousePosition);
                GridHitInfo info = view.CalcHitInfo(pt);

                if (info.InRowCell)
                {
                    // Lấy chi tiết chỉ định (con)
                    var obj = view.GetRow(info.RowHandle) as OrderGridView;
                    if (obj != null)
                    {
                        bool allowOpen = await CheckThanhToanHoiTiepAsync(obj.MaChiDinh);
                        if (!allowOpen)
                            return;

                        OpenOrderPage(obj.SoPhieuChiDinh, obj.MaChiDinh, obj.HoTen);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OpenOrderPage(string soPhieu, string maChiDinh, string patientName)
        {
            if (string.IsNullOrWhiteSpace(maChiDinh))
                return;

            if (_orderPages.TryGetValue(maChiDinh, out var existingPage))
            {
                xtraTabControl1.SelectedTabPage = existingPage;
                return;
            }

            var content = new DiagnosticReportConclusionControl(_tsmCbbVideoCapture.Text, soPhieu, maChiDinh)
            {
                Dock = DockStyle.Fill
            };
            content.OrderNavigationRequested += OrderContent_OrderNavigationRequested;

            var page = new XtraTabPage
            {
                Text = BuildOrderPageCaption(patientName, maChiDinh),
                Tag = maChiDinh,
                ShowCloseButton = DefaultBoolean.True
            };
            page.Controls.Add(content);
            xtraTabControl1.TabPages.Add(page);
            _orderPages[maChiDinh] = page;
            xtraTabControl1.SelectedTabPage = page;
        }

        private void OrderContent_OrderNavigationRequested(
            object sender, OrderNavigationRequestedEventArgs e)
        {
            OpenOrderPage(e.OrderCode, e.PlacerOrderItemCode, e.PatientName);
        }

        private async void OrderTabs_CloseButtonClick(object sender, EventArgs e)
        {
            var closeArgs = e as DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs;
            var page = closeArgs?.Page as XtraTabPage ?? xtraTabControl1.SelectedTabPage;
            if (page == null || page == xtraTabPage1)
                return;

            var confirm = MessageBox.Show(
                this,
                $"Bạn có chắc chắn muốn đóng màn hình kết luận:\n{page.Text} không?",
                "Xác nhận đóng",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            string maChiDinh = page.Tag as string;
            var content = page.Controls.OfType<DiagnosticReportConclusionControl>().FirstOrDefault();
            if (content != null)
            {
                content.OrderNavigationRequested -= OrderContent_OrderNavigationRequested;
                await content.StopCameraAsync();
            }

            if (!string.IsNullOrWhiteSpace(maChiDinh))
                _orderPages.Remove(maChiDinh);
            xtraTabControl1.TabPages.Remove(page);
            page.Dispose();
        }

        private static string BuildOrderPageCaption(string patientName, string maChiDinh)
        {
            string name = string.IsNullOrWhiteSpace(patientName) ? "Bệnh nhân" : patientName.Trim();
            return $"{name} - {maChiDinh}";
        }

        private void _tsmCbbVideoCapture_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void _tsmSetting_Click(object sender, EventArgs e)
        {
            using (var dialog = new SystemSettingsDialog())
            {
                dialog.ShowDialog(this);
            }
        }

        private void _tsmTemplateSuggestion_Click(object sender, EventArgs e)
        {
            using (var dialog = new TemplateSuggestionDialog())
            {
                dialog.ShowDialog(this);
            }
        }

        private void _tsmToUse_Click(object sender, EventArgs e)
        {

        }

        private void _tsmChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceLocator.SessionService?.OpenChangePasswordPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Không thể mở trang đổi mật khẩu.\n{ex.Message}",
                    "Đổi mật khẩu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }
    }
}
