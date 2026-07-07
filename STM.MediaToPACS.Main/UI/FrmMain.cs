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
using VisioForge.Core.VideoEdit;
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
    public partial class FrmMain : DevExpress.XtraEditors.XtraForm
    {
        #region Main...
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        const string sHelpInstructions =
        "Command Line Options:" + _sNewlineTab +
        "/? or /help\t\tDisplays this help" + _sNewlineTab +
        "/configure\t\tConfigures the client (use one or more options below)" + _sNewlineTab +
        "/server_aetitle={aetitle}\tServer AE title" + _sNewlineTab +
        "/server_ip={ip address}\tServer IP" + _sNewlineTab +
        "/server_port={port}\tServer Port" + _sNewlineTab +
        "/client_aetitle={aetitle}\tClient AE title" + _sNewlineTab +
        "/client_port={port}\t\tClient Port" + _sNewlineTab +
        "/defaults\t\t\tSets defaults for other options";

        static MyServer ParseOneServer(string serverString)
        {
            //   /servers=ae1,ip1,port1,timeout1,secure1

            MyServer server = null;
            string[] fields = serverString.Split(',');
            if (fields.Length == 5)
            {
                server = new MyServer();
                server._sAE = fields[0].Trim();
                server._sIP = fields[1].Trim();
                server._port = Convert.ToInt32(fields[2].Trim());
                server._timeout = Convert.ToInt32(fields[3].Trim());
                server._useTls = Convert.ToBoolean(fields[4].Trim());
            }
            return server;
        }

        static MyServer[] ParseServerList(string serversString)
        {
            //   /servers=ae1,ip1,port1,timeout1,secure1;ae1,ip1,port1,timeout1,secure1
            serversString.Trim();
            if (serversString.EndsWith(";"))
                serversString = serversString.Substring(0, serversString.Length - 1);
            string[] servers = serversString.Split(';');

            ArrayList list = new ArrayList();
            foreach (string s in servers)
            {
                MyServer server = ParseOneServer(s);
                list.Add(server);
            }

            MyServer[] items = new MyServer[servers.Length];
            list.CopyTo(items);
            return items;
        }

        static string GetDefaultIp()
        {
            ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'TRUE'");
            ManagementObjectCollection queryCollection = query.Get();

            foreach (ManagementObject mo in queryCollection)
            {
                if (queryCollection.Count > 0)
                {
                    string[] addresses = (string[])mo["IPAddress"];

                    foreach (string ip in addresses)
                    {
                        if (!ip.Contains(":") && (ip != "0.0.0.0"))
                            return ip;
                    }
                }
            }
            return string.Empty;
        }
        #endregion

        #region Constructor...
        private ModalityWorklistResult result;
        private List<string> _listPathVideoRecords = new List<string>();
        private readonly string _baseFolder;
        private KetQuaChanDoanResponse _kqChanDoanResponse;

        public FrmMain(ModalityWorklistResult result, string VideoInputDevice)
        {
            try
            {
                InitClass();
                InitListBoxImage();
                InitializeComponent();
                _baseFolder = ServiceLocator.GetAppDataBasePath();
                if (!Directory.Exists(_baseFolder))
                {
                    Directory.CreateDirectory(_baseFolder);
                }
                this.result = result;
                this._videoInputDevice = VideoInputDevice;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private string sophieu;
        private string _machidinh;
        public FrmMain(string VideoInputDevice, string soPhieu, string maChiDinh)
        {
            try
            {
                InitClass();
                InitListBoxImage();
                InitializeComponent();
                _baseFolder = ServiceLocator.GetAppDataBasePath();
                if (!Directory.Exists(_baseFolder))
                {
                    Directory.CreateDirectory(_baseFolder);
                }

                this._videoInputDevice = VideoInputDevice;
                this.sophieu = soPhieu;
                this._machidinh = maChiDinh;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void InitListBoxImage()
        {
            this._lstBoxPages = new STM.MediaToPACS.Main.UI.ListImageBox();
            // 
            // _lstBoxPages
            // 
            this._lstBoxPages.AutoScroll = true;
            this._lstBoxPages.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this._lstBoxPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lstBoxPages.ExpansionButtonLocation = System.Windows.Forms.AnchorStyles.Left;
            this._lstBoxPages.ItemHeight = 120;
            this._lstBoxPages.Location = new System.Drawing.Point(0, 0);
            this._lstBoxPages.Name = "_lstBoxPages";
            this._lstBoxPages.SelectedGroupIndex = -1;
            this._lstBoxPages.SelectedIndex = -1;
            this._lstBoxPages.SelectedItem = null;
            this._lstBoxPages.SelectedItemGroupIndex = -1;
            this._lstBoxPages.Size = new System.Drawing.Size(150, 678);
            this._lstBoxPages.TabIndex = 0;
            this._lstBoxPages.ViewMode = STM.MediaToPACS.Main.UI.ThumbMode.Expanded;
            this._lstBoxPages.ItemAdded += new System.EventHandler(this._lstBoxPages_ItemAdded);
            this._lstBoxPages.SelectedIndexChanged += new System.EventHandler(this._lstBoxPages_SelectedIndexChanged);
            this._lstBoxPages.KeyDown += new System.Windows.Forms.KeyEventHandler(this._lstBoxPages_KeyDown);
        }
        #endregion

        #region Fields...

        public delegate void AddLog(string action, string logText);
        public delegate void AddLogColor(string action, string logText, Color sActionColor);
        public delegate void EnableMenu(bool enable, string strCaption, string strBtnCaption);

        private const string _sNewline = "\r\n";
        private const string _sNewlineTab = "\r\n\t";
        private const string _sNewlineTabTab = "\r\n\t\t";
        private FrmProgress _frmProgress;
        private FrmOperation _frmOperation;
        private CameraControl _cameraControl;
        private MediaPlayerControl _mediaPlayerControl;
        private string _videoInputDevice;
        private ListImageBox.ImageCollection imgCollection = null;
        private int _pageNo = 0;
        private int _jobId = 0;
        public static string StartedPrinter = string.Empty;
        bool bFinishedPrinting = false;
        int iOldY = -1, iOldX = -1, _iOldIndex = -1;
        private RasterCodecs _codec;

        private bool bCancelOperation = false;


        public const string _sConfigurationImplementationClass = "1.2.840.114257.1123456";
        public const string _sConfigurationImplementationVersionName = "1";
        public const string _sConfigurationProtocolversion = "1";

        public static FlowLayoutPanel FPLRoll;

        private TextBoxTraceListener _tracer = null;
        private StoreScu _cstore;
        private bool bStored = false;
        public MySettings _mySettings = PacsSettings.Instance;
        List<DataGridViewRow> OldRowSelection = new List<DataGridViewRow>();
        List<DataGridViewCell> OldCellSelection = new List<DataGridViewCell>();
        LogWindow logWindow = PacsSettings.LogWindow;
        ListView _lstSelected;
        List<long> DICOMPatientInfo = new List<long>()
      {
         DicomTag.PatientName,
         DicomTag.PatientID,
         DicomTag.PatientSex,
         DicomTag.PatientBirthDate

      };

        List<long> DICOMStudyInfo = new List<long>()
      {
         DicomTag.StudyID,
         DicomTag.ReferringPhysicianName,
         DicomTag.AccessionNumber,
         DicomTag.StudyDate,
         DicomTag.StudyTime
      };

        List<DicomClassType> ClassTypes = new List<DicomClassType>(){
         DicomClassType.SCImageStorage,
         DicomClassType.SCMultiFrameGrayscaleByteImageStorage,
         DicomClassType.SCMultiFrameTrueColorImageStorage,
         DicomClassType.EncapsulatedPdfStorage
      };

        private const long ELEMENT_LENGTH_MAX = (long)0xFFFFFFFFUL;
        private List<long> _ExcludedTags = new List<long>();

        string strLastLocation = "";
        #endregion

        #region Forms Events...

        /*TEMP*/
        private Leadtools.Dicom.Common.Editing.Controls.DicomPropertyGrid _pgDicomInfo;
        private Leadtools.Dicom.Common.Editing.DicomEditableObject DicomEditableObject;
        private Leadtools.WinForms.RasterImageViewer _pictureBox = new Leadtools.WinForms.RasterImageViewer();
        private List<GoiYKetLuanResponse> _listGoiYKetLuan { get; set; }
        private List<DeviceDto> _listThietBi { get; set; }
        private List<ReportTemplateGridViewModel> _listMauBaoCao { get; set; }
        private List<PractitionerListDto> _listHisUser { get; set; }
        private ChiDinhDichVuResponse _chiDinhDichVuResponse { get; set; }
        private List<string> listImageKeyLocal { get; set; } = new List<string>();
        private const string FileNameXMLImage = "ImageSelected.xml";
        private HisUserKySoResponse _hisUserKySoResponse { get; set; }

        private RichTextBox GetCurrentBox()
        {
            return ActiveControl as RichTextBox;

        }
        #endregion

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

            // Load danh sách gợi ý kết luận (filter theo giới tính)
            var allGoiY = await ServiceLocator.RisService.GetDanhSachGoiYKetLuanResponseAsync(
                madichvu: _chiDinhDichVuResponse.MaDichVu);

            _listGoiYKetLuan = FilterGoiYKetLuanByGender(allGoiY?.data, bn?.GioiTinh ?? -1);

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

                // Chờ tất cả hoàn thành trước khi đụng vào UI
                await Task.WhenAll(loadKTVTask, loadImageTask);

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



        private void InitComboxMauGoiY()
        {
            _cbbMauGoiY.Properties.Items.Clear();
            foreach (var item in _listGoiYKetLuan)
            {
                var tb = _listThietBi.FirstOrDefault(x => x.code == item.mathietbi);
                if (tb != null)
                {
                    _cbbDSThietBi.EditValue = tb.id;
                }
                _cbbMauGoiY.Properties.Items.Add(item);
            }

            if (_cbbMauGoiY.Properties.Items.Count > 0)
            {
                if (_kqChanDoanResponse == null)
                    _cbbMauGoiY.SelectedIndex = 0;
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
                // Nếu đã kết luận thành công thì tra theo bác sĩ đã kết luận (MaBacSiKetLuan) trong
                // kết quả, không phải bác sĩ đang đăng nhập (có thể là người khác đang xem/sửa lại).
                bool daHoanThanh = _kqChanDoanResponse != null &&
                    TrangThaiKetLuan.HOAN_THANH.Equals(_kqChanDoanResponse.TrangThai);

                var staffCode = daHoanThanh
                    ? _kqChanDoanResponse.MaBacSiKetLuan
                    : ServiceLocator.KeycloakUserInfo?.HISCode;

                if (string.IsNullOrWhiteSpace(staffCode))
                {
                    Log.Warning("Không có mã bác sĩ để tra danh sách KTV/Y tá cùng khoa");
                    return;
                }

                // API RIS v1 (cũ) - giữ lại để tham khảo/rollback nếu cần
                //_listHisUser = (await ServiceLocator.RisService.GetDSNguoidungAsync()).data;

                // API RIS v2 (mới) - lấy danh sách KTV (TECHNICIAN), y tá (NURSE) cùng khoa với bác sĩ (staffCode = HISCode)
                _listHisUser = await ServiceLocator.RisService2.GetColleaguesAsync(
                    staffCode,
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


        private void _cbbReportTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cbbMauGoiY.SelectedItem is GoiYKetLuanResponse selected)
            {
                _rtMoTa.Text = selected.kqcls_mota;
                _rtKhuyenNghi.Text = selected.kqcls_denghi;
                _rtKetLuan.Text = selected.kqcls_ketluan;

                var tb = _listThietBi.FirstOrDefault(x => x.code == selected.mathietbi);
                if (tb != null)
                {
                    _cbbDSThietBi.EditValue = tb.id;
                }
            }
        }
        #endregion

        #region Event ToolStripMenu

        private void _tsmViewPACS_Click(object sender, EventArgs e)
        {
            if (_machidinh == null) return;

            string url = ServiceLocator.SystemConfig.UrlPacsPublic + _machidinh;

            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch
            {
                MessageBox.Show("Không mở được trình duyệt!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void _tsmDeletePACS_Click(object sender, EventArgs e)
        {
            string machidinh = _machidinh;

            if (string.IsNullOrWhiteSpace(machidinh))
            {
                MessageBox.Show(
                    "Không tìm thấy mã chỉ định để hủy liên kết PACS.",
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn hủy liên kết PACS với mã chỉ định:\n{machidinh} ?",
                this.Text,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                int count = await ServiceLocator.StudyService.DeleteStudyAsync(machidinh);
                if (count > 0)
                {
                    MessageBox.Show(
                        $"Hủy liên kết PACS thành công.\nSố Study đã cập nhật: {count}",
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        "Không tìm thấy Study nào tương ứng với mã chỉ định.",
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Có lỗi xảy ra khi hủy liên kết PACS.\n" + ex.Message,
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void _tsmEditPatient_Click(object sender, EventArgs e)
        {
            try
            {
                using (var patientForm = new PatientForm(
                    _chiDinhDichVuResponse.BenhNhan,
                    _chiDinhDichVuResponse.MaChiDinh))
                {
                    if (patientForm.ShowDialog() == DialogResult.OK)
                    {
                        PopulatePatientInfo(_chiDinhDichVuResponse.BenhNhan);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        private async void _tsmAsyncHis_Click(object sender, EventArgs e)
        {
            try
            {
                var result = await ServiceLocator.RisService.SendKetQuaChanDoanToHisAsync(_machidinh);

                if (result == null)
                {
                    ShowWarningMessage("Cảnh báo", "Không gửi được kết quả sang HIS.");
                    return;
                }

                ShowSuccessMessage("Đã gửi kết quả chẩn đoán sang HIS thành công.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Lỗi khi gửi kết quả chẩn đoán sang HIS cho MaChiDinh: {_machidinh}");
            }
        }

        #endregion

        #region UI Control Events - ListBox
        /// <summary>
        /// Xử lý sự kiện khi trạng thái ListBox thay đổi (thêm, xóa, chọn item)
        /// Cập nhật trạng thái toolbar để phản ánh trạng thái hiện tại của listbox
        /// </summary>
        private void _lstBoxPages_ListStateChanged(object sender, EventArgs e)
        {
            UpdateToolBarState();
        }

        /// <summary>
        /// Xử lý sự kiện khi Context Menu của ListBox sắp mở
        /// Cập nhật trạng thái các menu items dựa trên trạng thái hiện tại của listbox
        /// </summary>
        /// <param name="sender">Object gửi sự kiện</param>
        /// <param name="e">CancelEventArgs - có thể hủy việc mở menu</param>
        private void _cmListBox_Opening(object sender, CancelEventArgs e)
        {
            _cmiExpanded.Checked = _lstBoxPages.ViewMode == ThumbMode.Expanded;
            _cmiCondensed.Checked = _lstBoxPages.ViewMode == ThumbMode.Condensed;

            _cmiDeleteAll.Enabled = _lstBoxPages.Items.Count > 0;
            _cmiDeleteSelected.Enabled = _lstBoxPages.SelectedItems.Count > 0;

        }

        /// <summary>
        /// Chuyển ListBox sang chế độ Expanded (hiển thị chi tiết thumbnail)
        /// </summary>
        private void _cmiExpanded_Click(object sender, EventArgs e)
        {
            _lstBoxPages.ViewMode = ThumbMode.Expanded;
        }


        private void _miResample_Click(object sender, EventArgs e)
        {
            RasterPaintProperties prop = _pictureBox.PaintProperties;
            _mySettings._settings.UseResample = prop.PaintDisplayMode == RasterPaintDisplayModeFlags.Resample;
            if (_mySettings._settings.UseResample)
                prop.PaintDisplayMode = RasterPaintDisplayModeFlags.None;
            else
                prop.PaintDisplayMode = RasterPaintDisplayModeFlags.Resample;
            _mySettings._settings.UseResample = !_mySettings._settings.UseResample;
            _mySettings.Save();
            _pictureBox.PaintProperties = prop;
        }

        /// <summary>
        /// Chuyển ListBox sang chế độ Condensed (hiển thị thu gọn thumbnail)
        /// </summary>
        private void _cmiCondensed_Click(object sender, EventArgs e)
        {
            _lstBoxPages.ViewMode = ThumbMode.Condensed;
        }

        /// <summary>
        /// Mở dialog để chọn và load file ảnh raster vào ListBox
        /// Hỗ trợ preview ảnh và nhớ thư mục cuối cùng đã mở
        /// </summary>
        private void _miOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlgOpen = new OpenFileDialog())
            {
                dlgOpen.Title = "Chọn hình ảnh";
                dlgOpen.Multiselect = true;
                dlgOpen.Filter =
                    "Image files|*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff|" +
                    "All files|*.*";

                string targetFolder = Path.Combine(
                    _baseFolder,
                    "BenhNhan",
                    _machidinh
                );

                // Tạo folder nếu chưa tồn tại
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }

                if (targetFolder != null)
                    dlgOpen.InitialDirectory = Path.GetDirectoryName(Path.Combine($"{_baseFolder}\\BenhNhan\\{_machidinh}"));

                bool bTopMost = logWindow.TopMost;
                logWindow.TopMost = false;

                if (dlgOpen.ShowDialog(this) != DialogResult.OK)
                {
                    logWindow.TopMost = bTopMost;
                    return;
                }

                logWindow.TopMost = bTopMost;

                // Load nhiều file
                foreach (string fileName in dlgOpen.FileNames)
                {
                    string destFile = Path.Combine(
                        targetFolder,
                        Path.GetFileName(fileName)
                    );
                    File.Copy(fileName, destFile, true);
                    LoadRasterImage(fileName);
                }

                strLastLocation = dlgOpen.FileNames[0];
            }
        }

        private void _miPaste_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap img = (Bitmap)Clipboard.GetImage();
                RasterImage rImg = Leadtools.Drawing.RasterImageConverter.ChangeFromHBitmap(img.GetHbitmap(), IntPtr.Zero);
                CreateImageCollection("Pasted Image", rImg);
            }
            catch { }
        }

        private void _toolBtnTwain_Click(object sender, EventArgs e)
        {
            _miTwainAcquire_Click(null, null);
        }

        private bool BeforeAddTagDelegate(LinkedList<long> parent, object data, long tag)
        {
            DicomTag dcmTag = DicomTagTable.Instance.Find(tag);

            if (dcmTag != null)
            {
                Console.WriteLine("Tag: " + dcmTag.Name);
            }
            else
                Console.WriteLine(string.Format("Tag: {0:x4}:{1:x4}", DicomExtensions.GetGroup(tag), DicomExtensions.GetElement(tag)));

            return false;
        }

        private void _miRotate90_Click(object sender, EventArgs e)
        {
            if (_pictureBox.Image == null)
                return;

            try
            {
                _pictureBox.Image.RotateViewPerspective(90);
                _lstBoxPages.SelectedItem.RasterImage.RotateViewPerspective(90);
                string strFileLoc = (_lstBoxPages.SelectedItem.ImageItem.Tag as IPrintToPACSFile).FileLocation();

                if (_lstBoxPages.SelectedItem.ImageItem.Tag.GetType() == typeof(PrintPage))
                    _codec.Save(_lstBoxPages.SelectedItem.RasterImage, strFileLoc, RasterImageFormat.Emf, 0);
                else
                    _codec.Save(_lstBoxPages.SelectedItem.RasterImage, strFileLoc, RasterImageFormat.Tif, 0);
            }
            catch { }
        }

        private void _toolBtnRotate_Click(object sender, EventArgs e)
        {
            _miRotate90_Click(null, null);
        }

        /// <summary>
        ///  Xử lý khi bỏ chọn item
        /// </summary>
        private void _lstBoxPages_ItemDeSlect(object sender, EventArgs e)
        {
            if (_pictureBox.Image != null)
            {
                _pictureBox.Image.Dispose();
                _pictureBox.Image = null;
            }
            //_btnNext.Enabled = false;
            //_btnPrev.Enabled = false;
            //_lblPageInfo.Text = "";
            UpdateToolBarState();
        }

        private void _pictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) != 0)
            {
                if (e.Delta > 0)
                    _miZoomIn_Click(null, null);
                else
                    _miZoomOut_Click(null, null);
            }
            else
            {
                int iSelectedPage = 0;
                if (_lstBoxPages.ViewMode == ThumbMode.Condensed)
                    iSelectedPage = _lstBoxPages.SelectedItemGroupIndex;
                else
                    iSelectedPage = _lstBoxPages.SelectedIndex;

            }
        }

        private void _miClearPrintedList_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show(
                    "Bạn có chắc muốn xóa tất cả mục đã chọn (Ảnh sẽ mất vĩnh viễn)?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result != DialogResult.Yes)
                    return; // Người dùng không đồng ý → thoát

                // Nếu chọn Yes thì tiếp tục xóa
                ClearList();

                if (_pictureBox.Image != null)
                {
                    _pictureBox.Image.Dispose();
                    _pictureBox.Image = null;
                }

                UpdateToolBarState();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void _miExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void _lstBoxPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScalePicture(_lstBoxPages.SelectedItem.ImageItem);
            UpdateToolBarState();
            _iOldIndex = _lstBoxPages.SelectedIndex;
            _mySettings._settings.LastSelectedIndex = _lstBoxPages.SelectedIndex;
            _mySettings.Save();
        }

        private void _miFile_DropDownOpening(object sender, EventArgs e)
        {
            try
            {
                _miSaveAsDICOM.Enabled = (_lstBoxPages.CheckedItems.Count > 0);
                _miStoreToPACS.Enabled = (_lstBoxPages.CheckedItems.Count > 0);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void _lstBoxPages_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete && _lstBoxPages.SelectedItem != null)
                    _miDeleteSelected_Click(null, null);

                if (e.KeyCode == Keys.V && Control.ModifierKeys == Keys.Control)
                    _miPaste_Click(null, null);

                else if (e.KeyCode == Keys.Add)
                {
                    _miZoomIn_Click(_miZoomIn, new EventArgs());
                }
                else if (e.KeyCode == Keys.Subtract)
                {
                    _miZoomOut_Click(_miZoomOut, new EventArgs());
                }
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        #region UI Control Events - PictureBox & Image Viewer

        private void _miNormal_Click(object sender, EventArgs e)
        {
            try
            {
                _pictureBox.SizeMode = RasterPaintSizeMode.Normal;
                _pictureBox.ScaleFactor = 1;
            }
            catch (Exception exp)
            {
                Messager.ShowError(this, exp);
            }
        }

        private void _miFit_Click(object sender, EventArgs e)
        {
            try
            {
                _pictureBox.SizeMode = RasterPaintSizeMode.FitAlways;
                _pictureBox.ScaleFactor = 1;
            }
            catch (Exception exp)
            {
                Messager.ShowError(this, exp);
            }
        }

        private void _cmResultQuery_Opening(object sender, CancelEventArgs e)
        {
            //if (_lstSelected == null || _lstSelected.Items.Count == 0 || _lstSelected == _lstSCPStudies)
            //{
            //    e.Cancel = true;
            //}
            //toolStripSeparator22.Visible = _miDeleteSelectedDataSet.Visible = _lstSelected == _lstDSPatient;
            //_miDeleteSelectedDataSet.Enabled = _lstDSPatient.SelectedItems.Count >= 1;
        }

        private void _miZoomIn_Click(object sender, EventArgs e)
        {
            try
            {
                ZoomPicture(0.1f);
            }
            catch (Exception exp)
            {
                Messager.ShowError(this, exp);
            }
        }

        private void _miZoomOut_Click(object sender, EventArgs e)
        {
            try
            {
                if (_pictureBox.ScaleFactor > 0.1f)
                {
                    ZoomPicture(-0.1f);
                }
            }
            catch (Exception exp)
            {
                Messager.ShowError(this, exp);
            }
        }

        private void _pictureBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Add)
                {
                    _miZoomIn_Click(_miZoomIn, new EventArgs());
                }
                else if (e.KeyCode == Keys.Subtract)
                {
                    _miZoomOut_Click(_miZoomOut, new EventArgs());
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void _miSaveAsDICOM_Click(object sender, EventArgs e)
        {
            DicomDataSet dicom = (_pgDicomInfo.SelectedObject as DicomEditableObject).DataSet;
            if (!CheckRequiredTags(dicom))
                return;

            SaveFileDialog dlgSave = new SaveFileDialog();
            dlgSave.Filter = "DICOM Files|*.dcm|DICOM DataSet Files|*.dic";
            if (strLastLocation != "")
                dlgSave.InitialDirectory = Path.GetDirectoryName(strLastLocation);

            bool bTopMost = logWindow.TopMost;
            logWindow.TopMost = false;
            DialogResult dlgRes = dlgSave.ShowDialog();

            if (dlgRes == DialogResult.Cancel)
            {
                logWindow.TopMost = bTopMost;
                return;
            }
            try
            {
                List<string> lstSaved = new List<string>();
                string strSaveLocation = dlgSave.FileName;
                strLastLocation = strSaveLocation;
                bool bSuccess = false;
                EnableItems(false, "Saving Files To HardDisk Please Wait...", "Cancel");
                string strMessage = DoSave(dicom, ref lstSaved, strSaveLocation, ref bSuccess);

                MessageBoxIcon icon = MessageBoxIcon.Information;
                if (bSuccess)
                    icon = MessageBoxIcon.Information;
                else
                    icon = MessageBoxIcon.Error;

                EnableItems(true, "", "");
                if (bSuccess)
                {
                    DialogResult dlgClear = MessageBox.Show(this, strMessage + "\nDo you want to clear the DICOM information?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlgClear == DialogResult.Yes)
                    {
                        _miClearPG_Click(null, null);
                    }
                }
                else
                {
                    MessageBox.Show(this, "DICOM file was not saved successfully", this.Text, MessageBoxButtons.OK, icon);
                }
            }
            catch { }
            logWindow.TopMost = bTopMost;
        }

        private bool CheckRequiredTags(DicomDataSet dicom)
        {
            string strMessage = "";
            List<string> lstRequired = new List<string>();
            GetRequiredTags(dicom, lstRequired);

            DicomElement dElement = dicom.FindFirstElement(null, DicomTag.PatientName, true);
            string val = dicom.GetValue<string>(dElement, "");


            if (val == string.Empty)
                lstRequired.Add("Patient Name");

            dElement = dicom.FindFirstElement(null, DicomTag.PatientID, true);
            val = dicom.GetValue<string>(dElement, "");
            if (val == string.Empty)
                lstRequired.Add("Patient ID");

            if (lstRequired.Count > 0)
            {
                strMessage = "The Following Tags Are Required:\n";
                foreach (string strName in lstRequired)
                {
                    strMessage += "--> " + strName + "\n";
                }
            }

            if (_lstBoxPages.CheckedItems.Count == 0 && strMessage == "")
            {
                strMessage = "One or more Print job/pages needs to be checked";
            }
            if (strMessage != "")
            {
                bool bTopMost = logWindow.TopMost;
                logWindow.TopMost = false;
                MessageBox.Show(this, strMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                logWindow.TopMost = bTopMost;
                return false;
            }
            else
                return true;
        }

        private async void _miStoreToPACS_Click(object sender, EventArgs e)
        {
            try
            {
                if (_mySettings._settings.StoreServers.serverList.Length == 0)
                    return;
                //await ServiceLocator.RisService.SearchAndDeleteStudiesAsync(ServiceLocator.StorageServer, _machidinh);
                MyServer server = _mySettings._settings.StoreServers.serverList[toolStripComboBoxStoreServer.SelectedIndex];
                string strTemp, strMessage = string.Empty;
                strTemp = Path.GetTempFileName();

                DicomDataSet dicom = (_pgDicomInfo.SelectedObject as DicomEditableObject).DataSet;
                List<string> lstSaved = new List<string>();
                bool bSuccess = false;

                if (!CheckRequiredTags(dicom))
                    return;

                EnableItems(false, "Đang lưu các tập tin tạm thời vào ổ cứng \nVui lòng đợi...", "Cancel");
                bool bTopMost = logWindow.TopMost;
                logWindow.TopMost = false;
                strMessage = DoSave(dicom, ref lstSaved, strTemp, ref bSuccess);
                EnableItems(true, "", "");

                if (bSuccess && !bCancelOperation)
                {
                    EnableItems(false, "Đang lưu trữ vào PACS vui lòng đợi...", "Cancel");
                    try
                    {
                        strMessage = "\nĐang lưu trữ vào PACS:\n";
                        foreach (string strFile in lstSaved)
                        {
                            try
                            {
                                DoStore(strFile, server);
                            }
                            catch (Exception ex)
                            {
                                logWindow.TopMost = false;
                                EnableItems(true, "", "");
                                MessageBox.Show("Đã xảy ra lỗi: \n" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            Application.DoEvents();
                            if (bCancelOperation)
                                break;
                        }
                        bSuccess = true;
                        strMessage += "\nTệp được lưu trữ thành công";
                    }
                    catch (System.Exception ex)
                    {
                        bSuccess = false;
                        strMessage += "Đã xảy ra lỗi:\n" + ex.Message;
                    }
                }
                File.Delete(strTemp);

                EnableItems(true, "", "");

                if (bSuccess && bStored && !bCancelOperation)
                {
                }
                else
                {
                    logWindow.TopMost = false;
                    if (bCancelOperation)
                        MessageBox.Show(this, "Đã hủy thao tác", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        if (bSuccess)
                            MessageBox.Show(this, "Tệp không được lưu trữ thành công", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                            MessageBox.Show(this, strMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                logWindow.TopMost = bTopMost;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi thực hiện Store to PACS");
            }
        }

        private void _miClearPG_Click(object sender, EventArgs e)
        {
            DicomDataSet ds = _pgDicomInfo.DataSet;
            ds.Dispose();
            _pgDicomInfo.DataSet = null;
            _cmbSopClasses_SelectedIndexChanged(null, null);
        }

        private void _lstBoxPages_ItemAdded(object sender, System.EventArgs e)
        {
        }

        private void _cmbSopClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cmbSopClasses.SelectedIndex >= 0)
            {
                DicomDataSet tempDataSet = new DicomDataSet(), sourceDataSet = _pgDicomInfo.DataSet;
                DicomModule module = null;
                //Clone the dataset
                if (sourceDataSet != null)
                {
                    tempDataSet.Initialize(_pgDicomInfo.DataSet.InformationClass, DicomDataSetInitializeFlags.AddMandatoryElementsOnly |
                    DicomDataSetInitializeFlags.AddMandatoryModulesOnly);

                    module = sourceDataSet.FindModule(DicomModuleType.GeneralStudy);
                    if (module != null)
                        SetElements(tempDataSet, module.Elements, sourceDataSet);

                    module = sourceDataSet.FindModule(DicomModuleType.Patient);
                    if (module != null)
                        SetElements(tempDataSet, module.Elements, sourceDataSet);
                }

                InitializeDataSet(ClassTypes[_cmbSopClasses.SelectedIndex]);

                //Restore the dataset
                if (sourceDataSet != null)
                {
                    sourceDataSet = tempDataSet;

                    module = sourceDataSet.FindModule(DicomModuleType.GeneralStudy);
                    if (module != null)
                        SetElements(_pgDicomInfo.DataSet, module.Elements, sourceDataSet);

                    module = sourceDataSet.FindModule(DicomModuleType.Patient);
                    if (module != null)
                        SetElements(_pgDicomInfo.DataSet, module.Elements, sourceDataSet);

                    GenerateDefaultElements();
                }
                else
                {
                    GenerateDefaultElements();
                    InsertNewStudyModule();
                }

            }
        }


        private void InitializeDataSet(DicomClassType dClass)
        {
            DicomDataSet ds = new DicomDataSet();
            try
            {
                if (dClass == DicomClassType.SCImageStorage)
                    if (File.Exists(_mySettings._settings.secondaryCapturePath))
                        DicomExtensions.LoadXml(ds, _mySettings._settings.secondaryCapturePath, DicomDataSetLoadXmlFlags.None, null, null);

                if (dClass == DicomClassType.SCMultiFrameTrueColorImageStorage)
                    if (File.Exists(_mySettings._settings.secondaryCaptureColorPath))
                        DicomExtensions.LoadXml(ds, _mySettings._settings.secondaryCaptureColorPath, DicomDataSetLoadXmlFlags.None, null, null);

                if (dClass == DicomClassType.SCMultiFrameGrayscaleByteImageStorage)
                    if (File.Exists(_mySettings._settings.secondaryCaptureGrayPath))
                        DicomExtensions.LoadXml(ds, _mySettings._settings.secondaryCaptureGrayPath, DicomDataSetLoadXmlFlags.None, null, null);

                if (dClass == DicomClassType.EncapsulatedPdfStorage)
                    if (File.Exists(_mySettings._settings.PdfPath))
                        DicomExtensions.LoadXml(ds, _mySettings._settings.PdfPath, DicomDataSetLoadXmlFlags.None, null, null);
            }
            catch { }

            if (ds == null || ds.InformationClass != dClass)
                ds.Initialize(dClass, DicomDataSetInitializeFlags.AddMandatoryElementsOnly |
                   DicomDataSetInitializeFlags.AddMandatoryModulesOnly);

            ClearTag(ds, DicomTag.PixelData);
            ClearTag(ds, DicomTag.EncapsulatedDocument);

            DicomElement dElement = ds.FindFirstElement(null, DicomTag.Modality, true);
            if (dElement == null)
                dElement = ds.InsertElement(null, false, DicomTag.Modality, DicomVRType.UN, false, 0);
            ds.SetValue(dElement, _chiDinhDichVuResponse?.Modality ?? "OT");

            _pgDicomInfo.DataSet = ds;
        }

        public void LoadRasterImage(string strFileName)
        {
            bool bTopMost = logWindow.TopMost;
            RasterImage rImg = null;
            try
            {
                //EnableItems(false, "Opening Image Files Please Wait...", "Cancel");
                string strFile = strFileName;
                strLastLocation = strFile;

                _codec.Options.Load.AllPages = true;
                _codec.Options.Pdf.Load.DisplayDepth = 24;
                _codec.Options.Pdf.Load.GraphicsAlpha = 4;
                _codec.Options.Pdf.Load.TextAlpha = 2;
                _codec.Options.Pdf.Load.UseLibFonts = true;

                // Nếu cần siêu nét cho in ấn thì có thể lên 600 DPI
                _codec.Options.RasterizeDocument.Load.XResolution = 600;
                _codec.Options.RasterizeDocument.Load.YResolution = 600;
                _codec.Options.RasterizeDocument.Load.Resolution = 600;

                rImg = _codec.Load(strFile);

                GrayscaleCommand command = new GrayscaleCommand(8);
                if (rImg.IsGray && rImg.BitsPerPixel != 8)
                    command.Run(rImg);

                ListImageBox.ImageCollection imagecollection = new ListImageBox.ImageCollection(strFile);
                Page page = new Page();
                for (int i = 1; i <= rImg.PageCount; i++)
                {
                    string strTemp = null;
                    rImg.Page = i;

                    page = new Page();
                    strTemp = Path.GetTempFileName();
                    int iBPP = rImg.BitsPerPixel;
                    if (iBPP < 8)
                        iBPP = 8;
                    RasterImage rTempRaster = rImg.Clone();
                    _codec.Save(rTempRaster, strTemp, RasterImageFormat.Tif, iBPP);
                    rTempRaster.Dispose();
                    page.FilePath = strTemp;
                    page.DeleteOnDispose = true;
                    imagecollection.Images.Add(new ListImageBox.ImageItem(_codec.Load(strTemp), imagecollection, page));
                    Application.DoEvents();
                    if (bCancelOperation)
                        break;
                }
                rImg.Dispose();

                var parentName = imagecollection.Images[0]?.Parent?.Name;


                if (listImageKeyLocal != null && listImageKeyLocal.Contains(strFileName, StringComparer.OrdinalIgnoreCase))
                {
                    imagecollection.Images[0].Checked = true;
                }
                else
                {
                    // Nếu không có, có thể log hoặc bỏ qua
                    Console.WriteLine($"File {strFileName} không có trong danh sách.");
                }

                _lstBoxPages.AddImageCollection(imagecollection);
            }
            catch (System.Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
                rImg?.Dispose(); //Luôn dispose
            }
            EnableItems(true, "", "");
            logWindow.TopMost = bTopMost;
        }

        private void GenerateDefaultElements()
        {
            GenerateUidTag(_pgDicomInfo.DataSet, DicomTag.SeriesInstanceUID);
            GenerateUidTag(_pgDicomInfo.DataSet, DicomTag.SOPInstanceUID);

            DicomElement dElement = _pgDicomInfo.DataSet.FindFirstElement(null, DicomTag.InstanceNumber, false);
            if (dElement != null)
                _pgDicomInfo.DataSet.SetValue(dElement, "1");

            dElement = _pgDicomInfo.DataSet.FindFirstElement(null, DicomTag.ConversionType, false);
            if (dElement != null)
                _pgDicomInfo.DataSet.SetValue(dElement, "DI");

            dElement = _pgDicomInfo.DataSet.FindFirstElement(null, DicomTag.SeriesNumber, false);
            if (dElement != null)
                _pgDicomInfo.DataSet.SetValue(dElement, "1");

            dElement = _pgDicomInfo.DataSet.FindFirstElement(null, DicomTag.FrameIncrementPointer, false);
            if (dElement != null)
                _pgDicomInfo.DataSet.SetValue(dElement, 0x182001); //HEX 2C6F1H

            dElement = _pgDicomInfo.DataSet.FindFirstElement(null, DicomTag.MIMETypeOfEncapsulatedDocument, false);
            if (dElement != null)
                _pgDicomInfo.DataSet.SetValue(dElement, "PDF");

            if (_pgDicomInfo.DataSet.InformationClass == DicomClassType.SCMultiFrameGrayscaleByteImageStorage ||
               _pgDicomInfo.DataSet.InformationClass == DicomClassType.SCMultiFrameTrueColorImageStorage)
            {
                dElement = _pgDicomInfo.DataSet.FindFirstElement(null, DicomTag.PageNumberVector, true);
                if (dElement == null)
                    dElement = _pgDicomInfo.DataSet.InsertElement(null, false, DicomTag.PageNumberVector, DicomVRType.IS, false, 0);
            }

            _pgDicomInfo.DataSet = _pgDicomInfo.DataSet;
        }

        private void InsertNewSeries()
        {
            DicomDataSet ds = _pgDicomInfo.DataSet;
            DicomElement dElement = ds.FindFirstElement(null, DicomTag.Modality, true);
            if (dElement == null)
                dElement = ds.InsertElement(null, false, DicomTag.Modality, DicomVRType.UN, false, 0);
            if (ds.InformationClass == DicomClassType.EncapsulatedPdfStorage)
                ds.SetValue(dElement, "DOC");
            else
                ds.SetValue(dElement, _chiDinhDichVuResponse.Modality);
            _pgDicomInfo.DataSet = ds;
        }

        private void InsertNewStudyModule()
        {
            DicomElement dElement;
            GenerateUidTag(_pgDicomInfo.DataSet, DicomTag.StudyInstanceUID);

            dElement = _pgDicomInfo.DataSet.FindFirstElement(null, DicomTag.StudyDate, false);
            if (dElement != null)
                _pgDicomInfo.DataSet.SetDateValue(dElement, new DateTime[] { DateTime.Now.Date });

            dElement = _pgDicomInfo.DataSet.FindFirstElement(null, DicomTag.StudyTime, false);
            if (dElement != null)
                _pgDicomInfo.DataSet.SetTimeValue(dElement, new DateTime[] { new DateTime(DateTime.Now.Year, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) });

            dElement = _pgDicomInfo.DataSet.FindFirstElement(null, DicomTag.StudyID, false);
            if (dElement != null)
                _pgDicomInfo.DataSet.SetValue(dElement, "1");
            _pgDicomInfo.DataSet = _pgDicomInfo.DataSet;
        }

        private void _miEdit_DropDownOpening(object sender, EventArgs e)
        {
            _miRotate90.Enabled = _miDeleteSelected.Enabled = (_lstBoxPages.SelectedItems.Count > 0);
            _miDeleteAll.Enabled = (_lstBoxPages.Items.Count > 0);
            _miPaste.Enabled = Clipboard.ContainsImage();
        }

        private void _miDeleteSelected_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc muốn xóa mục đã chọn (Ảnh sẽ mất vĩnh viễn)?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
                return; // Người dùng không đồng ý → thoát

            // Nếu chọn Yes thì tiếp tục xóa
            DeleteSelectedItems();

            if (_pictureBox.Image != null)
            {
                _pictureBox.Image.Dispose();
                _pictureBox.Image = null;
            }

            UpdateToolBarState();
        }

        private void _miResetInfo_Click(object sender, EventArgs e)
        {
            _miClearPG_Click(null, null);
        }

        private void _toolBtnStoreToPacs_Click(object sender, EventArgs e)
        {
            _miStoreToPACS_Click(null, null);
        }

        private void _toolBtnSaveDicom_Click(object sender, EventArgs e)
        {
            _miSaveAsDICOM_Click(null, null);
        }

        private void _toolBtnCLearInfo_Click(object sender, EventArgs e)
        {
            _miClearPG_Click(null, null);
        }

        private void _toolBtnDeleteAll_Click(object sender, EventArgs e)
        {
            _miClearPrintedList_Click(null, null);
        }

        private void _toolBtnDeleteSelected_Click(object sender, EventArgs e)
        {
            _miDeleteSelected_Click(null, null);
        }

        private void _toolBtnViewLog_Click(object sender, EventArgs e)
        {
            _miViewLog_Click(null, null);
        }

        private void _miViewLog_Click(object sender, EventArgs e)
        {
            logWindow.Visible = !logWindow.Visible;
            UpdateToolBarState();
        }

        private void _toolBtnOpenRaster_Click(object sender, EventArgs e)
        {
            _miOpen_Click(null, null);
        }

        private void _miView_DropDownOpening(object sender, EventArgs e)
        {
            _miResample.Enabled = _miFit.Enabled = _miNormal.Enabled = _miZoomIn.Enabled = _miZoomOut.Enabled = (_pictureBox.Image != null);
            RasterPaintProperties prop = _pictureBox.PaintProperties;
            _miResample.Checked = (prop.PaintDisplayMode == RasterPaintDisplayModeFlags.Resample);
            _miNormal.Checked = _pictureBox.SizeMode == RasterPaintSizeMode.Normal;
            _miFit.Checked = _pictureBox.SizeMode == RasterPaintSizeMode.FitAlways;
            _miViewLog.Checked = logWindow.Visible;
            double oldScaleFactor = _pictureBox.ScaleFactor, dZoomFactor = 0.1;
            oldScaleFactor = _pictureBox.ScaleFactor + dZoomFactor;
            _miZoomIn.Enabled = _pictureBox.Image != null && !(oldScaleFactor > 3 && dZoomFactor > 0);
            oldScaleFactor = _pictureBox.ScaleFactor - dZoomFactor;
            _miZoomOut.Enabled = _pictureBox.Image != null && !(oldScaleFactor < .06 && -dZoomFactor < 0);
        }

        private void _cbSevers_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyServer server = (toolStripComboBoxStoreServer.SelectedItem as MyServer);
        }

        private void _toolBtnScreenCapture_Click(object sender, EventArgs e)
        {
            _engine.StopCapture();
            bool bTemp = _isHotKeyEnabled;
            _isHotKeyEnabled = false;
            Leadtools.ScreenCapture.ScreenCaptureOptions opt = _engine.CaptureOptions;
            Keys oldKey = opt.Hotkey;
            opt.Hotkey = Keys.None;
            _engine.CaptureOptions = opt;
            DoCapture(_mySettings._settings.capturetype);
            _isHotKeyEnabled = bTemp;
            opt.Hotkey = oldKey;
            _engine.CaptureOptions = opt;
        }

        void _pictureBox_DoubleClick(object sender, EventArgs e)
        {
        }

        private async void _btnPushToPACS_Click(object sender, EventArgs e)
        {
            try
            {
                _toolBtnStoreToPacs_Click(null, null);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi bấm nút Push to PACS");
            }
        }
        #endregion

        #endregion

        #region Save/Load Operations

        private void _btnSave_Click(object sender, EventArgs e)
        {
            LuuNhap();
        }

        private async void LuuNhap()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitFormLoading), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu...");
                SplashScreenManager.Default.SetWaitFormDescription("Vui lòng chờ trong giây lát...");
                _btnSave.Enabled = false;

                var layoutSelect = _cbbLayout.SelectedItem as ReportTemplateGridViewModel;
                if (layoutSelect != null)
                {
                    ServiceLocator.ReportCache[_chiDinhDichVuResponse.Modality] = layoutSelect.Id;
                }

                List<string> imageSelectedList = new List<string>();
                listImageKeyLocal = new List<string>();

                var checkedImages = _lstBoxPages.ImageCollections
                    .Where(x => x.Images?.Count > 0 && x.Images[0].Checked)
                    .Select(x => x.Images[0])
                    .ToList();

                foreach (var image in checkedImages)
                {
                    try
                    {
                        string filePath = image.Parent.Name;
                        byte[] fileBytes = File.ReadAllBytes(filePath);
                        string base64String = Convert.ToBase64String(fileBytes);
                        listImageKeyLocal.Add(filePath);
                        if (base64String != null)
                        {
                            imageSelectedList.Add(base64String);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                string mota = _rtMoTa.Text;
                string ketluan = _rtKetLuan.Text;
                string khuyennghi = _rtKhuyenNghi.Text;

                string maThietBi = null;
                string tenThietBi = null;
                string maKTV = null;
                string tenKTV = null;

                var selectedValue = _cbbDSThietBi.EditValue;
                if (selectedValue != null)
                {
                    int selectedThietBiId = Convert.ToInt32(selectedValue);
                    var thietBiSelect = _listThietBi.FirstOrDefault(x => x.id == selectedThietBiId);
                    if (thietBiSelect != null)
                    {
                        tenThietBi = thietBiSelect.name;
                        maThietBi = thietBiSelect.code;
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng chọn thiết bị hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn thiết bị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                var selectedValueHisUser = _cbbHisUser.EditValue;
                if (selectedValueHisUser != null)
                {
                    var ktvSelect = _cbbHisUser.Properties.GetDataSourceRowByKeyValue(selectedValueHisUser) as PractitionerListDto;
                    if (ktvSelect != null)
                    {
                        maKTV = ktvSelect.staffCode;
                        tenKTV = ktvSelect.fullName;
                    }
                }

                var ketQuaChanDoanRequest = new KetQuaChanDoanRequest()
                {
                    kqcls_mota = string.IsNullOrWhiteSpace(mota) ? "_" : mota,
                    kqcls_denghi = string.IsNullOrWhiteSpace(khuyennghi) ? "_" : khuyennghi,
                    kqcls_ketluan = string.IsNullOrWhiteSpace(ketluan) ? "_" : ketluan,
                    mabacsiketluan = ServiceLocator.KeycloakUserInfo.HISCode,
                    machidinh = _machidinh,
                    sophieu = sophieu,
                    bacsiketluan = ServiceLocator.KeycloakUserInfo.FirstName + " " + ServiceLocator.KeycloakUserInfo.LastName,
                    trangthai = "Nháp",
                    kythuatvien = tenKTV,
                    makythuatvien = maKTV,
                    mathietbi = maThietBi,
                    tenthietbi = tenThietBi,
                    thoigianketthuc = _dateTGKetThuc.DateTime,
                    thoigianthuchien = _dateTGThucHien.DateTime,
                    imageFileKeys = imageSelectedList,
                };

                _kqChanDoanResponse = await ServiceLocator.RisService.TaoKetQuaChanDoanAsync(ketQuaChanDoanRequest);

                SplashScreenManager.CloseForm(false);

                if (_kqChanDoanResponse != null)
                {
                    XmlSettingsHelper.Save<List<string>>(Path.Combine($"{_baseFolder}\\BenhNhan\\{_machidinh}", FileNameXMLImage), listImageKeyLocal);
                    _btnSignature.Enabled = true;
                    _btnPreviewMain.Enabled = true;
                    XtraMessageBox.Show(
                        this,
                        "Lưu Kết Luận Chẩn Đoán Thành Công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    XtraMessageBox.Show(
                        this,
                        "Lưu thất bại!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show(this, $"Lỗi khi lưu dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Luôn đóng SplashScreen
                if (SplashScreenManager.Default != null && SplashScreenManager.Default.IsSplashFormVisible)
                    SplashScreenManager.CloseForm(false);

                _btnSave.Enabled = true;
            }
        }

        public bool IsCheckRecord = false;
        private Panel _selectedPanel;
        private void PictureBoxRoll_Click(object sender, EventArgs e)
        {
            Panel parentPanel = null;
            string videoPath = "";

            if (sender is PictureBox pb)
            {
                parentPanel = pb.Parent as Panel;
                videoPath = pb.Tag as string;
            }
            else if (sender is Panel pnl)
            {
                parentPanel = pnl;
                if (pnl.Controls.Count > 0 && pnl.Controls[0] is PictureBox picture)
                    videoPath = picture.Tag as string;
            }

            if (_selectedPanel != null)
                _selectedPanel.BackColor = Color.Transparent;

            _selectedPanel = parentPanel;
            _selectedPanel.BackColor = Color.Red; // màu viền

            if (!string.IsNullOrEmpty(videoPath) && File.Exists(videoPath))
            {
                _mediaPlayerControl.SetFilePathMedia(videoPath);
            }
        }
        #endregion

        #region Camera/Media Operations
        // Constants
        private const int VIDEO_FILE_MIN_SIZE = 1024; // 1KB
        private const int VIDEO_FILE_CHECK_RETRIES = 20;
        private const int VIDEO_FILE_CHECK_DELAY_MS = 100;
        private const int STOP_RECORD_DELAY_MS = 1000;
        private const int SNAPSHOT_DELAY_MS = 500;
        private const int THUMBNAIL_IMAGE_SIZE = 110;
        private const int THUMBNAIL_BORDER_THICKNESS = 2;
        private const int THUMBNAIL_MARGIN = 5;

        //private async void _btnRecord_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (IsCheckRecord)
        //        {
        //            _btnRecord.Text = "Ghi lại";
        //            _btnRecord.Image = global::STM.MediaToPACS.Main.Properties.Resources.circle;
        //            IsCheckRecord = false;

        //            await _cameraControl.StopCaptureAsync();
        //            await Task.Delay(1000);
        //            string lastVideoPath = _listPathVideoRecords.LastOrDefault();
        //            if (string.IsNullOrEmpty(lastVideoPath)) return;

        //            for (int i = 0; i < 20; i++)
        //            {
        //                try
        //                {
        //                    using (FileStream fs = new FileStream(lastVideoPath, FileMode.Open, FileAccess.Read, FileShare.Read))
        //                    {
        //                        if (fs.Length > 1024) break;
        //                    }
        //                }
        //                catch
        //                {
        //                    await Task.Delay(100);
        //                }
        //            }

        //            var thumbnail = await GetVideoThumbnailAsync(lastVideoPath);

        //            if (thumbnail != null)
        //            {
        //                Console.WriteLine(lastVideoPath);
        //                _listPathVideoRecords.Remove(lastVideoPath);

        //                int borderThickness = 2;
        //                int imageSize = 110;

        //                // Panel bao ngoài PictureBox
        //                Panel containerPanel = new Panel
        //                {
        //                    Size = new Size(imageSize + borderThickness * 2, imageSize + borderThickness * 2),
        //                    BackColor = Color.Transparent,
        //                    Margin = new Padding(5),
        //                    Padding = new Padding(borderThickness),
        //                    Tag = lastVideoPath
        //                };

        //                PictureBox pictureBox = new PictureBox
        //                {
        //                    Image = thumbnail,
        //                    SizeMode = PictureBoxSizeMode.Zoom,
        //                    BackColor = Color.Black,
        //                    Cursor = Cursors.Hand,
        //                    BorderStyle = BorderStyle.None,
        //                    Tag = lastVideoPath,
        //                    Size = new Size(imageSize, imageSize)
        //                };

        //                pictureBox.Click += PictureBoxRoll_Click;
        //                containerPanel.Click += PictureBoxRoll_Click;

        //                containerPanel.Controls.Add(pictureBox);
        //                //_fPLRoll.Controls.Add(containerPanel);
        //            }
        //        }
        //        else
        //        {
        //            _btnRecord.Text = "Dừng";
        //            _btnRecord.Image = global::STM.MediaToPACS.Main.Properties.Resources.StopCamera;
        //            IsCheckRecord = true;
        //            _listPathVideoRecords.Add(await _cameraControl.StartRecordAsync(_folderChiDinh));
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        /// <summary>
        /// Xử lý sự kiện Record button - Bắt đầu/Dừng ghi video
        /// </summary>
        private async void _btnRecord_Click(object sender, EventArgs e)
        {
            try
            {
                _btnRecord.Enabled = false;
                if (IsCheckRecord)
                {
                    await StopRecordingAsync();
                }
                else
                {
                    await StartRecordingAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi thao tác ghi video");
                XtraMessageBox.Show(this, $"Lỗi khi ghi video: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _btnRecord.Enabled = true;
            }
        }

        /// <summary>
        /// Bắt đầu ghi video
        /// </summary>
        private async Task StartRecordingAsync()
        {
            try
            {
                Log.Information("Bắt đầu ghi video");

                string videoPath = await _cameraControl.StartRecordAsync(_machidinh);
                if (string.IsNullOrEmpty(videoPath))
                {
                    Log.Warning("Đường dẫn video rỗng");
                    return;
                }

                _listPathVideoRecords.Add(videoPath);
                IsCheckRecord = true;

                // Cập nhật UI
                _btnRecord.Text = "Dừng";

                Log.Information($"Đã bắt đầu ghi video: {videoPath}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi bắt đầu ghi video");
                throw;
            }
        }

        /// <summary>
        /// Dừng ghi video và tạo thumbnail
        /// </summary>
        private async Task StopRecordingAsync()
        {
            try
            {
                Log.Information("Dừng ghi video");

                // Dừng capture
                await _cameraControl.StopCaptureAsync();
                await Task.Delay(STOP_RECORD_DELAY_MS);

                // Lấy đường dẫn video cuối cùng
                string lastVideoPath = _listPathVideoRecords.LastOrDefault();
                if (string.IsNullOrEmpty(lastVideoPath))
                {
                    Log.Warning("Không tìm thấy đường dẫn video để xử lý");
                    UpdateRecordButtonUI(false);
                    IsCheckRecord = false;
                    return;
                }

                // Kiểm tra file video đã được ghi xong chưa
                bool fileReady = await WaitForVideoFileReadyAsync(lastVideoPath);
                if (!fileReady)
                {
                    Log.Warning($"File video chưa sẵn sàng: {lastVideoPath}");
                }

                // Tạo thumbnail
                await CreateVideoThumbnailAsync(lastVideoPath);

                // Xóa khỏi danh sách
                _listPathVideoRecords.Remove(lastVideoPath);

                // Cập nhật UI
                UpdateRecordButtonUI(false);
                IsCheckRecord = false;

                Log.Information($"Đã dừng ghi video: {lastVideoPath}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi dừng ghi video");
                throw;
            }
        }

        /// <summary>
        /// Đợi file video sẵn sàng (đã được ghi xong)
        /// </summary>
        private async Task<bool> WaitForVideoFileReadyAsync(string videoPath)
        {
            for (int i = 0; i < VIDEO_FILE_CHECK_RETRIES; i++)
            {
                try
                {
                    if (!File.Exists(videoPath))
                    {
                        await Task.Delay(VIDEO_FILE_CHECK_DELAY_MS);
                        continue;
                    }

                    using (var fs = new FileStream(videoPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        if (fs.Length > VIDEO_FILE_MIN_SIZE)
                        {
                            return true;
                        }
                    }
                }
                catch (IOException)
                {
                    // File đang được ghi, đợi thêm
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, $"Lỗi khi kiểm tra file video (lần {i + 1}): {videoPath}");
                }

                await Task.Delay(VIDEO_FILE_CHECK_DELAY_MS);
            }

            return false;
        }

        /// <summary>
        /// GetThumbnail từ video
        /// </summary>
        public async Task<Image> GetVideoThumbnailAsync(string videoPath)
        {
            if (!File.Exists(videoPath)) return null;

            using (var videoEdit = new VideoEditCore())
            {
                try
                {
                    await Task.Delay(300);

                    var bitmap = videoEdit.Helpful_GetFrameFromFile(
                        videoPath,
                        TimeSpan.FromSeconds(1),
                        false,
                        VisioForge.Core.Types.MediaPlayer.MediaPlayerSourceMode.FFMPEG
                    );

                    return bitmap;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi khi lấy thumbnail: " + ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// Tạo thumbnail cho video và thêm vào UI
        /// </summary>
        private async Task CreateVideoThumbnailAsync(string videoPath)
        {
            try
            {
                Image thumbnail = await GetVideoThumbnailAsync(videoPath);
                if (thumbnail == null)
                {
                    Log.Warning($"Không thể tạo thumbnail cho video: {videoPath}");
                    return;
                }

                // Tạo panel chứa thumbnail
                Panel containerPanel = CreateThumbnailPanel(thumbnail, videoPath);

                // Thêm vào UI (nếu có panel container)
                // _fPLRoll?.Controls.Add(containerPanel);

                Log.Information($"Đã tạo thumbnail cho video: {videoPath}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Lỗi khi tạo thumbnail cho video: {videoPath}");
            }
        }

        /// <summary>
        /// Tạo panel chứa thumbnail
        /// </summary>
        private Panel CreateThumbnailPanel(Image thumbnail, string videoPath)
        {
            int containerSize = THUMBNAIL_IMAGE_SIZE + THUMBNAIL_BORDER_THICKNESS * 2;

            Panel containerPanel = new Panel
            {
                Size = new Size(containerSize, containerSize),
                BackColor = Color.Transparent,
                Margin = new Padding(THUMBNAIL_MARGIN),
                Padding = new Padding(THUMBNAIL_BORDER_THICKNESS),
                Tag = videoPath
            };

            PictureBox pictureBox = new PictureBox
            {
                Image = thumbnail,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Black,
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.None,
                Tag = videoPath,
                Size = new Size(THUMBNAIL_IMAGE_SIZE, THUMBNAIL_IMAGE_SIZE),
                Dock = DockStyle.Fill
            };

            pictureBox.Click += PictureBoxRoll_Click;
            containerPanel.Click += PictureBoxRoll_Click;

            containerPanel.Controls.Add(pictureBox);
            return containerPanel;
        }

        /// <summary>
        /// Cập nhật UI của nút Record
        /// </summary>
        private void UpdateRecordButtonUI(bool isRecording)
        {
            if (isRecording)
            {
                _btnRecord.Text = "Dừng";
            }
            else
            {
                _btnRecord.Text = "Ghi lại";
            }
        }

        /// <summary>
        /// Chụp ảnh từ camera
        /// </summary>
        private async Task TakeSnapshotAsync()
        {
            try
            {
                Log.Information($"Bắt đầu chụp ảnh cho folder: {_machidinh}");

                string imagePath = await _cameraControl.SnapshotAsync(_machidinh);
                if (string.IsNullOrEmpty(imagePath))
                {
                    Log.Warning("Đường dẫn ảnh chụp rỗng");
                    return;
                }

                // Đợi một chút để đảm bảo file đã được ghi xong
                await Task.Delay(SNAPSHOT_DELAY_MS);

                LoadRasterImage(imagePath);
                Log.Information($"Đã chụp và load ảnh: {imagePath}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi chụp ảnh");
                throw; // Re-throw để caller có thể xử lý
            }
        }

        /// <summary>
        /// Xử lý sự kiện Snapshot button
        /// </summary>
        private async void _btnSnapshot_Click(object sender, EventArgs e)
        {
            try
            {
                await TakeSnapshotAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi xử lý snapshot");
                XtraMessageBox.Show(this, $"Lỗi khi chụp ảnh: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        /// <summary>
        /// Xử lý sự kiện Stop button - Dừng camera
        /// </summary>
        private async void _btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                _btnStop.Enabled = false;
                await _cameraControl.StopCaptureAsync();
                Log.Information("Đã dừng camera");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi dừng camera");
                XtraMessageBox.Show(this, $"Lỗi khi dừng camera: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _btnStop.Enabled = true;
            }
        }


        /// <summary>
        /// Xử lý sự kiện Preview button - Xem trước camera
        /// </summary>
        private async void _btnLinkCamera_Click(object sender, EventArgs e)
        {
            try
            {
                _btnLinkCamera.Enabled = false;
                await _cameraControl.PreviewCaptureAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi xem trước camera");
                XtraMessageBox.Show(this, $"Lỗi khi xem trước: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _btnLinkCamera.Enabled = true;
            }
        }
        #endregion

        #region Print/Preview Operations

        /// <summary>
        /// Xử lý in kết quả
        /// </summary>
        private void _btnPrint_Click(object sender, EventArgs e)
        {
            // Fire and forget - không cần await
            _ = PrintCurrentAsync();
        }

        /// <summary>
        /// In kết quả hiện tại
        /// </summary>
        private async Task PrintCurrentAsync()
        {
            if (!ValidateBeforePrint())
                return;

            var parentForm = this.FindForm();
            bool isSplashVisible = false;

            try
            {
                ShowSplashScreen(parentForm, "Đang chuẩn bị in...", "Vui lòng chờ trong giây lát...");
                isSplashVisible = true;

                byte[] pdfBytes = await GetPdfBytesForPrintAsync();

                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    CloseSplashScreen(isSplashVisible);
                    ShowErrorMessage("Lỗi", "Không thể tạo file PDF để in.");
                    return;
                }

                CloseSplashScreen(isSplashVisible);
                isSplashVisible = false;

                // In PDF
                await PrintPdfAsync(pdfBytes);
            }
            catch (Exception ex)
            {
                CloseSplashScreen(isSplashVisible);
                Log.Error(ex, "Lỗi khi in PDF");
                ShowErrorMessage("Lỗi khi in PDF", ex.Message);
            }
            finally
            {
                CloseSplashScreen(isSplashVisible);
            }
        }

        /// <summary>
        /// In PDF
        /// </summary>
        private async Task PrintPdfAsync(byte[] pdfBytes)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var ms = new MemoryStream(pdfBytes))
                    using (var viewer = new PdfViewer())
                    {
                        viewer.LoadDocument(ms);

                        var settings = new PrinterSettings
                        {
                            PrinterName = _cbbPrinters.Text
                        };

                        viewer.Print(settings);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Lỗi khi in PDF");
                    throw;
                }
            });
        }

        /// <summary>
        /// Lấy PDF bytes để in (từ API hoặc generate)
        /// </summary>
        private async Task<byte[]> GetPdfBytesForPrintAsync()
        {
            // Nếu đã hoàn thành -> tải từ API
            if (_kqChanDoanResponse.TrangThai == TrangThaiKetLuan.HOAN_THANH)
            {
                return await ServiceLocator.RisService.TaiFileKetQuaChanDoanAsync(_machidinh);
            }

            // Nếu chưa hoàn thành -> generate từ template
            return await GeneratePdfFromTemplateAsync();
        }

        /// <summary>
        /// Validate trước khi in
        /// </summary>
        private bool ValidateBeforePrint()
        {
            if (string.IsNullOrWhiteSpace(_machidinh))
            {
                ShowWarningMessage("Thông báo", "Chưa chọn mã chỉ định!");
                return false;
            }

            if (_kqChanDoanResponse == null)
            {
                ShowWarningMessage("Thông báo", "Chưa có kết quả chẩn đoán để in!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(_cbbPrinters.Text))
            {
                ShowWarningMessage("Thông báo", "Chưa chọn máy in!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Xử lý preview PDF
        /// </summary>
        private async void _btnPreviewMain_Click(object sender, EventArgs e)
        {
            if (!ValidateBeforePreview())
                return;

            var parentForm = this.FindForm();
            bool isSplashVisible = false;

            try
            {
                ShowSplashScreen(parentForm, "Đang tải dữ liệu...", "Vui lòng chờ trong giây lát...");
                isSplashVisible = true;

                byte[] pdfBytes = await GetPdfBytesForPreviewAsync();

                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    CloseSplashScreen(isSplashVisible);
                    ShowErrorMessage("Lỗi", "Không thể tải hoặc tạo file PDF.");
                    return;
                }

                CloseSplashScreen(isSplashVisible);
                isSplashVisible = false;

                // Hiển thị PDF viewer
                await ShowPdfViewerAsync(pdfBytes);
            }
            catch (Exception ex)
            {
                CloseSplashScreen(isSplashVisible);
                Log.Error(ex, "Lỗi khi preview PDF");
                ShowErrorMessage("Lỗi", $"Lỗi khi mở PDF: {ex.Message}");
            }
            finally
            {
                CloseSplashScreen(isSplashVisible);
            }
        }

        /// <summary>
        /// Validate trước khi preview
        /// </summary>
        private bool ValidateBeforePreview()
        {
            if (string.IsNullOrWhiteSpace(_machidinh))
            {
                ShowWarningMessage("Thông báo", "Chưa chọn mã chỉ định!");
                return false;
            }

            if (_kqChanDoanResponse == null)
            {
                ShowWarningMessage("Thông báo", "Chưa có kết quả chẩn đoán!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Lấy PDF bytes để preview
        /// </summary>
        private async Task<byte[]> GetPdfBytesForPreviewAsync()
        {
            // Nếu đã hoàn thành -> tải từ API và lưu file
            if (_kqChanDoanResponse.TrangThai == TrangThaiKetLuan.HOAN_THANH)
            {
                var pdfBytes = await ServiceLocator.RisService.TaiFileKetQuaChanDoanAsync(_machidinh);

                if (pdfBytes != null && pdfBytes.Length > 0)
                {
                    await SaveSignedPdfToFileAsync(pdfBytes);
                }

                return pdfBytes;
            }

            // Nếu chưa hoàn thành -> generate từ template
            return await GeneratePdfFromTemplateAsync();
        }

        /// <summary>
        /// Lưu file PDF đã ký vào thư mục
        /// </summary>
        private async Task SaveSignedPdfToFileAsync(byte[] pdfBytes)
        {
            await Task.Run(() =>
            {
                try
                {
                    string saveFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "BenhNhan",
                        _machidinh,
                        "KQChanDoan"
                    );

                    if (!Directory.Exists(saveFolder))
                    {
                        Directory.CreateDirectory(saveFolder);
                    }

                    string filePath = Path.Combine(saveFolder, "KetquaSigned.pdf");
                    File.WriteAllBytes(filePath, pdfBytes);
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Không thể lưu file PDF đã ký");
                }
            });
        }

        /// <summary>
        /// Hiển thị PDF viewer
        /// </summary>
        private Task ShowPdfViewerAsync(byte[] pdfBytes)
        {
            var pdfViewer = new FormPdfViewer(pdfBytes);
            pdfViewer.ShowDialog();
            pdfViewer.Dispose();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Generate PDF từ template
        /// </summary>
        private async Task<byte[]> GeneratePdfFromTemplateAsync()
        {
            var templateId = GetSelectedTemplateId();
            if (string.IsNullOrEmpty(templateId))
            {
                throw new InvalidOperationException("Chưa chọn layout báo cáo.");
            }

            var template = await ServiceLocator.RisService.GetReportTemplateByIdAsync(templateId);
            if (template == null)
            {
                throw new InvalidOperationException("Không tìm thấy template báo cáo.");
            }

            var reportData = CreateBaoCaoKetLuan();
            return await ExportReportToPdfBytesAsync(template.XmlTemplate, reportData);
        }

        /// <summary>
        /// Lấy template ID đã chọn
        /// </summary>
        private string GetSelectedTemplateId()
        {
            return (_cbbLayout.SelectedItem as ReportTemplateGridViewModel)?.Id;
        }

        #endregion

        #region Signature Operations

        /// <summary>
        /// Xử lý sự kiện click nút Ký số
        /// </summary>
        private async void _btnSignature_Click(object sender, EventArgs e)
        {
            await SignaturePdfAsync();
        }

        /// <summary>
        /// Xử lý quy trình ký số PDF
        /// </summary>
        private async Task SignaturePdfAsync()
        {
            _btnSignature.Enabled = false;
            var parentForm = this.FindForm();
            bool isSplashVisible = false;

            try
            {
                // Validate dữ liệu
                if (!ValidateSignatureData())
                    return;

                ShowSplashScreen(parentForm, "Đang tải dữ liệu...", "Vui lòng chờ trong giây lát...");
                isSplashVisible = true;

                // Kiểm tra quyền ký số
                if (!IsAuthorizedToSign())
                {
                    CloseSplashScreen(isSplashVisible);
                    await HandleUnauthorizedUserAsync();
                    return;
                }

                // Xử lý theo trạng thái
                if (_kqChanDoanResponse.TrangThai == TrangThaiKetLuan.NHAP)
                {
                    CloseSplashScreen(isSplashVisible);
                    await HandleSignDraftAsync();
                }
                else
                {
                    CloseSplashScreen(isSplashVisible);
                    isSplashVisible = false;
                    await HandleAlreadySignedAsync(parentForm);
                }
            }
            catch (Exception ex)
            {
                CloseSplashScreen(isSplashVisible);
                Log.Error(ex, "Lỗi khi ký số");
                ShowErrorMessage("Lỗi", $"Lỗi khi ký số: {ex.Message}");
            }
            finally
            {
                CloseSplashScreen(isSplashVisible);
                _btnSignature.Enabled = true;
            }
        }

        /// <summary>
        /// Kiểm tra dữ liệu trước khi ký số
        /// </summary>
        private bool ValidateSignatureData()
        {
            if (_kqChanDoanResponse == null)
            {
                ShowWarningMessage("Thông báo", "Không có dữ liệu kết quả để ký.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Kiểm tra quyền ký số của user hiện tại
        /// </summary>
        private bool IsAuthorizedToSign()
        {
            return ServiceLocator.KeycloakUserInfo?.HISCode == _kqChanDoanResponse?.MaBacSiKetLuan;
        }

        /// <summary>
        /// Xử lý trường hợp kết quả đang là Nháp -> Ký số
        /// </summary>
        private async Task HandleSignDraftAsync()
        {
            try
            {
                var parentForm = this.FindForm();
                // Kiểm tra layout
                string templateId = (_cbbLayout.SelectedItem as ReportTemplateGridViewModel)?.Id;
                if (string.IsNullOrEmpty(templateId))
                {
                    XtraMessageBox.Show(
                        this,
                        "Vui lòng chọn mẫu báo cáo trước khi ký số.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                ShowSplashScreen(parentForm, "Đang tải dữ liệu...", "Vui lòng chờ trong giây lát...");

                // Lấy template và dữ liệu báo cáo
                var template = await ServiceLocator.RisService.GetReportTemplateByIdAsync(templateId);
                var reportData = CreateBaoCaoKetLuan();

                // Xuất PDF
                byte[] pdfBytes = await ExportReportToPdfBytesAsync(template.XmlTemplate, reportData);

                // Tạo request ký số
                var request = BuildSignRequest(pdfBytes);
                var signedResult = await ServiceLocator.SignatureService.SignHashPdf(request);

                if (!string.IsNullOrEmpty(signedResult))
                {
                    byte[] signedPdfBytes = Convert.FromBase64String(signedResult);

                    // Upload file đã ký
                    await ServiceLocator.RisService.UploadSignedFileAsync(_machidinh, "pdf", "", signedPdfBytes);

                    // Cập nhật UI
                    _rtKetLuan.Enabled = false;
                    _rtKhuyenNghi.Enabled = false;
                    _rtMoTa.Enabled = false;

                    _btnPrint.Enabled = true;
                    _btnPreviewMain.Enabled = true;
                    _btnSave.Enabled = false;
                    _btnSignature.Text = $"Hủy Ký số({ ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.Sign})";
                    _kqChanDoanResponse.TrangThai = "Hoàn thành";
                    CloseSplashScreen(true);
                    XtraMessageBox.Show(
                        this,
                        "Báo cáo đã được ký số thành công và lưu vào hệ thống.",
                        "Ký số thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    CloseSplashScreen(true);
                    XtraMessageBox.Show(
                        this,
                        "Quá trình ký số không thành công. Vui lòng thử lại.",
                        "Ký số thất bại",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
            catch (Exception ex)
            {
                CloseSplashScreen(true);
                XtraMessageBox.Show(
                    this,
                    $"Đã xảy ra lỗi trong quá trình ký số: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                CloseSplashScreen(true);
            }
        }

        public async Task<byte[]> ExportReportToPdfBytesAsync(string repxContent, object dataSource)
        {
            XtraReport report = null;

            if (!string.IsNullOrEmpty(repxContent))
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(repxContent)))
                {
                    report = XtraReport.FromStream(ms, true);
                }
            }
            else
            {
                report = new XtraReport();
            }

            report.DataSource = new[] { dataSource };

            using (var ms = new MemoryStream())
            {
                report.ExportToPdf(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Build SignhashRequest từ pdfBytes
        /// </summary>
        private SignhashRequest BuildSignRequest(byte[] pdfBytes)
        {
            var request = new SignhashRequest
            {
                FileName = "Ketqua.pdf",
                FileBase64 = Convert.ToBase64String(pdfBytes),
                UserID = ServiceLocator.KeycloakUserInfo.CCCD
            };

            using (PdfDocumentProcessor processor = new PdfDocumentProcessor())
            using (var ms = new MemoryStream(pdfBytes))
            {
                processor.LoadDocument(ms);
                var searchResults = processor.FindText("[Sig]");

                if (searchResults != null && searchResults.Rectangles.Count > 0)
                {
                    var rect = searchResults.Rectangles[0];
                    float imageWidth = 80;
                    float imageHeight = 40;

                    request.numberPageSign = searchResults.PageIndex + 1;
                    request.coorX = (float)rect.Left - imageWidth / 4;
                    request.coorY = (float)rect.Top - imageHeight / 2;
                    request.width = imageWidth;
                    request.height = imageHeight;
                }
            }

            return request;
        }

        /// <summary>
        /// Xử lý trường hợp đã ký số -> hỏi có muốn hủy ký không
        /// </summary>
        private async Task HandleAlreadySignedAsync(Form parentForm)
        {
            var dialogResult = XtraMessageBox.Show(
                this,
                "Kết quả đã được ký số. Bạn có muốn hủy ký số để tạo lại không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (dialogResult != DialogResult.Yes) return;

            // Nhập lý do hủy
            string lyDoHuy = XtraInputBox.Show("Nhập lý do hủy ký số:", "Lý do hủy", "");
            if (string.IsNullOrWhiteSpace(lyDoHuy))
            {
                XtraMessageBox.Show(this, "Bạn phải nhập lý do hủy ký số!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Gọi API hủy ký số
            bool huyOk = await ServiceLocator.RisService.HuyKetQuaChanDoanAsync(_machidinh, lyDoHuy);

            if (!huyOk)
            {
                XtraMessageBox.Show(this, "Hủy ký số thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            _rtKetLuan.Enabled = true;
            _rtKhuyenNghi.Enabled = true;
            _rtMoTa.Enabled = true;
            _btnSave.Enabled = true;
            _btnPrint.Enabled = true;
            _btnSignature.Text = $"Ký số ({ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.Sign})";
            _kqChanDoanResponse.TrangThai = TrangThaiKetLuan.NHAP;

        }


        /// <summary>
        /// Xử lý khi user không có quyền ký
        /// </summary>
        private async Task HandleUnauthorizedUserAsync()
        {

            SplashScreenManager.CloseForm(false);
            if (_kqChanDoanResponse.TrangThai != TrangThaiKetLuan.NHAP)
            {
                var msg = "Bạn không có quyền ký hoặc hủy kết quả này.\n" +
                          "Vui lòng đăng nhập bằng tài khoản bác sĩ đã kết luận để thực hiện.";
                XtraMessageBox.Show(this, msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                var msg = "Bạn không phải là bác sĩ kết luận ban đầu.\n" +
                          "Hệ thống sẽ lưu kết quả dưới dạng NHÁP với thông tin bác sĩ hiện tại.";
                XtraMessageBox.Show(this, msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }


        /// <summary>
        /// Tạo báo cáo kết luận từ dữ liệu hiện tại
        /// </summary>
        private ThongTinKetLuanBaoCaoThuong CreateBaoCaoKetLuan()
        {
            var ketluan = new ThongTinKetLuanBaoCaoThuong
            {
                TGBacSiChiDinh = FormatDateTime(_chiDinhDichVuResponse?.Thoigianthuchien ?? DateTime.Now),
                TGBacSiKetLuan = FormatDateTime(_dateTGKetThuc?.DateTime ?? DateTime.Now),
                MoTa = _kqChanDoanResponse?.Kqcls_MoTa,
                KetLuan = _kqChanDoanResponse?.Kqcls_KetLuan,
                KhuyenNghi = _kqChanDoanResponse?.Kqcls_DeNghi,
                ChiDinhDichVu = _chiDinhDichVuResponse,
                TenBacSiKetLuan = GetBacSiKetLuanName()
            };

            // Lấy danh sách ảnh đã được chọn
            var selectedImagePaths = GetSelectedImagePaths();

            // Gán ảnh vào các thuộc tính (tối đa 4 ảnh)
            AssignImagesToKetLuan(ketluan, selectedImagePaths);

            // Gán ảnh chữ ký nếu có
            ketluan.AnhChuKy = GetAnhChuKy();

            // Chuyển đổi giới tính
            ketluan.GioiTinhConvert = ConvertGioiTinh(ketluan.ChiDinhDichVu?.BenhNhan?.GioiTinh);

            return ketluan;
        }

        /// <summary>
        /// Lấy tên bác sĩ kết luận
        /// </summary>
        private string GetBacSiKetLuanName()
        {
            if (ServiceLocator.KeycloakUserInfo == null)
                return string.Empty;

            var firstName = ServiceLocator.KeycloakUserInfo.FirstName ?? string.Empty;
            var lastName = ServiceLocator.KeycloakUserInfo.LastName ?? string.Empty;

            return $"{firstName} {lastName}".Trim();
        }

        /// <summary>
        /// Lấy danh sách đường dẫn ảnh đã được chọn
        /// </summary>
        private List<string> GetSelectedImagePaths()
        {
            var imagePaths = new List<string>();

            if (_lstBoxPages?.ImageCollections == null)
                return imagePaths;

            foreach (var item in _lstBoxPages.ImageCollections)
            {
                try
                {
                    if (item?.Images == null || item.Images.Count == 0)
                        continue;

                    var image = item.Images[0];
                    if (image?.Checked == true && image.Parent?.Name != null)
                    {
                        string imagePath = image.Parent.Name;
                        if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                        {
                            imagePaths.Add(imagePath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Lỗi khi lấy đường dẫn ảnh từ ImageCollection");
                }
            }

            return imagePaths;
        }

        /// <summary>
        /// Gán ảnh vào các thuộc tính của kết luận (tối đa 4 ảnh)
        /// </summary>
        private void AssignImagesToKetLuan(ThongTinKetLuanBaoCaoThuong ketluan, List<string> imagePaths)
        {
            if (imagePaths == null || imagePaths.Count == 0)
                return;

            // Sử dụng array để tránh nhiều if-else
            var imageProperties = new[]
            {
                new { Index = 0, Property = new Action<string>(path => ketluan.Image1 = path) },
                new { Index = 1, Property = new Action<string>(path => ketluan.Image2 = path) },
                new { Index = 2, Property = new Action<string>(path => ketluan.Image3 = path) },
                new { Index = 3, Property = new Action<string>(path => ketluan.Image4 = path) }
            };

            int maxImages = Math.Min(imagePaths.Count, imageProperties.Length);

            for (int i = 0; i < maxImages; i++)
            {
                try
                {
                    string base64Image = ConvertImageToBase64(imagePaths[i]);
                    if (!string.IsNullOrEmpty(base64Image))
                    {
                        imageProperties[i].Property(base64Image);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Lỗi khi chuyển đổi ảnh sang Base64: {imagePaths[i]}");
                }
            }
        }

        /// <summary>
        /// Chuyển đổi ảnh thành Base64 string
        /// </summary>
        private string ConvertImageToBase64(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
            {
                Log.Warning($"File ảnh không tồn tại: {imagePath}");
                return null;
            }

            try
            {
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                return Convert.ToBase64String(imageBytes);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Lỗi khi đọc file ảnh: {imagePath}");
                return null;
            }
        }

        /// <summary>
        /// Lấy ảnh chữ ký nếu có
        /// </summary>
        private string GetAnhChuKy()
        {
            return _hisUserKySoResponse != null && !string.IsNullOrEmpty(_hisUserKySoResponse.AnhChuKy)
                ? _hisUserKySoResponse.AnhChuKy
                : null;
        }

        /// <summary>
        /// Chuyển đổi giới tính từ số sang chuỗi
        /// </summary>
        private string ConvertGioiTinh(int? gioiTinh)
        {
            return gioiTinh == 1 ? "Nữ" :
                gioiTinh == 0 ? "Nam" :
                "";
        }

        /// <summary>
        /// Format datetime thành chuỗi theo định dạng tiếng Việt
        /// </summary>
        private string FormatDateTime(DateTime dateTime)
        {
            return $"{dateTime.Hour} giờ {dateTime.Minute} phút, " +
                   $"ngày {dateTime.Day} tháng {dateTime.Month} năm {dateTime.Year}";
        }
        #endregion

        #region Image Operations - Zoom, Rotate, Scale

        /// <summary>
        /// Phóng to/thu nhỏ ảnh trong PictureBox với hệ số zoom được chỉ định
        /// </summary>
        /// <param name="dZoomFactor">Hệ số zoom (dương = zoom in, âm = zoom out)</param>
        /// <remarks>
        /// - Nếu đang ở chế độ FitAlways, sẽ tính toán scale factor hiện tại dựa trên kích thước ảnh và PictureBox
        /// - Giới hạn zoom: tối đa 3x (300%) và tối thiểu 0.06x (6%)
        /// - Sau khi zoom, chuyển sang chế độ Normal để cho phép scroll
        /// </remarks>
        private void ZoomPicture(double dZoomFactor)
        {
            try
            {
                double oldScaleFactor = _pictureBox.ScaleFactor;
                if (_pictureBox.SizeMode == RasterPaintSizeMode.FitAlways)
                {
                    double dWidthFraction = (double)(_pictureBox.Width - 30) / (double)_pictureBox.Image.Width;
                    double dHeightFraction = (double)(_pictureBox.Height - 30) / (double)_pictureBox.Image.Height;
                    double dScale = dWidthFraction;
                    if (dHeightFraction < dWidthFraction)
                    {
                        dScale = dHeightFraction;
                    }
                    _pictureBox.SizeMode = RasterPaintSizeMode.Normal;
                    oldScaleFactor = dScale;
                }


                oldScaleFactor = oldScaleFactor + dZoomFactor;
                if (oldScaleFactor > 3 && dZoomFactor > 0)
                    return;
                if (oldScaleFactor < .06 && dZoomFactor < 0)
                    return;
                _pictureBox.SizeMode = RasterPaintSizeMode.Normal;
                _pictureBox.ScaleFactor = oldScaleFactor;
            }
            catch { }
        }

        /// <summary>
        /// Tạo một ImageCollection mới từ RasterImage và thêm vào ListBox
        /// </summary>
        /// <param name="strTittle">Tiêu đề của collection (tên hiển thị)</param>
        /// <param name="rasterImage">RasterImage cần thêm vào collection</param>
        /// <remarks>
        /// - Lưu ảnh vào file tạm thời (TIF format)
        /// - Tạo Page object với DeleteOnDispose = true để tự động xóa file khi dispose
        /// - Dispose RasterImage gốc sau khi đã lưu và load lại
        /// - Thêm collection vào ListBox để hiển thị
        /// </remarks>
        private void CreateImageCollection(string strTittle, RasterImage rasterImage)
        {
            ListImageBox.ImageCollection imagecollection = new ListImageBox.ImageCollection(strTittle);
            Page page = new Page();
            string strTemp = null;
            strTemp = Path.GetTempFileName();
            _codec.Save(rasterImage, strTemp, RasterImageFormat.Tif, 0);
            page.FilePath = strTemp;
            page.DeleteOnDispose = true;
            imagecollection.Images.Add(new ListImageBox.ImageItem(_codec.Load(strTemp), imagecollection, page));
            rasterImage.Dispose();

            _lstBoxPages.AddImageCollection(imagecollection);
        }

        /// <summary>
        /// Xóa các item đã được chọn trong ListBox và file tương ứng trên disk
        /// </summary>
        /// <remarks>
        /// - Lưu vị trí scroll hiện tại để restore sau khi xóa
        /// - Duyệt từ cuối lên đầu để tránh lỗi index khi xóa
        /// - Xóa cả item trong ListBox và file trên disk
        /// - Restore lại vị trí scroll sau khi xóa (phải dùng Math.Abs vì AutoScrollPosition trả về giá trị âm)
        /// </remarks>
        private void DeleteSelectedItems()
        {
            // 1. Lưu vị trí scroll hiện tại (pixel)
            Point oldScrollPos = _lstBoxPages.AutoScrollPosition;

            // 2. Tạm dừng layout để tránh auto scroll
            _lstBoxPages.SuspendLayout();

            int firstSelectedIndex = -1;

            for (int i = _lstBoxPages.Items.Count - 1; i >= 0; i--)
            {
                var item = _lstBoxPages.Items[i];
                if (item.Selected)
                {
                    if (firstSelectedIndex == -1)
                        firstSelectedIndex = i;

                    _lstBoxPages.RemoveItem(i);
                    TryDeleteFile(item.ImageItem.Parent.Name);
                }
            }

            _lstBoxPages.ResumeLayout(true);

            if (_lstBoxPages.Items.Count == 0)
                return;

            // 3. Restore lại vị trí scroll (LƯU Ý: phải đảo dấu)
            _lstBoxPages.AutoScrollPosition = new Point(
                Math.Abs(oldScrollPos.X),
                Math.Abs(oldScrollPos.Y)
            );
        }

        /// <summary>
        /// Xóa file với xử lý lỗi đầy đủ
        /// </summary>
        /// <param name="filePath">Đường dẫn file cần xóa</param>
        /// <remarks>
        /// Xử lý các trường hợp lỗi:
        /// - File không tồn tại: Hiển thị cảnh báo
        /// - UnauthorizedAccessException: Không có quyền xóa (cần quyền Administrator)
        /// - IOException: File đang được sử dụng bởi ứng dụng khác
        /// - Exception khác: Lỗi không xác định
        /// </remarks>
        public static void TryDeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                else
                {
                    MessageBox.Show("File không tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Không có quyền xóa file này. Hãy chạy chương trình bằng quyền Administrator.", "Lỗi quyền truy cập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show($"File đang được sử dụng bởi ứng dụng khác.\nChi tiết: {ex.Message}", "Lỗi IO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa file: {ex.Message}", "Lỗi không xác định", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Delegate để thêm ImageCollection từ thread khác
        /// </summary>
        private delegate void AddImageCollectionThreadedDelegate(ListImageBox.ImageCollection collection);

        /// <summary>
        /// Thêm ImageCollection vào ListBox một cách thread-safe
        /// Tự động invoke về UI thread nếu được gọi từ thread khác
        /// </summary>
        /// <param name="collection">ImageCollection cần thêm vào ListBox</param>
        private void AddImageCollectionThreaded(ListImageBox.ImageCollection collection)
        {
            if (InvokeRequired)
            {
                Invoke(new AddImageCollectionThreadedDelegate(AddImageCollectionThreaded), collection);
            }
            else
            {
                _lstBoxPages.AddImageCollection(collection);
            }
        }

        /// <summary>
        /// Enable/Disable các controls trên form và hiển thị form operation nếu cần
        /// Thread-safe: Tự động invoke về UI thread nếu được gọi từ thread khác
        /// </summary>
        /// <param name="enable">true = enable controls, false = disable controls</param>
        /// <param name="strCaption">Caption hiển thị trên form operation (khi disable)</param>
        /// <param name="strBtnCaption">Text của nút Cancel trên form operation (nếu có)</param>
        /// <remarks>
        /// Khi enable = false:
        /// - Đổi cursor sang WaitCursor
        /// - Disable tất cả controls chính
        /// - Hiển thị FrmOperation với caption và button nếu được cung cấp
        /// 
        /// Khi enable = true:
        /// - Đổi cursor về Arrow
        /// - Enable lại tất cả controls
        /// - Cập nhật toolbar state
        /// - Đóng FrmOperation nếu đang mở
        /// </remarks>
        public void EnableItems(bool enable, string strCaption, string strBtnCaption)
        {
            if (this.InvokeRequired)
            {
                Invoke(new EnableMenu(EnableItems), new object[] { enable, strCaption, strBtnCaption });
            }
            else
            {
                if (enable)
                    Cursor.Current = Cursors.Arrow;
                else
                    Cursor.Current = Cursors.WaitCursor;

                _mmMain.Enabled = enable;
                _lstBoxPages.Enabled = enable;
                //_tbDicomInfo.Enabled = enable;
                _cmbSopClasses.Enabled = enable;
                //_cbStoreServers.Enabled = enable;
                toolStripComboBoxStoreServer.Enabled = enable;
                _btnPushToPACS.Enabled = enable;
                _toolbarMain.Enabled = enable;
                _pgDicomInfo.Enabled = enable;
                //_btnPACSSettings.Enabled = enable;
                //_btnOpenImage.Enabled = enable;
                if (enable)
                {
                    UpdateToolBarState();
                    if (_frmOperation != null)
                        _frmOperation.Close();
                }
                else
                {
                    if (!(strCaption == "" && strBtnCaption == ""))
                        if (_frmOperation == null || !_frmOperation.Visible)
                        {
                            _frmOperation = new FrmOperation(strCaption, strBtnCaption);
                            bCancelOperation = false;
                            if (strBtnCaption != "")
                                _frmOperation.Cancel += new EventHandler(_frmOperation_Cancel);
                            _frmOperation.Show();
                        }
                }
            }
        }

        private void ScalePicture(STM.MediaToPACS.Main.UI.ListImageBox.ImageItem item)
        {
            if (_pictureBox.Image != null)
            {
                _pictureBox.Image.Dispose();
                _pictureBox.Image = null;
            }
            _pictureBox.Image = item.Image.Clone();

            //_pictureBox_DoubleClick(null, null);
            _pictureBox.SizeMode = RasterPaintSizeMode.FitAlways;
            _pictureBox.ScaleFactor = 1;

        }

        private void InitClass()
        {
            if (RasterSupport.IsLocked(RasterSupportType.PrintDriver) && RasterSupport.IsLocked(RasterSupportType.PrintDriverServer))
            {
                throw new Exception("Printer driver capability is required.");
            }
        }

        private void ClearList()
        {
            try
            {
                DeleteTempFiles();
                _lstBoxPages.ClearList();
                if (_pictureBox.Image != null)
                {
                    _pictureBox.Image.Dispose();
                    _pictureBox.Image = null;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void DeleteTempFiles()
        {
            foreach (ListImageBox.ListItem item in _lstBoxPages.Items)
            {
                try
                {
                    item.Dispose();
                    TryDeleteFile(item.ImageItem.Parent.Name);
                }
                catch
                {
                }
            }
        }

        public void LogError(string sLogText)
        {
            LogText("*** ERROR *** ", _sNewlineTab + sLogText, Color.Red);
        }

        public void LogText(string action, string logText)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new AddLog(LogText),
                   new object[] { action, logText });
            }
            else
            {
                PacsSettings.LogWindow.RichTextBox.AppendText(logText);
                PacsSettings.LogWindow.RichTextBox.AppendText("\r\n");
                PacsSettings.LogWindow.RichTextBox.ScrollToCaret();
            }
        }

        public void LogText(string sAction, string sLogText, Color sActionColor)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new AddLogColor(LogText), new object[] { sAction, sLogText, sActionColor });
            }
            else
            {
                //AddAction(sAction, sActionColor);
                PacsSettings.LogWindow.RichTextBox.AppendText(sLogText);
                PacsSettings.LogWindow.RichTextBox.AppendText(_sNewline);
                TextBoxTraceListener.SendMessage(PacsSettings.LogWindow.RichTextBox.Handle, TextBoxTraceListener.WM_VSCROLL, TextBoxTraceListener.SB_BOTTOM, 0);
            }
        }
        
        public delegate void StartUpdateDelegate(DataGridView lv);
        private void StartUpdate(DataGridView dg)
        {
            if (InvokeRequired)
            {
                Invoke(new StartUpdateDelegate(StartUpdate), dg);
            }
            else
            {
                dg.Rows.Clear();
            }
        }
        #endregion

        #region Dicom Methods

        private void SetElements(DicomDataSet dicomDestination, DicomElement[] elements, DicomDataSet dicomSource)
        {
            foreach (DicomElement item in elements)
            {
                if (item.Length == 0)
                    continue;

                DicomElement element;
                element = dicomDestination.FindFirstElement(null, item.Tag, true);
                if (element == null)
                    element = dicomDestination.InsertElement(null, false, item.Tag, item.VR, false, 0);
                switch (item.VR)
                {
                    case DicomVRType.DA:
                        dicomDestination.SetDateValue(element, dicomSource.GetDateValue(item, 0, 1));
                        break;
                    case DicomVRType.TM:
                        dicomDestination.SetTimeValue(element, dicomSource.GetTimeValue(item, 0, 1));
                        break;
                    default:
                        {
                            byte[] ba = dicomSource.GetBinaryValue(item, (int)item.Length);
                            dicomDestination.FreeElementValue(element);
                            bool ret = dicomDestination.SetBinaryValue(element, ba, (int)ba.Length);
                        }
                        break;
                }
            }
            _pgDicomInfo.DataSet = dicomDestination;
        }

        private List<string> SaveDicom(DicomDataSet dicom, string strSaveFile)
        {
            try
            {
                byte[] value = new byte[] { 0x00, 0x01 };
                dicom.InsertElementAndSetValue(DicomTag.FileMetaInformationVersion, value);
                dicom.InsertElementAndSetValue(DicomTag.MediaStorageSOPClassUID, dicom.GetValue<string>(DicomTag.SOPClassUID, string.Empty));
                dicom.InsertElementAndSetValue(DicomTag.MediaStorageSOPInstanceUID, dicom.GetValue<string>(DicomTag.SOPInstanceUID, string.Empty));
                dicom.InsertElementAndSetValue(DicomTag.ImplementationClassUID, "1.2.840.114257.1123456");
                dicom.InsertElementAndSetValue(DicomTag.ImplementationVersionName, "STM.MEDIA");

                List<string> saved = new List<string>();

                int bit = 0;
                if (ClassTypes[_cmbSopClasses.SelectedIndex] == DicomClassType.EncapsulatedPdfStorage)
                {
                    DocumentFormat documentFormat = DocumentFormat.User;
                    DocumentOptions documentOptions = null;
                    PdfDocumentOptions PdfdocumentOptions = new PdfDocumentOptions();
                    string fileName;
                    fileName = Path.GetTempFileName();
                    documentFormat = DocumentFormat.Pdf;
                    documentOptions = new PdfDocumentOptions();
                    (documentOptions as PdfDocumentOptions).DocumentType = PdfDocumentType.Pdf;
                    (documentOptions as PdfDocumentOptions).FontEmbedMode = DocumentFontEmbedMode.Auto;
                    documentOptions.PageRestriction = DocumentPageRestriction.Relaxed;
                    DocumentWriter documentWriter = new DocumentWriter();
                    documentWriter.SetOptions(documentFormat, documentOptions);
                    documentWriter.BeginDocument(fileName, documentFormat);

                    foreach (ListImageBox.ListItem item in _lstBoxPages.CheckedItems)
                    {
#if LEADTOOLS_V19_OR_LATER
                        DocumentEmfPage documentPage = new DocumentEmfPage();
#else
                                      DocumentPage documentPage = DocumentPage.Empty;
#endif // #if LEADTOOLS_V19_OR_LATER
                        if (item.ImageItem.Tag.GetType() == typeof(PrintPage))
                            documentPage.EmfHandle = (item.ImageItem.Tag as PrintPage).MetaFile;
                        else
                        {
                            RasterImage rI = _codec.Load((item.ImageItem.Tag as IPrintToPACSFile).FileLocation());
                            documentPage.EmfHandle = Leadtools.Drawing.RasterImageConverter.ChangeToEmf(rI);
                            rI.Dispose();
                        }

                        documentWriter.AddPage(documentPage);
                        Application.DoEvents();
                        if (bCancelOperation)
                            break;
                    }

                    documentWriter.EndDocument();
                    SetIncapsualtedDoc(dicom, fileName);
                    File.Delete(fileName);
                    saved.Add(strSaveFile);
                    dicom.Save(strSaveFile, DicomDataSetSaveFlags.ExplicitVR | DicomDataSetSaveFlags.MetaHeaderPresent);

                    //Delete Element
                    ClearTag(dicom, DicomTag.EncapsulatedDocument);
                    ClearTag(dicom, DicomTag.HL7InstanceIdentifier);
                    ClearTag(dicom, DicomTag.ListOfMIMETypes);
                    ClearTag(dicom, DicomTag.VerificationFlag);

                    DicomElement dElement = _pgDicomInfo.DataSet.FindFirstElement(null, DicomTag.MIMETypeOfEncapsulatedDocument, false);
                    if (dElement != null)
                        _pgDicomInfo.DataSet.SetValue(dElement, "PDF");
                }

                if (ClassTypes[_cmbSopClasses.SelectedIndex] == DicomClassType.SCImageStorage)
                {
                    //Pixel Data
                    int i = 0;
                    foreach (ListImageBox.ListItem item in _lstBoxPages.CheckedItems)
                    {
                        i++;

                        DicomElement dInstance = dicom.FindFirstElement(null, DicomTag.InstanceNumber, true);
                        if (dInstance == null)
                            dInstance = dicom.InsertElement(null, false, DicomTag.InstanceNumber, DicomVRType.OW, false, 0);
                        dicom.SetValue(dInstance, i);

                        DicomElement dPixel = dicom.FindFirstElement(null, DicomTag.PixelData, true);
                        if (dPixel == null)
                        {
                            dPixel = dicom.InsertElement(null, false, DicomTag.PixelData, DicomVRType.OW, false, 0);
                        }
                        else
                        {
                            dicom.DeleteElement(dPixel);
                            dPixel = dicom.InsertElement(null, false, DicomTag.PixelData, DicomVRType.OW, false, 0);
                        }

                        RasterImage rI = null;
                        if (rI == null)
                            rI = _codec.Load((item.ImageItem.Tag as IPrintToPACSFile).FileLocation());

                        DicomImagePhotometricInterpretationType imagePhotoMetric = DicomImagePhotometricInterpretationType.Rgb;
                        if (rI.IsGray)
                        {
                            bit = 8;
                            imagePhotoMetric = DicomImagePhotometricInterpretationType.Monochrome2;
                            if (rI.BitsPerPixel == 12 || rI.BitsPerPixel == 16)
                            {
                                GrayscaleCommand grayCommand = new GrayscaleCommand(bit);
                                grayCommand.Run(rI);
                            }
                        }
                        else
                        {
                            bit = 24;
                            ColorResolutionCommand colorRes = new ColorResolutionCommand();
                            colorRes.BitsPerPixel = bit;
                            colorRes.Order = RasterByteOrder.Rgb;
                            colorRes.Mode = ColorResolutionCommandMode.InPlace;
                            colorRes.Run(rI);
                        }

                        dicom.SetImage(dPixel,
                                          rI,
                                          _mySettings._settings.secondaryCaptureCompression,
                                          imagePhotoMetric,
                                          bit,
                                          2,
                                          DicomSetImageFlags.AutoSetVoiLut);
                        rI.Dispose();

                        GenerateUidTag(dicom, DicomTag.SOPInstanceUID);

                        string strFile = Path.GetDirectoryName(strSaveFile) + "\\" + Path.GetFileNameWithoutExtension(strSaveFile) + "_" + i + Path.GetExtension(strSaveFile);
                        saved.Add(strFile);
                        dicom.Save(strFile, DicomDataSetSaveFlags.ExplicitVR | DicomDataSetSaveFlags.MetaHeaderPresent);
                        Application.DoEvents();
                        if (bCancelOperation)
                            break;
                    }
                    ClearTag(dicom, DicomTag.PixelData);
                    ClearTag(dicom, DicomTag.WindowCenter);
                    ClearTag(dicom, DicomTag.WindowWidth);
                    DicomElement dInstElement = dicom.FindFirstElement(null, DicomTag.InstanceNumber, true);
                    if (dInstElement == null)
                        dInstElement = dicom.InsertElement(null, false, DicomTag.InstanceNumber, DicomVRType.OW, false, 0);
                    dicom.SetValue(dInstElement, "1");

                }

                if (ClassTypes[_cmbSopClasses.SelectedIndex] == DicomClassType.SCMultiFrameTrueColorImageStorage ||
                   ClassTypes[_cmbSopClasses.SelectedIndex] == DicomClassType.SCMultiFrameGrayscaleByteImageStorage)
                {

                    //Pixel Data
                    DicomElement dPixel = dicom.FindFirstElement(null, DicomTag.PixelData, true);
                    if (dPixel == null)
                        dPixel = dicom.InsertElement(null, false, DicomTag.PixelData, DicomVRType.OW, false, 0);

                    DicomElement dPageVector = dicom.FindFirstElement(null, DicomTag.PageNumberVector, true);

                    RasterImage rI = null;

                    int i = 1;
                    List<int> intArray = new List<int>();

                    DicomImageCompressionType compression = DicomImageCompressionType.None;
                    DicomImagePhotometricInterpretationType imagephotemetric = DicomImagePhotometricInterpretationType.Rgb;
                    ColorResolutionCommand colorRes = new ColorResolutionCommand();
                    if (ClassTypes[_cmbSopClasses.SelectedIndex] == DicomClassType.SCMultiFrameTrueColorImageStorage)
                    {
                        compression = _mySettings._settings.secondaryCaptureColorCompression;
                        imagephotemetric = DicomImagePhotometricInterpretationType.Rgb;
                        bit = 24;
                        colorRes.BitsPerPixel = bit;
                        colorRes.Order = RasterByteOrder.Bgr;
                        colorRes.Mode = ColorResolutionCommandMode.InPlace;
                    }
                    else
                    {
                        compression = _mySettings._settings.secondaryCaptureGrayCompression;
                        imagephotemetric = DicomImagePhotometricInterpretationType.Monochrome2;
                        bit = 8;
                        colorRes.BitsPerPixel = bit;
                        colorRes.Order = RasterByteOrder.Gray;
                        colorRes.Mode = ColorResolutionCommandMode.InPlace;
                    }
                    foreach (ListImageBox.ListItem item in _lstBoxPages.CheckedItems)
                    {
                        intArray.Add(i);
                        i++;
                        if (rI == null)
                        {
                            rI = _codec.Load((item.ImageItem.Tag as IPrintToPACSFile).FileLocation());
                            colorRes.Run(rI);
                            continue;
                        }
                        RasterImage rasterimage = _codec.Load((item.ImageItem.Tag as IPrintToPACSFile).FileLocation());
                        colorRes.Run(rasterimage);
                        rI.AddPage(rasterimage);
                        Application.DoEvents();
                        if (bCancelOperation)
                            break;
                    }

                    RasterImage rImg = null;
                    rI.Page = 1;
                    int iMaxWidth = rI.Width, iMaxHeight = rI.Height;
                    int iPage;
                    for (iPage = 1; iPage <= rI.PageCount; iPage++)
                    {
                        rI.Page = iPage;
                        rImg = rI;
                        if (rImg.Width > iMaxWidth)
                        {
                            iMaxWidth = rImg.Width;
                        }

                        if (rImg.Height > iMaxHeight)
                        {
                            iMaxHeight = rImg.Height;
                        }
                    }

                    RasterImage rImgNew = null;
                    List<RasterImage> lstRaster = new List<RasterImage>();
                    for (iPage = 1; iPage <= rI.PageCount; iPage++)
                    {
                        rI.Page = iPage;
                        rImg = rI;
                        if (rImg.ImageSize.Width < iMaxWidth || rImg.ImageSize.Height < iMaxHeight)
                        {
                            rImgNew = new RasterImage(RasterMemoryFlags.Conventional, iMaxWidth, iMaxHeight, bit, RasterByteOrder.Bgr, rImg.ViewPerspective, rImg.GetPalette(), IntPtr.Zero, 0);
                            FillCommand fillCommand = new FillCommand();
                            fillCommand.Color = RasterColorConverter.FromColor(Color.White);
                            fillCommand.Run(rImgNew);
                            CombineCommand combine = new CombineCommand();
                            int xStart, yStart;
                            xStart = Math.Abs(rImgNew.Width - rImg.Width) / 2;
                            yStart = Math.Abs(rImgNew.Height - rImg.Height) / 2;
                            combine.DestinationRectangle = new LeadRect(xStart, yStart, rImg.Width, rImg.Height);
                            combine.SourcePoint = new LeadPoint(0, 0);
                            combine.SourceImage = rImg;
                            combine.Flags = CombineCommandFlags.OperationAdd | CombineCommandFlags.Destination0;
                            combine.Run(rImgNew);
                            lstRaster.Add(rImgNew.Clone());
                        }
                        else
                        {
                            lstRaster.Add(rImg.Clone());
                        }
                    }
                    rI.Dispose();
                    rI = null;
                    foreach (RasterImage rasterimage in lstRaster)
                    {
                        if (rI == null)
                            rI = rasterimage;
                        else
                            rI.InsertPage(rI.PageCount + 1, rasterimage);
                    }

                    saved.Add(strSaveFile);
                    dicom.SetIntValue(dPageVector, intArray.ToArray(), intArray.Count);
                    dicom.SetImages(dPixel,
                          rI,
                          compression,
                          imagephotemetric,
                          bit,
                          2,
                          DicomSetImageFlags.AutoSetVoiLut);
                    dicom.Save(strSaveFile, DicomDataSetSaveFlags.ExplicitVR | DicomDataSetSaveFlags.MetaHeaderPresent);
                    rI.Dispose();
                    //Delete Element
                    ClearTag(dicom, DicomTag.PixelData);
                    ClearTag(dicom, DicomTag.WindowCenter);
                    ClearTag(dicom, DicomTag.WindowWidth);
                    ClearTag(dicom, DicomTag.RescaleIntercept);
                    ClearTag(dicom, DicomTag.RescaleSlope);
                    ClearTag(dicom, DicomTag.RescaleType);
                    ClearTag(dicom, DicomTag.PageNumberVector);
                }
                GenerateUidTag(dicom, DicomTag.SeriesInstanceUID);
                GenerateUidTag(dicom, DicomTag.SOPInstanceUID);
                _pgDicomInfo.DataSet = dicom;
                return saved;
            }
            finally
            {
                ClearTag(dicom, DicomTag.FileMetaInformationVersion);
                ClearTag(dicom, DicomTag.MediaStorageSOPClassUID);
                ClearTag(dicom, DicomTag.MediaStorageSOPInstanceUID);
                ClearTag(dicom, DicomTag.ImplementationClassUID);
                ClearTag(dicom, DicomTag.ImplementationVersionName);
            }
        }

        private void ClearTag(DicomDataSet dicom, long tag)
        {
            DicomElement dElement = dicom.FindFirstElement(null, tag, true);
            if (dElement != null)
                dicom.DeleteElement(dElement);
        }

        void SetIncapsualtedDoc(DicomDataSet ds, string sFileDocumentIn)
        {
            DicomElement dElement;
            string strDocumentTitle = "", strBurnedInAnnotation = "", strVerificationFlag = "", strInstanceNumber = "",
                   strCodeSchemeDesignator = "", strCodeValue = "", strCodeMeaning = "";
            DicomTimeValue contentTime = new DicomTimeValue();
            DicomDateValue contentDate = new DicomDateValue();
            DicomDateTimeValue acquistationTime = new DicomDateTimeValue();

            dElement = ds.FindFirstElement(null, DicomTag.InstanceNumber, true);
            if (dElement != null && dElement.Length != 0)
                strInstanceNumber = ds.GetValue<string>(dElement, "");

            dElement = ds.FindFirstElement(null, DicomTag.AcquisitionDateTime, true);
            if (dElement != null && dElement.Length != 0)
                acquistationTime = ds.GetDateTimeValue(dElement, 0, 1)[0];

            dElement = ds.FindFirstElement(null, DicomTag.DocumentTitle, true);
            if (dElement != null && dElement.Length != 0)
                strDocumentTitle = ds.GetStringValue(dElement, 0);

            dElement = ds.FindFirstElement(null, DicomTag.ContentTime, true);
            if (dElement != null && dElement.Length != 0)
                contentTime = ds.GetTimeValue(dElement, 0, 1)[0];

            dElement = ds.FindFirstElement(null, DicomTag.ContentDate, true);
            if (dElement != null && dElement.Length != 0)
                contentDate = ds.GetDateValue(dElement, 0, 1)[0];

            dElement = ds.FindFirstElement(null, DicomTag.BurnedInAnnotation, true);
            if (dElement != null && dElement.Length != 0)
                strBurnedInAnnotation = ds.GetStringValue(dElement, 0);

            dElement = ds.FindFirstElement(null, DicomTag.VerificationFlag, true);
            if (dElement != null && dElement.Length != 0)
                strVerificationFlag = ds.GetStringValue(dElement, 0);

            DicomElement dElementCNS = ds.FindFirstElement(null, DicomTag.ConceptNameCodeSequence, true);
            if (dElementCNS != null && dElementCNS.Length != 0)
                strCodeMeaning = ds.GetStringValue(dElement, 0);

            dElement = ds.FindFirstElement(dElementCNS, DicomTag.CodeMeaning, false);
            if (dElement != null && dElement.Length != 0)
                strCodeMeaning = ds.GetStringValue(dElement, 0);

            dElement = ds.FindFirstElement(dElementCNS, DicomTag.CodeValue, false);
            if (dElement != null && dElement.Length != 0)
                strCodeValue = ds.GetValue<string>(dElement, "");

            dElement = ds.FindFirstElement(dElementCNS, DicomTag.CodingSchemeDesignator, false);
            if (dElement != null && dElement.Length != 0)
                strCodeSchemeDesignator = ds.GetStringValue(dElement, 0);

            dElement = ds.FindFirstElement(null, DicomTag.EncapsulatedDocument, true);
            if (dElement == null)
                dElement = ds.InsertElement(null, false, DicomTag.EncapsulatedDocument, DicomVRType.UN, false, 0);

            bool child = false;
            DicomEncapsulatedDocument encapsulatedDocument = new DicomEncapsulatedDocument();
            encapsulatedDocument.Type = DicomEncapsulatedDocumentType.Pdf;
            encapsulatedDocument.InstanceNumber = int.Parse(strInstanceNumber);
            encapsulatedDocument.ContentDate = contentDate;

            encapsulatedDocument.ContentTime = contentTime;

            encapsulatedDocument.AcquisitionDateTime = acquistationTime;

            encapsulatedDocument.BurnedInAnnotation = strBurnedInAnnotation;
            encapsulatedDocument.DocumentTitle = strDocumentTitle;
            encapsulatedDocument.VerificationFlag = strVerificationFlag;
            encapsulatedDocument.HL7InstanceIdentifier = string.Empty;


            string[] sListOfMimeTypes = new string[] { "image/jpeg", "application/pdf" };
            encapsulatedDocument.SetListOfMimeTypes(sListOfMimeTypes);

            DicomCodeSequenceItem conceptNameCodeSequence = new DicomCodeSequenceItem();
            conceptNameCodeSequence.CodingSchemeDesignator = strCodeSchemeDesignator;
            conceptNameCodeSequence.CodeValue = strCodeValue;
            conceptNameCodeSequence.CodeMeaning = strCodeMeaning;

            ds.SetEncapsulatedDocument(dElement, child, sFileDocumentIn, encapsulatedDocument, conceptNameCodeSequence);
        }

        private void ResetModule(DicomModuleType moduleType, DicomDataSet dataset, bool bKeepOriginalElements)
        {
            if (bKeepOriginalElements)
            {
                DicomModule module = dataset.FindModule(moduleType);
                if (module == null)
                    return;

                byte[] b = new byte[1] { 0 };
                foreach (DicomElement item in module.Elements)
                {
                    if (item.Length == 0)
                        continue;

                    DicomElement element = dataset.FindFirstElement(null, item.Tag, true);
                    if (element != null)
                    {
                        dataset.SetBinaryValue(element, b, 0);
                    }
                }
            }
            else
            {
                dataset.DeleteModule(moduleType);
                dataset.InsertModule(moduleType, false);
            }
        }
       
        private string DoSave(DicomDataSet dicom, ref List<string> lstSaved, string strSaveLocation, ref bool bSuccess)
        {
            string strMessage = "";
            if (strMessage == "")
                try
                {
                    lstSaved = SaveDicom(dicom, strSaveLocation);
                    strMessage = "DICOM file was saved successfully\n";
                    bSuccess = lstSaved.Count > 0;
                    if (lstSaved.Count > 0)
                    {
                        foreach (string str in lstSaved)
                        {
                            strMessage += "--> " + str + "\n";
                        }
                    }
                }
                catch (Exception ex)
                {
                    strMessage = "DICOM file was not saved successfully, Reason:\n" + ex.Message;
                }
            return strMessage;
        }

        private void GetRequiredTags(DicomDataSet dicom, List<string> lstRequired)
        {
            DicomIod iod;
            DicomIodTable iodTable = DicomIodTable.Instance;
            DicomEditableObject editable = (DicomEditableObject)_pgDicomInfo.SelectedObject;
            DicomModule module;
            DicomIod IODClass = DicomIodTable.Instance.FindClass(dicom.InformationClass);
            for (int i = 0; i < dicom.ModuleCount; i++)
            {
                module = dicom.FindModuleByIndex(i);
                for (int j = 0; j < module.Elements.Length; j++)
                {
                    DicomElement dElement = module.Elements[j];
                    if (dElement.Length > 0)
                        continue;

                    iod = DicomIodTable.Instance.Find(IODClass, dElement.Tag, DicomIodType.Element, false);
                    if (!((iod != null) && (iod.Usage == DicomIodUsageType.Type1MandatoryElement) && (dElement.Length == 0) && (dElement.Length != ELEMENT_LENGTH_MAX)))
                        continue;

                    if (!lstRequired.Contains(iod.Name))
                        lstRequired.Add(iod.Name);

                }
            }
        }

        private void GenerateUidTag(DicomDataSet dicom, long UidTag)
        {
            DicomElement element;
            element = dicom.FindFirstElement(null, UidTag, true);
            if (element != null)
                dicom.SetValue(element, Utils.GenerateDicomUniqueIdentifier());

            _pgDicomInfo.DataSet = dicom;
        }

        #endregion

        #region StoreScu
        private void DoStore(string dsFile, MyServer storeserver)
        {
            string sMsg = string.Empty;
            DicomScp server = new DicomScp();
            server.AETitle = storeserver._sAE;
            server.PeerAddress = IPAddress.Parse(storeserver._sIP);
            server.Port = storeserver._port;
            server.Timeout = storeserver._timeout;
            MyServer s = null;
            s = (MyServer)(toolStripComboBoxStoreServer.SelectedItem);
            bStored = false;

            CreateCStoreObject(s);
            _cstore.AETitle = _mySettings._settings.clientAE;
            _cstore.HostPort = _mySettings._settings.clientPort;

            Thread t = new Thread(delegate ()
            {
                try
                {
                    _cstore.Store(server, dsFile);
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                    ShowErrorMessage(ex);
                }
            });

            t.Start();
            while (t.IsAlive)
            {
                Application.DoEvents();
            }
        }

        public delegate void ShowErrorMessageDelegate(Exception ex);

        private void ShowErrorMessage(Exception ex)
        {
            if (InvokeRequired)
            {
                Invoke(new ShowErrorMessageDelegate(ShowErrorMessage), ex);
            }
            else
            {
                EnableItems(true, "", "");
                bool bTopMost = logWindow.TopMost;
                logWindow.TopMost = false;
                MessageBox.Show(this, "Error Occurred: \n" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                logWindow.TopMost = bTopMost;
            }

        }

        void _cstore_BeforeConnect(object sender, Leadtools.Dicom.Scu.Common.BeforeConnectEventArgs e)
        {
            LogText("Before Connect", e.Scp.ToString());
            e.PrivateKeyPassword = _mySettings._settings.privateKeyPassword;
        }

        void _cstore_AfterConnect(object sender, Leadtools.Dicom.Scu.Common.AfterConnectEventArgs e)
        {
            string message;
            if (e.Error == DicomExceptionCode.Success)
            {
                message = _sNewlineTab + "Connection Successful";
            }
            else
            {
                message =
                   _sNewlineTab + "Connection Failed" +
                   _sNewlineTab + "Error:\t" + e.Error.ToString();
            }

            LogText("After Connect", message);
        }

        void _cstore_AfterSecureLinkReady(object sender, Leadtools.Dicom.Scu.Common.AfterSecureLinkReadyEventArgs e)
        {
            string message;
            if (e.Error == DicomExceptionCode.Success)
            {
                message = _sNewlineTab + "Secure Link Ready";
            }
            else
            {
                message =
                   _sNewlineTab + "Secure Link Failed" +
                   _sNewlineTab + "Error:\t" + e.Error.ToString();
            }

            LogText("After Secure Link Ready", message);
        }

        void _cstore_BeforeAssociateRequest(object sender, Leadtools.Dicom.Scu.Common.BeforeAssociateRequestEventArgs e)
        {
            LogText("Before Associate Request", e.Associate.ToString());
        }

        void _cstore_AfterAssociateRequest(object sender, Leadtools.Dicom.Scu.Common.AfterAssociateRequestEventArgs e)
        {
            string message;
            if (e.Rejected)
            {
                message =
                   _sNewlineTab + "Association Rejected" +
                   _sNewlineTab + "Result: " + e.Result.ToString() +
                   _sNewlineTab + "Reason: " + e.Reason.ToString() +
                   _sNewlineTab + "Source: " + e.Source.ToString();
            }
            else
            {
                message = _sNewlineTab + "Association Accepted" + e.Associate.ToString();
            }
            LogText("After Associate Request", message);
        }

        void _cstore_BeforeCStore(object sender, Leadtools.Dicom.Scu.Common.BeforeCStoreEventArgs e)
        {
            LogText("Before CStore", _sNewlineTab + "Current DataSet");
        }

        void _cstore_AfterCStore(object sender, Leadtools.Dicom.Scu.Common.AfterCStoreEventArgs e)
        {
            string message;
            if (e.Status == DicomCommandStatusType.Success)
            {
                message =
                   _sNewlineTab + "Success" +
                   _sNewlineTab + "Current DataSet";
                bStored = true;
            }
            else
            {
                message =
                   _sNewlineTab + "CStore Failed" +
                   _sNewlineTab + "Status: " + e.Status.ToString();
            }
            LogText("After CStore", message);
        }



        void CreateCStoreObject(MyServer server)
        {
            if (_cstore != null)
            {
                _cstore.Dispose();
            }

            if (server._useTls)
            {
                _cstore = new StoreScu(string.Empty, DicomNetSecurityeMode.Tls, null);
            }
            else
            {
                _cstore = new StoreScu();
            }

            _cstore.ImplementationClass = _sConfigurationImplementationClass;
            _cstore.ImplementationVersionName = _sConfigurationImplementationVersionName;
            _cstore.ProtocolVersion = _sConfigurationProtocolversion;

            // Subscribe to events for logging
            _cstore.BeforeConnect += new Leadtools.Dicom.Scu.Common.BeforeConnectDelegate(_cstore_BeforeConnect);
            _cstore.AfterConnect += new Leadtools.Dicom.Scu.Common.AfterConnectDelegate(_cstore_AfterConnect);
            _cstore.AfterSecureLinkReady += new Leadtools.Dicom.Scu.Common.AfterSecureLinkReadyDelegate(_cstore_AfterSecureLinkReady);
            _cstore.BeforeAssociateRequest += new Leadtools.Dicom.Scu.Common.BeforeAssociationRequestDelegate(_cstore_BeforeAssociateRequest);
            _cstore.AfterAssociateRequest += new Leadtools.Dicom.Scu.Common.AfterAssociateRequestDelegate(_cstore_AfterAssociateRequest);
            _cstore.BeforeCStore += new Leadtools.Dicom.Scu.Common.BeforeCStoreDelegate(_cstore_BeforeCStore);
            _cstore.AfterCStore += new Leadtools.Dicom.Scu.Common.AfterCStoreDelegate(_cstore_AfterCStore);

            _cstore.PrivateKeyPassword += new PrivateKeyPasswordDelegate(_cstore_PrivateKeyPassword);
            if (server._useTls)
            {
                try
                {
                    _cstore.SetTlsCipherSuiteByIndex(0, DicomTlsCipherSuiteType.DheRsaWith3DesEdeCbcSha);
                    _cstore.SetTlsClientCertificate(
                    _mySettings._settings.clientCertificate,
                    DicomTlsCertificateType.Pem,
                    _mySettings._settings.privateKey.Length > 0 ? _mySettings._settings.privateKey : null);
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }

            }

            if (_mySettings._settings.logLowLevel)
            {
                if (_tracer == null)
                {
                    _tracer = new TextBoxTraceListener(logWindow.RichTextBox);
                    Trace.Listeners.Add(_tracer);
                }
            }
            else
            {
                if (_tracer != null)
                {
                    Trace.Listeners.Remove(_tracer);
                    _tracer = null;
                }
            }

            _cstore.DebugLogFilename = string.Empty;
            _cstore.EnableDebugLog = true;
        }

        void _cstore_PrivateKeyPassword(object sender, PrivateKeyPasswordEventArgs e)
        {
            e.PrivateKeyPassword = _mySettings._settings.privateKeyPassword;
        }
        #endregion
        private void _btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmMain_Resize(object sender, EventArgs e)
        {
            try
            {
                _pictureBox.Invalidate();
                _lstBoxPages.Invalidate();
            }
            catch { }
        }

        private async void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FinilizeScreenCapture();
                FinilizeTwain();
            }
            catch (Exception)
            {
                //MessageBox.Show("Closing Form " + Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private async void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                await _cameraControl.VideoCapture1.StopAsync();
            }
            catch (Exception)
            {
                //MessageBox.Show("Closing Form " + Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            try
            {
                _codec?.Dispose();
                _cstore?.Dispose();
                _pgDicomInfo?.Dispose();
                _pictureBox?.Dispose();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi giải phóng tài nguyên khi đóng form");
            }
        }

        void _pgDicomInfo_BeforeAddElement(object sender, BeforeAddElementEventArgs e)
        {
            //
            // Excluded items and Volatile Elements will not be displayed in the editor
            //
            e.Cancel = _ExcludedTags.Contains(e.Element.DicomElement.Tag) || e.Element.DataSet.IsVolatileElement(e.Element.DicomElement);
            if (!e.Cancel)
            {
                if (e.Element.DicomElement.Tag == DicomTag.ConversionType)
                {
                    e.Element.Choices.Add("DV");
                    e.Element.Choices.Add("DI");
                    e.Element.Choices.Add("DF");
                    e.Element.Choices.Add("WSD");
                    e.Element.Choices.Add("SD");
                    e.Element.Choices.Add("SI");
                    e.Element.Choices.Add("DRW");
                    e.Element.Choices.Add("SYN");
                    e.Element.Attributes.Add(new TypeConverterAttribute(typeof(DicomPropertyChoiceConverter)));
                }
                else if (e.Element.DicomElement.Tag == DicomTag.Modality)
                {
                    e.Element.Choices.Add("CR");
                    e.Element.Choices.Add("CT");
                    e.Element.Choices.Add("MR");
                    e.Element.Choices.Add("NM");
                    e.Element.Choices.Add("US");
                    e.Element.Choices.Add("OT");
                    e.Element.Choices.Add("BI");
                    e.Element.Choices.Add("DG");
                    e.Element.Choices.Add("ES");
                    e.Element.Choices.Add("LS");
                    e.Element.Choices.Add("PT");
                    e.Element.Choices.Add("RG");
                    e.Element.Choices.Add("TG");
                    e.Element.Choices.Add("XA");
                    e.Element.Choices.Add("RF");
                    e.Element.Choices.Add("RTIMAGE");
                    e.Element.Choices.Add("RTDOSE");
                    e.Element.Choices.Add("RTSTRUCT");
                    e.Element.Choices.Add("RTPLAN");
                    e.Element.Choices.Add("RTRECORD");
                    e.Element.Choices.Add("HC");
                    e.Element.Choices.Add("DX");
                    e.Element.Choices.Add("MG");
                    e.Element.Choices.Add("IO");
                    e.Element.Choices.Add("PX");
                    e.Element.Choices.Add("GM");
                    e.Element.Choices.Add("SM");
                    e.Element.Choices.Add("XC");
                    e.Element.Choices.Add("PR");
                    e.Element.Choices.Add("AU");
                    e.Element.Choices.Add("ECG");
                    e.Element.Choices.Add("EPS");
                    e.Element.Choices.Add("HD");
                    e.Element.Choices.Add("SR");
                    e.Element.Choices.Add("IVUS");
                    e.Element.Choices.Add("OP");
                    e.Element.Choices.Add("SMR");
                    e.Element.Choices.Add("AR");
                    e.Element.Choices.Add("KER");
                    e.Element.Choices.Add("VA");
                    e.Element.Choices.Add("SRF");
                    e.Element.Choices.Add("OCT");
                    e.Element.Choices.Add("LEN");
                    e.Element.Choices.Add("OPV");
                    e.Element.Choices.Add("OPM");
                    e.Element.Choices.Add("OAM");
                    e.Element.Choices.Add("RESP");
                    e.Element.Choices.Add("KO");
                    e.Element.Choices.Add("SEG");
                    e.Element.Choices.Add("REG");
                    e.Element.Choices.Add("OPT");
                    e.Element.Choices.Add("BDUS");
                    e.Element.Choices.Add("BMD");
                    e.Element.Choices.Add("DOC");
                    e.Element.Choices.Add("FID");
                    e.Element.Choices.Add("DS");
                    e.Element.Choices.Add("CF");
                    e.Element.Choices.Add("DF");
                    e.Element.Choices.Add("VF");
                    e.Element.Choices.Add("AS");
                    e.Element.Choices.Add("CS");
                    e.Element.Choices.Add("EC");
                    e.Element.Choices.Add("LP");
                    e.Element.Choices.Add("FA");
                    e.Element.Choices.Add("CP");
                    e.Element.Choices.Add("DM");
                    e.Element.Choices.Add("FS");
                    e.Element.Choices.Add("MA");
                    e.Element.Choices.Add("MS");
                    e.Element.Choices.Add("CD");
                    e.Element.Choices.Add("DD");
                    e.Element.Choices.Add("ST");
                    e.Element.Choices.Add("OPR");
                    e.Element.Attributes.Add(new TypeConverterAttribute(typeof(DicomPropertyChoiceConverter)));
                }
            }
        }

        void _frmOperation_Cancel(object sender, EventArgs e)
        {
            bCancelOperation = true;
        }

        void _pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            int iDelteY = e.Y - iOldY;
            int iDelteX = e.X - iOldX;
            if (e.Button == MouseButtons.Middle && _pictureBox.Image != null)
            {
                if (iDelteY < 0)
                    ZoomPicture(0.03f);
                if (iDelteY > 0)
                    ZoomPicture(-0.03f);
            }
            if (e.Button == MouseButtons.Right && _pictureBox.Image != null)
            {
                _pictureBox.ScrollPosition = new Point(_pictureBox.ScrollPosition.X - iDelteX, _pictureBox.ScrollPosition.Y - iDelteY);
            }
            iOldY = e.Y;
            iOldX = e.X;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Lấy phím từ cấu hình
            Keys previewKey = ParseKey(ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.Preview);
            Keys signKey = ParseKey(ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.Sign);
            Keys printKey = ParseKey(ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.Print);
            Keys draftKey = ParseKey(ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.Draft);
            Keys exitKey = ParseKey(ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.Exit);
            Keys snapShot = ParseKey(ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.Snapshot);
            Keys stop = ParseKey(ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.Stop);
            Keys linkCamera = ParseKey(ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.LinkCamera);


            // So sánh keyData
            if (keyData == previewKey)
            {
                _btnPreviewMain.PerformClick();
                return true;
            }
            if (keyData == signKey)
            {
                _btnSignature.PerformClick();
                return true;
            }
            if (keyData == printKey)
            {
                _btnPrint.PerformClick();
                return true;
            }
            if (keyData == draftKey)
            {
                _btnSave.PerformClick();
                return true;
            }
            if (keyData == exitKey)
            {
                _btnCancel.PerformClick();
                return true;
            }

            if (keyData == snapShot)
            {
                _btnSnapshot.PerformClick();
                return true;
            }
            if (keyData == stop)
            {
                _btnStop.PerformClick();
                return true;
            }
            if (keyData == linkCamera)
            {
                _btnLinkCamera.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        // Hàm chuyển string thành Keys
        private Keys ParseKey(string key)
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

        #region Helper Methods - UI

        /// <summary>
        /// Hiển thị splash screen
        /// </summary>
        private void ShowSplashScreen(Form parentForm, string caption, string description)
        {
            if (parentForm == null)
                return;

            try
            {
                SplashScreenManager.ShowForm(parentForm, typeof(WaitFormLoading), true, true, false);
                SplashScreenManager.Default?.SetWaitFormCaption(caption);
                SplashScreenManager.Default?.SetWaitFormDescription(description);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không thể hiển thị splash screen");
            }
        }

        /// <summary>
        /// Đóng splash screen
        /// </summary>
        private void CloseSplashScreen(bool isVisible)
        {
            if (!isVisible)
                return;

            try
            {
                if (SplashScreenManager.Default != null && SplashScreenManager.Default.IsSplashFormVisible)
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không thể đóng splash screen");
            }
        }

        /// <summary>
        /// Hiển thị thông báo thành công
        /// </summary>
        private void ShowSuccessMessage(string message)
        {
            XtraMessageBox.Show(
                this,
                message,
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        /// <summary>
        /// Hiển thị thông báo lỗi
        /// </summary>
        private void ShowErrorMessage(string title, string message)
        {
            XtraMessageBox.Show(
                this,
                message,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        /// <summary>
        /// Hiển thị thông báo cảnh báo
        /// </summary>
        private void ShowWarningMessage(string title, string message)
        {
            XtraMessageBox.Show(
                this,
                message,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        #endregion
    }

    [Serializable]
    class PrintPage : IDisposable, IPrintToPACSFile
    {
        bool bSelected = false;
        public bool Selected
        {
            get { return bSelected; }
            set { bSelected = value; }
        }

        private int _jobId;
        public int JobId
        {
            get { return _jobId; }
        }

        private string _strRecognizedFilePath = "";
        public string RecognizedFilePath
        {
            get { return _strRecognizedFilePath; }
            set { _strRecognizedFilePath = value; }
        }

        IntPtr _metaFile;
        public IntPtr MetaFile
        {
            get { return _metaFile; }
            set { _metaFile = value; }
        }

        string file = string.Empty;
        public string FilePath
        {
            get { return file; }
            set { file = value; }
        }

        public PrintPage(int jobId)
        {
            _jobId = jobId;
        }

        ~PrintPage()
        {
            //if (File.Exists(tempFile))
            //   File.Delete(tempFile);


            //if (File.Exists(RecognizedFilePath))
            //   File.Delete(RecognizedFilePath);
            //MetaFile.Dispose();
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                if (File.Exists(file))
                    File.Delete(file);

                if (File.Exists(RecognizedFilePath))
                    File.Delete(RecognizedFilePath);
            }
            catch { }
            //MetaFile.Dispose();
        }
        #endregion

        public string FileLocation()
        {
            return file;
        }
    }

    [Serializable]
    class Page : IDisposable, IPrintToPACSFile
    {
        bool bDeleteOnDispose = false;
        public bool DeleteOnDispose
        {
            get { return bDeleteOnDispose; }
            set { bDeleteOnDispose = value; }
        }

        string file = string.Empty;
        public string FilePath
        {
            get { return file; }
            set { file = value; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                if (bDeleteOnDispose)
                    if (System.IO.File.Exists(file))
                        System.IO.File.Delete(file);
            }
            catch { }
        }

        #endregion

        public string FileLocation()
        {
            return file;
        }
    }

    interface IPrintToPACSFile
    {
        string FileLocation();
    }

    [Serializable]
    public class ProcedureStep
    {

        private ModalityWorklistResult _Result;
        private MPPSNCreate _MppsCreate;

        public ModalityWorklistResult Result
        {
            get { return _Result; }
            set { _Result = value; }
        }

        public MPPSNCreate MppsCreate
        {
            get { return _MppsCreate; }
            set { _MppsCreate = value; }
        }

        public ProcedureStep()
        {
        }

        public ProcedureStep(ModalityWorklistResult result)
        {
            _Result = result;
        }

        public ProcedureStep(ModalityWorklistResult result, MPPSNCreate mPPSNCreate)
        {
            _Result = result;
            _MppsCreate = mPPSNCreate;
        }
    }

    internal static class Extensions
    {
        public static void CopyTo<T>(this object source, T dest)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The object you are copying from cannot be null");

            if (dest == null)
                throw new ArgumentNullException("dest", "The object you are copying to cannot be null");

            // Don't copy if they are the same object
            if (!ReferenceEquals(source, dest))
            {
                List<PropertyInfo> matches = GetMatchingProperties(source, dest);

                foreach (PropertyInfo fromProperty in matches)
                {
                    PropertyInfo toProperty = dest.GetType().GetProperty(fromProperty.Name);

                    if (toProperty.CanWrite)
                    {
                        object value = null;

                        if (source is DataRow)
                        {
                            DataRow row = source as DataRow;

                            if (row[fromProperty.Name] != null)
                                value = row[fromProperty.Name];
                        }
                        else
                        {
                            value = fromProperty.GetValue(source, null);
                        }

                        if (value == DBNull.Value)
                            value = null;
                        toProperty.SetValue(dest, value, null);
                    }
                }
            }
        }

        private static List<PropertyInfo> GetMatchingProperties(object source, object target)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (target == null)
                throw new ArgumentNullException("target");

            var sourceType = source.GetType();
            var sourceProperties = sourceType.GetProperties();
            var targetType = target.GetType();
            var targetProperties = targetType.GetProperties();
            var properties = (from s in sourceProperties
                              from t in targetProperties
                              where s.Name == t.Name &&
                                    s.PropertyType == t.PropertyType
                              select s).ToList();

            return properties;
        }
    }
}