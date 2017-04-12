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

function LoadLinkedAccountsGrid() {
    var columns = [
    { dataField: 'Id', width: '0px', },
    { dataField: 'Type', caption: 'Type' },
    { dataField: 'LinkNumber', caption: 'Link Number' },
    { dataField: 'Name', caption: 'Name' },
    ];

    CustomLoadGrid('linkedaccountsgrid', '.linkedaccountsgridcontainer', columns, currentEntity.Id + '/linkedaccounts/', null, EditLinkedAccounts, null, null);

}

function NewLinkedAccountsModal() {

    $('.newnotesdetailmodallink').click(function (e) {

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

        $('.nd-Title').val(data.Data.Title),
        $('.nd-Description').val(data.Data.Text),
        $('.nd-AlertStartDate').val(data.Data.AlertStartDate),
        $('.nd-AlertEndDate').val(data.Data.AlertEndDate),
        $('.nd-ContactDate').val(data.Data.ContactDate),
        $('.nd-ContactDate').val(data.Data.NoteCode),
        $('.nd-CreatedBy').text(data.Data.CreatedBy),
        $('.nd-UpdatedBy').text(data.Data.LastModifiedBy),
        $('.nd-CreatedOn').text(data.Data.CreatedOn),
        $('.nd-UpdatedOn').text(data.Data.LastModifiedOn)


    }, function (xhr, status, err) {
        DisplayErrorMessage('Error', 'An error occurred during loading the Note Details.');
    });

}

function GetLinkedAccountsToSave() {

    var rawitem = {

        Title: $(modal).find('.nd-Title').val(),
        AlertStartDate: $(modal).find('.nd-AlertStartDate').val(),
        AlertEndDate: $(modal).find('.nd-AlertEndDate').val(),
        Text: $(modal).find('.nd-Description').val(),
        CategoryId: $(modal).find('.nd-Category').val(),
        ContactDate: $(modal).find('.nd-ContactDate').val(),
        NoteCodeId: $(modal).find('.nd-NoteCode').val(),
        ParentEntityId: currentEntity.Id,
        EntityType: NoteEntity[0],
        ContactMethodId: $(modal).find('.nd-ContactMethod').val(),

    };

    var item = JSON.stringify(rawitem);

    return item;

}

//end of linked accounts section

