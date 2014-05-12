/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;

namespace TyMetrix360.Core
{
    public class RelayCommand : IRelayCommand
    {
        private Action<object> _execute;
        private Predicate<object> _canExecute;
        public event EventHandler CanExecuteChanged;
        public RelayCommand(Action<object> execute) : this(null, execute) { }
        public RelayCommand(Predicate<object> canExecute, Action<object> execute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            return _canExecute != null ? _canExecute(parameter) : true;
        }
        public void Execute()
        {
            this.Execute(_execute);
        }
        public void Execute(object parameter)
        {
            if (_execute != null)
            {
                _execute(parameter);
            }
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

    }
}

