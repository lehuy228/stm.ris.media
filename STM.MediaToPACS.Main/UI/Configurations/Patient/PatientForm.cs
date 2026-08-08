using DevExpress.XtraEditors;
using MediaToPacs.Core.Interfaces;
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
        private readonly IRisService _risService;
        private Patient _Patient;
        private readonly string _maChiDinh;

        public PatientForm(IRisService risService, Patient Patient, string maChiDinh)
        {
            _risService = risService;
            InitializeComponent();
            _Patient = Patient;
            _maChiDinh = maChiDinh;
        }

        private void PatientForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        #region Load Data
        private void LoadData()
        {
            if (_Patient == null)
                return;

            try
            {
                _txtMaBenhNhan.Text = _Patient.MaBenhNhan ?? "";
                _txtHoTen.Text = _Patient.HoTen ?? "";

                _dtpNgaySinh.EditValue =
                    _Patient.NgaySinh > DateTime.MinValue
                        ? (object)_Patient.NgaySinh
                        : null;

                if (_Patient.GioiTinh == 0)
                {
                    _cboGioiTinh.Text = "Nam";
                }
                else if (_Patient.GioiTinh == 1)
                {
                    _cboGioiTinh.Text = "Nữ";
                }
                else
                {
                    _cboGioiTinh.Text = "Khác";
                }


                _cboDanToc.Text = _Patient.DanToc ?? "";
                _txtMaBHYT.Text = _Patient.MaBHYT ?? "";

                _dtpTuNgayBHYT.EditValue =
                    _Patient.TuNgayBHYT > DateTime.MinValue
                        ? (object)_Patient.TuNgayBHYT
                        : null;

                _dtpDenNgayBHYT.EditValue =
                    _Patient.DenNgayBHYT > DateTime.MinValue
                        ? (object)_Patient.DenNgayBHYT
                        : null;

                _cboTinhThanh.Text = _Patient.TinhThanh ?? "";
                _cboXaPhuong.Text = _Patient.XaPhuong ?? "";
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

                await _risService.UpdateChiDinhDichVu(_maChiDinh, _Patient);

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
            _Patient.HoTen = _txtHoTen.Text?.Trim();
            _Patient.NgaySinh = _dtpNgaySinh.EditValue == null
                ? DateTime.MinValue
                : _dtpNgaySinh.DateTime;

            if (_cboGioiTinh.Text == "Nam")
            {
                _Patient.GioiTinh = 0;
            }
            else if (_cboGioiTinh.Text == "Nữ")
            {
                _Patient.GioiTinh = 1;
            }
            _Patient.DanToc = _cboDanToc.Text?.Trim();
            _Patient.MaBHYT = _txtMaBHYT.Text?.Trim();

            _Patient.TuNgayBHYT = _dtpTuNgayBHYT.EditValue == null
                ? DateTime.MinValue
                : _dtpTuNgayBHYT.DateTime;

            _Patient.DenNgayBHYT = _dtpDenNgayBHYT.EditValue == null
                ? DateTime.MinValue
                : _dtpDenNgayBHYT.DateTime;

            _Patient.TinhThanh = _cboTinhThanh.Text?.Trim();
            _Patient.XaPhuong = _cboXaPhuong.Text?.Trim();
        }
        #endregion
    }
}
