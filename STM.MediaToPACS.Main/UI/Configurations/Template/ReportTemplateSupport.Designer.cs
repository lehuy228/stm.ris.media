
namespace STM.MediaToPACS.Main.Utilities
{
    partial class ReportTemplateSupport
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
            this._txNameReport = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this._cbbModality = new DevExpress.XtraEditors.ComboBoxEdit();
            this._btnClose = new DevExpress.XtraEditors.SimpleButton();
            this._btnContinue = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this._txNameReport.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._cbbModality.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tên mẫu báo cáo:";
            // 
            // _txNameReport
            // 
            this._txNameReport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txNameReport.Location = new System.Drawing.Point(191, 13);
            this._txNameReport.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._txNameReport.Name = "_txNameReport";
            this._txNameReport.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._txNameReport.Properties.Appearance.Options.UseFont = true;
            this._txNameReport.Size = new System.Drawing.Size(422, 28);
            this._txNameReport.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 61);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Phương thức chụp:";
            // 
            // _cbbModality
            // 
            this._cbbModality.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._cbbModality.Location = new System.Drawing.Point(191, 54);
            this._cbbModality.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._cbbModality.Name = "_cbbModality";
            this._cbbModality.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._cbbModality.Properties.Appearance.Options.UseFont = true;
            this._cbbModality.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._cbbModality.Properties.Items.AddRange(new object[] {
            "ES",
            "US",
            "DX",
            "CT",
            "MR",
            "US",
            "PT"});
            this._cbbModality.Size = new System.Drawing.Size(422, 28);
            this._cbbModality.TabIndex = 3;
            // 
            // _btnClose
            // 
            this._btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnClose.Location = new System.Drawing.Point(507, 120);
            this._btnClose.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(106, 32);
            this._btnClose.TabIndex = 4;
            this._btnClose.Text = "Thoát";
            this._btnClose.Click += new System.EventHandler(this._btnClose_Click);
            // 
            // _btnContinue
            // 
            this._btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnContinue.Location = new System.Drawing.Point(393, 120);
            this._btnContinue.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._btnContinue.Name = "_btnContinue";
            this._btnContinue.Size = new System.Drawing.Size(106, 32);
            this._btnContinue.TabIndex = 5;
            this._btnContinue.Text = "Tiếp theo";
            this._btnContinue.Click += new System.EventHandler(this._btnContinue_Click);
            // 
            // ReportTemplateSupport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 167);
            this.Controls.Add(this._btnContinue);
            this.Controls.Add(this._btnClose);
            this.Controls.Add(this._cbbModality);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._txNameReport);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "ReportTemplateSupport";
            this.Text = "Thông tin mẫu báo cáo";
            ((System.ComponentModel.ISupportInitialize)(this._txNameReport.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._cbbModality.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit _txNameReport;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ComboBoxEdit _cbbModality;
        private DevExpress.XtraEditors.SimpleButton _btnClose;
        private DevExpress.XtraEditors.SimpleButton _btnContinue;
    }
}