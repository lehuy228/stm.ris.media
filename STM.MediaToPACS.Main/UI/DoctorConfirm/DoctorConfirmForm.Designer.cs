namespace STM.MediaToPACS.Main.UI
{
    partial class DoctorConfirmForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.rootPanel = new DevExpress.XtraEditors.PanelControl();
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.leftPanel = new DevExpress.XtraEditors.PanelControl();
            this.brandPanel = new System.Windows.Forms.Panel();
            this.lblHeaderTitle = new DevExpress.XtraEditors.LabelControl();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.rightPanel = new DevExpress.XtraEditors.PanelControl();
            this.lblRightTitle = new DevExpress.XtraEditors.LabelControl();
            this.lblRightSubtitle = new DevExpress.XtraEditors.LabelControl();
            this.btnSystemSettings = new DevExpress.XtraEditors.SimpleButton();
            this.detailPanel = new DevExpress.XtraEditors.PanelControl();
            this.detailLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblCapCode = new DevExpress.XtraEditors.LabelControl();
            this.lblDoctorCodeValue = new DevExpress.XtraEditors.LabelControl();
            this.lblCapName = new DevExpress.XtraEditors.LabelControl();
            this.lblDoctorNameValue = new DevExpress.XtraEditors.LabelControl();
            this.lblCapKhoa = new DevExpress.XtraEditors.LabelControl();
            this.khoaLayout = new System.Windows.Forms.TableLayoutPanel();
            this.cboKhoa = new DevExpress.XtraEditors.LookUpEdit();
            this.btnReloadKhoa = new DevExpress.XtraEditors.SimpleButton();
            this.lblKhoaError = new DevExpress.XtraEditors.LabelControl();
            this.btnContinue = new DevExpress.XtraEditors.SimpleButton();
            this.lblHeaderSubtitle = new DevExpress.XtraEditors.LabelControl();
            this.lblCapEmail = new DevExpress.XtraEditors.LabelControl();
            this.lblEmailValue = new DevExpress.XtraEditors.LabelControl();
            this.separator = new System.Windows.Forms.Panel();
            this.panelFooter = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.rootPanel)).BeginInit();
            this.rootPanel.SuspendLayout();
            this.mainLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.leftPanel)).BeginInit();
            this.leftPanel.SuspendLayout();
            this.brandPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightPanel)).BeginInit();
            this.rightPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.detailPanel)).BeginInit();
            this.detailPanel.SuspendLayout();
            this.detailLayout.SuspendLayout();
            this.khoaLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboKhoa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelFooter)).BeginInit();
            this.SuspendLayout();
            // 
            // rootPanel
            // 
            this.rootPanel.Appearance.BackColor = System.Drawing.Color.White;
            this.rootPanel.Appearance.Options.UseBackColor = true;
            this.rootPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.rootPanel.Controls.Add(this.mainLayout);
            this.rootPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rootPanel.Location = new System.Drawing.Point(0, 0);
            this.rootPanel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.rootPanel.Name = "rootPanel";
            this.rootPanel.Size = new System.Drawing.Size(800, 480);
            this.rootPanel.TabIndex = 0;
            // 
            // mainLayout
            // 
            this.mainLayout.ColumnCount = 2;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52F));
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48F));
            this.mainLayout.Controls.Add(this.leftPanel, 0, 0);
            this.mainLayout.Controls.Add(this.rightPanel, 1, 0);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(2, 2);
            this.mainLayout.Margin = new System.Windows.Forms.Padding(0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.RowCount = 1;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Size = new System.Drawing.Size(796, 476);
            this.mainLayout.TabIndex = 0;
            // 
            // leftPanel
            // 
            this.leftPanel.Appearance.BackColor = System.Drawing.Color.White;
            this.leftPanel.Appearance.Options.UseBackColor = true;
            this.leftPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.leftPanel.Controls.Add(this.separator);
            this.leftPanel.Controls.Add(this.brandPanel);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftPanel.Location = new System.Drawing.Point(0, 0);
            this.leftPanel.Margin = new System.Windows.Forms.Padding(0);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Padding = new System.Windows.Forms.Padding(28);
            this.leftPanel.Size = new System.Drawing.Size(413, 476);
            this.leftPanel.TabIndex = 0;
            // 
            // brandPanel
            // 
            this.brandPanel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.brandPanel.Controls.Add(this.lblHeaderSubtitle);
            this.brandPanel.Controls.Add(this.lblHeaderTitle);
            this.brandPanel.Controls.Add(this.picLogo);
            this.brandPanel.Location = new System.Drawing.Point(24, 180);
            this.brandPanel.Name = "brandPanel";
            this.brandPanel.Size = new System.Drawing.Size(360, 110);
            this.brandPanel.TabIndex = 0;
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblHeaderTitle.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(72)))), ((int)(((byte)(116)))));
            this.lblHeaderTitle.Appearance.Options.UseFont = true;
            this.lblHeaderTitle.Appearance.Options.UseForeColor = true;
            this.lblHeaderTitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblHeaderTitle.Location = new System.Drawing.Point(88, 24);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(268, 30);
            this.lblHeaderTitle.TabIndex = 1;
            this.lblHeaderTitle.Text = "Bệnh Viện Quân Y 120 - Quân Khu 9";
            // 
            // picLogo
            // 
            this.picLogo.Image = global::STM.MediaToPACS.Main.Properties.Resources.logo120;
            this.picLogo.Location = new System.Drawing.Point(0, 16);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(72, 72);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 0;
            this.picLogo.TabStop = false;
            // 
            // rightPanel
            // 
            this.rightPanel.Appearance.BackColor = System.Drawing.Color.White;
            this.rightPanel.Appearance.Options.UseBackColor = true;
            this.rightPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rightPanel.Controls.Add(this.lblRightSubtitle);
            this.rightPanel.Controls.Add(this.lblRightTitle);
            this.rightPanel.Controls.Add(this.btnSystemSettings);
            this.rightPanel.Controls.Add(this.detailPanel);
            this.rightPanel.Controls.Add(this.btnContinue);
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightPanel.Location = new System.Drawing.Point(413, 0);
            this.rightPanel.Margin = new System.Windows.Forms.Padding(0);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Padding = new System.Windows.Forms.Padding(26, 24, 26, 24);
            this.rightPanel.Size = new System.Drawing.Size(383, 476);
            this.rightPanel.TabIndex = 1;
            // 
            // lblRightTitle
            // 
            this.lblRightTitle.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.lblRightTitle.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.lblRightTitle.Appearance.Options.UseFont = true;
            this.lblRightTitle.Appearance.Options.UseForeColor = true;
            this.lblRightTitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblRightTitle.Location = new System.Drawing.Point(18, 28);
            this.lblRightTitle.Name = "lblRightTitle";
            this.lblRightTitle.Size = new System.Drawing.Size(200, 28);
            this.lblRightTitle.TabIndex = 3;
            this.lblRightTitle.Text = "Xác nhận phiên làm việc";
            // 
            // lblRightSubtitle
            // 
            this.lblRightSubtitle.Appearance.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblRightSubtitle.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(100)))), ((int)(((byte)(110)))));
            this.lblRightSubtitle.Appearance.Options.UseFont = true;
            this.lblRightSubtitle.Appearance.Options.UseForeColor = true;
            this.lblRightSubtitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblRightSubtitle.Location = new System.Drawing.Point(19, 56);
            this.lblRightSubtitle.Name = "lblRightSubtitle";
            this.lblRightSubtitle.Size = new System.Drawing.Size(224, 20);
            this.lblRightSubtitle.TabIndex = 4;
            this.lblRightSubtitle.Text = "Kiểm tra bác sĩ và khoa trước khi tiếp tục";
            // 
            // btnSystemSettings
            // 
            this.btnSystemSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSystemSettings.AllowFocus = false;
            this.btnSystemSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSystemSettings.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnSystemSettings.Appearance.Options.UseFont = true;
            this.btnSystemSettings.ImageOptions.Image = global::STM.MediaToPACS.Main.Properties.Resources.properties_16x16;
            this.btnSystemSettings.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnSystemSettings.Location = new System.Drawing.Point(260, 31);
            this.btnSystemSettings.Name = "btnSystemSettings";
            this.btnSystemSettings.Size = new System.Drawing.Size(98, 30);
            this.btnSystemSettings.TabIndex = 0;
            this.btnSystemSettings.Text = "Cài đặt";
            this.btnSystemSettings.Click += new System.EventHandler(this.btnSystemSettings_Click);
            // 
            // detailPanel
            // 
            this.detailPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.detailPanel.Appearance.BackColor = System.Drawing.Color.White;
            this.detailPanel.Appearance.Options.UseBackColor = true;
            this.detailPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.detailPanel.Controls.Add(this.lblCapEmail);
            this.detailPanel.Controls.Add(this.detailLayout);
            this.detailPanel.Location = new System.Drawing.Point(18, 116);
            this.detailPanel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.detailPanel.Name = "detailPanel";
            this.detailPanel.Padding = new System.Windows.Forms.Padding(0);
            this.detailPanel.Size = new System.Drawing.Size(340, 234);
            this.detailPanel.TabIndex = 1;
            // 
            // detailLayout
            // 
            this.detailLayout.ColumnCount = 1;
            this.detailLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.detailLayout.Controls.Add(this.lblCapCode, 0, 0);
            this.detailLayout.Controls.Add(this.lblDoctorCodeValue, 0, 1);
            this.detailLayout.Controls.Add(this.lblCapName, 0, 2);
            this.detailLayout.Controls.Add(this.lblDoctorNameValue, 0, 3);
            this.detailLayout.Controls.Add(this.lblCapKhoa, 0, 4);
            this.detailLayout.Controls.Add(this.khoaLayout, 0, 5);
            this.detailLayout.Controls.Add(this.lblKhoaError, 0, 6);
            this.detailLayout.Dock = System.Windows.Forms.DockStyle.Top;
            this.detailLayout.Location = new System.Drawing.Point(0, 42);
            this.detailLayout.Name = "detailLayout";
            this.detailLayout.RowCount = 7;
            this.detailLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.detailLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.detailLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.detailLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.detailLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.detailLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.detailLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.detailLayout.Size = new System.Drawing.Size(340, 192);
            this.detailLayout.TabIndex = 0;
            // 
            // lblCapCode
            // 
            this.lblCapCode.Appearance.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCapCode.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(100)))), ((int)(((byte)(110)))));
            this.lblCapCode.Appearance.Options.UseFont = true;
            this.lblCapCode.Appearance.Options.UseForeColor = true;
            this.lblCapCode.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblCapCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCapCode.TabIndex = 0;
            this.lblCapCode.Text = "Mã bác sĩ";
            // 
            // lblDoctorCodeValue
            // 
            this.lblDoctorCodeValue.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblDoctorCodeValue.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.lblDoctorCodeValue.Appearance.Options.UseFont = true;
            this.lblDoctorCodeValue.Appearance.Options.UseForeColor = true;
            this.lblDoctorCodeValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDoctorCodeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDoctorCodeValue.TabIndex = 1;
            this.lblDoctorCodeValue.Text = "-";
            // 
            // lblCapName
            // 
            this.lblCapName.Appearance.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCapName.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(100)))), ((int)(((byte)(110)))));
            this.lblCapName.Appearance.Options.UseFont = true;
            this.lblCapName.Appearance.Options.UseForeColor = true;
            this.lblCapName.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblCapName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCapName.TabIndex = 2;
            this.lblCapName.Text = "Tên bác sĩ";
            // 
            // lblDoctorNameValue
            // 
            this.lblDoctorNameValue.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblDoctorNameValue.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.lblDoctorNameValue.Appearance.Options.UseFont = true;
            this.lblDoctorNameValue.Appearance.Options.UseForeColor = true;
            this.lblDoctorNameValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDoctorNameValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDoctorNameValue.TabIndex = 3;
            this.lblDoctorNameValue.Text = "-";
            // 
            // lblCapKhoa
            // 
            this.lblCapKhoa.Appearance.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCapKhoa.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(100)))), ((int)(((byte)(110)))));
            this.lblCapKhoa.Appearance.Options.UseFont = true;
            this.lblCapKhoa.Appearance.Options.UseForeColor = true;
            this.lblCapKhoa.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblCapKhoa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCapKhoa.TabIndex = 4;
            this.lblCapKhoa.Text = "Chọn khoa";
            // 
            // khoaLayout
            // 
            this.khoaLayout.ColumnCount = 2;
            this.khoaLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.khoaLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.khoaLayout.Controls.Add(this.cboKhoa, 0, 0);
            this.khoaLayout.Controls.Add(this.btnReloadKhoa, 1, 0);
            this.khoaLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.khoaLayout.Location = new System.Drawing.Point(0, 122);
            this.khoaLayout.Margin = new System.Windows.Forms.Padding(0);
            this.khoaLayout.Name = "khoaLayout";
            this.khoaLayout.RowCount = 1;
            this.khoaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.khoaLayout.Size = new System.Drawing.Size(340, 38);
            this.khoaLayout.TabIndex = 5;
            // 
            // cboKhoa
            // 
            this.cboKhoa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboKhoa.Location = new System.Drawing.Point(0, 5);
            this.cboKhoa.Margin = new System.Windows.Forms.Padding(0, 5, 8, 5);
            this.cboKhoa.Name = "cboKhoa";
            this.cboKhoa.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboKhoa.Properties.Appearance.Options.UseFont = true;
            this.cboKhoa.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboKhoa.Properties.AppearanceDropDown.Options.UseFont = true;
            this.cboKhoa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboKhoa.Properties.NullText = "-- Chọn khoa --";
            this.cboKhoa.Size = new System.Drawing.Size(294, 24);
            this.cboKhoa.TabIndex = 0;
            // 
            // btnReloadKhoa
            // 
            this.btnReloadKhoa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReloadKhoa.AllowFocus = false;
            this.btnReloadKhoa.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.btnReloadKhoa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReloadKhoa.ImageOptions.Image = global::STM.MediaToPACS.Main.Properties.Resources.refresh_16x16;
            this.btnReloadKhoa.Location = new System.Drawing.Point(302, 4);
            this.btnReloadKhoa.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.btnReloadKhoa.Name = "btnReloadKhoa";
            this.btnReloadKhoa.Size = new System.Drawing.Size(38, 30);
            this.btnReloadKhoa.TabIndex = 1;
            this.btnReloadKhoa.Click += new System.EventHandler(this.btnReloadKhoa_Click);
            // 
            // lblKhoaError
            // 
            this.lblKhoaError.Appearance.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblKhoaError.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblKhoaError.Appearance.Options.UseFont = true;
            this.lblKhoaError.Appearance.Options.UseForeColor = true;
            this.lblKhoaError.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblKhoaError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblKhoaError.Location = new System.Drawing.Point(0, 160);
            this.lblKhoaError.Margin = new System.Windows.Forms.Padding(0);
            this.lblKhoaError.Name = "lblKhoaError";
            this.lblKhoaError.Size = new System.Drawing.Size(340, 42);
            this.lblKhoaError.TabIndex = 6;
            this.lblKhoaError.Visible = false;
            // 
            // btnContinue
            // 
            this.btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnContinue.AllowFocus = false;
            this.btnContinue.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnContinue.Appearance.Options.UseFont = true;
            this.btnContinue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnContinue.Location = new System.Drawing.Point(252, 414);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(106, 36);
            this.btnContinue.TabIndex = 2;
            this.btnContinue.Text = "Tiếp tục";
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // lblHeaderSubtitle
            // 
            this.lblHeaderSubtitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblHeaderSubtitle.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblHeaderSubtitle.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(100)))), ((int)(((byte)(110)))));
            this.lblHeaderSubtitle.Appearance.Options.UseFont = true;
            this.lblHeaderSubtitle.Appearance.Options.UseForeColor = true;
            this.lblHeaderSubtitle.Location = new System.Drawing.Point(89, 55);
            this.lblHeaderSubtitle.Name = "lblHeaderSubtitle";
            this.lblHeaderSubtitle.Size = new System.Drawing.Size(220, 24);
            this.lblHeaderSubtitle.TabIndex = 2;
            this.lblHeaderSubtitle.Text = "Stm MediaToPacs - v1.1";
            // 
            // lblCapEmail
            // 
            this.lblCapEmail.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblCapEmail.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblCapEmail.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(72)))), ((int)(((byte)(116)))));
            this.lblCapEmail.Appearance.Options.UseFont = true;
            this.lblCapEmail.Appearance.Options.UseForeColor = true;
            this.lblCapEmail.Location = new System.Drawing.Point(0, 0);
            this.lblCapEmail.Name = "lblCapEmail";
            this.lblCapEmail.Size = new System.Drawing.Size(260, 26);
            this.lblCapEmail.TabIndex = 1;
            this.lblCapEmail.Text = "Thông tin xác nhận";
            // 
            // lblEmailValue
            // 
            this.lblEmailValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblEmailValue.Location = new System.Drawing.Point(0, 0);
            this.lblEmailValue.Name = "lblEmailValue";
            this.lblEmailValue.Size = new System.Drawing.Size(0, 0);
            this.lblEmailValue.TabIndex = 0;
            this.lblEmailValue.Visible = false;
            // 
            // separator
            // 
            this.separator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(231)))), ((int)(((byte)(236)))));
            this.separator.Dock = System.Windows.Forms.DockStyle.Right;
            this.separator.Location = new System.Drawing.Point(412, 28);
            this.separator.Name = "separator";
            this.separator.Size = new System.Drawing.Size(1, 420);
            this.separator.TabIndex = 0;
            // 
            // panelFooter
            // 
            this.panelFooter.Location = new System.Drawing.Point(0, 0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(0, 0);
            this.panelFooter.TabIndex = 0;
            this.panelFooter.Visible = false;
            // 
            // DoctorConfirmForm
            // 
            this.AcceptButton = this.btnContinue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.rootPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 480);
            this.Name = "DoctorConfirmForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Xác nhận thông tin";
            ((System.ComponentModel.ISupportInitialize)(this.rootPanel)).EndInit();
            this.rootPanel.ResumeLayout(false);
            this.mainLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.leftPanel)).EndInit();
            this.leftPanel.ResumeLayout(false);
            this.brandPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightPanel)).EndInit();
            this.rightPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.detailPanel)).EndInit();
            this.detailPanel.ResumeLayout(false);
            this.detailLayout.ResumeLayout(false);
            this.khoaLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboKhoa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelFooter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl rootPanel;
        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private DevExpress.XtraEditors.PanelControl leftPanel;
        private System.Windows.Forms.Panel brandPanel;
        private System.Windows.Forms.PictureBox picLogo;
        private DevExpress.XtraEditors.PanelControl rightPanel;
        private DevExpress.XtraEditors.LabelControl lblRightTitle;
        private DevExpress.XtraEditors.LabelControl lblRightSubtitle;
        private DevExpress.XtraEditors.PanelControl detailPanel;
        private System.Windows.Forms.TableLayoutPanel detailLayout;
        private System.Windows.Forms.TableLayoutPanel khoaLayout;
        private DevExpress.XtraEditors.SimpleButton btnSystemSettings;
        private DevExpress.XtraEditors.LabelControl lblHeaderTitle;
        private DevExpress.XtraEditors.LabelControl lblHeaderSubtitle;
        private DevExpress.XtraEditors.LabelControl lblCapName;
        private DevExpress.XtraEditors.LabelControl lblDoctorNameValue;
        private DevExpress.XtraEditors.LabelControl lblCapCode;
        private DevExpress.XtraEditors.LabelControl lblDoctorCodeValue;
        private DevExpress.XtraEditors.LabelControl lblCapEmail;
        private DevExpress.XtraEditors.LabelControl lblEmailValue;
        private System.Windows.Forms.Panel separator;
        private DevExpress.XtraEditors.LabelControl lblCapKhoa;
        private DevExpress.XtraEditors.LookUpEdit cboKhoa;
        private DevExpress.XtraEditors.SimpleButton btnReloadKhoa;
        private DevExpress.XtraEditors.LabelControl lblKhoaError;
        private DevExpress.XtraEditors.PanelControl panelFooter;
        private DevExpress.XtraEditors.SimpleButton btnContinue;
    }
}
