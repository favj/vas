/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Resources;

namespace Tymetrix.T360.Mobile.Client.Core
{
    public class AppException : Exception
    {
        #region Fields
        private List<Error> errors;
        #endregion

        #region Constructors

        public AppException() { }

        public AppException(string errorCode, Exception cause)
            : base(errorCode, cause)
        {
            errors = new List<Error>();
            if (errorCode != null)
            {
                Error error = new Error(errorCode, cause.Message);
                errors.Add(error);
            }
        }

        public AppException(string errorCode, WebException cause)
            : base(null, cause)
        {
            Error error = new Error(errorCode, (errorCode == T360ErrorCodes.ConnectFailure) ? cause.Message : null);
            errors = new List<Error>();
            errors.Add(error);
        }

        public AppException(string errorCode)
            : base(errorCode)
        {
            Error error = new Error(errorCode);
            errors = new List<Error>();
            errors.Add(error);
        }

        public AppException(string errorCode, string errorData)
            : base(errorCode)
        {
            Error error = new Error(errorCode, errorData);
            errors = new List<Error>();
            errors.Add(error);
        }

        public AppException(List<Error> errors)
        {
            this.errors = errors;
        }

        #endregion

        #region Methods

        public List<Error> GetErrorMessage(ResourceManager rm)
        {
            foreach (Error error in errors)
            {
                if (error.Code == T360ErrorCodes.ConnectFailure)
                {
                    if (error.Data != null)
                    {
                        error.Data[0] = string.Format(CultureInfo.CurrentCulture,
                                                            rm.GetString(error.Code),
                                                            error.Data[0]);

                        error.Data[0] = null;
                    }
                }
                else if (error.Data == null)
                {
                    error.Data = new List<string>();
                    error.Data.Add(string.Empty);
                    error.Data[0] = rm.GetString(error.Code);
                    if (string.IsNullOrWhiteSpace(error.Data[0]))
                    {
                        error.Data[0] = error.Code;
                    }
                }
                else
                {
                    string tmpData = error.Data[0];
                    error.Data[0] = rm.GetString(error.Code);
                    if (string.IsNullOrEmpty(error.Data[0]))
                    {
                        error.Data[0] = tmpData;
                    }
                    else if (error.Data.Contains("%"))
                    {
                        if (tmpData.Contains("~#~"))
                        {
                            error.Data[0] = error.Data[0].Replace("%", "\n" + tmpData.Replace("~#~", "\n"));
                        }
                        else
                        {
                            error.Data[0] = error.Data[0].Replace("%", "\n" + tmpData);
                        }
                    }
                }
            }
            return errors;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the errorcodes
        /// </summary>
        public List<Error> ErrorCodes
        {
            get { return errors; }
        }

        #endregion

    }

    public class QuitException : Exception { }
}
