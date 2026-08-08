using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using STM.MediaToPACS.Main.Utilities;
using Serilog;
using System;
using System.IO;
using Velopack;

namespace STM.MediaToPACS.Main
{
    public class Program
    {
        public static bool IsAuthencation = false;

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            VelopackApp.Build().Run();

            // Đảm bảo HTTP Listener của đăng nhập luôn được giải phóng khi tiến trình thoát,
            // tránh giữ port callback cho lần khởi động sau
            AppDomain.CurrentDomain.ProcessExit += (s, e) => STM.MediaToPacs.Connection.AuthSDK.Token.Cancel();

            string logFolder = Path.Combine(ServiceLocator.GetAppDataBasePath(), "Logs");
            Directory.CreateDirectory(logFolder);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug() // ghi từ mức Debug trở lên
                .WriteTo.File(
                    Path.Combine(logFolder, "log-.txt"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 10,
                    encoding: System.Text.Encoding.UTF8)
                .WriteTo.Debug()
                .CreateLogger();


            try
            {
                bool bConfigure = ReadCommandLine(args);
                if (bConfigure)
                    return;
            }
            catch { }

            ServiceLocator.Initialize();
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            UserLookAndFeel.Default.SetSkinStyle("McSkin");
            ServiceLocator.Initialize();


            System.Windows.Forms.Application.Run(new AppContextWithAuth());
        }
        static bool ReadCommandLine(string[] args)
        {
            return false;
        }
    }

}

