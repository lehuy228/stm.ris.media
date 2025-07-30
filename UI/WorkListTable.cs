using Leadtools.Dicom.Scu.Common;
using Leadtools.Dicom.Scu.Queries;
using Leadtools.Dicom.Scu;
using Leadtools.DicomDemos;
using PrintToPACS.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Net;
using PrintToPACSDemo.Queries;
using System.Threading;
using static PrintToPACSDemo.FrmMain;
using Leadtools.Dicom;
using System.Reflection;
using System.Runtime.InteropServices;
using Leadtools.Dicom.Common.DataTypes.Modality;
using Leadtools.Dicom.Common.DataTypes;
using Leadtools.Dicom.Common.Extensions;
using PrintToPACSDemo.UI.CameraUI;
using System.Drawing;
using Application = System.Windows.Forms.Application;
using Font = System.Drawing.Font;
using VisioForge.Core.VideoCapture;
using DevExpress.XtraGrid.Views.Grid;
using System.Threading.Tasks;

namespace PrintToPACSDemo.UI
{
    public partial class WorkListTable : DevExpress.XtraEditors.XtraForm
    {
        public MySettings _mySettings = PacsSettings.Instance;
        private MyQueryRetrieveScu _find;
        private FrmOperation _frmOperation;
        private FrmMain _frmMain;
        private Form _frmCamera = new Form(); 

        private DicomFindQuery _findQuery = new DicomFindQuery();
        private PatientBasedQuery _pbQuery = new PatientBasedQuery();

        private List<MWLItem> mWLItems = new List<MWLItem>();
        Dictionary<string, ModalityWorklistResult> mWLItemsDictionary = new Dictionary<string, ModalityWorklistResult>();

        private const string _sNewline = "\r\n";
        private const string _sNewlineTab = "\r\n\t";
        private const string _sNewlineTabTab = "\r\n\t\t";

        private bool bCancelOperation = false;
        private List<ProcedureStep> listProcedureStep = new List<ProcedureStep>();

