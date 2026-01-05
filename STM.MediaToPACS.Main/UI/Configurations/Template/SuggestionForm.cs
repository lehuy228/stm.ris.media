using DevExpress.XtraEditors;
using MediaToPacs.Core.Models.Ketluan;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STM.MediaToPACS.Main.Utilities
{
    public partial class SuggestionForm : XtraForm
    {
        private string _id { get; set; }
        private DanhMucDichVuGridView _dichVu { get; set; }
        private List<ThietBiResponse> _listThietBi { get; set; }
        private RichTextBox GetCurrentBox()
        {
            return ActiveControl as RichTextBox;

        }

        public SuggestionForm(string id = null, DanhMucDichVuGridView dichVu = null)
        {
            InitializeComponent();
            _id = id;
            _dichVu = dichVu;
        }

        private void SuggestionForm_Load(object sender, EventArgs e)
        {
            contextMenuRichTextBox.Items.Add("Sao chép", null, (s, ev) => GetCurrentBox()?.Copy());
            contextMenuRichTextBox.Items.Add("Dán", null, (s, ev) => GetCurrentBox()?.Paste());
            contextMenuRichTextBox.Items.Add("Cắt", null, (s, ev) => GetCurrentBox()?.Cut());
            contextMenuRichTextBox.Items.Add("Chọn tất cả", null, (s, ev) => GetCurrentBox()?.SelectAll());

            _richKetLuan.Font = new Font(ServiceLocator.ShortcutAndFontSetting.FontSettings.FontFamily, ServiceLocator.ShortcutAndFontSetting.FontSettings.FontSize);
            _richKhuyenNghi.Font = new Font(ServiceLocator.ShortcutAndFontSetting.FontSettings.FontFamily, ServiceLocator.ShortcutAndFontSetting.FontSettings.FontSize);
            _richMoTa.Font = new Font(ServiceLocator.ShortcutAndFontSetting.FontSettings.FontFamily, ServiceLocator.ShortcutAndFontSetting.FontSettings.FontSize);
            LoadData();
        }

        private async Task InitDanhSachThietbi()
        {
            try
            {
                // Lấy danh sách thiết bị từ RIS Service
                _listThietBi = (await ServiceLocator.RisService.GetDSThietBiAsync(loaiThietBi: "Máy chụp")).data;

                // Gán datasource cho LookUpEdit
                _cbbDSThietBi.Properties.DataSource = _listThietBi;
                _cbbDSThietBi.Properties.DisplayMember = "tenThietBi";
                _cbbDSThietBi.Properties.ValueMember = "id";
                _cbbDSThietBi.Properties.NullText = "Chọn thiết bị...";

                // Bật tìm kiếm thông minh
                _cbbDSThietBi.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
                _cbbDSThietBi.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                _cbbDSThietBi.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;

                // Hiển thị cột
                _cbbDSThietBi.Properties.PopulateColumns();
                _cbbDSThietBi.Properties.Columns["id"].Visible = false;
                _cbbDSThietBi.Properties.Columns["loaiThietBi"].Visible = false;
                _cbbDSThietBi.Properties.Columns["trangThaiThietBi"].Visible = false;
                _cbbDSThietBi.Properties.Columns["maThietBi"].Visible = true;
                _cbbDSThietBi.Properties.Columns["maThietBi"].Caption = "Mã thiết bị";
                _cbbDSThietBi.Properties.Columns["tenThietBi"].Caption = "Tên thiết bị";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách thiết bị: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadData()
        {
            try
            {
                await InitDanhSachThietbi();
                this.Text = _dichVu.tendichvu;

                if (_id != null)
                {
                    var goiY = await ServiceLocator.RisService.GetGoiYKetLuanById(_id);
                    if (goiY != null)
                    {

                        _txName.Text = goiY.name;
                        _richKetLuan.Text = goiY.kqcls_ketluan;
                        _richKhuyenNghi.Text = goiY.kqcls_denghi;
                        _richMoTa.Text = goiY.kqcls_mota;
                        _cbbGender.Text = goiY.gioitinh;
                        var tb = _listThietBi.FirstOrDefault(x => x.maThietBi == goiY.mathietbi);

                        if (tb != null)
                        {
                            _cbbDSThietBi.EditValue = tb.id;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi khi tải dữ liệu gợi ý: {ex.Message}");
            }
        }

        private async void _btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string gioitinh = string.IsNullOrWhiteSpace(_cbbGender.Text)
                    ? ""
                    : _cbbGender.Text;
                string maThietBi = "";
                var selectedValue = _cbbDSThietBi.EditValue;
                if (selectedValue != null)
                {
                    var thietBiSelect = _cbbDSThietBi.Properties
                        .GetDataSourceRowByKeyValue(selectedValue) as ThietBiResponse;
                    maThietBi = thietBiSelect?.maThietBi ?? "";
                }

                if (string.IsNullOrEmpty(_txName.Text))
                {
                    MessageBox.Show("Bạn chưa nhập tên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _txName.Focus();
                    return;
                }
                var request = new GoiYKetLuanRequest
                {
                    name = _txName.Text,
                    madichvu = _dichVu.madichvu,
                    kqcls_ketluan = _richKetLuan.Text,
                    kqcls_denghi = _richKhuyenNghi.Text,
                    kqcls_mota = _richMoTa.Text,
                    gioitinh = gioitinh,
                    mathietbi = maThietBi
                };

                GoiYKetLuanResponse result;

                if (string.IsNullOrEmpty(_id))
                {
                    result = await ServiceLocator.RisService.TaoMoiGoiYKetLuanAsync(request);
                    XtraMessageBox.Show("Tạo mới gợi ý kết luận thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    result = await ServiceLocator.RisService.CapNhatGoiYKetLuanAsync(request, _id);
                    XtraMessageBox.Show("Cập nhật gợi ý kết luận thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi khi lưu gợi ý kết luận: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}