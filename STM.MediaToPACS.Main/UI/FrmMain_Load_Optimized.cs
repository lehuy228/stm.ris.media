// ============================================
// ĐỀ XUẤT TỐI ƯU CHO FrmMain_Load
// ============================================
// File này chỉ là đề xuất, KHÔNG tự động apply
// Bạn cần review và chấp nhận trước khi áp dụng
// ============================================

private async void FrmMain_Load(object sender, EventArgs e)
{
    try
    {
        // ============================================
        // NHÓM 1: UI Setup (Nhanh, không block)
        // ============================================
        SetupUIComponents();
        
        // ============================================
        // NHÓM 2: Critical Initialization (Cần ngay)
        // ============================================
        await InitializeCriticalComponentsAsync();
        
        // ============================================
        // NHÓM 3: Background Loading (Có thể chạy sau)
        // ============================================
        _ = LoadBackgroundDataAsync(); // Fire and forget
        
        // ============================================
        // NHÓM 4: Final Setup
        // ============================================
        FinalizeInitialization();
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
    
    // Setup context menu
    SetupContextMenu();
    
    // Setup button texts
    SetupButtonTexts();
    
    // Setup fonts (cache font object để tránh tạo lại)
    SetupFonts();
    
    // Setup excluded tags (static data, không cần async)
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
    // Chạy song song các thao tác độc lập
    var initPermissionTask = Task.Run(() => InitPermissionControl());
    var initFormTask = Task.Run(() => InitializeForm());
    var initPrintersTask = Task.Run(() => InitCbbPrinters());
    
    // Chờ các thao tác critical hoàn thành
    await Task.WhenAll(initPermissionTask, initFormTask, initPrintersTask);
    
    // Các thao tác cần UI thread
    this.Invoke((MethodInvoker)delegate
    {
        InitUserInfo();
        SetServersComboBox(true);
        InitializeScreenCapture();
        UpdateToolBarState();
    });
    
    // Init thông tin chỉ định (có thể async)
    await InitThongTinChiDinhAsync();
}

// ============================================
// NHÓM 3: Background Loading - Chạy sau
// ============================================
private async Task LoadBackgroundDataAsync()
{
    try
    {
        // Chạy song song các thao tác độc lập
        var loadKTVTask = InitDanhSachKTV();
        var loadKetQuaTask = InitCheckKetQuaChanDoan();
        var loadImageTask = Task.Run(() => LoadImageData());
        
        // Chờ tất cả hoàn thành
        await Task.WhenAll(loadKTVTask, loadKetQuaTask, loadImageTask);
        
        // Các thao tác cần UI thread
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
    catch (Exception ex)
    {
        Log.Error(ex, "Lỗi khi load background data");
        // Không throw để không làm crash app
    }
}

private void LoadImageData()
{
    XmlSettingsHelper.EnsureFileExists(
        Path.Combine($"{_baseFolder}\\BenhNhan\\{_folderChiDinh}", FileNameXMLImage), 
        () => new List<string>());
    
    listImageKeyLocal = XmlSettingsHelper.Load<List<string>>(
        Path.Combine($"{_baseFolder}\\BenhNhan\\{_folderChiDinh}", FileNameXMLImage));
}

// ============================================
// NHÓM 4: Final Setup
// ============================================
private void FinalizeInitialization()
{
    // Các thao tác cuối cùng cần UI thread
    // Đã được xử lý trong LoadBackgroundDataAsync
}




