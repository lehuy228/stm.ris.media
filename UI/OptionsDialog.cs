using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using PrintToPACS.Utilities;
using System.Net;
using System.Collections.Generic;
using Leadtools.Dicom;
using Leadtools.Dicom.Common.Extensions;
using PrintToPACSDemo.UI;
using PrinterDemo;
using Leadtools.Dicom.Scu.Common;
using Leadtools;
using Leadtools.Dicom.Scu;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using Leadtools.DicomDemos;
using System.Threading.Tasks;
using VisioForge.Core.VideoCapture;
using VisioForge.Core.Types;
using VisioForge.Core.Types.Events;
using System.Linq;
using System.Globalization;
using VisioForge.Core.Types.VideoCapture;
using MediaToPacs.Core.Models;

namespace PrintToPACSDemo
{
    /// <summary>
    /// Summary description for OptionsDialog.
    /// </summary>
    public class OptionsDialog : DevExpress.XtraEditors.XtraForm
    {
        #region Fields
        private VisioForge.Core.UI.WinForms.VideoView videoView1;
        private System.Windows.Forms.GroupBox _groupBoxClient;
        private DevExpress.XtraEditors.LabelControl _labelClientAE;
        public DevExpress.XtraEditors.TextEdit _textBoxClientAE;
        private DevExpress.XtraEditors.SimpleButton buttonOK;
        private DevExpress.XtraEditors.SimpleButton buttonCancel;
        private DevExpress.XtraEditors.LabelControl _labelHint;
        private DevExpress.XtraEditors.TextEdit _textBoxKeyPassword;
        private DevExpress.XtraEditors.TextEdit _textBoxPrivateKey;
        private DevExpress.XtraEditors.SimpleButton _buttonPrivateKey;
        private DevExpress.XtraEditors.LabelControl _labelPrivateKey;
        private DevExpress.XtraEditors.LabelControl _labelPrivateKeyPassword;
        private DevExpress.XtraEditors.TextEdit _textBoxClientCertificate;
        private DevExpress.XtraEditors.SimpleButton _buttonClientCertificate;
        private DevExpress.XtraEditors.LabelControl _labelCertificate;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;


        private string _clientAE;
        public MyServerList serverlistSCP;
        public MyServerList serverlistMWL;
        public MyServerList serverlistStore;
        List<AssociationHolder> lstAssociations;
        int iLastNumber = 1;

        List<DicomClassType> ClassTypes = new List<DicomClassType>(){
         DicomClassType.SCImageStorage,
         DicomClassType.SCMultiFrameTrueColorImageStorage,
         DicomClassType.SCMultiFrameGrayscaleByteImageStorage,
         DicomClassType.EncapsulatedPdfStorage
      };

        private string _clientCertificate;
        private int _defaultSCPServer = 0;
        private int _defaultMWLServer = 0;
        private int _defaultStoreServer = 0;
        private string _privateKey;
        private string _privateKeyPassword;
        private DicomImageCompressionType _SCCompression;
        private DicomImageCompressionType _SCColorCompression;
        private DicomImageCompressionType _SCGrayCompression;
        private string _SCPath;
        private string _SCColorPath;
        private string _SCGrayPath;
        private string _PdfPath;
        private string _PrinterName;
        private string _TempDirectory;
        private bool _AutoDelete;
        private int _iSelectedTab;
        private DataGridView dataGridViewServers;
        private Panel panelButtonServer;
        private DevExpress.XtraEditors.SimpleButton buttonAddServer;
        private DevExpress.XtraEditors.SimpleButton buttonDeleteServer;
        private GroupBox _groupBoxSecurity;
        private TabControl _tbServers;
        private TabPage _tbSCPQuerypage;
        private TabPage _tbMWLQueryPage;
        private TabPage _tbStorePage;
        private DevExpress.XtraTab.XtraTabControl _tbOptions;
        private DevExpress.XtraTab.XtraTabPage _tpApplicationOptions;
        private DevExpress.XtraTab.XtraTabPage _tpDicomOptions;
        private GroupBox _gpDicomType;
        private DevExpress.XtraEditors.TextEdit _txtSCPDF;
        private DevExpress.XtraEditors.TextEdit _txtSCGray;
        private DevExpress.XtraEditors.TextEdit _txtSCColor;
        private DevExpress.XtraEditors.TextEdit _txtSC;
        private DevExpress.XtraEditors.TextEdit _txtTempDir;
        private DevExpress.XtraEditors.TextEdit _txtPrinterName;
        private DevExpress.XtraEditors.LabelControl _lblPrinterName;
        private DevExpress.XtraEditors.LabelControl label2;
        private DevExpress.XtraEditors.ComboBoxEdit _cmbSCColor;
        private DevExpress.XtraEditors.ComboBoxEdit _cmbSCGray;
        private DevExpress.XtraEditors.ComboBoxEdit _cmbSC;
        private DevExpress.XtraEditors.LabelControl label1;
        private RadioButton _rdPDF;
        private RadioButton _rdGrayScale;
        private RadioButton _rdColored;
        private RadioButton _rdSecondaryCapture;
        private DevExpress.XtraEditors.CheckEdit _ckAutoDelete;
        private DevExpress.XtraEditors.LabelControl label3;
        private DevExpress.XtraEditors.SimpleButton _btnRename;
        private bool _logLowLevel;
        private DicomClassType _selectedtype;

        List<String> Compressions = new List<String>(){
               "Uncompressed",
               "Lossless JPEG",
               "Lossy JPEG",
               "Lossless J2k",
               "Lossy J2K"
      };

        List<DicomImageCompressionType> ImgCompression = new List<DicomImageCompressionType>(){
         DicomImageCompressionType.None,
         DicomImageCompressionType.JpegLossless,
         DicomImageCompressionType.JpegLossy,
         DicomImageCompressionType.J2kLossless,
         DicomImageCompressionType.J2kLossy
      };

        bool bTimeOut = true;
        private DevExpress.XtraEditors.SimpleButton _btnBrowseSCPDF;
        private DevExpress.XtraEditors.SimpleButton _btnBrowseSCGray;
        private DevExpress.XtraEditors.SimpleButton _btnBrowseSCColor;
        private DevExpress.XtraEditors.SimpleButton _btnBrowseSC;
        private DataGridViewTextBoxColumn ColumnAE;
        private DataGridViewTextBoxColumn ColumnIP;
        private DataGridViewTextBoxColumn ColumnPort;
        private DataGridViewTextBoxColumn ColumnTimeout;
        private DataGridViewCheckBoxColumn ColumnTls;
        private DataGridViewButtonColumn TestServer;
        private DataGridViewCheckBoxColumn DefaultServer;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraTab.XtraTabPage _tpCameraOptions;
        private DevExpress.XtraEditors.ComboBoxEdit cbVideoInputDevice;
        private TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit cbFlipY;
        private DevExpress.XtraEditors.CheckEdit cbFlipX;
        private DevExpress.XtraEditors.CheckEdit cbInvert;
        private DevExpress.XtraEditors.CheckEdit cbGreyscale;
        private DevExpress.XtraEditors.SimpleButton btOutputConfigure;
        private DevExpress.XtraEditors.LabelControl label24;
        private DevExpress.XtraEditors.ComboBoxEdit cbOutputFormat;
        private DevExpress.XtraEditors.LabelControl label23;
        private DevExpress.XtraEditors.SimpleButton buttonSettingCam;
        private DevExpress.XtraEditors.ComboBoxEdit cbAudioInputLine;
        private DevExpress.XtraEditors.LabelControl label9;
        private DevExpress.XtraEditors.LabelControl label8;
        private DevExpress.XtraEditors.ComboBoxEdit cbVideoInputFormat;
        private DevExpress.XtraEditors.ComboBoxEdit cbAudioInputFormat;
        private DevExpress.XtraEditors.LabelControl label6;
        private DevExpress.XtraEditors.ComboBoxEdit cbAudioInputDevice;
        private DevExpress.XtraEditors.LabelControl label5;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cbVideoInputFrameRate;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.PanelControl panelControl6;
        private DevExpress.XtraEditors.PanelControl panelControl5;
        private DevExpress.XtraTab.XtraTabControl tabControl2;
        private DevExpress.XtraTab.XtraTabPage tabPage3;
        private DevExpress.XtraEditors.CheckEdit cbZoom;
        private DevExpress.XtraEditors.SimpleButton btEffZoomRight;
        private DevExpress.XtraEditors.SimpleButton btEffZoomLeft;
        private DevExpress.XtraEditors.SimpleButton btEffZoomDown;
        private DevExpress.XtraEditors.SimpleButton btEffZoomOut;
        private DevExpress.XtraEditors.SimpleButton btEffZoomIn;
        private DevExpress.XtraEditors.SimpleButton btEffZoomUp;
        private DevExpress.XtraTab.XtraTabPage tabPage4;
        private GroupBox groupBox5;
        private DevExpress.XtraEditors.TextEdit edPanDestHeight;
        private DevExpress.XtraEditors.LabelControl label16;
        private DevExpress.XtraEditors.TextEdit edPanDestTop;
        private DevExpress.XtraEditors.LabelControl label17;
        private DevExpress.XtraEditors.TextEdit edPanDestWidth;
        private DevExpress.XtraEditors.LabelControl label18;
        private DevExpress.XtraEditors.TextEdit edPanDestLeft;
        private DevExpress.XtraEditors.LabelControl label19;
        private GroupBox groupBox4;
        private DevExpress.XtraEditors.TextEdit edPanSourceHeight;
        private DevExpress.XtraEditors.LabelControl label14;
        private DevExpress.XtraEditors.TextEdit edPanSourceTop;
        private DevExpress.XtraEditors.LabelControl label15;
        private DevExpress.XtraEditors.TextEdit edPanSourceWidth;
        private DevExpress.XtraEditors.LabelControl label12;
        private DevExpress.XtraEditors.TextEdit edPanSourceLeft;
        private DevExpress.XtraEditors.LabelControl label13;
        private DevExpress.XtraEditors.CheckEdit cbPan;
        private GroupBox groupBox3;
        private DevExpress.XtraEditors.TextEdit edPanStopTime;
        private DevExpress.XtraEditors.LabelControl label11;
        private DevExpress.XtraEditors.TextEdit edPanStartTime;
        private DevExpress.XtraEditors.LabelControl label10;
        private DevExpress.XtraTab.XtraTabPage tabPage5;
        private DevExpress.XtraEditors.ZoomTrackBarControl tbLiveRotationAngle;
        private DevExpress.XtraEditors.CheckEdit cbLiveRotation;
        private DevExpress.XtraEditors.LabelControl labelLiveRotationAngle;
        private DevExpress.XtraEditors.LabelControl label21;
        private DevExpress.XtraTab.XtraTabPage _tbShortcutSettings;
        private TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.PanelControl panelControl8;
        private DevExpress.XtraEditors.TextEdit _txStoreKey;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.TextEdit _txExitKey;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.TextEdit _txPrintKey;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.TextEdit _txReloadKey;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.PanelControl panelControl7;
        private DevExpress.XtraEditors.TextEdit _txSaveKey;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.TextEdit _txSnapshotKey;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit _txPauseRecordingKey;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit _txStartRecordingKey;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.PanelControl panelControl9;
        private DevExpress.XtraEditors.PanelControl _panelCamera;
        private DevExpress.XtraEditors.SimpleButton _btnPreview;
        private DevExpress.XtraEditors.SimpleButton _btnStopCamera;
        private DevExpress.XtraEditors.SimpleButton _btnBrowseTempDir;

        #endregion

