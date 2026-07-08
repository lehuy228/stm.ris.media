using System;
using System.Windows.Forms;

namespace STM.MediaToPACS.Main.UI
{
    public partial class FrmSplash : Form
    {
        public FrmSplash()
        {
            InitializeComponent();

            try
            {
                lblVersion.Text = "Phiên bản " + Application.ProductVersion;
            }
            catch
            {
                lblVersion.Text = string.Empty;
            }
        }

        public void SetStatus(string text)
        {
            if (IsDisposed) return;

            if (InvokeRequired)
            {
                try { BeginInvoke(new Action<string>(SetStatus), text); } catch (ObjectDisposedException) { }
                return;
            }

            lblStatus.Text = text;
        }

        public void SetIndeterminate()
        {
            if (IsDisposed) return;

            if (InvokeRequired)
            {
                try { BeginInvoke(new Action(SetIndeterminate)); } catch (ObjectDisposedException) { }
                return;
            }

            progressBar.Style = ProgressBarStyle.Marquee;
        }

        public void SetProgress(int percent)
        {
            if (IsDisposed) return;

            if (InvokeRequired)
            {
                try { BeginInvoke(new Action<int>(SetProgress), percent); } catch (ObjectDisposedException) { }
                return;
            }

            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.Value = Math.Max(0, Math.Min(100, percent));
        }

        public void CloseSplash()
        {
            if (IsDisposed) return;

            if (InvokeRequired)
            {
                try { BeginInvoke(new Action(CloseSplash)); } catch (ObjectDisposedException) { }
                return;
            }

            Close();
        }
    }
}
