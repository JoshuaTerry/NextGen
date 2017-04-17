$(document).ready(function () {

    // apply system settings?

    SetupEditControls();

    LoadDropDowns();

    LoadInterestPaymentGrid();
    if (sessionStorage.getItem('investmentid')) {

        $('.hidinvestmentid').val(sessionStorage.getItem('investmentid'));

    }

    //GetInvestmentData(id);

    DisplayInvestmentData();
    // RefreshEntity(); ?
    
});


function RefreshEntity() {

    DisplayInvestmentData();

}

function GetInvestmentData(id) {

    MakeServiceCall('GET', '' + id, null, function () {

        DisplayInvestmentData();

    }); 

}


function DisplayInvestmentData() {


    CreateEditControls();

    SetupEditControls();

    LoadDepositsAndWithdrawalsSection();

}


function LoadDepositsAndWithdrawalsSection() {

    var columns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'DepositWithdrawal', caption: 'Deposit/Withdrawal' }, // group by
        { dataField: 'NextDate', caption: 'NextDate' }, //order by
        { dataField: 'Frequency', caption: 'Frequency' }, // Enum?
        { dataField: 'Method', caption: 'Degree' },
        { dataField: 'PaymentInfo', caption: 'Payment Info' }, // similar to constituent payment prefs
        { dataField: 'Amount', caption: 'Amount' },
        { dataField: 'IsActive', caption: 'Status' }
    ];

    //LoadGrid(container, gridClass, columns, getRoute, saveRoute, selected, prefix, editModalClass, newModalClass, modalWidth, showDelete, showFilter, showGroup, onComplete) 


    LoadGrid('.dwgridcontainer', 'dwgrid', columns, null, null, null, null, null, null, null, null, null, null, null);

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

    //LoadGrid('.interestpayment', 'interestgridcontainer', columns, 'investmentpayments/' + currentEntity.Id, 'interestpaymentmodal'
    //    , null, 'int-', '.dbamodal', '.dbamodal', 250, false, true, false, null);
}
function LoadAttributesSection() {



}

function LoadInterestSection() {



}

function LoadIRSInformationSection() {



}

function LoadLinkedAccountsSection() {



}

function LoadMaturitySection() {



}

function LoadPaymentPreferencesSectioon() {



}