using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using System.Security.Cryptography;
//using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
//using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
//using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Feng
{
    /// <summary>
    /// 加密解密
    /// </summary>
    public static class Cryptographer
    {
        // The file name where the DPAPI-protected symmetric key is stored
        //private const string symmKeyFileName = "SymmetricKeyFile.key";

        // The following strings correspond to the names of the providers as 
        // specified by the configuration file.
        private const string hashProvider = "hashprovider";
        private const string symmProvider = "symprovider";

        private static bool s_haveCreateKeys;
        private static void TryCreateKeys()
        {
            try
            {
                if (!s_haveCreateKeys)
                {
                    s_haveCreateKeys = true;

                    Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.CryptographySettings cryptographySettings =
                        ConfigurationManager.GetSection("securityCryptographyConfiguration") as Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.CryptographySettings;
                    Microsoft.Practices.EnterpriseLibrary.Common.Configuration.NameTypeConfigurationElementCollection<Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.SymmetricProviderData, Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.CustomSymmetricCryptoProviderData> elementCollection = cryptographySettings.SymmetricCryptoProviders;
                    Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.SymmetricProviderData symmetricProviderData = elementCollection.Get(symmProvider);
                    string fileName = symmetricProviderData.ElementInformation.Properties["protectedKeyFilename"].Value.ToString();

                    //string installedPath = "C:\\windows\\system32"; //System.IO.Directory.GetCurrentDirectory();
                    //string fileName = Path.Combine(installedPath, symmKeyFileName);
                    if (!System.IO.File.Exists(fileName))
                    {
                        Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.ProtectedKey symmetricKey =
                            Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.KeyManager.GenerateSymmetricKey(typeof(RijndaelManaged), DataProtectionScope.LocalMachine);
                        using (FileStream keyStream = new FileStream(fileName, FileMode.Create))
                        {
                            Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.KeyManager.Write(keyStream, symmetricKey);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// GenerateKey
        /// </summary>
        /// <returns></returns>
        public static string GenerateKey()
        {
            //// Create an instance of Symetric Algorithm. Key and IV is generated automatically.
            //DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
            //// Use the Automatically generated key for Encryption. 
            //return desCrypto.Key;

            //return  System.Text.Encoding.ASCII.GetBytes("VavicApp");
            return Feng.Utils.RandomHelper.GenerateRandomChars(8);
        }

        private static ICryptoTransform CreateEncryptor(string key)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(key,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
                0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            // Function to Generate a 64 bits Key.
            byte[] byKey = pdb.GetBytes(8);
            byte[] byIV = pdb.GetBytes(8);

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            return cryptoProvider.CreateEncryptor(byKey, byIV);
        }
        private static ICryptoTransform CreateDecryptor(string key)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(key,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
                0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            byte[] byKey = pdb.GetBytes(8);
            byte[] byIV = pdb.GetBytes(8);

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            return cryptoProvider.CreateDecryptor(byKey, byIV);
        }

        /// <summary>
        /// 生成MD5值
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Md5(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return null;

            byte[] data = Encoding.ASCII.GetBytes(plainText);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);
            
            StringBuilder sb = new StringBuilder();
            foreach (byte b in result)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 生成MD5值
        /// </summary>
        /// <param name="plainBytes"></param>
        /// <returns></returns>
        public static byte[] Md5(byte[] plainBytes)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(plainBytes);
            return result;
        }

        /// <summary>
        /// EncryptSymmetric
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncryptSymmetric(string plainText, string key)
        {
            if (string.IsNullOrEmpty(plainText))
                return null;

            ICryptoTransform cryptTrans = CreateEncryptor(key);

            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptTrans, CryptoStreamMode.Write);

            byte[] clearData = System.Text.Encoding.Unicode.GetBytes(plainText);
            cst.Write(clearData, 0, clearData.Length);
            cst.Close();

            byte[] encryptedData = ms.ToArray();

            return Convert.ToBase64String(encryptedData, 0, encryptedData.Length);
        }

        /// <summary>
        /// DecryptSymmetric
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DecryptSymmetric(string cipherText, string key)
        {
            if (string.IsNullOrEmpty(cipherText))
                return null;

            try
            {
                ICryptoTransform cryptTrans = CreateDecryptor(key);

                byte[] cipherData = Convert.FromBase64String(cipherText);
                MemoryStream ms = new MemoryStream();

                CryptoStream cst = new CryptoStream(ms, cryptTrans, CryptoStreamMode.Write);
                cst.Write(cipherData, 0, cipherData.Length); 
                cst.Close();

                byte[] b = ms.ToArray();
                return System.Text.Encoding.Unicode.GetString(b);
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        } 


        /// <summary>
        /// 用自动生成的密码文件对称加密。
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public static string EncryptSymmetric(string plaintext)
        {
            if (SystemConfiguration.LiteMode)
            {
                throw new InvalidOperationException("you'd better not use Microsoft Enterprise Library method!");
            }

            if (string.IsNullOrEmpty(plaintext))
                return null;
            TryCreateKeys();

            return Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Cryptographer.EncryptSymmetric(symmProvider, plaintext);
        }

        /// <summary>
        /// 用自动生成的密码文件对称解密。如失败，返回null
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        public static string DecryptSymmetric(string ciphertext)
        {
            if (SystemConfiguration.LiteMode)
            {
                throw new InvalidOperationException("you'd better not use Microsoft Enterprise Library method!");
            }
            if (string.IsNullOrEmpty(ciphertext))
                return null;
            TryCreateKeys();

            try
            {
                return Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Cryptographer.DecryptSymmetric(symmProvider, ciphertext);
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }

        /// <summary>
        /// 用自动生成的密码文件对称加密。如失败，返回null
        /// </summary>
        /// <param name="plainbytes"></param>
        /// <returns></returns>
        public static byte[] EncryptSymmetric(byte[] plainbytes)
        {
            if (SystemConfiguration.LiteMode)
            {
                throw new InvalidOperationException("you'd better not use Microsoft Enterprise Library method!");
            }
            if (plainbytes == null)
                return null;
            TryCreateKeys();

            byte[] encryptedContents = Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Cryptographer.EncryptSymmetric(symmProvider, plainbytes);
            // Clear the byte array memory that holds the password.
            Array.Clear(plainbytes, 0, plainbytes.Length);
            return encryptedContents;
        }

        /// <summary>
        /// DecryptSymmetric
        /// 如失败，返回null
        /// </summary>
        /// <param name="cipherbytes"></param>
        /// <returns></returns>
        public static byte[] DecryptSymmetric(byte[] cipherbytes)
        {
            if (SystemConfiguration.LiteMode)
            {
                throw new InvalidOperationException("you'd better not use Microsoft Enterprise Library method!");
            }
            if (cipherbytes == null)
                return null;
            TryCreateKeys();

            try
            {
                return Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Cryptographer.DecryptSymmetric(symmProvider, cipherbytes);
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }

        /// <summary>
        /// 用自定义Hash算法比较Hash值。
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="hashedText"></param>
        /// <returns></returns>
        public static bool CompareHash(byte[] plaintext, byte[] hashedText)
        {
            if (SystemConfiguration.LiteMode)
            {
                throw new InvalidOperationException("you'd better not use Microsoft Enterprise Library method!");
            }
            return Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Cryptographer.CompareHash(hashProvider, plaintext, hashedText);
        }

        /// <summary>
        /// 用自定义Hash算法比较Hash值。
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="hashedText"></param>
        /// <returns></returns>
        public static bool CompareHash(string plaintext, string hashedText)
        {
            if (SystemConfiguration.LiteMode)
            {
                throw new InvalidOperationException("you'd better not use Microsoft Enterprise Library method!");
            }
            if (string.IsNullOrEmpty(plaintext))
            {
                if (string.IsNullOrEmpty(hashedText))
                    return true;
                else
                    return false;
            }
            if (string.IsNullOrEmpty(hashedText))
                return false;

            return Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Cryptographer.CompareHash(hashProvider, plaintext, hashedText);
        }

        /// <summary>
        /// 用自定义Hash算法生成Hash值。
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public static byte[] CreateHash(byte[] plaintext)
        {
            if (SystemConfiguration.LiteMode)
            {
                throw new InvalidOperationException("you'd better not use Microsoft Enterprise Library method!");
            }
            byte[] generatedHash = Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Cryptographer.CreateHash(hashProvider, plaintext);
            // Clear the byte array memory.
            Array.Clear(plaintext, 0, plaintext.Length);
            return generatedHash;
        }

        /// <summary>
        /// 用自定义Hash算法比较Hash值。
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public static string CreateHash(string plaintext)
        {
            if (SystemConfiguration.LiteMode)
            {
                throw new InvalidOperationException("you'd better not use Microsoft Enterprise Library method!");
            }
            if (string.IsNullOrEmpty(plaintext))
                return null;

            return Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Cryptographer.CreateHash(hashProvider, plaintext);
        }
    }
}