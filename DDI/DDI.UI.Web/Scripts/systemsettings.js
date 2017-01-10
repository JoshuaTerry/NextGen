
$(document).ready(function () {

    $('.systemsettings a').click(function (e) {

        e.preventDefault();

        $('.contentcontainer').html('');

        $('.systemsettings a').removeClass('selected');

        var functionToCall = $(this).attr('class');

        $(this).addClass('selected');

        ExecuteFunction(functionToCall, window);

    });

});

function LoadGrid(grid, container, columns, route) {

    if (container.indexOf('.') != 0)
        container = '.' + container;

    var datagrid = $('<div>').addClass(grid);

    $.ajax({
        url: WEB_API_ADDRESS + route,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $(datagrid).dxDataGrid({
                dataSource: data.Data,
                columns: columns,
                paging: {
                    pageSize: 25
                },
                pager: {
                    showNavigationButtons: true,
                    showPageSizeSelector: true,
                    showInfo: true,
                    allowedPageSizes: [15, 25, 50, 100]
                },
                groupPanel: {
                    visible: true,
                    allowColumnDragging: true
                },
                filterRow: {
                    visible: true,
                    showOperationChooser: false
                }
            });

            $(datagrid).appendTo($(container));

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error loading Grid.');
        }
    });

}

/* CASH PROCESSING */
function LoadBankAccounts() {



}

function LoadBatchGroups() {



}

function LoadGeneralSettings() {



}

function LoadReceiptItems() {



}
/* END CASH PROCESSING */


/* COMMON SETTINGS */
function LoadAlternateIDTypes() {



}

function LoadBusinessDate() {



}

function LoadCalendarDates() {



}

function LoadDocumentTypes() {



}

function LoadCommonHomeSreen() {



}

function LoadMergeFormSystem() {



}

function LoadNotesSettings() {



}

function LoadStatusCodes() {



}

function LoadTransactionCodes() {



}
/* END COMMON SETTINGS */


/* CRM SETTINGS */
function LoadAlternateID() {

    var columns = [
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'AlternateId', caption: 'Alternate ID' }
    ];

    LoadGrid('alternateidgrid', 'contentcontainer', columns, 'alternateid');

}

