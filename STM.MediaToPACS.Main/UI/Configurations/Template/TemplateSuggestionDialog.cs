using System.Windows.Forms;
using DevExpress.XtraEditors;
using MediaToPacs.Core.Interfaces;
using STM.MediaToPACS.Main.Utilities;

namespace STM.MediaToPACS.Main.UI.Configurations
{
    public sealed class TemplateSuggestionDialog : XtraForm
    {
        public TemplateSuggestionDialog(IRisService risService)
        {
            Text = "Mẫu và gợi ý";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            WindowState = FormWindowState.Maximized;

            var manager = new ReportTemplateManager(risService)
            {
                Dock = DockStyle.Fill
            };

            Controls.Add(manager);
        }
    }
}
