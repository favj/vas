/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;

namespace TyMetrix360.Dto.Common
{
    public class BaseTaxItemDto 
    {
        public string Key { get; set; }
        public List<TaxValueDto> Value { get; set; }      
    }
}
