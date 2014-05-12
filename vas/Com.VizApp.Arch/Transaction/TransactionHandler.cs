/*
* @(#)TransactionHandler.cs
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
using System.Collections.Specialized;
using System.Transactions;

namespace Com.VizApp.Arch.Transaction
{
    [Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ConfigurationElementType(typeof(CustomCallHandlerData))]
    public class TransactionHandler : ICallHandler
    {
        public TransactionHandler(NameValueCollection collection)
        {
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IMethodReturn returnValue = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadCommitted;
            transactionOptions.Timeout = TransactionManager.MaximumTimeout;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                returnValue = getNext()(input, getNext);
                if (returnValue.Exception == null)
                {
                    scope.Complete();
                }
            }
            return returnValue;
        }

        public int Order { get; set; }
    }
}
