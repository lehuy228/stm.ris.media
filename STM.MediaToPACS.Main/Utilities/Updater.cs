using Serilog;
using System;
using System.Threading.Tasks;
using Velopack;
using Velopack.Sources;

namespace STM.MediaToPACS.Main.Utilities
{
    public static class Updater
    {
        public static async Task<bool> CheckAndUpdate(Action<string> onStatus = null, Action<int> onProgress = null)
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

                onStatus?.Invoke("Đang kiểm tra cập nhật...");
                var updateInfo = await mgr.CheckForUpdatesAsync();
                if (updateInfo == null) return false; // đã là bản mới nhất

                onStatus?.Invoke("Đang chờ xác nhận cập nhật...");
                var result = DevExpress.XtraEditors.XtraMessageBox.Show(
                    $"Có bản cập nhật mới {updateInfo.TargetFullRelease.Version}. Bạn có muốn cập nhật không?",
                    "Cập nhật",
                    System.Windows.Forms.MessageBoxButtons.YesNo,
                    System.Windows.Forms.MessageBoxIcon.Question);

                if (result != System.Windows.Forms.DialogResult.Yes) return false;

                onStatus?.Invoke("Đang tải bản cập nhật...");
                await mgr.DownloadUpdatesAsync(updateInfo, progress =>
                {
                    onProgress?.Invoke(progress);
                    onStatus?.Invoke($"Đang tải bản cập nhật... {progress}%");
                });

                onStatus?.Invoke("Đang cài đặt và khởi động lại...");
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