using System.Drawing;

namespace STM.MediaToPACS.Main.UI.V2
{
    partial class FrmMainV2Host
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            //
            // FrmMainV2Host
            //
            this.ClientSize = new Size(1200, 750);
            this.Name = "FrmMainV2Host";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMainV2Host_FormClosing);
            this.ResumeLayout(false);
        }
    }
}
