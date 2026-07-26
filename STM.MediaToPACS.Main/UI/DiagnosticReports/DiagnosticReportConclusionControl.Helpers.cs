using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using STM.MediaToPACS.Main.Utilities;
using Serilog;

namespace STM.MediaToPACS.Main.UI.DiagnosticReports
{
    /// <summary>Helper hiển thị splash/thông báo - copy nguyên từ FrmMain.Helpers.cs.</summary>
    public partial class DiagnosticReportConclusionControl
    {
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

        private void CloseSplashScreen(bool isVisible)
        {
            if (!isVisible)
                return;

            try
            {
                if (SplashScreenManager.Default != null && SplashScreenManager.Default.IsSplashFormVisible)
                    SplashScreenManager.CloseForm(false);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không thể đóng splash screen");
            }
        }

        private void CloseSplashScreenOnce(ref bool isVisible)
        {
            if (!isVisible)
                return;
            isVisible = false;
            CloseSplashScreen(true);
        }

        private void ShowErrorMessage(string title, string message)
        {
            XtraMessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowWarningMessage(string title, string message)
        {
            XtraMessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
