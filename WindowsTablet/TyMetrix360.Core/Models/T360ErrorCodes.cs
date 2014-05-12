/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */
using System.Collections.Generic;

namespace TyMetrix360.Core.Models
{
    public static class T360ErrorCodes
    {
        public const string DisclaimerError = "CONFIDENTIALITY_DISCLAIMER";
        public const string EncryptionFailed = "APP800";
        public const string DecryptionFailed = "APP801";
        public const string InternalServerError = "APP901";
        public const string NetworkConnectionFailed = "APP005";
        public const string ServerNotFound = "APP008";
        public const string RequestProcessingFailed = "APP006";
        public const string PasswordReset = "RESET_PASSWORD";
        public const string RequestPasswordReset = "REQUEST_PASSWORD_RESET";
        public const string TooSimplePassword = "TOO_SIMPLE_PASSWORD";
        public const string NewPasswordEmpty = "RESET_PWD001";
        public const string ConfirmPasswordEmpty = "RESET_PWD002";
        public const string PasswordsNotMatch = "RESET_PWD003";
        public const string LicenseAgreement = "LICENSE_AGREEMENT";
        public const string IsClientOrTymetrixUser = "ISCLIENT_TYMETRIXUSER";
        public const string HasInvoiceListAccess = "HAS_INVOICELIST_ACCESS";
        public const string MobileAccess = "INVOICE001";
        public const string AdjustAmountEmpty = "INVOICE008";
        public const string ReasonEmpty = "INVOICE006";
        public const string NarrativeEmpty = "INVOICE003";
        public const string NotInReviewerQueue = "INVOICE024";
        public const string LOGIN001 = "LOGIN001";
        public const string LOGIN002 = "LOGIN002";
        public const string LOGIN003 = "LOGIN003";

        public const string HandlerErrorMsg = "A handler is already registered!";
        public const string SessionTimeOutMsg = "Application session timed out. Please login to continue.";
        public const string UnknownErrorMsg = "Unknown error occurred during the process.";
        public const string NotSelectedInvoiceConfirmMsg = "You must select an invoice to Confirm.";
        public const string NotSelectedInvoiceDetailsMsg = "You must select an invoice to see its' details.";
        public const string InvoiceAdjustMsg = "This invoice contains taxes and cannot be adjusted.";
        public const string NotSelectedInvoiceAdjustMsg = "You must first select an invoice to Adjust.";
        public const string NotSelectedInvoiceRejectMsg = "You must select an invoice to Reject.";
        public const string PageCreationMsg = "Failed to create initial page";
        public const string ServerNotConnectedMsg = "Unable to connect to the remote server";

