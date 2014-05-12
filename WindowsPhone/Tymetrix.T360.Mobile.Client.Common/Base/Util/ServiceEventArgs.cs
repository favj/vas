/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using Tymetrix.T360.Mobile.Client.Model.Base;

namespace Tymetrix.T360.Mobile.Client.Common.Base.Util
{
    public class ServiceEventArgs : EventArgs
    {
        public ServiceResponse Result { get; set; }
    } 
}
