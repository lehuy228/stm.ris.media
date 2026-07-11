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
        #region Event ToolStripMenu

        private void _tsmViewPACS_Click(object sender, EventArgs e)
        {
            if (_machidinh == null) return;

            string url = ServiceLocator.SystemConfig.UrlPacsPublic + _machidinh;

            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch
            {
                MessageBox.Show("Không mở được trình duyệt!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void _tsmDeletePACS_Click(object sender, EventArgs e)
        {
            string machidinh = _machidinh;

            if (string.IsNullOrWhiteSpace(machidinh))
            {
                MessageBox.Show(
                    "Không tìm thấy mã chỉ định để hủy liên kết PACS.",
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn hủy liên kết PACS với mã chỉ định:\n{machidinh} ?",
                this.Text,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                int count = await ServiceLocator.StudyService.DeleteStudyAsync(machidinh);
                if (count > 0)
                {
                    MessageBox.Show(
                        $"Hủy liên kết PACS thành công.\nSố Study đã cập nhật: {count}",
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        "Không tìm thấy Study nào tương ứng với mã chỉ định.",
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Có lỗi xảy ra khi hủy liên kết PACS.\n" + ex.Message,
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void _tsmEditPatient_Click(object sender, EventArgs e)
        {
            try
            {
                using (var patientForm = new PatientForm(
                    _chiDinhDichVuResponse.BenhNhan,
                    _chiDinhDichVuResponse.MaChiDinh))
                {
                    if (patientForm.ShowDialog() == DialogResult.OK)
                    {
                        PopulatePatientInfo(_chiDinhDichVuResponse.BenhNhan);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        private async void _tsmAsyncHis_Click(object sender, EventArgs e)
        {
            try
            {
                var result = await ServiceLocator.RisService.SendKetQuaChanDoanToHisAsync(_machidinh);

                if (result == null)
                {
                    ShowWarningMessage("Cảnh báo", "Không gửi được kết quả sang HIS.");
                    return;
                }

                ShowSuccessMessage("Đã gửi kết quả chẩn đoán sang HIS thành công.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Lỗi khi gửi kết quả chẩn đoán sang HIS cho MaChiDinh: {_machidinh}");
            }
        }

        #endregion
    }
}
