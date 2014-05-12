/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Collections.Generic;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

using TyMetrix360.Core.Models;

namespace TyMetrix360.Core
{
    public static class Vault
    {

        /// <summary>
        /// Encrypts the specified input.
        /// </summary>
        /// <param name="input">The input to encrypt.</param>
        /// <param name="password">The password to use.</param>
        /// <returns>Encrypted input</returns>
        public static string Encrypt(string input, string password)
        {

            var rawPassword = Encoding.UTF8.GetBytes(password);
            var finalPassword = new List<byte>();
            // create password byte array
            for (int byteCounter = 0; byteCounter < 128; byteCounter += 8)
            {
                finalPassword.Add(rawPassword[(byteCounter/8)%rawPassword.Length]);
            }
            var passwordByteArray = finalPassword.ToArray();

            // create buffer for password for encryption
            var passwordBuffer = CryptographicBuffer.CreateFromByteArray(passwordByteArray);

            // create salt buffer
            var saltBuffer =
                CryptographicBuffer.CreateFromByteArray(new byte[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0});
            var asciiEncoding = Encoding.GetEncoding("ASCII");
            var inputBuffer = CryptographicBuffer.CreateFromByteArray(asciiEncoding.GetBytes(input));

            // create provider
            var symmetricAlgorithmProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm("AES_CBC_PKCS7");

            // create key
            var symmetricKey = symmetricAlgorithmProvider.CreateSymmetricKey(passwordBuffer);

            // encrypt data buffer using symmetric key and derived salt material
            var resultBuffer = CryptographicEngine.Encrypt(symmetricKey, inputBuffer, saltBuffer);
            // convert result to encoded string
            string result = CryptographicBuffer.EncodeToBase64String(resultBuffer);
            return result;
        }

        public static string AES_Encrypt(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;
            string LocalKey = @"A3$1E*8^%ER256%$#526SDES85)(_@DS5#$%GH5267";
            return AES_Encrypt(input, LocalKey);
        }

        public static string AES_Encrypt(string input, string pass)
        {
            if (string.IsNullOrEmpty(input)) return null;
            SymmetricKeyAlgorithmProvider SAP = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
            CryptographicKey AES;
            HashAlgorithmProvider HAP = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            CryptographicHash Hash_AES = HAP.CreateHash();

            string encrypted = string.Empty;
            try
            {
                byte[] hash = new byte[32];
                Hash_AES.Append(CryptographicBuffer.CreateFromByteArray(System.Text.Encoding.UTF8.GetBytes(pass)));
                byte[] temp;
                CryptographicBuffer.CopyToByteArray(Hash_AES.GetValueAndReset(), out temp);

                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);

                AES = SAP.CreateSymmetricKey(CryptographicBuffer.CreateFromByteArray(hash));

                IBuffer Buffer = CryptographicBuffer.CreateFromByteArray(System.Text.Encoding.UTF8.GetBytes(input));
                encrypted = CryptographicBuffer.EncodeToBase64String(CryptographicEngine.Encrypt(AES, Buffer, null));

                return encrypted;
            }
            catch
            {
                throw new T360Exception(T360ErrorCodes.EncryptionFailed);
            }
        }

        public static string AES_Decrypt(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;
            string LocalKey = @"A3$1E*8^%ER256%$#526SDES85)(_@DS5#$%GH5267";
            return AES_Decrypt(input, LocalKey);
        }

        public static string AES_Decrypt(string input, string pass)
        {
            if (string.IsNullOrEmpty(input)) return null;
            SymmetricKeyAlgorithmProvider SAP = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
            CryptographicKey AES;
            HashAlgorithmProvider HAP = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            CryptographicHash Hash_AES = HAP.CreateHash();

            string decrypted = string.Empty;
            try
            {
                byte[] hash = new byte[32];
                Hash_AES.Append(CryptographicBuffer.CreateFromByteArray(System.Text.Encoding.UTF8.GetBytes(pass)));
                byte[] temp;
                CryptographicBuffer.CopyToByteArray(Hash_AES.GetValueAndReset(), out temp);

                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);

                AES = SAP.CreateSymmetricKey(CryptographicBuffer.CreateFromByteArray(hash));

                IBuffer Buffer = CryptographicBuffer.DecodeFromBase64String(input);
                byte[] Decrypted;
                CryptographicBuffer.CopyToByteArray(CryptographicEngine.Decrypt(AES, Buffer, null), out Decrypted);
                decrypted = System.Text.Encoding.UTF8.GetString(Decrypted, 0, Decrypted.Length);

                return decrypted;
            }
            catch
            {
                throw new T360Exception(T360ErrorCodes.DecryptionFailed);
            }
        }
    }
}
