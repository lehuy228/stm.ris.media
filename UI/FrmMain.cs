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
using PrintToPACS.Utilities;
using Leadtools.Dicom.Scu.Common;
using PrinterDemo;
using Leadtools.Dicom.Scu;
using System.Diagnostics;
using Leadtools.Dicom.Common.DataTypes.Modality;
using PrintToPACSDemo.UI;
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
using PrintToPACSDemo.UI.CameraUI;
using Leadtools.Medical.Worklist.DataAccessLayer;
using Leadtools.Medical.DataAccessLayer;
using Leadtools.Medical.DataAccessLayer.Configuration;
using Leadtools.Medical.Worklist.DataAccessLayer.Configuration;
using Leadtools.Medical.Winforms;
using System.Data;
using System.Linq;
using PrintToPACSDemo.UI.Conclusion;
using VisioForge.Core.Helpers;
using DevExpress.XtraEditors;
using MediaToPacs.Entitys.Domain;

namespace PrintToPACSDemo
{
    public partial class FrmMain : DevExpress.XtraEditors.XtraForm
    {
        #region Main...
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        const string sHelpInstructions =
        "Command Line Options:" + _sNewlineTab +
        "/? or /help\t\tDisplays this help" + _sNewlineTab +
        "/configure\t\tConfigures the client (use one or more options below)" + _sNewlineTab +
        "/server_aetitle={aetitle}\tServer AE title" + _sNewlineTab +
        "/server_ip={ip address}\tServer IP" + _sNewlineTab +
        "/server_port={port}\tServer Port" + _sNewlineTab +
        "/client_aetitle={aetitle}\tClient AE title" + _sNewlineTab +
        "/client_port={port}\t\tClient Port" + _sNewlineTab +
        "/defaults\t\t\tSets defaults for other options";

        static MyServer ParseOneServer(string serverString)
        {
            //   /servers=ae1,ip1,port1,timeout1,secure1

            MyServer server = null;
            string[] fields = serverString.Split(',');
            if (fields.Length == 5)
            {
                server = new MyServer();
                server._sAE = fields[0].Trim();
                server._sIP = fields[1].Trim();
                server._port = Convert.ToInt32(fields[2].Trim());
                server._timeout = Convert.ToInt32(fields[3].Trim());
                server._useTls = Convert.ToBoolean(fields[4].Trim());
            }
            return server;
        }

        static MyServer[] ParseServerList(string serversString)
        {
            //   /servers=ae1,ip1,port1,timeout1,secure1;ae1,ip1,port1,timeout1,secure1
            serversString.Trim();
            if (serversString.EndsWith(";"))
                serversString = serversString.Substring(0, serversString.Length - 1);
            string[] servers = serversString.Split(';');

            ArrayList list = new ArrayList();
            foreach (string s in servers)
            {
                MyServer server = ParseOneServer(s);
                list.Add(server);
            }

            MyServer[] items = new MyServer[servers.Length];
            list.CopyTo(items);
            return items;
        }

        static string GetDefaultIp()
        {
            ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'TRUE'");
            ManagementObjectCollection queryCollection = query.Get();

            foreach (ManagementObject mo in queryCollection)
            {
                if (queryCollection.Count > 0)
                {
                    string[] addresses = (string[])mo["IPAddress"];

                    foreach (string ip in addresses)
                    {
                        if (!ip.Contains(":") && (ip != "0.0.0.0"))
                            return ip;
                    }
                }
            }
            return string.Empty;
        }

        static bool ReadCommandLine(string[] args)
        {
            return false;
        }
        //        [STAThread]
        //        static void Main(string[] args)
        //        {
        //            try
        //            {
        //                bool bConfigure = ReadCommandLine(args);
        //                if (bConfigure)
        //                    return;
        //            }
        //            catch { }

        //#if LEADTOOLS_V175_OR_LATER
        //            Support.SetLicense();
        //#else
        //         Support.Unlock(false);
        //#endif

        //            if (Support.KernelExpired)
        //                return;

        //            if (args.Length > 0)
        //            {
        //                FrmMain.StartedPrinter = args[0];
        //                MySettings mySettings = new MySettings();
        //                mySettings.Load();
        //                if (FrmMain.StartedPrinter != mySettings._settings.printerName)
        //                    return;
        //            }

        //            Utils.EngineStartup();
        //            Utils.DicomNetStartup();
        //            Application.EnableVisualStyles();
        //            Application.SetCompatibleTextRenderingDefault(false);
        //            Application.Run(new FrmMain());
        //        }
        #endregion

        #region Constructor...
        private ModalityWorklistResult result;
        private MPPSNCreate mppsCreate;
        private WorkListTable _workListTable;


        public FrmMain(WorkListTable workListTable, ModalityWorklistResult result, MPPSNCreate mppsCreate)
        {
            try
            {
                InitClass();
                InitializeComponent();
                this.result = result;
                this.mppsCreate = mppsCreate;
                this._workListTable = workListTable;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
        #endregion

        #region Fields...

        public delegate void AddLog(string action, string logText);
        public delegate void AddLogColor(string action, string logText, Color sActionColor);
        public delegate void EnableMenu(bool enable, string strCaption, string strBtnCaption);

        private const string _sNewline = "\r\n";
        private const string _sNewlineTab = "\r\n\t";
        private const string _sNewlineTabTab = "\r\n\t\t";
        private FrmProgress _frmProgress;
        private FrmOperation _frmOperation;
        private ListImageBox.ImageCollection imgCollection = null;
        private int _pageNo = 0;
        private int _jobId = 0;
        public static string StartedPrinter = string.Empty;
        bool bFinishedPrinting = false;
        int iOldY = -1, iOldX = -1, _iOldIndex = -1;
        private RasterCodecs _codec;

        private bool bCancelOperation = false;


        public const string _sConfigurationImplementationClass = "1.2.840.114257.1123456";
        public const string _sConfigurationImplementationVersionName = "1";
        public const string _sConfigurationProtocolversion = "1";

        public static FlowLayoutPanel FPLRoll;

        private TextBoxTraceListener _tracer = null;
        private StoreScu _cstore;
        private bool bStored = false;
        public MySettings _mySettings = PacsSettings.Instance;
        List<DataGridViewRow> OldRowSelection = new List<DataGridViewRow>();
        List<DataGridViewCell> OldCellSelection = new List<DataGridViewCell>();
        LogWindow logWindow = PacsSettings.LogWindow;
        ListView _lstSelected;
        List<long> DICOMPatientInfo = new List<long>()
      {
         DicomTag.PatientName,
         DicomTag.PatientID,
         DicomTag.PatientSex,
         DicomTag.PatientBirthDate

      };

        List<long> DICOMStudyInfo = new List<long>()
      {
         DicomTag.StudyID,
         DicomTag.ReferringPhysicianName,
         DicomTag.AccessionNumber,
         DicomTag.StudyDate,
         DicomTag.StudyTime
      };

        List<DicomClassType> ClassTypes = new List<DicomClassType>(){
         DicomClassType.SCImageStorage,
         DicomClassType.SCMultiFrameGrayscaleByteImageStorage,
         DicomClassType.SCMultiFrameTrueColorImageStorage,
         DicomClassType.EncapsulatedPdfStorage
      };

        private const long ELEMENT_LENGTH_MAX = (long)0xFFFFFFFFUL;
        private List<long> _ExcludedTags = new List<long>();

        string strLastLocation = "";
        #endregion

        #region Forms Events...

        /*TEMP*/
        private Leadtools.Dicom.Common.Editing.Controls.DicomPropertyGrid _pgDicomInfo;
        private Leadtools.Dicom.Common.Editing.DicomEditableObject DicomEditableObject;
        private Leadtools.WinForms.RasterImageViewer _pictureBox = new Leadtools.WinForms.RasterImageViewer();

        private void FrmMain_Load(object sender, EventArgs e)
        {
            propertyGridControl1.OptionsBehavior.PropertySort = DevExpress.XtraVerticalGrid.PropertySort.Alphabetical;
            propertyGridControl1.SelectedObject = new MedicalReport();
            //
            // Add Excluded Tags
            //
            _ExcludedTags.Add(DicomTag.SOPClassUID);
            _ExcludedTags.Add(DicomTag.SOPInstanceUID);
            _ExcludedTags.Add(DicomTag.StudyInstanceUID);
            _ExcludedTags.Add(DicomTag.SeriesInstanceUID);
            _ExcludedTags.Add(DicomTag.MediaStorageSOPClassUID);
            _ExcludedTags.Add(DicomTag.FrameIncrementPointer);
            _ExcludedTags.Add(DicomTag.MIMETypeOfEncapsulatedDocument);
            _ExcludedTags.Add(DicomTag.PageNumberVector);

            try
            {
                InitializeForm();
                //InitializeTwain();
                SetServersComboBox(true);
                InitializeScreenCapture();
                InitTranfer(result);
                DateTime tmStart = DateTime.Now;

                //while (!bFinishedPrinting && (DateTime.Now - tmStart).TotalSeconds < 20)
                //    Application.DoEvents();

                Deserialize(_mySettings._settings.DataPath);

                //Initialize Store and Query Options
                CreateCStoreObject(new MyServer());
                UpdateToolBarState();

                _captureType = CaptureType.None;
                CheckFirstRun();
                _mySettings._settings.FirstRun = false;
                _mySettings.Save();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }


        private void InitTranfer(ModalityWorklistResult result)
        {
            ResetModule(DicomModuleType.GeneralSeries, _pgDicomInfo.DataSet, true);
            GenerateDefaultElements();
            InsertNewSeries();

            DicomDataSet ds = _pgDicomInfo.DataSet;

            //Study
            DicomElement dElement;
            if (result.AccessionNumber != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.AccessionNumber, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.AccessionNumber, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, result.AccessionNumber);
            }

            if (result.ReferringPysician != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.ReferringPhysicianName, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.ReferringPhysicianName, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, result.ReferringPysician.FullDicomEncoded);
            }

            //Patient
            if (result.PatientName != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.PatientName, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.PatientName, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, result.PatientName.FullDicomEncoded);
            }

            if (result.PatientId != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.PatientID, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.PatientID, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, result.PatientId);
            }

            if (result.PatientSex != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.PatientSex, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.PatientSex, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, result.PatientSex);
            }

