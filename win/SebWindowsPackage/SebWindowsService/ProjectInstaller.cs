using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.IO;
using System.IO.Packaging;
using Ionic.Zip;



namespace SebWindowsService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {

        public ProjectInstaller()
        {
            InitializeComponent();
        }


        private string GetCommonDesktopDirectory()
        {
            // The common desktop directory (containîng the program shortcuts for all users)
            // has changed between Windows XP and Windows Vista.
            // As usual, Microsoft annoys the developers by not providing a
            // System.Environment.SpecialFolder variable for the common desktop.
            // The variable DesktopDirectory only points the the current user's desktop,
            // the variable CommonApplicationDirectory points to the common program data.
            // So we must construct the common desktop directory, depending on the Windows version.

            // Determine the major Windows version (actually "Windows NT" version).
            // Windows NT version <= 5 : Windows NT 4.0,..., XP
            // Windows NT version >= 6 : Windows Vista, 7, 8...
            OperatingSystem operatingSystem = Environment.OSVersion;
            Version         version         = operatingSystem.Version;
            int             versionMajor    = version.Major;

            // Build the common desktop directory, depending on the Windows version.
            // Windows NT version <= 5 : "C:\Documents and Settings\All Users\Desktop"
            // Windows NT version >= 6 : "C:\Users\Public\Desktop"
            string CommonDesktopDir = "";
            string      AllUsersDir = Environment.GetEnvironmentVariable("ALLUSERSPROFILE");
            string        PublicDir = Environment.GetEnvironmentVariable("PUBLIC");
            if (versionMajor <  6) CommonDesktopDir = AllUsersDir + "\\" + "Desktop";
            if (versionMajor >= 6) CommonDesktopDir =   PublicDir + "\\" + "Desktop";

/*
            // Write some debug data into a file
            string UserDesktopDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string UserDebugFile  = UserDesktopDir + "\\" + "WindowsVersion.txt";
            using (StreamWriter sw = new StreamWriter(UserDebugFile))
            {
                sw.WriteLine();
                sw.WriteLine("operatingSystem  = " + operatingSystem);
                sw.WriteLine("platform         = " + operatingSystem.Platform);
                sw.WriteLine("version          = " + operatingSystem.Version);
                sw.WriteLine("versionString    = " + operatingSystem.VersionString);
                sw.WriteLine("version.Major    = " + operatingSystem.Version.Major);
                sw.WriteLine();
                sw.WriteLine("     AllUsersDir = " +      AllUsersDir);
                sw.WriteLine("       PublicDir = " +        PublicDir);
                sw.WriteLine("CommonDesktopDir = " + CommonDesktopDir);
                sw.WriteLine("  UserDesktopDir = " +   UserDesktopDir);
                sw.WriteLine();
                sw.Flush();
            }
*/
            return CommonDesktopDir;
        }




