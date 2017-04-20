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
        // not necessarily where we want to put this...?

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

        LoadInterestSection();

        LoadIRSInformationSection();

        FormatFields();

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

//interest section
function LoadInterestSection() {

    var id = currentEntity.Id;

    var columns = [
    { dataField: 'Id', width: '0px', },
    { dataField: 'Priority', caption: 'Priority', sortOrder: 'asc', sortIndex: 0, alignment: 'left' },
    {
        dataField: 'InterestPaymentMethod', caption: 'Method', calculateCellValue: function (data) {
            return [GetInterestPaymentMethod(data.InterestPaymentMethod)];
        }
    },
    { dataField: 'Constituent.Name', caption: 'Name' },
    { dataField: 'Percent', caption: 'Percent/Amount', dataType: 'number', format: 'decimal', precision: 2, alignment: 'right' },
    ];

    LoadGrid('.interestpaymentgridcontainer', 'interestpaymentgrid', columns, 'interestpayouts/investment/' + id, 'interestpayouts', null, '',
        '.interestpaymentmodal', '.interestpaymentmodal', 450, true, false, false, null);
}

function GetInterestPaymentMethod(method) {
    var methodDesc;
    switch (method) {
        case 0:
            methodDesc = "Compound";
            break;
        case 1:
            methodDesc = "ACH";
            break;
        case 2:
            methodDesc = "Check";
            break;
        case 3:
            methodDesc = "Investment Deposit";
            break;
        case 4:
            methodDesc = "Wire";
            break;
    }
    return methodDesc;
}
// end of interest section

function LoadIRSInformationSection() {

    var id = currentEntity.Id;

    var columns = [
    { dataField: 'Id', width: '0px', },
    { dataField: 'Year', caption: 'Priority', sortOrder: 'asc', sortIndex: 0, alignment: 'left' },
    { dataField: 'InterestPaid', caption: 'Interest Paid', dataType: 'number', format: 'currency', precision: 2, alignment: 'right' },
    { dataField: 'InterestWithheld', caption: 'Interest Withheld', dataType: 'number', format: 'currency', precision: 2, alignment: 'right' },
    { dataField: 'PenaltyCharged', caption: 'Penalty Charged', dataType: 'number', format: 'currency', precision: 2, alignment: 'right' },
    ];

    LoadGrid('.interestIRSgridcontainer', 'interestIRSgrid', columns, 'IRSInformations/investment/' + id, 'IRSInformations/investment/' + id, null, '',
        '', '', 0, false, false, false, null);

}


function LoadLinkedAccountsSection() {



}

