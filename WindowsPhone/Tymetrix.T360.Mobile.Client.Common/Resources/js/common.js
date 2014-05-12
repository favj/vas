function constructInvoiceList(str)
{
	var i = 0;
	var invoiceListHtml = "";
   
    var invoices = JSON.parse(str);   
    if (invoices.length == 0)
    {
        invoiceListHtml = '<li class="novalue">"No Pending Invoices"</li>';
        document.getElementById("invoicelist").innerHTML = invoiceListHtml;
        return "YES";
    }
    
    // Invoice List
    for (i=0; i<invoices.length; i++) 
    {
        invoiceListHtml += '<li class="inv-list" tabindex="1" onclick="notifyInvoiceSelection('+invoices[i].InvoiceId+')" id="'+invoices[i].InvoiceId+'"><div id="inv-nowrap"><h1><label for="inv-no">Inv # </label> <span id="inv-no" class="gen-values">'+invoices[i].InvoiceNumber+'</span></h1></div>'
        invoiceListHtml += '<div id="inv-namewrap" class="truncatemore"><span id="inv-name">'+invoices[i].CompanyName+'</span></div>'
        invoiceListHtml += '<div id="inv-infowrap"  class="clear"><div id="infoleft">'
        invoiceListHtml += '<div id="inv-company" class="gen-values"><span class="truncatemore">'+invoices[i].MatterName+'</span></div><div id="inv-date" class="gen-values">'+invoices[i].InvoiceDate+'</div></div>'
        invoiceListHtml += '<div id="inforight"><div id="inv-amount"><label for="amount">Billed&nbsp;</label><span id="amount" class="gen-values">'+invoices[i].BilledAmount+'</span></div>'
        invoiceListHtml += ' <div id="inv-name"><label for="amount">Net&nbsp;</label><span id="amount" class="gen-values">'+invoices[i].NetAmount+'</span></div></div></div></li>'
    }
    document.getElementById("invoicelist").innerHTML = invoiceListHtml;
   
   document.getElementsByClassName('inv-list')[0].className+=" active";
    return "YES";
}

function appendElement(strElement)
{
    document.getElementById("invoicelist").appendChild (strElement);
}

