/*
* @(#)RegexUtilities.cs
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
using System.Globalization;
using System.Text.RegularExpressions;

namespace Com.VizApp.Arch.Util
{

    public class RegexUtilities
    {
        public static bool IsValidEmail(string strIn)
        {
            if (String.IsNullOrEmpty(strIn))
                return false;

            try
            {
                // Use IdnMapping class to convert Unicode domain names.
                strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper);
            }
            catch (ArgumentException)
            {
                return false;
            }

            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn,
                   @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                   RegexOptions.IgnoreCase);
        }

        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();
            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
                return match.Groups[1].Value + domainName;
            }
            catch (ArgumentException ag)
            {
                throw ag;
            }
        }

        public static bool IsNumericOnly(string strIn)
        {
            if (String.IsNullOrEmpty(strIn))
                return false;

            long numericValue;
            if (!long.TryParse(strIn, out numericValue))
                return false;

            return true;
        }

        public static bool IsDoubleOnly(string strIn)
        {
            if (String.IsNullOrEmpty(strIn))
                return false;

            Double doubleValue;
            if (!Double.TryParse(strIn, out doubleValue))
                return false;

            return true;
        }

        public static bool IsValidPassword(string password)
        {
            bool hasSpace = password.Contains(" ");
            return !hasSpace;

            //Regex regexObj = new Regex(@"(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{6,10})$");
            //bool foundMatch = regexObj.IsMatch(password);
            //return foundMatch;
        }
    }
}