        private void SebServiceInstaller_Committed(object sender, InstallEventArgs e)
        {
            // Unpack the XULRunner directories after installation

            string ProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string ProgramData  = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            string Manufacturer = "ETH Zuerich";
            string Product      = "SEB Windows";
            string Version      = "1.8.2";
            string Component    = "SebWindowsClient";
            string Build        = "Release";

            // Get the directory of the .msi installer file
            // and the directory of the target installation as CustomActionData.
            // To see where the "SourceDir" and "TargetDir" come from, look at:
            // Custom Actions window ->
            // Install and Commit phases ->
            // Primary output of SebWindowsService (Active) ->
            // Properties window ->
            // CustomActionData: /SourceDir="[SOURCEDIR]\" /TargetDir="[TARGETDIR]\" /Shortcut=[SHORTCUT]

            if (!this.Context.Parameters.ContainsKey("SourceDir"))
            {
                throw new Exception(string.Format("CustomAction SourceDir failed!"));
            }

            if (!this.Context.Parameters.ContainsKey("TargetDir"))
            {
                throw new Exception(string.Format("CustomAction TargetDir failed!"));
            }

            if (!this.Context.Parameters.ContainsKey("Shortcut"))
            {
                throw new Exception(string.Format("CustomAction Shortcut failed!"));
            }

            string SebSourceDir = this.Context.Parameters["SourceDir"];
            string SebTargetDir = this.Context.Parameters["TargetDir"];
            string SebShortcut  = this.Context.Parameters["Shortcut"];

            Boolean ShortcutDesired = (SebShortcut != string.Empty);

            // Write some debug data into a file
            string UserDesktopDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string UserDebugFile  = UserDesktopDir + "\\" + "ContextParameters.txt";

            using (StreamWriter sw = new StreamWriter(UserDebugFile))
            {
                int numParams    = this.Context.Parameters.Count;
                int numParamKeys = this.Context.Parameters.Keys.Count;

                sw.WriteLine();
                sw.WriteLine("this.Context.Parameters.Count      = " + numParams);
                sw.WriteLine("this.Context.Parameters.Keys.Count = " + numParamKeys);
                sw.WriteLine();

                foreach (string myString in Context.Parameters.Keys)
                {
                    sw.WriteLine("Context.Parameters[" + myString + "] = " + Context.Parameters[myString]);
                }

                sw.WriteLine();
                sw.WriteLine("SebSourceDir    = " + SebSourceDir);
                sw.WriteLine("SebTargetDir    = " + SebTargetDir);
                sw.WriteLine("SebShortcut     = " + SebShortcut);
                sw.WriteLine("ShortcutDesired = " + ShortcutDesired);
                sw.WriteLine();

                if (ShortcutDesired == true ) sw.WriteLine("ShortcutDesired = True");
                if (ShortcutDesired == false) sw.WriteLine("ShortcutDesired = False");

                sw.WriteLine();
                sw.Flush();
            }

            string SebBatchDir   = SebSourceDir;

            string SebConfigDir  = ProgramData  + "\\" + Manufacturer + "\\" + Product + " " + Version;
            string SebInstallDir = ProgramFiles + "\\" + Manufacturer + "\\" + Product + " " + Version;
            string SebClientDir  = ProgramFiles + "\\" + Manufacturer + "\\" + Product + " " + Version + "\\" + Component;
            string SebReleaseDir = ProgramFiles + "\\" + Manufacturer + "\\" + Product + " " + Version + "\\" + Component + "\\" + Build;

            string SebInstallMsi = "SebWindowsInstall.msi";
            string SebStarterExe = "SebStarter.exe";
            string SebStarterBat = "SebStarter.bat";
            string SebStarterIni = "SebStarter.ini";
            string SebMsgHookIni =    "MsgHook.ini";

            string SebInstallMsiFile = SebBatchDir + "\\" + SebInstallMsi;
            string SebStarterBatFile = SebBatchDir + "\\" + SebStarterBat;
            string SebStarterIniFile = SebBatchDir + "\\" + SebStarterIni;
            string SebMsgHookIniFile = SebBatchDir + "\\" + SebMsgHookIni;

            string SebStarterExeFileTarget = SebReleaseDir + "\\" + SebStarterExe;
            string SebStarterBatFileTarget = SebReleaseDir + "\\" + SebStarterBat;
            string SebStarterIniFileTarget = SebConfigDir  + "\\" + SebStarterIni;
            string SebMsgHookIniFileTarget = SebConfigDir  + "\\" + SebMsgHookIni;

            string CommonDesktopDirectory = GetCommonDesktopDirectory();
            string CommonDesktopIconUrl   =    CommonDesktopDirectory + "\\" + Product + " " + Version + ".url";

            string XulSebZip           = "xul_seb.zip";
            string XulRunnerZip        = "xulrunner.zip";
            string XulRunnerNoSslZip   = "xulrunner_no_ssl_warning.zip";
            string XulRunnerWithSslZip = "xulrunner_with_ssl_warning.zip";

            string XulSebZipFile           = SebClientDir + "\\" + XulSebZip;
            string XulRunnerZipFile        = SebClientDir + "\\" + XulRunnerZip;
            string XulRunnerNoSslZipFile   = SebClientDir + "\\" + XulRunnerNoSslZip;
            string XulRunnerWithSslZipFile = SebClientDir + "\\" + XulRunnerWithSslZip;


            // Extract all files from the "xul_seb.zip" file
            using (ZipFile zipFile = ZipFile.Read(XulSebZipFile))
            {
                foreach (ZipEntry zipEntry in zipFile)
                {
                    zipEntry.Extract(SebClientDir, ExtractExistingFileAction.OverwriteSilently);
                }
            }

            // Extract all files from the "xulrunner.zip" file
            using (ZipFile zipFile = ZipFile.Read(XulRunnerZipFile))
            {
                foreach (ZipEntry zipEntry in zipFile)
                {
                    zipEntry.Extract(SebClientDir, ExtractExistingFileAction.OverwriteSilently);
                }
            }

            // Extract all files from the "xulrunner_no_ssl.zip" file
            using (ZipFile zipFile = ZipFile.Read(XulRunnerNoSslZipFile))
            {
                foreach (ZipEntry zipEntry in zipFile)
                {
                    zipEntry.Extract(SebClientDir, ExtractExistingFileAction.OverwriteSilently);
                }
            }

            // Extract all files from the "xulrunner_with_ssl.zip" file
            using (ZipFile zipFile = ZipFile.Read(XulRunnerWithSslZipFile))
            {
                foreach (ZipEntry zipEntry in zipFile)
                {
                    zipEntry.Extract(SebClientDir, ExtractExistingFileAction.OverwriteSilently);
                }
            }


            // Copy the configured MsgHook.ini and SebStarter.ini files
            // to the configuration directory.
            // Overwrite the default .ini files previously installed there.
            // Additionally, copy the SebStarter.bat file to the installation directory.
            //
            // Note:
            // This copying can be deactivated when we use an Administrative Install.
            // But in any case, we have a two-phase scenario:
            //
            // 1st phase: the teacher configures the MsgHook.ini and SebStarter.ini
            // 2nd phase: the student executes the .msi file, which automatically uses
            // the .ini files configured (modified) by the teacher during 1st phase.

            //System.IO.File.Copy(SebMsgHookIniFile, SebMsgHookIniFileTarget, true);
            //System.IO.File.Copy(SebStarterIniFile, SebStarterIniFileTarget, true);
            //System.IO.File.Copy(SebStarterBatFile, SebStarterBatFileTarget, true);

            if (System.IO.File.Exists(SebMsgHookIniFile))
                System.IO.File.Copy  (SebMsgHookIniFile, SebMsgHookIniFileTarget, true);

            if (System.IO.File.Exists(SebStarterIniFile))
                System.IO.File.Copy  (SebStarterIniFile, SebStarterIniFileTarget, true);

            if (System.IO.File.Exists(SebStarterBatFile))
                System.IO.File.Copy  (SebStarterBatFile, SebStarterBatFileTarget, true);

/*
            try
            {
                System.IO.File.Copy(SebMsgHookIniFile, SebMsgHookIniFileTarget, true);
            }
            catch (Exception)
            {
                //throw;
            }

            try
            {
                System.IO.File.Copy(SebStarterIniFile, SebStarterIniFileTarget, true);
            }
            catch (Exception)
            {
                //throw;
            }

            try
            {
                System.IO.File.Copy(SebStarterBatFile, SebStarterBatFileTarget, true);
            }
            catch (Exception)
            {
                //throw;
            }
*/

            // If the user desired this, create a shortcut to the
            // program executable "SebStarter.exe" on the Common Desktop
            if    (ShortcutDesired)
            using (StreamWriter writer = new StreamWriter(CommonDesktopIconUrl))
            {
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=file:///" + SebStarterExeFileTarget);
                writer.WriteLine("IconIndex=0");
                string SebWindowsIcon = SebStarterExeFileTarget.Replace('\\', '/');
                writer.WriteLine("IconFile=" + SebWindowsIcon);
                writer.Flush();
            }

            // Autostart the SEB Windows Service after installation.
            // This avoids the necessity of a machine reboot.
            string sebServiceName       = this.SebServiceInstaller.ServiceName;
            var    sebServiceController = new ServiceController(sebServiceName);

            try
            {
                sebServiceController.Start();
            }
            catch (Exception)
            {
                //throw;
            }

            return;

        } // end of method   SebServiceInstaller_Committed()





