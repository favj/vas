/*
* @(#)BaseValidator.cs
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
using System.Net.Mail;

namespace Com.VizApp.Arch.Base
{
    public class BaseValidator
    {
        private List<Error> errors;

        protected void AddError(string errorCode)
        {
            Error error = new Error(errorCode);
            Errors.Add(error);
        }

        protected void AddError(string errorCode, string errorData)
        {
            Error error = new Error(errorCode, errorData);
            Errors.Add(error);
        }

        public AppException ClientException
        {
            get { return new AppException(Errors); }
        }

        protected bool NoErrors
        {
            get { return (errors == null); }
        }

        private List<Error> Errors
        {
            get
            {
                if (errors == null)
                {
                    errors = new List<Error>();
                }
                return errors;
            }
        }

        protected bool IsValidEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
