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
using SebWindowsClient.UI;
using SebWindowsClient.XULRunnerCommunication;

namespace SebWindowsClient
{
    public partial class SebCloseDialogForm : Form
    {
        public SebCloseDialogForm()
        {
            InitializeComponent();
            if ((int) SEBClientInfo.getSebSetting(SEBSettings.KeyTouchOptimized)[SEBSettings.KeyTouchOptimized] == 1)
            {
                this.Font = new Font(FontFamily.GenericSansSerif, 12);
                this.lblQuitPassword.Left = (Screen.PrimaryScreen.Bounds.Width/2) - (this.lblQuitPassword.Width/2);
                IntPtr hwnd = this.Handle;
                this.FormBorderStyle = FormBorderStyle.None;
                this.Top = 0;
                this.Left = 0;
                this.Width = Screen.PrimaryScreen.Bounds.Width;
                this.Height = Screen.PrimaryScreen.Bounds.Height;
                this.btnCancel.BackColor = Color.Red;
                this.btnCancel.FlatStyle = FlatStyle.Flat;
                this.btnCancel.Height = 35;
                this.btnCancel.Width = 120;
                this.btnCancel.Left = (Screen.PrimaryScreen.Bounds.Width/2) - (this.btnCancel.Width / 2) + 100;
                this.btnOk.BackColor = Color.Green;
                this.btnOk.FlatStyle = FlatStyle.Flat;
                this.btnOk.Height = 35;
                this.btnOk.Width = 120;
                this.btnOk.Left = (Screen.PrimaryScreen.Bounds.Width / 2) - (this.btnOk.Width / 2) - 100;
                this.txtQuitPassword.Width = 400;
                this.txtQuitPassword.Left = (Screen.PrimaryScreen.Bounds.Width / 2) - (this.txtQuitPassword.Width / 2);
                this.txtQuitPassword.Height = 30;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //SEBClientInfo.SebWindowsClientForm.closeSebClient = false;
            this.txtQuitPassword.Text = "";
            this.Visible = false;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string userQuitPassword = this.txtQuitPassword.Text;

            //SEBClientInfo.SebWindowsClientForm.closeSebClient = false;

            //SEBProtectionController sebProtectionControler = new SEBProtectionController();
            string hPassword = SEBProtectionController.ComputePasswordHash(userQuitPassword);
            string settingsPasswordHash = (string)SEBClientInfo.getSebSetting(SEBSettings.KeyHashedQuitPassword)[SEBSettings.KeyHashedQuitPassword];
            int quit = String.Compare(settingsPasswordHash, hPassword, StringComparison.OrdinalIgnoreCase);
            if (quit != 0)
            {
                this.Hide();
                SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.quittingFailed, SEBUIStrings.quittingFailedReason, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, MessageBoxButtons.OK);
                this.txtQuitPassword.Text = "";
                this.Visible = false;
            }
            else
            {
                //SEBClientInfo.SebWindowsClientForm.closeSebClient = true;
                this.Visible = false;
                SEBClientInfo.SebWindowsClientForm.ExitApplication();
                //Application.Exit();
            }
        }

        private void txtQuitPassword_Enter(object sender, EventArgs e)
        {
            SEBOnScreenKeyboardToolStripButton.ShowKeyboard();
        }

        private void txtQuitPassword_Leave(object sender, EventArgs e)
        {
            SEBOnScreenKeyboardToolStripButton.HideKeyboard();
        }
    }
}
