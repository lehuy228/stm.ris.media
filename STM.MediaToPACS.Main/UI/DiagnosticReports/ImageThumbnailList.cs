using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace STM.MediaToPACS.Main.UI.DiagnosticReports
{
    /// <summary>
    /// Lightweight thumbnail list for diagnostic-report images. It keeps local file state and
    /// server attachment metadata so later flows can sync document/PACS selections without
    /// depending on Leadtools image collections.
    /// </summary>
    public class ImageThumbnailList : UserControl
    {
        public class ThumbnailItem
        {
            public Guid? AttachmentId { get; set; }
            public string FilePath { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }
            public bool Checked { get; set; }
            public bool DocumentSelected { get; set; }
            public bool PacsSelected { get; set; }
            public string PacsStatus { get; set; }
            public string ErrorDetail { get; set; }

            internal Panel Container { get; set; }
            internal Label StatusLabel { get; set; }
        }

        private const int ThumbSize = 110;
        private const int BorderThickness = 3;
        private const int StatusHeight = 18;

        private readonly FlowLayoutPanel _flow;
        private readonly ToolTip _toolTip = new ToolTip();
        private readonly List<ThumbnailItem> _items = new List<ThumbnailItem>();
        private readonly ContextMenuStrip _itemMenu = new ContextMenuStrip();
        private ThumbnailItem _contextItem;

        public event EventHandler ItemAdded;
        public event EventHandler SelectionChanged;

        public ImageThumbnailList()
        {
            _flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = SystemColors.AppWorkspace
            };
            Controls.Add(_flow);

            _itemMenu.Items.Add("Chọn/Bỏ chọn ảnh PDF", null, (s, e) =>
            {
                if (_contextItem != null)
                    ToggleDocumentSelected(_contextItem);
            });
            _itemMenu.Items.Add("Chọn/Bỏ chọn PACS", null, (s, e) =>
            {
                if (_contextItem != null)
                    TogglePacsSelected(_contextItem);
            });
        }

        public IReadOnlyList<ThumbnailItem> Items => _items;

        public List<string> GetCheckedFilePaths()
            => _items.Where(i => i.Checked).Select(i => i.FilePath).ToList();

        public List<ThumbnailItem> GetDocumentSelectedItems()
            => _items.Where(i => i.DocumentSelected || i.Checked).ToList();

        public List<Guid> GetDocumentSelectedAttachmentIds()
            => GetDocumentSelectedItems()
                .Where(i => i.AttachmentId.HasValue)
                .Select(i => i.AttachmentId.Value)
                .ToList();

        public List<Guid> GetPacsSelectedAttachmentIds()
            => _items
                .Where(i => i.PacsSelected && i.AttachmentId.HasValue)
                .Select(i => i.AttachmentId.Value)
                .ToList();

        public List<ThumbnailItem> GetPendingUploadItems()
            => _items
                .Where(i => !i.AttachmentId.HasValue && !string.IsNullOrWhiteSpace(i.FilePath))
                .ToList();

        public bool AddImage(string filePath, bool isChecked = false)
            => TryAddImage(filePath, out _, isChecked);

        public bool TryAddImage(string filePath, out ThumbnailItem item, bool isChecked = false)
        {
            item = null;
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return false;

            Bitmap thumbnail;
            try
            {
                using (var original = Image.FromFile(filePath))
                {
                    thumbnail = new Bitmap(original, new Size(ThumbSize, ThumbSize));
                }
            }
            catch (Exception)
            {
                return false;
            }

            item = new ThumbnailItem
            {
                FilePath = filePath,
                FileName = Path.GetFileName(filePath),
                ContentType = GuessContentType(filePath),
                Checked = isChecked,
                DocumentSelected = isChecked
            };

            var container = new Panel
            {
                Size = new Size(
                    ThumbSize + BorderThickness * 2,
                    ThumbSize + BorderThickness * 2 + StatusHeight),
                Margin = new Padding(5),
                Padding = new Padding(BorderThickness),
                Tag = item
            };

            var statusLabel = new Label
            {
                Dock = DockStyle.Bottom,
                Height = StatusHeight,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Tahoma", 7.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(55, 65, 81),
                ForeColor = Color.White,
                Visible = false,
                Tag = item
            };

            var pictureBox = new PictureBox
            {
                Image = thumbnail,
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill,
                Cursor = Cursors.Hand,
                BackColor = Color.Black,
                Tag = item
            };

            var thumbnailItem = item;
            pictureBox.Click += (s, e) => ToggleDocumentSelected(thumbnailItem);
            container.Click += (s, e) => ToggleDocumentSelected(thumbnailItem);
            statusLabel.Click += (s, e) => ToggleDocumentSelected(thumbnailItem);
            pictureBox.MouseUp += (s, e) => ShowItemMenu(thumbnailItem, e);
            container.MouseUp += (s, e) => ShowItemMenu(thumbnailItem, e);
            statusLabel.MouseUp += (s, e) => ShowItemMenu(thumbnailItem, e);

            container.Controls.Add(pictureBox);
            container.Controls.Add(statusLabel);
            item.Container = container;
            item.StatusLabel = statusLabel;

            UpdateVisualState(item);

            _items.Add(item);
            _flow.Controls.Add(container);

            ItemAdded?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public void SetAttachmentMetadata(
            ThumbnailItem item,
            Guid attachmentId,
            string fileName = null,
            string contentType = null)
        {
            if (item == null)
                return;

            item.AttachmentId = attachmentId;
            if (!string.IsNullOrWhiteSpace(fileName))
                item.FileName = fileName;
            if (!string.IsNullOrWhiteSpace(contentType))
                item.ContentType = contentType;

            UpdateVisualState(item);
        }

        public void SetDocumentSelected(ThumbnailItem item, bool selected)
        {
            if (item == null)
                return;

            item.Checked = selected;
            item.DocumentSelected = selected;
            UpdateVisualState(item);
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SetPacsSelected(ThumbnailItem item, bool selected)
        {
            if (item == null)
                return;

            item.PacsSelected = selected;
            UpdateVisualState(item);
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        private void TogglePacsSelected(ThumbnailItem item)
        {
            if (item == null)
                return;

            if (!item.PacsSelected && !IsJpeg(item))
            {
                MessageBox.Show(
                    this,
                    "Hiện tại chỉ hỗ trợ đẩy PACS với ảnh JPEG.",
                    "Không thể chọn PACS",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            item.PacsSelected = !item.PacsSelected;
            if (!item.PacsSelected)
            {
                item.PacsStatus = null;
                item.ErrorDetail = null;
            }

            UpdateVisualState(item);
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ShowItemMenu(ThumbnailItem item, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || item == null)
                return;

            _contextItem = item;
            _itemMenu.Items[1].Enabled = item.PacsSelected || IsJpeg(item);
            _itemMenu.Show(item.Container, item.Container.PointToClient(Cursor.Position));
        }

        public void SetPacsStatus(ThumbnailItem item, string status, string errorDetail = null)
        {
            if (item == null)
                return;

            item.PacsStatus = status;
            item.ErrorDetail = errorDetail;
            UpdateVisualState(item);
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ToggleDocumentSelected(ThumbnailItem item)
        {
            item.Checked = !item.Checked;
            item.DocumentSelected = item.Checked;
            UpdateVisualState(item);
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateVisualState(ThumbnailItem item)
        {
            if (item == null || item.Container == null)
                return;

            item.Container.BackColor = item.DocumentSelected || item.Checked
                ? Color.LimeGreen
                : Color.Transparent;

            if (item.StatusLabel == null)
                return;

            var statusText = BuildStatusText(item);
            item.StatusLabel.Text = statusText;
            item.StatusLabel.Visible = !string.IsNullOrWhiteSpace(statusText);
            item.StatusLabel.BackColor = GetStatusBackColor(item);
            item.StatusLabel.ForeColor = Color.White;
            _toolTip.SetToolTip(item.StatusLabel, item.ErrorDetail);
        }

        private static string BuildStatusText(ThumbnailItem item)
        {
            if (!string.IsNullOrWhiteSpace(item.PacsStatus))
                return "PACS: " + item.PacsStatus;

            return item.PacsSelected ? "PACS" : null;
        }

        private static Color GetStatusBackColor(ThumbnailItem item)
        {
            if (string.Equals(item.PacsStatus, "completed", StringComparison.OrdinalIgnoreCase))
                return Color.FromArgb(22, 128, 61);

            if (string.Equals(item.PacsStatus, "failed", StringComparison.OrdinalIgnoreCase))
                return Color.FromArgb(185, 28, 28);

            if (item.PacsSelected || !string.IsNullOrWhiteSpace(item.PacsStatus))
                return Color.FromArgb(37, 99, 235);

            return Color.FromArgb(55, 65, 81);
        }

        private static bool IsJpeg(ThumbnailItem item)
        {
            if (item == null)
                return false;

            if (string.Equals(item.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase))
                return true;

            var ext = Path.GetExtension(item.FilePath ?? item.FileName);
            return string.Equals(ext, ".jpg", StringComparison.OrdinalIgnoreCase)
                || string.Equals(ext, ".jpeg", StringComparison.OrdinalIgnoreCase);
        }

        private static string GuessContentType(string filePath)
        {
            var ext = Path.GetExtension(filePath);
            if (string.IsNullOrWhiteSpace(ext))
                return null;

            switch (ext.ToLowerInvariant())
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".bmp":
                    return "image/bmp";
                case ".webp":
                    return "image/webp";
                case ".mp4":
                    return "video/mp4";
                case ".mpeg":
                case ".mpg":
                    return "video/mpeg";
                default:
                    return null;
            }
        }

        public void ClearAll()
        {
            foreach (var item in _items)
            {
                if (item.Container != null)
                {
                    foreach (Control control in item.Container.Controls)
                    {
                        var pictureBox = control as PictureBox;
                        if (pictureBox != null)
                        {
                            pictureBox.Image?.Dispose();
                            pictureBox.Image = null;
                        }
                    }

                    item.Container.Controls.Clear();
                    item.Container.Dispose();
                }
            }

            _items.Clear();
            _flow.Controls.Clear();
        }
    }
}
