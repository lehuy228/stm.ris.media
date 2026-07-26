namespace STM.MediaToPACS.Main.UI.Configurations
{
    partial class SystemSettingsDialog
    {
        private System.ComponentModel.IContainer components = null;
        private DevExpress.XtraEditors.PanelControl footerPanel;
        private System.Windows.Forms.FlowLayoutPanel footerButtonsPanel;
        private DevExpress.XtraEditors.SimpleButton cancelButton;
        private DevExpress.XtraEditors.SimpleButton saveButton;
        private DevExpress.XtraTab.XtraTabControl settingsTabControl;
        private DevExpress.XtraTab.XtraTabPage hisRisTabPage;
        private DevExpress.XtraTab.XtraTabPage cameraTabPage;
        private DevExpress.XtraTab.XtraTabPage shortcutPrinterTabPage;
        private DevExpress.XtraEditors.PanelControl hisRisHeaderPanel;
        private DevExpress.XtraEditors.LabelControl hisRisTitleLabel;
        private DevExpress.XtraEditors.LabelControl hisRisDescriptionLabel;
        private DevExpress.XtraEditors.PanelControl hisRisBodyPanel;
        private System.Windows.Forms.TableLayoutPanel hisRisTableLayoutPanel;
        private DevExpress.XtraEditors.LabelControl serverAddressLabel;
        private DevExpress.XtraEditors.TextEdit serverAddressTextEdit;
        private DevExpress.XtraEditors.LabelControl paymentCheckLabel;
        private DevExpress.XtraEditors.TextEdit paymentCheckTextEdit;
        private DevExpress.XtraEditors.PanelControl cameraHeaderPanel;
        private DevExpress.XtraEditors.LabelControl cameraTitleLabel;
        private DevExpress.XtraEditors.LabelControl cameraDescriptionLabel;
        private DevExpress.XtraEditors.PanelControl cameraBodyPanel;
        private ConfigCamera configCamera;
        private DevExpress.XtraEditors.PanelControl shortcutHeaderPanel;
        private DevExpress.XtraEditors.LabelControl shortcutTitleLabel;
        private DevExpress.XtraEditors.LabelControl shortcutDescriptionLabel;
        private DevExpress.XtraEditors.PanelControl shortcutBodyPanel;
        private System.Windows.Forms.TableLayoutPanel shortcutMainLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel shortcutLeftColumnPanel;
        private System.Windows.Forms.TableLayoutPanel shortcutRightColumnPanel;
        private DevExpress.XtraEditors.GroupControl worklistGroupControl;
        private System.Windows.Forms.TableLayoutPanel worklistShortcutTableLayoutPanel;
        private DevExpress.XtraEditors.LabelControl searchKeyLabel;
        private DevExpress.XtraEditors.ComboBoxEdit searchKeyComboBox;
        private DevExpress.XtraEditors.GroupControl cameraShortcutGroupControl;
        private System.Windows.Forms.TableLayoutPanel cameraShortcutTableLayoutPanel;
        private DevExpress.XtraEditors.LabelControl linkCameraKeyLabel;
        private DevExpress.XtraEditors.ComboBoxEdit linkCameraKeyComboBox;
        private DevExpress.XtraEditors.LabelControl snapshotKeyLabel;
        private DevExpress.XtraEditors.ComboBoxEdit snapshotKeyComboBox;
        private DevExpress.XtraEditors.LabelControl stopCameraKeyLabel;
        private DevExpress.XtraEditors.ComboBoxEdit stopCameraKeyComboBox;
        private DevExpress.XtraEditors.GroupControl conclusionGroupControl;
        private System.Windows.Forms.TableLayoutPanel conclusionShortcutTableLayoutPanel;
        private DevExpress.XtraEditors.LabelControl signKeyLabel;
        private DevExpress.XtraEditors.ComboBoxEdit signKeyComboBox;
        private DevExpress.XtraEditors.LabelControl printKeyLabel;
        private DevExpress.XtraEditors.ComboBoxEdit printKeyComboBox;
        private DevExpress.XtraEditors.LabelControl draftKeyLabel;
        private DevExpress.XtraEditors.ComboBoxEdit draftKeyComboBox;
        private DevExpress.XtraEditors.LabelControl exitKeyLabel;
        private DevExpress.XtraEditors.ComboBoxEdit exitKeyComboBox;
        private DevExpress.XtraEditors.LabelControl previewKeyLabel;
        private DevExpress.XtraEditors.ComboBoxEdit previewKeyComboBox;
        private DevExpress.XtraEditors.LabelControl captureImageKeyLabel;
        private DevExpress.XtraEditors.ComboBoxEdit captureImageKeyComboBox;
        private DevExpress.XtraEditors.GroupControl printerGroupControl;
        private DevExpress.XtraEditors.ComboBoxEdit printerComboBox;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.footerPanel = new DevExpress.XtraEditors.PanelControl();
            this.footerButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.cancelButton = new DevExpress.XtraEditors.SimpleButton();
            this.saveButton = new DevExpress.XtraEditors.SimpleButton();
            this.settingsTabControl = new DevExpress.XtraTab.XtraTabControl();
            this.hisRisTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.hisRisBodyPanel = new DevExpress.XtraEditors.PanelControl();
            this.hisRisTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.serverAddressLabel = new DevExpress.XtraEditors.LabelControl();
            this.serverAddressTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.paymentCheckLabel = new DevExpress.XtraEditors.LabelControl();
            this.paymentCheckTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.hisRisHeaderPanel = new DevExpress.XtraEditors.PanelControl();
            this.hisRisDescriptionLabel = new DevExpress.XtraEditors.LabelControl();
            this.hisRisTitleLabel = new DevExpress.XtraEditors.LabelControl();
            this.cameraTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.cameraBodyPanel = new DevExpress.XtraEditors.PanelControl();
            this.configCamera = new STM.MediaToPACS.Main.UI.Configurations.ConfigCamera();
            this.cameraHeaderPanel = new DevExpress.XtraEditors.PanelControl();
            this.cameraDescriptionLabel = new DevExpress.XtraEditors.LabelControl();
            this.cameraTitleLabel = new DevExpress.XtraEditors.LabelControl();
            this.shortcutPrinterTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.shortcutBodyPanel = new DevExpress.XtraEditors.PanelControl();
            this.shortcutMainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.shortcutLeftColumnPanel = new System.Windows.Forms.TableLayoutPanel();
            this.worklistGroupControl = new DevExpress.XtraEditors.GroupControl();
            this.worklistShortcutTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.searchKeyLabel = new DevExpress.XtraEditors.LabelControl();
            this.searchKeyComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cameraShortcutGroupControl = new DevExpress.XtraEditors.GroupControl();
            this.cameraShortcutTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.linkCameraKeyLabel = new DevExpress.XtraEditors.LabelControl();
            this.linkCameraKeyComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.snapshotKeyLabel = new DevExpress.XtraEditors.LabelControl();
            this.snapshotKeyComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.stopCameraKeyLabel = new DevExpress.XtraEditors.LabelControl();
            this.stopCameraKeyComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.shortcutRightColumnPanel = new System.Windows.Forms.TableLayoutPanel();
            this.conclusionGroupControl = new DevExpress.XtraEditors.GroupControl();
            this.conclusionShortcutTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.signKeyLabel = new DevExpress.XtraEditors.LabelControl();
            this.signKeyComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.printKeyLabel = new DevExpress.XtraEditors.LabelControl();
            this.printKeyComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.draftKeyLabel = new DevExpress.XtraEditors.LabelControl();
            this.draftKeyComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.exitKeyLabel = new DevExpress.XtraEditors.LabelControl();
            this.exitKeyComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.previewKeyLabel = new DevExpress.XtraEditors.LabelControl();
            this.previewKeyComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.captureImageKeyLabel = new DevExpress.XtraEditors.LabelControl();
            this.captureImageKeyComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.printerGroupControl = new DevExpress.XtraEditors.GroupControl();
            this.printerComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.shortcutHeaderPanel = new DevExpress.XtraEditors.PanelControl();
            this.shortcutDescriptionLabel = new DevExpress.XtraEditors.LabelControl();
            this.shortcutTitleLabel = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.footerPanel)).BeginInit();
            this.footerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.settingsTabControl)).BeginInit();
            this.settingsTabControl.SuspendLayout();
            this.hisRisTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hisRisBodyPanel)).BeginInit();
            this.hisRisBodyPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serverAddressTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paymentCheckTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hisRisHeaderPanel)).BeginInit();
            this.hisRisHeaderPanel.SuspendLayout();
            this.cameraTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cameraBodyPanel)).BeginInit();
            this.cameraBodyPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cameraHeaderPanel)).BeginInit();
            this.cameraHeaderPanel.SuspendLayout();
            this.shortcutPrinterTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shortcutBodyPanel)).BeginInit();
            this.shortcutBodyPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.worklistGroupControl)).BeginInit();
            this.worklistGroupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchKeyComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cameraShortcutGroupControl)).BeginInit();
            this.cameraShortcutGroupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.linkCameraKeyComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.snapshotKeyComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopCameraKeyComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.conclusionGroupControl)).BeginInit();
            this.conclusionGroupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.signKeyComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printKeyComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.draftKeyComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitKeyComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewKeyComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.captureImageKeyComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printerGroupControl)).BeginInit();
            this.printerGroupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.printerComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shortcutHeaderPanel)).BeginInit();
            this.shortcutHeaderPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // footerPanel
            //
            this.footerPanel.Controls.Add(this.footerButtonsPanel);
            this.footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footerPanel.Location = new System.Drawing.Point(0, 638);
            this.footerPanel.Name = "footerPanel";
            this.footerPanel.Size = new System.Drawing.Size(1080, 62);
            this.footerPanel.TabIndex = 1;
            //
            // footerButtonsPanel
            //
            this.footerButtonsPanel.Controls.Add(this.cancelButton);
            this.footerButtonsPanel.Controls.Add(this.saveButton);
            this.footerButtonsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.footerButtonsPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.footerButtonsPanel.Location = new System.Drawing.Point(833, 2);
            this.footerButtonsPanel.Name = "footerButtonsPanel";
            this.footerButtonsPanel.Size = new System.Drawing.Size(245, 58);
            this.footerButtonsPanel.TabIndex = 0;
            this.footerButtonsPanel.WrapContents = false;
            //
            // cancelButton
            //
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(0, 13);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(0, 13, 8, 0);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(95, 34);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Hủy";
            //
            // saveButton
            //
            this.saveButton.Location = new System.Drawing.Point(103, 13);
            this.saveButton.Margin = new System.Windows.Forms.Padding(0, 13, 14, 0);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(118, 34);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Lưu thay đổi";
            this.saveButton.Click += new System.EventHandler(this.Save_Click);
            //
            // settingsTabControl
            //
            this.settingsTabControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.settingsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsTabControl.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Left;
            this.settingsTabControl.HeaderOrientation = DevExpress.XtraTab.TabOrientation.Horizontal;
            this.settingsTabControl.Location = new System.Drawing.Point(0, 0);
            this.settingsTabControl.Name = "settingsTabControl";
            this.settingsTabControl.SelectedTabPage = this.hisRisTabPage;
            this.settingsTabControl.Size = new System.Drawing.Size(1080, 638);
            this.settingsTabControl.TabIndex = 0;
            this.settingsTabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.hisRisTabPage,
            this.cameraTabPage,
            this.shortcutPrinterTabPage});
            this.settingsTabControl.AppearancePage.Header.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.settingsTabControl.AppearancePage.Header.Options.UseFont = true;
            //
            // hisRisTabPage
            //
            this.hisRisTabPage.Controls.Add(this.hisRisBodyPanel);
            this.hisRisTabPage.Controls.Add(this.hisRisHeaderPanel);
            this.hisRisTabPage.Name = "hisRisTabPage";
            this.hisRisTabPage.Size = new System.Drawing.Size(994, 636);
            this.hisRisTabPage.Text = "HIS / RIS";
            //
            // hisRisBodyPanel
            //
            this.hisRisBodyPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.hisRisBodyPanel.Controls.Add(this.hisRisTableLayoutPanel);
            this.hisRisBodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hisRisBodyPanel.Location = new System.Drawing.Point(0, 76);
            this.hisRisBodyPanel.Name = "hisRisBodyPanel";
            this.hisRisBodyPanel.Padding = new System.Windows.Forms.Padding(26, 18, 26, 18);
            this.hisRisBodyPanel.Size = new System.Drawing.Size(994, 560);
            this.hisRisBodyPanel.TabIndex = 1;
            //
            // hisRisTableLayoutPanel
            //
            this.hisRisTableLayoutPanel.AutoSize = true;
            this.hisRisTableLayoutPanel.ColumnCount = 2;
            this.hisRisTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.hisRisTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.hisRisTableLayoutPanel.Controls.Add(this.serverAddressLabel, 0, 0);
            this.hisRisTableLayoutPanel.Controls.Add(this.serverAddressTextEdit, 1, 0);
            this.hisRisTableLayoutPanel.Controls.Add(this.paymentCheckLabel, 0, 1);
            this.hisRisTableLayoutPanel.Controls.Add(this.paymentCheckTextEdit, 1, 1);
            this.hisRisTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.hisRisTableLayoutPanel.Location = new System.Drawing.Point(26, 18);
            this.hisRisTableLayoutPanel.Name = "hisRisTableLayoutPanel";
            this.hisRisTableLayoutPanel.RowCount = 2;
            this.hisRisTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.hisRisTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.hisRisTableLayoutPanel.Size = new System.Drawing.Size(942, 96);
            this.hisRisTableLayoutPanel.TabIndex = 0;
            //
            // serverAddressLabel
            //
            this.serverAddressLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.serverAddressLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.serverAddressLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverAddressLabel.Location = new System.Drawing.Point(3, 3);
            this.serverAddressLabel.Name = "serverAddressLabel";
            this.serverAddressLabel.Size = new System.Drawing.Size(164, 42);
            this.serverAddressLabel.TabIndex = 0;
            this.serverAddressLabel.Text = "Địa chỉ server";
            //
            // serverAddressTextEdit
            //
            this.serverAddressTextEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverAddressTextEdit.Location = new System.Drawing.Point(180, 8);
            this.serverAddressTextEdit.Margin = new System.Windows.Forms.Padding(10, 8, 0, 8);
            this.serverAddressTextEdit.Name = "serverAddressTextEdit";
            this.serverAddressTextEdit.Size = new System.Drawing.Size(762, 20);
            this.serverAddressTextEdit.TabIndex = 1;
            //
            // paymentCheckLabel
            //
            this.paymentCheckLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.paymentCheckLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.paymentCheckLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paymentCheckLabel.Location = new System.Drawing.Point(3, 51);
            this.paymentCheckLabel.Name = "paymentCheckLabel";
            this.paymentCheckLabel.Size = new System.Drawing.Size(164, 42);
            this.paymentCheckLabel.TabIndex = 2;
            this.paymentCheckLabel.Text = "Kiểm tra thanh toán";
            //
            // paymentCheckTextEdit
            //
            this.paymentCheckTextEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paymentCheckTextEdit.Location = new System.Drawing.Point(180, 56);
            this.paymentCheckTextEdit.Margin = new System.Windows.Forms.Padding(10, 8, 0, 8);
            this.paymentCheckTextEdit.Name = "paymentCheckTextEdit";
            this.paymentCheckTextEdit.Size = new System.Drawing.Size(762, 20);
            this.paymentCheckTextEdit.TabIndex = 3;
            //
            // hisRisHeaderPanel
            //
            this.hisRisHeaderPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.hisRisHeaderPanel.Controls.Add(this.hisRisDescriptionLabel);
            this.hisRisHeaderPanel.Controls.Add(this.hisRisTitleLabel);
            this.hisRisHeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.hisRisHeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.hisRisHeaderPanel.Name = "hisRisHeaderPanel";
            this.hisRisHeaderPanel.Padding = new System.Windows.Forms.Padding(20, 12, 20, 8);
            this.hisRisHeaderPanel.Size = new System.Drawing.Size(994, 76);
            this.hisRisHeaderPanel.TabIndex = 0;
            //
            // hisRisDescriptionLabel
            //
            this.hisRisDescriptionLabel.Appearance.ForeColor = System.Drawing.Color.DimGray;
            this.hisRisDescriptionLabel.Appearance.Options.UseForeColor = true;
            this.hisRisDescriptionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.hisRisDescriptionLabel.Location = new System.Drawing.Point(20, 40);
            this.hisRisDescriptionLabel.Name = "hisRisDescriptionLabel";
            this.hisRisDescriptionLabel.Size = new System.Drawing.Size(253, 13);
            this.hisRisDescriptionLabel.TabIndex = 1;
            this.hisRisDescriptionLabel.Text = "Thiết lập địa chỉ server và API kiểm tra thanh toán.";
            //
            // hisRisTitleLabel
            //
            this.hisRisTitleLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.hisRisTitleLabel.Appearance.Options.UseFont = true;
            this.hisRisTitleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.hisRisTitleLabel.Location = new System.Drawing.Point(20, 12);
            this.hisRisTitleLabel.Name = "hisRisTitleLabel";
            this.hisRisTitleLabel.Size = new System.Drawing.Size(152, 21);
            this.hisRisTitleLabel.TabIndex = 0;
            this.hisRisTitleLabel.Text = "Kết nối HIS / RIS";
            //
            // cameraTabPage
            //
            this.cameraTabPage.Controls.Add(this.cameraBodyPanel);
            this.cameraTabPage.Controls.Add(this.cameraHeaderPanel);
            this.cameraTabPage.Name = "cameraTabPage";
            this.cameraTabPage.Size = new System.Drawing.Size(994, 636);
            this.cameraTabPage.Text = "Camera";
            //
            // cameraBodyPanel
            //
            this.cameraBodyPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.cameraBodyPanel.Controls.Add(this.configCamera);
            this.cameraBodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraBodyPanel.Location = new System.Drawing.Point(0, 76);
            this.cameraBodyPanel.Name = "cameraBodyPanel";
            this.cameraBodyPanel.Padding = new System.Windows.Forms.Padding(12);
            this.cameraBodyPanel.Size = new System.Drawing.Size(994, 560);
            this.cameraBodyPanel.TabIndex = 1;
            //
            // configCamera
            //
            this.configCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configCamera.Location = new System.Drawing.Point(12, 12);
            this.configCamera.Name = "configCamera";
            this.configCamera.Size = new System.Drawing.Size(970, 536);
            this.configCamera.TabIndex = 0;
            //
            // cameraHeaderPanel
            //
            this.cameraHeaderPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.cameraHeaderPanel.Controls.Add(this.cameraDescriptionLabel);
            this.cameraHeaderPanel.Controls.Add(this.cameraTitleLabel);
            this.cameraHeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.cameraHeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.cameraHeaderPanel.Name = "cameraHeaderPanel";
            this.cameraHeaderPanel.Padding = new System.Windows.Forms.Padding(20, 12, 20, 8);
            this.cameraHeaderPanel.Size = new System.Drawing.Size(994, 76);
            this.cameraHeaderPanel.TabIndex = 0;
            //
            // cameraDescriptionLabel
            //
            this.cameraDescriptionLabel.Appearance.ForeColor = System.Drawing.Color.DimGray;
            this.cameraDescriptionLabel.Appearance.Options.UseForeColor = true;
            this.cameraDescriptionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.cameraDescriptionLabel.Location = new System.Drawing.Point(20, 40);
            this.cameraDescriptionLabel.Name = "cameraDescriptionLabel";
            this.cameraDescriptionLabel.Size = new System.Drawing.Size(313, 13);
            this.cameraDescriptionLabel.TabIndex = 1;
            this.cameraDescriptionLabel.Text = "Cấu hình thiết bị, format, khung hình và hiệu chỉnh ảnh.";
            //
            // cameraTitleLabel
            //
            this.cameraTitleLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.cameraTitleLabel.Appearance.Options.UseFont = true;
            this.cameraTitleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.cameraTitleLabel.Location = new System.Drawing.Point(20, 12);
            this.cameraTitleLabel.Name = "cameraTitleLabel";
            this.cameraTitleLabel.Size = new System.Drawing.Size(153, 21);
            this.cameraTitleLabel.TabIndex = 0;
            this.cameraTitleLabel.Text = "Camera và hình ảnh";
            //
            // shortcutPrinterTabPage
            //
            this.shortcutPrinterTabPage.Controls.Add(this.shortcutBodyPanel);
            this.shortcutPrinterTabPage.Controls.Add(this.shortcutHeaderPanel);
            this.shortcutPrinterTabPage.Name = "shortcutPrinterTabPage";
            this.shortcutPrinterTabPage.Size = new System.Drawing.Size(994, 636);
            this.shortcutPrinterTabPage.Text = "Phím tắt && Máy in";
            //
            // shortcutBodyPanel
            //
            this.shortcutBodyPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.shortcutBodyPanel.Controls.Add(this.shortcutMainLayoutPanel);
            this.shortcutBodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shortcutBodyPanel.Location = new System.Drawing.Point(0, 76);
            this.shortcutBodyPanel.Name = "shortcutBodyPanel";
            this.shortcutBodyPanel.Padding = new System.Windows.Forms.Padding(24, 18, 24, 18);
            this.shortcutBodyPanel.Size = new System.Drawing.Size(994, 560);
            this.shortcutBodyPanel.TabIndex = 1;
            //
            // shortcutMainLayoutPanel
            //
            this.shortcutMainLayoutPanel.ColumnCount = 2;
            this.shortcutMainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.shortcutMainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.shortcutMainLayoutPanel.Controls.Add(this.shortcutLeftColumnPanel, 0, 0);
            this.shortcutMainLayoutPanel.Controls.Add(this.shortcutRightColumnPanel, 1, 0);
            this.shortcutMainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shortcutMainLayoutPanel.Location = new System.Drawing.Point(24, 18);
            this.shortcutMainLayoutPanel.Name = "shortcutMainLayoutPanel";
            this.shortcutMainLayoutPanel.RowCount = 1;
            this.shortcutMainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.shortcutMainLayoutPanel.Size = new System.Drawing.Size(946, 524);
            this.shortcutMainLayoutPanel.TabIndex = 0;
            //
            // shortcutLeftColumnPanel
            //
            this.shortcutLeftColumnPanel.AutoSize = true;
            this.shortcutLeftColumnPanel.ColumnCount = 1;
            this.shortcutLeftColumnPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.shortcutLeftColumnPanel.Controls.Add(this.worklistGroupControl, 0, 0);
            this.shortcutLeftColumnPanel.Controls.Add(this.cameraShortcutGroupControl, 0, 1);
            this.shortcutLeftColumnPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.shortcutLeftColumnPanel.Location = new System.Drawing.Point(0, 0);
            this.shortcutLeftColumnPanel.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.shortcutLeftColumnPanel.Name = "shortcutLeftColumnPanel";
            this.shortcutLeftColumnPanel.RowCount = 2;
            this.shortcutLeftColumnPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.shortcutLeftColumnPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 176F));
            this.shortcutLeftColumnPanel.Size = new System.Drawing.Size(463, 268);
            this.shortcutLeftColumnPanel.TabIndex = 0;
            //
            // worklistGroupControl
            //
            this.worklistGroupControl.Controls.Add(this.worklistShortcutTableLayoutPanel);
            this.worklistGroupControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.worklistGroupControl.Location = new System.Drawing.Point(0, 0);
            this.worklistGroupControl.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.worklistGroupControl.Name = "worklistGroupControl";
            this.worklistGroupControl.Padding = new System.Windows.Forms.Padding(12, 8, 12, 12);
            this.worklistGroupControl.Size = new System.Drawing.Size(463, 82);
            this.worklistGroupControl.TabIndex = 0;
            this.worklistGroupControl.Text = "Danh sách chỉ định";
            //
            // worklistShortcutTableLayoutPanel
            //
            this.worklistShortcutTableLayoutPanel.ColumnCount = 2;
            this.worklistShortcutTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.worklistShortcutTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.worklistShortcutTableLayoutPanel.Controls.Add(this.searchKeyLabel, 0, 0);
            this.worklistShortcutTableLayoutPanel.Controls.Add(this.searchKeyComboBox, 1, 0);
            this.worklistShortcutTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.worklistShortcutTableLayoutPanel.Location = new System.Drawing.Point(14, 29);
            this.worklistShortcutTableLayoutPanel.Name = "worklistShortcutTableLayoutPanel";
            this.worklistShortcutTableLayoutPanel.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.worklistShortcutTableLayoutPanel.RowCount = 1;
            this.worklistShortcutTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.worklistShortcutTableLayoutPanel.Size = new System.Drawing.Size(435, 39);
            this.worklistShortcutTableLayoutPanel.TabIndex = 0;
            //
            // searchKeyLabel
            //
            this.searchKeyLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.searchKeyLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.searchKeyLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchKeyLabel.Location = new System.Drawing.Point(3, 11);
            this.searchKeyLabel.Name = "searchKeyLabel";
            this.searchKeyLabel.Size = new System.Drawing.Size(317, 32);
            this.searchKeyLabel.TabIndex = 0;
            this.searchKeyLabel.Text = "Tìm kiếm Worklist";
            //
            // searchKeyComboBox
            //
            this.searchKeyComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchKeyComboBox.Location = new System.Drawing.Point(331, 13);
            this.searchKeyComboBox.Margin = new System.Windows.Forms.Padding(8, 5, 0, 5);
            this.searchKeyComboBox.Name = "searchKeyComboBox";
            this.searchKeyComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.searchKeyComboBox.Properties.Items.AddRange(new object[] {
            "Escape", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12"});
            this.searchKeyComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.searchKeyComboBox.Size = new System.Drawing.Size(104, 20);
            this.searchKeyComboBox.TabIndex = 1;
            //
            // cameraShortcutGroupControl
            //
            this.cameraShortcutGroupControl.Controls.Add(this.cameraShortcutTableLayoutPanel);
            this.cameraShortcutGroupControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.cameraShortcutGroupControl.Location = new System.Drawing.Point(0, 92);
            this.cameraShortcutGroupControl.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.cameraShortcutGroupControl.Name = "cameraShortcutGroupControl";
            this.cameraShortcutGroupControl.Padding = new System.Windows.Forms.Padding(12, 8, 12, 12);
            this.cameraShortcutGroupControl.Size = new System.Drawing.Size(463, 166);
            this.cameraShortcutGroupControl.TabIndex = 1;
            this.cameraShortcutGroupControl.Text = "Camera";
            //
            // cameraShortcutTableLayoutPanel
            //
            this.cameraShortcutTableLayoutPanel.ColumnCount = 2;
            this.cameraShortcutTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.cameraShortcutTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.cameraShortcutTableLayoutPanel.Controls.Add(this.linkCameraKeyLabel, 0, 0);
            this.cameraShortcutTableLayoutPanel.Controls.Add(this.linkCameraKeyComboBox, 1, 0);
            this.cameraShortcutTableLayoutPanel.Controls.Add(this.snapshotKeyLabel, 0, 1);
            this.cameraShortcutTableLayoutPanel.Controls.Add(this.snapshotKeyComboBox, 1, 1);
            this.cameraShortcutTableLayoutPanel.Controls.Add(this.stopCameraKeyLabel, 0, 2);
            this.cameraShortcutTableLayoutPanel.Controls.Add(this.stopCameraKeyComboBox, 1, 2);
            this.cameraShortcutTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraShortcutTableLayoutPanel.Location = new System.Drawing.Point(14, 29);
            this.cameraShortcutTableLayoutPanel.Name = "cameraShortcutTableLayoutPanel";
            this.cameraShortcutTableLayoutPanel.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.cameraShortcutTableLayoutPanel.RowCount = 3;
            this.cameraShortcutTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.cameraShortcutTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.cameraShortcutTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.cameraShortcutTableLayoutPanel.Size = new System.Drawing.Size(435, 123);
            this.cameraShortcutTableLayoutPanel.TabIndex = 0;
            //
            // linkCameraKeyLabel
            //
            this.linkCameraKeyLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.linkCameraKeyLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.linkCameraKeyLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkCameraKeyLabel.Location = new System.Drawing.Point(3, 11);
            this.linkCameraKeyLabel.Name = "linkCameraKeyLabel";
            this.linkCameraKeyLabel.Size = new System.Drawing.Size(317, 32);
            this.linkCameraKeyLabel.TabIndex = 0;
            this.linkCameraKeyLabel.Text = "Kết nối Camera";
            //
            // linkCameraKeyComboBox
            //
            this.linkCameraKeyComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkCameraKeyComboBox.Location = new System.Drawing.Point(331, 13);
            this.linkCameraKeyComboBox.Margin = new System.Windows.Forms.Padding(8, 5, 0, 5);
            this.linkCameraKeyComboBox.Name = "linkCameraKeyComboBox";
            this.linkCameraKeyComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.linkCameraKeyComboBox.Properties.Items.AddRange(new object[] {
            "Escape", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12"});
            this.linkCameraKeyComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.linkCameraKeyComboBox.Size = new System.Drawing.Size(104, 20);
            this.linkCameraKeyComboBox.TabIndex = 1;
            //
            // snapshotKeyLabel
            //
            this.snapshotKeyLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.snapshotKeyLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.snapshotKeyLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.snapshotKeyLabel.Location = new System.Drawing.Point(3, 49);
            this.snapshotKeyLabel.Name = "snapshotKeyLabel";
            this.snapshotKeyLabel.Size = new System.Drawing.Size(317, 32);
            this.snapshotKeyLabel.TabIndex = 2;
            this.snapshotKeyLabel.Text = "Chụp ảnh";
            //
            // snapshotKeyComboBox
            //
            this.snapshotKeyComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.snapshotKeyComboBox.Location = new System.Drawing.Point(331, 51);
            this.snapshotKeyComboBox.Margin = new System.Windows.Forms.Padding(8, 5, 0, 5);
            this.snapshotKeyComboBox.Name = "snapshotKeyComboBox";
            this.snapshotKeyComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.snapshotKeyComboBox.Properties.Items.AddRange(new object[] {
            "Escape", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12"});
            this.snapshotKeyComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.snapshotKeyComboBox.Size = new System.Drawing.Size(104, 20);
            this.snapshotKeyComboBox.TabIndex = 3;
            //
            // stopCameraKeyLabel
            //
            this.stopCameraKeyLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.stopCameraKeyLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.stopCameraKeyLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stopCameraKeyLabel.Location = new System.Drawing.Point(3, 87);
            this.stopCameraKeyLabel.Name = "stopCameraKeyLabel";
            this.stopCameraKeyLabel.Size = new System.Drawing.Size(317, 41);
            this.stopCameraKeyLabel.TabIndex = 4;
            this.stopCameraKeyLabel.Text = "Dừng Camera";
            //
            // stopCameraKeyComboBox
            //
            this.stopCameraKeyComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stopCameraKeyComboBox.Location = new System.Drawing.Point(331, 89);
            this.stopCameraKeyComboBox.Margin = new System.Windows.Forms.Padding(8, 5, 0, 5);
            this.stopCameraKeyComboBox.Name = "stopCameraKeyComboBox";
            this.stopCameraKeyComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.stopCameraKeyComboBox.Properties.Items.AddRange(new object[] {
            "Escape", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12"});
            this.stopCameraKeyComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.stopCameraKeyComboBox.Size = new System.Drawing.Size(104, 20);
            this.stopCameraKeyComboBox.TabIndex = 5;
            //
            // shortcutRightColumnPanel
            //
            this.shortcutRightColumnPanel.AutoSize = true;
            this.shortcutRightColumnPanel.ColumnCount = 1;
            this.shortcutRightColumnPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.shortcutRightColumnPanel.Controls.Add(this.conclusionGroupControl, 0, 0);
            this.shortcutRightColumnPanel.Controls.Add(this.printerGroupControl, 0, 1);
            this.shortcutRightColumnPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.shortcutRightColumnPanel.Location = new System.Drawing.Point(483, 0);
            this.shortcutRightColumnPanel.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.shortcutRightColumnPanel.Name = "shortcutRightColumnPanel";
            this.shortcutRightColumnPanel.RowCount = 2;
            this.shortcutRightColumnPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 302F));
            this.shortcutRightColumnPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 102F));
            this.shortcutRightColumnPanel.Size = new System.Drawing.Size(463, 404);
            this.shortcutRightColumnPanel.TabIndex = 1;
            //
            // conclusionGroupControl
            //
            this.conclusionGroupControl.Controls.Add(this.conclusionShortcutTableLayoutPanel);
            this.conclusionGroupControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.conclusionGroupControl.Location = new System.Drawing.Point(0, 0);
            this.conclusionGroupControl.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.conclusionGroupControl.Name = "conclusionGroupControl";
            this.conclusionGroupControl.Padding = new System.Windows.Forms.Padding(12, 8, 12, 12);
            this.conclusionGroupControl.Size = new System.Drawing.Size(463, 292);
            this.conclusionGroupControl.TabIndex = 0;
            this.conclusionGroupControl.Text = "Màn hình kết luận";
            //
            // conclusionShortcutTableLayoutPanel
            //
            this.conclusionShortcutTableLayoutPanel.ColumnCount = 2;
            this.conclusionShortcutTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.conclusionShortcutTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.conclusionShortcutTableLayoutPanel.Controls.Add(this.signKeyLabel, 0, 0);
            this.conclusionShortcutTableLayoutPanel.Controls.Add(this.signKeyComboBox, 1, 0);
            this.conclusionShortcutTableLayoutPanel.Controls.Add(this.printKeyLabel, 0, 1);
            this.conclusionShortcutTableLayoutPanel.Controls.Add(this.printKeyComboBox, 1, 1);
            this.conclusionShortcutTableLayoutPanel.Controls.Add(this.draftKeyLabel, 0, 2);
            this.conclusionShortcutTableLayoutPanel.Controls.Add(this.draftKeyComboBox, 1, 2);
            this.conclusionShortcutTableLayoutPanel.Controls.Add(this.exitKeyLabel, 0, 3);
            this.conclusionShortcutTableLayoutPanel.Controls.Add(this.exitKeyComboBox, 1, 3);
            this.conclusionShortcutTableLayoutPanel.Controls.Add(this.previewKeyLabel, 0, 4);
            this.conclusionShortcutTableLayoutPanel.Controls.Add(this.previewKeyComboBox, 1, 4);
            this.conclusionShortcutTableLayoutPanel.Controls.Add(this.captureImageKeyLabel, 0, 5);
            this.conclusionShortcutTableLayoutPanel.Controls.Add(this.captureImageKeyComboBox, 1, 5);
            this.conclusionShortcutTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conclusionShortcutTableLayoutPanel.Location = new System.Drawing.Point(14, 29);
            this.conclusionShortcutTableLayoutPanel.Name = "conclusionShortcutTableLayoutPanel";
            this.conclusionShortcutTableLayoutPanel.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.conclusionShortcutTableLayoutPanel.RowCount = 6;
            this.conclusionShortcutTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.conclusionShortcutTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.conclusionShortcutTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.conclusionShortcutTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.conclusionShortcutTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.conclusionShortcutTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.conclusionShortcutTableLayoutPanel.Size = new System.Drawing.Size(435, 249);
            this.conclusionShortcutTableLayoutPanel.TabIndex = 0;
            //
            // signKeyLabel
            //
            this.signKeyLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.signKeyLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.signKeyLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.signKeyLabel.Location = new System.Drawing.Point(3, 11);
            this.signKeyLabel.Name = "signKeyLabel";
            this.signKeyLabel.Size = new System.Drawing.Size(317, 32);
            this.signKeyLabel.TabIndex = 0;
            this.signKeyLabel.Text = "Ký số";
            //
            // signKeyComboBox
            //
            this.signKeyComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.signKeyComboBox.Location = new System.Drawing.Point(331, 13);
            this.signKeyComboBox.Margin = new System.Windows.Forms.Padding(8, 5, 0, 5);
            this.signKeyComboBox.Name = "signKeyComboBox";
            this.signKeyComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.signKeyComboBox.Properties.Items.AddRange(new object[] {
            "Escape", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12"});
            this.signKeyComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.signKeyComboBox.Size = new System.Drawing.Size(104, 20);
            this.signKeyComboBox.TabIndex = 1;
            //
            // printKeyLabel
            //
            this.printKeyLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.printKeyLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.printKeyLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printKeyLabel.Location = new System.Drawing.Point(3, 49);
            this.printKeyLabel.Name = "printKeyLabel";
            this.printKeyLabel.Size = new System.Drawing.Size(317, 32);
            this.printKeyLabel.TabIndex = 2;
            this.printKeyLabel.Text = "In kết quả";
            //
            // printKeyComboBox
            //
            this.printKeyComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printKeyComboBox.Location = new System.Drawing.Point(331, 51);
            this.printKeyComboBox.Margin = new System.Windows.Forms.Padding(8, 5, 0, 5);
            this.printKeyComboBox.Name = "printKeyComboBox";
            this.printKeyComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.printKeyComboBox.Properties.Items.AddRange(new object[] {
            "Escape", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12"});
            this.printKeyComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.printKeyComboBox.Size = new System.Drawing.Size(104, 20);
            this.printKeyComboBox.TabIndex = 3;
            //
            // draftKeyLabel
            //
            this.draftKeyLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.draftKeyLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.draftKeyLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.draftKeyLabel.Location = new System.Drawing.Point(3, 87);
            this.draftKeyLabel.Name = "draftKeyLabel";
            this.draftKeyLabel.Size = new System.Drawing.Size(317, 32);
            this.draftKeyLabel.TabIndex = 4;
            this.draftKeyLabel.Text = "Lưu nháp";
            //
            // draftKeyComboBox
            //
            this.draftKeyComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.draftKeyComboBox.Location = new System.Drawing.Point(331, 89);
            this.draftKeyComboBox.Margin = new System.Windows.Forms.Padding(8, 5, 0, 5);
            this.draftKeyComboBox.Name = "draftKeyComboBox";
            this.draftKeyComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.draftKeyComboBox.Properties.Items.AddRange(new object[] {
            "Escape", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12"});
            this.draftKeyComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.draftKeyComboBox.Size = new System.Drawing.Size(104, 20);
            this.draftKeyComboBox.TabIndex = 5;
            //
            // exitKeyLabel
            //
            this.exitKeyLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.exitKeyLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.exitKeyLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exitKeyLabel.Location = new System.Drawing.Point(3, 125);
            this.exitKeyLabel.Name = "exitKeyLabel";
            this.exitKeyLabel.Size = new System.Drawing.Size(317, 32);
            this.exitKeyLabel.TabIndex = 6;
            this.exitKeyLabel.Text = "Đóng màn hình";
            //
            // exitKeyComboBox
            //
            this.exitKeyComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exitKeyComboBox.Location = new System.Drawing.Point(331, 127);
            this.exitKeyComboBox.Margin = new System.Windows.Forms.Padding(8, 5, 0, 5);
            this.exitKeyComboBox.Name = "exitKeyComboBox";
            this.exitKeyComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.exitKeyComboBox.Properties.Items.AddRange(new object[] {
            "Escape", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12"});
            this.exitKeyComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.exitKeyComboBox.Size = new System.Drawing.Size(104, 20);
            this.exitKeyComboBox.TabIndex = 7;
            //
            // previewKeyLabel
            //
            this.previewKeyLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.previewKeyLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.previewKeyLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewKeyLabel.Location = new System.Drawing.Point(3, 163);
            this.previewKeyLabel.Name = "previewKeyLabel";
            this.previewKeyLabel.Size = new System.Drawing.Size(317, 32);
            this.previewKeyLabel.TabIndex = 8;
            this.previewKeyLabel.Text = "Xem trước";
            //
            // previewKeyComboBox
            //
            this.previewKeyComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewKeyComboBox.Location = new System.Drawing.Point(331, 165);
            this.previewKeyComboBox.Margin = new System.Windows.Forms.Padding(8, 5, 0, 5);
            this.previewKeyComboBox.Name = "previewKeyComboBox";
            this.previewKeyComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.previewKeyComboBox.Properties.Items.AddRange(new object[] {
            "Escape", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12"});
            this.previewKeyComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.previewKeyComboBox.Size = new System.Drawing.Size(104, 20);
            this.previewKeyComboBox.TabIndex = 9;
            //
            // captureImageKeyLabel
            //
            this.captureImageKeyLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.captureImageKeyLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.captureImageKeyLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.captureImageKeyLabel.Location = new System.Drawing.Point(3, 201);
            this.captureImageKeyLabel.Name = "captureImageKeyLabel";
            this.captureImageKeyLabel.Size = new System.Drawing.Size(317, 51);
            this.captureImageKeyLabel.TabIndex = 10;
            this.captureImageKeyLabel.Text = "Lấy ảnh";
            //
            // captureImageKeyComboBox
            //
            this.captureImageKeyComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.captureImageKeyComboBox.Location = new System.Drawing.Point(331, 203);
            this.captureImageKeyComboBox.Margin = new System.Windows.Forms.Padding(8, 5, 0, 5);
            this.captureImageKeyComboBox.Name = "captureImageKeyComboBox";
            this.captureImageKeyComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.captureImageKeyComboBox.Properties.Items.AddRange(new object[] {
            "Escape", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12"});
            this.captureImageKeyComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.captureImageKeyComboBox.Size = new System.Drawing.Size(104, 20);
            this.captureImageKeyComboBox.TabIndex = 11;
            //
            // printerGroupControl
            //
            this.printerGroupControl.Controls.Add(this.printerComboBox);
            this.printerGroupControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.printerGroupControl.Location = new System.Drawing.Point(0, 312);
            this.printerGroupControl.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.printerGroupControl.Name = "printerGroupControl";
            this.printerGroupControl.Padding = new System.Windows.Forms.Padding(12, 8, 12, 12);
            this.printerGroupControl.Size = new System.Drawing.Size(463, 92);
            this.printerGroupControl.TabIndex = 1;
            this.printerGroupControl.Text = "Máy in";
            //
            // printerComboBox
            //
            this.printerComboBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.printerComboBox.Location = new System.Drawing.Point(14, 35);
            this.printerComboBox.Margin = new System.Windows.Forms.Padding(0, 14, 0, 0);
            this.printerComboBox.Name = "printerComboBox";
            this.printerComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.printerComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.printerComboBox.Size = new System.Drawing.Size(435, 20);
            this.printerComboBox.TabIndex = 0;
            //
            // shortcutHeaderPanel
            //
            this.shortcutHeaderPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.shortcutHeaderPanel.Controls.Add(this.shortcutDescriptionLabel);
            this.shortcutHeaderPanel.Controls.Add(this.shortcutTitleLabel);
            this.shortcutHeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.shortcutHeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.shortcutHeaderPanel.Name = "shortcutHeaderPanel";
            this.shortcutHeaderPanel.Padding = new System.Windows.Forms.Padding(20, 12, 20, 8);
            this.shortcutHeaderPanel.Size = new System.Drawing.Size(994, 76);
            this.shortcutHeaderPanel.TabIndex = 0;
            //
            // shortcutDescriptionLabel
            //
            this.shortcutDescriptionLabel.Appearance.ForeColor = System.Drawing.Color.DimGray;
            this.shortcutDescriptionLabel.Appearance.Options.UseForeColor = true;
            this.shortcutDescriptionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.shortcutDescriptionLabel.Location = new System.Drawing.Point(20, 40);
            this.shortcutDescriptionLabel.Name = "shortcutDescriptionLabel";
            this.shortcutDescriptionLabel.Size = new System.Drawing.Size(393, 13);
            this.shortcutDescriptionLabel.TabIndex = 1;
            this.shortcutDescriptionLabel.Text = "Thiết lập thao tác nhanh và máy in kết quả trên máy trạm này.";
            //
            // shortcutTitleLabel
            //
            this.shortcutTitleLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.shortcutTitleLabel.Appearance.Options.UseFont = true;
            this.shortcutTitleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.shortcutTitleLabel.Location = new System.Drawing.Point(20, 12);
            this.shortcutTitleLabel.Name = "shortcutTitleLabel";
            this.shortcutTitleLabel.Size = new System.Drawing.Size(252, 21);
            this.shortcutTitleLabel.TabIndex = 0;
            this.shortcutTitleLabel.Text = "Phím tắt và máy in mặc định";
            //
            // SystemSettingsDialog
            //
            this.AcceptButton = this.saveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(1080, 700);
            this.Controls.Add(this.settingsTabControl);
            this.Controls.Add(this.footerPanel);
            this.MinimumSize = new System.Drawing.Size(940, 620);
            this.Name = "SystemSettingsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cài đặt hệ thống";
            ((System.ComponentModel.ISupportInitialize)(this.footerPanel)).EndInit();
            this.footerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.settingsTabControl)).EndInit();
            this.settingsTabControl.ResumeLayout(false);
            this.hisRisTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.hisRisBodyPanel)).EndInit();
            this.hisRisBodyPanel.ResumeLayout(false);
            this.hisRisBodyPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serverAddressTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paymentCheckTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hisRisHeaderPanel)).EndInit();
            this.hisRisHeaderPanel.ResumeLayout(false);
            this.hisRisHeaderPanel.PerformLayout();
            this.cameraTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cameraBodyPanel)).EndInit();
            this.cameraBodyPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cameraHeaderPanel)).EndInit();
            this.cameraHeaderPanel.ResumeLayout(false);
            this.cameraHeaderPanel.PerformLayout();
            this.shortcutPrinterTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shortcutBodyPanel)).EndInit();
            this.shortcutBodyPanel.ResumeLayout(false);
            this.worklistGroupControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.worklistGroupControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchKeyComboBox.Properties)).EndInit();
            this.cameraShortcutGroupControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cameraShortcutGroupControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkCameraKeyComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.snapshotKeyComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopCameraKeyComboBox.Properties)).EndInit();
            this.conclusionGroupControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.conclusionGroupControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.signKeyComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printKeyComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.draftKeyComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitKeyComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewKeyComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.captureImageKeyComboBox.Properties)).EndInit();
            this.printerGroupControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.printerGroupControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printerComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shortcutHeaderPanel)).EndInit();
            this.shortcutHeaderPanel.ResumeLayout(false);
            this.shortcutHeaderPanel.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
