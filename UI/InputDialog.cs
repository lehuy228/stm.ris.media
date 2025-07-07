using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PrintToPACSDemo.UI
{
    public partial class InputDialog : DevExpress.XtraEditors.XtraForm
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
            this.button1 = new DevExpress.XtraEditors.SimpleButton();
            this._lblCaption = new DevExpress.XtraEditors.LabelControl();
            this._txtValue = new DevExpress.XtraEditors.TextEdit();
            this.button2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this._txtValue.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(162, 118);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Cancel";
            // 
            // _lblCaption
            // 
            this._lblCaption.Location = new System.Drawing.Point(12, 18);
            this._lblCaption.Name = "_lblCaption";
            this._lblCaption.Size = new System.Drawing.Size(31, 14);
            this._lblCaption.TabIndex = 0;
            this._lblCaption.Text = "label1";
            // 
            // _txtValue
            // 
            this._txtValue.Location = new System.Drawing.Point(81, 60);
            this._txtValue.Name = "_txtValue";
            this._txtValue.Size = new System.Drawing.Size(225, 20);
            this._txtValue.TabIndex = 0;
            this._txtValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this._txtValue_KeyDown);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Location = new System.Drawing.Point(81, 118);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "OK";
            // 
            // InputDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 153);
            this.Controls.Add(this.button2);
            this.Controls.Add(this._txtValue);
            this.Controls.Add(this._lblCaption);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "InputDialog";
            ((System.ComponentModel.ISupportInitialize)(this._txtValue.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.SimpleButton button1;
      private DevExpress.XtraEditors.LabelControl _lblCaption;
      private DevExpress.XtraEditors.TextEdit _txtValue;
      private DevExpress.XtraEditors.SimpleButton button2;

      public string Value
      {
         get
         {
            return _txtValue.Text;
         }
      }

      public InputDialog(string DialogTittle, string DialogPromt, string DialogDefaultValue)
      {
         InitializeComponent();
         this.StartPosition = FormStartPosition.CenterScreen;
         this.Text = DialogTittle;
         _lblCaption.Text = DialogPromt;
         _txtValue.Text = DialogDefaultValue;
         _txtValue.Focus();
         _txtValue.SelectAll() ;
      }

      private void _txtValue_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.KeyCode == Keys.Enter)
            DialogResult = DialogResult.OK;
         if (e.KeyCode == Keys.Escape)
            DialogResult = DialogResult.Cancel;
      }

   }
}
