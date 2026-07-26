using System;
using System.Drawing.Printing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MediaToPacs.Core.Models;
using MediaToPacs.Core.Utilities;
using STM.MediaToPACS.Main.Utilities;

namespace STM.MediaToPACS.Main.UI.Configurations
{
    public sealed partial class SystemSettingsDialog : XtraForm
    {
        public SystemSettingsDialog(MySettings settings)
        {
            InitializeComponent();
            PopulatePrinterList();
            LoadValues();
        }

        void PopulatePrinterList()
        {
            printerComboBox.Properties.Items.Clear();
            foreach (string name in PrinterSettings.InstalledPrinters)
                printerComboBox.Properties.Items.Add(name);
        }

        void LoadValues()
        {
            var config = ServiceLocator.SystemConfig ?? new SystemConfig();
            serverAddressTextEdit.Text = config.UrlGateway ?? string.Empty;
            paymentCheckTextEdit.Text = config.CheckThanhToan ?? string.Empty;

            var settings = ServiceLocator.ShortcutAndFontSetting
                ?? ShortcutSettingsManager.LoadOrCreateSettings();

            searchKeyComboBox.EditValue = settings.AssignedKeys.Search;
            signKeyComboBox.EditValue = settings.ConclusionScreenKeys.Sign;
            printKeyComboBox.EditValue = settings.ConclusionScreenKeys.Print;
            draftKeyComboBox.EditValue = settings.ConclusionScreenKeys.Draft;
            exitKeyComboBox.EditValue = settings.ConclusionScreenKeys.Exit;
            captureImageKeyComboBox.EditValue = settings.ConclusionScreenKeys.CaptureImage;
            previewKeyComboBox.EditValue = settings.ConclusionScreenKeys.Preview;
            linkCameraKeyComboBox.EditValue = settings.ConclusionScreenKeys.LinkCamera;
            snapshotKeyComboBox.EditValue = settings.ConclusionScreenKeys.Snapshot;
            stopCameraKeyComboBox.EditValue = settings.ConclusionScreenKeys.Stop;

            if (settings.PrintSettings == null)
                settings.PrintSettings = new PrintSettings();
            printerComboBox.EditValue = settings.PrintSettings.Printer;
        }

        string SelectedShortcut(ComboBoxEdit editor) => Convert.ToString(editor.EditValue);

        void Save_Click(object sender, EventArgs e)
        {
            var config = ServiceLocator.SystemConfig ?? new SystemConfig();
            config.UrlGateway = serverAddressTextEdit.Text.Trim();
            config.CheckThanhToan = paymentCheckTextEdit.Text.Trim();
            ServiceLocator.SystemConfig = config;
            ServiceLocator.InitializeOptionalServices();
            XmlSettingsHelper.SaveEncrypted(
                System.IO.Path.Combine(ServiceLocator.GetAppDataBasePath(),
                    FileStorageSettingsProvider.Current.SystemConfigFile), config);

            configCamera.SaveSettingsCamera();

            var settings = ServiceLocator.ShortcutAndFontSetting
                ?? ShortcutSettingsManager.LoadOrCreateSettings();
            if (settings.PrintSettings == null)
                settings.PrintSettings = new PrintSettings();

            settings.AssignedKeys.Search = SelectedShortcut(searchKeyComboBox);
            settings.ConclusionScreenKeys.Sign = SelectedShortcut(signKeyComboBox);
            settings.ConclusionScreenKeys.Print = SelectedShortcut(printKeyComboBox);
            settings.ConclusionScreenKeys.Draft = SelectedShortcut(draftKeyComboBox);
            settings.ConclusionScreenKeys.Exit = SelectedShortcut(exitKeyComboBox);
            settings.ConclusionScreenKeys.CaptureImage = SelectedShortcut(captureImageKeyComboBox);
            settings.ConclusionScreenKeys.Preview = SelectedShortcut(previewKeyComboBox);
            settings.ConclusionScreenKeys.LinkCamera = SelectedShortcut(linkCameraKeyComboBox);
            settings.ConclusionScreenKeys.Snapshot = SelectedShortcut(snapshotKeyComboBox);
            settings.ConclusionScreenKeys.Stop = SelectedShortcut(stopCameraKeyComboBox);
            settings.PrintSettings.Printer = Convert.ToString(printerComboBox.EditValue);

            ServiceLocator.ShortcutAndFontSetting = settings;
            ShortcutSettingsManager.SaveSettings(settings);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
