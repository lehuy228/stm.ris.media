using DevExpress.XtraEditors;
using MediaToPacs.Core.Models;
using STM.MediaToPACS.Main.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STM.MediaToPACS.Main.UI.Configurations.Systems
{
    public partial class SystemSetup : DevExpress.XtraEditors.XtraForm
    {
        private SystemConfig _systemConfig { get; set; }
        private readonly string _configFilePath;
        private readonly string _modalityFilePath;
        private Modalities _modalities = new Modalities();

        public SystemSetup()
        {
            InitializeComponent();

            // Xác định đường dẫn file config
            string basePath = ConfigurationManager.AppSettings["File:BasePath"];
            string configFileName = ConfigurationManager.AppSettings["SystemConfigFile"] ?? "SystemConfig.xml";
            _modalityFilePath = Path.Combine(basePath, ConfigurationManager.AppSettings["Modality"] ?? "Modalities.xml");
            _configFilePath = Path.Combine(basePath, configFileName);
            LoadConfig();
        }

        /// <summary>
        /// Load config từ file XML
        /// </summary>
        private void LoadConfig()
        {
            try
            {
                _systemConfig = XmlSettingsHelper.LoadEncrypted<SystemConfig>(_configFilePath);

                // Nếu chưa có config, tạo mới
                if (_systemConfig == null)
                {
                    _systemConfig = new SystemConfig();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi load SystemConfig");
                _systemConfig = new SystemConfig();
            }
        }

        /// <summary>
        /// Load dữ liệu từ config vào UI
        /// </summary>
        private void SystemSetup_Load(object sender, EventArgs e)
        {
            try
            {
                if (_systemConfig == null)
                {
                    _systemConfig = new SystemConfig();
                    return;
                }

                // Load API RIS config
                _txUrlApiRis.Text = _systemConfig.UrlApiRis ?? "";
                _txUrlApiRisAuthen.Text = _systemConfig.UrlRisAuthen ?? "";
                _txUrlKySoMySign.Text = _systemConfig.UrlSignatureMysign ?? "";
                // Load PACS config
                _txUrlViewerPACS.Text = _systemConfig.UrlViewerPacs ?? "";
                _txUrlTokenPACS.Text = _systemConfig.UrlTokenPacs ?? "";
                _txUrlPACSServer.Text = _systemConfig.UrlPacsServer ?? "";
                _txUrlPACSUser.Text = _systemConfig.PacsUser ?? "";
                _txUrlPACSPassword.Text = _systemConfig.PacsPassword ?? "";
                _txUrlPACSPublic.Text = _systemConfig.UrlPacsPublic ?? "";

                // Load System Update config
                _txUrlSystemUpdate.Text = _systemConfig.UrlSystemUpdate ?? "";
                _txSystemUpdateUser.Text = _systemConfig.SystemUpdateUser ?? "";
                _txSystemUpdatePassword.Text = _systemConfig.SystemUpdatePassword ?? "";

                // Load other settings (nếu có control _txCheckThanhToan)

                _txCheckThanhToan.Text = _systemConfig.CheckThanhToan ?? "";

                LoadModalities();

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi load dữ liệu vào UI");
                XtraMessageBox.Show(
                    this,
                    $"Lỗi khi tải cấu hình: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void LoadModalities()
        {
            try
            {
                _modalities = XmlSettingsHelper.Load<Modalities>(_modalityFilePath);
                if(_modalities == null)
                {
                    _modalities = new Modalities();
                }
                _richModality.Text = _modalities?.ModalitiesList ?? "";
            }
            catch (Exception ex)
            {
                Log.Error("Lỗi load file xml modalities", ex.Message);
            }
        }

        /// <summary>
        /// Lưu cấu hình từ UI vào file
        /// </summary>
        private void _btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                // Lưu API RIS config
                _systemConfig.UrlApiRis = _txUrlApiRis.Text?.Trim();
                _systemConfig.UrlRisAuthen = _txUrlApiRisAuthen.Text?.Trim();
                _systemConfig.UrlSignatureMysign = _txUrlKySoMySign.Text?.Trim();

                // Lưu PACS config
                _systemConfig.UrlViewerPacs = _txUrlViewerPACS.Text?.Trim();
                _systemConfig.UrlTokenPacs = _txUrlTokenPACS.Text?.Trim();
                _systemConfig.UrlPacsServer = _txUrlPACSServer.Text?.Trim();
                _systemConfig.PacsUser = _txUrlPACSUser.Text?.Trim();
                _systemConfig.PacsPassword = _txUrlPACSPassword.Text?.Trim();
                _systemConfig.UrlPacsPublic = _txUrlPACSPublic.Text?.Trim();

                // Lưu System Update config
                _systemConfig.UrlSystemUpdate = _txUrlSystemUpdate.Text?.Trim();
                _systemConfig.SystemUpdateUser = _txSystemUpdateUser.Text?.Trim();
                _systemConfig.SystemUpdatePassword = _txSystemUpdatePassword.Text?.Trim();

                // Lưu other settings (nếu có)
                _systemConfig.CheckThanhToan = _txCheckThanhToan.Text?.Trim();

                ServiceLocator.SystemConfig = _systemConfig;
                ServiceLocator.InitializeOptionalServices();

                SaveModality();
                // Lưu vào file
                XmlSettingsHelper.SaveEncrypted(_configFilePath, _systemConfig);

                XtraMessageBox.Show(
                    this,
                    "Lưu cấu hình thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi lưu SystemConfig");
                XtraMessageBox.Show(
                    this,
                    $"Lỗi khi lưu cấu hình: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void SaveModality()
        {
            _modalities.ModalitiesList = _richModality.Text.ToUpper();
            XmlSettingsHelper.Save<Modalities>(_modalityFilePath, _modalities);
        }

        /// <summary>
        /// Validate dữ liệu đầu vào
        /// </summary>
        private bool ValidateInput()
        {
            // Có thể thêm validation ở đây nếu cần
            // Ví dụ: kiểm tra URL hợp lệ, required fields, etc.

            return true;
        }

        /// <summary>
        /// Hủy và đóng form
        /// </summary>
        private void _btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void _btnTestApiThanhToan_Click(object sender, EventArgs e)
        {
            _btnTestApiThanhToan.Enabled = false;
            try
            {
                var urlApi = _txCheckThanhToan.Text?.Trim();
                var maChiDinh = _txMaChiDinh.Text?.Trim();

                if (string.IsNullOrEmpty(urlApi))
                {
                    ShowWarningMessage("Thiếu cấu hình", "Chưa cấu hình URL kiểm tra thanh toán.");
                    return;
                }

                if (string.IsNullOrEmpty(maChiDinh))
                {
                    ShowWarningMessage("Thiếu dữ liệu", "Chưa nhập mã chỉ định.");
                    return;
                }

                bool duTien = await ServiceLocator.HisService.KiemTraDuTienAsync(urlApi, maChiDinh);

                if (duTien)
                    ShowSuccessMessage("Bệnh nhân đã thanh toán đủ tiền.");
                else
                    ShowWarningMessage("Thông báo", "Bệnh nhân chưa thanh toán đủ tiền.");
            }
            catch (HttpRequestException ex)
            {
                ShowErrorMessage("Lỗi kết nối", $"Không kết nối được tới hệ thống thanh toán.\n{ex.Message}");
            }
            catch (TaskCanceledException)
            {
                ShowErrorMessage("Timeout", "Hệ thống thanh toán phản hồi quá chậm.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi", $"Lỗi không xác định:\n{ex.Message}");
            }
            finally
            {
                _btnTestApiThanhToan.Enabled = true;
            }
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

        private void ShowSuccessMessage(string message)
        {
            XtraMessageBox.Show(
                this,
                message,
                null,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }


    }
}
