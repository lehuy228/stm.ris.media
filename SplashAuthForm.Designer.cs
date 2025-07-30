
namespace PrintToPACSDemo
{
    partial class SplashAuthForm
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
            this._btnLogin = new DevExpress.XtraEditors.SimpleButton();
            this._progressPanel = new DevExpress.XtraWaitForm.ProgressPanel();
            this._btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // _btnLogin
            // 
            this._btnLogin.Location = new System.Drawing.Point(242, 192);
            this._btnLogin.Name = "_btnLogin";
            this._btnLogin.Size = new System.Drawing.Size(123, 36);
            this._btnLogin.TabIndex = 0;
            this._btnLogin.Text = "Xác thực";
            this._btnLogin.Click += new System.EventHandler(this._btnLogin_Click);
            // 
            // _progressPanel
            // 
            this._progressPanel.Appearance.BackColor = System.Drawing.Color.Transparent;
            this._progressPanel.Appearance.Options.UseBackColor = true;
            this._progressPanel.Location = new System.Drawing.Point(264, 234);
            this._progressPanel.Name = "_progressPanel";
            this._progressPanel.Size = new System.Drawing.Size(219, 59);
            this._progressPanel.TabIndex = 2;
            this._progressPanel.Text = "Xác thực";
            // 
            // _btnCancel
            // 
            this._btnCancel.Location = new System.Drawing.Point(379, 192);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(123, 36);
            this._btnCancel.TabIndex = 3;
            this._btnCancel.Text = "Hủy";
            this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
            // 
            // SplashAuthForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._progressPanel);
            this.Controls.Add(this._btnLogin);
            this.Name = "SplashAuthForm";
            this.Text = "SplashAuthForm";
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton _btnLogin;
        private DevExpress.XtraWaitForm.ProgressPanel _progressPanel;
        private DevExpress.XtraEditors.SimpleButton _btnCancel;
    }
}