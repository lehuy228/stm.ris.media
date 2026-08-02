# TODO: Camera, ảnh kết luận và đẩy PACS trong màn hình kết luận

## Mục tiêu

Triển khai cơ chế trong màn hình kết luận để:

- Kết nối camera, chụp ảnh và hiển thị thumbnail ngay trên màn hình kết luận.
- Upload ảnh lên API attachment của `DiagnosticReport`.
- Chọn ảnh đưa vào PDF kết luận qua destination `document`.
- Chọn ảnh đẩy lên PACS/DICOM qua destination `pacs`.
- Thực hiện push ảnh PACS bằng API `pacs-push`.
- Đổi tên `FormMainV2` sang tên đúng nghiệp vụ.

## Quy tắc cập nhật TODO

- Mỗi việc khi bắt đầu làm phải chuyển từ `[ ]` sang `[~]`.
- Khi hoàn thành và đã build/test phần liên quan thì chuyển sang `[x]`.
- Nếu phát sinh việc mới, thêm vào đúng phase trong file này.
- Nếu blocked, ghi rõ lý do ngay dưới TODO đó bằng dòng `Blocked: ...`.
- Không xóa TODO đã hoàn thành; chỉ cập nhật trạng thái.

Ký hiệu:

- `[ ]` chưa làm
- `[~]` đang làm
- `[x]` hoàn thành
- `[!]` blocked/cần quyết định

---

## Phase 1: Refactor tên màn hình kết luận

- [x] Đổi tên class `FormMainV2` thành `DiagnosticReportConclusionControl`.
- [x] Di chuyển các file `UI/V2/FormMainV2*` sang thư mục nghiệp vụ mới, đề xuất `UI/DiagnosticReports/`.
- [x] Đổi tên toàn bộ partial files:
  - `FormMainV2.cs`
  - `FormMainV2.Designer.cs`
  - `FormMainV2.Camera.cs`
  - `FormMainV2.Loading.cs`
  - `FormMainV2.SaveLoad.cs`
  - `FormMainV2.Signature.cs`
  - `FormMainV2.Print.cs`
  - `FormMainV2.Suggestion.cs`
  - `FormMainV2.Helpers.cs`
  - `FormMainV2.PatientSidebar.cs`
  - `FormMainV2.resx`
- [x] Cập nhật namespace/class/constructor trong các file partial.
- [x] Cập nhật `STM.MediaToPACS.Main.csproj` cho Compile/EmbeddedResource mới.
- [x] Cập nhật nơi khởi tạo control trong `MainForm.cs`.
- [x] Cập nhật `FrmMainV2Host.cs` hoặc đổi tên host nếu vẫn cần host độc lập.
- [x] Build project tương ứng để xác nhận refactor không làm vỡ designer/partial class.
  - `dotnet build STM.MediaToPacs.Core\STM.MediaToPacs.Core.csproj`: pass.
  - `dotnet build STM.MediaToPacs.Infrastructure\STM.MediaToPacs.Infrastructure.csproj`: pass.
  - VS2019 MSBuild `STM.MediaToPACS.Main\STM.MediaToPACS.Main.csproj` Debug/x86: pass, tạo `STM.MediaToPACS.Main\bin\x86\Debug\STM.MediaToPacs.exe`.
  - Ghi chú: `dotnet build STM.MediaToPACS.sln` không phù hợp với WinForms/.NET Framework hiện tại do lỗi resource `MSB3822/MSB3823` trên .NET SDK preview. Khi build Main cần dùng MSBuild VS2019/Visual Studio tương ứng.

Tiêu chí hoàn thành:

- Solution build thành công.
- Màn hình kết luận vẫn mở được từ tab hiện tại.
- Camera preview/snapshot cũ vẫn hoạt động như trước.

---

## Phase 2: Bổ sung model attachment/PACS ở Core

