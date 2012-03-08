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
            using (StreamWriter writer1 = new StreamWriter(UserDebugFile))
            {
                writer1.WriteLine();
                writer1.WriteLine("operatingSystem  = " + operatingSystem);
                writer1.WriteLine("platform         = " + operatingSystem.Platform);
                writer1.WriteLine("version          = " + operatingSystem.Version);
                writer1.WriteLine("versionString    = " + operatingSystem.VersionString);
                writer1.WriteLine("version.Major    = " + operatingSystem.Version.Major);
                writer1.WriteLine();
                writer1.WriteLine("     AllUsersDir = " +      AllUsersDir);
                writer1.WriteLine("       PublicDir = " +        PublicDir);
                writer1.WriteLine("CommonDesktopDir = " + CommonDesktopDir);
                writer1.WriteLine("  UserDesktopDir = " +   UserDesktopDir);
                writer1.WriteLine();
                writer1.Flush();
            }
*/
            return CommonDesktopDir;
        }





        private void SebServiceInstaller_Committed(object sender, InstallEventArgs e)
        {
            // Unpack the XULRunner directories after installation

            string ProgramData    = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string ProgramFiles   = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            string Manufacturer = "ETH Zuerich";
            string Product      = "SEB Windows";
            string Version      = "1.9";
            string Component    = "SebWindowsClient";
            string Build        = "Release";

            // Get the directory of the .msi installer file as CustomActionData.
            // To see where the "SourceDir" comes from, look at:
            // Custom Actions window ->
            // Install and Commit phases ->
            // Primary output of SebWindowsService (Active) ->
            // Properties window ->
            // CustomActionData: /SourceDir="[SOURCEDIR]\"

            string SebBatchDir   = this.Context.Parameters["SourceDir"];
            string SebTargetDir  = this.Context.Parameters["TargetDir"];

            string SebConfigDir  = ProgramData  + "\\" + Manufacturer + "\\" + Product + " " + Version;
            string SebInstallDir = ProgramFiles + "\\" + Manufacturer + "\\" + Product + " " + Version;

            string SebClientDir  = SebInstallDir + "\\" + Component;
            string SebReleaseDir = SebInstallDir + "\\" + Component + "\\" + Build;

            string SebInstallMsi = "SebWindowsInstall.msi";
            string SebStarterExe = "SebStarter.exe";
            string SebStarterBat = "SebStarter.bat";
            string SebStarterIni = "SebStarter.ini";
            string SebMsgHookIni =    "MsgHook.ini";

            // SebBatchDir already has a "\\" at the end,
            // therefore we can append the file names directly to it.
            string SebInstallMsiFile = SebBatchDir + SebInstallMsi;
            string SebStarterBatFile = SebBatchDir + SebStarterBat;
            string SebStarterIniFile = SebBatchDir + SebStarterIni;
            string SebMsgHookIniFile = SebBatchDir + SebMsgHookIni;

            string SebStarterExeFileTarget = SebReleaseDir + "\\" + SebStarterExe;
            string SebStarterBatFileTarget = SebReleaseDir + "\\" + SebStarterBat;
            string SebStarterIniFileTarget = SebConfigDir  + "\\" + SebStarterIni;
            string SebMsgHookIniFileTarget = SebConfigDir  + "\\" + SebMsgHookIni;

            string CommonDesktopDirectory = GetCommonDesktopDirectory();
            string CommonDesktopIconUrl   = CommonDesktopDirectory + "\\" + Product + " " + Version + ".url";

            string XulSebZip           = "xul_seb.zip";
            string XulRunnerZip        = "xulrunner.zip";
            string XulRunnerNoSslZip   = "xulrunner_no_ssl_warning.zip";
            string XulRunnerWithSslZip = "xulrunner_with_ssl_warning.zip";

            string XulSebZipFile           = SebClientDir + "\\" + XulSebZip;
            string XulRunnerZipFile        = SebClientDir + "\\" + XulRunnerZip;
            string XulRunnerNoSslZipFile   = SebClientDir + "\\" + XulRunnerNoSslZip;
            string XulRunnerWithSslZipFile = SebClientDir + "\\" + XulRunnerWithSslZip;


            // Write some debug data into a file
            string UserDesktopDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string UserDebugFile  = UserDesktopDir + "\\" + "NickTargetDir.txt";
            using (StreamWriter sw = new StreamWriter(UserDebugFile))
            {
                sw.WriteLine();
                sw.WriteLine("SebBatchDir  = " + SebBatchDir);
                sw.WriteLine("SebTargetDir = " + SebTargetDir);
                sw.WriteLine();
                sw.WriteLine("SebConfigDir  = " + SebConfigDir);
                sw.WriteLine("SebInstallDir = " + SebInstallDir);
                sw.WriteLine();
                sw.WriteLine("SebClientDir  = " + SebClientDir);
                sw.WriteLine("SebReleaseDir = " + SebReleaseDir);
                sw.WriteLine();
                sw.WriteLine("SebInstallMsiFile = " + SebInstallMsiFile);
                sw.WriteLine("SebStarterBatFile = " + SebStarterBatFile);
                sw.WriteLine("SebStarterIniFile = " + SebStarterIniFile);
                sw.WriteLine("SebMsgHookIniFile = " + SebMsgHookIniFile);
                sw.WriteLine();
                sw.WriteLine("SebStarterExeFileTarget = " + SebStarterExeFileTarget);
                sw.WriteLine("SebStarterBatFileTarget = " + SebStarterBatFileTarget);
                sw.WriteLine("SebStarterIniFileTarget = " + SebStarterIniFileTarget);
                sw.WriteLine("SebMsgHookIniFileTarget = " + SebMsgHookIniFileTarget);
                sw.WriteLine();
                sw.WriteLine("CommonDesktopDirectory = " + CommonDesktopDirectory);
                sw.WriteLine("CommonDesktopIconUrl   = " + CommonDesktopIconUrl);
                sw.WriteLine();
                sw.WriteLine("XulSebZipFile           = " + XulSebZipFile);
                sw.WriteLine("XulRunnerZipFile        = " + XulRunnerZipFile);
                sw.WriteLine("XulRunnerNoSslZipFile   = " + XulRunnerNoSslZipFile);
                sw.WriteLine("XulRunnerWithSslZipFile = " + XulRunnerWithSslZipFile);
                sw.WriteLine();
                sw.Flush();
            }


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

            // Create a shortcut to the program executable "SebStarter.exe" on the common desktop
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

            string ProgramData    = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string ProgramFiles   = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            string Manufacturer = "ETH Zuerich";
            string Product      = "SEB Windows";
            string Version      = "1.9";
            string Component    = "SebWindowsClient";
            string Build        = "Release";

            string Service      = "SebWindowsService";
            string Logfile      = "SebWindowsService.logfile.txt";

            string SebConfigDir  = ProgramData   + "\\" + Manufacturer + "\\" + Product + " " + Version;
            string SebInstallDir = ProgramFiles  + "\\" + Manufacturer + "\\" + Product + " " + Version;

            string SebClientDir  = SebInstallDir + "\\" + Component;
            string SebReleaseDir = SebInstallDir + "\\" + Component + "\\" + Build;  
            string SebServiceDir = SebInstallDir + "\\" + Service;
            string SebServiceLog = SebInstallDir + "\\" + Service + "\\" + Logfile;

            string CommonDesktopDirectory = GetCommonDesktopDirectory();
            string CommonDesktopIconUrl   = CommonDesktopDirectory + "\\" + Product + " " + Version + ".url";


            // ATTENTION:
            //
            // Deleting the SEB configuration directory in the ProgramData directory, e.g.
            // C:\ProgramData\ETH Zuerich\SEB Windows 1.9 ,
            // mostly succeeds.
            //
            // Deleting the SEB installation directory in the Program Files directory, e.g.
            // C:\Program Files (x86)\ETH Zuerich\SEB Windows 1.9\SebWindowsClient\Release ,
            // mostly fails, even though its files have all been deleted before.
            //
            // This is a known and annoying Windows bug still occurring in Windows 7:
            // Often it is impossible to delete an empty (!) directory
            // "because a process is still needing it" (which is not true).
            //
            // Currently, only a user logoff or machine reboot solves this,
            // so maybe it is necessary to reboot and manually delete the
            // C:\Program Files (x86)\ETH Zuerich\SEB Windows 1.9\SebWindowsClient\Release
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


            // Delete the shortcut to the program executable "SebStarter.exe" on the common desktop
            System.IO.File.Delete(CommonDesktopIconUrl);

            return;

        }  // end of method   SebServiceInstaller_AfterUninstall()


    }
}
