using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.IO.Packaging;



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
            // Unzip the XULRunner directories after installation
            //string XulSebZipFile = "xul_seb.zip";
            //Console.WriteLine("XulSebZipFile = " + XulSebZipFile);

            //ZipFile zipFile = zipFile.Read("xul_seb.zip");

            //System.IO.Packaging;
            //System.IO.Compression.DeflateStream deflateStream = new System.IO.Compression.DeflateStream();
            //System("copy xul.zip xul2.zip");

            // Autostart the SEB Windows Service when installation was successful
            string sebServiceName       = this.SebServiceInstaller.ServiceName;
            var    sebServiceController = new ServiceController(sebServiceName);
                   sebServiceController.Start();
        }
    }
}
