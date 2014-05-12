/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Windows.System;

namespace TyMetrix360.BusinessObjects.Common
{
    public static class InputHelper
    {
        public static bool isValidNumberKey(this VirtualKey NumericKey)
        {
            switch (NumericKey)
            {
                case VirtualKey.Number0:
                    return true;
                case VirtualKey.Number1:
                    return true;
                case VirtualKey.Number2:
                    return true;
                case VirtualKey.Number3:
                    return true;
                case VirtualKey.Number4:
                    return true;
                case VirtualKey.Number5:
                    return true;
                case VirtualKey.Number6:
                    return true;
                case VirtualKey.Number7:
                    return true;
                case VirtualKey.Number8:
                    return true;
                case VirtualKey.Number9:
                    return true;
                case VirtualKey.NumberPad0:
                    return true;
                case VirtualKey.NumberPad1:
                    return true;
                case VirtualKey.NumberPad2:
                    return true;
                case VirtualKey.NumberPad3:
                    return true;
                case VirtualKey.NumberPad4:
                    return true;
                case VirtualKey.NumberPad5:
                    return true;
                case VirtualKey.NumberPad6:
                    return true;
                case VirtualKey.NumberPad7:
                    return true;
                case VirtualKey.NumberPad8:
                    return true;
                case VirtualKey.NumberPad9:
                    return true;
                case VirtualKey.Decimal:
                    return true;
                default:
                    return false;
            }
        }
    }
}
