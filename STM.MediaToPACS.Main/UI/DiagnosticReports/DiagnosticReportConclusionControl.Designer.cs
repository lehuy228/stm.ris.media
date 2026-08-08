using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTab;

namespace STM.MediaToPACS.Main.UI.DiagnosticReports
{
    partial class DiagnosticReportConclusionControl
    {
        private System.ComponentModel.IContainer components = null;

        // Layout g?c - b� nguy�n b? c?c c?a FrmMain.Designer.cs (panelControl4 tr�n c�ng,
        // tab K?t lu?n b�n tr�i, camera+n�t+?nh b�n ph?i, thanh n�t du?i c�ng), b? MenuStrip,
        // ToolStrip DICOM, tab "T?p d? li?u DICOM" v� n�t "T?i ?nh l�n PACS".
        private TableLayoutPanel _tbTableLayout;
        private Panel _bodyPanel;
        private TableLayoutPanel _contentTable;

        // Sidebar tr�i: l?ch s? kh�m b?nh nh�n + tham s? si�u �m (bu?c 3)
        private STM.MediaToPACS.Main.UI.PatientSidebar.PatientSidebarControl _patientSidebar;
        private Splitter _patientSidebarSplitter;

        // panelControl4: th�ng tin b?nh nh�n/ch? d?nh (h�ng tr�n c�ng, colspan 2) - b? c?c g?n
        // b?ng FlowLayoutPanel t? xu?ng d�ng (CreateLabeledField) thay v� to? d? tuy?t d?i,
        // d? kh�ng b? tr�n/khu?t khi chi?u r?ng th?c t? nh? hon thi?t k? g?c.
        private PanelControl _panelControl4;
        private Panel _patientActionBar;
        private Label _patientSectionTitle;
        private FlowLayoutPanel _patientActionButtons;
        private CheckBox _cbHoverPreview;
        private SimpleButton _btnEditPatient;
        private SimpleButton _btnAddFile;
        private TableLayoutPanel _patientInfoTable;
        private TextEdit _txMaBN;
        private TextEdit _txTenBN;
        private DateEdit _dateBN;
        private TextEdit _txPatientGender;
        private TextEdit _txQueQuan;
        private TextEdit _txDoiTuong;
        private TextEdit _txMaChiDinh;
        private DateEdit _dateNgayChiDinh;
        private TextEdit _txBSChiDinh;
        private TextEdit _txMaBHYT;
        private TextEdit _txDichVu;
        private TextEdit _txBSDoc;
        private LookUpEdit _cbbHisUser;
        private LookUpEdit _cbbDSThietBi;
        private DateEdit _dateTGThucHien;
        private DateEdit _dateTGKetThuc;
        private Panel _panelChanDoan;
        private LabelControl _labelControl24;
        private TextEdit _txChanDoan;

        // xtraTabControlReport: tab "K?t lu?n" (b? tab "T?p d? li?u DICOM")
        private XtraTabControl _xtraTabControlReport;
        private XtraTabPage _xtraTabPage1;
        private Panel _panelReport;
        private Panel _panel5;
        private LabelControl _label2;
        private ComboBoxEdit _cbbMauGoiY;
        private RichTextBox _rtMoTa;
        private GroupControl _groupControl1;
        private RichTextBox _rtKetLuan;
        private GroupControl _groupControl2;
        private RichTextBox _rtKhuyenNghi;

        // Camera (don gi?n ho�: 1 panel, b? tab con "Video Media"/"?nh d� ch?p" v� t�nh nang
        // quay video d� t?t t? b?n g?c - xem DiagnosticReportConclusionControl.Camera.cs)
        private PanelControl _panelCamera;
        private Panel _cameraHeader;
        private Label _cameraTitle;
        private SimpleButton _btnCameraSettings;
        private Panel _cameraViewport;
        private Panel _cameraColumnSplitter;

        private PanelControl _panelControl1;
        private TableLayoutPanel _cameraButtonTable;
        private SimpleButton _btnSnapshot;
        private SimpleButton _btnStop;
        private SimpleButton _btnLinkCamera;

        private Panel _panelImageList;
        private Panel _panelImage;
        private Panel _panel1;
        private Label _lbImageTitle;
        private Label _lbImageSelect;
        private ImageThumbnailList _thumbnailList;

        // panelControl2: thanh n�t h�nh d?ng du?i c�ng (colspan 2) - b? n�t "T?i ?nh l�n" (PACS)
        // v� panel th�ng tin b�c si dang nh?p (d� b? theo y�u c?u). Responsive: FlowLayoutPanel
        // 2 v�ng (tr�i = combo layout/m�y in, ph?i = n�t - FlowDirection.RightToLeft) thay v�
        // to? d? Location/Anchor tuy?t d?i (t?ng g�y l?ch khi c?a s? kh�ng d�ng 1768px thi?t k?).
        private PanelControl _panelControl2;
        private FlowLayoutPanel _flowPanelControl2Left;
        private FlowLayoutPanel _flowPanelControl2Right;
        private ComboBoxEdit _cbbLayout;
        private ComboBoxEdit _cbbPrinters;
        private SimpleButton _btnPrinterSettings;
        private SimpleButton _btnSignature;
        private SimpleButton _btnCancel;
        private SimpleButton _btnPrint;
        private SimpleButton _btnSave;
        private SimpleButton _btnPushPacs;
        private SimpleButton _btnPreviewMain;
        private SimpleButton _btnSyncHis;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (disposing)
            {
                _richTextContextMenu?.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>N�t mui t�n x? xu?ng cho combo/lookup/date - DevExpress kh�ng t? th�m n?u kh�ng khai b�o.</summary>
        private static EditorButton ComboDropDownButton()
        {
            return new EditorButton(ButtonPredefines.Combo);
        }

        /// <summary>Kh?i g?n: label nh? ph�a tr�n + control ph�a du?i, d�ng cho panel th�ng tin b?nh nh�n/ch? d?nh.</summary>
        private Panel CreateLabeledField(string label, Control editor, int width, int height = 40)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(3, 2, 3, 2),
                MinimumSize = new Size(0, height)
            };
            var lbl = new Label
            {
                Text = label,
                Dock = DockStyle.Top,
                Height = 15,
                Font = new Font("Tahoma", 7.8F)
            };
            editor.Dock = DockStyle.Bottom;
            editor.Width = width;
            panel.Controls.Add(editor);
            panel.Controls.Add(lbl);
            return panel;
        }

