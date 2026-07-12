using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;
using Leadtools;
using Leadtools.Demos;
using Leadtools.Forms.DocumentWriters;
using Leadtools.Codecs;
using Leadtools.Dicom;
using System.Net;
using System.Threading;
using Leadtools.Dicom.Common.Extensions;
using Leadtools.Dicom.Common.Editing;
using Leadtools.Dicom.Scu.Common;
using Leadtools.Dicom.Scu;
using System.Diagnostics;
using Leadtools.Dicom.Common.DataTypes.Modality;
using STM.MediaToPACS.Main.UI;
using Leadtools.DicomDemos;
using System.Collections.Generic;
using System.Collections;
using System.Management;
using Leadtools.WinForms.CommonDialogs.File;
using System.Reflection;
using Leadtools.Dicom.Common.Editing.Converters;
using Leadtools.ImageProcessing;
using Leadtools.Drawing;
using Leadtools.ImageProcessing.Effects;
using STM.MediaToPACS.Main.UI.CameraUI;
using Leadtools.Medical.Worklist.DataAccessLayer;
using Leadtools.Medical.DataAccessLayer;
using Leadtools.Medical.DataAccessLayer.Configuration;
using Leadtools.Medical.Worklist.DataAccessLayer.Configuration;
using Leadtools.Medical.Winforms;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
//using VisioForge.Core.VideoEdit; // VisioForge đã gỡ (thay bằng FlashCap)
using MediaToPacs.Core.Models;
using STM.MediaToPACS.Main.Utilities;
using DevExpress.XtraPdfViewer;
using System.Drawing.Printing;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraEditors;
using DevExpress.Pdf;
using MediaToPacs.Core.Models.Ketluan;
using DevExpress.XtraReports.UI;
using System.Text;
using MediaToPacs.Core.Enums;
using System.Xml.Serialization;
using Serilog;
using System.Configuration;
using System.Runtime.InteropServices;
using STM.MediaToPACS.Main.UI.Configurations;

namespace STM.MediaToPACS.Main
{
    public partial class FrmMain
    {
        #region Dicom Methods

        private void SetElements(DicomDataSet dicomDestination, DicomElement[] elements, DicomDataSet dicomSource)
        {
            foreach (DicomElement item in elements)
            {
                if (item.Length == 0)
                    continue;

                DicomElement element;
                element = dicomDestination.FindFirstElement(null, item.Tag, true);
                if (element == null)
                    element = dicomDestination.InsertElement(null, false, item.Tag, item.VR, false, 0);
                switch (item.VR)
                {
                    case DicomVRType.DA:
                        dicomDestination.SetDateValue(element, dicomSource.GetDateValue(item, 0, 1));
                        break;
                    case DicomVRType.TM:
                        dicomDestination.SetTimeValue(element, dicomSource.GetTimeValue(item, 0, 1));
                        break;
                    default:
                        {
                            byte[] ba = dicomSource.GetBinaryValue(item, (int)item.Length);
                            dicomDestination.FreeElementValue(element);
                            bool ret = dicomDestination.SetBinaryValue(element, ba, (int)ba.Length);
                        }
                        break;
                }
            }
            _pgDicomInfo.DataSet = dicomDestination;
        }

