using System.Windows.Forms;

namespace STM.MediaToPACS.Main.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel1 = new DevExpress.XtraEditors.PanelControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this._gridControlChiDinh = new DevExpress.XtraGrid.GridControl();
            this._gridViewChiDinh = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnSoPhieuChiDinh = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnMaChiDinh = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnMaBenhNhan = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnTenBenhNhan = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnGioiTinh = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnNgaySinh = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnTenBacSiChiDinh = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnThoigianthuchien = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnTenDichVu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnTrangThaiPhieu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this._cbbTrangThai = new System.Windows.Forms.ComboBox();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl13 = new DevExpress.XtraEditors.PanelControl();
            this._cbPageSize = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl12 = new DevExpress.XtraEditors.PanelControl();
            this._ccbModalities = new System.Windows.Forms.ComboBox();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl11 = new DevExpress.XtraEditors.PanelControl();
            this._nudPage = new System.Windows.Forms.NumericUpDown();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl10 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this._dtDateToRis = new DevExpress.XtraEditors.DateEdit();
            this.panelControl9 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this._dtDateFromRis = new DevExpress.XtraEditors.DateEdit();
            this.panelControl8 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this._txMaCD = new DevExpress.XtraEditors.TextEdit();
            this.panelControl7 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this._txPatientCodeRis = new DevExpress.XtraEditors.TextEdit();
            this.panelControl6 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this._txBSCDRis = new DevExpress.XtraEditors.TextEdit();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this._txPatientNameRis = new DevExpress.XtraEditors.TextEdit();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this._btnSearchRIS = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl14 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl21 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this._lbSLCaChup = new DevExpress.XtraEditors.LabelControl();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this._tSSLUserName = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this._tssNguoiDung = new System.Windows.Forms.ToolStripStatusLabel();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this._dGVMWLItems = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.AccessionNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PatientID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PatientName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BirthDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Gender = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RequestingPhysician = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ReferringPhysician = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PerformingPhysician = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ScheduledStartDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Modality = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ScheduledStationAE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ScheduleProcedureStep = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RequestedProcedureID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MPPS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel6 = new DevExpress.XtraEditors.PanelControl();
            this._btnMWLQuery = new DevExpress.XtraEditors.SimpleButton();
            this._tLPQuery = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new DevExpress.XtraEditors.GroupControl();
            this._cbMPPSINPROGRESS = new System.Windows.Forms.CheckBox();
            this._txtAccessionNumber = new DevExpress.XtraEditors.TextEdit();
            this._txtPatientFirst = new DevExpress.XtraEditors.TextEdit();
            this._txtPatientMiddle = new DevExpress.XtraEditors.TextEdit();
            this._txtPatientLast = new DevExpress.XtraEditors.TextEdit();
            this._txtPatientID = new DevExpress.XtraEditors.TextEdit();
            this.label6 = new DevExpress.XtraEditors.LabelControl();
            this.label5 = new DevExpress.XtraEditors.LabelControl();
            this._lbPatientFirst = new DevExpress.XtraEditors.LabelControl();
            this._lbPatientMiddle = new DevExpress.XtraEditors.LabelControl();
            this._lbPatientLast = new DevExpress.XtraEditors.LabelControl();
            this._lbPatientID = new DevExpress.XtraEditors.LabelControl();
            this.groupBox2 = new DevExpress.XtraEditors.GroupControl();
            this._cbStartEnd = new System.Windows.Forms.CheckBox();
            this._cbbModality = new DevExpress.XtraEditors.ComboBoxEdit();
            this._dTPEnd = new DevExpress.XtraEditors.DateEdit();
            this._dTPStart = new DevExpress.XtraEditors.DateEdit();
            this._txtAETitle = new DevExpress.XtraEditors.TextEdit();
            this.label14 = new DevExpress.XtraEditors.LabelControl();
            this.label13 = new DevExpress.XtraEditors.LabelControl();
            this.label12 = new DevExpress.XtraEditors.LabelControl();
            this.label15 = new DevExpress.XtraEditors.LabelControl();
            this.label16 = new DevExpress.XtraEditors.LabelControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this._menuAccount = new System.Windows.Forms.ToolStripMenuItem();
            this._tsmChangePassword = new System.Windows.Forms.ToolStripMenuItem();
            this._accountSeparator = new System.Windows.Forms.ToolStripSeparator();
            this._tsmLogout = new System.Windows.Forms.ToolStripMenuItem();
            this._menuSystem = new System.Windows.Forms.ToolStripMenuItem();
            this._tsmSetting = new System.Windows.Forms.ToolStripMenuItem();
            this._systemSeparator = new System.Windows.Forms.ToolStripSeparator();
            this._menuCamera = new System.Windows.Forms.ToolStripMenuItem();
            this._menuWorklist = new System.Windows.Forms.ToolStripMenuItem();
            this._menuTools = new System.Windows.Forms.ToolStripMenuItem();
            this._tsmLog = new System.Windows.Forms.ToolStripMenuItem();
            this._menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this._tsmToUse = new System.Windows.Forms.ToolStripMenuItem();
            this._tsmCbbVideoCapture = new System.Windows.Forms.ToolStripComboBox();
            this._tsmCbbWorklist = new System.Windows.Forms.ToolStripComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridControlChiDinh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._gridViewChiDinh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl13)).BeginInit();
            this.panelControl13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._cbPageSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl12)).BeginInit();
            this.panelControl12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl11)).BeginInit();
            this.panelControl11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._nudPage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl10)).BeginInit();
            this.panelControl10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dtDateToRis.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dtDateToRis.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl9)).BeginInit();
            this.panelControl9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dtDateFromRis.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dtDateFromRis.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl8)).BeginInit();
            this.panelControl8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txMaCD.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl7)).BeginInit();
            this.panelControl7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txPatientCodeRis.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).BeginInit();
            this.panelControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txBSCDRis.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            this.panelControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txPatientNameRis.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl14)).BeginInit();
            this.panelControl14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl21)).BeginInit();
            this.panelControl21.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dGVMWLItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel6)).BeginInit();
            this.panel6.SuspendLayout();
            this._tLPQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txtAccessionNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtPatientFirst.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtPatientMiddle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtPatientLast.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtPatientID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox2)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._cbbModality.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dTPEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dTPEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dTPStart.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dTPStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtAETitle.Properties)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.xtraTabControl1);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1795, 796);
            this.panel1.TabIndex = 0;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(2, 37);
            this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(1791, 757);
            this.xtraTabControl1.TabIndex = 27;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.groupControl2);
            this.xtraTabPage1.Controls.Add(this.panelControl2);
            this.xtraTabPage1.Controls.Add(this.panelControl14);
            this.xtraTabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(1781, 719);
            this.xtraTabPage1.Text = "Hệ thống RIS";
            // 
            // groupControl2
            // 
            this.groupControl2.AppearanceCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl2.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.groupControl2.AppearanceCaption.Options.UseFont = true;
            this.groupControl2.AppearanceCaption.Options.UseForeColor = true;
            this.groupControl2.Controls.Add(this._gridControlChiDinh);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(0, 106);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Padding = new System.Windows.Forms.Padding(5);
            this.groupControl2.Size = new System.Drawing.Size(1781, 569);
            this.groupControl2.TabIndex = 5;
            this.groupControl2.Text = "Danh sách chỉ định";
            // 
            // _gridControlChiDinh
            // 
            this._gridControlChiDinh.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridControlChiDinh.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._gridControlChiDinh.Location = new System.Drawing.Point(7, 36);
            this._gridControlChiDinh.MainView = this._gridViewChiDinh;
            this._gridControlChiDinh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._gridControlChiDinh.Name = "_gridControlChiDinh";
            this._gridControlChiDinh.Size = new System.Drawing.Size(1767, 526);
            this._gridControlChiDinh.TabIndex = 1;
            this._gridControlChiDinh.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this._gridViewChiDinh});
            // 
            // _gridViewChiDinh
            // 
            this._gridViewChiDinh.Appearance.GroupRow.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._gridViewChiDinh.Appearance.GroupRow.Options.UseFont = true;
            this._gridViewChiDinh.Appearance.HeaderPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._gridViewChiDinh.Appearance.HeaderPanel.Options.UseFont = true;
            this._gridViewChiDinh.Appearance.Row.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._gridViewChiDinh.Appearance.Row.Options.UseFont = true;
            this._gridViewChiDinh.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnId,
            this.gridColumnSoPhieuChiDinh,
            this.gridColumnMaChiDinh,
            this.gridColumnMaBenhNhan,
            this.gridColumnTenBenhNhan,
            this.gridColumnGioiTinh,
            this.gridColumnNgaySinh,
            this.gridColumnTenBacSiChiDinh,
            this.gridColumnThoigianthuchien,
            this.gridColumnTenDichVu,
            this.gridColumnTrangThaiPhieu});
            this._gridViewChiDinh.GridControl = this._gridControlChiDinh;
            this._gridViewChiDinh.GroupPanelText = "Danh sách chỉ định";
            this._gridViewChiDinh.Name = "_gridViewChiDinh";
            this._gridViewChiDinh.OptionsBehavior.Editable = false;
            this._gridViewChiDinh.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDownFocused;
            this._gridViewChiDinh.OptionsView.ShowGroupPanel = false;
            this._gridViewChiDinh.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this._gridViewChiDinh_FocusedRowChanged);
            this._gridViewChiDinh.DoubleClick += new System.EventHandler(this._gridViewChiDinh_DoubleClick);
            // 
            // gridColumnId
            // 
            this.gridColumnId.Caption = "Id";
            this.gridColumnId.FieldName = "Id";
            this.gridColumnId.MinWidth = 24;
            this.gridColumnId.Name = "gridColumnId";
            this.gridColumnId.Width = 94;
            // 
            // gridColumnSoPhieuChiDinh
            // 
            this.gridColumnSoPhieuChiDinh.Caption = "Số Phiếu CĐ";
            this.gridColumnSoPhieuChiDinh.FieldName = "SoPhieuChiDinh";
            this.gridColumnSoPhieuChiDinh.MaxWidth = 150;
            this.gridColumnSoPhieuChiDinh.MinWidth = 25;
            this.gridColumnSoPhieuChiDinh.Name = "gridColumnSoPhieuChiDinh";
            this.gridColumnSoPhieuChiDinh.Visible = true;
            this.gridColumnSoPhieuChiDinh.VisibleIndex = 0;
            this.gridColumnSoPhieuChiDinh.Width = 94;
            // 
            // gridColumnMaChiDinh
            // 
            this.gridColumnMaChiDinh.Caption = "Mã chỉ định";
            this.gridColumnMaChiDinh.FieldName = "MaChiDinh";
            this.gridColumnMaChiDinh.MinWidth = 25;
            this.gridColumnMaChiDinh.Name = "gridColumnMaChiDinh";
            this.gridColumnMaChiDinh.Width = 94;
            // 
            // gridColumnMaBenhNhan
            // 
            this.gridColumnMaBenhNhan.Caption = "Mã BN";
            this.gridColumnMaBenhNhan.FieldName = "MaBenhNhan";
            this.gridColumnMaBenhNhan.MaxWidth = 150;
            this.gridColumnMaBenhNhan.MinWidth = 24;
            this.gridColumnMaBenhNhan.Name = "gridColumnMaBenhNhan";
            this.gridColumnMaBenhNhan.Visible = true;
            this.gridColumnMaBenhNhan.VisibleIndex = 1;
            this.gridColumnMaBenhNhan.Width = 94;
            // 
            // gridColumnTenBenhNhan
            // 
            this.gridColumnTenBenhNhan.Caption = "Tên BN";
            this.gridColumnTenBenhNhan.FieldName = "HoTen";
            this.gridColumnTenBenhNhan.MinWidth = 24;
            this.gridColumnTenBenhNhan.Name = "gridColumnTenBenhNhan";
            this.gridColumnTenBenhNhan.Visible = true;
            this.gridColumnTenBenhNhan.VisibleIndex = 2;
            this.gridColumnTenBenhNhan.Width = 133;
            // 
            // gridColumnGioiTinh
            // 
            this.gridColumnGioiTinh.Caption = "Giới tính";
            this.gridColumnGioiTinh.FieldName = "GioiTinh";
            this.gridColumnGioiTinh.MaxWidth = 80;
            this.gridColumnGioiTinh.MinWidth = 24;
            this.gridColumnGioiTinh.Name = "gridColumnGioiTinh";
            this.gridColumnGioiTinh.Visible = true;
            this.gridColumnGioiTinh.VisibleIndex = 3;
            this.gridColumnGioiTinh.Width = 69;
            // 
            // gridColumnNgaySinh
            // 
            this.gridColumnNgaySinh.Caption = "Ngày Sinh";
            this.gridColumnNgaySinh.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.gridColumnNgaySinh.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumnNgaySinh.FieldName = "NgaySinh";
            this.gridColumnNgaySinh.MaxWidth = 150;
            this.gridColumnNgaySinh.MinWidth = 24;
            this.gridColumnNgaySinh.Name = "gridColumnNgaySinh";
            this.gridColumnNgaySinh.Visible = true;
            this.gridColumnNgaySinh.VisibleIndex = 4;
            this.gridColumnNgaySinh.Width = 94;
            // 
            // gridColumnTenBacSiChiDinh
            // 
            this.gridColumnTenBacSiChiDinh.Caption = "Tên bác sĩ CĐ";
            this.gridColumnTenBacSiChiDinh.FieldName = "TenBacSiChiDinh";
            this.gridColumnTenBacSiChiDinh.MinWidth = 25;
            this.gridColumnTenBacSiChiDinh.Name = "gridColumnTenBacSiChiDinh";
            this.gridColumnTenBacSiChiDinh.Visible = true;
            this.gridColumnTenBacSiChiDinh.VisibleIndex = 5;
            this.gridColumnTenBacSiChiDinh.Width = 94;
            // 
            // gridColumnThoigianthuchien
            // 
            this.gridColumnThoigianthuchien.Caption = "Thời gian dự kiến";
            this.gridColumnThoigianthuchien.DisplayFormat.FormatString = "HH:mm:ss dd/MM/yyyy";
            this.gridColumnThoigianthuchien.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumnThoigianthuchien.FieldName = "Thoigianthuchien";
            this.gridColumnThoigianthuchien.MinWidth = 25;
            this.gridColumnThoigianthuchien.Name = "gridColumnThoigianthuchien";
            this.gridColumnThoigianthuchien.Visible = true;
            this.gridColumnThoigianthuchien.VisibleIndex = 6;
            this.gridColumnThoigianthuchien.Width = 94;
            // 
            // gridColumnTenDichVu
            // 
            this.gridColumnTenDichVu.Caption = "Tên dịch vụ";
            this.gridColumnTenDichVu.FieldName = "TenDichVu";
            this.gridColumnTenDichVu.MinWidth = 24;
            this.gridColumnTenDichVu.Name = "gridColumnTenDichVu";
            this.gridColumnTenDichVu.Visible = true;
            this.gridColumnTenDichVu.VisibleIndex = 7;
            this.gridColumnTenDichVu.Width = 94;
            // 
            // gridColumnTrangThaiPhieu
            // 
            this.gridColumnTrangThaiPhieu.Caption = "Trạng Thái ";
            this.gridColumnTrangThaiPhieu.FieldName = "TrangThai";
            this.gridColumnTrangThaiPhieu.MaxWidth = 150;
            this.gridColumnTrangThaiPhieu.MinWidth = 24;
            this.gridColumnTrangThaiPhieu.Name = "gridColumnTrangThaiPhieu";
            this.gridColumnTrangThaiPhieu.Visible = true;
            this.gridColumnTrangThaiPhieu.VisibleIndex = 8;
            this.gridColumnTrangThaiPhieu.Width = 94;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.tableLayoutPanel1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1781, 106);
            this.panelControl2.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 291F));
            this.tableLayoutPanel1.Controls.Add(this.tablePanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelControl4, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1777, 102);
            this.tableLayoutPanel1.TabIndex = 36;
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 18F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 18F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 18F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 18F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 19.4F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 10.6F)});
            this.tablePanel1.Controls.Add(this.panelControl3);
            this.tablePanel1.Controls.Add(this.panelControl13);
            this.tablePanel1.Controls.Add(this.panelControl12);
            this.tablePanel1.Controls.Add(this.panelControl11);
            this.tablePanel1.Controls.Add(this.panelControl10);
            this.tablePanel1.Controls.Add(this.panelControl9);
            this.tablePanel1.Controls.Add(this.panelControl8);
            this.tablePanel1.Controls.Add(this.panelControl7);
            this.tablePanel1.Controls.Add(this.panelControl6);
            this.tablePanel1.Controls.Add(this.panelControl5);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(3, 3);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 47.59998F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(1480, 96);
            this.tablePanel1.TabIndex = 2;
            // 
            // panelControl3
            // 
            this.tablePanel1.SetColumn(this.panelControl3, 4);
            this.panelControl3.Controls.Add(this._cbbTrangThai);
            this.panelControl3.Controls.Add(this.labelControl5);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(1048, 3);
            this.panelControl3.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.panelControl3.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl3.Name = "panelControl3";
            this.tablePanel1.SetRow(this.panelControl3, 0);
            this.panelControl3.Size = new System.Drawing.Size(275, 42);
            this.panelControl3.TabIndex = 43;
            // 
            // _cbbTrangThai
            // 
            this._cbbTrangThai.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._cbbTrangThai.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._cbbTrangThai.FormattingEnabled = true;
            this._cbbTrangThai.Items.AddRange(new object[] {
            "Tất cả",
            "Chưa thực hiện",
            "Đã lên lịch",
            "Đã thực hiện",
            "Đã hủy"});
            this._cbbTrangThai.Location = new System.Drawing.Point(81, 4);
            this._cbbTrangThai.Margin = new System.Windows.Forms.Padding(2);
            this._cbbTrangThai.Name = "_cbbTrangThai";
            this._cbbTrangThai.Size = new System.Drawing.Size(183, 30);
            this._cbbTrangThai.TabIndex = 46;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(11, 9);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(65, 18);
            this.labelControl5.TabIndex = 41;
            this.labelControl5.Text = "Trạng thái";
            // 
            // panelControl13
            // 
            this.tablePanel1.SetColumn(this.panelControl13, 5);
            this.panelControl13.Controls.Add(this._cbPageSize);
            this.panelControl13.Controls.Add(this.labelControl6);
            this.panelControl13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl13.Location = new System.Drawing.Point(1329, 51);
            this.panelControl13.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.panelControl13.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl13.Name = "panelControl13";
            this.tablePanel1.SetRow(this.panelControl13, 1);
            this.panelControl13.Size = new System.Drawing.Size(148, 42);
            this.panelControl13.TabIndex = 43;
            // 
            // _cbPageSize
            // 
            this._cbPageSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._cbPageSize.EditValue = "1000";
            this._cbPageSize.Location = new System.Drawing.Point(68, 5);
            this._cbPageSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._cbPageSize.Name = "_cbPageSize";
            this._cbPageSize.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._cbPageSize.Properties.Appearance.Options.UseFont = true;
            this._cbPageSize.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._cbPageSize.Properties.Items.AddRange(new object[] {
            "20",
            "50",
            "100",
            "500",
            "1000"});
            this._cbPageSize.Size = new System.Drawing.Size(73, 28);
            this._cbPageSize.TabIndex = 35;
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl6.Appearance.Options.UseFont = true;
            this.labelControl6.Location = new System.Drawing.Point(6, 14);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(22, 18);
            this.labelControl6.TabIndex = 34;
            this.labelControl6.Text = "SL:";
            // 
            // panelControl12
            // 
            this.tablePanel1.SetColumn(this.panelControl12, 4);
            this.panelControl12.Controls.Add(this._ccbModalities);
            this.panelControl12.Controls.Add(this.labelControl3);
            this.panelControl12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl12.Location = new System.Drawing.Point(1048, 51);
            this.panelControl12.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.panelControl12.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl12.Name = "panelControl12";
            this.tablePanel1.SetRow(this.panelControl12, 1);
            this.panelControl12.Size = new System.Drawing.Size(275, 42);
            this.panelControl12.TabIndex = 1;
            // 
            // _ccbModalities
            // 
            this._ccbModalities.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._ccbModalities.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._ccbModalities.FormattingEnabled = true;
            this._ccbModalities.Items.AddRange(new object[] {
            "ES",
            "US",
            "DX",
            "CT",
            "MR",
            "US",
            "PT"});
            this._ccbModalities.Location = new System.Drawing.Point(140, 6);
            this._ccbModalities.Name = "_ccbModalities";
            this._ccbModalities.Size = new System.Drawing.Size(124, 26);
            this._ccbModalities.TabIndex = 42;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(11, 9);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(123, 18);
            this.labelControl3.TabIndex = 41;
            this.labelControl3.Text = "Phương thức chụp:";
            // 
            // panelControl11
            // 
            this.tablePanel1.SetColumn(this.panelControl11, 5);
            this.panelControl11.Controls.Add(this._nudPage);
            this.panelControl11.Controls.Add(this.labelControl7);
            this.panelControl11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl11.Location = new System.Drawing.Point(1329, 3);
            this.panelControl11.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.panelControl11.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl11.Name = "panelControl11";
            this.tablePanel1.SetRow(this.panelControl11, 0);
            this.panelControl11.Size = new System.Drawing.Size(148, 42);
            this.panelControl11.TabIndex = 1;
            // 
            // _nudPage
            // 
            this._nudPage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._nudPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._nudPage.Location = new System.Drawing.Point(68, 5);
            this._nudPage.Margin = new System.Windows.Forms.Padding(3, 2, 13, 2);
            this._nudPage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._nudPage.Name = "_nudPage";
            this._nudPage.Size = new System.Drawing.Size(73, 28);
            this._nudPage.TabIndex = 27;
            this._nudPage.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl7.Appearance.Options.UseFont = true;
            this.labelControl7.Location = new System.Drawing.Point(6, 12);
            this.labelControl7.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(42, 18);
            this.labelControl7.TabIndex = 26;
            this.labelControl7.Text = "Trang:";
            // 
            // panelControl10
            // 
            this.tablePanel1.SetColumn(this.panelControl10, 3);
            this.panelControl10.Controls.Add(this.labelControl9);
            this.panelControl10.Controls.Add(this._dtDateToRis);
            this.panelControl10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl10.Location = new System.Drawing.Point(787, 51);
            this.panelControl10.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.panelControl10.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl10.Name = "panelControl10";
            this.tablePanel1.SetRow(this.panelControl10, 1);
            this.panelControl10.Size = new System.Drawing.Size(255, 42);
            this.panelControl10.TabIndex = 3;
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl9.Appearance.Options.UseFont = true;
            this.labelControl9.Location = new System.Drawing.Point(10, 13);
            this.labelControl9.Margin = new System.Windows.Forms.Padding(19, 4, 3, 4);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(31, 18);
            this.labelControl9.TabIndex = 38;
            this.labelControl9.Text = "Đến:";
            // 
            // _dtDateToRis
            // 
            this._dtDateToRis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._dtDateToRis.EditValue = null;
            this._dtDateToRis.Location = new System.Drawing.Point(47, 7);
            this._dtDateToRis.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._dtDateToRis.Name = "_dtDateToRis";
            this._dtDateToRis.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._dtDateToRis.Properties.Appearance.Options.UseFont = true;
            this._dtDateToRis.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._dtDateToRis.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._dtDateToRis.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this._dtDateToRis.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this._dtDateToRis.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this._dtDateToRis.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this._dtDateToRis.Size = new System.Drawing.Size(202, 28);
            this._dtDateToRis.TabIndex = 36;
            // 
            // panelControl9
            // 
            this.tablePanel1.SetColumn(this.panelControl9, 2);
            this.panelControl9.Controls.Add(this.labelControl8);
            this.panelControl9.Controls.Add(this._dtDateFromRis);
            this.panelControl9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl9.Location = new System.Drawing.Point(525, 51);
            this.panelControl9.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.panelControl9.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl9.Name = "panelControl9";
            this.tablePanel1.SetRow(this.panelControl9, 1);
            this.panelControl9.Size = new System.Drawing.Size(255, 42);
            this.panelControl9.TabIndex = 1;
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl8.Appearance.Options.UseFont = true;
            this.labelControl8.Location = new System.Drawing.Point(10, 13);
            this.labelControl8.Margin = new System.Windows.Forms.Padding(19, 4, 3, 4);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(21, 18);
            this.labelControl8.TabIndex = 37;
            this.labelControl8.Text = "Từ:";
            // 
            // _dtDateFromRis
            // 
            this._dtDateFromRis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._dtDateFromRis.EditValue = null;
            this._dtDateFromRis.Location = new System.Drawing.Point(37, 6);
            this._dtDateFromRis.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._dtDateFromRis.Name = "_dtDateFromRis";
            this._dtDateFromRis.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._dtDateFromRis.Properties.Appearance.Options.UseFont = true;
            this._dtDateFromRis.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._dtDateFromRis.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._dtDateFromRis.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this._dtDateFromRis.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this._dtDateFromRis.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this._dtDateFromRis.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this._dtDateFromRis.Size = new System.Drawing.Size(212, 28);
            this._dtDateFromRis.TabIndex = 33;
            // 
            // panelControl8
            // 
            this.tablePanel1.SetColumn(this.panelControl8, 1);
            this.panelControl8.Controls.Add(this.labelControl4);
            this.panelControl8.Controls.Add(this._txMaCD);
            this.panelControl8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl8.Location = new System.Drawing.Point(264, 51);
            this.panelControl8.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.panelControl8.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl8.Name = "panelControl8";
            this.tablePanel1.SetRow(this.panelControl8, 1);
            this.panelControl8.Size = new System.Drawing.Size(255, 42);
            this.panelControl8.TabIndex = 1;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(6, 13);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(51, 18);
            this.labelControl4.TabIndex = 28;
            this.labelControl4.Text = "Mã CĐ:";
            // 
            // _txMaCD
            // 
            this._txMaCD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txMaCD.Location = new System.Drawing.Point(62, 7);
            this._txMaCD.Margin = new System.Windows.Forms.Padding(13, 4, 3, 4);
            this._txMaCD.Name = "_txMaCD";
            this._txMaCD.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._txMaCD.Properties.Appearance.Options.UseFont = true;
            this._txMaCD.Size = new System.Drawing.Size(186, 28);
            this._txMaCD.TabIndex = 29;
            this._txMaCD.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textEdit_KeyDown);
            // 
            // panelControl7
            // 
            this.tablePanel1.SetColumn(this.panelControl7, 0);
            this.panelControl7.Controls.Add(this.labelControl10);
            this.panelControl7.Controls.Add(this._txPatientCodeRis);
            this.panelControl7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl7.Location = new System.Drawing.Point(3, 51);
            this.panelControl7.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.panelControl7.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl7.Name = "panelControl7";
            this.tablePanel1.SetRow(this.panelControl7, 1);
            this.panelControl7.Size = new System.Drawing.Size(255, 42);
            this.panelControl7.TabIndex = 2;
            // 
            // labelControl10
            // 
            this.labelControl10.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl10.Appearance.Options.UseFont = true;
            this.labelControl10.Location = new System.Drawing.Point(10, 13);
            this.labelControl10.Margin = new System.Windows.Forms.Padding(13, 12, 3, 4);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(50, 18);
            this.labelControl10.TabIndex = 39;
            this.labelControl10.Text = "Mã BN:";
            // 
            // _txPatientCodeRis
            // 
            this._txPatientCodeRis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txPatientCodeRis.Location = new System.Drawing.Point(74, 6);
            this._txPatientCodeRis.Margin = new System.Windows.Forms.Padding(13, 4, 3, 4);
            this._txPatientCodeRis.Name = "_txPatientCodeRis";
            this._txPatientCodeRis.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._txPatientCodeRis.Properties.Appearance.Options.UseFont = true;
            this._txPatientCodeRis.Size = new System.Drawing.Size(175, 28);
            this._txPatientCodeRis.TabIndex = 40;
            this._txPatientCodeRis.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textEdit_KeyDown);
            // 
            // panelControl6
            // 
            this.tablePanel1.SetColumn(this.panelControl6, 2);
            this.tablePanel1.SetColumnSpan(this.panelControl6, 2);
            this.panelControl6.Controls.Add(this.labelControl2);
            this.panelControl6.Controls.Add(this._txBSCDRis);
            this.panelControl6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl6.Location = new System.Drawing.Point(525, 3);
            this.panelControl6.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.panelControl6.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl6.Name = "panelControl6";
            this.tablePanel1.SetRow(this.panelControl6, 0);
            this.panelControl6.Size = new System.Drawing.Size(517, 42);
            this.panelControl6.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(10, 12);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(19, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(84, 18);
            this.labelControl2.TabIndex = 24;
            this.labelControl2.Text = "BS Chỉ Định:";
            // 
            // _txBSCDRis
            // 
            this._txBSCDRis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txBSCDRis.Location = new System.Drawing.Point(110, 5);
            this._txBSCDRis.Margin = new System.Windows.Forms.Padding(13, 4, 3, 4);
            this._txBSCDRis.Name = "_txBSCDRis";
            this._txBSCDRis.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._txBSCDRis.Properties.Appearance.Options.UseFont = true;
            this._txBSCDRis.Size = new System.Drawing.Size(401, 28);
            this._txBSCDRis.TabIndex = 25;
            this._txBSCDRis.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textEdit_KeyDown);
            // 
            // panelControl5
            // 
            this.tablePanel1.SetColumn(this.panelControl5, 0);
            this.tablePanel1.SetColumnSpan(this.panelControl5, 2);
            this.panelControl5.Controls.Add(this.labelControl1);
            this.panelControl5.Controls.Add(this._txPatientNameRis);
            this.panelControl5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl5.Location = new System.Drawing.Point(3, 3);
            this.panelControl5.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.panelControl5.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl5.Name = "panelControl5";
            this.tablePanel1.SetRow(this.panelControl5, 0);
            this.panelControl5.Size = new System.Drawing.Size(516, 42);
            this.panelControl5.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(10, 12);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(13, 12, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(54, 18);
            this.labelControl1.TabIndex = 22;
            this.labelControl1.Text = "Tên BN:";
            // 
            // _txPatientNameRis
            // 
            this._txPatientNameRis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txPatientNameRis.Location = new System.Drawing.Point(74, 5);
            this._txPatientNameRis.Margin = new System.Windows.Forms.Padding(13, 4, 3, 4);
            this._txPatientNameRis.Name = "_txPatientNameRis";
            this._txPatientNameRis.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._txPatientNameRis.Properties.Appearance.Options.UseFont = true;
            this._txPatientNameRis.Size = new System.Drawing.Size(435, 28);
            this._txPatientNameRis.TabIndex = 23;
            this._txPatientNameRis.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textEdit_KeyDown);
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this._btnSearchRIS);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl4.Location = new System.Drawing.Point(1489, 2);
            this.panelControl4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Padding = new System.Windows.Forms.Padding(5);
            this.panelControl4.Size = new System.Drawing.Size(285, 98);
            this.panelControl4.TabIndex = 36;
            // 
            // _btnSearchRIS
            // 
            this._btnSearchRIS.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btnSearchRIS.Appearance.Options.UseFont = true;
            this._btnSearchRIS.Dock = System.Windows.Forms.DockStyle.Fill;
            this._btnSearchRIS.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("_btnSearchRIS.ImageOptions.Image")));
            this._btnSearchRIS.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this._btnSearchRIS.Location = new System.Drawing.Point(7, 7);
            this._btnSearchRIS.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._btnSearchRIS.Name = "_btnSearchRIS";
            this._btnSearchRIS.Size = new System.Drawing.Size(271, 84);
            this._btnSearchRIS.TabIndex = 33;
            this._btnSearchRIS.Text = "Tìm kiếm";
            this._btnSearchRIS.Click += new System.EventHandler(this._btnSearchRIS_Click);
            // 
            // panelControl14
            // 
            this.panelControl14.Controls.Add(this.panelControl21);
            this.panelControl14.Controls.Add(this.statusStrip1);
            this.panelControl14.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl14.Location = new System.Drawing.Point(0, 675);
            this.panelControl14.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.panelControl14.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl14.Name = "panelControl14";
            this.panelControl14.Size = new System.Drawing.Size(1781, 44);
            this.panelControl14.TabIndex = 35;
            // 
            // panelControl21
            // 
            this.panelControl21.Controls.Add(this.labelControl13);
            this.panelControl21.Controls.Add(this._lbSLCaChup);
            this.panelControl21.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl21.Location = new System.Drawing.Point(1578, 3);
            this.panelControl21.Name = "panelControl21";
            this.panelControl21.Size = new System.Drawing.Size(200, 38);
            this.panelControl21.TabIndex = 34;
            // 
            // labelControl13
            // 
            this.labelControl13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl13.Appearance.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl13.Appearance.ForeColor = System.Drawing.Color.SaddleBrown;
            this.labelControl13.Appearance.Options.UseFont = true;
            this.labelControl13.Appearance.Options.UseForeColor = true;
            this.labelControl13.Location = new System.Drawing.Point(5, 12);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(83, 21);
            this.labelControl13.TabIndex = 6;
            this.labelControl13.Text = "Số lượng:";
            // 
            // _lbSLCaChup
            // 
            this._lbSLCaChup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._lbSLCaChup.Appearance.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbSLCaChup.Appearance.ForeColor = System.Drawing.Color.Red;
            this._lbSLCaChup.Appearance.Options.UseFont = true;
            this._lbSLCaChup.Appearance.Options.UseForeColor = true;
            this._lbSLCaChup.Location = new System.Drawing.Point(94, 12);
            this._lbSLCaChup.Name = "_lbSLCaChup";
            this._lbSLCaChup.Size = new System.Drawing.Size(11, 21);
            this._lbSLCaChup.TabIndex = 7;
            this._lbSLCaChup.Text = "0";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this._tSSLUserName,
            this.toolStripStatusLabel2,
            this._tssNguoiDung});
            this.statusStrip1.Location = new System.Drawing.Point(3, 3);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1775, 38);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.SaddleBrown;
            this.toolStripStatusLabel1.Margin = new System.Windows.Forms.Padding(15, 4, 0, 2);
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(133, 32);
            this.toolStripStatusLabel1.Text = "Tên đăng nhập:";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _tSSLUserName
            // 
            this._tSSLUserName.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._tSSLUserName.ForeColor = System.Drawing.Color.Red;
            this._tSSLUserName.Name = "_tSSLUserName";
            this._tSSLUserName.Size = new System.Drawing.Size(35, 32);
            this._tSSLUserName.Text = "NA";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.SaddleBrown;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(133, 32);
            this.toolStripStatusLabel2.Text = "Người dùng:";
            this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _tssNguoiDung
            // 
            this._tssNguoiDung.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._tssNguoiDung.ForeColor = System.Drawing.Color.Red;
            this._tssNguoiDung.Name = "_tssNguoiDung";
            this._tssNguoiDung.Size = new System.Drawing.Size(35, 32);
            this._tssNguoiDung.Text = "NA";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this._dGVMWLItems);
            this.xtraTabPage2.Controls.Add(this.panel6);
            this.xtraTabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.PageVisible = false;
            this.xtraTabPage2.Size = new System.Drawing.Size(1781, 719);
            this.xtraTabPage2.Text = "Worklist";
            // 
            // _dGVMWLItems
            // 
            this._dGVMWLItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dGVMWLItems.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._dGVMWLItems.Location = new System.Drawing.Point(0, 176);
            this._dGVMWLItems.MainView = this.gridView1;
            this._dGVMWLItems.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._dGVMWLItems.Name = "_dGVMWLItems";
            this._dGVMWLItems.Size = new System.Drawing.Size(1781, 543);
            this._dGVMWLItems.TabIndex = 26;
            this._dGVMWLItems.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.AccessionNumber,
            this.PatientID,
            this.PatientName,
            this.BirthDate,
            this.Gender,
            this.RequestingPhysician,
            this.ReferringPhysician,
            this.PerformingPhysician,
            this.ScheduledStartDate,
            this.Modality,
            this.ScheduledStationAE,
            this.ScheduleProcedureStep,
            this.RequestedProcedureID,
            this.MPPS});
            this.gridView1.DetailHeight = 450;
            this.gridView1.GridControl = this._dGVMWLItems;
            this.gridView1.GroupPanelText = "Danh sách bệnh nhân";
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.DoubleClick += new System.EventHandler(this._dGVMWLItems_DoubleClick);
            // 
            // AccessionNumber
            // 
            this.AccessionNumber.Caption = "Mã phiếu chụp";
            this.AccessionNumber.FieldName = "AccessionNumber";
            this.AccessionNumber.MinWidth = 26;
            this.AccessionNumber.Name = "AccessionNumber";
            this.AccessionNumber.Visible = true;
            this.AccessionNumber.VisibleIndex = 0;
            this.AccessionNumber.Width = 96;
            // 
            // PatientID
            // 
            this.PatientID.Caption = "Mã bệnh nhân";
            this.PatientID.FieldName = "PatientID";
            this.PatientID.MinWidth = 26;
            this.PatientID.Name = "PatientID";
            this.PatientID.Visible = true;
            this.PatientID.VisibleIndex = 1;
            this.PatientID.Width = 96;
            // 
            // PatientName
            // 
            this.PatientName.Caption = "Tên bệnh nhân";
            this.PatientName.FieldName = "PatientName";
            this.PatientName.MinWidth = 26;
            this.PatientName.Name = "PatientName";
            this.PatientName.Visible = true;
            this.PatientName.VisibleIndex = 2;
            this.PatientName.Width = 96;
            // 
            // BirthDate
            // 
            this.BirthDate.Caption = "Ngày sinh";
            this.BirthDate.FieldName = "BirthDate";
            this.BirthDate.MinWidth = 26;
            this.BirthDate.Name = "BirthDate";
            this.BirthDate.Visible = true;
            this.BirthDate.VisibleIndex = 3;
            this.BirthDate.Width = 96;
            // 
            // Gender
            // 
            this.Gender.Caption = "Giới tính";
            this.Gender.FieldName = "Gender";
            this.Gender.MinWidth = 26;
            this.Gender.Name = "Gender";
            this.Gender.Visible = true;
            this.Gender.VisibleIndex = 4;
            this.Gender.Width = 96;
            // 
            // RequestingPhysician
            // 
            this.RequestingPhysician.Caption = "Bác sĩ chỉ định";
            this.RequestingPhysician.FieldName = "RequestingPhysician";
            this.RequestingPhysician.MinWidth = 26;
            this.RequestingPhysician.Name = "RequestingPhysician";
            this.RequestingPhysician.Visible = true;
            this.RequestingPhysician.VisibleIndex = 5;
            this.RequestingPhysician.Width = 96;
            // 
            // ReferringPhysician
            // 
            this.ReferringPhysician.Caption = "Bác sĩ chẩn đoán";
            this.ReferringPhysician.FieldName = "ReferringPhysician";
            this.ReferringPhysician.MinWidth = 26;
            this.ReferringPhysician.Name = "ReferringPhysician";
            this.ReferringPhysician.Visible = true;
            this.ReferringPhysician.VisibleIndex = 6;
            this.ReferringPhysician.Width = 96;
            // 
            // PerformingPhysician
            // 
            this.PerformingPhysician.Caption = "Bác sĩ chụp";
            this.PerformingPhysician.FieldName = "PerformingPhysician";
            this.PerformingPhysician.MinWidth = 26;
            this.PerformingPhysician.Name = "PerformingPhysician";
            this.PerformingPhysician.Visible = true;
            this.PerformingPhysician.VisibleIndex = 7;
            this.PerformingPhysician.Width = 96;
            // 
            // ScheduledStartDate
            // 
            this.ScheduledStartDate.Caption = "Thời gian dự kiến";
            this.ScheduledStartDate.FieldName = "ScheduledStartDate";
            this.ScheduledStartDate.MinWidth = 26;
            this.ScheduledStartDate.Name = "ScheduledStartDate";
            this.ScheduledStartDate.Visible = true;
            this.ScheduledStartDate.VisibleIndex = 8;
            this.ScheduledStartDate.Width = 96;
            // 
            // Modality
            // 
            this.Modality.Caption = "Phương thức chụp";
            this.Modality.FieldName = "Modality";
            this.Modality.MinWidth = 26;
            this.Modality.Name = "Modality";
            this.Modality.Visible = true;
            this.Modality.VisibleIndex = 9;
            this.Modality.Width = 96;
            // 
            // ScheduledStationAE
            // 
            this.ScheduledStationAE.Caption = "Scheduled Station AE";
            this.ScheduledStationAE.FieldName = "ScheduledStationAE";
            this.ScheduledStationAE.MinWidth = 26;
            this.ScheduledStationAE.Name = "ScheduledStationAE";
            this.ScheduledStationAE.Width = 96;
            // 
            // ScheduleProcedureStep
            // 
            this.ScheduleProcedureStep.Caption = "Scheduled Procedure Step";
            this.ScheduleProcedureStep.FieldName = "ScheduleProcedureStep";
            this.ScheduleProcedureStep.MinWidth = 26;
            this.ScheduleProcedureStep.Name = "ScheduleProcedureStep";
            this.ScheduleProcedureStep.Width = 96;
            // 
            // RequestedProcedureID
            // 
            this.RequestedProcedureID.Caption = "Requested Procedure ID";
            this.RequestedProcedureID.FieldName = "RequestedProcedureID";
            this.RequestedProcedureID.MinWidth = 26;
            this.RequestedProcedureID.Name = "RequestedProcedureID";
            this.RequestedProcedureID.Width = 96;
            // 
            // MPPS
            // 
            this.MPPS.Caption = "MPPS";
            this.MPPS.FieldName = "MPPS";
            this.MPPS.MinWidth = 26;
            this.MPPS.Name = "MPPS";
            this.MPPS.Visible = true;
            this.MPPS.VisibleIndex = 10;
            this.MPPS.Width = 96;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
            this.panel6.Controls.Add(this._btnMWLQuery);
            this.panel6.Controls.Add(this._tLPQuery);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1781, 176);
            this.panel6.TabIndex = 19;
            // 
            // _btnMWLQuery
            // 
            this._btnMWLQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnMWLQuery.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("_btnMWLQuery.ImageOptions.Image")));
            this._btnMWLQuery.Location = new System.Drawing.Point(1547, 6);
            this._btnMWLQuery.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._btnMWLQuery.Name = "_btnMWLQuery";
            this._btnMWLQuery.Padding = new System.Windows.Forms.Padding(39, 38, 39, 38);
            this._btnMWLQuery.Size = new System.Drawing.Size(229, 157);
            this._btnMWLQuery.TabIndex = 11;
            this._btnMWLQuery.Click += new System.EventHandler(this._btnMWLQuery_Click);
            // 
            // _tLPQuery
            // 
            this._tLPQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tLPQuery.ColumnCount = 2;
            this._tLPQuery.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tLPQuery.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tLPQuery.Controls.Add(this.groupBox1, 0, 0);
            this._tLPQuery.Controls.Add(this.groupBox2, 1, 0);
            this._tLPQuery.Location = new System.Drawing.Point(3, 2);
            this._tLPQuery.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._tLPQuery.Name = "_tLPQuery";
            this._tLPQuery.RowCount = 1;
            this._tLPQuery.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tLPQuery.Size = new System.Drawing.Size(1538, 161);
            this._tLPQuery.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._cbMPPSINPROGRESS);
            this.groupBox1.Controls.Add(this._txtAccessionNumber);
            this.groupBox1.Controls.Add(this._txtPatientFirst);
            this.groupBox1.Controls.Add(this._txtPatientMiddle);
            this.groupBox1.Controls.Add(this._txtPatientLast);
            this.groupBox1.Controls.Add(this._txtPatientID);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this._lbPatientFirst);
            this.groupBox1.Controls.Add(this._lbPatientMiddle);
            this.groupBox1.Controls.Add(this._lbPatientLast);
            this.groupBox1.Controls.Add(this._lbPatientID);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 4);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(763, 153);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.Text = "Bệnh nhân";
            // 
            // _cbMPPSINPROGRESS
            // 
            this._cbMPPSINPROGRESS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._cbMPPSINPROGRESS.AutoSize = true;
            this._cbMPPSINPROGRESS.Location = new System.Drawing.Point(150, 108);
            this._cbMPPSINPROGRESS.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._cbMPPSINPROGRESS.Name = "_cbMPPSINPROGRESS";
            this._cbMPPSINPROGRESS.Size = new System.Drawing.Size(134, 22);
            this._cbMPPSINPROGRESS.TabIndex = 26;
            this._cbMPPSINPROGRESS.Text = "&IN PROGRESS";
            this._cbMPPSINPROGRESS.UseVisualStyleBackColor = true;
            // 
            // _txtAccessionNumber
            // 
            this._txtAccessionNumber.Location = new System.Drawing.Point(130, 70);
            this._txtAccessionNumber.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._txtAccessionNumber.Name = "_txtAccessionNumber";
            this._txtAccessionNumber.Size = new System.Drawing.Size(409, 24);
            this._txtAccessionNumber.TabIndex = 25;
            // 
            // _txtPatientFirst
            // 
            this._txtPatientFirst.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtPatientFirst.Location = new System.Drawing.Point(595, 34);
            this._txtPatientFirst.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._txtPatientFirst.Name = "_txtPatientFirst";
            this._txtPatientFirst.Size = new System.Drawing.Size(163, 24);
            this._txtPatientFirst.TabIndex = 24;
            // 
            // _txtPatientMiddle
            // 
            this._txtPatientMiddle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtPatientMiddle.Location = new System.Drawing.Point(847, 71);
            this._txtPatientMiddle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._txtPatientMiddle.Name = "_txtPatientMiddle";
            this._txtPatientMiddle.Size = new System.Drawing.Size(139, 24);
            this._txtPatientMiddle.TabIndex = 23;
            // 
            // _txtPatientLast
            // 
            this._txtPatientLast.Location = new System.Drawing.Point(595, 72);
            this._txtPatientLast.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._txtPatientLast.Name = "_txtPatientLast";
            this._txtPatientLast.Size = new System.Drawing.Size(24, 24);
            this._txtPatientLast.TabIndex = 22;
            // 
            // _txtPatientID
            // 
            this._txtPatientID.Location = new System.Drawing.Point(130, 34);
            this._txtPatientID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._txtPatientID.Name = "_txtPatientID";
            this._txtPatientID.Size = new System.Drawing.Size(409, 24);
            this._txtPatientID.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point(10, 110);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 18);
            this.label6.TabIndex = 19;
            this.label6.Text = "&Trạng thái MPPS:";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(12, 73);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 18);
            this.label5.TabIndex = 17;
            this.label5.Text = "&Mã phiếu chụp:";
            // 
            // _lbPatientFirst
            // 
            this._lbPatientFirst.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lbPatientFirst.Location = new System.Drawing.Point(559, 36);
            this._lbPatientFirst.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._lbPatientFirst.Name = "_lbPatientFirst";
            this._lbPatientFirst.Size = new System.Drawing.Size(29, 18);
            this._lbPatientFirst.TabIndex = 16;
            this._lbPatientFirst.Text = "&Tên:";
            // 
            // _lbPatientMiddle
            // 
            this._lbPatientMiddle.Location = new System.Drawing.Point(775, 76);
            this._lbPatientMiddle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._lbPatientMiddle.Name = "_lbPatientMiddle";
            this._lbPatientMiddle.Size = new System.Drawing.Size(62, 18);
            this._lbPatientMiddle.TabIndex = 14;
            this._lbPatientMiddle.Text = "&Tên đệm:";
            // 
            // _lbPatientLast
            // 
            this._lbPatientLast.Location = new System.Drawing.Point(562, 74);
            this._lbPatientLast.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._lbPatientLast.Name = "_lbPatientLast";
            this._lbPatientLast.Size = new System.Drawing.Size(28, 18);
            this._lbPatientLast.TabIndex = 12;
            this._lbPatientLast.Text = "&Họ :";
            // 
            // _lbPatientID
            // 
            this._lbPatientID.Location = new System.Drawing.Point(12, 37);
            this._lbPatientID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._lbPatientID.Name = "_lbPatientID";
            this._lbPatientID.Size = new System.Drawing.Size(97, 18);
            this._lbPatientID.TabIndex = 2;
            this._lbPatientID.Text = "&Mã bệnh nhân:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this._cbStartEnd);
            this.groupBox2.Controls.Add(this._cbbModality);
            this.groupBox2.Controls.Add(this._dTPEnd);
            this.groupBox2.Controls.Add(this._dTPStart);
            this.groupBox2.Controls.Add(this._txtAETitle);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Location = new System.Drawing.Point(772, 4);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(763, 152);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.Text = "Scheduled Procedure Step";
            // 
            // _cbStartEnd
            // 
            this._cbStartEnd.AutoSize = true;
            this._cbStartEnd.Location = new System.Drawing.Point(140, 76);
            this._cbStartEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._cbStartEnd.Name = "_cbStartEnd";
            this._cbStartEnd.Size = new System.Drawing.Size(18, 17);
            this._cbStartEnd.TabIndex = 31;
            this._cbStartEnd.UseVisualStyleBackColor = true;
            this._cbStartEnd.CheckedChanged += new System.EventHandler(this._cbStartEnd_CheckedChanged);
            // 
            // _cbbModality
            // 
            this._cbbModality.Location = new System.Drawing.Point(180, 107);
            this._cbbModality.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._cbbModality.Name = "_cbbModality";
            this._cbbModality.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._cbbModality.Size = new System.Drawing.Size(426, 24);
            this._cbbModality.TabIndex = 30;
            // 
            // _dTPEnd
            // 
            this._dTPEnd.EditValue = null;
            this._dTPEnd.Location = new System.Drawing.Point(451, 70);
            this._dTPEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._dTPEnd.Name = "_dTPEnd";
            this._dTPEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._dTPEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._dTPEnd.Size = new System.Drawing.Size(154, 24);
            this._dTPEnd.TabIndex = 29;
            // 
            // _dTPStart
            // 
            this._dTPStart.EditValue = null;
            this._dTPStart.Location = new System.Drawing.Point(226, 71);
            this._dTPStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._dTPStart.Name = "_dTPStart";
            this._dTPStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._dTPStart.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._dTPStart.Size = new System.Drawing.Size(168, 24);
            this._dTPStart.TabIndex = 28;
            // 
            // _txtAETitle
            // 
            this._txtAETitle.Location = new System.Drawing.Point(180, 32);
            this._txtAETitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._txtAETitle.Name = "_txtAETitle";
            this._txtAETitle.Size = new System.Drawing.Size(426, 24);
            this._txtAETitle.TabIndex = 27;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(413, 73);
            this.label14.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(22, 18);
            this.label14.TabIndex = 22;
            this.label14.Text = "To:";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(9, 110);
            this.label13.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(123, 18);
            this.label13.TabIndex = 19;
            this.label13.Text = "&Phương thức chụp:";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(9, 35);
            this.label12.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(97, 18);
            this.label12.TabIndex = 18;
            this.label12.Text = "&Tên máy chụp:";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(180, 74);
            this.label15.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(40, 18);
            this.label15.TabIndex = 19;
            this.label15.Text = "From:";
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(9, 73);
            this.label16.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(116, 18);
            this.label16.TabIndex = 17;
            this.label16.Text = "&Thời gian bắt đầu:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuAccount,
            this._menuSystem,
            this._menuTools,
            this._menuHelp});
            this.menuStrip1.Location = new System.Drawing.Point(2, 2);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1791, 35);
            this.menuStrip1.TabIndex = 34;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // _menuAccount
            // 
            this._menuAccount.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._tsmChangePassword,
            this._accountSeparator,
            this._tsmLogout});
            this._menuAccount.Image = global::STM.MediaToPACS.Main.Properties.Resources.customer_16x16;
            this._menuAccount.Name = "_menuAccount";
            this._menuAccount.Text = "Tài khoản";
            // 
            // _tsmChangePassword
            // 
            this._tsmChangePassword.Name = "_tsmChangePassword";
            this._tsmChangePassword.Size = new System.Drawing.Size(224, 28);
            this._tsmChangePassword.Text = "Đổi mật khẩu";
            this._tsmChangePassword.Click += new System.EventHandler(this._tsmChangePassword_Click);
            // 
            // _tsmLogout
            // 
            this._tsmLogout.Name = "_tsmLogout";
            this._tsmLogout.Text = "Đăng xuất";
            this._tsmLogout.Click += new System.EventHandler(this._tsmLogout_Click);
            // 
            // _menuSystem
            // 
            this._menuSystem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._tsmSetting,
            this._systemSeparator,
            this._menuCamera,
            this._menuWorklist});
            this._menuSystem.Image = global::STM.MediaToPACS.Main.Properties.Resources.properties_32x32;
            this._menuSystem.Name = "_menuSystem";
            this._menuSystem.Text = "Hệ thống";
            // 
            // _tsmSetting
            // 
            this._tsmSetting.Image = global::STM.MediaToPACS.Main.Properties.Resources.properties_32x32;
            this._tsmSetting.Name = "_tsmSetting";
            this._tsmSetting.Text = "Cấu hình hệ thống...";
            this._tsmSetting.Click += new System.EventHandler(this._tsmSetting_Click);
            // 
            // _menuCamera
            // 
            this._menuCamera.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._tsmCbbVideoCapture});
            this._menuCamera.Name = "_menuCamera";
            this._menuCamera.Text = "Nguồn camera";
            // 
            // _menuWorklist
            // 
            this._menuWorklist.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._tsmCbbWorklist});
            this._menuWorklist.Name = "_menuWorklist";
            this._menuWorklist.Text = "Máy chủ Worklist";
            // 
            // _menuTools
            // 
            this._menuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._tsmLog});
            this._menuTools.Name = "_menuTools";
            this._menuTools.Text = "Công cụ";
            // 
            // _tsmLog
            // 
            this._tsmLog.Image = global::STM.MediaToPACS.Main.Properties.Resources.notes_16x16;
            this._tsmLog.Name = "_tsmLog";
            this._tsmLog.Text = "Nhật ký hệ thống";
            this._tsmLog.Click += new System.EventHandler(this._tsmLog_Click);
            // 
            // _menuHelp
            // 
            this._menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._tsmToUse});
            this._menuHelp.Name = "_menuHelp";
            this._menuHelp.Text = "Trợ giúp";
            // 
            // _tsmToUse
            // 
            this._tsmToUse.Image = global::STM.MediaToPACS.Main.Properties.Resources.about_16x16;
            this._tsmToUse.Name = "_tsmToUse";
            this._tsmToUse.Text = "Hướng dẫn sử dụng";
            this._tsmToUse.Click += new System.EventHandler(this._tsmToUse_Click);
            // 
            // _tsmCbbVideoCapture
            // 
            this._tsmCbbVideoCapture.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._tsmCbbVideoCapture.Margin = new System.Windows.Forms.Padding(4);
            this._tsmCbbVideoCapture.Name = "_tsmCbbVideoCapture";
            this._tsmCbbVideoCapture.Size = new System.Drawing.Size(250, 31);
            this._tsmCbbVideoCapture.ToolTipText = "Chọn nguồn camera đầu vào";
            // 
            // _tsmCbbWorklist
            // 
            this._tsmCbbWorklist.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._tsmCbbWorklist.Margin = new System.Windows.Forms.Padding(4);
            this._tsmCbbWorklist.Name = "_tsmCbbWorklist";
            this._tsmCbbWorklist.Size = new System.Drawing.Size(121, 31);
            this._tsmCbbWorklist.ToolTipText = "Worklist";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1795, 796);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.IconOptions.Image = global::STM.MediaToPACS.Main.Properties.Resources.stm;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "STM-Media To PACS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gridControlChiDinh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._gridViewChiDinh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl13)).EndInit();
            this.panelControl13.ResumeLayout(false);
            this.panelControl13.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._cbPageSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl12)).EndInit();
            this.panelControl12.ResumeLayout(false);
            this.panelControl12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl11)).EndInit();
            this.panelControl11.ResumeLayout(false);
            this.panelControl11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._nudPage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl10)).EndInit();
            this.panelControl10.ResumeLayout(false);
            this.panelControl10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dtDateToRis.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dtDateToRis.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl9)).EndInit();
            this.panelControl9.ResumeLayout(false);
            this.panelControl9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dtDateFromRis.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dtDateFromRis.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl8)).EndInit();
            this.panelControl8.ResumeLayout(false);
            this.panelControl8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txMaCD.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl7)).EndInit();
            this.panelControl7.ResumeLayout(false);
            this.panelControl7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txPatientCodeRis.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).EndInit();
            this.panelControl6.ResumeLayout(false);
            this.panelControl6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txBSCDRis.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            this.panelControl5.ResumeLayout(false);
            this.panelControl5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txPatientNameRis.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl14)).EndInit();
            this.panelControl14.ResumeLayout(false);
            this.panelControl14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl21)).EndInit();
            this.panelControl21.ResumeLayout(false);
            this.panelControl21.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._dGVMWLItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel6)).EndInit();
            this.panel6.ResumeLayout(false);
            this._tLPQuery.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txtAccessionNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtPatientFirst.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtPatientMiddle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtPatientLast.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtPatientID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox2)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._cbbModality.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dTPEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dTPEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dTPStart.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dTPStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtAETitle.Properties)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panel1;
        private DevExpress.XtraEditors.PanelControl panel6;
        private System.Windows.Forms.TableLayoutPanel _tLPQuery;
        private DevExpress.XtraEditors.GroupControl groupBox1;
        private DevExpress.XtraEditors.LabelControl _lbPatientFirst;
        private DevExpress.XtraEditors.LabelControl _lbPatientMiddle;
        private DevExpress.XtraEditors.LabelControl _lbPatientLast;
        private DevExpress.XtraEditors.LabelControl _lbPatientID;
        private DevExpress.XtraEditors.GroupControl groupBox2;
        private DevExpress.XtraEditors.LabelControl label14;
        private DevExpress.XtraEditors.LabelControl label13;
        private DevExpress.XtraEditors.LabelControl label12;
        private DevExpress.XtraEditors.LabelControl label15;
        private DevExpress.XtraEditors.LabelControl label16;
        private DevExpress.XtraEditors.LabelControl label5;
        private DevExpress.XtraEditors.LabelControl label6;
        private DevExpress.XtraEditors.TextEdit _txtPatientFirst;
        private DevExpress.XtraEditors.TextEdit _txtPatientMiddle;
        private DevExpress.XtraEditors.TextEdit _txtPatientLast;
        private DevExpress.XtraEditors.TextEdit _txtPatientID;
        private CheckBox _cbMPPSINPROGRESS;
        private DevExpress.XtraEditors.TextEdit _txtAccessionNumber;
        private CheckBox _cbStartEnd;
        private DevExpress.XtraEditors.ComboBoxEdit _cbbModality;
        private DevExpress.XtraEditors.DateEdit _dTPEnd;
        private DevExpress.XtraEditors.DateEdit _dTPStart;
        private DevExpress.XtraEditors.TextEdit _txtAETitle;
        private DevExpress.XtraEditors.SimpleButton _btnMWLQuery;
        private DevExpress.XtraGrid.GridControl _dGVMWLItems;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn AccessionNumber;
        private DevExpress.XtraGrid.Columns.GridColumn PatientID;
        private DevExpress.XtraGrid.Columns.GridColumn PatientName;
        private DevExpress.XtraGrid.Columns.GridColumn BirthDate;
        private DevExpress.XtraGrid.Columns.GridColumn Gender;
        private DevExpress.XtraGrid.Columns.GridColumn RequestingPhysician;
        private DevExpress.XtraGrid.Columns.GridColumn ReferringPhysician;
        private DevExpress.XtraGrid.Columns.GridColumn PerformingPhysician;
        private DevExpress.XtraGrid.Columns.GridColumn ScheduledStartDate;
        private DevExpress.XtraGrid.Columns.GridColumn Modality;
        private DevExpress.XtraGrid.Columns.GridColumn ScheduledStationAE;
        private DevExpress.XtraGrid.Columns.GridColumn ScheduleProcedureStep;
        private DevExpress.XtraGrid.Columns.GridColumn RequestedProcedureID;
        private DevExpress.XtraGrid.Columns.GridColumn MPPS;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraGrid.GridControl _gridControlChiDinh;
        private DevExpress.XtraGrid.Views.Grid.GridView _gridViewChiDinh;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnTrangThaiPhieu;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnMaBenhNhan;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnTenBenhNhan;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnNgaySinh;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnGioiTinh;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnTenDichVu;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnId;
        private DevExpress.XtraEditors.ComboBoxEdit _cbPageSize;
        private DevExpress.XtraEditors.DateEdit _dtDateFromRis;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.SimpleButton _btnSearchRIS;
        private NumericUpDown _nudPage;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.TextEdit _txBSCDRis;
        private DevExpress.XtraEditors.TextEdit _txPatientNameRis;
        private DevExpress.XtraEditors.TextEdit _txMaCD;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.DateEdit _dtDateToRis;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnSoPhieuChiDinh;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.TextEdit _txPatientCodeRis;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private ComboBox _ccbModalities;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnMaChiDinh;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnTenBacSiChiDinh;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnThoigianthuchien;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.PanelControl panelControl12;
        private DevExpress.XtraEditors.PanelControl panelControl11;
        private DevExpress.XtraEditors.PanelControl panelControl10;
        private DevExpress.XtraEditors.PanelControl panelControl9;
        private DevExpress.XtraEditors.PanelControl panelControl8;
        private DevExpress.XtraEditors.PanelControl panelControl7;
        private DevExpress.XtraEditors.PanelControl panelControl6;
        private DevExpress.XtraEditors.PanelControl panelControl5;
        private DevExpress.XtraEditors.PanelControl panelControl13;
        private DevExpress.XtraEditors.PanelControl panelControl14;
        private DevExpress.XtraEditors.PanelControl panelControl21;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraEditors.LabelControl _lbSLCaChup;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel _tSSLUserName;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripStatusLabel _tssNguoiDung;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private ComboBox _cbbTrangThai;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem _menuAccount;
        private ToolStripSeparator _accountSeparator;
        private ToolStripMenuItem _tsmLogout;
        private ToolStripMenuItem _menuSystem;
        private ToolStripSeparator _systemSeparator;
        private ToolStripMenuItem _menuCamera;
        private ToolStripMenuItem _menuWorklist;
        private ToolStripMenuItem _menuTools;
        private ToolStripMenuItem _menuHelp;
        private ToolStripMenuItem _tsmSetting;
        private ToolStripMenuItem _tsmLog;
        private ToolStripMenuItem _tsmToUse;
        private ToolStripComboBox _tsmCbbVideoCapture;
        private ToolStripComboBox _tsmCbbWorklist;
        private ToolStripMenuItem _tsmChangePassword;
    }
}
