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
using VisioForge.Core.VideoEdit;
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
        private async void _miStoreToPACS_Click(object sender, EventArgs e)
        {
            try
            {
                if (_mySettings._settings.StoreServers.serverList.Length == 0)
                    return;
                //await ServiceLocator.RisService.SearchAndDeleteStudiesAsync(ServiceLocator.StorageServer, _machidinh);
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
                                await DoStoreAsync(strFile, server);
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex, "Lỗi khi store file lên PACS: {File}", strFile);
                                LogError(ex.Message);
                                logWindow.TopMost = false;
                                EnableItems(true, "", "");
                                MessageBox.Show("Đã xảy ra lỗi: \n" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
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
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi thực hiện Store to PACS");
            }
        }
        #region StoreScu
        /// <summary>
        /// Store 1 file DICOM lên PACS. Chạy Store trên thread pool để không block UI,
        /// thay cho vòng lặp Application.DoEvents() trước đây.
        /// Cấu hình TLS/certificate lấy từ chính server được truyền vào (trước đây đọc lại
        /// SelectedItem của combobox nên có thể lệch server nếu người dùng đổi lựa chọn giữa chừng).
        /// Exception được ném lên caller xử lý trên UI thread.
        /// </summary>
        private async Task DoStoreAsync(string dsFile, MyServer storeserver)
        {
            DicomScp server = new DicomScp();
            server.AETitle = storeserver._sAE;
            server.PeerAddress = IPAddress.Parse(storeserver._sIP);
            server.Port = storeserver._port;
            server.Timeout = storeserver._timeout;
            bStored = false;

            // Tạo/cấu hình _cstore trên UI thread trước khi chạy Store trên thread pool
            CreateCStoreObject(storeserver);
            _cstore.AETitle = _mySettings._settings.clientAE;
            _cstore.HostPort = _mySettings._settings.clientPort;

            await Task.Run(() => _cstore.Store(server, dsFile));
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
}