- [x] Tạo model `DiagnosticReportAttachmentDto`.
- [x] Tạo model `DiagnosticReportAttachmentDestinationDto`.
- [x] Tạo model `UploadDiagnosticReportAttachmentsResult`.
- [x] Tạo model request `DocumentAttachmentSelectionRequest`.
- [x] Tạo model item `DocumentAttachmentSelectionItem`.
- [x] Tạo model request `PacsAttachmentSelectionRequest`.
- [x] Tạo model response `PacsPushResult`.
- [x] Tạo model item `PacsPushItemResult`.
- [x] Chuẩn hóa enum/string constants cho destination:
  - `document`
  - `pacs`
- [x] Chuẩn hóa enum/string constants cho status:
  - `pending`
  - `processing`
  - `completed`
  - `failed`
  - `AlreadyCompleted` nếu response push dùng PascalCase.

Tiêu chí hoàn thành:

- Model compile được.
- Tên field khớp JSON backend mục 9 tài liệu diagnostic-report.
- Không phá các model RIS V1 hiện có.

Kết quả kiểm tra:

- `dotnet build STM.MediaToPacs.Core\STM.MediaToPacs.Core.csproj` thành công.

---

## Phase 3: Bổ sung API client vào `IRisService2` / `RisService2`

- [x] Thêm endpoint constant `DiagnosticReports = Root + "/diagnostic-reports"` trong `ApiEndpoints.RisV2`.
- [x] Thêm method attachment dùng `orderItemId` theo route `/api/v1/order-items/{orderItemId}/attachments`.
- [x] Thêm method `GetDiagnosticReportAttachmentsAsync(Guid orderItemId)`.
- [x] Thêm method `UploadDiagnosticReportAttachmentsAsync(Guid orderItemId, IEnumerable<string> filePaths)`.
- [x] Thêm method `UpdateDocumentAttachmentSelectionAsync(Guid orderItemId, List<DocumentAttachmentSelectionItem> selections)`.
- [x] Thêm method `UpdatePacsAttachmentSelectionAsync(Guid orderItemId, List<Guid> attachmentIds)`.
- [x] Thêm method `PushDiagnosticReportAttachmentsToPacsAsync(Guid orderItemId, string targetServer = "MainStorage")`.
- [x] Thêm method stream ảnh nếu UI cần load ảnh từ server: `StreamDiagnosticReportAttachmentAsync(Guid orderItemId, Guid attachmentId)`.
- [x] Implement upload bằng `MultipartFormDataContent`, field name `files`.
- [x] Validate trước khi upload:
  - file tồn tại
  - file không rỗng
  - content-type hợp lệ
- [x] Map lỗi API thành message dễ hiểu cho UI:
  - `DICOM_SERVER_NOT_CONFIGURED`
  - `DIAGNOSTIC_REPORT_NOT_FOUND`
  - lỗi device chưa có AE Title
  - lỗi file không phải JPEG khi push PACS

Tiêu chí hoàn thành:

- `RisService2` compile.
- Có thể gọi GET/upload/selection/push bằng service.
- Không ảnh hưởng các method RIS V2 hiện có.

Kết quả kiểm tra:

- `dotnet build STM.MediaToPacs.Infrastructure\STM.MediaToPacs.Infrastructure.csproj` thành công.
- Build có 5 warning cũ `CS0168` trong `StudyService.cs` và `RisService.cs`, không liên quan thay đổi attachment.

---

## Phase 4: Mở rộng thumbnail list để quản lý attachment server

- [x] Mở rộng `ImageThumbnailList.ThumbnailItem` hoặc tạo control mới `DiagnosticReportAttachmentList`.
- [x] Bổ sung các field trên item:
  - `AttachmentId`
  - `FilePath`
  - `FileName`
  - `ContentType`
  - `DocumentSelected`
  - `PacsSelected`
  - `PacsStatus`
  - `ErrorDetail`
- [x] Hiển thị trạng thái chọn PDF/document bằng viền hoặc checkbox riêng.
- [x] Hiển thị trạng thái PACS bằng badge/text:
  - `PACS`
  - `Pending`
  - `Completed`
  - `Failed`
- [x] Thêm tooltip lỗi nếu destination PACS failed.
- [x] Thêm API lấy danh sách ảnh đang chọn PDF.
- [x] Thêm API lấy danh sách ảnh đang chọn PACS.
- [x] Thêm API lấy danh sách ảnh local chưa upload.
- [x] Đảm bảo dispose image/thumbnail đúng để không lock file local.