        private void SebServiceInstaller_BeforeUninstall(object sender, InstallEventArgs e)
        {
            // Stop the SEB Windows Service before uninstallation ???
            // Seems to be unnecessary since the Uninstaller automatically does this.
            // Even more: seems to be detrimental, since the Windows service is
            // sometimes NOT started after INSTALLATION then!!!
            // So due to current knowledge, do NOT use this code!!!
/*
            string sebServiceName       = this.SebServiceInstaller.ServiceName;
            var    sebServiceController = new ServiceController(sebServiceName);

            try
            {
                sebServiceController.Stop();
            }
            catch (Exception)
            {
                //throw;
            }
*/
            return;

        }   // end of method   SebServiceInstaller_BeforeUninstall()





        private void SebServiceInstaller_AfterUninstall(object sender, InstallEventArgs e)
        {
            // Delete all remaining directories and files after uninstallation

            string ProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string ProgramData  = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            string Manufacturer = "ETH Zuerich";
            string Product      = "SEB Windows";
            string Version      = "1.8.2";
            string Component    = "SebWindowsClient";
            string Build        = "Release";

            string Service      = "SebWindowsService";
            string Logfile      = "SebWindowsService.logfile.txt";

            string SebInstallDir = ProgramFiles + "\\" + Manufacturer + "\\" + Product + " " + Version;
            string SebConfigDir  = ProgramData  + "\\" + Manufacturer + "\\" + Product + " " + Version;

            string SebClientDir  = SebInstallDir + "\\" + Component;
            string SebReleaseDir = SebInstallDir + "\\" + Component + "\\" + Build;
            string SebServiceDir = SebInstallDir + "\\" + Service;
            string SebServiceLog = SebInstallDir + "\\" + Service + "\\" + Logfile;

            string CommonDesktopDirectory = GetCommonDesktopDirectory();
            string CommonDesktopIconUrl   =    CommonDesktopDirectory + "\\" + Product + " " + Version + ".url";


            // ATTENTION:
            //
            // Deleting the SEB configuration directory in the ProgramData directory, e.g.
            // C:\ProgramData\ETH Zuerich\SEB Windows 1.8.2 ,
            // mostly succeeds.
            //
            // Deleting the SEB installation directory in the Program Files directory, e.g.
            // C:\Program Files (x86)\ETH Zuerich\SEB Windows 1.8.2\SebWindowsClient\Release ,
            // mostly fails, even though its files have all been deleted before.
            //
            // This is a known and annoying Windows bug still occurring in Windows 7:
            // Often it is impossible to delete an empty (!) directory
            // "because a process is still needing it" (which is not true).
            //
            // Currently, only a user logoff or machine reboot solves this,
            // so maybe it is necessary to reboot and manually delete the
            // C:\Program Files (x86)\ETH Zuerich\SEB Windows 1.8.2\SebWindowsClient\Release
            // directory after reboot.


            // Stop the SEB Windows Service after uninstallation ???
            string sebServiceName       = this.SebServiceInstaller.ServiceName;
            var    sebServiceController = new ServiceController(sebServiceName);

            try
            {
                sebServiceController.Stop();
            }
            catch (Exception)
            {
                //throw;
            }


            // Try to delete the "SebWindowsClient\Release" subdirectory
            try
            {
                System.IO.Directory.Delete(SebReleaseDir, true);
            }
            catch (Exception)
            {
                //throw;
            }


            // Try to delete the "SebWindowsClient" subdirectory
            try
            {
                System.IO.Directory.Delete(SebClientDir, true);
            }
            catch (Exception)
            {
                //throw;
            }


            // Try to delete the "SebWindowsService.logfile.txt" file
            System.IO.File.Delete(SebServiceLog);


            // Try to delete the "SebWindowsService" subdirectory
            try
            {
                System.IO.Directory.Delete(SebServiceDir, true);
            }
            catch (Exception)
            {
                //throw;
            }


            //System.IO.Directory.Delete(SebConfigDir , true);
            //System.IO.Directory.Delete(SebInstallDir, true);

            try
            {
                System.IO.Directory.Delete(SebConfigDir, true);
            }
            catch (Exception)
            {
                //throw;
            }

            try
	        {
                System.IO.Directory.Delete(SebInstallDir, true);
	        }
	        catch (Exception)
	        {
		        //throw;
	        }


            // If existing, delete the shortcut to the
            // program executable "SebStarter.exe" on the Common Desktop
            if (System.IO.File.Exists(CommonDesktopIconUrl))
                System.IO.File.Delete(CommonDesktopIconUrl);

            return;

        }  // end of method   SebServiceInstaller_AfterUninstall()


    }
}
