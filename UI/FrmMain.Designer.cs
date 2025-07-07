using PrintToPACSDemo.UI;
namespace PrintToPACSDemo
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this._mmMain = new System.Windows.Forms.MenuStrip();
            this._miFile = new System.Windows.Forms.ToolStripMenuItem();
            this._miOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this._miSaveAsDICOM = new System.Windows.Forms.ToolStripMenuItem();
            this._miStoreToPACS = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._miExit = new System.Windows.Forms.ToolStripMenuItem();
            this._miEdit = new System.Windows.Forms.ToolStripMenuItem();
            this._miPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator25 = new System.Windows.Forms.ToolStripSeparator();
            this._miRotate90 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this._miDeleteAll = new System.Windows.Forms.ToolStripMenuItem();
            this._miDeleteSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._miResetInfo = new System.Windows.Forms.ToolStripMenuItem();
            this._miView = new System.Windows.Forms.ToolStripMenuItem();
            this._miNormal = new System.Windows.Forms.ToolStripMenuItem();
            this._miFit = new System.Windows.Forms.ToolStripMenuItem();
            this._miZoomIn = new System.Windows.Forms.ToolStripMenuItem();
            this._miZoomOut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._miResample = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this._miViewLog = new System.Windows.Forms.ToolStripMenuItem();
            this._miCapture = new System.Windows.Forms.ToolStripMenuItem();
            this._miCaptureActiveWindow = new System.Windows.Forms.ToolStripMenuItem();
            this._miCaptureFullScreen = new System.Windows.Forms.ToolStripMenuItem();
            this._miCaptureSelectedObject = new System.Windows.Forms.ToolStripMenuItem();
            this._miCaptureSelectedArea = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this._miCaptureStopCapture = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
            this._miCaptureOptionsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._miCaptureAreaOptions = new System.Windows.Forms.ToolStripMenuItem();
            this._miCaptureOptions = new System.Windows.Forms.ToolStripMenuItem();
            this._miCaptureObjectOptions = new System.Windows.Forms.ToolStripMenuItem();
            this._miEventsEmf2 = new System.Windows.Forms.ToolStripMenuItem();
            this._miEventsJob2 = new System.Windows.Forms.ToolStripMenuItem();
            this._panelPictureBox = new System.Windows.Forms.Panel();
            this._tbTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this._grpStoreServers = new DevExpress.XtraEditors.GroupControl();
            this._cbStoreServers = new DevExpress.XtraEditors.ComboBoxEdit();
            this._btnCreateConclusion = new DevExpress.XtraEditors.SimpleButton();
            this._btnPushToPACS = new DevExpress.XtraEditors.SimpleButton();
            this.label9 = new DevExpress.XtraEditors.LabelControl();
            this._gbDicomInfo = new DevExpress.XtraEditors.GroupControl();
            this._tbPropertyGrid = new System.Windows.Forms.TableLayoutPanel();
            this.panel6 = new System.Windows.Forms.Panel();
            this._cmbSopClasses = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label7 = new DevExpress.XtraEditors.LabelControl();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.tableLayoutPanelInfo = new System.Windows.Forms.TableLayoutPanel();
            this._grpBoxPictureBox = new DevExpress.XtraEditors.GroupControl();
            this._tbPicture = new System.Windows.Forms.TableLayoutPanel();
            this._btnNext = new DevExpress.XtraEditors.SimpleButton();
            this._btnPrev = new DevExpress.XtraEditors.SimpleButton();
            this._lblPageInfo = new DevExpress.XtraEditors.LabelControl();
            this._btnOpenImage = new DevExpress.XtraEditors.SimpleButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this._fPLRoll = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new DevExpress.XtraEditors.LabelControl();
            this._btnRecord = new DevExpress.XtraEditors.SimpleButton();
            this._btnSnapshot = new DevExpress.XtraEditors.SimpleButton();
            this._btnPause = new DevExpress.XtraEditors.SimpleButton();
            this._cmResultQuery = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._miClearResult = new System.Windows.Forms.ToolStripMenuItem();
            this.discontinueMPPSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._cnmnuClearSearch = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._miClearSearch = new System.Windows.Forms.ToolStripMenuItem();
            this._panelImageList = new System.Windows.Forms.Panel();
            this._cnmnuClearDicom = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._miClearPG = new System.Windows.Forms.ToolStripMenuItem();
            this._gbRecognized = new DevExpress.XtraEditors.GroupControl();
            this.label3 = new DevExpress.XtraEditors.LabelControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox1 = new DevExpress.XtraEditors.GroupControl();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel3 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new DevExpress.XtraEditors.LabelControl();
            this.label5 = new DevExpress.XtraEditors.LabelControl();
            this._toolbarMain = new System.Windows.Forms.ToolStrip();
            this._toolBtnOpenRaster = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator23 = new System.Windows.Forms.ToolStripSeparator();
            this._toolBtnStoreToPacs = new System.Windows.Forms.ToolStripButton();
            this._toolBtnSaveDicom = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this._toolBtnCLearInfo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this._toolBtnDeleteAll = new System.Windows.Forms.ToolStripButton();
            this._toolBtnDeleteSelected = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this._toolBtnRotate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator24 = new System.Windows.Forms.ToolStripSeparator();
            this._toolBtnViewLog = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this._toolBtnSettingsCamera = new System.Windows.Forms.ToolStripButton();
            this._cmListBox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._cmiViewMode = new System.Windows.Forms.ToolStripMenuItem();
            this._cmiExpanded = new System.Windows.Forms.ToolStripMenuItem();
            this._cmiCondensed = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this._cmiDeleteAll = new System.Windows.Forms.ToolStripMenuItem();
            this._cmiDeleteSelected = new System.Windows.Forms.ToolStripMenuItem();
            this._lstBoxPages = new PrintToPACSDemo.UI.ListImageBox();
            this._mmMain.SuspendLayout();
            this._panelPictureBox.SuspendLayout();
            this._tbTableLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grpStoreServers)).BeginInit();
            this._grpStoreServers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._cbStoreServers.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._gbDicomInfo)).BeginInit();
            this._gbDicomInfo.SuspendLayout();
            this._tbPropertyGrid.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._cmbSopClasses.Properties)).BeginInit();
            this.panelInfo.SuspendLayout();
            this.tableLayoutPanelInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grpBoxPictureBox)).BeginInit();
            this._grpBoxPictureBox.SuspendLayout();
            this._tbPicture.SuspendLayout();
            this.panel1.SuspendLayout();
            this._cmResultQuery.SuspendLayout();
            this._cnmnuClearSearch.SuspendLayout();
            this._cnmnuClearDicom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gbRecognized)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).BeginInit();
            this.panel3.SuspendLayout();
            this._toolbarMain.SuspendLayout();
            this._cmListBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // _mmMain
            // 
            this._mmMain.BackColor = System.Drawing.SystemColors.Control;
            this._mmMain.GripMargin = new System.Windows.Forms.Padding(0);
            this._mmMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._mmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._miFile,
            this._miEdit,
            this._miView,
            this._miCapture});
            this._mmMain.Location = new System.Drawing.Point(0, 0);
            this._mmMain.Name = "_mmMain";
            this._mmMain.Padding = new System.Windows.Forms.Padding(5, 1, 0, 1);
            this._mmMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this._mmMain.ShowItemToolTips = true;
            this._mmMain.Size = new System.Drawing.Size(1198, 24);
            this._mmMain.Stretch = false;
            this._mmMain.TabIndex = 0;
            // 
            // _miFile
            // 
            this._miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._miOpen,
            this.toolStripSeparator10,
            this._miSaveAsDICOM,
            this._miStoreToPACS,
            this.toolStripSeparator1,
            this._miExit});
            this._miFile.Name = "_miFile";
            this._miFile.Size = new System.Drawing.Size(37, 22);
            this._miFile.Text = "&File";
            this._miFile.DropDownOpening += new System.EventHandler(this._miFile_DropDownOpening);
            // 
            // _miOpen
            // 
            this._miOpen.Image = ((System.Drawing.Image)(resources.GetObject("_miOpen.Image")));
            this._miOpen.Name = "_miOpen";
            this._miOpen.Size = new System.Drawing.Size(170, 22);
            this._miOpen.Text = "&Open...";
            this._miOpen.Click += new System.EventHandler(this._miOpen_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(167, 6);
            // 
            // _miSaveAsDICOM
            // 
            this._miSaveAsDICOM.Image = ((System.Drawing.Image)(resources.GetObject("_miSaveAsDICOM.Image")));
            this._miSaveAsDICOM.Name = "_miSaveAsDICOM";
            this._miSaveAsDICOM.Size = new System.Drawing.Size(170, 22);
            this._miSaveAsDICOM.Text = "&Save DICOM File...";
            this._miSaveAsDICOM.Click += new System.EventHandler(this._miSaveAsDICOM_Click);
            // 
            // _miStoreToPACS
            // 
            this._miStoreToPACS.Image = ((System.Drawing.Image)(resources.GetObject("_miStoreToPACS.Image")));
            this._miStoreToPACS.Name = "_miStoreToPACS";
            this._miStoreToPACS.Size = new System.Drawing.Size(170, 22);
            this._miStoreToPACS.Text = "Sto&re to PACS";
            this._miStoreToPACS.Click += new System.EventHandler(this._miStoreToPACS_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(167, 6);
            // 
            // _miExit
            // 
            this._miExit.Name = "_miExit";
            this._miExit.Size = new System.Drawing.Size(170, 22);
            this._miExit.Text = "&Exit";
            this._miExit.Click += new System.EventHandler(this._miExit_Click);
            // 
            // _miEdit
            // 
            this._miEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._miPaste,
            this.toolStripSeparator25,
            this._miRotate90,
            this.toolStripSeparator11,
            this._miDeleteAll,
            this._miDeleteSelected,
            this.toolStripSeparator2,
            this._miResetInfo});
            this._miEdit.Name = "_miEdit";
            this._miEdit.Size = new System.Drawing.Size(39, 22);
            this._miEdit.Text = "&Edit";
            this._miEdit.DropDownOpening += new System.EventHandler(this._miEdit_DropDownOpening);
            // 
            // _miPaste
            // 
            this._miPaste.Name = "_miPaste";
            this._miPaste.Size = new System.Drawing.Size(196, 22);
            this._miPaste.Text = "&Paste ";
            this._miPaste.Click += new System.EventHandler(this._miPaste_Click);
            // 
            // toolStripSeparator25
            // 
            this.toolStripSeparator25.Name = "toolStripSeparator25";
            this.toolStripSeparator25.Size = new System.Drawing.Size(193, 6);
            // 
            // _miRotate90
            // 
            this._miRotate90.Image = ((System.Drawing.Image)(resources.GetObject("_miRotate90.Image")));
            this._miRotate90.Name = "_miRotate90";
            this._miRotate90.Size = new System.Drawing.Size(196, 22);
            this._miRotate90.Text = "Ro&tate 90 degree";
            this._miRotate90.Click += new System.EventHandler(this._miRotate90_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(193, 6);
            // 
            // _miDeleteAll
            // 
            this._miDeleteAll.Image = ((System.Drawing.Image)(resources.GetObject("_miDeleteAll.Image")));
            this._miDeleteAll.Name = "_miDeleteAll";
            this._miDeleteAll.Size = new System.Drawing.Size(196, 22);
            this._miDeleteAll.Text = "&Delete All Pages";
            this._miDeleteAll.Click += new System.EventHandler(this._miClearPrintedList_Click);
            // 
            // _miDeleteSelected
            // 
            this._miDeleteSelected.Image = ((System.Drawing.Image)(resources.GetObject("_miDeleteSelected.Image")));
            this._miDeleteSelected.Name = "_miDeleteSelected";
            this._miDeleteSelected.Size = new System.Drawing.Size(196, 22);
            this._miDeleteSelected.Text = "Delete &Selected Page(s)";
            this._miDeleteSelected.Click += new System.EventHandler(this._miDeleteSelected_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(193, 6);
            // 
            // _miResetInfo
            // 
            this._miResetInfo.Image = ((System.Drawing.Image)(resources.GetObject("_miResetInfo.Image")));
            this._miResetInfo.Name = "_miResetInfo";
            this._miResetInfo.Size = new System.Drawing.Size(196, 22);
            this._miResetInfo.Text = "&Reset DICOM Info";
            this._miResetInfo.Click += new System.EventHandler(this._miResetInfo_Click);
            // 
            // _miView
            // 
            this._miView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._miNormal,
            this._miFit,
            this._miZoomIn,
            this._miZoomOut,
            this.toolStripSeparator3,
            this._miResample,
            this.toolStripSeparator12,
            this._miViewLog});
            this._miView.Name = "_miView";
            this._miView.Size = new System.Drawing.Size(44, 22);
            this._miView.Text = "&View";
            this._miView.DropDownOpening += new System.EventHandler(this._miView_DropDownOpening);
            // 
            // _miNormal
            // 
            this._miNormal.Checked = true;
            this._miNormal.CheckState = System.Windows.Forms.CheckState.Checked;
            this._miNormal.Enabled = false;
            this._miNormal.Name = "_miNormal";
            this._miNormal.Size = new System.Drawing.Size(189, 22);
            this._miNormal.Text = "&Normal";
            this._miNormal.Click += new System.EventHandler(this._miNormal_Click);
            // 
            // _miFit
            // 
            this._miFit.Enabled = false;
            this._miFit.Name = "_miFit";
            this._miFit.Size = new System.Drawing.Size(189, 22);
            this._miFit.Text = "&Fit To Window";
            this._miFit.Click += new System.EventHandler(this._miFit_Click);
            // 
            // _miZoomIn
            // 
            this._miZoomIn.Enabled = false;
            this._miZoomIn.Name = "_miZoomIn";
            this._miZoomIn.Size = new System.Drawing.Size(189, 22);
            this._miZoomIn.Text = "Zoom &In (+)";
            this._miZoomIn.Click += new System.EventHandler(this._miZoomIn_Click);
            // 
            // _miZoomOut
            // 
            this._miZoomOut.Enabled = false;
            this._miZoomOut.Name = "_miZoomOut";
            this._miZoomOut.Size = new System.Drawing.Size(189, 22);
            this._miZoomOut.Text = "Zoom &Out (-)";
            this._miZoomOut.Click += new System.EventHandler(this._miZoomOut_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(186, 6);
            // 
            // _miResample
            // 
            this._miResample.Name = "_miResample";
            this._miResample.Size = new System.Drawing.Size(189, 22);
            this._miResample.Text = "&Resample Paint Mode";
            this._miResample.Click += new System.EventHandler(this._miResample_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(186, 6);
            // 
            // _miViewLog
            // 
            this._miViewLog.Image = ((System.Drawing.Image)(resources.GetObject("_miViewLog.Image")));
            this._miViewLog.Name = "_miViewLog";
            this._miViewLog.Size = new System.Drawing.Size(189, 22);
            this._miViewLog.Text = "Show &Log...";
            this._miViewLog.Click += new System.EventHandler(this._miViewLog_Click);
            // 
            // _miCapture
            // 
            this._miCapture.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._miCaptureActiveWindow,
            this._miCaptureFullScreen,
            this._miCaptureSelectedObject,
            this._miCaptureSelectedArea,
            this.toolStripSeparator13,
            this._miCaptureStopCapture,
            this.toolStripSeparator19,
            this._miCaptureOptionsMenu});
            this._miCapture.Name = "_miCapture";
            this._miCapture.Size = new System.Drawing.Size(61, 22);
            this._miCapture.Text = "&Capture";
            // 
            // _miCaptureActiveWindow
            // 
            this._miCaptureActiveWindow.Name = "_miCaptureActiveWindow";
            this._miCaptureActiveWindow.Size = new System.Drawing.Size(156, 22);
            this._miCaptureActiveWindow.Text = "Active &Window";
            this._miCaptureActiveWindow.Click += new System.EventHandler(this._miCaptureActiveWindow_Click);
            // 
            // _miCaptureFullScreen
            // 
            this._miCaptureFullScreen.Name = "_miCaptureFullScreen";
            this._miCaptureFullScreen.Size = new System.Drawing.Size(156, 22);
            this._miCaptureFullScreen.Text = "&Full Screen";
            this._miCaptureFullScreen.Click += new System.EventHandler(this._miCaptureFullScreen_Click);
            // 
            // _miCaptureSelectedObject
            // 
            this._miCaptureSelectedObject.Name = "_miCaptureSelectedObject";
            this._miCaptureSelectedObject.Size = new System.Drawing.Size(156, 22);
            this._miCaptureSelectedObject.Text = "&Selected Object";
            this._miCaptureSelectedObject.Click += new System.EventHandler(this._miCaptureSelectedObject_Click);
            // 
            // _miCaptureSelectedArea
            // 
            this._miCaptureSelectedArea.Name = "_miCaptureSelectedArea";
            this._miCaptureSelectedArea.Size = new System.Drawing.Size(156, 22);
            this._miCaptureSelectedArea.Text = "Selected &Area";
            this._miCaptureSelectedArea.Click += new System.EventHandler(this._miCaptureSelectedArea_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(153, 6);
            // 
            // _miCaptureStopCapture
            // 
            this._miCaptureStopCapture.Enabled = false;
            this._miCaptureStopCapture.Name = "_miCaptureStopCapture";
            this._miCaptureStopCapture.Size = new System.Drawing.Size(156, 22);
            this._miCaptureStopCapture.Text = "St&op Capture";
            this._miCaptureStopCapture.Click += new System.EventHandler(this._miCaptureStopCapture_Click);
            // 
            // toolStripSeparator19
            // 
            this.toolStripSeparator19.Name = "toolStripSeparator19";
            this.toolStripSeparator19.Size = new System.Drawing.Size(153, 6);
            // 
            // _miCaptureOptionsMenu
            // 
            this._miCaptureOptionsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._miCaptureAreaOptions,
            this._miCaptureOptions,
            this._miCaptureObjectOptions});
            this._miCaptureOptionsMenu.Name = "_miCaptureOptionsMenu";
            this._miCaptureOptionsMenu.Size = new System.Drawing.Size(156, 22);
            this._miCaptureOptionsMenu.Text = " &Options";
            // 
            // _miCaptureAreaOptions
            // 
            this._miCaptureAreaOptions.Name = "_miCaptureAreaOptions";
            this._miCaptureAreaOptions.Size = new System.Drawing.Size(208, 22);
            this._miCaptureAreaOptions.Text = "Capture &Area Options...";
            this._miCaptureAreaOptions.Click += new System.EventHandler(this._miCaptureAreaOptions_Click);
            // 
            // _miCaptureOptions
            // 
            this._miCaptureOptions.Name = "_miCaptureOptions";
            this._miCaptureOptions.Size = new System.Drawing.Size(208, 22);
            this._miCaptureOptions.Text = "&Capture Options...";
            this._miCaptureOptions.Click += new System.EventHandler(this._miCaptureOptions_Click);
            // 
            // _miCaptureObjectOptions
            // 
            this._miCaptureObjectOptions.Name = "_miCaptureObjectOptions";
            this._miCaptureObjectOptions.Size = new System.Drawing.Size(208, 22);
            this._miCaptureObjectOptions.Text = "Capture &Object Options...";
            this._miCaptureObjectOptions.Click += new System.EventHandler(this._miCaptureObjectOptions_Click);
            // 
            // _miEventsEmf2
            // 
            this._miEventsEmf2.Name = "_miEventsEmf2";
            this._miEventsEmf2.Size = new System.Drawing.Size(152, 22);
            this._miEventsEmf2.Text = "a";
            // 
            // _miEventsJob2
            // 
            this._miEventsJob2.Name = "_miEventsJob2";
            this._miEventsJob2.Size = new System.Drawing.Size(152, 22);
            this._miEventsJob2.Text = "a";
            // 
            // _panelPictureBox
            // 
            this._panelPictureBox.AutoScroll = true;
            this._panelPictureBox.Controls.Add(this._tbTableLayout);
            this._panelPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panelPictureBox.Location = new System.Drawing.Point(0, 66);
            this._panelPictureBox.Name = "_panelPictureBox";
            this._panelPictureBox.Size = new System.Drawing.Size(1198, 638);
            this._panelPictureBox.TabIndex = 5;
            // 
            // _tbTableLayout
            // 
            this._tbTableLayout.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this._tbTableLayout.ColumnCount = 3;
            this._tbTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this._tbTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this._tbTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 671F));
            this._tbTableLayout.Controls.Add(this._grpStoreServers, 2, 0);
            this._tbTableLayout.Controls.Add(this._gbDicomInfo, 2, 1);
            this._tbTableLayout.Controls.Add(this.panelInfo, 0, 0);
            this._tbTableLayout.Controls.Add(this.panel1, 2, 2);
            this._tbTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tbTableLayout.Location = new System.Drawing.Point(0, 0);
            this._tbTableLayout.Name = "_tbTableLayout";
            this._tbTableLayout.RowCount = 3;
            this._tbTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 91F));
            this._tbTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tbTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 179F));
            this._tbTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this._tbTableLayout.Size = new System.Drawing.Size(1198, 638);
            this._tbTableLayout.TabIndex = 0;
            // 
            // _grpStoreServers
            // 
            this._grpStoreServers.Controls.Add(this._cbStoreServers);
            this._grpStoreServers.Controls.Add(this._btnCreateConclusion);
            this._grpStoreServers.Controls.Add(this._btnPushToPACS);
            this._grpStoreServers.Controls.Add(this.label9);
            this._grpStoreServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grpStoreServers.Location = new System.Drawing.Point(527, 6);
            this._grpStoreServers.Name = "_grpStoreServers";
            this._grpStoreServers.Padding = new System.Windows.Forms.Padding(3);
            this._grpStoreServers.Size = new System.Drawing.Size(665, 85);
            this._grpStoreServers.TabIndex = 15;
            this._grpStoreServers.Text = "Tải lên PACS";
            // 
            // _cbStoreServers
            // 
            this._cbStoreServers.Location = new System.Drawing.Point(127, 33);
            this._cbStoreServers.Margin = new System.Windows.Forms.Padding(2);
            this._cbStoreServers.Name = "_cbStoreServers";
            this._cbStoreServers.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._cbStoreServers.Size = new System.Drawing.Size(271, 22);
            this._cbStoreServers.TabIndex = 15;
            this._cbStoreServers.SelectedIndexChanged += new System.EventHandler(this._cbSevers_SelectedIndexChanged);
            // 
            // _btnCreateConclusion
            // 
            this._btnCreateConclusion.AllowDrop = true;
            this._btnCreateConclusion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnCreateConclusion.Location = new System.Drawing.Point(538, 34);
            this._btnCreateConclusion.Name = "_btnCreateConclusion";
            this._btnCreateConclusion.Size = new System.Drawing.Size(110, 24);
            this._btnCreateConclusion.TabIndex = 14;
            this._btnCreateConclusion.Text = "Tạo kết luận";
            this._btnCreateConclusion.Click += new System.EventHandler(this._btnCreateConclusion_Click);
            // 
            // _btnPushToPACS
            // 
            this._btnPushToPACS.AllowDrop = true;
            this._btnPushToPACS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnPushToPACS.Location = new System.Drawing.Point(424, 34);
            this._btnPushToPACS.Name = "_btnPushToPACS";
            this._btnPushToPACS.Size = new System.Drawing.Size(100, 24);
            this._btnPushToPACS.TabIndex = 13;
            this._btnPushToPACS.Text = "Tải lên";
            this._btnPushToPACS.Click += new System.EventHandler(this._btnPushToPACS_Click);
            // 
            // label9
            // 
            this.label9.Appearance.Options.UseFont = true;
            this.label9.Location = new System.Drawing.Point(6, 37);
            this.label9.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 15);
            this.label9.TabIndex = 11;
            this.label9.Text = "Máy chụp PACS";
            // 
            // _gbDicomInfo
            // 
            this._gbDicomInfo.Controls.Add(this._tbPropertyGrid);
            this._gbDicomInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gbDicomInfo.Location = new System.Drawing.Point(527, 100);
            this._gbDicomInfo.Name = "_gbDicomInfo";
            this._gbDicomInfo.Padding = new System.Windows.Forms.Padding(3);
            this._gbDicomInfo.Size = new System.Drawing.Size(665, 350);
            this._gbDicomInfo.TabIndex = 10;
            this._gbDicomInfo.Text = "Thông tin DICOM được sử dụng với hình ảnh";
            // 
            // _tbPropertyGrid
            // 
            this._tbPropertyGrid.ColumnCount = 1;
            this._tbPropertyGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tbPropertyGrid.Controls.Add(this.panel6, 0, 0);
            this._tbPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tbPropertyGrid.Location = new System.Drawing.Point(5, 25);
            this._tbPropertyGrid.Name = "_tbPropertyGrid";
            this._tbPropertyGrid.RowCount = 2;
            this._tbPropertyGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this._tbPropertyGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tbPropertyGrid.Size = new System.Drawing.Size(655, 320);
            this._tbPropertyGrid.TabIndex = 3;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this._cmbSopClasses);
            this.panel6.Controls.Add(this.label7);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(3, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(649, 54);
            this.panel6.TabIndex = 3;
            // 
            // _cmbSopClasses
            // 
            this._cmbSopClasses.Location = new System.Drawing.Point(118, 14);
            this._cmbSopClasses.Margin = new System.Windows.Forms.Padding(2);
            this._cmbSopClasses.Name = "_cmbSopClasses";
            this._cmbSopClasses.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this._cmbSopClasses.Properties.Items.AddRange(new object[] {
            "Secondary Capture",
            "SC multi frame 8-bit gray",
            "SC multi frame 24-bit color",
            "Encapsulated PDF"});
            this._cmbSopClasses.Size = new System.Drawing.Size(271, 22);
            this._cmbSopClasses.TabIndex = 2;
            this._cmbSopClasses.SelectedIndexChanged += new System.EventHandler(this._cmbSopClasses_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Appearance.Options.UseFont = true;
            this.label7.Location = new System.Drawing.Point(6, 16);
            this.label7.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 15);
            this.label7.TabIndex = 1;
            this.label7.Text = "Kiểu DICOM";
            // 
            // panelInfo
            // 
            this.panelInfo.BackColor = System.Drawing.Color.Black;
            this._tbTableLayout.SetColumnSpan(this.panelInfo, 2);
            this.panelInfo.Controls.Add(this.tableLayoutPanelInfo);
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInfo.Location = new System.Drawing.Point(6, 6);
            this.panelInfo.Name = "panelInfo";
            this._tbTableLayout.SetRowSpan(this.panelInfo, 2);
            this.panelInfo.Size = new System.Drawing.Size(512, 444);
            this.panelInfo.TabIndex = 1;
            // 
            // tableLayoutPanelInfo
            // 
            this.tableLayoutPanelInfo.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanelInfo.ColumnCount = 1;
            this.tableLayoutPanelInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.76778F));
            this.tableLayoutPanelInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.23222F));
            this.tableLayoutPanelInfo.Controls.Add(this._grpBoxPictureBox, 0, 0);
            this.tableLayoutPanelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelInfo.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelInfo.Name = "tableLayoutPanelInfo";
            this.tableLayoutPanelInfo.RowCount = 1;
            this.tableLayoutPanelInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 438F));
            this.tableLayoutPanelInfo.Size = new System.Drawing.Size(512, 444);
            this.tableLayoutPanelInfo.TabIndex = 0;
            // 
            // _grpBoxPictureBox
            // 
            this._grpBoxPictureBox.Controls.Add(this._tbPicture);
            this._grpBoxPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grpBoxPictureBox.Location = new System.Drawing.Point(3, 3);
            this._grpBoxPictureBox.Name = "_grpBoxPictureBox";
            this._grpBoxPictureBox.Padding = new System.Windows.Forms.Padding(3);
            this._grpBoxPictureBox.Size = new System.Drawing.Size(506, 438);
            this._grpBoxPictureBox.TabIndex = 13;
            this._grpBoxPictureBox.Text = "Lấy hình ảnh từ nguồn hoặc in từ ứng dụng khác:";
            // 
            // _tbPicture
            // 
            this._tbPicture.ColumnCount = 4;
            this._tbPicture.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 104F));
            this._tbPicture.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tbPicture.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tbPicture.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 134F));
            this._tbPicture.Controls.Add(this._btnNext, 3, 0);
            this._tbPicture.Controls.Add(this._btnPrev, 0, 0);
            this._tbPicture.Controls.Add(this._lblPageInfo, 1, 1);
            this._tbPicture.Controls.Add(this._btnOpenImage, 1, 0);
            this._tbPicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tbPicture.Location = new System.Drawing.Point(5, 25);
            this._tbPicture.Name = "_tbPicture";
            this._tbPicture.RowCount = 2;
            this._tbPicture.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this._tbPicture.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this._tbPicture.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this._tbPicture.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this._tbPicture.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this._tbPicture.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tbPicture.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this._tbPicture.Size = new System.Drawing.Size(496, 408);
            this._tbPicture.TabIndex = 13;
            // 
            // _btnNext
            // 
            this._btnNext.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnNext.Enabled = false;
            this._btnNext.Location = new System.Drawing.Point(455, 3);
            this._btnNext.Name = "_btnNext";
            this._btnNext.Size = new System.Drawing.Size(38, 38);
            this._btnNext.TabIndex = 21;
            this._btnNext.Text = "->";
            this._btnNext.Click += new System.EventHandler(this._btnNext_Click);
            // 
            // _btnPrev
            // 
            this._btnPrev.Dock = System.Windows.Forms.DockStyle.Left;
            this._btnPrev.Enabled = false;
            this._btnPrev.Location = new System.Drawing.Point(3, 3);
            this._btnPrev.Name = "_btnPrev";
            this._btnPrev.Size = new System.Drawing.Size(38, 38);
            this._btnPrev.TabIndex = 20;
            this._btnPrev.Text = "<-";
            this._btnPrev.Click += new System.EventHandler(this._btnPrev_Click);
            // 
            // _lblPageInfo
            // 
            this._tbPicture.SetColumnSpan(this._lblPageInfo, 2);
            this._lblPageInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblPageInfo.Location = new System.Drawing.Point(107, 44);
            this._lblPageInfo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this._lblPageInfo.Name = "_lblPageInfo";
            this._lblPageInfo.Size = new System.Drawing.Size(252, 364);
            this._lblPageInfo.TabIndex = 19;
            // 
            // _btnOpenImage
            // 
            this._tbPicture.SetColumnSpan(this._btnOpenImage, 2);
            this._btnOpenImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this._btnOpenImage.Location = new System.Drawing.Point(107, 3);
            this._btnOpenImage.Name = "_btnOpenImage";
            this._btnOpenImage.Size = new System.Drawing.Size(252, 38);
            this._btnOpenImage.TabIndex = 14;
            this._btnOpenImage.Text = "Chọn tệp...";
            this._btnOpenImage.Click += new System.EventHandler(this._btnOpenImage_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this._fPLRoll);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this._btnRecord);
            this.panel1.Controls.Add(this._btnSnapshot);
            this.panel1.Controls.Add(this._btnPause);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(526, 457);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(667, 177);
            this.panel1.TabIndex = 16;
            // 
            // _fPLRoll
            // 
            this._fPLRoll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._fPLRoll.BackColor = System.Drawing.Color.White;
            this._fPLRoll.Location = new System.Drawing.Point(19, 87);
            this._fPLRoll.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this._fPLRoll.Name = "_fPLRoll";
            this._fPLRoll.Size = new System.Drawing.Size(639, 84);
            this._fPLRoll.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Appearance.Options.UseFont = true;
            this.label1.Location = new System.Drawing.Point(15, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Bảng điều khiển camera";
            // 
            // _btnRecord
            // 
            this._btnRecord.ImageOptions.Image = global::PrintToPACSDemo.Properties.Resources.circle;
            this._btnRecord.Location = new System.Drawing.Point(17, 31);
            this._btnRecord.Name = "_btnRecord";
            this._btnRecord.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this._btnRecord.Size = new System.Drawing.Size(104, 44);
            this._btnRecord.TabIndex = 3;
            this._btnRecord.Text = "Ghi lại";
            this._btnRecord.Click += new System.EventHandler(this._btnRecord_Click);
            // 
            // _btnSnapshot
            // 
            this._btnSnapshot.ImageOptions.Image = global::PrintToPACSDemo.Properties.Resources.shutter;
            this._btnSnapshot.Location = new System.Drawing.Point(262, 31);
            this._btnSnapshot.Name = "_btnSnapshot";
            this._btnSnapshot.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this._btnSnapshot.Size = new System.Drawing.Size(136, 44);
            this._btnSnapshot.TabIndex = 2;
            this._btnSnapshot.Text = "Chụp nhanh";
            this._btnSnapshot.Click += new System.EventHandler(this._btnSnapshot_Click);
            // 
            // _btnPause
            // 
            this._btnPause.Enabled = false;
            this._btnPause.ImageOptions.Image = global::PrintToPACSDemo.Properties.Resources.pause;
            this._btnPause.Location = new System.Drawing.Point(128, 31);
            this._btnPause.Name = "_btnPause";
            this._btnPause.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this._btnPause.Size = new System.Drawing.Size(127, 44);
            this._btnPause.TabIndex = 0;
            this._btnPause.Text = "Tạm dừng";
            this._btnPause.Click += new System.EventHandler(this._btnPause_Click);
            // 
            // _cmResultQuery
            // 
            this._cmResultQuery.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._cmResultQuery.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._miClearResult,
            this.discontinueMPPSToolStripMenuItem});
            this._cmResultQuery.Name = "_cmResultQuery";
            this._cmResultQuery.Size = new System.Drawing.Size(172, 48);
            this._cmResultQuery.Opening += new System.ComponentModel.CancelEventHandler(this._cmResultQuery_Opening);
            // 
            // _miClearResult
            // 
            this._miClearResult.Name = "_miClearResult";
            this._miClearResult.Size = new System.Drawing.Size(171, 22);
            this._miClearResult.Text = "Clear Results";
            // 
            // discontinueMPPSToolStripMenuItem
            // 
            this.discontinueMPPSToolStripMenuItem.Name = "discontinueMPPSToolStripMenuItem";
            this.discontinueMPPSToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.discontinueMPPSToolStripMenuItem.Text = "Discontinue MPPS";
            this.discontinueMPPSToolStripMenuItem.Click += new System.EventHandler(this.discontinueMPPSToolStripMenuItem_Click);
            // 
            // _cnmnuClearSearch
            // 
            this._cnmnuClearSearch.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._cnmnuClearSearch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._miClearSearch});
            this._cnmnuClearSearch.Name = "_cnmnuDicomInfo";
            this._cnmnuClearSearch.Size = new System.Drawing.Size(102, 26);
            // 
            // _miClearSearch
            // 
            this._miClearSearch.Name = "_miClearSearch";
            this._miClearSearch.Size = new System.Drawing.Size(101, 22);
            this._miClearSearch.Text = "Clear";
            this._miClearSearch.Click += new System.EventHandler(this._miClearSearch_Click);
            // 
            // _panelImageList
            // 
            this._panelImageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panelImageList.Location = new System.Drawing.Point(3, 532);
            this._panelImageList.Name = "_panelImageList";
            this._panelImageList.Size = new System.Drawing.Size(708, 150);
            this._panelImageList.TabIndex = 11;
            // 
            // _cnmnuClearDicom
            // 
            this._cnmnuClearDicom.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._cnmnuClearDicom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._miClearPG});
            this._cnmnuClearDicom.Name = "_cnmnuDicomInfo";
            this._cnmnuClearDicom.Size = new System.Drawing.Size(102, 26);
            // 
            // _miClearPG
            // 
            this._miClearPG.Name = "_miClearPG";
            this._miClearPG.Size = new System.Drawing.Size(101, 22);
            this._miClearPG.Text = "Clear";
            this._miClearPG.Click += new System.EventHandler(this._miClearPG_Click);
            // 
            // _gbRecognized
            // 
            this._gbRecognized.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gbRecognized.Location = new System.Drawing.Point(232, 272);
            this._gbRecognized.Name = "_gbRecognized";
            this._gbRecognized.Size = new System.Drawing.Size(394, 109);
            this._gbRecognized.TabIndex = 8;
            this._gbRecognized.Text = "Recognized Text";
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(3, 406);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(544, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Studies Found";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(553, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(138, 24);
            this.panel2.TabIndex = 9;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 0;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(130, 130);
            this.propertyGrid1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.comboBox1);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(544, 24);
            this.panel3.TabIndex = 10;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(78, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(463, 21);
            this.comboBox1.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "Query Server";
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(3, 249);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(544, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Patients Found";
            // 
            // _toolbarMain
            // 
            this._toolbarMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this._toolbarMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._toolBtnOpenRaster,
            this.toolStripSeparator23,
            this._toolBtnStoreToPacs,
            this._toolBtnSaveDicom,
            this.toolStripSeparator4,
            this._toolBtnCLearInfo,
            this.toolStripSeparator5,
            this._toolBtnDeleteAll,
            this._toolBtnDeleteSelected,
            this.toolStripSeparator6,
            this._toolBtnRotate,
            this.toolStripSeparator24,
            this._toolBtnViewLog,
            this.toolStripSeparator7,
            this._toolBtnSettingsCamera});
            this._toolbarMain.Location = new System.Drawing.Point(0, 24);
            this._toolbarMain.MinimumSize = new System.Drawing.Size(0, 42);
            this._toolbarMain.Name = "_toolbarMain";
            this._toolbarMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this._toolbarMain.Size = new System.Drawing.Size(1198, 42);
            this._toolbarMain.TabIndex = 6;
            this._toolbarMain.TabStop = true;
            // 
            // _toolBtnOpenRaster
            // 
            this._toolBtnOpenRaster.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolBtnOpenRaster.Image = ((System.Drawing.Image)(resources.GetObject("_toolBtnOpenRaster.Image")));
            this._toolBtnOpenRaster.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolBtnOpenRaster.Name = "_toolBtnOpenRaster";
            this._toolBtnOpenRaster.Size = new System.Drawing.Size(28, 39);
            this._toolBtnOpenRaster.Text = "Mở ảnh";
            this._toolBtnOpenRaster.Click += new System.EventHandler(this._toolBtnOpenRaster_Click);
            // 
            // toolStripSeparator23
            // 
            this.toolStripSeparator23.Name = "toolStripSeparator23";
            this.toolStripSeparator23.Size = new System.Drawing.Size(6, 42);
            // 
            // _toolBtnStoreToPacs
            // 
            this._toolBtnStoreToPacs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolBtnStoreToPacs.Image = ((System.Drawing.Image)(resources.GetObject("_toolBtnStoreToPacs.Image")));
            this._toolBtnStoreToPacs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolBtnStoreToPacs.Name = "_toolBtnStoreToPacs";
            this._toolBtnStoreToPacs.Size = new System.Drawing.Size(28, 39);
            this._toolBtnStoreToPacs.Text = "Tải lên PACS";
            this._toolBtnStoreToPacs.Click += new System.EventHandler(this._toolBtnStoreToPacs_Click);
            // 
            // _toolBtnSaveDicom
            // 
            this._toolBtnSaveDicom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolBtnSaveDicom.Image = ((System.Drawing.Image)(resources.GetObject("_toolBtnSaveDicom.Image")));
            this._toolBtnSaveDicom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolBtnSaveDicom.Name = "_toolBtnSaveDicom";
            this._toolBtnSaveDicom.Size = new System.Drawing.Size(28, 39);
            this._toolBtnSaveDicom.Text = "Lưu DICOM";
            this._toolBtnSaveDicom.Click += new System.EventHandler(this._toolBtnSaveDicom_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 42);
            // 
            // _toolBtnCLearInfo
            // 
            this._toolBtnCLearInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolBtnCLearInfo.Image = ((System.Drawing.Image)(resources.GetObject("_toolBtnCLearInfo.Image")));
            this._toolBtnCLearInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolBtnCLearInfo.Name = "_toolBtnCLearInfo";
            this._toolBtnCLearInfo.Size = new System.Drawing.Size(28, 39);
            this._toolBtnCLearInfo.Text = "Xóa thông tin DICOM";
            this._toolBtnCLearInfo.Click += new System.EventHandler(this._toolBtnCLearInfo_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 42);
            // 
            // _toolBtnDeleteAll
            // 
            this._toolBtnDeleteAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolBtnDeleteAll.Image = ((System.Drawing.Image)(resources.GetObject("_toolBtnDeleteAll.Image")));
            this._toolBtnDeleteAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolBtnDeleteAll.Name = "_toolBtnDeleteAll";
            this._toolBtnDeleteAll.Size = new System.Drawing.Size(28, 39);
            this._toolBtnDeleteAll.Text = "Xóa tất cả ảnh";
            this._toolBtnDeleteAll.Click += new System.EventHandler(this._toolBtnDeleteAll_Click);
            // 
            // _toolBtnDeleteSelected
            // 
            this._toolBtnDeleteSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolBtnDeleteSelected.Image = ((System.Drawing.Image)(resources.GetObject("_toolBtnDeleteSelected.Image")));
            this._toolBtnDeleteSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolBtnDeleteSelected.Name = "_toolBtnDeleteSelected";
            this._toolBtnDeleteSelected.Size = new System.Drawing.Size(28, 39);
            this._toolBtnDeleteSelected.Text = "Xóa ảnh chọn";
            this._toolBtnDeleteSelected.Click += new System.EventHandler(this._toolBtnDeleteSelected_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 42);
            // 
            // _toolBtnRotate
            // 
            this._toolBtnRotate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolBtnRotate.Image = ((System.Drawing.Image)(resources.GetObject("_toolBtnRotate.Image")));
            this._toolBtnRotate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolBtnRotate.Name = "_toolBtnRotate";
            this._toolBtnRotate.Size = new System.Drawing.Size(28, 39);
            this._toolBtnRotate.Text = "Xoay 90";
            this._toolBtnRotate.Click += new System.EventHandler(this._toolBtnRotate_Click);
            // 
            // toolStripSeparator24
            // 
            this.toolStripSeparator24.Name = "toolStripSeparator24";
            this.toolStripSeparator24.Size = new System.Drawing.Size(6, 42);
            // 
            // _toolBtnViewLog
            // 
            this._toolBtnViewLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolBtnViewLog.Image = ((System.Drawing.Image)(resources.GetObject("_toolBtnViewLog.Image")));
            this._toolBtnViewLog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolBtnViewLog.Name = "_toolBtnViewLog";
            this._toolBtnViewLog.Size = new System.Drawing.Size(28, 39);
            this._toolBtnViewLog.Text = "Show/Hide Log Window";
            this._toolBtnViewLog.Click += new System.EventHandler(this._toolBtnViewLog_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 42);
            // 
            // _toolBtnSettingsCamera
            // 
            this._toolBtnSettingsCamera.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolBtnSettingsCamera.Image = global::PrintToPACSDemo.Properties.Resources.AppSettings;
            this._toolBtnSettingsCamera.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolBtnSettingsCamera.Name = "_toolBtnSettingsCamera";
            this._toolBtnSettingsCamera.Size = new System.Drawing.Size(28, 39);
            this._toolBtnSettingsCamera.ToolTipText = "Cài đặt camera";
            this._toolBtnSettingsCamera.Click += new System.EventHandler(this._toolBtnSettingsCamera_Click);
            // 
            // _cmListBox
            // 
            this._cmListBox.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._cmListBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._cmiViewMode,
            this.toolStripSeparator9,
            this._cmiDeleteAll,
            this._cmiDeleteSelected});
            this._cmListBox.Name = "_cnmnuDicomInfo";
            this._cmListBox.Size = new System.Drawing.Size(157, 76);
            this._cmListBox.Opening += new System.ComponentModel.CancelEventHandler(this._cmListBox_Opening);
            // 
            // _cmiViewMode
            // 
            this._cmiViewMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._cmiExpanded,
            this._cmiCondensed});
            this._cmiViewMode.Name = "_cmiViewMode";
            this._cmiViewMode.Size = new System.Drawing.Size(156, 22);
            this._cmiViewMode.Text = "&View Mode";
            // 
            // _cmiExpanded
            // 
            this._cmiExpanded.Name = "_cmiExpanded";
            this._cmiExpanded.Size = new System.Drawing.Size(134, 22);
            this._cmiExpanded.Text = "&Expanded";
            this._cmiExpanded.Click += new System.EventHandler(this._cmiExpanded_Click);
            // 
            // _cmiCondensed
            // 
            this._cmiCondensed.Name = "_cmiCondensed";
            this._cmiCondensed.Size = new System.Drawing.Size(134, 22);
            this._cmiCondensed.Text = "Condensed";
            this._cmiCondensed.Click += new System.EventHandler(this._cmiCondensed_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(153, 6);
            // 
            // _cmiDeleteAll
            // 
            this._cmiDeleteAll.Name = "_cmiDeleteAll";
            this._cmiDeleteAll.Size = new System.Drawing.Size(156, 22);
            this._cmiDeleteAll.Text = "&Delete All Items";
            this._cmiDeleteAll.Click += new System.EventHandler(this._miClearPrintedList_Click);
            // 
            // _cmiDeleteSelected
            // 
            this._cmiDeleteSelected.Name = "_cmiDeleteSelected";
            this._cmiDeleteSelected.Size = new System.Drawing.Size(156, 22);
            this._cmiDeleteSelected.Text = "Delete &Selected";
            this._cmiDeleteSelected.Click += new System.EventHandler(this._miDeleteSelected_Click);
            // 
            // _lstBoxPages
            // 
            this._lstBoxPages.AutoScroll = true;
            this._lstBoxPages.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this._lstBoxPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lstBoxPages.ExpansionButtonLocation = System.Windows.Forms.AnchorStyles.Left;
            this._lstBoxPages.ItemHeight = 120;
            this._lstBoxPages.Location = new System.Drawing.Point(0, 0);
            this._lstBoxPages.Name = "_lstBoxPages";
            this._lstBoxPages.SelectedGroupIndex = -1;
            this._lstBoxPages.SelectedIndex = -1;
            this._lstBoxPages.SelectedItem = null;
            this._lstBoxPages.SelectedItemGroupIndex = -1;
            this._lstBoxPages.Size = new System.Drawing.Size(150, 678);
            this._lstBoxPages.TabIndex = 0;
            this._lstBoxPages.ViewMode = PrintToPACSDemo.UI.ThumbMode.Condensed;
            this._lstBoxPages.ItemAdded += new System.EventHandler(this._lstBoxPages_ItemAdded);
            this._lstBoxPages.SelectedIndexChanged += new System.EventHandler(this._lstBoxPages_SelectedIndexChanged);
            this._lstBoxPages.KeyDown += new System.Windows.Forms.KeyEventHandler(this._lstBoxPages_KeyDown);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1198, 704);
            this.Controls.Add(this._panelPictureBox);
            this.Controls.Add(this._toolbarMain);
            this.Controls.Add(this._mmMain);
            this.IconOptions.Image = global::PrintToPACSDemo.Properties.Resources.logo;
            this.MainMenuStrip = this._mmMain;
            this.MinimumSize = new System.Drawing.Size(910, 591);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chương trình chuyển đổi video DICOM";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.Resize += new System.EventHandler(this.FrmMain_Resize);
            this._mmMain.ResumeLayout(false);
            this._mmMain.PerformLayout();
            this._panelPictureBox.ResumeLayout(false);
            this._tbTableLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._grpStoreServers)).EndInit();
            this._grpStoreServers.ResumeLayout(false);
            this._grpStoreServers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._cbStoreServers.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._gbDicomInfo)).EndInit();
            this._gbDicomInfo.ResumeLayout(false);
            this._tbPropertyGrid.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._cmbSopClasses.Properties)).EndInit();
            this.panelInfo.ResumeLayout(false);
            this.tableLayoutPanelInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._grpBoxPictureBox)).EndInit();
            this._grpBoxPictureBox.ResumeLayout(false);
            this._tbPicture.ResumeLayout(false);
            this._tbPicture.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this._cmResultQuery.ResumeLayout(false);
            this._cnmnuClearSearch.ResumeLayout(false);
            this._cnmnuClearDicom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gbRecognized)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this._toolbarMain.ResumeLayout(false);
            this._toolbarMain.PerformLayout();
            this._cmListBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.MenuStrip _mmMain;
        private ListImageBox _lstBoxPages;
        private System.Windows.Forms.Panel _panelPictureBox;
        private System.Windows.Forms.ToolStripMenuItem _miEventsEmf2;
        private System.Windows.Forms.ToolStripMenuItem _miEventsJob2;
        private System.Windows.Forms.ToolStripMenuItem _miView;
        private System.Windows.Forms.ToolStripMenuItem _miNormal;
        private System.Windows.Forms.ToolStripMenuItem _miFit;
        private System.Windows.Forms.ToolStripMenuItem _miZoomIn;
        private System.Windows.Forms.ToolStripMenuItem _miZoomOut;
        private System.Windows.Forms.TableLayoutPanel _tbTableLayout;
        private DevExpress.XtraEditors.GroupControl _gbRecognized;
        private System.Windows.Forms.ContextMenuStrip _cnmnuClearDicom;
        private System.Windows.Forms.ToolStripMenuItem _miClearPG;
        private DevExpress.XtraEditors.GroupControl _gbDicomInfo;

        private System.Windows.Forms.ContextMenuStrip _cmResultQuery;
        private System.Windows.Forms.ToolStripMenuItem _miClearResult;
        private System.Windows.Forms.ContextMenuStrip _cnmnuClearSearch;
        private System.Windows.Forms.ToolStripMenuItem _miClearSearch;
        private System.Windows.Forms.ToolStripMenuItem _miEdit;
        private System.Windows.Forms.ToolStripMenuItem _miDeleteAll;
        private System.Windows.Forms.ToolStripMenuItem _miFile;
        private System.Windows.Forms.ToolStripMenuItem _miSaveAsDICOM;
        private System.Windows.Forms.ToolStripMenuItem _miStoreToPACS;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem _miExit;
        private DevExpress.XtraEditors.LabelControl label3;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraEditors.SimpleButton button1;
        private DevExpress.XtraEditors.GroupControl groupBox1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox comboBox1;
        private DevExpress.XtraEditors.LabelControl label4;
        private DevExpress.XtraEditors.LabelControl label5;
        private System.Windows.Forms.TableLayoutPanel _tbPropertyGrid;
        private System.Windows.Forms.Panel panel6;
        private DevExpress.XtraEditors.LabelControl label7;
        private System.Windows.Forms.ToolStripMenuItem _miOpen;
        private System.Windows.Forms.ToolStripMenuItem _miResetInfo;
        private System.Windows.Forms.ToolStripMenuItem _miDeleteSelected;
        private System.Windows.Forms.ToolStripMenuItem _miViewLog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStrip _toolbarMain;
        private System.Windows.Forms.ToolStripButton _toolBtnStoreToPacs;
        private System.Windows.Forms.ToolStripButton _toolBtnSaveDicom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton _toolBtnCLearInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton _toolBtnDeleteAll;
        private System.Windows.Forms.ToolStripButton _toolBtnDeleteSelected;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton _toolBtnViewLog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ContextMenuStrip _cmListBox;
        private System.Windows.Forms.ToolStripMenuItem _cmiViewMode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem _cmiDeleteAll;
        private System.Windows.Forms.ToolStripMenuItem _cmiDeleteSelected;
        private System.Windows.Forms.ToolStripMenuItem _cmiExpanded;
        private System.Windows.Forms.ToolStripMenuItem _cmiCondensed;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem _miPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem _miCapture;
        private System.Windows.Forms.ToolStripMenuItem _miCaptureActiveWindow;
        private System.Windows.Forms.ToolStripMenuItem _miCaptureFullScreen;
        private System.Windows.Forms.ToolStripMenuItem _miCaptureSelectedObject;
        private System.Windows.Forms.ToolStripMenuItem _miCaptureSelectedArea;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripMenuItem _miCaptureStopCapture;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator19;
        private System.Windows.Forms.ToolStripMenuItem _miCaptureOptionsMenu;
        private System.Windows.Forms.ToolStripMenuItem _miCaptureAreaOptions;
        private System.Windows.Forms.ToolStripMenuItem _miCaptureOptions;
        private System.Windows.Forms.ToolStripMenuItem _miCaptureObjectOptions;
        private System.Windows.Forms.Panel _panelImageList;
        private System.Windows.Forms.ToolStripMenuItem _miResample;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripButton _toolBtnOpenRaster;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator23;
        private System.Windows.Forms.ToolStripButton _toolBtnRotate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator24;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator25;
        private System.Windows.Forms.ToolStripMenuItem _miRotate90;
        private DevExpress.XtraEditors.GroupControl _grpBoxPictureBox;
        private System.Windows.Forms.TableLayoutPanel _tbPicture;
        private DevExpress.XtraEditors.SimpleButton _btnPrev;
        private DevExpress.XtraEditors.LabelControl _lblPageInfo;
        private DevExpress.XtraEditors.SimpleButton _btnOpenImage;
        private DevExpress.XtraEditors.SimpleButton _btnNext;
        private DevExpress.XtraEditors.GroupControl _grpStoreServers;
        private DevExpress.XtraEditors.SimpleButton _btnPushToPACS;
        private DevExpress.XtraEditors.LabelControl label9;
        public System.Windows.Forms.Panel panelInfo;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanelInfo;
        private System.Windows.Forms.ToolStripMenuItem discontinueMPPSToolStripMenuItem;
        private DevExpress.XtraEditors.SimpleButton _btnCreateConclusion;
        private System.Windows.Forms.ToolStripButton _toolBtnSettingsCamera;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton _btnPause;
        private DevExpress.XtraEditors.SimpleButton _btnRecord;
        private DevExpress.XtraEditors.SimpleButton _btnSnapshot;
        private DevExpress.XtraEditors.LabelControl label1;
        public System.Windows.Forms.FlowLayoutPanel _fPLRoll;
        private DevExpress.XtraEditors.ComboBoxEdit _cbStoreServers;
        private DevExpress.XtraEditors.ComboBoxEdit _cmbSopClasses;
    }
}

