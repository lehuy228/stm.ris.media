using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using MediaToPacs.Core.Models;
using MediaToPacs.Core.Models.Ketluan;
using STM.MediaToPACS.Main.UI.CameraUI;
using STM.MediaToPACS.Main.UI.PatientSidebar;
using STM.MediaToPACS.Main.Utilities;
using Serilog;

namespace STM.MediaToPACS.Main.UI.DiagnosticReports
{
    /// <summary>
    /// Bản dựng tạm thời (không phụ thuộc Leadtools/DICOM) tái sử dụng logic Camera + Kết luận +
    /// Lịch sử bệnh nhân từ FrmMain.
    /// Bước 1: scaffold UserControl + camera + danh sách ảnh thumbnail (ImageThumbnailList thay
    /// cho ListImageBox vốn dựng trên Leadtools RasterImage).
    /// Bước 2: kết luận (gợi ý/lưu/ký số/in) - xem DiagnosticReportConclusionControl.Suggestion.cs, .SaveLoad.cs,
    /// .Signature.cs, .Print.cs, .Loading.cs.
    /// Lịch sử bệnh nhân (bước 3) sẽ thêm sau.
    /// </summary>
    public partial class DiagnosticReportConclusionControl : UserControl
    {
        public event EventHandler<OrderNavigationRequestedEventArgs> OrderNavigationRequested;
        public event EventHandler TabCaptionChanged;
        public string PatientTabCaption { get; private set; }

        private readonly string _videoInputDevice;
        private readonly string _sophieu;
        private readonly string _machidinh;
        private readonly string _baseFolder;

        private CameraControl _cameraControl;

        // Dữ liệu chỉ định/kết luận - xem DiagnosticReportConclusionControl.Loading.cs
        private ChiDinhDichVuResponse _chiDinhDichVuResponse;
        private KetQuaChanDoanResponse _kqChanDoanResponse;
        private List<DeviceDto> _listThietBi;
        private List<ReportTemplateGridViewModel> _listMauBaoCao;
        private List<PractitionerListDto> _listHisUser;
        private HisUserKySoResponse _hisUserKySoResponse;
        private List<string> listImageKeyLocal = new List<string>();
        private const string FileNameXMLImage = "ImageSelected.xml";

        public DiagnosticReportConclusionControl(string videoInputDevice, string soPhieu, string maChiDinh)
        {
            _videoInputDevice = videoInputDevice;
            _sophieu = soPhieu;
            _machidinh = maChiDinh;
            _baseFolder = ServiceLocator.GetMediaStorageBasePath();
            if (!Directory.Exists(_baseFolder))
                Directory.CreateDirectory(_baseFolder);

            InitializeComponent();
            InitCamera();
            InitThumbnailListCounter();
            _patientSidebar.OrderNavigationRequested += PatientSidebar_OrderNavigationRequested;

            this.Load += DiagnosticReportConclusionControl_Load;
        }

        private void PatientSidebar_OrderNavigationRequested(
            object sender, OrderNavigationRequestedEventArgs e)
        {
            OrderNavigationRequested?.Invoke(this, e);
        }

        private void InitCamera()
        {
            _cameraControl = new CameraControl(_videoInputDevice)
            {
                Dock = DockStyle.Fill
            };
            _cameraViewport.Controls.Add(_cameraControl);
        }

        /// <summary>Cập nhật bộ đếm "đã chọn/tổng số" ảnh (_lbImageSelect) - tương đương UpdateToolBarState trong FrmMain.</summary>
        private void InitThumbnailListCounter()
        {
            void UpdateCounter(object s, EventArgs e) =>
                _lbImageSelect.Text = $"{_thumbnailList.GetCheckedFilePaths().Count}/{_thumbnailList.Items.Count}";

            _thumbnailList.ItemAdded += UpdateCounter;
            _thumbnailList.SelectionChanged += UpdateCounter;
        }

        /// <summary>Đóng form host chứa control này (tương đương _btnCancel_Click trong FrmMain).</summary>
        private void _btnCancel_Click(object sender, EventArgs e)
        {
            this.FindForm()?.Close();
        }

        private async void DiagnosticReportConclusionControl_Load(object sender, EventArgs e)
        {
            try
            {
                // Nạp bề rộng/trạng thái ghim sidebar đã lưu sau khi control đã có kích thước thật
                // (tránh clamp sai bằng kích thước lúc thiết kế).
                RestorePatientSidebarState();

                await InitConclusionDataAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi tải dữ liệu kết luận cho DiagnosticReportConclusionControl");
                MessageBox.Show(this, $"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Phải gọi từ FormClosing của Form host trước khi đóng, để dừng camera an toàn
        /// (giống FrmMain_FormClosing) - tránh dispose tài nguyên trong khi camera còn đang dừng.
        /// </summary>
        public async System.Threading.Tasks.Task StopCameraAsync()
        {
            try
            {
                if (_cameraControl != null)
                    await _cameraControl.StopCaptureAsync();
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Lỗi khi dừng camera lúc đóng DiagnosticReportConclusionControl");
            }
        }
    }
}
