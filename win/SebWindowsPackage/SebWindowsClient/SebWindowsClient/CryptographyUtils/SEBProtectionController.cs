using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Collections;

namespace SebWindowsClient.CryptographyUtils
{
    public class SEBProtectionController
    {
        // Prefix
        private const int PREFIX_LENGTH = 4;
        private const string PUBLIC_KEY_HASH_MODE = "pkhs";
        private const string PASSWORD_MODE = "pswd";
        private const string PLAIN_DATA_MODE = "plnd";
        private const string PASSWORD_CONFIGURING_CLIENT_MODE = "pwcc";

        // Public key
        private const int PUBLIC_KEY_HASH_LENGTH = 20;

        // Password encryption constants
        private const int PASSWORD_ENCRYPTION_VERSION_LENGTH = 1;
        private const int PASSWORD_ENCRYPTION_OPTIONS_LENGTH = 1;
        private const int PASSWORD_ENCRYPTION_SALT_LENGTH = 8;
        private const int PASSWORD_HMAC_LENGTH = 32;

        enum EncryptionT
        {
            pkhs,
            pswd,
            plnd,
            pwcc,
            unknown
        };

        private EncryptionT _encryptionType;

        private string _keyCertFilename = "";
        private string _keyCertPassword = "";
        private X509Certificate2 x509certificate;
        private RSACryptoServiceProvider rSACryptoServiceProvider;

        private EncryptionT EncryptionType
        {
            get { return _encryptionType; }
            set { _encryptionType = value; }
        }

        public string KeyCertFilename
        {
            get { return _keyCertFilename; }
            set { _keyCertFilename = value; }
        }
        public string KeyCertPassword
        {
            get { return _keyCertPassword; }
            set { _keyCertPassword = value; }
        }


        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Check encrypted data.
        /// Format: 0-3 Prefix
        ///         4-n Encryptet data
        /// Prefix: 1. pkhs 0-3 Prefix
        ///                 4-23 Public key hash
        ///                 24-n Encrypted data  
        ///         2. pswd 0 version
        ///                 1 option
        ///                 2-9 encryption salt
        ///                 10-17 HMAC salt
        ///                 18-33 IV
        ///                 34-n-33 encrypted data
        ///                 n-31-n HMAC
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public string DecryptSebClientSettings(byte[] encryptedData)
        {
            string decryptedDataString = null;
            byte[] encryptedBytesWithPrefix = encryptedData;
            byte[] encryptedBytesWithKey = new byte[encryptedBytesWithPrefix.Length - PREFIX_LENGTH];
            byte[] prefix = new byte[PREFIX_LENGTH];

            Buffer.BlockCopy(encryptedBytesWithPrefix, 0, prefix, 0, PREFIX_LENGTH);
            Buffer.BlockCopy(encryptedBytesWithPrefix, PREFIX_LENGTH, encryptedBytesWithKey, 0, encryptedBytesWithPrefix.Length - PREFIX_LENGTH);

            // Check prefix and set encryption type
            string prefixStr = Encoding.UTF8.GetString(prefix);
            if (prefixStr.CompareTo(PUBLIC_KEY_HASH_MODE) == 0)
            {
                // decrypt settings with private key
                _encryptionType = EncryptionT.pkhs;
                decryptedDataString = DecryptWithCertificate(encryptedBytesWithKey);
            }
            else if (prefixStr.CompareTo(PASSWORD_MODE) == 0)
            {
                // decrypt settings with password
                _encryptionType = EncryptionT.pswd;
                decryptedDataString = DecryptWithPassword(encryptedBytesWithKey, "seb");
            }
            else if (prefixStr.CompareTo(PLAIN_DATA_MODE) == 0)
            {
                _encryptionType = EncryptionT.plnd;
            }
            else if (prefixStr.CompareTo(PASSWORD_CONFIGURING_CLIENT_MODE) == 0)
            {
                _encryptionType = EncryptionT.pwcc;
                decryptedDataString = DecryptWithPassword(encryptedBytesWithKey, "seb");
            }
            else
            {
                _encryptionType = EncryptionT.unknown;
                decryptedDataString = Encoding.UTF8.GetString(encryptedData);
            }

            return decryptedDataString;
        }


