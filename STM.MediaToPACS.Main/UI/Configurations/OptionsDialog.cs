using System;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Collections.Generic;
using Leadtools.Dicom;
using Leadtools.Dicom.Common.Extensions;
using STM.MediaToPACS.Main.UI;
using Leadtools.Dicom.Scu.Common;
using Leadtools.Dicom.Scu;
using System.Threading.Tasks;
using VisioForge.Core.VideoCapture;
using VisioForge.Core.Types;
using VisioForge.Core.Types.Events;
using System.Linq;
using System.Globalization;
using VisioForge.Core.Types.VideoCapture;
using MediaToPacs.Core.Models;
using STM.MediaToPACS.Main.Utilities;
using System.Drawing;
using STM.MediaToPACS.Main.UI.Configurations;
using STM.MediaToPACS.Main.UI.Configurations.Systems;

namespace STM.MediaToPACS.Main
{
    /// <summary>
    /// Summary description for OptionsDialog.
    /// </summary>
    public class OptionsDialog : DevExpress.XtraEditors.XtraForm
    {
        #region Fields
        private VisioForge.Core.UI.WinForms.VideoView videoView1;
        private DevExpress.XtraEditors.SimpleButton buttonOK;
        private DevExpress.XtraEditors.SimpleButton buttonCancel;
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
        private DataGridViewTextBoxColumn ColumnAE;
        private DataGridViewTextBoxColumn ColumnIP;
        private DataGridViewTextBoxColumn ColumnPort;
        private DataGridViewTextBoxColumn ColumnTimeout;
        private DataGridViewCheckBoxColumn ColumnTls;
        private DataGridViewButtonColumn TestServer;
        private DataGridViewCheckBoxColumn DefaultServer;
        private DevExpress.XtraTab.XtraTabPage _tabTemplateManager;
        private DevExpress.XtraTab.XtraTabPage _xtraKySoPhimtat;
        private DevExpress.XtraEditors.PanelControl panelControl10;
        private PictureBox _picImageSign;
        private DevExpress.XtraEditors.ComboBoxEdit _cbbCert;
        private DevExpress.XtraEditors.LabelControl labelControl15;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraTab.XtraTabPage _tpCameraOptions;
        private DevExpress.XtraTab.XtraTabPage _tpDicomOptions;
        private TabControl _tbServers;
        private TabPage _tbSCPQuerypage;
        private TabPage _tbMWLQueryPage;
        private TabPage _tbStorePage;
        private GroupBox _groupBoxSecurity;
        private DevExpress.XtraEditors.LabelControl _labelCertificate;
        private DevExpress.XtraEditors.LabelControl _labelHint;
        private DevExpress.XtraEditors.SimpleButton _buttonClientCertificate;
        private DevExpress.XtraEditors.TextEdit _textBoxClientCertificate;
        private DevExpress.XtraEditors.TextEdit _textBoxKeyPassword;
        private DevExpress.XtraEditors.LabelControl _labelPrivateKeyPassword;
        private DevExpress.XtraEditors.LabelControl _labelPrivateKey;
        private DevExpress.XtraEditors.TextEdit _textBoxPrivateKey;
        private DevExpress.XtraEditors.SimpleButton _buttonPrivateKey;
        private GroupBox _groupBoxClient;
        public DevExpress.XtraEditors.TextEdit _textBoxClientAE;
        private DevExpress.XtraEditors.LabelControl _labelClientAE;
        private DevExpress.XtraTab.XtraTabPage _tpApplicationOptions;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.LabelControl _lblPrinterName;
        private DevExpress.XtraEditors.TextEdit _txtPrinterName;
        private DevExpress.XtraEditors.SimpleButton _btnRename;
        private DevExpress.XtraEditors.CheckEdit _ckAutoDelete;
        private GroupBox _gpDicomType;
        private DevExpress.XtraEditors.SimpleButton _btnBrowseSCPDF;
        private DevExpress.XtraEditors.SimpleButton _btnBrowseTempDir;
        private DevExpress.XtraEditors.LabelControl label3;
        private DevExpress.XtraEditors.SimpleButton _btnBrowseSCGray;
        private DevExpress.XtraEditors.SimpleButton _btnBrowseSCColor;
        private DevExpress.XtraEditors.SimpleButton _btnBrowseSC;
        private DevExpress.XtraEditors.LabelControl label2;
        private DevExpress.XtraEditors.TextEdit _txtTempDir;
        private DevExpress.XtraEditors.ComboBoxEdit _cmbSCColor;
        private DevExpress.XtraEditors.ComboBoxEdit _cmbSCGray;
        private DevExpress.XtraEditors.ComboBoxEdit _cmbSC;
        private DevExpress.XtraEditors.LabelControl label1;
        private RadioButton _rdPDF;
        private RadioButton _rdGrayScale;
        private RadioButton _rdColored;
        private RadioButton _rdSecondaryCapture;
        private DevExpress.XtraEditors.TextEdit _txtSCPDF;
        private DevExpress.XtraEditors.TextEdit _txtSCGray;
        private DevExpress.XtraEditors.TextEdit _txtSCColor;
        private DevExpress.XtraEditors.TextEdit _txtSC;
        private DevExpress.XtraTab.XtraTabControl _tbOptions;
        private DevExpress.XtraEditors.SimpleButton _btnSetUpKeyShortcuts;
        private DevExpress.XtraEditors.SimpleButton _btnSetUpSystem;
        private DevExpress.XtraEditors.PanelControl panelControl1;

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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this._tabTemplateManager = new DevExpress.XtraTab.XtraTabPage();
            this._xtraKySoPhimtat = new DevExpress.XtraTab.XtraTabPage();
            this.panelControl10 = new DevExpress.XtraEditors.PanelControl();
            this._btnSetUpSystem = new DevExpress.XtraEditors.SimpleButton();
            this._btnSetUpKeyShortcuts = new DevExpress.XtraEditors.SimpleButton();
            this._picImageSign = new System.Windows.Forms.PictureBox();
            this._cbbCert = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this._tpCameraOptions = new DevExpress.XtraTab.XtraTabPage();
            this._tpDicomOptions = new DevExpress.XtraTab.XtraTabPage();
            this._tbServers = new System.Windows.Forms.TabControl();
            this._tbSCPQuerypage = new System.Windows.Forms.TabPage();
            this._tbMWLQueryPage = new System.Windows.Forms.TabPage();
            this._tbStorePage = new System.Windows.Forms.TabPage();
            this._groupBoxSecurity = new System.Windows.Forms.GroupBox();
            this._labelCertificate = new DevExpress.XtraEditors.LabelControl();
            this._labelHint = new DevExpress.XtraEditors.LabelControl();
            this._buttonClientCertificate = new DevExpress.XtraEditors.SimpleButton();
            this._textBoxClientCertificate = new DevExpress.XtraEditors.TextEdit();
            this._textBoxKeyPassword = new DevExpress.XtraEditors.TextEdit();
            this._labelPrivateKeyPassword = new DevExpress.XtraEditors.LabelControl();
            this._labelPrivateKey = new DevExpress.XtraEditors.LabelControl();
            this._textBoxPrivateKey = new DevExpress.XtraEditors.TextEdit();
            this._buttonPrivateKey = new DevExpress.XtraEditors.SimpleButton();
            this._groupBoxClient = new System.Windows.Forms.GroupBox();
            this._textBoxClientAE = new DevExpress.XtraEditors.TextEdit();
            this._labelClientAE = new DevExpress.XtraEditors.LabelControl();
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
            this._tbOptions = new DevExpress.XtraTab.XtraTabControl();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServers)).BeginInit();
            this.panelButtonServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this._xtraKySoPhimtat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl10)).BeginInit();
            this.panelControl10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._picImageSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._cbbCert.Properties)).BeginInit();
            this._tpDicomOptions.SuspendLayout();
            this._tbServers.SuspendLayout();
            this._groupBoxSecurity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._textBoxClientCertificate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._textBoxKeyPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._textBoxPrivateKey.Properties)).BeginInit();
            this._groupBoxClient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._textBoxClientAE.Properties)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(this._tbOptions)).BeginInit();
            this._tbOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(20, 12);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(123, 32);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "&Lưu";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.CausesValidation = false;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(150, 12);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(136, 32);
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
            this.ColumnIP.Width = 125;
            // 
            // ColumnPort
            // 
            this.ColumnPort.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColumnPort.HeaderText = "Server Port";
            this.ColumnPort.MinimumWidth = 6;
            this.ColumnPort.Name = "ColumnPort";
            this.ColumnPort.Width = 125;
            // 
            // ColumnTimeout
            // 
            this.ColumnTimeout.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColumnTimeout.HeaderText = "Timeout (sec)";
            this.ColumnTimeout.MinimumWidth = 6;
            this.ColumnTimeout.Name = "ColumnTimeout";
            this.ColumnTimeout.Width = 125;
            // 
            // ColumnTls
            // 
            this.ColumnTls.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnTls.HeaderText = "Secure (TLS)";
            this.ColumnTls.MinimumWidth = 6;
            this.ColumnTls.Name = "ColumnTls";
            this.ColumnTls.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnTls.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColumnTls.Width = 125;
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
            this.buttonAddServer.ImageOptions.Image = global::STM.MediaToPACS.Main.Properties.Resources.client_add_32;
            this.buttonAddServer.Location = new System.Drawing.Point(6, 6);
            this.buttonAddServer.Name = "buttonAddServer";
            this.buttonAddServer.Size = new System.Drawing.Size(40, 39);
            this.buttonAddServer.TabIndex = 7;
            this.buttonAddServer.Click += new System.EventHandler(this.buttonAddServer_Click);
            // 
            // buttonDeleteServer
            // 
            this.buttonDeleteServer.CausesValidation = false;
            this.buttonDeleteServer.ImageOptions.Image = global::STM.MediaToPACS.Main.Properties.Resources.client_remove_32;
            this.buttonDeleteServer.Location = new System.Drawing.Point(49, 6);
            this.buttonDeleteServer.Name = "buttonDeleteServer";
            this.buttonDeleteServer.Size = new System.Drawing.Size(40, 39);
            this.buttonDeleteServer.TabIndex = 8;
            this.buttonDeleteServer.Click += new System.EventHandler(this.buttonDeleteServer_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.buttonCancel);
            this.panelControl1.Controls.Add(this.buttonOK);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 607);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(991, 61);
            this.panelControl1.TabIndex = 19;
            // 
            // _tabTemplateManager
            // 
            this._tabTemplateManager.Name = "_tabTemplateManager";
            this._tabTemplateManager.Size = new System.Drawing.Size(981, 569);
            this._tabTemplateManager.Text = "Quản lý mẫu";
            // 
            // _xtraKySoPhimtat
            // 
            this._xtraKySoPhimtat.Controls.Add(this.panelControl10);
            this._xtraKySoPhimtat.Name = "_xtraKySoPhimtat";
            this._xtraKySoPhimtat.Size = new System.Drawing.Size(981, 569);
            this._xtraKySoPhimtat.Text = "Ký số & Phím tắt";
            // 
            // panelControl10
            // 
            this.panelControl10.Controls.Add(this._btnSetUpSystem);
            this.panelControl10.Controls.Add(this._btnSetUpKeyShortcuts);
            this.panelControl10.Controls.Add(this._picImageSign);
            this.panelControl10.Controls.Add(this._cbbCert);
            this.panelControl10.Controls.Add(this.labelControl15);
            this.panelControl10.Controls.Add(this.labelControl14);
            this.panelControl10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl10.Location = new System.Drawing.Point(0, 0);
            this.panelControl10.Name = "panelControl10";
            this.panelControl10.Size = new System.Drawing.Size(981, 569);
            this.panelControl10.TabIndex = 0;
            // 
            // _btnSetUpSystem
            // 
            this._btnSetUpSystem.Location = new System.Drawing.Point(542, 46);
            this._btnSetUpSystem.Name = "_btnSetUpSystem";
            this._btnSetUpSystem.Size = new System.Drawing.Size(173, 28);
            this._btnSetUpSystem.TabIndex = 8;
            this._btnSetUpSystem.Text = "Cấu hình hệ thống";
            this._btnSetUpSystem.Click += new System.EventHandler(this._btnSetUpSystem_Click);
            // 
            // _btnSetUpKeyShortcuts
            // 
            this._btnSetUpKeyShortcuts.Location = new System.Drawing.Point(542, 11);
            this._btnSetUpKeyShortcuts.Name = "_btnSetUpKeyShortcuts";
            this._btnSetUpKeyShortcuts.Size = new System.Drawing.Size(173, 28);
            this._btnSetUpKeyShortcuts.TabIndex = 7;
            this._btnSetUpKeyShortcuts.Text = "Cấu hình phím tắt";
            this._btnSetUpKeyShortcuts.Click += new System.EventHandler(this._btnSetUpKeyShortcuts_Click);
            // 
            // _picImageSign
            // 
            this._picImageSign.Location = new System.Drawing.Point(113, 51);
            this._picImageSign.Name = "_picImageSign";
            this._picImageSign.Size = new System.Drawing.Size(408, 125);
            this._picImageSign.TabIndex = 6;
            this._picImageSign.TabStop = false;
            // 
            // _cbbCert
            // 
            this._cbbCert.Location = new System.Drawing.Point(113, 13);
            this._cbbCert.Name = "_cbbCert";
            this._cbbCert.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._cbbCert.Size = new System.Drawing.Size(408, 24);
            this._cbbCert.TabIndex = 2;
            this._cbbCert.SelectedIndexChanged += new System.EventHandler(this._cbbCert_SelectedIndexChanged);
            // 
            // labelControl15
            // 
            this.labelControl15.Location = new System.Drawing.Point(15, 51);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(69, 18);
            this.labelControl15.TabIndex = 1;
            this.labelControl15.Text = "Ảnh ký số:";
            // 
            // labelControl14
            // 
            this.labelControl14.Location = new System.Drawing.Point(15, 16);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(92, 18);
            this.labelControl14.TabIndex = 0;
            this.labelControl14.Text = "Chứng thư số:";
            // 
            // _tpCameraOptions
            // 
            this._tpCameraOptions.Name = "_tpCameraOptions";
            this._tpCameraOptions.Size = new System.Drawing.Size(981, 569);
            this._tpCameraOptions.Text = "Cấu hình camera";
            // 
            // _tpDicomOptions
            // 
            this._tpDicomOptions.AutoScroll = true;
            this._tpDicomOptions.Controls.Add(this._tbServers);
            this._tpDicomOptions.Controls.Add(this._groupBoxSecurity);
            this._tpDicomOptions.Controls.Add(this._groupBoxClient);
            this._tpDicomOptions.Name = "_tpDicomOptions";
            this._tpDicomOptions.Padding = new System.Windows.Forms.Padding(3);
            this._tpDicomOptions.Size = new System.Drawing.Size(981, 569);
            this._tpDicomOptions.Text = "Cấu hình PACS";
            // 
            // _tbServers
            // 
            this._tbServers.Controls.Add(this._tbSCPQuerypage);
            this._tbServers.Controls.Add(this._tbMWLQueryPage);
            this._tbServers.Controls.Add(this._tbStorePage);
            this._tbServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tbServers.Location = new System.Drawing.Point(3, 266);
            this._tbServers.Name = "_tbServers";
            this._tbServers.SelectedIndex = 0;
            this._tbServers.Size = new System.Drawing.Size(975, 300);
            this._tbServers.TabIndex = 17;
            this._tbServers.SelectedIndexChanged += new System.EventHandler(this._tbServers_SelectedIndexChanged);
            this._tbServers.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this._tbServers_Selecting);
            // 
            // _tbSCPQuerypage
            // 
            this._tbSCPQuerypage.AutoScroll = true;
            this._tbSCPQuerypage.Location = new System.Drawing.Point(4, 27);
            this._tbSCPQuerypage.Name = "_tbSCPQuerypage";
            this._tbSCPQuerypage.Padding = new System.Windows.Forms.Padding(3);
            this._tbSCPQuerypage.Size = new System.Drawing.Size(967, 269);
            this._tbSCPQuerypage.TabIndex = 0;
            this._tbSCPQuerypage.Text = "Máy chủ Query";
            this._tbSCPQuerypage.UseVisualStyleBackColor = true;
            // 
            // _tbMWLQueryPage
            // 
            this._tbMWLQueryPage.Location = new System.Drawing.Point(4, 27);
            this._tbMWLQueryPage.Name = "_tbMWLQueryPage";
            this._tbMWLQueryPage.Padding = new System.Windows.Forms.Padding(3);
            this._tbMWLQueryPage.Size = new System.Drawing.Size(967, 269);
            this._tbMWLQueryPage.TabIndex = 1;
            this._tbMWLQueryPage.Text = "Máy chủ Worklist";
            this._tbMWLQueryPage.UseVisualStyleBackColor = true;
            // 
            // _tbStorePage
            // 
            this._tbStorePage.Location = new System.Drawing.Point(4, 27);
            this._tbStorePage.Name = "_tbStorePage";
            this._tbStorePage.Padding = new System.Windows.Forms.Padding(3);
            this._tbStorePage.Size = new System.Drawing.Size(967, 269);
            this._tbStorePage.TabIndex = 2;
            this._tbStorePage.Text = "Máy chủ PACS";
            this._tbStorePage.UseVisualStyleBackColor = true;
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
            this._groupBoxSecurity.Location = new System.Drawing.Point(3, 90);
            this._groupBoxSecurity.Name = "_groupBoxSecurity";
            this._groupBoxSecurity.Size = new System.Drawing.Size(975, 176);
            this._groupBoxSecurity.TabIndex = 16;
            this._groupBoxSecurity.TabStop = false;
            this._groupBoxSecurity.Text = "Bảo mật";
            // 
            // _labelCertificate
            // 
            this._labelCertificate.Location = new System.Drawing.Point(21, 42);
            this._labelCertificate.Name = "_labelCertificate";
            this._labelCertificate.Size = new System.Drawing.Size(83, 18);
            this._labelCertificate.TabIndex = 7;
            this._labelCertificate.Text = "Chứng nhận:";
            // 
            // _labelHint
            // 
            this._labelHint.Appearance.ForeColor = System.Drawing.Color.Blue;
            this._labelHint.Appearance.Options.UseForeColor = true;
            this._labelHint.Location = new System.Drawing.Point(419, 125);
            this._labelHint.Name = "_labelHint";
            this._labelHint.Size = new System.Drawing.Size(219, 18);
            this._labelHint.TabIndex = 15;
            this._labelHint.Text = "<== Sử dụng \'test\' cho client.pem";
            // 
            // _buttonClientCertificate
            // 
            this._buttonClientCertificate.Location = new System.Drawing.Point(147, 38);
            this._buttonClientCertificate.Name = "_buttonClientCertificate";
            this._buttonClientCertificate.Size = new System.Drawing.Size(41, 24);
            this._buttonClientCertificate.TabIndex = 8;
            this._buttonClientCertificate.Text = "...";
            this._buttonClientCertificate.Click += new System.EventHandler(this._buttonClientCertificate_Click);
            // 
            // _textBoxClientCertificate
            // 
            this._textBoxClientCertificate.Location = new System.Drawing.Point(196, 38);
            this._textBoxClientCertificate.Name = "_textBoxClientCertificate";
            this._textBoxClientCertificate.Size = new System.Drawing.Size(658, 24);
            this._textBoxClientCertificate.TabIndex = 9;
            // 
            // _textBoxKeyPassword
            // 
            this._textBoxKeyPassword.Location = new System.Drawing.Point(196, 123);
            this._textBoxKeyPassword.Name = "_textBoxKeyPassword";
            this._textBoxKeyPassword.Size = new System.Drawing.Size(204, 24);
            this._textBoxKeyPassword.TabIndex = 14;
            // 
            // _labelPrivateKeyPassword
            // 
            this._labelPrivateKeyPassword.Location = new System.Drawing.Point(21, 123);
            this._labelPrivateKeyPassword.Name = "_labelPrivateKeyPassword";
            this._labelPrivateKeyPassword.Size = new System.Drawing.Size(65, 18);
            this._labelPrivateKeyPassword.TabIndex = 13;
            this._labelPrivateKeyPassword.Text = "Mật khẩu:";
            // 
            // _labelPrivateKey
            // 
            this._labelPrivateKey.Location = new System.Drawing.Point(21, 79);
            this._labelPrivateKey.Name = "_labelPrivateKey";
            this._labelPrivateKey.Size = new System.Drawing.Size(91, 18);
            this._labelPrivateKey.TabIndex = 10;
            this._labelPrivateKey.Text = "Khóa riêng tư:";
            // 
            // _textBoxPrivateKey
            // 
            this._textBoxPrivateKey.Location = new System.Drawing.Point(196, 79);
            this._textBoxPrivateKey.Name = "_textBoxPrivateKey";
            this._textBoxPrivateKey.Size = new System.Drawing.Size(658, 24);
            this._textBoxPrivateKey.TabIndex = 12;
            // 
            // _buttonPrivateKey
            // 
            this._buttonPrivateKey.Location = new System.Drawing.Point(147, 79);
            this._buttonPrivateKey.Name = "_buttonPrivateKey";
            this._buttonPrivateKey.Size = new System.Drawing.Size(41, 27);
            this._buttonPrivateKey.TabIndex = 11;
            this._buttonPrivateKey.Text = "...";
            this._buttonPrivateKey.Click += new System.EventHandler(this._buttonPrivateKey_Click);
            // 
            // _groupBoxClient
            // 
            this._groupBoxClient.Controls.Add(this._textBoxClientAE);
            this._groupBoxClient.Controls.Add(this._labelClientAE);
            this._groupBoxClient.Dock = System.Windows.Forms.DockStyle.Top;
            this._groupBoxClient.Location = new System.Drawing.Point(3, 3);
            this._groupBoxClient.Name = "_groupBoxClient";
            this._groupBoxClient.Size = new System.Drawing.Size(975, 87);
            this._groupBoxClient.TabIndex = 1;
            this._groupBoxClient.TabStop = false;
            this._groupBoxClient.Text = "Thông tin máy thực hiện";
            // 
            // _textBoxClientAE
            // 
            this._textBoxClientAE.Location = new System.Drawing.Point(197, 33);
            this._textBoxClientAE.Name = "_textBoxClientAE";
            this._textBoxClientAE.Size = new System.Drawing.Size(657, 24);
            this._textBoxClientAE.TabIndex = 1;
            // 
            // _labelClientAE
            // 
            this._labelClientAE.Location = new System.Drawing.Point(21, 39);
            this._labelClientAE.Name = "_labelClientAE";
            this._labelClientAE.Size = new System.Drawing.Size(50, 18);
            this._labelClientAE.TabIndex = 0;
            this._labelClientAE.Text = "AE Title";
            // 
            // _tpApplicationOptions
            // 
            this._tpApplicationOptions.Controls.Add(this.panelControl2);
            this._tpApplicationOptions.Controls.Add(this._ckAutoDelete);
            this._tpApplicationOptions.Controls.Add(this._gpDicomType);
            this._tpApplicationOptions.Name = "_tpApplicationOptions";
            this._tpApplicationOptions.Padding = new System.Windows.Forms.Padding(3);
            this._tpApplicationOptions.Size = new System.Drawing.Size(981, 569);
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
            this.panelControl2.Size = new System.Drawing.Size(975, 70);
            this.panelControl2.TabIndex = 32;
            // 
            // _lblPrinterName
            // 
            this._lblPrinterName.Location = new System.Drawing.Point(22, 23);
            this._lblPrinterName.Name = "_lblPrinterName";
            this._lblPrinterName.Size = new System.Drawing.Size(204, 18);
            this._lblPrinterName.TabIndex = 0;
            this._lblPrinterName.Text = "Trình điều khiển máy in DICOM";
            // 
            // _txtPrinterName
            // 
            this._txtPrinterName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtPrinterName.Location = new System.Drawing.Point(244, 21);
            this._txtPrinterName.Name = "_txtPrinterName";
            this._txtPrinterName.Properties.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this._txtPrinterName.Properties.Appearance.Options.UseBackColor = true;
            this._txtPrinterName.Properties.ReadOnly = true;
            this._txtPrinterName.Size = new System.Drawing.Size(569, 24);
            this._txtPrinterName.TabIndex = 1;
            // 
            // _btnRename
            // 
            this._btnRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnRename.Location = new System.Drawing.Point(820, 18);
            this._btnRename.Name = "_btnRename";
            this._btnRename.Size = new System.Drawing.Size(146, 28);
            this._btnRename.TabIndex = 22;
            this._btnRename.Text = "Đổi tên";
            // 
            // _ckAutoDelete
            // 
            this._ckAutoDelete.Location = new System.Drawing.Point(31, 740);
            this._ckAutoDelete.Name = "_ckAutoDelete";
            this._ckAutoDelete.Properties.Caption = "Auto delete Images after successful transfer";
            this._ckAutoDelete.Size = new System.Drawing.Size(466, 22);
            this._ckAutoDelete.TabIndex = 26;
            // 
            // _gpDicomType
            // 
            this._gpDicomType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this._gpDicomType.Location = new System.Drawing.Point(6, 97);
            this._gpDicomType.Name = "_gpDicomType";
            this._gpDicomType.Size = new System.Drawing.Size(1243, 244);
            this._gpDicomType.TabIndex = 7;
            this._gpDicomType.TabStop = false;
            this._gpDicomType.Text = "Loại DICOM";
            // 
            // _btnBrowseSCPDF
            // 
            this._btnBrowseSCPDF.Location = new System.Drawing.Point(377, 152);
            this._btnBrowseSCPDF.Name = "_btnBrowseSCPDF";
            this._btnBrowseSCPDF.Size = new System.Drawing.Size(43, 24);
            this._btnBrowseSCPDF.TabIndex = 30;
            this._btnBrowseSCPDF.Text = "...";
            // 
            // _btnBrowseTempDir
            // 
            this._btnBrowseTempDir.Location = new System.Drawing.Point(367, 188);
            this._btnBrowseTempDir.Name = "_btnBrowseTempDir";
            this._btnBrowseTempDir.Size = new System.Drawing.Size(53, 32);
            this._btnBrowseTempDir.TabIndex = 31;
            this._btnBrowseTempDir.Text = "...";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(20, 196);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 18);
            this.label3.TabIndex = 23;
            this.label3.Text = "Thư mục tạm thời DICOM";
            // 
            // _btnBrowseSCGray
            // 
            this._btnBrowseSCGray.Location = new System.Drawing.Point(377, 114);
            this._btnBrowseSCGray.Name = "_btnBrowseSCGray";
            this._btnBrowseSCGray.Size = new System.Drawing.Size(43, 27);
            this._btnBrowseSCGray.TabIndex = 29;
            this._btnBrowseSCGray.Text = "...";
            // 
            // _btnBrowseSCColor
            // 
            this._btnBrowseSCColor.Location = new System.Drawing.Point(377, 79);
            this._btnBrowseSCColor.Name = "_btnBrowseSCColor";
            this._btnBrowseSCColor.Size = new System.Drawing.Size(43, 27);
            this._btnBrowseSCColor.TabIndex = 28;
            this._btnBrowseSCColor.Text = "...";
            // 
            // _btnBrowseSC
            // 
            this._btnBrowseSC.Location = new System.Drawing.Point(377, 44);
            this._btnBrowseSC.Name = "_btnBrowseSC";
            this._btnBrowseSC.Size = new System.Drawing.Size(43, 24);
            this._btnBrowseSC.TabIndex = 27;
            this._btnBrowseSC.Text = "...";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(729, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 18);
            this.label2.TabIndex = 20;
            this.label2.Text = "Bản nén";
            // 
            // _txtTempDir
            // 
            this._txtTempDir.Location = new System.Drawing.Point(435, 192);
            this._txtTempDir.Name = "_txtTempDir";
            this._txtTempDir.Size = new System.Drawing.Size(440, 24);
            this._txtTempDir.TabIndex = 2;
            this._txtTempDir.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // _cmbSCColor
            // 
            this._cmbSCColor.Location = new System.Drawing.Point(729, 80);
            this._cmbSCColor.Name = "_cmbSCColor";
            this._cmbSCColor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._cmbSCColor.Size = new System.Drawing.Size(140, 24);
            this._cmbSCColor.TabIndex = 19;
            // 
            // _cmbSCGray
            // 
            this._cmbSCGray.Location = new System.Drawing.Point(729, 114);
            this._cmbSCGray.Name = "_cmbSCGray";
            this._cmbSCGray.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._cmbSCGray.Size = new System.Drawing.Size(140, 24);
            this._cmbSCGray.TabIndex = 18;
            // 
            // _cmbSC
            // 
            this._cmbSC.Location = new System.Drawing.Point(729, 45);
            this._cmbSC.Name = "_cmbSC";
            this._cmbSC.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._cmbSC.Size = new System.Drawing.Size(140, 24);
            this._cmbSC.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(377, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 18);
            this.label1.TabIndex = 15;
            this.label1.Text = "Bản mẫu";
            // 
            // _rdPDF
            // 
            this._rdPDF.AutoSize = true;
            this._rdPDF.Location = new System.Drawing.Point(20, 152);
            this._rdPDF.Name = "_rdPDF";
            this._rdPDF.Size = new System.Drawing.Size(113, 22);
            this._rdPDF.TabIndex = 14;
            this._rdPDF.Text = "DICOM PDF";
            this._rdPDF.UseVisualStyleBackColor = true;
            // 
            // _rdGrayScale
            // 
            this._rdGrayScale.AutoSize = true;
            this._rdGrayScale.Location = new System.Drawing.Point(20, 118);
            this._rdGrayScale.Name = "_rdGrayScale";
            this._rdGrayScale.Size = new System.Drawing.Size(307, 22);
            this._rdGrayScale.TabIndex = 13;
            this._rdGrayScale.Text = "Chụp thứ cấp Đa khung hình thang độ xám";
            this._rdGrayScale.UseVisualStyleBackColor = true;
            // 
            // _rdColored
            // 
            this._rdColored.AutoSize = true;
            this._rdColored.Location = new System.Drawing.Point(20, 80);
            this._rdColored.Name = "_rdColored";
            this._rdColored.Size = new System.Drawing.Size(208, 22);
            this._rdColored.TabIndex = 12;
            this._rdColored.Text = "Chụp phụ nhiều khung màu";
            this._rdColored.UseVisualStyleBackColor = true;
            // 
            // _rdSecondaryCapture
            // 
            this._rdSecondaryCapture.AutoSize = true;
            this._rdSecondaryCapture.Checked = true;
            this._rdSecondaryCapture.Location = new System.Drawing.Point(20, 46);
            this._rdSecondaryCapture.Name = "_rdSecondaryCapture";
            this._rdSecondaryCapture.Size = new System.Drawing.Size(92, 22);
            this._rdSecondaryCapture.TabIndex = 11;
            this._rdSecondaryCapture.TabStop = true;
            this._rdSecondaryCapture.Text = "Chụp phụ";
            this._rdSecondaryCapture.UseVisualStyleBackColor = true;
            // 
            // _txtSCPDF
            // 
            this._txtSCPDF.Location = new System.Drawing.Point(435, 152);
            this._txtSCPDF.Name = "_txtSCPDF";
            this._txtSCPDF.Size = new System.Drawing.Size(269, 24);
            this._txtSCPDF.TabIndex = 10;
            this._txtSCPDF.Tag = "3";
            // 
            // _txtSCGray
            // 
            this._txtSCGray.Location = new System.Drawing.Point(435, 114);
            this._txtSCGray.Name = "_txtSCGray";
            this._txtSCGray.Size = new System.Drawing.Size(269, 24);
            this._txtSCGray.TabIndex = 9;
            this._txtSCGray.Tag = "2";
            // 
            // _txtSCColor
            // 
            this._txtSCColor.Location = new System.Drawing.Point(435, 80);
            this._txtSCColor.Name = "_txtSCColor";
            this._txtSCColor.Size = new System.Drawing.Size(269, 24);
            this._txtSCColor.TabIndex = 8;
            this._txtSCColor.Tag = "1";
            this._txtSCColor.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // _txtSC
            // 
            this._txtSC.Location = new System.Drawing.Point(435, 45);
            this._txtSC.Name = "_txtSC";
            this._txtSC.Size = new System.Drawing.Size(269, 24);
            this._txtSC.TabIndex = 7;
            this._txtSC.Tag = "0";
            // 
            // _tbOptions
            // 
            this._tbOptions.Appearance.BackColor = System.Drawing.Color.Gray;
            this._tbOptions.Appearance.Options.UseBackColor = true;
            this._tbOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tbOptions.Location = new System.Drawing.Point(0, 0);
            this._tbOptions.Name = "_tbOptions";
            this._tbOptions.SelectedTabPage = this._tpApplicationOptions;
            this._tbOptions.Size = new System.Drawing.Size(991, 607);
            this._tbOptions.TabIndex = 18;
            this._tbOptions.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this._tpApplicationOptions,
            this._tpDicomOptions,
            this._tpCameraOptions,
            this._xtraKySoPhimtat,
            this._tabTemplateManager});
            // 
            // OptionsDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(991, 668);
            this.Controls.Add(this._tbOptions);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IconOptions.Image = global::STM.MediaToPACS.Main.Properties.Resources.stm;
            this.Name = "OptionsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cài đặt";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OptionsDialog_FormClosed);
            this.Load += new System.EventHandler(this.OptionsDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServers)).EndInit();
            this.panelButtonServer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this._xtraKySoPhimtat.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl10)).EndInit();
            this.panelControl10.ResumeLayout(false);
            this.panelControl10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._picImageSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._cbbCert.Properties)).EndInit();
            this._tpDicomOptions.ResumeLayout(false);
            this._tbServers.ResumeLayout(false);
            this._groupBoxSecurity.ResumeLayout(false);
            this._groupBoxSecurity.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._textBoxClientCertificate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._textBoxKeyPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._textBoxPrivateKey.Properties)).EndInit();
            this._groupBoxClient.ResumeLayout(false);
            this._groupBoxClient.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._textBoxClientAE.Properties)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this._tbOptions)).EndInit();
            this._tbOptions.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Properties

        public double Zoom = 1.0;
        public int ZoomShiftX;
        public int ZoomShiftY;
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
            _configCamera.SaveSettingsCamera();
        }

        private void OptionsDialog_Load(object sender, EventArgs e)
        {
            InitSignatureKey();
            InitTemplateManager();

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

            this.Width = 990;
            this.Height = 700;
            InitSettingConfigCamera();
            
        }

        private ConfigCamera _configCamera;
        private void InitSettingConfigCamera()
        {
            _configCamera = new ConfigCamera()
            {
                Dock = DockStyle.Fill
            };
            _tpCameraOptions.Controls.Add(_configCamera);
        }

        private void InitTemplateManager()
        {
            var reportTemplateManager = new ReportTemplateManager()
            {
                Dock = DockStyle.Fill
            };
            _tabTemplateManager.Controls.Add(reportTemplateManager);
        }

        private void InitSignatureKey()
        {
            try
            {
                if (ServiceLocator.UserInfo != null)
                {
                    _cbbCert.Text = ServiceLocator.UserInfo.Certificate.CredentialId;
                    byte[] imageBytes = Convert.FromBase64String(ServiceLocator.UserInfo.imageData);
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        _picImageSign.Image = Image.FromStream(ms);
                        _picImageSign.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                }
            }
            catch (Exception)
            {

            }
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

        private  void OptionsDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                _configCamera._btnStopCamera_Click(null, null);
            }catch(Exception ex)
            {

            }
        }


        private string _base64Image = null;
        private CertBO _selectedCert = null;
        private string _selectedCredentialId = null;
        private Dictionary<string, CertBO> _certificates;
        private void _btnSelectImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn hình ảnh";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                ofd.Multiselect = false; // chỉ chọn 1 file

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string filePath = ofd.FileName;
                    _picImageSign.Image = Image.FromFile(filePath);
                    _picImageSign.SizeMode = PictureBoxSizeMode.Zoom;

                    byte[] fileBytes = File.ReadAllBytes(filePath);
                    _base64Image = Convert.ToBase64String(fileBytes);
                }
            }
        }

        private async void _btnGetCert_Click(object sender, EventArgs e)
        {
            try
            {
                _certificates = await ServiceLocator.SignatureService
                    .GetCertificateInfosAsync(ServiceLocator.KeycloakUserInfo.CCCD);

                if (_certificates != null && _certificates.Any())
                {
                    MessageBox.Show("Lấy chứng chỉ số thành công !");
                    _cbbCert.Properties.Items.Clear();
                    _cbbCert.Properties.Items.AddRange(_certificates.Keys.ToList());

                    string firstKey = _certificates.Keys.First();
                    _selectedCert = _certificates[firstKey];
                    _selectedCredentialId = firstKey;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy chứng chỉ: " + ex.Message);
            }
        }

        private void _cbbCert_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cbbCert.SelectedItem != null)
            {
                string selectedKey = _cbbCert.SelectedItem.ToString();

                if (_certificates != null && _certificates.ContainsKey(selectedKey))
                {
                    _selectedCert = _certificates[selectedKey];
                    _selectedCredentialId = selectedKey;
                }
            }
        }

        private void btOutputConfigure_Click(object sender, EventArgs e)
        {

        }

        private void _btnSetUpKeyShortcuts_Click(object sender, EventArgs e)
        {
            SetUpKeyShortcuts setUpKeyShortcuts = new SetUpKeyShortcuts();
            setUpKeyShortcuts.ShowDialog();
        }

        private void _btnSetUpSystem_Click(object sender, EventArgs e)
        {
            SystemSetup systemSetup = new SystemSetup();
            systemSetup.ShowDialog(this);
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