Tiêu chí hoàn thành:

- UI thumbnail hiển thị được ảnh local và ảnh đã upload.
- User phân biệt được ảnh chọn PDF và ảnh chọn PACS.
- Không leak/lock file khi đóng màn hình.

---

## Phase 5: Load attachment khi mở màn hình kết luận

- [x] Sau `ResolveRisV1OrderItemAsync`, lấy `orderItemId` từ `_risOrderItem.id`.
- [x] Nếu có `orderItemId`, gọi `GetDiagnosticReportAttachmentsAsync(orderItemId)`.
- [x] Bind attachment server vào thumbnail list.
- [x] Với ảnh server, preview dùng endpoint stream:
  - `/order-items/{orderItemId}/attachments/{attachmentId}/stream`
- [x] Nếu chưa có report, giữ trạng thái ảnh local như hiện tại.
- [x] Xử lý lỗi load attachment theo kiểu best-effort, không chặn load kết luận.

Tiêu chí hoàn thành:

- Mở lại màn hình thấy danh sách ảnh đã upload.
- Trạng thái document/pacs được restore từ `destinations`.
- Lỗi attachment không làm mất khả năng nhập kết luận.

---

## Phase 6: Sửa luồng chụp ảnh camera

- [x] Giữ luồng snapshot local hiện tại để UI phản hồi nhanh.
- [x] Sau khi snapshot, add ảnh vào thumbnail list với trạng thái local pending upload.
- [x] Nếu đã có `orderItemId`, upload ngay ảnh vừa chụp lên attachments.
- [x] Sau upload thành công, cập nhật `AttachmentId` cho thumbnail item.
- [x] Mặc định chọn ảnh mới chụp vào destination `document`.
- [x] Sau upload, gọi lại `document-selection` full-replace.
- [x] Nếu chưa có `orderItemId`, chờ resolve y lệnh RIS mới rồi upload.
- [x] Không tự chọn PACS cho ảnh mới chụp.

Tiêu chí hoàn thành:

- Chụp ảnh vẫn nhanh, không phụ thuộc mạng ở bước hiển thị local.
- Nếu đã có report, ảnh được upload lên server.
- Ảnh mới chụp mặc định xuất hiện trong danh sách ảnh PDF.

---

## Phase 7: Sửa luồng lưu nháp kết luận

- [x] Giữ phần lưu text kết luận hiện tại.
- [x] Sau khi lưu/upsert kết luận, xác định chắc chắn `orderItemId`.
- [x] Upload toàn bộ ảnh local chưa có `AttachmentId`.
- [x] Sau khi upload, cập nhật thumbnail item bằng attachment trả về.
- [x] Gọi `document-selection` bằng toàn bộ danh sách ảnh đang chọn PDF.
- [x] Nếu có ảnh chọn PACS, gọi `pacs-selection` bằng toàn bộ danh sách ảnh đang chọn PACS.
- [x] Không còn coi `imageFileKeys` base64 là nguồn chính cho ảnh kết luận mới.
- [x] Nếu vẫn cần tương thích API cũ, giữ base64 như fallback có kiểm soát và ghi chú rõ trong code.

Tiêu chí hoàn thành:

- Lưu nháp vừa lưu text, vừa đồng bộ attachment.
- Full-replace selection không làm mất ảnh đã chọn trước đó.
- Mở lại màn hình vẫn thấy ảnh và selection đúng.

---

## Phase 8: Thêm luồng chọn và đẩy ảnh PACS

- [x] Thêm nút `Đẩy PACS` vào khu vực camera/thumbnail.
- [x] Thêm thao tác chọn/bỏ chọn PACS trên từng thumbnail.
- [x] Chỉ cho chọn PACS với `image/jpeg`.
- [x] Nếu user chọn file không phải JPEG vào PACS, hiển thị cảnh báo rõ.
- [x] Khi bấm `Đẩy PACS`, kiểm tra có `orderItemId`.
- [x] Nếu chưa có `orderItemId`, báo chưa resolve được y lệnh RIS mới.
- [x] Upload ảnh local pending trước khi push.
- [x] Gọi `pacs-selection` full-replace với danh sách attachment chọn PACS.
- [x] Gọi `pacs-push`.
- [x] Sau push, gọi lại `GET attachments` để refresh trạng thái.
- [x] Hiển thị kết quả:
  - số ảnh push thành công
  - số ảnh đã completed từ trước
  - số ảnh failed
  - chi tiết lỗi nếu có

