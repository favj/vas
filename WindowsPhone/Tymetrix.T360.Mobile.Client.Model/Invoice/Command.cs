/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Windows.Input;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    public class Command : ICommand
    {
        private Action<object> _execute;
        public Command(Action<object> execute)
        {
            _execute = execute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) { return true; }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
