/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Newtonsoft.Json;
using System.Collections.Generic;

using Tymetrix.T360.Mobile.Client.Core;

namespace Tymetrix.T360.Mobile.Client.Model.Base
{
    public class ServiceResponse
    {
        private string errors;
        public bool Status { get; set; }

        public string Errors
        {
            get
            {
                return errors;
            }
            set
            {
                if (value != null && !Status)
                {
                    errors = value;
                    this.ErrorDetails = JsonConvert.DeserializeObject<List<Error>>(value);
                }
            }
        }

        public List<Error> ErrorDetails { get; set; }
        public string Output { get; set; }
    }
}
