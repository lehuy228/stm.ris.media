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
                _baseFolder = ServiceLocator.GetMediaStorageBasePath();
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
                _baseFolder = ServiceLocator.GetMediaStorageBasePath();
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
            catch (Exception ex)
            {
                Log.Warning(ex, "Lỗi khi resize form");
            }
        }

        // Cờ đánh dấu đã dọn dẹp async xong - lần Close() thứ hai mới cho form đóng thật
        private bool _isCleanupDone;

        private async void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Không chặn khi Windows shutdown - hệ điều hành cần đóng form ngay
            if (_isCleanupDone || e.CloseReason == CloseReason.WindowsShutDown)
                return;

            // Hoãn việc đóng form: phải dừng camera (async) xong rồi mới đóng,
            // nếu không FormClosed sẽ dispose tài nguyên trong khi camera còn đang dừng
            e.Cancel = true;
            try
            {
                FinilizeScreenCapture();
                FinilizeTwain();
                //if (_cameraControl != null && _cameraControl.VideoCapture1 != null) // VisioForge đã thay bằng FlashCap
                //{
                //    await _cameraControl.VideoCapture1.StopAsync();
                //}
                if (_cameraControl != null)
                {
                    await _cameraControl.StopCaptureAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Lỗi khi dừng camera/thiết bị lúc đóng form");
            }
            finally
            {
                _isCleanupDone = true;
                Close();
            }
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
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

    }
}