function constructSummary(invoiceSummaryString) 
{
    var i = 0;
    var j = 0;
    var invoiceSummary = JSON.parse(invoiceSummaryString);

    // Invoice Details
    document.getElementById("invoicedate").innerHTML = invoiceSummary.InvoiceDate;
    document.getElementById("billingperiod").innerHTML = invoiceSummary.BillingPeriod;
    document.getElementById("totalbilledamount").innerHTML = invoiceSummary.TotalBilledAmount;   
    document.getElementById("billedfees").innerHTML = invoiceSummary.BilledFees;
    document.getElementById("billedexpense").innerHTML = invoiceSummary.BilledExpenses;
    document.getElementById("netamount").innerHTML = invoiceSummary.NetAmount;
    document.getElementById("flags").innerHTML = invoiceSummary.Flags.length;
    document.getElementById("status").innerHTML = invoiceSummary.Status;
    
    // Invoice Flags
    var invoiceflagHtml ="";
    if (invoiceSummary.Flags.length == 0)
    {
        invoiceflagHtml += '<li class="NoneRow clear"><label>None</label></li>'
       
    }
    else
    {
        for (i = 0; i <invoiceSummary.Flags.length; i++)
        {
           
            if (invoiceSummary.Flags[i].Priority == "FlagInvoiceHigh")
            {
               
            invoiceflagHtml += (i == invoiceSummary.Flags.length - 1) ? '<li class="NoBorder clear hp">' : '<li class="clear hp">'
            invoiceflagHtml += '<label>'+invoiceSummary.Flags[i].DisplayName+'</label></li>'
            }
           
            else if (invoiceSummary.Flags[i].Priority == "FlagInvoiceMedium")
            {
               
                invoiceflagHtml += (i == invoiceSummary.Flags.length - 1) ? '<li class="NoBorder clear mp">' : '<li class="clear mp">'
                invoiceflagHtml += '<label>'+invoiceSummary.Flags[i].DisplayName+'</label></li>'
            }
           
            else if (invoiceSummary.Flags[i].Priority == "FlagInvoiceLow")
            {
               
                invoiceflagHtml += (i == invoiceSummary.Flags.length - 1) ? '<li class="NoBorder clear lp">' : '<li class="clear lp">'
                invoiceflagHtml += '<label>'+invoiceSummary.Flags[i].DisplayName+'</label></li>'
            }
        }
    }
    document.getElementById("invoiceflags").innerHTML = invoiceflagHtml;
    
    // ReviewRoute
    var reviewRouteHtml = "";
    if (invoiceSummary.ReviewRouteList.length == 0)
    {
        reviewRouteHtml += '<li class="NoneRow clear"><label>None</label></li>';
    }
    else
    {
        for (i = 0; i < invoiceSummary.ReviewRouteList.length; i++)
        {
            reviewRouteHtml += (i == invoiceSummary.ReviewRouteList.length - 1) ? '<li class="NoBorder clear">' : '<li class="clear">'
            reviewRouteHtml += '<label>'+invoiceSummary.ReviewRouteList[i].ReviewerName+'</label>'           
            if (invoiceSummary.ReviewRouteList[i].ReviewStatus == "Reviewed")
            {
                reviewRouteHtml += '<span class="checkedflag"></span></li>'
            }
            else if (invoiceSummary.ReviewRouteList[i].ReviewStatus == "InReview")
            {
                reviewRouteHtml +='<span> Current Reviewer </span></li>'
            }
            else if (invoiceSummary.ReviewRouteList[i].ReviewStatus == "YetToReview")
            {
                reviewRouteHtml +='<span class="uncheckedflag"></span></li>'
            }
        }
    }
    document.getElementById("reviewroute").innerHTML = reviewRouteHtml;
    
    // Vendor Billing
    document.getElementById("currenttype").innerHTML = invoiceSummary.CurrencyType;
    document.getElementById("grossamount").innerHTML = invoiceSummary.GrossAmount;
    document.getElementById("vendoradjustment").innerHTML = invoiceSummary.VendorAdjustment;
    document.getElementById("billedamount").innerHTML = invoiceSummary.BilledAmount;

    // In-House Review
    document.getElementById("reviewadjustment").innerHTML = invoiceSummary.ReviewerAdjustment;
    document.getElementById("itpadjustment").innerHTML = invoiceSummary.ItpAdjustment;
    document.getElementById("subtotal").innerHTML = invoiceSummary.SubTotal;
    document.getElementById("tax").innerHTML = invoiceSummary.Tax;   
    document.getElementById("nettotal").innerHTML = invoiceSummary.NetTotal;
    
    // Discounts
    document.getElementById("proposedcredit").innerHTML = invoiceSummary.Credit;    
    document.getElementById("totalwithcredit").innerHTML = invoiceSummary.TotalWithCredit;

    // Common Properties
    var commonPropertiesHtml = "";
    if (invoiceSummary.CommonProperties.length == 0)
    {
        commonPropertiesHtml += '<li class="NoneRow clear"><label>None</label></li>'
    }
    else
    {
        for (i = 0; i < invoiceSummary.CommonProperties.length; i++)
        {
            commonPropertiesHtml += (i == invoiceSummary.CommonProperties.length - 1) ? '<li class="NoBorder clear">' : '<li class="clear">'
            commonPropertiesHtml += '<label>'+invoiceSummary.CommonProperties[i].LabelText+'</label><span>'+invoiceSummary.CommonProperties[i].ValueText+'</span></li>'       
        }
    }
    document.getElementById("commonproperties").innerHTML = commonPropertiesHtml;
    
    // Properties
    if (!invoiceSummary.Permissions.Properties)
    {
        document.getElementById("prop").style.display="none";
    }
    else
    {
        document.getElementById("prop").style.display="compact";
        var propertiesHtml = "";
        if (invoiceSummary.Properties.length == 0)
        {
            propertiesHtml += '<li class="NoneRow clear"><label>None</label></li>'
        }
        else
        {
            for (i = 0; i < invoiceSummary.Properties.length; i++)
            {
                propertiesHtml += (i == invoiceSummary.Properties.length - 1) ? '<li class="NoBorder clear">' : '<li class="clear">'
                propertiesHtml += '<label>'+invoiceSummary.Properties[i].LabelText+'</label><span>'+invoiceSummary.Properties[i].ValueText+'</span></li>'
            }
        }
		document.getElementById("properties").innerHTML = propertiesHtml;
    }
   
    // Notes
    if (!invoiceSummary.Permissions.Notes)
    {
        document.getElementById("notesDiv").style.display="none";
    }
    else
    {
        document.getElementById("notesDiv").style.display="compact";
        var notesHtml = "";
        if (invoiceSummary.Notes.length == 0)
        {
            notesHtml += '<li class="NoneRow clear"><label>None</label></li>'
        }
        else
        {
            for (i = 0;i < invoiceSummary.Notes.length; i++)
            {
                notesHtml += (i == invoiceSummary.Notes.length - 1) ? '<li class="NoBorder clear">' : '<li class="clear">'
                notesHtml += '<label>'+invoiceSummary.Notes[i].Creator+'</label><span>'+invoiceSummary.Notes[i].Description+'</span><span class="notesdate">'+invoiceSummary.Notes[i].CreatedTime+'</span></li>'
            }
        }
        document.getElementById("notes").innerHTML = notesHtml;
    }
    
    // Taxes
    if (invoiceSummary.TaxList.length == 0)
    {
        document.getElementById("taxes").style.display="none";
    }
    else
    {
        document.getElementById("taxes").style.display="compact";
        var taxesHtml = "";
        for(i = 0; i < invoiceSummary.TaxList.length; i++)
        {
            taxesHtml += '<h6 class="subtitle">'+invoiceSummary.TaxList[i].Key+'</h6>'
            taxesHtml += '<div class="contents"><ul>'
            
            for(j = 0; j < invoiceSummary.TaxList[i].Value.length; j++)
            {
                taxesHtml += '<li class="clear"><label>Tax Jurisdiction Code</label><span>'+invoiceSummary.TaxList[i].Value[j].TaxJurisdictionCode+'</span></li>'    
                taxesHtml += '<li class="clear"><label>Tax Type Code</label><span>'+invoiceSummary.TaxList[i].Value[j].TaxTypeCode+'</span></li>'
                taxesHtml += '<li class="clear"><label>Tax Rate</label><span>'+invoiceSummary.TaxList[i].Value[j].TaxRate+'</span></li>'
                taxesHtml += '<li class="clear"><label>Taxable Amount</label><span>'+invoiceSummary.TaxList[i].Value[j].TaxableAmount+'</span></li>'
                taxesHtml += '<li class="NoBorder clear"><label>Tax Amount</label><span>'+invoiceSummary.TaxList[i].Value[j].TaxAmount+'</span></li>'
            }
              
            taxesHtml += '</ul></div>' 
        }
        document.getElementById("subtaxes").innerHTML = taxesHtml;
    }
    return "YES";
}

