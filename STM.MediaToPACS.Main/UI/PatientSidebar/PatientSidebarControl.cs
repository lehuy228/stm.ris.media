using MediaToPacs.Core.Models;
using MediaToPacs.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList;
using STM.MediaToPACS.Main.Utilities;
using Serilog;

namespace STM.MediaToPACS.Main.UI.PatientSidebar
{
    public sealed class OrderNavigationRequestedEventArgs : EventArgs
    {
        public OrderNavigationRequestedEventArgs(
            string orderCode, string placerOrderItemCode, string patientCode, string patientName)
        {
            OrderCode = orderCode;
            PlacerOrderItemCode = placerOrderItemCode;
            PatientCode = patientCode;
            PatientName = patientName;
        }

        public string OrderCode { get; }
        public string PlacerOrderItemCode { get; }
        public string PatientCode { get; }
        public string PatientName { get; }
    }

    /// <summary>
    /// Sidebar trái trong tab "Kết luận" (Dock = Left, chỉ chiếm đúng vùng Mô tả - xem
    /// FrmMain.Designer.cs). Tab dọc (HeaderLocation = Left): "Lịch sử khám bệnh nhân"
    /// (chỉ 1 TreeList - API risv1) và "Tham số siêu âm" (host cho ParamFormControl).
    /// Tự quản lý thu gọn/mở rộng: mặc định thu gọn (chỉ còn dải tab dọc), bấm 1 tab thì
    /// tự mở rộng; ◂ để đóng lại; 📌 ghim để không tự đóng khi rời focus (Leave).
    /// </summary>
    public partial class PatientSidebarControl : UserControl
    {
        private const int CollapsedWidth = 32;
        internal const int DefaultExpandedWidth = 320;

        private bool _pinned;
        private bool _collapsed = true;

        /// <summary>Phát sinh khi người dùng bấm nút ghim - FrmMain nghe để lưu lại lựa chọn.</summary>
        public event EventHandler PinnedChanged;

        /// <summary>Phát sinh khi chuyển giữa thu gọn/mở rộng - FrmMain nghe để ẩn/hiện splitter kéo giãn.</summary>
        public event EventHandler CollapsedChanged;
        public event EventHandler<OrderNavigationRequestedEventArgs> OrderNavigationRequested;

        /// <summary>Người dùng mở tab "Nhật ký" hoặc bấm "Tải lại" - màn kết luận nghe để gọi API audit-log.</summary>
        public event EventHandler LogRefreshRequested;

        public bool Collapsed => _collapsed;
        private string _currentOrderCode;
        private TreeListNode _currentOrderNode;
        private string _patientCode;
        private string _patientName;

        /// <summary>
        /// Danh mục phương thức chụp đã khai báo (Modality.xml) - nạp 1 lần rồi cache.
        /// null = chưa nạp, rỗng = chưa khai báo gì (khi đó không chặn chỉ định nào).
        /// </summary>
        private HashSet<string> _declaredModalities;

        /// <summary>Các node chỉ định bị khoá vì modality không nằm trong danh mục khai báo.</summary>
        private readonly HashSet<TreeListNode> _unsupportedNodes = new HashSet<TreeListNode>();

        /// <summary>
        /// Bề rộng khi mở rộng (px). Splitter bên FrmMain chỉnh trực tiếp qua Width khi đang mở;
        /// FrmMain đọc lại giá trị này sau khi kéo xong để lưu vào UiLayoutSettings.
        /// </summary>
        public int ExpandedWidth { get; set; } = DefaultExpandedWidth;

