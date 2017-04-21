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

        //InitLinkedAccounts();

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

    LoadGrid('.interestpaymentgridcontainer', 'interestpaymentgrid', columns, 'investmentinterestpayouts/investment/' + id, 'investmentinterestpayouts', null, '',
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

    LoadGrid('.interestIRSgridcontainer', 'interestIRSgrid', columns, 'investmentirsinformations/investment/' + id, 'investmentirsinformations/investment/' + id, null, '',
        '', '', 0, false, false, false, null);

}


//linked accounts section

function InitLinkedAccounts() {

    LoadLinkedAccountsGrid();
    NewLinkedAccountsModal();
}

function LoadLinkedAccountsGrid() {
    var columns = [
    { dataField: 'Id', width: '0px', },
    {
        dataField: 'LinkedAccountType', caption: 'Type', sortOrder: 'asc', sortIndex: 0, caption: '', calculateCellValue: function (data) {
            return [GetLinkedType(data.LinkedAccountType)];
        }
    },
    { dataField: 'LinkedAccountNumber', caption: 'Link Number', alignment: 'left'},
    { dataField: 'DisplayName', caption: 'Name' },
    ];

    CustomLoadGrid('linkedaccountsgrid', '.linkedaccountsgridcontainer', columns, 'linkedaccounts/investment/' + currentEntity.Id, null, EditLinkedAccounts, null, null);

}

function NewLinkedAccountsModal() {

    $('.newlinkedaccountsmodallink').click(function (e) {

        e.preventDefault();

        modal = $('.linkedaccountsmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 500,
            resizable: false
        });

        $('.cancelmodal').click(function (e) {

            e.preventDefault();

            CloseModal(modal);

        });

        $('.savelinkedaccounts').unbind('click');

        $('.savelinkedaccounts').click(function () {

            var item = GetLinkedAccountsToSave();

            MakeServiceCall('POST', 'linkedaccounts', item, function (data) {

                DisplaySuccessMessage('Success', 'Linked Account saved successfully.');

                CloseModal(modal);

                LoadLinkedAccountsGrid();

            }, function (xhr, status, err) {

                DisplayErrorMessage('Error', 'An error occurred during saving the Linked Account.');

            });

        });

    });
}

function EditLinkedAccounts(id) {

    var modal = $('.linkedaccountsmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 500,
        resizable: false
    });

    LoadLinkedAccounts(id);


    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.savelinkedaccounts').unbind('click');

    $('.savelinkedaccounts').click(function () {

        var topicsavelist = GetNoteTopicsToSave();

        var item = GetLinkedAccountsToSave();

        MakeServiceCall('PATCH', 'linkedaccounts/' + id, item, function (data) {

            DisplaySuccessMessage('Success', 'Linked Account saved successfully.');

            CloseModal(modal);

            LoadlinkedAccountsGrid();

        }, function (xhr, status, err) {

            DisplayErrorMessage('Error', 'An error occurred during saving the Linked Account.');
        });

    });
}

function LoadLinkedAccounts(id) {

    MakeServiceCall('GET', 'linkedaccounts/' + id, null, function (data) {

        $('.linkedAccountType').val(data.Data.LinkedAccountType),
        $('.la-LoadInd').val(data.Data.LoanInd),
        $('.la-LoanNumber').val(data.Data.LoanNumber),
        $('.la-CollateralInd').val(data.Data.CollateralInd),
        $('.la-CollateralAmtPct').val(data.Data.CollateralIndAmtPct),
        $('.la-BlockLink').val(data.Data.BlockLink)


    }, function (xhr, status, err) {
        DisplayErrorMessage('Error', 'An error occurred during loading the Linked Account.');
    });

}

function GetLinkedAccountsToSave() {

    var rawitem = {

        Type: $(modal).find('.linkedAccounts-Type').val(),
        LoadInd: $(modal).find('.al-LoadInd').val(),
        LoanNumber: $(modal).find('.al-LoanNumber').val(),
        CollateralInd: $(modal).find('.al-CollateralInd').val(),
        CollateralAmtPct: $(modal).find('.al-CollateralAmtPct').val(),
        BlockLink: $(modal).find('.al-BlockLink').val()

    };

    var item = JSON.stringify(rawitem);

    return item;

}

function GetLinkedType(type) {
    var typeDesc;
    switch (type) {
        case 0:
            typeDesc = "Loan Support";
            break;
        case 1:
            typeDesc = "Pool";
            break;
        case 2:
            typeDesc = "Down Payment";
            break;
        case 3:
            typeDesc = "Grant";
            break;
    }
    return typeDesc;
}
//end linked accounts section

