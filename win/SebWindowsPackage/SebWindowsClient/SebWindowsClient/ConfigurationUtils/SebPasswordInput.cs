using System;
using SebShared;
using SebShared.Properties;

namespace SebWindowsClient.ConfigurationUtils
{
	public static class SebPasswordInput
	{
		public static bool? ClientGetPassword(GetPasswordPurpose purpose, Predicate<string> checkPassword, out string password)
		{
			for(var i = 0; i < 5; i++)
			{
				string enterPasswordString;
				string title;
				switch(purpose)
				{
					case GetPasswordPurpose.PasswordMode:
					{
						enterPasswordString = (i == 0) ? SEBUIStrings.enterEncryptionPassword : SEBUIStrings.enterEncryptionPasswordAgain;
						title = SEBUIStrings.reconfiguringLocalSettings;
						break;
					}
					case GetPasswordPurpose.ConfigureClient:
					{
						enterPasswordString = (i == 0) ? SEBUIStrings.enterPassword : SEBUIStrings.enterPasswordAgain;
						title = SEBUIStrings.reconfiguringLocalSettings;
						break;
					}
					case GetPasswordPurpose.ConfigureLocalAdmin:
					{
						enterPasswordString = (i == 0) ? SEBUIStrings.enterCurrentAdminPwdForReconfiguring : SEBUIStrings.enterCurrentAdminPwdForReconfiguringAgain;
						title = SEBUIStrings.reconfiguringLocalSettings;
						break;
					}
					case GetPasswordPurpose.LoadingSettings:
					{
						enterPasswordString = (i == 0) ? SEBUIStrings.enterAdminPasswordRequired : SEBUIStrings.enterAdminPasswordRequiredAgain;
						title = SEBUIStrings.loadingSettings + (String.IsNullOrEmpty(SEBClientInfo.LoadingSettingsFileName) ? "" : ": " + SEBClientInfo.LoadingSettingsFileName);
						break;
					}
					default:
					{
						throw new NotImplementedException("Purpose not implemented");
					}
				}
				password = ThreadedDialog.ShowPasswordDialogForm(title, enterPasswordString);
				if(password == null)
				{
					return null;
				}
				if(checkPassword(password))
				{
					return true;
				}
			}
			password = null;
			return false;
		}
	}
}