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
