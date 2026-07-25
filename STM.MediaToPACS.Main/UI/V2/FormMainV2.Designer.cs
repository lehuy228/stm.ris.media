using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTab;

namespace STM.MediaToPACS.Main.UI.V2
{
    partial class FormMainV2
    {
        private System.ComponentModel.IContainer components = null;

        // Layout gốc - bê nguyên bố cục của FrmMain.Designer.cs (panelControl4 trên cùng,
        // tab Kết luận bên trái, camera+nút+ảnh bên phải, thanh nút dưới cùng), bỏ MenuStrip,
        // ToolStrip DICOM, tab "Tập dữ liệu DICOM" và nút "Tải ảnh lên PACS".
        private TableLayoutPanel _tbTableLayout;
        private Panel _bodyPanel;
        private TableLayoutPanel _contentTable;

        // Sidebar trái: lịch sử khám bệnh nhân + tham số siêu âm (bước 3)
        private STM.MediaToPACS.Main.UI.PatientSidebar.PatientSidebarControl _patientSidebar;
        private Splitter _patientSidebarSplitter;

        // panelControl4: thông tin bệnh nhân/chỉ định (hàng trên cùng, colspan 2) - bố cục gọn
        // bằng FlowLayoutPanel tự xuống dòng (CreateLabeledField) thay vì toạ độ tuyệt đối,
        // để không bị tràn/khuất khi chiều rộng thực tế nhỏ hơn thiết kế gốc.
        private PanelControl _panelControl4;
        private Panel _patientActionBar;
        private Label _patientSectionTitle;
        private FlowLayoutPanel _patientActionButtons;
        private SimpleButton _btnViewPacs;
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

        // xtraTabControlReport: tab "Kết luận" (bỏ tab "Tập dữ liệu DICOM")
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

        // Camera (đơn giản hoá: 1 panel, bỏ tab con "Video Media"/"Ảnh đã chụp" vì tính năng
        // quay video đã tắt từ bản gốc - xem FormMainV2.Camera.cs)
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

        // panelControl2: thanh nút hành động dưới cùng (colspan 2) - bỏ nút "Tải ảnh lên" (PACS)
        // và panel thông tin bác sĩ đăng nhập (đã bỏ theo yêu cầu). Responsive: FlowLayoutPanel
        // 2 vùng (trái = combo layout/máy in, phải = nút - FlowDirection.RightToLeft) thay vì
        // toạ độ Location/Anchor tuyệt đối (từng gây lệch khi cửa sổ không đúng 1768px thiết kế).
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
        private SimpleButton _btnPreviewMain;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>Nút mũi tên xổ xuống cho combo/lookup/date - DevExpress không tự thêm nếu không khai báo.</summary>
        private static EditorButton ComboDropDownButton()
        {
            return new EditorButton(ButtonPredefines.Combo);
        }

