using DevExpress.XtraSplashScreen;
using Serilog;
using System;
using System.Threading.Tasks;
using Velopack;
using Velopack.Sources;

namespace STM.MediaToPACS.Main.Utilities
{
    public static class Updater
    {
        public static async Task<bool> CheckAndUpdate()
        {
            try
            {
                // URL: link repo GitHub,https://github.com/your-org/your-repo
                string repoUrl = ServiceLocator.SystemConfig?.UrlSystemUpdate;
                // Token: PAT fine-grained (read-only)
                string token = ServiceLocator.SystemConfig?.SystemUpdatePassword;

                if (string.IsNullOrWhiteSpace(repoUrl))
                {
                    Log.Information("Chưa cấu hình URL cập nhật, bỏ qua.");
                    return false;
                }

                var mgr = new UpdateManager(new GithubSource(repoUrl, token, prerelease: false));

                // Chạy từ Visual Studio (bin\Debug) thì không phải bản đã cài -> bỏ qua
                if (!mgr.IsInstalled)
                {
                    Log.Information("App không chạy từ bản cài đặt Velopack, bỏ qua update.");
                    return false;
                }

                var updateInfo = await mgr.CheckForUpdatesAsync();
                if (updateInfo == null) return false; // đã là bản mới nhất

                var result = DevExpress.XtraEditors.XtraMessageBox.Show(
                    $"Có bản cập nhật mới {updateInfo.TargetFullRelease.Version}. Bạn có muốn cập nhật không?",
                    "Cập nhật",
                    System.Windows.Forms.MessageBoxButtons.YesNo,
                    System.Windows.Forms.MessageBoxIcon.Question);

                if (result != System.Windows.Forms.DialogResult.Yes) return false;

                SplashScreenManager.ShowDefaultWaitForm("Đang cập nhật...", "Xin chờ trong giây lát");
                await mgr.DownloadUpdatesAsync(updateInfo);
                SplashScreenManager.CloseDefaultWaitForm();

                mgr.ApplyUpdatesAndRestart(updateInfo); // tự tắt app, cài và mở lại
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"Updater error: {ex.Message}");
                return false;
            }
        }
    }
}