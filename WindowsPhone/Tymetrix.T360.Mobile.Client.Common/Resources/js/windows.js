function notifyInvoiceSelection(invno) {
	var res = 'selectedInvoiceId=' + invno;
    window.external.notify(res);
}

function notifyLineItemSelection(lineitemNo) {
	var res = 'selectedLineItemId=' + lineitemNo;
    window.external.notify(res);
}

function notifyCompleted() {
	window.external.notify("Completed");
}

function constructInvoiceListWithCallBack(str)
{
	disableSelection();
	constructInvoiceList(str);
	notifyCompleted();
}

function constructSummaryWithCallBack(str)
{
	disableSelection();
	constructSummary(str);
	notifyCompleted();
}

function constructLineItemListWithCallBack(str)
{
	disableSelection();
	constructLineItemList(str);
	notifyCompleted();
}

function constructLineItemDetailsWithCallBack(str)
{
	disableSelection();
	constructLineItemDetails(str);
	notifyCompleted();
}

function constructDisclaimerWithCallBack(title, data)
{
	disableSelection();
	constructDisclaimer(title, data);
	notifyCompleted();
}

function constructSupport(str)
{
	disableSelection();
	document.getElementById("username").innerHTML = str;
	notifyCompleted();
}

function CallTo()
{
	window.external.notify("tel:8602409059");
}

function EmailTo()
{
	window.external.notify("mailto:T360Support@wolterskluwer.com");
}

function disableSelection()
{
	document.body.onselectstart=function(){ return false; };
	document.body.ondragstart=function() { return false; };
}
