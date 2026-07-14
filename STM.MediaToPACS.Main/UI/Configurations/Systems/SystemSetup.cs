using STM.MediaToPACS.Main.UI;

namespace STM.MediaToPACS.Main.UI.Configurations.Systems
{
    /// <summary>
    /// Màn hình cấu hình cục bộ của máy trạm.
    /// Cấu hình update và tài khoản PACS được quản lý tập trung qua RIS V1 API.
    /// </summary>
    public partial class SystemSetup : DevExpress.XtraEditors.XtraForm
    {
        public SystemSetup()
        {
            InitializeComponent();

            var settingsControl = new DoctorQuickSettingsControl();
            settingsControl.CloseRequested += (sender, args) => Close();
            contentPanel.Controls.Add(settingsControl);
        }
    }
}
