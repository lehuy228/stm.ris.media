using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STM.MediaToPACS.Main.Utilities
{
    public partial class WaitFormLoading : DevExpress.XtraWaitForm.WaitForm
    {
        public WaitFormLoading()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.AllowTransparency = true;
            this.StartPosition = FormStartPosition.CenterParent;

            progressPanel1.ContentAlignment = ContentAlignment.MiddleCenter;
            progressPanel1.Caption = "Đang tải dữ liệu...";
            progressPanel1.Description = "Vui lòng chờ trong giây lát...";
        }

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
        }

        public override void SetDescription(string description)
        {
            base.SetDescription(description);
        }
    }
}