        #region Constructor
        public OptionsDialog()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._groupBoxClient = new System.Windows.Forms.GroupBox();
            this._textBoxClientAE = new DevExpress.XtraEditors.TextEdit();
            this._labelClientAE = new DevExpress.XtraEditors.LabelControl();
            this._labelHint = new DevExpress.XtraEditors.LabelControl();
            this._textBoxKeyPassword = new DevExpress.XtraEditors.TextEdit();
            this._textBoxPrivateKey = new DevExpress.XtraEditors.TextEdit();
            this._buttonPrivateKey = new DevExpress.XtraEditors.SimpleButton();
            this._labelPrivateKey = new DevExpress.XtraEditors.LabelControl();
            this._labelPrivateKeyPassword = new DevExpress.XtraEditors.LabelControl();
            this._textBoxClientCertificate = new DevExpress.XtraEditors.TextEdit();
            this._buttonClientCertificate = new DevExpress.XtraEditors.SimpleButton();
            this._labelCertificate = new DevExpress.XtraEditors.LabelControl();
            this.buttonOK = new DevExpress.XtraEditors.SimpleButton();
            this.buttonCancel = new DevExpress.XtraEditors.SimpleButton();
            this.dataGridViewServers = new System.Windows.Forms.DataGridView();
            this.ColumnAE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTimeout = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTls = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.TestServer = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DefaultServer = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panelButtonServer = new System.Windows.Forms.Panel();
            this.buttonAddServer = new DevExpress.XtraEditors.SimpleButton();
            this.buttonDeleteServer = new DevExpress.XtraEditors.SimpleButton();
            this._groupBoxSecurity = new System.Windows.Forms.GroupBox();
            this._tbServers = new System.Windows.Forms.TabControl();
            this._tbSCPQuerypage = new System.Windows.Forms.TabPage();
            this._tbMWLQueryPage = new System.Windows.Forms.TabPage();
            this._tbStorePage = new System.Windows.Forms.TabPage();
            this._tbOptions = new DevExpress.XtraTab.XtraTabControl();
            this._tpApplicationOptions = new DevExpress.XtraTab.XtraTabPage();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this._lblPrinterName = new DevExpress.XtraEditors.LabelControl();
            this._txtPrinterName = new DevExpress.XtraEditors.TextEdit();
            this._btnRename = new DevExpress.XtraEditors.SimpleButton();
            this._ckAutoDelete = new DevExpress.XtraEditors.CheckEdit();
            this._gpDicomType = new System.Windows.Forms.GroupBox();
            this._btnBrowseSCPDF = new DevExpress.XtraEditors.SimpleButton();
            this._btnBrowseTempDir = new DevExpress.XtraEditors.SimpleButton();
            this.label3 = new DevExpress.XtraEditors.LabelControl();
            this._btnBrowseSCGray = new DevExpress.XtraEditors.SimpleButton();
            this._btnBrowseSCColor = new DevExpress.XtraEditors.SimpleButton();
            this._btnBrowseSC = new DevExpress.XtraEditors.SimpleButton();
            this.label2 = new DevExpress.XtraEditors.LabelControl();
            this._txtTempDir = new DevExpress.XtraEditors.TextEdit();
            this._cmbSCColor = new DevExpress.XtraEditors.ComboBoxEdit();
            this._cmbSCGray = new DevExpress.XtraEditors.ComboBoxEdit();
            this._cmbSC = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label1 = new DevExpress.XtraEditors.LabelControl();
            this._rdPDF = new System.Windows.Forms.RadioButton();
            this._rdGrayScale = new System.Windows.Forms.RadioButton();
            this._rdColored = new System.Windows.Forms.RadioButton();
            this._rdSecondaryCapture = new System.Windows.Forms.RadioButton();
            this._txtSCPDF = new DevExpress.XtraEditors.TextEdit();
            this._txtSCGray = new DevExpress.XtraEditors.TextEdit();
            this._txtSCColor = new DevExpress.XtraEditors.TextEdit();
            this._txtSC = new DevExpress.XtraEditors.TextEdit();
            this._tpDicomOptions = new DevExpress.XtraTab.XtraTabPage();
            this._tpCameraOptions = new DevExpress.XtraTab.XtraTabPage();
            this.panelControl9 = new DevExpress.XtraEditors.PanelControl();
            this.tabControl2 = new DevExpress.XtraTab.XtraTabControl();
            this.tabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.cbZoom = new DevExpress.XtraEditors.CheckEdit();
            this.btEffZoomRight = new DevExpress.XtraEditors.SimpleButton();
            this.btEffZoomLeft = new DevExpress.XtraEditors.SimpleButton();
            this.btEffZoomDown = new DevExpress.XtraEditors.SimpleButton();
            this.btEffZoomOut = new DevExpress.XtraEditors.SimpleButton();
            this.btEffZoomIn = new DevExpress.XtraEditors.SimpleButton();
            this.btEffZoomUp = new DevExpress.XtraEditors.SimpleButton();
            this.tabPage4 = new DevExpress.XtraTab.XtraTabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.edPanSourceHeight = new DevExpress.XtraEditors.TextEdit();
            this.label14 = new DevExpress.XtraEditors.LabelControl();
            this.edPanSourceTop = new DevExpress.XtraEditors.TextEdit();
            this.label15 = new DevExpress.XtraEditors.LabelControl();
            this.edPanSourceWidth = new DevExpress.XtraEditors.TextEdit();
            this.label12 = new DevExpress.XtraEditors.LabelControl();
            this.edPanSourceLeft = new DevExpress.XtraEditors.TextEdit();
            this.label13 = new DevExpress.XtraEditors.LabelControl();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.edPanDestHeight = new DevExpress.XtraEditors.TextEdit();
            this.label16 = new DevExpress.XtraEditors.LabelControl();
            this.edPanDestTop = new DevExpress.XtraEditors.TextEdit();
            this.label17 = new DevExpress.XtraEditors.LabelControl();
            this.edPanDestWidth = new DevExpress.XtraEditors.TextEdit();
            this.label18 = new DevExpress.XtraEditors.LabelControl();
            this.edPanDestLeft = new DevExpress.XtraEditors.TextEdit();
            this.label19 = new DevExpress.XtraEditors.LabelControl();
            this.cbPan = new DevExpress.XtraEditors.CheckEdit();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.edPanStopTime = new DevExpress.XtraEditors.TextEdit();
            this.label11 = new DevExpress.XtraEditors.LabelControl();
            this.edPanStartTime = new DevExpress.XtraEditors.TextEdit();
            this.label10 = new DevExpress.XtraEditors.LabelControl();
            this.tabPage5 = new DevExpress.XtraTab.XtraTabPage();
            this.tbLiveRotationAngle = new DevExpress.XtraEditors.ZoomTrackBarControl();
            this.cbLiveRotation = new DevExpress.XtraEditors.CheckEdit();
            this.labelLiveRotationAngle = new DevExpress.XtraEditors.LabelControl();
            this.label21 = new DevExpress.XtraEditors.LabelControl();
            this._panelCamera = new DevExpress.XtraEditors.PanelControl();
            this.panelControl6 = new DevExpress.XtraEditors.PanelControl();
            this._btnPreview = new DevExpress.XtraEditors.SimpleButton();
            this.label24 = new DevExpress.XtraEditors.LabelControl();
            this.btOutputConfigure = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.cbAudioInputDevice = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label5 = new DevExpress.XtraEditors.LabelControl();
            this.cbAudioInputFormat = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label6 = new DevExpress.XtraEditors.LabelControl();
            this.cbAudioInputLine = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label9 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cbVideoInputDevice = new DevExpress.XtraEditors.ComboBoxEdit();
            this.buttonSettingCam = new DevExpress.XtraEditors.SimpleButton();
            this.label8 = new DevExpress.XtraEditors.LabelControl();
            this.cbVideoInputFormat = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbVideoInputFrameRate = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbOutputFormat = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.label23 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            this.cbFlipY = new DevExpress.XtraEditors.CheckEdit();
            this.cbFlipX = new DevExpress.XtraEditors.CheckEdit();
            this.cbGreyscale = new DevExpress.XtraEditors.CheckEdit();
            this.cbInvert = new DevExpress.XtraEditors.CheckEdit();
            this._tbShortcutSettings = new DevExpress.XtraTab.XtraTabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl8 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl7 = new DevExpress.XtraEditors.PanelControl();
            this._txStoreKey = new DevExpress.XtraEditors.TextEdit();
            this._txSaveKey = new DevExpress.XtraEditors.TextEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this._txExitKey = new DevExpress.XtraEditors.TextEdit();
            this._txSnapshotKey = new DevExpress.XtraEditors.TextEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this._txPrintKey = new DevExpress.XtraEditors.TextEdit();
            this._txPauseRecordingKey = new DevExpress.XtraEditors.TextEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this._txReloadKey = new DevExpress.XtraEditors.TextEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this._txStartRecordingKey = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this._btnStopCamera = new DevExpress.XtraEditors.SimpleButton();
            this._groupBoxClient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._textBoxClientAE.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._textBoxKeyPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._textBoxPrivateKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._textBoxClientCertificate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServers)).BeginInit();
            this.panelButtonServer.SuspendLayout();
            this._groupBoxSecurity.SuspendLayout();
            this._tbServers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tbOptions)).BeginInit();
            this._tbOptions.SuspendLayout();
            this._tpApplicationOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txtPrinterName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._ckAutoDelete.Properties)).BeginInit();
            this._gpDicomType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txtTempDir.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._cmbSCColor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._cmbSCGray.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._cmbSC.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtSCPDF.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtSCGray.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtSCColor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtSC.Properties)).BeginInit();
            this._tpDicomOptions.SuspendLayout();
            this._tpCameraOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl9)).BeginInit();
            this.panelControl9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl2)).BeginInit();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbZoom.Properties)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edPanSourceHeight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edPanSourceTop.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edPanSourceWidth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edPanSourceLeft.Properties)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edPanDestHeight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edPanDestTop.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edPanDestWidth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edPanDestLeft.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbPan.Properties)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edPanStopTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edPanStartTime.Properties)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbLiveRotationAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLiveRotationAngle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbLiveRotation.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._panelCamera)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).BeginInit();
            this.panelControl6.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbAudioInputDevice.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbAudioInputFormat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbAudioInputLine.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbVideoInputDevice.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbVideoInputFormat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbVideoInputFrameRate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbOutputFormat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            this.panelControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbFlipY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbFlipX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbGreyscale.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbInvert.Properties)).BeginInit();
            this._tbShortcutSettings.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl7)).BeginInit();
            this.panelControl7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txStoreKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txSaveKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txExitKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txSnapshotKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txPrintKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txPauseRecordingKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txReloadKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txStartRecordingKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _groupBoxClient
            // 
            this._groupBoxClient.Controls.Add(this._textBoxClientAE);
            this._groupBoxClient.Controls.Add(this._labelClientAE);
            this._groupBoxClient.Dock = System.Windows.Forms.DockStyle.Top;
            this._groupBoxClient.Location = new System.Drawing.Point(3, 3);
            this._groupBoxClient.Name = "_groupBoxClient";
            this._groupBoxClient.Size = new System.Drawing.Size(660, 72);
            this._groupBoxClient.TabIndex = 1;
            this._groupBoxClient.TabStop = false;
            this._groupBoxClient.Text = "Thông tin máy thực hiện";
            // 
            // _textBoxClientAE
            // 
            this._textBoxClientAE.Location = new System.Drawing.Point(170, 28);
            this._textBoxClientAE.Name = "_textBoxClientAE";
            this._textBoxClientAE.Size = new System.Drawing.Size(563, 22);
            this._textBoxClientAE.TabIndex = 1;
            // 
            // _labelClientAE
            // 
            this._labelClientAE.Location = new System.Drawing.Point(19, 32);
            this._labelClientAE.Name = "_labelClientAE";
            this._labelClientAE.Size = new System.Drawing.Size(41, 15);
            this._labelClientAE.TabIndex = 0;
            this._labelClientAE.Text = "AE Title";
            // 
            // _labelHint
            // 
            this._labelHint.Appearance.ForeColor = System.Drawing.Color.Blue;
            this._labelHint.Appearance.Options.UseForeColor = true;
            this._labelHint.Location = new System.Drawing.Point(360, 104);
            this._labelHint.Name = "_labelHint";
            this._labelHint.Size = new System.Drawing.Size(181, 15);
            this._labelHint.TabIndex = 15;
            this._labelHint.Text = "<== Sử dụng \'test\' cho client.pem";
            // 
            // _textBoxKeyPassword
            // 
            this._textBoxKeyPassword.Location = new System.Drawing.Point(169, 101);
            this._textBoxKeyPassword.Name = "_textBoxKeyPassword";
            this._textBoxKeyPassword.Size = new System.Drawing.Size(175, 22);
            this._textBoxKeyPassword.TabIndex = 14;
            // 
            // _textBoxPrivateKey
            // 
            this._textBoxPrivateKey.Location = new System.Drawing.Point(169, 66);
            this._textBoxPrivateKey.Name = "_textBoxPrivateKey";
            this._textBoxPrivateKey.Size = new System.Drawing.Size(564, 22);
            this._textBoxPrivateKey.TabIndex = 12;
            // 
            // _buttonPrivateKey
            // 
            this._buttonPrivateKey.Location = new System.Drawing.Point(127, 66);
            this._buttonPrivateKey.Name = "_buttonPrivateKey";
            this._buttonPrivateKey.Size = new System.Drawing.Size(35, 21);
            this._buttonPrivateKey.TabIndex = 11;
            this._buttonPrivateKey.Text = "...";
            this._buttonPrivateKey.Click += new System.EventHandler(this._buttonPrivateKey_Click);
            // 
            // _labelPrivateKey
            // 
            this._labelPrivateKey.Location = new System.Drawing.Point(19, 66);
            this._labelPrivateKey.Name = "_labelPrivateKey";
            this._labelPrivateKey.Size = new System.Drawing.Size(76, 15);
            this._labelPrivateKey.TabIndex = 10;
            this._labelPrivateKey.Text = "Khóa riêng tư:";
            // 
            // _labelPrivateKeyPassword
            // 
            this._labelPrivateKeyPassword.Location = new System.Drawing.Point(19, 101);
            this._labelPrivateKeyPassword.Name = "_labelPrivateKeyPassword";
            this._labelPrivateKeyPassword.Size = new System.Drawing.Size(54, 15);
            this._labelPrivateKeyPassword.TabIndex = 13;
            this._labelPrivateKeyPassword.Text = "Mật khẩu:";
            // 
            // _textBoxClientCertificate
            // 
            this._textBoxClientCertificate.Location = new System.Drawing.Point(169, 31);
            this._textBoxClientCertificate.Name = "_textBoxClientCertificate";
            this._textBoxClientCertificate.Size = new System.Drawing.Size(564, 22);
            this._textBoxClientCertificate.TabIndex = 9;
            // 
            // _buttonClientCertificate
            // 
            this._buttonClientCertificate.Location = new System.Drawing.Point(127, 31);
            this._buttonClientCertificate.Name = "_buttonClientCertificate";
            this._buttonClientCertificate.Size = new System.Drawing.Size(35, 21);
            this._buttonClientCertificate.TabIndex = 8;
            this._buttonClientCertificate.Text = "...";
            this._buttonClientCertificate.Click += new System.EventHandler(this._buttonClientCertificate_Click);
            // 
            // _labelCertificate
            // 
            this._labelCertificate.Location = new System.Drawing.Point(19, 35);
            this._labelCertificate.Name = "_labelCertificate";
            this._labelCertificate.Size = new System.Drawing.Size(70, 15);
            this._labelCertificate.TabIndex = 7;
            this._labelCertificate.Text = "Chứng nhận:";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(18, 11);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(105, 25);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "&Lưu";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.CausesValidation = false;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(129, 11);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(116, 25);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "&Thoát";
            // 
            // dataGridViewServers
            // 
            this.dataGridViewServers.AllowUserToAddRows = false;
            this.dataGridViewServers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewServers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewServers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnAE,
            this.ColumnIP,
            this.ColumnPort,
            this.ColumnTimeout,
            this.ColumnTls,
            this.TestServer,
            this.DefaultServer});
            this.dataGridViewServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewServers.Location = new System.Drawing.Point(3, 51);
            this.dataGridViewServers.Name = "dataGridViewServers";
            this.dataGridViewServers.RowHeadersWidth = 51;
            this.dataGridViewServers.Size = new System.Drawing.Size(619, 169);
            this.dataGridViewServers.TabIndex = 5;
            this.dataGridViewServers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewServers_CellClick);
            this.dataGridViewServers.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridViewServers_CurrentCellDirtyStateChanged);
            this.dataGridViewServers.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewServers_RowValidating);
            // 
            // ColumnAE
            // 
            this.ColumnAE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnAE.HeaderText = "Server AE Title";
            this.ColumnAE.MinimumWidth = 6;
            this.ColumnAE.Name = "ColumnAE";
            // 
            // ColumnIP
            // 
            this.ColumnIP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnIP.HeaderText = "Server IP Address";
            this.ColumnIP.MinimumWidth = 6;
            this.ColumnIP.Name = "ColumnIP";
            // 
            // ColumnPort
            // 
            this.ColumnPort.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColumnPort.HeaderText = "Server Port";
            this.ColumnPort.MinimumWidth = 6;
            this.ColumnPort.Name = "ColumnPort";
            // 
            // ColumnTimeout
            // 
            this.ColumnTimeout.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColumnTimeout.HeaderText = "Timeout (sec)";
            this.ColumnTimeout.MinimumWidth = 6;
            this.ColumnTimeout.Name = "ColumnTimeout";
            // 
            // ColumnTls
            // 
            this.ColumnTls.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnTls.HeaderText = "Secure (TLS)";
            this.ColumnTls.MinimumWidth = 6;
            this.ColumnTls.Name = "ColumnTls";
            this.ColumnTls.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnTls.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // TestServer
            // 
            this.TestServer.HeaderText = "Test Connection";
            this.TestServer.MinimumWidth = 6;
            this.TestServer.Name = "TestServer";
            this.TestServer.Text = "Test";
            this.TestServer.UseColumnTextForButtonValue = true;
            // 
            // DefaultServer
            // 
            this.DefaultServer.HeaderText = "Default Server";
            this.DefaultServer.MinimumWidth = 6;
            this.DefaultServer.Name = "DefaultServer";
            // 
            // panelButtonServer
            // 
            this.panelButtonServer.BackColor = System.Drawing.Color.LightGray;
            this.panelButtonServer.Controls.Add(this.buttonAddServer);
            this.panelButtonServer.Controls.Add(this.buttonDeleteServer);
            this.panelButtonServer.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtonServer.Location = new System.Drawing.Point(0, 0);
            this.panelButtonServer.Name = "panelButtonServer";
            this.panelButtonServer.Padding = new System.Windows.Forms.Padding(5);
            this.panelButtonServer.Size = new System.Drawing.Size(200, 60);
            this.panelButtonServer.TabIndex = 0;
            // 
            // buttonAddServer
            // 
            this.buttonAddServer.ImageOptions.Image = global::PrintToPACSDemo.Properties.Resources.client_add_32;
            this.buttonAddServer.Location = new System.Drawing.Point(6, 6);
            this.buttonAddServer.Name = "buttonAddServer";
            this.buttonAddServer.Size = new System.Drawing.Size(40, 39);
            this.buttonAddServer.TabIndex = 7;
            this.buttonAddServer.Click += new System.EventHandler(this.buttonAddServer_Click);
            // 
            // buttonDeleteServer
            // 
            this.buttonDeleteServer.CausesValidation = false;
            this.buttonDeleteServer.ImageOptions.Image = global::PrintToPACSDemo.Properties.Resources.client_remove_32;
            this.buttonDeleteServer.Location = new System.Drawing.Point(49, 6);
            this.buttonDeleteServer.Name = "buttonDeleteServer";
            this.buttonDeleteServer.Size = new System.Drawing.Size(40, 39);
            this.buttonDeleteServer.TabIndex = 8;
            this.buttonDeleteServer.Click += new System.EventHandler(this.buttonDeleteServer_Click);
            // 
            // _groupBoxSecurity
            // 
            this._groupBoxSecurity.Controls.Add(this._labelCertificate);
            this._groupBoxSecurity.Controls.Add(this._labelHint);
            this._groupBoxSecurity.Controls.Add(this._buttonClientCertificate);
            this._groupBoxSecurity.Controls.Add(this._textBoxClientCertificate);
            this._groupBoxSecurity.Controls.Add(this._textBoxKeyPassword);
            this._groupBoxSecurity.Controls.Add(this._labelPrivateKeyPassword);
            this._groupBoxSecurity.Controls.Add(this._labelPrivateKey);
            this._groupBoxSecurity.Controls.Add(this._textBoxPrivateKey);
            this._groupBoxSecurity.Controls.Add(this._buttonPrivateKey);
            this._groupBoxSecurity.Dock = System.Windows.Forms.DockStyle.Top;
            this._groupBoxSecurity.Location = new System.Drawing.Point(3, 75);
            this._groupBoxSecurity.Name = "_groupBoxSecurity";
            this._groupBoxSecurity.Size = new System.Drawing.Size(660, 145);
            this._groupBoxSecurity.TabIndex = 16;
            this._groupBoxSecurity.TabStop = false;
            this._groupBoxSecurity.Text = "Bảo mật";
            // 
            // _tbServers
            // 
            this._tbServers.Controls.Add(this._tbSCPQuerypage);
            this._tbServers.Controls.Add(this._tbMWLQueryPage);
            this._tbServers.Controls.Add(this._tbStorePage);
            this._tbServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tbServers.Location = new System.Drawing.Point(3, 220);
            this._tbServers.Name = "_tbServers";
            this._tbServers.SelectedIndex = 0;
            this._tbServers.Size = new System.Drawing.Size(660, 172);
            this._tbServers.TabIndex = 17;
            this._tbServers.SelectedIndexChanged += new System.EventHandler(this._tbServers_SelectedIndexChanged);
            this._tbServers.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this._tbServers_Selecting);
            // 
            // _tbSCPQuerypage
            // 
            this._tbSCPQuerypage.AutoScroll = true;
            this._tbSCPQuerypage.Location = new System.Drawing.Point(4, 24);
            this._tbSCPQuerypage.Name = "_tbSCPQuerypage";
            this._tbSCPQuerypage.Padding = new System.Windows.Forms.Padding(3);
            this._tbSCPQuerypage.Size = new System.Drawing.Size(764, 246);
            this._tbSCPQuerypage.TabIndex = 0;
            this._tbSCPQuerypage.Text = "Máy chủ Query";
            this._tbSCPQuerypage.UseVisualStyleBackColor = true;
            // 
            // _tbMWLQueryPage
            // 
            this._tbMWLQueryPage.Location = new System.Drawing.Point(4, 24);
            this._tbMWLQueryPage.Name = "_tbMWLQueryPage";
            this._tbMWLQueryPage.Padding = new System.Windows.Forms.Padding(3);
            this._tbMWLQueryPage.Size = new System.Drawing.Size(764, 246);
            this._tbMWLQueryPage.TabIndex = 1;
            this._tbMWLQueryPage.Text = "Máy chủ Worklist";
            this._tbMWLQueryPage.UseVisualStyleBackColor = true;
            // 
            // _tbStorePage
            // 
            this._tbStorePage.Location = new System.Drawing.Point(4, 24);
            this._tbStorePage.Name = "_tbStorePage";
            this._tbStorePage.Padding = new System.Windows.Forms.Padding(3);
            this._tbStorePage.Size = new System.Drawing.Size(764, 246);
            this._tbStorePage.TabIndex = 2;
            this._tbStorePage.Text = "Máy chủ PACS";
            this._tbStorePage.UseVisualStyleBackColor = true;
            // 
            // _tbOptions
            // 
            this._tbOptions.Appearance.BackColor = System.Drawing.Color.Gray;
            this._tbOptions.Appearance.Options.UseBackColor = true;
            this._tbOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tbOptions.Location = new System.Drawing.Point(0, 0);
            this._tbOptions.Name = "_tbOptions";
            this._tbOptions.SelectedTabPage = this._tpApplicationOptions;
            this._tbOptions.Size = new System.Drawing.Size(786, 529);
            this._tbOptions.TabIndex = 18;
            this._tbOptions.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this._tpApplicationOptions,
            this._tpDicomOptions,
            this._tpCameraOptions,
            this._tbShortcutSettings});
            // 
            // _tpApplicationOptions
            // 
            this._tpApplicationOptions.Controls.Add(this.panelControl2);
            this._tpApplicationOptions.Controls.Add(this._ckAutoDelete);
            this._tpApplicationOptions.Controls.Add(this._gpDicomType);
            this._tpApplicationOptions.Name = "_tpApplicationOptions";
            this._tpApplicationOptions.Padding = new System.Windows.Forms.Padding(3);
            this._tpApplicationOptions.Size = new System.Drawing.Size(778, 497);
            this._tpApplicationOptions.Text = "Tùy chọn ứng dụng";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this._lblPrinterName);
            this.panelControl2.Controls.Add(this._txtPrinterName);
            this.panelControl2.Controls.Add(this._btnRename);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(3, 3);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(772, 59);
            this.panelControl2.TabIndex = 32;
            // 
            // _lblPrinterName
            // 
            this._lblPrinterName.Location = new System.Drawing.Point(20, 20);
            this._lblPrinterName.Name = "_lblPrinterName";
            this._lblPrinterName.Size = new System.Drawing.Size(170, 15);
            this._lblPrinterName.TabIndex = 0;
            this._lblPrinterName.Text = "Trình điều khiển máy in DICOM";
            // 
            // _txtPrinterName
            // 
            this._txtPrinterName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtPrinterName.Location = new System.Drawing.Point(209, 17);
            this._txtPrinterName.Name = "_txtPrinterName";
            this._txtPrinterName.Properties.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this._txtPrinterName.Properties.Appearance.Options.UseBackColor = true;
            this._txtPrinterName.Properties.ReadOnly = true;
            this._txtPrinterName.Size = new System.Drawing.Size(427, 22);
            this._txtPrinterName.TabIndex = 1;
            // 
            // _btnRename
            // 
            this._btnRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnRename.Location = new System.Drawing.Point(642, 16);
            this._btnRename.Name = "_btnRename";
            this._btnRename.Size = new System.Drawing.Size(125, 23);
            this._btnRename.TabIndex = 22;
            this._btnRename.Text = "Đổi tên";
            // 
            // _ckAutoDelete
            // 
            this._ckAutoDelete.Location = new System.Drawing.Point(27, 609);
            this._ckAutoDelete.Name = "_ckAutoDelete";
            this._ckAutoDelete.Properties.Caption = "Auto delete Images after successful transfer";
            this._ckAutoDelete.Size = new System.Drawing.Size(400, 20);
            this._ckAutoDelete.TabIndex = 26;
            // 
            // _gpDicomType
            // 
            this._gpDicomType.Controls.Add(this._btnBrowseSCPDF);
            this._gpDicomType.Controls.Add(this._btnBrowseTempDir);
            this._gpDicomType.Controls.Add(this.label3);
            this._gpDicomType.Controls.Add(this._btnBrowseSCGray);
            this._gpDicomType.Controls.Add(this._btnBrowseSCColor);
            this._gpDicomType.Controls.Add(this._btnBrowseSC);
            this._gpDicomType.Controls.Add(this.label2);
            this._gpDicomType.Controls.Add(this._txtTempDir);
            this._gpDicomType.Controls.Add(this._cmbSCColor);
            this._gpDicomType.Controls.Add(this._cmbSCGray);
            this._gpDicomType.Controls.Add(this._cmbSC);
            this._gpDicomType.Controls.Add(this.label1);
            this._gpDicomType.Controls.Add(this._rdPDF);
            this._gpDicomType.Controls.Add(this._rdGrayScale);
            this._gpDicomType.Controls.Add(this._rdColored);
            this._gpDicomType.Controls.Add(this._rdSecondaryCapture);
            this._gpDicomType.Controls.Add(this._txtSCPDF);
            this._gpDicomType.Controls.Add(this._txtSCGray);
            this._gpDicomType.Controls.Add(this._txtSCColor);
            this._gpDicomType.Controls.Add(this._txtSC);
            this._gpDicomType.Location = new System.Drawing.Point(6, 81);
            this._gpDicomType.Name = "_gpDicomType";
            this._gpDicomType.Size = new System.Drawing.Size(769, 201);
            this._gpDicomType.TabIndex = 7;
            this._gpDicomType.TabStop = false;
            this._gpDicomType.Text = "Loại DICOM";
            // 
            // _btnBrowseSCPDF
            // 
            this._btnBrowseSCPDF.Location = new System.Drawing.Point(323, 125);
            this._btnBrowseSCPDF.Name = "_btnBrowseSCPDF";
            this._btnBrowseSCPDF.Size = new System.Drawing.Size(38, 21);
            this._btnBrowseSCPDF.TabIndex = 30;
            this._btnBrowseSCPDF.Text = "...";
            // 
            // _btnBrowseTempDir
            // 
            this._btnBrowseTempDir.Location = new System.Drawing.Point(315, 156);
            this._btnBrowseTempDir.Name = "_btnBrowseTempDir";
            this._btnBrowseTempDir.Size = new System.Drawing.Size(46, 25);
            this._btnBrowseTempDir.TabIndex = 31;
            this._btnBrowseTempDir.Text = "...";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(18, 161);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 15);
            this.label3.TabIndex = 23;
            this.label3.Text = "Thư mục tạm thời DICOM";
            // 
            // _btnBrowseSCGray
            // 
            this._btnBrowseSCGray.Location = new System.Drawing.Point(323, 95);
            this._btnBrowseSCGray.Name = "_btnBrowseSCGray";
            this._btnBrowseSCGray.Size = new System.Drawing.Size(38, 21);
            this._btnBrowseSCGray.TabIndex = 29;
            this._btnBrowseSCGray.Text = "...";
            // 
            // _btnBrowseSCColor
            // 
            this._btnBrowseSCColor.Location = new System.Drawing.Point(323, 66);
            this._btnBrowseSCColor.Name = "_btnBrowseSCColor";
            this._btnBrowseSCColor.Size = new System.Drawing.Size(38, 21);
            this._btnBrowseSCColor.TabIndex = 28;
            this._btnBrowseSCColor.Text = "...";
            // 
            // _btnBrowseSC
            // 
            this._btnBrowseSC.Location = new System.Drawing.Point(323, 36);
            this._btnBrowseSC.Name = "_btnBrowseSC";
            this._btnBrowseSC.Size = new System.Drawing.Size(38, 21);
            this._btnBrowseSC.TabIndex = 27;
            this._btnBrowseSC.Text = "...";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(626, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 15);
            this.label2.TabIndex = 20;
            this.label2.Text = "Bản nén";
            // 
            // _txtTempDir
            // 
            this._txtTempDir.Location = new System.Drawing.Point(374, 158);
            this._txtTempDir.Name = "_txtTempDir";
            this._txtTempDir.Size = new System.Drawing.Size(377, 22);
            this._txtTempDir.TabIndex = 2;
            this._txtTempDir.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // _cmbSCColor
            // 
            this._cmbSCColor.Location = new System.Drawing.Point(626, 67);
            this._cmbSCColor.Name = "_cmbSCColor";
            this._cmbSCColor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._cmbSCColor.Size = new System.Drawing.Size(120, 22);
            this._cmbSCColor.TabIndex = 19;
            // 
            // _cmbSCGray
            // 
            this._cmbSCGray.Location = new System.Drawing.Point(626, 95);
            this._cmbSCGray.Name = "_cmbSCGray";
            this._cmbSCGray.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._cmbSCGray.Size = new System.Drawing.Size(120, 22);
            this._cmbSCGray.TabIndex = 18;
            // 
            // _cmbSC
            // 
            this._cmbSC.Location = new System.Drawing.Point(626, 38);
            this._cmbSC.Name = "_cmbSC";
            this._cmbSC.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._cmbSC.Size = new System.Drawing.Size(120, 22);
            this._cmbSC.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(323, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "Bản mẫu";
            // 
            // _rdPDF
            // 
            this._rdPDF.AutoSize = true;
            this._rdPDF.Location = new System.Drawing.Point(18, 126);
            this._rdPDF.Name = "_rdPDF";
            this._rdPDF.Size = new System.Drawing.Size(92, 19);
            this._rdPDF.TabIndex = 14;
            this._rdPDF.Text = "DICOM PDF";
            this._rdPDF.UseVisualStyleBackColor = true;
            // 
            // _rdGrayScale
            // 
            this._rdGrayScale.AutoSize = true;
            this._rdGrayScale.Location = new System.Drawing.Point(18, 97);
            this._rdGrayScale.Name = "_rdGrayScale";
            this._rdGrayScale.Size = new System.Drawing.Size(258, 19);
            this._rdGrayScale.TabIndex = 13;
            this._rdGrayScale.Text = "Chụp thứ cấp Đa khung hình thang độ xám";
            this._rdGrayScale.UseVisualStyleBackColor = true;
            // 
            // _rdColored
            // 
            this._rdColored.AutoSize = true;
            this._rdColored.Location = new System.Drawing.Point(18, 67);
            this._rdColored.Name = "_rdColored";
            this._rdColored.Size = new System.Drawing.Size(177, 19);
            this._rdColored.TabIndex = 12;
            this._rdColored.Text = "Chụp phụ nhiều khung màu";
            this._rdColored.UseVisualStyleBackColor = true;
            // 
            // _rdSecondaryCapture
            // 
            this._rdSecondaryCapture.AutoSize = true;
            this._rdSecondaryCapture.Checked = true;
            this._rdSecondaryCapture.Location = new System.Drawing.Point(18, 39);
            this._rdSecondaryCapture.Name = "_rdSecondaryCapture";
            this._rdSecondaryCapture.Size = new System.Drawing.Size(78, 19);
            this._rdSecondaryCapture.TabIndex = 11;
            this._rdSecondaryCapture.TabStop = true;
            this._rdSecondaryCapture.Text = "Chụp phụ";
            this._rdSecondaryCapture.UseVisualStyleBackColor = true;
            // 
            // _txtSCPDF
            // 
            this._txtSCPDF.Location = new System.Drawing.Point(374, 125);
            this._txtSCPDF.Name = "_txtSCPDF";
            this._txtSCPDF.Size = new System.Drawing.Size(230, 22);
            this._txtSCPDF.TabIndex = 10;
            this._txtSCPDF.Tag = "3";
            // 
            // _txtSCGray
            // 
            this._txtSCGray.Location = new System.Drawing.Point(374, 95);
            this._txtSCGray.Name = "_txtSCGray";
            this._txtSCGray.Size = new System.Drawing.Size(230, 22);
            this._txtSCGray.TabIndex = 9;
            this._txtSCGray.Tag = "2";
            // 
            // _txtSCColor
            // 
            this._txtSCColor.Location = new System.Drawing.Point(374, 67);
            this._txtSCColor.Name = "_txtSCColor";
            this._txtSCColor.Size = new System.Drawing.Size(230, 22);
            this._txtSCColor.TabIndex = 8;
            this._txtSCColor.Tag = "1";
            this._txtSCColor.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // _txtSC
            // 
            this._txtSC.Location = new System.Drawing.Point(374, 38);
            this._txtSC.Name = "_txtSC";
            this._txtSC.Size = new System.Drawing.Size(230, 22);
            this._txtSC.TabIndex = 7;
            this._txtSC.Tag = "0";
            // 
            // _tpDicomOptions
            // 
            this._tpDicomOptions.AutoScroll = true;
            this._tpDicomOptions.Controls.Add(this._tbServers);
            this._tpDicomOptions.Controls.Add(this._groupBoxSecurity);
            this._tpDicomOptions.Controls.Add(this._groupBoxClient);
            this._tpDicomOptions.Name = "_tpDicomOptions";
            this._tpDicomOptions.Padding = new System.Windows.Forms.Padding(3);
            this._tpDicomOptions.Size = new System.Drawing.Size(666, 395);
            this._tpDicomOptions.Text = "Cấu hình PACS";
            // 
            // _tpCameraOptions
            // 
            this._tpCameraOptions.Controls.Add(this.panelControl9);
            this._tpCameraOptions.Controls.Add(this.panelControl6);
            this._tpCameraOptions.Controls.Add(this.tableLayoutPanel1);
            this._tpCameraOptions.Name = "_tpCameraOptions";
            this._tpCameraOptions.Size = new System.Drawing.Size(778, 497);
            this._tpCameraOptions.Text = "Cấu hình camera";
            // 
            // panelControl9
            // 
            this.panelControl9.Controls.Add(this.tabControl2);
            this.panelControl9.Controls.Add(this._panelCamera);
            this.panelControl9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl9.Location = new System.Drawing.Point(0, 234);
            this.panelControl9.Name = "panelControl9";
            this.panelControl9.Size = new System.Drawing.Size(778, 263);
            this.panelControl9.TabIndex = 138;
            // 
            // tabControl2
            // 
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(2, 2);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedTabPage = this.tabPage3;
            this.tabControl2.Size = new System.Drawing.Size(515, 259);
            this.tabControl2.TabIndex = 137;
            this.tabControl2.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabPage3,
            this.tabPage4,
            this.tabPage5});
            // 
            // tabPage3
            // 
            this.tabPage3.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent;
            this.tabPage3.Appearance.PageClient.Options.UseBackColor = true;
            this.tabPage3.Controls.Add(this.cbZoom);
            this.tabPage3.Controls.Add(this.btEffZoomRight);
            this.tabPage3.Controls.Add(this.btEffZoomLeft);
            this.tabPage3.Controls.Add(this.btEffZoomDown);
            this.tabPage3.Controls.Add(this.btEffZoomOut);
            this.tabPage3.Controls.Add(this.btEffZoomIn);
            this.tabPage3.Controls.Add(this.btEffZoomUp);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tabPage3.Size = new System.Drawing.Size(507, 227);
            this.tabPage3.Text = "Thu phóng";
            // 
            // cbZoom
            // 
            this.cbZoom.Location = new System.Drawing.Point(9, 10);
            this.cbZoom.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cbZoom.Name = "cbZoom";
            this.cbZoom.Properties.Caption = "Cho phép";
            this.cbZoom.Size = new System.Drawing.Size(95, 20);
            this.cbZoom.TabIndex = 75;
            // 
            // btEffZoomRight
            // 
            this.btEffZoomRight.Location = new System.Drawing.Point(363, 12);
            this.btEffZoomRight.Name = "btEffZoomRight";
            this.btEffZoomRight.Size = new System.Drawing.Size(42, 146);
            this.btEffZoomRight.TabIndex = 5;
            this.btEffZoomRight.Text = "R";
            // 
            // btEffZoomLeft
            // 
            this.btEffZoomLeft.Location = new System.Drawing.Point(166, 12);
            this.btEffZoomLeft.Name = "btEffZoomLeft";
            this.btEffZoomLeft.Size = new System.Drawing.Size(45, 146);
            this.btEffZoomLeft.TabIndex = 4;
            this.btEffZoomLeft.Text = "L";
            // 
            // btEffZoomDown
            // 
            this.btEffZoomDown.Location = new System.Drawing.Point(224, 122);
            this.btEffZoomDown.Name = "btEffZoomDown";
            this.btEffZoomDown.Size = new System.Drawing.Size(133, 37);
            this.btEffZoomDown.TabIndex = 3;
            this.btEffZoomDown.Text = "Down";
            // 
            // btEffZoomOut
            // 
            this.btEffZoomOut.Location = new System.Drawing.Point(296, 56);
            this.btEffZoomOut.Name = "btEffZoomOut";
            this.btEffZoomOut.Size = new System.Drawing.Size(61, 60);
            this.btEffZoomOut.TabIndex = 2;
            this.btEffZoomOut.Text = "-";
            // 
            // btEffZoomIn
            // 
            this.btEffZoomIn.Location = new System.Drawing.Point(224, 56);
            this.btEffZoomIn.Name = "btEffZoomIn";
            this.btEffZoomIn.Size = new System.Drawing.Size(65, 60);
            this.btEffZoomIn.TabIndex = 1;
            this.btEffZoomIn.Text = "+";
            // 
            // btEffZoomUp
            // 
            this.btEffZoomUp.Location = new System.Drawing.Point(224, 12);
            this.btEffZoomUp.Name = "btEffZoomUp";
            this.btEffZoomUp.Size = new System.Drawing.Size(133, 37);
            this.btEffZoomUp.TabIndex = 0;
            this.btEffZoomUp.Text = "Up";
            // 
            // tabPage4
            // 
            this.tabPage4.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent;
            this.tabPage4.Appearance.PageClient.Options.UseBackColor = true;
            this.tabPage4.Controls.Add(this.groupBox4);
            this.tabPage4.Controls.Add(this.groupBox5);
            this.tabPage4.Controls.Add(this.cbPan);
            this.tabPage4.Controls.Add(this.groupBox3);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tabPage4.Size = new System.Drawing.Size(396, 131);
            this.tabPage4.Text = "Pan";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.Controls.Add(this.edPanSourceHeight);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.edPanSourceTop);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.edPanSourceWidth);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.edPanSourceLeft);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Location = new System.Drawing.Point(289, 34);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(215, 180);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Nguồn";
            // 
            // edPanSourceHeight
            // 
            this.edPanSourceHeight.EditValue = "480";
            this.edPanSourceHeight.Location = new System.Drawing.Point(51, 85);
            this.edPanSourceHeight.Name = "edPanSourceHeight";
            this.edPanSourceHeight.Size = new System.Drawing.Size(63, 22);
            this.edPanSourceHeight.TabIndex = 7;
            // 
            // label14
            // 
            this.label14.Appearance.Options.UseForeColor = true;
            this.label14.Location = new System.Drawing.Point(7, 90);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(22, 15);
            this.label14.TabIndex = 6;
            this.label14.Text = "Cao";
            // 
            // edPanSourceTop
            // 
            this.edPanSourceTop.EditValue = "0";
            this.edPanSourceTop.Location = new System.Drawing.Point(51, 57);
            this.edPanSourceTop.Name = "edPanSourceTop";
            this.edPanSourceTop.Size = new System.Drawing.Size(63, 22);
            this.edPanSourceTop.TabIndex = 5;
            // 
            // label15
            // 
            this.label15.Appearance.Options.UseForeColor = true;
            this.label15.Location = new System.Drawing.Point(7, 61);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(25, 15);
            this.label15.TabIndex = 4;
            this.label15.Text = "Trên";
            // 
            // edPanSourceWidth
            // 
            this.edPanSourceWidth.EditValue = "640";
            this.edPanSourceWidth.Location = new System.Drawing.Point(51, 113);
            this.edPanSourceWidth.Name = "edPanSourceWidth";
            this.edPanSourceWidth.Size = new System.Drawing.Size(63, 22);
            this.edPanSourceWidth.TabIndex = 3;
            // 
            // label12
            // 
            this.label12.Appearance.Options.UseForeColor = true;
            this.label12.Location = new System.Drawing.Point(6, 116);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(37, 15);
            this.label12.TabIndex = 2;
            this.label12.Text = "Ngang";
            // 
            // edPanSourceLeft
            // 
            this.edPanSourceLeft.EditValue = "0";
            this.edPanSourceLeft.Location = new System.Drawing.Point(51, 27);
            this.edPanSourceLeft.Name = "edPanSourceLeft";
            this.edPanSourceLeft.Size = new System.Drawing.Size(63, 22);
            this.edPanSourceLeft.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.Appearance.Options.UseForeColor = true;
            this.label13.Location = new System.Drawing.Point(7, 30);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(21, 15);
            this.label13.TabIndex = 0;
            this.label13.Text = "Trái";
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Transparent;
            this.groupBox5.Controls.Add(this.edPanDestHeight);
            this.groupBox5.Controls.Add(this.label16);
            this.groupBox5.Controls.Add(this.edPanDestTop);
            this.groupBox5.Controls.Add(this.label17);
            this.groupBox5.Controls.Add(this.edPanDestWidth);
            this.groupBox5.Controls.Add(this.label18);
            this.groupBox5.Controls.Add(this.edPanDestLeft);
            this.groupBox5.Controls.Add(this.label19);
            this.groupBox5.Location = new System.Drawing.Point(10, 112);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(273, 102);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Đích";
            // 
            // edPanDestHeight
            // 
            this.edPanDestHeight.EditValue = "240";
            this.edPanDestHeight.Location = new System.Drawing.Point(173, 57);
            this.edPanDestHeight.Name = "edPanDestHeight";
            this.edPanDestHeight.Size = new System.Drawing.Size(61, 22);
            this.edPanDestHeight.TabIndex = 7;
            // 
            // label16
            // 
            this.label16.Appearance.Options.UseForeColor = true;
            this.label16.Location = new System.Drawing.Point(116, 61);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(22, 15);
            this.label16.TabIndex = 6;
            this.label16.Text = "Cao";
            // 
            // edPanDestTop
            // 
            this.edPanDestTop.EditValue = "0";
            this.edPanDestTop.Location = new System.Drawing.Point(51, 57);
            this.edPanDestTop.Name = "edPanDestTop";
            this.edPanDestTop.Size = new System.Drawing.Size(52, 22);
            this.edPanDestTop.TabIndex = 5;
            // 
            // label17
            // 
            this.label17.Appearance.Options.UseForeColor = true;
            this.label17.Location = new System.Drawing.Point(7, 61);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(25, 15);
            this.label17.TabIndex = 4;
            this.label17.Text = "Phải";
            // 
            // edPanDestWidth
            // 
            this.edPanDestWidth.EditValue = "320";
            this.edPanDestWidth.Location = new System.Drawing.Point(173, 27);
            this.edPanDestWidth.Name = "edPanDestWidth";
            this.edPanDestWidth.Size = new System.Drawing.Size(61, 22);
            this.edPanDestWidth.TabIndex = 3;
            // 
            // label18
            // 
            this.label18.Appearance.Options.UseForeColor = true;
            this.label18.Location = new System.Drawing.Point(116, 30);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(37, 15);
            this.label18.TabIndex = 2;
            this.label18.Text = "Ngang";
            // 
            // edPanDestLeft
            // 
            this.edPanDestLeft.EditValue = "0";
            this.edPanDestLeft.Location = new System.Drawing.Point(51, 27);
            this.edPanDestLeft.Name = "edPanDestLeft";
            this.edPanDestLeft.Size = new System.Drawing.Size(52, 22);
            this.edPanDestLeft.TabIndex = 1;
            // 
            // label19
            // 
            this.label19.Appearance.Options.UseForeColor = true;
            this.label19.Location = new System.Drawing.Point(7, 30);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(21, 15);
            this.label19.TabIndex = 0;
            this.label19.Text = "Trái";
            // 
            // cbPan
            // 
            this.cbPan.Location = new System.Drawing.Point(9, 10);
            this.cbPan.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cbPan.Name = "cbPan";
            this.cbPan.Properties.Caption = "Cho phép";
            this.cbPan.Size = new System.Drawing.Size(78, 20);
            this.cbPan.TabIndex = 62;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.edPanStopTime);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.edPanStartTime);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Location = new System.Drawing.Point(8, 34);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(275, 72);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Thời lượng";
            // 
            // edPanStopTime
            // 
            this.edPanStopTime.EditValue = "15000";
            this.edPanStopTime.Location = new System.Drawing.Point(173, 27);
            this.edPanStopTime.Name = "edPanStopTime";
            this.edPanStopTime.Size = new System.Drawing.Size(63, 22);
            this.edPanStopTime.TabIndex = 3;
            // 
            // label11
            // 
            this.label11.Appearance.Options.UseForeColor = true;
            this.label11.Location = new System.Drawing.Point(118, 30);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 15);
            this.label11.TabIndex = 2;
            this.label11.Text = "Kết thúc";
            // 
            // edPanStartTime
            // 
            this.edPanStartTime.EditValue = "5000";
            this.edPanStartTime.Location = new System.Drawing.Point(51, 27);
            this.edPanStartTime.Name = "edPanStartTime";
            this.edPanStartTime.Size = new System.Drawing.Size(54, 22);
            this.edPanStartTime.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.Appearance.Options.UseForeColor = true;
            this.label10.Location = new System.Drawing.Point(7, 30);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 15);
            this.label10.TabIndex = 0;
            this.label10.Text = "Bắt đầu";
            // 
            // tabPage5
            // 
            this.tabPage5.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent;
            this.tabPage5.Appearance.PageClient.Options.UseBackColor = true;
            this.tabPage5.Controls.Add(this.tbLiveRotationAngle);
            this.tabPage5.Controls.Add(this.cbLiveRotation);
            this.tabPage5.Controls.Add(this.labelLiveRotationAngle);
            this.tabPage5.Controls.Add(this.label21);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(396, 131);
            this.tabPage5.Text = "Live rotation";
            // 
            // tbLiveRotationAngle
            // 
            this.tbLiveRotationAngle.EditValue = null;
            this.tbLiveRotationAngle.Location = new System.Drawing.Point(121, 37);
            this.tbLiveRotationAngle.MinimumSize = new System.Drawing.Size(0, 32);
            this.tbLiveRotationAngle.Name = "tbLiveRotationAngle";
            this.tbLiveRotationAngle.Properties.Maximum = 360;
            this.tbLiveRotationAngle.Size = new System.Drawing.Size(328, 32);
            this.tbLiveRotationAngle.TabIndex = 66;
            // 
            // cbLiveRotation
            // 
            this.cbLiveRotation.Location = new System.Drawing.Point(8, 8);
            this.cbLiveRotation.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cbLiveRotation.Name = "cbLiveRotation";
            this.cbLiveRotation.Properties.Caption = "Cho phép";
            this.cbLiveRotation.Size = new System.Drawing.Size(81, 20);
            this.cbLiveRotation.TabIndex = 65;
            // 
            // labelLiveRotationAngle
            // 
            this.labelLiveRotationAngle.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLiveRotationAngle.Appearance.Options.UseFont = true;
            this.labelLiveRotationAngle.Location = new System.Drawing.Point(465, 41);
            this.labelLiveRotationAngle.Margin = new System.Windows.Forms.Padding(13, 0, 5, 0);
            this.labelLiveRotationAngle.Name = "labelLiveRotationAngle";
            this.labelLiveRotationAngle.Size = new System.Drawing.Size(8, 18);
            this.labelLiveRotationAngle.TabIndex = 64;
            this.labelLiveRotationAngle.Text = "0";
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(8, 43);
            this.label21.Margin = new System.Windows.Forms.Padding(13, 0, 5, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(31, 15);
            this.label21.TabIndex = 62;
            this.label21.Text = "Angle";
            // 
            // _panelCamera
            // 
            this._panelCamera.Appearance.BackColor = System.Drawing.Color.Black;
            this._panelCamera.Appearance.Options.UseBackColor = true;
            this._panelCamera.Dock = System.Windows.Forms.DockStyle.Right;
            this._panelCamera.Location = new System.Drawing.Point(517, 2);
            this._panelCamera.Name = "_panelCamera";
            this._panelCamera.Size = new System.Drawing.Size(259, 259);
            this._panelCamera.TabIndex = 138;
            // 
            // panelControl6
            // 
            this.panelControl6.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.panelControl6.Appearance.Options.UseBackColor = true;
            this.panelControl6.Controls.Add(this._btnStopCamera);
            this.panelControl6.Controls.Add(this._btnPreview);
            this.panelControl6.Controls.Add(this.label24);
            this.panelControl6.Controls.Add(this.btOutputConfigure);
            this.panelControl6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl6.Location = new System.Drawing.Point(0, 183);
            this.panelControl6.Name = "panelControl6";
            this.panelControl6.Size = new System.Drawing.Size(778, 51);
            this.panelControl6.TabIndex = 136;
            // 
            // _btnPreview
            // 
            this._btnPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnPreview.Appearance.ForeColor = System.Drawing.Color.Black;
            this._btnPreview.Appearance.Options.UseForeColor = true;
            this._btnPreview.Location = new System.Drawing.Point(657, 10);
            this._btnPreview.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this._btnPreview.Name = "_btnPreview";
            this._btnPreview.Size = new System.Drawing.Size(111, 30);
            this._btnPreview.TabIndex = 131;
            this._btnPreview.Text = "Xem trước";
            this._btnPreview.ToolTipAnchor = DevExpress.Utils.ToolTipAnchor.Cursor;
            this._btnPreview.Click += new System.EventHandler(this._btnPreview_Click);
            // 
            // label24
            // 
            this.label24.Location = new System.Drawing.Point(7, 17);
            this.label24.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(315, 15);
            this.label24.TabIndex = 129;
            this.label24.Text = "Sử dụng hộp thoại hoặc mã để cấu hình cài đặt định dạng";
            // 
            // btOutputConfigure
            // 
            this.btOutputConfigure.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btOutputConfigure.Appearance.Options.UseForeColor = true;
            this.btOutputConfigure.Location = new System.Drawing.Point(391, 10);
            this.btOutputConfigure.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btOutputConfigure.Name = "btOutputConfigure";
            this.btOutputConfigure.Size = new System.Drawing.Size(111, 30);
            this.btOutputConfigure.TabIndex = 130;
            this.btOutputConfigure.Text = "Cấu hình ...";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panelControl4, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelControl3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelControl5, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(778, 183);
            this.tableLayoutPanel1.TabIndex = 135;
            // 
            // panelControl4
            // 
            this.panelControl4.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.panelControl4.Appearance.Options.UseBackColor = true;
            this.panelControl4.Controls.Add(this.cbAudioInputDevice);
            this.panelControl4.Controls.Add(this.label5);
            this.panelControl4.Controls.Add(this.cbAudioInputFormat);
            this.panelControl4.Controls.Add(this.label6);
            this.panelControl4.Controls.Add(this.cbAudioInputLine);
            this.panelControl4.Controls.Add(this.label9);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl4.Location = new System.Drawing.Point(392, 3);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(383, 127);
            this.panelControl4.TabIndex = 1;
            // 
            // cbAudioInputDevice
            // 
            this.cbAudioInputDevice.Location = new System.Drawing.Point(122, 6);
            this.cbAudioInputDevice.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cbAudioInputDevice.Name = "cbAudioInputDevice";
            this.cbAudioInputDevice.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbAudioInputDevice.Size = new System.Drawing.Size(254, 22);
            this.cbAudioInputDevice.TabIndex = 119;
            this.cbAudioInputDevice.SelectedIndexChanged += new System.EventHandler(this.cbAudioInputDevice_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(7, 9);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 15);
            this.label5.TabIndex = 118;
            this.label5.Text = "Thiết bị âm thanh;";
            // 
            // cbAudioInputFormat
            // 
            this.cbAudioInputFormat.Location = new System.Drawing.Point(122, 36);
            this.cbAudioInputFormat.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cbAudioInputFormat.Name = "cbAudioInputFormat";
            this.cbAudioInputFormat.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbAudioInputFormat.Size = new System.Drawing.Size(254, 22);
            this.cbAudioInputFormat.TabIndex = 121;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 39);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 15);
            this.label6.TabIndex = 120;
            this.label6.Text = "Tốc độ âm thanh:";
            // 
            // cbAudioInputLine
            // 
            this.cbAudioInputLine.Location = new System.Drawing.Point(122, 66);
            this.cbAudioInputLine.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cbAudioInputLine.Name = "cbAudioInputLine";
            this.cbAudioInputLine.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbAudioInputLine.Size = new System.Drawing.Size(254, 22);
            this.cbAudioInputLine.TabIndex = 125;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(7, 69);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(103, 15);
            this.label9.TabIndex = 124;
            this.label9.Text = "Đầu vào âm thanh:";
            // 
            // panelControl3
            // 
            this.panelControl3.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.panelControl3.Appearance.Options.UseBackColor = true;
            this.panelControl3.Controls.Add(this.labelControl1);
            this.panelControl3.Controls.Add(this.cbVideoInputDevice);
            this.panelControl3.Controls.Add(this.buttonSettingCam);
            this.panelControl3.Controls.Add(this.label8);
            this.panelControl3.Controls.Add(this.cbVideoInputFormat);
            this.panelControl3.Controls.Add(this.cbVideoInputFrameRate);
            this.panelControl3.Controls.Add(this.cbOutputFormat);
            this.panelControl3.Controls.Add(this.labelControl2);
            this.panelControl3.Controls.Add(this.label23);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(3, 3);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(383, 127);
            this.panelControl3.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(7, 9);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(43, 15);
            this.labelControl1.TabIndex = 117;
            this.labelControl1.Text = "Thiết bị:";
            // 
            // cbVideoInputDevice
            // 
            this.cbVideoInputDevice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbVideoInputDevice.Location = new System.Drawing.Point(122, 6);
            this.cbVideoInputDevice.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cbVideoInputDevice.Name = "cbVideoInputDevice";
            this.cbVideoInputDevice.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbVideoInputDevice.Size = new System.Drawing.Size(173, 22);
            this.cbVideoInputDevice.TabIndex = 137;
            this.cbVideoInputDevice.SelectedIndexChanged += new System.EventHandler(this.cbVideoInputDevice_SelectedIndexChanged);
            // 
            // buttonSettingCam
            // 
            this.buttonSettingCam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSettingCam.Appearance.ForeColor = System.Drawing.Color.Black;
            this.buttonSettingCam.Appearance.Options.UseForeColor = true;
            this.buttonSettingCam.Location = new System.Drawing.Point(305, 6);
            this.buttonSettingCam.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonSettingCam.Name = "buttonSettingCam";
            this.buttonSettingCam.Size = new System.Drawing.Size(71, 22);
            this.buttonSettingCam.TabIndex = 126;
            this.buttonSettingCam.Text = "Settings";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(7, 39);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 15);
            this.label8.TabIndex = 123;
            this.label8.Text = "Định dạng:";
            // 
            // cbVideoInputFormat
            // 
            this.cbVideoInputFormat.Location = new System.Drawing.Point(122, 36);
            this.cbVideoInputFormat.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cbVideoInputFormat.Name = "cbVideoInputFormat";
            this.cbVideoInputFormat.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbVideoInputFormat.Size = new System.Drawing.Size(254, 22);
            this.cbVideoInputFormat.TabIndex = 122;
            this.cbVideoInputFormat.SelectedIndexChanged += new System.EventHandler(this.cbVideoInputFormat_SelectedIndexChanged);
            // 
            // cbVideoInputFrameRate
            // 
            this.cbVideoInputFrameRate.Location = new System.Drawing.Point(122, 66);
            this.cbVideoInputFrameRate.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cbVideoInputFrameRate.Name = "cbVideoInputFrameRate";
            this.cbVideoInputFrameRate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbVideoInputFrameRate.Size = new System.Drawing.Size(254, 22);
            this.cbVideoInputFrameRate.TabIndex = 115;
            // 
            // cbOutputFormat
            // 
            this.cbOutputFormat.Location = new System.Drawing.Point(122, 99);
            this.cbOutputFormat.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cbOutputFormat.Name = "cbOutputFormat";
            this.cbOutputFormat.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbOutputFormat.Properties.Items.AddRange(new object[] {
            "AVI",
            "MKV (Legacy)",
            "WMV (Windows Media Video)",
            "DV",
            "PCM/ACM",
            "MP3",
            "M4A (AAC)",
            "WMA (Windows Media Audio)",
            "FLAC",
            "Ogg Vorbis",
            "Speex",
            "Custom",
            "DirectCapture DV (DV devices only)",
            "DirectCapture AVI (some specific devices)",
            "DirectCapture MPEG (MPEG 1/2/4 devices only)",
            "DirectCapture MKV (IP cameras / H264 devices)",
            "DirectCapture MP4 GDCL Mux (IP cameras / H264 devices)",
            "DirectCapture MP4 Monogram Mux (IP cameras / H264 devices)",
            "DirectCapture Custom (IP Cameras / H264 devices)",
            "WebM",
            "FFMPEG",
            "FFMPEG (external exe)",
            "MP4 (CPU)",
            "MP4 (GPU: Intel, Nvidia, AMD/ATI)",
            "Animated GIF",
            "Encrypted video",
            "MPEG-TS",
            "MOV"});
            this.cbOutputFormat.Size = new System.Drawing.Size(254, 22);
            this.cbOutputFormat.TabIndex = 128;
            this.cbOutputFormat.SelectedIndexChanged += new System.EventHandler(this.cbOutputFormat_SelectedIndexChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(7, 69);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(63, 15);
            this.labelControl2.TabIndex = 116;
            this.labelControl2.Text = "Khung hình";
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(7, 102);
            this.label23.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(98, 15);
            this.label23.TabIndex = 127;
            this.label23.Text = "Định dạng đầu ra:";
            // 
            // panelControl5
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panelControl5, 2);
            this.panelControl5.Controls.Add(this.cbFlipY);
            this.panelControl5.Controls.Add(this.cbFlipX);
            this.panelControl5.Controls.Add(this.cbGreyscale);
            this.panelControl5.Controls.Add(this.cbInvert);
            this.panelControl5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl5.Location = new System.Drawing.Point(3, 136);
            this.panelControl5.Name = "panelControl5";
            this.panelControl5.Size = new System.Drawing.Size(772, 44);
            this.panelControl5.TabIndex = 2;
            // 
            // cbFlipY
            // 
            this.cbFlipY.Location = new System.Drawing.Point(325, 6);
            this.cbFlipY.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cbFlipY.Name = "cbFlipY";
            this.cbFlipY.Properties.Caption = "Xoay dọc";
            this.cbFlipY.Size = new System.Drawing.Size(81, 20);
            this.cbFlipY.TabIndex = 134;
            // 
            // cbFlipX
            // 
            this.cbFlipX.Location = new System.Drawing.Point(222, 6);
            this.cbFlipX.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cbFlipX.Name = "cbFlipX";
            this.cbFlipX.Properties.Caption = "Xoay ngang";
            this.cbFlipX.Size = new System.Drawing.Size(97, 20);
            this.cbFlipX.TabIndex = 133;
            // 
            // cbGreyscale
            // 
            this.cbGreyscale.Location = new System.Drawing.Point(3, 6);
            this.cbGreyscale.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cbGreyscale.Name = "cbGreyscale";
            this.cbGreyscale.Properties.Caption = "Thang độ xám";
            this.cbGreyscale.Size = new System.Drawing.Size(109, 20);
            this.cbGreyscale.TabIndex = 131;
            // 
            // cbInvert
            // 
            this.cbInvert.Location = new System.Drawing.Point(122, 6);
            this.cbInvert.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cbInvert.Name = "cbInvert";
            this.cbInvert.Properties.Caption = "Đảo màu";
            this.cbInvert.Size = new System.Drawing.Size(90, 20);
            this.cbInvert.TabIndex = 132;
            // 
            // _tbShortcutSettings
            // 
            this._tbShortcutSettings.Controls.Add(this.tableLayoutPanel2);
            this._tbShortcutSettings.Name = "_tbShortcutSettings";
            this._tbShortcutSettings.Size = new System.Drawing.Size(666, 395);
            this._tbShortcutSettings.Text = "Phím tắt";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.panelControl8, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.panelControl7, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(666, 395);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // panelControl8
            // 
            this.panelControl8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl8.Location = new System.Drawing.Point(336, 3);
            this.panelControl8.Name = "panelControl8";
            this.panelControl8.Size = new System.Drawing.Size(327, 389);
            this.panelControl8.TabIndex = 1;
            // 
            // panelControl7
            // 
            this.panelControl7.Controls.Add(this._txStoreKey);
            this.panelControl7.Controls.Add(this._txSaveKey);
            this.panelControl7.Controls.Add(this.labelControl10);
            this.panelControl7.Controls.Add(this.labelControl6);
            this.panelControl7.Controls.Add(this._txExitKey);
            this.panelControl7.Controls.Add(this._txSnapshotKey);
            this.panelControl7.Controls.Add(this.labelControl9);
            this.panelControl7.Controls.Add(this.labelControl5);
            this.panelControl7.Controls.Add(this._txPrintKey);
            this.panelControl7.Controls.Add(this._txPauseRecordingKey);
            this.panelControl7.Controls.Add(this.labelControl8);
            this.panelControl7.Controls.Add(this.labelControl4);
            this.panelControl7.Controls.Add(this._txReloadKey);
            this.panelControl7.Controls.Add(this.labelControl7);
            this.panelControl7.Controls.Add(this._txStartRecordingKey);
            this.panelControl7.Controls.Add(this.labelControl3);
            this.panelControl7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl7.Location = new System.Drawing.Point(3, 3);
            this.panelControl7.Name = "panelControl7";
            this.panelControl7.Size = new System.Drawing.Size(327, 389);
            this.panelControl7.TabIndex = 0;
            // 
            // _txStoreKey
            // 
            this._txStoreKey.Location = new System.Drawing.Point(263, 207);
            this._txStoreKey.Name = "_txStoreKey";
            this._txStoreKey.Size = new System.Drawing.Size(100, 22);
            this._txStoreKey.TabIndex = 15;
            // 
            // _txSaveKey
            // 
            this._txSaveKey.Location = new System.Drawing.Point(263, 95);
            this._txSaveKey.Name = "_txSaveKey";
            this._txSaveKey.Size = new System.Drawing.Size(100, 22);
            this._txSaveKey.TabIndex = 7;
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(11, 210);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(102, 15);
            this.labelControl10.TabIndex = 14;
            this.labelControl10.Text = "Đẩy ảnh lên PACS:";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(11, 98);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(24, 15);
            this.labelControl6.TabIndex = 6;
            this.labelControl6.Text = "Lưu:";
            // 
            // _txExitKey
            // 
            this._txExitKey.Location = new System.Drawing.Point(263, 179);
            this._txExitKey.Name = "_txExitKey";
            this._txExitKey.Size = new System.Drawing.Size(100, 22);
            this._txExitKey.TabIndex = 13;
            // 
            // _txSnapshotKey
            // 
            this._txSnapshotKey.Location = new System.Drawing.Point(263, 67);
            this._txSnapshotKey.Name = "_txSnapshotKey";
            this._txSnapshotKey.Size = new System.Drawing.Size(100, 22);
            this._txSnapshotKey.TabIndex = 5;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(11, 182);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(34, 15);
            this.labelControl9.TabIndex = 12;
            this.labelControl9.Text = "Thoát:";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(11, 70);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(70, 15);
            this.labelControl5.TabIndex = 4;
            this.labelControl5.Text = "Chụp nhanh:";
            // 
            // _txPrintKey
            // 
            this._txPrintKey.Location = new System.Drawing.Point(263, 151);
            this._txPrintKey.Name = "_txPrintKey";
            this._txPrintKey.Size = new System.Drawing.Size(100, 22);
            this._txPrintKey.TabIndex = 11;
            // 
            // _txPauseRecordingKey
            // 
            this._txPauseRecordingKey.Location = new System.Drawing.Point(263, 39);
            this._txPauseRecordingKey.Name = "_txPauseRecordingKey";
            this._txPauseRecordingKey.Size = new System.Drawing.Size(100, 22);
            this._txPauseRecordingKey.TabIndex = 3;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(11, 154);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(13, 15);
            this.labelControl8.TabIndex = 10;
            this.labelControl8.Text = "In:";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(11, 42);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(59, 15);
            this.labelControl4.TabIndex = 2;
            this.labelControl4.Text = "Tạm dừng:";
            // 
            // _txReloadKey
            // 
            this._txReloadKey.Location = new System.Drawing.Point(263, 123);
            this._txReloadKey.Name = "_txReloadKey";
            this._txReloadKey.Size = new System.Drawing.Size(100, 22);
            this._txReloadKey.TabIndex = 9;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(11, 126);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(64, 15);
            this.labelControl7.TabIndex = 8;
            this.labelControl7.Text = "Tải lại trang";
            // 
            // _txStartRecordingKey
            // 
            this._txStartRecordingKey.Location = new System.Drawing.Point(263, 11);
            this._txStartRecordingKey.Name = "_txStartRecordingKey";
            this._txStartRecordingKey.Size = new System.Drawing.Size(100, 22);
            this._txStartRecordingKey.TabIndex = 1;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(11, 14);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(74, 15);
            this.labelControl3.TabIndex = 0;
            this.labelControl3.Text = "Bắt đầu quay:";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.buttonCancel);
            this.panelControl1.Controls.Add(this.buttonOK);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 529);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(786, 49);
            this.panelControl1.TabIndex = 19;
            // 
            // _btnStopCamera
            // 
            this._btnStopCamera.Appearance.ForeColor = System.Drawing.Color.Black;
            this._btnStopCamera.Appearance.Options.UseForeColor = true;
            this._btnStopCamera.Location = new System.Drawing.Point(536, 10);
            this._btnStopCamera.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this._btnStopCamera.Name = "_btnStopCamera";
            this._btnStopCamera.Size = new System.Drawing.Size(111, 30);
            this._btnStopCamera.TabIndex = 132;
            this._btnStopCamera.Text = "Tạm dừng";
            this._btnStopCamera.Click += new System.EventHandler(this._btnStopCamera_Click);
            // 
            // OptionsDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(786, 578);
            this.Controls.Add(this._tbOptions);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IconOptions.Image = global::PrintToPACSDemo.Properties.Resources.stm;
            this.Name = "OptionsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cài đặt";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OptionsDialog_FormClosed);
            this.Load += new System.EventHandler(this.OptionsDialog_Load);
            this._groupBoxClient.ResumeLayout(false);
            this._groupBoxClient.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._textBoxClientAE.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._textBoxKeyPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._textBoxPrivateKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._textBoxClientCertificate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServers)).EndInit();
            this.panelButtonServer.ResumeLayout(false);
            this._groupBoxSecurity.ResumeLayout(false);
            this._groupBoxSecurity.PerformLayout();
            this._tbServers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._tbOptions)).EndInit();
            this._tbOptions.ResumeLayout(false);
            this._tpApplicationOptions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txtPrinterName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._ckAutoDelete.Properties)).EndInit();
            this._gpDicomType.ResumeLayout(false);
            this._gpDicomType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txtTempDir.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._cmbSCColor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._cmbSCGray.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._cmbSC.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtSCPDF.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtSCGray.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtSCColor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtSC.Properties)).EndInit();
            this._tpDicomOptions.ResumeLayout(false);
            this._tpCameraOptions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl9)).EndInit();
            this.panelControl9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl2)).EndInit();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cbZoom.Properties)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edPanSourceHeight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edPanSourceTop.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edPanSourceWidth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edPanSourceLeft.Properties)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edPanDestHeight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edPanDestTop.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edPanDestWidth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edPanDestLeft.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbPan.Properties)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edPanStopTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edPanStartTime.Properties)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbLiveRotationAngle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLiveRotationAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbLiveRotation.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._panelCamera)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).EndInit();
            this.panelControl6.ResumeLayout(false);
            this.panelControl6.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.panelControl4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbAudioInputDevice.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbAudioInputFormat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbAudioInputLine.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbVideoInputDevice.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbVideoInputFormat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbVideoInputFrameRate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbOutputFormat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            this.panelControl5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cbFlipY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbFlipX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbGreyscale.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbInvert.Properties)).EndInit();
            this._tbShortcutSettings.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl7)).EndInit();
            this.panelControl7.ResumeLayout(false);
            this.panelControl7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txStoreKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txSaveKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txExitKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txSnapshotKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txPrintKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txPauseRecordingKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txReloadKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txStartRecordingKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Properties


        public VideoCaptureCore VideoCapture1 { get; private set; }
        public double Zoom = 1.0;
        public int ZoomShiftX;
        public int ZoomShiftY;
        private AppSettings _appSettings;
        public MyServerList ServerList
        {
            get
            {
                MyServerList serverlist = new MyServerList();
                MyServer[] items = new MyServer[dataGridViewServers.Rows.Count];
                for (int i = 0; i < dataGridViewServers.Rows.Count; i++)
                {
                    items[i] = new MyServer();
                    items[i]._sAE = dataGridViewServers.Rows[i].Cells["ColumnAE"].Value.ToString();
                    items[i]._sIP = dataGridViewServers.Rows[i].Cells["ColumnIP"].Value.ToString();
                    items[i]._timeout = Convert.ToInt32(dataGridViewServers.Rows[i].Cells["ColumnTimeout"].Value);
                    items[i]._port = Convert.ToInt32(dataGridViewServers.Rows[i].Cells["ColumnPort"].Value);
                    items[i]._useTls = Convert.ToBoolean(dataGridViewServers.Rows[i].Cells["ColumnTls"].Value);
                }
                serverlist.serverList = items;
                return serverlist;
            }

            set
            {
                dataGridViewServers.Rows.Clear();
                foreach (MyServer s in value.serverList)
                {
                    int n = dataGridViewServers.Rows.Add();
                    dataGridViewServers.Rows[n].Cells["ColumnAE"].Value = s._sAE;
                    dataGridViewServers.Rows[n].Cells["ColumnIP"].Value = s._sIP;
                    dataGridViewServers.Rows[n].Cells["ColumnTimeout"].Value = s._timeout.ToString();
                    dataGridViewServers.Rows[n].Cells["ColumnPort"].Value = s._port.ToString();
                    dataGridViewServers.Rows[n].Cells["ColumnTls"].Value = s._useTls.ToString();
                }
            }
        }

        public DicomClassType Selectedtype
        {
            get { return _selectedtype; }
            set { _selectedtype = value; }
        }

        public bool LogLowLevel
        {
            get { return _logLowLevel; }
            set { _logLowLevel = value; }
        }

        public string PrivateKeyPassword
        {
            get { return _privateKeyPassword; }
            set { _privateKeyPassword = value; }
        }

        public DicomImageCompressionType SCCompression
        {
            get { return _SCCompression; }
            set { _SCCompression = value; }
        }

        public DicomImageCompressionType SCColorCompression
        {
            get { return _SCColorCompression; }
            set { _SCColorCompression = value; }
        }

        public DicomImageCompressionType SCGrayCompression
        {
            get { return _SCGrayCompression; }
            set { _SCGrayCompression = value; }
        }

        public string SCPath
        {
            get { return _SCPath; }
            set { _SCPath = value; }
        }

        public string SCColorPath
        {
            get { return _SCColorPath; }
            set { _SCColorPath = value; }
        }

        public string SCGrayPath
        {
            get { return _SCGrayPath; }
            set { _SCGrayPath = value; }
        }

        public string PdfPath
        {
            get { return _PdfPath; }
            set { _PdfPath = value; }
        }

        public string PrinterName
        {
            get { return _PrinterName; }
            set { _PrinterName = value; }
        }

        public string TempDirectory
        {
            get { return _TempDirectory; }
            set { _TempDirectory = value; }
        }

        public bool AutoDelete
        {
            get { return _AutoDelete; }
            set { _AutoDelete = value; }
        }

        public int SelectedTab
        {
            get { return _iSelectedTab; }
            set { _iSelectedTab = value; }
        }

        public int DefaultSCPServer
        {
            get { return _defaultSCPServer; }
            set { _defaultSCPServer = value; }
        }

        public int DefaultMWLServer
        {
            get { return _defaultMWLServer; }
            set { _defaultMWLServer = value; }
        }

        public int DefaultStoreServer
        {
            get { return _defaultStoreServer; }
            set { _defaultStoreServer = value; }
        }

        public string PrivateKey
        {
            get { return _privateKey; }
            set { _privateKey = value; }
        }

        public string ClientCertificate
        {
            get { return _clientCertificate; }
            set { _clientCertificate = value; }
        }

        public string ClientAE
        {
            get { return _clientAE; }
            set { _clientAE = value; }
        }

        #endregion

        #region Form Events

        private void ServerIp_KeyPress
        (
           object sender,
           System.Windows.Forms.KeyPressEventArgs e
        )
        {
            bool bValid = Char.IsNumber(e.KeyChar) || (e.KeyChar == '.') || Char.IsControl(e.KeyChar);
            e.Handled = !bValid;
        }


        private void Port_KeyPress
        (
           object sender,
           System.Windows.Forms.KeyPressEventArgs e
        )
        {
            if (!(Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            _tbServers_Selecting(null, null);
            if (IsAnyServerUseTls())
            {
                if (!CheckFileExists("Certificate", _textBoxClientCertificate, true))
                    return;
                if (!CheckFileExists("Private Key", _textBoxPrivateKey, true))
                    return;
            }


            ClientAE = _textBoxClientAE.Text;
            ClientCertificate = _textBoxClientCertificate.Text;
            PrivateKey = _textBoxPrivateKey.Text;
            PrivateKeyPassword = _textBoxKeyPassword.Text;

            SCPath = _txtSC.Text;
            SCColorPath = _txtSCColor.Text;
            SCGrayPath = _txtSCGray.Text;
            PdfPath = _txtSCPDF.Text;

            if (_rdGrayScale.Checked)
                Selectedtype = DicomClassType.SCMultiFrameGrayscaleByteImageStorage;
            if (_rdSecondaryCapture.Checked)
                Selectedtype = DicomClassType.SCImageStorage;
            if (_rdColored.Checked)
                Selectedtype = DicomClassType.SCMultiFrameTrueColorImageStorage;
            if (_rdPDF.Checked)
                Selectedtype = DicomClassType.EncapsulatedPdfStorage;

            SCCompression = ImgCompression[_cmbSC.SelectedIndex];
            SCColorCompression = ImgCompression[_cmbSCColor.SelectedIndex];
            SCGrayCompression = ImgCompression[_cmbSCGray.SelectedIndex];

            PrinterName = _txtPrinterName.Text;
            TempDirectory = _txtTempDir.Text;
            AutoDelete = _ckAutoDelete.Checked;

            DefaultSCPServer = _defaultSCPServer;
            DefaultMWLServer = _defaultMWLServer;
            DefaultStoreServer = _defaultStoreServer;
            SaveSettingsCamera();
            SaveSettingShortcut();
        }


        private void VideoCapture1_OnError(object sender, ErrorsEventArgs e)
        {
        }


        private async Task CreateEngineAsync()
        {
            VideoCapture1 = await VideoCaptureCore.CreateAsync(videoView1 as IVideoView);

            VideoCapture1.OnError += VideoCapture1_OnError;
        }

        private void cbVideoInputDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbVideoInputDevice.SelectedIndex != -1)
            {
                cbVideoInputFormat.Properties.Items.Clear();
                var deviceItem = VideoCapture1.Video_CaptureDevices().FirstOrDefault(device => device.Name == cbVideoInputDevice.Text);
                if (deviceItem == null)
                {
                    return;
                }

                foreach (var format in deviceItem.VideoFormats)
                {
                    cbVideoInputFormat.Properties.Items.Add(format.Name);
                }

                if (cbVideoInputFormat.Properties.Items.Count > 0)
                {
                    cbVideoInputFormat.SelectedIndex = 0;
                    cbVideoInputFormat_SelectedIndexChanged(null, null);
                }
            }
        }


        private void cbVideoInputFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbVideoInputFormat.Text))
            {
                return;
            }

            if (cbVideoInputDevice.SelectedIndex != -1)
            {
                var deviceItem = VideoCapture1.Video_CaptureDevices().FirstOrDefault(device => device.Name == cbVideoInputDevice.Text);
                if (deviceItem == null)
                {
                    return;
                }

                var videoFormat = deviceItem.VideoFormats.Find(format => format.Name == cbVideoInputFormat.Text);
                if (videoFormat == null)
                {
                    return;
                }

                cbVideoInputFrameRate.Properties.Items.Clear();
                foreach (var frameRate in videoFormat.FrameRates)
                {
                    cbVideoInputFrameRate.Properties.Items.Add(frameRate.ToString(CultureInfo.CurrentCulture));
                }

                if (cbVideoInputFrameRate.Properties.Items.Count > 0)
                {
                    cbVideoInputFrameRate.SelectedIndex = 0;
                }
            }
        }

        private void cbAudioInputDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbAudioInputDevice.SelectedIndex != -1)
            {
                cbAudioInputFormat.Properties.Items.Clear();

                var deviceItem = VideoCapture1.Audio_CaptureDevices().FirstOrDefault(device => device.Name == cbAudioInputDevice.Text);
                if (deviceItem == null)
                {
                    return;
                }

                var defaultValue = "PCM, 44100 Hz, 16 Bits, 2 Channels";
                var defaultValueExists = false;
                foreach (string format in deviceItem.Formats)
                {
                    cbAudioInputFormat.Properties.Items.Add(format);

                    if (defaultValue == format)
                    {
                        defaultValueExists = true;
                    }
                }

                if (cbAudioInputFormat.Properties.Items.Count > 0)
                {
                    cbAudioInputFormat.SelectedIndex = 0;

                    if (defaultValueExists)
                    {
                        cbAudioInputFormat.Text = defaultValue;
                    }
                }

                cbAudioInputLine.Properties.Items.Clear();

                foreach (string line in deviceItem.Lines)
                {
                    cbAudioInputLine.Properties.Items.Add(line);
                }

                if (cbAudioInputLine.Properties.Items.Count > 0)
                {
                    cbAudioInputLine.SelectedIndex = 0;
                }
            }
        }

        private void cbOutputFormat_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void InitVideoCamera()
        {
            await CreateEngineAsync();

            cbOutputFormat.SelectedIndex = 2;

            foreach (var device in VideoCapture1.Video_CaptureDevices())
            {
                cbVideoInputDevice.Properties.Items.Add(device.Name);
            }

            if (cbVideoInputDevice.Properties.Items.Count > 0)
            {
                if (cbVideoInputDevice.Properties.Items.Count > 0)
                {
                    string selectedDevice = _appSettings.CameraSettings.VideoInputDevice?.ToString();

                    if (!string.IsNullOrEmpty(selectedDevice))
                    {
                        int index = -1;
                        for (int i = 0; i < cbVideoInputDevice.Properties.Items.Count; i++)
                        {
                            if (cbVideoInputDevice.Properties.Items[i].ToString() == selectedDevice)
                            {
                                index = i;
                                break;
                            }
                        }
                        cbVideoInputDevice.SelectedIndex = (index >= 0) ? index : 0;
                    }
                    else
                    {
                        cbVideoInputDevice.SelectedIndex = 0;
                    }
                }
            }

            cbVideoInputDevice_SelectedIndexChanged(null, null);

            foreach (var device in VideoCapture1.Audio_CaptureDevices())
            {
                cbAudioInputDevice.Properties.Items.Add(device.Name);
            }

            if (cbAudioInputDevice.Properties.Items.Count > 0)
            {
                string selectedAudioDevice = _appSettings.CameraSettings.AudioInputDevice?.ToString();

                if (!string.IsNullOrEmpty(selectedAudioDevice))
                {
                    int index = -1;
                    for (int i = 0; i < cbAudioInputDevice.Properties.Items.Count; i++)
                    {
                        if (cbAudioInputDevice.Properties.Items[i].ToString() == selectedAudioDevice)
                        {
                            index = i;
                            break;
                        }
                    }
                    cbAudioInputDevice.SelectedIndex = (index >= 0) ? index : 0;
                }
                else
                {
                    cbAudioInputDevice.SelectedIndex = 0;
                }
            }

            cbAudioInputLine.Properties.Items.Clear();

            if (!string.IsNullOrEmpty(cbAudioInputDevice.Text))
            {
                var deviceItem =
                    VideoCapture1.Audio_CaptureDevices().FirstOrDefault(device => device.Name == cbAudioInputDevice.Text);
                if (deviceItem != null)
                {
                    foreach (string line in deviceItem.Lines)
                    {
                        cbAudioInputLine.Properties.Items.Add(line);
                    }

                    if (cbAudioInputLine.Properties.Items.Count > 0)
                    {
                        cbAudioInputLine.SelectedIndex = 0;
                    }
                    if (cbAudioInputLine.Properties.Items.Count > 0)
                    {
                        string selectedAudioInputLine = _appSettings.CameraSettings.AudioInputLine?.ToString();

                        if (!string.IsNullOrEmpty(selectedAudioInputLine))
                        {
                            int index = -1;
                            for (int i = 0; i < cbAudioInputLine.Properties.Items.Count; i++)
                            {
                                if (cbAudioInputLine.Properties.Items[i].ToString() == selectedAudioInputLine)
                                {
                                    index = i;
                                    break;
                                }
                            }
                            cbAudioInputLine.SelectedIndex = (index >= 0) ? index : 0;
                        }
                        else
                        {
                            cbAudioInputLine.SelectedIndex = 0;
                        }
                    }
                }
            }


            if (!string.IsNullOrEmpty(_appSettings.CameraSettings.InphutFormat?.ToString()))
            {
                cbVideoInputFormat.Text = _appSettings.CameraSettings.InphutFormat?.ToString();
            }

            if (!string.IsNullOrEmpty(_appSettings.CameraSettings.FrameRate?.ToString()))
            {
                cbVideoInputFrameRate.Text = _appSettings.CameraSettings.FrameRate?.ToString();
            }

            if (!string.IsNullOrEmpty(_appSettings.CameraSettings.OutputFormat?.ToString()))
            {
                cbOutputFormat.Text = _appSettings.CameraSettings.OutputFormat?.ToString();
            }

            cbGreyscale.Checked = _appSettings.CameraSettings.Greyscale;
            cbInvert.Checked = _appSettings.CameraSettings.Invert;
            cbFlipX.Checked = _appSettings.CameraSettings.FlipX;
            cbFlipY.Checked = _appSettings.CameraSettings.FlipY;

            cbPan.Checked = _appSettings.CameraSettings.EnablePan;
            edPanStartTime.Text= _appSettings.CameraSettings.PanStartTime.ToString();
            edPanStopTime.Text = _appSettings.CameraSettings.PanStopTime.ToString();
            edPanSourceLeft.Text = _appSettings.CameraSettings.PanSourceLeft.ToString();
            edPanSourceWidth.Text = _appSettings.CameraSettings.PanSourceWidth.ToString();
            edPanSourceHeight.Text = _appSettings.CameraSettings.PanSourceHeight.ToString();
            edPanSourceTop.Text = _appSettings.CameraSettings.PanSourceTop.ToString();
            edPanDestLeft.Text = _appSettings.CameraSettings.PanDestLeft.ToString();
            edPanDestWidth.Text = _appSettings.CameraSettings.PanDestWidth.ToString();
            edPanDestHeight.Text = _appSettings.CameraSettings.PanDestHeight.ToString();
            edPanDestTop.Text = _appSettings.CameraSettings.PanDestTop.ToString();

            cbLiveRotation.Checked = _appSettings.CameraSettings.EnableLiveRotation;
            tbLiveRotationAngle.Value = _appSettings.CameraSettings.LiveRotationAngle;

            VideoCapture1.Video_Renderer_SetAuto();
        }

        private void InitVideoView()
        {


            this.videoView1 = new VisioForge.Core.UI.WinForms.VideoView();
            // 
            // videoView1
            // 
            this.videoView1.BackColor = System.Drawing.Color.Black;
            this.videoView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoView1.Location = new System.Drawing.Point(0, 40);
            this.videoView1.Margin = new System.Windows.Forms.Padding(4);
            this.videoView1.Name = "videoView1";
            this.videoView1.Size = new System.Drawing.Size(912, 797);
            this.videoView1.StatusOverlay = null;
            this.videoView1.TabIndex = 9;
            _panelCamera.Controls.Add(videoView1);
        }

        private void SettingCaptureDevice()
        {
            VideoCapture1.Video_Effects_Enabled = false;
            VideoCapture1.Video_CaptureDevice = new VideoCaptureSource(cbVideoInputDevice.Text)
            {
                Format_UseBest = false,
                Format = cbVideoInputFormat.Text,
                FrameRate = new VideoFrameRate(Convert.ToDouble(cbVideoInputFrameRate.Text, CultureInfo.CurrentCulture))
            };

            VideoCapture1.Audio_RecordAudio = false;
            VideoCapture1.Audio_PlayAudio = false;

            VideoCapture1.Video_Sample_Grabber_Enabled = true;
            VideoCapture1.Video_Renderer.Zoom_Ratio = 0;
            VideoCapture1.Video_Renderer.Zoom_ShiftX = 0;
            VideoCapture1.Video_Renderer.Zoom_ShiftY = 0;
        }

        private async void _btnPreview_Click(object sender, EventArgs e)
        {
            VideoCapture1.Video_Filters_Clear();
            await VideoCapture1.StopAsync();
            SettingCaptureDevice();
            VideoCapture1.Mode = VideoCaptureMode.VideoPreview;
            await VideoCapture1.StartAsync();
        }

        private void SaveSettingsCamera()
        {
            var VideoInputDevice = cbVideoInputDevice.Text;
            var VideoInputFormat = cbVideoInputFormat.Text;
            var VideoInputFrameRate = cbVideoInputFrameRate.Text;
            var OutputFormat = cbOutputFormat.Text;
            var AudioInputDevice = cbAudioInputDevice.Text;
            var AudioInputFormat = cbAudioInputFormat.Text;
            var AudioInputLine = cbAudioInputLine.Text;

            var Greyscale = cbGreyscale.Checked;
            var Invert = cbInvert.Checked;
            var FlipX = cbFlipX.Checked;
            var FlipY = cbFlipY.Checked;

            //var EnableZoom = cbZoom.Checked;
            //var EffZoomLeft = EffZoomLeft;
            //var EffZoomUp =;
            //var EffZoomDown =;
            //var EffZoomRight =;
            //var EffZoomIn =;
            //var EffZoomOut =;

            var EnablePan = cbPan.Checked;
            int PanStartTime = int.Parse(edPanStartTime.Text);
            int PanStopTime = int.Parse(edPanStopTime.Text);
            int PanSourceLeft = int.Parse(edPanSourceLeft.Text);
            int PanSourceWidth = int.Parse(edPanSourceWidth.Text);
            int PanSourceHeight = int.Parse(edPanSourceHeight.Text);
            int PanSourceTop = int.Parse(edPanSourceTop.Text);
            int PanDestLeft = int.Parse(edPanDestLeft.Text);
            int PanDestWidth = int.Parse(edPanDestWidth.Text);
            int PanDestHeight = int.Parse(edPanDestHeight.Text);
            int PanDestTop = int.Parse(edPanDestTop.Text);

            var EnableLiveRotation = cbLiveRotation.Checked;
            int LiveRotationAngle = tbLiveRotationAngle.Value;

            CameraSettings cameraSettings = new CameraSettings
            {
                VideoInputDevice = VideoInputDevice,
                InphutFormat = VideoInputFormat,
                FrameRate = VideoInputFrameRate,
                OutputFormat = OutputFormat,
                AudioInputDevice = AudioInputDevice,
                AudioInputFormat = AudioInputFormat,
                AudioInputLine = AudioInputLine,

                Greyscale = Greyscale,
                Invert = Invert,
                FlipX = FlipX,
                FlipY = FlipY,

                //EnableZoom = Zoom.Checked;
                //EffZoomLeft = EffZoomLeft;
                //EffZoomUp =;
                //EffZoomDown =;
                //EffZoomRight =;
                //EffZoomIn =;
                //EffZoomOut =;

                EnablePan = EnablePan,
                PanStartTime = PanStartTime,
                PanStopTime = PanStopTime,
                PanSourceLeft = PanSourceLeft,
                PanSourceWidth = PanSourceWidth,
                PanSourceHeight = PanSourceHeight,
                PanSourceTop = PanSourceTop,
                PanDestLeft = PanDestLeft,
                PanDestWidth = PanDestWidth,
                PanDestHeight = PanDestHeight,
                PanDestTop = PanDestTop,

                EnableLiveRotation = EnableLiveRotation,
                LiveRotationAngle = LiveRotationAngle,
            };
            AppSettingsLoader.SaveCameraSettings(cameraSettings);
        }

        private void SaveSettingShortcut()
        {
            ShortcutSettings shortcut = new ShortcutSettings();

            shortcut.StartRecordingKey = Enum.TryParse(_txStartRecordingKey.Text, out Keys start) ? start : Keys.None;
            shortcut.PauseRecordingKey = Enum.TryParse(_txPauseRecordingKey.Text, out Keys pause) ? pause : Keys.None;
            shortcut.SnapshotKey = Enum.TryParse(_txSnapshotKey.Text, out Keys snapshot) ? snapshot : Keys.None;
            shortcut.SaveDicomKey = Enum.TryParse(_txSaveKey.Text, out Keys save) ? save : Keys.None;
            shortcut.ReloadKey = Enum.TryParse(_txReloadKey.Text, out Keys reload) ? reload : Keys.None;
            shortcut.PrintKey = Enum.TryParse(_txPrintKey.Text, out Keys print) ? print : Keys.None;
            shortcut.ExitKey = Enum.TryParse(_txExitKey.Text, out Keys exit) ? exit : Keys.None;
            shortcut.StoreKey = Enum.TryParse(_txStoreKey.Text, out Keys store) ? store : Keys.None;

            AppSettingsLoader.SaveShortcutSettings(shortcut);
        }


        private void ShortcutKeyEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is DevExpress.XtraEditors.TextEdit textEdit)
            {
                Keys key = e.KeyCode;

                if ((key >= Keys.F1 && key <= Keys.F12) || key == Keys.Escape)
                {
                    textEdit.Text = key.ToString();
                    textEdit.Tag = key;
                }

                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }


        private void InitShortcutKeySetting()
        {
            _txStartRecordingKey.ReadOnly = true;
            _txPauseRecordingKey.ReadOnly = true;
            _txSnapshotKey.ReadOnly = true;
            _txSaveKey.ReadOnly = true;
            _txReloadKey.ReadOnly = true;
            _txPrintKey.ReadOnly = true;
            _txExitKey.ReadOnly = true;
            _txStoreKey.ReadOnly = true;

            _txStartRecordingKey.KeyDown += ShortcutKeyEdit_KeyDown;
            _txPauseRecordingKey.KeyDown += ShortcutKeyEdit_KeyDown;
            _txSnapshotKey.KeyDown += ShortcutKeyEdit_KeyDown;
            _txSaveKey.KeyDown += ShortcutKeyEdit_KeyDown;
            _txReloadKey.KeyDown += ShortcutKeyEdit_KeyDown;
            _txPrintKey.KeyDown += ShortcutKeyEdit_KeyDown;
            _txExitKey.KeyDown += ShortcutKeyEdit_KeyDown;
            _txStoreKey.KeyDown += ShortcutKeyEdit_KeyDown;

            _txStartRecordingKey.Text = _appSettings.ShortcutSettings.StartRecordingKey.ToString();
            _txPauseRecordingKey.Text = _appSettings.ShortcutSettings.PauseRecordingKey.ToString();
            _txSnapshotKey.Text = _appSettings.ShortcutSettings.SnapshotKey.ToString();
            _txSaveKey.Text = _appSettings.ShortcutSettings.SaveDicomKey.ToString();
            _txReloadKey.Text = _appSettings.ShortcutSettings.ReloadKey.ToString();
            _txPrintKey.Text = _appSettings.ShortcutSettings.PrintKey.ToString();
            _txExitKey.Text = _appSettings.ShortcutSettings.ExitKey.ToString();
            _txStoreKey.Text = _appSettings.ShortcutSettings.StoreKey.ToString();
        }


        private void OptionsDialog_Load(object sender, EventArgs e)
        {
            _appSettings = AppSettingsLoader.Load();
            InitVideoView();
            InitVideoCamera();
            InitShortcutKeySetting();

            _textBoxClientAE.Text = ClientAE;
            _textBoxClientCertificate.Text = ClientCertificate;
            _textBoxPrivateKey.Text = PrivateKey;
            _textBoxKeyPassword.Text = PrivateKeyPassword;
            EnableDialogItems();
            _tbSCPQuerypage.Tag = serverlistSCP;
            _tbMWLQueryPage.Tag = serverlistMWL;
            _tbStorePage.Tag = serverlistStore;
            _tbServers_Selecting(null, null);
            _btnBrowseSC.Tag = _txtSC;
            _btnBrowseSCColor.Tag = _txtSCColor;
            _btnBrowseSCGray.Tag = _txtSCGray;
            _btnBrowseSCPDF.Tag = _txtSCPDF;

            _btnRename.Click += new EventHandler(_btnRename_Click);
            _btnBrowseTempDir.Click += new EventHandler(_btnBrowseTempDir_Click);

            _btnBrowseSC.Click += new EventHandler(_btnBrowseSC_Click);
            _btnBrowseSCColor.Click += new EventHandler(_btnBrowseSC_Click);
            _btnBrowseSCGray.Click += new EventHandler(_btnBrowseSC_Click);
            _btnBrowseSCPDF.Click += new EventHandler(_btnBrowseSC_Click);

            _txtSC.Leave += new EventHandler(_txt_Leave);
            _txtSCColor.Leave += new EventHandler(_txt_Leave);
            _txtSCGray.Leave += new EventHandler(_txt_Leave);
            _txtSCPDF.Leave += new EventHandler(_txt_Leave);

            _txtTempDir.Leave += new EventHandler(_txtTempDir_Leave);

            _cmbSC.Properties.Items.AddRange(Compressions.ToArray());
            _cmbSCColor.Properties.Items.AddRange(Compressions.ToArray());
            _cmbSCGray.Properties.Items.AddRange(Compressions.ToArray());

            _txtSC.Text = SCPath;
            _txtSCGray.Text = SCGrayPath;
            _txtSCColor.Text = SCColorPath;
            _txtSCPDF.Text = PdfPath;

            _txtSC.Text = SCPath;
            _txtSCGray.Text = SCGrayPath;
            _txtSCColor.Text = SCColorPath;
            _txtSCPDF.Text = PdfPath;

            _txtTempDir.Text = TempDirectory;
            _txtPrinterName.Text = PrinterName;

            _cmbSC.SelectedIndex = ImgCompression.IndexOf(SCCompression);
            _cmbSCColor.SelectedIndex = ImgCompression.IndexOf(SCColorCompression);
            _cmbSCGray.SelectedIndex = ImgCompression.IndexOf(SCGrayCompression);

            if (Selectedtype == DicomClassType.SCMultiFrameGrayscaleByteImageStorage)
                _rdGrayScale.Checked = true;
            if (Selectedtype == DicomClassType.SCImageStorage)
                _rdSecondaryCapture.Checked = true;
            if (Selectedtype == DicomClassType.SCMultiFrameTrueColorImageStorage)
                _rdColored.Checked = true;
            if (Selectedtype == DicomClassType.EncapsulatedPdfStorage)
                _rdPDF.Checked = true;

            _ckAutoDelete.Checked = _AutoDelete;

            if (_tbOptions.TabPages.Count > SelectedTab)
                _tbOptions.SelectedTabPageIndex = SelectedTab;
        }

        void _txtTempDir_Leave(object sender, EventArgs e)
        {
            if (!Directory.Exists(_txtTempDir.Text))
            {
                MessageBox.Show("The selected directory does not exist");
                _txtTempDir.Text = "";
            }
        }

        void _btnRename_Click(object sender, EventArgs e)
        {
            InputDialog input = new InputDialog("Change Printer Driver Name", "New Printer Driver Name", _txtPrinterName.Text);
            if (input.ShowDialog() != DialogResult.Cancel)
            {
                _txtPrinterName.Text = input.Value;
            }
        }

        void _btnBrowseTempDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlgFolder = new FolderBrowserDialog();
            dlgFolder.ShowNewFolderButton = true;
            DialogResult dlgRes = dlgFolder.ShowDialog();

            if (dlgRes != DialogResult.OK)
                return;

            _txtTempDir.Text = dlgFolder.SelectedPath;
        }

        void _btnBrowseSC_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Filter = "DICOM Xml files|*.xml";
            DialogResult dlgRes = dlgOpen.ShowDialog();
            if (dlgRes == DialogResult.Cancel)
                return;

            DevExpress.XtraEditors.TextEdit textBox = ((sender as Button).Tag as DevExpress.XtraEditors.TextEdit);
            textBox.Text = dlgOpen.FileName;
            _txt_Leave(textBox, e);

        }

        private void _buttonClientCertificate_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "Select Client Certificate";
            openDialog.FileName = _textBoxClientCertificate.Text;
            openDialog.Filter = "PEM files (*.pem)|*.pem|All files (*.*)|*.*";
            DialogResult result = openDialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                _textBoxClientCertificate.Text = openDialog.FileName;
            }
        }

        private void _buttonPrivateKey_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "Select Private Key File";
            openDialog.FileName = _textBoxClientCertificate.Text;
            openDialog.Filter = "PEM files (*.pem)|*.pem|All files (*.*)|*.*";
            DialogResult result = openDialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                _textBoxPrivateKey.Text = openDialog.FileName;
            }
        }

        private void _checkBoxLoggingLowLevel_CheckedChanged(object sender, EventArgs e)
        {
            EnableDialogItems();
        }

        private void buttonAddServer_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = dataGridViewServers.Rows.Add();
                DataGridViewRow row = dataGridViewServers.Rows[rowIndex];
                row.ReadOnly = false;
                row.Selected = true;
                dataGridViewServers.CurrentCell = row.Cells[0];
                dataGridViewServers.ShowEditingIcon = true;
                dataGridViewServers.BeginEdit(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }
        }

        private void buttonDeleteServer_Click(object sender, EventArgs e)
        {
            try
            {
                int iDefaultRowNumber = -1;
                if (_tbServers.SelectedIndex == 0)
                    iDefaultRowNumber = DefaultSCPServer;
                if (_tbServers.SelectedIndex == 1)
                    iDefaultRowNumber = DefaultMWLServer;
                if (_tbServers.SelectedIndex == 2)
                    iDefaultRowNumber = DefaultStoreServer;

                bool bDefaultChanged = false;
                foreach (DataGridViewRow row in dataGridViewServers.SelectedRows)
                {
                    if (row.Cells[6].Value != null && (bool)row.Cells[6].Value == true)
                        bDefaultChanged = true;
                    dataGridViewServers.Rows.Remove(row);
                }

                if (bDefaultChanged)
                    if (dataGridViewServers.Rows.Count > 0)
                        dataGridViewServers.Rows[0].Cells[6].Value = true;

                if (_tbServers.SelectedIndex == 0)
                    DefaultSCPServer = iDefaultRowNumber;
                if (_tbServers.SelectedIndex == 1)
                    DefaultMWLServer = iDefaultRowNumber;
                if (_tbServers.SelectedIndex == 2)
                    DefaultStoreServer = iDefaultRowNumber;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }

        }

        private void dataGridViewServers_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                DataGridViewRow validatingRow = dataGridViewServers.Rows[e.RowIndex];
                if ((null == validatingRow.Cells[ColumnAE.Name].EditedFormattedValue) ||
                           (string.IsNullOrEmpty(validatingRow.Cells[ColumnAE.Name].EditedFormattedValue.ToString())))
                {
                    validatingRow.ErrorText = ColumnAE.HeaderText + " cannot be empty";
                    e.Cancel = true;
                    return;
                }

                if ((null != validatingRow.Cells[ColumnAE.Name].EditedFormattedValue &&
                     validatingRow.Cells[ColumnAE.Name].EditedFormattedValue.ToString().Length > 16))
                {
                    validatingRow.ErrorText = ColumnAE.HeaderText + " must be less than 16 characters";
                    e.Cancel = true;
                    return;
                }
                if ((null == validatingRow.Cells[ColumnIP.Name].EditedFormattedValue) ||
                     (string.IsNullOrEmpty(validatingRow.Cells[ColumnIP.Name].EditedFormattedValue.ToString())))
                {
                    validatingRow.ErrorText = ColumnIP.HeaderText + " cannot be empty.";
                    e.Cancel = true;
                    return;
                }

                try
                {
                    string ip = validatingRow.Cells[ColumnIP.Name].EditedFormattedValue.ToString();
                    //Utils.ResolveIPAddress(ip);
                }
                catch (Exception exception)
                {
                    validatingRow.ErrorText = exception.Message;
                    e.Cancel = true;
                    return;
                }

                int number;
                if ((null == validatingRow.Cells[ColumnPort.Name].EditedFormattedValue) ||
                     (string.IsNullOrEmpty(validatingRow.Cells[ColumnPort.Name].EditedFormattedValue.ToString())) ||
                     (!int.TryParse(validatingRow.Cells[ColumnPort.Name].EditedFormattedValue.ToString(), out number)))
                {
                    validatingRow.ErrorText = string.Format("Invalid {0}.", ColumnPort.HeaderText);
                    e.Cancel = true;
                    return;
                }

                if ((null == validatingRow.Cells[ColumnTimeout.Name].EditedFormattedValue) ||
                     (string.IsNullOrEmpty(validatingRow.Cells[ColumnTimeout.Name].EditedFormattedValue.ToString())) ||
                     (!int.TryParse(validatingRow.Cells[ColumnTimeout.Name].EditedFormattedValue.ToString(), out number)))
                {
                    validatingRow.Cells[ColumnTimeout.Name].Value = 15;
                    e.Cancel = false;
                    return;
                }

                validatingRow.ErrorText = "";

                int iDefault = -1;
                TabPage tbPage = _tbServers.SelectedTab;
                if (_tbMWLQueryPage == tbPage)
                    iDefault = _defaultMWLServer;

                if (_tbSCPQuerypage == tbPage)
                    iDefault = _defaultSCPServer;

                if (_tbStorePage == tbPage)
                    iDefault = _defaultStoreServer;

                if (dataGridViewServers.Rows.Count > 0)
                {
                    if (e.RowIndex == iDefault && e.ColumnIndex == 6)
                        dataGridViewServers.Rows[e.RowIndex].Cells[6].Value = true;
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                throw;
            }
        }

        private void dataGridViewServers_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridView d = sender as DataGridView;
            if (d != null)
            {
                DataGridViewCheckBoxCell cb = d.CurrentCell as DataGridViewCheckBoxCell;
                if (cb != null && cb.ColumnIndex != 6)
                {
                    d.CommitEdit(DataGridViewDataErrorContexts.CurrentCellChange);
                    TabPage page = _tbServers.SelectedTab;
                    MyServerList servers = (MyServerList)page.Tag;

                    try
                    {
                        ServerList.serverList[d.CurrentCell.RowIndex]._useTls = Convert.ToBoolean(dataGridViewServers.Rows[d.CurrentCell.RowIndex].Cells["ColumnTls"].Value);
                        EnableDialogItems();
                    }
                    catch { }

                }
            }
        }

        private void _tbServers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void _tbServers_Selecting(object sender, TabControlCancelEventArgs e)
        {
            foreach (TabPage page in _tbServers.TabPages)
            {
                if (page.Controls.Count != 0)
                {
                    page.Controls.Clear();
                    MyServerList servers = (MyServerList)page.Tag;

                    if (_tbMWLQueryPage == page)
                        serverlistMWL = ServerList;
                    if (_tbSCPQuerypage == page)
                        serverlistSCP = ServerList;
                    if (_tbStorePage == page)
                        serverlistStore = ServerList;

                }
            }


            TabPage tbPage = _tbServers.SelectedTab;
            //tbPage.Controls.Add(buttonDeleteServer);
            //tbPage.Controls.Add(buttonAddServer);
            tbPage.Controls.Add(dataGridViewServers);

            tbPage.Controls.Add(panelButtonServer);
            int iDefault = -1;
            if (_tbMWLQueryPage == tbPage)
            {
                ServerList = serverlistMWL;
                iDefault = _defaultMWLServer;
            }
            if (_tbSCPQuerypage == tbPage)
            {
                ServerList = serverlistSCP;
                iDefault = _defaultSCPServer;
            }
            if (_tbStorePage == tbPage)
            {
                ServerList = serverlistStore;
                iDefault = _defaultStoreServer;
            }

            if (dataGridViewServers.Rows.Count > 0)
            {
                if (dataGridViewServers.Rows.Count <= iDefault)
                    iDefault = dataGridViewServers.Rows.Count - 1;
                dataGridViewServers.Rows[iDefault].Cells[6].Value = true;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void _txt_Leave(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit txtBox = sender as DevExpress.XtraEditors.TextEdit;
            int iClass = int.Parse((string)txtBox.Tag);
            DicomClassType dclass = ClassTypes[iClass];
            if (txtBox.Text == string.Empty)
                return;
            DicomDataSet ds = new DicomDataSet();
            try
            {
                DicomExtensions.LoadXml(ds, txtBox.Text, DicomDataSetLoadXmlFlags.None);
            }
            catch
            { ds = null; }

            if (ds == null)
            {
                MessageBox.Show("The selected file is not a valid DICOM XML File");
                txtBox.Text = "";
            }
            else
               if (ds.InformationClass != dclass)
            {
                MessageBox.Show("The selected DICOM XML file is not a " + dclass.ToString() + " file");
                txtBox.Text = "";
            }

        }

        private void dataGridViewServers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5 && e.RowIndex >= 0)
                try
                {
                    CheckAssociation((sender as Control).Parent, e.RowIndex);
                }
                catch
                {
                    MessageBox.Show(this, "Some fields are not valid", "Print To PACS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            MyServerList servers = (MyServerList)_tbServers.SelectedTab.Tag;
            if (e.ColumnIndex == 6 && e.RowIndex >= 0)
            {
                foreach (DataGridViewRow row in dataGridViewServers.Rows)
                {
                    row.Cells[6].Value = false;
                }

                dataGridViewServers[6, e.RowIndex].Value = true;
                if ((sender as Control).Parent == _tbMWLQueryPage)
                    _defaultMWLServer = e.RowIndex;
                if ((sender as Control).Parent == _tbSCPQuerypage)
                    _defaultSCPServer = e.RowIndex;
                if ((sender as Control).Parent == _tbStorePage)
                    _defaultStoreServer = e.RowIndex;
            }
            EnableDialogItems();
        }

        private void _find_AfterAssociateRequest(object sender, AfterAssociateRequestEventArgs e)
        {
            bTimeOut = false;
            foreach (AssociationHolder association in lstAssociations)
            {
                try
                {
                    association.Result = e.Associate.GetResult(association.PresentationContextNumber) == DicomAssociateAcceptResultType.Success ? "Success" : "Failed";
                }
                catch { association.Result = "Failed"; }
            }
        }

        private void _find_BeforeAssociateRequest(object sender, BeforeAssociateRequestEventArgs e)
        {
            for (int i = 0; i <= e.Associate.PresentationContextCount; i++)
            {
                e.Associate.DeletePresentation((byte)(2 * i + 1));
            }
            foreach (AssociationHolder association in lstAssociations)
            {
                e.Associate.AddPresentationContext(association.PresentationContextNumber, DicomAssociateAcceptResultType.Success, association.PresentationContext);
                foreach (string str in association.TransferSyntax)
                    e.Associate.AddTransfer(association.PresentationContextNumber, str);
            }
        }

        private void _find_AfterConnect(object sender, AfterConnectEventArgs e)
        {
            //if (e.Error == DicomExceptionCode.Success)
            //   MessageBox.Show("Dicom Verification Success");
            //else
            //   MessageBox.Show("Dicom Verification Failed");
        }

        #endregion

        #region Methods

        private void CheckAssociation(Control parent, int iRow)
        {
            string strServerAE = dataGridViewServers[0, iRow].Value.ToString();
            string strServerIP = dataGridViewServers[1, iRow].Value.ToString();
            string strServerPort = dataGridViewServers[2, iRow].Value.ToString();
            string strServerTimeOut = null;
            try
            {
                strServerTimeOut = dataGridViewServers[3, iRow].Value.ToString();
            }
            catch { dataGridViewServers[3, iRow].Value = strServerTimeOut = "15"; }

            bool tls = false;
            if (dataGridViewServers[4, iRow].Value != null)
                tls = bool.Parse(dataGridViewServers[4, iRow].Value.ToString());

            DicomScp dicomScp = new DicomScp();
            dicomScp.AETitle = strServerAE;
            dicomScp.PeerAddress = IPAddress.Parse(strServerIP);
            dicomScp.Port = int.Parse(strServerPort);
            dicomScp.Timeout = int.Parse(strServerTimeOut);
            QueryRetrieveScu _find = null;

            if (tls)
                _find = new QueryRetrieveScu(_txtTempDir.Text, DicomNetSecurityeMode.Tls, null);
            else
                _find = new QueryRetrieveScu(null, DicomNetSecurityeMode.None, null);

            try
            {

                _find.ImplementationClass = FrmMain._sConfigurationImplementationClass;
                _find.ProtocolVersion = FrmMain._sConfigurationProtocolversion;
                _find.ImplementationVersionName = FrmMain._sConfigurationImplementationVersionName;
                _find.AETitle = _textBoxClientAE.Text;
                _find.HostPort = 1000;

                _find.AfterConnect += new AfterConnectDelegate(_find_AfterConnect);
                _find.BeforeAssociateRequest += new BeforeAssociationRequestDelegate(_find_BeforeAssociateRequest);
                _find.AfterAssociateRequest += new AfterAssociateRequestDelegate(_find_AfterAssociateRequest);
                _find.PrivateKeyPassword += new PrivateKeyPasswordDelegate(_find_PrivateKeyPassword);
                if (tls)
                {
                    try
                    {
                        if (!CheckFileExists("Certificate", _textBoxClientCertificate, true))
                            return;
                        if (!CheckFileExists("Private Key", _textBoxPrivateKey, true))
                            return;

                        _find.SetTlsCipherSuiteByIndex(0, DicomTlsCipherSuiteType.DheRsaWith3DesEdeCbcSha);
                        _find.SetTlsClientCertificate(
                           _textBoxClientCertificate.Text,
                           DicomTlsCertificateType.Pem,
                           _textBoxPrivateKey.Text.Length > 0 ? _textBoxPrivateKey.Text : null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                DoAssociation(dicomScp, _find, parent != _tbStorePage);
            }
            catch { }
        }

        void _find_PrivateKeyPassword(object sender, PrivateKeyPasswordEventArgs e)
        {
            e.PrivateKeyPassword = _textBoxKeyPassword.Text;
        }

        private void EnableDialogItems()
        {
            bool bEnable = IsAnyServerUseTls();
            _labelCertificate.Enabled = bEnable;
            _buttonClientCertificate.Enabled = bEnable;
            _textBoxClientCertificate.Enabled = bEnable;

            _labelPrivateKey.Enabled = bEnable;
            _buttonClientCertificate.Enabled = bEnable;
            _textBoxClientCertificate.Enabled = bEnable;

            _labelPrivateKey.Enabled = bEnable;
            _buttonPrivateKey.Enabled = bEnable;
            _textBoxPrivateKey.Enabled = bEnable;

            _labelPrivateKeyPassword.Enabled = bEnable;
            _textBoxKeyPassword.Enabled = bEnable;
            _labelHint.Enabled = bEnable;
        }

        private bool CheckInteger(DevExpress.XtraEditors.TextEdit tb, DevExpress.XtraEditors.LabelControl lb)
        {
            try
            {
                Convert.ToInt32(tb.Text);
                return true;
            }
            catch (Exception)
            {
                if (tb.Text.Trim() == string.Empty)
                    MessageBox.Show("Invalid " + lb.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tb.SelectAll();
                tb.Focus();
                DialogResult = DialogResult.None;
                return false;
            }
        }

        private bool CheckIP(string ip)
        {
            try
            {
                System.Net.IPAddress.Parse(ip);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool CheckIP(DevExpress.XtraEditors.TextEdit tb, DevExpress.XtraEditors.LabelControl lb)
        {
            bool valid = CheckIP(tb.Text);
            if (!valid)
            {
                if (tb.Text.Trim() == string.Empty)
                    MessageBox.Show("Invalid " + lb.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tb.SelectAll();
                tb.Focus();
                DialogResult = DialogResult.None;
                return false;
            }
            return true;
        }

        private bool CheckFileExists(string title, DevExpress.XtraEditors.TextEdit tb, bool showMessageBox)
        {
            bool ret = true;
            string sMsg = string.Empty;
            string sFile = tb.Text.Trim();
            if (sFile.Length == 0)
            {
                sMsg = title + " Field Error\nField can not be empty if 'Secure (TLS)' is checked.";
                ret = false;
            }
            else if (!File.Exists(sFile))
            {
                sMsg = title + " Field Error\nFile does not exist: " + sFile;
                ret = false;
            }
            if ((ret == false) && showMessageBox)
            {
                MessageBox.Show(sMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tb.SelectAll();
                tb.Focus();
                DialogResult = DialogResult.None;
            }
            return ret;
        }

        private void DoAssociation(DicomScp dicomScp, QueryRetrieveScu _find, bool bFind)
        {
            string strError = "";
            if (!bFind)
            {
                lstAssociations = new List<AssociationHolder>();
                List<string> transfersyntax;

                transfersyntax = new List<string>();
                transfersyntax.Add(DicomUidType.ImplicitVRLittleEndian);
                lstAssociations.Add(new AssociationHolder(1, DicomUidType.VerificationClass, transfersyntax, "DICOM Verification"));

                //Encapsulated Pdf Storage
                transfersyntax = new List<string>();
                transfersyntax.Add(DicomUidType.ImplicitVRLittleEndian);
                transfersyntax.Add(DicomUidType.ExplicitVRLittleEndian);
                lstAssociations.Add(new AssociationHolder(3, DicomUidType.EncapsulatedPdfStorage, transfersyntax, "\nEncapsulated PDF Storage"));

                iLastNumber = 3;
                AddAssociate(lstAssociations, "\nSecondary Capture Image Storage", DicomUidType.SCImageStorage);
                AddAssociate(lstAssociations, "\nSecondary Capture Multi-Frame Grayscale Byte Image Storage", DicomUidType.SCMultiFrameTrueColorImageStorage);
                AddAssociate(lstAssociations, "\nSecondary Capture Multi-Frame True Color Image Storage", DicomUidType.SCMultiFrameGrayscaleByteImageStorage);

                try
                {
                    bTimeOut = true;
                    _find.Verify(dicomScp);
                }
                catch (Exception ex)
                {
                    strError = ", Reason:\n" + ex.Message;
                    bTimeOut = true;
                }
                if (bTimeOut)
                    MessageBox.Show("DICOM Verification Failed" + strError, "Print To PACS");
                else
                {
                    string Result = "";
                    foreach (AssociationHolder associate in lstAssociations)
                    {
                        Result += associate.Title + "  " + associate.Result + "\n";
                    }
                    MessageBox.Show(Result, "Print To PACS");
                }
            }
            else
            {
                lstAssociations = new List<AssociationHolder>();
                List<string> transfersyntax = new List<string>();
                transfersyntax.Add(DicomUidType.ImplicitVRLittleEndian);
                lstAssociations.Add(new AssociationHolder(1, DicomUidType.VerificationClass, transfersyntax, "DICOM Verification"));

                try
                {
                    bTimeOut = true;
                    _find.Verify(dicomScp);
                }
                catch (Exception ex)
                {
                    strError = ", Reason:\n" + ex.Message;
                    bTimeOut = true;
                }
                if (bTimeOut)
                    MessageBox.Show("DICOM Verification Failed" + strError, "Print To PACS");
                else
                {
                    string Result = "";
                    foreach (AssociationHolder associate in lstAssociations)
                    {
                        Result += associate.Title + "  " + associate.Result + "\n";
                    }
                    MessageBox.Show(Result, "Print To PACS");
                }
            }
        }

        private void AddAssociate(List<AssociationHolder> lstAssociations, string strTitle, string strClass)
        {
            List<string> transfersyntax;
            iLastNumber += 2;
            //Secondary Capture Image Storage
            transfersyntax = new List<string>();
            transfersyntax.Add(DicomUidType.ImplicitVRLittleEndian);
            transfersyntax.Add(DicomUidType.ExplicitVRLittleEndian);
            lstAssociations.Add(new AssociationHolder((byte)iLastNumber, strClass, transfersyntax, strTitle));
            iLastNumber += 2;
            //JPEG Baseline (Process 1)
            transfersyntax = new List<string>();
            transfersyntax.Add(DicomUidType.JPEGBaseline1);
            lstAssociations.Add(new AssociationHolder((byte)iLastNumber, strClass, transfersyntax, "--> JPEG Baseline (Process 1)"));
            iLastNumber += 2;
            //JPEG Lossless, Non-Hierarchical, First-Order Prediction 
            transfersyntax = new List<string>();
            transfersyntax.Add(DicomUidType.JPEGLosslessNonhier14B);
            lstAssociations.Add(new AssociationHolder((byte)iLastNumber, strClass, transfersyntax, "--> JPEG Lossless, Non-Hierarchical, First-Order Prediction"));
            iLastNumber += 2;
            //JPEG 2000 Image Compression (Lossless Only)
            transfersyntax = new List<string>();
            transfersyntax.Add(DicomUidType.JPEG2000LosslessOnly);
            lstAssociations.Add(new AssociationHolder((byte)iLastNumber, strClass, transfersyntax, "--> JPEG 2000 Image Compression (Lossless Only)"));
            iLastNumber += 2;
            //JPEG 2000 Image Compression
            transfersyntax = new List<string>();
            transfersyntax.Add(DicomUidType.JPEG2000);
            lstAssociations.Add(new AssociationHolder((byte)iLastNumber, strClass, transfersyntax, "--> JPEG 2000 Image Compression"));
        }

        // Return true if any of the servers are using tls
        // Return false if all of the servers do not use tls
        private bool IsAnyServerUseTls()
        {
            UpdateServers();

            for (int i = 0; i < serverlistSCP.serverList.Length; i++)
            {
                if (serverlistSCP.serverList[i]._useTls)
                    return true;
            }
            for (int i = 0; i < serverlistMWL.serverList.Length; i++)
            {
                if (serverlistMWL.serverList[i]._useTls)
                    return true;
            }
            for (int i = 0; i < serverlistStore.serverList.Length; i++)
            {
                if (serverlistStore.serverList[i]._useTls)
                    return true;
            }
            return false;
        }

        private void UpdateServers()
        {
            try
            {
                if (_tbServers.SelectedTab == _tbSCPQuerypage)
                    for (int i = 0; i < serverlistSCP.serverList.Length; i++)
                        serverlistSCP.serverList[i]._useTls = Convert.ToBoolean(dataGridViewServers.Rows[i].Cells["ColumnTls"].Value);

                if (_tbServers.SelectedTab == _tbMWLQueryPage)
                    for (int i = 0; i < serverlistSCP.serverList.Length; i++)
                        serverlistMWL.serverList[i]._useTls = Convert.ToBoolean(dataGridViewServers.Rows[i].Cells["ColumnTls"].Value);

                if (_tbServers.SelectedTab == _tbStorePage)
                    for (int i = 0; i < serverlistSCP.serverList.Length; i++)
                        serverlistStore.serverList[i]._useTls = Convert.ToBoolean(dataGridViewServers.Rows[i].Cells["ColumnTls"].Value);
            }
            catch { }
        }



        #endregion

        private async void OptionsDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            await VideoCapture1.StopAsync();
        }

        private async void _btnStopCamera_Click(object sender, EventArgs e)
        {
            await VideoCapture1.StopAsync();
        }
    }

    class AssociationHolder
    {
        public string Title;
        public string Result;
        public string PresentationContext;
        public List<string> TransferSyntax;
        public byte PresentationContextNumber;

        public AssociationHolder(byte presentationContextNumber, string presentationContext, List<string> transferSyntax, string title)
        {
            PresentationContext = presentationContext;
            TransferSyntax = transferSyntax;
            PresentationContextNumber = presentationContextNumber;
            Title = title;
        }
    }
}


