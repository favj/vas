/*
* @(#)Error.cs
*
* Copyright (c) 2014, VizitSolutions.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

using System.Collections.Generic;

namespace Com.VizApp.Arch
{
    public class Error
    {
        private string code;
        private List<string> data;

        public Error()
        {
        }

        public Error(string errorCode)
        {
            this.code = errorCode;
        }

        public Error(string errorCode, string errordata)
        {
            this.code = errorCode;
            this.data = new List<string>();
            this.data.Add(errordata);

        }

        public Error(string errorCode, List<string> errordata)
        {
            this.code = errorCode;
            this.data = errordata;
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public List<string> Data
        {
            get { return data; }
            set { data = value; }
        }
    }
}
