/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */
namespace TyMetrix360.Dto.Common
{
    public class TaxValueDto
    {
        public string TaxJurisdictionCode { set; get; }
        public string TaxTypeCode { get; set; }
        public string TaxRate { get; set; }
        public string TaxAmount { get; set; }
        public string TaxableAmount { get; set; }
    }
}