            if (result.PatientBirthDate != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.PatientBirthDate, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.PatientBirthDate, DicomVRType.UN, false, 0);
                ds.SetDateValue(dElement, new DateTime[] { (DateTime)result.PatientBirthDate });
            }

            if (result.RequestedProcedureId != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.StudyID, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.StudyID, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, result.RequestedProcedureId);
            }

            if (result.StudyInstanceUid != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.StudyInstanceUID, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.StudyInstanceUID, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, result.StudyInstanceUid);
            }

            _pgDicomInfo.DataSet = ds;
        }

        void _pgDicomInfo_BeforeAddElement(object sender, BeforeAddElementEventArgs e)
        {
            //
            // Excluded items and Volatile Elements will not be displayed in the editor
            //
            e.Cancel = _ExcludedTags.Contains(e.Element.DicomElement.Tag) || e.Element.DataSet.IsVolatileElement(e.Element.DicomElement);
            if (!e.Cancel)
            {
                if (e.Element.DicomElement.Tag == DicomTag.ConversionType)
                {
                    e.Element.Choices.Add("DV");
                    e.Element.Choices.Add("DI");
                    e.Element.Choices.Add("DF");
                    e.Element.Choices.Add("WSD");
                    e.Element.Choices.Add("SD");
                    e.Element.Choices.Add("SI");
                    e.Element.Choices.Add("DRW");
                    e.Element.Choices.Add("SYN");
                    e.Element.Attributes.Add(new TypeConverterAttribute(typeof(DicomPropertyChoiceConverter)));
                }
                else if (e.Element.DicomElement.Tag == DicomTag.Modality)
                {
                    e.Element.Choices.Add("CR");
                    e.Element.Choices.Add("CT");
                    e.Element.Choices.Add("MR");
                    e.Element.Choices.Add("NM");
                    e.Element.Choices.Add("US");
                    e.Element.Choices.Add("OT");
                    e.Element.Choices.Add("BI");
                    e.Element.Choices.Add("DG");
                    e.Element.Choices.Add("ES");
                    e.Element.Choices.Add("LS");
                    e.Element.Choices.Add("PT");
                    e.Element.Choices.Add("RG");
                    e.Element.Choices.Add("TG");
                    e.Element.Choices.Add("XA");
                    e.Element.Choices.Add("RF");
                    e.Element.Choices.Add("RTIMAGE");
                    e.Element.Choices.Add("RTDOSE");
                    e.Element.Choices.Add("RTSTRUCT");
                    e.Element.Choices.Add("RTPLAN");
                    e.Element.Choices.Add("RTRECORD");
                    e.Element.Choices.Add("HC");
                    e.Element.Choices.Add("DX");
                    e.Element.Choices.Add("MG");
                    e.Element.Choices.Add("IO");
                    e.Element.Choices.Add("PX");
                    e.Element.Choices.Add("GM");
                    e.Element.Choices.Add("SM");
                    e.Element.Choices.Add("XC");
                    e.Element.Choices.Add("PR");
                    e.Element.Choices.Add("AU");
                    e.Element.Choices.Add("ECG");
                    e.Element.Choices.Add("EPS");
                    e.Element.Choices.Add("HD");
                    e.Element.Choices.Add("SR");
                    e.Element.Choices.Add("IVUS");
                    e.Element.Choices.Add("OP");
                    e.Element.Choices.Add("SMR");
                    e.Element.Choices.Add("AR");
                    e.Element.Choices.Add("KER");
                    e.Element.Choices.Add("VA");
                    e.Element.Choices.Add("SRF");
                    e.Element.Choices.Add("OCT");
                    e.Element.Choices.Add("LEN");
                    e.Element.Choices.Add("OPV");
                    e.Element.Choices.Add("OPM");
                    e.Element.Choices.Add("OAM");
                    e.Element.Choices.Add("RESP");
                    e.Element.Choices.Add("KO");
                    e.Element.Choices.Add("SEG");
                    e.Element.Choices.Add("REG");
                    e.Element.Choices.Add("OPT");
                    e.Element.Choices.Add("BDUS");
                    e.Element.Choices.Add("BMD");
                    e.Element.Choices.Add("DOC");
                    e.Element.Choices.Add("FID");
                    e.Element.Choices.Add("DS");
                    e.Element.Choices.Add("CF");
                    e.Element.Choices.Add("DF");
                    e.Element.Choices.Add("VF");
                    e.Element.Choices.Add("AS");
                    e.Element.Choices.Add("CS");
                    e.Element.Choices.Add("EC");
                    e.Element.Choices.Add("LP");
                    e.Element.Choices.Add("FA");
                    e.Element.Choices.Add("CP");
                    e.Element.Choices.Add("DM");
                    e.Element.Choices.Add("FS");
                    e.Element.Choices.Add("MA");
                    e.Element.Choices.Add("MS");
                    e.Element.Choices.Add("CD");
                    e.Element.Choices.Add("DD");
                    e.Element.Choices.Add("ST");
                    e.Element.Choices.Add("OPR");
                    e.Element.Attributes.Add(new TypeConverterAttribute(typeof(DicomPropertyChoiceConverter)));
                }
            }
        }

        private void _miResample_Click(object sender, EventArgs e)
        {
            RasterPaintProperties prop = _pictureBox.PaintProperties;
            _mySettings._settings.UseResample = prop.PaintDisplayMode == RasterPaintDisplayModeFlags.Resample;
            if (_mySettings._settings.UseResample)
                prop.PaintDisplayMode = RasterPaintDisplayModeFlags.None;
            else
                prop.PaintDisplayMode = RasterPaintDisplayModeFlags.Resample;
            _mySettings._settings.UseResample = !_mySettings._settings.UseResample;
            _mySettings.Save();
            _pictureBox.PaintProperties = prop;
        }

        void _frmOperation_Cancel(object sender, EventArgs e)
        {
            bCancelOperation = true;
        }

        void _pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            int iDelteY = e.Y - iOldY;
            int iDelteX = e.X - iOldX;
            if (e.Button == MouseButtons.Middle && _pictureBox.Image != null)
            {
                if (iDelteY < 0)
                    ZoomPicture(0.03f);
                if (iDelteY > 0)
                    ZoomPicture(-0.03f);
            }
            if (e.Button == MouseButtons.Right && _pictureBox.Image != null)
            {
                _pictureBox.ScrollPosition = new Point(_pictureBox.ScrollPosition.X - iDelteX, _pictureBox.ScrollPosition.Y - iDelteY);
            }
            iOldY = e.Y;
            iOldX = e.X;
        }

        private void _lstBoxPages_ListStateChanged(object sender, EventArgs e)
        {
            UpdateToolBarState();
        }

        private void _cmListBox_Opening(object sender, CancelEventArgs e)
        {
            _cmiExpanded.Checked = _lstBoxPages.ViewMode == ThumbMode.Expanded;
            _cmiCondensed.Checked = _lstBoxPages.ViewMode == ThumbMode.Condensed;

            _cmiDeleteAll.Enabled = _lstBoxPages.Items.Count > 0;
            _cmiDeleteSelected.Enabled = _lstBoxPages.SelectedItems.Count > 0;

        }

        private void _cmiExpanded_Click(object sender, EventArgs e)
        {
            _lstBoxPages.ViewMode = ThumbMode.Expanded;
        }

        private void _cmiCondensed_Click(object sender, EventArgs e)
        {
            _lstBoxPages.ViewMode = ThumbMode.Condensed;
        }

        private void _miOpen_Click(object sender, EventArgs e)
        {
            RasterOpenDialog dlgOpen = new RasterOpenDialog(_codec);
            dlgOpen.ShowPreview = true;
            dlgOpen.PreviewWindowVisible = true;
            dlgOpen.FilterIndex = 1;
            dlgOpen.ShowFileInformation = true;
            if (strLastLocation != "")
                dlgOpen.InitialDirectory = Path.GetDirectoryName(strLastLocation);

            bool bTopMost = logWindow.TopMost;
            logWindow.TopMost = false;
            DialogResult dlgRes = dlgOpen.ShowDialog(this);
            if (dlgRes == DialogResult.Cancel)
            {
                dlgOpen.Dispose();
                logWindow.TopMost = bTopMost;
                return;
            }
            LoadRasterImage(dlgOpen.FileName);

            if (dlgOpen != null)
                dlgOpen.Dispose();
        }

        private void _miPaste_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap img = (Bitmap)Clipboard.GetImage();
                RasterImage rImg = Leadtools.Drawing.RasterImageConverter.ChangeFromHBitmap(img.GetHbitmap(), IntPtr.Zero);
                CreateImageCollection("Pasted Image", rImg);
            }
            catch { }
        }

        private void CopySpecificCharacterSetElement(DicomDataSet imageDataSet, DicomDataSet infoDataSet)
        {
            DicomElement element = infoDataSet.FindFirstElement(null, DicomTag.SpecificCharacterSet, true);
            if (element != null)
            {
                byte[] ba = infoDataSet.GetBinaryValue(element, (int)element.Length);
                imageDataSet.InsertElementAndSetValue(DicomTag.SpecificCharacterSet, ba);
            }
        }

        private void _toolBtnTwain_Click(object sender, EventArgs e)
        {
            _miTwainAcquire_Click(null, null);
        }

        private bool BeforeAddTagDelegate(LinkedList<long> parent, object data, long tag)
        {
            DicomTag dcmTag = DicomTagTable.Instance.Find(tag);

            if (dcmTag != null)
            {
                Console.WriteLine("Tag: " + dcmTag.Name);
            }
            else
                Console.WriteLine(string.Format("Tag: {0:x4}:{1:x4}", DicomExtensions.GetGroup(tag), DicomExtensions.GetElement(tag)));

            return false;
        }

        private void _miRotate90_Click(object sender, EventArgs e)
        {
            if (_pictureBox.Image == null)
                return;

            try
            {
                _pictureBox.Image.RotateViewPerspective(90);
                _lstBoxPages.SelectedItem.RasterImage.RotateViewPerspective(90);
                string strFileLoc = (_lstBoxPages.SelectedItem.ImageItem.Tag as IPrintToPACSFile).FileLocation();

                if (_lstBoxPages.SelectedItem.ImageItem.Tag.GetType() == typeof(PrintPage))
                    _codec.Save(_lstBoxPages.SelectedItem.RasterImage, strFileLoc, RasterImageFormat.Emf, 0);
                else
                    _codec.Save(_lstBoxPages.SelectedItem.RasterImage, strFileLoc, RasterImageFormat.Tif, 0);
            }
            catch { }
        }

        private void _toolBtnRotate_Click(object sender, EventArgs e)
        {
            _miRotate90_Click(null, null);
        }

        private void _lstBoxPages_ItemDeSlect(object sender, EventArgs e)
        {
            if (_pictureBox.Image != null)
            {
                _pictureBox.Image.Dispose();
                _pictureBox.Image = null;
            }
            //_btnNext.Enabled = false;
            //_btnPrev.Enabled = false;
            //_lblPageInfo.Text = "";
            UpdateToolBarState();
        }

        private void _miShowHelp_Click(object sender, System.EventArgs e)
        {
            HelpDialog dlg = new HelpDialog(null, false, false);
            bool bTopMost = logWindow.TopMost;
            logWindow.TopMost = false;
            dlg.ShowDialog(this);
            logWindow.TopMost = bTopMost;

        }

        private void _pictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) != 0)
            {
                if (e.Delta > 0)
                    _miZoomIn_Click(null, null);
                else
                    _miZoomOut_Click(null, null);
            }
            else
            {
                int iSelectedPage = 0;
                if (_lstBoxPages.ViewMode == ThumbMode.Condensed)
                    iSelectedPage = _lstBoxPages.SelectedItemGroupIndex;
                else
                    iSelectedPage = _lstBoxPages.SelectedIndex;

                if (e.Delta > 0)
                {

                    if (iSelectedPage < _lstBoxPages.GetGroupImageItems().Count - 1)
                    {
                        _btnNext_Click(null, null);
                    }
                }
                else
                {
                    if (iSelectedPage > 0)
                    {
                        _btnPrev_Click(null, null);
                    }
                }
            }
        }

        private void FrmMain_Resize(object sender, EventArgs e)
        {
            try
            {
                _pictureBox.Invalidate();
                _lstBoxPages.Invalidate();
            }
            catch { }
        }

        private void _miClearPrintedList_Click(object sender, EventArgs e)
        {
            try
            {
                ClearList();
                EnableNextPrevious();
                UpdateToolBarState();
                //_lblPageInfo.Text = "";
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void _miExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void _lstBoxPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScalePicture(_lstBoxPages.SelectedItem.ImageItem);
            EnableNextPrevious();
            UpdateToolBarState();
            _iOldIndex = _lstBoxPages.SelectedIndex;
            _mySettings._settings.LastSelectedIndex = _lstBoxPages.SelectedIndex;
            _mySettings.Save();
        }

        private void _miFile_DropDownOpening(object sender, EventArgs e)
        {
            try
            {
                _miSaveAsDICOM.Enabled = (_lstBoxPages.CheckedItems.Count > 0);
                _miStoreToPACS.Enabled = (_lstBoxPages.CheckedItems.Count > 0);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void _lstBoxPages_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete && _lstBoxPages.SelectedItem != null)
                    _miDeleteSelected_Click(null, null);

                if (e.KeyCode == Keys.V && Control.ModifierKeys == Keys.Control)
                    _miPaste_Click(null, null);

                else if (e.KeyCode == Keys.Add)
                {
                    _miZoomIn_Click(_miZoomIn, new EventArgs());
                }
                else if (e.KeyCode == Keys.Subtract)
                {
                    _miZoomOut_Click(_miZoomOut, new EventArgs());
                }
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private async void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FinilizeScreenCapture();
                FinilizeTwain();
            }
            catch (Exception)
            {
                //MessageBox.Show("Closing Form " + Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private async void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                await _CameraControl.VideoCapture1.StopAsync();
            }
            catch (Exception)
            {
                //MessageBox.Show("Closing Form " + Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

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

        private void _btnNext_Click(object sender, EventArgs e)
        {
            if (_lstBoxPages.ViewMode == ThumbMode.Expanded)
            {
                if (_lstBoxPages.SelectedIndex < _lstBoxPages.Items.Count - 1)
                    _lstBoxPages.SelectedIndex = _lstBoxPages.SelectedIndex + 1;
            }
            else
            {
                if (_lstBoxPages.SelectedItemGroupIndex < _lstBoxPages.GetGroupImageItems().Count - 1)
                    _lstBoxPages.SelectedIndex = _lstBoxPages.SelectedIndex + 1;
            }
        }

        private void _btnPrev_Click(object sender, EventArgs e)
        {
            if (_lstBoxPages.ViewMode == ThumbMode.Expanded)
            {
                if (_lstBoxPages.SelectedIndex > 0)
                    _lstBoxPages.SelectedIndex = _lstBoxPages.SelectedIndex - 1;
            }
            else
            {
                if (_lstBoxPages.SelectedItemGroupIndex > 0)
                    _lstBoxPages.SelectedIndex = _lstBoxPages.SelectedIndex - 1;
            }
        }

        private void _miSCPOptions_Click(object sender, EventArgs e)
        {
            if (PacsSettings.DoOptions(0) != DialogResult.Cancel)
                SetServersComboBox(false);
            UpdateComboBoxes();
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

        private void _miStoreToPACS_Click(object sender, EventArgs e)
        {
            if (_mySettings._settings.StoreServers.serverList.Length == 0)
                return;

            MyServer server = _mySettings._settings.StoreServers.serverList[toolStripComboBoxStoreServer.SelectedIndex];
            string strTemp, strMessage = string.Empty;
            strTemp = Path.GetTempFileName();

            DicomDataSet dicom = (_pgDicomInfo.SelectedObject as DicomEditableObject).DataSet;
            List<string> lstSaved = new List<string>();
            bool bSuccess = false;

            if (!CheckRequiredTags(dicom))
                return;

            EnableItems(false, "Đang lưu các tập tin tạm thời vào ổ cứng \nVui lòng đợi...", "Cancel");
            bool bTopMost = logWindow.TopMost;
            logWindow.TopMost = false;
            strMessage = DoSave(dicom, ref lstSaved, strTemp, ref bSuccess);
            EnableItems(true, "", "");

            if (bSuccess && !bCancelOperation)
            {
                EnableItems(false, "Đang lưu trữ vào PACS vui lòng đợi...", "Cancel");
                try
                {
                    strMessage = "\nĐang lưu trữ vào PACS:\n";
                    foreach (string strFile in lstSaved)
                    {
                        try
                        {
                            DoStore(strFile, server);
                        }
                        catch (Exception ex)
                        {
                            logWindow.TopMost = false;
                            EnableItems(true, "", "");
                            MessageBox.Show("Đã xảy ra lỗi: \n" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        Application.DoEvents();
                        if (bCancelOperation)
                            break;
                    }
                    bSuccess = true;
                    strMessage += "\nTệp được lưu trữ thành công";
                }
                catch (System.Exception ex)
                {
                    bSuccess = false;
                    strMessage += "Đã xảy ra lỗi:\n" + ex.Message;
                }
            }
            File.Delete(strTemp);

            EnableItems(true, "", "");

            if (bSuccess && bStored && !bCancelOperation)
            {
                if (_mySettings._settings.autodelete)
                    DeleteCheckedItems();
                logWindow.TopMost = false;
                DialogResult dlgClear = MessageBox.Show(this, strMessage + "\nBạn có muốn xóa thông tin DICOM?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlgClear == DialogResult.Yes)
                {
                    _miClearPG_Click(null, null);
                }
            }
            else
            {
                logWindow.TopMost = false;
                if (bCancelOperation)
                    MessageBox.Show(this, "Đã hủy thao tác", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (bSuccess)
                        MessageBox.Show(this, "Tệp không được lưu trữ thành công", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show(this, strMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            logWindow.TopMost = bTopMost;
        }

        private void _miClearPG_Click(object sender, EventArgs e)
        {
            DicomDataSet ds = _pgDicomInfo.DataSet;
            ds.Dispose();
            _pgDicomInfo.DataSet = null;
            _cmbSopClasses_SelectedIndexChanged(null, null);
        }

        private void _miClearSearch_Click(object sender, EventArgs e)
        {
            //if (_tbDicomInfo.SelectedTab == _pageMWLQuery)
            //{
            //    if (!_toolBtnPatient.Checked)
            //    {
            //        _bbQuery = new BroadBasedQuery();
            //        _pgSearchMWL.SelectedObject = _bbQuery;
            //    }
            //    else
            //    {
            //        _pbQuery = new PatientBasedQuery();
            //        _pgSearchMWL.SelectedObject = _pbQuery;
            //    }
            //}

            //if (_tbDicomInfo.SelectedTab == _pageSCPQuery)
            //{
            //    _findQuery = new DicomFindQuery();
            //    _pgSearchSCP.SelectedObject = _findQuery;
            //}
        }

        private void _lstBoxPages_ItemAdded(object sender, System.EventArgs e)
        {
            try
            {
                if (_lstBoxPages.ViewMode == ThumbMode.Expanded)
                {
                    UpdateLabel(_lstBoxPages.SelectedIndex + 1);
                }
                else
                { UpdateLabel(_lstBoxPages.SelectedItemGroupIndex + 1); }
            }
            catch { UpdateLabel(1); }
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

                    //ResetModule(DicomModuleType.GeneralSeries, _pgDicomInfo.DataSet);
                    GenerateDefaultElements();
                }
                else
                {
                    GenerateDefaultElements();
                    InsertNewStudyModule();
                }

            }
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
                ds.InsertElement(null, false, dElement.Tag, dElement.VR, false, 0);
            if (ds.InformationClass == DicomClassType.EncapsulatedPdfStorage)
                ds.SetValue(dElement, "DOC");
            else
                ds.SetValue(dElement, "OT");
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
            DeleteSelectedItems();

            if (_pictureBox.Image != null)
            {
                _pictureBox.Image.Dispose();
                _pictureBox.Image = null;
            }

            EnableNextPrevious();
            UpdateToolBarState();
            //_lblPageInfo.Text = "";
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

        private void _toolBtnHelp_Click(object sender, EventArgs e)
        {
            _miShowHelp_Click(null, null);
        }

        private void _toolBtnOpenRaster_Click(object sender, EventArgs e)
        {
            _miOpen_Click(null, null);
        }

        private void _toolBtnSettings_Click(object sender, EventArgs e)
        {
            if (PacsSettings.DoOptions(0) != DialogResult.Cancel)
                SetServersComboBox(false);
            UpdateComboBoxes();
        }

        private void _btnPACSSettings_Click(object sender, EventArgs e)
        {
            if (PacsSettings.DoOptions(1) != DialogResult.Cancel)
                SetServersComboBox(false);
            UpdateComboBoxes();
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

        private void _btnPushToPACS_Click(object sender, EventArgs e)
        {
            _toolBtnStoreToPacs_Click(null, null);
        }

        private void _btnOpenImage_Click(object sender, EventArgs e)
        {
            //_toolBtnOpenRaster_Click(null, null);
            IWorklistDataAccessAgent dataAccessAgent = GetDataAccessAgent();
        }

        private static IWorklistDataAccessAgent GetDataAccessAgent()
        {
            //IL_0016: Unknown result type (might be due to invalid IL or missing references)
            //IL_0020: Expected O, but got Unknown
            IWorklistDataAccessAgent val;
            if (DataAccessServices.IsDataAccessServiceRegistered<IWorklistDataAccessAgent>())
            {
                val = DataAccessFactory.GetInstance((DataAccessConfigurationView)new WorklistDataAccessConfigurationView(DicomDemoSettingsManager.GetGlobalPacsConfiguration(), PacsProduct.ProductName, PacsProduct.ServiceName)).CreateDataAccessAgent<IWorklistDataAccessAgent>();
                DataAccessServices.RegisterDataAccessService<IWorklistDataAccessAgent>(val);
            }
            else
            {
                val = DataAccessServices.GetDataAccessService<IWorklistDataAccessAgent>();
            }

            return val;
        }

        private void _btnScreenCapture_Click(object sender, EventArgs e)
        {
            _toolBtnScreenCapture_Click(null, null);
        }

        private void _miHowToUse_Click(object sender, EventArgs e)
        {
            FrmUsage usage = new FrmUsage();
            usage.ShowDialog(this);
        }

        //private void _btnBrowseDataSet_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog dlgOpen = new OpenFileDialog();
        //    DialogResult dlgRes;
        //    dlgOpen.Filter = "Dicom Files|*.dcm|Dicom DataSet Files|*.dic|Dicom XML DataSet Files|*.xml";
        //    dlgOpen.Multiselect = false;
        //    bool bTopMost = logWindow.TopMost;
        //    logWindow.TopMost = false;
        //    dlgRes = dlgOpen.ShowDialog();
        //    if (dlgRes == DialogResult.Cancel)
        //    {
        //        logWindow.TopMost = bTopMost;
        //        return;
        //    }

        //    _txtDataSet.Text = dlgOpen.FileName;
        //    logWindow.TopMost = bTopMost;

        //    LoadDataSet(_txtDataSet.Text);
        //}
        #endregion

        #region Methods...

        private void InitializeForm()
        {
            _frmProgress = new FrmProgress();

            _pgDicomInfo = new Leadtools.Dicom.Common.Editing.Controls.DicomPropertyGrid();
            DicomEditableObject = new Leadtools.Dicom.Common.Editing.DicomEditableObject();
            _pictureBox = new Leadtools.WinForms.RasterImageViewer();
            /*TEMP*/
            //this._tbTableLayout.Controls.Add(this._pictureBox, 0, 3);
            // 
            // _pictureBox
            // 
            _pictureBox.BackColor = System.Drawing.SystemColors.ButtonFace;
            _pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _pictureBox.EnableScrollingInterface = true;
            _pictureBox.Location = new System.Drawing.Point(3, 43);
            _pictureBox.Name = "_pictureBox";
            _pictureBox.Size = new System.Drawing.Size(394, 394);
            _pictureBox.TabIndex = 5;
            // 
            // _pgDicomInfo
            // 
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            _pgDicomInfo.ContextMenuStrip = _cnmnuClearDicom;
            _pgDicomInfo.DataSet = null;
            _pgDicomInfo.DefaultTag = ((long)(-1));
            _pgDicomInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            _pgDicomInfo.Location = new System.Drawing.Point(3, 33);
            _pgDicomInfo.Name = "_pgDicomInfo";
            _pgDicomInfo.SelectedObject = DicomEditableObject;
            _pgDicomInfo.ShowCommands = true;
            _pgDicomInfo.ShowTagInfo = true;
            _pgDicomInfo.ShowUsageImages = true;
            _pgDicomInfo.TabIndex = 0;
            _pgDicomInfo.ToolbarVisible = false;
            _pgDicomInfo.BeforeAddElement += new EventHandler<BeforeAddElementEventArgs>(_pgDicomInfo_BeforeAddElement);
            _tbPropertyGrid.Controls.Add(_pgDicomInfo, 0, 1);
            _panelImageList.Controls.Add(_lstBoxPages);
            _lstBoxPages.ViewMode = ThumbMode.Condensed;
            _lstBoxPages.ContextMenuStrip = _cmListBox;
            _lstBoxPages.ListStateChanged += new EventHandler(_lstBoxPages_ListStateChanged);
            _pictureBox.MouseWheel += new MouseEventHandler(_pictureBox_MouseWheel);
            _pictureBox.BorderPadding.Bottom = 10;
            _pictureBox.BorderPadding.Top = 10;
            _pictureBox.BorderPadding.Left = 10;
            _pictureBox.BorderPadding.Right = 10;
            _pictureBox.HorizontalAlignMode = RasterPaintAlignMode.Center;
            _pictureBox.VerticalAlignMode = RasterPaintAlignMode.Center;
            _pictureBox.BackColor = Color.Black;
            _pictureBox.EnableScrollingInterface = true;
            _pictureBox.KeyDown += new KeyEventHandler(_pictureBox_KeyDown);
            _lstBoxPages.ItemDeSlect += new EventHandler(_lstBoxPages_ItemDeSlect);
            _pictureBox.InteractiveMode = Leadtools.WinForms.RasterViewerInteractiveMode.ZoomTo;
            _pictureBox.MouseMove += new MouseEventHandler(_pictureBox_MouseMove);
            //_pgSearchSCP.SelectedObject = _findQuery;
            //_tbPicture.Controls.Add(_pictureBox, 0, 2);
            //_tbPicture.SetColumnSpan(_pictureBox, 4);
            _pgDicomInfo.ShowTagInfo = false;
            _pgDicomInfo.ShowCommands = false;
            _pgDicomInfo.CommandsVisibleIfAvailable = false;
            _pgDicomInfo.HelpVisible = false;
            _pictureBox.DoubleClick += new EventHandler(_pictureBox_DoubleClick);

            RasterPaintProperties prop = _pictureBox.PaintProperties;
            if (!_mySettings._settings.UseResample)
                prop.PaintDisplayMode = RasterPaintDisplayModeFlags.None;
            else
                prop.PaintDisplayMode = RasterPaintDisplayModeFlags.Resample;
            _pictureBox.PaintProperties = prop;
            Text = "Chương trình chuyển đổi video DICOM";
            _codec = new RasterCodecs();
            _cmbSopClasses.SelectedIndex = ClassTypes.IndexOf(_mySettings._settings.selectedtype);

            logWindow = new LogWindow(this);
            logWindow.Visible = false;

            //_pageMWLQuery.Controls.Add(_tbQueryMWList);
            //_pgSearchMWL.SelectedObject = _bbQuery;
        }

        private void ZoomPicture(double dZoomFactor)
        {
            try
            {
                double oldScaleFactor = _pictureBox.ScaleFactor;
                if (_pictureBox.SizeMode == RasterPaintSizeMode.FitAlways)
                {
                    double dWidthFraction = (double)(_pictureBox.Width - 30) / (double)_pictureBox.Image.Width;
                    double dHeightFraction = (double)(_pictureBox.Height - 30) / (double)_pictureBox.Image.Height;
                    double dScale = dWidthFraction;
                    if (dHeightFraction < dWidthFraction)
                    {
                        dScale = dHeightFraction;
                    }
                    _pictureBox.SizeMode = RasterPaintSizeMode.Normal;
                    oldScaleFactor = dScale;
                }


                oldScaleFactor = oldScaleFactor + dZoomFactor;
                if (oldScaleFactor > 3 && dZoomFactor > 0)
                    return;
                if (oldScaleFactor < .06 && dZoomFactor < 0)
                    return;
                _pictureBox.SizeMode = RasterPaintSizeMode.Normal;
                _pictureBox.ScaleFactor = oldScaleFactor;
            }
            catch { }
        }

        private void CreateImageCollection(string strTittle, RasterImage rasterImage)
        {
            ListImageBox.ImageCollection imagecollection = new ListImageBox.ImageCollection(strTittle);
            Page page = new Page();
            string strTemp = null;
            strTemp = Path.GetTempFileName();
            _codec.Save(rasterImage, strTemp, RasterImageFormat.Tif, 0);
            page.FilePath = strTemp;
            page.DeleteOnDispose = true;
            imagecollection.Images.Add(new ListImageBox.ImageItem(_codec.Load(strTemp), imagecollection, page));
            rasterImage.Dispose();

            _lstBoxPages.AddImageCollection(imagecollection);
        }

        private void DeleteCheckedItems()
        {
            for (int i = _lstBoxPages.Items.Count - 1; i >= 0; i--)
            {
                ListImageBox.ListItem item = _lstBoxPages.Items[i];
                if (item.ImageItem.Checked)
                {
                    _lstBoxPages.RemoveItem(i);
                }
            }

            try
            {
                _lstBoxPages_SelectedIndexChanged(null, null);
            }
            catch
            {

                if (_pictureBox.Image != null)
                {
                    _pictureBox.Image.Dispose();
                    _pictureBox.Image = null;
                }

                EnableNextPrevious();
                //_lblPageInfo.Text = "";
            }
        }

        private void DeleteSelectedItems()
        {
            for (int i = _lstBoxPages.Items.Count - 1; i >= 0; i--)
            {
                ListImageBox.ListItem item = _lstBoxPages.Items[i];
                if (item.Selected)
                {
                    _lstBoxPages.RemoveItem(i);
                }
            }
        }

        public void UpdateToolBarState()
        {
            _toolBtnDeleteAll.Enabled = _lstBoxPages.Items.Count > 0;
            _toolBtnRotate.Enabled = _toolBtnDeleteSelected.Enabled = _lstBoxPages.SelectedItems.Count > 0;
            _toolBtnSaveDicom.Enabled = _lstBoxPages.CheckedItems.Count > 0;
            _btnPushToPACS.Enabled = _toolBtnStoreToPacs.Enabled = _lstBoxPages.CheckedItems.Count > 0 && _mySettings._settings.StoreServers.serverList.Length > 0;
            _btnCreateConclusion.Enabled = _toolBtnStoreToPacs.Enabled = _lstBoxPages.CheckedItems.Count > 0 && _mySettings._settings.StoreServers.serverList.Length > 0;
            _toolBtnViewLog.Checked = logWindow.Visible;
        }

        public void CloseFrmMain()
        {
            _CameraControl.VideoCapture1.StopAsync();
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
            catch { }

            if (ds == null || ds.InformationClass != dClass)
                ds.Initialize(dClass, DicomDataSetInitializeFlags.AddMandatoryElementsOnly |
                   DicomDataSetInitializeFlags.AddMandatoryModulesOnly);

            ClearTag(ds, DicomTag.PixelData);
            ClearTag(ds, DicomTag.EncapsulatedDocument);

            DicomElement dElement = ds.FindFirstElement(null, DicomTag.Modality, true);
            if (dElement == null)
                ds.InsertElement(null, false, dElement.Tag, dElement.VR, false, 0);
            if (ds.InformationClass == DicomClassType.EncapsulatedPdfStorage)
                ds.SetValue(dElement, "DOC");
            else
                ds.SetValue(dElement, "OT");

            _pgDicomInfo.DataSet = ds;
        }

        private static void ClearListView(ListView lv)
        {
            foreach (ListViewItem item in lv.Items)
            {
                if (item.Tag != null)
                    (item.Tag as DicomDataSet).Dispose();
            }
            lv.Items.Clear();
        }

        private void Serialize(String filelocation)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            FileStream fStream = File.Create(filelocation);
            foreach (ListImageBox.ListItem item in _lstBoxPages.Items)
            {
                item.RasterImage.Dispose();
                item.RasterImage = null;
                item._Controls = null;
                item.ImageItem.Image = null;
                Application.DoEvents();
            }
            binFormatter.Serialize(fStream, _lstBoxPages.ImageCollections);
            fStream.Close();
        }

        private void Deserialize(String filelocation)
        {
            EnableItems(false, "Opening Previous Files Please Wait...", "");
            Thread t = new Thread(delegate ()
            {
                try
                {
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    FileStream fStream = File.Open(filelocation, FileMode.Open);
                    List<ListImageBox.ImageCollection> lstLoaded = (List<ListImageBox.ImageCollection>)binFormatter.Deserialize(fStream);

                    for (int iCol = lstLoaded.Count - 1; iCol >= 0; iCol--)
                    {
                        ListImageBox.ImageCollection imgCol = lstLoaded[iCol];
                        for (int iImg = imgCol.Images.Count - 1; iImg >= 0; iImg--)
                        {
                            ListImageBox.ImageItem item = imgCol.Images[iImg];
                            if (item.Tag.GetType() == typeof(PrintPage))
                            {
                                PrintPage printpage = (item.Tag as PrintPage);

                                if (File.Exists(printpage.FilePath))
                                {
                                    Image img = Image.FromFile(printpage.FilePath);
                                    Metafile mt = img as Metafile;
                                    printpage.MetaFile = mt.GetHenhmetafile();
                                    mt.Dispose();
                                    item.Image = _codec.Load(printpage.FilePath);
                                }
                            }
                            if (item.Tag.GetType() == typeof(Page))
                            {
                                Page page = item.Tag as Page;

                                if (File.Exists(page.FilePath))
                                    item.Image = _codec.Load(page.FilePath);
                            }

                            if (item.Image == null)
                                imgCol.Images.Remove(item);

                            Application.DoEvents();
                            if (bCancelOperation)
                                break;
                        }
                        if (imgCol.Images.Count == 0)
                            lstLoaded.Remove(imgCol);
                    }
                    fStream.Close();

                    foreach (ListImageBox.ImageCollection collection in lstLoaded)
                    {
                        AddImageCollectionThreaded(collection);
                        Application.DoEvents();
                    }
                }
                catch { }
                EnableItems(true, "", "");
            });
            t.Start();
            while (t.IsAlive)
            {
                Application.DoEvents();
            }

        }

        private delegate void AddImageCollectionThreadedDelegate(ListImageBox.ImageCollection collection);
        private void AddImageCollectionThreaded(ListImageBox.ImageCollection collection)
        {
            if (InvokeRequired)
            {
                Invoke(new AddImageCollectionThreadedDelegate(AddImageCollectionThreaded), collection);
            }
            else
            {
                _lstBoxPages.AddImageCollection(collection);
            }
        }

        private void CheckFirstRun()
        {
            if (_mySettings._settings.FirstRun)
            {
                try
                {
                    string strFirstImage = DemosGlobal.ImagesFolder + "\\image1.cmp";
                    LoadRasterImage(strFirstImage);
                    //_txtDataSet.Text = DemosGlobal.ImagesFolder + "\\image2.dcm";
                    // LoadDataSet(_txtDataSet.Text);
                    //_btnTransferLoadedStudies_Click(null, null);
                    _lstBoxPages.SelectedIndex = 0;
                    _lstBoxPages.Items[0].ImageItem.Checked = true;
                    _lstBoxPages.Items[0].CheckState = CheckState.Checked;
                    _btnPushToPACS.Enabled = true;
                    _btnCreateConclusion.Enabled = true;
                    FrmUsage usage = new FrmUsage();
                    usage.ShowDialog(this);
                }
                catch { }
            }
            else
            {
                try
                {
                    _lstBoxPages.SelectedIndex = _mySettings._settings.LastSelectedIndex;
                }
                catch { }
            }
        }

        private void UpdateComboBoxes()
        {
            int iStoreIndex = toolStripComboBoxStoreServer.SelectedIndex;

            if (toolStripComboBoxStoreServer.Items.Count != 0)
            {
                if (toolStripComboBoxStoreServer.Items.Count > iStoreIndex && iStoreIndex >= 0)
                    toolStripComboBoxStoreServer.SelectedIndex = iStoreIndex;
                else if (toolStripComboBoxStoreServer.Items.Count > _mySettings._settings.DefaultStoreServer)
                    toolStripComboBoxStoreServer.SelectedIndex = _mySettings._settings.DefaultStoreServer;
                else
                    toolStripComboBoxStoreServer.SelectedIndex = 0;
            }
        }

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

                _mmMain.Enabled = enable;
                _lstBoxPages.Enabled = enable;
                //_tbDicomInfo.Enabled = enable;
                _cmbSopClasses.Enabled = enable;
                //_cbStoreServers.Enabled = enable;
                toolStripComboBoxStoreServer.Enabled = enable;
                _btnPushToPACS.Enabled = enable;
                _btnCreateConclusion.Enabled = enable;
                _toolbarMain.Enabled = enable;
                _pgDicomInfo.Enabled = enable;
                //_btnPACSSettings.Enabled = enable;
                //_btnOpenImage.Enabled = enable;
                if (enable)
                {
                    UpdateToolBarState();
                    if (_frmOperation != null)
                        _frmOperation.Close();
                }
                else
                {
                    if (!(strCaption == "" && strBtnCaption == ""))
                        if (_frmOperation == null || !_frmOperation.Visible)
                        {
                            _frmOperation = new FrmOperation(strCaption, strBtnCaption);
                            bCancelOperation = false;
                            if (strBtnCaption != "")
                                _frmOperation.Cancel += new EventHandler(_frmOperation_Cancel);
                            _frmOperation.Show();
                        }
                }
            }
        }

        private Rectangle GetFitRect(Rectangle rect, int width, int height)
        {
            int newWidth = 0;
            int newHeight = 0;

            newHeight = rect.Height;
            newWidth = newHeight * width / height;

            if (newWidth > rect.Width)
            {
                newWidth = rect.Width;
                newHeight = newWidth * height / width;
            }

            return new Rectangle(rect.Left, rect.Top, newWidth, newHeight);
        }

        private bool InstallPACSPrinter(string printername)
        {
            bool bRet = false;
            bool bExsists = PrintingUtilities.IsPrinterExist(printername);

            if (!bExsists)
            {
                bRet = PrintingUtilities.InstallNewPrinter(printername, "");
            }
            return bRet || bExsists;
        }

        private void EnableNextPrevious()
        {
            //_btnNext.Enabled = true;
            //_btnPrev.Enabled = true;

            //if (_lstBoxPages.Items.Count == 0)
            //{
            //    _btnPrev.Enabled = false;
            //    _btnNext.Enabled = false;
            //    return;
            //}

            //if (_lstBoxPages.ViewMode == ThumbMode.Condensed)
            //{
            //    if (_lstBoxPages.SelectedItemGroupIndex < 0)
            //    {
            //        _btnPrev.Enabled = false;
            //        _btnNext.Enabled = false;
            //        return;
            //    }
            //    if (_lstBoxPages.SelectedItemGroupIndex <= 0)
            //        _btnPrev.Enabled = false;

            //    if (_lstBoxPages.SelectedItemGroupIndex == _lstBoxPages.GetSelectedImageCollection().Images.Count - 1)
            //        _btnNext.Enabled = false;
            //}
            //else
            //{
            //    if (_lstBoxPages.SelectedIndex <= 0)
            //        _btnPrev.Enabled = false;

            //    if (_lstBoxPages.SelectedIndex == _lstBoxPages.Items.Count - 1)
            //        _btnNext.Enabled = false;

            //}
        }

        private void ScalePicture(PrintToPACSDemo.UI.ListImageBox.ImageItem item)
        {
            if (_pictureBox.Image != null)
            {
                _pictureBox.Image.Dispose();
                _pictureBox.Image = null;
            }
            _pictureBox.Image = item.Image.Clone();

            //_pictureBox_DoubleClick(null, null);
            _pictureBox.SizeMode = RasterPaintSizeMode.FitAlways;
            _pictureBox.ScaleFactor = 1;

            if (_lstBoxPages.ViewMode == ThumbMode.Condensed)
                UpdateLabel(_lstBoxPages.SelectedItemGroupIndex + 1);
            else
                UpdateLabel(_lstBoxPages.SelectedIndex + 1);
        }

        private void UpdateLabel(int iSelectedindex)
        {
            //try
            //{
            //    _lblPageInfo.Text = "Page " + (iSelectedindex).ToString() + " / " + (_lstBoxPages.GetGroupImageItems().Count).ToString();
            //}
            //catch { _lblPageInfo.Text = ""; }
        }

        private void InitClass()
        {
            if (RasterSupport.IsLocked(RasterSupportType.PrintDriver) && RasterSupport.IsLocked(RasterSupportType.PrintDriverServer))
            {
                throw new Exception("Printer driver capability is required.");
            }
        }

        private void ClearList()
        {
            try
            {
                DeleteTempFiles();
                _lstBoxPages.ClearList();
                if (_pictureBox.Image != null)
                {
                    _pictureBox.Image.Dispose();
                    _pictureBox.Image = null;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void DeleteTempFiles()
        {
            foreach (ListImageBox.ListItem item in _lstBoxPages.Items)
            {
                try
                {
                    item.Dispose();
                }
                catch
                {
                }
            }
        }

        public void LogError(string sLogText)
        {
            LogText("*** ERROR *** ", _sNewlineTab + sLogText, Color.Red);
        }

        public void LogText(string action, string logText)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new AddLog(LogText),
                   new object[] { action, logText });
            }
            else
            {
                AddAction(action);
                PacsSettings.LogWindow.RichTextBox.AppendText(logText);
                PacsSettings.LogWindow.RichTextBox.AppendText("\r\n");
                PacsSettings.LogWindow.RichTextBox.ScrollToCaret();
            }
        }

        public void LogText(string sAction, string sLogText, Color sActionColor)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new AddLogColor(LogText), new object[] { sAction, sLogText, sActionColor });
            }
            else
            {
                //AddAction(sAction, sActionColor);
                PacsSettings.LogWindow.RichTextBox.AppendText(sLogText);
                PacsSettings.LogWindow.RichTextBox.AppendText(_sNewline);
                TextBoxTraceListener.SendMessage(PacsSettings.LogWindow.RichTextBox.Handle, TextBoxTraceListener.WM_VSCROLL, TextBoxTraceListener.SB_BOTTOM, 0);
            }
        }

        private void AddAction(string sAction, Color color)
        {
            System.Drawing.Color oldColor = logWindow.RichTextBox.SelectionColor;

            logWindow.RichTextBox.SelectionLength = 0;
            logWindow.RichTextBox.SelectionStart = logWindow.RichTextBox.Text.Length;
            logWindow.RichTextBox.SelectionColor = color;
            logWindow.RichTextBox.SelectionFont = new Font(logWindow.RichTextBox.SelectionFont, FontStyle.Bold);
            logWindow.RichTextBox.AppendText(sAction + ": ");
            logWindow.RichTextBox.SelectionColor = oldColor;
        }

        private void AddAction(string action)
        {
            //System.Drawing.Color oldColor = logWindow.RichTextBox.SelectionColor;
            //if (action == "")
            //{
            //    return;
            //}
            //logWindow.RichTextBox.SelectionLength = 0;
            //logWindow.RichTextBox.SelectionStart = logWindow.RichTextBox.Text.Length;
            //logWindow.RichTextBox.SelectionColor = Color.Blue;
            //logWindow.RichTextBox.SelectionFont = new Font(logWindow.RichTextBox.SelectionFont, FontStyle.Bold);
            //logWindow.RichTextBox.AppendText(action + ": ");

            //logWindow.RichTextBox.SelectionColor = oldColor;
        }

        public delegate void StartUpdateDelegate(DataGridView lv);
        private void StartUpdate(DataGridView dg)
        {
            if (InvokeRequired)
            {
                Invoke(new StartUpdateDelegate(StartUpdate), dg);
            }
            else
            {
                dg.Rows.Clear();
            }
        }

        private void SetServersComboBox(bool bSelectDefault)
        {
            toolStripComboBoxStoreServer.Items.Clear();
            MyServer[] list;
            int defaultserver = 0;

            list = _mySettings._settings.StoreServers.serverList;
            defaultserver = _mySettings._settings.DefaultStoreServer;

            if (list.Length == 0)
            {
                //_toolBtnStoreToPacs.Enabled = _miStoreToPACS.Enabled = _grpStoreServers.Enabled = false;
            }
            else
            {
                //_miStoreToPACS.Enabled = _grpStoreServers.Enabled = true;
                UpdateToolBarState();
                foreach (MyServer server in list)
                {
                    toolStripComboBoxStoreServer.Items.Add(server);
                }
                if (bSelectDefault)
                    if (defaultserver < list.Length)
                        toolStripComboBoxStoreServer.SelectedIndex = defaultserver;
                    else
                        toolStripComboBoxStoreServer.SelectedIndex = 0;
            }
        }

        public void LoadRasterImage(string strFileName)
        {
            bool bTopMost = logWindow.TopMost;
            RasterImage rImg = null;
            try
            {
                EnableItems(false, "Opening Image Files Please Wait...", "Cancel");
                string strFile = strFileName;
                strLastLocation = strFile;
                rImg = _codec.Load(strFile);

                GrayscaleCommand command = new GrayscaleCommand(8);
                if (rImg.IsGray && rImg.BitsPerPixel != 8)
                    command.Run(rImg);

                ListImageBox.ImageCollection imagecollection = new ListImageBox.ImageCollection(Path.GetFileName(strFile));
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
                _lstBoxPages.AddImageCollection(imagecollection);
            }
            catch (System.Exception ex)
            {
                if (rImg != null)
                    rImg.Dispose();

                ShowErrorMessage(ex);
            }
            EnableItems(true, "", "");
            logWindow.TopMost = bTopMost;
        }

        #endregion

        #region Dicom Methods

        //private void LoadDataSet(string strFileName)
        //{
        //    bool bTopMost = logWindow.TopMost;
        //    logWindow.TopMost = false;
        //    if (!File.Exists(strFileName))
        //    {
        //        MessageBox.Show("The selected file does not exist", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }
        //    try
        //    {
        //        _btnTransferLoadedPatient.Enabled = _btnTransferLoadedStudies.Enabled = false;

        //        DicomDataSet dicom = new DicomDataSet();

        //        if (Path.GetExtension(strFileName) == ".xml")
        //            DicomExtensions.LoadXml(dicom, strFileName, DicomDataSetLoadXmlFlags.None, null, null);
        //        else
        //            dicom.Load(strFileName, DicomDataSetLoadFlags.None);

        //        ClearTag(dicom, DicomTag.PixelData);
        //        ClearTag(dicom, DicomTag.EncapsulatedDocument);

        //        DicomElement dElement;
        //        ListViewItem item = null;

        //        string val = "";

        //        foreach (long dTag in DICOMPatientInfo)
        //        {
        //            dElement = dicom.FindFirstElement(null, dTag, true);
        //            val = "";
        //            if (dElement != null)
        //                val = dicom.GetValue<string>(dElement, null);

        //            if (item == null)
        //                item = _lstDSPatient.Items.Add(val);
        //            else
        //                item.SubItems.Add(val);
        //        }
        //        item.Tag = dicom;
        //        item.Selected = true;
        //        //Series
        //    }
        //    catch { MessageBox.Show("The selected file is not a valid dicom file", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        //    logWindow.TopMost = bTopMost;
        //}

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
                dicom.InsertElementAndSetValue(DicomTag.ImplementationVersionName, "LEADPRINTTOPACS");

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

        private void InsertPatientInfo(DicomDataSet ds, Patient patient)
        {
            DicomElement dElement;
            if (patient.Name != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.PatientName, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.PatientName, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, patient.Name.FullDicomEncoded);
            }

            if (patient.Id != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.PatientID, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.PatientID, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, patient.Id);
            }

            if (patient.Sex != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.PatientSex, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.PatientSex, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, patient.Sex);
            }

            if (patient.BirthDate != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.PatientBirthDate, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.PatientBirthDate, DicomVRType.UN, false, 0);
                ds.SetDateValue(dElement, new DateTime[] { (DateTime)patient.BirthDate });
            }
        }

        private void InsertStudyInfo(DicomDataSet ds, Study study)
        {
            DicomElement dElement;
            if (study.Id != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.StudyID, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.StudyID, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, study.Id);
            }

            if (study.AccessionNumber != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.AccessionNumber, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.AccessionNumber, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, study.AccessionNumber);
            }

            if (study.ReferringPhysiciansName != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.ReferringPhysicianName, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.ReferringPhysicianName, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, study.ReferringPhysiciansName.FullDicomEncoded);
            }

            if (study.Date != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.StudyDate, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.StudyDate, DicomVRType.UN, false, 0);
                ds.SetDateValue(dElement, new DateTime[] { (DateTime)study.Date });
            }

            if (study.Description != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.StudyDescription, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.StudyDescription, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, study.Description);
            }

            if (study.Time != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.StudyTime, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.StudyTime, DicomVRType.UN, false, 0);
                ds.SetTimeValue(dElement, new DateTime[] { (DateTime)study.Time });
            }

            if (study.InstanceUID != null)
            {
                dElement = ds.FindFirstElement(null, DicomTag.StudyInstanceUID, true);
                if (dElement == null)
                    dElement = ds.InsertElement(null, false, DicomTag.StudyInstanceUID, DicomVRType.UN, false, 0);
                ds.SetValue(dElement, study.InstanceUID);
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
            if (element != null)
                dicom.SetValue(element, Utils.GenerateDicomUniqueIdentifier());

            _pgDicomInfo.DataSet = dicom;
        }

        #endregion

        #region StoreScu

        private void DoStore(string dsFile, MyServer storeserver)
        {
            string sMsg = string.Empty;
            DicomScp server = new DicomScp();
            server.AETitle = storeserver._sAE;
            server.PeerAddress = IPAddress.Parse(storeserver._sIP);
            server.Port = storeserver._port;
            server.Timeout = storeserver._timeout;
            MyServer s = null;
            s = (MyServer)(toolStripComboBoxStoreServer.SelectedItem);
            bStored = false;

            CreateCStoreObject(s);
            _cstore.AETitle = _mySettings._settings.clientAE;
            _cstore.HostPort = _mySettings._settings.clientPort;

            Thread t = new Thread(delegate ()
            {
                try
                {
                    _cstore.Store(server, dsFile);
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                    ShowErrorMessage(ex);
                }
            });

            t.Start();
            while (t.IsAlive)
            {
                Application.DoEvents();
            }
        }

        public delegate void ShowErrorMessageDelegate(Exception ex);
        private void ShowErrorMessage(Exception ex)
        {
            if (InvokeRequired)
            {
                Invoke(new ShowErrorMessageDelegate(ShowErrorMessage), ex);
            }
            else
            {
                EnableItems(true, "", "");
                bool bTopMost = logWindow.TopMost;
                logWindow.TopMost = false;
                MessageBox.Show(this, "Error Occurred: \n" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                logWindow.TopMost = bTopMost;
            }

        }

        void _cstore_BeforeConnect(object sender, Leadtools.Dicom.Scu.Common.BeforeConnectEventArgs e)
        {
            LogText("Before Connect", e.Scp.ToString());
            e.PrivateKeyPassword = _mySettings._settings.privateKeyPassword;
        }

        void _cstore_AfterConnect(object sender, Leadtools.Dicom.Scu.Common.AfterConnectEventArgs e)
        {
            string message;
            if (e.Error == DicomExceptionCode.Success)
            {
                message = _sNewlineTab + "Connection Successful";
            }
            else
            {
                message =
                   _sNewlineTab + "Connection Failed" +
                   _sNewlineTab + "Error:\t" + e.Error.ToString();
            }

            LogText("After Connect", message);
        }

        void _cstore_AfterSecureLinkReady(object sender, Leadtools.Dicom.Scu.Common.AfterSecureLinkReadyEventArgs e)
        {
            string message;
            if (e.Error == DicomExceptionCode.Success)
            {
                message = _sNewlineTab + "Secure Link Ready";
            }
            else
            {
                message =
                   _sNewlineTab + "Secure Link Failed" +
                   _sNewlineTab + "Error:\t" + e.Error.ToString();
            }

            LogText("After Secure Link Ready", message);
        }

        void _cstore_BeforeAssociateRequest(object sender, Leadtools.Dicom.Scu.Common.BeforeAssociateRequestEventArgs e)
        {
            LogText("Before Associate Request", e.Associate.ToString());
        }

        void _cstore_AfterAssociateRequest(object sender, Leadtools.Dicom.Scu.Common.AfterAssociateRequestEventArgs e)
        {
            string message;
            if (e.Rejected)
            {
                message =
                   _sNewlineTab + "Association Rejected" +
                   _sNewlineTab + "Result: " + e.Result.ToString() +
                   _sNewlineTab + "Reason: " + e.Reason.ToString() +
                   _sNewlineTab + "Source: " + e.Source.ToString();
            }
            else
            {
                message = _sNewlineTab + "Association Accepted" + e.Associate.ToString();
            }
            LogText("After Associate Request", message);
        }

        void _cstore_BeforeCStore(object sender, Leadtools.Dicom.Scu.Common.BeforeCStoreEventArgs e)
        {
            LogText("Before CStore", _sNewlineTab + "Current DataSet");
        }

        void _cstore_AfterCStore(object sender, Leadtools.Dicom.Scu.Common.AfterCStoreEventArgs e)
        {
            string message;
            if (e.Status == DicomCommandStatusType.Success)
            {
                message =
                   _sNewlineTab + "Success" +
                   _sNewlineTab + "Current DataSet";
                bStored = true;
            }
            else
            {
                message =
                   _sNewlineTab + "CStore Failed" +
                   _sNewlineTab + "Status: " + e.Status.ToString();
            }
            LogText("After CStore", message);
        }

        private CameraControl cameraControl;
        private MediaPlayerControl mediaPlayerControl;
        private Form form;
        private void cardCameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadCameraControl();
        }

        private void LoadCameraControl()
        {
            //cameraControl = new CameraControl();
            //cameraControl.Dock = DockStyle.Fill;
            //cameraControl.VideoRoll_Click += new EventHandler(VideoRoll_Click);
            //cameraControl.CloseCamera_Click += new EventHandler(CloseCamera_Click);
            //cameraControl.Snapshot_Click += new EventHandler(Snapshot_Click);

            //if (Screen.AllScreens.Length > 1)
            //{
            //    Form form = new Form();
            //    form.Controls.Add(cameraControl);
            //    cameraControl.CloseButtonVisible();
            //    form.Text = "Form on Second Screen";
            //    form.StartPosition = FormStartPosition.Manual;
            //    form.WindowState = FormWindowState.Maximized;
            //    form.Location = Screen.AllScreens[1].WorkingArea.Location;

            //    form.FormClosed += CloseCamera_Click;
            //    form.ShowDialog();
            //}
            //else
            //{
            //    tableLayoutPanelInfo.Visible = false;
            //    panelInfo.Controls.Add(cameraControl);
            //}
        }

        //private void VideoRoll_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        PictureBox pictureBox = sender as PictureBox;
        //        cameraControl.Visible = false;
        //        cameraControl.VideoCapture1.StopAsync();

        //        mediaPlayerControl = new MediaPlayerControl(pictureBox.Tag.ToString());
        //        mediaPlayerControl.Dock = DockStyle.Fill;

        //        mediaPlayerControl.SnapshotMedia_Click += new EventHandler(SnapshotMedia_Click);
        //        mediaPlayerControl.BackCamera_Click += new EventHandler(BackCamera_Click);

        //        if (Screen.AllScreens.Length > 1)
        //        {
        //            form.Controls.Add(mediaPlayerControl);
        //        }
        //        else
        //        {
        //            panelInfo.Controls.Add(mediaPlayerControl);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void BackCamera_Click(object sender, EventArgs e)
        //{
        //    cameraControl.VideoCapture1.StartAsync();
        //    cameraControl.Visible = true;
        //    if (Screen.AllScreens.Length > 1)
        //    {
        //        form.Controls.Remove(mediaPlayerControl);
        //    }
        //    else
        //    {
        //        panelInfo.Controls.Remove(mediaPlayerControl);
        //    }
        //}

        //private void SnapshotMedia_Click(object sender, EventArgs e)
        //{
        //    LoadRasterImage(mediaPlayerControl.SnapshotMedia);
        //}

        //private void Snapshot_Click(object sender, EventArgs e)
        //{
        //    LoadRasterImage(cameraControl.LinkImageSnapshot);
        //}

        //private void CloseCamera_Click(object sender, EventArgs e)
        //{
        //    tableLayoutPanelInfo.Visible = true;
        //    cameraControl.CameraControlRemoved();
        //    panelInfo.Controls.Remove(cameraControl);
        //}

        private void discontinueMPPSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ModalityWorklistResult result = _lstMWLItems.SelectedItems[0].Tag as ModalityWorklistResult;
            //MPPSNCreate mpps = MPPSNCreate.FromWorklistItem(result);

            ////mpps.PerformedProcedureStepEndDate = DateTime.Now;
            ////mpps.PerformedProcedureStepEndTime = DateTime.Now;
            ////mpps.PerformedProcedureStepStatus = "DISCONTINUED";
            ////pps.Set<ModalityPerformedProcedureStep>(GetQueryServer(), mpps);

            //mpps.PerformedProcedureStepEndDate = DateTime.Now;
            //mpps.PerformedProcedureStepEndTime = DateTime.Now;
            //mpps.PerformedProcedureStepStatus = "DISCONTINUED";


            //try
            //{
            //    pps.Set<ModalityPerformedProcedureStep>(GetQueryServer(),false, mpps, new BeforeAddTagDelegate(BeforeAddTagDelegate));
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }

        static Bitmap ChangeBitDepth(Image originalImage)
        {
            // Tạo một Bitmap mới với độ sâu bit mong muốn
            PixelFormat newPixelFormat = PixelFormat.Format32bppArgb;

            Bitmap newImage = new Bitmap(originalImage.Width, originalImage.Height, newPixelFormat);

            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.DrawImage(originalImage, new Rectangle(0, 0, newImage.Width, newImage.Height));
            }
            return newImage;
        }

        private bool isCreateConclusion = false;

        private void _btnCreateConclusion_Click(object sender, EventArgs e)
        {
            string commonFolder = DicomDemoSettingsManager.GetFolderPath();
            string folderPath = Path.Combine(commonFolder, "BenhNhan");

            mppsCreate.PerformedProcedureStepEndDate = DateTime.Now;
            mppsCreate.PerformedProcedureStepEndTime = DateTime.Now;
            mppsCreate.PerformedProcedureStepStatus = "COMPLETED";
            PacsSettings.PPS.Set<ModalityPerformedProcedureStep>(PacsSettings.SCP, mppsCreate, new BeforeAddTagDelegate(BeforeAddTagDelegate));

            string saveStorePath = $"D:\\DICOM Store\\L23_WS_SERVER64\\Images\\{result.PatientId.Trim()}\\Image Conclusion";
            if (!Directory.Exists(saveStorePath))
            {
                Directory.CreateDirectory(saveStorePath);
            }

            List<string> conclusionImages = new List<string>();
            foreach (ListImageBox.ListItem item in _lstBoxPages.Items)
            {
                conclusionImages.Add(Path.Combine(folderPath, item.ImageItem.Parent.Name));
            }


            if (!isCreateConclusion)
            {
                //conclusion = new PrintToPACSDemo.AnPhatData.Conclusion()
                //{
                //    PatientID = result.PatientId.Trim(),
                //    PatientName = result.PatientName.Full.ToString().Trim(),
                //    PatientDoB = result.PatientBirthDate.Value,
                //    PatientGender = result.PatientSex,
                //    MedicalImagingCode = result.AccessionNumber,
                //    ImagingServiceCode = "0001",
                //    StudyInstanceUID = result.StudyInstanceUid.Trim(),
                //    MedicalImagingCreateAt = result.ScheduledProcedureStepSequence[0].ScheduledProcedureStepStartDate.Value,
                //    MedicalImagingReportedAt = mppsCreate.PerformedProcedureStepEndTime.Value,
                //    DeviveName = !result.ScheduledProcedureStepSequence[0].ScheduledStationAeTitle.IsNullOrEmpty() ? result.ScheduledProcedureStepSequence[0].ScheduledStationAeTitle : string.Empty,
                //    HealthIdentificationCode = "",
                //    OrderingPhysician = result.RequestingPhysician.Full.Trim(),
                //    Radiologist = !result.ReferringPysician.Full.IsNullOrEmpty() ? result.ReferringPysician.Full : string.Empty,
                //    Technicians = !result.ScheduledProcedureStepSequence[0].ScheduledPerformingPhysician.Full.IsNullOrEmpty() ? result.ScheduledProcedureStepSequence[0].ScheduledPerformingPhysician.Full : string.Empty,
                //};
                ConclusionForm createConclusion = new ConclusionForm( conclusionImages);
                createConclusion.FormClosed += CreateConclusion_FormClosed;
                createConclusion.Name = "Conclusion";
                createConclusion.Show(this);


                isCreateConclusion = true;
            }
        }

        private void CreateConclusion_FormClosed(object sender, FormClosedEventArgs e)
        {
            isCreateConclusion = false;
            _workListTable.DeleteItemSelectedListview();
        }

        public event EventHandler VideoRoll_Click;
        private void PictureBoxRoll_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            if (VideoRoll_Click != null)
            {
                VideoRoll_Click(sender, EventArgs.Empty);
            }
        }

        //
        // Xử lý sự kiện điều khiển của camera
        //
        public CameraControl _CameraControl;
        public bool IsCheckRecord = false;
        public bool IsCheckPause = false;

        private void _btnRecord_Click(object sender, EventArgs e)
        {
            if (IsCheckRecord)
            {
                _btnRecord.Text = "Ghi lại";
                _btnRecord.Image = global::PrintToPACSDemo.Properties.Resources.circle;
                _btnPause.Text = "Tạm dừng";
                _btnPause.Image = global::PrintToPACSDemo.Properties.Resources.pause;
                _btnPause.Enabled = false;
                IsCheckRecord = false;
                IsCheckPause = false;
                _CameraControl.StopCaptureAsync();

                PictureBox pictureBox = new PictureBox();
                pictureBox.Image = global::PrintToPACSDemo.Properties.Resources.videoEx;
                pictureBox.Tag = _CameraControl.LinkVideos[_CameraControl.LinkVideos.Count - 1];
                pictureBox.Size = new System.Drawing.Size(60, 60);
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.Click += PictureBoxRoll_Click;
                _fPLRoll.Controls.Add(pictureBox);
            }
            else
            {
                _btnRecord.Text = "Dừng";
                _btnRecord.Image = global::PrintToPACSDemo.Properties.Resources.StopCamera;
                _btnPause.Enabled = true;
                IsCheckRecord = true;
                _CameraControl.StartRecordAsync();
            }
        }

        private void _btnSnapshot_Click(object sender, EventArgs e)
        {
            _CameraControl.Snapshot();
            LoadRasterImage(_CameraControl.LinkImageSnapshot);
        }

        private void _btnSettings_Click(object sender, EventArgs e)
        {
        }

        private void _btnPause_Click(object sender, EventArgs e)
        {
            _CameraControl.SetPauseResumeCaptureAsync(IsCheckPause);
            if (IsCheckPause)
            {
                _btnPause.Text = "Tạm dừng";
                _btnPause.Image = global::PrintToPACSDemo.Properties.Resources.pause;
                IsCheckPause = false;
            }
            else
            {
                _btnPause.Text = "Tiếp tục";
                _btnPause.Image = global::PrintToPACSDemo.Properties.Resources.ResumeCamera;
                IsCheckPause = true;
            }
        }

        SettingsCamera settingsCamera;
        private void _toolBtnSettingsCamera_Click(object sender, EventArgs e)
        {
            try
            {
                if (settingsCamera == null || settingsCamera.IsDisposed)
                {
                    settingsCamera = new SettingsCamera(_CameraControl);
                    settingsCamera.Show();
                }
                else
                {
                    settingsCamera.BringToFront();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void xtraTabPage1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void labelControl5_Click(object sender, EventArgs e)
        {

        }

        private void labelControl11_Click(object sender, EventArgs e)
        {

        }

        private void textEdit9_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void textEdit7_EditValueChanged(object sender, EventArgs e)
        {

        }

        void CreateCStoreObject(MyServer server)
        {
            if (_cstore != null)
            {
                _cstore.Dispose();
            }

            if (server._useTls)
            {
                _cstore = new StoreScu(string.Empty, DicomNetSecurityeMode.Tls, null);
            }
            else
            {
                _cstore = new StoreScu();
            }

            _cstore.ImplementationClass = _sConfigurationImplementationClass;
            _cstore.ImplementationVersionName = _sConfigurationImplementationVersionName;
            _cstore.ProtocolVersion = _sConfigurationProtocolversion;

            // Subscribe to events for logging
            _cstore.BeforeConnect += new Leadtools.Dicom.Scu.Common.BeforeConnectDelegate(_cstore_BeforeConnect);
            _cstore.AfterConnect += new Leadtools.Dicom.Scu.Common.AfterConnectDelegate(_cstore_AfterConnect);
            _cstore.AfterSecureLinkReady += new Leadtools.Dicom.Scu.Common.AfterSecureLinkReadyDelegate(_cstore_AfterSecureLinkReady);
            _cstore.BeforeAssociateRequest += new Leadtools.Dicom.Scu.Common.BeforeAssociationRequestDelegate(_cstore_BeforeAssociateRequest);
            _cstore.AfterAssociateRequest += new Leadtools.Dicom.Scu.Common.AfterAssociateRequestDelegate(_cstore_AfterAssociateRequest);
            _cstore.BeforeCStore += new Leadtools.Dicom.Scu.Common.BeforeCStoreDelegate(_cstore_BeforeCStore);
            _cstore.AfterCStore += new Leadtools.Dicom.Scu.Common.AfterCStoreDelegate(_cstore_AfterCStore);

            _cstore.PrivateKeyPassword += new PrivateKeyPasswordDelegate(_cstore_PrivateKeyPassword);
            if (server._useTls)
            {
                try
                {
                    _cstore.SetTlsCipherSuiteByIndex(0, DicomTlsCipherSuiteType.DheRsaWith3DesEdeCbcSha);
                    _cstore.SetTlsClientCertificate(
                       _mySettings._settings.clientCertificate,
                       DicomTlsCertificateType.Pem,
                       _mySettings._settings.privateKey.Length > 0 ? _mySettings._settings.privateKey : null);
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }

            }

            if (_mySettings._settings.logLowLevel)
            {
                if (_tracer == null)
                {
                    _tracer = new TextBoxTraceListener(logWindow.RichTextBox);
                    Trace.Listeners.Add(_tracer);
                }
            }
            else
            {
                if (_tracer != null)
                {
                    Trace.Listeners.Remove(_tracer);
                    _tracer = null;
                }
            }

            _cstore.DebugLogFilename = string.Empty;
            _cstore.EnableDebugLog = true;
        }

        void _cstore_PrivateKeyPassword(object sender, PrivateKeyPasswordEventArgs e)
        {
            e.PrivateKeyPassword = _mySettings._settings.privateKeyPassword;
        }

        #endregion
    }

    [Serializable]
    class PrintPage : IDisposable, IPrintToPACSFile
    {
        bool bSelected = false;
        public bool Selected
        {
            get { return bSelected; }
            set { bSelected = value; }
        }

        private int _jobId;
        public int JobId
        {
            get { return _jobId; }
        }

        private string _strRecognizedFilePath = "";
        public string RecognizedFilePath
        {
            get { return _strRecognizedFilePath; }
            set { _strRecognizedFilePath = value; }
        }

        IntPtr _metaFile;
        public IntPtr MetaFile
        {
            get { return _metaFile; }
            set { _metaFile = value; }
        }

        string file = string.Empty;
        public string FilePath
        {
            get { return file; }
            set { file = value; }
        }

        public PrintPage(int jobId)
        {
            _jobId = jobId;
        }

        ~PrintPage()
        {
            //if (File.Exists(tempFile))
            //   File.Delete(tempFile);


            //if (File.Exists(RecognizedFilePath))
            //   File.Delete(RecognizedFilePath);
            //MetaFile.Dispose();
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                if (File.Exists(file))
                    File.Delete(file);

                if (File.Exists(RecognizedFilePath))
                    File.Delete(RecognizedFilePath);
            }
            catch { }
            //MetaFile.Dispose();
        }
        #endregion

        public string FileLocation()
        {
            return file;
        }
    }

    [Serializable]
    class Page : IDisposable, IPrintToPACSFile
    {
        bool bDeleteOnDispose = false;
        public bool DeleteOnDispose
        {
            get { return bDeleteOnDispose; }
            set { bDeleteOnDispose = value; }
        }

        string file = string.Empty;
        public string FilePath
        {
            get { return file; }
            set { file = value; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                if (bDeleteOnDispose)
                    if (System.IO.File.Exists(file))
                        System.IO.File.Delete(file);
            }
            catch { }
        }

        #endregion

        public string FileLocation()
        {
            return file;
        }
    }

    interface IPrintToPACSFile
    {
        string FileLocation();
    }

    [Serializable]
    public class ProcedureStep
    {

        private ModalityWorklistResult _Result;
        private MPPSNCreate _MppsCreate;

        public ModalityWorklistResult Result
        {
            get { return _Result; }
            set { _Result = value; }
        }

        public MPPSNCreate MppsCreate
        {
            get { return _MppsCreate; }
            set { _MppsCreate = value; }
        }

        public ProcedureStep()
        {
        }

        public ProcedureStep(ModalityWorklistResult result)
        {
            _Result = result;
        }

        public ProcedureStep(ModalityWorklistResult result, MPPSNCreate mPPSNCreate)
        {
            _Result = result;
            _MppsCreate = mPPSNCreate;
        }
    }

    internal static class Extensions
    {
        public static void CopyTo<T>(this object source, T dest)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The object you are copying from cannot be null");

            if (dest == null)
                throw new ArgumentNullException("dest", "The object you are copying to cannot be null");

            // Don't copy if they are the same object
            if (!ReferenceEquals(source, dest))
            {
                List<PropertyInfo> matches = GetMatchingProperties(source, dest);

                foreach (PropertyInfo fromProperty in matches)
                {
                    PropertyInfo toProperty = dest.GetType().GetProperty(fromProperty.Name);

                    if (toProperty.CanWrite)
                    {
                        object value = null;

                        if (source is DataRow)
                        {
                            DataRow row = source as DataRow;

                            if (row[fromProperty.Name] != null)
                                value = row[fromProperty.Name];
                        }
                        else
                        {
                            value = fromProperty.GetValue(source, null);
                        }

                        if (value == DBNull.Value)
                            value = null;
                        toProperty.SetValue(dest, value, null);
                    }
                }
            }
        }

        private static List<PropertyInfo> GetMatchingProperties(object source, object target)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (target == null)
                throw new ArgumentNullException("target");

            var sourceType = source.GetType();
            var sourceProperties = sourceType.GetProperties();
            var targetType = target.GetType();
            var targetProperties = targetType.GetProperties();
            var properties = (from s in sourceProperties
                              from t in targetProperties
                              where s.Name == t.Name &&
                                    s.PropertyType == t.PropertyType
                              select s).ToList();

            return properties;
        }
    }
}