        private List<string> SaveDicom(DicomDataSet dicom, string strSaveFile)
        {
            try
            {
                byte[] value = new byte[] { 0x00, 0x01 };
                dicom.InsertElementAndSetValue(DicomTag.FileMetaInformationVersion, value);
                dicom.InsertElementAndSetValue(DicomTag.MediaStorageSOPClassUID, dicom.GetValue<string>(DicomTag.SOPClassUID, string.Empty));
                dicom.InsertElementAndSetValue(DicomTag.MediaStorageSOPInstanceUID, dicom.GetValue<string>(DicomTag.SOPInstanceUID, string.Empty));
                dicom.InsertElementAndSetValue(DicomTag.ImplementationClassUID, "1.2.840.114257.1123456");
                dicom.InsertElementAndSetValue(DicomTag.ImplementationVersionName, "STM.MEDIA");

                List<string> saved = new List<string>();

                int bit = 0;
                if (ClassTypes[_cmbSopClasses.SelectedIndex] == DicomClassType.EncapsulatedPdfStorage)
                {
                    DocumentFormat documentFormat = DocumentFormat.User;
                    DocumentOptions documentOptions = null;
                    PdfDocumentOptions PdfdocumentOptions = new PdfDocumentOptions();
                    string fileName;
                    fileName = Path.GetTempFileName();
                    documentFormat = DocumentFormat.Pdf;
                    documentOptions = new PdfDocumentOptions();
                    (documentOptions as PdfDocumentOptions).DocumentType = PdfDocumentType.Pdf;
                    (documentOptions as PdfDocumentOptions).FontEmbedMode = DocumentFontEmbedMode.Auto;
                    documentOptions.PageRestriction = DocumentPageRestriction.Relaxed;
                    DocumentWriter documentWriter = new DocumentWriter();
                    documentWriter.SetOptions(documentFormat, documentOptions);
                    documentWriter.BeginDocument(fileName, documentFormat);

                    foreach (ListImageBox.ListItem item in _lstBoxPages.CheckedItems)
                    {
#if LEADTOOLS_V19_OR_LATER
                        DocumentEmfPage documentPage = new DocumentEmfPage();
#else
                                      DocumentPage documentPage = DocumentPage.Empty;
#endif // #if LEADTOOLS_V19_OR_LATER
                        if (item.ImageItem.Tag.GetType() == typeof(PrintPage))
                            documentPage.EmfHandle = (item.ImageItem.Tag as PrintPage).MetaFile;
                        else
                        {
                            RasterImage rI = _codec.Load((item.ImageItem.Tag as IPrintToPACSFile).FileLocation());
                            documentPage.EmfHandle = Leadtools.Drawing.RasterImageConverter.ChangeToEmf(rI);
                            rI.Dispose();
                        }

                        documentWriter.AddPage(documentPage);
                        Application.DoEvents();
                        if (bCancelOperation)
                            break;
                    }

                    documentWriter.EndDocument();
                    SetIncapsualtedDoc(dicom, fileName);
                    File.Delete(fileName);
                    saved.Add(strSaveFile);
                    dicom.Save(strSaveFile, DicomDataSetSaveFlags.ExplicitVR | DicomDataSetSaveFlags.MetaHeaderPresent);

                    //Delete Element
                    ClearTag(dicom, DicomTag.EncapsulatedDocument);
                    ClearTag(dicom, DicomTag.HL7InstanceIdentifier);
                    ClearTag(dicom, DicomTag.ListOfMIMETypes);
                    ClearTag(dicom, DicomTag.VerificationFlag);

                    DicomElement dElement = _pgDicomInfo.DataSet.FindFirstElement(null, DicomTag.MIMETypeOfEncapsulatedDocument, false);
                    if (dElement != null)
                        _pgDicomInfo.DataSet.SetValue(dElement, "PDF");
                }

                if (ClassTypes[_cmbSopClasses.SelectedIndex] == DicomClassType.SCImageStorage)
                {
                    //Pixel Data
                    int i = 0;
                    foreach (ListImageBox.ListItem item in _lstBoxPages.CheckedItems)
                    {
                        i++;

                        DicomElement dInstance = dicom.FindFirstElement(null, DicomTag.InstanceNumber, true);
                        if (dInstance == null)
                            dInstance = dicom.InsertElement(null, false, DicomTag.InstanceNumber, DicomVRType.OW, false, 0);
                        dicom.SetValue(dInstance, i);

                        DicomElement dPixel = dicom.FindFirstElement(null, DicomTag.PixelData, true);
                        if (dPixel == null)
                        {
                            dPixel = dicom.InsertElement(null, false, DicomTag.PixelData, DicomVRType.OW, false, 0);
                        }
                        else
                        {
                            dicom.DeleteElement(dPixel);
                            dPixel = dicom.InsertElement(null, false, DicomTag.PixelData, DicomVRType.OW, false, 0);
                        }

                        RasterImage rI = null;
                        if (rI == null)
                            rI = _codec.Load((item.ImageItem.Tag as IPrintToPACSFile).FileLocation());

                        DicomImagePhotometricInterpretationType imagePhotoMetric = DicomImagePhotometricInterpretationType.Rgb;
                        if (rI.IsGray)
                        {
                            bit = 8;
                            imagePhotoMetric = DicomImagePhotometricInterpretationType.Monochrome2;
                            if (rI.BitsPerPixel == 12 || rI.BitsPerPixel == 16)
                            {
                                GrayscaleCommand grayCommand = new GrayscaleCommand(bit);
                                grayCommand.Run(rI);
                            }
                        }
                        else
                        {
                            bit = 24;
                            ColorResolutionCommand colorRes = new ColorResolutionCommand();
                            colorRes.BitsPerPixel = bit;
                            colorRes.Order = RasterByteOrder.Rgb;
                            colorRes.Mode = ColorResolutionCommandMode.InPlace;
                            colorRes.Run(rI);
                        }

                        dicom.SetImage(dPixel,
                                          rI,
                                          _mySettings._settings.secondaryCaptureCompression,
                                          imagePhotoMetric,
                                          bit,
                                          2,
                                          DicomSetImageFlags.AutoSetVoiLut);
                        rI.Dispose();

                        GenerateUidTag(dicom, DicomTag.SOPInstanceUID);
                        // Đồng bộ meta header với SOP Instance UID vừa sinh cho từng ảnh,
                        // nếu không (0002,0003) sẽ giữ giá trị cũ/rỗng lệch với (0008,0018)
                        dicom.InsertElementAndSetValue(DicomTag.MediaStorageSOPInstanceUID, dicom.GetValue<string>(DicomTag.SOPInstanceUID, string.Empty));

                        string strFile = Path.GetDirectoryName(strSaveFile) + "\\" + Path.GetFileNameWithoutExtension(strSaveFile) + "_" + i + Path.GetExtension(strSaveFile);
                        saved.Add(strFile);
                        dicom.Save(strFile, DicomDataSetSaveFlags.ExplicitVR | DicomDataSetSaveFlags.MetaHeaderPresent);
                        Application.DoEvents();
                        if (bCancelOperation)
                            break;
                    }
                    ClearTag(dicom, DicomTag.PixelData);
                    ClearTag(dicom, DicomTag.WindowCenter);
                    ClearTag(dicom, DicomTag.WindowWidth);
                    DicomElement dInstElement = dicom.FindFirstElement(null, DicomTag.InstanceNumber, true);
                    if (dInstElement == null)
                        dInstElement = dicom.InsertElement(null, false, DicomTag.InstanceNumber, DicomVRType.OW, false, 0);
                    dicom.SetValue(dInstElement, "1");

                }

                if (ClassTypes[_cmbSopClasses.SelectedIndex] == DicomClassType.SCMultiFrameTrueColorImageStorage ||
                   ClassTypes[_cmbSopClasses.SelectedIndex] == DicomClassType.SCMultiFrameGrayscaleByteImageStorage)
                {

                    //Pixel Data
                    DicomElement dPixel = dicom.FindFirstElement(null, DicomTag.PixelData, true);
                    if (dPixel == null)
                        dPixel = dicom.InsertElement(null, false, DicomTag.PixelData, DicomVRType.OW, false, 0);

                    DicomElement dPageVector = dicom.FindFirstElement(null, DicomTag.PageNumberVector, true);

                    RasterImage rI = null;

                    int i = 1;
                    List<int> intArray = new List<int>();

                    DicomImageCompressionType compression = DicomImageCompressionType.None;
                    DicomImagePhotometricInterpretationType imagephotemetric = DicomImagePhotometricInterpretationType.Rgb;
                    ColorResolutionCommand colorRes = new ColorResolutionCommand();
                    if (ClassTypes[_cmbSopClasses.SelectedIndex] == DicomClassType.SCMultiFrameTrueColorImageStorage)
                    {
                        compression = _mySettings._settings.secondaryCaptureColorCompression;
                        imagephotemetric = DicomImagePhotometricInterpretationType.Rgb;
                        bit = 24;
                        colorRes.BitsPerPixel = bit;
                        colorRes.Order = RasterByteOrder.Bgr;
                        colorRes.Mode = ColorResolutionCommandMode.InPlace;
                    }
                    else
                    {
                        compression = _mySettings._settings.secondaryCaptureGrayCompression;
                        imagephotemetric = DicomImagePhotometricInterpretationType.Monochrome2;
                        bit = 8;
                        colorRes.BitsPerPixel = bit;
                        colorRes.Order = RasterByteOrder.Gray;
                        colorRes.Mode = ColorResolutionCommandMode.InPlace;
                    }
                    foreach (ListImageBox.ListItem item in _lstBoxPages.CheckedItems)
                    {
                        intArray.Add(i);
                        i++;
                        if (rI == null)
                        {
                            rI = _codec.Load((item.ImageItem.Tag as IPrintToPACSFile).FileLocation());
                            colorRes.Run(rI);
                            continue;
                        }
                        RasterImage rasterimage = _codec.Load((item.ImageItem.Tag as IPrintToPACSFile).FileLocation());
                        colorRes.Run(rasterimage);
                        rI.AddPage(rasterimage);
                        Application.DoEvents();
                        if (bCancelOperation)
                            break;
                    }

                    RasterImage rImg = null;
                    rI.Page = 1;
                    int iMaxWidth = rI.Width, iMaxHeight = rI.Height;
                    int iPage;
                    for (iPage = 1; iPage <= rI.PageCount; iPage++)
                    {
                        rI.Page = iPage;
                        rImg = rI;
                        if (rImg.Width > iMaxWidth)
                        {
                            iMaxWidth = rImg.Width;
                        }

                        if (rImg.Height > iMaxHeight)
                        {
                            iMaxHeight = rImg.Height;
                        }
                    }

                    RasterImage rImgNew = null;
                    List<RasterImage> lstRaster = new List<RasterImage>();
                    for (iPage = 1; iPage <= rI.PageCount; iPage++)
                    {
                        rI.Page = iPage;
                        rImg = rI;
                        if (rImg.ImageSize.Width < iMaxWidth || rImg.ImageSize.Height < iMaxHeight)
                        {
                            rImgNew = new RasterImage(RasterMemoryFlags.Conventional, iMaxWidth, iMaxHeight, bit, RasterByteOrder.Bgr, rImg.ViewPerspective, rImg.GetPalette(), IntPtr.Zero, 0);
                            FillCommand fillCommand = new FillCommand();
                            fillCommand.Color = RasterColorConverter.FromColor(Color.White);
                            fillCommand.Run(rImgNew);
                            CombineCommand combine = new CombineCommand();
                            int xStart, yStart;
                            xStart = Math.Abs(rImgNew.Width - rImg.Width) / 2;
                            yStart = Math.Abs(rImgNew.Height - rImg.Height) / 2;
                            combine.DestinationRectangle = new LeadRect(xStart, yStart, rImg.Width, rImg.Height);
                            combine.SourcePoint = new LeadPoint(0, 0);
                            combine.SourceImage = rImg;
                            combine.Flags = CombineCommandFlags.OperationAdd | CombineCommandFlags.Destination0;
                            combine.Run(rImgNew);
                            lstRaster.Add(rImgNew.Clone());
                        }
                        else
                        {
                            lstRaster.Add(rImg.Clone());
                        }
                    }
                    rI.Dispose();
                    rI = null;
                    foreach (RasterImage rasterimage in lstRaster)
                    {
                        if (rI == null)
                            rI = rasterimage;
                        else
                            rI.InsertPage(rI.PageCount + 1, rasterimage);
                    }

                    saved.Add(strSaveFile);
                    dicom.SetIntValue(dPageVector, intArray.ToArray(), intArray.Count);
                    dicom.SetImages(dPixel,
                          rI,
                          compression,
                          imagephotemetric,
                          bit,
                          2,
                          DicomSetImageFlags.AutoSetVoiLut);
                    dicom.Save(strSaveFile, DicomDataSetSaveFlags.ExplicitVR | DicomDataSetSaveFlags.MetaHeaderPresent);
                    rI.Dispose();
                    //Delete Element
                    ClearTag(dicom, DicomTag.PixelData);
                    ClearTag(dicom, DicomTag.WindowCenter);
                    ClearTag(dicom, DicomTag.WindowWidth);
                    ClearTag(dicom, DicomTag.RescaleIntercept);
                    ClearTag(dicom, DicomTag.RescaleSlope);
                    ClearTag(dicom, DicomTag.RescaleType);
                    ClearTag(dicom, DicomTag.PageNumberVector);
                }
                GenerateUidTag(dicom, DicomTag.SeriesInstanceUID);
                GenerateUidTag(dicom, DicomTag.SOPInstanceUID);
                _pgDicomInfo.DataSet = dicom;
                return saved;
            }
            finally
            {
                ClearTag(dicom, DicomTag.FileMetaInformationVersion);
                ClearTag(dicom, DicomTag.MediaStorageSOPClassUID);
                ClearTag(dicom, DicomTag.MediaStorageSOPInstanceUID);
                ClearTag(dicom, DicomTag.ImplementationClassUID);
                ClearTag(dicom, DicomTag.ImplementationVersionName);
            }
        }

