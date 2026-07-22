using DevExpress.XtraEditors;
using MediaToPacs.Core.Models;
using MediaToPacs.Core.Utilities;
using STM.MediaToPACS.Main.Utilities;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace STM.MediaToPACS.Main.UI
{
    internal sealed class DoctorQuickSettingsControl : UserControl
    {
        private readonly TextBox _gatewayTextBox;
        private readonly TextBox _hisPaymentTextBox;
        private readonly TextBox _fileStoragePathTextBox;
        private readonly Button _browseFileStoragePathButton;
        private readonly RichTextBox _modalitiesTextBox;
        private readonly string _systemConfigPath;
        private readonly string _modalitiesPath;

        public event EventHandler CloseRequested;

        public DoctorQuickSettingsControl()
        {
            BackColor = Color.White;
            Dock = DockStyle.Fill;
            Font = new Font("Segoe UI", 10F);
            Padding = new Padding(24, 18, 24, 18);

            string basePath = ServiceLocator.GetAppDataBasePath();
            var fileSettings = FileStorageSettingsProvider.Current;
            _systemConfigPath = Path.Combine(basePath, fileSettings.SystemConfigFile);
            _modalitiesPath = Path.Combine(basePath, fileSettings.Modality);

            _gatewayTextBox = CreateTextBox();
            _hisPaymentTextBox = CreateTextBox();
            _fileStoragePathTextBox = CreateTextBox();
            _browseFileStoragePathButton = CreateButton("...", Color.White, Color.FromArgb(55, 65, 81));
            _browseFileStoragePathButton.Size = new Size(38, 30);
            _browseFileStoragePathButton.Click += BrowseFileStoragePathButton_Click;
            _modalitiesTextBox = new RichTextBox
            {
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                TabIndex = 2
            };

            var scrollHost = new Panel
            {
                AutoScroll = true,
                BackColor = Color.White,
                Dock = DockStyle.Fill
            };
            scrollHost.Controls.Add(BuildLayout());

            Controls.Add(scrollHost);
            LoadSettings();
        }

        private Control BuildLayout()
        {
            var layout = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.White,
                ColumnCount = 1,
                Dock = DockStyle.Top,
                RowCount = 13
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 12F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));

            var title = CreateLabel("Cấu hình hệ thống", 14F, FontStyle.Bold);
            var description = CreateLabel(
                "Thiết lập các kết nối cần thiết trước khi tiếp tục.", 9.5F, FontStyle.Regular);
            description.ForeColor = Color.FromArgb(91, 100, 110);

            layout.Controls.Add(title, 0, 0);
            layout.Controls.Add(description, 0, 1);
            layout.Controls.Add(CreateLabel("Địa chỉ server", 10F, FontStyle.Bold), 0, 2);
            layout.Controls.Add(WrapInput(_gatewayTextBox), 0, 3);
            layout.Controls.Add(CreateLabel("Địa chỉ Check thanh toán HIS", 10F, FontStyle.Bold), 0, 4);
            layout.Controls.Add(WrapInput(_hisPaymentTextBox), 0, 5);
            layout.Controls.Add(CreateLabel("Thư mục lưu ảnh/video bệnh nhân", 10F, FontStyle.Bold), 0, 6);
            layout.Controls.Add(WrapInput(BuildFileStoragePathInput()), 0, 7);
            layout.Controls.Add(CreateLabel("Phương thức chụp", 10F, FontStyle.Bold), 0, 8);
            layout.Controls.Add(_modalitiesTextBox, 0, 9);

            var hint = CreateLabel("Ví dụ: CR, CT, MR, US", 9F, FontStyle.Italic);
            hint.ForeColor = Color.FromArgb(108, 117, 125);
            layout.Controls.Add(hint, 0, 10);

            var buttons = new FlowLayoutPanel
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Margin = Padding.Empty,
                WrapContents = false
            };
            var saveButton = CreateButton("Lưu cấu hình", Color.FromArgb(15, 72, 116), Color.White);
            saveButton.Click += SaveButton_Click;
            var cancelButton = CreateButton("Hủy", Color.White, Color.FromArgb(55, 65, 81));
            cancelButton.FlatAppearance.BorderColor = Color.FromArgb(198, 207, 216);
            cancelButton.Click += (sender, args) => CloseRequested?.Invoke(this, EventArgs.Empty);
            buttons.Controls.Add(saveButton);
            buttons.Controls.Add(cancelButton);
            layout.Controls.Add(buttons, 0, 12);

            return layout;
        }

        private Control BuildFileStoragePathInput()
        {
            var panel = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                RowCount = 1
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 42F));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panel.Controls.Add(_fileStoragePathTextBox, 0, 0);
            panel.Controls.Add(_browseFileStoragePathButton, 1, 0);
            return panel;
        }

        private void BrowseFileStoragePathButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Chọn thư mục lưu ảnh/video bệnh nhân";
                if (Directory.Exists(_fileStoragePathTextBox.Text))
                    dialog.SelectedPath = _fileStoragePathTextBox.Text;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                    _fileStoragePathTextBox.Text = dialog.SelectedPath;
            }
        }

        private static Control WrapInput(Control input)
        {
            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 2, 0, 8) };
            panel.Controls.Add(input);
            return panel;
        }

        private void LoadSettings()
        {
            var systemConfig = ServiceLocator.SystemConfig
                ?? XmlSettingsHelper.LoadEncrypted<SystemConfig>(_systemConfigPath)
                ?? new SystemConfig();
            var modalities = XmlSettingsHelper.Load<Modalities>(_modalitiesPath) ?? new Modalities();

            _gatewayTextBox.Text = systemConfig.UrlGateway ?? string.Empty;
            _hisPaymentTextBox.Text = systemConfig.CheckThanhToan ?? string.Empty;
            _fileStoragePathTextBox.Text = systemConfig.FileStoragePath ?? string.Empty;
            _modalitiesTextBox.Text = modalities.ModalitiesList ?? string.Empty;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                var systemConfig = ServiceLocator.SystemConfig
                    ?? XmlSettingsHelper.LoadEncrypted<SystemConfig>(_systemConfigPath)
                    ?? new SystemConfig();
                systemConfig.UrlGateway = _gatewayTextBox.Text.Trim();
                systemConfig.CheckThanhToan = _hisPaymentTextBox.Text.Trim();
                systemConfig.FileStoragePath = _fileStoragePathTextBox.Text.Trim();
                ServiceLocator.SystemConfig = systemConfig;
                ServiceLocator.InitializeOptionalServices();
                XmlSettingsHelper.SaveEncrypted(_systemConfigPath, systemConfig);

                var modalities = XmlSettingsHelper.Load<Modalities>(_modalitiesPath) ?? new Modalities();
                modalities.ModalitiesList = _modalitiesTextBox.Text.Trim().ToUpperInvariant();
                XmlSettingsHelper.Save(_modalitiesPath, modalities);

                XtraMessageBox.Show(this, "Đã lưu cấu hình.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                CloseRequested?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(this, "Không thể lưu cấu hình: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static Label CreateLabel(string text, float size, FontStyle style)
        {
            return new Label
            {
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", size, style),
                Text = text,
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private static TextBox CreateTextBox()
        {
            return new TextBox
            {
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F)
            };
        }

        private static Button CreateButton(string text, Color backColor, Color foreColor)
        {
            return new Button
            {
                BackColor = backColor,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = foreColor,
                Margin = new Padding(8, 2, 0, 2),
                Size = new Size(120, 38),
                Text = text,
                UseVisualStyleBackColor = false
            };
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            //
            // DoctorQuickSettingsControl
            //
            this.Name = "DoctorQuickSettingsControl";
            this.Size = new System.Drawing.Size(552, 460);
            this.ResumeLayout(false);

        }
    }
}
