using System.Windows.Forms;

namespace PrintToPACSDemo.UI
{
    partial class WorkListTable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkListTable));
            this.panel1 = new DevExpress.XtraEditors.PanelControl();
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
            this.panel4 = new DevExpress.XtraEditors.PanelControl();
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this._btnSettings = new DevExpress.XtraEditors.SimpleButton();
            this._cbMWLServers = new DevExpress.XtraEditors.ComboBoxEdit();
            this._btnLogs = new DevExpress.XtraEditors.SimpleButton();
            this._cbCapture = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label1 = new DevExpress.XtraEditors.LabelControl();
            this._btnUse = new DevExpress.XtraEditors.SimpleButton();
            this.label2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dGVMWLItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel4)).BeginInit();
            this.panel4.SuspendLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._cbMWLServers.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._cbCapture.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._dGVMWLItems);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1275, 594);
            this.panel1.TabIndex = 0;
            // 
            // _dGVMWLItems
            // 
            this._dGVMWLItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dGVMWLItems.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._dGVMWLItems.Location = new System.Drawing.Point(2, 206);
            this._dGVMWLItems.MainView = this.gridView1;
            this._dGVMWLItems.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._dGVMWLItems.Name = "_dGVMWLItems";
            this._dGVMWLItems.Size = new System.Drawing.Size(1271, 386);
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
            this.gridView1.DetailHeight = 375;
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
            this.AccessionNumber.Name = "AccessionNumber";
            this.AccessionNumber.Visible = true;
            this.AccessionNumber.VisibleIndex = 0;
            // 
            // PatientID
            // 
            this.PatientID.Caption = "Mã bệnh nhân";
            this.PatientID.FieldName = "PatientID";
            this.PatientID.Name = "PatientID";
            this.PatientID.Visible = true;
            this.PatientID.VisibleIndex = 1;
            // 
            // PatientName
            // 
            this.PatientName.Caption = "Tên bệnh nhân";
            this.PatientName.FieldName = "PatientName";
            this.PatientName.Name = "PatientName";
            this.PatientName.Visible = true;
            this.PatientName.VisibleIndex = 2;
            // 
            // BirthDate
            // 
            this.BirthDate.Caption = "Ngày sinh";
            this.BirthDate.FieldName = "BirthDate";
            this.BirthDate.Name = "BirthDate";
            this.BirthDate.Visible = true;
            this.BirthDate.VisibleIndex = 3;
            // 
            // Gender
            // 
            this.Gender.Caption = "Giới tính";
            this.Gender.FieldName = "Gender";
            this.Gender.Name = "Gender";
            this.Gender.Visible = true;
            this.Gender.VisibleIndex = 4;
            // 
            // RequestingPhysician
            // 
            this.RequestingPhysician.Caption = "Bác sĩ chỉ định";
            this.RequestingPhysician.FieldName = "RequestingPhysician";
            this.RequestingPhysician.Name = "RequestingPhysician";
            this.RequestingPhysician.Visible = true;
            this.RequestingPhysician.VisibleIndex = 5;
            // 
            // ReferringPhysician
            // 
            this.ReferringPhysician.Caption = "Bác sĩ chẩn đoán";
            this.ReferringPhysician.FieldName = "ReferringPhysician";
            this.ReferringPhysician.Name = "ReferringPhysician";
            this.ReferringPhysician.Visible = true;
            this.ReferringPhysician.VisibleIndex = 6;
            // 
            // PerformingPhysician
            // 
            this.PerformingPhysician.Caption = "Bác sĩ chụp";
            this.PerformingPhysician.FieldName = "PerformingPhysician";
            this.PerformingPhysician.Name = "PerformingPhysician";
            this.PerformingPhysician.Visible = true;
            this.PerformingPhysician.VisibleIndex = 7;
            // 
            // ScheduledStartDate
            // 
            this.ScheduledStartDate.Caption = "Scheduled Start Date";
            this.ScheduledStartDate.FieldName = "ScheduledStartDate";
            this.ScheduledStartDate.Name = "ScheduledStartDate";
            this.ScheduledStartDate.Visible = true;
            this.ScheduledStartDate.VisibleIndex = 8;
            // 
            // Modality
            // 
            this.Modality.Caption = "Phương thức chụp";
            this.Modality.FieldName = "Modality";
            this.Modality.Name = "Modality";
            this.Modality.Visible = true;
            this.Modality.VisibleIndex = 9;
            // 
            // ScheduledStationAE
            // 
            this.ScheduledStationAE.Caption = "Scheduled Station AE";
            this.ScheduledStationAE.FieldName = "ScheduledStationAE";
            this.ScheduledStationAE.Name = "ScheduledStationAE";
            this.ScheduledStationAE.Visible = true;
            this.ScheduledStationAE.VisibleIndex = 10;
            // 
            // ScheduleProcedureStep
            // 
            this.ScheduleProcedureStep.Caption = "Scheduled Procedure Step";
            this.ScheduleProcedureStep.FieldName = "ScheduleProcedureStep";
            this.ScheduleProcedureStep.Name = "ScheduleProcedureStep";
            this.ScheduleProcedureStep.Visible = true;
            this.ScheduleProcedureStep.VisibleIndex = 11;
            // 
            // RequestedProcedureID
            // 
            this.RequestedProcedureID.Caption = "Requested Procedure ID";
            this.RequestedProcedureID.FieldName = "RequestedProcedureID";
            this.RequestedProcedureID.Name = "RequestedProcedureID";
            this.RequestedProcedureID.Visible = true;
            this.RequestedProcedureID.VisibleIndex = 12;
            // 
            // MPPS
            // 
            this.MPPS.Caption = "MPPS";
            this.MPPS.FieldName = "MPPS";
            this.MPPS.Name = "MPPS";
            this.MPPS.Visible = true;
            this.MPPS.VisibleIndex = 13;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.panelControl1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(2, 2);
            this.panel4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1271, 204);
            this.panel4.TabIndex = 21;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
            this.panel6.Controls.Add(this._btnMWLQuery);
            this.panel6.Controls.Add(this._tLPQuery);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(2, 51);
            this.panel6.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1267, 150);
            this.panel6.TabIndex = 19;
            // 
            // _btnMWLQuery
            // 
            this._btnMWLQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnMWLQuery.ImageOptions.Image = global::PrintToPACSDemo.Properties.Resources.loupe;
            this._btnMWLQuery.Location = new System.Drawing.Point(1087, 2);
            this._btnMWLQuery.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._btnMWLQuery.Name = "_btnMWLQuery";
            this._btnMWLQuery.Padding = new System.Windows.Forms.Padding(30, 32, 30, 32);
            this._btnMWLQuery.Size = new System.Drawing.Size(177, 134);
            this._btnMWLQuery.TabIndex = 11;
            this._btnMWLQuery.Text = "Tìm kiếm";
            this._btnMWLQuery.Click += new System.EventHandler(this._btnMWLQuery_Click);
            // 
            // _tLPQuery
            // 
            this._tLPQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tLPQuery.ColumnCount = 2;
            this._tLPQuery.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.60748F));
            this._tLPQuery.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.39252F));
            this._tLPQuery.Controls.Add(this.groupBox1, 0, 0);
            this._tLPQuery.Controls.Add(this.groupBox2, 1, 0);
            this._tLPQuery.Location = new System.Drawing.Point(2, 2);
            this._tLPQuery.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._tLPQuery.Name = "_tLPQuery";
            this._tLPQuery.RowCount = 1;
            this._tLPQuery.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tLPQuery.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tLPQuery.Size = new System.Drawing.Size(1078, 134);
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
            this.groupBox1.Location = new System.Drawing.Point(2, 3);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(595, 128);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.Text = "Bệnh nhân";
            // 
            // _cbMPPSINPROGRESS
            // 
            this._cbMPPSINPROGRESS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._cbMPPSINPROGRESS.AutoSize = true;
            this._cbMPPSINPROGRESS.Location = new System.Drawing.Point(117, 90);
            this._cbMPPSINPROGRESS.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._cbMPPSINPROGRESS.Name = "_cbMPPSINPROGRESS";
            this._cbMPPSINPROGRESS.Size = new System.Drawing.Size(109, 19);
            this._cbMPPSINPROGRESS.TabIndex = 26;
            this._cbMPPSINPROGRESS.Text = "&IN PROGRESS";
            this._cbMPPSINPROGRESS.UseVisualStyleBackColor = true;
            // 
            // _txtAccessionNumber
            // 
            this._txtAccessionNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtAccessionNumber.Location = new System.Drawing.Point(421, 28);
            this._txtAccessionNumber.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._txtAccessionNumber.Name = "_txtAccessionNumber";
            this._txtAccessionNumber.Size = new System.Drawing.Size(170, 22);
            this._txtAccessionNumber.TabIndex = 25;
            // 
            // _txtPatientFirst
            // 
            this._txtPatientFirst.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtPatientFirst.Location = new System.Drawing.Point(445, 59);
            this._txtPatientFirst.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._txtPatientFirst.Name = "_txtPatientFirst";
            this._txtPatientFirst.Size = new System.Drawing.Size(146, 22);
            this._txtPatientFirst.TabIndex = 24;
            // 
            // _txtPatientMiddle
            // 
            this._txtPatientMiddle.Location = new System.Drawing.Point(252, 59);
            this._txtPatientMiddle.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._txtPatientMiddle.Name = "_txtPatientMiddle";
            this._txtPatientMiddle.Size = new System.Drawing.Size(141, 22);
            this._txtPatientMiddle.TabIndex = 23;
            // 
            // _txtPatientLast
            // 
            this._txtPatientLast.Location = new System.Drawing.Point(42, 59);
            this._txtPatientLast.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._txtPatientLast.Name = "_txtPatientLast";
            this._txtPatientLast.Size = new System.Drawing.Size(140, 22);
            this._txtPatientLast.TabIndex = 22;
            // 
            // _txtPatientID
            // 
            this._txtPatientID.Location = new System.Drawing.Point(101, 28);
            this._txtPatientID.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._txtPatientID.Name = "_txtPatientID";
            this._txtPatientID.Size = new System.Drawing.Size(199, 22);
            this._txtPatientID.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point(8, 92);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 15);
            this.label6.TabIndex = 19;
            this.label6.Text = "&Trạng thái MPPS:";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(320, 30);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 15);
            this.label5.TabIndex = 17;
            this.label5.Text = "&Mã phiếu chụp:";
            // 
            // _lbPatientFirst
            // 
            this._lbPatientFirst.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lbPatientFirst.Location = new System.Drawing.Point(410, 62);
            this._lbPatientFirst.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._lbPatientFirst.Name = "_lbPatientFirst";
            this._lbPatientFirst.Size = new System.Drawing.Size(24, 15);
            this._lbPatientFirst.TabIndex = 16;
            this._lbPatientFirst.Text = "&Tên:";
            // 
            // _lbPatientMiddle
            // 
            this._lbPatientMiddle.Location = new System.Drawing.Point(196, 62);
            this._lbPatientMiddle.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._lbPatientMiddle.Name = "_lbPatientMiddle";
            this._lbPatientMiddle.Size = new System.Drawing.Size(52, 15);
            this._lbPatientMiddle.TabIndex = 14;
            this._lbPatientMiddle.Text = "&Tên đệm:";
            // 
            // _lbPatientLast
            // 
            this._lbPatientLast.Location = new System.Drawing.Point(9, 62);
            this._lbPatientLast.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._lbPatientLast.Name = "_lbPatientLast";
            this._lbPatientLast.Size = new System.Drawing.Size(22, 15);
            this._lbPatientLast.TabIndex = 12;
            this._lbPatientLast.Text = "&Họ :";
            // 
            // _lbPatientID
            // 
            this._lbPatientID.Location = new System.Drawing.Point(9, 31);
            this._lbPatientID.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._lbPatientID.Name = "_lbPatientID";
            this._lbPatientID.Size = new System.Drawing.Size(83, 15);
            this._lbPatientID.TabIndex = 2;
            this._lbPatientID.Text = "&Mã bệnh nhân:";
            // 
            // groupBox2
            // 
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
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(601, 3);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(475, 128);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.Text = "Scheduled Procedure Step";
            // 
            // _cbStartEnd
            // 
            this._cbStartEnd.AutoSize = true;
            this._cbStartEnd.Location = new System.Drawing.Point(109, 63);
            this._cbStartEnd.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._cbStartEnd.Name = "_cbStartEnd";
            this._cbStartEnd.Size = new System.Drawing.Size(15, 14);
            this._cbStartEnd.TabIndex = 31;
            this._cbStartEnd.UseVisualStyleBackColor = true;
            this._cbStartEnd.CheckedChanged += new System.EventHandler(this._cbStartEnd_CheckedChanged);
            // 
            // _cbbModality
            // 
            this._cbbModality.Location = new System.Drawing.Point(140, 89);
            this._cbbModality.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._cbbModality.Name = "_cbbModality";
            this._cbbModality.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._cbbModality.Size = new System.Drawing.Size(331, 22);
            this._cbbModality.TabIndex = 30;
            // 
            // _dTPEnd
            // 
            this._dTPEnd.EditValue = null;
            this._dTPEnd.Location = new System.Drawing.Point(351, 58);
            this._dTPEnd.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._dTPEnd.Name = "_dTPEnd";
            this._dTPEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._dTPEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._dTPEnd.Size = new System.Drawing.Size(120, 22);
            this._dTPEnd.TabIndex = 29;
            // 
            // _dTPStart
            // 
            this._dTPStart.EditValue = null;
            this._dTPStart.Location = new System.Drawing.Point(176, 59);
            this._dTPStart.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._dTPStart.Name = "_dTPStart";
            this._dTPStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._dTPStart.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._dTPStart.Size = new System.Drawing.Size(131, 22);
            this._dTPStart.TabIndex = 28;
            // 
            // _txtAETitle
            // 
            this._txtAETitle.Location = new System.Drawing.Point(140, 27);
            this._txtAETitle.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._txtAETitle.Name = "_txtAETitle";
            this._txtAETitle.Size = new System.Drawing.Size(331, 22);
            this._txtAETitle.TabIndex = 27;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(321, 61);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(17, 15);
            this.label14.TabIndex = 22;
            this.label14.Text = "To:";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(7, 92);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(102, 15);
            this.label13.TabIndex = 19;
            this.label13.Text = "&Phương thức chụp:";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(7, 29);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 15);
            this.label12.TabIndex = 18;
            this.label12.Text = "&Tên máy chụp:";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(140, 62);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(32, 15);
            this.label15.TabIndex = 19;
            this.label15.Text = "From:";
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(7, 61);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(98, 15);
            this.label16.TabIndex = 17;
            this.label16.Text = "&Thời gian bắt đầu:";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this._btnSettings);
            this.panelControl1.Controls.Add(this._cbMWLServers);
            this.panelControl1.Controls.Add(this._btnLogs);
            this.panelControl1.Controls.Add(this._cbCapture);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this._btnUse);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(2, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1267, 49);
            this.panelControl1.TabIndex = 33;
            // 
            // _btnSettings
            // 
            this._btnSettings.ImageOptions.Image = global::PrintToPACSDemo.Properties.Resources.cogwheel;
            this._btnSettings.Location = new System.Drawing.Point(7, 8);
            this._btnSettings.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._btnSettings.Name = "_btnSettings";
            this._btnSettings.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this._btnSettings.Size = new System.Drawing.Size(93, 29);
            this._btnSettings.TabIndex = 21;
            this._btnSettings.Text = "Cài đặt";
            this._btnSettings.Click += new System.EventHandler(this._btnSettings_Click);
            // 
            // _cbMWLServers
            // 
            this._cbMWLServers.Location = new System.Drawing.Point(889, 13);
            this._cbMWLServers.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._cbMWLServers.Name = "_cbMWLServers";
            this._cbMWLServers.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._cbMWLServers.Size = new System.Drawing.Size(260, 22);
            this._cbMWLServers.TabIndex = 32;
            // 
            // _btnLogs
            // 
            this._btnLogs.ImageOptions.Image = global::PrintToPACSDemo.Properties.Resources.log;
            this._btnLogs.Location = new System.Drawing.Point(105, 8);
            this._btnLogs.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._btnLogs.Name = "_btnLogs";
            this._btnLogs.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this._btnLogs.Size = new System.Drawing.Size(93, 29);
            this._btnLogs.TabIndex = 22;
            this._btnLogs.Text = "Logs";
            this._btnLogs.Click += new System.EventHandler(this._btnLogs_Click);
            // 
            // _cbCapture
            // 
            this._cbCapture.Location = new System.Drawing.Point(499, 13);
            this._cbCapture.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._cbCapture.Name = "_cbCapture";
            this._cbCapture.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._cbCapture.Size = new System.Drawing.Size(259, 22);
            this._cbCapture.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(779, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 15);
            this.label1.TabIndex = 14;
            this.label1.Text = "Máy chủ Worklist";
            // 
            // _btnUse
            // 
            this._btnUse.ImageOptions.Image = global::PrintToPACSDemo.Properties.Resources.cogwheel;
            this._btnUse.Location = new System.Drawing.Point(202, 8);
            this._btnUse.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._btnUse.Name = "_btnUse";
            this._btnUse.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this._btnUse.Size = new System.Drawing.Size(177, 29);
            this._btnUse.TabIndex = 23;
            this._btnUse.Text = "Hướng dẫn sử dụng";
            // 
            // label2
            // 
            this.label2.Appearance.Options.UseFont = true;
            this.label2.Location = new System.Drawing.Point(397, 15);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 15);
            this.label2.TabIndex = 17;
            this.label2.Text = "Danh sách thiết bị";
            // 
            // WorkListTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1275, 594);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("WorkListTable.IconOptions.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "WorkListTable";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chương trình chuyển đổi video DICOM ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WorkListTable_FormClosing);
            this.Load += new System.EventHandler(this.WorkListTable_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._dGVMWLItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel4)).EndInit();
            this.panel4.ResumeLayout(false);
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
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._cbMWLServers.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._cbCapture.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panel1;
        private DevExpress.XtraEditors.PanelControl panel4;
        private DevExpress.XtraEditors.LabelControl label1;
        private DevExpress.XtraEditors.LabelControl label2;
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
        private DevExpress.XtraEditors.SimpleButton _btnUse;
        private DevExpress.XtraEditors.SimpleButton _btnLogs;
        private DevExpress.XtraEditors.SimpleButton _btnSettings;
        private DevExpress.XtraEditors.TextEdit _txtPatientFirst;
        private DevExpress.XtraEditors.TextEdit _txtPatientMiddle;
        private DevExpress.XtraEditors.TextEdit _txtPatientLast;
        private DevExpress.XtraEditors.TextEdit _txtPatientID;
        private DevExpress.XtraEditors.ComboBoxEdit _cbMWLServers;
        private DevExpress.XtraEditors.ComboBoxEdit _cbCapture;
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
        private DevExpress.XtraEditors.PanelControl panelControl1;
    }
}