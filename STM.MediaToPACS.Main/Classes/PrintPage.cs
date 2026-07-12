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
            catch (Exception ex)
            {
                // Không xóa được file tạm không chặn luồng chính, nhưng cần log để theo dõi rác trên đĩa
                Log.Warning(ex, "Không xóa được file tạm khi dispose PrintPage");
            }
            //MetaFile.Dispose();
        }
        #endregion

        public string FileLocation()
        {
            return file;
        }
    }
}
