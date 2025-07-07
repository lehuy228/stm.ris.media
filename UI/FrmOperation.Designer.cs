namespace PrintToPACSDemo.UI
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
            this._btnCancelOperation.Location = new System.Drawing.Point(105, 71);
            this._btnCancelOperation.Name = "_btnCancelOperation";
            this._btnCancelOperation.Size = new System.Drawing.Size(150, 23);
            this._btnCancelOperation.TabIndex = 0;
            this._btnCancelOperation.Text = "_btnCancelOperation";
            this._btnCancelOperation.Click += new System.EventHandler(this._btnCancelOperation_Click);
            // 
            // _lblCaption
            // 
            this._lblCaption.Location = new System.Drawing.Point(12, 9);
            this._lblCaption.Name = "_lblCaption";
            this._lblCaption.Size = new System.Drawing.Size(59, 14);
            this._lblCaption.TabIndex = 1;
            this._lblCaption.Text = "_lblCaption";
            // 
            // FrmOperation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 96);
            this.ControlBox = false;
            this.Controls.Add(this._lblCaption);
            this.Controls.Add(this._btnCancelOperation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmOperation";
            this.ShowIcon = false;
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