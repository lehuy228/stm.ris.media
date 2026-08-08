using System.Windows.Forms;
using DevExpress.XtraEditors;
using STM.MediaToPACS.Main.Utilities;

namespace STM.MediaToPACS.Main.UI.Configurations
{
    public sealed class TemplateSuggestionDialog : XtraForm
    {
        public TemplateSuggestionDialog()
        {
            Text = "Mẫu và gợi ý";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            WindowState = FormWindowState.Maximized;

            var manager = new ReportTemplateManager
            {
                Dock = DockStyle.Fill
            };

            Controls.Add(manager);
        }
    }
}
