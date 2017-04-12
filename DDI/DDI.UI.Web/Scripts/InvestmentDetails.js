$(document).ready(function () {

    CreateEditControls();

    SetupEditControls();

    LoadDropDowns();

    LoadInterestPaymentGrid();

    LoadLinkedAccountsGrid();
    
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

    //LoadGrid('.interestpayment', 'interestpaymentgrid', columns, 'investmentpayments/' + currentEntity.Id + '/interestpayment', 'interestpaymentmodal'
    //    , null, 'intpmt-', '.dbamodal', '.dbamodal', 250, false, true, false, null);
}


function LoadLinkedAccountsGrid() {
    var columns = [
    { dataField: 'Id', width: '0px', },
    { dataField: 'Type', caption: 'Type' },
    { dataField: 'LinkNumber', caption: 'Link Number' },
    { dataField: 'Name', caption: 'Name' },
    ];

    //LoadGrid('.linkedaccountsgridcontainer', 'linkedaccountsgrid', columns, 'investmentlinkedaccounts/' + currentEntity.Id + '/????', 'linkedaccountsmodal'
    //    , null, 'intlink-', '.dbamodal', '.dbamodal', 250, false, true, false, null);
}

