using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
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
    public partial class FormPdfViewer : RibbonForm
    {
        private MemoryStream _pdfStream;
        private string _currentFileName = "Không rõ";

        public FormPdfViewer(byte[] pdfBytes, string fileName = "")
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(fileName))
            {
                _currentFileName = fileName;
            }

            this.WindowState = FormWindowState.Maximized;
            // this.Icon = Properties.Resources.AppIcon; // Nếu có icon

            LoadPdfByte(pdfBytes);
            SetupEvents();
            UpdateTitle();
        }

        /// <summary>
        /// Load PDF từ byte array
        /// </summary>
        private void LoadPdfByte(byte[] pdfBytes)
        {
            if (pdfBytes == null || pdfBytes.Length == 0)
            {
                XtraMessageBox.Show(
                    "Dữ liệu PDF rỗng hoặc không hợp lệ!",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            try
            {
                _pdfStream = new MemoryStream(pdfBytes);
                pdfViewer1.LoadDocument(_pdfStream);

                // Thiết lập zoom để hiển thị toàn bộ trang
                pdfViewer1.ZoomMode = DevExpress.XtraPdfViewer.PdfZoomMode.PageLevel;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(
                    $"Lỗi khi tải PDF: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Thiết lập các sự kiện
        /// </summary>
        private void SetupEvents()
        {
            //// Sự kiện khi document load xong
            //pdfViewer1.DocumentLoaded += PdfViewer1_DocumentLoaded;

            //// Sự kiện khi thay đổi trang
            //pdfViewer1.PageChanged += PdfViewer1_PageChanged;
        }

        /// <summary>
        /// Xử lý khi PDF load xong
        /// </summary>
        private void PdfViewer1_DocumentLoaded(object sender, EventArgs e)
        {
            UpdateTitle();
        }

        /// <summary>
        /// Xử lý khi đổi trang
        /// </summary>
        private void PdfViewer1_PageChanged(object sender, EventArgs e)
        {
            UpdateTitle();
        }

        /// <summary>
        /// Cập nhật tiêu đề form
        /// </summary>
        private void UpdateTitle()
        {
            //if (pdfViewer1.Document != null && pdfViewer1.Document.PageCount > 0)
            //{
            //    this.Text = $"Trình xem PDF - {_currentFileName} (Trang {pdfViewer1.CurrentPageNumber}/{pdfViewer1.PageCount})";
            //}
            //else
            //{
            //    this.Text = $"Trình xem PDF - {_currentFileName}";
            //}
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try
            {
                _pdfStream?.Dispose();
                _pdfStream = null;
            }
            catch { }

            base.OnFormClosed(e);
        }

        /// <summary>
        /// Export PDF sang file (nếu cần)
        /// </summary>
        public bool ExportToFile(string filePath)
        {
            try
            {
                if (_pdfStream != null)
                {
                    _pdfStream.Position = 0;
                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        _pdfStream.CopyTo(fs);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(
                    $"Lỗi khi xuất file: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
