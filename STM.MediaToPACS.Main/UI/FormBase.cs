using MediaToPacs.Core.Auths;
using MediaToPacs.Infrastructure.Auths;
using STM.MediaToPACS.Main.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STM.MediaToPACS.Main.UI
{
    public partial class FormBase : DevExpress.XtraEditors.XtraForm
    {
        protected IPermissionService PermissionService;

        public FormBase()
        {
            InitializeComponent();
            if (!IsInDesignMode())
            {
                PermissionService = new PermissionService(ServiceLocator.SessionService);
            }
        }

        protected void SetControlPermission(Control control, string requiredPermission)
        {
            if (!IsInDesignMode() && !PermissionService.HasPermission(requiredPermission))
            {
                control.Enabled = false;
            }
        }

        protected bool IsInDesignMode()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime ||
                   System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv";
        }
    }

}
