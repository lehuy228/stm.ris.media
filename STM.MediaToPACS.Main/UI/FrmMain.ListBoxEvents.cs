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
        #region UI Control Events - ListBox
        /// <summary>
        /// Xử lý sự kiện khi trạng thái ListBox thay đổi (thêm, xóa, chọn item)
        /// Cập nhật trạng thái toolbar để phản ánh trạng thái hiện tại của listbox
        /// </summary>
        private void _lstBoxPages_ListStateChanged(object sender, EventArgs e)
        {
            UpdateToolBarState();
        }

        /// <summary>
        /// Xử lý sự kiện khi Context Menu của ListBox sắp mở
        /// Cập nhật trạng thái các menu items dựa trên trạng thái hiện tại của listbox
        /// </summary>
        /// <param name="sender">Object gửi sự kiện</param>
        /// <param name="e">CancelEventArgs - có thể hủy việc mở menu</param>
        private void _cmListBox_Opening(object sender, CancelEventArgs e)
        {
            _cmiExpanded.Checked = _lstBoxPages.ViewMode == ThumbMode.Expanded;
            _cmiCondensed.Checked = _lstBoxPages.ViewMode == ThumbMode.Condensed;

            _cmiDeleteAll.Enabled = _lstBoxPages.Items.Count > 0;
            _cmiDeleteSelected.Enabled = _lstBoxPages.SelectedItems.Count > 0;

        }

        /// <summary>
        /// Chuyển ListBox sang chế độ Expanded (hiển thị chi tiết thumbnail)
        /// </summary>
        private void _cmiExpanded_Click(object sender, EventArgs e)
        {
            _lstBoxPages.ViewMode = ThumbMode.Expanded;
        }


        private void _miResample_Click(object sender, EventArgs e)
        {
            RasterPaintProperties prop = _pictureBox.PaintProperties;
            _mySettings._settings.UseResample = prop.PaintDisplayMode == RasterPaintDisplayModeFlags.Resample;
            if (_mySettings._settings.UseResample)
                prop.PaintDisplayMode = RasterPaintDisplayModeFlags.None;
            else
                prop.PaintDisplayMode = RasterPaintDisplayModeFlags.Resample;
            _mySettings._settings.UseResample = !_mySettings._settings.UseResample;
            _mySettings.Save();
            _pictureBox.PaintProperties = prop;
        }

        /// <summary>
        /// Chuyển ListBox sang chế độ Condensed (hiển thị thu gọn thumbnail)
        /// </summary>
        private void _cmiCondensed_Click(object sender, EventArgs e)
        {
            _lstBoxPages.ViewMode = ThumbMode.Condensed;
        }

        /// <summary>
        /// Mở dialog để chọn và load file ảnh raster vào ListBox
        /// Hỗ trợ preview ảnh và nhớ thư mục cuối cùng đã mở
        /// </summary>
        private void _miOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlgOpen = new OpenFileDialog())
            {
                dlgOpen.Title = "Chọn hình ảnh";
                dlgOpen.Multiselect = true;
                dlgOpen.Filter =
                    "Image files|*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff|" +
                    "All files|*.*";

                string targetFolder = Path.Combine(
                    _baseFolder,
                    "BenhNhan",
                    _machidinh
                );

                // Tạo folder nếu chưa tồn tại
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }

                if (targetFolder != null)
                    dlgOpen.InitialDirectory = Path.GetDirectoryName(Path.Combine($"{_baseFolder}\\BenhNhan\\{_machidinh}"));

                bool bTopMost = logWindow.TopMost;
                logWindow.TopMost = false;

                if (dlgOpen.ShowDialog(this) != DialogResult.OK)
                {
                    logWindow.TopMost = bTopMost;
                    return;
                }

                logWindow.TopMost = bTopMost;

                // Load nhiều file
                foreach (string fileName in dlgOpen.FileNames)
                {
                    string destFile = Path.Combine(
                        targetFolder,
                        Path.GetFileName(fileName)
                    );
                    File.Copy(fileName, destFile, true);
                    LoadRasterImage(fileName);
                }

                strLastLocation = dlgOpen.FileNames[0];
            }
        }

        private void _miPaste_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap img = (Bitmap)Clipboard.GetImage();
                RasterImage rImg = Leadtools.Drawing.RasterImageConverter.ChangeFromHBitmap(img.GetHbitmap(), IntPtr.Zero);
                CreateImageCollection("Pasted Image", rImg);
            }
            catch (Exception ex)
            {
                // Clipboard không có ảnh hoặc ảnh không hợp lệ - không chặn thao tác nhưng phải ghi log
                Log.Warning(ex, "Không thể dán ảnh từ clipboard");
            }
        }

        private void _toolBtnTwain_Click(object sender, EventArgs e)
        {
            _miTwainAcquire_Click(null, null);
        }

        private bool BeforeAddTagDelegate(LinkedList<long> parent, object data, long tag)
        {
            DicomTag dcmTag = DicomTagTable.Instance.Find(tag);

            if (dcmTag != null)
            {
                Log.Debug("Tag: " + dcmTag.Name);
            }
            else
                Log.Debug(string.Format("Tag: {0:x4}:{1:x4}", DicomExtensions.GetGroup(tag), DicomExtensions.GetElement(tag)));

            return false;
        }

        private void _miRotate90_Click(object sender, EventArgs e)
        {
            if (_pictureBox.Image == null)
                return;

            try
            {
                _pictureBox.Image.RotateViewPerspective(90);
                _lstBoxPages.SelectedItem.RasterImage.RotateViewPerspective(90);
                string strFileLoc = (_lstBoxPages.SelectedItem.ImageItem.Tag as IPrintToPACSFile).FileLocation();

                if (_lstBoxPages.SelectedItem.ImageItem.Tag.GetType() == typeof(PrintPage))
                    _codec.Save(_lstBoxPages.SelectedItem.RasterImage, strFileLoc, RasterImageFormat.Emf, 0);
                else
                    _codec.Save(_lstBoxPages.SelectedItem.RasterImage, strFileLoc, RasterImageFormat.Tif, 0);
            }
            catch (Exception ex)
            {
                // Nếu xoay/lưu thất bại, ảnh hiển thị và ảnh trên đĩa có thể lệch nhau - cần log để truy vết
                Log.Warning(ex, "Lỗi khi xoay ảnh 90 độ");
            }
        }

        private void _toolBtnRotate_Click(object sender, EventArgs e)
        {
            _miRotate90_Click(null, null);
        }

        /// <summary>
        ///  Xử lý khi bỏ chọn item
        /// </summary>
        private void _lstBoxPages_ItemDeSlect(object sender, EventArgs e)
        {
            if (_pictureBox.Image != null)
            {
                _pictureBox.Image.Dispose();
                _pictureBox.Image = null;
            }
            //_btnNext.Enabled = false;
            //_btnPrev.Enabled = false;
            //_lblPageInfo.Text = "";
            UpdateToolBarState();
        }

        private void _pictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) != 0)
            {
                if (e.Delta > 0)
                    _miZoomIn_Click(null, null);
                else
                    _miZoomOut_Click(null, null);
            }
            else
            {
                int iSelectedPage = 0;
                if (_lstBoxPages.ViewMode == ThumbMode.Condensed)
                    iSelectedPage = _lstBoxPages.SelectedItemGroupIndex;
                else
                    iSelectedPage = _lstBoxPages.SelectedIndex;

            }
        }

        private void _miClearPrintedList_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show(
                    "Bạn có chắc muốn xóa tất cả mục đã chọn (Ảnh sẽ mất vĩnh viễn)?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result != DialogResult.Yes)
                    return; // Người dùng không đồng ý → thoát

                // Nếu chọn Yes thì tiếp tục xóa
                ClearList();

                if (_pictureBox.Image != null)
                {
                    _pictureBox.Image.Dispose();
                    _pictureBox.Image = null;
                }

                UpdateToolBarState();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void _miExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void _lstBoxPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScalePicture(_lstBoxPages.SelectedItem.ImageItem);
            UpdateToolBarState();
            _iOldIndex = _lstBoxPages.SelectedIndex;
            _mySettings._settings.LastSelectedIndex = _lstBoxPages.SelectedIndex;
            _mySettings.Save();
        }

        private void _miFile_DropDownOpening(object sender, EventArgs e)
        {
            try
            {
                _miSaveAsDICOM.Enabled = (_lstBoxPages.CheckedItems.Count > 0);
                _miStoreToPACS.Enabled = (_lstBoxPages.CheckedItems.Count > 0);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void _lstBoxPages_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete && _lstBoxPages.SelectedItem != null)
                    _miDeleteSelected_Click(null, null);

                if (e.KeyCode == Keys.V && Control.ModifierKeys == Keys.Control)
                    _miPaste_Click(null, null);

                else if (e.KeyCode == Keys.Add)
                {
                    _miZoomIn_Click(_miZoomIn, new EventArgs());
                }
                else if (e.KeyCode == Keys.Subtract)
                {
                    _miZoomOut_Click(_miZoomOut, new EventArgs());
                }
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        #endregion
    }
}
