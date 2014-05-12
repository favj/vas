/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;

using TyMetrix360.Core.Models;

namespace TyMetrix360.BusinessObjects.Services
{
    public interface IServiceResult<out T>
    {
        T Result { get; }
        List<Error> Errors { get; }
        bool Success { get; }
    }
}
