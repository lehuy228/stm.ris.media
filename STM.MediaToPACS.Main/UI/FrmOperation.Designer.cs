namespace STM.MediaToPACS.Main.UI
{
   partial class FrmOperation
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
            this._btnCancelOperation = new DevExpress.XtraEditors.SimpleButton();
            this._lblCaption = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // _btnCancelOperation
            // 
            this._btnCancelOperation.Location = new System.Drawing.Point(158, 98);
            this._btnCancelOperation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._btnCancelOperation.Name = "_btnCancelOperation";
            this._btnCancelOperation.Size = new System.Drawing.Size(225, 32);
            this._btnCancelOperation.TabIndex = 0;
            this._btnCancelOperation.Text = "_btnCancelOperation";
            this._btnCancelOperation.Click += new System.EventHandler(this._btnCancelOperation_Click);
            // 
            // _lblCaption
            // 
            this._lblCaption.Location = new System.Drawing.Point(18, 12);
            this._lblCaption.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._lblCaption.Name = "_lblCaption";
            this._lblCaption.Size = new System.Drawing.Size(73, 18);
            this._lblCaption.TabIndex = 1;
            this._lblCaption.Text = "_lblCaption";
            // 
            // FrmOperation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 138);
            this.ControlBox = false;
            this.Controls.Add(this._lblCaption);
            this.Controls.Add(this._btnCancelOperation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.IconOptions.ShowIcon = false;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmOperation";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trạng thái hoạt động";
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.SimpleButton _btnCancelOperation;
      private DevExpress.XtraEditors.LabelControl _lblCaption;
   }
}