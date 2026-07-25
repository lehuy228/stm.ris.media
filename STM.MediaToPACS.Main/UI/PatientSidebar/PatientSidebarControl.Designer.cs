namespace STM.MediaToPACS.Main.UI.PatientSidebar
{
    partial class PatientSidebarControl
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel _headerPanel;
        private DevExpress.XtraEditors.LabelControl _lblTitle;
        private DevExpress.XtraEditors.LabelControl _btnPin;
        private DevExpress.XtraEditors.LabelControl _btnClose;
        private DevExpress.XtraTab.XtraTabControl _tabControl;
        private DevExpress.XtraTab.XtraTabPage _tabHistory;
        private DevExpress.XtraTab.XtraTabPage _tabParams;
        private DevExpress.XtraTreeList.TreeList _treeHistory;
        private DevExpress.XtraTreeList.Columns.TreeListColumn _colContent;
        private DevExpress.XtraTreeList.Columns.TreeListColumn _colDate;
        private DevExpress.XtraTreeList.Columns.TreeListColumn _colStatus;
        private System.Windows.Forms.Panel _paramsHostPanel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._headerPanel = new System.Windows.Forms.Panel();
            this._btnClose = new DevExpress.XtraEditors.LabelControl();
            this._btnPin = new DevExpress.XtraEditors.LabelControl();
            this._lblTitle = new DevExpress.XtraEditors.LabelControl();
            this._tabControl = new DevExpress.XtraTab.XtraTabControl();
            this._tabHistory = new DevExpress.XtraTab.XtraTabPage();
            this._treeHistory = new DevExpress.XtraTreeList.TreeList();
            this._colContent = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this._colDate = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this._colStatus = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this._tabParams = new DevExpress.XtraTab.XtraTabPage();
            this._paramsHostPanel = new System.Windows.Forms.Panel();
            this._headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tabControl)).BeginInit();
            this._tabControl.SuspendLayout();
            this._tabHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._treeHistory)).BeginInit();
            this._tabParams.SuspendLayout();
            this.SuspendLayout();
            //
            // _headerPanel
            //
            this._headerPanel.BackColor = System.Drawing.Color.FromArgb(15, 72, 116);
            this._headerPanel.Controls.Add(this._btnClose);
            this._headerPanel.Controls.Add(this._btnPin);
            this._headerPanel.Controls.Add(this._lblTitle);
            this._headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._headerPanel.Location = new System.Drawing.Point(0, 0);
            this._headerPanel.Name = "_headerPanel";
            this._headerPanel.Size = new System.Drawing.Size(320, 30);
            this._headerPanel.TabIndex = 0;
            //
            // _btnClose
            //
            this._btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnClose.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this._btnClose.Appearance.ForeColor = System.Drawing.Color.White;
            this._btnClose.Appearance.Options.UseFont = true;
            this._btnClose.Appearance.Options.UseForeColor = true;
            this._btnClose.AppearanceHovered.ForeColor = System.Drawing.Color.FromArgb(200, 220, 240);
            this._btnClose.AppearanceHovered.Options.UseForeColor = true;
            this._btnClose.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this._btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnClose.Location = new System.Drawing.Point(250, 4);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(28, 22);
            this._btnClose.TabIndex = 2;
            this._btnClose.Text = "×";
            this._btnClose.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this._btnClose.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this._btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            //
            // _btnPin
            //
            this._btnPin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnPin.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this._btnPin.Appearance.ForeColor = System.Drawing.Color.White;
            this._btnPin.Appearance.Options.UseFont = true;
            this._btnPin.Appearance.Options.UseForeColor = true;
            this._btnPin.AppearanceHovered.ForeColor = System.Drawing.Color.FromArgb(200, 220, 240);
            this._btnPin.AppearanceHovered.Options.UseForeColor = true;
            this._btnPin.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this._btnPin.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnPin.Location = new System.Drawing.Point(284, 4);
            this._btnPin.Name = "_btnPin";
            this._btnPin.Size = new System.Drawing.Size(28, 22);
            this._btnPin.TabIndex = 1;
            this._btnPin.Text = "○";
            this._btnPin.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this._btnPin.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this._btnPin.Click += new System.EventHandler(this.BtnPin_Click);
            //
            // _lblTitle
            //
            this._lblTitle.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._lblTitle.Appearance.ForeColor = System.Drawing.Color.White;
            this._lblTitle.Appearance.Options.UseFont = true;
            this._lblTitle.Appearance.Options.UseForeColor = true;
            this._lblTitle.Location = new System.Drawing.Point(8, 7);
            this._lblTitle.Name = "_lblTitle";
            this._lblTitle.Size = new System.Drawing.Size(108, 15);
            this._lblTitle.TabIndex = 0;
            this._lblTitle.Text = "Thông tin bệnh nhân";
            //
            // _tabControl
            //
            this._tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabControl.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Left;
            this._tabControl.Location = new System.Drawing.Point(0, 30);
            this._tabControl.Name = "_tabControl";
            this._tabControl.SelectedTabPage = this._tabHistory;
            this._tabControl.Size = new System.Drawing.Size(320, 595);
            this._tabControl.TabIndex = 1;
            this._tabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this._tabHistory,
            this._tabParams});
            this._tabControl.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.TabControl_SelectedPageChanged);
            //
            // _tabHistory
            //
            this._tabHistory.Controls.Add(this._treeHistory);
            this._tabHistory.Name = "_tabHistory";
            this._tabHistory.Size = new System.Drawing.Size(313, 553);
            this._tabHistory.Text = "Lịch sử khám bệnh nhân";
            //
            // _treeHistory
            //
            this._treeHistory.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this._colContent,
            this._colDate,
            this._colStatus});
            this._treeHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this._treeHistory.Location = new System.Drawing.Point(0, 0);
            this._treeHistory.Name = "_treeHistory";
            this._treeHistory.OptionsBehavior.Editable = false;
            this._treeHistory.OptionsSelection.EnableAppearanceFocusedCell = false;
            this._treeHistory.OptionsSelection.EnableAppearanceFocusedRow = false;
            this._treeHistory.OptionsView.ShowIndicator = false;
            this._treeHistory.Size = new System.Drawing.Size(313, 553);
            this._treeHistory.TabIndex = 0;
            this._treeHistory.NodeCellStyle += new DevExpress.XtraTreeList.GetCustomNodeCellStyleEventHandler(this.TreeHistory_NodeCellStyle);
            this._treeHistory.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TreeHistory_MouseDoubleClick);
            //
            // _colContent
            //
            this._colContent.Caption = "Nội dung";
            this._colContent.FieldName = "Content";
            this._colContent.MinWidth = 110;
            this._colContent.Name = "_colContent";
            this._colContent.Visible = true;
            this._colContent.VisibleIndex = 0;
            this._colContent.Width = 150;
            //
            // _colDate
            //
            this._colDate.Caption = "Ngày";
            this._colDate.FieldName = "DateText";
            this._colDate.MinWidth = 65;
            this._colDate.Name = "_colDate";
            this._colDate.Visible = true;
            this._colDate.VisibleIndex = 1;
            this._colDate.Width = 80;
            //
            // _colStatus
            //
            this._colStatus.Caption = "Trạng thái";
            this._colStatus.FieldName = "StatusText";
            this._colStatus.MinWidth = 65;
            this._colStatus.Name = "_colStatus";
            this._colStatus.Visible = true;
            this._colStatus.VisibleIndex = 2;
            this._colStatus.Width = 80;
            //
            // _tabParams
            //
            this._tabParams.Controls.Add(this._paramsHostPanel);
            this._tabParams.Name = "_tabParams";
            this._tabParams.Size = new System.Drawing.Size(313, 553);
            this._tabParams.Text = "Tham số siêu âm";
            //
            // _paramsHostPanel
            //
            this._paramsHostPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._paramsHostPanel.Location = new System.Drawing.Point(0, 0);
            this._paramsHostPanel.Name = "_paramsHostPanel";
            this._paramsHostPanel.Size = new System.Drawing.Size(313, 553);
            this._paramsHostPanel.TabIndex = 0;
            //
            // PatientSidebarControl
            //
            this.Controls.Add(this._tabControl);
            this.Controls.Add(this._headerPanel);
            this.Name = "PatientSidebarControl";
            this.Size = new System.Drawing.Size(320, 625);
            this._headerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._tabControl)).EndInit();
            this._tabControl.ResumeLayout(false);
            this._tabHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._treeHistory)).EndInit();
            this._tabParams.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