        private static Dictionary<string, string> ServiceErrors()
        {
            var result = new Dictionary<string, string>();
            result.Add("A100", "Aaa111");
            result.Add("APP005", "Unable to connect network. Enable network connection and try again.");
            result.Add("APP006", "Unable to process the request due to network error.");
            result.Add("APP007", "Server encounter an error unable to process the request.");
            result.Add("APP008", "The server could not be found but may be available again in the future.");
            result.Add("APP200", "Request did not succeed.");
            result.Add("APP800", "Error occured during Encryption.");
            result.Add("APP801", "Error occured during Decryption.");
            result.Add("APP900", "Connection Failed.");
            result.Add("APP901", "Internal Server Error.");
            result.Add("ApproveError", "You are the last person in the review route and you do not have an authorization amount high enough to approve it. Please contact your administrator.");
            result.Add("COMPANY_IS_DISABLED", "Company is disabled.");
            result.Add("CONFIDENTIALITY_DISCLAIMER", "Member must accept the network Confidentiality Disclaimer. Please contact your network administrator.");
            result.Add("HAS_INVOICELIST_ACCESS", "You do not have access to Invoices Awaiting My Review. Please contact your network administrator.");
            result.Add("INCORRECT_LOGIN_PASSWORD", "Invalid Username or Password.");
            result.Add("UNAUTHORIZED_USER", "User not authorized.");
            result.Add("INCORRECT_SSO_LOGIN", "Unable to authenticate integrated login. Contact your company administrator for details.");
            result.Add("InsufficientNumberOfReviewers", "This invoice cannot be approved because the Minimum Number of Reviewers setting has not been met. Please contact your T360 Administrator.");
            result.Add("INVOICE001", "You do not have access to T360° Mobile. Please contact your network administrator.");
            result.Add("INVOICE002", "TyMetrix 360 mobile is not configured for Law Firm or Vendor use at this time. We look forward to adding this functionality and we will notify you when these features have been added.");
            result.Add("INVOICE003", "Enter narrative text.");
            result.Add("INVOICE004", "Action type is empty. Unable to fetch the reasons.");
            result.Add("INVOICE005", "Action type is invalid. Unable to fetch the reasons.");
            result.Add("INVOICE006", "Select Reason.");
            result.Add("INVOICE007", "Select adjustment mode.");
            result.Add("INVOICE008", "Enter adjustment amount.");
            result.Add("INVOICE009", "Select adjustment type.");
            result.Add("INVOICE010", "Select adjustment style.");
            result.Add("INVOICE011", "Net total is empty.");
            result.Add("INVOICE012", "LineItem Id is empty.");
            result.Add("INVOICE013", "Invalid Invoice.");
            result.Add("INVOICE014", "Invalid LineItem.");
            result.Add("INVOICE015", "Invalid Access.");
            result.Add("INVOICE016", "You do not have access to Invoices Awaiting My Review. Please contact your network administrator.");
            result.Add("INVOICE017", "You do not have the proper access to view invoices. Please contact your network administrator.");
            result.Add("INVOICE018", "You do not have the proper access to approve invoice. Please contact your network administrator.");
            result.Add("INVOICE019", "Taxed and Adjusted invoices cannot be Approved.");
            result.Add("INVOICE020", "You do not have the proper access to adjust invoice. Please contact your network administrator.");
            result.Add("INVOICE021", "You do not have the proper access to reject invoice. Please contact your network administrator.");
            result.Add("INVOICE022", "This invoice contains taxes and cannot be adjusted.");
            result.Add("INVOICE023", "A negative Invoice balance is not allowed.");
            result.Add("INVOICE024", "Current invoice doesn't belong to reviewer’s queue.");
            result.Add("INVOICE025", "A Positive Credit Note balance is not allowed.");
            result.Add("INVOICE026", "The following Credit Note(s) result in a negative balance of the original invoice. Please edit and resubmit.");
            result.Add("ISCLIENT_TYMETRIXUSER", "TyMetrix 360 mobile is not configured for Law Firm or Vendor use at this time. We look forward to adding this functionality and we will notify you when these features have been added.");
            result.Add("LAST_USED_NETWORK_IS_DISABLED", "Last used network is disabled.");
            result.Add("LICENSE_AGREEMENT", "Member must accept the License Agreement. Please contact your network administrator.");
            result.Add("LOGIN001", "Enter Username.");
            result.Add("LOGIN002", "Enter Password.");
            result.Add("LOGIN003", "Enter Integrated Login Id.");
            result.Add("LOGIN004", "Notification Id is empty.");
            result.Add("LOGIN005", "DeviceOS is empty.");
            result.Add("LOGIN006", "DeviceType is empty.");
            result.Add("LOGIN007", "Application version is empty.");
            result.Add("MatterSpendValidationError", "This invoice cannot be approved. %");
            result.Add("MEMBER_IS_DISABLED", "Your member account is disabled.");
            result.Add("MEMBER_IS_LOCKED", "Your member account is locked. Contact your company administrator for details.");
            result.Add("PRELOGIN001", "Client is not approved.");
            result.Add("PRELOGIN002", "Client is not authenticated.");
            result.Add("PRELOGIN003", "Payload data received from the client is empty.");
            result.Add("REQUEST_PASSWORD_RESET", "Your password needs to be reset, please log into the desktop version of T360 to change your password.");
            result.Add("RESET_PASSWORD", "Your password needs to be reset, please log into the desktop version of T360 to change your password.");
            result.Add("RESET_PWD001", "New password is required.");
            result.Add("RESET_PWD002", "Confirm password is required.");
            result.Add("RESET_PWD003", "New and confirm passwords must match.");
            result.Add("RouteError", "Review route policy violation detected.  You cannot approve the invoice this time. Please contact your administrator.");
            result.Add("SessionTimedOut", "Application session timed out. Please login to continue.");
            result.Add("TaxedAdjustedInvoice", "Taxed and Adjusted invoices cannot be Approved.");
            result.Add("THERE_IS_NOT_NETWORK", "The account is not associated with any active networks and therefore can not be logged into the system.");
            result.Add("TOO_SIMPLE_PASSWORD", "Your password has expired, please log into the desktop version of T360 to change your password.");
            result.Add("UNKNOWN_EXCEPTION", "Unknown error occurred during the process.");

            return result;
            
        }

        private static Dictionary<string, string> errors;
        private static Dictionary<string, string> Errors
        {
            get
            {
                if (errors == null) errors = ServiceErrors();
                return errors;
            }
        }

        public static string GetError(string input)
        {
            if (input.Contains("[{\"Code\":\""))
            {
                var temp = input.Replace("[{\"Code\":\"",string.Empty);
               input = temp.Substring(0,temp.IndexOf("\""));
            }
            if (Errors.ContainsKey(input))
            {
                return Errors[input];
            }
            return input;
        }
    }
}