        /// <summary>Khối gọn: label nhỏ phía trên + control phía dưới, dùng cho panel thông tin bệnh nhân/chỉ định.</summary>
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
            // Không ép Flat/BackColor/BorderColor: để McSkin hiện tại của ứng dụng
            // tự render border, hover, pressed và disabled nhất quán.
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
            this._btnViewPacs = new SimpleButton();
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
            this._btnPreviewMain = new SimpleButton();
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
            // _tbTableLayout: row0 = panelControl4 (colspan2) | row1..3 col0 = tab Kết luận (rowspan3),
            // col1 = camera(row1) + nút camera(row2) + danh sách ảnh(row3) | row4 = thanh nút (colspan2)
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
            // Toolbar + hai hàng compact + chẩn đoán.
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
            // _bodyPanel: sidebar chỉ thuộc vùng làm việc, không che bên trái
            // thông tin bệnh nhân phía trên hay thanh hành động phía dưới.
            //
            this._bodyPanel.Dock = DockStyle.Fill;
            this._bodyPanel.Margin = Padding.Empty;
            this._bodyPanel.Controls.Add(this._contentTable);
            this._bodyPanel.Controls.Add(this._patientSidebarSplitter);
            this._bodyPanel.Controls.Add(this._patientSidebar);
            //
            // _contentTable: Mô tả/Kết luận bên trái, camera/ảnh bên phải.
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
            // _panelControl4: thông tin bệnh nhân/chỉ định - gọn bằng FlowLayoutPanel tự xuống dòng
            //
            this._panelControl4.Dock = DockStyle.Fill;
            this._panelControl4.Margin = new Padding(2);
            this._panelControl4.Padding = new Padding(6, 5, 6, 5);
            this._panelControl4.Controls.Add(this._patientInfoTable);
            // Thêm panel Dock.Bottom sau cùng để WinForms dành chỗ cho nó trước
            // khi bố trí vùng thông tin Dock.Fill.
            this._panelControl4.Controls.Add(this._panelChanDoan);
            this._panelControl4.Controls.Add(this._patientActionBar);
            //
            // _patientActionBar: tiêu đề hồ sơ và các thao tác liên quan bệnh nhân.
            //
            this._patientActionBar.Dock = DockStyle.Top;
            this._patientActionBar.Height = 30;
            this._patientActionBar.BackColor = Color.Transparent;
            this._patientActionBar.Padding = new Padding(7, 1, 3, 1);
            this._patientActionBar.Controls.Add(this._patientSectionTitle);
            this._patientActionBar.Controls.Add(this._patientActionButtons);
            this._patientSectionTitle.Dock = DockStyle.Fill;
            this._patientSectionTitle.Text = "THÔNG TIN BỆNH NHÂN & CHỈ ĐỊNH";
            this._patientSectionTitle.TextAlign = ContentAlignment.MiddleLeft;
            this._patientSectionTitle.Font = new Font("Tahoma", 8F, FontStyle.Bold);
            this._patientSectionTitle.ForeColor = Color.FromArgb(55, 65, 81);
            this._patientActionButtons.Dock = DockStyle.Right;
            this._patientActionButtons.Width = 370;
            this._patientActionButtons.FlowDirection = FlowDirection.RightToLeft;
            this._patientActionButtons.WrapContents = false;
            StyleActionButton(this._btnAddFile, 88);
            this._btnAddFile.Text = "Thêm file";
            StyleActionButton(this._btnEditPatient, 122);
            this._btnEditPatient.Text = "Sửa thông tin";
            StyleActionButton(this._btnViewPacs, 112);
            this._btnViewPacs.Text = "Xem ảnh PACS";
            this._patientActionButtons.Controls.Add(this._btnAddFile);
            this._patientActionButtons.Controls.Add(this._btnEditPatient);
            this._patientActionButtons.Controls.Add(this._btnViewPacs);
            //
            // _patientInfoTable: lưới 10 cột x 2 hàng. Tỷ trọng cột được tối ưu
            // theo độ dài dữ liệu thực tế để không lãng phí một hàng riêng cho
            // thiết bị/thời gian thực hiện.
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
            this._cbbHisUser.Properties.NullText = "Chọn KTV...";
            this._cbbHisUser.Properties.Buttons.AddRange(new EditorButton[] { ComboDropDownButton() });
            this._cbbDSThietBi.Properties.NullText = "Chọn thiết bị...";
            this._cbbDSThietBi.Properties.Buttons.AddRange(new EditorButton[] { ComboDropDownButton() });
            // Hàng 1: nhận diện bệnh nhân và thông tin bảo hiểm.
            this.AddPatientField("Mã BN:", this._txMaBN, 0, 0, 1, 46);
            this.AddPatientField("Tên BN:", this._txTenBN, 1, 0, 2, 54);
            this.AddPatientField("Ngày sinh:", this._dateBN, 3, 0, 1, 62);
            this.AddPatientField("Giới tính:", this._txPatientGender, 4, 0, 1, 60);
            this.AddPatientField("Quê quán:", this._txQueQuan, 5, 0, 2, 62);
            this.AddPatientField("Đối tượng:", this._txDoiTuong, 7, 0, 1, 64);
            this.AddPatientField("Mã BHYT:", this._txMaBHYT, 8, 0, 2, 62);

