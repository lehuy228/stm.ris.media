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
        #region Load Ui và Dữ liệu
        private async void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                // NHÓM 1: UI Setup (Nhanh, không block)
                SetupUIComponents();

                // NHÓM 2: Critical Initialization (Cần ngay)
                await InitializeCriticalComponentsAsync();

                // NHÓM 3: Background Loading (Có thể chạy sau)
                _ = LoadBackgroundDataAsync();

                // NHÓM 4: Final Setup
                FinalizeInitialization();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FinalizeInitialization()
        {
        }

        // ============================================
        // NHÓM 1: UI Setup - Chạy ngay, không block
        // ============================================
        private void SetupUIComponents()
        {
            // Setup context menu
            SetupContextMenu();

            // Setup button texts
            SetupButtonTexts();

            // Setup fonts (cache font object để tránh tạo lại)
            SetupFonts();

            // Setup excluded tags (static data, không cần async)
            SetupExcludedTags();
        }

        private void SetupContextMenu()
        {
            contextMenuRichTextBox.Items.Add("Sao chép", null, (s, ev) => GetCurrentBox()?.Copy());
            contextMenuRichTextBox.Items.Add("Dán", null, (s, ev) => GetCurrentBox()?.Paste());
            contextMenuRichTextBox.Items.Add("Cắt", null, (s, ev) => GetCurrentBox()?.Cut());
            contextMenuRichTextBox.Items.Add("Chọn tất cả", null, (s, ev) => GetCurrentBox()?.SelectAll());

            _rtKetLuan.ContextMenuStrip = contextMenuRichTextBox;
            _rtKhuyenNghi.ContextMenuStrip = contextMenuRichTextBox;
            _rtMoTa.ContextMenuStrip = contextMenuRichTextBox;
        }

        private void SetupButtonTexts()
        {
            var keys = ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys;

            _btnCancel.Text = $"Hủy ({keys.Exit})";
            _btnPreviewMain.Text = $"Xem trước ({keys.Preview})";
            _btnPrint.Text = $"In ({keys.Print})";
            _btnSave.Text = $"Lưu nháp ({keys.Draft})";
            _btnSignature.Text = $"Ký số ({keys.Sign})";
            _btnSnapshot.Text = $"Chụp nhanh ({keys.Snapshot})";
            _btnLinkCamera.Text = $"Liên kết ({keys.LinkCamera})";
            _btnStop.Text = $"Dừng ({keys.Stop})";
        }

        private void SetupFonts()
        {
            // Cache font object để tránh tạo lại nhiều lần
            var fontSettings = ServiceLocator.ShortcutAndFontSetting.FontSettings;
            var font = new Font(fontSettings.FontFamily, fontSettings.FontSize);

            _rtKetLuan.Font = font;
            _rtKhuyenNghi.Font = font;
            _rtMoTa.Font = font;
        }

        private void SetupExcludedTags()
        {
            _ExcludedTags.AddRange(new[]
            {
                DicomTag.SOPClassUID,
                DicomTag.SOPInstanceUID,
                DicomTag.StudyInstanceUID,
                DicomTag.SeriesInstanceUID,
                DicomTag.MediaStorageSOPClassUID,
                DicomTag.FrameIncrementPointer,
                DicomTag.MIMETypeOfEncapsulatedDocument,
                DicomTag.PageNumberVector
            });
        }

        // ============================================
        // NHÓM 2: Critical Initialization - Cần ngay
        // ============================================
        private async Task InitializeCriticalComponentsAsync()
        {
            InitPermissionControl();
            InitializeForm();
            InitCbbPrinters();
            SetServersComboBox(true);
            InitializeScreenCapture();
            UpdateToolBarState();

            // Init thông tin chỉ định (có thể async)
            await InitThongTinChiDinhAsync();
        }


        private async Task InitThongTinChiDinhAsync()
        {
            try
            {
                // Lấy thông tin chỉ định dịch vụ
                _chiDinhDichVuResponse = await ServiceLocator.RisService.GetChiDinhDichVuAsync(_machidinh);
                if (_chiDinhDichVuResponse == null)
                {
                    Log.Warning($"Không tìm thấy thông tin chỉ định cho MaChiDinh: {_machidinh}");
                    return;
                }

                // Load dữ liệu phụ thuộc song song
                await LoadDependentDataAsync();

                PopulateFormData();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi khởi tạo thông tin chỉ định");
                XtraMessageBox.Show(this, $"Lỗi khi tải thông tin chỉ định: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadDependentDataAsync()
        {
            var bn = _chiDinhDichVuResponse.BenhNhan;

            // Load danh sách gợi ý kết luận: API mới (risv1) có fallback API cũ
            await LoadSuggestionsSafeAsync(bn?.GioiTinh);

            // Load song song các dữ liệu không phụ thuộc
            await Task.WhenAll(
                InitDanhSachThietbiAsync(),
                InitLayoutMauAsync(_chiDinhDichVuResponse.Modality)
            );
        }

        public void UpdateToolBarState()
        {
            _toolBtnDeleteAll.Enabled = _lstBoxPages.Items.Count > 0;
            _toolBtnRotate.Enabled = _toolBtnDeleteSelected.Enabled = _lstBoxPages.SelectedItems.Count > 0;
            _toolBtnSaveDicom.Enabled = _lstBoxPages.CheckedItems.Count > 0;
            _btnPushToPACS.Enabled = _toolBtnStoreToPacs.Enabled = _lstBoxPages.CheckedItems.Count > 0 && _mySettings._settings.StoreServers.serverList.Length > 0;
            _lbImageSelect.Text = _lstBoxPages.CheckedItems.Count.ToString() + "/" + _lstBoxPages.Items.Count.ToString();
            _toolBtnViewLog.Checked = logWindow.Visible;
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
                {

                    _cbbPrinters.SelectedIndex = 0;
                }
                else
                {
                    _cbbPrinters.Text = ServiceLocator.ShortcutAndFontSetting.PrintSettings.Printer;
                }
            }
        }

        private void InitUserInfo()
        {
            var userInfo = ServiceLocator.SessionService.GetCurrentUser();
            string fullName = string.Join(" ", new[] { userInfo.LastName, userInfo.FirstName }
                .Where(s => !string.IsNullOrWhiteSpace(s)));

            _lbUserName.Text = $"{userInfo.Username}";
            _txNguoiDung.Text = $"{fullName}";
        }

        /// <summary>
        /// Load Quyền của tài khoản
        /// </summary>
        private void InitPermissionControl()
        {
            var userInfo = ServiceLocator.SessionService.GetCurrentUser();
            string fullName = string.Join(" ", new[] { userInfo.LastName, userInfo.FirstName }
                .Where(s => !string.IsNullOrWhiteSpace(s)));

            _lbUserName.Text = $"{userInfo.Username}";
            _txNguoiDung.Text = $"{fullName}";
            //SetControlPermission(_btnSettings, AppPermissions.Admin);
            //SetControlPermission(_btnMWLQuery, AppPermissions.RisWorklistList);
        }

        /// <summary>
        /// Lload Form
        /// </summary>
        private void InitializeForm()
        {
            _frmProgress = new FrmProgress();
            _cameraControl = new CameraControl(_videoInputDevice);
            _mediaPlayerControl = new MediaPlayerControl();

            _pgDicomInfo = new Leadtools.Dicom.Common.Editing.Controls.DicomPropertyGrid();
            DicomEditableObject = new Leadtools.Dicom.Common.Editing.DicomEditableObject();
            _pictureBox = new Leadtools.WinForms.RasterImageViewer();

            //
            //_cameraControl
            //
            panelCamera.Controls.Add(_cameraControl);
            _cameraControl.Location = new Point(0, 0);
            _cameraControl.Dock = DockStyle.None; // để tự resize theo tỉ lệ vuông
            _cameraControl.Anchor = AnchorStyles.None;
            panelCamera.Resize += PanelCamera_Resize;
            ResizeCameraViewport();
            InitCameraColumnResize();
            //
            //_mediaPlayerControl
            //
            _panelControlMedia.Controls.Add(_mediaPlayerControl);
            _mediaPlayerControl.Location = new Point(0, 0);
            _mediaPlayerControl.Dock = DockStyle.Fill;
            /*TEMP*/
            //this._tbTableLayout.Controls.Add(this._pictureBox, 0, 3);
            // 
            // _pictureBox
            // 
            _pictureBox.BackColor = System.Drawing.SystemColors.ButtonFace;
            _pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _pictureBox.EnableScrollingInterface = true;
            _pictureBox.Location = new System.Drawing.Point(3, 43);
            _pictureBox.Name = "_pictureBox";
            _pictureBox.Size = new System.Drawing.Size(394, 394);
            _pictureBox.TabIndex = 5;
            // 
            // _pgDicomInfo
            // 
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            _pgDicomInfo.ContextMenuStrip = _cnmnuClearDicom;
            _pgDicomInfo.DataSet = null;
            _pgDicomInfo.DefaultTag = ((long)(-1));
            _pgDicomInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            _pgDicomInfo.Location = new System.Drawing.Point(3, 33);
            _pgDicomInfo.Name = "_pgDicomInfo";
            _pgDicomInfo.SelectedObject = DicomEditableObject;
            _pgDicomInfo.ShowCommands = true;
            _pgDicomInfo.ShowTagInfo = true;
            _pgDicomInfo.ShowUsageImages = true;
            _pgDicomInfo.TabIndex = 0;
            _pgDicomInfo.ToolbarVisible = false;
            _pgDicomInfo.BeforeAddElement += new EventHandler<BeforeAddElementEventArgs>(_pgDicomInfo_BeforeAddElement);
            _tbPropertyGrid.Controls.Add(_pgDicomInfo, 0, 1);
            _panelImage.Controls.Add(_lstBoxPages);
            _lstBoxPages.ViewMode = ThumbMode.Expanded;
            _lstBoxPages.ContextMenuStrip = _cmListBox;
            _lstBoxPages.ListStateChanged += new EventHandler(_lstBoxPages_ListStateChanged);
            _panelPictureReview.Controls.Add(_pictureBox);
            _pictureBox.MouseWheel += new MouseEventHandler(_pictureBox_MouseWheel);
            _pictureBox.BorderPadding.Bottom = 10;
            _pictureBox.BorderPadding.Top = 10;
            _pictureBox.BorderPadding.Left = 10;
            _pictureBox.BorderPadding.Right = 10;
            _pictureBox.HorizontalAlignMode = RasterPaintAlignMode.Center;
            _pictureBox.VerticalAlignMode = RasterPaintAlignMode.Center;
            _pictureBox.BackColor = Color.Black;
            _pictureBox.EnableScrollingInterface = true;
            _pictureBox.KeyDown += new KeyEventHandler(_pictureBox_KeyDown);
            _lstBoxPages.ItemDeSlect += new EventHandler(_lstBoxPages_ItemDeSlect);
            _pictureBox.InteractiveMode = Leadtools.WinForms.RasterViewerInteractiveMode.ZoomTo;
            _pictureBox.MouseMove += new MouseEventHandler(_pictureBox_MouseMove);
            //_pgSearchSCP.SelectedObject = _findQuery;
            //_tbPicture.Controls.Add(_pictureBox, 0, 2);
            //_tbPicture.SetColumnSpan(_pictureBox, 4);
            _pgDicomInfo.ShowTagInfo = false;
            _pgDicomInfo.ShowCommands = false;
            _pgDicomInfo.CommandsVisibleIfAvailable = false;
            _pgDicomInfo.HelpVisible = false;
            _pictureBox.DoubleClick += new EventHandler(_pictureBox_DoubleClick);

            RasterPaintProperties prop = _pictureBox.PaintProperties;
            if (!_mySettings._settings.UseResample)
                prop.PaintDisplayMode = RasterPaintDisplayModeFlags.None;
            else
                prop.PaintDisplayMode = RasterPaintDisplayModeFlags.Resample;
            _pictureBox.PaintProperties = prop;
            _codec = new RasterCodecs();
            _cmbSopClasses.SelectedIndex = ClassTypes.IndexOf(_mySettings._settings.selectedtype);

            logWindow = new LogWindow(this);
            logWindow.Visible = false;

            //_pageMWLQuery.Controls.Add(_tbQueryMWList);
            //_pgSearchMWL.SelectedObject = _bbQuery;
        }

        private void PanelCamera_Resize(object sender, EventArgs e)
        {
            ResizeCameraViewport();
        }

        /// <summary>
        /// Fit khung camera theo đúng tỷ lệ nguồn (mặc định 640x480 = 4:3),
        /// ưu tiên "tối đa chiều cao" trong panel; nếu vượt chiều rộng thì fallback theo chiều rộng.
        /// Tránh kéo giãn hình khi màn hình gần vuông / thay đổi kích thước.
        /// </summary>
        private void ResizeCameraViewport()
        {
            try
            {
                if (_cameraControl == null || panelCamera == null || panelCamera.IsDisposed)
                    return;

                // _xtraCamera: luôn fill _panelCamera, nhưng không vượt quá chiều cao _panelCamera
                if (_panelCamera != null && !_panelCamera.IsDisposed && _xtraCamera != null && !_xtraCamera.IsDisposed)
                {
                    _xtraCamera.Dock = DockStyle.Fill;
                    var host = _panelCamera.ClientSize;
                    if (host.Width > 0 && host.Height > 0)
                    {
                        _xtraCamera.MaximumSize = new Size(int.MaxValue, host.Height);
                        if (_xtraCamera.Height > host.Height)
                            _xtraCamera.Height = host.Height;
                    }
                    else
                    {
                        _xtraCamera.MaximumSize = Size.Empty;
                    }
                }

                var client = panelCamera.ClientSize;
                if (client.Width <= 0 || client.Height <= 0)
                    return;

                _cameraControl.SuspendLayout();

                // Lấy tỷ lệ từ cấu hình camera: PanSourceWidth / PanSourceHeight
                int srcW = ServiceLocator.CameraSettingConfig?.PanSourceWidth > 0
                    ? ServiceLocator.CameraSettingConfig.PanSourceWidth
                    : 640;
                int srcH = ServiceLocator.CameraSettingConfig?.PanSourceHeight > 0
                    ? ServiceLocator.CameraSettingConfig.PanSourceHeight
                    : 480;

                double aspect = (double)srcW / srcH; // width / height

                // Ưu tiên tối đa chiều cao
                int targetH = client.Height;
                int targetW = (int)Math.Round(targetH * aspect);

                // Nếu vượt quá chiều rộng panel -> fit theo chiều rộng
                if (targetW > client.Width)
                {
                    targetW = client.Width;
                    targetH = (int)Math.Round(targetW / aspect);
                }

                if (targetW <= 0 || targetH <= 0)
                    return;

                _cameraControl.Size = new Size(targetW, targetH);
                _cameraControl.Location = new Point(
                    (client.Width - targetW) / 2,
                    (client.Height - targetH) / 2
                );
            }
            finally
            {
                _cameraControl?.ResumeLayout();
            }
        }

        #region Kéo thả bề rộng cột camera + lưu lại cho lần mở sau

        private Panel _cameraColGrip;
        private bool _cameraColDragging;
        private int _cameraColDragStartX;
        private float _cameraColDragStartWidth;

        /// <summary>Bề rộng cột camera do người dùng tự kéo chỉnh (px hiển thị thực).</summary>
        private float _userCameraColWidth = -1f;

        /// <summary>Bề rộng tối thiểu cột camera khi kéo (còn đủ chỗ cho hàng nút bên dưới)</summary>
        private const float CameraColMinWidth = 480f;
        /// <summary>Vùng tab bên trái (Kết luận/DICOM...) giữ tối thiểu chừng này khi kéo</summary>
        private const float LeftAreaMinWidth = 520f;

        private static string UiLayoutSettingsPath => Path.Combine(
            ServiceLocator.GetAppDataBasePath(), "UiLayoutSettings.xml");

        /// <summary>Cột camera (cột 1 - Absolute) trong _tbTableLayout, null nếu layout thay đổi</summary>
        private ColumnStyle CameraColumnStyle
        {
            get
            {
                if (_tbTableLayout == null || _tbTableLayout.ColumnStyles.Count < 2)
                    return null;
                var style = _tbTableLayout.ColumnStyles[1];
                return style.SizeType == SizeType.Absolute ? style : null;
            }
        }

        /// <summary>
        /// Tạo thanh kéo (grip) ở mép trái panel camera để chỉnh bề rộng cột camera,
        /// và nạp lại bề rộng đã lưu của máy này khi form hiển thị.
        /// </summary>
        private void InitCameraColumnResize()
        {
            if (_cameraColGrip != null || _panelCamera == null)
                return;

            _cameraColGrip = new Panel
            {
                Dock = DockStyle.Left,
                Width = 6,
                Cursor = Cursors.SizeWE,
                BackColor = Color.FromArgb(220, 226, 236),
            };
            _cameraColGrip.MouseDown += CameraColGrip_MouseDown;
            _cameraColGrip.MouseMove += CameraColGrip_MouseMove;
            _cameraColGrip.MouseUp += CameraColGrip_MouseUp;

            _panelCamera.Controls.Add(_cameraColGrip);
            // Đưa grip về cuối z-order để nó dock trước, chiếm dải mép trái;
            // _xtraCamera (Dock=Fill) sẽ lấp phần còn lại
            _cameraColGrip.SendToBack();

            // Áp bề rộng đã lưu sau khi form đã có kích thước thật (tránh clamp sai lúc chưa layout)
            this.Shown += (s, e) => ApplySavedCameraColumnWidth();
        }

        /// <summary>Giới hạn bề rộng cột camera trong khoảng hợp lệ theo kích thước hiện tại</summary>
        private float ClampCameraColumnWidth(float width)
        {
            float max = Math.Max(CameraColMinWidth, _tbTableLayout.ClientSize.Width - LeftAreaMinWidth);
            return Math.Max(CameraColMinWidth, Math.Min(width, max));
        }

        private void ApplySavedCameraColumnWidth()
        {
            try
            {
                var style = CameraColumnStyle;
                if (style == null)
                    return;

                var saved = XmlSettingsHelper.Load<UiLayoutSettings>(UiLayoutSettingsPath);
                if (saved == null || saved.CameraColumnWidth <= 0)
                    return;

                // Nạp đúng bề rộng hiển thị người dùng đã chọn, không cộng/trừ theo sidebar
                float width = ClampCameraColumnWidth(saved.CameraColumnWidth);
                _userCameraColWidth = width;
                style.Width = width;

                Log.Information("Đã nạp bề rộng cột camera đã lưu: {Width}px", width);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Lỗi khi nạp bề rộng cột camera đã lưu");
            }
        }

        private void CameraColGrip_MouseDown(object sender, MouseEventArgs e)
        {
            var style = CameraColumnStyle;
            if (style == null || e.Button != MouseButtons.Left)
                return;

            _cameraColDragging = true;
            _cameraColDragStartX = Cursor.Position.X;
            _cameraColDragStartWidth = style.Width;
        }

        private void CameraColGrip_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_cameraColDragging)
                return;
            var style = CameraColumnStyle;
            if (style == null)
                return;

            // Kéo sang trái -> cột camera rộng ra, sang phải -> hẹp lại
            float newWidth = ClampCameraColumnWidth(
                _cameraColDragStartWidth + (_cameraColDragStartX - Cursor.Position.X));
            if (Math.Abs(newWidth - style.Width) >= 1f)
            {
                style.Width = newWidth;
                // panelCamera.Resize sẽ tự gọi ResizeCameraViewport để giữ đúng tỷ lệ khung hình
            }
        }

        private void CameraColGrip_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_cameraColDragging)
                return;
            _cameraColDragging = false;

            var style = CameraColumnStyle;
            if (style == null)
                return;

            _userCameraColWidth = style.Width;
            SaveCameraColumnWidth(style.Width);
        }

        private void SaveCameraColumnWidth(float width)
        {
            SaveUiLayout(settings => settings.CameraColumnWidth = width);
            Log.Information("Đã lưu bề rộng cột camera: {Width}px", width);
        }

        /// <summary>
        /// Cập nhật một phần UiLayoutSettings theo kiểu load-sửa-lưu
        /// để các giá trị layout khác trong file không bị ghi đè mất.
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

        #endregion


        private void SetServersComboBox(bool bSelectDefault)
        {
            toolStripComboBoxStoreServer.Items.Clear();
            MyServer[] list;
            int defaultserver = 0;

            list = _mySettings._settings.StoreServers.serverList;
            defaultserver = _mySettings._settings.DefaultStoreServer;

            if (list.Length == 0)
            {
                //_toolBtnStoreToPacs.Enabled = _miStoreToPACS.Enabled = _grpStoreServers.Enabled = false;
            }
            else
            {
                //_miStoreToPACS.Enabled = _grpStoreServers.Enabled = true;
                UpdateToolBarState();
                foreach (MyServer server in list)
                {
                    toolStripComboBoxStoreServer.Items.Add(server);
                }
                if (bSelectDefault)
                    if (defaultserver < list.Length)
                        toolStripComboBoxStoreServer.SelectedIndex = defaultserver;
                    else
                        toolStripComboBoxStoreServer.SelectedIndex = 0;
            }
        }

        // ============================================
        // NHÓM 3: Background Loading - Chạy sau
        // ============================================
        private async Task LoadBackgroundDataAsync()
        {
            try
            {
                // Phải lấy kết quả chẩn đoán trước: nếu đã kết luận thành công thì staffCode
                // để tra KTV/y tá cùng khoa sẽ lấy theo bác sĩ đã kết luận (MaBacSiKetLuan)
                // trong kết quả, không phải bác sĩ đang đăng nhập.
                await InitCheckKetQuaChanDoanAsync();

                var loadImageTask = Task.Run(() => LoadImageData());
                var loadKTVTask = InitDanhSachKTVAsync();
                // Tra y lệnh bên RIS mới (best-effort, tự nuốt lỗi) để sync/khôi phục bảng chỉ số
                var resolveRisV1Task = ResolveRisV1OrderItemAsync();
                // Lịch sử khám bệnh nhân cho sidebar (best-effort, không ảnh hưởng luồng chính)
                var loadHistoryTask = LoadPatientHistorySafeAsync();

                // Chờ tất cả hoàn thành trước khi đụng vào UI
                await Task.WhenAll(loadKTVTask, loadImageTask, resolveRisV1Task, loadHistoryTask);

                // Các thao tác cần UI thread
                this.Invoke((MethodInvoker)delegate
                {
                    // _listHisUser (loadKTVTask) và _kqChanDoanResponse (đã await trước đó) đã sẵn sàng ở đây;
                    // _listThietBi đã có từ NHÓM 2 (chạy trước NHÓM 3)
                    ApplyThietBiVaKTVSelectionFromResult();

                    if (result != null)
                    {
                        InitTranfer(result);
                    }
                    else
                    {
                        InitTranferRIS();
                    }
                    LoadImageVideoCaptured();
                    // Khôi phục form chỉ số từ snapshot đã lưu bên RIS mới (nếu có) -
                    // fire-and-forget, mọi lỗi được nuốt + log bên trong
                    _ = RestoreParamFormFromRisV1Async();
                    CreateCStoreObject(new MyServer());
                    _captureType = CaptureType.None;
                    //CheckFirstRun();
                    _mySettings._settings.FirstRun = false;
                    _mySettings.Save();
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi load background data");
                // Không throw để không làm crash app
            }
        }


        /// <summary>
        /// Khởi tạo dicom tag với dữ liệu worklist
        /// </summary>
        private void InitTranfer(ModalityWorklistResult result)
        {
            ResetModule(DicomModuleType.GeneralSeries, _pgDicomInfo.DataSet, true);
            GenerateDefaultElements();
            InsertNewSeries();

            DicomDataSet ds = _pgDicomInfo.DataSet;

            //Study
            DicomElement dElement;
            if (result.AccessionNumber != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.AccessionNumber, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.AccessionNumber, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, result.AccessionNumber);
            }

            if (result.ReferringPysician != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.ReferringPhysicianName, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.ReferringPhysicianName, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, result.ReferringPysician.FullDicomEncoded);
            }

            //Patient
            if (result.PatientName != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.PatientName, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.PatientName, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, result.PatientName.FullDicomEncoded);
            }

            if (result.PatientId != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.PatientID, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.PatientID, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, result.PatientId);
            }

            if (result.PatientSex != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.PatientSex, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.PatientSex, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, result.PatientSex);
            }

            if (result.PatientBirthDate != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.PatientBirthDate, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.PatientBirthDate, DicomVRType.UN, false, 0);
                ds.SetDateValue(dElement, new DateTime[] { (DateTime)result.PatientBirthDate });
            }

            if (result.RequestedProcedureId != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.StudyID, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.StudyID, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, result.RequestedProcedureId);
            }

            if (result.StudyInstanceUid != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.StudyInstanceUID, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.StudyInstanceUID, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, result.StudyInstanceUid);
            }
            _pgDicomInfo.DataSet = ds;
        }

        /// <summary>
        /// Khởi tạo dicom tag với dữ liệu RIS
        /// </summary>
        private void InitTranferRIS()
        {
            ResetModule(DicomModuleType.GeneralSeries, _pgDicomInfo.DataSet, true);
            GenerateDefaultElements();
            //InsertNewSeries();

            DicomDataSet ds = _pgDicomInfo.DataSet;

            string gender = null;
            switch (_chiDinhDichVuResponse.BenhNhan.GioiTinh)
            {
                case 0:
                    gender = "M";
                    break;
                case 1:
                    gender = "F";
                    break;
                case 2:
                    gender = "O";
                    break;
            }

            DicomElement dElement;
            if (_chiDinhDichVuResponse.MaChiDinh != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.AccessionNumber, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.AccessionNumber, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, _chiDinhDichVuResponse.MaChiDinh);
            }

            if (_chiDinhDichVuResponse.TenBacSiChiDinh != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.ReferringPhysicianName, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.ReferringPhysicianName, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, RemoveVietnameseSigns(_chiDinhDichVuResponse.TenBacSiChiDinh));
            }

            //Patient
            if (_chiDinhDichVuResponse.BenhNhan.HoTen != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.PatientName, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.PatientName, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, RemoveVietnameseSigns(_chiDinhDichVuResponse.BenhNhan.HoTen));
            }

            if (_chiDinhDichVuResponse.BenhNhan.MaBenhNhan != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.PatientID, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.PatientID, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, _chiDinhDichVuResponse.BenhNhan.MaBenhNhan);
            }

            if (_chiDinhDichVuResponse.BenhNhan.GioiTinh != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.PatientSex, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.PatientSex, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, gender);
            }

            if (_chiDinhDichVuResponse.BenhNhan.NgaySinh != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.PatientBirthDate, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.PatientBirthDate, DicomVRType.UN, false, 0);
                ds.SetDateValue(dElement, new DateTime[] { _chiDinhDichVuResponse.BenhNhan.NgaySinh });
            }

            if (_chiDinhDichVuResponse.TenDichVu != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.StudyDescription, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.StudyDescription, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, RemoveVietnameseSigns(_chiDinhDichVuResponse.TenDichVu));
            }

            if (ServiceLocator.KeycloakUserInfo.FirstName != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.ReferringPhysicianName, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.ReferringPhysicianName, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, RemoveVietnameseSigns($"{ServiceLocator.KeycloakUserInfo.FirstName} {ServiceLocator.KeycloakUserInfo.LastName}_{ServiceLocator.KeycloakUserInfo.HISCode}"));
            }

            if (_chiDinhDichVuResponse.Modality != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.Modality, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.Modality, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, _chiDinhDichVuResponse.Modality == "ECG" ? "OT" : _chiDinhDichVuResponse.Modality);
            }

            _pgDicomInfo.DataSet = ds;
        }

        /// <summary>
        /// Xóa ký tự unikey
        /// </summary>
        public static string RemoveVietnameseSigns(string str)
        {
            string[] VietnameseSigns = new string[]
            {
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ"
            };

            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                {
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
                }
            }
            return str;
        }

        private void LoadImageVideoCaptured()
        {
            listImageKeyLocal = XmlSettingsHelper.Load<List<string>>(Path.Combine($"{_baseFolder}\\BenhNhan\\{_machidinh}", FileNameXMLImage));
            string folderImageVideoCapture = $"{_baseFolder}\\BenhNhan\\{_machidinh}";
            if (Directory.Exists(folderImageVideoCapture))
            {
                string[] extensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tif", ".tiff" };

                var imageFiles = Directory
                    .EnumerateFiles(folderImageVideoCapture, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(file => extensions.Contains(Path.GetExtension(file).ToLower()))
                    .ToList();

                foreach (var file in imageFiles)
                {
                    LoadRasterImage(file);
                }
            }
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
                    Log.Warning($"Không có mẫu báo cáo cho modality: {modality}");
                    return;
                }

                // Cập nhật items
                _cbbLayout.Properties.Items.Clear();
                foreach (var item in _listMauBaoCao)
                {
                    _cbbLayout.Properties.Items.Add(item);
                }

                // Chọn layout từ cache hoặc mặc định
                SelectLayoutFromCache(modality);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Lỗi khi load layout mẫu cho modality: {modality}");
            }
        }

        private void SelectLayoutFromCache(string modality)
        {
            // Lấy layout từ cache
            string lastSelectedId = null;
            if (ServiceLocator.ReportCache.TryGetValue(modality, out var cachedId))
            {
                lastSelectedId = cachedId;
            }

            if (!string.IsNullOrEmpty(lastSelectedId))
            {
                var matchedItem = _listMauBaoCao.FirstOrDefault(x => x.Id == lastSelectedId);
                if (matchedItem != null)
                {
                    _cbbLayout.SelectedItem = matchedItem;
                    return;
                }
            }

            // Fallback: chọn item đầu tiên
            if (_cbbLayout.Properties.Items.Count > 0)
            {
                _cbbLayout.SelectedIndex = 0;

                // Lưu vào cache
                var layoutSelect = _cbbLayout.SelectedItem as ReportTemplateGridViewModel;
                if (layoutSelect != null)
                {
                    ServiceLocator.ReportCache[modality] = layoutSelect.Id;
                }
            }
        }

        private void PopulateFormData()
        {
            var benhNhan = _chiDinhDichVuResponse.BenhNhan;
            if (benhNhan != null)
            {
                PopulatePatientInfo(benhNhan);
            }

            PopulateChiDinhInfo();

            // Load thông tin ký số nếu có
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
            _txDichVu.ToolTipTitle = "Dịch vụ";
            _txDichVu.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            _txDichVu.ToolTip = _chiDinhDichVuResponse.TenDichVu ?? "";
            _txDichVu.Text = _chiDinhDichVuResponse.TenDichVu ?? "";

            _txChanDoan.ToolTipTitle = "Chẩn đoán";
            _txChanDoan.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            _txChanDoan.ToolTip = _chiDinhDichVuResponse.ChanDoanSoBo ?? "";
            _txChanDoan.Text = _chiDinhDichVuResponse.ChanDoanSoBo ?? "";

            // Set tên bác sĩ đọc
            if (ServiceLocator.KeycloakUserInfo != null)
            {
                _txBSDoc.Text = $"{ServiceLocator.KeycloakUserInfo.FirstName} {ServiceLocator.KeycloakUserInfo.LastName}";
            }
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
                Log.Warning(ex, $"Không thể load thông tin ký số cho : {_chiDinhDichVuResponse.MaBacSiChiDinh}");
            }
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


        private async Task InitDanhSachThietbiAsync()
        {
            try
            {
                // API RIS v1 (cũ) - giữ lại để tham khảo/rollback nếu cần
                //var response = await ServiceLocator.RisService.GetDSThietBiAsync(loaiThietBi: "Máy chụp");
                //_listThietBi = response?.data;

                // API RIS v2 (mới)
                _listThietBi = await ServiceLocator.RisService2.GetDevicesAsync(modality: _chiDinhDichVuResponse?.Modality);

                if (_listThietBi == null || _listThietBi.Count == 0)
                {
                    Log.Warning("Danh sách thiết bị rỗng");
                    return;
                }

                // Cấu hình LookUpEdit
                ConfigureThietBiLookup();

                // Khởi tạo combo mẫu gợi ý
                InitComboxMauGoiY();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi tải danh sách thiết bị");
                XtraMessageBox.Show(this, $"Lỗi khi tải danh sách thiết bị: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureThietBiLookup()
        {
            _cbbDSThietBi.Properties.DataSource = _listThietBi;
            _cbbDSThietBi.Properties.DisplayMember = "name";
            _cbbDSThietBi.Properties.ValueMember = "id";
            _cbbDSThietBi.Properties.NullText = "Chọn thiết bị...";

            // Bật tìm kiếm thông minh
            _cbbDSThietBi.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
            _cbbDSThietBi.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            _cbbDSThietBi.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;

            // Cấu hình cột: chỉ hiển thị mã và tên thiết bị, ẩn các cột còn lại của DeviceDto
            _cbbDSThietBi.Properties.PopulateColumns();
            foreach (DevExpress.XtraEditors.Controls.LookUpColumnInfo col in _cbbDSThietBi.Properties.Columns)
            {
                col.Visible = col.FieldName == "code" || col.FieldName == "name";
            }
            _cbbDSThietBi.Properties.Columns["code"].Caption = "Mã thiết bị";
            _cbbDSThietBi.Properties.Columns["name"].Caption = "Tên thiết bị";
        }


        // ============================================
        // HELPER METHODS - Chuyển đổi async void thành async Task
        // ============================================

        /// <summary>
        /// Chuyển đổi InitDanhSachKTV từ async void thành async Task
        /// </summary>
        private async Task InitDanhSachKTVAsync()
        {
            try
            {
                // Lọc theo khoa đã chọn lúc đăng nhập (không lọc theo bác sĩ/staffCode nữa).
                var orgCode = ServiceLocator.SelectedOrganizationCode;

                if (string.IsNullOrWhiteSpace(orgCode))
                {
                    Log.Warning("Không có mã khoa để tra danh sách KTV/Y tá cùng khoa");
                    return;
                }

                // API RIS v1 (cũ) - giữ lại để tham khảo/rollback nếu cần
                //_listHisUser = (await ServiceLocator.RisService.GetDSNguoidungAsync()).data;

                // API RIS v2 (mới) - lấy danh sách KTV (TECHNICIAN), y tá (NURSE) cùng khoa (orgCode)
                _listHisUser = await ServiceLocator.RisService2.GetColleaguesAsync(
                    orgCode,
                    titleCodes: new List<string> { "NURSE", "TECHNICIAN" });

                // Cập nhật UI trên UI thread
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        ConfigureKTVLookup();
                    });
                }
                else
                {
                    ConfigureKTVLookup();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi tải danh sách KTV");
                // Có thể hiển thị message box nếu cần
            }
        }

        /// <summary>
        /// Tách phần cấu hình UI ra method riêng
        /// </summary>
        private void ConfigureKTVLookup()
        {
            // Gán dữ liệu vào LookUpEdit
            _cbbHisUser.Properties.DataSource = _listHisUser;
            _cbbHisUser.Properties.DisplayMember = "fullName";
            _cbbHisUser.Properties.ValueMember = "id";
            _cbbHisUser.Properties.NullText = "Chọn KTV...";

            // Bật tìm kiếm & lọc
            _cbbHisUser.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
            _cbbHisUser.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            _cbbHisUser.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;

            // Hiển thị cột: chỉ hiển thị mã và tên, ẩn các cột còn lại của PractitionerListDto
            _cbbHisUser.Properties.PopulateColumns();
            foreach (DevExpress.XtraEditors.Controls.LookUpColumnInfo col in _cbbHisUser.Properties.Columns)
            {
                col.Visible = col.FieldName == "staffCode" || col.FieldName == "fullName";
            }

            _cbbHisUser.Properties.Columns["fullName"].Caption = "Tên KTV";
            _cbbHisUser.Properties.Columns["staffCode"].Caption = "Mã KTV";
        }

        /// <summary>
        /// Chọn KTV/y tá và thiết bị trên combobox theo kết quả đã lưu (kể cả bản Nháp,
        /// vì lúc lưu nháp đã có sẵn thông tin thiết bị/KTV rồi). Chưa có kết quả nào thì để trống.
        /// Phải gọi sau khi cả _listHisUser, _listThietBi và _kqChanDoanResponse đã load xong.
        /// </summary>
        private void ApplyThietBiVaKTVSelectionFromResult()
        {
            // KTV / Y tá
            if (_listHisUser != null && _listHisUser.Count > 0)
            {
                var selectedUser = _kqChanDoanResponse != null
                    ? _listHisUser.FirstOrDefault(u => u.staffCode == _kqChanDoanResponse.MaKyThuatVien)
                    : null;

                _cbbHisUser.EditValue = selectedUser?.id;
            }

            // Thiết bị
            if (_listThietBi != null && _listThietBi.Count > 0)
            {
                var selectedThietBi = _kqChanDoanResponse != null
                    ? _listThietBi.FirstOrDefault(x => x.code == _kqChanDoanResponse.MaThietBi)
                    : null;

                _cbbDSThietBi.EditValue = selectedThietBi?.id ?? 0;
            }
        }



        /// <summary>
        /// Chuyển đổi InitCheckKetQuaChanDoan từ async void thành async Task
        /// </summary>
        private async Task InitCheckKetQuaChanDoanAsync()
        {
            try
            {
                _kqChanDoanResponse = await ServiceLocator.RisService.GetKetQuaChanDoanAsync(_machidinh);

                // Cập nhật UI trên UI thread
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        UpdateUIFromKetQuaChanDoan();
                    });
                }
                else
                {
                    UpdateUIFromKetQuaChanDoan();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi load kết quả chẩn đoán");
                // Khởi tạo thời gian mặc định
                _dateTGThucHien.DateTime = DateTime.Now;
                _dateTGKetThuc.DateTime = DateTime.Now.AddMinutes(5);
            }
        }

        /// <summary>
        /// Tách phần cập nhật UI ra method riêng
        /// </summary>
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

                bool isNhap = _kqChanDoanResponse.TrangThai != null && _kqChanDoanResponse.TrangThai.Equals(TrangThaiKetLuan.NHAP);
                _btnSave.Enabled = isNhap;
                _btnPrint.Enabled = true;
                _rtMoTa.Enabled = isNhap;
                _rtKetLuan.Enabled = isNhap;
                _rtKhuyenNghi.Enabled = isNhap;

                if (isNhap)
                {
                    _btnSignature.Text = $"Ký số ({ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.Sign})";
                }
                else
                {
                    _btnSignature.Text = $"Hủy ký số({ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.Sign})";
                }
            }
            else
            {
                // Khởi tạo thời gian mặc định
                _dateTGThucHien.DateTime = DateTime.Now;
                _dateTGKetThuc.DateTime = DateTime.Now.AddMinutes(5);
                _btnPreviewMain.Enabled = false;
                _btnPrint.Enabled = false;
                _btnSignature.Enabled = false;
            }
        }

        /// <summary>
        /// Load image data từ file system (chạy trên background thread)
        /// </summary>
        private void LoadImageData()
        {
            try
            {
                XmlSettingsHelper.EnsureFileExists(
                    Path.Combine($"{_baseFolder}\\BenhNhan\\{_machidinh}", FileNameXMLImage),
                    () => new List<string>());

                listImageKeyLocal = XmlSettingsHelper.Load<List<string>>(
                    Path.Combine($"{_baseFolder}\\BenhNhan\\{_machidinh}", FileNameXMLImage));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi load image data");
            }
        }


        #endregion
    }
}
