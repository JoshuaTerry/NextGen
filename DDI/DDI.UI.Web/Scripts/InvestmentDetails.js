$(document).ready(function () {

    // investment id will probably be coming from constituent (and elsewhere?)
    // set in sessionstorage and set hid investment id

    // apply system settings?

    // see roughly line 45 in constituents.js - will have to implement as part of investments tab


    CreateEditControls();

    SetupEditControls();

    LoadDepositsAndWithdrawalsSection();
    
});


function RefreshEntity() {

    DisplayInvestmentData();

}


function DisplayInvestmentData() {



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

    LoadGrid('.depositswithdrawalsgrid', 'dwgrid', columns);

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