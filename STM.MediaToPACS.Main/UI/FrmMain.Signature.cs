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
//using VisioForge.Core.VideoEdit; // VisioForge đã gỡ (thay bằng FlashCap)
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
        #region Signature Operations

        /// <summary>
        /// Xử lý sự kiện click nút Ký số
        /// </summary>
        private async void _btnSignature_Click(object sender, EventArgs e)
        {
            await SignaturePdfAsync();
        }

        /// <summary>
        /// Xử lý quy trình ký số PDF
        /// </summary>
        private async Task SignaturePdfAsync()
        {
            _btnSignature.Enabled = false;
            var parentForm = this.FindForm();
            bool isSplashVisible = false;

            try
            {
                // Validate dữ liệu
                if (!ValidateSignatureData())
                    return;

                ShowSplashScreen(parentForm, "Đang tải dữ liệu...", "Vui lòng chờ trong giây lát...");
                isSplashVisible = true;

                // Kiểm tra quyền ký số
                if (!IsAuthorizedToSign())
                {
                    CloseSplashScreenOnce(ref isSplashVisible);
                    await HandleUnauthorizedUserAsync();
                    return;
                }

                // Xử lý theo trạng thái
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

        /// <summary>
        /// Kiểm tra dữ liệu trước khi ký số
        /// </summary>
        private bool ValidateSignatureData()
        {
            if (_kqChanDoanResponse == null)
            {
                ShowWarningMessage("Thông báo", "Không có dữ liệu kết quả để ký.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Kiểm tra quyền ký số của user hiện tại
        /// </summary>
        private bool IsAuthorizedToSign()
        {
            string hisCode = ServiceLocator.KeycloakUserInfo?.HISCode;
            string maBacSi = _kqChanDoanResponse?.MaBacSiKetLuan;

            // Nếu một trong hai giá trị rỗng/null thì từ chối: tránh trường hợp
            // null == null vô tình cho phép ký nhầm người với dữ liệu cũ thiếu mã bác sĩ
            if (string.IsNullOrEmpty(hisCode) || string.IsNullOrEmpty(maBacSi))
                return false;

            return hisCode == maBacSi;
        }

        /// <summary>
        /// Xử lý trường hợp kết quả đang là Nháp -> Ký số
        /// </summary>
        private async Task HandleSignDraftAsync()
        {
            bool splashShown = false;
            try
            {
                var parentForm = this.FindForm();
                // Kiểm tra layout
                string templateId = (_cbbLayout.SelectedItem as ReportTemplateGridViewModel)?.Id;
                if (string.IsNullOrEmpty(templateId))
                {
                    XtraMessageBox.Show(
                        this,
                        "Vui lòng chọn mẫu báo cáo trước khi ký số.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                ShowSplashScreen(parentForm, "Đang tải dữ liệu...", "Vui lòng chờ trong giây lát...");
                splashShown = true;

                // Lấy template và dữ liệu báo cáo
                var template = await ServiceLocator.RisService.GetReportTemplateByIdAsync(templateId);
                var reportData = CreateBaoCaoKetLuan();

                // Xuất PDF
                byte[] pdfBytes = await ExportReportToPdfBytesAsync(template.XmlTemplate, reportData);

                // Tạo request ký số
                var request = BuildSignRequest(pdfBytes);
                var signedResult = await ServiceLocator.SignatureService.SignHashPdf(request);

                if (!string.IsNullOrEmpty(signedResult))
                {
                    byte[] signedPdfBytes = Convert.FromBase64String(signedResult);

                    // Upload file đã ký
                    await ServiceLocator.RisService.UploadSignedFileAsync(_machidinh, "pdf", "", signedPdfBytes);

                    // Cập nhật UI
                    _rtKetLuan.Enabled = false;
                    _rtKhuyenNghi.Enabled = false;
                    _rtMoTa.Enabled = false;

                    _btnPrint.Enabled = true;
                    _btnPreviewMain.Enabled = true;
                    _btnSave.Enabled = false;
                    _btnSignature.Text = $"Hủy Ký số({ ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.Sign})";
                    _kqChanDoanResponse.TrangThai = "Hoàn thành";
                    // Đóng splash TRƯỚC khi show dialog vì splash TopMost sẽ che dialog
                    CloseSplashScreenOnce(ref splashShown);
                    XtraMessageBox.Show(
                        this,
                        "Báo cáo đã được ký số thành công và lưu vào hệ thống.",
                        "Ký số thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    CloseSplashScreenOnce(ref splashShown);
                    XtraMessageBox.Show(
                        this,
                        "Quá trình ký số không thành công. Vui lòng thử lại.",
                        "Ký số thất bại",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
            catch (Exception ex)
            {
                CloseSplashScreenOnce(ref splashShown);
                XtraMessageBox.Show(
                    this,
                    $"Đã xảy ra lỗi trong quá trình ký số: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
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
                    {
                        report = XtraReport.FromStream(ms, true);
                    }
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
                // XtraReport là IDisposable - không dispose sẽ leak GDI handle khi Preview/Ký/In gọi lặp lại
                report?.Dispose();
            }
        }

        /// <summary>
        /// Build SignhashRequest từ pdfBytes
        /// </summary>
        private SignhashRequest BuildSignRequest(byte[] pdfBytes)
        {
            var request = new SignhashRequest
            {
                FileName = "Ketqua.pdf",
                FileBase64 = Convert.ToBase64String(pdfBytes),
                UserID = ServiceLocator.KeycloakUserInfo.CCCD
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

        /// <summary>
        /// Xử lý trường hợp đã ký số -> hỏi có muốn hủy ký không
        /// </summary>
        private async Task HandleAlreadySignedAsync(Form parentForm)
        {
            var dialogResult = XtraMessageBox.Show(
                this,
                "Kết quả đã được ký số. Bạn có muốn hủy ký số để tạo lại không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (dialogResult != DialogResult.Yes) return;

            // Nhập lý do hủy
            string lyDoHuy = XtraInputBox.Show("Nhập lý do hủy ký số:", "Lý do hủy", "");
            if (string.IsNullOrWhiteSpace(lyDoHuy))
            {
                XtraMessageBox.Show(this, "Bạn phải nhập lý do hủy ký số!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Gọi API hủy ký số
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


        /// <summary>
        /// Xử lý khi user không có quyền ký
        /// </summary>
        private async Task HandleUnauthorizedUserAsync()
        {

            SplashScreenManager.CloseForm(false);
            if (_kqChanDoanResponse.TrangThai != TrangThaiKetLuan.NHAP)
            {
                var msg = "Bạn không có quyền ký hoặc hủy kết quả này.\n" +
                          "Vui lòng đăng nhập bằng tài khoản bác sĩ đã kết luận để thực hiện.";
                XtraMessageBox.Show(this, msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                var msg = "Bạn không phải là bác sĩ kết luận ban đầu.\n" +
                          "Hệ thống sẽ lưu kết quả dưới dạng NHÁP với thông tin bác sĩ hiện tại.";
                XtraMessageBox.Show(this, msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }


        /// <summary>
        /// Tạo báo cáo kết luận từ dữ liệu hiện tại
        /// </summary>
        private ThongTinKetLuanBaoCaoThuong CreateBaoCaoKetLuan()
        {
            var ketluan = new ThongTinKetLuanBaoCaoThuong
            {
                TGBacSiChiDinh = FormatDateTime(_chiDinhDichVuResponse?.Thoigianthuchien ?? DateTime.Now),
                TGBacSiKetLuan = FormatDateTime(_dateTGKetThuc?.DateTime ?? DateTime.Now),
                // Cắt khối text chỉ số đã sinh khỏi Mô tả khi in - bảng chỉ số sẽ in riêng, tránh trùng 2 lần
                MoTa = StripGeneratedParamText(_kqChanDoanResponse?.Kqcls_MoTa),
                KetLuan = _kqChanDoanResponse?.Kqcls_KetLuan,
                KhuyenNghi = _kqChanDoanResponse?.Kqcls_DeNghi,
                ChiDinhDichVu = _chiDinhDichVuResponse,
                TenBacSiKetLuan = GetBacSiKetLuanName()
            };

            // Bảng chỉ số động cho template có DetailReportBand (suggestion Structured)
            if (_paramFormControl != null && _paramFormControl.Visible)
            {
                ketluan.DanhSachChiSo = _paramFormControl.GetReportItems();

                var paramValues = _paramFormControl.GetParamValues();

                // Map MÃ param -> giá trị cho script bind trong repx (ô đặt Tag = mã).
                // Lấy đủ mọi chỉ số kể cả rỗng để ô trống vẫn hiện đúng (không bị giá trị cũ).
                // Checkbox chưa tích trả rỗng kể cả khi ô giá trị còn text (giá trị không có hiệu lực).
                ketluan.ChiSoTheoMa = paramValues.ToDictionary(
                    p => p.paramCode,
                    p => p.@checked == false
                        ? string.Empty
                        : (!string.IsNullOrEmpty(p.value)
                            ? p.value
                            : (p.@checked == true ? (p.displayLabel ?? "x") : string.Empty)));

                // Map MÃ param -> trạng thái tích cho XRCheckBox trong repx (Tag = mã, gọi GetChiSoCheck)
                ketluan.ChiSoCheckTheoMa = paramValues
                    .Where(p => p.@checked.HasValue)
                    .ToDictionary(p => p.paramCode, p => p.@checked.Value);
            }

            // Lấy danh sách ảnh đã được chọn
            var selectedImagePaths = GetSelectedImagePaths();

            // Gán ảnh vào các thuộc tính (tối đa 4 ảnh)
            AssignImagesToKetLuan(ketluan, selectedImagePaths);

            // Gán ảnh chữ ký nếu có
            ketluan.AnhChuKy = GetAnhChuKy();

            // Chuyển đổi giới tính
            ketluan.GioiTinhConvert = ConvertGioiTinh(ketluan.ChiDinhDichVu?.BenhNhan?.GioiTinh);

            return ketluan;
        }

        /// <summary>
        /// Lấy tên bác sĩ kết luận
        /// </summary>
        private string GetBacSiKetLuanName()
        {
            if (ServiceLocator.KeycloakUserInfo == null)
                return string.Empty;

            var firstName = ServiceLocator.KeycloakUserInfo.FirstName ?? string.Empty;
            var lastName = ServiceLocator.KeycloakUserInfo.LastName ?? string.Empty;

            return $"{firstName} {lastName}".Trim();
        }

        /// <summary>
        /// Lấy danh sách đường dẫn ảnh đã được chọn
        /// </summary>
        private List<string> GetSelectedImagePaths()
        {
            var imagePaths = new List<string>();

            if (_lstBoxPages?.ImageCollections == null)
                return imagePaths;

            foreach (var item in _lstBoxPages.ImageCollections)
            {
                try
                {
                    if (item?.Images == null || item.Images.Count == 0)
                        continue;

                    var image = item.Images[0];
                    if (image?.Checked == true && image.Parent?.Name != null)
                    {
                        string imagePath = image.Parent.Name;
                        if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                        {
                            imagePaths.Add(imagePath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Lỗi khi lấy đường dẫn ảnh từ ImageCollection");
                }
            }

            return imagePaths;
        }

        /// <summary>
        /// Gán ảnh vào các thuộc tính của kết luận (tối đa 4 ảnh)
        /// </summary>
        private void AssignImagesToKetLuan(ThongTinKetLuanBaoCaoThuong ketluan, List<string> imagePaths)
        {
            if (imagePaths == null || imagePaths.Count == 0)
                return;

            // Sử dụng array để tránh nhiều if-else
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
                    {
                        imageProperties[i].Property(base64Image);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Lỗi khi chuyển đổi ảnh sang Base64: {imagePaths[i]}");
                }
            }
        }

        /// <summary>
        /// Chuyển đổi ảnh thành Base64 string
        /// </summary>
        private string ConvertImageToBase64(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
            {
                Log.Warning($"File ảnh không tồn tại: {imagePath}");
                return null;
            }

            try
            {
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                return Convert.ToBase64String(imageBytes);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Lỗi khi đọc file ảnh: {imagePath}");
                return null;
            }
        }

        /// <summary>
        /// Lấy ảnh chữ ký nếu có
        /// </summary>
        private string GetAnhChuKy()
        {
            return _hisUserKySoResponse != null && !string.IsNullOrEmpty(_hisUserKySoResponse.AnhChuKy)
                ? _hisUserKySoResponse.AnhChuKy
                : null;
        }

        /// <summary>
        /// Chuyển đổi giới tính từ số sang chuỗi
        /// </summary>
        private string ConvertGioiTinh(int? gioiTinh)
        {
            return gioiTinh == 1 ? "Nữ" :
                gioiTinh == 0 ? "Nam" :
                "";
        }

        /// <summary>
        /// Format datetime thành chuỗi theo định dạng tiếng Việt
        /// </summary>
        private string FormatDateTime(DateTime dateTime)
        {
            return $"{dateTime.Hour} giờ {dateTime.Minute} phút, " +
                   $"ngày {dateTime.Day} tháng {dateTime.Month} năm {dateTime.Year}";
        }
        #endregion
    }
}
