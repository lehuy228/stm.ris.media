namespace PrintToPACSDemo.UI.Conclusion
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxVertify = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonVertify = new System.Windows.Forms.Button();
            this.pictureBoxQRCode = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRCode)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(121, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(293, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "OTP Microsoft Anthenticate";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 50);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 12, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(452, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Scan the QR code below using an authenticator Microsoft Authenticator App";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(164, 378);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 25, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(210, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Enter the generated code to vertify";
            // 
            // textBoxVertify
            // 
            this.textBoxVertify.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxVertify.Location = new System.Drawing.Point(32, 438);
            this.textBoxVertify.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxVertify.MinimumSize = new System.Drawing.Size(4, 23);
            this.textBoxVertify.Name = "textBoxVertify";
            this.textBoxVertify.Size = new System.Drawing.Size(257, 24);
            this.textBoxVertify.TabIndex = 4;
            // 
            // buttonCancel
            // 
            this.buttonCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(116)))), ((int)(((byte)(10)))));
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.ForeColor = System.Drawing.Color.DimGray;
            this.buttonCancel.Location = new System.Drawing.Point(413, 438);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(107, 28);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonVertify
            // 
            this.buttonVertify.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(116)))), ((int)(((byte)(10)))));
            this.buttonVertify.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(116)))), ((int)(((byte)(10)))));
            this.buttonVertify.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonVertify.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonVertify.ForeColor = System.Drawing.Color.Transparent;
            this.buttonVertify.Location = new System.Drawing.Point(299, 438);
            this.buttonVertify.Margin = new System.Windows.Forms.Padding(4);
            this.buttonVertify.Name = "buttonVertify";
            this.buttonVertify.Size = new System.Drawing.Size(107, 28);
            this.buttonVertify.TabIndex = 6;
            this.buttonVertify.Text = "Vertify";
            this.buttonVertify.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonVertify.UseVisualStyleBackColor = false;
            this.buttonVertify.Click += new System.EventHandler(this.buttonVertify_Click);
            // 
            // pictureBoxQRCode
            // 
            this.pictureBoxQRCode.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxQRCode.Image = global::PrintToPACSDemo.Properties.Resources.logo;
            this.pictureBoxQRCode.Location = new System.Drawing.Point(145, 103);
            this.pictureBoxQRCode.Margin = new System.Windows.Forms.Padding(133, 4, 133, 4);
            this.pictureBoxQRCode.Name = "pictureBoxQRCode";
            this.pictureBoxQRCode.Size = new System.Drawing.Size(267, 246);
            this.pictureBoxQRCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxQRCode.TabIndex = 1;
            this.pictureBoxQRCode.TabStop = false;
            // 
            // QRCodeAuthentication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(548, 506);
            this.Controls.Add(this.buttonVertify);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.textBoxVertify);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBoxQRCode);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(548, 506);
            this.MinimumSize = new System.Drawing.Size(548, 506);
            this.Name = "QRCodeAuthentication";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QRCodeAuthentication";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBoxQRCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxVertify;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonVertify;
    }
}