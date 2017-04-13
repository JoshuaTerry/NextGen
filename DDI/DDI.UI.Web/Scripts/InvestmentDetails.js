$(document).ready(function () {

    CreateEditControls();

    SetupEditControls();

    LoadDropDowns();

    LoadInterestPaymentGrid();

    InitLinkedAccounts();

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
    { dataField: 'Priority', caption: 'Priority' },
    { dataField: 'Method', caption: 'Method' },
    { dataField: 'Name', caption: 'Name' },
    { dataField: 'Percent', caption: 'Percent' },
    { dataField: 'Amount', caption: 'Amount' }
    ];

    //LoadGrid('.interestpayment', 'interestpaymentgrid', columns, 'investmentpayments/' + currentEntity.Id + '/interestpayment', 'interestpaymentmodal'
    //    , null, 'intpmt-', '.dbamodal', '.dbamodal', 250, false, true, false, null);
}

//linked accounts section

function InitLinkedAccounts() {

    LoadLinkedAccountsGrid();
    NewLinkedAccountsModal();
}

function LoadLinkedAccountsGrid() {
    var columns = [
    { dataField: 'Id', width: '0px', },
    { dataField: 'Type', caption: 'Type' },
    { dataField: 'LinkNumber', caption: 'Link Number' },
    { dataField: 'Name', caption: 'Name' },
    ];

    //CustomLoadGrid('linkedaccountsgrid', '.linkedaccountsgridcontainer', columns, currentEntity.Id + '/linkedaccounts/', null, EditLinkedAccounts, null, null);

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

        $('.la-Type').val(data.Data.Type),
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

        Type: $(modal).find('.al-Type').val(),
        LoadInd: $(modal).find('.al-LoadInd').val(),
        LoanNumber: $(modal).find('.al-LoanNumber').val(),
        CollateralInd: $(modal).find('.al-CollateralInd').val(),
        CollateralAmtPct: $(modal).find('.al-CollateralAmtPct').val(),
        BlockLink: $(modal).find('.al-BlockLink').val()

    };

    var item = JSON.stringify(rawitem);

    return item;

}

//end of linked accounts section

