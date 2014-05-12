/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Microsoft.Phone.Net.NetworkInformation;

namespace Tymetrix.T360.Mobile.Client.Common.Base.Util
{
    public static class Reachability
    {
        public static bool IsNetworkAvailable
        {
            get
            {
                return NetworkInterface.GetIsNetworkAvailable();
            }
        }

        public static NetworkInterfaceType NetworkInterfaceType
        {
            get
            {
                return NetworkInterface.NetworkInterfaceType;
            }
        }
    }
}
