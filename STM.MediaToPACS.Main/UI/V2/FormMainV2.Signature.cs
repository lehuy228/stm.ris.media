using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Pdf;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using MediaToPacs.Core.Enums;
using MediaToPacs.Core.Models;
using MediaToPacs.Core.Models.Ketluan;
using STM.MediaToPACS.Main.Utilities;
using Serilog;

namespace STM.MediaToPACS.Main.UI.V2
{
    /// <summary>Ký số kết luận - chuyển thể nguyên vẹn từ FrmMain.Signature.cs (không liên quan DICOM).</summary>
    public partial class FormMainV2
    {
        private async void _btnSignature_Click(object sender, EventArgs e)
        {
            await SignaturePdfAsync();
        }

        private async Task SignaturePdfAsync()
        {
            _btnSignature.Enabled = false;
            var parentForm = this.FindForm();
            bool isSplashVisible = false;

            try
            {
                if (!ValidateSignatureData())
                    return;

                ShowSplashScreen(parentForm, "Đang tải dữ liệu...", "Vui lòng chờ trong giây lát...");
                isSplashVisible = true;

                if (!IsAuthorizedToSign())
                {
                    CloseSplashScreenOnce(ref isSplashVisible);
                    await HandleUnauthorizedUserAsync();
                    return;
                }

                if (_kqChanDoanResponse.TrangThai == TrangThaiKetLuan.NHAP)
                {
                    CloseSplashScreenOnce(ref isSplashVisible);
                    await HandleSignDraftAsync();
                }
                else
                {
                    CloseSplashScreenOnce(ref isSplashVisible);
                    await HandleAlreadySignedAsync(parentForm);
                }
            }
            catch (Exception ex)
            {
                CloseSplashScreenOnce(ref isSplashVisible);
                Log.Error(ex, "Lỗi khi ký số");
                ShowErrorMessage("Lỗi", $"Lỗi khi ký số: {ex.Message}");
            }
            finally
            {
                CloseSplashScreenOnce(ref isSplashVisible);
                _btnSignature.Enabled = true;
            }
        }

        private bool ValidateSignatureData()
        {
            if (_kqChanDoanResponse == null)
            {
                ShowWarningMessage("Thông báo", "Không có dữ liệu kết quả để ký.");
                return false;
            }
            return true;
        }

        private bool IsAuthorizedToSign()
        {
            string hisCode = ServiceLocator.KeycloakUserInfo?.HISCode;
            string maBacSi = _kqChanDoanResponse?.MaBacSiKetLuan;

            if (string.IsNullOrEmpty(hisCode) || string.IsNullOrEmpty(maBacSi))
                return false;

            return hisCode == maBacSi;
        }

