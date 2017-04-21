$(document).ready(function () {

    
    // apply system settings?

    CreateEditControls();

    SetupEditControls();

    if (sessionStorage.getItem('investmentid')) {

        $('.hidinvestmentid').val(sessionStorage.getItem('investmentid'));

        GetInvestmentData($('.hidinvestmentid').val());

    }

    DisplayInvestmentData();

    // RefreshEntity(); ?
    
});


function GetInvestmentData(investid) {

    MakeServiceCall('GET', 'investments/' + investid, null, function (data) {

        currentEntity = data.Data;

        DisplayInvestmentData();

    });

}


function RefreshEntity() {

    DisplayInvestmentData();

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

        LoadDepositsAndWithdrawalsSection();

    }
}


function LoadDepositsAndWithdrawalsSection() {

    var columns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'AutomatedTransactionMethod', caption: 'Deposit/Withdrawal/Transfer' }, // group by
        { dataField: 'NextTransactionDate', caption: 'NextDate' }, //order by
        { dataField: 'RecurringType', caption: 'Frequency' },
       // { dataField: 'AutomatedTransactionMethod', caption: 'Method' },
        { dataField: 'PaymentPreference.DisplayName', caption: 'Payment Info' }, // similar to constituent payment prefs
        { dataField: 'Amount', caption: 'Amount' },
        { dataField: 'IsActive', caption: 'Status' }
    ];



    LoadGrid('.dwgridcontainer', 'dwgrid', columns, 'automatedtransactions/investment/' + currentEntity.Id, null, null, 'at-', '.autotransmodal', '.autotransmodal', 500, false, false, false);

    PopulateDropDown('.at-Info', 'paymentmethods/constituents/' + currentEntity.Constituent.Id, '', ''); // payment preferences

}


function LoadInterestSection() {



}


function LoadIRSInformationSection() {



}


function LoadLinkedAccountsSection() {



}

