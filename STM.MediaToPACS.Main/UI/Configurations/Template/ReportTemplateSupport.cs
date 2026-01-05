using DevExpress.XtraEditors;
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
    public partial class ReportTemplateSupport : XtraForm
    {
        public string ReportName { get; private set; }
        public string Modality { get; private set; }

        public ReportTemplateSupport(string name = null, string modality = null)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(name))
            {
                _txNameReport.Text = name;
            }
            if (!string.IsNullOrEmpty(modality))
            {
                _cbbModality.Text = modality;
            }
        }

        private void _btnContinue_Click(object sender, EventArgs e)
        {
            ReportName = _txNameReport.Text.Trim();
            Modality = _cbbModality.Text.Trim();

            if (string.IsNullOrEmpty(ReportName) || string.IsNullOrEmpty(Modality))
            {
                XtraMessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void _btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }

}

