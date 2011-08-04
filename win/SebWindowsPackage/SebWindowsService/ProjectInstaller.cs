using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
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


        private void SebServiceInstaller_Committed(object sender, InstallEventArgs e)
        {
            // Unpack the XULRunner directories after installation

            string ProgramData  = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string ProgramFiles = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            string Manufacturer = "ETH Zuerich";
            string Product      = "SEB Windows";
            string Version      = "1.7";
            string Component    = "SebWindowsClient";
            string Build        = "Release";

            string SebConfigDir  = ProgramData  + "\\" + Manufacturer + "\\" + Product + " " + Version;
            string SebInstallDir = ProgramFiles + "\\" + Manufacturer + "\\" + Product + " " + Version;
            string SebClientDir  = ProgramFiles + "\\" + Manufacturer + "\\" + Product + " " + Version + "\\" + Component;
            string SebReleaseDir = ProgramFiles + "\\" + Manufacturer + "\\" + Product + " " + Version + "\\" + Component + "\\" + Build;

            string XulSebZip         = "xul_seb.zip";
            string XulRunnerZip      = "xulrunner.zip";
            string XulRunnerNoSslZip = "xulrunner_no_ssl_warning.zip";

            string XulSebZipFile         = SebClientDir + "\\" + XulSebZip;
            string XulRunnerZipFile      = SebClientDir + "\\" + XulRunnerZip;
            string XulRunnerNoSslZipFile = SebClientDir + "\\" + XulRunnerNoSslZip;


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

            // Extract all files from the "xulrunner.zip" file
            using (ZipFile zipFile = ZipFile.Read(XulRunnerNoSslZipFile))
            {
                foreach (ZipEntry zipEntry in zipFile)
                {
                    zipEntry.Extract(SebClientDir, ExtractExistingFileAction.OverwriteSilently);
                }
            }


            // Autostart the SEB Windows Service after installation
            string sebServiceName       = this.SebServiceInstaller.ServiceName;
            var    sebServiceController = new ServiceController(sebServiceName);
                   sebServiceController.Start();
        }



        private void SebServiceInstaller_AfterUninstall(object sender, InstallEventArgs e)
        {
            // Delete all remaining directories and files after uninstallation
/*
            string ProgramData  = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string ProgramFiles = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            string Manufacturer = "ETH Zuerich";
            string Product      = "SEB Windows";
            string Version      = "1.7";

            string SebConfigDir  = ProgramData  + "\\" + Manufacturer + "\\" + Product + " " + Version;
            string SebInstallDir = ProgramFiles + "\\" + Manufacturer + "\\" + Product + " " + Version;

            System.IO.Directory.Delete(SebConfigDir , true);
            System.IO.Directory.Delete(SebInstallDir, true);
*/
        }



        private void SebServiceInstaller_BeforeUninstall(object sender, InstallEventArgs e)
        {
            // Stop the SEB Windows Service before uninstallation
/*
            string sebServiceName       = this.SebServiceInstaller.ServiceName;
            var    sebServiceController = new ServiceController(sebServiceName);
                   sebServiceController.Stop();
*/
        }

    }
}
