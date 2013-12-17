using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.IO.Compression;
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
        ///  Get certificate from store.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public static X509Certificate2 GetCertificateFromStore(byte[] publicKeyHash)
        {
            //string certificateName;
            //string certificateHash;
            X509Certificate2 sebCertificate = null;

            //Create new X509 store called teststore from the local certificate store.
            X509Store store = new X509Store(StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            
            foreach (X509Certificate2 x509Certificate in store.Certificates)
            {
                byte[] publicKeyRawData = x509Certificate.PublicKey.EncodedKeyValue.RawData;
                SHA1 sha = new SHA1CryptoServiceProvider(); 
                byte[] certificateHash = sha.ComputeHash(publicKeyRawData);

                //certificateName = x509Certificate.Subject;
                if (certificateHash.SequenceEqual(publicKeyHash))
                {
                    sebCertificate = x509Certificate;
                }
                //if (certificateName.CompareTo("C=CH, CN=SEB-Configuration") == 0)
                //{
                //    sebCertificate = x509Certificate;
                //}
            }
 
            //Close the store.
            store.Close();

            return sebCertificate;
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Decrypt with X509 certificate/private key and RSA algorithm
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public static byte[] DecryptWithCertificate(byte[] encryptedData, X509Certificate2 sebCertificate)
        {
            try
            {
                // Decrypt config data

                RSACryptoServiceProvider privateKey = sebCertificate.PrivateKey as RSACryptoServiceProvider;
                //byte[] decryptedData = privateKey.Decrypt(encryptedDataBytes, false);

                // Blocksize is for example 2048/8 = 256 
                int blockSize = privateKey.KeySize / 8;

                // buffer to hold byte sequence of the encrypted source data
                byte[] encryptedBuffer = new byte[blockSize];

                // buffer for the decrypted information
                byte[] decryptedBuffer = new byte[blockSize];

                // initialize array so it holds at least the amount needed to decrypt.
                //byte[] decryptedData = new byte[encryptedData.Length];
                MemoryStream decryptedStream = new MemoryStream();

                // Calculate number of full data blocks that will have to be decrypted
                int blockCount = encryptedData.Length / blockSize;

                for (int i = 0; i < blockCount; i ++)
                {
                    // copy byte sequence from encrypted source data to the buffer
                    Buffer.BlockCopy(encryptedData, i*blockSize, encryptedBuffer, 0, blockSize);
                    // decrypt the block in the buffer
                    decryptedBuffer = privateKey.Decrypt(encryptedBuffer, false);
                    // write decrypted result back to the destination array
                    //decryptedBuffer.CopyTo(decryptedData, i*blockSize);
                    decryptedStream.Write(decryptedBuffer, 0, decryptedBuffer.Length);
                }
                int remainingBytes = encryptedData.Length - (blockCount * blockSize);
                if (remainingBytes > 0) {
                    encryptedBuffer = new byte[remainingBytes];
                    // copy remaining bytes from encrypted source data to the buffer
                    Buffer.BlockCopy(encryptedData, blockCount * blockSize, encryptedBuffer, 0, remainingBytes);
                    // decrypt the block in the buffer
                    decryptedBuffer = privateKey.Decrypt(encryptedBuffer, false);
                    // write decrypted result back to the destination array
                    //decryptedBuffer.CopyTo(decryptedData, blockCount * blockSize);
                    decryptedStream.Write(decryptedBuffer, 0, decryptedBuffer.Length);
                }
                byte[] decryptedData = decryptedStream.ToArray();

                return decryptedData;
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

        public static string Encrypt(string dataToEncrypt, RSAParameters publicKeyInfo)
        {
            //// Our bytearray to hold all of our data after the encryption
            byte[] encryptedBytes = new byte[0];
            using (var RSA = new RSACryptoServiceProvider())
            {
                try
                {
                    //Create a new instance of RSACryptoServiceProvider.
                    UTF8Encoding encoder = new UTF8Encoding();

                    byte[] encryptThis = encoder.GetBytes(dataToEncrypt);

                    //// Importing the public key
                    RSA.ImportParameters(publicKeyInfo);

                    int blockSize = (RSA.KeySize / 8) - 32;

                    //// buffer to write byte sequence of the given block_size
                    byte[] buffer = new byte[blockSize];

                    byte[] encryptedBuffer = new byte[blockSize];

                    //// Initializing our encryptedBytes array to a suitable size, depending on the size of data to be encrypted
                    encryptedBytes = new byte[encryptThis.Length + blockSize - (encryptThis.Length % blockSize) + 32];

                    for (int i = 0; i < encryptThis.Length; i += blockSize)
                    {
                        //// If there is extra info to be parsed, but not enough to fill out a complete bytearray, fit array for last bit of data
                        if (2 * i > encryptThis.Length && ((encryptThis.Length - i) % blockSize != 0))
                        {
                            buffer = new byte[encryptThis.Length - i];
                            blockSize = encryptThis.Length - i;
                        }

                        //// If the amount of bytes we need to decrypt isn't enough to fill out a block, only decrypt part of it
                        if (encryptThis.Length < blockSize)
                        {
                            buffer = new byte[encryptThis.Length];
                            blockSize = encryptThis.Length;
                        }

                        //// encrypt the specified size of data, then add to final array.
                        Buffer.BlockCopy(encryptThis, i, buffer, 0, blockSize);
                        encryptedBuffer = RSA.Encrypt(buffer, false);
                        encryptedBuffer.CopyTo(encryptedBytes, i);
                    }
                }
                catch (CryptographicException e)
                {
                    Console.Write(e);
                }
                finally
                {
                    //// Clear the RSA key container, deleting generated keys.
                    RSA.PersistKeyInCsp = false;
                }
            }
            //// Convert the byteArray using Base64 and returns as an encrypted string
            return Convert.ToBase64String(encryptedBytes);
        }

        /// <summary>
        /// Decrypt this message using this key
        /// </summary>
        /// <param name="dataToDecrypt">
        /// The data To decrypt.
        /// </param>
        /// <param name="privateKeyInfo">
        /// The private Key Info.
        /// </param>
        /// <returns>
        /// The decrypted data.
        /// </returns>
        public static string Decrypt(string dataToDecrypt, RSAParameters privateKeyInfo)
        {
            //// The bytearray to hold all of our data after decryption
            byte[] decryptedBytes;

            //Create a new instance of RSACryptoServiceProvider.
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                try
                {
                    byte[] bytesToDecrypt = Convert.FromBase64String(dataToDecrypt);

                    //// Import the private key info
                    RSA.ImportParameters(privateKeyInfo);

                    //// No need to subtract padding size when decrypting (OR do I?)
                    int blockSize = RSA.KeySize / 8;

                    //// buffer to write byte sequence of the given block_size
                    byte[] buffer = new byte[blockSize];

                    //// buffer containing decrypted information
                    byte[] decryptedBuffer = new byte[blockSize];

                    //// Initializes our array to make sure it can hold at least the amount needed to decrypt.
                    decryptedBytes = new byte[dataToDecrypt.Length];

                    for (int i = 0; i < bytesToDecrypt.Length; i += blockSize)
                    {
                        if (2 * i > bytesToDecrypt.Length && ((bytesToDecrypt.Length - i) % blockSize != 0))
                        {
                            buffer = new byte[bytesToDecrypt.Length - i];
                            blockSize = bytesToDecrypt.Length - i;
                        }

                        //// If the amount of bytes we need to decrypt isn't enough to fill out a block, only decrypt part of it
                        if (bytesToDecrypt.Length < blockSize)
                        {
                            buffer = new byte[bytesToDecrypt.Length];
                            blockSize = bytesToDecrypt.Length;
                        }

                        Buffer.BlockCopy(bytesToDecrypt, i, buffer, 0, blockSize);
                        decryptedBuffer = RSA.Decrypt(buffer, false);
                        decryptedBuffer.CopyTo(decryptedBytes, i);
                    }
                }
                finally
                {
                    //// Clear the RSA key container, deleting generated keys.
                    RSA.PersistKeyInCsp = false;
                }
            }

            //// We encode each byte with UTF8 and then write to a string while trimming off the extra empty data created by the overhead.
            var encoder = new UTF8Encoding();
            return encoder.GetString(decryptedBytes).TrimEnd(new[] { '\0' });

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
                // encrypt bytes
                byte[] encryptedBytesWithSalt;

                return null;

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
        public static byte[] DecryptWithPassword(byte[] encryptedBytesWithSalt, string passphrase)
        {

            try
            {
                byte[] decryptedData = AESThenHMAC.SimpleDecryptWithPassword(encryptedBytesWithSalt, passphrase, nonSecretPayloadLength: 2);

                return decryptedData;
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


        public static class AESThenHMAC
        {
            private static readonly RandomNumberGenerator Random = RandomNumberGenerator.Create();

            //Preconfigured Encryption Parameters
            public static readonly int BlockBitSize = 128;
            public static readonly int KeyBitSize = 256;

            //Preconfigured Password Key Derivation Parameters
            public static readonly int SaltBitSize = 64;
            public static readonly int Iterations = 10000;
            public static readonly int MinPasswordLength = 3;

            /// <summary>
            /// Helper that generates a random key on each call.
            /// </summary>
            /// <returns></returns>
            public static byte[] NewKey()
            {
                var key = new byte[KeyBitSize / 8];
                Random.GetBytes(key);
                return key;
            }

            /// <summary>
            /// Simple Encryption (AES) then Authentication (HMAC) for a UTF8 Message.
            /// </summary>
            /// <param name="secretMessage">The secret message.</param>
            /// <param name="cryptKey">The crypt key.</param>
            /// <param name="authKey">The auth key.</param>
            /// <param name="nonSecretPayload">(Optional) Non-Secret Payload.</param>
            /// <returns>
            /// Encrypted Message
            /// </returns>
            /// <exception cref="System.ArgumentException">Secret Message Required!;secretMessage</exception>
            /// <remarks>
            /// Adds overhead of (Optional-Payload + BlockSize(16) + Message-Padded-To-Blocksize +  HMac-Tag(32)) * 1.33 Base64
            /// </remarks>
            public static string SimpleEncrypt(string secretMessage, byte[] cryptKey, byte[] authKey,
                               byte[] nonSecretPayload = null)
            {
                if (string.IsNullOrEmpty(secretMessage))
                    throw new ArgumentException("Secret Message Required!", "secretMessage");

                var plainText = Encoding.UTF8.GetBytes(secretMessage);
                var cipherText = SimpleEncrypt(plainText, cryptKey, authKey, nonSecretPayload);
                return Convert.ToBase64String(cipherText);
            }

            /// <summary>
            /// Simple Authentication (HMAC) then Decryption (AES) for a secrets UTF8 Message.
            /// </summary>
            /// <param name="encryptedMessage">The encrypted message.</param>
            /// <param name="cryptKey">The crypt key.</param>
            /// <param name="authKey">The auth key.</param>
            /// <param name="nonSecretPayloadLength">Length of the non secret payload.</param>
            /// <returns>
            /// Decrypted Message
            /// </returns>
            /// <exception cref="System.ArgumentException">Encrypted Message Required!;encryptedMessage</exception>
            public static string SimpleDecrypt(string encryptedMessage, byte[] cryptKey, byte[] authKey,
                               int nonSecretPayloadLength = 0)
            {
                if (string.IsNullOrWhiteSpace(encryptedMessage))
                    throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");

                var cipherText = Convert.FromBase64String(encryptedMessage);
                var plainText = SimpleDecrypt(cipherText, cryptKey, authKey, nonSecretPayloadLength);
                return Encoding.UTF8.GetString(plainText);
            }

            /// <summary>
            /// Simple Encryption (AES) then Authentication (HMAC) of a UTF8 message
            /// using Keys derived from a Password (PBKDF2).
            /// </summary>
            /// <param name="secretMessage">The secret message.</param>
            /// <param name="password">The password.</param>
            /// <param name="nonSecretPayload">The non secret payload.</param>
            /// <returns>
            /// Encrypted Message
            /// </returns>
            /// <exception cref="System.ArgumentException">password</exception>
            /// <remarks>
            /// Significantly less secure than using random binary keys.
            /// Adds additional non secret payload for key generation parameters.
            /// </remarks>
            public static string SimpleEncryptWithPassword(string secretMessage, string password,
                                     byte[] nonSecretPayload = null)
            {
                if (string.IsNullOrEmpty(secretMessage))
                    throw new ArgumentException("Secret Message Required!", "secretMessage");

                var plainText = Encoding.UTF8.GetBytes(secretMessage);
                var cipherText = SimpleEncryptWithPassword(plainText, password, nonSecretPayload);
                return Convert.ToBase64String(cipherText);
            }

            /// <summary>
            /// Simple Authentication (HMAC) and then Descryption (AES) of a UTF8 Message
            /// using keys derived from a password (PBKDF2). 
            /// </summary>
            /// <param name="encryptedMessage">The encrypted message.</param>
            /// <param name="password">The password.</param>
            /// <param name="nonSecretPayloadLength">Length of the non secret payload.</param>
            /// <returns>
            /// Decrypted Message
            /// </returns>
            /// <exception cref="System.ArgumentException">Encrypted Message Required!;encryptedMessage</exception>
            /// <remarks>
            /// Significantly less secure than using random binary keys.
            /// </remarks>
            public static string SimpleDecryptWithPassword(string encryptedMessage, string password,
                                     int nonSecretPayloadLength = 0)
            {
                if (string.IsNullOrWhiteSpace(encryptedMessage))
                    throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");

                var cipherText = Convert.FromBase64String(encryptedMessage);
                var plainText = SimpleDecryptWithPassword(cipherText, password, nonSecretPayloadLength);
                return Encoding.UTF8.GetString(plainText);
            }

            public static byte[] SimpleEncrypt(byte[] secretMessage, byte[] cryptKey, byte[] authKey, byte[] nonSecretPayload = null)
            {
                //User Error Checks
                if (cryptKey == null || cryptKey.Length != KeyBitSize / 8)
                    throw new ArgumentException(String.Format("Key needs to be {0} bit!", KeyBitSize), "cryptKey");

                if (authKey == null || authKey.Length != KeyBitSize / 8)
                    throw new ArgumentException(String.Format("Key needs to be {0} bit!", KeyBitSize), "authKey");

                if (secretMessage == null || secretMessage.Length < 1)
                    throw new ArgumentException("Secret Message Required!", "secretMessage");

                //non-secret payload optional
                nonSecretPayload = nonSecretPayload ?? new byte[] { };

                byte[] cipherText;
                byte[] iv;

                using (var aes = new AesManaged
                {
                    KeySize = KeyBitSize,
                    BlockSize = BlockBitSize,
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7
                })
                {

                    //Use random IV
                    aes.GenerateIV();
                    iv = aes.IV;

                    using (var encrypter = aes.CreateEncryptor(cryptKey, iv))
                    using (var cipherStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(cipherStream, encrypter, CryptoStreamMode.Write))
                        using (var binaryWriter = new BinaryWriter(cryptoStream))
                        {
                            //Encrypt Data
                            binaryWriter.Write(secretMessage);
                        }

                        cipherText = cipherStream.ToArray();
                    }

                }

                //Assemble encrypted message and add authentication
                using (var hmac = new HMACSHA256(authKey))
                using (var encryptedStream = new MemoryStream())
                {
                    using (var binaryWriter = new BinaryWriter(encryptedStream))
                    {
                        //Prepend non-secret payload if any
                        binaryWriter.Write(nonSecretPayload);
                        //Prepend IV
                        binaryWriter.Write(iv);
                        //Write Ciphertext
                        binaryWriter.Write(cipherText);
                        binaryWriter.Flush();

                        //Authenticate all data
                        var tag = hmac.ComputeHash(encryptedStream.ToArray());
                        //Postpend tag
                        binaryWriter.Write(tag);
                    }
                    return encryptedStream.ToArray();
                }

            }

            public static byte[] SimpleDecrypt(byte[] encryptedMessage, byte[] cryptKey, byte[] authKey, int nonSecretPayloadLength = 0)
            {

                //Basic Usage Error Checks
                if (cryptKey == null || cryptKey.Length != KeyBitSize / 8)
                    throw new ArgumentException(String.Format("CryptKey needs to be {0} bit!", KeyBitSize), "cryptKey");

                if (authKey == null || authKey.Length != KeyBitSize / 8)
                    throw new ArgumentException(String.Format("AuthKey needs to be {0} bit!", KeyBitSize), "authKey");

                if (encryptedMessage == null || encryptedMessage.Length == 0)
                    throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");

                using (var hmac = new HMACSHA256(authKey))
                {
                    var sentTag = new byte[hmac.HashSize / 8];
                    //Calculate Tag
                    var calcTag = hmac.ComputeHash(encryptedMessage, 0, encryptedMessage.Length - sentTag.Length);
                    var ivLength = (BlockBitSize / 8);

                    //if message length is to small just return null
                    if (encryptedMessage.Length < sentTag.Length + nonSecretPayloadLength + ivLength)
                        return null;

                    //Grab Sent Tag
                    Array.Copy(encryptedMessage, encryptedMessage.Length - sentTag.Length, sentTag, 0, sentTag.Length);

                    //Compare Tag with constant time comparison
                    var compare = 0;
                    for (var i = 0; i < sentTag.Length; i++)
                        compare |= sentTag[i] ^ calcTag[i];

                    //if message doesn't authenticate return null
                    if (compare != 0)
                        return null;

                    using (var aes = new AesManaged
                    {
                        KeySize = KeyBitSize,
                        BlockSize = BlockBitSize,
                        Mode = CipherMode.CBC,
                        Padding = PaddingMode.PKCS7
                    })
                    {

                        //Grab IV from message
                        var iv = new byte[ivLength];
                        Array.Copy(encryptedMessage, nonSecretPayloadLength, iv, 0, iv.Length);

                        using (var decrypter = aes.CreateDecryptor(cryptKey, iv))
                        using (var plainTextStream = new MemoryStream())
                        {
                            using (var decrypterStream = new CryptoStream(plainTextStream, decrypter, CryptoStreamMode.Write))
                            using (var binaryWriter = new BinaryWriter(decrypterStream))
                            {
                                //Decrypt Cipher Text from Message
                                binaryWriter.Write(
                                  encryptedMessage,
                                  nonSecretPayloadLength + iv.Length,
                                  encryptedMessage.Length - nonSecretPayloadLength - iv.Length - sentTag.Length
                                );
                            }
                            //Return Plain Text
                            return plainTextStream.ToArray();
                        }
                    }
                }
            }

            public static byte[] SimpleEncryptWithPassword(byte[] secretMessage, string password, byte[] nonSecretPayload = null)
            {
                nonSecretPayload = nonSecretPayload ?? new byte[] { };

                //User Error Checks
                if (string.IsNullOrWhiteSpace(password) || password.Length < MinPasswordLength)
                    throw new ArgumentException(String.Format("Must have a password of at least {0} characters!", MinPasswordLength), "password");

                if (secretMessage == null || secretMessage.Length == 0)
                    throw new ArgumentException("Secret Message Required!", "secretMessage");

                var payload = new byte[((SaltBitSize / 8) * 2) + nonSecretPayload.Length];

                Array.Copy(nonSecretPayload, payload, nonSecretPayload.Length);
                int payloadIndex = nonSecretPayload.Length;

                byte[] cryptKey;
                byte[] authKey;
                //Use Random Salt to prevent pre-generated weak password attacks.
                using (var generator = new Rfc2898DeriveBytes(password, SaltBitSize / 8, Iterations))
                {
                    var salt = generator.Salt;

                    //Generate Keys
                    cryptKey = generator.GetBytes(KeyBitSize / 8);

                    //Create Non Secret Payload
                    Array.Copy(salt, 0, payload, payloadIndex, salt.Length);
                    payloadIndex += salt.Length;
                }

                //Deriving separate key, might be less efficient than using HKDF, 
                //but now compatible with RNEncryptor which had a very similar wireformat and requires less code than HKDF.
                using (var generator = new Rfc2898DeriveBytes(password, SaltBitSize / 8, Iterations))
                {
                    var salt = generator.Salt;

                    //Generate Keys
                    authKey = generator.GetBytes(KeyBitSize / 8);

                    //Create Rest of Non Secret Payload
                    Array.Copy(salt, 0, payload, payloadIndex, salt.Length);
                }

                return SimpleEncrypt(secretMessage, cryptKey, authKey, payload);
            }

            public static byte[] SimpleDecryptWithPassword(byte[] encryptedMessage, string password, int nonSecretPayloadLength = 0)
            {
                //User Error Checks
                //if (string.IsNullOrWhiteSpace(password) || password.Length < MinPasswordLength)
                //    throw new ArgumentException(String.Format("Must have a password of at least {0} characters!", MinPasswordLength), "password");

                if (encryptedMessage == null || encryptedMessage.Length == 0)
                    throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");

                var cryptSalt = new byte[SaltBitSize / 8];
                var authSalt = new byte[SaltBitSize / 8];

                //Grab Salt from Non-Secret Payload
                Array.Copy(encryptedMessage, nonSecretPayloadLength, cryptSalt, 0, cryptSalt.Length);
                Array.Copy(encryptedMessage, nonSecretPayloadLength + cryptSalt.Length, authSalt, 0, authSalt.Length);

                byte[] cryptKey;
                byte[] authKey;

                //Generate crypt key
                using (var generator = new Rfc2898DeriveBytes(password, cryptSalt, Iterations))
                {
                    cryptKey = generator.GetBytes(KeyBitSize / 8);
                }
                //Generate auth key
                using (var generator = new Rfc2898DeriveBytes(password, authSalt, Iterations))
                {
                    authKey = generator.GetBytes(KeyBitSize / 8);
                }

                return SimpleDecrypt(encryptedMessage, cryptKey, authKey, cryptSalt.Length + authSalt.Length + nonSecretPayloadLength);
            }
        }

        public static string ComputePasswordHash(string input)
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