        /// ----------------------------------------------------------------------------------------
        /// <summary>
        ///  Get certificate from store.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private X509Certificate2 GetCertificateFromStore(byte[] publicKeyHash)
        {
            string certificateName;
            string certificateHash;
            X509Certificate2 sebCertificate = null;

            //Create new X509 store called teststore from the local certificate store.
            X509Store store = new X509Store(StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            
            foreach (X509Certificate2 x509Certificate in store.Certificates)
            {
                certificateHash = x509Certificate.Thumbprint;
                certificateName = x509Certificate.Subject;
                if (certificateHash == Encoding.ASCII.GetString(publicKeyHash))
                {
                    sebCertificate = x509Certificate;
                }
                if (certificateName.CompareTo("C=CH, CN=SEB-Configuration") == 0)
                {
                    sebCertificate = x509Certificate;
                }
            }
 
            //Close the store.
            store.Close();

            return sebCertificate;
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Decrypt with Public key and RSA Algoritmus.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public string DecryptWithCertificate(byte[] encryptedBytesWithKey)
        {
            int dwKeySize;
            try
            {
                // Get certificate
                byte[] publicKeyHash = new byte[PUBLIC_KEY_HASH_LENGTH];
                Buffer.BlockCopy(encryptedBytesWithKey, 0, publicKeyHash, 0, PUBLIC_KEY_HASH_LENGTH);
                X509Certificate2 sebCertificate = GetCertificateFromStore(publicKeyHash);

                // decrypt config data

                byte[] encryptedDataBytes = new byte[encryptedBytesWithKey.Length - PUBLIC_KEY_HASH_LENGTH];
                Buffer.BlockCopy(encryptedBytesWithKey, PUBLIC_KEY_HASH_LENGTH, encryptedDataBytes, 0, encryptedBytesWithKey.Length - PUBLIC_KEY_HASH_LENGTH);

                RSACryptoServiceProvider privateKey = sebCertificate.PrivateKey as RSACryptoServiceProvider;
                byte[] decryptedData = privateKey.Decrypt(encryptedDataBytes, false);
                string decryptedStr = Encoding.UTF8.GetString(decryptedData);

                //byte[] encryptedDataBytes = new byte[encryptedBytesWithKey.Length - PUBLIC_KEY_HASH_LENGTH];
                //Buffer.BlockCopy(encryptedBytesWithKey, PUBLIC_KEY_HASH_LENGTH, encryptedDataBytes, 0, encryptedBytesWithKey.Length - PUBLIC_KEY_HASH_LENGTH);
                //string encryptedDataString = Encoding.ASCII.GetString(encryptedDataBytes);

                //// TODO: Add Proper Exception Handlers
                //dwKeySize = sebCertificate.PrivateKey.KeySize;
                //RSACryptoServiceProvider rsaCryptoServiceProvider = (RSACryptoServiceProvider)sebCertificate.PrivateKey;
                ////RSACryptoServiceProvider rsaCryptoServiceProvider
                ////                         = new RSACryptoServiceProvider(dwKeySize);
                ////rsaCryptoServiceProvider.FromXmlString(xmlString);
                //int base64BlockSize = ((dwKeySize / 8) % 3 != 0) ?
                //  (((dwKeySize / 8) / 3) * 4) + 4 : ((dwKeySize / 8) / 3) * 4;
                //int iterations = encryptedDataString.Length / base64BlockSize;
                //ArrayList arrayList = new ArrayList();
                //for (int i = 0; i < iterations; i++)
                //{
                //    byte[] encryptedBytes = Convert.FromBase64String(
                //         encryptedDataString.Substring(base64BlockSize * i, base64BlockSize));
                //    // Be aware the RSACryptoServiceProvider reverses the order of 
                //    // encrypted bytes after encryption and before decryption.
                //    // If you do not require compatibility with Microsoft Cryptographic 
                //    // API (CAPI) and/or other vendors.
                //    // Comment out the next line and the corresponding one in the 
                //    // EncryptString function.
                //    Array.Reverse(encryptedBytes);
                //    arrayList.AddRange(rsaCryptoServiceProvider.Decrypt(
                //                        encryptedBytes, true));
                //}
                //return Encoding.UTF8.GetString(arrayList.ToArray(
                //                          Type.GetType("System.Byte")) as byte[]);
                return decryptedStr;
            }
            catch (CryptographicException cex)
            {
                return cex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
          }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Encrypt with Public key and RSA Algoritmus.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public byte[] EncryptWithCertificate(string inputString, X509Certificate2 sebCertificate)
        {
            int dwKeySize;
            StringBuilder stringBuilder = new StringBuilder();
            byte[] dstBytes = Encoding.UTF8.GetBytes(inputString); 

            try
            {
                // TODO: Add Proper Exception Handlers
                dwKeySize = sebCertificate.PublicKey.Key.KeySize;
                RSACryptoServiceProvider rsaCryptoServiceProvider = (RSACryptoServiceProvider)sebCertificate.PublicKey.Key;
                //RSACryptoServiceProvider rsaCryptoServiceProvider =
                //                              new RSACryptoServiceProvider(dwKeySize);
                //rsaCryptoServiceProvider.FromXmlString(xmlString);
                int keySize = dwKeySize / 8;
                byte[] bytes = Encoding.UTF8.GetBytes(inputString);
                

                Buffer.BlockCopy(Encoding.UTF8.GetBytes("pkhs"), 0, dstBytes, 0, 4);
                byte[] publicKeyHash = sebCertificate.GetCertHash();
                Buffer.BlockCopy(publicKeyHash, 0, dstBytes, 4, publicKeyHash.Count());

                // The hash function in use by the .NET RSACryptoServiceProvider here 
                // is SHA1
                // int maxLength = ( keySize ) - 2 - 
                //              ( 2 * SHA1.Create().ComputeHash( rawBytes ).Length );
                int maxLength = keySize - 42;
                int dataLength = bytes.Length;
                int iterations = dataLength / maxLength;
                int dstOffset = 24;
                for (int i = 0; i <= iterations; i++)
                {
                    byte[] tempBytes = new byte[
                            (dataLength - maxLength * i > maxLength) ? maxLength :
                                                          dataLength - maxLength * i];
                    Buffer.BlockCopy(bytes, maxLength * i, tempBytes, 0,
                                      tempBytes.Length);
                    byte[] encryptedBytes = rsaCryptoServiceProvider.Encrypt(tempBytes,
                                                                              true);
                    // Be aware the RSACryptoServiceProvider reverses the order of 
                    // encrypted bytes. It does this after encryption and before 
                    // decryption. If you do not require compatibility with Microsoft 
                    // Cryptographic API (CAPI) and/or other vendors. Comment out the 
                    // next line and the corresponding one in the DecryptString function.
                        //Array.Reverse(encryptedBytes);
                    // Why convert to base 64?
                    // Because it is the largest power-of-two base printable using only 
                    // ASCII characters
                    Buffer.BlockCopy(encryptedBytes, 0, dstBytes, dstOffset, encryptedBytes.Count());
                    dstOffset = dstOffset + encryptedBytes.Count();
                    //stringBuilder.Append(Convert.ToBase64String(encryptedBytes));
                }

                // Insert encoding mode and public key hash
                //byte[] publicKeyHash = new byte[20];
                //byte[] publicKeyHashASCII = Encoding.ASCII.GetBytes(sebCertificate.PublicKey.GetHashCode().ToString());
                //byte[] publicKeyHash = sebCertificate.GetCertHash();
                //byte[] publicKeyHashASCII = Encoding.ASCII.GetBytes(sebCertificate.GetCertHashString());
                //Buffer.BlockCopy(publicKeyHash1, 0, publicKeyHash, 0, publicKeyHash1.Count());
                //string publicKeyHashStr = Encoding.UTF8.GetString(publicKeyHash);
                //stringBuilder.Insert(0, publicKeyHashStr).Insert(0, PUBLIC_KEY_HASH_MODE);
                //return stringBuilder.ToString();
                return dstBytes;
            }
            catch (CryptographicException cex)
            {
                //return cex.Message;
            }
            catch (Exception ex)
            {
                //return ex.Message;
            }
            return dstBytes;
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Encrypt with password, key, salt using AES (Open SSL Encrypt).
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public byte[] EncryptWithPassword(string plainText, string passphrase)
        {
            try
            {
                // generate salt
                byte[] key, iv;
                byte[] salt = new byte[8];
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                rng.GetNonZeroBytes(salt);
                DeriveKeyAndIV(passphrase, salt, out key, out iv);
                // encrypt bytes
                byte[] encryptedBytes = EncryptStringToBytesAes(plainText, key, iv);
                // add salt as first 8 bytes
                byte[] encryptedBytesWithSalt = new byte[salt.Length + encryptedBytes.Length + PASSWORD_MODE.Length];


                Buffer.BlockCopy(Encoding.ASCII.GetBytes(PASSWORD_MODE), 0, encryptedBytesWithSalt, 0, PASSWORD_MODE.Length);
                Buffer.BlockCopy(salt, 0, encryptedBytesWithSalt, PASSWORD_MODE.Length, salt.Length);
                Buffer.BlockCopy(encryptedBytes, 0, encryptedBytesWithSalt, salt.Length + PASSWORD_MODE.Length, encryptedBytes.Length);

                return encryptedBytesWithSalt;

                // base64 encode
                //return Convert.ToBase64String(encryptedBytesWithSalt);
            }
            catch (CryptographicException cex)
            {
                //return cex.Message;
                return null;
            }
            catch (Exception ex)
            {
                //return ex.Message;
                return null;
            }

        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Decrypt with password, key, salt using AES (Open SSL Decrypt)..
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public string DecryptWithPassword(byte[] encryptedBytesWithSalt, string passphrase)
        {

            try
            {
                //string encryptedDataString = Encoding.ASCII.GetString(encryptedBytesWithKey);
                // base 64 decode
                //byte[] encryptedBytesWithSalt = Convert.FromBase64String(encryptedDataString);

                // extract salt (first 8 bytes of encrypted)
                byte[] salt = new byte[8];
                byte[] encryptedBytes = new byte[encryptedBytesWithSalt.Length - salt.Length - PASSWORD_MODE.Length];
                Buffer.BlockCopy(encryptedBytesWithSalt, PASSWORD_MODE.Length, salt, 0, salt.Length);
                Buffer.BlockCopy(encryptedBytesWithSalt, salt.Length + PASSWORD_MODE.Length, encryptedBytes, 0, encryptedBytes.Length);
                // get key and iv
                byte[] key, iv;
                DeriveKeyAndIV(passphrase, salt, out key, out iv);
                return DecryptStringFromBytesAes(encryptedBytes, key, iv);
            }
            catch (CryptographicException cex)
            {
                return cex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Derive Key and IV.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private static void DeriveKeyAndIV(string passphrase, byte[] salt, out byte[] key, out byte[] iv)
        {
            // generate key and iv
            List<byte> concatenatedHashes = new List<byte>(48);

            byte[] password = Encoding.UTF8.GetBytes(passphrase);
            byte[] currentHash = new byte[0];
            MD5 md5 = MD5.Create();
            bool enoughBytesForKey = false;
            // See http://www.openssl.org/docs/crypto/EVP_BytesToKey.html#KEY_DERIVATION_ALGORITHM
            while (!enoughBytesForKey)
            {
                int preHashLength = currentHash.Length + password.Length + salt.Length;
                byte[] preHash = new byte[preHashLength];

                Buffer.BlockCopy(currentHash, 0, preHash, 0, currentHash.Length);
                Buffer.BlockCopy(password, 0, preHash, currentHash.Length, password.Length);
                Buffer.BlockCopy(salt, 0, preHash, currentHash.Length + password.Length, salt.Length);

                currentHash = md5.ComputeHash(preHash);
                concatenatedHashes.AddRange(currentHash);

                if (concatenatedHashes.Count >= 48)
                    enoughBytesForKey = true;
            }

            key = new byte[32];
            iv = new byte[16];
            concatenatedHashes.CopyTo(0, key, 0, 32);
            concatenatedHashes.CopyTo(32, iv, 0, 16);

            md5.Clear();
            md5 = null;
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Encrypt using AES (Rijndael algoritmus).
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private static byte[] EncryptStringToBytesAes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");

            // Declare the stream used to encrypt to an in memory
            // array of bytes.
            MemoryStream msEncrypt;

            // Declare the RijndaelManaged object
            // used to encrypt the data.
            RijndaelManaged aesAlg = null;

            try
            {
                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged { Mode = CipherMode.CBC, KeySize = 256, BlockSize = 128, Key = key, IV = iv };

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                msEncrypt = new MemoryStream();
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {

                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                        swEncrypt.Flush();
                        swEncrypt.Close();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return msEncrypt.ToArray();
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Decrypt using AES (Rijndael algoritmus).
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private static string DecryptStringFromBytesAes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext;

            try
            {
                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged {Mode = CipherMode.CBC, KeySize = 256, BlockSize = 128, Key = key, IV = iv};

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                            srDecrypt.Close();
                        }
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }

        public string ComputeQuitPasswordHash(string input)
        {
            HashAlgorithm algorithm = new SHA256CryptoServiceProvider();
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);

            string pswdHash = BitConverter.ToString(hashedBytes);

            return pswdHash.Replace("-", "");
        }

        //public string RSAEncrypt(string str, string publicKey)
        //{
        //    //---Creates a new instance of RSACryptoServiceProvider---  
        //    try
        //    {
        //        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
        //        //---Loads the public key---  
        //        RSA.FromXmlString(publicKey);
        //        byte[] EncryptedStr = null;

        //        //---Encrypts the string---  
        //        EncryptedStr = RSA.Encrypt(Encoding.UTF8.GetBytes(str), false);
        //        //---Converts the encrypted byte array to string---  
        //        int i = 0;
        //        System.Text.StringBuilder s = new System.Text.StringBuilder();
        //        for (i = 0; i <= EncryptedStr.Length - 1; i++)
        //        {
        //            //Console.WriteLine(EncryptedStr(i))  
        //            if (i != EncryptedStr.Length - 1)
        //            {
        //                s.Append(EncryptedStr[i] + " ");
        //            }
        //            else
        //            {
        //                s.Append(EncryptedStr[i]);
        //            }
        //        }

        //        return s.ToString();
        //    }
        //    catch (Exception err)
        //    {
        //        Interaction.MsgBox(err.ToString());
        //    }
        //}  

        //public string RSADecrypt(string str, string privateKey)
        //{
        //    try
        //    {
        //        //---Creates a new instance of RSACryptoServiceProvider---  
        //        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
        //        //---Loads the private key---  
        //        RSA.FromXmlString(privateKey);

        //        //---Decrypts the string---  
        //        byte[] DecryptedStr = RSA.Decrypt(HexToByteArr(str), false);
        //        //---Converts the decrypted byte array to string---  
        //        System.Text.StringBuilder s = new System.Text.StringBuilder();
        //        int i = 0;
        //        for (i = 0; i <= DecryptedStr.Length - 1; i++)
        //        {
        //            //Console.WriteLine(DecryptedStr(i))  
        //            s.Append(System.Convert.ToChar(DecryptedStr[i]));
        //        }
        //        //Console.WriteLine(s)  
        //        return s.ToString();
        //    }
        //    catch (Exception err)
        //    {
        //        Interaction.MsgBox(err.ToString());
        //    }
        //}

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        ///  Encrypt with Certificate and save settings.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        //public void EncryptWithCertificateAndSave(string settings, byte[] publicKeyHash, string sebEncryptedWithCertClientConfigPath)
        //{
        //    string encrypted = EncryptWithCertificate(settings, GetCertificateFromStore(publicKeyHash));

        //    TextWriter tx = new StreamWriter(sebEncryptedWithCertClientConfigPath);
        //    tx.Write(encrypted);
        //    tx.Close();
        //}

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        ///  Encrypt with Password and save settings.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        //public void EncryptWithPasswordAndSave(string settings, string sebEncryptedWithPswClientConfigPath)
        //{
        //    string encrypted = EncryptWithPassword(settings, "seb");

        //    TextWriter tx = new StreamWriter(sebEncryptedWithPswClientConfigPath);
        //    tx.Write(encrypted);
        //    tx.Close();
        //}


        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Check encrypted data.
        /// Format: 0-3 Prefix
        ///         4-n Encryptet data
        /// Prefix: 1. pkhs 0-3 Prefix
        ///                 4-23 Public key hash
        ///                 24-n Encrypted data  
        ///         2. pswd 0 version
        ///                 1 option
        ///                 2-9 encryption salt
        ///                 10-17 HMAC salt
        ///                 18-33 IV
        ///                 34-n-33 encrypted data
        ///                 n-31-n HMAC
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        //public string DecryptSebClientSettings(string encryptedData)
        //{
        //    string decryptedDataString = null;
        //    byte[] encryptedBytesWithPrefix = Encoding.ASCII.GetBytes(encryptedData);
        //    byte[] encryptedBytesWithKey = new byte[encryptedBytesWithPrefix.Length - PREFIX_LENGTH];
        //    byte[] prefix = new byte[PREFIX_LENGTH];

        //    Buffer.BlockCopy(encryptedBytesWithPrefix, 0, prefix, 0, PREFIX_LENGTH);
        //    Buffer.BlockCopy(encryptedBytesWithPrefix, PREFIX_LENGTH, encryptedBytesWithKey, 0, encryptedBytesWithPrefix.Length - PREFIX_LENGTH);

        //    // Check prefix and set encryption type
        //    string prefixStr = Encoding.UTF8.GetString(prefix);
        //    if (prefixStr.CompareTo(PUBLIC_KEY_HASH_MODE) == 0)
        //    {
        //        // decrypt settings with private key
        //        _encryptionType = EncryptionT.pkhs;
        //        decryptedDataString = DecryptWithCertificate(encryptedBytesWithKey);
        //    }
        //    else if (prefixStr.CompareTo(PASSWORD_MODE) == 0) 
        //    {
        //        // decrypt settings with password
        //        _encryptionType = EncryptionT.pswd;
        //         decryptedDataString = DecryptWithPassword(encryptedBytesWithKey, "seb"); 
        //    }
        //    else if (prefixStr.CompareTo(PLAIN_DATA_MODE) == 0)
        //    {
        //         _encryptionType = EncryptionT.plnd;
        //    }
        //    else if (prefixStr.CompareTo(PASSWORD_CONFIGURING_CLIENT_MODE) == 0)
        //    {
        //         _encryptionType = EncryptionT.pwcc;
        //    }
        //    else
        //    {
        //        _encryptionType = EncryptionT.unknown;
        //    }

        //    return decryptedDataString;
        //}

    } 
}