        private void ClearTag(DicomDataSet dicom, long tag)
        {
            DicomElement dElement = dicom.FindFirstElement(null, tag, true);
            if (dElement != null)
                dicom.DeleteElement(dElement);
        }

        void SetIncapsualtedDoc(DicomDataSet ds, string sFileDocumentIn)
        {
            DicomElement dElement;
            string strDocumentTitle = "", strBurnedInAnnotation = "", strVerificationFlag = "", strInstanceNumber = "",
                   strCodeSchemeDesignator = "", strCodeValue = "", strCodeMeaning = "";
            DicomTimeValue contentTime = new DicomTimeValue();
            DicomDateValue contentDate = new DicomDateValue();
            DicomDateTimeValue acquistationTime = new DicomDateTimeValue();

            dElement = ds.FindFirstElement(null, DicomTag.InstanceNumber, true);
            if (dElement != null && dElement.Length != 0)
                strInstanceNumber = ds.GetValue<string>(dElement, "");

            dElement = ds.FindFirstElement(null, DicomTag.AcquisitionDateTime, true);
            if (dElement != null && dElement.Length != 0)
                acquistationTime = ds.GetDateTimeValue(dElement, 0, 1)[0];

            dElement = ds.FindFirstElement(null, DicomTag.DocumentTitle, true);
            if (dElement != null && dElement.Length != 0)
                strDocumentTitle = ds.GetStringValue(dElement, 0);

            dElement = ds.FindFirstElement(null, DicomTag.ContentTime, true);
            if (dElement != null && dElement.Length != 0)
                contentTime = ds.GetTimeValue(dElement, 0, 1)[0];

            dElement = ds.FindFirstElement(null, DicomTag.ContentDate, true);
            if (dElement != null && dElement.Length != 0)
                contentDate = ds.GetDateValue(dElement, 0, 1)[0];

            dElement = ds.FindFirstElement(null, DicomTag.BurnedInAnnotation, true);
            if (dElement != null && dElement.Length != 0)
                strBurnedInAnnotation = ds.GetStringValue(dElement, 0);

            dElement = ds.FindFirstElement(null, DicomTag.VerificationFlag, true);
            if (dElement != null && dElement.Length != 0)
                strVerificationFlag = ds.GetStringValue(dElement, 0);

            DicomElement dElementCNS = ds.FindFirstElement(null, DicomTag.ConceptNameCodeSequence, true);
            if (dElementCNS != null && dElementCNS.Length != 0)
                strCodeMeaning = ds.GetStringValue(dElement, 0);

            dElement = ds.FindFirstElement(dElementCNS, DicomTag.CodeMeaning, false);
            if (dElement != null && dElement.Length != 0)
                strCodeMeaning = ds.GetStringValue(dElement, 0);

            dElement = ds.FindFirstElement(dElementCNS, DicomTag.CodeValue, false);
            if (dElement != null && dElement.Length != 0)
                strCodeValue = ds.GetValue<string>(dElement, "");

            dElement = ds.FindFirstElement(dElementCNS, DicomTag.CodingSchemeDesignator, false);
            if (dElement != null && dElement.Length != 0)
                strCodeSchemeDesignator = ds.GetStringValue(dElement, 0);

            dElement = ds.FindFirstElement(null, DicomTag.EncapsulatedDocument, true);
            if (dElement == null)
                dElement = ds.InsertElement(null, false, DicomTag.EncapsulatedDocument, DicomVRType.UN, false, 0);

            bool child = false;
            DicomEncapsulatedDocument encapsulatedDocument = new DicomEncapsulatedDocument();
            encapsulatedDocument.Type = DicomEncapsulatedDocumentType.Pdf;
            encapsulatedDocument.InstanceNumber = int.Parse(strInstanceNumber);
            encapsulatedDocument.ContentDate = contentDate;

            encapsulatedDocument.ContentTime = contentTime;

            encapsulatedDocument.AcquisitionDateTime = acquistationTime;

            encapsulatedDocument.BurnedInAnnotation = strBurnedInAnnotation;
            encapsulatedDocument.DocumentTitle = strDocumentTitle;
            encapsulatedDocument.VerificationFlag = strVerificationFlag;
            encapsulatedDocument.HL7InstanceIdentifier = string.Empty;


            string[] sListOfMimeTypes = new string[] { "image/jpeg", "application/pdf" };
            encapsulatedDocument.SetListOfMimeTypes(sListOfMimeTypes);

            DicomCodeSequenceItem conceptNameCodeSequence = new DicomCodeSequenceItem();
            conceptNameCodeSequence.CodingSchemeDesignator = strCodeSchemeDesignator;
            conceptNameCodeSequence.CodeValue = strCodeValue;
            conceptNameCodeSequence.CodeMeaning = strCodeMeaning;

            ds.SetEncapsulatedDocument(dElement, child, sFileDocumentIn, encapsulatedDocument, conceptNameCodeSequence);
        }

