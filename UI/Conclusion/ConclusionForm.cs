using Leadtools.Demos;
using Newtonsoft.Json.Linq;
using QRCoder;
using SignLib;
using SignLib.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static PrintToPACSDemo.FrmMain;
using DateTime = System.DateTime;
using System.Xml.Serialization;
using PrintToPACSDemo.UI.Login;
using PrintToPACSDemo.AnPhat.Data;
using PrintToPACSDemo.AnPhatData;
using System.Dynamic;

namespace PrintToPACSDemo.UI.Conclusion
{
    public partial class ConclusionForm : DevExpress.XtraEditors.XtraForm
    {
        //File doc teamplate mẫu 
        private string templateDocxPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Phiếu trả kết luận.docx");
        private string saveStorePath;
        private List<Image> images = new List<Image>();
        private PrintToPACSDemo.AnPhatData.Conclusion conclusion;
        private ImageServiceLocal imageService;
        private List<string> conclusionImages = new List<string>();

        public ConclusionForm(PrintToPACSDemo.AnPhatData.Conclusion conclusion, List<string> conclusionImages)
        {
            InitializeComponent();

            this.conclusion = conclusion;
            this.conclusionImages = conclusionImages;
        }

        private int CaculatorDate(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;

            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        private void LoadInfoOrder()
        {
            labelPatientName.Text = string.IsNullOrEmpty(conclusion.PatientName) ? "" : conclusion.PatientName;
            labelImagingService.Text = string.IsNullOrEmpty(imageService.HospitalServiceName) ? "" : imageService.HospitalServiceName;
            textBoxPatientGender.Text = string.IsNullOrEmpty(conclusion.PatientGender) ? "" : conclusion.PatientGender;
            textBoxPatientDoB.Text = (conclusion.PatientDoB == null) ? "" : CaculatorDate(conclusion.PatientDoB).ToString();
            textBoxPatientID.Text = string.IsNullOrEmpty(conclusion.PatientID) ? "" : conclusion.PatientID;
            textBoxStudyInstanceUID.Text = string.IsNullOrEmpty(conclusion.StudyInstanceUID) ? "" : conclusion.StudyInstanceUID;
            textBoxHealthIdentificationCode.Text = string.IsNullOrEmpty(conclusion.HealthIdentificationCode) ? "" : conclusion.HealthIdentificationCode;
            textBoxMedicalCreateAt.Text = (conclusion.MedicalImagingCreateAt == null) ? "" : conclusion.MedicalImagingCreateAt.ToString();
            textBoxMedicalReportedAt.Text = (conclusion.MedicalImagingReportedAt == null) ? "" : conclusion.MedicalImagingReportedAt.ToString();
            textBoxMedicalCode.Text = string.IsNullOrEmpty(conclusion.MedicalImagingCode) ? "" : conclusion.MedicalImagingCode;
            textBoxOrderingPhysician.Text = string.IsNullOrEmpty(conclusion.OrderingPhysician) ? "" : conclusion.OrderingPhysician;
            textBoxRadiologist.Text = string.IsNullOrEmpty(conclusion.Radiologist) ? "" : conclusion.Radiologist;
            textBoxTechnicians.Text = string.IsNullOrEmpty(conclusion.Technicians) ? "" : conclusion.Technicians;
            textBoxDeviceName.Text = string.IsNullOrEmpty(conclusion.DeviveName) ? "" : conclusion.DeviveName;
        }

        private void UpdateControl()
        {
            int cells = images.Count;
            if (cells <= 1)
            {
                UpdateViewTableLayout(1, 1);
            }
            else if (cells <= 2)
            {
                UpdateViewTableLayout(2, 1);
            }
            else if (cells <= 4)
            {
                UpdateViewTableLayout(2, 2);
            }
            else if (cells <= 6)
            {
                UpdateViewTableLayout(3, 2);
            }
            else if (cells <= 8)
            {
                UpdateViewTableLayout(4, 2);
            }
        }

        private void AddImageToConclusionLocal(Image image, string path)
        {
            images.Add(image);
            UpdateControl();
            ImageControl imageControl = new ImageControl();
            imageControl.imagePath = path;
            imageControl.Dock = DockStyle.Fill;
            imageControl.AddImage(image);
            tablePanelImageConclusion.Controls.Add(imageControl);
        }

        private void LoadInfoConclusion()
        {
            foreach (string imageControl in conclusionImages)
            {
                using (FileStream fs = new FileStream(imageControl, FileMode.Open, FileAccess.Read))
                {
                    Image image = Image.FromStream(fs);
                    AddImageToConclusionLocal(image, imageControl);
                }
            }
            richTextBoxDiagnoseInfo.Text = string.IsNullOrEmpty(imageService.ServiceSampleDescription) ? "" : imageService.ServiceSampleDescription;
            richTextBoxDiagnoseResult.Text = string.IsNullOrEmpty(imageService.ServiceSampleConclusion) ? "" : imageService.ServiceSampleConclusion;
            richTextBoxDiagnoseNote.Text = string.IsNullOrEmpty(imageService.SampleInstructions) ? "" : imageService.SampleInstructions;
        }

        private void LoadConclusionLocal()
        {
            //imageService = BaseEntity.getByField<ImageServiceLocal>("HospitalServiceCode", conclusion.ImagingServiceCode);
            imageService = new ImageServiceLocal()
            {
                Id = 01,
                HospitalServiceCode = "0001",
                HospitalServiceName = "Chụp cộng hưởng từ tầng bụng có tiêm chất tương phản (gồm: chụp cộng hưởng từ gan-mật, tụy, lách, thận, dạ dày-tá tràng...) (0.2-1.5T)",
                MoHServiceCode = "",
                MoHServiceName = "",
                SampleInstructions = "",
                ServiceSampleDescription = "Kỹ Thuật: Chụp MRI gan mật với các chuỗi xung Inphase, Oppose-phase, T1W, T2W, DWI axial, HASTE coronal và chuỗi xung đường mật MRCP. Trước và sau tiêm thuốc đối quang từ\\r\\n·    Gan kích thước bình thường, bờ đều. Nhu mô gan hạ phân thùy VII-VIII gan phải sát bao gan có khối bờ không đều, ranh giới rõ, kích thước 30x50mm: giảm tín hiệu trên xung T1W, tăng tín hiệu trên xung T2W , hạn chế khuếch tán trên xung DWI, sau tiêm ngấm thuốc dạng chấm nốt từ trung tâm vào ngoại vi, đầy thuốc ở thì muộn. Ngoài ra hạ phân thùy VIII có thêm 02 nốt đường kính 3-4mm và kích thước 16x17mm có tín hiệu và tính chất ngấm thuốc tương tự khối trên. Nhu mô gan còn lại không thấy bất thường.\\r\\n·    Tĩnh mạch cửa: không giãn, không có huyết khối.\\r\\n·    Đường mật trong và ngoài gan: không giãn, không thấy dày thành, không thấy sỏi hay cấu trúc choán chỗ.\\r\\n·    Túi mật: hình dạng và cấu trúc bình thườn, thành đều. Dịch mật trong và đồng nhất, không thấy sỏi.\\r\\n·    Tụy: hình dạng và cấu trúc bình thường, nhu mô đồng nhất, không có khối khu trú. Ống tụy không giãn, không có sỏi. Bờ tụy đều, xung quanh không có dịch.\\r\\n·    Lách: hình dạng và cấu trúc bình thường.\\r\\n·    Không có dịch tự do ổ bụng.\\r\\n·    Hai thận có hình thái, kích thước bình thường, nhu mô thận phải có nang 5mm, đài bể thận hai bên không giãn, không thấy cấu trúc choán chỗ.\\r\\n·    Niệu quản hai bên không giãn.\r\n",
                ServiceSampleConclusion = "-Hình ảnh vài U máu (Hemagioma) gan phải. -Nang nhỏ thận phải."
            };
            LoadInfoOrder();
            LoadInfoConclusion();
        }

        private void DeleteButtonActionClick(object sender, EventArgs e)
        {
            if (sender is ImageControl imageControl)
            {
                tablePanelImageConclusion.Controls.Remove(imageControl);
                conclusionImages.Remove(conclusionImages.FirstOrDefault(item => item == imageControl.imagePath));
                images.Remove(imageControl.GetImageToPicturebox());
                imageControl.Dispose();
                if (File.Exists(imageControl.imagePath))
                {
                    File.Delete(imageControl.imagePath);
                }
                UpdateControl();
            }
        }

        private void UpdateViewTableLayout(int columns, int row)
        {
            var x = tabPageImage.Width;
            var y = tabPageImage.Height;
            tablePanelImageConclusion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(1)))), ((int)(((byte)(1)))));
            tablePanelImageConclusion.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            tablePanelImageConclusion.ColumnCount = columns;
            for (int i = 0; i < columns; i++)
            {
                tablePanelImageConclusion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
            }
            tablePanelImageConclusion.RowCount = row;
            for (int i = 0; i < row; i++)
            {
                tablePanelImageConclusion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
            }
            var edge = Math.Min((x - 6) / columns, (y - 20) / row);
            tablePanelImageConclusion.Size = new Size(edge * columns, edge * row);
            tablePanelImageConclusion.TabIndex = 0;
            var mgl = (x - edge * columns) / 2;
            tablePanelImageConclusion.Location = new System.Drawing.Point(mgl, y / 2 - (edge * row) / 2);
        }

        private void ReplaceTextInRange(Microsoft.Office.Interop.Word.Range range, string findText, string replaceText)
        {
            range.Find.ClearFormatting();
            range.Find.Text = findText;
            range.Find.Forward = true;
            range.Find.Wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue;
            range.Find.Format = false;
            range.Find.MatchCase = false;
            range.Find.MatchWholeWord = false;
            range.Find.MatchSoundsLike = false;
            range.Find.MatchAllWordForms = false;

            while (range.Find.Execute())
            {
                range.Text = replaceText;
                range.Collapse(Microsoft.Office.Interop.Word.WdCollapseDirection.wdCollapseEnd);
            }
        }

        private string InitQRCode()
        {
            string totpUrl = $"https://ris.anphat.ai.vn/shootingInformation/{ShareLinksID}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(totpUrl, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            var qrCodeAsBitmap = qrCode.GetGraphic(2);
            Image image = qrCodeAsBitmap;
            image.Save(Path.Combine(saveStorePath, "QRCode.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
            return Path.Combine(saveStorePath, "QRCode.jpg");
        }

        private int pageNumber;
        private void CreateWordDocument(string templatePath, string savePath, bool checkFileDoc)
        {
            string savePathDoc = Path.Combine(saveStorePath, $"{conclusion.StudyInstanceUID.Trim()}NewDoc.docx"); ;
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document doc = null;
            try
            {

                conclusion.DiagnoseInfo = richTextBoxDiagnoseInfo.Text;
                conclusion.DiagnoseResult = richTextBoxDiagnoseResult.Text;
                conclusion.DiagnoseNote = richTextBoxDiagnoseNote.Text;

                if (!File.Exists(templatePath))
                {
                    MessageBox.Show("Template file not found.");
                    return;
                }

                doc = wordApp.Documents.Open(templatePath);

                // Lưu tài liệu mẫu thành bản sao để thực hiện thay đổi
                doc.SaveAs2(savePathDoc);
                doc.Close(false);

                // Mở bản sao để thực hiện thay đổi
                doc = wordApp.Documents.Open(savePathDoc);

                Microsoft.Office.Interop.Word.Range range = doc.Content;
                ReplaceTextInRange(range, "{PatientName}", String.IsNullOrEmpty(conclusion.PatientName) ? "" : conclusion.PatientName);
                ReplaceTextInRange(range, "{PatientUID}", String.IsNullOrEmpty(conclusion.PatientID) ? "" : conclusion.PatientID);
                ReplaceTextInRange(range, "{PatientGender}", String.IsNullOrEmpty(conclusion.PatientGender) ? "" : conclusion.PatientGender);
                ReplaceTextInRange(range, "{PatientDate}", CaculatorDate(conclusion.PatientDoB).ToString());
                ReplaceTextInRange(range, "{StudyInstanceUID}", String.IsNullOrEmpty(conclusion.StudyInstanceUID) ? "" : conclusion.StudyInstanceUID);
                ReplaceTextInRange(range, "{HealthIdentificationCode}", String.IsNullOrEmpty(conclusion.HealthIdentificationCode) ? "" : conclusion.HealthIdentificationCode);
                ReplaceTextInRange(range, "{MedicalImagingCreateAt}", String.IsNullOrEmpty(conclusion.MedicalImagingCreateAt.ToString()) ? "" : conclusion.MedicalImagingCreateAt.ToString());
                ReplaceTextInRange(range, "{MedicalImagingReportedAt}", String.IsNullOrEmpty(conclusion.MedicalImagingReportedAt.ToString()) ? "" : conclusion.MedicalImagingReportedAt.ToString());
                ReplaceTextInRange(range, "{MedicalImagingCode}", String.IsNullOrEmpty(conclusion.MedicalImagingCode) ? "" : conclusion.MedicalImagingCode);
                ReplaceTextInRange(range, "{OrderingPhysician}", String.IsNullOrEmpty(conclusion.OrderingPhysician) ? "" : conclusion.OrderingPhysician);
                ReplaceTextInRange(range, "{Radiologist}", String.IsNullOrEmpty(conclusion.Radiologist) ? "" : conclusion.Radiologist);
                ReplaceTextInRange(range, "{Technicians}", String.IsNullOrEmpty(conclusion.Technicians) ? "" : conclusion.Technicians);
                ReplaceTextInRange(range, "{DeviveName}", String.IsNullOrEmpty(conclusion.DeviveName) ? "" : conclusion.DeviveName);
                ReplaceTextInRange(range, "{ImagingServiceCode}", String.IsNullOrEmpty(imageService.HospitalServiceName) ? "" : imageService.HospitalServiceName);
                ReplaceTextInRange(range, "{DiagnoseInfo}", String.IsNullOrEmpty(conclusion.DiagnoseInfo) ? "" : conclusion.DiagnoseInfo);
                ReplaceTextInRange(range, "{DiagnoseResult}", String.IsNullOrEmpty(conclusion.DiagnoseResult) ? "" : conclusion.DiagnoseResult);
                ReplaceTextInRange(range, "{DiagnoseNote}", String.IsNullOrEmpty(conclusion.DiagnoseNote) ? "" : conclusion.DiagnoseNote);

                range.Collapse(Microsoft.Office.Interop.Word.WdCollapseDirection.wdCollapseEnd);


                int totalPageCount = doc.ComputeStatistics(Microsoft.Office.Interop.Word.WdStatistic.wdStatisticPages);
                pageNumber = totalPageCount - 1;
                Microsoft.Office.Interop.Word.Range pageRange = doc.GoTo(
                    What: Microsoft.Office.Interop.Word.WdGoToItem.wdGoToPage,
                    Which: Microsoft.Office.Interop.Word.WdGoToDirection.wdGoToAbsolute,
                    Count: pageNumber
                );
                pageRange.Collapse(Microsoft.Office.Interop.Word.WdCollapseDirection.wdCollapseStart);

                // Chèn hình ảnh vào trang trước trang cuối
                string imagePath = InitQRCode();
                Microsoft.Office.Interop.Word.Shape picture = doc.Shapes.AddPicture(imagePath, LinkToFile: false, SaveWithDocument: true, Anchor: pageRange);
                picture.Width = 45;
                picture.Height = 45;
                picture.Left = 5;
                if (pageNumber == 1)
                {
                    picture.Top = 665;
                }
                else
                {
                    picture.Top = 750;
                }



                // Thêm hình ảnh vào trang cuối
                Microsoft.Office.Interop.Word.PageSetup pageSetup = doc.PageSetup;
                Microsoft.Office.Interop.Word.Range lastPageRange = range.GoTo(
                    What: Microsoft.Office.Interop.Word.WdGoToItem.wdGoToPage,
                    Which: Microsoft.Office.Interop.Word.WdGoToDirection.wdGoToLast
                );

                float width;
                if (conclusionImages.Count < 3)
                {
                    width = pageSetup.PageHeight / 2 - 50;
                }
                else
                {
                    width = pageSetup.PageWidth / 2 - 30;
                }

                if (conclusionImages != null && conclusionImages.Count > 0)
                {
                    foreach (string conclusionImage in conclusionImages)
                    {
                        Microsoft.Office.Interop.Word.InlineShape inlineShape = lastPageRange.InlineShapes.AddPicture(conclusionImage, LinkToFile: false, SaveWithDocument: true);
                        inlineShape.LockAspectRatio = Microsoft.Office.Core.MsoTriState.msoTrue;
                        inlineShape.Width = width;
                    }
                }

                // Lưu tài liệu mới
                if (checkFileDoc)
                {
                    doc.SaveAs2(savePath);
                }
                else
                {
                    doc.SaveAs2(savePath, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF);
                }

            }
            catch (Exception ex)
            {
                Messager.ShowError((IWin32Window)this, ex);
            }
            finally
            {
                // Đóng tài liệu và ứng dụng
                doc?.Close(false);
                wordApp.Quit();
                if (File.Exists(savePathDoc))
                {
                    File.Delete(savePathDoc);
                }
            }
        }



        private void CreateXmlDocument(string savePath)
        {
            using (XmlWriter writer = XmlWriter.Create(savePath, new XmlWriterSettings
            {
                Indent = true,
                Encoding = System.Text.Encoding.UTF8,
                OmitXmlDeclaration = false,
                ConformanceLevel = ConformanceLevel.Document
            }))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Conclusion");

                var properties = conclusion.GetType().GetProperties();
                foreach (PropertyInfo propertyInfo in properties)
                {
                    object value = propertyInfo.GetValue(conclusion);
                    writer.WriteElementString(propertyInfo.Name, value?.ToString() ?? string.Empty);
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word Documents|*.docx|XML Files|*.xml|PDF Files|*.pdf";
            saveFileDialog.Title = "Save as Word Document or XML File";
            saveFileDialog.FileName = $"Phiếu kết luận bệnh nhân {conclusion.PatientName}";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = saveFileDialog.FileName;
                string fileExtension = Path.GetExtension(selectedFileName).ToLower();

                if (fileExtension == ".docx")
                {
                    CreateWordDocument(templateDocxPath, selectedFileName, true);
                    MessageBox.Show("Word document saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (fileExtension == ".pdf")
                {
                    CreateWordDocument(templateDocxPath, selectedFileName, false);
                    MessageBox.Show("Word document saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (fileExtension == ".xml")
                {
                    CreateXmlDocument(selectedFileName);
                    MessageBox.Show("XML file saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Unsupported file format selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public static string ConvertFileToBase64(string filePath)
        {
            try
            {
                byte[] fileBytes = File.ReadAllBytes(filePath);
                string base64String = Convert.ToBase64String(fileBytes);
                return base64String;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        private string ShareLinksID;
        private async Task<bool> UpdateShareLinkSupabaseAsync()
        {
            try
            {
                ShareLinks shareLinks = new ShareLinks
                {
                    StudyUid = conclusion.StudyInstanceUID.Trim(),
                    Role = "patient",
                    Expiration = DateTime.Now,
                    PatientID = conclusion.PatientID.Trim(),
                    PatientName = conclusion.PatientName.Trim(),
                    PatientGender = conclusion.PatientGender.Trim(),
                    PatientDoB = conclusion.PatientDoB,
                    HealthIdentificationCode = conclusion.HealthIdentificationCode.Trim(),
                    MedicalImagingCreateAt = conclusion.MedicalImagingCreateAt,
                    MedicalImagingReportedAt = conclusion.MedicalImagingReportedAt,
                    MedicalImagingCode = conclusion.MedicalImagingCode.Trim(),
                    OrderingPhysician = conclusion.OrderingPhysician.Trim(),
                    Radiologist = conclusion.Radiologist.Trim(),
                    Technicians = conclusion.Technicians.Trim(),
                    DeviveName = conclusion.DeviveName.Trim(),
                    DiagnoseInfo = conclusion.DiagnoseInfo.Trim(),
                    DiagnoseResult = conclusion.DiagnoseResult.Trim(),
                    DiagnoseNote = conclusion.DiagnoseNote.Trim(),
                    ImagingServiceName = labelImagingService.Text.ToString().Trim(),
                    CreateAt = (DateTime)conclusion.CreateAt,
                    Images = new List<string> { labelImagingService.Text.ToString() }
                };

                var x = await SupabaseConnection.Client.From<ShareLinks>().Insert(shareLinks);

                JArray jsonArray = JArray.Parse(x.Content);
                JObject jsonObject = (JObject)jsonArray[0];
                ShareLinksID = jsonObject["id"].ToString();

                foreach (string item in conclusionImages)
                {
                    string path = $"{conclusion.StudyInstanceUID.Trim()}/{Guid.NewGuid().ToString()}.jpg";
                    await SupabaseConnection.Client.Storage
                        .From("conclusion")
                        .Upload(item, path);
                }
                return true;
            }
            catch (Exception ex)
            {                            
                Messager.ShowError((IWin32Window)this, ex);
                return false;
            }
        }

        //private PdfSignature BuildPDFSignatureObject(string inputFile, X509Certificate2 certificate)
        //{
        //    try
        //    {
        //        PdfSignature pdfObj = new PdfSignature("86f3bab70e49b07d4d02");

        //        pdfObj.LoadPdfDocument(inputFile);

        //        pdfObj.DigitalSignatureCertificate = certificate;

        //        if (pdfObj.DigitalSignatureCertificate == null)
        //            throw new Exception("Digital signature certificate is not valid.");

        //        //Signature Reason/Location
        //        pdfObj.SigningReason = "Khoa Chẩn đoán hình ảnh";
        //        pdfObj.SigningLocation = "";
        //        pdfObj.SignaturePage = pageNumber;
        //        pdfObj.SignatureAdvancedPosition = new System.Drawing.Rectangle(280, 20, 300, 30);

        //        pdfObj.FontFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Arial.ttf");

        //        //pdfObj.SignatureToAllPages = true;
        //        pdfObj.SignatureImage = Certificate.ImageSigner;
        //        pdfObj.SignatureImageType = SignLib.Pdf.SignatureImageType.ImageAndText;

        //        //Certify Document
        //        //pdfObj.CertifySignature = CertifyMethod.NoChangesAllowed;

        //        //Signature Hash Algorithm
        //        pdfObj.HashAlgorithm = SignLib.HashAlgorithm.SHA1;

        //        //pdfObj.TimeStamping.ServerUrl = new Uri("http://192.168.1.122/TimeStampServer/get.aspx");
        //        //pdfObj.TimeStamping.PolicyOid = new System.Security.Cryptography.Oid("1.3.6.1.4.1.13762.3");
        //        //pdfObj.TimeStamping.UserName = "linh";
        //        //pdfObj.TimeStamping.Password = "123456";

        //        pdfObj.SignatureText = "Ký bởi: " + pdfObj.DigitalSignatureCertificate.GetNameInfo(X509NameType.SimpleName, false)
        //            + "\nNgày ký: " + DateTime.Now.ToString("yyyy.MM.dd HH:mm")
        //            + "\nTổ chức: Bệnh viện quân y 120 Tiền Giang - Khoa chẩn đoán hình ảnh";

        //        return pdfObj;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //static void DigitallySignXMLDocument(string unsignedDocument, string signedDocument, X509Certificate2 card)
        //{
        //    XmlSignature cs = new XmlSignature("86f3bab70e49b07d4d02");

        //    //Digital signature certificate can be loaded from various sources

        //    //Load the signature certificate from a PFX or P12 file
        //    cs.DigitalSignatureCertificate = card;

        //    //Create a digital signature certificate on the fly (X509Certificate2 certificate)
        //    //cs.DigitalSignatureCertificate = CreateDigitalCertificate();

        //    //Load the certificate from Microsoft Store. 
        //    //The smart card or USB token certificates are usually available on Microsoft Certificate Store (start - run - certmgr.msc).
        //    //If the smart card certificate not appears on Microsoft Certificate Store it cannot be used by the library
        //    //cs.DigitalSignatureCertificate = DigitalCertificate.LoadCertificate(false, string.Empty, "Select Certificate", "Select the certificate for digital signature");

        //    //The smart card PIN dialog can be bypassed for some smart cards/USB Tokens. 
        //    //ATTENTION: This feature will NOT work for all available smart card/USB Tokens becauase of the drivers or other security measures.
        //    //Use this property carefully.
        //    //DigitalCertificate.SmartCardPin = "123456";

        //    cs.IncludeKeyInfo = true;
        //    cs.IncludeSignatureCertificate = true;

        //    //apply the digital signature
        //    cs.ApplyDigitalSignature(unsignedDocument, signedDocument);

        //    Console.WriteLine("XML signature was created." + Environment.NewLine);

        //}

        //static void VerifyXMLSignature(string signedDocument)
        //{
        //    XmlSignature cv = new XmlSignature("86f3bab70e49b07d4d02");

        //    Console.WriteLine("Number of signatures: " + cv.GetNumberOfSignatures(signedDocument));

        //    ///verify the first signature
        //    Console.WriteLine("Signature validity status: " + cv.VerifyDigitalSignature(signedDocument));

        //    Console.WriteLine("Done XML signature verification." + Environment.NewLine + Environment.NewLine);
        //}

        //private void SignPdf(string inputFile, string outputFile, X509Certificate2 card, bool  isCheckXML)
        //{
        //    try
        //    {
        //        if(isCheckXML)
        //        {
        //            DigitallySignXMLDocument(inputFile, outputFile, card);
        //            VerifyXMLSignature(outputFile);
        //        }
        //        else
        //        {
        //            PdfSignature pdfSign = BuildPDFSignatureObject(inputFile, card);
        //            File.WriteAllBytes(outputFile, pdfSign.ApplyDigitalSignature());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("An error has occurred: " + ex.Message);
        //    }
        //}

        private FrmOperation _frmOperation;
        public void EnableItems(bool enable, string strCaption, string strBtnCaption)
        {
            if (this.InvokeRequired)
            {
                Invoke(new EnableMenu(EnableItems), new object[] { enable, strCaption, strBtnCaption });
            }
            else
            {
                if (enable)
                    Cursor.Current = Cursors.Arrow;
                else
                    Cursor.Current = Cursors.WaitCursor;

                if (enable)
                {
                    if (_frmOperation != null)
                        _frmOperation.Close();
                }
                else
                {
                    if (!(strCaption == "" && strBtnCaption == ""))
                        if (_frmOperation == null || !_frmOperation.Visible)
                        {
                            _frmOperation = new FrmOperation(strCaption, strBtnCaption);
                            _frmOperation.Show(this);
                        }
                }
            }
        }

        private void CreateXMLDocument(string pathCreateFileXML)
        {
            foreach(string image in conclusionImages)
            {
                try
                {
                    byte[] bytes = File.ReadAllBytes(image);
                    string file = Convert.ToBase64String(bytes);
                    conclusion.Images.Add(file);
                }catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            XmlSerializer serializer = new XmlSerializer(typeof(PrintToPACSDemo.AnPhatData.Conclusion));

            using (FileStream fs = new FileStream(pathCreateFileXML, FileMode.Create))
            {
                serializer.Serialize(fs, conclusion);
            }
        }

        private async void SaveConclusionAsync()
        {
            try
            {
                if (Program.IsAuthencation)
                {
                    EnableItems(false, "Đang tạo kết luận \nVui lòng đợi...", "Cancel");
                    //Gán giá trị vào conclusion
                    string pathCreateFilePdf = Path.Combine(saveStorePath, $"{conclusion.StudyInstanceUID.Trim()}.pdf");
                    string pathCreateFileXML = Path.Combine(saveStorePath, $"{conclusion.StudyInstanceUID.Trim()}.xml");
                    conclusion.DiagnoseInfo = richTextBoxDiagnoseInfo.Text;
                    conclusion.DiagnoseResult = richTextBoxDiagnoseResult.Text;
                    conclusion.DiagnoseNote = richTextBoxDiagnoseNote.Text;
                    conclusion.CreateAt = DateTime.Now;

                    if (!await UpdateShareLinkSupabaseAsync())
                    {
                        return;
                    }
                    conclusion.Id = ShareLinksID;

                    CreateWordDocument(templateDocxPath, pathCreateFilePdf, false);
                    CreateXMLDocument(pathCreateFileXML);

                    try
                    {
                        dynamic data = new ExpandoObject();
                        data.MIMEType = "application/pdf";
                        data.Username = PacsSettings.Staff.Username.Trim();
                        data.IsCheckSetting = false;
                        data.DepartmentCode = PacsSettings.Staff.StaffDepartment.Trim();
                        await ClientAPI.DigitalSigninFile(pathCreateFilePdf, data, saveStorePath);
                    }
                    catch (Exception ex)
                    {
                        Messager.ShowError((IWin32Window)this, ex);
                    }

                    try
                    {
                        dynamic data = new ExpandoObject();
                        data.MIMEType = "application/xml";
                        data.Username = PacsSettings.Staff.Username;
                        ClientAPI.DigitalSigninFile(pathCreateFileXML, data, saveStorePath);
                    }
                    catch (Exception ex)
                    {
                        Messager.ShowError((IWin32Window)this, ex);
                    }

                    //DialogResult result = MessageBox.Show("Tạo kết luận thành công !", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //if (result == DialogResult.OK)
                    //{
                    //    this.Close();
                    //}
                }
                else
                {
                    EnableItems(true, "", "");
                    QRCodeAuthentication qRCodeAuthentication = new QRCodeAuthentication(PacsSettings.Staff);
                    qRCodeAuthentication.Show(this);
                }
            }
            finally
            {
                EnableItems(true, "", "");
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveConclusionAsync();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ConclusionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            tablePanelImageConclusion.Controls.Clear();
            foreach (string conclusionImage in conclusionImages)
            {
                if (File.Exists(conclusionImage))
                {
                    File.Delete(conclusionImage);
                }
            }
        }

        private void ConclusionForm_Load(object sender, EventArgs e)
        {
            saveStorePath = $"D:\\DICOM Store\\L23_WS_SERVER64\\Images\\{conclusion.PatientID.Trim()}\\Image Conclusion";
            LoadConclusionLocal();
        } 
    }
}