        public bool Pinned
        {
            get => _pinned;
            private set
            {
                if (_pinned == value)
                    return;
                _pinned = value;
                UpdatePinButtonAppearance();
                PinnedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Nạp trạng thái ghim đã lưu (gọi 1 lần lúc khởi tạo form) - không phát sinh PinnedChanged.
        /// Đã ghim thì mở rộng sẵn luôn, khỏi phải bấm lại mỗi lần mở form.
        /// </summary>
        public void RestorePinned(bool pinned)
        {
            _pinned = pinned;
            UpdatePinButtonAppearance();
            if (pinned)
                SetCollapsed(false);
        }

        /// <summary>Panel rỗng để FrmMain gắn ParamFormControl vào (Dock = Fill).</summary>
        public Panel ParamsHostPanel => _paramsHostPanel;

        public PatientSidebarControl()
        {
            InitializeComponent();
            UpdatePinButtonAppearance();

            this.Leave += (s, e) =>
            {
                if (!_pinned)
                    SetCollapsed(true);
            };

            SetCollapsed(true);

            // Chưa chọn mẫu gợi ý nào nên chưa có chỉ số - khoá sẵn tab "Tham số siêu âm"
            SetParamsTabAvailable(false);

            // Nhật ký chỉ nạp khi mở tab - lúc đầu chỉ hiện dòng trạng thái, chưa có lưới
            SetLogPlaceholder("Chưa có nhật ký");
        }

        public void ActivateHistoryTab() => _tabControl.SelectedTabPage = _tabHistory;

        public void ActivateParamsTab()
        {
            if (!_tabParams.PageEnabled)
                return;

            _tabControl.SelectedTabPage = _tabParams;
            SetCollapsed(false);
        }

        /// <summary>
        /// Bật/khoá tab "Tham số siêu âm" theo mẫu gợi ý đang chọn: mẫu không có chỉ số
        /// (Text/luồng cũ) thì khoá lại để không bấm vào tab rỗng được. Đang đứng ở tab này
        /// mà bị khoá thì chuyển về tab lịch sử, giữ nguyên trạng thái thu gọn hiện tại.
        /// </summary>
        public void SetParamsTabAvailable(bool available)
        {
            if (_tabParams.PageEnabled == available)
                return;

            if (!available && _tabControl.SelectedTabPage == _tabParams)
            {
                bool wasCollapsed = _collapsed;
                _tabControl.SelectedTabPage = _tabHistory;
                SetCollapsed(wasCollapsed);
            }

            _tabParams.PageEnabled = available;
        }

        public void ActivateLogTab()
        {
            _tabControl.SelectedTabPage = _tabLog;
            SetCollapsed(false);
        }

        private void TabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            SetCollapsed(false);

            // Nhật ký nạp theo yêu cầu (lazy) - vào tab mới gọi API, tránh gọi thừa mỗi lần mở chỉ định.
            if (e.Page == _tabLog)
                LogRefreshRequested?.Invoke(this, EventArgs.Empty);
        }

        private void BtnReloadLog_Click(object sender, EventArgs e)
        {
            LogRefreshRequested?.Invoke(this, EventArgs.Empty);
        }

        #region Nhật ký (audit-log của chỉ định đang mở)

        /// <summary>Dòng hiển thị trên lưới nhật ký - chỉ để bind, không dùng ngoài sidebar.</summary>
        private sealed class AuditLogRow
        {
            public string TimeText { get; set; }
            public string Content { get; set; }
            public string UserText { get; set; }
        }

        public void ShowLogLoading()
        {
            SetLogPlaceholder("Đang tải nhật ký...");
        }

        public void ShowLogError(string message)
        {
            SetLogPlaceholder(message);
        }

        /// <summary>Đổ danh sách nhật ký (đã sắp mới nhất trước) lên lưới tab "Nhật ký".</summary>
        public void ShowLogs(List<AuditLogListItemDto> logs)
        {
            if (logs == null || logs.Count == 0)
            {
                SetLogPlaceholder("Chưa có nhật ký cho chỉ định này");
                return;
            }

            _gridLog.DataSource = logs.Select(x => new AuditLogRow
            {
                TimeText = FormatLogTime(x.timestampUtc),
                Content = BuildLogContent(x),
                UserText = FormatLogUser(x)
            }).ToList();

            _lblLogPlaceholder.Visible = false;
            _gridLog.Visible = true;
            _gridViewLog.BestFitColumns();
        }

        private void SetLogPlaceholder(string text)
        {
            _gridLog.DataSource = null;
            _gridLog.Visible = false;
            _lblLogPlaceholder.Text = text;
            _lblLogPlaceholder.Visible = true;
            _lblLogPlaceholder.BringToFront();
        }

        /// <summary>timestampUtc là giờ UTC - đổi về giờ máy để đối chiếu với thao tác vừa làm.</summary>
        private static string FormatLogTime(DateTimeOffset value)
        {
            return value.ToLocalTime().ToString("dd/MM HH:mm:ss", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Hiển thị tên người thực hiện; backend chỉ tra được userFullName khi userId là
        /// Practitioner.Id dạng số, còn lại (vd "risv1", username Keycloak) thì hiện userId thô.
        /// </summary>
        private static string FormatLogUser(AuditLogListItemDto log)
        {
            if (!string.IsNullOrWhiteSpace(log.userFullName))
                return log.userFullName.Trim();

            if (string.Equals(log.userId, "risv1", StringComparison.OrdinalIgnoreCase))
                return "Hệ thống (RIS V1)";

            return log.userId ?? "";
        }

        /// <summary>
        /// Ưu tiên message tiếng Việt do backend ghi; không có thì rơi về action/source để
        /// dòng nhật ký không bị rỗng.
        /// </summary>
        private static string BuildLogContent(AuditLogListItemDto log)
        {
            if (!string.IsNullOrWhiteSpace(log.message))
                return log.message.Trim();
            if (!string.IsNullOrWhiteSpace(log.action))
                return log.action.Trim();
            return log.source ?? "";
        }

        #endregion

        private void BtnPin_Click(object sender, EventArgs e)
        {
            Pinned = !Pinned;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            SetCollapsed(true);
        }

        private void SetCollapsed(bool collapsed)
        {
            bool changed = _collapsed != collapsed;
            _collapsed = collapsed;
            Width = collapsed ? CollapsedWidth : ExpandedWidth;
            _headerPanel.Visible = !collapsed;

            if (changed)
                CollapsedChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdatePinButtonAppearance()
        {
            // Đã ghim: chấm đặc + màu vàng để phân biệt trạng thái đang giữ mở cố định
            _btnPin.Text = _pinned ? "●" : "○";
            _btnPin.Appearance.ForeColor = _pinned
                ? System.Drawing.Color.FromArgb(255, 214, 0)
                : System.Drawing.Color.White;
        }

        public void ShowHistoryLoading()
        {
            _treeHistory.Nodes.Clear();
            _treeHistory.AppendNode(new object[] { "Đang tải lịch sử khám...", "", "" }, -1);
        }

        public void ShowHistoryError(string message)
        {
            _treeHistory.Nodes.Clear();
            _treeHistory.AppendNode(new object[] { message, "", "" }, -1);
        }

        public void ShowHistory(RisV1PatientOrderHistoryDto history, string currentOrderCode)
        {
            _treeHistory.Nodes.Clear();
            _currentOrderCode = currentOrderCode;
            _currentOrderNode = null;
            _unsupportedNodes.Clear();

            if (history == null)
            {
                _treeHistory.AppendNode(new object[] { "Không tìm thấy lịch sử khám", "", "" }, -1);
                return;
            }

            var patient = history.patient;
            _patientCode = patient?.patientCode;
            _patientName = patient?.fullName;
            string patientLabel = patient != null
                ? $"{patient.fullName} ({patient.patientCode})"
                : "Bệnh nhân";

            var patientNode = _treeHistory.AppendNode(
                new object[] { patientLabel, "", patient != null ? FormatGender(patient.gender) : "" },
                -1);

            if (history.visits == null || history.visits.Count == 0)
            {
                _treeHistory.AppendNode(new object[] { "Chưa có lượt khám nào", "", "" }, patientNode.Id);
                _treeHistory.ExpandAll();
                return;
            }

            foreach (var visit in history.visits)
            {
                string visitLabel = string.IsNullOrWhiteSpace(visit.admissionDiagnosis)
                    ? visit.visitCode
                    : $"{visit.visitCode} - {visit.admissionDiagnosis}";

                var visitNode = _treeHistory.AppendNode(
                    new object[] { visitLabel, FormatDate(visit.admissionTime), TranslateVisitType(visit.visitType) },
                    patientNode.Id);

                if (visit.orderItems == null)
                    continue;

                foreach (var item in visit.orderItems)
                {
                    string content = string.IsNullOrWhiteSpace(item.serviceName)
                        ? item.serviceCode
                        : item.serviceName;

                    var orderNode = _treeHistory.AppendNode(
                        new object[]
                        {
                            content,
                            FormatDate(item.scheduledAt ?? item.completedAt),
                            FormatOrderItemStatus(item)
                        },
                        visitNode.Id);
                    orderNode.Tag = item;

                    // Chỉ định thuộc phương thức chụp không khai báo trên máy này thì khoá lại,
                    // tránh mở nhầm chỉ định máy không xử lý được.
                    if (!IsModalityDeclared(item.modalityCode))
                        _unsupportedNodes.Add(orderNode);

                    if (string.Equals(item.placerOrderItemCode, currentOrderCode,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        _currentOrderNode = orderNode;
                    }
                }
            }

            _treeHistory.ExpandAll();
            if (_currentOrderNode != null)
            {
                _treeHistory.FocusedNode = _currentOrderNode;
                _treeHistory.MakeNodeVisible(_currentOrderNode);
            }
        }

        /// <summary>
        /// Phương thức chụp có nằm trong danh mục khai báo trên máy này không.
        /// Chưa khai báo danh mục nào = không giới hạn (cho mở tất cả).
        /// </summary>
        private bool IsModalityDeclared(string modalityCode)
        {
            var declared = GetDeclaredModalities();
            if (declared.Count == 0)
                return true;

            if (string.IsNullOrWhiteSpace(modalityCode))
                return false;

            return declared.Contains(modalityCode.Trim());
        }

        /// <summary>
        /// Nạp danh mục phương thức chụp từ Modality.xml (cùng nguồn với combobox
        /// phương thức chụp - xem MainForm.InitializeModalityComboBox). Lỗi đọc file coi như
        /// chưa khai báo để không chặn nhầm toàn bộ lịch sử.
        /// </summary>
        private HashSet<string> GetDeclaredModalities()
        {
            if (_declaredModalities != null)
                return _declaredModalities;

            _declaredModalities = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var modalities = XmlSettingsHelper.Load<Modalities>(
                    Path.Combine(
                        ServiceLocator.GetAppDataBasePath(),
                        FileStorageSettingsProvider.Current.Modality));

                if (!string.IsNullOrWhiteSpace(modalities?.ModalitiesList))
                {
                    foreach (var modality in modalities.ModalitiesList
                        .Split(new[] { "\r\n", "\n", ",", ";" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(m => m.Trim())
                        .Where(m => m.Length > 0))
                    {
                        _declaredModalities.Add(modality);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không đọc được danh mục phương thức chụp - bỏ qua lọc modality ở lịch sử khám");
            }

            return _declaredModalities;
        }

        private void TreeHistory_NodeCellStyle(object sender, GetCustomNodeCellStyleEventArgs e)
        {
            if (e.Node == _currentOrderNode)
            {
                e.Appearance.BackColor = Color.FromArgb(219, 238, 255);
                e.Appearance.ForeColor = Color.FromArgb(0, 76, 140);
                e.Appearance.FontStyleDelta = FontStyle.Bold;
                return;
            }

            // Modality không khai báo: làm mờ như control bị disable
            if (_unsupportedNodes.Contains(e.Node))
                e.Appearance.ForeColor = Color.FromArgb(160, 160, 160);
        }

        private void TreeHistory_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var hitInfo = _treeHistory.CalcHitInfo(e.Location);
            var item = hitInfo.Node?.Tag as RisV1OrderItemHistoryDto;
            if (item == null || string.IsNullOrWhiteSpace(item.placerOrderItemCode))
                return;

            if (string.Equals(item.placerOrderItemCode, _currentOrderCode,
                StringComparison.OrdinalIgnoreCase))
                return;

            if (_unsupportedNodes.Contains(hitInfo.Node))
            {
                Log.Information(
                    "Bỏ qua mở chỉ định {Code}: phương thức chụp {Modality} không có trong danh mục khai báo",
                    item.placerOrderItemCode, item.modalityCode);
                return;
            }

            OrderNavigationRequested?.Invoke(this,
                new OrderNavigationRequestedEventArgs(
                    item.orderCode, item.placerOrderItemCode, _patientCode, _patientName));
        }

        private static string FormatGender(string gender)
        {
            if (string.Equals(gender, "male", StringComparison.OrdinalIgnoreCase)) return "Nam";
            if (string.Equals(gender, "female", StringComparison.OrdinalIgnoreCase)) return "Nữ";
            return gender ?? "";
        }

        private static string FormatDate(DateTimeOffset? value)
        {
            return value.HasValue
                ? value.Value.LocalDateTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                : "";
        }

        /// <summary>
        /// Ưu tiên hiển thị trạng thái phiếu kết quả (reportStatus) nếu đã có phiếu,
        /// chưa có thì hiển thị trạng thái chỉ định (OrderStatus). Xem docs/api/ris-v1.md §8, §10.
        /// </summary>
        private static string FormatOrderItemStatus(RisV1OrderItemHistoryDto item)
        {
            if (!string.IsNullOrWhiteSpace(item.reportStatus))
                return TranslateReportStatus(item.reportStatus);
            return TranslateOrderStatus(item.status);
        }

        private static string TranslateOrderStatus(string status)
        {
            switch (status)
            {
                case "accepted": return "Đã tiếp nhận";
                case "scheduled": return "Đã lên lịch";
                case "acquired": return "Đã chụp";
                case "reading": return "Đang đọc kết quả";
                case "preliminary": return "Kết quả sơ bộ";
                case "completed": return "Hoàn thành";
                case "cancelled": return "Đã hủy";
                case "no-show": return "Không đến khám";
                case "on-hold": return "Tạm hoãn";
                default: return status ?? "";
            }
        }

        /// <summary>
        /// visitType là mã FHIR Encounter.class (hệ mã v3-ActCode), ví dụ "AMB" = ngoại trú.
        /// </summary>
        private static string TranslateVisitType(string visitType)
        {
            switch (visitType)
            {
                case "AMB": return "Ngoại trú";
                case "IMP": return "Nội trú";
                case "ACUTE": return "Nội trú cấp tính";
                case "NONAC": return "Nội trú không cấp tính";
                case "EMER": return "Cấp cứu";
                case "FLD": return "Tại hiện trường";
                case "HH": return "Chăm sóc tại nhà";
                case "OBSENC": return "Theo dõi";
                case "PRENC": return "Tiền nhập viện";
                case "SS": return "Lưu trú ngắn ngày";
                case "VR": return "Khám từ xa";
                default: return visitType ?? "";
            }
        }

        private static string TranslateReportStatus(string reportStatus)
        {
            switch (reportStatus)
            {
                case "draft": return "Nháp";
                case "preliminary": return "Sơ bộ";
                case "final": return "Đã ký";
                case "amended": return "Đã chỉnh sửa";
                case "cancelled": return "Đã hủy";
                default: return reportStatus ?? "";
            }
        }
    }
}
