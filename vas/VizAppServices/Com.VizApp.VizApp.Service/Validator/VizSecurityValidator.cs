/*
* @(#)VizFBValidator.cs
*
* Copyright (c) 2014, VizitSolutions.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

using Com.VizApp.Arch;
using Com.VizApp.Arch.Api;
using Com.VizApp.Arch.Base;
using Com.VizApp.VizApp.Arch;
using Com.VizApp.VizApp.Service.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.VizApp.VizApp.Service.Validator
{
    public class VizSecurityValidator : BaseValidator
    {
        internal void ValidateLogin(Credentials creds)
        {
            if (creds == null)
            {
                AddError(ErrorCode.INVALID_CREDENTIALS);
                throw ClientException;
            }
            if (string.IsNullOrEmpty(creds.Email))
            {
                AddError(ErrorCode.EMAIL_EMPTY);
            }
            if (string.IsNullOrEmpty(creds.Password))
            {
                AddError(ErrorCode.PASSWORD_EMPTY);
            }

            if (NoErrors) return;

            throw ClientException;
        }
    }
}