        public WorkListTable()
        {

            try
            {
                InitializeComponent();
                PacsSettings.LogWindow = new LogWindow();
                PacsSettings.LogWindow.Visible = false;
                LoadSettings();
                _mySettings.CopyGlobalSettings();
                foreach (var device in new VideoCaptureCore().Video_CaptureDevices())
                {
                    _cbCapture.Properties.Items.Add(device.Name);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private async void WorkListTable_Load(object sender, EventArgs e)
        {
                    InitializeForm();
                    SetServersComboBox(true);
                    _cbCapture.SelectedIndex = 0;
                    _cbStartEnd.Checked = false;
             
            

        }

        private void WorkListTable_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Utils.EngineShutdown();
                Utils.DicomNetShutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Closing Form " + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        #region Methods...
        private void LoadSettings()
        {
            try
            {
                // Settings are stored at:
                // %USERPROFILE%\Local Settings\Application Data\<Company Name>\<appdomainname>_<eid>_<hash>\<verison>\user.config
                _mySettings.Load();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }
        }
        private void InitializeForm()
        {
            string fileClientCertificate = Application.StartupPath + "\\client.pem";
            if (File.Exists(fileClientCertificate))
            {
                if (_mySettings._settings.clientCertificate == string.Empty)
                    _mySettings._settings.clientCertificate = Application.StartupPath + "\\client.pem";
                if (_mySettings._settings.privateKey == string.Empty)
                    _mySettings._settings.privateKey = Application.StartupPath + "\\client.pem";
                if (_mySettings._settings.privateKeyPassword == string.Empty)
                    _mySettings._settings.privateKeyPassword = "test";
            }
        }
        #endregion

        private void _btnSettings_Click(object sender, EventArgs e)
        {
            if (PacsSettings.DoOptions(0) != DialogResult.Cancel)
                SetServersComboBox(false);
            UpdateComboBoxes();
        }

        private void UpdateComboBoxes()
        {
            int iMWLIndex = _cbMWLServers.SelectedIndex;

            if (_cbMWLServers.Properties.Items.Count != 0)
                if (_cbMWLServers.Properties.Items.Count > iMWLIndex && iMWLIndex >= 0)
                    _cbMWLServers.SelectedIndex = iMWLIndex;
                else
                   if (_cbMWLServers.Properties.Items.Count > _mySettings._settings.DefaultMWLServer)
                    _cbMWLServers.SelectedIndex = _mySettings._settings.DefaultMWLServer;
                else
                    _cbMWLServers.SelectedIndex = 0;
        }

        private void SetServersComboBox(bool bSelectDefault)
        {
            //_cbSCPServers.Items.Clear();
            _cbMWLServers.Properties.Items.Clear();

            MyServer[] list;
            int defaultserver = 0;

            list = _mySettings._settings.QueryMWLServers.serverList;
            defaultserver = _mySettings._settings.DefaultMWLServer;

            if (list.Length == 0)
            {
                //_tbQueryMWList.Enabled = false;
            }
            else
            {
                //_tbQueryMWList.Enabled = true;
                foreach (MyServer server in list)
                {
                    _cbMWLServers.Properties.Items.Add(server);
                }
                if (bSelectDefault)
                    if (defaultserver < list.Length)
                        _cbMWLServers.SelectedIndex = defaultserver;
                    else
                        _cbMWLServers.SelectedIndex = 0;
            }
        }

        public string GetProcedureStepFilename()
        {
            string commonFolder = DicomDemoSettingsManager.GetFolderPath();
            string settingsFilename = commonFolder + @"\procedurestep_printToPacs.xml";
            if (!File.Exists(settingsFilename))
            {
                File.Create(settingsFilename).Dispose();
            }
            return settingsFilename;
        }

        public void LoadProcedureStep()
        {
            XmlSerializer SerializerObj = new XmlSerializer(typeof(List<ProcedureStep>));
            string filename = GetProcedureStepFilename();

            if (File.Exists(filename))
            {
                try
                {
                    FileStream ReadFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                    listProcedureStep = (List<ProcedureStep>)SerializerObj.Deserialize(ReadFileStream);

                    ReadFileStream.Close();
                }
                catch (Exception)
                {
                }
            }
        }

        private ModalityWorklistQuery GetQueryParams()
        {
            ModalityWorklistQuery query = new ModalityWorklistQuery();

            //Truy vấn theo bệnh nhân
            query.PatientId = _txtPatientID.Text;
            query.AccessionNumber = _txtAccessionNumber.Text;
            query.PatientName.Middle = _txtPatientMiddle.Text;
            query.PatientName.Given = _txtPatientFirst.Text;
            query.PatientName.Family = _txtPatientLast.Text;

            //truy vấn theo ca chụp

            BroadQuery bq = new BroadQuery();
            bq.Modality = _cbbModality.Text;
            bq.ScheduledStationAeTitle = _txtAETitle.Text;

            if (_cbStartEnd.Checked)
            {

                bq.ScheduledProcedureStepStartDate = new DateRange
                {
                    StartDate = _dTPStart.DateTime,
                    EndDate = _dTPEnd.DateTime
                };
            }

            query.Broad.Add(bq);

            return query;
        }

        private DicomScp GetQueryServer()
        {
            DicomScp server;
            server = new DicomScp();
            MyServer s = null;

            //if (_tbDicomInfo.SelectedTab == _pageSCPQuery)
            //    s = (MyServer)(_cbSCPServers.SelectedItem);
            //else
            s = (MyServer)(_cbMWLServers.SelectedItem);

            server.AETitle = s._sAE;
            server.PeerAddress = IPAddress.Parse(s._sIP);
            server.Port = s._port;
            server.Timeout = s._timeout;
            return server;
        }

        private void IsQueryEmpty(out bool bSCPQueryEmpty, out bool bSWLQueryEmpty, out bool bPWLQueryEmpty)
        {
            DicomFindQuery newFindQ = new DicomFindQuery();
            bSCPQueryEmpty = (_findQuery.InstanceNumber == newFindQ.InstanceNumber) &
                (_findQuery.Modalities == newFindQ.Modalities) &
                (_findQuery.Modality == newFindQ.Modality) &
                (_findQuery.PatientId == newFindQ.PatientId) &
                (_findQuery.PatientName == newFindQ.PatientName) &
                (_findQuery.PerfProcStepStartDate == newFindQ.PerfProcStepStartDate) &
                (_findQuery.PerfProcStepStartTime.ToString() == newFindQ.PerfProcStepStartTime.ToString()) &
                (_findQuery.AccessionNumber == newFindQ.AccessionNumber) &
                (_findQuery.QueryLevel == newFindQ.QueryLevel) &
                (_findQuery.ReferringPhysiciansName == newFindQ.ReferringPhysiciansName) &
                (_findQuery.RequestedProcId == newFindQ.RequestedProcId) &
                (_findQuery.SchedProcStepId == newFindQ.SchedProcStepId) &
                (_findQuery.SeriesInstanceUID == newFindQ.SeriesInstanceUID) &
                (_findQuery.SeriesNumber == newFindQ.SeriesNumber) &
                (_findQuery.SOPInstanceUID == newFindQ.SOPInstanceUID) &
                (_findQuery.StudyDate.ToString() == newFindQ.StudyDate.ToString()) &
                (_findQuery.StudyId == newFindQ.StudyId) &
                (_findQuery.StudyInstanceUID == newFindQ.StudyInstanceUID) &
                (_findQuery.StudyTime.ToString() == newFindQ.StudyTime.ToString());
            //BroadBasedQuery newBroadQuery = new BroadBasedQuery();
            //bSWLQueryEmpty = (_bbQuery.Modality == newBroadQuery.Modality) &
            //    (_bbQuery.ScheduledProcedureStepStartDate == newBroadQuery.ScheduledProcedureStepStartDate) &
            //    (_bbQuery.ScheduledStationAeTitle == newBroadQuery.ScheduledStationAeTitle);
            bSWLQueryEmpty = false;
            PatientBasedQuery newPatientQuery = new PatientBasedQuery();
            bPWLQueryEmpty = (_pbQuery.PatientId == newPatientQuery.PatientId) &
                (_pbQuery.PatientName.ToString() == newPatientQuery.PatientName.ToString()) &
                (_pbQuery.AccessionNumber == newPatientQuery.AccessionNumber) &
                (_pbQuery.RequestedProcedureId == newPatientQuery.RequestedProcedureId);
        }

        private void ShowErrorMessage(Exception ex)
        {
            if (InvokeRequired)
            {
                Invoke(new ShowErrorMessageDelegate(ShowErrorMessage), ex);
            }
            else
            {
                //EnableItems(true, "", "");
                //bool bTopMost = PacsSettings.LogWindow.TopMost;
                //PacsSettings.LogWindow.TopMost = false;
                MessageBox.Show(this, "Error Occurred: \n" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //PacsSettings.LogWindow.TopMost = bTopMost;
            }

        }

        void _frmOperation_Cancel(object sender, EventArgs e)
        {
            bCancelOperation = true;
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

                _btnSettings.Enabled = enable;
                _btnLogs.Enabled = enable;
                _btnMWLQuery.Enabled = enable;
                _btnUse.Enabled = enable;
                _cbCapture.Enabled = enable;
                _cbMWLServers.Enabled = enable;
                _tLPQuery.Enabled = enable;

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
                            bCancelOperation = false;
                            if (strBtnCaption != "")
                                _frmOperation.Cancel += new EventHandler(_frmOperation_Cancel);
                            _frmOperation.Show();
                        }
                }
            }
        }

        private void AddAction(string action)
        {
            System.Drawing.Color oldColor = PacsSettings.LogWindow.RichTextBox.SelectionColor;
            if (action == "")
            {
                return;
            }
            PacsSettings.LogWindow.RichTextBox.SelectionLength = 0;
            PacsSettings.LogWindow.RichTextBox.SelectionStart = PacsSettings.LogWindow.RichTextBox.Text.Length;
            PacsSettings.LogWindow.RichTextBox.SelectionColor = Color.Blue;
            PacsSettings.LogWindow.RichTextBox.SelectionFont = new Font(PacsSettings.LogWindow.RichTextBox.SelectionFont, FontStyle.Bold);
            PacsSettings.LogWindow.RichTextBox.AppendText(action + ": ");

            PacsSettings.LogWindow.RichTextBox.SelectionColor = oldColor;
        }

        private DicomDataSet LoadDatasetResource(string name, out IntPtr handle)
        {
            DicomDataSet ds = new DicomDataSet();
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] resourceNames = assembly.GetManifestResourceNames();

            handle = IntPtr.Zero;
            foreach (string n in resourceNames)
            {
                if (n.ToLower().Contains(name.ToLower()))
                {
                    Stream stream = assembly.GetManifestResourceStream(n);
                    byte[] data = new byte[stream.Length];
                    handle = Marshal.AllocHGlobal(Convert.ToInt32(stream.Length));

                    stream.Read(data, 0, Convert.ToInt32(stream.Length));
                    Marshal.Copy(data, 0, handle, Convert.ToInt32(stream.Length));
                    ds.Load(handle, stream.Length, DicomDataSetFlags.None);
                    continue;
                }
            }

            return ds;
        }

