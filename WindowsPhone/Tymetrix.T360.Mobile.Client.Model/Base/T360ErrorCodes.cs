/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */


namespace Tymetrix.T360.Mobile.Client.Model.Base
{
    public class T360ErrorCodes
    {
        #region Fields
        //
        public const string UnableToConnectServer = "APP005";
        public const string ServerError           = "APP006";
        public const string ClientHandShakeFailed = "APP500";
        public const string ServerNotFound        = "APP008";

        public const string EncryptionFailed = "APP800";
        public const string DecryptionFailed = "APP801";

        //Web Exceptions
        public const string ConnectFailure      = "APP900";
        public const string InternalServerError = "APP901";

        // Login module
        public const string AppSessionExpired      = "PRELOGIN002";
        public const string UserIdEmpty            = "LOGIN001";
        public const string PasswordEmpty          = "LOGIN002";
        public const string IntegratedLoginIdEmpty = "LOGIN003";
        public const string IsClientOrTymetrixUser = "ISCLIENT_TYMETRIXUSER";
        public const string HasInvoiceListAccess   = "HAS_INVOICELIST_ACCESS";
        public const string Disclaimer             = "CONFIDENTIALITY_DISCLAIMER";
        public const string PasswordReset          = "RESET_PASSWORD";
        public const string RequestPasswordReset   = "REQUEST_PASSWORD_RESET";
        public const string TooSimplePassword      = "TOO_SIMPLE_PASSWORD";
        public const string NewPasswordEmpty       = "RESET_PWD001";
        public const string ConfirmPasswordEmpty   = "RESET_PWD002";
        public const string PasswordsNotMatch      = "RESET_PWD003";
        public const string LicenseAgreement       = "LICENSE_AGREEMENT";


        // Adjust module
        public const string AdjustAmountEmpty  = "INVOICE008";
        public const string NotInReviewerQueue = "INVOICE024";
        public const string ReasonEmpty        = "INVOICE006";
        public const string NarrativeEmpty     = "INVOICE003";

        //Session 
        public const string SessionExpired = "SessionTimedOut";
        #endregion
    }
}
