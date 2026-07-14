namespace STM.MediaToPACS.Main.UI.Configurations.Systems
{
    partial class SystemSetup
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel contentPanel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.contentPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            //
            // contentPanel
            //
            this.contentPanel.BackColor = System.Drawing.Color.White;
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(0, 0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(620, 620);
            this.contentPanel.TabIndex = 0;
            //
            // SystemSetup
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(620, 620);
            this.Controls.Add(this.contentPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(560, 560);
            this.Name = "SystemSetup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cấu hình hệ thống";
            this.ResumeLayout(false);
        }
    }
}