        public delegate void AddResultItemDelegate(ModalityWorklistResult result);
        private void AddResultItem(ModalityWorklistResult result)
        {
            ListViewItem item;

            if (InvokeRequired)
            {
                Invoke(new AddResultItemDelegate(AddResultItem), result);
            }
            else
            {
                MWLItem mWLItem = new MWLItem()
                {
                    AccessionNumber = !result.AccessionNumber.IsNullOrEmpty() ? result.AccessionNumber : string.Empty,
                    PatientID = !result.PatientId.IsNullOrEmpty() ? result.PatientId : string.Empty,
                    PatientName = !result.PatientName.FullDicomEncoded.IsNullOrEmpty() ? result.PatientName.FullDicomEncoded : string.Empty,
                    BirthDate = result.PatientBirthDate.HasValue ? result.PatientBirthDate.Value.ToShortDateString() : string.Empty,
                    Gender = !result.PatientSex.IsNullOrEmpty() ? result.PatientSex : string.Empty,
                    ScheduledStartDate = result.ScheduledProcedureStepSequence[0].ScheduledProcedureStepStartDate.HasValue ? result.ScheduledProcedureStepSequence[0].ScheduledProcedureStepStartDate.Value.ToShortDateString() : string.Empty,
                    Modality = !result.ScheduledProcedureStepSequence[0].Modality.IsNullOrEmpty() ? result.ScheduledProcedureStepSequence[0].Modality : string.Empty,
                    ScheduledStationAE = !result.ScheduledProcedureStepSequence[0].ScheduledStationAeTitle.IsNullOrEmpty() ? result.ScheduledProcedureStepSequence[0].ScheduledStationAeTitle : string.Empty,
                    ScheduleProcedureStep = !result.ScheduledProcedureStepSequence[0].ScheduledProcedureStepDescription.IsNullOrEmpty() ? result.ScheduledProcedureStepSequence[0].ScheduledProcedureStepDescription : string.Empty,
                    RequestedProcedureID = !result.RequestedProcedureId.IsNullOrEmpty() ? result.RequestedProcedureId : string.Empty,
                    ReferringPhysician = !result.ReferringPysician.FullDicomEncoded.IsNullOrEmpty() ? result.ReferringPysician.FullDicomEncoded : string.Empty,
                    RequestingPhysician = !result.RequestingPhysician.FullDicomEncoded.IsNullOrEmpty() ? result.RequestingPhysician.FullDicomEncoded : string.Empty,
                    //PerformingPhysician = !result.ScheduledProcedureStepSequence[0].ScheduledPerformingPhysician.FullDicomEncoded.IsNullOrEmpty() ? result.ScheduledProcedureStepSequence[0].ScheduledPerformingPhysician.FullDicomEncoded : string.Empty,
                };


                mWLItemsDictionary.Add(result.AccessionNumber, result);

                foreach (ProcedureStep ps in listProcedureStep)
                {
                    if (ps.Result.AccessionNumber.Equals(result.AccessionNumber))
                    {
                        mWLItem.MPPS = "INPROCESSING";
                        if (_cbMPPSINPROGRESS.Checked)
                        {
                            mWLItems.Add(mWLItem);
                        }
                    }
                }

                if (!_cbMPPSINPROGRESS.Checked)
                {
                    mWLItems.Add(mWLItem);
                }

                //  FindSeries(result.StudyInstanceUid);
            }
        }

