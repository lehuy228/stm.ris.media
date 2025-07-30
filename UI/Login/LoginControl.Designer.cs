using System.Windows.Forms;

namespace PrintToPACSDemo.UI.Login
{
    partial class LoginControl
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
            this._lbChangePassword = new DevExpress.XtraEditors.LabelControl();
            this._btnLogin = new DevExpress.XtraEditors.SimpleButton();
            this._cbShowPass = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this._txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this._txtUsername = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this._cbShowPass.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtUsername.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // _lbChangePassword
            // 
            this._lbChangePassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._lbChangePassword.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this._lbChangePassword.Appearance.ForeColor = System.Drawing.Color.SteelBlue;
            this._lbChangePassword.Appearance.Options.UseFont = true;
            this._lbChangePassword.Appearance.Options.UseForeColor = true;
            this._lbChangePassword.Location = new System.Drawing.Point(282, 148);
            this._lbChangePassword.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this._lbChangePassword.Name = "_lbChangePassword";
            this._lbChangePassword.Size = new System.Drawing.Size(107, 18);
            this._lbChangePassword.TabIndex = 18;
            this._lbChangePassword.Text = "Đổi mật khẩu?";
            this._lbChangePassword.Click += new System.EventHandler(this._lbChangePassword_Click);
            // 
            // _btnLogin
            // 
            this._btnLogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._btnLogin.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this._btnLogin.Appearance.Options.UseFont = true;
            this._btnLogin.Location = new System.Drawing.Point(15, 113);
            this._btnLogin.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this._btnLogin.Name = "_btnLogin";
            this._btnLogin.Size = new System.Drawing.Size(381, 28);
            this._btnLogin.TabIndex = 17;
            this._btnLogin.Text = "ĐĂNG NHẬP";
            this._btnLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // _cbShowPass
            // 
            this._cbShowPass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._cbShowPass.Location = new System.Drawing.Point(256, 82);
            this._cbShowPass.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this._cbShowPass.Name = "_cbShowPass";
            this._cbShowPass.Properties.Caption = "Hiện mật khẩu";
            this._cbShowPass.Size = new System.Drawing.Size(139, 22);
            this._cbShowPass.TabIndex = 16;
            this._cbShowPass.Click += new System.EventHandler(this.checkboxShowPass_CheckedChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(19, 58);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(68, 18);
            this.labelControl2.TabIndex = 15;
            this.labelControl2.Text = "Mật khẩu";
            // 
            // _txtPassword
            // 
            this._txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtPassword.EditValue = "";
            this._txtPassword.Location = new System.Drawing.Point(110, 55);
            this._txtPassword.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this._txtPassword.Name = "_txtPassword";
            this._txtPassword.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this._txtPassword.Properties.Appearance.Options.UseBackColor = true;
            this._txtPassword.Properties.PasswordChar = '*';
            this._txtPassword.Size = new System.Drawing.Size(286, 24);
            this._txtPassword.TabIndex = 14;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(19, 17);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 18);
            this.labelControl1.TabIndex = 13;
            this.labelControl1.Text = "Tài khoản";
            // 
            // _txtUsername
            // 
            this._txtUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtUsername.EditValue = "";
            this._txtUsername.Location = new System.Drawing.Point(110, 14);
            this._txtUsername.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this._txtUsername.Name = "_txtUsername";
            this._txtUsername.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this._txtUsername.Properties.Appearance.Options.UseBackColor = true;
            this._txtUsername.Size = new System.Drawing.Size(286, 24);
            this._txtUsername.TabIndex = 12;
            // 
            // LoginControl
            // 
            this.Appearance.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(165)))), ((int)(((byte)(169)))));
            this.Appearance.Options.UseFont = true;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._lbChangePassword);
            this.Controls.Add(this._btnLogin);
            this.Controls.Add(this._cbShowPass);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this._txtPassword);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this._txtUsername);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "LoginControl";
            this.Size = new System.Drawing.Size(414, 172);
            ((System.ComponentModel.ISupportInitialize)(this._cbShowPass.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtUsername.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl _lbChangePassword;
        private DevExpress.XtraEditors.SimpleButton _btnLogin;
        private DevExpress.XtraEditors.CheckEdit _cbShowPass;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit _txtPassword;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit _txtUsername;
    }
}
