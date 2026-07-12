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
        #region Image Operations - Zoom, Rotate, Scale

        /// <summary>
        /// Phóng to/thu nhỏ ảnh trong PictureBox với hệ số zoom được chỉ định
        /// </summary>
        /// <param name="dZoomFactor">Hệ số zoom (dương = zoom in, âm = zoom out)</param>
        /// <remarks>
        /// - Nếu đang ở chế độ FitAlways, sẽ tính toán scale factor hiện tại dựa trên kích thước ảnh và PictureBox
        /// - Giới hạn zoom: tối đa 3x (300%) và tối thiểu 0.06x (6%)
        /// - Sau khi zoom, chuyển sang chế độ Normal để cho phép scroll
        /// </remarks>
        private void ZoomPicture(double dZoomFactor)
        {
            try
            {
                double oldScaleFactor = _pictureBox.ScaleFactor;
                if (_pictureBox.SizeMode == RasterPaintSizeMode.FitAlways)
                {
                    double dWidthFraction = (double)(_pictureBox.Width - 30) / (double)_pictureBox.Image.Width;
                    double dHeightFraction = (double)(_pictureBox.Height - 30) / (double)_pictureBox.Image.Height;
                    double dScale = dWidthFraction;
                    if (dHeightFraction < dWidthFraction)
                    {
                        dScale = dHeightFraction;
                    }
                    _pictureBox.SizeMode = RasterPaintSizeMode.Normal;
                    oldScaleFactor = dScale;
                }


                oldScaleFactor = oldScaleFactor + dZoomFactor;
                if (oldScaleFactor > 3 && dZoomFactor > 0)
                    return;
                if (oldScaleFactor < .06 && dZoomFactor < 0)
                    return;
                _pictureBox.SizeMode = RasterPaintSizeMode.Normal;
                _pictureBox.ScaleFactor = oldScaleFactor;
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Lỗi khi zoom ảnh");
            }
        }

        /// <summary>
        /// Tạo một ImageCollection mới từ RasterImage và thêm vào ListBox
        /// </summary>
        /// <param name="strTittle">Tiêu đề của collection (tên hiển thị)</param>
        /// <param name="rasterImage">RasterImage cần thêm vào collection</param>
        /// <remarks>
        /// - Lưu ảnh vào file tạm thời (TIF format)
        /// - Tạo Page object với DeleteOnDispose = true để tự động xóa file khi dispose
        /// - Dispose RasterImage gốc sau khi đã lưu và load lại
        /// - Thêm collection vào ListBox để hiển thị
        /// </remarks>
        private void CreateImageCollection(string strTittle, RasterImage rasterImage)
        {
            ListImageBox.ImageCollection imagecollection = new ListImageBox.ImageCollection(strTittle);
            Page page = new Page();
            string strTemp = null;
            strTemp = Path.GetTempFileName();
            _codec.Save(rasterImage, strTemp, RasterImageFormat.Tif, 0);
            page.FilePath = strTemp;
            page.DeleteOnDispose = true;
            imagecollection.Images.Add(new ListImageBox.ImageItem(_codec.Load(strTemp), imagecollection, page));
            rasterImage.Dispose();

            _lstBoxPages.AddImageCollection(imagecollection);
        }

        /// <summary>
        /// Xóa các item đã được chọn trong ListBox và file tương ứng trên disk
        /// </summary>
        /// <remarks>
        /// - Lưu vị trí scroll hiện tại để restore sau khi xóa
        /// - Duyệt từ cuối lên đầu để tránh lỗi index khi xóa
        /// - Xóa cả item trong ListBox và file trên disk
        /// - Restore lại vị trí scroll sau khi xóa (phải dùng Math.Abs vì AutoScrollPosition trả về giá trị âm)
        /// </remarks>
        private void DeleteSelectedItems()
        {
            // 1. Lưu vị trí scroll hiện tại (pixel)
            Point oldScrollPos = _lstBoxPages.AutoScrollPosition;

            // 2. Tạm dừng layout để tránh auto scroll
            _lstBoxPages.SuspendLayout();

            int firstSelectedIndex = -1;

            for (int i = _lstBoxPages.Items.Count - 1; i >= 0; i--)
            {
                var item = _lstBoxPages.Items[i];
                if (item.Selected)
                {
                    if (firstSelectedIndex == -1)
                        firstSelectedIndex = i;

                    _lstBoxPages.RemoveItem(i);
                    TryDeleteFile(item.ImageItem.Parent.Name);
                }
            }

            _lstBoxPages.ResumeLayout(true);

            if (_lstBoxPages.Items.Count == 0)
                return;

            // 3. Restore lại vị trí scroll (LƯU Ý: phải đảo dấu)
            _lstBoxPages.AutoScrollPosition = new Point(
                Math.Abs(oldScrollPos.X),
                Math.Abs(oldScrollPos.Y)
            );
        }

        /// <summary>
        /// Xóa file với xử lý lỗi đầy đủ
        /// </summary>
        /// <param name="filePath">Đường dẫn file cần xóa</param>
        /// <remarks>
        /// Xử lý các trường hợp lỗi:
        /// - File không tồn tại: Hiển thị cảnh báo
        /// - UnauthorizedAccessException: Không có quyền xóa (cần quyền Administrator)
        /// - IOException: File đang được sử dụng bởi ứng dụng khác
        /// - Exception khác: Lỗi không xác định
        /// </remarks>
        public static void TryDeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                else
                {
                    MessageBox.Show("File không tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Không có quyền xóa file này. Hãy chạy chương trình bằng quyền Administrator.", "Lỗi quyền truy cập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show($"File đang được sử dụng bởi ứng dụng khác.\nChi tiết: {ex.Message}", "Lỗi IO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa file: {ex.Message}", "Lỗi không xác định", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Delegate để thêm ImageCollection từ thread khác
        /// </summary>
        private delegate void AddImageCollectionThreadedDelegate(ListImageBox.ImageCollection collection);

        /// <summary>
        /// Thêm ImageCollection vào ListBox một cách thread-safe
        /// Tự động invoke về UI thread nếu được gọi từ thread khác
        /// </summary>
        /// <param name="collection">ImageCollection cần thêm vào ListBox</param>
        private void AddImageCollectionThreaded(ListImageBox.ImageCollection collection)
        {
            if (InvokeRequired)
            {
                Invoke(new AddImageCollectionThreadedDelegate(AddImageCollectionThreaded), collection);
            }
            else
            {
                _lstBoxPages.AddImageCollection(collection);
            }
        }

        /// <summary>
        /// Enable/Disable các controls trên form và hiển thị form operation nếu cần
        /// Thread-safe: Tự động invoke về UI thread nếu được gọi từ thread khác
        /// </summary>
        /// <param name="enable">true = enable controls, false = disable controls</param>
        /// <param name="strCaption">Caption hiển thị trên form operation (khi disable)</param>
        /// <param name="strBtnCaption">Text của nút Cancel trên form operation (nếu có)</param>
        /// <remarks>
        /// Khi enable = false:
        /// - Đổi cursor sang WaitCursor
        /// - Disable tất cả controls chính
        /// - Hiển thị FrmOperation với caption và button nếu được cung cấp
        /// 
        /// Khi enable = true:
        /// - Đổi cursor về Arrow
        /// - Enable lại tất cả controls
        /// - Cập nhật toolbar state
        /// - Đóng FrmOperation nếu đang mở
        /// </remarks>
        public void EnableItems(bool enable, string strCaption, string strBtnCaption)
        {
            if (this.InvokeRequired)
            {
                Invoke(new EnableMenu(EnableItems), new object[] { enable, strCaption, strBtnCaption });
            }
            else
            {
                if (enable)
                    Cursor.Current = Cursors.Arrow;
                else
                    Cursor.Current = Cursors.WaitCursor;

                _mmMain.Enabled = enable;
                _lstBoxPages.Enabled = enable;
                //_tbDicomInfo.Enabled = enable;
                _cmbSopClasses.Enabled = enable;
                //_cbStoreServers.Enabled = enable;
                toolStripComboBoxStoreServer.Enabled = enable;
                _btnPushToPACS.Enabled = enable;
                _toolbarMain.Enabled = enable;
                _pgDicomInfo.Enabled = enable;
                //_btnPACSSettings.Enabled = enable;
                //_btnOpenImage.Enabled = enable;
                if (enable)
                {
                    UpdateToolBarState();
                    if (_frmOperation != null)
                        _frmOperation.Close();
                }
                else
                {
                    if (!(strCaption == "" && strBtnCaption == ""))
                        if (_frmOperation == null || !_frmOperation.Visible)
                        {
                            _frmOperation = new FrmOperation(strCaption, strBtnCaption);
                            bCancelOperation = false;
                            if (strBtnCaption != "")
                                _frmOperation.Cancel += new EventHandler(_frmOperation_Cancel);
                            _frmOperation.Show();
                        }
                }
            }
        }

        private void ScalePicture(STM.MediaToPACS.Main.UI.ListImageBox.ImageItem item)
        {
            if (_pictureBox.Image != null)
            {
                _pictureBox.Image.Dispose();
                _pictureBox.Image = null;
            }
            _pictureBox.Image = item.Image.Clone();

            //_pictureBox_DoubleClick(null, null);
            _pictureBox.SizeMode = RasterPaintSizeMode.FitAlways;
            _pictureBox.ScaleFactor = 1;

        }

        private void InitClass()
        {
            if (RasterSupport.IsLocked(RasterSupportType.PrintDriver) && RasterSupport.IsLocked(RasterSupportType.PrintDriverServer))
            {
                throw new Exception("Printer driver capability is required.");
            }
        }

        private void ClearList()
        {
            try
            {
                DeleteTempFiles();
                _lstBoxPages.ClearList();
                if (_pictureBox.Image != null)
                {
                    _pictureBox.Image.Dispose();
                    _pictureBox.Image = null;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void DeleteTempFiles()
        {
            foreach (ListImageBox.ListItem item in _lstBoxPages.Items)
            {
                try
                {
                    item.Dispose();
                    TryDeleteFile(item.ImageItem.Parent.Name);
                }
                catch
                {
                }
            }
        }

        public void LogError(string sLogText)
        {
            LogText("*** ERROR *** ", _sNewlineTab + sLogText, Color.Red);
        }

        public void LogText(string action, string logText)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new AddLog(LogText),
                   new object[] { action, logText });
            }
            else
            {
                PacsSettings.LogWindow.RichTextBox.AppendText(logText);
                PacsSettings.LogWindow.RichTextBox.AppendText("\r\n");
                PacsSettings.LogWindow.RichTextBox.ScrollToCaret();
            }
        }

        public void LogText(string sAction, string sLogText, Color sActionColor)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new AddLogColor(LogText), new object[] { sAction, sLogText, sActionColor });
            }
            else
            {
                //AddAction(sAction, sActionColor);
                PacsSettings.LogWindow.RichTextBox.AppendText(sLogText);
                PacsSettings.LogWindow.RichTextBox.AppendText(_sNewline);
                TextBoxTraceListener.SendMessage(PacsSettings.LogWindow.RichTextBox.Handle, TextBoxTraceListener.WM_VSCROLL, TextBoxTraceListener.SB_BOTTOM, 0);
            }
        }
        
        public delegate void StartUpdateDelegate(DataGridView lv);
        private void StartUpdate(DataGridView dg)
        {
            if (InvokeRequired)
            {
                Invoke(new StartUpdateDelegate(StartUpdate), dg);
            }
            else
            {
                dg.Rows.Clear();
            }
        }
        #endregion
    }
}
