using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using SebWindowsClient.CryptographyUtils;
using SebWindowsClient.ConfigurationUtils;
using DictObj = System.Collections.Generic.Dictionary<string, object>;
using PlistCS;


namespace SebWindowsClient.ConfigurationUtils
{
    public class SEBConfigFileManager
    {
        public static SebPasswordDialogForm sebPasswordDialogForm;

        // Prefixes
        private const int PREFIX_LENGTH = 4;
        private const string PUBLIC_KEY_HASH_MODE = "pkhs";
        private const string PASSWORD_MODE = "pswd";
        private const string PLAIN_DATA_MODE = "plnd";
        private const string PASSWORD_CONFIGURING_CLIENT_MODE = "pwcc";
        private const string UNENCRYPTED_MODE = "<?xm";

        // Public key hash identifier length
        private const int PUBLIC_KEY_HASH_LENGTH = 20;

        // Initializing
        public static void InitSEBConfigFileManager()
        {
            // Initialize the password entry dialog form
            if (sebPasswordDialogForm == null)
            {
                sebPasswordDialogForm = new SebPasswordDialogForm();
                sebPasswordDialogForm.TopMost = true;
                //sebPasswordDialogForm.Show();
                //sebPasswordDialogForm.Visible = false;
            }
        }


        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Decrypt, parse and store SEB settings
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public static bool StoreDecryptedSEBSettings(byte[] sebData)
        {
            DictObj sebPreferencesDict;

            sebPreferencesDict = DecryptSEBSettings(sebData);
             if (sebPreferencesDict == null) return false; //Decryption didn't work, we abort

             if ((int)SEBSettings.settingsCurrent[SEBSettings.KeySebConfigPurpose] == (int)SEBSettings.sebConfigPurposes.sebConfigPurposeStartingExam)
             {

                 /// If these SEB settings are ment to start an exam

                 // Switch to private UserDefaults (saved non-persistantly in memory instead in ~/Library/Preferences)
                 //[NSUserDefaults setUserDefaultsPrivate:YES];

                 // Write values from .seb config file to the local preferences (shared UserDefaults)
                 //[self saveIntoUserDefaults:sebPreferencesDict];

                 //[self.sebController.preferencesController initPreferencesWindow];

                 return true; //reading preferences was successful

             }
             else
             {

                 /// If these SEB settings are ment to configure a client

                 // Write values from .seb config file to the local preferences (shared UserDefaults)

                 SEBErrorMessages.OutputErrorMessage(SEBGlobalConstants.IND_CLIENT_SETTINGS_RECONFIGURED, SEBGlobalConstants.IND_MESSAGE_KIND_QUESTION);
                 //int answer = NSRunAlertPanel(NSLocalizedString(@"SEB Re-Configured",nil), NSLocalizedString(@"Local settings of SEB have been reconfigured. Do you want to start working with SEB now or quit?",nil),
                 //                             NSLocalizedString(@"Continue",nil), NSLocalizedString(@"Quit",nil), nil);
                 //switch(answer)
                 //{
                 //    case NSAlertDefaultReturn:
                 //        break; //Cancel: don't quit
                 //    default:
                 //SEBClientInfo.SebWindowsClientForm.closeSebClient = true;
                 //Application.Exit();

                 //}

                 return true; //reading preferences was successful
             }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Decrypt, parse and save SEB settings
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public static DictObj DecryptSEBSettings(byte[] sebData)
        {
            // Ungzip the .seb (according to specification >= v14) source data
            byte[] unzippedSebData = GZipByte.Decompress(sebData);

            // if unzipped data is not null, then unzipping worked, we use unzipped data
            // if unzipped data is null, then the source data may be an uncompressed .seb file, we proceed with it
            if (unzippedSebData != null) sebData = unzippedSebData;

            string prefixString;

            // save the data including the first 4 bytes for the case that it's acutally an unencrypted XML plist
            byte [] sebDataUnencrypted = sebData.Clone() as byte[];

            // Get 4-char prefix
            prefixString = GetPrefixStringFromData(ref sebData);

            //// Check prefix identifying encryption modes

            // Prefix = pkhs ("Public Key Hash") ?
    
            if (prefixString.CompareTo(PUBLIC_KEY_HASH_MODE) == 0) {

                // Decrypt with cryptographic identity/private key
                sebData = DecryptDataWithPublicKeyHashPrefix(sebData);
                if (sebData == null) {
                    return null;
                }

                // Get 4-char prefix again
                // and remaining data without prefix, which is either plain or still encoded with password
                prefixString = GetPrefixStringFromData(ref sebData);
            }
    
            // Prefix = pswd ("Password") ?
    
            if (prefixString.CompareTo(PASSWORD_MODE) == 0) {

                // Decrypt with password
                // if the user enters the right one
                byte [] sebDataDecrypted = null;
                // Allow up to 5 attempts for entering decoding password
                string enterPasswordString = SEBUIStrings.enterPassword;
                int i = 5;
                do {
                    i--;
                    // Prompt for password
                    string password = ShowPasswordDialogForm(SEBUIStrings.loadingSettings, enterPasswordString);
                    if (password == null) return null;
                    //error = nil;
                    sebDataDecrypted = SEBProtectionController.DecryptWithPassword(sebData, password);
                    enterPasswordString = SEBUIStrings.enterPasswordAgain;
                    // in case we get an error we allow the user to try it again
                } while ((sebDataDecrypted == null) && i>0);
                if (sebDataDecrypted == null) {
                    //wrong password entered in 5th try: stop reading .seb file
                    SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.decryptingSettingsFailed, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                    return null;
                }
                sebData = sebDataDecrypted;
            } else {
        
                // Prefix = pwcc ("Password Configuring Client") ?
        
                if (prefixString.CompareTo(PASSWORD_CONFIGURING_CLIENT_MODE) == 0) {
            
                    // Decrypt with password and configure local client settings
                    // and quit afterwards, returning if reading the .seb file was successfull
                    return DecryptDataWithPasswordAndConfigureClient(sebData);
            
                } else {

                    // Prefix = plnd ("Plain Data") ?
            
                    if (prefixString.CompareTo(PLAIN_DATA_MODE) != 0) {
                        // No valid 4-char prefix was found in the .seb file
                        // Check if .seb file is unencrypted
                        if (prefixString.CompareTo(UNENCRYPTED_MODE) == 0) {
                            // .seb file seems to be an unencrypted XML plist
                            // get the original data including the first 4 bytes
                            sebData = sebDataUnencrypted;
                        } else {
                            // No valid prefix and no unencrypted file with valid header
                            // cancel reading .seb file
                            SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.settingsNotUsable, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                            return null;
                        }
                    }
                }
            }
    
            //if decrypting wasn't successfull then stop here
            if (sebData == null) {
                SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.decryptingSettingsFailed, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                return null;
            }
            // If we don't deal with an unencrypted seb file
            // ungzip the .seb (according to specification >= v14) decrypted serialized XML plist data
            if (prefixString.CompareTo(UNENCRYPTED_MODE) != 0) sebData = GZipByte.Decompress(sebData);

            try
            {
                // Get preferences dictionary from decrypted data
                DictObj sebPreferencesDict = (DictObj)Plist.readPlist(sebData);

                // We need to set the right value for the key sebConfigPurpose to know later where to store the new settings
                SEBSettings.settingsCurrent[SEBSettings.KeySebConfigPurpose] = (int)SEBSettings.sebConfigPurposes.sebConfigPurposeStartingExam;

                // Reading preferences was successful!
                return sebPreferencesDict; 
            }
            catch (Exception readPlistException)
            {
                SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.loadingSettingsFailed, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                Console.WriteLine(readPlistException.Message);
                return null;
            }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Helper method which decrypts the byte array using an empty password, 
        /// the administrator password currently set in SEB 
        /// or asks for the password used for encrypting this SEB file
        /// then the local client settings in the LOCALAPPDATA folder 
        /// are replaced with those new settings 
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private static DictObj DecryptDataWithPasswordAndConfigureClient(byte [] sebData)
        {
            // First try to decrypt with the current admin password
            // get admin password hash
            //string hashedAdminPassword = (string)SEBSettings.settingsCurrent[SEBSettings.KeyHashedAdminPassword];
            string hashedAdminPassword = (string)SEBClientInfo.getSebSetting(SEBSettings.KeyHashedAdminPassword)[SEBSettings.KeyHashedAdminPassword];
            //if (!hashedAdminPassword) hashedAdminPassword = @"";
            DictObj sebPreferencesDict = null;
            byte[] decryptedSebData = SEBProtectionController.DecryptWithPassword(sebData, hashedAdminPassword);
            if (decryptedSebData == null)
            {
                // If decryption with admin password didn't work, try it with an empty password
                decryptedSebData = SEBProtectionController.DecryptWithPassword(sebData, "");
                if (decryptedSebData != null)
                {
                    // Decrypting with empty password worked
                    // Ungzip the .seb (according to specification >= v14) decrypted serialized XML plist data
                    decryptedSebData = GZipByte.Decompress(decryptedSebData);
                    // Check if the openend reconfiguring seb file has the same admin password inside like the current one
                    try
                    {
                        sebPreferencesDict = (DictObj)Plist.readPlist(sebData);
                    }
                    catch (Exception readPlistException)
                    {
                        // We abort reading the new settings here
                        SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.loadingSettingsFailed, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                        Console.WriteLine(readPlistException.Message);
                        return null;
                    }
                    string sebFileHashedAdminPassword = (string)sebPreferencesDict[SEBSettings.KeyHashedAdminPassword];
                    if (String.Compare(hashedAdminPassword, sebFileHashedAdminPassword, StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        //No: The admin password inside the .seb file wasn't the same like the current one
                        //now we have to ask for the current admin password and
                        //allow reconfiguring only if the user enters the right one
                        // Allow up to 5 attempts for entering current admin password
                        int i = 5;
                        string password = null;
                        string hashedPassword;
                        string enterPasswordString = SEBUIStrings.enterCurrentAdminPwdForReconfiguring;
                        bool passwordsMatch;
                        do
                        {
                            i--;
                            // Prompt for password
                            password = ShowPasswordDialogForm(SEBUIStrings.reconfiguringLocalSettings, enterPasswordString);
                            // If cancel was pressed, abort
                            if (password == null) return null;
                            hashedPassword = SEBProtectionController.ComputePasswordHash(password);
                            if (String.Compare(hashedAdminPassword, hashedPassword, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                passwordsMatch = true;
                            }
                            else
                            {
                                passwordsMatch = false;
                            }
                            // in case we get an error we allow the user to try it again
                            enterPasswordString = SEBUIStrings.enterCurrentAdminPwdForReconfiguringAgain;
                        } while ((password == null || !passwordsMatch) && i > 0);
                        if (!passwordsMatch)
                        {
                            //wrong password entered in 5th try: stop reading .seb file
                            SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.decryptingSettingsFailed, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                            return null;
                        }
                    }

                }
                else
                {
                    // If decryption with admin password didn't work, ask for the password the .seb file was encrypted with
                    // Allow up to 5 attempts for entering decoding password
                    int i = 5;
                    string password = null;
                    string enterPasswordString = SEBUIStrings.enterEncryptionPassword;
                    do
                    {
                        i--;
                        // Prompt for password
                        password = ShowPasswordDialogForm(SEBUIStrings.reconfiguringLocalSettings, enterPasswordString);
                        // If cancel was pressed, abort
                        if (password == null) return null;
                        decryptedSebData = SEBProtectionController.DecryptWithPassword(sebData, password);
                        // in case we get an error we allow the user to try it again
                        enterPasswordString = SEBUIStrings.enterEncryptionPasswordAgain;
                    } while (decryptedSebData == null && i > 0);
                    if (decryptedSebData == null)
                    {
                        //wrong password entered in 5th try: stop reading .seb file
                        SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.decryptingSettingsFailed, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                        return null;
                    }
                }
            }

            sebData = decryptedSebData;
            //if decrypting wasn't successfull then stop here

            if (sebData == null)
            {
                SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.decryptingSettingsFailed, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                return null;
            }
            // Decryption worked
            // Ungzip the .seb (according to specification >= v14) decrypted serialized XML plist data
            sebData = GZipByte.Decompress(sebData);

            // If we don't have the dictionary yet from above
            if (sebPreferencesDict == null)
            {
                try
                {
                    // Get preferences dictionary from decrypted data
                    sebPreferencesDict = (DictObj)Plist.readPlist(sebData);

                    // We need to set the right value for the key sebConfigPurpose to know later where to store the new settings
                    SEBSettings.settingsCurrent[SEBSettings.KeySebConfigPurpose] = (int)SEBSettings.sebConfigPurposes.sebConfigPurposeStartingExam;

                    // Reading preferences was successful!
                    return sebPreferencesDict;
                }
                catch (Exception readPlistException)
                {
                    SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.loadingSettingsFailed, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                    Console.WriteLine(readPlistException.Message);
                    return null;
                }

            }
            // We need to set the right value for the key sebConfigPurpose to know later where to store the new settings
            SEBSettings.settingsCurrent[SEBSettings.KeySebConfigPurpose] = (int)SEBSettings.sebConfigPurposes.sebConfigPurposeConfiguringClient;

            // Reading preferences was successful!
            return sebPreferencesDict;
        }


        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Helper method which fetches the public key hash from a byte array, 
        /// retrieves the according cryptographic identity from the certificate store
        /// and returns the decrypted bytes 
        /// </summary>
        /// ----------------------------------------------------------------------------------------

        private static byte[] DecryptDataWithPublicKeyHashPrefix(byte [] sebData)
        {
            // Get 20 bytes public key hash prefix
            // and remaining data with the prefix stripped
            byte[] publicKeyHash = GetPrefixDataFromData(ref sebData, PUBLIC_KEY_HASH_LENGTH);
    
            X509Certificate2 certificateRef = SEBProtectionController.GetCertificateFromStore(publicKeyHash);
            if (certificateRef == null) {
                SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.certificateNotFoundInStore, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                return null;
            }

            sebData = SEBProtectionController.DecryptWithCertificate(sebData, certificateRef);
    
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
        /// Show SEB Password Dialog Form.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public static string ShowPasswordDialogForm(string title, string passwordRequestText)
        {
            // Set the title of the dialog window
            sebPasswordDialogForm.Text = title;
            // Set the text of the dialog
            sebPasswordDialogForm.LabelText = passwordRequestText;
            // Show password dialog as a modal dialog and determine if DialogResult = OK.
            if (sebPasswordDialogForm.ShowDialog() == DialogResult.OK)
            {
                // Read the contents of testDialog's TextBox.
                string password = sebPasswordDialogForm.txtSEBPassword.Text;
                sebPasswordDialogForm.txtSEBPassword.Text = "";
                sebPasswordDialogForm.txtSEBPassword.Focus();
                return password;
            }
            else
            {
                return null;
            }
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
            using (MemoryStream output = new MemoryStream())
            {
                using (GZipStream zip = new GZipStream(output, CompressionMode.Compress))
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
                using (GZipStream stream = new GZipStream(new MemoryStream(input),
                              CompressionMode.Decompress))
                {
                    const int size = 4096;
                    byte[] buffer = new byte[size];
                    using (MemoryStream output = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            count = stream.Read(buffer, 0, size);
                            if (count > 0)
                            {
                                output.Write(buffer, 0, count);
                            }
                        }
                        while (count > 0);
                        return output.ToArray();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}    

