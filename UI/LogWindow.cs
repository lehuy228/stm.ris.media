using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PrintToPACSDemo.UI
{
    public partial class LogWindow : DevExpress.XtraEditors.XtraForm
    {
        public LogWindow(FrmMain frmMain)
      {
         InitializeComponent();
         this.Visible = false;
      }

        public LogWindow()
        {
            InitializeComponent();
            this.Visible = false;
        }

        public RichTextBox RichTextBox
      {
         get { return _rctxtLog; }
         set { _rctxtLog = value; }
      }

      private void _btnClearLog_Click(object sender, EventArgs e)
      {
         _rctxtLog.Clear();
      }

      private void _rctxtLog_TextChanged(object sender, EventArgs e)
      {
         _btnClearLog.Enabled = !(_rctxtLog.Text.Length == 0);

      }

      private void LogWindow_FormClosing(object sender, FormClosingEventArgs e)
      {
         e.Cancel = true;
         this.Visible = false;
      }

      private void checkBox1_CheckedChanged(object sender, EventArgs e)
      {
         this.TopMost = checkBox1.Checked;
      }

      private void _btnSaveToText_Click(object sender, EventArgs e)
      {
         SaveFileDialog dlgSave = new SaveFileDialog();
         dlgSave.Filter = "Text File|*.txt";
         DialogResult dlgRes = dlgSave.ShowDialog();
         if (dlgRes == DialogResult.Cancel)
            return;

         try
         {
            System.IO.File.WriteAllLines(dlgSave.FileName,_rctxtLog.Lines);
         }
         catch (System.Exception ex)
         {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }

      }
   }
}
