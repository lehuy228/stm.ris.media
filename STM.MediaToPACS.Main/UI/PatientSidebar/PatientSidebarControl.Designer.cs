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
        private DevExpress.XtraTab.XtraTabPage _tabLog;
        private DevExpress.XtraTreeList.TreeList _treeHistory;
        private DevExpress.XtraTreeList.Columns.TreeListColumn _colContent;
        private DevExpress.XtraTreeList.Columns.TreeListColumn _colDate;
        private DevExpress.XtraTreeList.Columns.TreeListColumn _colStatus;
        private System.Windows.Forms.Panel _paramsHostPanel;
        private DevExpress.XtraEditors.LabelControl _lblLogPlaceholder;
        private DevExpress.XtraGrid.GridControl _gridLog;
        private DevExpress.XtraGrid.Views.Grid.GridView _gridViewLog;
        private DevExpress.XtraGrid.Columns.GridColumn _colLogTime;
        private DevExpress.XtraGrid.Columns.GridColumn _colLogContent;
        private DevExpress.XtraGrid.Columns.GridColumn _colLogUser;
        private System.Windows.Forms.Panel _logHeaderPanel;
        private DevExpress.XtraEditors.LabelControl _btnReloadLog;

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
            this._tabLog = new DevExpress.XtraTab.XtraTabPage();
            this._lblLogPlaceholder = new DevExpress.XtraEditors.LabelControl();
            this._gridLog = new DevExpress.XtraGrid.GridControl();
            this._gridViewLog = new DevExpress.XtraGrid.Views.Grid.GridView();
            this._colLogTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this._colLogContent = new DevExpress.XtraGrid.Columns.GridColumn();
            this._colLogUser = new DevExpress.XtraGrid.Columns.GridColumn();
            this._logHeaderPanel = new System.Windows.Forms.Panel();
            this._btnReloadLog = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this._gridLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._gridViewLog)).BeginInit();
            this._logHeaderPanel.SuspendLayout();
            this._headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tabControl)).BeginInit();
            this._tabControl.SuspendLayout();
            this._tabHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._treeHistory)).BeginInit();
            this._tabParams.SuspendLayout();
            this._tabLog.SuspendLayout();
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
            this._tabControl.SelectedTabPage = this._tabParams;
            this._tabControl.Size = new System.Drawing.Size(320, 595);
            this._tabControl.TabIndex = 1;
            this._tabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this._tabParams,
            this._tabHistory,
            this._tabLog});
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
            // _tabLog
            //
            this._tabLog.Controls.Add(this._gridLog);
            this._tabLog.Controls.Add(this._lblLogPlaceholder);
            this._tabLog.Controls.Add(this._logHeaderPanel);
            this._tabLog.Name = "_tabLog";
            this._tabLog.Size = new System.Drawing.Size(313, 553);
            this._tabLog.Text = "Nhật ký";
            //
            // _logHeaderPanel
            //
            this._logHeaderPanel.Controls.Add(this._btnReloadLog);
            this._logHeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._logHeaderPanel.Name = "_logHeaderPanel";
            this._logHeaderPanel.Size = new System.Drawing.Size(313, 24);
            //
            // _btnReloadLog
            //
            this._btnReloadLog.Appearance.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._btnReloadLog.Appearance.ForeColor = System.Drawing.Color.FromArgb(15, 72, 116);
            this._btnReloadLog.Appearance.Options.UseFont = true;
            this._btnReloadLog.Appearance.Options.UseForeColor = true;
            this._btnReloadLog.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnReloadLog.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnReloadLog.Name = "_btnReloadLog";
            this._btnReloadLog.Size = new System.Drawing.Size(70, 24);
            this._btnReloadLog.Text = "⟳ Tải lại";
            this._btnReloadLog.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this._btnReloadLog.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this._btnReloadLog.Click += new System.EventHandler(this.BtnReloadLog_Click);
            //
            // _gridLog
            //
            this._gridLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridLog.MainView = this._gridViewLog;
            this._gridLog.Name = "_gridLog";
            this._gridLog.Size = new System.Drawing.Size(313, 529);
            this._gridLog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this._gridViewLog});
            //
            // _gridViewLog
            //
            this._gridViewLog.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this._colLogTime,
            this._colLogContent,
            this._colLogUser});
            this._gridViewLog.GridControl = this._gridLog;
            this._gridViewLog.Name = "_gridViewLog";
            this._gridViewLog.OptionsBehavior.Editable = false;
            this._gridViewLog.OptionsSelection.EnableAppearanceFocusedCell = false;
            this._gridViewLog.OptionsView.ShowGroupPanel = false;
            this._gridViewLog.OptionsView.ShowIndicator = false;
            this._gridViewLog.OptionsView.RowAutoHeight = true;
            //
            // _colLogTime
            //
            this._colLogTime.Caption = "Thời gian";
            this._colLogTime.FieldName = "TimeText";
            this._colLogTime.MinWidth = 80;
            this._colLogTime.Name = "_colLogTime";
            this._colLogTime.Visible = true;
            this._colLogTime.VisibleIndex = 0;
            this._colLogTime.Width = 95;
            //
            // _colLogContent
            //
            this._colLogContent.Caption = "Nội dung";
            this._colLogContent.FieldName = "Content";
            this._colLogContent.MinWidth = 110;
            this._colLogContent.Name = "_colLogContent";
            this._colLogContent.Visible = true;
            this._colLogContent.VisibleIndex = 1;
            this._colLogContent.Width = 150;
            //
            // _colLogUser
            //
            this._colLogUser.Caption = "Người thực hiện";
            this._colLogUser.FieldName = "UserText";
            this._colLogUser.MinWidth = 60;
            this._colLogUser.Name = "_colLogUser";
            this._colLogUser.Visible = true;
            this._colLogUser.VisibleIndex = 2;
            this._colLogUser.Width = 80;
            //
            // _lblLogPlaceholder
            //
            this._lblLogPlaceholder.Appearance.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._lblLogPlaceholder.Appearance.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this._lblLogPlaceholder.Appearance.Options.UseFont = true;
            this._lblLogPlaceholder.Appearance.Options.UseForeColor = true;
            this._lblLogPlaceholder.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this._lblLogPlaceholder.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblLogPlaceholder.Name = "_lblLogPlaceholder";
            this._lblLogPlaceholder.Size = new System.Drawing.Size(313, 553);
            this._lblLogPlaceholder.Text = "Chưa có nhật ký";
            this._lblLogPlaceholder.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this._lblLogPlaceholder.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
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
            this._logHeaderPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gridViewLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._gridLog)).EndInit();
            this._tabLog.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
