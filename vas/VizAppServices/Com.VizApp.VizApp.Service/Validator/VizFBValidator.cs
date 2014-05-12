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
using Com.VizApp.Arch.Base;
using Com.VizApp.VizApp.Service.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.VizApp.VizApp.Service.Validator
{
    public class VizFBValidator : BaseValidator
    {
        private string Zero = "0";

        internal void ValidateSaveFBDetails(FBData fbData)
        {
            if (fbData == null)
            {
                AddError(ErrorCode.INVALID_USER);
            }
            if (fbData.User.id.Equals(string.Empty) || fbData.User.id.Equals(Zero))
            {
                AddError(ErrorCode.INVALID_FB_USER);
            }

            if (NoErrors) return;

            throw ClientException;
        }

        internal void ValidateSaveFBFriends(FBFriends fbfriends)
        {
            if (fbfriends == null)
            {
                AddError(ErrorCode.INVALID_FRIENDS);
            }
            foreach (FBFriend friend in fbfriends.Friends)
            {
                if (friend == null)
                {
                    AddError(ErrorCode.INVALID_FRIEND);
                }
                if (friend.id.Equals(Zero))
                {
                    AddError(ErrorCode.FBFRIEND_NOT_VALID, friend.Name);
                }
            }

            if (NoErrors) return;

            throw ClientException;
        }
    }
}
