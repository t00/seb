//
//  SEBConfigFileManager.cs
//  SafeExamBrowser
//
//  Copyright (c) 2010-2014 Daniel R. Schneider, 
//  ETH Zurich, Educational Development and Technology (LET),
//  based on the original idea of Safe Exam Browser
//  by Stefan Schneider, University of Giessen
//  Project concept: Thomas Piendl, Daniel R. Schneider,
//  Dirk Bauer, Kai Reuter, Tobias Halbherr, Karsten Burger, Marco Lehre,
//  Brigitte Schmucki, Oliver Rahs. French localization: Nicolas Dunand
//
//  ``The contents of this file are subject to the Mozilla Public License
//  Version 1.1 (the "License"); you may not use this file except in
//  compliance with the License. You may obtain a copy of the License at
//  http://www.mozilla.org/MPL/
//
//  Software distributed under the License is distributed on an "AS IS"
//  basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//  License for the specific language governing rights and limitations
//  under the License.
//
//  The Original Code is Safe Exam Browser for Windows.
//
//  The Initial Developer of the Original Code is Daniel R. Schneider.
//  Portions created by Daniel R. Schneider
//  are Copyright (c) 2010-2014 Daniel R. Schneider, 
//  ETH Zurich, Educational Development and Technology (LET), 
//  based on the original idea of Safe Exam Browser
//  by Stefan Schneider, University of Giessen. All Rights Reserved.
//
//  Contributor(s): ______________________________________.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using SebShared.CryptographyUtils;
using SebShared.DiagnosticUtils;
using SebShared.Properties;
using SebShared.Utils;

namespace SebShared
{
	public enum GetPasswordPurpose
	{
		PasswordMode,
		ConfigureClient,
		ConfigureLocalAdmin,
		LoadingSettings
	}

	public delegate bool? GetPasswordMethod(GetPasswordPurpose purpose, Predicate<string> checkPassword, out string password);

	public class SebConfigFileManager
	{

		// Prefixes
		public const int PREFIX_LENGTH = 4;
		public const string PUBLIC_KEY_HASH_MODE = "pkhs";
		public const string PASSWORD_MODE = "pswd";
		public const string PLAIN_DATA_MODE = "plnd";
		public const string PASSWORD_CONFIGURING_CLIENT_MODE = "pwcc";
		public const string UNENCRYPTED_MODE = "<?xm";

