using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using MediaToPacs.Core.Models;
using MediaToPacs.Core.Utilities;
using STM.MediaToPACS.Main.Utilities;
using STM.MediaToPACS.Main.UI.CameraUI;

namespace STM.MediaToPACS.Main.UI.Configurations
{
    public sealed class SystemSettingsDialog : XtraForm
    {
        readonly MySettings pacs;
        readonly TextEdit clientAe = new TextEdit();
        readonly ServerEditor worklist = new ServerEditor("Worklist");
        readonly Dictionary<string, ComboBoxEdit> keys = new Dictionary<string, ComboBoxEdit>();
        readonly Dictionary<string, TextEdit> systemFields = new Dictionary<string, TextEdit>();
        readonly ComboBoxEdit printer = new ComboBoxEdit();
        readonly ComboBoxEdit cameraDevice = new ComboBoxEdit();
        readonly ComboBoxEdit cameraFrameRate = new ComboBoxEdit();
        readonly CheckEdit cameraGrey = new CheckEdit { Text = "Ảnh đen trắng" };
        readonly CheckEdit cameraInvert = new CheckEdit { Text = "Đảo màu" };
        readonly CheckEdit cameraFlipX = new CheckEdit { Text = "Lật ngang" };
        readonly CheckEdit cameraFlipY = new CheckEdit { Text = "Lật dọc" };

        public SystemSettingsDialog(MySettings settings)
        {
            pacs = settings;
            Text = "Cài đặt hệ thống";
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(940, 620);
            Size = new Size(1080, 700);
            ShowInTaskbar = false;

            var footer = new PanelControl { Dock = DockStyle.Bottom, Height = 62 };
            var cancel = new SimpleButton { Text = "Hủy", DialogResult = DialogResult.Cancel, Size = new Size(95, 34), Margin = new Padding(0, 13, 8, 0) };
            var save = new SimpleButton { Text = "Lưu thay đổi", Size = new Size(118, 34), Margin = new Padding(0, 13, 14, 0) };
            save.Click += Save_Click;
            var footerButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                Width = 245,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };
            footerButtons.Controls.Add(cancel);
            footerButtons.Controls.Add(save);
            footer.Controls.Add(footerButtons);
            Controls.Add(footer);
            CancelButton = cancel;
            AcceptButton = save;

            var tabs = new XtraTabControl
            {
                Dock = DockStyle.Fill,
                HeaderLocation = TabHeaderLocation.Left,
                HeaderOrientation = TabOrientation.Horizontal,
                BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            };
            tabs.AppearancePage.Header.Font = new Font("Tahoma", 9.5F, FontStyle.Bold);
            tabs.AppearancePage.Header.Options.UseFont = true;
            tabs.TabPages.Add(IntegrationPage());
            tabs.TabPages.Add(ModalityPage());
            tabs.TabPages.Add(CameraPage());
            tabs.TabPages.Add(ShortcutPage());
            Controls.Add(tabs);
            LoadValues();
        }

        XtraTabPage IntegrationPage()
        {
            var page = Page("HIS / RIS");
            var body = SettingsBody();
            var table = SettingsTable();
            AddSystemRow(table, "Gateway", "UrlGateway", "Địa chỉ kết nối tập trung của hệ thống");
            AddSystemRow(table, "RIS API", "UrlApiRis", "API RIS phiên bản hiện tại");
            AddSystemRow(table, "RIS API V2", "UrlApiRisV2", "API RIS cho luồng chỉ định mới");
            AddSystemRow(table, "Xác thực RIS", "UrlRisAuthen", "Dịch vụ đăng nhập/xác thực người dùng");
            AddSystemRow(table, "Kiểm tra thanh toán", "CheckThanhToan", "API HIS kiểm tra trạng thái thanh toán");
            AddSystemRow(table, "Dịch vụ ký số", "UrlSignatureMysign", "API ký số kết quả");
            body.Controls.Add(table);
            page.Controls.Add(body);
            page.Controls.Add(Header("Kết nối HIS / RIS", "Các địa chỉ tích hợp nghiệp vụ được nhóm theo đúng mục đích sử dụng."));
            return page;
        }

