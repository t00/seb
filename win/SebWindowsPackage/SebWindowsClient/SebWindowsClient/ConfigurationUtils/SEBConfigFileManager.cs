using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;
using SebWindowsClient.CryptographyUtils;
using DictObj = System.Collections.Generic.Dictionary<string, object>;
using PlistCS;


namespace SebWindowsClient.ConfigurationUtils
{
    public class SEBConfigFileManager
    {
        // Prefixes
        private const int PREFIX_LENGTH = 4;
        private const string PUBLIC_KEY_HASH_MODE = "pkhs";
        private const string PASSWORD_MODE = "pswd";
        private const string PLAIN_DATA_MODE = "plnd";
        private const string PASSWORD_CONFIGURING_CLIENT_MODE = "pwcc";
        private const string UNENCRYPTED_MODE = "<?xm";

        // Public key hash identifier length
        private const int PUBLIC_KEY_HASH_LENGTH = 20;

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


                 return true; //reading preferences was successful

             }
             else
             {

                 /// If these SEB settings are ment to configure a client

                 // Write values from .seb config file to the local preferences (shared UserDefaults)

                 //int answer = NSRunAlertPanel(NSLocalizedString(@"SEB Re-Configured",nil), NSLocalizedString(@"Local settings of SEB have been reconfigured. Do you want to start working with SEB now or quit?",nil),
                 //                             NSLocalizedString(@"Continue",nil), NSLocalizedString(@"Quit",nil), nil);
                 //switch(answer)
                 //{
                 //    case NSAlertDefaultReturn:
                 //        break; //Cancel: don't quit
                 //    default:
                 //        self.sebController.quittingMyself = TRUE; //SEB is terminating itself
                 //        [NSApp terminate: nil]; //quit SEB
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
                string enterPasswordString = "Enter Password:";
                int i = 5;
                do {
                    i--;
                    // Prompt for password
                    //if ([self.sebController showEnterPasswordDialog:enterPasswordString modalForWindow:nil windowTitle:NSLocalizedString(@"Loading New SEB Settings",nil)] == SEBEnterPasswordCancel) return NO;
                    string password = "seb"; //[self.sebController.enterPassword stringValue];
                    //if (!password) return NO;
                    //error = nil;
                    sebDataDecrypted = SEBProtectionController.DecryptWithPassword(sebData, password);
                    enterPasswordString = "Try again to enter the correct password:";
                    // in case we get an error we allow the user to try it again
                } while ((sebDataDecrypted == null) && i>0);
                if (sebDataDecrypted == null) {
                    //wrong password entered in 5th try: stop reading .seb file
                    //NSRunAlertPanel(NSLocalizedString(@"Cannot Decrypt SEB Settings", nil),
                    //                NSLocalizedString(@"You either entered the wrong password or these settings were saved with an incompatible SEB version.", nil),
                    //                NSLocalizedString(@"OK", nil), nil, nil);
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
                            //NSRunAlertPanel(NSLocalizedString(@"Loading new SEB settings failed!", nil),
                            //                NSLocalizedString(@"This settings file cannot be used. It may have been created by an newer, incompatible version of SEB or it is corrupted.", nil),
                            //                NSLocalizedString(@"OK", nil), nil, nil);
                            return null;
                        }
                    }
                }
            }
    
            //if decrypting wasn't successfull then stop here
            if (sebData == null) return null;
    
            // If we don't deal with an unencrypted seb file
            // ungzip the .seb (according to specification >= v14) decrypted serialized XML plist data
            if (prefixString.CompareTo(UNENCRYPTED_MODE) != 0) sebData = GZipByte.Decompress(sebData);

            try
            {
            // Get preferences dictionary from decrypted data
            DictObj sebPreferencesDict = (DictObj)Plist.readPlist(sebData);
            //Dictionary sebPreferencesDict = [self getPreferencesDictionaryFromConfigData:sebData error:&error];
            //if (error) {
            //    [NSApp presentError:error];
            //    return false; //we abort reading the new settings here
            //}

            // Check if a some value is from a wrong class (another than the value from default settings)
            // and quit reading .seb file if a wrong value was found
            //if (![self checkClassOfSettings:sebPreferencesDict]) return NO;
        
            // Switch to private UserDefaults (saved non-persistantly in memory instead in ~/Library/Preferences)
            //[NSUserDefaults setUserDefaultsPrivate:YES];
    
            // Write values from .seb config file to the local preferences (shared UserDefaults)
            //[self saveIntoUserDefaults:sebPreferencesDict];

            //[self.sebController.preferencesController initPreferencesWindow];
    
            return sebPreferencesDict; //reading preferences was successful
            }
            catch (Exception streamReadException)
            {
                // Let the user know what went wrong
                Console.WriteLine("The .seb file could not be read:");
                Console.WriteLine(streamReadException.Message);
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
            return null;
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
                //NSRunAlertPanel(NSLocalizedString(@"Error Decrypting Settings", nil),
                //                NSLocalizedString(@"The identity needed to decrypt settings has not been found in the keychain!", nil),
                //                NSLocalizedString(@"OK", nil), nil, nil);
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
    }


    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Class for compressing and decompressing byte arrays using gzip
    /// Thanks to K. Jacobson
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
            using (GZipStream stream = new GZipStream(new MemoryStream(input),
                          CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
            /*
            using (MemoryStream output = new MemoryStream(input))
            {
                using (GZipStream zip = new GZipStream(output, CompressionMode.Decompress))
                {
                    List<byte> bytes = new List<byte>();
                    int b = zip.ReadByte();
                    while (b != -1)
                    {
                        bytes.Add((byte)b);
                        b = zip.ReadByte();
                    }
                    return bytes.ToArray();
                }
            }*/
        }
    }
}    

