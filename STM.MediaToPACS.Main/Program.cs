using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using Leadtools.Demos;
using Leadtools.DicomDemos;
using STM.MediaToPACS.Main.Utilities;
using Serilog;
using System;
using System.Configuration;
using System.IO;

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
            string logFolder = Path.Combine(ConfigurationManager.AppSettings["File:BasePath"], "Logs");
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


            Support.SetLicense();
#if LEADTOOLS_V175_OR_LATER
            Support.SetLicense();
#else
                                 Support.Unlock(false);
#endif

            try
            {
                bool bConfigure = ReadCommandLine(args);
                if (bConfigure)
                    return;
            }
            catch { }



            //if (Support.KernelExpired)
            //    return;

            if (args.Length > 0)
            {
                FrmMain.StartedPrinter = args[0];
                MySettings mySettings = new MySettings();
                mySettings.Load();
                if (FrmMain.StartedPrinter != mySettings._settings.printerName)
                    return;
            }
            ServiceLocator.Initialize();
            Utils.EngineStartup();
            Utils.DicomNetStartup();
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

