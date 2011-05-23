using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


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
            string XulRunnerDirectory = "xul.zip";
            Console.WriteLine("XulRunnerDirectory = " + XulRunnerDirectory);
            //System("copy xul.zip xul2.zip");

            // Autostart the SEB Windows Service when installation was successful
            string sebServiceName       = this.SebServiceInstaller.ServiceName;
            var    sebServiceController = new ServiceController(sebServiceName);
                   sebServiceController.Start();
        }
    }
}
