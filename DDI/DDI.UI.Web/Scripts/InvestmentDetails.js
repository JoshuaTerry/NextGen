$(document).ready(function () {

    
    // apply system settings?

    TestCurrentEntity();

    if (sessionStorage.getItem('investmentid')) {

        $('.hidinvestmentid').val(sessionStorage.getItem('investmentid'));

    }

    //GetInvestmentData(id);

    DisplayInvestmentData();
    // RefreshEntity(); ?
    
});

function TestCurrentEntity() {
    // testing purposes only, until we have an investmentid set up
    investid = 'FFD6D1A4-D7FE-41A0-9ECA-0439EFB7B7AA';
    // not actually an investment id, 

    MakeServiceCall('GET', 'investments/' + investid, null, function (data) {

        currentEntity = data.Data;

        DisplayInvestmentData();
        // not necessarily where we want to put this...

    });

}


function RefreshEntity() {

    DisplayInvestmentData();

}

function GetInvestmentData(id) {

    MakeServiceCall('GET', '' + id, null, function () {

        DisplayInvestmentData();

    }); 

}

function DisplayInvestmentData() {

    if (currentEntity) {

        var id = currentEntity.Id;
        sessionStorage.setItem('constituentid', id);
        $('.hidconstituentid').val(id);

        $.map(currentEntity, function (value, key) {

            if (typeof (value) == 'string')
                value = value.replace('"', '').replace('"', '');



            if (key != '$id') {

                var display = '';

                if (typeof (value) == 'object' && !!value) {
                    display = value.DisplayName;
                } else {
                    display = value;
                }

                var classname = '.' + key;

                if ($(classname).is('input')) {
                    if ($(classname).is(':checkbox')) {
                        $(classname).prop('checked', display);
                    }
                    else {
                        $(classname).val(display);
                    }
                }

                if ($(classname).is('select')) {
                    $(classname).val(display);
                }

                if (key.toLowerCase().indexOf('date') !== -1) {

                    var date = FormatJSONDate(display);

                    $(classname).text(date);
                }

            }
        });

        CreateEditControls();

        SetupEditControls();

        LoadDepositsAndWithdrawalsSection();

    }
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