/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;

namespace Tymetrix.T360.Mobile.Client.Model.Base
{
    public class Error
    {
        #region Fields

        private string errorCode;
        private List<string> errorData;

        #endregion

        #region Constructor

        public Error()
        {
        }

        public Error(string errorCode)
        {
            this.errorCode = errorCode;
        }

        public Error(string errorCode, string errorMessage)
        {
            this.errorCode = errorCode;
            List<string> errorData = new List<string>();
            errorData.Add(errorMessage);
            this.Data = errorData;
        }

        public Error(string errorCode, List<string> errorMessage)
        {
            this.errorCode = errorCode;
            this.Data = errorMessage;
        }

        #endregion

        #region Properties
        //[DataMember(Name = "Code")]
        public string Code
        {
            get { return errorCode; }
            set { errorCode = value; }
        }

        //[DataMember(Name = "Data")]
        public List<string> Data
        {
            get { return errorData; }
            set { errorData = value; }
        }

        #endregion
    }
}
