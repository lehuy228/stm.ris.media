namespace PrintToPACSDemo.UI.Login
{
    partial class QRCodeAuthentication
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.pictureBoxQRCode = new DevExpress.XtraEditors.PictureEdit();
            this.textBoxVertify = new DevExpress.XtraEditors.TextEdit();
            this.buttonVertify = new DevExpress.XtraEditors.SimpleButton();
            this.buttonCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxVertify.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(9, 37);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(341, 13);
            this.labelControl1.TabIndex = 10;
            this.labelControl1.Text = "Quét mã QR bên dưới bằng ứng dụng xác thực Microsoft Authenticator";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(73, 11);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(211, 22);
            this.labelControl2.TabIndex = 11;
            this.labelControl2.Text = "Xác thực OTP Microsoft";
            // 
            // pictureBoxQRCode
            // 
            this.pictureBoxQRCode.EditValue = global::PrintToPACSDemo.Properties.Resources.logo;
            this.pictureBoxQRCode.Location = new System.Drawing.Point(93, 70);
            this.pictureBoxQRCode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBoxQRCode.Name = "pictureBoxQRCode";
            this.pictureBoxQRCode.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxQRCode.Properties.Appearance.Options.UseBackColor = true;
            this.pictureBoxQRCode.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureBoxQRCode.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            this.pictureBoxQRCode.Size = new System.Drawing.Size(171, 186);
            this.pictureBoxQRCode.TabIndex = 12;
            // 
            // textBoxVertify
            // 
            this.textBoxVertify.Location = new System.Drawing.Point(14, 301);
            this.textBoxVertify.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxVertify.Name = "textBoxVertify";
            this.textBoxVertify.Size = new System.Drawing.Size(159, 20);
            this.textBoxVertify.TabIndex = 13;
            // 
            // buttonVertify
            // 
            this.buttonVertify.Location = new System.Drawing.Point(178, 300);
            this.buttonVertify.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonVertify.Name = "buttonVertify";
            this.buttonVertify.Size = new System.Drawing.Size(86, 21);
            this.buttonVertify.TabIndex = 14;
            this.buttonVertify.Text = "Xác thực";
            this.buttonVertify.Click += new System.EventHandler(this.buttonVertify_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(269, 300);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(76, 21);
            this.buttonCancel.TabIndex = 15;
            this.buttonCancel.Text = "Đóng";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // QRCodeAuthentication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 342);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonVertify);
            this.Controls.Add(this.textBoxVertify);
            this.Controls.Add(this.pictureBoxQRCode);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.IconOptions.Image = global::PrintToPACSDemo.Properties.Resources.logo;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "QRCodeAuthentication";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QRCodeAuthentication";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxVertify.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.PictureEdit pictureBoxQRCode;
        private DevExpress.XtraEditors.TextEdit textBoxVertify;
        private DevExpress.XtraEditors.SimpleButton buttonVertify;
        private DevExpress.XtraEditors.SimpleButton buttonCancel;
    }
}