function constructLineItemList(str)
{
    var i = 0;
	var lineItemListHtml = "";
    var lineItems = JSON.parse(str);   
    
    // LineItemList 
    for (i = 0; i < lineItems.length; i++) 
    {
        lineItemListHtml += '<li class="inv-list" tabindex="1" onClick="notifyLineItemSelection(\''+lineItems[i].LineItemId+'\');" id="'+lineItems[i].LineItemId+'"><div id="inv-infowrap" class="clear"><div id="infoleft"><div id="inv-namewrap" class="truncate">'
        lineItemListHtml += '<span id="inv-name">'+lineItems[i].TimeKeeper+'</span></div>'
        lineItemListHtml += '<div id="inv-description" class="gen-values truncate">'+lineItems[i].NarrativeText+'</div>'
        lineItemListHtml += '</div>'
        lineItemListHtml += '<div id="inforight"><div id="inv-amount"><span id="amount" class="gen-values">'+lineItems[i].Amount+'</span></div>'
        lineItemListHtml += '<div id="inv-date" class="gen-values">'+lineItems[i].Date+'</div>'
        lineItemListHtml += '<div id="inv-flag"><span id="flag">('+lineItems[i].Flags+')</span></div>'
        lineItemListHtml += '</div></div></li>'
        
        
    }
    document.getElementById("lineitemlist").innerHTML = lineItemListHtml;
    document.getElementsByClassName('inv-list')[0].className+=" active";
    return "YES";
   
}

