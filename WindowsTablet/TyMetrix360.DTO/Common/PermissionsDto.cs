/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */
namespace TyMetrix360.Dto.Common
{
    public class PermissionsDto
    {
        public bool AdjustExpense { get; set; }
        public bool AdjustFee { get; set; }
        public bool AdjustInvoiceAllowed { get; set; }
        public bool Approve { get; set; }
        public object NegativeAdjust { get; set; }
        public bool Notes { get; set; }
        public bool PositiveAdjustmentAllowed { get; set; }
        public bool Properties { get; set; }
        public bool Reject { get; set; }
    }
}