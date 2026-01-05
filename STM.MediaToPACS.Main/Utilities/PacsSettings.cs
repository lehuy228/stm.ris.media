using Leadtools.Dicom.Scu;
using STM.MediaToPACS.Main.Utilities;
using System.Windows.Forms;

namespace STM.MediaToPACS.Main.UI
{
    internal class PacsSettings
    {
        private static MySettings _mySettings;
        private static PerformedProcedureStepScu _pps = new PerformedProcedureStepScu { AETitle = "LEAD_CLIENT" };
        private static DicomScp _scp;

        private static readonly object _lock = new object();
        private static readonly object _lockScp = new object();
        private static readonly object _lockStaff = new object();

        public string SharedValue { get; set; }
        public static LogWindow LogWindow { get; set; }


        private PacsSettings() { }

        public static MySettings Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_mySettings == null)
                    {
                        _mySettings = new MySettings();
                    }
                    return _mySettings;
                }
            }
        }

        public static DicomScp SCP
        {
            get
            {
                lock (_lockScp)
                {
                    if (_scp == null)
                    {
                         _scp = new DicomScp();
                    }
                    return _scp;
                }
            }
            set
            {
                lock (_lockScp)
                {
                    _scp = value;
                }
            }
        }

        public static PerformedProcedureStepScu PPS
        {
            get { return _pps; }
            set { _pps = value; }
        }

        public static DialogResult DoOptions(int iSelectedTab)
        {
            OptionsDialog options = new OptionsDialog();
            options.serverlistSCP = (MyServerList)_mySettings._settings.QuerySCPServers.Clone();
            options.serverlistMWL = (MyServerList)_mySettings._settings.QueryMWLServers.Clone();
            options.serverlistStore = (MyServerList)_mySettings._settings.StoreServers.Clone();
            options.SelectedTab = iSelectedTab;

            options.ClientAE = _mySettings._settings.clientAE;
            options.ClientCertificate = _mySettings._settings.clientCertificate;
            options.PrivateKey = _mySettings._settings.privateKey;
            options.PrivateKeyPassword = _mySettings._settings.privateKeyPassword;
            options.LogLowLevel = _mySettings._settings.logLowLevel;

            options.AutoDelete = _mySettings._settings.autodelete;

            options.TempDirectory = _mySettings._settings.TempDir;
            options.Selectedtype = _mySettings._settings.selectedtype;

            options.SCCompression = _mySettings._settings.secondaryCaptureCompression;
            options.SCColorCompression = _mySettings._settings.secondaryCaptureColorCompression;
            options.SCGrayCompression = _mySettings._settings.secondaryCaptureGrayCompression;

            options.SCGrayPath = _mySettings._settings.secondaryCaptureGrayPath;
            options.SCColorPath = _mySettings._settings.secondaryCaptureColorPath;
            options.SCPath = _mySettings._settings.secondaryCapturePath;
            options.PdfPath = _mySettings._settings.PdfPath;

            options.PrinterName = _mySettings._settings.printerName;

            options.DefaultSCPServer = _mySettings._settings.DefaultSCPServer;
            options.DefaultMWLServer = _mySettings._settings.DefaultMWLServer;
            options.DefaultStoreServer = _mySettings._settings.DefaultStoreServer;

            DialogResult dr = options.ShowDialog();
            if (dr == DialogResult.OK)
            {
                _mySettings._settings.clientAE = options.ClientAE;
                _mySettings._settings.clientCertificate = options.ClientCertificate;
                _mySettings._settings.privateKey = options.PrivateKey;
                _mySettings._settings.privateKeyPassword = options.PrivateKeyPassword;
                _mySettings._settings.logLowLevel = options.LogLowLevel;

                _mySettings._settings.querySCPServers = options.serverlistSCP;
                _mySettings._settings.queryMWLServers = options.serverlistMWL;
                _mySettings._settings.storeServers = options.serverlistStore;

                _mySettings._settings.autodelete = options.AutoDelete;

                _mySettings._settings.TempDir = options.TempDirectory;
                _mySettings._settings.selectedtype = options.Selectedtype;

                _mySettings._settings.secondaryCaptureCompression = options.SCCompression;
                _mySettings._settings.secondaryCaptureColorCompression = options.SCColorCompression;
                _mySettings._settings.secondaryCaptureGrayCompression = options.SCGrayCompression;

                _mySettings._settings.secondaryCaptureGrayPath = options.SCGrayPath;
                _mySettings._settings.secondaryCaptureColorPath = options.SCColorPath;
                _mySettings._settings.secondaryCapturePath = options.SCPath;
                _mySettings._settings.PdfPath = options.PdfPath;

                _mySettings._settings.DefaultSCPServer = options.DefaultSCPServer;
                _mySettings._settings.DefaultMWLServer = options.DefaultMWLServer;
                _mySettings._settings.DefaultStoreServer = options.DefaultStoreServer;


                _mySettings.Save();
            }
            return dr;
        }
    }
}
