namespace STM.MediaToPACS.Main.UI
{
    partial class FrmDoctorConfirm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnSystemSettings = new System.Windows.Forms.Button();
            this.lblHeaderTitle = new DevExpress.XtraEditors.LabelControl();
            this.lblHeaderSubtitle = new DevExpress.XtraEditors.LabelControl();
            this.panelBody = new DevExpress.XtraEditors.PanelControl();
            this.cboKhoa = new DevExpress.XtraEditors.LookUpEdit();
            this.btnReloadKhoa = new System.Windows.Forms.Button();
            this.lblKhoaError = new DevExpress.XtraEditors.LabelControl();
            this.lblCapKhoa = new DevExpress.XtraEditors.LabelControl();
            this.separator = new System.Windows.Forms.Panel();
            this.lblEmailValue = new DevExpress.XtraEditors.LabelControl();
            this.lblCapEmail = new DevExpress.XtraEditors.LabelControl();
            this.lblDoctorCodeValue = new DevExpress.XtraEditors.LabelControl();
            this.lblCapCode = new DevExpress.XtraEditors.LabelControl();
            this.lblDoctorNameValue = new DevExpress.XtraEditors.LabelControl();
            this.lblCapName = new DevExpress.XtraEditors.LabelControl();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.btnContinue = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelBody)).BeginInit();
            this.panelBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboKhoa.Properties)).BeginInit();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            //
            // panelHeader
            //
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(72)))), ((int)(((byte)(116)))));
            this.panelHeader.Controls.Add(this.lblHeaderTitle);
            this.panelHeader.Controls.Add(this.lblHeaderSubtitle);
            this.panelHeader.Controls.Add(this.btnSystemSettings);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1120, 104);
            this.panelHeader.TabIndex = 0;
            // 
            // btnSystemSettings
            //
            this.btnSystemSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(91)))), ((int)(((byte)(139)))));
            this.btnSystemSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSystemSettings.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(145)))), ((int)(((byte)(181)))));
            this.btnSystemSettings.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(52)))), ((int)(((byte)(86)))));
            this.btnSystemSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(107)))), ((int)(((byte)(157)))));
            this.btnSystemSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSystemSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnSystemSettings.ForeColor = System.Drawing.Color.White;
            this.btnSystemSettings.Location = new System.Drawing.Point(24, 24);
            this.btnSystemSettings.Name = "btnSystemSettings";
            this.btnSystemSettings.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.btnSystemSettings.Size = new System.Drawing.Size(132, 42);
            this.btnSystemSettings.TabIndex = 0;
            this.btnSystemSettings.Text = "Cài đặt";
            this.btnSystemSettings.UseVisualStyleBackColor = false;
            this.btnSystemSettings.Click += new System.EventHandler(this.btnSystemSettings_Click);
            //
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblHeaderTitle.Appearance.ForeColor = System.Drawing.Color.White;
            this.lblHeaderTitle.Appearance.Options.UseFont = true;
            this.lblHeaderTitle.Appearance.Options.UseForeColor = true;
            this.lblHeaderTitle.Appearance.Options.UseTextOptions = true;
            this.lblHeaderTitle.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.lblHeaderTitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblHeaderTitle.Location = new System.Drawing.Point(184, 18);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(900, 28);
            this.lblHeaderTitle.TabIndex = 1;
            this.lblHeaderTitle.Text = "BỆNH VIỆN QUÂN Y 120";
            // 
            // lblHeaderSubtitle
            // 
            this.lblHeaderSubtitle.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblHeaderSubtitle.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(230)))), ((int)(((byte)(242)))));
            this.lblHeaderSubtitle.Appearance.Options.UseFont = true;
            this.lblHeaderSubtitle.Appearance.Options.UseForeColor = true;
            this.lblHeaderSubtitle.Appearance.Options.UseTextOptions = true;
            this.lblHeaderSubtitle.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.lblHeaderSubtitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblHeaderSubtitle.Location = new System.Drawing.Point(185, 52);
            this.lblHeaderSubtitle.Name = "lblHeaderSubtitle";
            this.lblHeaderSubtitle.Size = new System.Drawing.Size(899, 22);
            this.lblHeaderSubtitle.TabIndex = 2;
            this.lblHeaderSubtitle.Text = "Xác nhận thông tin bác sĩ trước khi tiếp tục";
            // 
            // panelBody
            // 
            this.panelBody.Appearance.BackColor = System.Drawing.Color.White;
            this.panelBody.Appearance.Options.UseBackColor = true;
            this.panelBody.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelBody.Controls.Add(this.lblKhoaError);
            this.panelBody.Controls.Add(this.btnReloadKhoa);
            this.panelBody.Controls.Add(this.cboKhoa);
            this.panelBody.Controls.Add(this.lblCapKhoa);
            this.panelBody.Controls.Add(this.separator);
            this.panelBody.Controls.Add(this.lblEmailValue);
            this.panelBody.Controls.Add(this.lblCapEmail);
            this.panelBody.Controls.Add(this.lblDoctorCodeValue);
            this.panelBody.Controls.Add(this.lblCapCode);
            this.panelBody.Controls.Add(this.lblDoctorNameValue);
            this.panelBody.Controls.Add(this.lblCapName);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 104);
            this.panelBody.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelBody.Name = "panelBody";
            this.panelBody.Size = new System.Drawing.Size(1120, 506);
            this.panelBody.TabIndex = 1;
            // 
            // cboKhoa
            // 
            this.cboKhoa.Location = new System.Drawing.Point(246, 290);
            this.cboKhoa.Name = "cboKhoa";
            this.cboKhoa.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboKhoa.Properties.Appearance.Options.UseFont = true;
            this.cboKhoa.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboKhoa.Properties.AppearanceDropDown.Options.UseFont = true;
            this.cboKhoa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboKhoa.Properties.NullText = "-- Chọn khoa --";
            this.cboKhoa.Size = new System.Drawing.Size(587, 28);
            this.cboKhoa.TabIndex = 8;
            // 
            // btnReloadKhoa
            // 
            this.btnReloadKhoa.BackColor = System.Drawing.Color.White;
            this.btnReloadKhoa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReloadKhoa.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(207)))), ((int)(((byte)(216)))));
            this.btnReloadKhoa.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(235)))), ((int)(((byte)(241)))));
            this.btnReloadKhoa.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.btnReloadKhoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReloadKhoa.Image = global::STM.MediaToPACS.Main.Properties.Resources.refresh_16x16;
            this.btnReloadKhoa.Location = new System.Drawing.Point(841, 290);
            this.btnReloadKhoa.Name = "btnReloadKhoa";
            this.btnReloadKhoa.Size = new System.Drawing.Size(32, 28);
            this.btnReloadKhoa.TabIndex = 9;
            this.btnReloadKhoa.UseVisualStyleBackColor = false;
            this.btnReloadKhoa.Click += new System.EventHandler(this.btnReloadKhoa_Click);
            // 
            // lblKhoaError
            // 
            this.lblKhoaError.Appearance.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblKhoaError.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblKhoaError.Appearance.Options.UseFont = true;
            this.lblKhoaError.Appearance.Options.UseForeColor = true;
            this.lblKhoaError.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblKhoaError.Location = new System.Drawing.Point(246, 326);
            this.lblKhoaError.Name = "lblKhoaError";
            this.lblKhoaError.Size = new System.Drawing.Size(627, 40);
            this.lblKhoaError.TabIndex = 10;
            this.lblKhoaError.Text = "";
            this.lblKhoaError.Visible = false;
            // 
            // lblCapKhoa
            // 
            this.lblCapKhoa.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCapKhoa.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(100)))), ((int)(((byte)(110)))));
            this.lblCapKhoa.Appearance.Options.UseFont = true;
            this.lblCapKhoa.Appearance.Options.UseForeColor = true;
            this.lblCapKhoa.Appearance.Options.UseTextOptions = true;
            this.lblCapKhoa.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblCapKhoa.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblCapKhoa.Location = new System.Drawing.Point(104, 293);
            this.lblCapKhoa.Name = "lblCapKhoa";
            this.lblCapKhoa.Size = new System.Drawing.Size(120, 24);
            this.lblCapKhoa.TabIndex = 7;
            this.lblCapKhoa.Text = "Khoa:";
            // 
            // separator
            // 
            this.separator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(231)))), ((int)(((byte)(236)))));
            this.separator.Location = new System.Drawing.Point(104, 250);
            this.separator.Name = "separator";
            this.separator.Size = new System.Drawing.Size(769, 1);
            this.separator.TabIndex = 6;
            // 
            // lblEmailValue
            // 
            this.lblEmailValue.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblEmailValue.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblEmailValue.Appearance.Options.UseFont = true;
            this.lblEmailValue.Appearance.Options.UseForeColor = true;
            this.lblEmailValue.Appearance.Options.UseTextOptions = true;
            this.lblEmailValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.lblEmailValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblEmailValue.Location = new System.Drawing.Point(246, 179);
            this.lblEmailValue.Name = "lblEmailValue";
            this.lblEmailValue.Size = new System.Drawing.Size(627, 24);
            this.lblEmailValue.TabIndex = 5;
            this.lblEmailValue.Text = "-";
            // 
            // lblCapEmail
            // 
            this.lblCapEmail.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCapEmail.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(100)))), ((int)(((byte)(110)))));
            this.lblCapEmail.Appearance.Options.UseFont = true;
            this.lblCapEmail.Appearance.Options.UseForeColor = true;
            this.lblCapEmail.Appearance.Options.UseTextOptions = true;
            this.lblCapEmail.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblCapEmail.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblCapEmail.Location = new System.Drawing.Point(104, 179);
            this.lblCapEmail.Name = "lblCapEmail";
            this.lblCapEmail.Size = new System.Drawing.Size(120, 24);
            this.lblCapEmail.TabIndex = 4;
            this.lblCapEmail.Text = "Email:";
            // 
            // lblDoctorCodeValue
            // 
            this.lblDoctorCodeValue.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblDoctorCodeValue.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblDoctorCodeValue.Appearance.Options.UseFont = true;
            this.lblDoctorCodeValue.Appearance.Options.UseForeColor = true;
            this.lblDoctorCodeValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDoctorCodeValue.Location = new System.Drawing.Point(246, 124);
            this.lblDoctorCodeValue.Name = "lblDoctorCodeValue";
            this.lblDoctorCodeValue.Size = new System.Drawing.Size(627, 24);
            this.lblDoctorCodeValue.TabIndex = 3;
            this.lblDoctorCodeValue.Text = "-";
            // 
            // lblCapCode
            // 
            this.lblCapCode.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCapCode.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(100)))), ((int)(((byte)(110)))));
            this.lblCapCode.Appearance.Options.UseFont = true;
            this.lblCapCode.Appearance.Options.UseForeColor = true;
            this.lblCapCode.Appearance.Options.UseTextOptions = true;
            this.lblCapCode.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblCapCode.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblCapCode.Location = new System.Drawing.Point(104, 124);
            this.lblCapCode.Name = "lblCapCode";
            this.lblCapCode.Size = new System.Drawing.Size(120, 24);
            this.lblCapCode.TabIndex = 2;
            this.lblCapCode.Text = "Mã bác sĩ:";
            // 
            // lblDoctorNameValue
            // 
            this.lblDoctorNameValue.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblDoctorNameValue.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.lblDoctorNameValue.Appearance.Options.UseFont = true;
            this.lblDoctorNameValue.Appearance.Options.UseForeColor = true;
            this.lblDoctorNameValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDoctorNameValue.Location = new System.Drawing.Point(246, 69);
            this.lblDoctorNameValue.Name = "lblDoctorNameValue";
            this.lblDoctorNameValue.Size = new System.Drawing.Size(627, 26);
            this.lblDoctorNameValue.TabIndex = 1;
            this.lblDoctorNameValue.Text = "-";
            // 
            // lblCapName
            // 
            this.lblCapName.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCapName.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(100)))), ((int)(((byte)(110)))));
            this.lblCapName.Appearance.Options.UseFont = true;
            this.lblCapName.Appearance.Options.UseForeColor = true;
            this.lblCapName.Appearance.Options.UseTextOptions = true;
            this.lblCapName.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblCapName.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblCapName.Location = new System.Drawing.Point(104, 70);
            this.lblCapName.Name = "lblCapName";
            this.lblCapName.Size = new System.Drawing.Size(120, 24);
            this.lblCapName.TabIndex = 0;
            this.lblCapName.Text = "Họ và tên:";
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(249)))), ((int)(((byte)(251)))));
            this.panelFooter.Controls.Add(this.btnContinue);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 610);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(1120, 70);
            this.panelFooter.TabIndex = 2;
            // 
            // btnContinue
            // 
            this.btnContinue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(72)))), ((int)(((byte)(116)))));
            this.btnContinue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnContinue.FlatAppearance.BorderSize = 0;
            this.btnContinue.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(52)))), ((int)(((byte)(86)))));
            this.btnContinue.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(88)))), ((int)(((byte)(140)))));
            this.btnContinue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContinue.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnContinue.ForeColor = System.Drawing.Color.White;
            this.btnContinue.Location = new System.Drawing.Point(946, 16);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(127, 38);
            this.btnContinue.TabIndex = 0;
            this.btnContinue.Text = "Tiếp tục";
            this.btnContinue.UseVisualStyleBackColor = false;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // FrmDoctorConfirm
            // 
            this.AcceptButton = this.btnContinue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1120, 680);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDoctorConfirm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Xác nhận thông tin";
            this.panelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelBody)).EndInit();
            this.panelBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboKhoa.Properties)).EndInit();
            this.panelFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Button btnSystemSettings;
        private DevExpress.XtraEditors.LabelControl lblHeaderTitle;
        private DevExpress.XtraEditors.LabelControl lblHeaderSubtitle;
        private DevExpress.XtraEditors.PanelControl panelBody;
        private DevExpress.XtraEditors.LabelControl lblCapName;
        private DevExpress.XtraEditors.LabelControl lblDoctorNameValue;
        private DevExpress.XtraEditors.LabelControl lblCapCode;
        private DevExpress.XtraEditors.LabelControl lblDoctorCodeValue;
        private DevExpress.XtraEditors.LabelControl lblCapEmail;
        private DevExpress.XtraEditors.LabelControl lblEmailValue;
        private System.Windows.Forms.Panel separator;
        private DevExpress.XtraEditors.LabelControl lblCapKhoa;
        private DevExpress.XtraEditors.LookUpEdit cboKhoa;
        private System.Windows.Forms.Button btnReloadKhoa;
        private DevExpress.XtraEditors.LabelControl lblKhoaError;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Button btnContinue;
    }
}
