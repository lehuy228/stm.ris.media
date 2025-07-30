using Leadtools.Demos;
using Leadtools.DicomDemos;
using PrintToPACS.Utilities;
using PrintToPACSDemo.UI;
using PrintToPACSDemo.UI.Login;
using System;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;

namespace PrintToPACSDemo
{
    public class Program
    {
        public FrmMain _FrmMain;

        public static bool IsAuthencation = false;

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static async Task Main(string[] args)
        {
            try
            {
                bool bConfigure = ReadCommandLine(args);
                if (bConfigure)
                    return;
            }
            catch { }

#if LEADTOOLS_V175_OR_LATER
            Support.SetLicense();
#else
         Support.Unlock(false);
#endif

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

            Utils.EngineStartup();
            Utils.DicomNetStartup();
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new SplashAuthForm());
        }
        static bool ReadCommandLine(string[] args)
        {
            return false;
        }
    }

}