        private void ResetModule(DicomModuleType moduleType, DicomDataSet dataset, bool bKeepOriginalElements)
        {
            if (bKeepOriginalElements)
            {
                DicomModule module = dataset.FindModule(moduleType);
                if (module == null)
                    return;

                byte[] b = new byte[1] { 0 };
                foreach (DicomElement item in module.Elements)
                {
                    if (item.Length == 0)
                        continue;

                    DicomElement element = dataset.FindFirstElement(null, item.Tag, true);
                    if (element != null)
                    {
                        dataset.SetBinaryValue(element, b, 0);
                    }
                }
            }
            else
            {
                dataset.DeleteModule(moduleType);
                dataset.InsertModule(moduleType, false);
            }
        }
       
        private string DoSave(DicomDataSet dicom, ref List<string> lstSaved, string strSaveLocation, ref bool bSuccess)
        {
            string strMessage = "";
            if (strMessage == "")
                try
                {
                    lstSaved = SaveDicom(dicom, strSaveLocation);
                    strMessage = "DICOM file was saved successfully\n";
                    bSuccess = lstSaved.Count > 0;
                    if (lstSaved.Count > 0)
                    {
                        foreach (string str in lstSaved)
                        {
                            strMessage += "--> " + str + "\n";
                        }
                    }
                }
                catch (Exception ex)
                {
                    strMessage = "DICOM file was not saved successfully, Reason:\n" + ex.Message;
                }
            return strMessage;
        }