function createAdjustmentObject(aOwner, aDesc, aAmount)
{
    var adjustmentobject = new Object();
    adjustmentobject.Owner = aOwner;
    adjustmentobject.Description = aDesc;
    adjustmentobject.Amount = aAmount;
    return adjustmentobject;
}

function constructLineItemDetails(itemDetailString) 
{
    var i = 0;
    var j = 0;
	var x = 0;
	var y = 0;
    var itemDetail = JSON.parse(itemDetailString);

    // General
    document.getElementById("date").innerHTML = itemDetail.Date;
    if (itemDetail.TimeKeeper.length > 0)
    {
        document.getElementById("tk").style.display="compact";
        document.getElementById("timekeeper").innerHTML = itemDetail.TimeKeeper;
    } else {
        document.getElementById("tk").style.display="none";
        
    }
    document.getElementById("billedamount").innerHTML = itemDetail.NetAmount;
    
    // Narrative
    document.getElementById("narrative").innerHTML = (itemDetail.NarrativeText.length == 0) ? "None" : itemDetail.NarrativeText;
    
    // Firm/Vendor
    document.getElementById("task").innerHTML = itemDetail.VendorTask;
    document.getElementById("activity").innerHTML = itemDetail.VendorActivity;
    document.getElementById("hours").innerHTML = itemDetail.VendorUnits;
    document.getElementById("rate").innerHTML = itemDetail.VendorRate;
    document.getElementById("vendoradjustment").innerHTML = itemDetail.VendorAdjustment;
    document.getElementById("firmbilledamount").innerHTML = itemDetail.VendorBilledTotal;
        
    //LineItem Flags
    var lineItemFlagsHtml = "";
    if (itemDetail.Flagslist.length == 0)
    {
        lineItemFlagsHtml += '<li class="NoneRow clear"><label>None</label></li>'
    }
    else
    {
        for (i = 0; i < itemDetail.Flagslist.length; i++)
        {
            if (itemDetail.Flagslist[i].Priority == "FlagLineItemHigh")
            {               
           
            lineItemFlagsHtml += (i == itemDetail.Flagslist.length - 1) ? '<li class="NoBorder clear hp">' : '<li class="clear hp">'
            lineItemFlagsHtml += '<label>'+itemDetail.Flagslist[i].WarningInfo+'</label></li>' 
            }
           
            else if (itemDetail.Flagslist[i].Priority == "FlagLineItemMedium")
            {               
               
                lineItemFlagsHtml += (i == itemDetail.Flagslist.length - 1) ? '<li class="NoBorder clear mp">' : '<li class="clear mp">'
                lineItemFlagsHtml += '<label>'+itemDetail.Flagslist[i].WarningInfo+'</label></li>' 
            }
           
            else if (itemDetail.Flagslist[i].Priority == "FlagLineItemLow")
            {               
               
                lineItemFlagsHtml += (i == itemDetail.Flagslist.length - 1) ? '<li class="NoBorder clear lp">' : '<li class="clear lp">'
                lineItemFlagsHtml += '<label>'+itemDetail.Flagslist[i].WarningInfo+'</label></li>' 
            }
           
        }
    }
    document.getElementById("flags").innerHTML = lineItemFlagsHtml;
    
    // In-House Review
    document.getElementById("itpadjustment").innerHTML = itemDetail.ItpAdjustment;
    document.getElementById("reviewadjustment").innerHTML = itemDetail.ReviewerAdjustment;
    document.getElementById("nettotal").innerHTML = itemDetail.NetTotal;
    
    // Adjustments
    var adjustmentHtml = "";
    var topics = new Array();
    var adjustmentContents = new Array();
    if (itemDetail.AdjustmentList.length == 0)
    {
        adjustmentHtml += '<h4 class="title">Adjustments</h4><div class="contents column3"><ul><li class="NoneRow clear"><label>None</label><span></span><label></label></li></ul></div>'
    }
    else
    {
        var topicCount = 0;
        for (i = 0; i < itemDetail.AdjustmentList.length; i++)
        {
           if(-1 == topics.indexOf(itemDetail.AdjustmentList[i].GroupDescription))
           {
               topics[topicCount] = itemDetail.AdjustmentList[i].GroupDescription;
               adjustmentContents[topicCount] = new Array();
               adjustmentContents[topicCount][0] = createAdjustmentObject(itemDetail.AdjustmentList[i].Owner, itemDetail.AdjustmentList[i].Description, itemDetail.AdjustmentList[i].Amount);
               topicCount ++;
           }
           else
           {
               var index = topics.indexOf(itemDetail.AdjustmentList[i].GroupDescription);
               var length = adjustmentContents[index].length;
               adjustmentContents[index][length] = createAdjustmentObject(itemDetail.AdjustmentList[i].Owner, itemDetail.AdjustmentList[i].Description, itemDetail.AdjustmentList[i].Amount);
           }
        }
        for (x = 0; x < topics.length; x++)
        {
            adjustmentHtml += '<h4 class="title">'+topics[x]+'</h4><div class="contents column3">'
            for (y = 0; y < adjustmentContents[x].length; y++)
            {
                if (y == adjustmentContents[x].length - 1)
                {
                    adjustmentHtml += '<ul><li class="NoBorder clear">'
                }
                else
                {
                    adjustmentHtml += '<ul><li class="clear">'                   
                }
                adjustmentHtml += '<label>'+adjustmentContents[x][y].Owner+'</label><span>'+adjustmentContents[x][y].Description+'</span><label>'+adjustmentContents[x][y].Amount+'</label></li>'
                adjustmentHtml += '</ul>'
            }
            adjustmentHtml += '</div>'
        }
    }
    document.getElementById("adjustment").innerHTML = adjustmentHtml;
    
    // Notes
    if (!itemDetail.NotesAllowed)
    {
        document.getElementById("notesDiv").style.display="none";
    }
    else
    {
        document.getElementById("notesDiv").style.display="compact";
        var notesHtml = "";
        if (itemDetail.NotesList.length == 0)
        {
            notesHtml += '<li class="NoneRow clear"><label>None</label></li>'
        }
        else
        {
            for (i = 0;i < itemDetail.NotesList.length; i++)
            {
                notesHtml += (i == itemDetail.NotesList.length - 1) ? '<li class="NoBorder clear">' : '<li class="clear">'
                notesHtml += '<label>'+itemDetail.NotesList[i].Owner+'</label><span>'+itemDetail.NotesList[i].Description+'</span><span class="notesdate">'+itemDetail.NotesList[i].Date+'</span></li>'
            }
        }
        document.getElementById("notes").innerHTML = notesHtml;
    }
    
    // Taxes
    if (itemDetail.TaxList.length == 0)
    {
        document.getElementById("taxes").style.display="none";
    }
    else
    {
        document.getElementById("taxes").style.display="compact";
        var taxesHtml = "";
        for (i = 0; i < itemDetail.TaxList.length; i++)
        {
            taxesHtml += '<h6 class="subtitle">'+itemDetail.TaxList[i].Key+'</h6>'
            taxesHtml += '<div class="contents"><ul>'
            for(j = 0; j < itemDetail.TaxList[i].Value.length; j++)
            {
                taxesHtml += '<li class="clear"><label>Tax Type</label><span>'+itemDetail.TaxList[i].Value[j].TaxJurisdictionCode+'</span></li>'
                taxesHtml += '<li class="clear"><label>Tax Jurisdiction</label><span>'+itemDetail.TaxList[i].Value[j].TaxTypeCode+'</span></li>'
                taxesHtml += '<li class="NoBorder clear"><label>Tax Rate</label><span>'+itemDetail.TaxList[i].Value[j].TaxRate+'</span></li>'
            }
            taxesHtml += '</ul></div>'
        }
        document.getElementById("subtaxes").innerHTML = taxesHtml;
    }
    return "YES";
}

function constructDisclaimer(disclaimerTitle, disclaimerContent)
{
    // Disclaimer
    var disclaimerhtml = "";
    disclaimerhtml += '<h4 id="disclaimerbody">'+disclaimerTitle+'</h4>'
    disclaimerhtml += '<div id="disclaimerDiv">'+disclaimerContent+'</div>'    
    document.getElementById("disclaimerDiv").innerHTML = disclaimerhtml;
    return "YES";    
}