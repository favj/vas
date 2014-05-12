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
    public class VizValidator : BaseValidator
    {
        internal void ValidateUpdateSettings(Settings setting)
        {
            if (setting == null)
            {
                AddError(ErrorCode.INVALID_SETTING);
                throw ClientException;
            }
            if (setting.LocationUpdate < 1)
            {
                AddError(ErrorCode.INVALID_LOCATION_UPDATE_VALUE);
            }

            if (NoErrors) return;

            throw ClientException;
        }

        internal void ValidateUpdateUserLocation(Location location)
        {
            if (location == null)
            {
                AddError(ErrorCode.INVALID_LOCATION);
                throw ClientException;
            }
            if (string.Empty.Equals(location.Latitude.Trim()))
            {
                AddError(ErrorCode.INVALID_LATITUDE);
            }
            if (string.Empty.Equals(location.Longitude.Trim()))
            {
                AddError(ErrorCode.INVALID_LONGTITUDE);
            }
        }

        internal void ValidateRegisterUser(FBUser user)
        {
            if (user == null)
            {
                AddError(ErrorCode.INVALID_REGISTRATION_USER);
                throw ClientException;
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                AddError(ErrorCode.INVALID_REGISTRATION_EMAIL_EMPTY);
            }
            if (IsValidEmail(user.Email))
            {
                AddError(ErrorCode.INVALID_REGISTRATION_EMAIL);
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                AddError(ErrorCode.INVALID_REGISTRATION_PASSWORD_EMPTY);
            }
        }
    }
}
