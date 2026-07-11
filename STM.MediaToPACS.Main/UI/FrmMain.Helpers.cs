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
        #region Helper Methods - UI

        /// <summary>
        /// Hiển thị splash screen
        /// </summary>
        private void ShowSplashScreen(Form parentForm, string caption, string description)
        {
            if (parentForm == null)
                return;

            try
            {
                SplashScreenManager.ShowForm(parentForm, typeof(WaitFormLoading), true, true, false);
                SplashScreenManager.Default?.SetWaitFormCaption(caption);
                SplashScreenManager.Default?.SetWaitFormDescription(description);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không thể hiển thị splash screen");
            }
        }

        /// <summary>
        /// Đóng splash screen
        /// </summary>
        private void CloseSplashScreen(bool isVisible)
        {
            if (!isVisible)
                return;

            try
            {
                if (SplashScreenManager.Default != null && SplashScreenManager.Default.IsSplashFormVisible)
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không thể đóng splash screen");
            }
        }

        /// <summary>
        /// Đóng splash screen đúng 1 lần: các lần gọi sau bị bỏ qua nhờ flag truyền theo ref.
        /// Dùng khi một luồng xử lý có nhiều nhánh thoát (catch/finally) cùng muốn đóng splash.
        /// </summary>
        private void CloseSplashScreenOnce(ref bool isVisible)
        {
            if (!isVisible)
                return;
            isVisible = false;
            CloseSplashScreen(true);
        }

        /// <summary>
        /// Hiển thị thông báo thành công
        /// </summary>
        private void ShowSuccessMessage(string message)
        {
            XtraMessageBox.Show(
                this,
                message,
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        /// <summary>
        /// Hiển thị thông báo lỗi
        /// </summary>
        private void ShowErrorMessage(string title, string message)
        {
            XtraMessageBox.Show(
                this,
                message,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        /// <summary>
        /// Hiển thị thông báo cảnh báo
        /// </summary>
        private void ShowWarningMessage(string title, string message)
        {
            XtraMessageBox.Show(
                this,
                message,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        #endregion
    }
}