        XtraTabPage ModalityPage()
        {
            var page = Page("Modality && Worklist");
            var client = new PanelControl { Dock = DockStyle.Top, Height = 70, Padding = new Padding(24, 16, 24, 10) };
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3 };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 280));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.Controls.Add(FieldLabel("AE Title máy trạm"), 0, 0);
            clientAe.Dock = DockStyle.Fill;
            clientAe.Margin = new Padding(0, 5, 14, 5);
            layout.Controls.Add(clientAe, 1, 0);
            var hint = FieldLabel("Được dùng khi truy vấn DICOM Worklist");
            hint.Appearance.ForeColor = Color.DimGray;
            hint.Appearance.Options.UseForeColor = true;
            layout.Controls.Add(hint, 2, 0);
            client.Controls.Add(layout);
            while (worklist.Page.Controls.Count > 0)
                page.Controls.Add(worklist.Page.Controls[0]);
            page.Controls.Add(client);
            page.Controls.Add(Header("Modality và Worklist", "Khai báo định danh máy trạm và nguồn nhận danh sách chỉ định."));
            return page;
        }

        XtraTabPage CameraPage()
        {
            var page = Page("Camera");
            var body = SettingsBody();
            var table = SettingsTable();
            AddControlRow(table, "Thiết bị hình ảnh", cameraDevice, "Camera mặc định dùng khi mở chỉ định");
            cameraDevice.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            try
            {
                foreach (var device in CameraControl.GetVideoDevices())
                    cameraDevice.Properties.Items.Add(device.Name);
            }
            catch { }
            cameraFrameRate.Properties.Items.AddRange(new object[] { "15", "20", "25", "30", "50", "60" });
            AddControlRow(table, "Tốc độ khung hình", cameraFrameRate, "Số khung hình mỗi giây");
            var effects = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true };
            effects.Controls.Add(cameraGrey);
            effects.Controls.Add(cameraInvert);
            effects.Controls.Add(cameraFlipX);
            effects.Controls.Add(cameraFlipY);
            AddControlRow(table, "Hiệu chỉnh hình ảnh", effects, "Áp dụng trực tiếp lên hình ảnh camera");
            body.Controls.Add(table);
            page.Controls.Add(body);
            page.Controls.Add(Header("Camera và hình ảnh", "Cấu hình gọn cho camera mới, không phụ thuộc giao diện Leadtools cũ."));
            return page;
        }

        static PanelControl SettingsBody() => new PanelControl
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(26, 18, 26, 18),
            BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        };

        static TableLayoutPanel SettingsTable()
        {
            var table = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 3 };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
            return table;
        }

        void AddSystemRow(TableLayoutPanel table, string label, string name, string hint)
        {
            var edit = new TextEdit { Dock = DockStyle.Fill, Margin = new Padding(0, 8, 18, 8) };
            systemFields[name] = edit;
            AddControlRow(table, label, edit, hint);
        }

        static void AddControlRow(TableLayoutPanel table, string label, Control control, string hint)
        {
            int row = table.RowCount++;
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
            table.Controls.Add(FieldLabel(label), 0, row);
            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(0, 8, 18, 8);
            table.Controls.Add(control, 1, row);
            var description = FieldLabel(hint);
            description.Appearance.ForeColor = Color.DimGray;
            description.Appearance.Options.UseForeColor = true;
            table.Controls.Add(description, 2, row);
        }

        XtraTabPage ShortcutPage()
        {
            var page = Page("Phím tắt && Máy in");
            var body = new PanelControl { Dock = DockStyle.Fill, Padding = new Padding(24, 18, 24, 18), BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder };
            var table = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 4 };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            AddKeyRow(table, "Tìm kiếm Worklist", "Search", "Ký số", "Sign");
            AddKeyRow(table, "In kết quả", "Print", "Lưu nháp", "Draft");
            AddKeyRow(table, "Đóng màn hình", "Exit", "Lấy ảnh", "CaptureImage");
            AddKeyRow(table, "Xem trước", "Preview", "Kết nối Camera", "LinkCamera");
            AddKeyRow(table, "Chụp ảnh", "Snapshot", "Dừng Camera", "Stop");
            var printLabel = new LabelControl { Text = "MÁY IN KẾT QUẢ MẶC ĐỊNH", Dock = DockStyle.Top, Height = 36 };
            printLabel.Appearance.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            printLabel.Appearance.Options.UseFont = true;
            printer.Dock = DockStyle.Top;
            printer.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            foreach (string name in PrinterSettings.InstalledPrinters) printer.Properties.Items.Add(name);
            body.Controls.Add(printer);
            body.Controls.Add(printLabel);
            body.Controls.Add(table);
            page.Controls.Add(body);
            page.Controls.Add(Header("Phím tắt và máy in mặc định", "Thiết lập thao tác nhanh và máy in kết quả trên máy trạm này."));
            return page;
        }

        void AddKeyRow(TableLayoutPanel table, string text1, string key1, string text2, string key2)
        {
            int row = table.RowCount++;
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
            table.Controls.Add(FieldLabel(text1), 0, row);
            table.Controls.Add(KeyEditor(key1), 1, row);
            table.Controls.Add(FieldLabel(text2), 2, row);
            table.Controls.Add(KeyEditor(key2), 3, row);
        }

        static LabelControl FieldLabel(string text) => new LabelControl
        {
            Text = text, Dock = DockStyle.Fill, AutoSizeMode = LabelAutoSizeMode.None,
            Appearance = { TextOptions = { VAlignment = VertAlignment.Center } }
        };

        ComboBoxEdit KeyEditor(string name)
        {
            var edit = new ComboBoxEdit { Dock = DockStyle.Fill, Margin = new Padding(4, 8, 20, 8) };
            edit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            edit.Properties.Items.Add("Escape");
            for (int i = 1; i <= 12; i++) edit.Properties.Items.Add("F" + i);
            keys[name] = edit;
            return edit;
        }

        static XtraTabPage Page(string text) => new XtraTabPage { Text = text };
        static PanelControl Header(string title, string description)
        {
            var panel = new PanelControl { Dock = DockStyle.Top, Height = 76, Padding = new Padding(20, 12, 20, 8), BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder };
            var titleLabel = new LabelControl { Text = title, Dock = DockStyle.Top, Height = 28 };
            titleLabel.Appearance.Font = new Font("Tahoma", 13F, FontStyle.Bold);
            titleLabel.Appearance.Options.UseFont = true;
            var descriptionLabel = new LabelControl { Text = description, Dock = DockStyle.Top };
            descriptionLabel.Appearance.ForeColor = Color.DimGray;
            descriptionLabel.Appearance.Options.UseForeColor = true;
            panel.Controls.Add(descriptionLabel);
            panel.Controls.Add(titleLabel);
            return panel;
        }

        void LoadValues()
        {
            var app = pacs._settings;
            clientAe.Text = app.clientAE;
            worklist.Load((MyServerList)app.QueryMWLServers.Clone());
            var config = ServiceLocator.SystemConfig ?? new SystemConfig();
            SetSystem("UrlGateway", config.UrlGateway);
            SetSystem("UrlApiRis", config.UrlApiRis);
            SetSystem("UrlApiRisV2", config.UrlApiRisV2);
            SetSystem("UrlRisAuthen", config.UrlRisAuthen);
            SetSystem("CheckThanhToan", config.CheckThanhToan);
            SetSystem("UrlSignatureMysign", config.UrlSignatureMysign);
            var camera = ServiceLocator.CameraSettingConfig ?? new CameraSettings();
            cameraDevice.EditValue = camera.VideoInputDevice;
            cameraFrameRate.EditValue = camera.FrameRate;
            cameraGrey.Checked = camera.Greyscale;
            cameraInvert.Checked = camera.Invert;
            cameraFlipX.Checked = camera.FlipX;
            cameraFlipY.Checked = camera.FlipY;
            var s = ServiceLocator.ShortcutAndFontSetting ?? ShortcutSettingsManager.LoadOrCreateSettings();
            Set("Search", s.AssignedKeys.Search); Set("Sign", s.ConclusionScreenKeys.Sign);
            Set("Print", s.ConclusionScreenKeys.Print); Set("Draft", s.ConclusionScreenKeys.Draft);
            Set("Exit", s.ConclusionScreenKeys.Exit); Set("CaptureImage", s.ConclusionScreenKeys.CaptureImage);
            Set("Preview", s.ConclusionScreenKeys.Preview); Set("LinkCamera", s.ConclusionScreenKeys.LinkCamera);
            Set("Snapshot", s.ConclusionScreenKeys.Snapshot); Set("Stop", s.ConclusionScreenKeys.Stop);
            if (s.PrintSettings == null) s.PrintSettings = new PrintSettings();
            printer.EditValue = s.PrintSettings.Printer;
        }

        void Set(string name, string value) { keys[name].EditValue = value; }
        string Get(string name) => Convert.ToString(keys[name].EditValue);
        void SetSystem(string name, string value) { systemFields[name].Text = value ?? string.Empty; }
        string GetSystem(string name) => systemFields[name].Text.Trim();

        void Save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(clientAe.Text))
            {
                XtraMessageBox.Show(this, "Vui lòng nhập AE Title của máy trạm.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!worklist.Validate(this)) return;
            var app = pacs._settings;
            app.clientAE = clientAe.Text.Trim();
            app.queryMWLServers = worklist.Build();
            app.DefaultMWLServer = worklist.DefaultIndex;
            pacs.Save();

            var config = ServiceLocator.SystemConfig ?? new SystemConfig();
            config.UrlGateway = GetSystem("UrlGateway");
            config.UrlApiRis = GetSystem("UrlApiRis");
            config.UrlApiRisV2 = GetSystem("UrlApiRisV2");
            config.UrlRisAuthen = GetSystem("UrlRisAuthen");
            config.CheckThanhToan = GetSystem("CheckThanhToan");
            config.UrlSignatureMysign = GetSystem("UrlSignatureMysign");
            ServiceLocator.SystemConfig = config;
            XmlSettingsHelper.SaveEncrypted(
                System.IO.Path.Combine(ServiceLocator.GetAppDataBasePath(),
                    FileStorageSettingsProvider.Current.SystemConfigFile), config);

            var camera = ServiceLocator.CameraSettingConfig ?? new CameraSettings();
            camera.VideoInputDevice = Convert.ToString(cameraDevice.EditValue);
            camera.FrameRate = Convert.ToString(cameraFrameRate.EditValue);
            camera.Greyscale = cameraGrey.Checked;
            camera.Invert = cameraInvert.Checked;
            camera.FlipX = cameraFlipX.Checked;
            camera.FlipY = cameraFlipY.Checked;
            ServiceLocator.CameraSettingConfig = camera;
            XmlSettingsHelper.Save(
                System.IO.Path.Combine(ServiceLocator.GetAppDataBasePath(),
                    FileStorageSettingsProvider.Current.CameraConfig), camera);

            var s = ServiceLocator.ShortcutAndFontSetting ?? ShortcutSettingsManager.LoadOrCreateSettings();
            s.AssignedKeys.Search = Get("Search"); s.ConclusionScreenKeys.Sign = Get("Sign");
            s.ConclusionScreenKeys.Print = Get("Print"); s.ConclusionScreenKeys.Draft = Get("Draft");
            s.ConclusionScreenKeys.Exit = Get("Exit"); s.ConclusionScreenKeys.CaptureImage = Get("CaptureImage");
            s.ConclusionScreenKeys.Preview = Get("Preview"); s.ConclusionScreenKeys.LinkCamera = Get("LinkCamera");
            s.ConclusionScreenKeys.Snapshot = Get("Snapshot"); s.ConclusionScreenKeys.Stop = Get("Stop");
            s.PrintSettings.Printer = Convert.ToString(printer.EditValue);
            ServiceLocator.ShortcutAndFontSetting = s;
            ShortcutSettingsManager.SaveSettings(s);
            DialogResult = DialogResult.OK;
            Close();
        }

        sealed class ServerEditor
        {
            readonly BindingList<ServerRow> rows = new BindingList<ServerRow>();
            readonly DataGridView grid;
            MyServerList source;
            public XtraTabPage Page { get; }
            public int DefaultIndex { get { int i = rows.ToList().FindIndex(x => x.IsDefault); return i < 0 ? 0 : i; } }

            public ServerEditor(string caption)
            {
                Page = SystemSettingsDialog.Page(caption);
                var bar = new PanelControl { Dock = DockStyle.Bottom, Height = 52 };
                var add = new SimpleButton { Text = "Thêm máy chủ", Size = new Size(120, 30), Location = new Point(12, 10) };
                var remove = new SimpleButton { Text = "Xóa", Size = new Size(82, 30), Location = new Point(140, 10) };
                add.Click += (s, e) => rows.Add(new ServerRow { AeTitle = "NEW_SERVER", Host = "127.0.0.1", Port = 104, Timeout = 30 });
                remove.Click += (s, e) => { if (grid.CurrentRow?.DataBoundItem is ServerRow row) rows.Remove(row); };
                bar.Controls.Add(add); bar.Controls.Add(remove);
                grid = new DataGridView
                {
                    Dock = DockStyle.Fill, AutoGenerateColumns = false, DataSource = rows,
                    AllowUserToAddRows = false, AllowUserToDeleteRows = false, RowHeadersVisible = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
                    BackgroundColor = SystemColors.Window, BorderStyle = BorderStyle.None,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                };
                grid.RowTemplate.Height = 30;
                grid.Columns.Add(Column("AeTitle", "AE Title", 22));
                grid.Columns.Add(Column("Host", "Địa chỉ IP / Host", 30));
                grid.Columns.Add(Column("Port", "Cổng", 12));
                grid.Columns.Add(Column("Timeout", "Timeout (giây)", 15));
                grid.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = "UseTls", HeaderText = "TLS", FillWeight = 9 });
                grid.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = "IsDefault", HeaderText = "Mặc định", FillWeight = 12 });
                grid.CurrentCellDirtyStateChanged += (s, e) => { if (grid.IsCurrentCellDirty) grid.CommitEdit(DataGridViewDataErrorContexts.Commit); };
                grid.CellValueChanged += (s, e) =>
                {
                    if (e.RowIndex < 0 || e.ColumnIndex != 5 || !rows[e.RowIndex].IsDefault) return;
                    for (int i = 0; i < rows.Count; i++) if (i != e.RowIndex) rows[i].IsDefault = false;
                    grid.Refresh();
                };
                Page.Controls.Add(grid); Page.Controls.Add(bar);
            }

            static DataGridViewTextBoxColumn Column(string property, string title, float width) =>
                new DataGridViewTextBoxColumn { DataPropertyName = property, HeaderText = title, FillWeight = width };

            public void Load(MyServerList list)
            {
                source = list; rows.Clear();
                foreach (MyServer server in list.serverArrayList)
                    rows.Add(new ServerRow { AeTitle = server._sAE, Host = server._sIP, Port = server._port, Timeout = server._timeout, UseTls = server._useTls, IsDefault = string.Equals(server._sAE, list.currentServerAE, StringComparison.OrdinalIgnoreCase) });
                if (rows.Count > 0 && !rows.Any(x => x.IsDefault)) rows[0].IsDefault = true;
            }

            public bool Validate(IWin32Window owner)
            {
                grid.EndEdit();
                bool invalid = rows.Count == 0 || rows.Any(x => string.IsNullOrWhiteSpace(x.AeTitle) || string.IsNullOrWhiteSpace(x.Host) || x.Port < 1 || x.Port > 65535 || x.Timeout < 1);
                if (invalid)
                {
                    XtraMessageBox.Show(owner, $"{Page.Text}: kiểm tra AE Title, địa chỉ, cổng và timeout.", "Dữ liệu chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (rows.GroupBy(x => x.AeTitle.Trim(), StringComparer.OrdinalIgnoreCase).Any(x => x.Count() > 1))
                {
                    XtraMessageBox.Show(owner, $"{Page.Text}: AE Title không được trùng nhau.", "Dữ liệu chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (!rows.Any(x => x.IsDefault)) rows[0].IsDefault = true;
                return true;
            }

            public MyServerList Build()
            {
                var list = source ?? new MyServerList();
                list.serverArrayList = new ArrayList();
                foreach (var row in rows) list.serverArrayList.Add(new MyServer(row.AeTitle.Trim(), row.Host.Trim(), row.Port, row.Timeout, row.UseTls));
                list.currentServerAE = rows[DefaultIndex].AeTitle.Trim();
                return list;
            }
        }

        sealed class ServerRow : INotifyPropertyChanged
        {
            bool isDefault;
            public string AeTitle { get; set; }
            public string Host { get; set; }
            public int Port { get; set; }
            public int Timeout { get; set; }
            public bool UseTls { get; set; }
            public bool IsDefault { get => isDefault; set { if (isDefault == value) return; isDefault = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDefault))); } }
            public event PropertyChangedEventHandler PropertyChanged;
        }
    }
}