		// Public key hash identifier length
		private const int PUBLIC_KEY_HASH_LENGTH = 20;

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Decrypt, parse and use new SEB settings
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		public static bool StoreDecryptedSEBSettings(byte[] sebData, GetPasswordMethod getPassword)
		{
			Logger.AddInformation("Reconfiguring");
			string sebFilePassword = null;
			bool passwordIsHash = false;
			X509Certificate2 sebFileCertificateRef = null;

			var sebPreferencesDict = DecryptSEBSettings(sebData, getPassword, false, ref sebFilePassword, ref passwordIsHash, ref sebFileCertificateRef);
			if(sebPreferencesDict == null)
			{
				return false; //Decryption didn't work, we abort
			}

			// Store decrypted settings
			Logger.AddInformation("Attempting to StoreSebClientSettings");
			SebSettings.StoreSebClientSettings(sebPreferencesDict);
			Logger.AddInformation("Successfully StoreSebClientSettings");

			return true;
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Decrypt and deserialize SEB settings
		/// When forEditing = true, then the decrypting password the user entered and/or 
		/// certificate reference found in the .seb file is returned 
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		public static Dictionary<string, object> DecryptSEBSettings(byte[] sebData, GetPasswordMethod getPassword, bool forEditing, ref string sebFilePassword, ref bool passwordIsHash, ref X509Certificate2 sebFileCertificateRef)
		{
			// Ungzip the .seb (according to specification >= v14) source data
			byte[] unzippedSebData = GZipByte.Decompress(sebData);

			// if unzipped data is not null, then unzipping worked, we use unzipped data
			// if unzipped data is null, then the source data may be an uncompressed .seb file, we proceed with it
			if(unzippedSebData != null) sebData = unzippedSebData;

			string prefixString;

			// save the data including the first 4 bytes for the case that it's acutally an unencrypted XML plist
			byte[] sebDataUnencrypted = sebData.Clone() as byte[];

			// Get 4-char prefix
			prefixString = GetPrefixStringFromData(ref sebData);

			//// Check prefix identifying encryption modes

			// Prefix = pkhs ("Public Key Hash") ?

			if(String.CompareOrdinal(prefixString, PUBLIC_KEY_HASH_MODE) == 0)
			{

				// Decrypt with cryptographic identity/private key
				sebData = DecryptDataWithPublicKeyHashPrefix(sebData, forEditing, ref sebFileCertificateRef);
				//SEBMessageBox.Show(SEBUIStrings.errorDecryptingSettings, SEBUIStrings.certificateNotFoundInStore, MessageBoxIcon.Error, MessageBoxButtons.OK, neverShowTouchOptimized : forEditing);
				//SEBMessageBox.Show(SEBUIStrings.errorDecryptingSettings, SEBUIStrings.certificateDecryptingError + ex.Message, MessageBoxImage.Error, MessageBoxButton.OK);
				if(sebData == null)
				{
					return null;
				}

				// Get 4-char prefix again
				// and remaining data without prefix, which is either plain or still encoded with password
				prefixString = GetPrefixStringFromData(ref sebData);
			}

			// Prefix = pswd ("Password") ?
			if(String.CompareOrdinal(prefixString, PASSWORD_MODE) == 0)
			{
				// Decrypt with password
				// if the user enters the right one
				byte[] sebDataDecrypted = null;
				string password;
				var gotPassword = getPassword(GetPasswordPurpose.PasswordMode, p =>
				{
					sebDataDecrypted = SebProtectionController.DecryptDataWithPassword(sebData, p);
					return sebDataDecrypted != null;
				}, out password);

				if(!gotPassword.HasValue)
				{
					return null;
				}
				if(!gotPassword.Value)
				{
					//wrong password entered in 5th try: stop reading .seb file
					SebMessageBox.Show(SEBUIStrings.decryptingSettingsFailed, SEBUIStrings.decryptingSettingsFailedReason, MessageBoxImage.Error, MessageBoxButton.OK);
					return null;
				}
				sebData = sebDataDecrypted;
				// If these settings are being decrypted for editing, we return the decryption password
				if(forEditing) sebFilePassword = password;
			}
			else
			{
				// Prefix = pwcc ("Password Configuring Client") ?
				if(String.CompareOrdinal(prefixString, PASSWORD_CONFIGURING_CLIENT_MODE) == 0)
				{

					// Decrypt with password and configure local client settings
					// and quit afterwards, returning if reading the .seb file was successfull
					var sebSettings = DecryptDataWithPasswordForConfiguringClient(sebData, getPassword, forEditing, ref sebFilePassword, ref passwordIsHash);
					return sebSettings;

				}
				else
				{
					// Prefix = plnd ("Plain Data") ?

					if(String.CompareOrdinal(prefixString, PLAIN_DATA_MODE) != 0)
					{
						// No valid 4-char prefix was found in the .seb file
						// Check if .seb file is unencrypted
						if(String.CompareOrdinal(prefixString, UNENCRYPTED_MODE) == 0)
						{
							// .seb file seems to be an unencrypted XML plist
							// get the original data including the first 4 bytes
							sebData = sebDataUnencrypted;
						}
						else
						{
							// No valid prefix and no unencrypted file with valid header
							// cancel reading .seb file
							SebMessageBox.Show(SEBUIStrings.settingsNotUsable, SEBUIStrings.settingsNotUsableReason, MessageBoxImage.Error, MessageBoxButton.OK);
							return null;
						}
					}
				}
			}
			// If we don't deal with an unencrypted seb file
			// ungzip the .seb (according to specification >= v14) decrypted serialized XML plist data
			if(String.CompareOrdinal(prefixString, UNENCRYPTED_MODE) != 0)
			{
				sebData = GZipByte.Decompress(sebData);
			}

			// Get preferences dictionary from decrypted data
			var sebPreferencesDict = GetPreferencesDictFromConfigData(sebData, getPassword, forEditing);
			// If we didn't get a preferences dict back, we abort reading settings
			if(sebPreferencesDict == null) return null;

			// We need to set the right value for the key sebConfigPurpose to know later where to store the new settings
			sebPreferencesDict[SebSettings.KeySebConfigPurpose] = (int)SebSettings.sebConfigPurposes.sebConfigPurposeStartingExam;

			// Reading preferences was successful!
			return sebPreferencesDict;
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Helper method which decrypts the byte array using an empty password, 
		/// or the administrator password currently set in SEB 
		/// or asks for the password used for encrypting this SEB file
		/// for configuring the client 
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		private static Dictionary<string, object> DecryptDataWithPasswordForConfiguringClient(byte[] sebData, GetPasswordMethod getPassword, bool forEditing, ref string sebFilePassword, ref bool passwordIsHash)
		{
			passwordIsHash = false;
			string password;
			// First try to decrypt with the current admin password
			// get admin password hash
			string hashedAdminPassword = (string)SebSettings.valueForDictionaryKey(SebSettings.settingsCurrent, SebSettings.KeyHashedAdminPassword);
			if(hashedAdminPassword == null)
			{
				hashedAdminPassword = "";
			}
			// We use always uppercase letters in the base16 hashed admin password used for encrypting
			hashedAdminPassword = hashedAdminPassword.ToUpper();
			Dictionary<string, object> sebPreferencesDict = null;
			byte[] decryptedSebData = SebProtectionController.DecryptDataWithPassword(sebData, hashedAdminPassword);
			if(decryptedSebData == null)
			{
				// If decryption with admin password didn't work, try it with an empty password
				decryptedSebData = SebProtectionController.DecryptDataWithPassword(sebData, "");
				if(decryptedSebData == null)
				{
					// If decryption with empty and admin password didn't work, ask for the password the .seb file was encrypted with
					// Allow up to 5 attempts for entering decoding password
					var gotPassword = getPassword(GetPasswordPurpose.ConfigureClient, p =>
					{
						var hashedPassword = SebProtectionController.ComputePasswordHash(p);
						// we try to decrypt with the hashed password
						decryptedSebData = SebProtectionController.DecryptDataWithPassword(sebData, hashedPassword);
						return decryptedSebData != null;
					}, out password);
					// If cancel was pressed, abort
					if(!gotPassword.HasValue)
					{
						return null;
					}
					if(!gotPassword.Value)
					{
						//wrong password entered in 5th try: stop reading .seb file
						SebMessageBox.Show(SEBUIStrings.reconfiguringLocalSettingsFailed, SEBUIStrings.reconfiguringLocalSettingsFailedWrongPassword, MessageBoxImage.Error, MessageBoxButton.OK);
						return null;
					}
					else
					{
						// Decrypting with entered password worked: We save it for returning it later
						if(forEditing) sebFilePassword = password;
					}
				}
			}
			else
			{
				//decrypting with hashedAdminPassword worked: we save it for returning as decryption password 
				sebFilePassword = hashedAdminPassword;
				// identify that password as hash
				passwordIsHash = true;
			}
			/// Decryption worked

			// Ungzip the .seb (according to specification >= v14) decrypted serialized XML plist data
			decryptedSebData = GZipByte.Decompress(decryptedSebData);

			// Check if the openend reconfiguring seb file has the same admin password inside like the current one

			try
			{
				sebPreferencesDict = (Dictionary<string, object>)PropertyList.readPlist(decryptedSebData);
			}
			catch(Exception readPlistException)
			{
				// Error when deserializing the decrypted configuration data
				// We abort reading the new settings here
				SebMessageBox.Show(SEBUIStrings.loadingSettingsFailed, SEBUIStrings.loadingSettingsFailedReason, MessageBoxImage.Error, MessageBoxButton.OK);
				Console.WriteLine(readPlistException.Message);
				return null;
			}
			// Get the admin password set in these settings
			string sebFileHashedAdminPassword = (string)SebSettings.valueForDictionaryKey(sebPreferencesDict, SebSettings.KeyHashedAdminPassword);
			if(sebFileHashedAdminPassword == null)
			{
				sebFileHashedAdminPassword = "";
			}
			// Has the SEB config file the same admin password inside as the current settings have?
			if(String.Compare(hashedAdminPassword, sebFileHashedAdminPassword, StringComparison.OrdinalIgnoreCase) != 0)
			{
				//No: The admin password inside the .seb file wasn't the same as the current one
				if(forEditing)
				{
					// If the file is openend for editing (and not to reconfigure SEB)
					// we have to ask the user for the admin password inside the file
					if(!askForPasswordAndCompareToHashedPassword(sebFileHashedAdminPassword, getPassword, forEditing))
					{
						// If the user didn't enter the right password we abort
						return null;
					}
				}
				else
				{
					// The file was actually opened for reconfiguring the SEB client:
					// we have to ask for the current admin password and
					// allow reconfiguring only if the user enters the right one
					// We don't check this for the case the current admin password was used to encrypt the new settings
					// In this case there can be a new admin pw defined in the new settings and users don't need to enter the old one
					if(passwordIsHash == false && hashedAdminPassword.Length > 0)
					{
						// Allow up to 5 attempts for entering current admin password
						var gotPassword = getPassword(GetPasswordPurpose.ConfigureLocalAdmin, p =>
						{
							var hashedPassword = string.IsNullOrEmpty(p) ? string.Empty : SebProtectionController.ComputePasswordHash(p);
							return String.Compare(hashedPassword, hashedAdminPassword, StringComparison.OrdinalIgnoreCase) == 0;
						}, out password);
						if(!gotPassword.GetValueOrDefault())
						{
							//wrong password entered in 5th try: stop reading .seb file
							SebMessageBox.Show(SEBUIStrings.reconfiguringLocalSettingsFailed, SEBUIStrings.reconfiguringLocalSettingsFailedWrongCurrentAdminPwd, MessageBoxImage.Error, MessageBoxButton.OK);
							return null;
						}
					}
				}
			}

			// We need to set the right value for the key sebConfigPurpose to know later where to store the new settings
			sebPreferencesDict[SebSettings.KeySebConfigPurpose] = (int)SebSettings.sebConfigPurposes.sebConfigPurposeConfiguringClient;

			// Reading preferences was successful!
			return sebPreferencesDict;
		}


		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Helper method: Get preferences dictionary from decrypted data.
		/// In editing mode, users have to enter the right SEB administrator password 
		/// before they can access the settings contents
		/// and returns the decrypted bytes 
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		private static Dictionary<string, object> GetPreferencesDictFromConfigData(byte[] sebData, GetPasswordMethod getPassword, bool forEditing)
		{
			Dictionary<string, object> sebPreferencesDict = null;
			try
			{
				// Get preferences dictionary from decrypted data
				sebPreferencesDict = (Dictionary<string, object>)PropertyList.readPlist(sebData);
			}
			catch(Exception readPlistException)
			{
				SebMessageBox.Show(SEBUIStrings.loadingSettingsFailed, SEBUIStrings.loadingSettingsFailedReason, MessageBoxImage.Error, MessageBoxButton.OK);
				Console.WriteLine(readPlistException.Message);
				return null;
			}
			// In editing mode, the user has to enter the right SEB administrator password used in those settings before he can access their contents
			if(forEditing)
			{
				// Get the admin password set in these settings
				string sebFileHashedAdminPassword = (string)SebSettings.valueForDictionaryKey(sebPreferencesDict, SebSettings.KeyHashedAdminPassword);
				// If there was no or empty admin password set in these settings, the user can access them anyways
				if(!String.IsNullOrEmpty(sebFileHashedAdminPassword))
				{
					// Get the current hashed admin password
					string hashedAdminPassword = (string)SebSettings.valueForDictionaryKey(SebSettings.settingsCurrent, SebSettings.KeyHashedAdminPassword);
					if(hashedAdminPassword == null)
					{
						hashedAdminPassword = "";
					}
					// If the current hashed admin password is same as the hashed admin password from the settings file
					// then the user is allowed to access the settings
					if(String.Compare(hashedAdminPassword, sebFileHashedAdminPassword, StringComparison.OrdinalIgnoreCase) != 0)
					{
						// otherwise we have to ask for the SEB administrator password used in those settings and
						// allow opening settings only if the user enters the right one

						if(!askForPasswordAndCompareToHashedPassword(sebFileHashedAdminPassword, getPassword, forEditing))
						{
							return null;
						}
					}
				}
			}
			// Reading preferences was successful!
			return sebPreferencesDict;
		}


		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Ask user to enter password and compare it to the passed (hashed) password string 
		/// Returns true if correct password was entered 
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		private static bool askForPasswordAndCompareToHashedPassword(string sebFileHashedAdminPassword, GetPasswordMethod getPassword, bool forEditing)
		{
			// Check if there wasn't a hashed password (= empty password)
			if(sebFileHashedAdminPassword.Length == 0)
			{
				return true;
			}
			// We have to ask for the SEB administrator password used in the settings 
			// and allow opening settings only if the user enters the right one
			// Allow up to 5 attempts for entering  admin password
			string password;
			var gotPassword = getPassword(GetPasswordPurpose.LoadingSettings, p =>
			{
				var hashedPassword = string.IsNullOrEmpty(p) ? string.Empty : SebProtectionController.ComputePasswordHash(p);
				return String.Compare(hashedPassword, sebFileHashedAdminPassword, StringComparison.OrdinalIgnoreCase) == 0;
			}, out password);
			if(!gotPassword.GetValueOrDefault())
			{
				//wrong password entered in 5th try: stop reading .seb file
				SebMessageBox.Show(SEBUIStrings.loadingSettingsFailed, SEBUIStrings.loadingSettingsFailedWrongAdminPwd, MessageBoxImage.Error, MessageBoxButton.OK);
				return false;
			}
			// Right password entered
			return true;
		}


		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Helper method which fetches the public key hash from a byte array, 
		/// retrieves the according cryptographic identity from the certificate store
		/// and returns the decrypted bytes 
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		private static byte[] DecryptDataWithPublicKeyHashPrefix(byte[] sebData, bool forEditing, ref X509Certificate2 sebFileCertificateRef)
		{
			// Get 20 bytes public key hash prefix
			// and remaining data with the prefix stripped
			byte[] publicKeyHash = GetPrefixDataFromData(ref sebData, PUBLIC_KEY_HASH_LENGTH);

			X509Certificate2 certificateRef = SebProtectionController.GetCertificateFromStore(publicKeyHash);
			if(certificateRef == null)
			{
				SebMessageBox.Show(SEBUIStrings.errorDecryptingSettings, SEBUIStrings.certificateNotFoundInStore, MessageBoxImage.Error, MessageBoxButton.OK);
				return null;
			}
			// If these settings are being decrypted for editing, we will return the decryption certificate reference
			// in the variable which was passed as reference when calling this method
			if(forEditing)
			{
				sebFileCertificateRef = certificateRef;
			}

			sebData = SebProtectionController.DecryptDataWithCertificate(sebData, certificateRef);
			return sebData;
		}


		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Helper method for returning a prefix string (of PREFIX_LENGTH, currently 4 chars)
		/// from a data byte array which is returned without the stripped prefix
		/// </summary>
		/// ----------------------------------------------------------------------------------------

		public static string GetPrefixStringFromData(ref byte[] data)
		{
			string decryptedDataString = Encoding.UTF8.GetString(GetPrefixDataFromData(ref data, PREFIX_LENGTH));
			return decryptedDataString;
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Helper method for stripping (and returning) a prefix byte array of prefixLength
		/// from a data byte array which is returned without the stripped prefix
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		public static byte[] GetPrefixDataFromData(ref byte[] data, int prefixLength)
		{
			// Get prefix with indicated length
			byte[] prefixData = new byte[prefixLength];
			Buffer.BlockCopy(data, 0, prefixData, 0, prefixLength);

			// Get data without the stripped prefix
			byte[] dataStrippedKey = new byte[data.Length - prefixLength];
			Buffer.BlockCopy(data, prefixLength, dataStrippedKey, 0, data.Length - prefixLength);
			data = dataStrippedKey;

			return prefixData;
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Read SEB settings from UserDefaults and encrypt them using provided security credentials
		/// </summary>
		/// ----------------------------------------------------------------------------------------

		public static byte[] EncryptSEBSettingsWithCredentials(string settingsPassword, bool passwordIsHash, X509Certificate2 certificateRef, SebSettings.sebConfigPurposes configPurpose, bool forEditing)
		{
			// Get current settings dictionary and clean it from empty arrays and dictionaries
			//DictObj cleanedCurrentSettings = SEBSettings.CleanSettingsDictionary();

			// Serialize preferences dictionary to an XML string
			string sebXML = PropertyList.writeXml(SebSettings.settingsCurrent);
			string cleanedSebXML = sebXML.Replace("<array />", "<array></array>");
			cleanedSebXML = cleanedSebXML.Replace("<dict />", "<dict></dict>");
			cleanedSebXML = cleanedSebXML.Replace("<data />", "<data></data>");

			byte[] encryptedSebData = Encoding.UTF8.GetBytes(cleanedSebXML);

			string encryptingPassword = null;

			// Check for special case: .seb configures client, empty password
			if(String.IsNullOrEmpty(settingsPassword) && configPurpose == SebSettings.sebConfigPurposes.sebConfigPurposeConfiguringClient)
			{
				encryptingPassword = "";
			}
			else
			{
				// in all other cases:
				// Check if no password entered and no identity selected
				if(String.IsNullOrEmpty(settingsPassword) && certificateRef == null)
				{
					if(SebMessageBox.Show(SEBUIStrings.noEncryptionChosen, SEBUIStrings.noEncryptionChosenSaveUnencrypted, MessageBoxImage.Question, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
					{
						// OK: save .seb config data unencrypted
						return encryptedSebData;
					}
					else
					{
						return null;
					}
				}
			}
			// gzip the serialized XML data
			encryptedSebData = GZipByte.Compress(encryptedSebData);

			// Check if password for encryption is provided and use it then
			if(!String.IsNullOrEmpty(settingsPassword))
			{
				encryptingPassword = settingsPassword;
			}
			// So if password is empty (special case) or provided
			if(!(encryptingPassword == null))
			{
				// encrypt with password
				encryptedSebData = EncryptDataUsingPassword(encryptedSebData, encryptingPassword, passwordIsHash, configPurpose);
			}
			else
			{
				// Create byte array large enough to hold prefix and data
				byte[] encryptedData = new byte[encryptedSebData.Length + PREFIX_LENGTH];

				// if no encryption with password: Add a 4-char prefix identifying plain data
				string prefixString = PLAIN_DATA_MODE;
				Buffer.BlockCopy(Encoding.UTF8.GetBytes(prefixString), 0, encryptedData, 0, PREFIX_LENGTH);
				// append plain data
				Buffer.BlockCopy(encryptedSebData, 0, encryptedData, PREFIX_LENGTH, encryptedSebData.Length);
				encryptedSebData = (byte[])encryptedData.Clone();
			}
			// Check if cryptographic identity for encryption is selected
			if(certificateRef != null)
			{
				// Encrypt preferences using a cryptographic identity
				encryptedSebData = EncryptDataUsingIdentity(encryptedSebData, certificateRef);
			}

			// gzip the encrypted data
			encryptedSebData = GZipByte.Compress(encryptedSebData);

			return encryptedSebData;
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Encrypt preferences using a certificate
		/// </summary>
		/// ----------------------------------------------------------------------------------------

		public static byte[] EncryptDataUsingIdentity(byte[] data, X509Certificate2 certificateRef)
		{
			//get public key hash from selected identity's certificate
			byte[] publicKeyHash = SebProtectionController.GetPublicKeyHashFromCertificate(certificateRef);

			//encrypt data using public key
			byte[] encryptedData = SebProtectionController.EncryptDataWithCertificate(data, certificateRef);

			// Create byte array large enough to hold prefix, public key hash and encrypted data
			byte[] encryptedSebData = new byte[encryptedData.Length + PREFIX_LENGTH + publicKeyHash.Length];
			// Copy prefix indicating data has been encrypted with a public key identified by hash into out data
			string prefixString = PUBLIC_KEY_HASH_MODE;
			Buffer.BlockCopy(Encoding.UTF8.GetBytes(prefixString), 0, encryptedSebData, 0, PREFIX_LENGTH);
			// Copy public key hash to out data
			Buffer.BlockCopy(publicKeyHash, 0, encryptedSebData, PREFIX_LENGTH, publicKeyHash.Length);
			// Copy encrypted data to out data
			Buffer.BlockCopy(encryptedData, 0, encryptedSebData, PREFIX_LENGTH + publicKeyHash.Length, encryptedData.Length);

			return encryptedSebData;
		}


		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Encrypt preferences using a password
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		// Encrypt preferences using a password
		public static byte[] EncryptDataUsingPassword(byte[] data, string password, bool passwordIsHash, SebSettings.sebConfigPurposes configPurpose)
		{
			string prefixString;
			// Check if .seb file should start exam or configure client
			if(configPurpose == SebSettings.sebConfigPurposes.sebConfigPurposeStartingExam)
			{
				// prefix string for starting exam: normal password will be prompted
				prefixString = PASSWORD_MODE;
			}
			else
			{
				// prefix string for configuring client: configuring password will either be hashed admin pw on client
				// or if no admin pw on client set: empty pw
				prefixString = PASSWORD_CONFIGURING_CLIENT_MODE;
				if(!String.IsNullOrEmpty(password) && !passwordIsHash)
				{
					//empty password means no admin pw on clients and should not be hashed
					//or we got already a hashed admin pw as settings pw, then we don't hash again
					password = SebProtectionController.ComputePasswordHash(password);
				}
			}
			byte[] encryptedData = SebProtectionController.EncryptDataWithPassword(data, password);
			// Create byte array large enough to hold prefix and data
			byte[] encryptedSebData = new byte[encryptedData.Length + PREFIX_LENGTH];
			Buffer.BlockCopy(Encoding.UTF8.GetBytes(prefixString), 0, encryptedSebData, 0, PREFIX_LENGTH);
			Buffer.BlockCopy(encryptedData, 0, encryptedSebData, PREFIX_LENGTH, encryptedData.Length);

			return encryptedSebData;
		}

	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Compressing and decompressing byte arrays using gzip
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public static class GZipByte
	{
		public static byte[] Compress(byte[] input)
		{
			using(MemoryStream output = new MemoryStream())
			{
				using(GZipStream zip = new GZipStream(output, CompressionMode.Compress))
				{
					zip.Write(input, 0, input.Length);
				}
				return output.ToArray();
			}
		}

		public static byte[] Decompress(byte[] input)
		{
			try
			{
				using(GZipStream stream = new GZipStream(new MemoryStream(input),
							  CompressionMode.Decompress))
				{
					const int size = 4096;
					byte[] buffer = new byte[size];
					using(MemoryStream output = new MemoryStream())
					{
						int count = 0;
						do
						{
							count = stream.Read(buffer, 0, size);
							if(count > 0)
							{
								output.Write(buffer, 0, count);
							}
						}
						while(count > 0);
						return output.ToArray();
					}
				}
			}
			catch(Exception)
			{
				return null;
			}
		}
	}
}

