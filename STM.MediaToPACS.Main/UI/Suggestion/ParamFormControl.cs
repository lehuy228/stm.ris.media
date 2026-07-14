using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MediaToPacs.Core.Models;
using MediaToPacs.Core.Models.Ketluan;
using Serilog;

namespace STM.MediaToPACS.Main.UI
{
    /// <summary>
    /// Sidebar form chỉ số động cho suggestion "Structured" (giao diện DevExpress):
    /// render paramGroups (lưới 4 cột theo colSpan/paramColumns/forceNewRow),
    /// input theo inputMode (value/checkbox/checkbox_value), tô màu giá trị theo ranges.
    /// Dock vào cạnh TRÁI ngoài cùng của form, thu gọn/mở rộng theo CHIỀU NGANG:
    /// thu gọn chỉ còn dải dọc hẹp có chữ xoay, click để mở lại.
    /// Mỗi lần giá trị thay đổi sẽ raise ParamValuesChanged để form chủ
    /// đồng bộ text sinh ra vào ô Mô tả.
    /// </summary>
    public class ParamFormControl : PanelControl
    {
        private const int HEADER_HEIGHT = 30;
        private const int COLLAPSED_WIDTH = 30;
        private const int GRID_COLUMNS = 4;
        private const int GROUP_CAPTION_HEIGHT = 30;

        private readonly PanelControl _headerPanel;
        private readonly LabelControl _headerLabel;
        private readonly PanelControl _collapsedStrip;
        private readonly Panel _contentPanel;
        private readonly TableLayoutPanel _grid;

        private bool _collapsed;
        private int _expandedWidth = 460;
        private string _title = "Chỉ số";
        // Chặn raise event trong lúc đang build form (preset value sẽ kích hoạt TextChanged)
        private bool _suspendEvents;

        // Danh sách control nhập liệu theo thứ tự render, dùng để sinh text
        private readonly List<ParamInputEntry> _entries = new List<ParamInputEntry>();

        private class ParamInputEntry
        {
            public ParamGroupFormDto Group;
            public ParamFieldDto Field;
            public CheckEdit CheckBox;  // null nếu inputMode = value
            public TextEdit ValueBox;   // null nếu inputMode = checkbox
        }

        /// <summary>Raise khi bác sĩ thay đổi bất kỳ giá trị nào trên form</summary>
        public event EventHandler ParamValuesChanged;

        /// <summary>Raise khi trạng thái thu gọn/mở rộng thay đổi (để form chủ ẩn/hiện splitter)</summary>
        public event EventHandler CollapsedChanged;

        /// <summary>true = đang thu gọn thành dải dọc hẹp</summary>
        public bool Collapsed
        {
            get { return _collapsed; }
        }

