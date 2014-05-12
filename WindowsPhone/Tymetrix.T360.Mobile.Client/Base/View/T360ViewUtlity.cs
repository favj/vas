/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;
using System.Text;
using System.Windows;
using Tymetrix.T360.Mobile.Client.Common.Base.Util;
using Tymetrix.T360.Mobile.Client.Core;
using Tymetrix.T360.Mobile.Client.Model.Base;

namespace Tymetrix.T360.Mobile.Client.Common.Base.View
{
    public static class T360ViewUtlity
    {
        public static MessageBoxResult Handle(CultureManager cm, AppException cause, string title="Error")
        {
            List<Error> errorMessages = cause.GetErrorMessage(cm.GetCulture(CultureType.Message.ToString()));
            StringBuilder messages = new StringBuilder();
            string errorType = string.Empty;
            foreach (Error exceptionData in errorMessages)
            {
                messages.Append("\n");
                if (exceptionData.Data == null || exceptionData.Data.Count == 0)
                {
                    messages.Append(exceptionData.Code);
                }
                else
                {
                    foreach (string errorData in exceptionData.Data)
                    {
                        messages.Append(errorData);
                        if (exceptionData.Data.Count > 1)
                        {
                            messages.Append("\n");
                        }
                    }
                }
            }
            return MessageBox.Show(messages.ToString(), title, MessageBoxButton.OK);
        }
    }
}
