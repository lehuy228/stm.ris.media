using DevExpress.XtraEditors;
using MediaToPacs.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STM.MediaToPACS.Main.Utilities
{
    public partial class SetUpKeyShortcuts : DevExpress.XtraEditors.XtraForm
    {
        private ShortcutAndFontSettings _settings;

        public SetUpKeyShortcuts()
        {
            InitializeComponent();
            LoadSettingsToUI();
            LoadFontList();
            LoadFontSizes();
            LoadPrinterList();
        }

        private void LoadSettingsToUI()
        {
            _cbbKeywordName.EditValueChanged += OnFontSettingChanged;
            _numKeywordSize.ValueChanged += OnFontSettingChanged;
            _tcKySo.KeyDown += OnlyFunctionKeys;
            _txIn.KeyDown += OnlyFunctionKeys;
            _txLuuNhap.KeyDown += OnlyFunctionKeys;
            _txThoat.KeyDown += OnlyFunctionKeys;
            _txXemTruoc.KeyDown += OnlyFunctionKeys;
            _txChupNhanh.KeyDown += OnlyFunctionKeys;
            _txLienKet.KeyDown += OnlyFunctionKeys;
            _txDung.KeyDown += OnlyFunctionKeys;

            _txSearchMain.KeyDown += OnlyFunctionKeys;

            // Đọc hoặc tạo mới nếu file chưa có
            _settings = ServiceLocator.ShortcutAndFontSetting;

            // ========== 1. Danh sách chỉ định ==========
            _txSearchMain.Text = _settings.AssignedKeys.Search;

            // ========== 2. Màn hình kết luận ==========
            _tcKySo.Text = _settings.ConclusionScreenKeys.Sign;
            _txIn.Text = _settings.ConclusionScreenKeys.Print;
            _txLuuNhap.Text = _settings.ConclusionScreenKeys.Draft;
            _txThoat.Text = _settings.ConclusionScreenKeys.Exit;
            _txXemTruoc.Text = _settings.ConclusionScreenKeys.Preview;
            _txChupNhanh.Text = _settings.ConclusionScreenKeys.Snapshot;
            _txLienKet.Text = _settings.ConclusionScreenKeys.LinkCamera;
            _txDung.Text = _settings.ConclusionScreenKeys.Stop;

            // ========== 3. Font settings ==========
            _cbbKeywordName.EditValue = _settings.FontSettings.FontFamily;
            _numKeywordSize.Value = _settings.FontSettings.FontSize;

            _cbbPrint.EditValue = _settings.PrintSettings.Printer;

            // RichTextBox sample hiển thị preview font
            UpdateSamplePreview();
        }


        private void UpdateSamplePreview()
        {
            string fontName = _cbbKeywordName.EditValue?.ToString() ?? "Arial";
            int fontSize = (int)_numKeywordSize.Value;

            try
            {
                _richKeyWordSample.Font = new Font(fontName, fontSize);
            }
            catch
            {
                _richKeyWordSample.Font = new Font("Arial", 12);
            }
        }

        private void LoadFontList()
        {
            var fontFamilies = FontFamily.Families.Select(f => f.Name).ToList();

            _cbbKeywordName.Properties.Items.Clear();
            _cbbKeywordName.Properties.Items.AddRange(fontFamilies);

            // Chọn font mặc định nếu chưa có
            if (_cbbKeywordName.EditValue == null)
                _cbbKeywordName.EditValue = "Arial";
        }

        private void LoadPrinterList()
        {
            // Lấy danh sách máy in đã cài trên máy
            var printers = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Cast<string>().ToList();

            // Xóa và thêm vào ComboBox
            _cbbPrint.Properties.Items.Clear();
            _cbbPrint.Properties.Items.AddRange(printers);

            // Chọn máy in mặc định nếu chưa chọn
            if (_cbbPrint.EditValue == null && printers.Count > 0)
                _cbbPrint.EditValue = new System.Drawing.Printing.PrinterSettings().PrinterName;
        }


        private void LoadFontSizes()
        {
            _numKeywordSize.Minimum = 6;
            _numKeywordSize.Maximum = 72;
            _numKeywordSize.Value = 12; // mặc định

            _numKeywordSize.Increment = 1;
        }

        private void OnFontSettingChanged(object sender, EventArgs e)
        {
            UpdateSamplePreview();
        }

        private void OnlyFunctionKeys(object sender, KeyEventArgs e)
        {
            TextEdit txt = sender as TextEdit;

            // ESC
            if (e.KeyCode == Keys.Escape)
            {
                txt.Text = "Escape";
                e.SuppressKeyPress = true;
                return;
            }

            // F1 → F12
            if (e.KeyCode >= Keys.F1 && e.KeyCode <= Keys.F12)
            {
                txt.Text = e.KeyCode.ToString();
                e.SuppressKeyPress = true;
                return;
            }

            e.SuppressKeyPress = true;
        }


        private void _btnSave_Click(object sender, EventArgs e)
        {
            _settings.AssignedKeys.Search = _txSearchMain.Text;

            _settings.ConclusionScreenKeys.Sign = _tcKySo.Text;
            _settings.ConclusionScreenKeys.Print = _txIn.Text;
            _settings.ConclusionScreenKeys.Draft = _txLuuNhap.Text;
            _settings.ConclusionScreenKeys.Exit = _txThoat.Text;
            _settings.ConclusionScreenKeys.Preview = _txXemTruoc.Text;
            _settings.ConclusionScreenKeys.Snapshot = _txChupNhanh.Text;
            _settings.ConclusionScreenKeys.LinkCamera = _txLienKet.Text;
            _settings.ConclusionScreenKeys.Stop = _txDung.Text;

            _settings.FontSettings.FontFamily = _cbbKeywordName.EditValue?.ToString();
            _settings.FontSettings.FontSize = (int)_numKeywordSize.Value;

            _settings.PrintSettings.Printer = _cbbPrint.EditValue?.ToString();

            ShortcutSettingsManager.SaveSettings(_settings);
            ServiceLocator.ShortcutAndFontSetting = _settings;
            MessageBox.Show("Đã lưu cài đặt thành công!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void _btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
