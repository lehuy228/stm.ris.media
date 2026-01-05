using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using MediaToPacs.Core.Models;
using MediaToPacs.Core.Models.Ketluan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STM.MediaToPACS.Main.Utilities
{
    public partial class TemplateSetting : XtraForm
    {
        private readonly string _templateId;
        private readonly object _sampleReportData;
        private ReportTemplateResponse _currentTemplate;

        public TemplateSetting(string templateId = null, object sampleReportData = null)
        {
            InitializeComponent();
            _templateId = templateId;
            _sampleReportData = sampleReportData;
        }

        #region Sample Data

        private ThongTinKetLuanBaoCaoThuong CreateSampleBaoCaoThuong()
        {
            return new ThongTinKetLuanBaoCaoThuong
            {
                TGBacSiChiDinh = "00 giờ 00 phút, Ngày ... Tháng ... Năm ...",
                TGBacSiKetLuan = "00 giờ 00 phút, Ngày ... Tháng ... Năm ...",
                MoTa = "Mô tả mẫu (dùng cho test hiển thị)...",
                KetLuan = "Kết luận mẫu",
                KhuyenNghi = "Khuyến nghị mẫu",
                ChiDinhDichVu = CreateSampleChiDinh(),
                GioiTinhConvert = "Nam",
                TenBacSiKetLuan = "Nguyễn Văn A",
                AnhChuKy = "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",
            };
        }

        private ChiDinhDichVuResponse CreateSampleChiDinh()
        {
            return new ChiDinhDichVuResponse
            {
                Sovaovien = "SV001",
                MaChiDinh = "CD12345",
                SoPhieuChiDinh = "P20230831001",
                MaDichVu = "DV001",
                TenDichVu = "Chụp X-quang Ngực Thẳng",
                //SoLuong = 1,
                Modality = "CR",
                MaBacSiChiDinh = "BS001",
                TenBacSiChiDinh = "Nguyễn Văn B",
                Thoigianthuchien = DateTime.Now,
                MaNoiChiDinh = "K01",
                ChanDoanSoBo = "Chẩn đoán sơ bộ",
                KhoaDieuTri = "Khoa điều trị",
                Phong = "Phòng",
                TenNoiChiDinh = "Khoa Chẩn Đoán Hình Ảnh",
                TrangThai = "Đã thực hiện",
                UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                Id = Guid.NewGuid().ToString(),
                admissionType = "Thường",
                BenhNhan = new BenhNhan
                {
                    MaBenhNhan = "BN0001",
                    HoTen = "Nguyễn Văn A",
                    NgaySinh = new DateTime(1985, 5, 20),
                    GioiTinh = 1, // Nam
                    TuNgayBHYT = new DateTime(2023, 1, 1),
                    DenNgayBHYT = new DateTime(2023, 12, 31),
                    MaBHYT = "HN1234567890",
                    XaPhuong = "Phường 1",
                    TinhThanh = "Hà Nội",
                    DanToc = "Kinh"
                }
            };
        }

        #endregion

        //private async void TemplateSetting_Load(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(_templateId))
        //        {

        //            _currentTemplate = await ServiceLocator.RisService.GetReportTemplateByIdAsync(_templateId);
        //        }
        //        // Nếu chưa có data truyền vào thì dùng sample
        //        var reportData = _sampleReportData ?? CreateSampleBaoCaoThuong();
        //        var dataSource = new[] { reportData };

        //        // Load report
        //        XtraReport report = null;
        //        if (_currentTemplate != null)
        //        {
        //            if (!string.IsNullOrEmpty(_currentTemplate.XmlTemplate))
        //            {
        //                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(_currentTemplate.XmlTemplate)))
        //                {
        //                    report = XtraReport.FromStream(ms, true);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            report = new XtraReport();
        //        }

        //        report.DataSource = dataSource;
        //        reportDesigner1.OpenReport(report);
        //    }
        //    catch (Exception ex)
        //    {
        //        XtraMessageBox.Show($"Lỗi khi load template: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private async void TemplateSetting_Load(object sender, EventArgs e)
        {
            try
            {
                XtraReport report = await LoadReportAsync();
                reportDesigner1.OpenReport(report);
            }
            catch (IOException ioEx)
            {
                XtraMessageBox.Show($"Lỗi đọc/ghi file template: {ioEx.Message}", "Lỗi I/O", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (UnauthorizedAccessException uaEx)
            {
                XtraMessageBox.Show($"Không có quyền truy cập template: {uaEx.Message}", "Lỗi Quyền", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi khi load template: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<XtraReport> LoadReportAsync()
        {
            if (!string.IsNullOrEmpty(_templateId))
                _currentTemplate = await ServiceLocator.RisService.GetReportTemplateByIdAsync(_templateId);

            var reportData = _sampleReportData ?? CreateSampleBaoCaoThuong();
            var dataSource = new[] { reportData };

            XtraReport report;

            if (_currentTemplate?.XmlTemplate != null)
            {
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(_currentTemplate.XmlTemplate));
                report = XtraReport.FromStream(ms, true);
            }
            else
            {
                report = new XtraReport();
            }

            report.DataSource = dataSource;

            report.DisplayName = !string.IsNullOrEmpty(_currentTemplate?.Name)
                ? _currentTemplate.Name
                : "Báo cáo mới";

            return report;
        }

        private async void _barBtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (reportDesigner1.ActiveDesignPanel == null)
            {
                XtraMessageBox.Show("Không tìm thấy báo cáo đang mở!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var report = reportDesigner1.ActiveDesignPanel.Report;
                var templateInfo = _currentTemplate ?? new ReportTemplateResponse();

                var inputForm = new ReportTemplateSupport(templateInfo.Name, templateInfo.Modality);
                if (inputForm.ShowDialog() != DialogResult.OK) return;

                string xmlTemplate;
                using (var ms = new MemoryStream())
                {
                    report.SaveLayoutToXml(ms);
                    xmlTemplate = Encoding.UTF8.GetString(ms.ToArray());
                }

                var request = new ReportTemplateRequest
                {
                    name = inputForm.ReportName,
                    modality = inputForm.Modality,
                    xmlTemplate = xmlTemplate
                };

                bool result;
                if (!string.IsNullOrEmpty(_templateId))
                {
                    result = await ServiceLocator.RisService.UpdateReportTemplateAsync(request, _templateId);
                    if (result) ShowInfo("Cập nhật mấu kết luận thành công!");
                }
                else
                {
                    result = await ServiceLocator.RisService.CreateReportTemplateAsync(request);
                    if (result) ShowInfo("Lưu mấu kết luận mới thành công!");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi lưu mấu kết luận: {ex.Message}");
            }
        }

        private void ShowInfo(string message) => XtraMessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        private void ShowError(string message) => XtraMessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
