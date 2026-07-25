using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MediaToPacs.Core.Models;
using MediaToPacs.Core.Models.Ketluan;
using STM.MediaToPACS.Main.Utilities;
using Serilog;

namespace STM.MediaToPACS.Main.UI.V2
{
    /// <summary>
    /// Lưu nháp kết luận - chuyển thể từ FrmMain.SaveLoad.cs. Khác biệt: lấy ảnh đã chọn từ
    /// _thumbnailList.GetCheckedFilePaths() (ImageThumbnailList) thay vì _lstBoxPages.ImageCollections
    /// (ListImageBox - Leadtools).
    /// </summary>
    public partial class FormMainV2
    {
        private void _btnSave_Click(object sender, EventArgs e)
        {
            LuuNhap();
        }

        private async void LuuNhap()
        {
            if (_chiDinhDichVuResponse == null || _listThietBi == null || _listThietBi.Count == 0)
            {
                MessageBox.Show(this,
                    "Chưa tải được thông tin chỉ định hoặc danh sách thiết bị.\n" +
                    "Vui lòng kiểm tra kết nối RIS.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _btnSave.Enabled = false;

                var layoutSelect = _cbbLayout.SelectedItem as ReportTemplateGridViewModel;
                if (layoutSelect != null)
                    ServiceLocator.ReportCache[_chiDinhDichVuResponse.Modality] = layoutSelect.Id;

                List<string> imageSelectedList = new List<string>();
                listImageKeyLocal = new List<string>();

                var checkedImagePaths = _thumbnailList.GetCheckedFilePaths();
                var failedImages = new List<string>();
                foreach (var filePath in checkedImagePaths)
                {
                    try
                    {
                        byte[] fileBytes = File.ReadAllBytes(filePath);
                        string base64String = Convert.ToBase64String(fileBytes);
                        listImageKeyLocal.Add(filePath);
                        imageSelectedList.Add(base64String);
                    }
                    catch (Exception ex)
                    {
                        Log.Warning(ex, "Không đọc được file ảnh: {FilePath}", filePath);
                        failedImages.Add(Path.GetFileName(filePath));
                    }
                }

                if (failedImages.Count > 0)
                {
                    var confirm = MessageBox.Show(this,
                        $"{failedImages.Count} ảnh không đọc được và sẽ KHÔNG được đính kèm vào kết quả:\n- " +
                        string.Join("\n- ", failedImages) +
                        "\n\nBạn có muốn tiếp tục lưu không?",
                        "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirm != DialogResult.Yes)
                        return;
                }

                string mota = _rtMoTa.Text;
                string ketluan = _rtKetLuan.Text;
                string khuyennghi = _rtKhuyenNghi.Text;

                string maThietBi = null, tenThietBi = null, maKTV = null, tenKTV = null;

                var selectedValue = _cbbDSThietBi.EditValue;
                if (selectedValue == null)
                {
                    MessageBox.Show(this, "Vui lòng chọn thiết bị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!int.TryParse(Convert.ToString(selectedValue), out int selectedThietBiId))
                {
                    MessageBox.Show(this, "Thiết bị được chọn không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var thietBiSelect = _listThietBi.FirstOrDefault(x => x.id == selectedThietBiId);
                if (thietBiSelect == null)
                {
                    MessageBox.Show(this, "Vui lòng chọn thiết bị hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                tenThietBi = thietBiSelect.name;
                maThietBi = thietBiSelect.code;

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
                    sophieu = _sophieu,
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

                if (_kqChanDoanResponse != null)
                {
                    _ = SyncConclusionToRisV1Async(mota, ketluan, khuyennghi);

                    XmlSettingsHelper.Save(Path.Combine($"{_baseFolder}\\BenhNhan\\{_machidinh}", FileNameXMLImage), listImageKeyLocal);
                    _btnSignature.Enabled = true;
                    _btnPreviewMain.Enabled = true;
                    MessageBox.Show(this, "Lưu Kết Luận Chẩn Đoán Thành Công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this, "Lưu thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi lưu dữ liệu kết luận");
                MessageBox.Show(this, $"Lỗi khi lưu dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _btnSave.Enabled = true;
            }
        }
    }
}
