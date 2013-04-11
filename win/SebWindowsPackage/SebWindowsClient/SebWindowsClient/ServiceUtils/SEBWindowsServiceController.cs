// -------------------------------------------------------------
//     Viktor tomas
//     BFH-TI, http://www.ti.bfh.ch
//     Biel, 2012
// -------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace SebWindowsClient.ServiceUtils
{
    public class SEBWindowsServiceController
    {

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Check if service is running.
        /// </summary>
        /// <param name="service">service to check</param>
        /// <returns>true if service is running</returns>
        /// ----------------------------------------------------------------------------------------
        public static bool ServiceAvailable(String service) 
        {
            bool serviceAvailable = false;

            ServiceController serviceController   = new ServiceController(service);
            serviceAvailable = (serviceController.Status == ServiceControllerStatus.Running);
            serviceController.Close();

            return serviceAvailable;
        }

    }
}
