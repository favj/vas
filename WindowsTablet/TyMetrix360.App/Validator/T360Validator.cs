/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;

using TyMetrix360.Core.Models;

namespace TyMetrix360.App.Validator
{
    public class T360Validator
    {

        public static void ValidateLogin(string userName, string password, string integratedLogin)
        {
            List<Error> errors = new List<Error>();

            if (userName == null)
            {
                errors.Add(new Error(T360ErrorCodes.LOGIN001));
            }
            else
            {
                if (string.Empty.Equals(userName.Trim())) errors.Add(new Error(T360ErrorCodes.LOGIN001));
            }
            if (string.IsNullOrEmpty(password)) { errors.Add(new Error(T360ErrorCodes.LOGIN002)); }

            if (integratedLogin != null)
            {
                if (string.IsNullOrWhiteSpace(integratedLogin)) errors.Add(new Error(T360ErrorCodes.LOGIN003));
            }
            if (errors.Count > 0) throw new T360Exception(errors);
        }

        public static void ValidateResetPassword(string password, string confirmPassword)
        {
            List<Error> errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add(new Error(T360ErrorCodes.NewPasswordEmpty));
            }
            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                errors.Add(new Error(T360ErrorCodes.ConfirmPasswordEmpty));
            }
            if (password != confirmPassword)
            {
                errors.Add(new Error(T360ErrorCodes.PasswordsNotMatch));
            }
            if (errors.Count > 0) throw new T360Exception(errors);
        }

        public static void ValidateAdjustInvoice(string adjustAmount, int? reason)
        {
            List<Error> errors = new List<Error>();

            if ("0".Equals(adjustAmount) || string.IsNullOrWhiteSpace(adjustAmount))
            {
                errors.Add(new Error(T360ErrorCodes.AdjustAmountEmpty));
            }

            if (reason == null || reason == 0)
            {
                errors.Add(new Error(T360ErrorCodes.ReasonEmpty));
            }

            if (errors.Count > 0) throw new T360Exception(errors);
        }

        public static void ValidateRejectInvoice(int? reason, string narrative)
        {
            List<Error> errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(narrative))
            {
                errors.Add(new Error(T360ErrorCodes.NarrativeEmpty));
            }

            if (reason == null || reason == 0)
            {
                errors.Add(new Error(T360ErrorCodes.ReasonEmpty));
            }

            if (errors.Count > 0) throw new T360Exception(errors);
        }
    }
}
