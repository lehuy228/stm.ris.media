using System;
using System.Drawing.Printing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraPdfViewer;
using MediaToPacs.Core.Enums;
using MediaToPacs.Core.Models;
using MediaToPacs.Core.Models.Ketluan;
using STM.MediaToPACS.Main.Utilities;
using Serilog;

namespace STM.MediaToPACS.Main.UI.DiagnosticReports
{
    /// <summary>In/Xem trước kết luận - copy nguyên từ FrmMain.Print.cs (không liên quan DICOM).</summary>
    public partial class DiagnosticReportConclusionControl
    {
        private void _btnPrint_Click(object sender, EventArgs e)
        {
            _ = PrintCurrentAsync();
        }

        private async Task PrintCurrentAsync()
        {
            if (!ValidateBeforePrint())
                return;

            var parentForm = this.FindForm();
            bool isSplashVisible = false;

            try
            {
                ShowSplashScreen(parentForm, "Đang chuẩn bị in...", "Vui lòng chờ trong giây lát...");
                isSplashVisible = true;

                byte[] pdfBytes = await GetPdfBytesForPrintAsync();

                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    CloseSplashScreen(isSplashVisible);
                    ShowErrorMessage("Lỗi", "Không thể tạo file PDF để in.");
                    return;
                }

                CloseSplashScreen(isSplashVisible);
                isSplashVisible = false;

                await PrintPdfAsync(pdfBytes);
            }
            catch (Exception ex)
            {
                CloseSplashScreen(isSplashVisible);
                Log.Error(ex, "Lỗi khi in PDF");
                ShowErrorMessage("Lỗi khi in PDF", ex.Message);
            }
            finally
            {
                CloseSplashScreen(isSplashVisible);
            }
        }

        private async Task PrintPdfAsync(byte[] pdfBytes)
        {
            await Task.Run(() =>
            {
                using (var ms = new MemoryStream(pdfBytes))
                using (var viewer = new PdfViewer())
                {
                    viewer.LoadDocument(ms);
                    var settings = new PrinterSettings { PrinterName = _cbbPrinters.Text };
                    viewer.Print(settings);
                }
            });
        }

        private async Task<byte[]> GetPdfBytesForPrintAsync()
        {
            if (_kqChanDoanResponse.TrangThai == TrangThaiKetLuan.HOAN_THANH)
                return await ServiceLocator.RisService.TaiFileKetQuaChanDoanAsync(_machidinh);

            return await GeneratePdfFromTemplateAsync();
        }

        private bool ValidateBeforePrint()
        {
            if (string.IsNullOrWhiteSpace(_machidinh))
            {
                ShowWarningMessage("Thông báo", "Chưa chọn mã chỉ định!");
                return false;
            }
            if (_kqChanDoanResponse == null)
            {
                ShowWarningMessage("Thông báo", "Chưa có kết quả chẩn đoán để in!");
                return false;
            }
            if (string.IsNullOrWhiteSpace(_cbbPrinters.Text))
            {
                ShowWarningMessage("Thông báo", "Chưa chọn máy in!");
                return false;
            }
            return true;
        }

        private async void _btnPreviewMain_Click(object sender, EventArgs e)
        {
            if (!ValidateBeforePreview())
                return;

            var parentForm = this.FindForm();
            bool isSplashVisible = false;

            try
            {
                ShowSplashScreen(parentForm, "Đang tải dữ liệu...", "Vui lòng chờ trong giây lát...");
                isSplashVisible = true;

                byte[] pdfBytes = await GetPdfBytesForPreviewAsync();

                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    CloseSplashScreen(isSplashVisible);
                    ShowErrorMessage("Lỗi", "Không thể tải hoặc tạo file PDF.");
                    return;
                }

                CloseSplashScreen(isSplashVisible);
                isSplashVisible = false;

                await ShowPdfViewerAsync(pdfBytes);
            }
            catch (Exception ex)
            {
                CloseSplashScreen(isSplashVisible);
                Log.Error(ex, "Lỗi khi preview PDF");
                ShowErrorMessage("Lỗi", $"Lỗi khi mở PDF: {ex.Message}");
            }
            finally
            {
                CloseSplashScreen(isSplashVisible);
            }
        }

        private bool ValidateBeforePreview()
        {
            if (string.IsNullOrWhiteSpace(_machidinh))
            {
                ShowWarningMessage("Thông báo", "Chưa chọn mã chỉ định!");
                return false;
            }
            if (_kqChanDoanResponse == null)
            {
                ShowWarningMessage("Thông báo", "Chưa có kết quả chẩn đoán!");
                return false;
            }
            return true;
        }

        private async Task<byte[]> GetPdfBytesForPreviewAsync()
        {
            if (_kqChanDoanResponse.TrangThai == TrangThaiKetLuan.HOAN_THANH)
            {
                var pdfBytes = await ServiceLocator.RisService.TaiFileKetQuaChanDoanAsync(_machidinh);
                if (pdfBytes != null && pdfBytes.Length > 0)
                    await SaveSignedPdfToFileAsync(pdfBytes);
                return pdfBytes;
            }

            return await GeneratePdfFromTemplateAsync();
        }

        private async Task SaveSignedPdfToFileAsync(byte[] pdfBytes)
        {
            await Task.Run(() =>
            {
                try
                {
                    string saveFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BenhNhan", _machidinh, "KQChanDoan");
                    if (!Directory.Exists(saveFolder))
                        Directory.CreateDirectory(saveFolder);

                    string filePath = Path.Combine(saveFolder, "KetquaSigned.pdf");
                    File.WriteAllBytes(filePath, pdfBytes);
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Không thể lưu file PDF đã ký");
                }
            });
        }

        private Task ShowPdfViewerAsync(byte[] pdfBytes)
        {
            var pdfViewer = new FormPdfViewer(pdfBytes);
            pdfViewer.ShowDialog();
            pdfViewer.Dispose();
            return Task.CompletedTask;
        }

        private async Task<byte[]> GeneratePdfFromTemplateAsync()
        {
            var templateId = GetSelectedTemplateId();
            if (string.IsNullOrEmpty(templateId))
                throw new InvalidOperationException("Chưa chọn layout báo cáo.");

            var template = await ServiceLocator.RisService.GetReportTemplateByIdAsync(templateId);
            if (template == null)
                throw new InvalidOperationException("Không tìm thấy template báo cáo.");

            var reportData = CreateBaoCaoKetLuan();
            return await ExportReportToPdfBytesAsync(template.XmlTemplate, reportData);
        }

        private string GetSelectedTemplateId()
        {
            return (_cbbLayout.SelectedItem as ReportTemplateGridViewModel)?.Id;
        }
    }
}
