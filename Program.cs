using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using Leadtools.Demos;
using Leadtools.DicomDemos;
using MediaToPacs.Core.Models;
using PrintToPACS.Utilities;
using PrintToPACSDemo.Utilities;
using System;

namespace PrintToPACSDemo
{
    public class Program
    {
        private static void EnsureDefaults(AppSettings settings)
        {
            bool changed = false;

            if (settings.CameraSettings == null)
            {
                settings.CameraSettings = new CameraSettings();
                changed = true;
            }

            if (settings.ShortcutSettings == null)
            {
                settings.ShortcutSettings = new ShortcutSettings();
                changed = true;
            }

            if (changed)
                AppSettingsLoader.Save(settings);
        }

        public static bool IsAuthencation = false;

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
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

            var appSettings = AppSettingsLoader.Load();
            EnsureDefaults(appSettings);

            Utils.EngineStartup();
            Utils.DicomNetStartup();
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            BonusSkins.Register();
            SkinManager.EnableFormSkins(); 
            UserLookAndFeel.Default.SetSkinStyle("McSkin"); 

            System.Windows.Forms.Application.Run(new AppContextWithAuth());
        }
        static bool ReadCommandLine(string[] args)
        {
            return false;
        }
    }

}

