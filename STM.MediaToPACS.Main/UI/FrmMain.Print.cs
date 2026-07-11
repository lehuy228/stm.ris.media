using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;
using Leadtools;
using Leadtools.Demos;
using Leadtools.Forms.DocumentWriters;
using Leadtools.Codecs;
using Leadtools.Dicom;
using System.Net;
using System.Threading;
using Leadtools.Dicom.Common.Extensions;
using Leadtools.Dicom.Common.Editing;
using Leadtools.Dicom.Scu.Common;
using Leadtools.Dicom.Scu;
using System.Diagnostics;
using Leadtools.Dicom.Common.DataTypes.Modality;
using STM.MediaToPACS.Main.UI;
using Leadtools.DicomDemos;
using System.Collections.Generic;
using System.Collections;
using System.Management;
using Leadtools.WinForms.CommonDialogs.File;
using System.Reflection;
using Leadtools.Dicom.Common.Editing.Converters;
using Leadtools.ImageProcessing;
using Leadtools.Drawing;
using Leadtools.ImageProcessing.Effects;
using STM.MediaToPACS.Main.UI.CameraUI;
using Leadtools.Medical.Worklist.DataAccessLayer;
using Leadtools.Medical.DataAccessLayer;
using Leadtools.Medical.DataAccessLayer.Configuration;
using Leadtools.Medical.Worklist.DataAccessLayer.Configuration;
using Leadtools.Medical.Winforms;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VisioForge.Core.VideoEdit;
using MediaToPacs.Core.Models;
using STM.MediaToPACS.Main.Utilities;
using DevExpress.XtraPdfViewer;
using System.Drawing.Printing;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraEditors;
using DevExpress.Pdf;
using MediaToPacs.Core.Models.Ketluan;
using DevExpress.XtraReports.UI;
using System.Text;
using MediaToPacs.Core.Enums;
using System.Xml.Serialization;
using Serilog;
using System.Configuration;
using System.Runtime.InteropServices;
using STM.MediaToPACS.Main.UI.Configurations;

namespace STM.MediaToPACS.Main
{
    public partial class FrmMain
    {
        #region Print/Preview Operations

        /// <summary>
        /// Xử lý in kết quả
        /// </summary>
        private void _btnPrint_Click(object sender, EventArgs e)
        {
            // Fire and forget - không cần await
            _ = PrintCurrentAsync();
        }

        /// <summary>
        /// In kết quả hiện tại
        /// </summary>
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

                // In PDF
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

        /// <summary>
        /// In PDF
        /// </summary>
        private async Task PrintPdfAsync(byte[] pdfBytes)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var ms = new MemoryStream(pdfBytes))
                    using (var viewer = new PdfViewer())
                    {
                        viewer.LoadDocument(ms);

                        var settings = new PrinterSettings
                        {
                            PrinterName = _cbbPrinters.Text
                        };

                        viewer.Print(settings);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Lỗi khi in PDF");
                    throw;
                }
            });
        }

        /// <summary>
        /// Lấy PDF bytes để in (từ API hoặc generate)
        /// </summary>
        private async Task<byte[]> GetPdfBytesForPrintAsync()
        {
            // Nếu đã hoàn thành -> tải từ API
            if (_kqChanDoanResponse.TrangThai == TrangThaiKetLuan.HOAN_THANH)
            {
                return await ServiceLocator.RisService.TaiFileKetQuaChanDoanAsync(_machidinh);
            }

            // Nếu chưa hoàn thành -> generate từ template
            return await GeneratePdfFromTemplateAsync();
        }

        /// <summary>
        /// Validate trước khi in
        /// </summary>
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

        /// <summary>
        /// Xử lý preview PDF
        /// </summary>
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

                // Hiển thị PDF viewer
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

        /// <summary>
        /// Validate trước khi preview
        /// </summary>
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

        /// <summary>
        /// Lấy PDF bytes để preview
        /// </summary>
        private async Task<byte[]> GetPdfBytesForPreviewAsync()
        {
            // Nếu đã hoàn thành -> tải từ API và lưu file
            if (_kqChanDoanResponse.TrangThai == TrangThaiKetLuan.HOAN_THANH)
            {
                var pdfBytes = await ServiceLocator.RisService.TaiFileKetQuaChanDoanAsync(_machidinh);

                if (pdfBytes != null && pdfBytes.Length > 0)
                {
                    await SaveSignedPdfToFileAsync(pdfBytes);
                }

                return pdfBytes;
            }

            // Nếu chưa hoàn thành -> generate từ template
            return await GeneratePdfFromTemplateAsync();
        }

        /// <summary>
        /// Lưu file PDF đã ký vào thư mục
        /// </summary>
        private async Task SaveSignedPdfToFileAsync(byte[] pdfBytes)
        {
            await Task.Run(() =>
            {
                try
                {
                    string saveFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "BenhNhan",
                        _machidinh,
                        "KQChanDoan"
                    );

                    if (!Directory.Exists(saveFolder))
                    {
                        Directory.CreateDirectory(saveFolder);
                    }

                    string filePath = Path.Combine(saveFolder, "KetquaSigned.pdf");
                    File.WriteAllBytes(filePath, pdfBytes);
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Không thể lưu file PDF đã ký");
                }
            });
        }

        /// <summary>
        /// Hiển thị PDF viewer
        /// </summary>
        private Task ShowPdfViewerAsync(byte[] pdfBytes)
        {
            var pdfViewer = new FormPdfViewer(pdfBytes);
            pdfViewer.ShowDialog();
            pdfViewer.Dispose();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Generate PDF từ template
        /// </summary>
        private async Task<byte[]> GeneratePdfFromTemplateAsync()
        {
            var templateId = GetSelectedTemplateId();
            if (string.IsNullOrEmpty(templateId))
            {
                throw new InvalidOperationException("Chưa chọn layout báo cáo.");
            }

            var template = await ServiceLocator.RisService.GetReportTemplateByIdAsync(templateId);
            if (template == null)
            {
                throw new InvalidOperationException("Không tìm thấy template báo cáo.");
            }

            var reportData = CreateBaoCaoKetLuan();
            return await ExportReportToPdfBytesAsync(template.XmlTemplate, reportData);
        }

        /// <summary>
        /// Lấy template ID đã chọn
        /// </summary>
        private string GetSelectedTemplateId()
        {
            return (_cbbLayout.SelectedItem as ReportTemplateGridViewModel)?.Id;
        }

        #endregion
    }
}
