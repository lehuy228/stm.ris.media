using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PrintToPACSDemo
{
    /// <summary>
    /// Summary description for ErrorList.
    /// </summary>
    public class ErrorList : DevExpress.XtraEditors.XtraForm
    {
        private DevExpress.XtraEditors.SimpleButton _btnClear;
      private DevExpress.XtraEditors.SimpleButton _btnCancel;
      private System.Windows.Forms.ListBox _lstError;
      private System.Windows.Forms.GroupBox _gbErrorList;
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.Container components = null;

      public ErrorList( )
      {
         //
         // Required for Windows Form Designer support
         //
         InitializeComponent();

         //
         // TODO: Add any constructor code after InitializeComponent call
         //
      }

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      protected override void Dispose(bool disposing)
      {
         if(disposing)
         {
            if(components != null)
            {
               components.Dispose();
            }
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code
      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent( )
      {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorList));
            this._gbErrorList = new System.Windows.Forms.GroupBox();
            this._lstError = new System.Windows.Forms.ListBox();
            this._btnClear = new DevExpress.XtraEditors.SimpleButton();
            this._btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this._gbErrorList.SuspendLayout();
            this.SuspendLayout();
            // 
            // _gbErrorList
            // 
            this._gbErrorList.Controls.Add(this._lstError);
            this._gbErrorList.Location = new System.Drawing.Point(10, 9);
            this._gbErrorList.Name = "_gbErrorList";
            this._gbErrorList.Size = new System.Drawing.Size(566, 231);
            this._gbErrorList.TabIndex = 0;
            this._gbErrorList.TabStop = false;
            // 
            // _lstError
            // 
            this._lstError.ItemHeight = 14;
            this._lstError.Location = new System.Drawing.Point(10, 18);
            this._lstError.Name = "_lstError";
            this._lstError.Size = new System.Drawing.Size(547, 186);
            this._lstError.TabIndex = 0;
            // 
            // _btnClear
            // 
            this._btnClear.Location = new System.Drawing.Point(202, 249);
            this._btnClear.Name = "_btnClear";
            this._btnClear.Size = new System.Drawing.Size(90, 27);
            this._btnClear.TabIndex = 1;
            this._btnClear.Text = "Xóa hết";
            this._btnClear.Click += new System.EventHandler(this._btnClear_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(298, 249);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(90, 27);
            this._btnCancel.TabIndex = 2;
            this._btnCancel.Text = "Đóng";
            // 
            // ErrorList
            // 
            this.AcceptButton = this._btnClear;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.CancelButton = this._btnCancel;
            this.ClientSize = new System.Drawing.Size(591, 292);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnClear);
            this.Controls.Add(this._gbErrorList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("ErrorList.IconOptions.Icon")));
            this.IconOptions.Image = global::PrintToPACSDemo.Properties.Resources.logo;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorList";
            this.Text = "Danh sách lỗi TWAIN";
            this.Load += new System.EventHandler(this.ErrorList_Load);
            this._gbErrorList.ResumeLayout(false);
            this.ResumeLayout(false);

      }
      #endregion
      public ArrayList _arrayList;
      public bool _listCleared;

      private void ErrorList_Load(object sender, System.EventArgs e)
      {
         _listCleared = false;
         for(int i = 0; i < _arrayList.Count; i++)
            _lstError.Items.Add(_arrayList[i]);
      }

      private void _btnClear_Click(object sender, System.EventArgs e)
      {
         _arrayList.Clear();
         _lstError.Items.Clear();
         _listCleared = true;
         DialogResult = DialogResult.OK;
      }
   }
}
