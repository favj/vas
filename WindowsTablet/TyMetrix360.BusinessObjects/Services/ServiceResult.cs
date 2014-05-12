/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Newtonsoft.Json;
using System.Collections.Generic;

using TyMetrix360.Core.Models;

namespace TyMetrix360.BusinessObjects.Services
{
    public class ServiceResult<T> : IServiceResult<T>
    {
        public ServiceResult(T result, string error, bool success)
        {
            Result = result;
            ErrorString = error;
            Success = success;
        }

        public T Result { get; private set; }
        public bool Success { get; private set; }

        private List<Error> errors;
        public List<Error> Errors { get { return Errors; } }

        private string errorString;
        private string ErrorString
        {
            get { return errorString; }
            set
            {
                errorString = value;
                errors = string.IsNullOrEmpty(value) ? null : JsonConvert.DeserializeObject<List<Error>>(value);
            }
        }
    }
}