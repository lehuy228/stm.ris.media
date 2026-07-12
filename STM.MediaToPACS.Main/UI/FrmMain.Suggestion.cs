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
        // Luồng gợi ý cũ (API goi-y-ketluan) - giữ làm fallback khi API risv1 chưa sẵn sàng
        private List<GoiYKetLuanResponse> _listGoiYKetLuan { get; set; }

        // Luồng gợi ý mới (API risv1 quick-suggestions)
        private List<QuickSuggestionListItemDto> _listQuickSuggestions;
        private SuggestionPresenter _suggestionPresenter;

        // Form chỉ số động cho suggestion Structured (tạo lazy, sidebar trái trong tab Kết luận)
        private ParamFormControl _paramFormControl;
        // Thanh kéo cho phép bác sĩ tự nới/thu bề ngang sidebar bằng chuột
        private Splitter _paramSplitter;
        // Bề rộng gốc của cột camera (cột 1 trong _tbTableLayout) để khôi phục khi ẩn sidebar
        private float _originalCameraColWidth = -1f;
        // Khối text đã sinh lần trước - dùng để thay thế đúng khối đó trong ô Mô tả,
        // không ghi đè phần bác sĩ gõ tay bên ngoài khối
        private string _lastGeneratedParamText = "";

        private SuggestionPresenter GetSuggestionPresenter()
        {
            if (_suggestionPresenter == null)
                _suggestionPresenter = new SuggestionPresenter(ServiceLocator.RisService2);
            return _suggestionPresenter;
        }

        /// <summary>
        /// Load danh sách gợi ý: thử API mới (risv1 quick-suggestions, lọc gender + modality server-side),
        /// nếu lỗi hoặc rỗng thì fallback về API cũ (goi-y-ketluan, lọc gender client-side).
        /// </summary>
        private async Task LoadSuggestionsSafeAsync(int? hisGioiTinh)
        {
            _listQuickSuggestions = null;
            _listGoiYKetLuan = null;

            try
            {
                string modality = _chiDinhDichVuResponse?.Modality;
                int genderApi = SuggestionPresenter.MapHisGenderToApi(hisGioiTinh);
                Log.Information(
                    "Gọi quick-suggestions (risv1): baseUrl={BaseUrl}, modalityCode={Modality}, gender={Gender}",
                    ServiceLocator.SystemConfig?.UrlApiRisV2, modality, genderApi);

                _listQuickSuggestions = await GetSuggestionPresenter().LoadSuggestionsAsync(modality, hisGioiTinh);

                Log.Information("quick-suggestions trả về {Count} item",
                    _listQuickSuggestions != null ? _listQuickSuggestions.Count : 0);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không gọi được API quick-suggestions (risv1), fallback sang API gợi ý cũ");
            }

            if (_listQuickSuggestions == null || _listQuickSuggestions.Count == 0)
            {
                // TODO(legacy): gỡ nhánh fallback sau khi backend risv1 triển khai đủ dữ liệu ở các bệnh viện
                Log.Information("Fallback sang API gợi ý kết luận cũ (goi-y-ketluan)");
                var allGoiY = await ServiceLocator.RisService.GetDanhSachGoiYKetLuanResponseAsync(
                    madichvu: _chiDinhDichVuResponse.MaDichVu);
                _listGoiYKetLuan = FilterGoiYKetLuanByGender(allGoiY?.data, hisGioiTinh ?? -1);
            }
        }

        private void InitComboxMauGoiY()
        {
            _cbbMauGoiY.Properties.Items.Clear();

            if (_listQuickSuggestions != null && _listQuickSuggestions.Count > 0)
            {
                foreach (var item in _listQuickSuggestions)
                {
                    _cbbMauGoiY.Properties.Items.Add(item);
                }
            }
            else if (_listGoiYKetLuan != null)
            {
                foreach (var item in _listGoiYKetLuan)
                {
                    _cbbMauGoiY.Properties.Items.Add(item);
                }
            }

            if (_cbbMauGoiY.Properties.Items.Count > 0)
            {
                if (_kqChanDoanResponse == null)
                    _cbbMauGoiY.SelectedIndex = 0;
            }
        }
        private List<GoiYKetLuanResponse> FilterGoiYKetLuanByGender(
            List<GoiYKetLuanResponse> allGoiY, int gioiTinh)
        {
            if (allGoiY == null || allGoiY.Count == 0)
                return new List<GoiYKetLuanResponse>();

            return allGoiY.Where(x =>
            {
                if (string.IsNullOrWhiteSpace(x.gioitinh))
                    return true;

                var g = x.gioitinh.Trim().ToLower();

                if (gioiTinh == 0)          // Nam
                    return g == "nam";

                if (gioiTinh == 1)          // Nữ
                    return g == "nữ" || g == "nu";

                return true;                // Không xác định
            }).ToList();
        }
        private void _cbbReportTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Luồng mới: item là QuickSuggestionListItemDto - phải gọi API chi tiết để lấy nội dung
            if (_cbbMauGoiY.SelectedItem is QuickSuggestionListItemDto quickItem)
            {
                ApplyQuickSuggestionAsync(quickItem.id);
                return;
            }

            // Luồng fallback (API cũ): giữ nguyên hành vi, không có form chỉ số
            if (_cbbMauGoiY.SelectedItem is GoiYKetLuanResponse selected)
            {
                HideParamForm();
                _rtMoTa.Text = selected.kqcls_mota;
                _rtKhuyenNghi.Text = selected.kqcls_denghi;
                _rtKetLuan.Text = selected.kqcls_ketluan;

                var tb = _listThietBi?.FirstOrDefault(x => x.code == selected.mathietbi);
                if (tb != null)
                {
                    _cbbDSThietBi.EditValue = tb.id;
                }
            }
        }

        /// <summary>
        /// Lấy chi tiết suggestion mới và đổ vào 3 ô rich text.
        /// async void có chủ đích (fire-and-forget từ event handler) - mọi exception được nuốt tại chỗ kèm log.
        /// </summary>
        private async void ApplyQuickSuggestionAsync(long id)
        {
            try
            {
                var content = await GetSuggestionPresenter().GetContentAsync(id);
                if (content == null)
                {
                    Log.Warning("Suggestion {Id} không còn tồn tại trên server", id);
                    return;
                }

                _rtMoTa.Text = content.MoTa ?? "";
                _rtKhuyenNghi.Text = content.KhuyenNghi ?? "";
                _rtKetLuan.Text = content.KetLuan ?? "";
                // Mô tả vừa bị thay mới hoàn toàn nên khối text sinh cũ không còn
                _lastGeneratedParamText = "";

                if (content.IsStructured)
                {
                    OnStructuredSuggestionSelected(content.Detail);
                }
                else
                {
                    HideParamForm();
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Lỗi khi áp dụng gợi ý kết luận {Id}", id);
            }
        }

        /// <summary>
        /// Suggestion Structured: render form chỉ số động từ detail.paramGroups
        /// vào panel trượt trong tab Kết luận và đồng bộ text sinh ra vào ô Mô tả.
        /// </summary>
        private void OnStructuredSuggestionSelected(QuickSuggestionPublicDetailDto detail)
        {
            Log.Information("Suggestion Structured được chọn: {Title} (reportParam: {Code})",
                detail.title, detail.reportParamCode);

            EnsureParamFormControl();
            _paramFormControl.SetData(detail);
            _paramFormControl.Visible = true;
            UpdateParamSplitterVisible();
            // Thu bớt bề ngang cột camera để nhường chỗ (camera khung vuông nên không ảnh hưởng)
            SetCameraColumnShrink(true);

            // Đồng bộ ngay lần đầu để các presetValue xuất hiện trong Mô tả
            SyncParamTextToMoTa();
        }

        /// <summary>
        /// Tạo sidebar form chỉ số (1 lần duy nhất) và dock vào cạnh trái BÊN TRONG
        /// tab Kết luận (vùng Mô tả/Kết luận/Đề nghị) - không đè lên hàng thông tin
        /// chỉ định phía trên và footer phía dưới.
        /// </summary>
        private void EnsureParamFormControl()
        {
            if (_paramFormControl != null)
                return;

            _paramSplitter = new Splitter
            {
                Dock = DockStyle.Left,
                Width = 6,
                Visible = false,
                BackColor = Color.FromArgb(220, 226, 236),
                MinSize = 260,   // sidebar không hẹp hơn 260px khi kéo
                MinExtra = 320   // vùng Mô tả bên phải giữ tối thiểu 320px
            };
            // Kéo splitter xong thì lưu bề rộng sidebar, lần mở sau nạp lại
            _paramSplitter.SplitterMoved += (s, e) =>
            {
                if (_paramFormControl != null && _paramFormControl.Visible && !_paramFormControl.Collapsed)
                    SaveParamSidebarWidth(_paramFormControl.Width);
            };

            _paramFormControl = new ParamFormControl
            {
                Visible = false,
                Dock = DockStyle.Left
            };
            _paramFormControl.ParamValuesChanged += (s, e) => SyncParamTextToMoTa();
            _paramFormControl.CollapsedChanged += (s, e) => UpdateParamSplitterVisible();

            // Thứ tự Add quyết định thứ tự dock (z-index cao dock trước):
            // sidebar dock trước chiếm dải trái, splitter dock ngay sau nằm sát mép phải sidebar,
            // các control còn lại (combo gợi ý Top, Kết luận/Đề nghị Bottom, Mô tả Fill) dồn sang phải
            xtraTabPage1.Controls.Add(_paramSplitter);
            xtraTabPage1.Controls.Add(_paramFormControl);

            // Bề rộng khởi tạo: ưu tiên giá trị người dùng đã lưu (clamp theo kích thước tab hiện tại),
            // chưa có thì mặc định ~2/5 tab Kết luận, tối đa 500px; sau đó user tự kéo splitter
            int sidebarWidth = Math.Min(500, xtraTabPage1.ClientSize.Width * 2 / 5);
            int savedSidebarWidth = GetSavedParamSidebarWidth();
            if (savedSidebarWidth > 0)
            {
                sidebarWidth = Math.Max(_paramSplitter.MinSize,
                    Math.Min(savedSidebarWidth, xtraTabPage1.ClientSize.Width - _paramSplitter.MinExtra));
                Log.Information("Đã nạp bề rộng sidebar chỉ số đã lưu: {Width}px", sidebarWidth);
            }
            _paramFormControl.SetExpandedWidth(sidebarWidth);
        }

        /// <summary>
        /// Cắt khối text chỉ số đã sinh (live-sync) ra khỏi Mô tả khi tạo báo cáo PDF:
        /// trên màn hình và khi lưu về RIS vẫn giữ nguyên text, chỉ bản in dùng bảng chỉ số riêng.
        /// Nếu form chỉ số không mở hoặc không tìm thấy khối đã sinh thì trả về nguyên văn.
        /// </summary>
        private string StripGeneratedParamText(string moTa)
        {
            if (string.IsNullOrEmpty(moTa))
                return moTa;
            if (_paramFormControl == null || !_paramFormControl.Visible)
                return moTa;
            if (string.IsNullOrEmpty(_lastGeneratedParamText))
                return moTa;

            int idx = moTa.LastIndexOf(_lastGeneratedParamText, StringComparison.Ordinal);
            if (idx < 0)
                return moTa;

            return moTa.Remove(idx, _lastGeneratedParamText.Length).TrimEnd('\r', '\n', ' ');
        }

        /// <summary>Splitter chỉ hiện khi sidebar đang mở rộng (ẩn khi thu gọn/đóng)</summary>
        private void UpdateParamSplitterVisible()
        {
            if (_paramSplitter == null || _paramFormControl == null)
                return;
            _paramSplitter.Visible = _paramFormControl.Visible && !_paramFormControl.Collapsed;
        }

        /// <summary>Ẩn form chỉ số khi chọn suggestion Text/luồng cũ</summary>
        private void HideParamForm()
        {
            if (_paramFormControl == null)
                return;
            _paramFormControl.Visible = false;
            _paramFormControl.ClearData();
            _lastGeneratedParamText = "";
            UpdateParamSplitterVisible();
            SetCameraColumnShrink(false);
        }

        /// <summary>
        /// Thu hẹp/khôi phục bề ngang cột camera (cột 1 - Absolute trong _tbTableLayout).
        /// Camera thiết bị hầu hết khung hình vuông nên hẹp bớt ~90px không ảnh hưởng,
        /// viewport tự tính lại tỷ lệ qua ResizeCameraViewport.
        /// </summary>
        private void SetCameraColumnShrink(bool shrink)
        {
            if (_tbTableLayout.ColumnStyles.Count < 2)
                return;

            var style = _tbTableLayout.ColumnStyles[1];
            if (style.SizeType != SizeType.Absolute)
                return;

            // Người dùng đã tự kéo chỉnh bề rộng cột camera -> giữ nguyên lựa chọn đó,
            // không tự thu/nới theo sidebar chỉ số nữa
            if (_userCameraColWidth > 0)
            {
                style.Width = ClampCameraColumnWidth(_userCameraColWidth);
                return;
            }

            if (_originalCameraColWidth < 0)
                _originalCameraColWidth = style.Width;

            style.Width = shrink
                ? Math.Max(CameraColShrunkMinWidth, _originalCameraColWidth - CameraColShrinkPx)
                : _originalCameraColWidth;
        }

        /// <summary>
        /// Đồng bộ text sinh từ form chỉ số vào ô Mô tả theo cơ chế thay thế khối:
        /// tìm và thay đúng khối đã sinh lần trước, phần bác sĩ gõ tay bên ngoài khối được giữ nguyên.
        /// Nếu bác sĩ sửa tay vào giữa khối đã sinh thì khối mới sẽ được nối vào cuối.
        /// </summary>
        private void SyncParamTextToMoTa()
        {
            if (_paramFormControl == null || !_paramFormControl.Visible)
                return;

            // Không tự sửa Mô tả khi đã ký số (ô bị khóa)
            if (!_rtMoTa.Enabled)
                return;

            string newText = _paramFormControl.GenerateText();
            string current = _rtMoTa.Text ?? "";

            if (!string.IsNullOrEmpty(_lastGeneratedParamText) && current.Contains(_lastGeneratedParamText))
            {
                int idx = current.LastIndexOf(_lastGeneratedParamText, StringComparison.Ordinal);
                current = current.Remove(idx, _lastGeneratedParamText.Length).Insert(idx, newText);
            }
            else if (!string.IsNullOrEmpty(newText))
            {
                if (current.Length > 0 && !current.EndsWith("\n"))
                    current += "\n";
                current += newText;
            }

            _rtMoTa.Text = current;
            _lastGeneratedParamText = newText;
        }
    }
}
