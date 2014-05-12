/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;
using Tymetrix.T360.Mobile.Client.Core;
using Tymetrix.T360.Mobile.Client.Model.Base;
using Tymetrix.T360.Mobile.Client.Model.Invoice;
using Tymetrix.T360.Mobile.Client.Model.ResetPassword;

namespace Tymetrix.T360.Mobile.Client.Common.Base.Util
{
    public class BaseValidator
    {
        #region Fields

        private List<Error> errors;

        #endregion

        #region Methods

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

        public bool ValidateLogOnFields(UserData userData)
        {
            if (string.IsNullOrEmpty(userData.UserName))
            {
                AddError(T360ErrorCodes.UserIdEmpty);
            }
            if (string.IsNullOrEmpty(userData.Password))
            {
                AddError(T360ErrorCodes.PasswordEmpty);
            }
            if (userData.IsSSOEnabled)
            {
                if (string.IsNullOrEmpty(userData.IntegratedLoginId))
                {
                    AddError(T360ErrorCodes.IntegratedLoginIdEmpty);
                }
            }
            return NoErrors;
        }

        public bool AdjustInvoice(AdjustInputDetails adjustInvoice)
        {
            if (string.IsNullOrEmpty(adjustInvoice.AdjustmentAmount) || "-0".Equals(adjustInvoice.AdjustmentAmount) || "0".Equals(adjustInvoice.AdjustmentAmount))
            {
                AddError(T360ErrorCodes.AdjustAmountEmpty);
            }
            else if (adjustInvoice.AdjustmentAmount.Trim() == "-")
            {
                AddError(T360ErrorCodes.AdjustAmountEmpty);
            }
            else if (string.IsNullOrEmpty(adjustInvoice.ReasonId))
            {
                AddError(T360ErrorCodes.ReasonEmpty);
            }
            return NoErrors;
        }

        public bool RejectInvoice(RejectInputMultipleInvoice rejectInvoice)
        {
            if (string.IsNullOrEmpty(rejectInvoice.ReasonId))
            {
                AddError(T360ErrorCodes.ReasonEmpty);
            }
            else if (string.IsNullOrEmpty(rejectInvoice.NarrativeText))
            {
                AddError(T360ErrorCodes.NarrativeEmpty);
            }
            return NoErrors;
        }

        public bool RejectInvoice(RejectInputDetails rejectInvoice)
        {
            if (string.IsNullOrEmpty(rejectInvoice.ReasonId))
            {
                AddError(T360ErrorCodes.ReasonEmpty);
            }
            else if (string.IsNullOrEmpty(rejectInvoice.NarrativeText))
            {
                AddError(T360ErrorCodes.NarrativeEmpty);
            }
            return NoErrors;
        }

        public bool ResetPassword(Credential credential)
        {
            if (string.IsNullOrWhiteSpace(credential.Password))
            {
                AddError(T360ErrorCodes.NewPasswordEmpty);
            }
            if (string.IsNullOrWhiteSpace(credential.ConfirmPassword))
            {
                AddError(T360ErrorCodes.ConfirmPasswordEmpty);
            }
            if (credential.Password != credential.ConfirmPassword)
            {
                AddError(T360ErrorCodes.PasswordsNotMatch);
            }
            return NoErrors;
        }

        #endregion
    }
}