Tiêu chí hoàn thành:

- User có thể chọn ảnh JPEG và push lên PACS.
- UI phản ánh trạng thái completed/failed.
- Lỗi cấu hình DICOM server/device AE Title được hiển thị dễ hiểu.

---

## Phase 9: Kiểm thử build và nghiệp vụ

- [x] Build solution bằng `dotnet build` hoặc build tương ứng của solution hiện tại.
  - Đã build tương ứng bằng VS2019 MSBuild cho Main Debug/x86. Build pass, còn warning cũ LEADTOOLS/async/unused không chặn compile.
- [ ] Test mở màn hình kết luận.
- [ ] Test preview camera.
- [ ] Test chụp ảnh local.
- [ ] Test lưu nháp với ảnh document.
- [ ] Test mở lại màn hình và restore attachment.
- [ ] Test bỏ chọn toàn bộ ảnh PDF bằng `document-selection` rỗng.
- [ ] Test chọn lại thứ tự ảnh PDF.
- [ ] Test chọn PACS với JPEG.
- [ ] Test chặn hoặc cảnh báo PACS với PNG/WebP/BMP.
- [ ] Test push PACS khi thiếu DICOM server config.
- [ ] Test push PACS khi device thiếu AE Title.
- [ ] Test push PACS thành công vào PACS test.
- [ ] Test push lại cùng study để xác minh backend/PACS xử lý ảnh cũ như mong muốn.

Tiêu chí hoàn thành:

- Không lỗi build.
- Các luồng chính chạy được trên môi trường test.
- Có ghi nhận rõ hành vi re-push PACS.

---

## Các quyết định cần chốt

- [x] Có tự động lưu nháp khi user bấm `Đẩy PACS` mà chưa có report không?
  - Đã áp dụng: không tự động; yêu cầu user lưu nháp trước để tránh tạo report ngoài ý muốn.

- [x] Khi chụp ảnh xong có tự động chọn đưa vào PDF không?
  - Đã áp dụng: có.

- [x] Khi chụp ảnh xong có tự động chọn PACS không?
  - Đã áp dụng: không.

- [!] Re-push PACS có thật sự override ảnh cũ không?
  - Cần test với PACS mục tiêu. Nếu backend sinh SOP Instance UID mới, nhiều PACS sẽ giữ cả ảnh cũ và ảnh mới thay vì override.

- [!] Có cần hỗ trợ PNG/WebP/BMP cho PACS bằng cách convert sang JPEG trước khi upload/push không?
  - Đề xuất phase đầu chỉ hỗ trợ JPEG đúng theo backend hiện tại.

---

## Ghi chú kỹ thuật từ tài liệu API

- Base route attachment:
  - `/api/v1/order-items/{orderItemId}/attachments`
- Upload:
  - `POST /attachments/batch`
  - multipart field `files`
  - tối đa 15 file/lần
  - tối đa 50 MB/file
- Chọn ảnh PDF:
  - `PUT /attachments/document-selection`
  - full-replace
  - gửi rỗng là bỏ chọn toàn bộ
- Chọn ảnh PACS:
  - `PUT /attachments/pacs-selection`
  - full-replace
  - gửi rỗng là bỏ chọn toàn bộ
- Đẩy PACS:
  - `POST /attachments/pacs-push?targetServer=MainStorage`
  - chỉ xử lý destination `pacs` chưa completed
  - dùng DICOM C-STORE
  - hiện chỉ hỗ trợ JPEG
- Stream ảnh:
  - `GET /attachments/{attachmentId}/stream`
  - RIS kiểm tra auth/permission rồi stream từ MinIO nội bộ
