namespace PrintToPACSDemo.UI
{
   partial class LogWindow
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
            this._btnClearLog = new DevExpress.XtraEditors.SimpleButton();
            this._rctxtLog = new System.Windows.Forms.RichTextBox();
            this.checkBox1 = new DevExpress.XtraEditors.CheckEdit();
            this._btnSaveToText = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.checkBox1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // _btnClearLog
            // 
            this._btnClearLog.Enabled = false;
            this._btnClearLog.Location = new System.Drawing.Point(14, 14);
            this._btnClearLog.Name = "_btnClearLog";
            this._btnClearLog.Size = new System.Drawing.Size(73, 27);
            this._btnClearLog.TabIndex = 0;
            this._btnClearLog.Text = "Xóa hết";
            this._btnClearLog.Click += new System.EventHandler(this._btnClearLog_Click);
            // 
            // _rctxtLog
            // 
            this._rctxtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._rctxtLog.Location = new System.Drawing.Point(14, 47);
            this._rctxtLog.Name = "_rctxtLog";
            this._rctxtLog.Size = new System.Drawing.Size(389, 269);
            this._rctxtLog.TabIndex = 1;
            this._rctxtLog.Text = "";
            this._rctxtLog.TextChanged += new System.EventHandler(this._rctxtLog_TextChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.Location = new System.Drawing.Point(308, 16);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Properties.Caption = "Hiển thị trên";
            this.checkBox1.Size = new System.Drawing.Size(95, 20);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // _btnSaveToText
            // 
            this._btnSaveToText.Location = new System.Drawing.Point(94, 14);
            this._btnSaveToText.Name = "_btnSaveToText";
            this._btnSaveToText.Size = new System.Drawing.Size(91, 27);
            this._btnSaveToText.TabIndex = 4;
            this._btnSaveToText.Text = "Xuất tệp";
            this._btnSaveToText.Click += new System.EventHandler(this._btnSaveToText_Click);
            // 
            // LogWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 351);
            this.Controls.Add(this._btnSaveToText);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this._rctxtLog);
            this.Controls.Add(this._btnClearLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(356, 262);
            this.Name = "LogWindow";
            this.Text = "Nhật ký";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogWindow_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.checkBox1.Properties)).EndInit();
            this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.SimpleButton _btnClearLog;
      private System.Windows.Forms.RichTextBox _rctxtLog;
      private DevExpress.XtraEditors.CheckEdit checkBox1;
      private DevExpress.XtraEditors.SimpleButton _btnSaveToText;
   }
}