        private async Task HandleSignDraftAsync()
        {
            bool splashShown = false;
            try
            {
                var parentForm = this.FindForm();
                string templateId = (_cbbLayout.SelectedItem as ReportTemplateGridViewModel)?.Id;
                if (string.IsNullOrEmpty(templateId))
                {
                    XtraMessageBox.Show(this, "Vui lòng chọn mẫu báo cáo trước khi ký số.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ShowSplashScreen(parentForm, "Đang tải dữ liệu...", "Vui lòng chờ trong giây lát...");
                splashShown = true;

                var template = await ServiceLocator.RisService.GetReportTemplateByIdAsync(templateId);
                var reportData = CreateBaoCaoKetLuan();

                byte[] pdfBytes = await ExportReportToPdfBytesAsync(template.XmlTemplate, reportData);

                var request = BuildSignRequest(pdfBytes);
                var signedResult = await ServiceLocator.SignatureService.SignHashPdfV2(request);

                if (!string.IsNullOrEmpty(signedResult))
                {
                    byte[] signedPdfBytes = Convert.FromBase64String(signedResult);
                    await ServiceLocator.RisService.UploadSignedFileAsync(_machidinh, "pdf", "", signedPdfBytes);

                    _rtKetLuan.Enabled = false;
                    _rtKhuyenNghi.Enabled = false;
                    _rtMoTa.Enabled = false;

                    _btnPrint.Enabled = true;
                    _btnPreviewMain.Enabled = true;
                    _btnSave.Enabled = false;
                    _btnSignature.Text = $"Hủy Ký số({ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.Sign})";
                    _kqChanDoanResponse.TrangThai = "Hoàn thành";

                    CloseSplashScreenOnce(ref splashShown);
                    XtraMessageBox.Show(this, "Báo cáo đã được ký số thành công và lưu vào hệ thống.",
                        "Ký số thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    CloseSplashScreenOnce(ref splashShown);
                    XtraMessageBox.Show(this, "Quá trình ký số không thành công. Vui lòng thử lại.",
                        "Ký số thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (OperationCanceledException ex)
            {
                CloseSplashScreenOnce(ref splashShown);
                Log.Warning(ex, "Timeout khi gọi API ký số");
                XtraMessageBox.Show(this, "Yêu cầu ký số thất bại. Vui lòng vào MySign để xác nhận chữ ký.",
                    "Ký số đang xử lý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                CloseSplashScreenOnce(ref splashShown);
                XtraMessageBox.Show(this, $"Đã xảy ra lỗi trong quá trình ký số: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseSplashScreenOnce(ref splashShown);
            }
        }

        public async Task<byte[]> ExportReportToPdfBytesAsync(string repxContent, object dataSource)
        {
            XtraReport report = null;
            try
            {
                if (!string.IsNullOrEmpty(repxContent))
                {
                    using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(repxContent)))
                        report = XtraReport.FromStream(ms, true);
                }
                else
                {
                    report = new XtraReport();
                }

                report.DataSource = new[] { dataSource };

                using (var ms = new MemoryStream())
                {
                    report.ExportToPdf(ms);
                    return ms.ToArray();
                }
            }
            finally
            {
                report?.Dispose();
            }
        }

        private SignhashRequestV2 BuildSignRequest(byte[] pdfBytes)
        {
            var request = new SignhashRequestV2
            {
                FileName = BuildSignFileName(),
                FileBase64 = Convert.ToBase64String(pdfBytes),
                UserID = ServiceLocator.KeycloakUserInfo.CCCD,
                OrderItemCode = _machidinh
            };

            using (PdfDocumentProcessor processor = new PdfDocumentProcessor())
            using (var ms = new MemoryStream(pdfBytes))
            {
                processor.LoadDocument(ms);
                var searchResults = processor.FindText("[Sig]");

                if (searchResults != null && searchResults.Rectangles.Count > 0)
                {
                    var rect = searchResults.Rectangles[0];
                    float imageWidth = 80;
                    float imageHeight = 40;

                    request.numberPageSign = searchResults.PageIndex + 1;
                    request.coorX = (float)rect.Left - imageWidth / 4;
                    request.coorY = (float)rect.Top - imageHeight / 2;
                    request.width = imageWidth;
                    request.height = imageHeight;
                }
            }

            return request;
        }

        private string BuildSignFileName()
        {
            string tenBenhNhan = RemoveDiacriticsForFileName(_chiDinhDichVuResponse?.BenhNhan?.HoTen);
            string maChiDinh = _machidinh ?? string.Empty;

            return string.IsNullOrEmpty(tenBenhNhan)
                ? $"{maChiDinh}.pdf"
                : $"{tenBenhNhan}_{maChiDinh}.pdf";
        }

        private static string RemoveDiacriticsForFileName(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            string normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char c in normalized)
            {
                if (c == 'đ') { sb.Append('d'); continue; }
                if (c == 'Đ') { sb.Append('D'); continue; }

                var category = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (category != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            string result = sb.ToString().Normalize(NormalizationForm.FormC).Trim();
            result = System.Text.RegularExpressions.Regex.Replace(result, @"\s+", "_");
            return result;
        }

        private async Task HandleAlreadySignedAsync(Form parentForm)
        {
            var dialogResult = XtraMessageBox.Show(this,
                "Kết quả đã được ký số. Bạn có muốn hủy ký số để tạo lại không?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult != DialogResult.Yes) return;

            string lyDoHuy = XtraInputBox.Show("Nhập lý do hủy ký số:", "Lý do hủy", "");
            if (string.IsNullOrWhiteSpace(lyDoHuy))
            {
                XtraMessageBox.Show(this, "Bạn phải nhập lý do hủy ký số!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool huyOk = await ServiceLocator.RisService.HuyKetQuaChanDoanAsync(_machidinh, lyDoHuy);

            if (!huyOk)
            {
                XtraMessageBox.Show(this, "Hủy ký số thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _rtKetLuan.Enabled = true;
            _rtKhuyenNghi.Enabled = true;
            _rtMoTa.Enabled = true;
            _btnSave.Enabled = true;
            _btnPrint.Enabled = true;
            _btnSignature.Text = $"Ký số ({ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.Sign})";
            _kqChanDoanResponse.TrangThai = TrangThaiKetLuan.NHAP;
        }

        private async Task HandleUnauthorizedUserAsync()
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
            if (_kqChanDoanResponse.TrangThai != TrangThaiKetLuan.NHAP)
            {
                var msg = "Bạn không có quyền ký hoặc hủy kết quả này.\n" +
                          "Vui lòng đăng nhập bằng tài khoản bác sĩ đã kết luận để thực hiện.";
                XtraMessageBox.Show(this, msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                var msg = "Bạn không phải là bác sĩ kết luận ban đầu.\n" +
                          "Hệ thống sẽ lưu kết quả dưới dạng NHÁP với thông tin bác sĩ hiện tại.";
                XtraMessageBox.Show(this, msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            await Task.CompletedTask;
        }

        private ThongTinKetLuanBaoCaoThuong CreateBaoCaoKetLuan()
        {
            var ketluan = new ThongTinKetLuanBaoCaoThuong
            {
                TGBacSiChiDinh = FormatDateTime(_chiDinhDichVuResponse?.Thoigianthuchien ?? DateTime.Now),
                TGBacSiKetLuan = FormatDateTime(_dateTGKetThuc?.DateTime ?? DateTime.Now),
                MoTa = StripGeneratedParamText(_kqChanDoanResponse?.Kqcls_MoTa),
                KetLuan = _kqChanDoanResponse?.Kqcls_KetLuan,
                KhuyenNghi = _kqChanDoanResponse?.Kqcls_DeNghi,
                ChiDinhDichVu = _chiDinhDichVuResponse,
                TenBacSiKetLuan = GetBacSiKetLuanName()
            };

            if (_paramFormControl != null && _paramFormControl.Visible)
            {
                ketluan.DanhSachChiSo = _paramFormControl.GetReportItems();

                var paramValues = _paramFormControl.GetParamValues();

                ketluan.ChiSoTheoMa = paramValues.ToDictionary(
                    p => p.paramCode,
                    p => p.@checked == false
                        ? string.Empty
                        : (!string.IsNullOrEmpty(p.value)
                            ? p.value
                            : (p.@checked == true ? (p.displayLabel ?? "x") : string.Empty)));

                ketluan.ChiSoCheckTheoMa = paramValues
                    .Where(p => p.@checked.HasValue)
                    .ToDictionary(p => p.paramCode, p => p.@checked.Value);
            }

            var selectedImagePaths = GetSelectedImagePaths();
            AssignImagesToKetLuan(ketluan, selectedImagePaths);
            ketluan.AnhChuKy = GetAnhChuKy();
            ketluan.GioiTinhConvert = ConvertGioiTinh(ketluan.ChiDinhDichVu?.BenhNhan?.GioiTinh);

            return ketluan;
        }

        private string GetBacSiKetLuanName()
        {
            if (ServiceLocator.KeycloakUserInfo == null)
                return string.Empty;

            var firstName = ServiceLocator.KeycloakUserInfo.FirstName ?? string.Empty;
            var lastName = ServiceLocator.KeycloakUserInfo.LastName ?? string.Empty;
            return $"{firstName} {lastName}".Trim();
        }

        /// <summary>Lấy đường dẫn ảnh đã chọn - dùng ImageThumbnailList thay cho _lstBoxPages (Leadtools).</summary>
        private List<string> GetSelectedImagePaths()
        {
            return _thumbnailList.GetCheckedFilePaths();
        }

        private void AssignImagesToKetLuan(ThongTinKetLuanBaoCaoThuong ketluan, List<string> imagePaths)
        {
            if (imagePaths == null || imagePaths.Count == 0)
                return;

            var imageProperties = new[]
            {
                new { Index = 0, Property = new Action<string>(path => ketluan.Image1 = path) },
                new { Index = 1, Property = new Action<string>(path => ketluan.Image2 = path) },
                new { Index = 2, Property = new Action<string>(path => ketluan.Image3 = path) },
                new { Index = 3, Property = new Action<string>(path => ketluan.Image4 = path) }
            };

            int maxImages = Math.Min(imagePaths.Count, imageProperties.Length);
            for (int i = 0; i < maxImages; i++)
            {
                try
                {
                    string base64Image = ConvertImageToBase64(imagePaths[i]);
                    if (!string.IsNullOrEmpty(base64Image))
                        imageProperties[i].Property(base64Image);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Lỗi khi chuyển đổi ảnh sang Base64: {ImagePath}", imagePaths[i]);
                }
            }
        }

        private string ConvertImageToBase64(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
            {
                Log.Warning("File ảnh không tồn tại: {ImagePath}", imagePath);
                return null;
            }

            try
            {
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                return Convert.ToBase64String(imageBytes);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi đọc file ảnh: {ImagePath}", imagePath);
                return null;
            }
        }

        private string GetAnhChuKy()
        {
            return _hisUserKySoResponse != null && !string.IsNullOrEmpty(_hisUserKySoResponse.AnhChuKy)
                ? _hisUserKySoResponse.AnhChuKy
                : null;
        }

        private string ConvertGioiTinh(int? gioiTinh)
        {
            return gioiTinh == 1 ? "Nữ" : gioiTinh == 0 ? "Nam" : "";
        }

        private string FormatDateTime(DateTime dateTime)
        {
            return $"{dateTime.Hour} giờ {dateTime.Minute} phút, " +
                   $"ngày {dateTime.Day} tháng {dateTime.Month} năm {dateTime.Year}";
        }
    }
}
