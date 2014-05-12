/*
* @(#)EncryptUtil.cs
*
* Copyright (c) 2014, VizitSolutions.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Com.VizApp.Arch.Util
{
    public class EncryptUtil
    {
        public static string EncryptString(string inputString, string configKey)
        {
            //Set up the encryption objects
            using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(configKey)))
            {
                byte[] sourceBytes = Encoding.ASCII.GetBytes(inputString);
                ICryptoTransform ictE = acsp.CreateEncryptor();

                //Set up stream to contain the encryption
                MemoryStream msS = new MemoryStream();

                //Perform the encrpytion, storing output into the stream
                CryptoStream csS = new CryptoStream(msS, ictE, CryptoStreamMode.Write);
                csS.Write(sourceBytes, 0, sourceBytes.Length);
                csS.FlushFinalBlock();

                //sourceBytes are now encrypted as an array of secure bytes
                byte[] encryptedBytes = msS.ToArray(); //.ToArray() is important, don't mess with the buffer                

                //return the encrypted bytes as a BASE64 encoded string
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        private static AesCryptoServiceProvider GetProvider(byte[] key)
        {
            AesCryptoServiceProvider result = new AesCryptoServiceProvider();
            result.BlockSize = 128;
            result.KeySize = 128;
            result.Mode = CipherMode.CBC;
            result.Padding = PaddingMode.PKCS7;

            result.GenerateIV();
            result.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            byte[] RealKey = GetKey(key, result);
            result.Key = RealKey;
            // result.IV = RealKey;
            return result;
        }

        private static byte[] GetKey(byte[] suggestedKey, SymmetricAlgorithm p)
        {
            byte[] kRaw = suggestedKey;
            List<byte> kList = new List<byte>();

            for (int i = 0; i < p.LegalKeySizes[0].MinSize; i += 8)
            {
                kList.Add(kRaw[(i / 8) % kRaw.Length]);
            }
            byte[] k = kList.ToArray();
            return k;
        }

        public static string EncryptPassword(string stringToEncrypt)
        {
            string LocalKey = @"A3$1E*8^%ER256%$#526SDES85)(_@DS5#$%GH5267";
            return EncryptString(stringToEncrypt, LocalKey);
        }

        //public static string EncryptPassword(string str)
        //{
        //    byte[] textBytes = System.Text.Encoding.Default.GetBytes(str);
        //    try
        //    {
        //        System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
        //        cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
        //        byte[] hash = cryptHandler.ComputeHash(textBytes);
        //        string ret = "";
        //        foreach (byte a in hash)
        //        {
        //            if (a < 16)
        //                ret += "0" + a.ToString("x");
        //            else
        //                ret += a.ToString("x");
        //        }
        //        return ret;
        //    }
        //    catch
        //    {
        //        throw new AppException("Unable to encrypt"); ;
        //    }
        //}
    }
}
