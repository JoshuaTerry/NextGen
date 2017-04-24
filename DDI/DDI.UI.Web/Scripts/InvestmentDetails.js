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

        InitLinkedAccounts();

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



    LoadGrid('.dwgridcontainer', 'dwgrid', columns, 'investmentautomatedtransactions/investment/' + currentEntity.Id, null, null, 'at-', '.autotransmodal', '.autotransmodal', 500, false, false, false);

    PopulateDropDown('.at-Info', 'paymentmethods/constituents/' + currentEntity.Id, '', ''); // payment preferences

    // need logic for selected constituent on search

}


function LoadInterestSection() {



}


function LoadIRSInformationSection() {



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

