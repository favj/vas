/*
* @(#)AppException.cs
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

namespace Com.VizApp.Arch
{
    public class AppException : Exception
    {
        private List<Error> errors;

        public AppException() { }

        public AppException(string errorCode, System.Exception cause)
            : base(errorCode, cause)
        {
            errors = new List<Error>();
            if (errorCode != null)
            {
                Error error = new Error(errorCode, cause.Message);
                errors.Add(error);
            }
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

        /// <summary>
        /// Returns the errorcodes
        /// </summary>
        public List<Error> Errors
        {
            get { return errors; }
        }
    }
}
