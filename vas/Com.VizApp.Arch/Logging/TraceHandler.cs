/*
* @(#)TraceHandler.cs
*
* Copyright (c) 2014, VizitSolutions.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;

namespace Com.VizApp.Arch.Logging
{
    [Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ConfigurationElementType(typeof(CustomCallHandlerData))]
    public class TraceHandler : ICallHandler
    {
        public TraceHandler(NameValueCollection collection)
        {
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            StringBuilder builder = new StringBuilder();

            var returnValue = getNext()(input, getNext);

            List<string> arguments = new List<string>();
            bool first = true;

            if (input.Arguments.Count > 0)
            {
                builder.AppendFormat("Input Params \n");
            }

            for (int index = 0; index < input.Arguments.Count; index++)
            {
                if (!first) builder.Append(", ");

                string paramName = input.Arguments.ParameterName(index);
                ParameterInfo info = input.Arguments.GetParameterInfo(paramName);
                builder.AppendFormat("\t {0} = {1}\n", info, input.Arguments[paramName]);

                first = false;
            }

            if (returnValue.ReturnValue != null)
            {
                builder.AppendFormat("Return Value \n");
                builder.AppendFormat("\t {0}", returnValue.ReturnValue);
            }

            if (returnValue.Exception != null)
            {
                builder.AppendFormat("Exception \n");
                builder.AppendFormat("\t {0}", returnValue.Exception);
            }

            Logger.Trace(string.Format("Entering - {0}.{1}\n{2}", input.Target.GetType().FullName, input.MethodBase.Name, builder.ToString()));
            Logger.Trace(string.Format("Exiting  - {0}.{1}\n", input.Target.GetType().FullName, input.MethodBase.Name));

            return returnValue;
        }

        public int Order { get; set; }
    }
}
