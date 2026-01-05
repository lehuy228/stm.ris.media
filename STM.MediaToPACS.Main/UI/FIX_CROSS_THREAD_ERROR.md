# 🔧 SỬA LỖI CROSS-THREAD OPERATION

## ❌ Lỗi gặp phải
```
Cross-thread operation not valid
→ Bạn đang set giá trị control (_lbUserName) từ thread khác UI thread
```

## 🔍 Nguyên nhân

Lỗi này xảy ra khi:
1. Method `InitPermissionControl()` được gọi từ background thread
2. Hoặc có code nào đó đang dùng `Task.Run()` để chạy các method cần UI thread

## ✅ Giải pháp

### Cách 1: Đảm bảo chạy trên UI thread (Đã sửa trong code)

`FrmMain_Load` là event handler của form, **LUÔN chạy trên UI thread**. Do đó:

```csharp
private async Task InitializeCriticalComponentsAsync()
{
    // KHÔNG dùng Task.Run() cho các thao tác UI
    // Tất cả code này chạy trên UI thread
    
    InitPermissionControl();      // ✅ OK - chạy trên UI thread
    InitializeForm();              // ✅ OK - chạy trên UI thread
    InitCbbPrinters();             // ✅ OK - chạy trên UI thread
    // ...
}
```

### Cách 2: Nếu vẫn gặp lỗi, thêm InvokeRequired check

Nếu bạn vẫn gặp lỗi (có thể do code khác gọi), thêm check:

```csharp
private void InitPermissionControl()
{
    // Đảm bảo chạy trên UI thread
    if (this.InvokeRequired)
    {
        this.Invoke((MethodInvoker)delegate
        {
            InitPermissionControlInternal();
        });
        return;
    }
    
    InitPermissionControlInternal();
}

private void InitPermissionControlInternal()
{
    var userInfo = ServiceLocator.SessionService.GetCurrentUser();
    string fullName = string.Join(" ", new[] { userInfo.LastName, userInfo.FirstName }
        .Where(s => !string.IsNullOrWhiteSpace(s)));

    _lbUserName.Text = $"{userInfo.Username}";
    _txNguoiDung.Text = $"{fullName}";
}
```

## 📋 Checklist các method cần UI thread

Các method sau **PHẢI** chạy trên UI thread:
- ✅ `InitPermissionControl()` - Set `_lbUserName.Text`, `_txNguoiDung.Text`
- ✅ `InitializeForm()` - Thêm controls vào form
- ✅ `InitCbbPrinters()` - Set properties của `_cbbPrinter`
- ✅ `InitUserInfo()` - Set text controls
- ✅ `SetServersComboBox()` - Set combobox items
- ✅ `InitializeScreenCapture()` - Có thể cần UI thread
- ✅ `UpdateToolBarState()` - Update toolbar buttons
- ✅ `LoadImageVideoCaptured()` - Load images vào UI
- ✅ `InitTranfer()` / `InitTranferRIS()` - Update DICOM info
- ✅ `CreateCStoreObject()` - Có thể cần UI thread
- ✅ `CheckFirstRun()` - Update UI state

## 🚫 KHÔNG BAO GIỜ làm thế này:

```csharp
// ❌ SAI - Chạy UI operation trên background thread
await Task.Run(() => {
    InitPermissionControl();  // ❌ LỖI!
});

// ❌ SAI - Task.Run cho UI operations
var task = Task.Run(() => {
    InitializeForm();  // ❌ LỖI!
});
```

## ✅ ĐÚNG - Cách làm:

```csharp
// ✅ ĐÚNG - Chạy trực tiếp (đã trên UI thread)
InitPermissionControl();

// ✅ ĐÚNG - Nếu cần async, chỉ await phần async
await InitThongTinChiDinhAsync();  // Method này có await bên trong

// ✅ ĐÚNG - Background work + Invoke về UI thread
await Task.Run(() => {
    // Background work - không chạm UI
    var data = LoadDataFromFile();
    
    // Sau đó invoke về UI thread
    this.Invoke((MethodInvoker)delegate {
        UpdateUI(data);  // ✅ OK - đã trên UI thread
    });
});
```

## 🔍 Debug

Nếu vẫn gặp lỗi, kiểm tra:

1. **Xem stack trace**: Method nào đang gọi từ background thread?
2. **Kiểm tra Task.Run()**: Có chỗ nào dùng `Task.Run()` cho UI operations không?
3. **Kiểm tra async/await**: Có method nào đang await từ background thread không?

## 📝 Code đã được sửa

File `FrmMain_Load_OPTIMIZED_FULL.cs` đã được cập nhật:
- ✅ Loại bỏ `Task.Run()` cho UI operations
- ✅ Đảm bảo tất cả UI operations chạy trên UI thread
- ✅ Chỉ dùng `Task.Run()` cho file I/O (LoadImageData)




