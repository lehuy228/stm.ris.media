using Serilog;
using STM.MediaToPACS.Main.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STM.MediaToPACS.Main.UI
{
    public partial class FrmDoctorConfirm : DevExpress.XtraEditors.XtraForm
    {
        private readonly DoctorConfirmDrawerController _drawerController;

        public string SelectedOrganizationCode => cboKhoa.EditValue as string;

        private sealed class DepartmentLookupItem
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }

        public FrmDoctorConfirm()
        {
            InitializeComponent();
            _drawerController = new DoctorConfirmDrawerController(this);
            LoadDoctorInfo();
            Load += FrmDoctorConfirm_Load;
            FormClosed += (sender, args) => _drawerController.Dispose();
        }

        private async void FrmDoctorConfirm_Load(object sender, EventArgs e)
        {
            await LoadKhoaListAsync();
        }

        private async void btnReloadKhoa_Click(object sender, EventArgs e)
        {
            await LoadKhoaListAsync();
        }

        private void btnSystemSettings_Click(object sender, EventArgs e)
        {
            _drawerController.Toggle();
        }

        private void LoadDoctorInfo()
        {
            var user = ServiceLocator.KeycloakUserInfo;
            if (user == null) return;

            lblDoctorNameValue.Text = $"{user.FirstName} {user.LastName}".Trim();
            lblDoctorCodeValue.Text = string.IsNullOrWhiteSpace(user.HISCode) ? "-" : user.HISCode;
            lblEmailValue.Text = string.IsNullOrWhiteSpace(user.Email) ? "-" : user.Email;
        }

        private async System.Threading.Tasks.Task LoadKhoaListAsync()
        {
            cboKhoa.Properties.NullText = "Đang tải danh sách khoa...";
            cboKhoa.EditValue = null;
            cboKhoa.Enabled = false;
            btnReloadKhoa.Enabled = false;
            lblKhoaError.Visible = false;

            try
            {
                var departments = await ServiceLocator.RisService2.GetDepartmentsAsync();
                var departmentItems = (departments ?? new List<global::MediaToPacs.Core.Models.OrganizationDto>())
                    .Where(x => !string.IsNullOrWhiteSpace(x.code) || !string.IsNullOrWhiteSpace(x.name))
                    .Select(x => new DepartmentLookupItem
                    {
                        Code = x.code,
                        Name = string.IsNullOrWhiteSpace(x.name) ? x.code : x.name
                    })
                    .ToList();

                cboKhoa.Properties.DataSource = departmentItems;
                cboKhoa.Properties.DisplayMember = nameof(DepartmentLookupItem.Name);
                cboKhoa.Properties.ValueMember = nameof(DepartmentLookupItem.Code);
                cboKhoa.Properties.NullText = "-- Chọn khoa --";
                cboKhoa.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
                cboKhoa.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                cboKhoa.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;

                cboKhoa.Properties.PopulateColumns();
                foreach (DevExpress.XtraEditors.Controls.LookUpColumnInfo col in cboKhoa.Properties.Columns)
                {
                    col.Visible = col.FieldName == nameof(DepartmentLookupItem.Code) || col.FieldName == nameof(DepartmentLookupItem.Name);
                }

                cboKhoa.Properties.Columns[nameof(DepartmentLookupItem.Code)].Caption = "Mã khoa";
                cboKhoa.Properties.Columns[nameof(DepartmentLookupItem.Code)].Width = 90;
                cboKhoa.Properties.Columns[nameof(DepartmentLookupItem.Name)].Caption = "Tên khoa";
                cboKhoa.Properties.Columns[nameof(DepartmentLookupItem.Name)].Width = 220;

                // Ép kích thước popup cố định, tránh trường hợp tự tính ra rộng/cao = 0 khiến dropdown trông như trống
                cboKhoa.Properties.PopupWidth = 320;
                cboKhoa.Properties.DropDownRows = 12;
                cboKhoa.Properties.PopupSizeable = true;

                cboKhoa.Properties.NullText = departmentItems.Count == 0
                    ? "-- Không có khoa nào --"
                    : "-- Chọn khoa --";
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không tải được danh sách khoa");
                cboKhoa.Properties.NullText = "-- Không tải được danh sách khoa --";
                lblKhoaError.Text = "Lỗi tải khoa: " + ex.Message;
                lblKhoaError.Visible = true;
            }
            finally
            {
                cboKhoa.Enabled = true;
                btnReloadKhoa.Enabled = true;
            }
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            ServiceLocator.SelectedOrganizationCode = SelectedOrganizationCode;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}

