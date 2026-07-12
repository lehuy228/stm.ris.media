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
        #region Save/Load Operations

        private void _btnSave_Click(object sender, EventArgs e)
        {
            LuuNhap();
        }

        private async void LuuNhap()
        {
            // Luồng mở từ Worklist không có mã chỉ định nên dữ liệu chỉ định/thiết bị chưa được tải,
            // nếu không chặn ở đây sẽ NullReferenceException khi truy cập _chiDinhDichVuResponse/_listThietBi
            if (_chiDinhDichVuResponse == null || _listThietBi == null || _listThietBi.Count == 0)
            {
                XtraMessageBox.Show(this,
                    "Chưa tải được thông tin chỉ định hoặc danh sách thiết bị.\n" +
                    "Vui lòng mở phiếu từ danh sách chỉ định RIS hoặc kiểm tra kết nối RIS.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitFormLoading), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu...");
                SplashScreenManager.Default.SetWaitFormDescription("Vui lòng chờ trong giây lát...");
                _btnSave.Enabled = false;

                var layoutSelect = _cbbLayout.SelectedItem as ReportTemplateGridViewModel;
                if (layoutSelect != null)
                {
                    ServiceLocator.ReportCache[_chiDinhDichVuResponse.Modality] = layoutSelect.Id;
                }

                List<string> imageSelectedList = new List<string>();
                listImageKeyLocal = new List<string>();

                var checkedImages = _lstBoxPages.ImageCollections
                    .Where(x => x.Images?.Count > 0 && x.Images[0].Checked)
                    .Select(x => x.Images[0])
                    .ToList();

                var failedImages = new List<string>();
                foreach (var image in checkedImages)
                {
                    try
                    {
                        string filePath = image.Parent.Name;
                        byte[] fileBytes = File.ReadAllBytes(filePath);
                        string base64String = Convert.ToBase64String(fileBytes);
                        listImageKeyLocal.Add(filePath);
                        if (base64String != null)
                        {
                            imageSelectedList.Add(base64String);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Không được nuốt lỗi âm thầm: ảnh y tế bị thiếu phải có cảnh báo rõ ràng
                        Log.Warning(ex, "Không đọc được file ảnh: {FilePath}", image.Parent?.Name);
                        failedImages.Add(Path.GetFileName(image.Parent?.Name ?? "(không rõ tên file)"));
                    }
                }

                if (failedImages.Count > 0)
                {
                    SplashScreenManager.CloseForm(false);
                    var confirm = XtraMessageBox.Show(this,
                        $"{failedImages.Count} ảnh không đọc được và sẽ KHÔNG được đính kèm vào kết quả:\n- " +
                        string.Join("\n- ", failedImages) +
                        "\n\nBạn có muốn tiếp tục lưu không?",
                        "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirm != DialogResult.Yes)
                        return;
                    SplashScreenManager.ShowForm(this, typeof(WaitFormLoading), true, true, false);
                    SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu...");
                    SplashScreenManager.Default.SetWaitFormDescription("Vui lòng chờ trong giây lát...");
                }

                string mota = _rtMoTa.Text;
                string ketluan = _rtKetLuan.Text;
                string khuyennghi = _rtKhuyenNghi.Text;

                string maThietBi = null;
                string tenThietBi = null;
                string maKTV = null;
                string tenKTV = null;

                var selectedValue = _cbbDSThietBi.EditValue;
                if (selectedValue != null)
                {
                    int selectedThietBiId;
                    if (!int.TryParse(Convert.ToString(selectedValue), out selectedThietBiId))
                    {
                        MessageBox.Show("Thiết bị được chọn không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    var thietBiSelect = _listThietBi.FirstOrDefault(x => x.id == selectedThietBiId);
                    if (thietBiSelect != null)
                    {
                        tenThietBi = thietBiSelect.name;
                        maThietBi = thietBiSelect.code;
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng chọn thiết bị hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn thiết bị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                var selectedValueHisUser = _cbbHisUser.EditValue;
                if (selectedValueHisUser != null)
                {
                    var ktvSelect = _cbbHisUser.Properties.GetDataSourceRowByKeyValue(selectedValueHisUser) as PractitionerListDto;
                    if (ktvSelect != null)
                    {
                        maKTV = ktvSelect.staffCode;
                        tenKTV = ktvSelect.fullName;
                    }
                }

                var ketQuaChanDoanRequest = new KetQuaChanDoanRequest()
                {
                    kqcls_mota = string.IsNullOrWhiteSpace(mota) ? "_" : mota,
                    kqcls_denghi = string.IsNullOrWhiteSpace(khuyennghi) ? "_" : khuyennghi,
                    kqcls_ketluan = string.IsNullOrWhiteSpace(ketluan) ? "_" : ketluan,
                    mabacsiketluan = ServiceLocator.KeycloakUserInfo.HISCode,
                    machidinh = _machidinh,
                    sophieu = sophieu,
                    bacsiketluan = ServiceLocator.KeycloakUserInfo.FirstName + " " + ServiceLocator.KeycloakUserInfo.LastName,
                    trangthai = "Nháp",
                    kythuatvien = tenKTV,
                    makythuatvien = maKTV,
                    mathietbi = maThietBi,
                    tenthietbi = tenThietBi,
                    thoigianketthuc = _dateTGKetThuc.DateTime,
                    thoigianthuchien = _dateTGThucHien.DateTime,
                    imageFileKeys = imageSelectedList,
                };

                _kqChanDoanResponse = await ServiceLocator.RisService.TaoKetQuaChanDoanAsync(ketQuaChanDoanRequest);

                SplashScreenManager.CloseForm(false);

                if (_kqChanDoanResponse != null)
                {
                    // API cũ đã lưu THÀNH CÔNG -> sync best-effort kết luận + bảng chỉ số sang RIS mới.
                    // Fire-and-forget có chủ đích: lỗi được nuốt + log bên trong, không ảnh hưởng luồng chính
                    _ = SyncConclusionToRisV1Async(mota, ketluan, khuyennghi);

                    XmlSettingsHelper.Save<List<string>>(Path.Combine($"{_baseFolder}\\BenhNhan\\{_machidinh}", FileNameXMLImage), listImageKeyLocal);
                    _btnSignature.Enabled = true;
                    _btnPreviewMain.Enabled = true;
                    XtraMessageBox.Show(
                        this,
                        "Lưu Kết Luận Chẩn Đoán Thành Công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    XtraMessageBox.Show(
                        this,
                        "Lưu thất bại!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show(this, $"Lỗi khi lưu dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Luôn đóng SplashScreen
                if (SplashScreenManager.Default != null && SplashScreenManager.Default.IsSplashFormVisible)
                    SplashScreenManager.CloseForm(false);

                _btnSave.Enabled = true;
            }
        }

        public bool IsCheckRecord = false;
        private Panel _selectedPanel;
        private void PictureBoxRoll_Click(object sender, EventArgs e)
        {
            Panel parentPanel = null;
            string videoPath = "";

            if (sender is PictureBox pb)
            {
                parentPanel = pb.Parent as Panel;
                videoPath = pb.Tag as string;
            }
            else if (sender is Panel pnl)
            {
                parentPanel = pnl;
                if (pnl.Controls.Count > 0 && pnl.Controls[0] is PictureBox picture)
                    videoPath = picture.Tag as string;
            }

            if (_selectedPanel != null)
                _selectedPanel.BackColor = Color.Transparent;

            _selectedPanel = parentPanel;
            _selectedPanel.BackColor = Color.Red; // màu viền

            if (!string.IsNullOrEmpty(videoPath) && File.Exists(videoPath))
            {
                _mediaPlayerControl.SetFilePathMedia(videoPath);
            }
        }
        #endregion
    }
}