        private void FoundMatch(ModalityWorklistResult result, DicomDataSet ds)
        {
            string startDate = result.ScheduledProcedureStepSequence[0].ScheduledProcedureStepStartDate.HasValue ? result.ScheduledProcedureStepSequence[0].ScheduledProcedureStepStartDate.Value.ToShortDateString() : string.Empty;
            string message =
                _sNewlineTab + "Accession #:\t\t " + result.AccessionNumber +
                _sNewlineTab + "Patient Name:\t\t" + result.PatientName.FullDicomEncoded +
                _sNewlineTab + "Scheduled Start Date:\t" + startDate;
            LogText("Worklist Item Found", message);

            if (ds != null)
            {
                DicomDataSet data = new DicomDataSet();

                data.Copy(ds, null, null);
                result.Tag = data;
            }
            AddResultItem(result);
        }

        private bool DoSearch()
        {
            bool bRet = false;
            ModalityWorklistQuery query = GetQueryParams();
            PacsSettings.SCP = GetQueryServer();

            MyServer s = null;
            s = (MyServer)(_cbMWLServers.SelectedItem);

            bool bSCPQueryEmpty;
            bool bSWLQueryEmpty;
            bool bPWLQueryEmpty;
            IsQueryEmpty(out bSCPQueryEmpty, out bSWLQueryEmpty, out bPWLQueryEmpty);

            //bool bTopMost = PacsSettings.LogWindow.TopMost;
            //PacsSettings.LogWindow.TopMost = false;
            //PacsSettings.LogWindow.TopMost = bTopMost;

            EnableItems(false, "Đang truy vấn tới PACS vui lòng đợi...", "");
            CreateCFindObject(s);
            _find.MatchStudy += new MatchStudyDelegate(_find_MatchPatient);
            _find.AETitle = _mySettings._settings.clientAE;
            _find.HostPort = _mySettings._settings.clientPort;
            Thread t = new Thread(delegate ()
            {
                IntPtr handle = IntPtr.Zero;
                try
                {
                    using (DicomDataSet template = LoadDatasetResource("MwlIheCFindScu.dcm", out handle))
                    {
                        _find.Find<ModalityWorklistQuery, ModalityWorklistResult>(PacsSettings.SCP, query,
                                                                             new DicomMatchDelegate<ModalityWorklistResult>(FoundMatch));
                        if (handle != IntPtr.Zero)
                            Marshal.FreeHGlobal(handle);
                    }
                }
                catch (Exception ex)
                {
                    //LogError(ex.Message);
                    ShowErrorMessage(ex);
                    bRet = false;
                }

            });
            t.Start();
            while (t.IsAlive)
            {
                Application.DoEvents();
            }
            bRet = true;
            return bRet;
        }

