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
        /// Decrypt, deserialize and store new settings as current SEB settings
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public static bool StoreDecryptedSEBSettings(byte[] sebData)
        {
            DictObj sebPreferencesDict;
            string sebFilePassword = null;
            bool passwordIsHash = false;
            X509Certificate2 sebFileCertificateRef = null;

            sebPreferencesDict = DecryptSEBSettings(sebData, false, ref sebFilePassword, ref passwordIsHash, ref sebFileCertificateRef);
            if (sebPreferencesDict == null) return false; //Decryption didn't work, we abort

            // Reset SEB, close third party applications
            SEBClientInfo.SebWindowsClientForm.closeSebClient = false;
            SEBClientInfo.SebWindowsClientForm.CloseSEBForm();
            SEBClientInfo.SebWindowsClientForm.closeSebClient = true;

            if ((int)sebPreferencesDict[SEBSettings.KeySebConfigPurpose] == (int)SEBSettings.sebConfigPurposes.sebConfigPurposeStartingExam)
            {

                /// If these SEB settings are ment to start an exam

                // Store decrypted settings
                SEBSettings.StoreSebClientSettings(sebPreferencesDict);

                // Set the flag that SEB is running in exam mode now
                SEBClientInfo.examMode = true;

                // Re-Initialize SEB according to the new settings
                //if (!SebWindowsClientMain.InitSebDesktop()) return false;

                //return if initializing SEB with openend preferences was successful
                return SEBClientInfo.SebWindowsClientForm.OpenSEBForm();
            }
            else
            {

                /// If these SEB settings are ment to configure a client

                // Write values from .seb config file to the local preferences (shared UserDefaults)

                // Store decrypted settings
                SEBSettings.StoreSebClientSettings(sebPreferencesDict);

                // Write new settings to the localapp directory
                SEBSettings.WriteSebConfigurationFile(SEBClientInfo.SebClientSettingsLocalAppDataFile, "", false, null, SEBSettings.sebConfigPurposes.sebConfigPurposeConfiguringClient);

                // Re-Initialize SEB according to the new settings
                //if (!SebWindowsClientMain.InitSebDesktop()) return false;
                if (SEBClientInfo.SebWindowsClientForm.OpenSEBForm())
                {
                    // Activate SebWindowsClient so the message box gets focus
                    //SEBClientInfo.SebWindowsClientForm.Activate();

                    if (!SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.sebReconfigured, SEBUIStrings.sebReconfiguredQuestion, SEBGlobalConstants.IND_MESSAGE_KIND_QUESTION, MessageBoxButtons.YesNo))
                    {
                        SEBClientInfo.SebWindowsClientForm.closeSebClient = true;
                        Application.Exit();
                    }

                    return true; //reading preferences was successful
                }
                else
                {
                    return false;
                }
            }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Decrypt and deserialize SEB settings
        /// When forEditing = true, then the decrypting password the user entered and/or 
        /// certificate reference found in the .seb file is returned 
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public static DictObj DecryptSEBSettings(byte[] sebData, bool forEditing, ref string sebFilePassword, ref bool passwordIsHash, ref X509Certificate2 sebFileCertificateRef)
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
                sebData = DecryptDataWithPublicKeyHashPrefix(sebData, forEditing, ref sebFileCertificateRef);
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
                string password;
                // Allow up to 5 attempts for entering decoding password
                string enterPasswordString = SEBUIStrings.enterPassword;
                int i = 5;
                do {
                    i--;
                    // Prompt for password
                    password = ShowPasswordDialogForm(SEBUIStrings.loadingSettings, enterPasswordString);
                    if (password == null) return null;
                    //error = nil;
                    sebDataDecrypted = SEBProtectionController.DecryptDataWithPassword(sebData, password);
                    enterPasswordString = SEBUIStrings.enterPasswordAgain;
                    // in case we get an error we allow the user to try it again
                } while ((sebDataDecrypted == null) && i>0);
                if (sebDataDecrypted == null) {
                    //wrong password entered in 5th try: stop reading .seb file
                    SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.decryptingSettingsFailed, SEBUIStrings.decryptingSettingsFailedReason, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, MessageBoxButtons.OK);
                    return null;
                }
                sebData = sebDataDecrypted;
                // If these settings are being decrypted for editing, we return the decryption password
                if (forEditing) sebFilePassword = password;
            } else {
        
                // Prefix = pwcc ("Password Configuring Client") ?
        
                if (prefixString.CompareTo(PASSWORD_CONFIGURING_CLIENT_MODE) == 0) {
            
                    // Decrypt with password and configure local client settings
                    // and quit afterwards, returning if reading the .seb file was successfull
                    DictObj sebSettings = DecryptDataWithPasswordAndConfigureClient(sebData, forEditing, ref sebFilePassword, ref passwordIsHash);
                    return sebSettings;
            
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
                            SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.settingsNotUsable, SEBUIStrings.settingsNotUsableReason, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, MessageBoxButtons.OK);
                            return null;
                        }
                    }
                }
            }
            // If we don't deal with an unencrypted seb file
            // ungzip the .seb (according to specification >= v14) decrypted serialized XML plist data
            if (prefixString.CompareTo(UNENCRYPTED_MODE) != 0) sebData = GZipByte.Decompress(sebData);

            DictObj sebPreferencesDict = null;
            try
            {
                // Get preferences dictionary from decrypted data
                sebPreferencesDict = (DictObj)Plist.readPlist(sebData);

                // We need to set the right value for the key sebConfigPurpose to know later where to store the new settings
                sebPreferencesDict[SEBSettings.KeySebConfigPurpose] = (int)SEBSettings.sebConfigPurposes.sebConfigPurposeStartingExam;

            }
            catch (Exception readPlistException)
            {
                SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.loadingSettingsFailed, SEBUIStrings.loadingSettingsFailedReason, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, MessageBoxButtons.OK);
                Console.WriteLine(readPlistException.Message);
                return null;
            }
            // In editing mode, the user has to enter the right SEB administrator password used in those settings before he can access their contents
            if (forEditing)
            {
                string sebFileHashedAdminPassword = (string)SEBSettings.valueForDictionaryKey(sebPreferencesDict, SEBSettings.KeyHashedAdminPassword);
                // If there was no admin password set in these settings, the user can access them anyways
                if (!String.IsNullOrEmpty(sebFileHashedAdminPassword))
                {
                    // We have to ask for the SEB administrator password used in the settings 
                    // and allow opening settings only if the user enters the right one
                    // Allow up to 5 attempts for entering  admin password
                    int i = 5;
                    string password = null;
                    string hashedPassword;
                    string enterPasswordString = SEBUIStrings.enterAdminPasswordRequired;
                    bool passwordsMatch;
                    do
                    {
                        i--;
                        // Prompt for password
                        password = ShowPasswordDialogForm(SEBUIStrings.loadingSettings, enterPasswordString);
                        // If cancel was pressed, abort
                        if (password == null) return null;
                        hashedPassword = SEBProtectionController.ComputePasswordHash(password);
                        if (String.Compare(sebFileHashedAdminPassword, hashedPassword, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            passwordsMatch = true;
                        }
                        else
                        {
                            passwordsMatch = false;
                        }
                        // in case we get an error we allow the user to try it again
                        enterPasswordString = SEBUIStrings.enterAdminPasswordRequiredAgain;
                    } while ((password == null || !passwordsMatch) && i > 0);
                    if (!passwordsMatch)
                    {
                        //wrong password entered in 5th try: stop reading .seb file
                        SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.loadingSettingsFailed, SEBUIStrings.loadingSettingsFailedWrongAdminPwd, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, MessageBoxButtons.OK);
                        return null;
                    }
                }
            }
            // Reading preferences was successful!
            return sebPreferencesDict; 
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
        private static DictObj DecryptDataWithPasswordAndConfigureClient(byte[] sebData, bool forEditing, ref string sebFilePassword, ref bool passwordIsHash)
        {
            passwordIsHash = false;
            string password = null;
            // First try to decrypt with the current admin password
            // get admin password hash
            string hashedAdminPassword = (string)SEBSettings.valueForDictionaryKey(SEBSettings.settingsCurrent, SEBSettings.KeyHashedAdminPassword);
            if (hashedAdminPassword == null) hashedAdminPassword = @"";
            // We use always uppercase letters in the base16 hashed admin password used for encrypting
            hashedAdminPassword = hashedAdminPassword.ToUpper();
            DictObj sebPreferencesDict = null;
            byte[] decryptedSebData = SEBProtectionController.DecryptDataWithPassword(sebData, hashedAdminPassword);
            if (decryptedSebData == null)
            {
                // If decryption with admin password didn't work, try it with an empty password
                decryptedSebData = SEBProtectionController.DecryptDataWithPassword(sebData, "");
                if (decryptedSebData != null)
                {
                    // Decrypting with empty password worked
                    // Ungzip the .seb (according to specification >= v14) decrypted serialized XML plist data
                    decryptedSebData = GZipByte.Decompress(decryptedSebData);

                    // Check if the openend reconfiguring seb file has the same admin password inside like the current one
                    
                    try
                    {
                        sebPreferencesDict = (DictObj)Plist.readPlist(decryptedSebData);
                    }
                    catch (Exception readPlistException)
                    {
                        // We abort reading the new settings here
                        SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.loadingSettingsFailed, SEBUIStrings.loadingSettingsFailedReason, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, MessageBoxButtons.OK);
                        Console.WriteLine(readPlistException.Message);
                        return null;
                    }
                    string sebFileHashedAdminPassword = (string)SEBSettings.valueForDictionaryKey(sebPreferencesDict, SEBSettings.KeyHashedAdminPassword);
                    if (String.Compare(hashedAdminPassword, sebFileHashedAdminPassword, StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        //No: The admin password inside the .seb file wasn't the same like the current one
                        //now we have to ask for the current admin password and
                        //allow reconfiguring only if the user enters the right one
                        // Allow up to 5 attempts for entering current admin password
                        int i = 5;
                        password = null;
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
                            if (String.Compare(hashedPassword, sebFileHashedAdminPassword, StringComparison.OrdinalIgnoreCase) == 0)
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
                            SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.reconfiguringLocalSettingsFailed, SEBUIStrings.reconfiguringLocalSettingsFailedWrongAdminPwd, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, MessageBoxButtons.OK);
                            return null;
                        }
                    }

                }
                else
                {
                    // If decryption with empty password didn't work, ask for the password the .seb file was encrypted with
                    // Allow up to 5 attempts for entering decoding password
                    int i = 5;
                    password = null;
                    string enterPasswordString = SEBUIStrings.enterEncryptionPassword;
                    do
                    {
                        i--;
                        // Prompt for password
                        password = ShowPasswordDialogForm(SEBUIStrings.reconfiguringLocalSettings, enterPasswordString);
                        // If cancel was pressed, abort
                        if (password == null) return null;
                        string hashedPassword = SEBProtectionController.ComputePasswordHash(password);
                        // we try to decrypt with the hashed password
                        decryptedSebData = SEBProtectionController.DecryptDataWithPassword(sebData, hashedPassword);
                        // in case we get an error we allow the user to try it again
                        enterPasswordString = SEBUIStrings.enterEncryptionPasswordAgain;
                    } while (decryptedSebData == null && i > 0);
                    if (decryptedSebData == null)
                    {
                        //wrong password entered in 5th try: stop reading .seb file
                        SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.reconfiguringLocalSettingsFailed, SEBUIStrings.reconfiguringLocalSettingsFailedWrongPassword, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, MessageBoxButtons.OK);
                        return null;
                    }
                    else
                    {
                        // Decrypting with entered password worked: We save it for returning it later
                        if (forEditing) sebFilePassword = password;
                    }
                }
            }
            else
            {
                //decrypting with hashedAdminPassword worked: we save it for returning as decryption password if settings are meant for editing
                if (forEditing) {
                    sebFilePassword = hashedAdminPassword;
                    // identify that password as hash
                    passwordIsHash = true;
                }

            }
            /// Decryption worked
            // If we don't have the dictionary yet from above
            if (sebPreferencesDict == null)
            {
                sebData = decryptedSebData;
                // Ungzip the .seb (according to specification >= v14) decrypted serialized XML plist data
                sebData = GZipByte.Decompress(sebData);

                try
                {
                    // Get preferences dictionary from decrypted data
                    sebPreferencesDict = (DictObj)Plist.readPlist(sebData);
                }
                catch (Exception readPlistException)
                {
                    SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.loadingSettingsFailed, SEBUIStrings.loadingSettingsFailedReason, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, MessageBoxButtons.OK);
                    Console.WriteLine(readPlistException.Message);
                    return null;
                }
                // In editing mode, the user has to enter the right SEB administrator password before he can access the settings contents
                if (forEditing)
                {
                    string sebFileHashedAdminPassword = (string)sebPreferencesDict[SEBSettings.KeyHashedAdminPassword];
                    if (!String.IsNullOrEmpty(sebFileHashedAdminPassword))
                    {
                        // We have to ask for the current SEB administrator password and
                        // allow opening settings only if the user enters the right one
                        // Allow up to 5 attempts for entering  admin password
                        int i = 5;
                        password = null;
                        string hashedPassword;
                        string enterPasswordString = SEBUIStrings.enterAdminPasswordRequired;
                        bool passwordsMatch;
                        do
                        {
                            i--;
                            // Prompt for password
                            password = ShowPasswordDialogForm(SEBUIStrings.loadingSettings, enterPasswordString);
                            // If cancel was pressed, abort
                            if (password == null) return null;
                            hashedPassword = SEBProtectionController.ComputePasswordHash(password);
                            if (String.Compare(sebFileHashedAdminPassword, hashedPassword, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                passwordsMatch = true;
                            }
                            else
                            {
                                passwordsMatch = false;
                            }
                            // in case we get an error we allow the user to try it again
                            enterPasswordString = SEBUIStrings.enterAdminPasswordRequiredAgain;
                        } while ((password == null || !passwordsMatch) && i > 0);
                        if (!passwordsMatch)
                        {
                            //wrong password entered in 5th try: stop reading .seb file
                            SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.loadingSettingsFailed, SEBUIStrings.loadingSettingsFailedWrongAdminPwd, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, MessageBoxButtons.OK);
                            return null;
                        }
                    }
                }
            }
            // We need to set the right value for the key sebConfigPurpose to know later where to store the new settings
            sebPreferencesDict[SEBSettings.KeySebConfigPurpose] = (int)SEBSettings.sebConfigPurposes.sebConfigPurposeConfiguringClient;

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

        private static byte[] DecryptDataWithPublicKeyHashPrefix(byte[] sebData, bool forEditing, ref X509Certificate2 sebFileCertificateRef)
        {
            // Get 20 bytes public key hash prefix
            // and remaining data with the prefix stripped
            byte[] publicKeyHash = GetPrefixDataFromData(ref sebData, PUBLIC_KEY_HASH_LENGTH);
    
            X509Certificate2 certificateRef = SEBProtectionController.GetCertificateFromStore(publicKeyHash);
            if (certificateRef == null) {
                SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.errorDecryptingSettings, SEBUIStrings.certificateNotFoundInStore, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, MessageBoxButtons.OK);
                return null;
            }
            // If these settings are being decrypted for editing, we will return the decryption certificate reference
            // in the variable which was passed as reference when calling this method
            if (forEditing) sebFileCertificateRef = certificateRef;

            sebData = SEBProtectionController.DecryptDataWithCertificate(sebData, certificateRef);
    
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
            sebPasswordDialogForm.txtSEBPassword.Focus();
            // If we are running in SebWindowsClient we need to activate it before showing the password dialog
            if (SEBClientInfo.SebWindowsClientForm != null) SebWindowsClientForm.SEBToForeground(); //SEBClientInfo.SebWindowsClientForm.Activate();
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

        /// Generate Encrypted .seb Settings Data

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Read SEB settings from UserDefaults and encrypt them using provided security credentials
        /// </summary>
        /// ----------------------------------------------------------------------------------------

        public static byte[] EncryptSEBSettingsWithCredentials(string settingsPassword, bool passwordIsHash, X509Certificate2 certificateRef, SEBSettings.sebConfigPurposes configPurpose)
        {
            // Get current settings dictionary and clean it from empty arrays and dictionaries
            //DictObj cleanedCurrentSettings = SEBSettings.CleanSettingsDictionary();

            // Serialize preferences dictionary to an XML string
            string sebXML = Plist.writeXml(SEBSettings.settingsCurrent);
            string cleanedSebXML = sebXML.Replace("<array />", "<array></array>");
            cleanedSebXML = cleanedSebXML.Replace("<dict />", "<dict></dict>");
            cleanedSebXML = cleanedSebXML.Replace("<data />", "<data></data>");

            byte[] encryptedSebData = Encoding.UTF8.GetBytes(cleanedSebXML);

            string encryptingPassword = null;

            // Check for special case: .seb configures client, empty password
            if (String.IsNullOrEmpty(settingsPassword) && configPurpose == SEBSettings.sebConfigPurposes.sebConfigPurposeConfiguringClient)
            {
                encryptingPassword = "";
            }
            else
            {
                // in all other cases:
                // Check if no password entered and no identity selected
                if (String.IsNullOrEmpty(settingsPassword) && certificateRef == null)
                {
                    if (SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.noEncryptionChosen, SEBUIStrings.noEncryptionChosenSaveUnencrypted, SEBGlobalConstants.IND_MESSAGE_KIND_QUESTION, MessageBoxButtons.YesNo))
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
            if (!String.IsNullOrEmpty(settingsPassword))
            {
                encryptingPassword = settingsPassword;
            }
            // So if password is empty (special case) or provided
            if (!(encryptingPassword == null))
            {
                // encrypt with password
                encryptedSebData = EncryptDataUsingPassword(encryptedSebData, encryptingPassword, passwordIsHash, configPurpose);
            }
            else
            {
                // Create byte array large enough to hold prefix and data
                byte[] encryptedData = new byte [encryptedSebData.Length + PREFIX_LENGTH];
 
                // if no encryption with password: Add a 4-char prefix identifying plain data
                string prefixString = PLAIN_DATA_MODE;
                Buffer.BlockCopy(Encoding.UTF8.GetBytes(prefixString), 0, encryptedData, 0, PREFIX_LENGTH);
                // append plain data
                Buffer.BlockCopy(encryptedSebData, 0, encryptedData, PREFIX_LENGTH, encryptedSebData.Length);
                encryptedSebData = (byte[])encryptedData.Clone();
            }
            // Check if cryptographic identity for encryption is selected
            if (certificateRef != null)
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
            byte[] publicKeyHash = SEBProtectionController.GetPublicKeyHashFromCertificate(certificateRef);

            //encrypt data using public key
            byte[] encryptedData = SEBProtectionController.EncryptDataWithCertificate(data, certificateRef);

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
        public static byte[] EncryptDataUsingPassword(byte[] data, string password, bool passwordIsHash, SEBSettings.sebConfigPurposes configPurpose)
        {
            string prefixString;
            // Check if .seb file should start exam or configure client
            if (configPurpose == SEBSettings.sebConfigPurposes.sebConfigPurposeStartingExam)
            {
                // prefix string for starting exam: normal password will be prompted
                prefixString = PASSWORD_MODE;
            }
            else
            {
                // prefix string for configuring client: configuring password will either be hashed admin pw on client
                // or if no admin pw on client set: empty pw
                prefixString = PASSWORD_CONFIGURING_CLIENT_MODE;
                if (!String.IsNullOrEmpty(password) && !passwordIsHash)
                {
                    //empty password means no admin pw on clients and should not be hashed
                    //or we got already a hashed admin pw as settings pw, then we don't hash again
                    password = SEBProtectionController.ComputePasswordHash(password);
                }
            }
            byte[] encryptedData = SEBProtectionController.EncryptDataWithPassword(data, password);
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

