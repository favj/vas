/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Tymetrix.T360.Mobile.Client.Core
{

    public static class Vault
    {
        #region Private Constants

        private const int BLOCK_SIZE = 128;
        private const int KEY_SIZE = 128;

        #endregion Private Constants

        #region Private Variables

        private static byte[] staticIV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        #endregion Private Variables

        #region Public Methods

        /// <summary>
        /// Encrypt using local key
        /// </summary>
        /// <param name="stringToEncrypt"></param>
        /// <returns></returns>
        public static byte[] Encrypt(string stringToEncrypt)
        {
            string LocalKey = @"A3$1E*8^%ER256%$#526SDES85)(_@DS5#$%GH5267";
            return Encrypt(stringToEncrypt, LocalKey);
        }

        /// <summary>
        /// Encrypt using dynamic key
        /// </summary>
        /// <param name="stringToEncrypt"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static byte[] Encrypt(string stringToEncrypt, string password)
        {
            byte[] byteArrayToEncrypt = UTF8Encoding.UTF8.GetBytes(stringToEncrypt);
            return Encrypt(byteArrayToEncrypt, password);
        }

        private static byte[] Encrypt(byte[] byteArrayToEncrypt, string password)
        {
            try
            {
                using (Aes aes = new AesManaged())
                {
                    aes.BlockSize = BLOCK_SIZE;
                    aes.KeySize = KEY_SIZE;
                    aes.Key = GetKey(Encoding.UTF8.GetBytes(password), aes);
                    aes.IV = staticIV;

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            byte[] utfD1 = byteArrayToEncrypt;
                            cryptoStream.Write(utfD1, 0, utfD1.Length);
                            cryptoStream.FlushFinalBlock();
                        }
                        return memoryStream.ToArray();
                    }
                }
            }
            catch
            {
                throw new AppException(T360ErrorCodes.EncryptionFailed);
            }
        }

        public static string Decrypt(byte[] byteArrayToDecrypt)
        {
            string LocalKey = @"A3$1E*8^%ER256%$#526SDES85)(_@DS5#$%GH5267";
            return Decrypt(byteArrayToDecrypt, LocalKey);
        }

        public static string Decrypt(byte[] byteArrayToDecrypt, string password)
        {
            try
            {
                using (Aes aes = new AesManaged())
                {
                    aes.BlockSize = BLOCK_SIZE;
                    aes.KeySize = KEY_SIZE;
                    aes.Key = GetKey(Encoding.UTF8.GetBytes(password), aes);
                    aes.IV = staticIV;

                    using (MemoryStream decryptionStream = new MemoryStream())
                    {
                        using (CryptoStream decrypt = new CryptoStream(decryptionStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            byte[] encryptedData = byteArrayToDecrypt;
                            decrypt.Write(encryptedData, 0, encryptedData.Length);
                            decrypt.Flush();
                        }
                        byte[] decryptedData = decryptionStream.ToArray();
                        return UTF8Encoding.UTF8.GetString(decryptedData, 0, decryptedData.Length);
                    }
                }
            }
            catch
            {
                throw new AppException(T360ErrorCodes.DecryptionFailed);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private static byte[] GetKey(byte[] suggestedKey, SymmetricAlgorithm symmetricAlgorithm)
        {
            byte[] kRaw = suggestedKey;
            List<byte> kList = new List<byte>();

            for (int i = 0; i < symmetricAlgorithm.LegalKeySizes[0].MinSize; i += 8)
            {
                kList.Add(kRaw[(i / 8) % kRaw.Length]);
            }
            byte[] k = kList.ToArray();
            return k;
        }

        #endregion Private Methods

    }
}
