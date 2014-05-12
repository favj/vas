/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Collections.Generic;

namespace TyMetrix360.Core.Models
{
    public class T360Exception : Exception
    {
        #region Fields
        private List<Error> errors;
        #endregion

        #region Constructors

        public T360Exception() { }

        public T360Exception(string errorCode, Exception cause)
            : base(errorCode, cause)
        {
            errors = new List<Error>();
            if (errorCode != null)
            {
                Error error = new Error(errorCode, cause.Message);
                errors.Add(error);
            }
        }

        public T360Exception(string errorCode)
            : base(errorCode)
        {
            Error error = new Error(errorCode);
            errors = new List<Error>();
            errors.Add(error);
        }

        public T360Exception(string errorCode, string errorData)
            : base(errorCode)
        {
            Error error = new Error(errorCode, errorData);
            errors = new List<Error>();
            errors.Add(error);
        }

        public T360Exception(List<Error> errors)
        {
            this.errors = errors;
        }

        #endregion

        #region Properties

        public List<Error> ErrorCodes
        {
            get { return errors; }
        }

        #endregion

    }

    public class QuitException : Exception { }
}
