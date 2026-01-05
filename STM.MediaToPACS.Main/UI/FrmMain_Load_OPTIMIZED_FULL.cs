// ============================================
// CODE TỐI ƯU HOÀN CHỈNH CHO FrmMain_Load
// ============================================
// File này chứa code đã được tối ưu
// Bạn cần COPY và PASTE vào FrmMain.cs
// KHÔNG tự động apply - cần review trước
// ============================================

// ============================================
// THAY THẾ METHOD FrmMain_Load HIỆN TẠI
// ============================================
private async void FrmMain_Load(object sender, EventArgs e)
{
    try
    {
        // NHÓM 1: UI Setup (Nhanh, không block)
        SetupUIComponents();

        // NHÓM 2: Critical Initialization (Cần ngay)
        await InitializeCriticalComponentsAsync();

        // NHÓM 3: Background Loading (Có thể chạy sau, không block UI)
        _ = LoadBackgroundDataAsync(); // Fire and forget

        // NHÓM 4: Final Setup
        // Lưu ý: Các thao tác cuối cùng đã được xử lý trong LoadBackgroundDataAsync():
        // - CreateCStoreObject()
        // - UpdateToolBarState() (đã gọi trong InitializeCriticalComponentsAsync)
        // - CheckFirstRun()
        // - Save settings
        // Do đó FinalizeInitialization() không cần thiết nữa
    }
    catch (Exception Ex)
    {
        MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

// ============================================
// NHÓM 1: UI Setup - Chạy ngay, không block
// ============================================
private void SetupUIComponents()
{
    UpdateCameraSize();
    SetupContextMenu();
    SetupButtonTexts();
    SetupFonts();
    SetupExcludedTags();
}

private void SetupContextMenu()
{
    contextMenuRichTextBox.Items.Add("Sao chép", null, (s, ev) => GetCurrentBox()?.Copy());
    contextMenuRichTextBox.Items.Add("Dán", null, (s, ev) => GetCurrentBox()?.Paste());
    contextMenuRichTextBox.Items.Add("Cắt", null, (s, ev) => GetCurrentBox()?.Cut());
    contextMenuRichTextBox.Items.Add("Chọn tất cả", null, (s, ev) => GetCurrentBox()?.SelectAll());

    _rtKetLuan.ContextMenuStrip = contextMenuRichTextBox;
    _rtKhuyenNghi.ContextMenuStrip = contextMenuRichTextBox;
    _rtMoTa.ContextMenuStrip = contextMenuRichTextBox;
}

private void SetupButtonTexts()
{
    var keys = ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys;

    _btnCancel.Text = $"Hủy ({keys.Exit})";
    _btnPreviewMain.Text = $"Xem trước ({keys.Preview})";
    _btnPrint.Text = $"In ({keys.Print})";
    _btnSave.Text = $"Lưu nháp ({keys.Draft})";
    _btnSignature.Text = $"Ký số ({keys.Sign})";
    _btnSnapshot.Text = $"Chụp nhanh ({keys.Snapshot})";
    _btnLinkCamera.Text = $"Liên kết ({keys.LinkCamera})";
    _btnStop.Text = $"Dừng ({keys.Stop})";
}

private void SetupFonts()
{
    // Cache font object để tránh tạo lại nhiều lần
    var fontSettings = ServiceLocator.ShortcutAndFontSetting.FontSettings;
    var font = new Font(fontSettings.FontFamily, fontSettings.FontSize);

    _rtKetLuan.Font = font;
    _rtKhuyenNghi.Font = font;
    _rtMoTa.Font = font;
}

private void SetupExcludedTags()
{
    _ExcludedTags.AddRange(new[]
    {
        DicomTag.SOPClassUID,
        DicomTag.SOPInstanceUID,
        DicomTag.StudyInstanceUID,
        DicomTag.SeriesInstanceUID,
        DicomTag.MediaStorageSOPClassUID,
        DicomTag.FrameIncrementPointer,
        DicomTag.MIMETypeOfEncapsulatedDocument,
        DicomTag.PageNumberVector
    });
}

// ============================================
// NHÓM 2: Critical Initialization - Cần ngay
// ============================================
private async Task InitializeCriticalComponentsAsync()
{
    // QUAN TRỌNG: FrmMain_Load chạy trên UI thread
    // Do đó tất cả code trong đây cũng chạy trên UI thread
    // KHÔNG dùng Task.Run() cho các thao tác UI
    
    // Các thao tác cần UI thread - chạy trực tiếp (đã trên UI thread)
    InitPermissionControl();      // ✅ Chạy trên UI thread
    InitializeForm();              // ✅ Chạy trên UI thread
    InitCbbPrinters();             // ✅ Chạy trên UI thread
    InitUserInfo();                // ✅ Chạy trên UI thread
    SetServersComboBox(true);      // ✅ Chạy trên UI thread
    InitializeScreenCapture();     // ✅ Chạy trên UI thread
    UpdateToolBarState();          // ✅ Chạy trên UI thread

    // Init thông tin chỉ định (async, có thể chạy song song)
    // Method này có await bên trong nhưng vẫn chạy trên UI thread
    // Chỉ phần await sẽ chờ, không block UI thread
    await InitThongTinChiDinhAsync();
}

// ============================================
// NHÓM 3: Background Loading - Chạy sau
// ============================================
private async Task LoadBackgroundDataAsync()
{
    try
    {
        // Chạy song song các thao tác async độc lập
        var loadKTVTask = InitDanhSachKTVAsync();
        var loadKetQuaTask = InitCheckKetQuaChanDoanAsync();
        var loadImageTask = Task.Run(() => LoadImageData());

        // Chờ tất cả hoàn thành
        await Task.WhenAll(loadKTVTask, loadKetQuaTask, loadImageTask);

        // Các thao tác cần UI thread - phải invoke về UI thread
        if (this.InvokeRequired)
        {
            this.Invoke((MethodInvoker)delegate
            {
                LoadImageVideoCaptured();

                if (result != null)
                {
                    InitTranfer(result);
                }
                else
                {
                    InitTranferRIS();
                }

                CreateCStoreObject(new MyServer());
                _captureType = CaptureType.None;
                CheckFirstRun();
                _mySettings._settings.FirstRun = false;
                _mySettings.Save();
            });
        }
        else
        {
            LoadImageVideoCaptured();

            if (result != null)
            {
                InitTranfer(result);
            }
            else
            {
                InitTranferRIS();
            }

            CreateCStoreObject(new MyServer());
            _captureType = CaptureType.None;
            CheckFirstRun();
            _mySettings._settings.FirstRun = false;
            _mySettings.Save();
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Lỗi khi load background data");
        // Không throw để không làm crash app
    }
}

// ============================================
// HELPER METHODS - Chuyển đổi async void thành async Task
// ============================================

/// <summary>
/// Chuyển đổi InitDanhSachKTV từ async void thành async Task
/// </summary>
private async Task InitDanhSachKTVAsync()
{
    try
    {
        // Lấy danh sách KTV từ RIS service
        _listHisUser = (await ServiceLocator.RisService.GetDSNguoidungAsync()).data;

        // Cập nhật UI trên UI thread
        if (this.InvokeRequired)
        {
            this.Invoke((MethodInvoker)delegate
            {
                ConfigureKTVLookup();
            });
        }
        else
        {
            ConfigureKTVLookup();
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Lỗi tải danh sách KTV");
        // Có thể hiển thị message box nếu cần
    }
}

/// <summary>
/// Tách phần cấu hình UI ra method riêng
/// </summary>
private void ConfigureKTVLookup()
{
    // Gán dữ liệu vào LookUpEdit
    _cbbHisUser.Properties.DataSource = _listHisUser;
    _cbbHisUser.Properties.DisplayMember = "full_name";
    _cbbHisUser.Properties.ValueMember = "id";
    _cbbHisUser.Properties.NullText = "Chọn KTV...";

    // Bật tìm kiếm & lọc
    _cbbHisUser.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
    _cbbHisUser.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
    _cbbHisUser.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;

    // Hiển thị cột
    _cbbHisUser.Properties.PopulateColumns();
    _cbbHisUser.Properties.Columns["id"].Visible = false;
    _cbbHisUser.Properties.Columns["user_name"].Visible = false;
    _cbbHisUser.Properties.Columns["is_synced"].Visible = false;

    _cbbHisUser.Properties.Columns["full_name"].Caption = "Tên KTV";
    _cbbHisUser.Properties.Columns["his_id"].Caption = "Mã KTV";

    // Nếu có dữ liệu
    if (_listHisUser?.Count > 0)
    {
        // Tìm user có his_id = "4"
        var selectedUser = _listHisUser.FirstOrDefault(u => u.his_id == "4");

        if (selectedUser != null)
            _cbbHisUser.EditValue = selectedUser.id;
        else
            _cbbHisUser.EditValue = _listHisUser.First().id;
    }
}

/// <summary>
/// Chuyển đổi InitCheckKetQuaChanDoan từ async void thành async Task
/// </summary>
private async Task InitCheckKetQuaChanDoanAsync()
{
    try
    {
        _kqChanDoanResponse = await ServiceLocator.RisService.GetKetQuaChanDoanAsync(machidinh);
        
        // Cập nhật UI trên UI thread
        if (this.InvokeRequired)
        {
            this.Invoke((MethodInvoker)delegate
            {
                UpdateUIFromKetQuaChanDoan();
            });
        }
        else
        {
            UpdateUIFromKetQuaChanDoan();
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Lỗi khi load kết quả chẩn đoán");
    }
}

/// <summary>
/// Tách phần cập nhật UI ra method riêng
/// </summary>
private void UpdateUIFromKetQuaChanDoan()
{
    if (_kqChanDoanResponse != null)
    {
        _rtMoTa.Text = _kqChanDoanResponse.Kqcls_MoTa ?? "";
        _rtKetLuan.Text = _kqChanDoanResponse.Kqcls_KetLuan ?? "";
        _rtKhuyenNghi.Text = _kqChanDoanResponse.Kqcls_DeNghi ?? "";

        bool isNhap = _kqChanDoanResponse.TrangThai != null && _kqChanDoanResponse.TrangThai.Equals(TrangThaiKetLuan.NHAP);
        _btnSave.Enabled = isNhap;
        _btnPrint.Enabled = true;
        _rtMoTa.Enabled = isNhap;
        _rtKetLuan.Enabled = isNhap;
        _rtKhuyenNghi.Enabled = isNhap;
        
        if (isNhap)
        {
            _btnSignature.Text = $"Ký số ({ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.Sign})";
        }
        else
        {
            _btnSignature.Text = $"Hủy ký số({ServiceLocator.ShortcutAndFontSetting.ConclusionScreenKeys.Sign})";
        }
    }
    else
    {
        _tsmPreviewPdf.Enabled = false;
        _btnPreviewMain.Enabled = false;
        _btnPrint.Enabled = false;
        _btnSignature.Enabled = false;
    }
}

/// <summary>
/// Load image data từ file system (chạy trên background thread)
/// </summary>
private void LoadImageData()
{
    try
    {
        XmlSettingsHelper.EnsureFileExists(
            Path.Combine($"{_baseFolder}\\BenhNhan\\{_folderChiDinh}", FileNameXMLImage),
            () => new List<string>());

        listImageKeyLocal = XmlSettingsHelper.Load<List<string>>(
            Path.Combine($"{_baseFolder}\\BenhNhan\\{_folderChiDinh}", FileNameXMLImage));
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Lỗi khi load image data");
    }
}

// ============================================
// GIỮ NGUYÊN CÁC METHOD SAU (KHÔNG CẦN SỬA)
// ============================================
// - UpdateCameraSize()
// - InitPermissionControl()
// - InitializeForm()
// - InitCbbPrinters()
// - InitUserInfo()
// - SetServersComboBox()
// - InitializeScreenCapture()
// - UpdateToolBarState()
// - InitThongTinChiDinhAsync()
// - LoadImageVideoCaptured()
// - InitTranfer()
// - InitTranferRIS()
// - CreateCStoreObject()
// - CheckFirstRun()
// ============================================

// ============================================
// LƯU Ý QUAN TRỌNG:
// ============================================
// 1. Các method async void cũ (InitDanhSachKTV, InitCheckKetQuaChanDoan)
//    vẫn giữ nguyên để tương thích với code khác, nhưng sẽ được gọi qua
//    các method async Task mới (InitDanhSachKTVAsync, InitCheckKetQuaChanDoanAsync)
//
// 2. Nếu bạn muốn xóa hoàn toàn các method async void cũ, có thể:
//    - Xóa InitDanhSachKTV() và InitCheckKetQuaChanDoan()
//    - Đổi tên InitDanhSachKTVAsync() thành InitDanhSachKTV()
//    - Đổi tên InitCheckKetQuaChanDoanAsync() thành InitCheckKetQuaChanDoan()
//
// 3. Method LoadImageData() chạy trên background thread vì chỉ đọc file,
//    không cần UI thread
//
// 4. Tất cả các thao tác cập nhật UI đều được đảm bảo chạy trên UI thread
//    thông qua Invoke() hoặc kiểm tra InvokeRequired
// ============================================

