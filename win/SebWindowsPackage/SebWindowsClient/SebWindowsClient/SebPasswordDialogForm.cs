﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SebShared;
using SebWindowsClient.ConfigurationUtils;
using SebWindowsClient.ProcessUtils;
using SebWindowsClient.UI;

namespace SebWindowsClient
{
    public partial class SebPasswordDialogForm : Form
    {
        [DllImportAttribute("User32.dll")]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);


        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Show SEB Password Dialog Form.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public static string ShowPasswordDialogForm(string title, string passwordRequestText)
        {
            using (SebPasswordDialogForm sebPasswordDialogForm = new SebPasswordDialogForm())
            {
                SetForegroundWindow(sebPasswordDialogForm.Handle);
                sebPasswordDialogForm.TopMost = true;
                // Set the title of the dialog window
                sebPasswordDialogForm.Text = title;
                // Set the text of the dialog
                sebPasswordDialogForm.LabelText = passwordRequestText;
                sebPasswordDialogForm.txtSEBPassword.Focus();
                // If we are running in SebWindowsClient we need to activate it before showing the password dialog
                // Don't do this; it will fail when the password dialog is running in a separate thread
                //if (SEBClientInfo.SebWindowsClientForm != null) SEBClientInfo.SebWindowsClientForm.Activate();
                // Show password dialog as a modal dialog and determine if DialogResult = OK.
                if (sebPasswordDialogForm.ShowDialog() == DialogResult.OK)
                {
                    // Read the contents of testDialog's TextBox.
                    string password = sebPasswordDialogForm.txtSEBPassword.Text;
                    sebPasswordDialogForm.txtSEBPassword.Text = "";
                    //sebPasswordDialogForm.txtSEBPassword.Focus();
                    return password;
                }
                else
                {
                    return null;
                }
            }
        }

        public SebPasswordDialogForm()
        {
            InitializeComponent();
			if(SebInstance.Settings.Get<bool>(SebSettings.KeyTouchOptimized))
			{
				InitializeForTouch();
			}
			else
			{
				InitializeForNonTouch();
			}
        }

        public void InitializeForTouch()
        {
            this.Font = new Font(FontFamily.GenericSansSerif, 12);
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
            this.btnCancel.Left = (Screen.PrimaryScreen.Bounds.Width / 2) - (this.btnCancel.Width / 2) + 100;
            this.btnOk.BackColor = Color.Green;
            this.btnOk.FlatStyle = FlatStyle.Flat;
            this.btnOk.Height = 35;
            this.btnOk.Width = 120;
            this.btnOk.Left = (Screen.PrimaryScreen.Bounds.Width / 2) - (this.btnOk.Width / 2) - 100;
            this.txtSEBPassword.Width = 400;
            this.txtSEBPassword.Left = (Screen.PrimaryScreen.Bounds.Width / 2) - (this.txtSEBPassword.Width / 2);
            this.txtSEBPassword.Height = 30;
        }
  
        public void InitializeForNonTouch()
        {
            this.Font = DefaultFont;
            this.lblSEBPassword.Left = (int)(12 * SEBClientInfo.scaleFactor);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Width = (int)(365 * SEBClientInfo.scaleFactor);
            this.Height = (int)(175 * SEBClientInfo.scaleFactor);
            this.Top = Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2;
            this.Left = Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2;
            this.btnCancel.BackColor = SystemColors.Control;
            this.btnCancel.FlatStyle = FlatStyle.Standard;
            this.btnCancel.Height = (int)(23 * SEBClientInfo.scaleFactor);
            this.btnCancel.Width = (int)(75 * SEBClientInfo.scaleFactor);
            this.btnCancel.Left = (int)(180 * SEBClientInfo.scaleFactor);
            this.btnOk.BackColor = SystemColors.Control;
            this.btnOk.FlatStyle = FlatStyle.Standard;
            this.btnOk.Height = (int)(23 * SEBClientInfo.scaleFactor);
            this.btnOk.Width = (int)(75 * SEBClientInfo.scaleFactor);
            this.btnOk.Left = (int)(94 * SEBClientInfo.scaleFactor);
            this.txtSEBPassword.Width = (int)(325 * SEBClientInfo.scaleFactor);
            this.txtSEBPassword.Left = (int)(12 * SEBClientInfo.scaleFactor);
            this.txtSEBPassword.Height = (int)(20 * SEBClientInfo.scaleFactor);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
			txtSEBPassword.Text = null;
			DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Visible = false;
			DialogResult = DialogResult.OK;
			if(SebInstance.Settings.Get<bool>(SebSettings.KeyTouchOptimized))
            {
                var topWindow = SEBWindowHandler.GetOpenWindows().FirstOrDefault();
                if (topWindow.Value != null)
                {
                    topWindow.Key.AdaptWindowToWorkingArea();
                }
            }
        }
		
        // Expose the label for changing from outside of the form
        public string LabelText
        {
            get
            {
                return this.lblSEBPassword.Text;
            }
            set
            {
                this.lblSEBPassword.Text = value;
                try
                {
					if(SebInstance.Settings.Get<bool>(SebSettings.KeyTouchOptimized))
                    {
                        this.lblSEBPassword.Left = (Screen.PrimaryScreen.Bounds.Width / 2) - (this.lblSEBPassword.Width / 2);
                    }
                }
                catch (Exception)
                {
                }
                
            }
        }

        private void txtSEBPassword_Enter(object sender, EventArgs e)
        {
			if(SebInstance.Settings.Get<bool>(SebSettings.KeyEnableOnScreenKeyboardNative))
	        {
		        TapTipHandler.ShowKeyboard();
	        }
        }

        private void txtSEBPassword_Leave(object sender, EventArgs e)
        {
			if(SebInstance.Settings.Get<bool>(SebSettings.KeyEnableOnScreenKeyboardNative))
	        {
		        TapTipHandler.HideKeyboard();
	        }
        }

    }
}
