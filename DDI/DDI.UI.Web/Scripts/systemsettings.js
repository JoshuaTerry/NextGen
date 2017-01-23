
$(document).ready(function () {

    $('.systemsettings a').click(function (e) {

        e.preventDefault();

        $('.contentcontainer').html('');

        $('.systemsettings a').removeClass('selected');

        var functionToCall = $(this).attr('class');

        $(this).addClass('selected');

        ExecuteFunction(functionToCall, window);

    });

    CreateNewCustomFieldModalLink(customfieldentity.CRM, 'New CRM Custom Field');

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
var modalLeft = 0;
var options = [];

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
        resizable: false,
        beforeClose: function (event, ui) {
            ClearModal();
        }
    });

    $('.options').hide();

    modalLeft = parseInt($('.ui-dialog').css('left').replace('px', ''));

    var type = $(modal).find('.cftype');
    var save = $(modal).find('.submitcf');

    $('<option>').text('').val('').appendTo($(type));
    $.each(customfieldtype, function (key, value) {

        $('<option>').text(key).val(value).appendTo($(type));

    });

    $('.addoption').click(function () {
        AddOption();
    });

    $(type).change(function () {

        CustomFieldTypeSelected($(this).val());

    });

    $(save).click(function () {

        SaveCustomField();

    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        ClearModal();

        $('.ui-dialog').css('width', '300px');
        $('.fieldproperties').attr('style', 'width: 100%');
        
        $(modal).dialog('close');

    });

}

function AddOption() {

    var code = $('.cfoptioncode').val();
    var desc = $('.cfoptiondesc').val();
    var order = $('.cfoptionorder').val();

    var option = {
        CustomFieldId: '',
        Code: code,
        Description: desc,
        SortOrder: order
    }

    options.push(option);

    var tr = $('<tr>');

    var tdcode = $('<td>').text(code).css('width', '28px').appendTo($(tr));
    var tddesc = $('<td>').text(desc).css('padding-left', '5px').css('width', '155px').appendTo($(tr));
    var tdorder = $('<td>').text(order).css('padding-left', '2px').css('width', '30px').appendTo($(tr));

    $(tr).appendTo($('.tempoptions'));
    
    $('.cfoptioncode').val('');
    $('.cfoptiondesc').val('');
    $('.cfoptionorder').val('');
    
}

function ClearModal() {

    $('.options').hide();
    $('.tempoptions').html('');

    $(modal).find('div.fieldblock input').not('.noclear').each(function () {
        $(this).val('');
    });

    $(modal).find('select').not('.noclear').each(function () {
        $(this).html('');
    });

}

function CustomFieldTypeSelected(selectedvalue) {

    if (selectedvalue)
    {
        if (selectedvalue == customfieldtype.Number ||
        selectedvalue == customfieldtype.Date ||
        selectedvalue == customfieldtype.DateTime) {
            $('.minmaxvalues').show()
        }
        else {
            $('.minmaxvalues').hide()
        }

        if (selectedvalue && selectedvalue == customfieldtype.Number) {
            $('.decimalplacecontainer').show();
        }
        else {
            $('.decimalplacecontainer').hide();
        }
    
        if (selectedvalue == customfieldtype.Radio ||
            selectedvalue == customfieldtype.DropDown) {

            var left = parseInt($('.ui-dialog').css('left').replace('px', ''));

            if (left >= modalLeft)
                left -= 150;

            $('.ui-dialog').stop().animate(
                {
                    width: '600px',
                    left: left
                },
                {
                    start: function () {
                        $('.fieldproperties').attr('style', '');
                    },
                    complete: function () {
                        $('.options').show();
                    }
                }
            , 500);
            
            
        }
        else {
            
            var left = parseInt($('.ui-dialog').css('left').replace('px', ''));

            if (left < modalLeft)
                left += 150;

            $('.ui-dialog').stop().animate(
                {
                    width: '300px',
                    left: left 
                },
                {
                    start: function () {
                        $('.options').hide();
                        $('.fieldproperties').attr('style', 'width: 100%');
                    },
                    complete: function () {
                        
                    }
                }
            , 500);
        }
    }
    else {

        $('.minmaxvalues').hide()
        $('.decimalplacecontainer').hide();

    }

}

function SaveCustomField() {

    var id = $('.cfid').val();

    if (id) {
        // Update
        var data = [];

        data.push('"Id": "' + id + '"');
        data.push('"LabelText": "' + $('.cflabel').val() + '"');
        data.push('"MinValue": "' + $('.cfminvalue').val() + '"');
        data.push('"MaxValue": "' + $('.cfmaxvalue').val() + '"');
        data.push('"DecimalPlaces": "' + $('.cfdecimalplaces').val() + '"');
        data.push('"IsActive": "' + true + '"');
        data.push('"IsRequired": "' + $('.cfisrequired').prop('checked') + '"');
        data.push('"DisplayOrder": "' + 1 + '"'); // $('.cforder').val(),
        data.push('"FieldType": "' + $('.cftype').val() + '"');
        data.push('"Entity": "' + customfieldentity.CRM + '"');
        
        data = "{" + data + "}";

        CustomFieldDataCall('customfields', 'PATCH', data);
    }
    else {
        // Insert
        
        var data = [];

        data.push('"LabelText": "' + $('.cflabel').val() + '"');
        data.push('"MinValue": "' + $('.cfminvalue').val() + '"');
        data.push('"MaxValue": "' + $('.cfmaxvalue').val() + '"');
        data.push('"DecimalPlaces": "' + $('.cfdecimalplaces').val() + '"');
        data.push('"IsActive": "' + true + '"');
        data.push('"IsRequired": "' + $('.cfisrequired').prop('checked') + '"');
        data.push('"DisplayOrder": "' + 1 + '"'); // $('.cforder').val(),
        data.push('"FieldType": "' + $('.cftype').val() + '"');
        data.push('"Entity": "' + customfieldentity.CRM + '"');

        data = '{' + data + '}';

        CustomFieldDataCall('customfields', 'POST', data);
    }

}

function SaveOptions(id) {

    options.forEach(function (o){

        o.CustomFieldId = id;

        CustomFieldDataCall('customfieldoptions', 'POST', o);

    })

}

function CustomFieldDataCall(route, action, data) {

    $.ajax({
        url: WEB_API_ADDRESS + route,
        method: action,
        data: data,
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            SaveOptions(data.Id);

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error saving custom field.');
        }
    });

}
/* END CUSTOM FIELDS */






