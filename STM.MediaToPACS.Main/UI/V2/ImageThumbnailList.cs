using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace STM.MediaToPACS.Main.UI.V2
{
    /// <summary>
    /// Danh sách ảnh thumbnail nhẹ dùng System.Drawing.Image (không Leadtools) - thay tạm cho
    /// ListImageBox (vốn dựng trên Leadtools RasterImage/RasterCodecs) trong FormMainV2.
    /// Chỉ hỗ trợ ảnh đơn (jpg/png/bmp...), KHÔNG hỗ trợ multi-page PDF/TIFF như ListImageBox cũ.
    /// Click vào ảnh để chọn/bỏ chọn (viền xanh lá = đã chọn, dùng khi lưu/in kết luận).
    /// </summary>
    public class ImageThumbnailList : UserControl
    {
        public class ThumbnailItem
        {
            public string FilePath { get; set; }
            public bool Checked { get; set; }
            internal Panel Container { get; set; }
        }

        private const int ThumbSize = 110;
        private const int BorderThickness = 3;

        private readonly FlowLayoutPanel _flow;
        private readonly List<ThumbnailItem> _items = new List<ThumbnailItem>();

        /// <summary>Bắn ra khi có 1 ảnh mới được thêm vào danh sách.</summary>
        public event EventHandler ItemAdded;

        /// <summary>Bắn ra khi người dùng chọn/bỏ chọn 1 ảnh.</summary>
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
        }

        public IReadOnlyList<ThumbnailItem> Items => _items;

        /// <summary>Đường dẫn file của các ảnh đang được chọn (dùng khi lưu/ký/in kết luận).</summary>
        public List<string> GetCheckedFilePaths()
            => _items.Where(i => i.Checked).Select(i => i.FilePath).ToList();

        /// <summary>Nạp 1 ảnh vào danh sách. Trả về false nếu file không tồn tại/không đọc được.</summary>
        public bool AddImage(string filePath, bool isChecked = false)
        {
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

            var item = new ThumbnailItem { FilePath = filePath, Checked = isChecked };

            var container = new Panel
            {
                Size = new Size(ThumbSize + BorderThickness * 2, ThumbSize + BorderThickness * 2),
                Margin = new Padding(5),
                Padding = new Padding(BorderThickness),
                BackColor = isChecked ? Color.LimeGreen : Color.Transparent,
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

            pictureBox.Click += (s, e) => ToggleChecked(item);
            container.Click += (s, e) => ToggleChecked(item);

            container.Controls.Add(pictureBox);
            item.Container = container;
            _items.Add(item);
            _flow.Controls.Add(container);

            ItemAdded?.Invoke(this, EventArgs.Empty);
            return true;
        }

        private void ToggleChecked(ThumbnailItem item)
        {
            item.Checked = !item.Checked;
            item.Container.BackColor = item.Checked ? Color.LimeGreen : Color.Transparent;
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Xoá toàn bộ ảnh khỏi danh sách (dùng khi đổi chỉ định/mở lại form).</summary>
        public void ClearAll()
        {
            foreach (var item in _items)
            {
                item.Container?.Controls.Clear();
                item.Container?.Dispose();
            }
            _items.Clear();
            _flow.Controls.Clear();
        }
    }
}
