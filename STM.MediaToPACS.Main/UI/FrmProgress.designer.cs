namespace STM.MediaToPACS.Main
{
   partial class FrmProgress
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
            this._lblPage = new DevExpress.XtraEditors.LabelControl();
            this._lblPrinter = new DevExpress.XtraEditors.LabelControl();
            this._btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // _lblPage
            // 
            this._lblPage.Location = new System.Drawing.Point(102, 63);
            this._lblPage.Name = "_lblPage";
            this._lblPage.Size = new System.Drawing.Size(0, 14);
            this._lblPage.TabIndex = 0;
            // 
            // _lblPrinter
            // 
            this._lblPrinter.Location = new System.Drawing.Point(12, 15);
            this._lblPrinter.Name = "_lblPrinter";
            this._lblPrinter.Size = new System.Drawing.Size(36, 14);
            this._lblPrinter.TabIndex = 1;
            this._lblPrinter.Text = "printer";
            // 
            // _btnCancel
            // 
            this._btnCancel.Location = new System.Drawing.Point(69, 88);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(114, 21);
            this._btnCancel.TabIndex = 2;
            this._btnCancel.Text = "Cancel Printing";
            this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
            // 
            // FrmProgress
            // 
            this.ClientSize = new System.Drawing.Size(255, 121);
            this.ControlBox = false;
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._lblPrinter);
            this.Controls.Add(this._lblPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.IconOptions.ShowIcon = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmProgress";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Receiving Print Job...";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.LabelControl _lblPage;
      private DevExpress.XtraEditors.LabelControl _lblPrinter;
      private DevExpress.XtraEditors.SimpleButton _btnCancel;
   }
}