function LoadClergy() {

    var accordion = $('<div>').addClass('accordions');
    var status = $('<div>').addClass('clergystatuscontainer');
    var types = $('<div>').addClass('clergytypecontainer');

    var statuscolumns = [
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    $('<h1>').text('Clergy Status').appendTo($(accordion));
    LoadGrid('clergystatusgrid', 'clergystatuscontainer', statuscolumns, 'clergystatuses');
    $(status).appendTo($(accordion));

    var typecolumns = [
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    $('<h1>').text('Clergy Type').appendTo($(accordion));
    LoadGrid('clergytypegrid', 'clergytypecontainer', typecolumns, 'clergytypes');
    $(types).appendTo($(accordion));

    $(accordion).appendTo($('.contentcontainer'));

    LoadAccordions();

}

function LoadConstituentTypes() {



}

function LoadContactInformation() {



}

function LoadDemographics() {



}

function LoadDBA() {



}

function LoadEducation() {

    var accordion = $('<div>').addClass('accordions');
    var degrees = $('<div>').addClass('degreecontainer');
    var levels = $('<div>').addClass('educationlevelscontainer');
    var schools = $('<div>').addClass('schoolscontainer');

    $('<h1>').text('Degrees').appendTo($(accordion));
    LoadGrid('degreegrid', 'degreecontainer', null, 'degrees');
    $(degrees).appendTo($(accordion));

    $('<h1>').text('Education Level').appendTo($(accordion));
    LoadGrid('educationlevelsgrid', 'educationlevelscontainer', null, 'educationlevels');
    $(levels).appendTo($(accordion));

    $('<h1>').text('Schools').appendTo($(accordion));
    LoadGrid('schoolsgrid', 'schoolscontainer', null, 'schools');
    $(schools).appendTo($(accordion));

    $(accordion).appendTo($('.contentcontainer'));

    LoadAccordions();
}

function LoadGender() {

    var columns = [
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Gender' }
    ];

    LoadGrid('gendergridcontainer', 'contentcontainer', columns, 'genders');
}

function LoadHubSearch() {



}

function LoadOrganization() {



}

function LoadPaymentPreferences() {



}

function LoadPersonal() {



}

function LoadPrefix() {



}

function LoadProfessional() {



}

function LoadRegions() {



}

function LoadRelationship() {



}

function LoadTags() {



}
/* END CRM SETTINGS */


/* DONATIONS SETTINGS */
function LoadDonationSettings() {



}

function LoadDonorSettings() {



}

function LoadGLAccountAutoAssign() {



}

function LoadDonationHomeScreen() {



}
/* END DONATIONS SETTINGS */


/* GENERAL LEDGER SETTINGS */
function LoadAccountingSettings() {



}

function LoadBudgetSettings() {



}

function LoadChartAccountsSettings() {



}

function LoadEntities() {



}

function LoadFiscalYearSettings() {



}

function LoadFundAccountingSettings() {



}

function LoadGLFormatSettings() {



}

function LoadJournalSettings() {



}

function LoadUtilities() {



}
/* END GENERAL LEDGER SETTINGS */


/* REPORTS SETTINGS */
function LoadPageFooters() {



}

function LoadPageHeaders() {



}

function LoadReportFooters() {



}

function LoadReportHeaders() {



}
/* END REPORTS SETTINGS */


/* CUSTOM FIELDS */
function LoadCRMClientCustomFields() {

    DisplayCustomFieldsGrid('gridcontainer', customfieldentity.CRM); // CRM = 19

    CreateNewCustomFieldModalLink(customfieldentity.CRM, 'New CRM Custom Field');

}

function LoadDonationClientCustomFields() {

    DisplayCustomFieldsGrid('gridcontainer', customfieldentity.Gifts); // Gifts = 9

    CreateNewCustomFieldModalLink(customfieldentity.Gifts, 'New Donations Custom Field');

}

function LoadGLClientCustomFields() {

    DisplayCustomFieldsGrid('gridcontainer', customfieldentity.GeneralLedger); // GeneralLedger = 1

    CreateNewCustomFieldModalLink(customfieldentity.GeneralLedger, 'New General Ledger Custom Field');
    
}

function CreateNewCustomFieldModalLink(entity, title) {

    var modallink = $('<a>').attr('href', '#').text('New Custom Field').appendTo($('.contentcontainer'));
    $('.gridcontainer').before($(modallink));

    $(modallink).click(function (e) {

        e.preventDefault();

        CreateNewCustomFieldModal(entity, title);

    });

}

function CreateNewCustomFieldModal(entity, title) {

    modal = $('.newcustomfieldmodal').dialog({
        closeOnEscape: false,
        modal: true,
        resizable: false
    });

    var type = $(modal).find('.cftype');
    var save = $(modal).find('.submitcf');

    $('<option>').text('').val('').appendTo($(type));
    $.each(customfieldtype, function (key, value) {

        $('<option>').text(key).val(value).appendTo($(type));

    });

    $(type).change(function () {

        CustomFieldTypeSelected($(this).val());

    });

    $(save).click(function () {

        SaveCustomField();

    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal();

    });

}

function CustomFieldTypeSelected(selectedvalue) {

    if (selectedvalue &&
        (selectedvalue == customfieldtype.Number ||
        selectedvalue == customfieldtype.Date ||
        selectedvalue == customfieldtype.DateTime)) {

        $('.minmaxvalues').show()

    }
    else {
        $('.minmaxvalues').hide()
    }

}

function SaveCustomField() {

    var id = $('.cfid').val();

    if (id) {
        // Update

    }
    else {
        // Insert

    }

}
/* END CUSTOM FIELDS */






