using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using SebShared.CryptographyUtils;
using SebShared.Properties;
using SebShared.Utils;
using SebShared;
using SebWindowsClient.ConfigurationUtils;
using SebWindowsClient.UI;

namespace SebWindowsClient
{
	public partial class SebCloseDialogForm: Form
	{
		public SebCloseDialogForm()
		{
			InitializeComponent();
			if((Boolean)SEBClientInfo.getSebSetting(SebSettings.KeyTouchOptimized)[SebSettings.KeyTouchOptimized] == true)
			{
				this.Font = new Font(FontFamily.GenericSansSerif, 12);
				this.lblQuitPassword.Left = (Screen.PrimaryScreen.Bounds.Width / 2) - (this.lblQuitPassword.Width / 2);
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
			string hPassword = SebProtectionController.ComputePasswordHash(userQuitPassword);
			string settingsPasswordHash = (string)SEBClientInfo.getSebSetting(SebSettings.KeyHashedQuitPassword)[SebSettings.KeyHashedQuitPassword];
			int quit = String.Compare(settingsPasswordHash, hPassword, StringComparison.OrdinalIgnoreCase);
			if(quit != 0)
			{
				this.Hide();
				SebMessageBox.Show(SEBUIStrings.quittingFailed, SEBUIStrings.quittingFailedReason, MessageBoxImage.Error, MessageBoxButton.OK);
				this.txtQuitPassword.Text = "";
				this.Visible = false;
			}
			else
			{
				//SEBClientInfo.SebWindowsClientForm.closeSebClient = true;
				this.Visible = false;
				SEBClientInfo.SebWindowsClientForm.ExitApplication();
			}
		}

		private void txtQuitPassword_Enter(object sender, EventArgs e)
		{
			if(SebInstance.Settings.Get<bool>(SebSettings.KeyEnableOnScreenKeyboardNative))
	        {
				TapTipHandler.ShowKeyboard();
	        }
		}

		private void txtQuitPassword_Leave(object sender, EventArgs e)
		{
			if(SebInstance.Settings.Get<bool>(SebSettings.KeyEnableOnScreenKeyboardNative))
			{
				TapTipHandler.HideKeyboard();
			}
		}
	}
}
