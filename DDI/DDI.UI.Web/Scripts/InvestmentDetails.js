$(document).ready(function () {

    CreateEditControls();

    SetupEditControls();

    LoadDropDowns();

    LoadInterestPaymentGrid();
    
});


function RefreshEntity() {

    DisplayInvestmentData();

}


function DisplayInvestmentData() {


}
function LoadDropDowns() {

    PopulateDropDown('interest-PaymentPreference', 'paymentmethods', '', '');

}

function LoadInterestPaymentGrid() {
    var columns = [
    { dataField: 'Id', width: '0px', },
    { dataField: 'Priority', caption: 'Priority'},
    { dataField: 'Method', caption: 'Method'},
    { dataField: 'Name', caption: 'Name' },
    { dataField: 'Percent', caption: 'Percent' },
    { dataField: 'Amount', caption: 'Amount' }
];

    //LoadGrid('.interestpayment', 'interestpaymentgrid', columns, 'investmentpayments/' + currentEntity.Id + '/interestpaymentmodal', 'interestpaymentmodal'
    //    , null, 'int-', '.dbamodal', '.dbamodal', 250, false, true, false, null);
}


