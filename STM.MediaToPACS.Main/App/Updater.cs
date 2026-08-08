using MediaToPacs.Core.Interfaces;
using Serilog;
using System;
using System.Threading.Tasks;
using Velopack;
using Velopack.Sources;

namespace STM.MediaToPACS.Main.Utilities
{
    public static class Updater
    {
        public static async Task<bool> CheckAndUpdate(
            IRisService2 risService2, Action<string> onStatus = null, Action<int> onProgress = null)
        {
            try
            {
                onStatus?.Invoke("Đang tải cấu hình cập nhật...");
                var updateConfig = await risService2.GetSystemUpdateConfigAsync();
                if (updateConfig == null || !updateConfig.HasUpdateConfiguration)
                {
                    Log.Information("API không có cấu hình cập nhật hệ thống, bỏ qua kiểm tra cập nhật.");
                    onStatus?.Invoke("Không có cấu hình cập nhật, tiếp tục đăng nhập...");
                    return false;
                }

                string repoUrl = updateConfig.Link.Trim();
                string token = string.IsNullOrWhiteSpace(updateConfig.Token)
                    ? null
                    : updateConfig.Token.Trim();

                // Chỉ giữ tài khoản PACS trong phiên chạy, không ghi xuống SystemConfig.xml.
                if (ServiceLocator.SystemConfig != null)
                {
                    ServiceLocator.SystemConfig.PacsUser = updateConfig.PacsUsername;
                    ServiceLocator.SystemConfig.PacsPassword = updateConfig.PacsPassword;
                }

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
                    onStatus?.Invoke("Bản chạy thử không hỗ trợ tự cập nhật, tiếp tục đăng nhập...");
                    return false;
                }

                onStatus?.Invoke("Đang kiểm tra cập nhật...");
                var updateInfo = await mgr.CheckForUpdatesAsync();
                if (updateInfo == null)
                {
                    onStatus?.Invoke("Ứng dụng đang ở phiên bản mới nhất.");
                    return false;
                }

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
                Log.Error(ex, "Không thể kiểm tra cập nhật; ứng dụng sẽ tiếp tục chạy bình thường");
                onStatus?.Invoke("Không thể kiểm tra cập nhật, tiếp tục khởi động...");
                return false;
            }
        }
    }
}
