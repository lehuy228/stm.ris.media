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
        #region UI Control Events - PictureBox & Image Viewer

        private void _miNormal_Click(object sender, EventArgs e)
        {
            try
            {
                _pictureBox.SizeMode = RasterPaintSizeMode.Normal;
                _pictureBox.ScaleFactor = 1;
            }
            catch (Exception exp)
            {
                Messager.ShowError(this, exp);
            }
        }

        private void _miFit_Click(object sender, EventArgs e)
        {
            try
            {
                _pictureBox.SizeMode = RasterPaintSizeMode.FitAlways;
                _pictureBox.ScaleFactor = 1;
            }
            catch (Exception exp)
            {
                Messager.ShowError(this, exp);
            }
        }

        private void _cmResultQuery_Opening(object sender, CancelEventArgs e)
        {
            //if (_lstSelected == null || _lstSelected.Items.Count == 0 || _lstSelected == _lstSCPStudies)
            //{
            //    e.Cancel = true;
            //}
            //toolStripSeparator22.Visible = _miDeleteSelectedDataSet.Visible = _lstSelected == _lstDSPatient;
            //_miDeleteSelectedDataSet.Enabled = _lstDSPatient.SelectedItems.Count >= 1;
        }

        private void _miZoomIn_Click(object sender, EventArgs e)
        {
            try
            {
                ZoomPicture(0.1f);
            }
            catch (Exception exp)
            {
                Messager.ShowError(this, exp);
            }
        }

        private void _miZoomOut_Click(object sender, EventArgs e)
        {
            try
            {
                if (_pictureBox.ScaleFactor > 0.1f)
                {
                    ZoomPicture(-0.1f);
                }
            }
            catch (Exception exp)
            {
                Messager.ShowError(this, exp);
            }
        }

        private void _pictureBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Add)
                {
                    _miZoomIn_Click(_miZoomIn, new EventArgs());
                }
                else if (e.KeyCode == Keys.Subtract)
                {
                    _miZoomOut_Click(_miZoomOut, new EventArgs());
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void _miSaveAsDICOM_Click(object sender, EventArgs e)
        {
            DicomDataSet dicom = (_pgDicomInfo.SelectedObject as DicomEditableObject).DataSet;
            if (!CheckRequiredTags(dicom))
                return;

            SaveFileDialog dlgSave = new SaveFileDialog();
            dlgSave.Filter = "DICOM Files|*.dcm|DICOM DataSet Files|*.dic";
            if (strLastLocation != "")
                dlgSave.InitialDirectory = Path.GetDirectoryName(strLastLocation);

            bool bTopMost = logWindow.TopMost;
            logWindow.TopMost = false;
            DialogResult dlgRes = dlgSave.ShowDialog();

            if (dlgRes == DialogResult.Cancel)
            {
                logWindow.TopMost = bTopMost;
                return;
            }
            try
            {
                List<string> lstSaved = new List<string>();
                string strSaveLocation = dlgSave.FileName;
                strLastLocation = strSaveLocation;
                bool bSuccess = false;
                EnableItems(false, "Saving Files To HardDisk Please Wait...", "Cancel");
                string strMessage = DoSave(dicom, ref lstSaved, strSaveLocation, ref bSuccess);

                MessageBoxIcon icon = MessageBoxIcon.Information;
                if (bSuccess)
                    icon = MessageBoxIcon.Information;
                else
                    icon = MessageBoxIcon.Error;

                EnableItems(true, "", "");
                if (bSuccess)
                {
                    DialogResult dlgClear = MessageBox.Show(this, strMessage + "\nDo you want to clear the DICOM information?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlgClear == DialogResult.Yes)
                    {
                        _miClearPG_Click(null, null);
                    }
                }
                else
                {
                    MessageBox.Show(this, "DICOM file was not saved successfully", this.Text, MessageBoxButtons.OK, icon);
                }
            }
            catch { }
            logWindow.TopMost = bTopMost;
        }

        private bool CheckRequiredTags(DicomDataSet dicom)
        {
            string strMessage = "";
            List<string> lstRequired = new List<string>();
            GetRequiredTags(dicom, lstRequired);

            DicomElement dElement = dicom.FindFirstElement(null, DicomTag.PatientName, true);
            string val = dicom.GetValue<string>(dElement, "");


            if (val == string.Empty)
                lstRequired.Add("Patient Name");

            dElement = dicom.FindFirstElement(null, DicomTag.PatientID, true);
            val = dicom.GetValue<string>(dElement, "");
            if (val == string.Empty)
                lstRequired.Add("Patient ID");

            if (lstRequired.Count > 0)
            {
                strMessage = "The Following Tags Are Required:\n";
                foreach (string strName in lstRequired)
                {
                    strMessage += "--> " + strName + "\n";
                }
            }

            if (_lstBoxPages.CheckedItems.Count == 0 && strMessage == "")
            {
                strMessage = "One or more Print job/pages needs to be checked";
            }
            if (strMessage != "")
            {
                bool bTopMost = logWindow.TopMost;
                logWindow.TopMost = false;
                MessageBox.Show(this, strMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                logWindow.TopMost = bTopMost;
                return false;
            }
            else
                return true;
        }


        private void _miClearPG_Click(object sender, EventArgs e)
        {
            DicomDataSet ds = _pgDicomInfo.DataSet;
            ds.Dispose();
            _pgDicomInfo.DataSet = null;
            _cmbSopClasses_SelectedIndexChanged(null, null);
        }

        private void _lstBoxPages_ItemAdded(object sender, System.EventArgs e)
        {
        }

        private void _cmbSopClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cmbSopClasses.SelectedIndex >= 0)
            {
                DicomDataSet tempDataSet = new DicomDataSet(), sourceDataSet = _pgDicomInfo.DataSet;
                DicomModule module = null;
                //Clone the dataset
                if (sourceDataSet != null)
                {
                    tempDataSet.Initialize(_pgDicomInfo.DataSet.InformationClass, DicomDataSetInitializeFlags.AddMandatoryElementsOnly |
                    DicomDataSetInitializeFlags.AddMandatoryModulesOnly);

                    module = sourceDataSet.FindModule(DicomModuleType.GeneralStudy);
                    if (module != null)
                        SetElements(tempDataSet, module.Elements, sourceDataSet);

                    module = sourceDataSet.FindModule(DicomModuleType.Patient);
                    if (module != null)
                        SetElements(tempDataSet, module.Elements, sourceDataSet);
                }

                InitializeDataSet(ClassTypes[_cmbSopClasses.SelectedIndex]);

                //Restore the dataset
                if (sourceDataSet != null)
                {
                    sourceDataSet = tempDataSet;

                    module = sourceDataSet.FindModule(DicomModuleType.GeneralStudy);
                    if (module != null)
                        SetElements(_pgDicomInfo.DataSet, module.Elements, sourceDataSet);

                    module = sourceDataSet.FindModule(DicomModuleType.Patient);
                    if (module != null)
                        SetElements(_pgDicomInfo.DataSet, module.Elements, sourceDataSet);

                    GenerateDefaultElements();
                }
                else
                {
                    GenerateDefaultElements();
                    InsertNewStudyModule();
                }

            }
        }


        private void InitializeDataSet(DicomClassType dClass)
        {
            DicomDataSet ds = new DicomDataSet();
            try
            {
                if (dClass == DicomClassType.SCImageStorage)
                    if (File.Exists(_mySettings._settings.secondaryCapturePath))
                        DicomExtensions.LoadXml(ds, _mySettings._settings.secondaryCapturePath, DicomDataSetLoadXmlFlags.None, null, null);

                if (dClass == DicomClassType.SCMultiFrameTrueColorImageStorage)
                    if (File.Exists(_mySettings._settings.secondaryCaptureColorPath))
                        DicomExtensions.LoadXml(ds, _mySettings._settings.secondaryCaptureColorPath, DicomDataSetLoadXmlFlags.None, null, null);

                if (dClass == DicomClassType.SCMultiFrameGrayscaleByteImageStorage)
                    if (File.Exists(_mySettings._settings.secondaryCaptureGrayPath))
                        DicomExtensions.LoadXml(ds, _mySettings._settings.secondaryCaptureGrayPath, DicomDataSetLoadXmlFlags.None, null, null);

                if (dClass == DicomClassType.EncapsulatedPdfStorage)
                    if (File.Exists(_mySettings._settings.PdfPath))
                        DicomExtensions.LoadXml(ds, _mySettings._settings.PdfPath, DicomDataSetLoadXmlFlags.None, null, null);
            }
            catch (Exception ex)
            {
                // Template XML lỗi thì dataset sẽ được Initialize mặc định bên dưới - vẫn cần log để biết
                Log.Warning(ex, "Không load được template DICOM XML cho class {DicomClass}", dClass);
            }

            if (ds == null || ds.InformationClass != dClass)
                ds.Initialize(dClass, DicomDataSetInitializeFlags.AddMandatoryElementsOnly |
                   DicomDataSetInitializeFlags.AddMandatoryModulesOnly);

            ClearTag(ds, DicomTag.PixelData);
            ClearTag(ds, DicomTag.EncapsulatedDocument);

            // SOP Class UID phải khớp loại SOP class đang chọn. Initialize() không tạo được
            // element này khi bảng IOD của Leadtools rỗng nên phải chèn và set trực tiếp
            DicomElement dSopClass = ds.FindFirstElement(null, DicomTag.SOPClassUID, true);
            if (dSopClass == null)
                dSopClass = ds.InsertElement(null, false, DicomTag.SOPClassUID, DicomVRType.UI, false, 0);
            ds.SetValue(dSopClass, GetSopClassUid(dClass));

            DicomElement dElement = ds.FindFirstElement(null, DicomTag.Modality, true);
            if (dElement == null)
                dElement = ds.InsertElement(null, false, DicomTag.Modality, DicomVRType.UN, false, 0);
            ds.SetValue(dElement, _chiDinhDichVuResponse?.Modality ?? "OT");

            _pgDicomInfo.DataSet = ds;
        }

        public void LoadRasterImage(string strFileName)
        {
            bool bTopMost = logWindow.TopMost;
            RasterImage rImg = null;
            try
            {
                //EnableItems(false, "Opening Image Files Please Wait...", "Cancel");
                string strFile = strFileName;
                strLastLocation = strFile;

                _codec.Options.Load.AllPages = true;
                _codec.Options.Pdf.Load.DisplayDepth = 24;
                _codec.Options.Pdf.Load.GraphicsAlpha = 4;
                _codec.Options.Pdf.Load.TextAlpha = 2;
                _codec.Options.Pdf.Load.UseLibFonts = true;

                // Nếu cần siêu nét cho in ấn thì có thể lên 600 DPI
                _codec.Options.RasterizeDocument.Load.XResolution = 600;
                _codec.Options.RasterizeDocument.Load.YResolution = 600;
                _codec.Options.RasterizeDocument.Load.Resolution = 600;

                rImg = _codec.Load(strFile);

                GrayscaleCommand command = new GrayscaleCommand(8);
                if (rImg.IsGray && rImg.BitsPerPixel != 8)
                    command.Run(rImg);

                ListImageBox.ImageCollection imagecollection = new ListImageBox.ImageCollection(strFile);
                Page page = new Page();
                for (int i = 1; i <= rImg.PageCount; i++)
                {
                    string strTemp = null;
                    rImg.Page = i;

                    page = new Page();
                    strTemp = Path.GetTempFileName();
                    int iBPP = rImg.BitsPerPixel;
                    if (iBPP < 8)
                        iBPP = 8;
                    RasterImage rTempRaster = rImg.Clone();
                    _codec.Save(rTempRaster, strTemp, RasterImageFormat.Tif, iBPP);
                    rTempRaster.Dispose();
                    page.FilePath = strTemp;
                    page.DeleteOnDispose = true;
                    imagecollection.Images.Add(new ListImageBox.ImageItem(_codec.Load(strTemp), imagecollection, page));
                    Application.DoEvents();
                    if (bCancelOperation)
                        break;
                }
                rImg.Dispose();

                if (listImageKeyLocal != null && listImageKeyLocal.Contains(strFileName, StringComparer.OrdinalIgnoreCase))
                {
                    imagecollection.Images[0].Checked = true;
                }
                else
                {
                    Log.Debug("File {FileName} không có trong danh sách ảnh đã chọn", strFileName);
                }

                _lstBoxPages.AddImageCollection(imagecollection);
            }
            catch (System.Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
                rImg?.Dispose(); //Luôn dispose
            }
            EnableItems(true, "", "");
            logWindow.TopMost = bTopMost;
        }

        private void GenerateDefaultElements()
        {
            GenerateUidTag(_pgDicomInfo.DataSet, DicomTag.SeriesInstanceUID);
            GenerateUidTag(_pgDicomInfo.DataSet, DicomTag.SOPInstanceUID);

            DicomElement dElement = _pgDicomInfo.DataSet.FindFirstElement(null, DicomTag.InstanceNumber, false);
            if (dElement != null)
                _pgDicomInfo.DataSet.SetValue(dElement, "1");

            dElement = _pgDicomInfo.DataSet.FindFirstElement(null, DicomTag.ConversionType, false);
            if (dElement != null)
                _pgDicomInfo.DataSet.SetValue(dElement, "DI");

            dElement = _pgDicomInfo.DataSet.FindFirstElement(null, DicomTag.SeriesNumber, false);
            if (dElement != null)
                _pgDicomInfo.DataSet.SetValue(dElement, "1");

            dElement = _pgDicomInfo.DataSet.FindFirstElement(null, DicomTag.FrameIncrementPointer, false);
            if (dElement != null)
                _pgDicomInfo.DataSet.SetValue(dElement, 0x182001); //HEX 2C6F1H

            dElement = _pgDicomInfo.DataSet.FindFirstElement(null, DicomTag.MIMETypeOfEncapsulatedDocument, false);
            if (dElement != null)
                _pgDicomInfo.DataSet.SetValue(dElement, "PDF");

            if (_pgDicomInfo.DataSet.InformationClass == DicomClassType.SCMultiFrameGrayscaleByteImageStorage ||
               _pgDicomInfo.DataSet.InformationClass == DicomClassType.SCMultiFrameTrueColorImageStorage)
            {
                dElement = _pgDicomInfo.DataSet.FindFirstElement(null, DicomTag.PageNumberVector, true);
                if (dElement == null)
                    dElement = _pgDicomInfo.DataSet.InsertElement(null, false, DicomTag.PageNumberVector, DicomVRType.IS, false, 0);
            }

            _pgDicomInfo.DataSet = _pgDicomInfo.DataSet;
        }

        private void InsertNewSeries()
        {
            DicomDataSet ds = _pgDicomInfo.DataSet;
            DicomElement dElement = ds.FindFirstElement(null, DicomTag.Modality, true);
            if (dElement == null)
                dElement = ds.InsertElement(null, false, DicomTag.Modality, DicomVRType.UN, false, 0);
            if (ds.InformationClass == DicomClassType.EncapsulatedPdfStorage)
                ds.SetValue(dElement, "DOC");
            else
                ds.SetValue(dElement, _chiDinhDichVuResponse.Modality);
            _pgDicomInfo.DataSet = ds;
        }

        private void InsertNewStudyModule()
        {
            DicomElement dElement;
            GenerateUidTag(_pgDicomInfo.DataSet, DicomTag.StudyInstanceUID);

            dElement = _pgDicomInfo.DataSet.FindFirstElement(null, DicomTag.StudyDate, false);
            if (dElement != null)
                _pgDicomInfo.DataSet.SetDateValue(dElement, new DateTime[] { DateTime.Now.Date });

            dElement = _pgDicomInfo.DataSet.FindFirstElement(null, DicomTag.StudyTime, false);
            if (dElement != null)
                _pgDicomInfo.DataSet.SetTimeValue(dElement, new DateTime[] { new DateTime(DateTime.Now.Year, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) });

            dElement = _pgDicomInfo.DataSet.FindFirstElement(null, DicomTag.StudyID, false);
            if (dElement != null)
                _pgDicomInfo.DataSet.SetValue(dElement, "1");
            _pgDicomInfo.DataSet = _pgDicomInfo.DataSet;
        }

        private void _miEdit_DropDownOpening(object sender, EventArgs e)
        {
            _miRotate90.Enabled = _miDeleteSelected.Enabled = (_lstBoxPages.SelectedItems.Count > 0);
            _miDeleteAll.Enabled = (_lstBoxPages.Items.Count > 0);
            _miPaste.Enabled = Clipboard.ContainsImage();
        }

        private void _miDeleteSelected_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc muốn xóa mục đã chọn (Ảnh sẽ mất vĩnh viễn)?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
                return; // Người dùng không đồng ý → thoát

            // Nếu chọn Yes thì tiếp tục xóa
            DeleteSelectedItems();

            if (_pictureBox.Image != null)
            {
                _pictureBox.Image.Dispose();
                _pictureBox.Image = null;
            }

            UpdateToolBarState();
        }

        private void _miResetInfo_Click(object sender, EventArgs e)
        {
            _miClearPG_Click(null, null);
        }

        private void _toolBtnStoreToPacs_Click(object sender, EventArgs e)
        {
            _miStoreToPACS_Click(null, null);
        }

        private void _toolBtnSaveDicom_Click(object sender, EventArgs e)
        {
            _miSaveAsDICOM_Click(null, null);
        }

        private void _toolBtnCLearInfo_Click(object sender, EventArgs e)
        {
            _miClearPG_Click(null, null);
        }

        private void _toolBtnDeleteAll_Click(object sender, EventArgs e)
        {
            _miClearPrintedList_Click(null, null);
        }

        private void _toolBtnDeleteSelected_Click(object sender, EventArgs e)
        {
            _miDeleteSelected_Click(null, null);
        }

        private void _toolBtnViewLog_Click(object sender, EventArgs e)
        {
            _miViewLog_Click(null, null);
        }

        private void _miViewLog_Click(object sender, EventArgs e)
        {
            logWindow.Visible = !logWindow.Visible;
            UpdateToolBarState();
        }

        private void _toolBtnOpenRaster_Click(object sender, EventArgs e)
        {
            _miOpen_Click(null, null);
        }

        private void _miView_DropDownOpening(object sender, EventArgs e)
        {
            _miResample.Enabled = _miFit.Enabled = _miNormal.Enabled = _miZoomIn.Enabled = _miZoomOut.Enabled = (_pictureBox.Image != null);
            RasterPaintProperties prop = _pictureBox.PaintProperties;
            _miResample.Checked = (prop.PaintDisplayMode == RasterPaintDisplayModeFlags.Resample);
            _miNormal.Checked = _pictureBox.SizeMode == RasterPaintSizeMode.Normal;
            _miFit.Checked = _pictureBox.SizeMode == RasterPaintSizeMode.FitAlways;
            _miViewLog.Checked = logWindow.Visible;
            double oldScaleFactor = _pictureBox.ScaleFactor, dZoomFactor = 0.1;
            oldScaleFactor = _pictureBox.ScaleFactor + dZoomFactor;
            _miZoomIn.Enabled = _pictureBox.Image != null && !(oldScaleFactor > 3 && dZoomFactor > 0);
            oldScaleFactor = _pictureBox.ScaleFactor - dZoomFactor;
            _miZoomOut.Enabled = _pictureBox.Image != null && !(oldScaleFactor < .06 && -dZoomFactor < 0);
        }

        private void _cbSevers_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyServer server = (toolStripComboBoxStoreServer.SelectedItem as MyServer);
        }

        private void _toolBtnScreenCapture_Click(object sender, EventArgs e)
        {
            _engine.StopCapture();
            bool bTemp = _isHotKeyEnabled;
            _isHotKeyEnabled = false;
            Leadtools.ScreenCapture.ScreenCaptureOptions opt = _engine.CaptureOptions;
            Keys oldKey = opt.Hotkey;
            opt.Hotkey = Keys.None;
            _engine.CaptureOptions = opt;
            DoCapture(_mySettings._settings.capturetype);
            _isHotKeyEnabled = bTemp;
            opt.Hotkey = oldKey;
            _engine.CaptureOptions = opt;
        }

        void _pictureBox_DoubleClick(object sender, EventArgs e)
        {
        }

        private async void _btnPushToPACS_Click(object sender, EventArgs e)
        {
            try
            {
                _toolBtnStoreToPacs_Click(null, null);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi bấm nút Push to PACS");
            }
        }
        #endregion
    }
}
