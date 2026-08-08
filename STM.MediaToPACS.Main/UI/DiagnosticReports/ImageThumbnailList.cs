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

            /// <summary>
            /// Đã đẩy thành công vào docs hay chưa - chỉ do đồng bộ docs (Lưu nháp) đặt lại,
            /// KHÔNG tự đổi theo checkbox tick/bỏ tick (checkbox chỉ dùng để chọn ảnh sẽ đẩy).
            /// </summary>
            public bool DocPushed { get; set; }

            internal Panel Container { get; set; }
            internal Label PacsBadge { get; set; }
            internal Label DocBadge { get; set; }
            internal CheckBox DocumentCheckBox { get; set; }
            internal Button DeleteButton { get; set; }
        }

        private const int ThumbSize = 110;
        private const int BorderThickness = 3;
        private const int BadgeSize = 18;

        /// <summary>Bề ngang tối đa của ô xem trước khi hover - chiều cao tự tính theo tỷ lệ ảnh.</summary>
        private const int PreviewMaxWidth = 480;
        private const int PreviewGap = 8;

        private readonly FlowLayoutPanel _flow;
        private readonly ToolTip _toolTip = new ToolTip();
        private readonly List<ThumbnailItem> _items = new List<ThumbnailItem>();
        private readonly ContextMenuStrip _itemMenu = new ContextMenuStrip();
        private readonly ContextMenuStrip _listMenu = new ContextMenuStrip();
        private ThumbnailItem _contextItem;
        private bool _updatingDocumentCheckBox;
        private readonly PreviewWindow _previewWindow = new PreviewWindow();
        private ThumbnailItem _previewItem;

        public event EventHandler ItemAdded;
        public event EventHandler SelectionChanged;
        public delegate void ThumbnailItemEventHandler(object sender, ThumbnailItem item);
        public event ThumbnailItemEventHandler DeleteRequested;
        public event EventHandler DeleteAllRequested;

        public bool ReadOnly { get; private set; }

        /// <summary>Bật/tắt ô xem trước ảnh khi rê chuột (hover) - một số người dùng thấy phiền nên có thể tắt.</summary>
        public bool HoverPreviewEnabled { get; set; } = true;

        public ImageThumbnailList()
        {
            // FlowLayoutPanel tự hỗ trợ AutoScroll ngang sẵn (WrapContents=false + AutoScroll=true)
            // - dùng thẳng cơ chế cuộn gốc của Windows, bỏ hẳn kiểu tự tính Width/Left/Maximum thủ
            // công (từng gây kéo không ăn, layout lệch, kích thước ảnh gấp đôi mỗi lần thêm ảnh).
            _flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = SystemColors.AppWorkspace,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            Controls.Add(_flow);

            _flow.Resize += (s, e) => ResizeThumbnailItems();
            // Cuộn/đổi kích thước làm ô xem trước lệch vị trí - đóng lại cho gọn
            _flow.Resize += (s, e) => HidePreview();
            _flow.Scroll += (s, e) => HidePreview();
            VisibleChanged += (s, e) => { if (!Visible) HidePreview(); };
            _flow.MouseUp += ShowListMenu;

            _itemMenu.Items.Add("Xoa anh", null, (s, e) =>
            {
                if (_contextItem != null && !ReadOnly)
                    DeleteRequested?.Invoke(this, _contextItem);
            });
            _itemMenu.Items.Add("Xoa tat ca anh", null, (s, e) =>
            {
                if (!ReadOnly)
                    DeleteAllRequested?.Invoke(this, EventArgs.Empty);
            });
            _itemMenu.Items.Add(new ToolStripSeparator());

            _listMenu.Items.Add("Xoa tat ca anh", null, (s, e) =>
            {
                if (!ReadOnly)
                    DeleteAllRequested?.Invoke(this, EventArgs.Empty);
            });

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

        public void ClearImages()
        {
            HidePreview();
            foreach (var item in _items)
            {
                DisposeItemContainer(item);
            }

            _items.Clear();
            _flow.Controls.Clear();
            ItemAdded?.Invoke(this, EventArgs.Empty);
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SetReadOnly(bool readOnly)
        {
            ReadOnly = readOnly;
            foreach (var item in _items)
                UpdateInteractiveState(item);
        }

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

        /// <summary>Cuộn tới ảnh cuối cùng - gọi sau khi thêm hàng loạt ảnh bằng
        /// <see cref="TryAddImage"/> với scrollToEnd=false (mỗi item tự cuộn giữa chừng khi thêm
        /// hàng loạt gây giật/lệch vị trí cuộn cuối cùng).</summary>
        public void ScrollToLastItem() => ScrollToEnd();

        public bool TryAddImage(string filePath, out ThumbnailItem item, bool isChecked = false, bool scrollToEnd = true)
        {
            item = null;
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return false;

            var normalizedPath = NormalizePath(filePath);
            var existing = _items.FirstOrDefault(i =>
                string.Equals(NormalizePath(i.FilePath), normalizedPath, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                item = existing;
                if (isChecked)
                    SetDocumentSelected(existing, true);
                return true;
            }

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
                Margin = new Padding(5),
                Padding = new Padding(BorderThickness),
                Tag = item
            };
            container.Size = GetContainerSize(container.Margin);

            // Nhãn "P" (đã gán PACS, góc phải dưới) và "D" (đã tải lên docs, góc trái dưới) -
            // nhỏ gọn như checkbox/nút xoá ở 2 góc trên, không che khuất ảnh.
            var pacsBadge = new Label
            {
                Width = BadgeSize,
                Height = BadgeSize,
                Text = "P",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Tahoma", 8F, FontStyle.Bold),
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                Visible = false,
                Tag = item
            };

            var docBadge = new Label
            {
                Width = BadgeSize,
                Height = BadgeSize,
                Text = "D",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Tahoma", 8F, FontStyle.Bold),
                BackColor = Color.FromArgb(55, 65, 81),
                ForeColor = Color.White,
                Visible = false,
                Tag = item
            };

            var documentCheckBox = new CheckBox
            {
                Width = 18,
                Height = 18,
                Checked = isChecked,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Standard,
                Tag = item
            };

            var deleteButton = new Button
            {
                Width = 18,
                Height = 18,
                Text = "X",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(220, 38, 38),
                ForeColor = Color.White,
                Font = new Font("Tahoma", 6F, FontStyle.Bold),
                TabStop = false,
                Tag = item
            };
            deleteButton.FlatAppearance.BorderSize = 0;

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
            documentCheckBox.CheckedChanged += (s, e) =>
            {
                if (_updatingDocumentCheckBox)
                    return;

                thumbnailItem.Checked = documentCheckBox.Checked;
                thumbnailItem.DocumentSelected = documentCheckBox.Checked;
                UpdateVisualState(thumbnailItem);
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            };
            deleteButton.Click += (s, e) =>
            {
                if (!ReadOnly)
                    DeleteRequested?.Invoke(this, thumbnailItem);
            };
            pictureBox.MouseUp += (s, e) => ShowItemMenu(thumbnailItem, e);
            container.MouseUp += (s, e) => ShowItemMenu(thumbnailItem, e);
            pacsBadge.MouseUp += (s, e) => ShowItemMenu(thumbnailItem, e);
            docBadge.MouseUp += (s, e) => ShowItemMenu(thumbnailItem, e);
            documentCheckBox.MouseUp += (s, e) => ShowItemMenu(thumbnailItem, e);
            deleteButton.MouseUp += (s, e) => ShowItemMenu(thumbnailItem, e);

            AttachPreviewHandlers(thumbnailItem, container, pictureBox, pacsBadge, docBadge, documentCheckBox, deleteButton);

            container.Controls.Add(pictureBox);
            pictureBox.Controls.Add(pacsBadge);
            pictureBox.Controls.Add(docBadge);
            pictureBox.Controls.Add(documentCheckBox);
            pictureBox.Controls.Add(deleteButton);
            pacsBadge.BringToFront();
            docBadge.BringToFront();
            documentCheckBox.BringToFront();
            deleteButton.BringToFront();
            item.Container = container;
            item.PacsBadge = pacsBadge;
            item.DocBadge = docBadge;
            item.DocumentCheckBox = documentCheckBox;
            item.DeleteButton = deleteButton;

            ResizeThumbnailItems();
            LayoutPacsBadge(item);
            LayoutDocBadge(item);
            LayoutDocumentCheckBox(item);
            LayoutDeleteButton(item);
            UpdateVisualState(item);

            _items.Add(item);
            _flow.Controls.Add(container);
            UpdateInteractiveState(item);
            if (scrollToEnd)
                ScrollToEnd();

            ItemAdded?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public ThumbnailItem FindByAttachmentId(Guid attachmentId)
        {
            if (attachmentId == Guid.Empty)
                return null;

            return _items.FirstOrDefault(i => i.AttachmentId.HasValue && i.AttachmentId.Value == attachmentId);
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
            SetDocumentCheckBoxValue(item, selected);
            UpdateVisualState(item);
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Đặt trực tiếp trạng thái đã đẩy docs cho một ảnh (dùng khi nạp lại từ server).</summary>
        public void SetDocPushed(ThumbnailItem item, bool pushed)
        {
            if (item == null || item.DocPushed == pushed)
                return;

            item.DocPushed = pushed;
            UpdateVisualState(item);
        }

        /// <summary>
        /// Cập nhật badge "D" sau khi đồng bộ docs thành công: các ảnh có attachment id nằm trong
        /// <paramref name="pushedAttachmentIds"/> được đánh dấu đã đẩy docs, ảnh nào trước đó có
        /// dấu nhưng lần này không còn trong danh sách (bị bỏ tích rồi đồng bộ lại) thì bỏ dấu.
        /// </summary>
        public void ApplyDocPushResult(IEnumerable<Guid> pushedAttachmentIds)
        {
            var pushedSet = new HashSet<Guid>(pushedAttachmentIds ?? Enumerable.Empty<Guid>());
            foreach (var item in _items)
            {
                bool shouldBePushed = item.AttachmentId.HasValue && pushedSet.Contains(item.AttachmentId.Value);
                if (item.DocPushed == shouldBePushed)
                    continue;

                item.DocPushed = shouldBePushed;
                UpdateVisualState(item);
            }
        }

        public void SetPacsSelected(ThumbnailItem item, bool selected)
        {
            if (item == null)
                return;

            item.PacsSelected = selected;
            UpdateVisualState(item);
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Tự động gán PACS cho các ảnh JPEG đang được tick checkbox mà chưa gán PACS -
        /// dùng khi bấm "Đẩy PACS" để gộp bước "chọn PACS" + "đẩy" làm một, khỏi cần chuột phải chọn PACS riêng.
        /// Trả về true nếu có ít nhất 1 ảnh vừa được gán.
        /// </summary>
        /// <summary>
        /// Đồng bộ TOÀN PHẦN cờ PacsSelected theo đúng checkbox đang tick tại thời điểm gọi:
        /// ảnh JPEG nào đang tick thì bật, ảnh nào không tick (kể cả đã bật từ trước - do lần lưu
        /// trước hoặc do server trả sẵn destination) thì tắt. Không tính lịch sử/tồn đọng.
        /// PacsStatus/ErrorDetail (kết quả đẩy thật) giữ nguyên - không xoá theo, vì đó là dấu vết
        /// đã đẩy thành công/thất bại trước đó, độc lập với đang tick hay không.
        /// </summary>
        public bool SyncPacsSelectionWithCheckedItems()
        {
            bool changedAny = false;
            foreach (var item in _items)
            {
                bool shouldBeSelected = (item.Checked || item.DocumentSelected) && IsJpeg(item);
                if (item.PacsSelected == shouldBeSelected)
                    continue;

                item.PacsSelected = shouldBeSelected;
                UpdateVisualState(item);
                changedAny = true;
            }

            if (changedAny)
                SelectionChanged?.Invoke(this, EventArgs.Empty);

            return changedAny;
        }

        private void TogglePacsSelected(ThumbnailItem item)
        {
            if (ReadOnly)
                return;

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

        #region Xem trước ảnh khi hover

        /// <summary>
        /// Rê chuột vào ô ảnh (kể cả các nút nhỏ nằm đè lên ảnh) thì bật ô xem trước;
        /// rời hẳn khỏi ô ảnh mới đóng lại - đi từ ảnh sang checkbox/nút xoá vẫn giữ nguyên.
        /// </summary>
        private void AttachPreviewHandlers(ThumbnailItem item, params Control[] controls)
        {
            foreach (var control in controls)
            {
                if (control == null)
                    continue;

                control.MouseEnter += (s, e) => ShowPreview(item);
                control.MouseLeave += (s, e) => HidePreviewIfPointerLeft(item);
            }
        }

        private void ShowPreview(ThumbnailItem item)
        {
            if (!HoverPreviewEnabled)
                return;

            if (item == null || item.Container == null || item.Container.IsDisposed)
                return;

            if (string.IsNullOrWhiteSpace(item.FilePath) || !File.Exists(item.FilePath))
                return;

            if (_previewItem == item && _previewWindow.Visible)
                return;

            Image image;
            try
            {
                // Đọc qua stream + copy sang Bitmap để không giữ khoá file gốc
                using (var stream = new FileStream(item.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var original = Image.FromStream(stream))
                {
                    image = new Bitmap(original);
                }
            }
            catch (Exception)
            {
                HidePreview();
                return;
            }

            _previewItem = item;
            _previewWindow.SetImage(image);
            _previewWindow.Size = CalculatePreviewSize(image.Size);
            _previewWindow.Location = CalculatePreviewLocation(item, _previewWindow.Size);

            if (!_previewWindow.Visible)
            {
                var owner = FindForm();
                if (owner != null && !owner.IsDisposed && _previewWindow.Owner != owner)
                    _previewWindow.Owner = owner;

                _previewWindow.Show();
            }
        }

        private void HidePreviewIfPointerLeft(ThumbnailItem item)
        {
            if (item?.Container == null || item.Container.IsDisposed)
            {
                HidePreview();
                return;
            }

            // MouseLeave bắn cả khi chuột chỉ đi từ ảnh sang control con nằm đè lên ảnh
            var bounds = item.Container.RectangleToScreen(item.Container.ClientRectangle);
            if (bounds.Contains(Cursor.Position))
                return;

            HidePreview();
        }

        private void HidePreview()
        {
            _previewItem = null;

            if (_previewWindow.IsDisposed)
                return;

            if (_previewWindow.Visible)
                _previewWindow.Hide();

            _previewWindow.SetImage(null);
        }

        /// <summary>
        /// Chốt bề ngang tối đa <see cref="PreviewMaxWidth"/> (không phóng to quá ảnh gốc),
        /// chiều dọc tính theo đúng tỷ lệ ảnh. Ảnh dạng dọc mà cao quá màn hình thì
        /// thu lại theo chiều cao - vẫn giữ nguyên tỷ lệ.
        /// </summary>
        private static Size CalculatePreviewSize(Size imageSize)
        {
            var screen = Screen.FromPoint(Cursor.Position).WorkingArea;
            int maxWidth = Math.Max(120, Math.Min(PreviewMaxWidth, screen.Width - PreviewGap * 4));
            int maxHeight = Math.Max(120, screen.Height - PreviewGap * 4);

            if (imageSize.Width <= 0 || imageSize.Height <= 0)
                return new Size(maxWidth, maxWidth);

            int width = Math.Min(maxWidth, imageSize.Width);
            int height = (int)Math.Round(width * (double)imageSize.Height / imageSize.Width);

            if (height > maxHeight)
            {
                height = maxHeight;
                width = (int)Math.Round(height * (double)imageSize.Width / imageSize.Height);
            }

            return new Size(Math.Max(1, width), Math.Max(1, height));
        }

        /// <summary>Ưu tiên hiện phía trên ô ảnh, không đủ chỗ thì xuống dưới; luôn nằm gọn trong màn hình.</summary>
        private static Point CalculatePreviewLocation(ThumbnailItem item, Size previewSize)
        {
            var anchor = item.Container.RectangleToScreen(item.Container.ClientRectangle);
            var screen = Screen.FromRectangle(anchor).WorkingArea;

            int x = anchor.Left + (anchor.Width - previewSize.Width) / 2;
            int y = anchor.Top - PreviewGap - previewSize.Height;
            if (y < screen.Top)
                y = anchor.Bottom + PreviewGap;

            x = Math.Max(screen.Left, Math.Min(x, screen.Right - previewSize.Width));
            y = Math.Max(screen.Top, Math.Min(y, screen.Bottom - previewSize.Height));
            return new Point(x, y);
        }

        /// <summary>
        /// Cửa sổ xem trước: không viền, không nhận focus (WS_EX_NOACTIVATE) để rê chuột
        /// không cướp con trỏ khỏi ô đang nhập bên phiếu kết luận.
        /// </summary>
        private sealed class PreviewWindow : Form
        {
            private const int WS_EX_NOACTIVATE = 0x08000000;
            private const int WS_EX_TOOLWINDOW = 0x00000080;

            private readonly PictureBox _picture;

            public PreviewWindow()
            {
                FormBorderStyle = FormBorderStyle.None;
                ShowInTaskbar = false;
                StartPosition = FormStartPosition.Manual;
                BackColor = Color.FromArgb(55, 65, 81);
                Padding = new Padding(1);

                _picture = new PictureBox
                {
                    Dock = DockStyle.Fill,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Black
                };
                Controls.Add(_picture);
            }

            protected override bool ShowWithoutActivation => true;

            protected override CreateParams CreateParams
            {
                get
                {
                    var cp = base.CreateParams;
                    cp.ExStyle |= WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW;
                    return cp;
                }
            }

            public void SetImage(Image image)
            {
                var previous = _picture.Image;
                _picture.Image = image;
                previous?.Dispose();
            }
        }

        #endregion

        private void ShowItemMenu(ThumbnailItem item, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || item == null)
                return;

            HidePreview();
            _contextItem = item;
            _itemMenu.Items[0].Enabled = !ReadOnly;
            _itemMenu.Items[1].Enabled = !ReadOnly && _items.Count > 0;
            _itemMenu.Items[3].Enabled = !ReadOnly;
            _itemMenu.Items[4].Enabled = !ReadOnly && (item.PacsSelected || IsJpeg(item));
            _itemMenu.Show(item.Container, item.Container.PointToClient(Cursor.Position));
        }

        private void ShowListMenu(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            _listMenu.Items[0].Enabled = !ReadOnly && _items.Count > 0;
            _listMenu.Show(this, PointToClient(Cursor.Position));
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
            if (ReadOnly)
                return;

            item.Checked = !item.Checked;
            item.DocumentSelected = item.Checked;
            SetDocumentCheckBoxValue(item, item.Checked);
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

            if (item.PacsBadge == null || item.DocBadge == null)
                return;

            LayoutPacsBadge(item);
            LayoutDocBadge(item);
            LayoutDocumentCheckBox(item);
            LayoutDeleteButton(item);
            SetDocumentCheckBoxValue(item, item.DocumentSelected || item.Checked);

            // "P" chỉ hiện khi đã có kết quả đẩy PACS thực sự (server trả về status) - không
            // hiện chỉ vì ảnh đang được đánh dấu chọn PACS mà chưa đẩy.
            item.PacsBadge.Visible = !string.IsNullOrWhiteSpace(item.PacsStatus);
            item.PacsBadge.BackColor = GetStatusBackColor(item);
            _toolTip.SetToolTip(item.PacsBadge, item.ErrorDetail);

            // "D" chỉ hiện khi ảnh đã đẩy thành công vào docs (DocPushed do đồng bộ docs đặt) -
            // không tự đổi theo checkbox tick/bỏ tick, checkbox chỉ dùng để chọn ảnh sẽ đẩy.
            item.DocBadge.Visible = item.DocPushed;

            UpdateInteractiveState(item);
        }

        private void UpdateInteractiveState(ThumbnailItem item)
        {
            if (item == null)
                return;

            if (item.DocumentCheckBox != null)
                item.DocumentCheckBox.Enabled = !ReadOnly;
            if (item.DeleteButton != null)
                item.DeleteButton.Visible = !ReadOnly;
        }

        private void ResizeThumbnailItems()
        {
            foreach (var item in _items)
            {
                if (item.Container != null)
                {
                    item.Container.Size = GetContainerSize(item.Container.Margin);
                    LayoutPacsBadge(item);
                    LayoutDocBadge(item);
                    LayoutDocumentCheckBox(item);
                    LayoutDeleteButton(item);
                }
            }
        }

        private void SetDocumentCheckBoxValue(ThumbnailItem item, bool selected)
        {
            if (item?.DocumentCheckBox == null)
                return;

            _updatingDocumentCheckBox = true;
            try
            {
                item.DocumentCheckBox.Checked = selected;
            }
            finally
            {
                _updatingDocumentCheckBox = false;
            }
        }

        private Size GetContainerSize(Padding margin)
        {
            // Dùng _flow.Height (kích thước ngoài, không đổi) thay vì ClientSize.Height (co lại khi
            // thanh cuộn ngang hiện ra) - nếu không, kích thước ảnh sẽ phụ thuộc ngược vào chính
            // trạng thái thanh cuộn, gây vòng lặp: ảnh to -> tràn -> hiện scroll -> ảnh nhỏ lại ->
            // hết tràn -> ẩn scroll -> ảnh to lại -> tràn... khiến bề rộng nội dung tính sai mỗi lần
            // thêm ảnh (chụp nhanh).
            var availableHeight = _flow.Height
                - SystemInformation.HorizontalScrollBarHeight
                - margin.Vertical
                - 2;

            var thumbSize = Math.Max(
                48,
                Math.Min(ThumbSize, availableHeight));

            return new Size(thumbSize, thumbSize);
        }

        // FlowLayoutPanel.AutoScroll tự tính vùng cuộn theo control con - cuộn tới ảnh cuối chỉ cần
        // gọi ScrollControlIntoView (API chuẩn cho control có AutoScroll), không cần tự tính Width/Left.
        private void ScrollToEnd()
        {
            if (_flow.Controls.Count == 0)
                return;

            _flow.ScrollControlIntoView(_flow.Controls[_flow.Controls.Count - 1]);
        }

        /// <summary>Nhãn "D" (đã tải lên docs) neo góc trái dưới ảnh.</summary>
        private void LayoutDocBadge(ThumbnailItem item)
        {
            if (item?.Container == null || item.DocBadge == null)
                return;

            var parentHeight = item.DocBadge.Parent?.ClientSize.Height ?? item.Container.ClientSize.Height;
            item.DocBadge.Location = new Point(0, Math.Max(0, parentHeight - item.DocBadge.Height));
        }

        /// <summary>Nhãn "P" (đã gán/đẩy PACS) neo góc phải dưới ảnh.</summary>
        private void LayoutPacsBadge(ThumbnailItem item)
        {
            if (item?.Container == null || item.PacsBadge == null)
                return;

            var parent = item.PacsBadge.Parent;
            var parentWidth = parent?.ClientSize.Width ?? item.Container.ClientSize.Width;
            var parentHeight = parent?.ClientSize.Height ?? item.Container.ClientSize.Height;
            item.PacsBadge.Location = new Point(
                Math.Max(0, parentWidth - item.PacsBadge.Width),
                Math.Max(0, parentHeight - item.PacsBadge.Height));
        }

        private void LayoutDocumentCheckBox(ThumbnailItem item)
        {
            if (item?.Container == null || item.DocumentCheckBox == null)
                return;

            item.DocumentCheckBox.Location = Point.Empty;
        }

        private void LayoutDeleteButton(ThumbnailItem item)
        {
            if (item?.Container == null || item.DeleteButton == null)
                return;

            var parentWidth = item.DeleteButton.Parent?.ClientSize.Width ?? item.Container.ClientSize.Width;
            item.DeleteButton.Location = new Point(
                Math.Max(0, parentWidth - item.DeleteButton.Width),
                0);
        }

        public bool RemoveItem(ThumbnailItem item)
        {
            if (item == null || !_items.Contains(item))
                return false;

            if (_previewItem == item)
                HidePreview();

            // Xoá xong giữ nguyên vị trí đang xem quanh ảnh vừa xoá (ảnh liền trước, hoặc ảnh liền
            // sau nếu xoá ảnh đầu) - không để FlowLayoutPanel tự nhảy cuộn về đầu danh sách.
            var removedIndex = _items.IndexOf(item);

            _items.Remove(item);
            if (item.Container != null)
            {
                _flow.Controls.Remove(item.Container);
                DisposeItemContainer(item);
            }

            var neighborIndex = Math.Min(removedIndex, _items.Count - 1);
            if (neighborIndex >= 0)
                _flow.ScrollControlIntoView(_items[neighborIndex].Container);

            ItemAdded?.Invoke(this, EventArgs.Empty);
            SelectionChanged?.Invoke(this, EventArgs.Empty);
            return true;
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

        private static string NormalizePath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return string.Empty;

            try
            {
                return Path.GetFullPath(filePath);
            }
            catch
            {
                return filePath;
            }
        }

        public void ClearAll()
        {
            HidePreview();
            foreach (var item in _items)
            {
                DisposeItemContainer(item);
            }

            _items.Clear();
            _flow.Controls.Clear();
            ItemAdded?.Invoke(this, EventArgs.Empty);
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_previewWindow.IsDisposed)
            {
                _previewWindow.SetImage(null);
                _previewWindow.Dispose();
            }

            base.Dispose(disposing);
        }

        private static void DisposeItemContainer(ThumbnailItem item)
        {
            if (item?.Container == null)
                return;

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
}