        public ParamFormControl()
        {
            BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            Width = _expandedWidth;
            // Font hiện đại, dễ đọc cho toàn bộ control con (thay Tahoma mặc định)
            Font = new Font("Segoe UI", 9.75F);

            _grid = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = GRID_COLUMNS
            };
            for (int i = 0; i < GRID_COLUMNS; i++)
            {
                _grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / GRID_COLUMNS));
            }

            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(4)
            };
            _contentPanel.Controls.Add(_grid);

            _headerLabel = new LabelControl
            {
                Dock = DockStyle.Fill,
                Text = "◄ Chỉ số",
                Padding = new Padding(8, 0, 0, 0),
                AutoSizeMode = LabelAutoSizeMode.None
            };
            _headerLabel.Appearance.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            _headerLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            _headerLabel.Appearance.Options.UseFont = true;
            _headerLabel.Appearance.Options.UseTextOptions = true;

            _headerPanel = new PanelControl
            {
                Dock = DockStyle.Top,
                Height = HEADER_HEIGHT,
                Cursor = Cursors.Hand,
                BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            };
            _headerPanel.Appearance.BackColor = Color.FromArgb(235, 240, 246);
            _headerPanel.Appearance.Options.UseBackColor = true;
            _headerPanel.Controls.Add(_headerLabel);
            _headerPanel.Click += (s, e) => ToggleCollapsed();
            _headerLabel.Click += (s, e) => ToggleCollapsed();

            // Dải dọc hẹp hiển thị khi thu gọn: vẽ mũi tên + chữ xoay dọc, click để mở lại
            _collapsedStrip = new PanelControl
            {
                Dock = DockStyle.Fill,
                Cursor = Cursors.Hand,
                Visible = false,
                BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            };
            _collapsedStrip.Appearance.BackColor = Color.FromArgb(235, 240, 246);
            _collapsedStrip.Appearance.Options.UseBackColor = true;
            _collapsedStrip.Paint += CollapsedStrip_Paint;
            _collapsedStrip.Click += (s, e) => ToggleCollapsed();

            Controls.Add(_contentPanel);
            Controls.Add(_headerPanel);
            Controls.Add(_collapsedStrip);
        }

        /// <summary>
        /// Đặt bề rộng khi mở rộng (tính từ container cha) - gọi trước khi hiển thị
        /// </summary>
        public void SetExpandedWidth(int width)
        {
            _expandedWidth = Math.Max(COLLAPSED_WIDTH * 10, width);
            if (!_collapsed)
                Width = _expandedWidth;
        }

        /// <summary>Thu gọn về dải dọc hẹp / mở rộng lại theo chiều ngang</summary>
        private void ToggleCollapsed()
        {
            // Giữ lại bề rộng hiện tại (có thể user đã kéo splitter) để mở lại đúng cỡ
            if (!_collapsed)
                _expandedWidth = Width;

            _collapsed = !_collapsed;
            _contentPanel.Visible = !_collapsed;
            _headerPanel.Visible = !_collapsed;
            _collapsedStrip.Visible = _collapsed;
            Width = _collapsed ? COLLAPSED_WIDTH : _expandedWidth;
            if (_collapsed)
                _collapsedStrip.Invalidate();

            var handler = CollapsedChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>Vẽ mũi tên mở rộng + tiêu đề xoay dọc trên dải thu gọn</summary>
        private void CollapsedStrip_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            using (var font = new Font("Segoe UI", 10F, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.FromArgb(60, 80, 110)))
            {
                // Mũi tên "mở sang phải" ở đầu dải
                g.DrawString("►", font, brush, 6, 8);

                // Chữ xoay 90 độ chạy dọc theo dải
                g.TranslateTransform(_collapsedStrip.Width / 2f + 7, 36);
                g.RotateTransform(90);
                g.DrawString(_title, font, brush, 0, 0);
                g.ResetTransform();
            }
        }

        private void UpdateHeaderText(string title)
        {
            _title = title;
            _headerLabel.Text = "◄ " + title;
            if (_collapsed)
                _collapsedStrip.Invalidate();
        }

        /// <summary>Render form từ detail của suggestion Structured</summary>
        public void SetData(QuickSuggestionPublicDetailDto detail)
        {
            _suspendEvents = true;
            try
            {
                ClearData();

                if (detail == null || detail.paramGroups == null || detail.paramGroups.Count == 0)
                    return;

                UpdateHeaderText("Chỉ số: " + (detail.reportParamName ?? detail.title ?? ""));

                _grid.SuspendLayout();
                int col = 0;
                int row = 0;
                foreach (var group in detail.paramGroups.OrderBy(g => g.sortOrder))
                {
                    int span = Math.Max(1, Math.Min(GRID_COLUMNS, group.colSpan));
                    if (group.forceNewRow || col + span > GRID_COLUMNS)
                    {
                        if (col > 0)
                        {
                            col = 0;
                            row++;
                        }
                    }

                    var groupBox = CreateGroupControl(group);
                    _grid.Controls.Add(groupBox, col, row);
                    _grid.SetColumnSpan(groupBox, span);

                    col += span;
                    if (col >= GRID_COLUMNS)
                    {
                        col = 0;
                        row++;
                    }
                }
                _grid.ResumeLayout();

                // Mở rộng lại nếu đang thu gọn từ suggestion trước
                if (_collapsed)
                    ToggleCollapsed();
            }
            finally
            {
                _suspendEvents = false;
            }
        }

        /// <summary>Xóa toàn bộ nội dung form</summary>
        public void ClearData()
        {
            _entries.Clear();
            _grid.Controls.Clear();
            _grid.RowStyles.Clear();
            UpdateHeaderText("Chỉ số");
        }

        private GroupControl CreateGroupControl(ParamGroupFormDto group)
        {
            var groupBox = new GroupControl
            {
                Text = group.groupName,
                Dock = DockStyle.Fill,
                Margin = new Padding(4)
            };
            groupBox.AppearanceCaption.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            groupBox.AppearanceCaption.Options.UseFont = true;

            int halves = Math.Max(1, group.paramColumns);
            // Mỗi "nửa" (paramColumns) gồm 3 cột: nhãn | ô nhập | đơn vị.
            // Cột nhãn/đơn vị AutoSize tự canh THẲNG HÀNG theo nhãn dài nhất,
            // cột ô nhập Percent để ô nhập CO GIÃN theo bề ngang sidebar.
            var innerTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = halves * 3,
                Padding = new Padding(4, 2, 4, 4)
            };
            for (int i = 0; i < halves; i++)
            {
                innerTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                innerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / halves));
                innerTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            }

            var fields = (group.@params ?? new List<ParamFieldDto>()).OrderBy(p => p.sortOrder).ToList();
            for (int i = 0; i < fields.Count; i++)
            {
                AddParamToTable(innerTable, (i % halves) * 3, i / halves, group, fields[i]);
            }

            groupBox.Controls.Add(innerTable);
            // GroupControl không tự AutoSize - dùng MinimumSize để TableLayoutPanel cha
            // (row AutoSize) dãn theo nội dung bên trong
            groupBox.MinimumSize = new Size(0, innerTable.PreferredSize.Height + GROUP_CAPTION_HEIGHT);
            return groupBox;
        }

        /// <summary>Thêm 1 chỉ số vào lưới của nhóm tại (colBase..colBase+2, row)</summary>
        private void AddParamToTable(TableLayoutPanel table, int colBase, int row, ParamGroupFormDto group, ParamFieldDto field)
        {
            var entry = new ParamInputEntry { Group = group, Field = field };

            if (field.inputMode == "checkbox")
            {
                // checkbox thuần: caption là câu mô tả hoàn chỉnh, chiếm cả 3 cột của nửa
                var checkBox = CreateCheckEdit(field, field.presetLabel ?? field.label);
                entry.CheckBox = checkBox;
                table.Controls.Add(checkBox, colBase, row);
                table.SetColumnSpan(checkBox, 3);
                _entries.Add(entry);
                return;
            }

            // Cột nhãn: checkbox_value dùng CheckEdit làm nhãn, value dùng LabelControl
            if (field.inputMode == "checkbox_value")
            {
                var checkBox = CreateCheckEdit(field, field.label);
                entry.CheckBox = checkBox;
                table.Controls.Add(checkBox, colBase, row);
            }
            else
            {
                var label = new LabelControl
                {
                    Text = field.label,
                    Margin = new Padding(4, 8, 4, 4)
                };
                if (field.isHighlighted)
                {
                    label.Appearance.Font = new Font(label.Font, FontStyle.Bold);
                    label.Appearance.Options.UseFont = true;
                }
                table.Controls.Add(label, colBase, row);
            }

            // Cột ô nhập: neo 2 đầu để co giãn theo bề ngang sidebar (cột Percent),
            // MinimumSize giữ bề rộng tối thiểu khi sidebar bị thu hẹp
            var valueBox = new TextEdit
            {
                Width = field.dataType == "number" ? 72 : 110,
                MinimumSize = new Size(field.dataType == "number" ? 56 : 90, 0),
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
                Text = field.presetValue ?? "",
                Margin = new Padding(2, 4, 2, 4)
            };
            entry.ValueBox = valueBox;
            ApplyRangeColor(entry);
            valueBox.TextChanged += (s, e) =>
            {
                ApplyRangeColor(entry);
                OnInputChanged(s, e);
            };
            table.Controls.Add(valueBox, colBase + 1, row);

            // Cột đơn vị
            if (!string.IsNullOrEmpty(field.unit))
            {
                table.Controls.Add(new LabelControl
                {
                    Text = field.unit,
                    Margin = new Padding(2, 8, 4, 4)
                }, colBase + 2, row);
            }

            _entries.Add(entry);
        }

        private CheckEdit CreateCheckEdit(ParamFieldDto field, string caption)
        {
            var checkBox = new CheckEdit
            {
                Text = caption,
                Margin = new Padding(2, 4, 2, 4)
            };
            if (field.isHighlighted)
            {
                checkBox.Properties.Appearance.Font = new Font(checkBox.Font, FontStyle.Bold);
                checkBox.Properties.Appearance.Options.UseFont = true;
            }
            // CheckEdit không tự dãn theo caption - đo text để không bị cắt chữ
            checkBox.Width = TextRenderer.MeasureText(caption, checkBox.Font).Width + 30;
            checkBox.CheckedChanged += OnInputChanged;
            return checkBox;
        }

        private void OnInputChanged(object sender, EventArgs e)
        {
            if (_suspendEvents)
                return;
            var handler = ParamValuesChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Tìm dải ngưỡng khớp với giá trị đang nhập của một chỉ số:
        /// dải đầu tiên thỏa min <= value < max (min/max null = không giới hạn).
        /// Trả về null nếu không có ranges, giá trị không phải số hoặc không khớp dải nào.
        /// </summary>
        private static ReportParamRangeMeta FindMatchedRange(ParamInputEntry entry)
        {
            if (entry.ValueBox == null || entry.Field.ranges == null || entry.Field.ranges.Count == 0)
                return null;

            double value;
            string raw = (entry.ValueBox.Text ?? "").Trim().Replace(',', '.');
            if (!double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                return null;

            return entry.Field.ranges.FirstOrDefault(r =>
                (!r.min.HasValue || value >= r.min.Value) &&
                (!r.max.HasValue || value < r.max.Value));
        }

        /// <summary>Tô màu giá trị số theo dải ngưỡng (ranges) và hiện tooltip nhãn ngưỡng</summary>
        private void ApplyRangeColor(ParamInputEntry entry)
        {
            if (entry.ValueBox == null || entry.Field.ranges == null || entry.Field.ranges.Count == 0)
                return;

            var range = FindMatchedRange(entry);
            if (range == null)
            {
                ResetValueBoxColor(entry.ValueBox);
                return;
            }

            try
            {
                entry.ValueBox.Properties.Appearance.ForeColor = ColorTranslator.FromHtml(range.color);
                entry.ValueBox.Properties.Appearance.Options.UseForeColor = true;
            }
            catch (Exception ex)
            {
                Log.Debug(ex, "Mã màu range không hợp lệ: {Color}", range.color);
                ResetValueBoxColor(entry.ValueBox);
                return;
            }
            entry.ValueBox.ToolTip = range.label;
        }

        private static void ResetValueBoxColor(TextEdit valueBox)
        {
            valueBox.Properties.Appearance.Options.UseForeColor = false;
            valueBox.ToolTip = null;
        }

        // Dòng kẻ ngăn cách khối chỉ số với phần mô tả bác sĩ gõ tay phía dưới
        private const string PARAM_TEXT_SEPARATOR = "--------------";

        /// <summary>
        /// Sinh text mô tả từ giá trị đã nhập: chỉ số dạng value gộp trên dòng tên nhóm
        /// "TênNhóm: chỉ số 1; chỉ số 2." (chỉ lấy khi có giá trị),
        /// checkbox hiện cả 2 trạng thái ☑/☐ và MỖI PARAM 1 DÒNG riêng bên dưới.
        /// Cuối khối thêm 1 dòng kẻ ngăn cách với phần mô tả gõ tay phía dưới.
        /// Dùng "\n" (không dùng \r\n) vì RichTextBox chuẩn hóa newline về \n,
        /// nếu lệch sẽ hỏng cơ chế thay thế khối text đã sinh.
        /// </summary>
        public string GenerateText()
        {
            var lines = new List<string>();

            foreach (var groupEntries in _entries.GroupBy(x => x.Group).OrderBy(g => g.Key.sortOrder))
            {
                var valueFragments = new List<string>();
                var checkboxLines = new List<string>();
                foreach (var entry in groupEntries)
                {
                    string fragment = BuildFragment(entry);
                    if (string.IsNullOrEmpty(fragment))
                        continue;

                    if (entry.Field.inputMode == "checkbox" || entry.Field.inputMode == "checkbox_value")
                        checkboxLines.Add(fragment);
                    else
                        valueFragments.Add(fragment);
                }

                if (valueFragments.Count == 0 && checkboxLines.Count == 0)
                    continue;

                lines.Add(valueFragments.Count > 0
                    ? groupEntries.Key.groupName + ": " + string.Join("; ", valueFragments) + "."
                    : groupEntries.Key.groupName + ":");
                lines.AddRange(checkboxLines);
            }

            if (lines.Count == 0)
                return "";

            lines.Add(PARAM_TEXT_SEPARATOR);
            return string.Join("\n", lines);
        }

        private static string BuildFragment(ParamInputEntry entry)
        {
            var field = entry.Field;

            if (field.inputMode == "checkbox")
            {
                bool ticked = entry.CheckBox != null && entry.CheckBox.Checked;
                return (ticked ? "☑ " : "☐ ") + (field.presetLabel ?? field.label);
            }

            if (field.inputMode == "checkbox_value")
            {
                bool ticked = entry.CheckBox != null && entry.CheckBox.Checked;
                string prefix = ticked ? "☑ " : "☐ ";
                string val = entry.ValueBox != null ? entry.ValueBox.Text.Trim() : "";
                // Không tích thì chỉ hiện nhãn, không kèm giá trị
                if (!ticked || string.IsNullOrEmpty(val))
                    return prefix + field.label;
                return prefix + field.label + ": " + val + FormatUnit(field.unit);
            }

            // inputMode = value
            string text = entry.ValueBox != null ? entry.ValueBox.Text.Trim() : "";
            if (string.IsNullOrEmpty(text))
                return null;
            return field.label + ": " + text + FormatUnit(field.unit);
        }

        private static string FormatUnit(string unit)
        {
            return string.IsNullOrEmpty(unit) ? "" : " " + unit;
        }

        /// <summary>
        /// Xuất toàn bộ giá trị hiện tại của form thành input thô (groupCode/paramCode → value/checked)
        /// để gửi lên RIS mới — backend tự build Schema B (tính status/màu theo ranges).
        /// Xuất đủ mọi chỉ số (kể cả rỗng/không tích) để SetParamValues khôi phục đúng trạng thái.
        /// </summary>
        public List<RisV1ReportParamInputItem> GetParamValues()
        {
            var list = new List<RisV1ReportParamInputItem>();
            foreach (var entry in _entries)
            {
                list.Add(new RisV1ReportParamInputItem
                {
                    groupCode = entry.Group.groupCode,
                    paramCode = entry.Field.paramCode,
                    value = entry.ValueBox != null ? entry.ValueBox.Text.Trim() : null,
                    displayLabel = entry.CheckBox != null ? (entry.Field.presetLabel ?? entry.Field.label) : null,
                    @checked = entry.CheckBox != null ? (bool?)entry.CheckBox.Checked : null
                });
            }
            return list;
        }

        /// <summary>
        /// Đổ lại giá trị đã lưu vào form theo paramCode (gọi sau SetData cùng reportParam).
        /// Không raise ParamValuesChanged — caller tự quyết định việc đồng bộ text Mô tả.
        /// </summary>
        public void SetParamValues(List<RisV1ReportParamSnapshotItem> values)
        {
            if (values == null || values.Count == 0)
                return;

            _suspendEvents = true;
            try
            {
                foreach (var entry in _entries)
                {
                    var saved = values.FirstOrDefault(v => v.paramCode == entry.Field.paramCode);
                    if (saved == null)
                        continue;

                    if (entry.CheckBox != null)
                        entry.CheckBox.Checked = saved.@checked == true;

                    if (entry.ValueBox != null)
                    {
                        entry.ValueBox.Text = saved.value ?? "";
                        ApplyRangeColor(entry);
                    }
                }
            }
            finally
            {
                _suspendEvents = false;
            }
        }

        /// <summary>
        /// Xuất các chỉ số có giá trị/được tích thành danh sách dòng cho bảng chỉ số
        /// trong báo cáo PDF (DetailReportBand bind DanhSachChiSo).
        /// Kèm nhãn đánh giá + mã màu theo dải ngưỡng để template tô màu cột giá trị.
        /// </summary>
        public List<ChiSoBaoCao> GetReportItems()
        {
            var items = new List<ChiSoBaoCao>();

            foreach (var entry in _entries)
            {
                var field = entry.Field;

                if (field.inputMode == "checkbox")
                {
                    if (entry.CheckBox == null || !entry.CheckBox.Checked)
                        continue;
                    items.Add(new ChiSoBaoCao
                    {
                        NhomChiSo = entry.Group.groupName,
                        TenChiSo = field.presetLabel ?? field.label,
                        GiaTri = "",
                        DonVi = "",
                        DanhGia = "",
                        MauSac = ""
                    });
                    continue;
                }

                if (field.inputMode == "checkbox_value" && (entry.CheckBox == null || !entry.CheckBox.Checked))
                    continue;

                string value = entry.ValueBox != null ? entry.ValueBox.Text.Trim() : "";
                if (string.IsNullOrEmpty(value))
                    continue;

                var range = FindMatchedRange(entry);
                items.Add(new ChiSoBaoCao
                {
                    NhomChiSo = entry.Group.groupName,
                    TenChiSo = field.label,
                    GiaTri = value,
                    DonVi = field.unit ?? "",
                    DanhGia = range != null ? range.label : "",
                    MauSac = range != null ? range.color : ""
                });
            }

            return items;
        }
    }
}
