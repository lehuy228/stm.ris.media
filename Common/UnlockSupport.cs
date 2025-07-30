using System;
using System.IO;
using System.Windows.Forms;

using Leadtools;

namespace Leadtools.Demos
{
    public static class Support
    {
        public const string MedicalServerKey = "";

        public static bool KernelExpired
        {
            get
            {
                if (RasterSupport.KernelExpired)
                {
                    // Try Default LIC
                    try
                    {
                        string licenseFilePath = System.IO.Path.Combine(Application.StartupPath, "LEADTOOLS.LIC");
                        string developerKey = System.IO.File.ReadAllText(System.IO.Path.Combine(Application.StartupPath, "LEADTOOLS.LIC.KEY"));
                        RasterSupport.SetLicense(licenseFilePath, developerKey);
                    }
                    catch { System.Diagnostics.Debug.WriteLine("Default LIC failed!"); }

                    if (RasterSupport.KernelExpired)
                    {
                        MessageBox.Show(
                        null,
                        "Your license file is missing, invalid or expired. LEADTOOLS will not function. Please contact LEAD Sales for information on obtaining a valid license.",
                        "No LEADTOOLS License",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                        System.Diagnostics.Process.Start("https://www.leadtools.com/downloads/evaluation-form.asp?evallicenseonly=true");
                        return true;
                    }
                    return false;
                }
                return false;
            }
        }

        public static void SetLicense()
        {
            try
            {
                //Uncomment this and add your license file and developer key
                //string licenseFilePath = "Replace this with the path to the LEADTOOLS license file";
                //string developerKey = "Replace this with your developer key";
                //RasterSupport.SetLicense(licenseFilePath, developerKey);
                string exeFolder = AppDomain.CurrentDomain.BaseDirectory;
                var key = File.ReadAllText($"{exeFolder}\\full_license.key");
                var lic = File.ReadAllBytes($"{exeFolder}\\full_license.lic");
                RasterSupport.SetLicense(lic, key);
                if (RasterSupport.KernelExpired)
                {
                    System.Diagnostics.Debugger.Log(0, null, "*******************************************************************************" + Environment.NewLine);
                    System.Diagnostics.Debugger.Log(0, null, "*** NOTE: Your license file is missing, invalid or expired. LEADTOOLS will not function. Please contact LEAD Sales for information on obtaining a valid license.***" + Environment.NewLine);
                    System.Diagnostics.Debugger.Log(0, null, "*******************************************************************************" + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
            }
        }
    }
}