        private void GetRequiredTags(DicomDataSet dicom, List<string> lstRequired)
        {
            DicomIod iod;
            DicomIodTable iodTable = DicomIodTable.Instance;
            DicomEditableObject editable = (DicomEditableObject)_pgDicomInfo.SelectedObject;
            DicomModule module;
            DicomIod IODClass = DicomIodTable.Instance.FindClass(dicom.InformationClass);
            for (int i = 0; i < dicom.ModuleCount; i++)
            {
                module = dicom.FindModuleByIndex(i);
                for (int j = 0; j < module.Elements.Length; j++)
                {
                    DicomElement dElement = module.Elements[j];
                    if (dElement.Length > 0)
                        continue;

                    iod = DicomIodTable.Instance.Find(IODClass, dElement.Tag, DicomIodType.Element, false);
                    if (!((iod != null) && (iod.Usage == DicomIodUsageType.Type1MandatoryElement) && (dElement.Length == 0) && (dElement.Length != ELEMENT_LENGTH_MAX)))
                        continue;

                    if (!lstRequired.Contains(iod.Name))
                        lstRequired.Add(iod.Name);

                }
            }
        }

        private void GenerateUidTag(DicomDataSet dicom, long UidTag)
        {
            DicomElement element;
            element = dicom.FindFirstElement(null, UidTag, true);
            // Bảng IOD/UID của Leadtools rỗng khi app chạy ngoài bộ demo LEADTOOLS nên
            // Initialize() không tạo sẵn các element UID — phải tự chèn, nếu không file
            // DICOM sẽ thiếu UID và store lên PACS báo "Missing SOP Instance UID Value"
            if (element == null)
                element = dicom.InsertElement(null, false, UidTag, DicomVRType.UI, false, 0);
            dicom.SetValue(element, Utils.GenerateDicomUniqueIdentifier());

            _pgDicomInfo.DataSet = dicom;
        }

        /// <summary>
        /// Lấy SOP Class UID tương ứng với loại SOP class đang chọn.
        /// Không tra DicomUidTable vì bảng này rỗng khi chạy ngoài bộ demo LEADTOOLS.
        /// </summary>
        private static string GetSopClassUid(DicomClassType dClass)
        {
            switch (dClass)
            {
                case DicomClassType.SCMultiFrameGrayscaleByteImageStorage:
                    return DicomUidType.SCMultiFrameGrayscaleByteImageStorage;
                case DicomClassType.SCMultiFrameTrueColorImageStorage:
                    return DicomUidType.SCMultiFrameTrueColorImageStorage;
                case DicomClassType.EncapsulatedPdfStorage:
                    return DicomUidType.EncapsulatedPdfStorage;
                default:
                    return DicomUidType.SCImageStorage;
            }
        }

        #endregion
    }
}
