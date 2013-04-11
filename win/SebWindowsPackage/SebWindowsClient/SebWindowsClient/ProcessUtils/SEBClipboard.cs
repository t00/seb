// -------------------------------------------------------------
//     Viktor tomas
//     BFH-TI, http://www.ti.bfh.ch
//     Biel, 2012
// -------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SebWindowsClient.ProcessUtils
{
    public static class SEBClipboard
    {
        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Clean clipboard.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public static void CleanClipboard()
        {
            Clipboard.Clear();
        }
    }
}
