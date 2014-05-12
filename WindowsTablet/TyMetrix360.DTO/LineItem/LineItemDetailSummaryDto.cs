/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;

using TyMetrix360.Dto.Common;
using TyMetrix360.Dto.Invoice;

namespace TyMetrix360.Dto.LineItem
{
    public class LineItemDetailSummaryDto
    {
        public IEnumerable<AdjustmentDto> AdjustmentList { get; set; }
        public string Date { get; set; }
        public IEnumerable<FlagDetailsDto> Flagslist { get; set; }
        public string ITPAdjustment { get; set; }
        public string NarrativeText { get; set; }
        public string NetAmount { get; set; }
        public string NetTotal { get; set; }
        public bool NotesAllowed { get; set; }
        public IEnumerable<NoteDto> NotesList {get; set;}
        public string ReviewerAdjustment { get; set; }
        public IEnumerable<BaseTaxItemDto> TaxList { get; set; }
        public string TimeKeeper { get; set; }
        public string VendorActivity { get; set; }
        public string VendorAdjustment { get; set; }
        public string VendorBilledTotal { get; set; }
        public string VendorRate { get; set; }
        public string VendorTask { get; set; }
        public string VendorUnits { get; set; }       
    }
}
