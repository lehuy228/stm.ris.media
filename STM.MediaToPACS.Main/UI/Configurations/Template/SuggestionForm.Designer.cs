
namespace STM.MediaToPACS.Main.Utilities
{
    partial class SuggestionForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SuggestionForm));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this._btnSave = new DevExpress.XtraEditors.SimpleButton();
            this._btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this._richKhuyenNghi = new System.Windows.Forms.RichTextBox();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this._richKetLuan = new System.Windows.Forms.RichTextBox();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this._cbbDSThietBi = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl23 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this._cbbGender = new DevExpress.XtraEditors.ComboBoxEdit();
            this._txName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this._richMoTa = new System.Windows.Forms.RichTextBox();
            this.contextMenuRichTextBox = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._cbbDSThietBi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._cbbGender.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this._btnSave);
            this.panelControl1.Controls.Add(this._btnClose);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 737);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1427, 48);
            this.panelControl1.TabIndex = 1;
            // 
            // _btnSave
            // 
            this._btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnSave.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this._btnSave.Location = new System.Drawing.Point(1212, 5);
            this._btnSave.Name = "_btnSave";
            this._btnSave.Size = new System.Drawing.Size(102, 32);
            this._btnSave.TabIndex = 1;
            this._btnSave.Text = "Lưu";
            this._btnSave.Click += new System.EventHandler(this._btnSave_Click);
            // 
            // _btnClose
            // 
            this._btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnClose.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("_btnClose.ImageOptions.Image")));
            this._btnClose.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this._btnClose.Location = new System.Drawing.Point(1322, 5);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(99, 32);
            this._btnClose.TabIndex = 0;
            this._btnClose.Text = "Đóng";
            this._btnClose.Click += new System.EventHandler(this._btnClose_Click);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this._richKhuyenNghi);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl2.Location = new System.Drawing.Point(0, 598);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(1427, 139);
            this.groupControl2.TabIndex = 1;
            this.groupControl2.Text = "Khuyến nghị";
            // 
            // _richKhuyenNghi
            // 
            this._richKhuyenNghi.Dock = System.Windows.Forms.DockStyle.Fill;
            this._richKhuyenNghi.Location = new System.Drawing.Point(2, 27);
            this._richKhuyenNghi.Name = "_richKhuyenNghi";
            this._richKhuyenNghi.Size = new System.Drawing.Size(1423, 110);
            this._richKhuyenNghi.TabIndex = 0;
            this._richKhuyenNghi.Text = "";
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this._richKetLuan);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl3.Location = new System.Drawing.Point(0, 433);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(1427, 165);
            this.groupControl3.TabIndex = 1;
            this.groupControl3.Text = "Kết luận";
            // 
            // _richKetLuan
            // 
            this._richKetLuan.Dock = System.Windows.Forms.DockStyle.Fill;
            this._richKetLuan.Location = new System.Drawing.Point(2, 27);
            this._richKetLuan.Name = "_richKetLuan";
            this._richKetLuan.Size = new System.Drawing.Size(1423, 136);
            this._richKetLuan.TabIndex = 0;
            this._richKetLuan.Text = "";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this._cbbDSThietBi);
            this.panelControl2.Controls.Add(this.labelControl23);
            this.panelControl2.Controls.Add(this.labelControl4);
            this.panelControl2.Controls.Add(this._cbbGender);
            this.panelControl2.Controls.Add(this._txName);
            this.panelControl2.Controls.Add(this.labelControl2);
            this.panelControl2.Controls.Add(this.labelControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1427, 49);
            this.panelControl2.TabIndex = 2;
            // 
            // _cbbDSThietBi
            // 
            this._cbbDSThietBi.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._cbbDSThietBi.Location = new System.Drawing.Point(820, 6);
            this._cbbDSThietBi.Name = "_cbbDSThietBi";
            this._cbbDSThietBi.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._cbbDSThietBi.Properties.Appearance.Options.UseFont = true;
            this._cbbDSThietBi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._cbbDSThietBi.Size = new System.Drawing.Size(593, 28);
            this._cbbDSThietBi.TabIndex = 25;
            // 
            // labelControl23
            // 
            this.labelControl23.Appearance.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl23.Appearance.Options.UseFont = true;
            this.labelControl23.Location = new System.Drawing.Point(727, 9);
            this.labelControl23.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl23.Name = "labelControl23";
            this.labelControl23.Size = new System.Drawing.Size(76, 21);
            this.labelControl23.TabIndex = 26;
            this.labelControl23.Text = "Máy chụp:";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(4, 13);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(58, 18);
            this.labelControl4.TabIndex = 8;
            this.labelControl4.Text = "Giới tính:";
            // 
            // _cbbGender
            // 
            this._cbbGender.Location = new System.Drawing.Point(69, 6);
            this._cbbGender.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._cbbGender.Name = "_cbbGender";
            this._cbbGender.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._cbbGender.Properties.Appearance.Options.UseFont = true;
            this._cbbGender.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._cbbGender.Properties.Items.AddRange(new object[] {
            "Nam",
            "Nữ",
            "Khác",
            " "});
            this._cbbGender.Size = new System.Drawing.Size(92, 28);
            this._cbbGender.TabIndex = 7;
            // 
            // _txName
            // 
            this._txName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txName.Location = new System.Drawing.Point(234, 6);
            this._txName.Name = "_txName";
            this._txName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._txName.Properties.Appearance.Options.UseFont = true;
            this._txName.Size = new System.Drawing.Size(485, 28);
            this._txName.TabIndex = 4;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(167, 14);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(53, 18);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "Dịch vụ:";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(6, 13);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(0, 18);
            this.labelControl1.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this._richMoTa);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 49);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1427, 384);
            this.groupControl1.TabIndex = 3;
            this.groupControl1.Text = "Mô tả";
            // 
            // _richMoTa
            // 
            this._richMoTa.Dock = System.Windows.Forms.DockStyle.Fill;
            this._richMoTa.Location = new System.Drawing.Point(2, 27);
            this._richMoTa.Name = "_richMoTa";
            this._richMoTa.Size = new System.Drawing.Size(1423, 355);
            this._richMoTa.TabIndex = 0;
            this._richMoTa.Text = "";
            // 
            // contextMenuRichTextBox
            // 
            this.contextMenuRichTextBox.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuRichTextBox.Name = "contextMenuRichTextBox";
            this.contextMenuRichTextBox.Size = new System.Drawing.Size(61, 4);
            // 
            // SuggestionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1427, 785);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.groupControl3);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "SuggestionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mẫu mô tả";
            this.Load += new System.EventHandler(this.SuggestionForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._cbbDSThietBi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._cbbGender.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton _btnSave;
        private DevExpress.XtraEditors.SimpleButton _btnClose;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private System.Windows.Forms.RichTextBox _richKhuyenNghi;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private System.Windows.Forms.RichTextBox _richKetLuan;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private System.Windows.Forms.RichTextBox _richMoTa;
        private DevExpress.XtraEditors.TextEdit _txName;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ComboBoxEdit _cbbGender;
        private System.Windows.Forms.ContextMenuStrip contextMenuRichTextBox;
        private DevExpress.XtraEditors.LookUpEdit _cbbDSThietBi;
        private DevExpress.XtraEditors.LabelControl labelControl23;
    }
}