        private Panel CreateInlineField(string label, Control editor, int labelWidth = 74)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(4, 2, 4, 2),
                Padding = new Padding(0, 2, 0, 2)
            };
            var lbl = new Label
            {
                Text = label,
                Dock = DockStyle.Left,
                Width = labelWidth,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                ForeColor = Color.FromArgb(70, 78, 88),
                Font = new Font("Tahoma", 8.25F, FontStyle.Regular)
            };
            editor.Dock = DockStyle.Fill;
            panel.Controls.Add(editor);
            panel.Controls.Add(lbl);
            return panel;
        }

        private void AddPatientField(
            string label, Control editor, int column, int row, int columnSpan = 1, int labelWidth = 68)
        {
            var field = CreateInlineField(label, editor, labelWidth);
            this._patientInfoTable.Controls.Add(field, column, row);
            if (columnSpan > 1)
                this._patientInfoTable.SetColumnSpan(field, columnSpan);
        }

        private static void StyleActionButton(SimpleButton button, int width)
        {
            button.Size = new Size(width, 26);
            button.Margin = new Padding(3, 1, 3, 1);
            button.Appearance.Font = new Font("Tahoma", 8F, FontStyle.Regular);
            button.Appearance.Options.UseFont = true;
            // Kh�ng �p Flat/BackColor/BorderColor: d? McSkin hi?n t?i c?a ?ng d?ng
            // t? render border, hover, pressed v� disabled nh?t qu�n.
            button.LookAndFeel.UseDefaultLookAndFeel = true;
        }

        private void InitializeComponent()
        {
            this._tbTableLayout = new TableLayoutPanel();
            this._bodyPanel = new Panel();
            this._contentTable = new TableLayoutPanel();
            this._panelControl4 = new PanelControl();
            this._patientActionBar = new Panel();
            this._patientSectionTitle = new Label();
            this._patientActionButtons = new FlowLayoutPanel();
            this._cbHoverPreview = new CheckBox();
            this._btnEditPatient = new SimpleButton();
            this._btnAddFile = new SimpleButton();
            this._patientInfoTable = new TableLayoutPanel();
            this._txMaBN = new TextEdit();
            this._txTenBN = new TextEdit();
            this._dateBN = new DateEdit();
            this._txPatientGender = new TextEdit();
            this._txQueQuan = new TextEdit();
            this._txDoiTuong = new TextEdit();
            this._txMaChiDinh = new TextEdit();
            this._dateNgayChiDinh = new DateEdit();
            this._txBSChiDinh = new TextEdit();
            this._txMaBHYT = new TextEdit();
            this._txDichVu = new TextEdit();
            this._txBSDoc = new TextEdit();
            this._cbbHisUser = new LookUpEdit();
            this._cbbDSThietBi = new LookUpEdit();
            this._dateTGThucHien = new DateEdit();
            this._dateTGKetThuc = new DateEdit();
            this._panelChanDoan = new Panel();
            this._labelControl24 = new LabelControl();
            this._txChanDoan = new TextEdit();
            this._xtraTabControlReport = new XtraTabControl();
            this._xtraTabPage1 = new XtraTabPage();
            this._panelReport = new Panel();
            this._panel5 = new Panel();
            this._label2 = new LabelControl();
            this._cbbMauGoiY = new ComboBoxEdit();
            this._rtMoTa = new RichTextBox();
            this._groupControl1 = new GroupControl();
            this._rtKetLuan = new RichTextBox();
            this._groupControl2 = new GroupControl();
            this._rtKhuyenNghi = new RichTextBox();
            this._panelCamera = new PanelControl();
            this._cameraHeader = new Panel();
            this._cameraTitle = new Label();
            this._btnCameraSettings = new SimpleButton();
            this._cameraViewport = new Panel();
            this._cameraColumnSplitter = new Panel();
            this._panelControl1 = new PanelControl();
            this._cameraButtonTable = new TableLayoutPanel();
            this._btnSnapshot = new SimpleButton();
            this._btnStop = new SimpleButton();
            this._btnLinkCamera = new SimpleButton();
            this._panelImageList = new Panel();
            this._panelImage = new Panel();
            this._panel1 = new Panel();
            this._lbImageTitle = new Label();
            this._lbImageSelect = new Label();
            this._thumbnailList = new ImageThumbnailList();
            this._panelControl2 = new PanelControl();
            this._flowPanelControl2Left = new FlowLayoutPanel();
            this._flowPanelControl2Right = new FlowLayoutPanel();
            this._cbbLayout = new ComboBoxEdit();
            this._cbbPrinters = new ComboBoxEdit();
            this._btnPrinterSettings = new SimpleButton();
            this._btnSignature = new SimpleButton();
            this._btnCancel = new SimpleButton();
            this._btnPrint = new SimpleButton();
            this._btnSave = new SimpleButton();
            this._btnPushPacs = new SimpleButton();
            this._btnPreviewMain = new SimpleButton();
            this._btnSyncHis = new SimpleButton();
            this._patientSidebar = new STM.MediaToPACS.Main.UI.PatientSidebar.PatientSidebarControl();
            this._patientSidebarSplitter = new Splitter();

            ((System.ComponentModel.ISupportInitialize)(this._panelControl4)).BeginInit();
            this._panelControl4.SuspendLayout();
            this._patientActionBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txMaBN.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txTenBN.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dateBN.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txPatientGender.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txQueQuan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txDoiTuong.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txMaChiDinh.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dateNgayChiDinh.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txBSChiDinh.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txMaBHYT.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txDichVu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txBSDoc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._cbbHisUser.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._cbbDSThietBi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dateTGThucHien.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dateTGKetThuc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txChanDoan.Properties)).BeginInit();
            this._panelChanDoan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._xtraTabControlReport)).BeginInit();
            this._xtraTabControlReport.SuspendLayout();
            this._xtraTabPage1.SuspendLayout();
            this._panelReport.SuspendLayout();
            this._panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._cbbMauGoiY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._groupControl1)).BeginInit();
            this._groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._groupControl2)).BeginInit();
            this._groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._panelCamera)).BeginInit();
            this._panelCamera.SuspendLayout();
            this._cameraHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._panelControl1)).BeginInit();
            this._panelControl1.SuspendLayout();
            this._cameraButtonTable.SuspendLayout();
            this._panelImageList.SuspendLayout();
            this._panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._panelControl2)).BeginInit();
            this._panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._cbbLayout.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._cbbPrinters.Properties)).BeginInit();
            this._tbTableLayout.SuspendLayout();
            this._bodyPanel.SuspendLayout();
            this._contentTable.SuspendLayout();
            this.SuspendLayout();
            //
            // _tbTableLayout: row0 = panelControl4 (colspan2) | row1..3 col0 = tab K?t lu?n (rowspan3),
            // col1 = camera(row1) + n�t camera(row2) + danh s�ch ?nh(row3) | row4 = thanh n�t (colspan2)
            //
            this._tbTableLayout.Dock = DockStyle.Fill;
            this._tbTableLayout.BackColor = Color.FromArgb(236, 239, 243);
            this._tbTableLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this._tbTableLayout.Margin = Padding.Empty;
            this._tbTableLayout.Padding = new Padding(3);
            this._tbTableLayout.ColumnCount = 2;
            this._tbTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this._tbTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this._tbTableLayout.RowCount = 5;
            // Toolbar + hai h�ng compact + ch?n do�n.
            this._tbTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 134F));
            this._tbTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this._tbTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            this._tbTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            this._tbTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            this._tbTableLayout.Controls.Add(this._panelControl4, 0, 0);
            this._tbTableLayout.SetColumnSpan(this._panelControl4, 2);
            this._tbTableLayout.Controls.Add(this._bodyPanel, 0, 1);
            this._tbTableLayout.SetColumnSpan(this._bodyPanel, 2);
            this._tbTableLayout.SetRowSpan(this._bodyPanel, 3);
            this._tbTableLayout.Controls.Add(this._panelControl2, 0, 4);
            this._tbTableLayout.SetColumnSpan(this._panelControl2, 2);
            //
            // _bodyPanel: sidebar ch? thu?c v�ng l�m vi?c, kh�ng che b�n tr�i
            // th�ng tin b?nh nh�n ph�a tr�n hay thanh h�nh d?ng ph�a du?i.
            //
            this._bodyPanel.Dock = DockStyle.Fill;
            this._bodyPanel.Margin = Padding.Empty;
            this._bodyPanel.Controls.Add(this._contentTable);
            this._bodyPanel.Controls.Add(this._patientSidebarSplitter);
            this._bodyPanel.Controls.Add(this._patientSidebar);
            //
            // _contentTable: M� t?/K?t lu?n b�n tr�i, camera/?nh b�n ph?i.
            //
            this._contentTable.Dock = DockStyle.Fill;
            this._contentTable.Margin = Padding.Empty;
            this._contentTable.ColumnCount = 3;
            this._contentTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this._contentTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 7F));
            this._contentTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 520F));
            this._contentTable.RowCount = 3;
            this._contentTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this._contentTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            this._contentTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            this._contentTable.Controls.Add(this._panelReport, 0, 0);
            this._contentTable.SetRowSpan(this._panelReport, 3);
            this._contentTable.Controls.Add(this._cameraColumnSplitter, 1, 0);
            this._contentTable.SetRowSpan(this._cameraColumnSplitter, 3);
            this._contentTable.Controls.Add(this._panelCamera, 2, 0);
            this._contentTable.Controls.Add(this._panelControl1, 2, 1);
            this._contentTable.Controls.Add(this._panelImageList, 2, 2);
            this._cameraColumnSplitter.Dock = DockStyle.Fill;
            this._cameraColumnSplitter.Margin = Padding.Empty;
            this._cameraColumnSplitter.Cursor = Cursors.SizeWE;
            this._cameraColumnSplitter.BackColor = Color.FromArgb(218, 224, 232);
            this._cameraColumnSplitter.MouseDown += new MouseEventHandler(this.CameraColumnSplitter_MouseDown);
            this._cameraColumnSplitter.MouseMove += new MouseEventHandler(this.CameraColumnSplitter_MouseMove);
            this._cameraColumnSplitter.MouseUp += new MouseEventHandler(this.CameraColumnSplitter_MouseUp);
            //
            // _panelControl4: th�ng tin b?nh nh�n/ch? d?nh - g?n b?ng FlowLayoutPanel t? xu?ng d�ng
            //
            this._panelControl4.Dock = DockStyle.Fill;
            this._panelControl4.Margin = new Padding(2);
            this._panelControl4.Padding = new Padding(6, 5, 6, 5);
            this._panelControl4.Controls.Add(this._patientInfoTable);
            // Th�m panel Dock.Bottom sau c�ng d? WinForms d�nh ch? cho n� tru?c
            // khi b? tr� v�ng th�ng tin Dock.Fill.
            this._panelControl4.Controls.Add(this._panelChanDoan);
            this._panelControl4.Controls.Add(this._patientActionBar);
            //
            // _patientActionBar: ti�u d? h? so v� c�c thao t�c li�n quan b?nh nh�n.
            //
            this._patientActionBar.Dock = DockStyle.Top;
            this._patientActionBar.Height = 30;
            this._patientActionBar.BackColor = Color.Transparent;
            this._patientActionBar.Padding = new Padding(7, 1, 3, 1);
            this._patientActionBar.Controls.Add(this._patientSectionTitle);
            this._patientActionBar.Controls.Add(this._patientActionButtons);
            this._patientSectionTitle.Dock = DockStyle.Fill;
            this._patientSectionTitle.Text = "TH�NG TIN B?NH NH�N & CH? �?NH";
            this._patientSectionTitle.AutoEllipsis = true;
            this._patientSectionTitle.TextAlign = ContentAlignment.MiddleLeft;
            this._patientSectionTitle.Font = new Font("Tahoma", 8F, FontStyle.Bold);
            this._patientSectionTitle.ForeColor = Color.FromArgb(55, 65, 81);
            this._patientActionButtons.Dock = DockStyle.Right;
            // AutoSize thay v� Width c? d?nh: s? n�t viewer PACS thay d?i tu? b�c si dang nh?p
            // (RenderViewerButtons ? DiagnosticReportConclusionControl.Viewer.cs) n�n b? r?ng
            // v�ng n�t ph?i t? co gi�n theo d�ng s? n�t dang c�, kh�ng b? c?t/th?a ch?.
            this._patientActionButtons.AutoSize = true;
            this._patientActionButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this._patientActionButtons.MaximumSize = new Size(900, 0);
            this._patientActionButtons.FlowDirection = FlowDirection.RightToLeft;
            this._patientActionButtons.WrapContents = false;
            StyleActionButton(this._btnAddFile, 88);
            this._btnAddFile.Text = "Th�m file";
            StyleActionButton(this._btnEditPatient, 122);
            this._btnEditPatient.Text = "S?a th�ng tin";
            this._cbHoverPreview.AutoSize = false;
            this._cbHoverPreview.Size = new Size(128, 26);
            this._cbHoverPreview.Margin = new Padding(3, 1, 3, 1);
            this._cbHoverPreview.Text = "Xem tru?c khi hover";
            this._cbHoverPreview.TextAlign = ContentAlignment.MiddleLeft;
            this._cbHoverPreview.Font = new Font("Tahoma", 8F, FontStyle.Regular);
            this._cbHoverPreview.ForeColor = Color.FromArgb(70, 78, 88);
            this._cbHoverPreview.Checked = true;
            this._patientActionButtons.Controls.Add(this._btnAddFile);
            this._patientActionButtons.Controls.Add(this._btnEditPatient);
            this._patientActionButtons.Controls.Add(this._cbHoverPreview);
            //
            // _patientInfoTable: lu?i 10 c?t x 2 h�ng. T? tr?ng c?t du?c t?i uu
            // theo d? d�i d? li?u th?c t? d? kh�ng l�ng ph� m?t h�ng ri�ng cho
            // thi?t b?/th?i gian th?c hi?n.
            //
            this._patientInfoTable.Dock = DockStyle.Fill;
            this._patientInfoTable.ColumnCount = 10;
            this._patientInfoTable.RowCount = 2;
            this._patientInfoTable.Margin = Padding.Empty;
            this._patientInfoTable.Padding = Padding.Empty;
            this._patientInfoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 7F));
            this._patientInfoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            this._patientInfoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            this._patientInfoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            this._patientInfoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            this._patientInfoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            this._patientInfoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            this._patientInfoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            this._patientInfoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.5F));
            this._patientInfoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.5F));
            this._patientInfoTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            this._patientInfoTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            this._txMaBN.Properties.ReadOnly = true;
            this._txTenBN.Properties.ReadOnly = true;
            this._txPatientGender.Properties.ReadOnly = true;
            this._txQueQuan.Properties.ReadOnly = true;
            this._txDoiTuong.Properties.ReadOnly = true;
            this._txMaChiDinh.Properties.ReadOnly = true;
            this._txBSChiDinh.Properties.ReadOnly = true;
            this._txMaBHYT.Properties.ReadOnly = true;
            this._txDichVu.Properties.ReadOnly = true;
            this._txBSDoc.Properties.ReadOnly = true;
            this._dateBN.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this._dateBN.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this._dateBN.Properties.Buttons.AddRange(new EditorButton[] { ComboDropDownButton() });
            this._dateNgayChiDinh.Properties.DisplayFormat.FormatString = "HH:mm dd/MM/yyyy";
            this._dateNgayChiDinh.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this._dateNgayChiDinh.Properties.Buttons.AddRange(new EditorButton[] { ComboDropDownButton() });
            this._dateTGThucHien.Properties.DisplayFormat.FormatString = "HH:mm dd/MM/yyyy";
            this._dateTGThucHien.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this._dateTGThucHien.Properties.Buttons.AddRange(new EditorButton[] { ComboDropDownButton() });
            this._dateTGKetThuc.Properties.DisplayFormat.FormatString = "HH:mm dd/MM/yyyy";
            this._dateTGKetThuc.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this._dateTGKetThuc.Properties.Buttons.AddRange(new EditorButton[] { ComboDropDownButton() });
            this._cbbHisUser.Properties.NullText = "Ch?n KTV...";
            this._cbbHisUser.Properties.Buttons.AddRange(new EditorButton[] { ComboDropDownButton() });
            this._cbbDSThietBi.Properties.NullText = "Ch?n thi?t b?...";
            this._cbbDSThietBi.Properties.Buttons.AddRange(new EditorButton[] { ComboDropDownButton() });
            // H�ng 1: nh?n di?n b?nh nh�n v� th�ng tin b?o hi?m.
            this.AddPatientField("M� BN:", this._txMaBN, 0, 0, 1, 46);
            this.AddPatientField("T�n BN:", this._txTenBN, 1, 0, 2, 54);
            this.AddPatientField("Ng�y sinh:", this._dateBN, 3, 0, 1, 62);
            this.AddPatientField("Gi?i t�nh:", this._txPatientGender, 4, 0, 1, 60);
            this.AddPatientField("Qu� qu�n:", this._txQueQuan, 5, 0, 2, 62);
            this.AddPatientField("�?i tu?ng:", this._txDoiTuong, 7, 0, 1, 64);
            this.AddPatientField("M� BHYT:", this._txMaBHYT, 8, 0, 2, 62);

            // H�ng 2: th�ng tin ch? d?nh v� qu� tr�nh th?c hi?n theo th? t? nghi?p v?.
            this.AddPatientField("M� C�:", this._txMaChiDinh, 0, 1, 1, 46);
            this.AddPatientField("TG C�:", this._dateNgayChiDinh, 1, 1, 1, 48);
            this.AddPatientField("BS C�:", this._txBSChiDinh, 2, 1, 1, 48);
            this.AddPatientField("D?ch v?:", this._txDichVu, 3, 1, 2, 54);
            this.AddPatientField("BS d?c:", this._txBSDoc, 5, 1, 1, 52);
            this.AddPatientField("KTV:", this._cbbHisUser, 6, 1, 1, 36);
            this.AddPatientField("Thi?t b?:", this._cbbDSThietBi, 7, 1, 1, 54);
            this.AddPatientField("B?t d?u:", this._dateTGThucHien, 8, 1, 1, 52);
            this.AddPatientField("K?t th�c:", this._dateTGKetThuc, 9, 1, 1, 56);
            //
            // _panelChanDoan: Ch?n do�n (1 h�ng, d?y chi?u r?ng, n?m du?i _flowPatientInfo)
            //
            this._panelChanDoan.Dock = DockStyle.Bottom;
            this._panelChanDoan.Height = 30;
            this._panelChanDoan.Padding = new Padding(4, 3, 4, 2);
            this._panelChanDoan.Controls.Add(this._txChanDoan);
            this._panelChanDoan.Controls.Add(this._labelControl24);
            this._labelControl24.Dock = DockStyle.Left;
            this._labelControl24.Text = "Ch?n do�n:";
            this._labelControl24.Size = new Size(76, 21);
            this._labelControl24.Appearance.ForeColor = Color.FromArgb(70, 78, 88);
            this._labelControl24.Appearance.Options.UseForeColor = true;
            this._txChanDoan.Dock = DockStyle.Fill;
            this._txChanDoan.Properties.ReadOnly = true;
            //
            // _panelReport: tab ch? bao ph?n M� t?; K?t lu?n v� Khuy?n ngh? l�
            // hai kh?i c�ng c?p b�n du?i, kh�ng n?m trong chi?u cao c?a tab.
            //
            this._panelReport.Dock = DockStyle.Fill;
            this._panelReport.Margin = new Padding(2);
            this._panelReport.Controls.Add(this._xtraTabControlReport);
            this._panelReport.Controls.Add(this._groupControl1);
            this._panelReport.Controls.Add(this._groupControl2);
            this._xtraTabControlReport.Dock = DockStyle.Fill;
            this._xtraTabControlReport.Margin = Padding.Empty;
            this._xtraTabControlReport.SelectedTabPage = this._xtraTabPage1;
            this._xtraTabControlReport.TabPages.AddRange(new XtraTabPage[] { this._xtraTabPage1 });
            this._xtraTabPage1.Text = "K?t lu?n";
            this._xtraTabPage1.Padding = new Padding(3);
            this._xtraTabPage1.Controls.Add(this._rtMoTa);
            this._xtraTabPage1.Controls.Add(this._panel5);
            //
            // _panel5: combo g?i � k?t lu?n
            //
            this._panel5.Dock = DockStyle.Top;
            this._panel5.Height = 38;
            this._panel5.Padding = new Padding(3, 4, 3, 4);
            this._panel5.Controls.Add(this._cbbMauGoiY);
            this._panel5.Controls.Add(this._label2);
            this._label2.Dock = DockStyle.Left;
            this._label2.Text = "M� t?:";
            this._label2.Size = new Size(67, 25);
            this._label2.Appearance.Font = new Font("Microsoft Sans Serif", 12F);
            this._label2.Appearance.Options.UseFont = true;
            this._cbbMauGoiY.Dock = DockStyle.Fill;
            this._cbbMauGoiY.Properties.NullText = "Ch?n m?u g?i � k?t lu?n...";
            this._cbbMauGoiY.Properties.Buttons.AddRange(new EditorButton[] { ComboDropDownButton() });
            this._cbbMauGoiY.SelectedIndexChanged += new System.EventHandler(this._cbbReportTemplate_SelectedIndexChanged);
            //
            // _rtMoTa
            //
            this._rtMoTa.BackColor = Color.White;
            this._rtMoTa.Dock = DockStyle.Fill;
            this._rtMoTa.Font = new Font("Microsoft Sans Serif", 12F);
            //
            // _groupControl1 "K?t lu?n"
            //
            this._groupControl1.Dock = DockStyle.Bottom;
            this._groupControl1.Height = 166;
            this._groupControl1.Text = "K?t lu?n";
            this._groupControl1.Controls.Add(this._rtKetLuan);
            this._rtKetLuan.Dock = DockStyle.Fill;
            this._rtKetLuan.Font = new Font("Microsoft Sans Serif", 12F);
            //
            // _groupControl2 "Khuy?n ngh?"
            //
            this._groupControl2.Dock = DockStyle.Bottom;
            this._groupControl2.Height = 115;
            this._groupControl2.Text = "Khuy?n ngh?";
            this._groupControl2.Controls.Add(this._rtKhuyenNghi);
            this._rtKhuyenNghi.Dock = DockStyle.Fill;
            this._rtKhuyenNghi.Font = new Font("Microsoft Sans Serif", 12F);
            //
            // _panelCamera: header ri�ng + viewport camera, t?o ph�n c?p r� r�ng.
            //
            this._panelCamera.Appearance.BackColor = Color.White;
            this._panelCamera.Appearance.Options.UseBackColor = true;
            this._panelCamera.Dock = DockStyle.Fill;
            this._panelCamera.Margin = new Padding(2);
            this._panelCamera.Padding = new Padding(1);
            this._panelCamera.Controls.Add(this._cameraViewport);
            this._panelCamera.Controls.Add(this._cameraHeader);
            this._cameraHeader.Dock = DockStyle.Top;
            this._cameraHeader.Height = 32;
            this._cameraHeader.BackColor = Color.FromArgb(245, 247, 250);
            this._cameraHeader.Padding = new Padding(10, 0, 10, 0);
            this._cameraHeader.Controls.Add(this._cameraTitle);
            this._cameraTitle.Dock = DockStyle.Fill;
            this._cameraTitle.Text = "CAMERA TR?C TI?P";
            this._cameraTitle.TextAlign = ContentAlignment.MiddleLeft;
            this._cameraTitle.Font = new Font("Tahoma", 8.5F, FontStyle.Bold);
            this._cameraTitle.ForeColor = Color.FromArgb(55, 65, 81);
            StyleActionButton(this._btnCameraSettings, 28);
            this._btnCameraSettings.Dock = DockStyle.Right;
            this._btnCameraSettings.Margin = Padding.Empty;
            this._btnCameraSettings.Text = "?";
            this._btnCameraSettings.ToolTip = "C�i d?t camera";
            this._btnCameraSettings.Appearance.Font = new Font("Segoe UI Symbol", 10F);
            this._btnCameraSettings.Appearance.Options.UseFont = true;
            this._cameraViewport.Dock = DockStyle.Fill;
            this._cameraViewport.BackColor = Color.Black;
            this._cameraViewport.Padding = new Padding(2);
            //
            // _panelControl1: ba n�t c�n theo c?t, kh�ng l?ch tr�i khi d?i d? r?ng.
            //
            this._panelControl1.Dock = DockStyle.Fill;
            this._panelControl1.Margin = new Padding(2);
            this._panelControl1.Padding = new Padding(10, 3, 10, 3);
            this._panelControl1.Controls.Add(this._cameraButtonTable);
            this._cameraButtonTable.Dock = DockStyle.Fill;
            this._cameraButtonTable.ColumnCount = 3;
            this._cameraButtonTable.RowCount = 1;
            this._cameraButtonTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            this._cameraButtonTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            this._cameraButtonTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            this._cameraButtonTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            StyleActionButton(this._btnSnapshot, 132);
            this._btnSnapshot.Anchor = AnchorStyles.None;
            this._btnSnapshot.Appearance.BackColor = Color.FromArgb(0, 120, 212);
            this._btnSnapshot.Appearance.ForeColor = Color.White;
            this._btnSnapshot.Text = "Ch?p nhanh"; this._btnSnapshot.Click += new System.EventHandler(this._btnSnapshot_Click);
            StyleActionButton(this._btnStop, 104);
            this._btnStop.Anchor = AnchorStyles.None;
            this._btnStop.Text = "D?ng"; this._btnStop.Click += new System.EventHandler(this._btnStop_Click);
            StyleActionButton(this._btnLinkCamera, 120);
            this._btnLinkCamera.Anchor = AnchorStyles.None;
            this._btnLinkCamera.Text = "Li�n k?t"; this._btnLinkCamera.Click += new System.EventHandler(this._btnLinkCamera_Click);
            this._cameraButtonTable.Controls.Add(this._btnSnapshot, 0, 0);
            this._cameraButtonTable.Controls.Add(this._btnStop, 1, 0);
            this._cameraButtonTable.Controls.Add(this._btnLinkCamera, 2, 0);
            //
            // _panelImageList: danh s�ch ?nh thumbnail + b? d?m d� ch?n
            //
            this._panelImageList.Dock = DockStyle.Fill;
            this._panelImageList.Margin = new Padding(2);
            this._panelImageList.Padding = new Padding(3);
            this._panelImageList.Controls.Add(this._panelImage);
            this._panelImageList.Controls.Add(this._panel1);
            this._panel1.Dock = DockStyle.Top;
            this._panel1.Height = 32;
            this._panel1.BackColor = Color.FromArgb(245, 247, 250);
            this._panel1.Padding = new Padding(10, 0, 10, 0);
            this._panel1.Controls.Add(this._lbImageTitle);
            this._panel1.Controls.Add(this._lbImageSelect);
            this._lbImageTitle.Dock = DockStyle.Fill;
            this._lbImageTitle.Text = "?NH �� CH?P";
            this._lbImageTitle.TextAlign = ContentAlignment.MiddleLeft;
            this._lbImageTitle.Font = new Font("Tahoma", 8.5F, FontStyle.Bold);
            this._lbImageTitle.ForeColor = Color.FromArgb(55, 65, 81);
            this._lbImageSelect.Dock = DockStyle.Right;
            this._lbImageSelect.Width = 72;
            this._lbImageSelect.TextAlign = ContentAlignment.MiddleRight;
            this._lbImageSelect.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            this._lbImageSelect.ForeColor = Color.FromArgb(0, 120, 212);
            this._lbImageSelect.Text = "0/0";
            this._panelImage.Dock = DockStyle.Fill;
            this._panelImage.BackColor = Color.White;
            this._panelImage.Padding = new Padding(2);
            this._panelImage.Controls.Add(this._thumbnailList);
            this._thumbnailList.Dock = DockStyle.Fill;
            //
            // _panelControl2: thanh n�t h�nh d?ng du?i c�ng - responsive b?ng 2 FlowLayoutPanel
            // (tr�i = Dock.Left ch?a combo layout/m�y in; ph?i = Dock.Fill, FlowDirection
            // RightToLeft ch?a c�c n�t) thay v� Location/Anchor tuy?t d?i.
            //
            this._panelControl2.Appearance.BackColor = Color.White;
            this._panelControl2.Appearance.Options.UseBackColor = true;
            this._panelControl2.Dock = DockStyle.Fill;
            this._panelControl2.Margin = new Padding(2);
            this._panelControl2.Padding = new Padding(6, 4, 6, 5);
            this._panelControl2.Controls.Add(this._flowPanelControl2Right);
            this._panelControl2.Controls.Add(this._flowPanelControl2Left);
            //
            // _flowPanelControl2Left: combo layout in / m�y in
            //
            this._flowPanelControl2Left.Dock = DockStyle.Left;
            this._flowPanelControl2Left.Width = 460;
            this._flowPanelControl2Left.WrapContents = false;
            this._flowPanelControl2Left.Padding = new Padding(0, 3, 0, 2);
            this._cbbLayout.Properties.Buttons.AddRange(new EditorButton[] { ComboDropDownButton() });
            this._cbbPrinters.Properties.Buttons.AddRange(new EditorButton[] { ComboDropDownButton() });
            var layoutField = CreateInlineField("M?u in:", this._cbbLayout, 58);
            layoutField.Size = new Size(220, 34);
            layoutField.Dock = DockStyle.None;
            var printerField = CreateInlineField("M�y in:", this._cbbPrinters, 58);
            printerField.Size = new Size(230, 34);
            printerField.Dock = DockStyle.None;
            StyleActionButton(this._btnPrinterSettings, 28);
            this._btnPrinterSettings.Dock = DockStyle.Right;
            this._btnPrinterSettings.Margin = new Padding(2, 0, 0, 0);
            this._btnPrinterSettings.Text = "?";
            this._btnPrinterSettings.ToolTip = "C�i d?t m�y in m?c d?nh";
            this._btnPrinterSettings.Appearance.Font = new Font("Segoe UI Symbol", 10F);
            this._btnPrinterSettings.Appearance.Options.UseFont = true;
            this._flowPanelControl2Left.Controls.Add(layoutField);
            this._flowPanelControl2Left.Controls.Add(printerField);
            //
            // _flowPanelControl2Right: c�c n�t h�nh d?ng, FlowDirection.RightToLeft n�n th�m
            // theo th? t? t? ph?i sang tr�i (Tho�t th�m tru?c c�ng ra ngo�i c�ng b�n ph?i).
            //
            this._flowPanelControl2Right.Dock = DockStyle.Fill;
            this._flowPanelControl2Right.FlowDirection = FlowDirection.RightToLeft;
            this._flowPanelControl2Right.WrapContents = false;
            this._flowPanelControl2Right.Padding = new Padding(0, 3, 0, 2);
            StyleActionButton(this._btnCancel, 104);
            this._btnCancel.Text = "Tho�t"; this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
            StyleActionButton(this._btnSave, 120);
            this._btnSave.Appearance.BackColor = Color.FromArgb(0, 120, 212);
            this._btnSave.Appearance.ForeColor = Color.White;
            this._btnSave.Text = "Luu Nh�p"; this._btnSave.Click += new System.EventHandler(this._btnSave_Click);
            StyleActionButton(this._btnPushPacs, 120);
            this._btnPushPacs.Appearance.BackColor = Color.FromArgb(22, 128, 61);
            this._btnPushPacs.Appearance.ForeColor = Color.White;
            this._btnPushPacs.Text = "�?y PACS"; this._btnPushPacs.Click += new System.EventHandler(this._btnPushPacs_Click);
            StyleActionButton(this._btnPrint, 82);
            this._btnPrint.Text = "In"; this._btnPrint.Click += new System.EventHandler(this._btnPrint_Click);
            StyleActionButton(this._btnSignature, 116);
            this._btnSignature.Appearance.BackColor = Color.FromArgb(24, 133, 92);
            this._btnSignature.Appearance.ForeColor = Color.White;
            this._btnSignature.Text = "K� s?"; this._btnSignature.Click += new System.EventHandler(this._btnSignature_Click);
            StyleActionButton(this._btnPreviewMain, 120);
            this._btnPreviewMain.Text = "Xem tru?c"; this._btnPreviewMain.Click += new System.EventHandler(this._btnPreviewMain_Click);
            StyleActionButton(this._btnSyncHis, 112);
            this._btnSyncHis.Text = "G?i l?i HIS"; this._btnSyncHis.Click += new System.EventHandler(this._btnSyncHis_Click);
            this._flowPanelControl2Right.Controls.Add(this._btnCancel);
            this._flowPanelControl2Right.Controls.Add(this._btnSave);
            this._flowPanelControl2Right.Controls.Add(this._btnPushPacs);
            this._flowPanelControl2Right.Controls.Add(this._btnPrint);
            this._flowPanelControl2Right.Controls.Add(this._btnSignature);
            this._flowPanelControl2Right.Controls.Add(this._btnPreviewMain);
            this._flowPanelControl2Right.Controls.Add(this._btnSyncHis);
            //
            // _patientSidebar / _patientSidebarSplitter: d?i sidebar tr�i c?a ri�ng _bodyPanel.
            //
            this._patientSidebarSplitter.Dock = DockStyle.Left;
            this._patientSidebarSplitter.MinExtra = 320;
            this._patientSidebarSplitter.MinSize = 260;
            this._patientSidebarSplitter.TabStop = false;
            this._patientSidebarSplitter.Visible = false;
            this._patientSidebarSplitter.SplitterMoved += new SplitterEventHandler(this.PatientSidebarSplitter_SplitterMoved);
            this._patientSidebar.Dock = DockStyle.Left;
            this._patientSidebar.CollapsedChanged += (s, e) => this._patientSidebarSplitter.Visible = !this._patientSidebar.Collapsed;
            this._patientSidebar.PinnedChanged += this.PatientSidebar_PinnedChanged;
            //
            // DiagnosticReportConclusionControl
            //
            this.Controls.Add(this._tbTableLayout);
            this.Name = "DiagnosticReportConclusionControl";
            this.Size = new Size(1768, 920);

            ((System.ComponentModel.ISupportInitialize)(this._txMaBN.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txTenBN.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dateBN.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txPatientGender.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txQueQuan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txDoiTuong.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txMaChiDinh.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dateNgayChiDinh.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txBSChiDinh.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txMaBHYT.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txDichVu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txBSDoc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._cbbHisUser.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._cbbDSThietBi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dateTGThucHien.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dateTGKetThuc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txChanDoan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._cbbMauGoiY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._cbbLayout.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._cbbPrinters.Properties)).EndInit();
            this._panelChanDoan.ResumeLayout(false);
            this._patientActionBar.ResumeLayout(false);
            this._panelControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._panelControl4)).EndInit();
            this._panel5.ResumeLayout(false);
            this._groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._groupControl1)).EndInit();
            this._groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._groupControl2)).EndInit();
            this._panelReport.ResumeLayout(false);
            this._xtraTabPage1.ResumeLayout(false);
            this._xtraTabControlReport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._xtraTabControlReport)).EndInit();
            this._cameraHeader.ResumeLayout(false);
            this._panelCamera.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._panelCamera)).EndInit();
            this._cameraButtonTable.ResumeLayout(false);
            this._panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._panelControl1)).EndInit();
            this._panel1.ResumeLayout(false);
            this._panel1.PerformLayout();
            this._panelImageList.ResumeLayout(false);
            this._panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._panelControl2)).EndInit();
            this._contentTable.ResumeLayout(false);
            this._bodyPanel.ResumeLayout(false);
            this._tbTableLayout.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}


