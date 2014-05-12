/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */
namespace TyMetrix360.Core.ViewBase
{
    public interface IViewCore
    {
        object DataContext { get; set; }
        double Height { get; set; }
        double Width { get; set; }
    }
}
