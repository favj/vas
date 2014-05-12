/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Windows.Input;

namespace TyMetrix360.Core
{
    public interface IRelayCommand : ICommand
    {
        void Execute();
    }
}