            // Hàng 2: thông tin chỉ định và quá trình thực hiện theo thứ tự nghiệp vụ.
            this.AddPatientField("Mã CĐ:", this._txMaChiDinh, 0, 1, 1, 46);
            this.AddPatientField("TG CĐ:", this._dateNgayChiDinh, 1, 1, 1, 48);
            this.AddPatientField("BS CĐ:", this._txBSChiDinh, 2, 1, 1, 48);
            this.AddPatientField("Dịch vụ:", this._txDichVu, 3, 1, 2, 54);
            this.AddPatientField("BS đọc:", this._txBSDoc, 5, 1, 1, 52);
            this.AddPatientField("KTV:", this._cbbHisUser, 6, 1, 1, 36);
            this.AddPatientField("Thiết bị:", this._cbbDSThietBi, 7, 1, 1, 54);
            this.AddPatientField("Bắt đầu:", this._dateTGThucHien, 8, 1, 1, 52);
            this.AddPatientField("Kết thúc:", this._dateTGKetThuc, 9, 1, 1, 56);
            //
            // _panelChanDoan: Chẩn đoán (1 hàng, đầy chiều rộng, nằm dưới _flowPatientInfo)
            //
            this._panelChanDoan.Dock = DockStyle.Bottom;
            this._panelChanDoan.Height = 30;
            this._panelChanDoan.Padding = new Padding(4, 3, 4, 2);
            this._panelChanDoan.Controls.Add(this._txChanDoan);
            this._panelChanDoan.Controls.Add(this._labelControl24);
            this._labelControl24.Dock = DockStyle.Left;
            this._labelControl24.Text = "Chẩn đoán:";
            this._labelControl24.Size = new Size(76, 21);
            this._labelControl24.Appearance.ForeColor = Color.FromArgb(70, 78, 88);
            this._labelControl24.Appearance.Options.UseForeColor = true;
            this._txChanDoan.Dock = DockStyle.Fill;
            this._txChanDoan.Properties.ReadOnly = true;
            //
            // _panelReport: tab chỉ bao phần Mô tả; Kết luận và Khuyến nghị là
            // hai khối cùng cấp bên dưới, không nằm trong chiều cao của tab.
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
            this._xtraTabPage1.Text = "Kết luận";
            this._xtraTabPage1.Padding = new Padding(3);
            this._xtraTabPage1.Controls.Add(this._rtMoTa);
            this._xtraTabPage1.Controls.Add(this._panel5);
            //
            // _panel5: combo gợi ý kết luận
            //
            this._panel5.Dock = DockStyle.Top;
            this._panel5.Height = 38;
            this._panel5.Padding = new Padding(3, 4, 3, 4);
            this._panel5.Controls.Add(this._cbbMauGoiY);
            this._panel5.Controls.Add(this._label2);
            this._label2.Dock = DockStyle.Left;
            this._label2.Text = "Mô tả:";
            this._label2.Size = new Size(67, 25);
            this._label2.Appearance.Font = new Font("Microsoft Sans Serif", 12F);
            this._label2.Appearance.Options.UseFont = true;
            this._cbbMauGoiY.Dock = DockStyle.Fill;
            this._cbbMauGoiY.Properties.NullText = "Chọn mẫu gợi ý kết luận...";
            this._cbbMauGoiY.Properties.Buttons.AddRange(new EditorButton[] { ComboDropDownButton() });
            this._cbbMauGoiY.SelectedIndexChanged += new System.EventHandler(this._cbbReportTemplate_SelectedIndexChanged);
            //
            // _rtMoTa
            //
            this._rtMoTa.BackColor = Color.White;
            this._rtMoTa.Dock = DockStyle.Fill;
            this._rtMoTa.Font = new Font("Microsoft Sans Serif", 12F);
            //
            // _groupControl1 "Kết luận"
            //
            this._groupControl1.Dock = DockStyle.Bottom;
            this._groupControl1.Height = 166;
            this._groupControl1.Text = "Kết luận";
            this._groupControl1.Controls.Add(this._rtKetLuan);
            this._rtKetLuan.Dock = DockStyle.Fill;
            this._rtKetLuan.Font = new Font("Microsoft Sans Serif", 12F);
            //
            // _groupControl2 "Khuyến nghị"
            //
            this._groupControl2.Dock = DockStyle.Bottom;
            this._groupControl2.Height = 115;
            this._groupControl2.Text = "Khuyến nghị";
            this._groupControl2.Controls.Add(this._rtKhuyenNghi);
            this._rtKhuyenNghi.Dock = DockStyle.Fill;
            this._rtKhuyenNghi.Font = new Font("Microsoft Sans Serif", 12F);
            //
            // _panelCamera: header riêng + viewport camera, tạo phân cấp rõ ràng.
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
            this._cameraHeader.Padding = new Padding(10, 0, 8, 0);
            this._cameraHeader.Controls.Add(this._cameraTitle);
            this._cameraHeader.Controls.Add(this._btnCameraSettings);
            this._cameraTitle.Dock = DockStyle.Fill;
            this._cameraTitle.Text = "CAMERA TRỰC TIẾP";
            this._cameraTitle.TextAlign = ContentAlignment.MiddleLeft;
            this._cameraTitle.Font = new Font("Tahoma", 8.5F, FontStyle.Bold);
            this._cameraTitle.ForeColor = Color.FromArgb(55, 65, 81);
            StyleActionButton(this._btnCameraSettings, 28);
            this._btnCameraSettings.Dock = DockStyle.Right;
            this._btnCameraSettings.Margin = Padding.Empty;
            this._btnCameraSettings.Text = "⚙";
            this._btnCameraSettings.ToolTip = "Cài đặt camera";
            this._btnCameraSettings.Appearance.Font = new Font("Segoe UI Symbol", 10F);
            this._btnCameraSettings.Appearance.Options.UseFont = true;
            this._cameraViewport.Dock = DockStyle.Fill;
            this._cameraViewport.BackColor = Color.Black;
            this._cameraViewport.Padding = new Padding(2);
            //
            // _panelControl1: ba nút cân theo cột, không lệch trái khi đổi độ rộng.
            //
            this._panelControl1.Dock = DockStyle.Fill;
            this._panelControl1.Margin = new Padding(2);
            this._panelControl1.Padding = new Padding(10, 3, 10, 3);
            this._panelControl1.Controls.Add(this._cameraButtonTable);
            this._cameraButtonTable.Dock = DockStyle.Fill;
            this._cameraButtonTable.ColumnCount = 3;
            this._cameraButtonTable.RowCount = 1;
            this._cameraButtonTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            this._cameraButtonTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            this._cameraButtonTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.334F));
            this._cameraButtonTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            StyleActionButton(this._btnSnapshot, 132);
            this._btnSnapshot.Anchor = AnchorStyles.None;
            this._btnSnapshot.Appearance.BackColor = Color.FromArgb(0, 120, 212);
            this._btnSnapshot.Appearance.ForeColor = Color.White;
            this._btnSnapshot.Text = "Chụp nhanh"; this._btnSnapshot.Click += new System.EventHandler(this._btnSnapshot_Click);
            StyleActionButton(this._btnStop, 104);
            this._btnStop.Anchor = AnchorStyles.None;
            this._btnStop.Text = "Dừng"; this._btnStop.Click += new System.EventHandler(this._btnStop_Click);
            StyleActionButton(this._btnLinkCamera, 120);
            this._btnLinkCamera.Anchor = AnchorStyles.None;
            this._btnLinkCamera.Text = "Liên kết"; this._btnLinkCamera.Click += new System.EventHandler(this._btnLinkCamera_Click);
            this._cameraButtonTable.Controls.Add(this._btnSnapshot, 0, 0);
            this._cameraButtonTable.Controls.Add(this._btnStop, 1, 0);
            this._cameraButtonTable.Controls.Add(this._btnLinkCamera, 2, 0);
            //
            // _panelImageList: danh sách ảnh thumbnail + bộ đếm đã chọn
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
            this._lbImageTitle.Text = "ẢNH ĐÃ CHỤP";
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
            // _panelControl2: thanh nút hành động dưới cùng - responsive bằng 2 FlowLayoutPanel
            // (trái = Dock.Left chứa combo layout/máy in; phải = Dock.Fill, FlowDirection
            // RightToLeft chứa các nút) thay vì Location/Anchor tuyệt đối.
            //
            this._panelControl2.Appearance.BackColor = Color.White;
            this._panelControl2.Appearance.Options.UseBackColor = true;
            this._panelControl2.Dock = DockStyle.Fill;
            this._panelControl2.Margin = new Padding(2);
            this._panelControl2.Padding = new Padding(6, 4, 6, 5);
            this._panelControl2.Controls.Add(this._flowPanelControl2Right);
            this._panelControl2.Controls.Add(this._flowPanelControl2Left);
            //
            // _flowPanelControl2Left: combo layout in / máy in
            //
            this._flowPanelControl2Left.Dock = DockStyle.Left;
            this._flowPanelControl2Left.Width = 500;
            this._flowPanelControl2Left.WrapContents = false;
            this._flowPanelControl2Left.Padding = new Padding(0, 3, 0, 2);
            this._cbbLayout.Properties.Buttons.AddRange(new EditorButton[] { ComboDropDownButton() });
            this._cbbPrinters.Properties.Buttons.AddRange(new EditorButton[] { ComboDropDownButton() });
            var layoutField = CreateInlineField("Mẫu in:", this._cbbLayout, 58);
            layoutField.Size = new Size(220, 34);
            layoutField.Dock = DockStyle.None;
            var printerField = CreateInlineField("Máy in:", this._cbbPrinters, 58);
            printerField.Size = new Size(270, 34);
            printerField.Dock = DockStyle.None;
            StyleActionButton(this._btnPrinterSettings, 28);
            this._btnPrinterSettings.Dock = DockStyle.Right;
            this._btnPrinterSettings.Margin = new Padding(2, 0, 0, 0);
            this._btnPrinterSettings.Text = "⚙";
            this._btnPrinterSettings.ToolTip = "Cài đặt máy in mặc định";
            this._btnPrinterSettings.Appearance.Font = new Font("Segoe UI Symbol", 10F);
            this._btnPrinterSettings.Appearance.Options.UseFont = true;
            printerField.Controls.Add(this._btnPrinterSettings);
            this._flowPanelControl2Left.Controls.Add(layoutField);
            this._flowPanelControl2Left.Controls.Add(printerField);
            //
            // _flowPanelControl2Right: các nút hành động, FlowDirection.RightToLeft nên thêm
            // theo thứ tự từ phải sang trái (Thoát thêm trước cùng ra ngoài cùng bên phải).
            //
            this._flowPanelControl2Right.Dock = DockStyle.Fill;
            this._flowPanelControl2Right.FlowDirection = FlowDirection.RightToLeft;
            this._flowPanelControl2Right.WrapContents = false;
            this._flowPanelControl2Right.Padding = new Padding(0, 3, 0, 2);
            StyleActionButton(this._btnCancel, 104);
            this._btnCancel.Text = "Thoát"; this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
            StyleActionButton(this._btnSave, 120);
            this._btnSave.Appearance.BackColor = Color.FromArgb(0, 120, 212);
            this._btnSave.Appearance.ForeColor = Color.White;
            this._btnSave.Text = "Lưu Nháp"; this._btnSave.Click += new System.EventHandler(this._btnSave_Click);
            StyleActionButton(this._btnPrint, 82);
            this._btnPrint.Text = "In"; this._btnPrint.Click += new System.EventHandler(this._btnPrint_Click);
            StyleActionButton(this._btnSignature, 116);
            this._btnSignature.Appearance.BackColor = Color.FromArgb(24, 133, 92);
            this._btnSignature.Appearance.ForeColor = Color.White;
            this._btnSignature.Text = "Ký số"; this._btnSignature.Click += new System.EventHandler(this._btnSignature_Click);
            StyleActionButton(this._btnPreviewMain, 120);
            this._btnPreviewMain.Text = "Xem trước"; this._btnPreviewMain.Click += new System.EventHandler(this._btnPreviewMain_Click);
            this._flowPanelControl2Right.Controls.Add(this._btnCancel);
            this._flowPanelControl2Right.Controls.Add(this._btnSave);
            this._flowPanelControl2Right.Controls.Add(this._btnPrint);
            this._flowPanelControl2Right.Controls.Add(this._btnSignature);
            this._flowPanelControl2Right.Controls.Add(this._btnPreviewMain);
            //
            // _patientSidebar / _patientSidebarSplitter: dải sidebar trái của riêng _bodyPanel.
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
            // FormMainV2
            //
            this.Controls.Add(this._tbTableLayout);
            this.Name = "FormMainV2";
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
