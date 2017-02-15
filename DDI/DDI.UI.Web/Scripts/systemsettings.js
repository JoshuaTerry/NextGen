
$(document).ready(function () {

    $('.systemsettings a').click(function (e) {

        e.preventDefault();

        $('.contentcontainer').html('');

        $('.systemsettings a').removeClass('selected');

        var functionToCall = $(this).attr('class');

        $(this).addClass('selected');

        ExecuteFunction(functionToCall, window);

    });

    // CreateNewCustomFieldModalLink(CustomFieldEntity.CRM, 'New CRM Custom Field');

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

    DisplayCustomFieldsGrid('contentcontainer', CustomFieldEntity.CRM); // CRM = 19

    CreateNewCustomFieldModalLink(CustomFieldEntity.CRM, 'New CRM Custom Field');

}

function LoadDonationClientCustomFields() {

    DisplayCustomFieldsGrid('contentcontainer', CustomFieldEntity.Gifts); // Gifts = 9

    CreateNewCustomFieldModalLink(CustomFieldEntity.Gifts, 'New Donations Custom Field');

}

function LoadGLClientCustomFields() {

    DisplayCustomFieldsGrid('contentcontainer', CustomFieldEntity.GeneralLedger); // GeneralLedger = 1

    CreateNewCustomFieldModalLink(CustomFieldEntity.GeneralLedger, 'New General Ledger Custom Field');
    
}

function RefreshCustomFieldsGrid() {

    $('.contentcontainer').html('');

    DisplayCustomFieldsGrid('contentcontainer', currentCustomFieldEntity);

}

function CreateNewCustomFieldModalLink(entity, title) {

    var modallink = $('<a>').attr('href', '#').addClass('customfieldmodallink').text('New Custom Field').appendTo($('.contentcontainer'));
    $('.gridcontainer').before($(modallink));

    $(modallink).unbind('click');

    $(modallink).click(function (e) {

        e.preventDefault();

        CreateNewCustomFieldModal(entity, title);

    });

}

function CreateNewCustomFieldModal(entity, title) {

    var modal = $('.newcustomfieldmodal').dialog({
        closeOnEscape: false,
        modal: true,
        resizable: false,
        beforeClose: function (event, ui) {
            ClearModal(modal);
        }
    });

    $('.options').hide();

    modalLeft = parseInt($('.ui-dialog').css('left').replace('px', ''));

    var type = $(modal).find('.cftype');
    var save = $(modal).find('.submitcf');

    $('<option>').text('').val('').appendTo($(type));
    $.each(CustomFieldType, function (key, value) {

        $('<option>').text(key).val(value).appendTo($(type));

    });

    $('.addoption').click(function () {
        AddOption();
    });

    $(type).change(function () {

        CustomFieldTypeSelected($(this).val());

    });

    $(save).unbind('click');

    $(save).click(function () {

        SaveCustomField(modal);

    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        ClearModal(modal);

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
        Code: code,
        Description: desc,
        SortOrder: order
    };

    options.push(option);

    var tr = $('<tr>');

    var tdcode = $('<td>').text(code).css('width', '28px').appendTo($(tr));
    var tddesc = $('<td>').text(desc).css('padding-left', '5px').css('width', '155px').appendTo($(tr));
    var tdorder = $('<td>').text(order).css('padding-left', '2px').css('width', '30px').appendTo($(tr));

    $(tr).appendTo($('.tempoptions'));
    
    $('.cfoptioncode').val('').focus();
    $('.cfoptiondesc').val('');
    $('.cfoptionorder').val('');
}

function ClearModal(modal) {

    $('.options').hide();
    options = [];
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
        if (selectedvalue == CustomFieldType.Number ||
        selectedvalue == CustomFieldType.Date ||
        selectedvalue == CustomFieldType.DateTime) {
            $('.minmaxvalues').show()
        }
        else {
            $('.minmaxvalues').hide()
        }

        if (selectedvalue && selectedvalue == CustomFieldType.Number) {
            $('.decimalplacecontainer').show();
        }
        else {
            $('.decimalplacecontainer').hide();
        }
    
        if (selectedvalue == CustomFieldType.Radio ||
            selectedvalue == CustomFieldType.DropDown) {

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

function SaveCustomField(modal) {

    var id = $('.cfid').val();
    var method = '';

    if (id) {
        // Update
        var data = {
            Id: id,
            LabelText: $('.cflabel').val(),
            MinValue: $('.cfminvalue').val(),
            MaxValue: $('.cfmaxvalue').val(),
            DecimalPlaces: $('.cfdecimalplaces').val(),
            IsActive: true,
            IsRequired: $('.cfisrequired').prop('checked'),
            DisplayOrder: 1,
            FieldType: $('.cftype').val(),
            Entity: currentCustomFieldEntity,
            Options: []
        }

        method = 'PATCH';
    }
    else {
        // Insert
        
        var data = {
            LabelText: $('.cflabel').val(),
            MinValue: $('.cfminvalue').val(),
            MaxValue: $('.cfmaxvalue').val(),
            DecimalPlaces: $('.cfdecimalplaces').val(),
            IsActive: true,
            IsRequired: $('.cfisrequired').prop('checked'),
            DisplayOrder: 1,
            FieldType: $('.cftype').val(),
            Entity: currentCustomFieldEntity,
            Options: []
        }

        method = 'POST';
    }

    if (options && options.length > 0) {

        data.Options = options;

    }

    SendCustomField('customfields', method, data, modal);

}

function SendCustomField(route, action, data, modal) {

    $.ajax({
        url: WEB_API_ADDRESS + route,
        method: action,
        data: JSON.stringify(data),
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            DisplaySuccessMessage('Success', 'Custom field saved successfully.');

            CloseModal(modal);

            RefreshCustomFieldsGrid();

            CreateNewCustomFieldModalLink(currentCustomFieldEntity, '');
            
        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error saving custom field.');
        }
    });

}
/* END CUSTOM FIELDS */






