/*
* @(#)RandomUtil.cs
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

namespace Com.VizApp.Arch.Util
{
    public class RandomUtil
    {
        private const int PASSWORD_LENGTH = 6;

        public static string GenerateRandomPassword()
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789_";

            Random randNum = new Random();
            char[] chars = new char[PASSWORD_LENGTH];

            for (int i = 0; i < PASSWORD_LENGTH; i++)
            {
                chars[i] = allowedChars[(int) ((allowedChars.Length) * randNum.NextDouble())];
            }

            return new string(chars);
        }
    }
}
