/*
* @(#)ErrorCode.cs
*
* Copyright (c) 2014, VizitSolutions.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

namespace Com.VizApp.Arch
{
    public class ErrorCode
    {
        //General Unknown exceptions start with "B2B.0.1.0"
        //public const string UNKNOWN_GENERAL_ERROR = "B2B.0.0.1.1";
        public const string SQL_GENERAL_ERROR = "VA.0.0.1.1";

        //Security
        public const string CLIENT_NOT_APPROVED = "VA.0.0.2.1";
        public const string INVALID_CREDENTIALS = "VA.0.0.2.2";
        public const string AUTHENTICATION_FAILED = "VA.0.0.2.3";
        public const string USER_INACTIVE = "VA.0.0.2.4";
        public const string INVALID_SESSION = "VA.0.0.2.5";

        //Facebook
        public const string INVALID_FB_USER = "VA.0.0.3.1";
        public const string FBUSER_SAVE_FAILED = "VA.0.0.3.2";
        public const string SETTINGS_RETRIEVAL_FAILED = "VA.0.0.3.3";
        public const string FBFRIENDS_SAVE_FAILED = "VA.0.0.3.4";
        public const string INVALID_FRIENDS = "VA.0.0.3.5";
        public const string INVALID_FRIEND = "VA.0.0.3.6";
        public const string INVALID_USER = "VA.0.0.3.7";

        //Facebook Friends
        public const string FBFRIEND_NOT_VALID = "VA.0.0.4.1";

        //Settings
        public const string INVALID_LOCATION_UPDATE_VALUE = "VA.0.0.5.1";
        public const string INVALID_SETTING = "VA.0.0.5.2";
        public const string INVALID_LOCATION = "VA.0.0.5.3";
        public const string INVALID_LATITUDE = "VA.0.0.5.4";
        public const string INVALID_LONGTITUDE = "VA.0.0.5.5";

        //Register User
        public const string INVALID_REGISTRATION_USER = "VA.0.0.6.1";
        public const string INVALID_REGISTRATION_EMAIL_EMPTY = "VA.0.0.6.2";
        public const string INVALID_REGISTRATION_EMAIL = "VA.0.0.6.3";
        public const string INVALID_REGISTRATION_PASSWORD_EMPTY = "VA.0.0.6.4";
        public const string USER_ALREADY_EXISTS = "VA.0.0.6.5";

        //Login
        public const string EMAIL_EMPTY = "VA.0.0.7.1";
        public const string PASSWORD_EMPTY = "VA.0.0.7.2";
    }
}
