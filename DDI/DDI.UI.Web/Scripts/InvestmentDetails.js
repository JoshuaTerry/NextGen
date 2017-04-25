$(document).ready(function () {

    
    // apply system settings?

    CreateEditControls();

    SetupEditControls();

    if (sessionStorage.getItem('investmentid')) {

        $('.hidinvestmentid').val(sessionStorage.getItem('investmentid'));

        GetInvestmentData($('.hidinvestmentid').val());

    }

    DisplayInvestmentData();

    function LoadDropDowns() {

        PopulateDropDown('.ConstituentPaymentPreference', 'paymentmethods', '', '');
    }


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

        InitInterestPayouts();

        LoadIRSInformationSection();

        InitLinkedAccounts();

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

// interest payout section
function InitInterestPayouts() {

    LoadInterestPayoutsGrid();
    NewInterestPayoutsModal();
    NewPaymentPreferencesModal('newinterestpayoutpaymentpreferences')
}

function LoadInterestPayoutsGrid() {

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
    { dataField: 'Percent', caption: 'Percent', dataType: 'number', format: 'decimal', precision: 2, alignment: 'right' },
    { dataField: 'Amount', caption: 'Amount', dataType: 'number', format: 'decimal', precision: 2, alignment: 'right' },
    ];

    CustomLoadGrid('interestpayoutsgrid', '.interestpayoutsgridcontainer', columns, 'investmentinterestpayouts/investment/' + id, null, EditInterestPayouts);

}

function NewInterestPayoutsModal() {

    
    $('.newinterestpayoutsmodallink').click(function (e) {

        e.preventDefault();

        modal = $('.interestpayoutsmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 500,
            resizable: false
        });

        $('.cancelmodal').click(function (e) {

            e.preventDefault();

            CloseModal(modal);

        });

        $('.saveinterestpayouts').unbind('click');

        $('.saveinterestpayouts').click(function () {

            var item = GetInterestPayoutsToSave();

            MakeServiceCall('POST', 'interestpayouts', item, function (data) {

                DisplaySuccessMessage('Success', 'Interest Payout saved successfully.');

                CloseModal(modal);

                LoadInterestPayoutsGrid();

            }, function (xhr, status, err) {

                DisplayErrorMessage('Error', 'An error occurred during saving the Linked Account.');

            });

        });

    });
}

function EditInterestPayouts(id) {

    var modal = $('.interestpayoutsmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 500,
        resizable: false
    });

    LoadInterestPayouts(id);


    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.saveInterestPayouts').unbind('click');

    $('.saveInterestPayouts').click(function () {

        var topicsavelist = GetNoteTopicsToSave();

        var item = GetInterestPayoutsToSave();

        MakeServiceCall('PATCH', 'InterestPayouts/' + id, item, function (data) {

            DisplaySuccessMessage('Success', 'Interest Payout saved successfully.');

            CloseModal(modal);

            LoadInterestPayoutsGrid();

        }, function (xhr, status, err) {

            DisplayErrorMessage('Error', 'An error occurred during saving the Interest Payout.');
        });

    });
}

function LoadInterestPayouts(id) {

    MakeServiceCall('GET', 'investmentinterestpayouts/' + id, null, function (data) {

        $('.Priority').val(data.Data.Priority),
        $('.InterestPaymentMethod').val(data.Data.InterestPaymentMethod),
        $('.ConstituentId').val(data.Data.Constituent.Id),
        $('.ConstituentName').val(data.Data.Constituent.DisplayName),
        $('.InterestPayoutOptionPercent').prop("checked", (data.Data.Percent === 0) ? false : true),
        $('.InterestPayoutOptionAmount').prop("checked", (data.Data.Amount === 0) ? false : true),
        $('.InterestPayoutPercentAmount').val($(data.Data.Percent === 0) ? data.Data.Amount : data.Data.Percent)


    }, function (xhr, status, err) {
        DisplayErrorMessage('Error', 'An error occurred during loading the Interest Payout.');
    });

}


function GetInterestPayoutsToSave() {

    var rawitem = {

        Priority: $(modal).find('.Priority').val(),
        InterestPaymentMethod: $(modal).find('.InterestPaymentMethod').val(),
        ConstituentId: $(modal).find('.Constituent.Id').val(),
        ConstituentName: $(modal).find('.Constituent.Name').val(),
        Percent: ($('.InterestPayoutOptionPercent').prop("checked") === true ? $(modal).find('.InterestPayoutPercentAmount').val() : 0),
        Amount: ($('.InterestPayoutOptionAmount').prop("checked") === true ? $(modal).find('.InterestPayoutPercentAmount').val() : 0),

    };

    var item = JSON.stringify(rawitem);

    return item;

}

function GetInterestPaymentMethod(method) {
    var methodDesc;
    switch (method) {
        case 0:
            methodDesc = "Compound";
            break;
        case 1:
            methodDesc = "EFT";
            break;
        case 2:
            methodDesc = "Check";
            break;
        case 3:
            methodDesc = "Investment Deposit";
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
    { dataField: 'LinkedAccountNumber', caption: 'Link Number', alignment: 'left' },
    { dataField: 'DisplayName', caption: 'Name' },
    ];

    CustomLoadGrid('linkedaccountsgrid', '.linkedaccountsgridcontainer', columns, 'linkedaccounts/investment/' + currentEntity.Id, null, EditLinkedAccounts);

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

        $('.LinkedAccountType').val(data.Data.LinkedAccountType),
        $('.LinkedAccountInd').val((data.Data.LinkedAccountNumber > 0) ? 1 : 0),
        $('.LinkedAccountNumber').val(data.Data.LinkedAccountNumber),
        $('.CollateralTypePercent').prop("checked", (data.Data.CollateralType === 0) ? true : false),
        $('.CollateralTypeAmount').prop("checked", (data.Data.CollateralType === 1) ? true : false),
        $('.CollateralType').val(data.Data.CollateralType),
        $('.Collateral').val(data.Data.Collateral),
        $('.BlockOtherLoanLinks').val(data.Data.BlockOtherLoanLinks)


    }, function (xhr, status, err) {
        DisplayErrorMessage('Error', 'An error occurred during loading the Linked Account.');
    });

}

function GetLinkedAccountsToSave() {

    var rawitem = {

        LinkedAccountType: $(modal).find('.LinkedAccountType').val(),
        //LinkedAccountInd: ($.isNumeric($(modal).find('.LinkedAccountNumber').val()) ? 1 : 0),    // when the item is available
        LinkedAccountNumber: $(modal).find('.LinkedAccountNumber').val(),
        CollateralType: ($('.CollateralTypePercent').prop("checked" === true) ? 0 : 1),
        Collateral: $(modal).find('.Collateral').val(),
        BlockOtherLoanLinks: $(modal).find('.BlockOtherLoanLinks').val()

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

