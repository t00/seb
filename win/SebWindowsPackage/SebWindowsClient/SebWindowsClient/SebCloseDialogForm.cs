using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SebWindowsClient.ConfigurationUtils;
using SebWindowsClient.CryptographyUtils;

namespace SebWindowsClient
{
    public partial class SebCloseDialogForm : Form
    {
        public SebCloseDialogForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SEBClientInfo.SebWindowsClientForm.closeSebClient = false;
            this.txtQuitPassword.Text = "";
            this.Visible = false;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string userQuitPassword = this.txtQuitPassword.Text;

            SEBClientInfo.SebWindowsClientForm.closeSebClient = false;

            SEBProtectionController sEBProtectionControler = new SEBProtectionController();
            string hPassword = sEBProtectionControler.ComputeQuitPasswordHash(userQuitPassword);
            int quit = ((string)SEBClientInfo.getSebSetting(SEBGlobalConstants.MessageHashedQuitPassword)[SEBGlobalConstants.MessageHashedQuitPassword]).CompareTo(hPassword);
            if (quit != 0)
            {
                SEBErrorMessages.OutputErrorMessage(SEBGlobalConstants.IND_CLOSE_SEB_FAILED, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                this.Visible = false;
            }
            else
            {
                SEBClientInfo.SebWindowsClientForm.closeSebClient = true;
                Application.Exit();
            }
        }
    }
}
