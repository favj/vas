/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Threading.Tasks;

using TyMetrix360.BusinessObjects.Invoice;
using TyMetrix360.Core.ViewModelBase;

namespace TyMetrix360.App.ViewModel
{
    public interface IAdjustmentViewModel : IViewModelCore
    {
        void SaveAdjustment();
        void Calculate();
        Task SetReasonCodes();

        AdjustmentToggleType AdjustmentToggleType { get; set; }
        AdjustmentType AdjustmentType { get; set; }
        InvoiceSummary InvoiceSummary { get; set; }
        AdjustmentByTo AdjustmentByTo { get; set; }
        string NetAmount { get; set; }
        double AdjustedAmount { get; set; }
    }
}
