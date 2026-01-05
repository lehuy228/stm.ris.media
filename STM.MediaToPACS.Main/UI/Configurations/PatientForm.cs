using DevExpress.XtraEditors;
using MediaToPacs.Core.Models;
using STM.MediaToPACS.Main.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STM.MediaToPACS.Main.UI.Configurations
{
    public partial class PatientForm : XtraForm
    {
        private BenhNhan _benhNhan;
        private readonly string _maChiDinh;

        public PatientForm(BenhNhan benhNhan, string maChiDinh)
        {
            InitializeComponent();
            _benhNhan = benhNhan;
            _maChiDinh = maChiDinh;
        }

        private void PatientForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        #region Load Data
        private void LoadData()
        {
            if (_benhNhan == null)
                return;

            try
            {
                _txtMaBenhNhan.Text = _benhNhan.MaBenhNhan ?? "";
                _txtHoTen.Text = _benhNhan.HoTen ?? "";

                _dtpNgaySinh.EditValue =
                    _benhNhan.NgaySinh > DateTime.MinValue
                        ? (object)_benhNhan.NgaySinh
                        : null;

                if (_benhNhan.GioiTinh == 0)
                {
                    _cboGioiTinh.Text = "Nam";
                }
                else if (_benhNhan.GioiTinh == 1)
                {
                    _cboGioiTinh.Text = "Nữ";
                }
                else
                {
                    _cboGioiTinh.Text = "Khác";
                }


                _cboDanToc.Text = _benhNhan.DanToc ?? "";
                _txtMaBHYT.Text = _benhNhan.MaBHYT ?? "";

                _dtpTuNgayBHYT.EditValue =
                    _benhNhan.TuNgayBHYT > DateTime.MinValue
                        ? (object)_benhNhan.TuNgayBHYT
                        : null;

                _dtpDenNgayBHYT.EditValue =
                    _benhNhan.DenNgayBHYT > DateTime.MinValue
                        ? (object)_benhNhan.DenNgayBHYT
                        : null;

                _cboTinhThanh.Text = _benhNhan.TinhThanh ?? "";
                _cboXaPhuong.Text = _benhNhan.XaPhuong ?? "";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(
                    $"Lỗi khi tải dữ liệu: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Button Events
        private void _btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void _btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateModelFromUI();

                await ServiceLocator.RisService.UpdateChiDinhDichVu(_maChiDinh, _benhNhan);

                XtraMessageBox.Show(
                    "Cập nhật thông tin bệnh nhân thành công",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(
                    $"Lỗi khi lưu dữ liệu: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Update Model
        private void UpdateModelFromUI()
        {
            _benhNhan.HoTen = _txtHoTen.Text?.Trim();
            _benhNhan.NgaySinh = _dtpNgaySinh.EditValue == null
                ? DateTime.MinValue
                : _dtpNgaySinh.DateTime;

            if (_cboGioiTinh.Text == "Nam")
            {
                _benhNhan.GioiTinh = 0;
            }
            else if (_cboGioiTinh.Text == "Nữ")
            {
                _benhNhan.GioiTinh = 1;
            }
            _benhNhan.DanToc = _cboDanToc.Text?.Trim();
            _benhNhan.MaBHYT = _txtMaBHYT.Text?.Trim();

            _benhNhan.TuNgayBHYT = _dtpTuNgayBHYT.EditValue == null
                ? DateTime.MinValue
                : _dtpTuNgayBHYT.DateTime;

            _benhNhan.DenNgayBHYT = _dtpDenNgayBHYT.EditValue == null
                ? DateTime.MinValue
                : _dtpDenNgayBHYT.DateTime;

            _benhNhan.TinhThanh = _cboTinhThanh.Text?.Trim();
            _benhNhan.XaPhuong = _cboXaPhuong.Text?.Trim();
        }
        #endregion
    }
}
