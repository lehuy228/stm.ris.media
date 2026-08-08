using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Serilog;

namespace STM.MediaToPACS.Main.UI
{
    /// <summary>
    /// Form nhúng WebView2 (Chromium thật, qua Microsoft Edge WebView2 Runtime) để hiện link
    /// DICOM viewer ngay trong app, thay vì mở trình duyệt ngoài. Không modal - bác sĩ có thể
    /// xem ảnh song song với việc nhập kết luận ở form chính.
    /// </summary>
    public class FormViewerBrowser : Form
    {
        private readonly WebView2 _webView;
        private readonly string _launchUrl;

        public FormViewerBrowser(string title, string launchUrl)
        {
            _launchUrl = launchUrl;

            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            WindowState = FormWindowState.Maximized;
            Size = new Size(1280, 800);

            _webView = new WebView2 { Dock = DockStyle.Fill };
            Controls.Add(_webView);

            Load += FormViewerBrowser_Load;
            FormClosed += (s, e) => Dispose();
        }

        private async void FormViewerBrowser_Load(object sender, EventArgs e)
        {
            try
            {
                await _webView.EnsureCoreWebView2Async(null);
                _webView.CoreWebView2.Navigate(_launchUrl);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Không khởi tạo được WebView2 để mở viewer PACS");
                MessageBox.Show(
                    this,
                    "Không khởi tạo được trình duyệt nhúng (WebView2).\n" +
                    "Vui lòng kiểm tra máy đã cài Microsoft Edge WebView2 Runtime chưa.\n\n" +
                    "Chi tiết lỗi: " + ex.Message,
                    "Lỗi mở viewer",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Close();
            }
        }
    }
}
