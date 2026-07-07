using DevExpress.XtraSplashScreen;
using Serilog;
using Squirrel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STM.MediaToPACS.Main.Utilities
{
    public static class Updater
    {
        public static async Task<bool> CheckAndUpdate()
        {
            try
            {
                var systemConfig = ServiceLocator.SystemConfig;
                string updatePath = systemConfig?.UrlSystemUpdate;

                if (systemConfig == null || string.IsNullOrWhiteSpace(updatePath))
                {
                    Log.Warning("Chưa cấu hình UrlSystemUpdate - bỏ qua kiểm tra cập nhật.");
                    return false;
                }

                var psi = new ProcessStartInfo("net", $@"use {updatePath} /user:{systemConfig.SystemUpdateUser} {systemConfig.SystemUpdatePassword}")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (var proc = Process.Start(psi))
                {
                    if (proc == null)
                    {
                        Log.Error("Không thể khởi tạo process.");
                        throw new InvalidOperationException("Không thể khởi tạo process.");
                    }

                    // Đọc async để tránh deadlock nếu output lớn
                    var outputReader = proc.StandardOutput;
                    var errorReader = proc.StandardError;

                    // Wait for exit with timeout
                    if (!proc.WaitForExit(10000))
                    {
                        try { proc.Kill(); } catch { }
                        Log.Error("Command 'net use' timed out.");
                        throw new TimeoutException("Command 'net use' timed out.");
                    }

                    // Sau khi exit, đọc toàn bộ output
                    string stdout = outputReader.ReadToEnd();
                    string stderr = errorReader.ReadToEnd();
                    int exitCode = proc.ExitCode;

                    // Kiểm tra: exitCode == 0 thường là OK, nhưng đọc stdout để chắc chắn
                    bool success = exitCode == 0 &&
                                   (stdout.IndexOf("The command completed successfully", StringComparison.OrdinalIgnoreCase) >= 0
                                    || stdout.IndexOf("Reconnected", StringComparison.OrdinalIgnoreCase) >= 0
                                    || string.IsNullOrWhiteSpace(stderr));

                    // Bạn có thể log stdout/stderr để debug
                    Log.Information("net stdout: " + stdout);
                    Log.Information("net stderr: " + stderr);
                    Log.Information("net exitCode: " + exitCode);

                    if (success)
                    {

                        using (var mgr = new UpdateManager(updatePath))
                        {
                            var updateInfo = await mgr.CheckForUpdate();

                            if (updateInfo.ReleasesToApply.Any())
                            {
                                var latest = updateInfo.ReleasesToApply.Last();

                                var result = DevExpress.XtraEditors.XtraMessageBox.Show(
                                    $"Có bản cập nhật mới {latest.Version}. Bạn có muốn cập nhật không?",
                                    "Cập nhật",
                                    System.Windows.Forms.MessageBoxButtons.YesNo,
                                    System.Windows.Forms.MessageBoxIcon.Question);

                                if (result == System.Windows.Forms.DialogResult.Yes)
                                {
                                    SplashScreenManager.ShowDefaultWaitForm("Đang cập nhật...", "Xin chờ trong giây lát");
                                    await mgr.UpdateApp();
                                    SplashScreenManager.CloseDefaultWaitForm();

                                    DevExpress.XtraEditors.XtraMessageBox.Show(
                                        "Ứng dụng đã được cập nhật. Vui lòng khởi động lại.",
                                        "Thông báo");

                                    return true;
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error($"Updater error: {ex.Message}");
            }

            return false;
        }
    }

}
