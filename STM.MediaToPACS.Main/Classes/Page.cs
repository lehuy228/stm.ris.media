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
            catch (Exception ex)
            {
                // Không xóa được file tạm không chặn luồng chính, nhưng cần log để theo dõi rác trên đĩa
                Log.Warning(ex, "Không xóa được file tạm khi dispose Page");
            }
        }

        #endregion

        public string FileLocation()
        {
            return file;
        }
    }
}
