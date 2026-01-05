using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraSplashScreen;
using MediaToPacs.Core.Models.Ketluan;
using Serilog;
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
    public partial class ReportTemplateManager : UserControl
    {
        public ReportTemplateManager()
        {
            InitializeComponent();
        }

        private async void ReportTemplateManager_Load(object sender, EventArgs e)
        {
            LoadTemplateReport();
            LoadDSDV();
        }

        private async void LoadTemplateReport()
        {
            var parentForm = this.FindForm();
            try
            {
                SplashScreenManager.ShowForm(parentForm, typeof(WaitFormLoading), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu...");
                SplashScreenManager.Default.SetWaitFormDescription("Vui lòng chờ trong giây lát...");

                gridControlReportTemplate.DataSource = null;

                var reports = await ServiceLocator.RisService.GetReportTemplateAsync();

                gridControlReportTemplate.DataSource = reports.data;
                gridControlReportTemplate.RefreshDataSource();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show(this, $"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void _btnSearchTempalate_Click(object sender, EventArgs e)
        {
            LoadTemplateReport();
        }

        private void _btnAddNew_Click(object sender, EventArgs e)
        {

            try
            {
                SplashScreenManager.ShowDefaultWaitForm("Đang tải", "Vui lòng chờ...");
                TemplateSetting templateSetting = new TemplateSetting();

                templateSetting.Show();
            }
            finally
            {
                if (SplashScreenManager.Default != null)
                    SplashScreenManager.CloseDefaultWaitForm();
            }
        }

        private void gridViewReportTemplate_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                Point pt = view.GridControl.PointToClient(Control.MousePosition);
                GridHitInfo info = view.CalcHitInfo(pt);

                if (info.InRowCell && info.Column == gridColumnActionLayout)
                    return;

                string id = GetSelectedRowLayoutId(view, pt);

                if (string.IsNullOrEmpty(id))
                {
                    XtraMessageBox.Show(this, "Không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                OpenTemplateSetupForFocusedRowAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }


        private async void OpenTemplateSetupForFocusedRowAsync()
        {
            var id = gridViewReportTemplate.GetFocusedRowCellValue("Id")?.ToString();
            if (string.IsNullOrEmpty(id))
            {
                XtraMessageBox.Show(this, "Không tồn tại mẫu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                SplashScreenManager.ShowDefaultWaitForm("Đang tải", "Vui lòng chờ...");
                TemplateSetting templateSetting = new TemplateSetting(id);

                templateSetting.Show();
            }
            finally
            {
                if (SplashScreenManager.Default != null)
                    SplashScreenManager.CloseDefaultWaitForm();
            }
        }

        private void OpenSuggestionForm(string id = null)
        {
            try
            {
                SplashScreenManager.ShowDefaultWaitForm("Đang tải", "Vui lòng chờ...");

                var dichvu = GetSelectedDichVu();
                if (dichvu == null)
                {
                    XtraMessageBox.Show("Chưa có dịch vụ nào được chọn không thể tạo gợi ý");
                    return;
                }

                SuggestionForm suggestionForm = string.IsNullOrEmpty(id)
                    ? new SuggestionForm(dichVu: dichvu)
                    : new SuggestionForm(id, dichvu);
                suggestionForm.ShowDialog();
            }
            finally
            {
                if (SplashScreenManager.Default != null)
                    SplashScreenManager.CloseDefaultWaitForm();
            }
        }

        private string GetSelectedRowId(GridView view, Point pt)
        {
            GridHitInfo info = view.CalcHitInfo(pt);
            if (info.InRowCell && info.RowHandle >= 0)
            {
                return view.GetRowCellValue(info.RowHandle, "id")?.ToString();
            }
            return null;
        }


        private async void gridViewReportTemplate_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                if (e.Column == gridColumnActionLayout)
                {
                    var view = sender as GridView;
                    var id = view.GetRowCellValue(e.RowHandle, "Id")?.ToString();

                    if (!string.IsNullOrEmpty(id))
                    {
                        var result = XtraMessageBox.Show(
                            $"Bạn có chắc chắn muốn xoá bản ghi không?",
                            "Xác nhận xoá",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            try
                            {
                                var success = await ServiceLocator.RisService.XoaReportTemplateAsync(id);

                                if (success)
                                {
                                    XtraMessageBox.Show("Đã xoá thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    view.DeleteRow(e.RowHandle);
                                }
                            }
                            catch (Exception ex)
                            {
                                XtraMessageBox.Show($"Xoá thất bại: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private string GetSelectedRowLayoutId(GridView view, Point pt)
        {
            GridHitInfo info = view.CalcHitInfo(pt);
            if (info.InRowCell && info.RowHandle >= 0)
            {
                return view.GetRowCellValue(info.RowHandle, "Id")?.ToString();
            }
            return null;
        }

        private void _gridViewGoiYKL_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                Point pt = view.GridControl.PointToClient(Control.MousePosition);
                GridHitInfo info = view.CalcHitInfo(pt);

                if (info.InRowCell && info.Column == gridColumnActionGoiY)
                    return;

                string id = GetSelectedRowId(view, pt);

                if (string.IsNullOrEmpty(id))
                {
                    XtraMessageBox.Show(this, "Không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                OpenSuggestionForm(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }


        private void _btnCreateGoiY_Click(object sender, EventArgs e)
        {
            OpenSuggestionForm();
        }

        private async void _gridViewGoiYKL_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                if (e.Column == gridColumnActionGoiY)
                {
                    var view = sender as GridView;
                    var id = view.GetRowCellValue(e.RowHandle, "id")?.ToString();

                    if (!string.IsNullOrEmpty(id))
                    {
                        var result = XtraMessageBox.Show(
                            $"Bạn có chắc chắn muốn xoá bản ghi không?",
                            "Xác nhận xoá",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            try
                            {
                                var success = await ServiceLocator.RisService.XoaGoiYKetLuanAsync(id);

                                if (success)
                                {
                                    XtraMessageBox.Show("Đã xoá thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    view.DeleteRow(e.RowHandle);
                                }
                            }
                            catch (Exception ex)
                            {
                                XtraMessageBox.Show($"Xoá thất bại: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        private async void LoadDSDV()
        {
            var parentForm = this.FindForm();
            try
            {
                SplashScreenManager.ShowForm(parentForm, typeof(WaitFormLoading), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu...");
                SplashScreenManager.Default.SetWaitFormDescription("Vui lòng chờ trong giây lát...");

                gridControlDSDV.DataSource = null;
                var tenDichVu = _txtTenDV.Text;
                var maDichVu = _txMaDV.Text;
                var modality = _cbbModality.Text;
                var dsDichVu = await ServiceLocator.RisService.GetDanhSachDichVuAsync(tenDichVu: tenDichVu, maDichVu: maDichVu, modality: modality, pageSize: int.Parse(_cbbPagesizeDV.Text));
                gridControlDSDV.DataSource = dsDichVu;
                gridControlDSDV.RefreshDataSource();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show(this, $"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private async void _btnSearchDV_Click(object sender, EventArgs e)
        {
            LoadDSDV();

        }

        private void _gridViewDSDV_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            var view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view == null) return;

            var obj = GetSelectedDichVu();
            if (obj == null) return;
            LoadDanhSachGoiY(obj);
        }

        private async void LoadDanhSachGoiY(DanhMucDichVuGridView obj)
        {
            var parentForm = this.FindForm();
            try
            {
                SplashScreenManager.ShowForm(parentForm, typeof(WaitFormLoading), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu...");
                SplashScreenManager.Default.SetWaitFormDescription("Vui lòng chờ trong giây lát...");

                gridControlGoiYKL.DataSource = null;
                var dsDichVu = await ServiceLocator.RisService.GetDanhSachGoiYKetLuanAsync(madichvu: obj.madichvu);
                gridControlGoiYKL.DataSource = dsDichVu.data;
                gridControlGoiYKL.RefreshDataSource();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show(this, $"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private DanhMucDichVuGridView GetSelectedDichVu()
        {
            if (_gridViewDSDV == null) return null;

            int rowHandle = _gridViewDSDV.FocusedRowHandle;
            if (rowHandle < 0) return null;

            return _gridViewDSDV.GetRow(rowHandle) as DanhMucDichVuGridView;
        }
    }
}