        private void _btnMWLQuery_Click(object sender, EventArgs e)
        {
            MWLQuery();
        }

        private void MWLQuery()
        {
            if (listProcedureStep != null)
            {
                listProcedureStep.Clear();
            }
            LoadProcedureStep();
            mWLItems.Clear();
            mWLItemsDictionary.Clear();
            _dGVMWLItems.DataSource = null;

            if (DoSearch())
            {
                int iMatchCount = 0;
                iMatchCount = mWLItems.Count;

                EnableItems(true, "", "");
                if (iMatchCount == 0)
                {
                    //bool bTopMost = PacsSettings.LogWindow.TopMost;

                    MessageBox.Show(this, "Không tìm thấy thông tin phù hợp! ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PacsSettings.LogWindow.TopMost = false;
                    //PacsSettings.LogWindow.TopMost = bTopMost;
                }
                else
                {
                    _dGVMWLItems.DataSource = mWLItems;
                }
            }
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

        private MPPSNCreate mppsCreate;
        public void SaveProcedureStep(ProcedureStep procedureStep)
        {
            string filename = GetProcedureStepFilename();
            XmlSerializer xs = new XmlSerializer(typeof(List<ProcedureStep>));
            TextWriter xmlTextWriter = new StreamWriter(filename);
            listProcedureStep.Add(procedureStep);
            xs.Serialize(xmlTextWriter, listProcedureStep);
            xmlTextWriter.Close();
        }

        private ProcedureStep ProcedureStepDelete;
        //private void LoadCapture(FrmMain frmMain)
        //{
        //    CameraControl cameraControl = new CameraControl(_cbCapture.Text);
        //    MediaPlayerControl mediaPlayerControl;
        //    cameraControl.Dock = DockStyle.Fill;

        //    //
        //    //Sự kiện quay click vào video thu được sau khi capture
        //    //
        //    frmMain.VideoRoll_Click += (o, i) =>
        //    {
        //        try
        //        {
        //            PictureBox pictureBox = o as PictureBox;
        //            cameraControl.Visible = false;
        //            cameraControl.VideoCapture1.StopAsync();

        //            mediaPlayerControl = new MediaPlayerControl(pictureBox.Tag.ToString());
        //            mediaPlayerControl.Dock = DockStyle.Fill;

        //            mediaPlayerControl.SnapshotMedia_Click += (s, e) =>
        //            {
        //                _frmMain.LoadRasterImage(mediaPlayerControl.SnapshotMedia);
        //            };
        //            mediaPlayerControl.BackCamera_Click += (s, e) =>
        //            {
        //                cameraControl.VideoCapture1.StartAsync();
        //                cameraControl.Visible = true;
        //                if (Screen.AllScreens.Length > 1)
        //                {
        //                    _frmCamera.Controls.Remove(mediaPlayerControl);
        //                }
        //                else
        //                {
        //                    _frmMain.panelCamera.Controls.Remove(mediaPlayerControl);
        //                }
        //            };

        //            if (Screen.AllScreens.Length > 1)
        //            {
        //                _frmCamera.Controls.Add(mediaPlayerControl);
        //            }
        //            else
        //            {
        //                _frmMain.panelCamera.Controls.Add(mediaPlayerControl);
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    };

        //    //if (Screen.AllScreens.Length > 1)
        //    //{
        //    //    _frmCamera.Controls.Add(cameraControl);
        //    //    _frmCamera.Name = "FormCamera";
        //    //    _frmCamera.StartPosition = FormStartPosition.Manual;
        //    //    _frmCamera.WindowState = FormWindowState.Maximized;
        //    //    _frmCamera.FormBorderStyle = FormBorderStyle.None;
        //    //    _frmCamera.Location = Screen.AllScreens[1].WorkingArea.Location;
        //    //    _frmCamera.TopMost = true;
        //    //    _frmCamera.FormClosed += (s, e) =>
        //    //    {
        //    //        cameraControl.CameraControlRemoved();
        //    //    };
        //    //    _frmCamera.Show(this);
        //    //}
        //    //else
        //    //{
        //        //frmMain.tableLayoutPanelInfo.Visible = false;
        //        cameraControl.Dock = DockStyle.Fill;
        //        frmMain.panelCamera.Controls.Add(cameraControl);
        //    //}

        //    frmMain._CameraControl = cameraControl;
        //}

        //private void FormMain_Closed(object sender, FormClosedEventArgs e)
        //{
        //    if (_frmCamera.Name == "FormCamera")
        //    {
        //        _frmCamera.Close();
        //    }
        //    else
        //    {
        //        _frmMain.CloseFrmMain();
        //    }
        //}

        //
        //Xóa item trong listview khi trạng thái MPPS là thành công
        //
        public void DeleteItemSelectedListview()
        {
            string filename = GetProcedureStepFilename();
            XmlSerializer xs = new XmlSerializer(typeof(List<ProcedureStep>));
            TextWriter xmlTextWriter = new StreamWriter(filename);
            listProcedureStep.Remove(ProcedureStepDelete);
            xs.Serialize(xmlTextWriter, listProcedureStep);
            xmlTextWriter.Close();
        }

        private void _btnLogs_Click(object sender, EventArgs e)
        {
            PacsSettings.LogWindow.Visible = !PacsSettings.LogWindow.Visible;
        }

        private void _dGVMWLItems_DoubleClick(object sender, EventArgs e)
        {
            GridView gridView = _dGVMWLItems.MainView as GridView;

            if (gridView.SelectedRowsCount <= 0)
                return;

            int selectedRowHandle = gridView.GetSelectedRows()[0];
            object value = gridView.GetRowCellValue(selectedRowHandle, gridView.Columns[0]);
            ModalityWorklistResult result = mWLItemsDictionary[value.ToString()] as ModalityWorklistResult;

            // 
            // Initialize required procedure step variables 
            // 
            try
            {

                if (string.IsNullOrEmpty(gridView.GetRowCellValue(selectedRowHandle, gridView.Columns[12]).ToString()))
                {     
                    foreach (ProcedureStep ps in listProcedureStep)
                    {
                        if (ps.Result.AccessionNumber.Equals(result.AccessionNumber))
                        {
                            ProcedureStepDelete = ps;
                            mppsCreate = ps.MppsCreate;
                            break;
                        }
                    }
                }
                else
                {
                    mppsCreate = MPPSNCreate.FromWorklistItem(result);
                    mppsCreate.PerformedStationAeTitle = Environment.MachineName;
                    mppsCreate.PerformedStationName = Environment.MachineName;
                    mppsCreate.PerformedProcedureStepStartDate = DateTime.Now;
                    mppsCreate.PerformedProcedureStepStartTime = DateTime.Now;
                    mppsCreate.PerformedSeriesSequence = new List<PerformedSeries>();
                    mppsCreate.PerformedSeriesSequence.Add(new PerformedSeries());
                    mppsCreate.PerformedSeriesSequence[0].ProtocolName = MPPSNCreate.RandomId(16);
                    mppsCreate.PerformedSeriesSequence[0].SeriesInstanceUID = Utils.GenerateDicomUniqueIdentifier();
                    mppsCreate.SOPInstance.SOPInstanceUid = Utils.GenerateDicomUniqueIdentifier();

                    PacsSettings.PPS.Create<MPPSNCreate>(PacsSettings.SCP, mppsCreate, new BeforeAddTagDelegate(BeforeAddTagDelegate));
                    gridView.SetRowCellValue(selectedRowHandle, gridView.Columns[10], "INPROCESSING");
                    ProcedureStepDelete = new ProcedureStep(result, mppsCreate);
                    SaveProcedureStep(ProcedureStepDelete);
                }

                _frmMain = new FrmMain(this, result, mppsCreate);
                //_frmMain.FormClosed += FormMain_Closed;
                _frmMain.BringToFront();
                _frmMain.Owner = this;
                _frmCamera.Owner = this;
                _frmMain.Show();

                //LoadCapture(_frmMain);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region FindQuery
        private void LogText(string action, string logText)
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

        void _find_BeforeConnect(object sender, Leadtools.Dicom.Scu.Common.BeforeConnectEventArgs e)
        {
            LogText("Before Connect", e.Scp.ToString());

        }

        void _find_AfterConnect(object sender, Leadtools.Dicom.Scu.Common.AfterConnectEventArgs e)
        {
            string message;
            if (e.Error == DicomExceptionCode.Success)
            {
                message = _sNewlineTab + "Connection Successful";
            }
            else
            {
                message =
                   _sNewlineTab + "Connection failed" +
                   _sNewlineTab + "Error:\t" + e.Error.ToString();
            }

            LogText("After Connect", message);
        }

        void _find_BeforeAssociateRequest(object sender, Leadtools.Dicom.Scu.Common.BeforeAssociateRequestEventArgs e)
        {
            LogText("Before Associate Request", e.Associate.ToString());
        }

        void _find_AfterAssociateRequest(object sender, Leadtools.Dicom.Scu.Common.AfterAssociateRequestEventArgs e)
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

        void _find_BeforeCFind(object sender, Leadtools.Dicom.Scu.Common.BeforeCFindEventArgs e)
        {
            string message =
               _sNewlineTab + "QueryLevel:\t" + e.QueryLevel.ToString() +
               _sNewlineTab + "Priority:\t" + e.Priority.ToString();

            LogText("Before CFind", message);

            //EnableCancel(true);
        }

        void _find_AfterCFind(object sender, Leadtools.Dicom.Scu.Common.AfterCFindEventArgs e)
        {
            string message;
            if (e.Status == DicomCommandStatusType.Success)
            {
                message =
                   _sNewlineTab + "MatchCount:\t" + e.MatchCount.ToString() +
                   _sNewlineTab + "Status:\t" + e.Status.ToString();
            }
            else
            {
                message =
                   _sNewlineTab + " CFind failed" +
                   _sNewlineTab + "Status: " + e.Status.ToString();
            }
            LogText("After CFind", message + _sNewlineTab + "****************************" + _sNewlineTab);
            //EnableCancel(false);
        }

        void _find_MatchPatient(object sender, Leadtools.Dicom.Scu.Common.MatchEventArgs<Study> e)
        {
            string message =
               _sNewlineTab + "QueryLevel: " + e.QueryLevel.ToString() +
               _sNewlineTab + "Availability:\t" + e.Availability.ToString() +
               _sNewlineTab + "Patient:\t" + e.Info.Patient.ToString() +
               _sNewlineTab + "RetrieveAETitle:\t" + e.RetrieveAETitle.ToString();
            LogText("Study Patient Found Found", message);
            try
            {
                AddPatientItem(e);
            }
            catch { }
        }

        public delegate void AddStudyItemDelegate(MatchEventArgs<Study> ds);
        private void AddStudyItem(MatchEventArgs<Study> e)
        {
            //ListViewItem item;

            //if (InvokeRequired)
            //{
            //    Invoke(new AddStudyItemDelegate(AddStudyItem), e);
            //}
            //else
            //{
            //    item = _lstSCPStudies.Items.Add(e.Info.Id);
            //    if (e.Info.ReferringPhysiciansName != null)
            //        item.SubItems.Add(e.Info.ReferringPhysiciansName.FullDicomEncoded);
            //    else
            //        item.SubItems.Add("");
            //    item.SubItems.Add(e.Info.AccessionNumber);
            //    item.SubItems.Add(e.Info.Date.HasValue ? e.Info.Date.ToString() : string.Empty);
            //    item.SubItems.Add(e.Info.Time.HasValue ? e.Info.Time.ToString() : string.Empty);

            //    item.Tag = e.Info;
            //}
        }

        public delegate void AddPatientItemDelegate(MatchEventArgs<Study> ds);
        private void AddPatientItem(MatchEventArgs<Study> e)
        {
            ListViewItem item = null;

            if (InvokeRequired)
            {
                Invoke(new AddStudyItemDelegate(AddPatientItem), e);
            }
            else
            {
                if (e.Info.Patient == null)
                    return;

                // Check if the Patient already exist in _lstSCPPatient
                //foreach (ListViewItem lvi in _lstSCPPatient.Items)
                //{
                //    if (lvi.SubItems[0].Text == e.Info.Patient.Name.FullDicomEncoded || lvi.SubItems[1].Text == e.Info.Patient.Id)
                //    {
                //        item = lvi;
                //        break;
                //    }
                //}

                //if (item == null)
                //{
                //    item = _lstSCPPatient.Items.Add(e.Info.Patient.Name.FullDicomEncoded);
                //    item.SubItems.Add(e.Info.Patient.Id);
                //    item.SubItems.Add(e.Info.Patient.Sex);
                //    item.SubItems.Add(e.Info.Patient.BirthDate.HasValue ? e.Info.Patient.BirthDate.ToString() : string.Empty);
                //    item.Tag = new List<Study>();
                //}

                (item.Tag as List<Study>).Add(e.Info);
            }
        }

        void _find_AfterSecureLinkReady(object sender, AfterSecureLinkReadyEventArgs e)
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

        void _find_BeforeCMove(object sender, BeforeCMoveEventArgs e)
        {
            string message =
               _sNewlineTab + "Priority:\t" + e.Priority + e.Scp.ToString() +
               _sNewlineTab + "Desination AE:\t" + e.DestinationAETitle;
            LogText("Before CMove", message);
            //EnableCancel(true);
            //         _moveCount = 0;
        }

        void _find_AfterCMove(object sender, AfterCMoveEventArgs e)
        {
            string message;
            if (e.Status == DicomCommandStatusType.Success || e.Status == DicomCommandStatusType.Pending || e.Status == DicomCommandStatusType.Warning)
            {
                message =
                   _sNewlineTab + "Status:\t" + e.Status.ToString() +
                   _sNewlineTab + "Completed:\t" + e.Completed.ToString() +
                   _sNewlineTab + "Warning:\t" + e.Warning.ToString() +
                   _sNewlineTab + "Failed:\t" + e.Failed.ToString();
            }
            else
            {
                message = _sNewlineTab + " CMove failed\r\n\tStatus: " + e.Status.ToString();
            }
            LogText("After CMove", message);
            //if (e.Status != DicomCommandStatusType.Pending)
            //EnableCancel(false);
        }

        void _find_PrivateKeyPassword(object sender, PrivateKeyPasswordEventArgs e)
        {
            e.PrivateKeyPassword = _mySettings._settings.privateKeyPassword;
        }

        public delegate void CreateCFind(MyServer server);
        void CreateCFindObject(MyServer server)
        {
            if (this.InvokeRequired)
            {
                Invoke(new CreateCFind(CreateCFindObject), new object[] { server });
            }
            else
            {
                if (_find != null)
                {
                    _find.Dispose();
                }
                if (server._useTls)
                {
                    _find = new MyQueryRetrieveScu(this, _mySettings._settings.TempDir, DicomNetSecurityeMode.Tls, null);
                }
                else
                {
                    _find = new MyQueryRetrieveScu(this);
                }

                _find.ImplementationClass = _sConfigurationImplementationClass;
                _find.ProtocolVersion = _sConfigurationProtocolversion;
                _find.ImplementationVersionName = _sConfigurationImplementationVersionName;
                _find.AETitle = _mySettings._settings.clientAE;
                _find.HostPort = 1000;

                _find.BeforeConnect += new Leadtools.Dicom.Scu.Common.BeforeConnectDelegate(_find_BeforeConnect);
                _find.AfterConnect += new Leadtools.Dicom.Scu.Common.AfterConnectDelegate(_find_AfterConnect);
                _find.AfterSecureLinkReady += new AfterSecureLinkReadyDelegate(_find_AfterSecureLinkReady);
                _find.BeforeAssociateRequest += new Leadtools.Dicom.Scu.Common.BeforeAssociationRequestDelegate(_find_BeforeAssociateRequest);
                _find.AfterAssociateRequest += new Leadtools.Dicom.Scu.Common.AfterAssociateRequestDelegate(_find_AfterAssociateRequest);
                _find.BeforeCFind += new Leadtools.Dicom.Scu.Common.BeforeCFindDelegate(_find_BeforeCFind);
                _find.AfterCFind += new Leadtools.Dicom.Scu.Common.AfterCFindDelegate(_find_AfterCFind);

                _find.BeforeCMove += new BeforeCMoveDelegate(_find_BeforeCMove);
                _find.AfterCMove += new AfterCMoveDelegate(_find_AfterCMove);

                _find.PrivateKeyPassword += new PrivateKeyPasswordDelegate(_find_PrivateKeyPassword);

                if (server._useTls)
                {
                    try
                    {
                        _find.SetTlsCipherSuiteByIndex(0, DicomTlsCipherSuiteType.DheRsaWith3DesEdeCbcSha);
                        _find.SetTlsClientCertificate(
                           _mySettings._settings.clientCertificate,
                           DicomTlsCertificateType.Pem,
                           _mySettings._settings.privateKey.Length > 0 ? _mySettings._settings.privateKey : null);
                    }
                    catch (Exception ex)
                    {
                        //LogError(ex.Message);
                    }
                    _find._workListTable = this;
                }

                //if (_mySettings._settings.logLowLevel)
                //{
                //    if (_tracer == null)
                //    {
                //        _tracer = new TextBoxTraceListener(PacsSettings.LogWindow.RichTextBox);
                //        Trace.Listeners.Add(_tracer);
                //    }
                //}
                //else
                //{
                //    if (_tracer != null)
                //    {
                //        Trace.Listeners.Remove(_tracer);
                //        _tracer = null;
                //    }
                //}
                _find.DebugLogFilename = string.Empty;
                _find.EnableDebugLog = true;
            }
        }

        #endregion

        private void _cbStartEnd_CheckedChanged(object sender, EventArgs e)
        {
            if (_cbStartEnd.Checked)
            {
                _dTPStart.Enabled = true;
                _dTPEnd.Enabled = true;
            }
            else
            {
                _dTPStart.Enabled = false;
                _dTPEnd.Enabled = false;
            }

        }

        private void _btnUse_Click(object sender, EventArgs e)
        {
            FrmUsage frmUsage = new FrmUsage();
            frmUsage.ShowDialog();
        }
    }


    public class MWLItem
    {
        public string AccessionNumber { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public string ScheduledStartDate { get; set; }
        public string RequestingPhysician { get; set; }
        public string ReferringPhysician { get; set; }
        public string PerformingPhysician { get; set; }
        public string Modality { get; set; }
        public string ScheduledStationAE { get; set; }
        public string ScheduleProcedureStep { get; set; }
        public string RequestedProcedureID { get; set; }
        public string MPPS { get; set; }
    }
}