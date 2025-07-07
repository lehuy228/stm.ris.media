namespace PrintToPACSDemo.UI.Login
{
    partial class ResetPassword
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._btnResetPasswod = new DevExpress.XtraEditors.SimpleButton();
            this._btnLogin = new DevExpress.XtraEditors.SimpleButton();
            this._cbShowPass = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this._txtConfirmPassword = new DevExpress.XtraEditors.TextEdit();
            this._txtNewPassword = new DevExpress.XtraEditors.TextEdit();
            this._txtPasswordCurrent = new DevExpress.XtraEditors.TextEdit();
            this._txtUsername = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this._cbShowPass.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtConfirmPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtNewPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtPasswordCurrent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtUsername.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // _btnResetPasswod
            // 
            this._btnResetPasswod.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this._btnResetPasswod.Appearance.Options.UseFont = true;
            this._btnResetPasswod.Location = new System.Drawing.Point(10, 132);
            this._btnResetPasswod.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this._btnResetPasswod.Name = "_btnResetPasswod";
            this._btnResetPasswod.Size = new System.Drawing.Size(147, 22);
            this._btnResetPasswod.TabIndex = 35;
            this._btnResetPasswod.Text = "ĐỔI MẬT KHẨU";
            this._btnResetPasswod.Click += new System.EventHandler(this._btnResetPasswod_Click);
            // 
            // _btnLogin
            // 
            this._btnLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnLogin.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this._btnLogin.Appearance.ForeColor = System.Drawing.Color.Gray;
            this._btnLogin.Appearance.Options.UseFont = true;
            this._btnLogin.Appearance.Options.UseForeColor = true;
            this._btnLogin.Location = new System.Drawing.Point(171, 132);
            this._btnLogin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this._btnLogin.Name = "_btnLogin";
            this._btnLogin.Size = new System.Drawing.Size(149, 22);
            this._btnLogin.TabIndex = 34;
            this._btnLogin.Text = "ĐĂNG NHẬP";
            this._btnLogin.Click += new System.EventHandler(this._btnLogin_Click);
            // 
            // _cbShowPass
            // 
            this._cbShowPass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._cbShowPass.Location = new System.Drawing.Point(230, 108);
            this._cbShowPass.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this._cbShowPass.Name = "_cbShowPass";
            this._cbShowPass.Properties.Caption = "Hiện mật khẩu";
            this._cbShowPass.Size = new System.Drawing.Size(90, 19);
            this._cbShowPass.TabIndex = 33;
            this._cbShowPass.Click += new System.EventHandler(this.checkBoxShowPass_CheckedChanged);
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(10, 86);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(104, 13);
            this.labelControl5.TabIndex = 32;
            this.labelControl5.Text = "Nhập lại mật khẩu mới";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(10, 63);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(63, 13);
            this.labelControl4.TabIndex = 31;
            this.labelControl4.Text = "Mật khẩu mới";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(10, 38);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(82, 13);
            this.labelControl3.TabIndex = 30;
            this.labelControl3.Text = "Mật khẩu hiện tại";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(10, 14);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(83, 13);
            this.labelControl2.TabIndex = 29;
            this.labelControl2.Text = "Tài khoản cần đổi";
            // 
            // _txtConfirmPassword
            // 
            this._txtConfirmPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._txtConfirmPassword.Location = new System.Drawing.Point(126, 84);
            this._txtConfirmPassword.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this._txtConfirmPassword.Name = "_txtConfirmPassword";
            this._txtConfirmPassword.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this._txtConfirmPassword.Properties.Appearance.Options.UseBackColor = true;
            this._txtConfirmPassword.Properties.PasswordChar = '*';
            this._txtConfirmPassword.Size = new System.Drawing.Size(195, 20);
            this._txtConfirmPassword.TabIndex = 28;
            // 
            // _txtNewPassword
            // 
            this._txtNewPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._txtNewPassword.Location = new System.Drawing.Point(126, 59);
            this._txtNewPassword.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this._txtNewPassword.Name = "_txtNewPassword";
            this._txtNewPassword.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this._txtNewPassword.Properties.Appearance.Options.UseBackColor = true;
            this._txtNewPassword.Properties.PasswordChar = '*';
            this._txtNewPassword.Size = new System.Drawing.Size(195, 20);
            this._txtNewPassword.TabIndex = 27;
            // 
            // _txtPasswordCurrent
            // 
            this._txtPasswordCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._txtPasswordCurrent.Location = new System.Drawing.Point(126, 35);
            this._txtPasswordCurrent.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this._txtPasswordCurrent.Name = "_txtPasswordCurrent";
            this._txtPasswordCurrent.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this._txtPasswordCurrent.Properties.Appearance.Options.UseBackColor = true;
            this._txtPasswordCurrent.Properties.PasswordChar = '*';
            this._txtPasswordCurrent.Size = new System.Drawing.Size(195, 20);
            this._txtPasswordCurrent.TabIndex = 26;
            // 
            // _txtUsername
            // 
            this._txtUsername.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._txtUsername.Location = new System.Drawing.Point(126, 11);
            this._txtUsername.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this._txtUsername.Name = "_txtUsername";
            this._txtUsername.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this._txtUsername.Properties.Appearance.Options.UseBackColor = true;
            this._txtUsername.Size = new System.Drawing.Size(195, 20);
            this._txtUsername.TabIndex = 25;
            // 
            // ResetPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._btnResetPasswod);
            this.Controls.Add(this._btnLogin);
            this.Controls.Add(this._cbShowPass);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this._txtConfirmPassword);
            this.Controls.Add(this._txtNewPassword);
            this.Controls.Add(this._txtPasswordCurrent);
            this.Controls.Add(this._txtUsername);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ResetPassword";
            this.Size = new System.Drawing.Size(334, 176);
            ((System.ComponentModel.ISupportInitialize)(this._cbShowPass.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtConfirmPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtNewPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtPasswordCurrent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtUsername.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton _btnResetPasswod;
        private DevExpress.XtraEditors.SimpleButton _btnLogin;
        private DevExpress.XtraEditors.CheckEdit _cbShowPass;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit _txtConfirmPassword;
        private DevExpress.XtraEditors.TextEdit _txtNewPassword;
        private DevExpress.XtraEditors.TextEdit _txtPasswordCurrent;
        private DevExpress.XtraEditors.TextEdit _txtUsername;
    }
}
