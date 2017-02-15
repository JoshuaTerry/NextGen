var SystemSettingsLinks = [];
SystemSettingsLinks["NewTagGroup"] = { Href: WEB_API_ADDRESS + "taggroups", Method: "Get" };

$(document).ready(function () {

    $('.systemsettings a').click(function (e) {

        e.preventDefault();

        $('.contentcontainer').html('');

        $('.systemsettings a').removeClass('selected');

        var functionToCall = $(this).attr('class');

        $(this).addClass('selected');

        ExecuteFunction(functionToCall, window);

    });

    // CreateNewCustomFieldModalLink(customfieldentity.CRM, 'New CRM Custom Field');

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

function LoadTagGroupGrid() {
    var selectOptions = [
        { Id: 0, Description: "Single" },
        { Id: 1, Description: "Multiple" }
    ];
    var columns = [
        { dataField: 'Id', width: "0px" },
        { dataField: 'Order', caption: 'Order' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'TagSelectionType', caption: 'Multi/Single Select', lookup: { dataSource: selectOptions, valueExpr: 'Id', displayExpr: 'Description' } },
        { dataField: 'IsActive', caption: 'Active' },
    ];
    LoadGridFromHateoas("tagsgrid",
        "contentcontainer",
        columns,
        WEB_API_ADDRESS + "taggroups?OrderBy=Order",
        null,
        "TagGroup",
        EditTagGroup,
        DeleteEntity,
        "Delete tag group: ",
        null);
    CreateNewModalLink("New Tag Group", NewTagGroupModal, '.tagsgrid');

}
/* END CRM SETTINGS */

function EditTagGroup(getUrl, patchUrl) {
    EditEntity(getUrl, patchUrl, "Tag Group", ".taggroupmodal", ".savetaggroup", 250, LoadTagGroup, LoadTagGroupGrid, GetTagGroupToSave);
}

function LoadTagGroup(url, modal) {
    LoadEntity(url, modal, "GET", LoadTagGroupData, "Tag Group");
}

function LoadTagGroupData(data, modal) {
    $(modal).find('.hidtaggroupid').val(data.Data.Id);
    $(modal).find('.tg-Order').val(data.Data.Order);
    $(modal).find('.tg-Name').val(data.Data.Name);
    $(modal).find('.tg-Select').val(data.Data.TagSelectionType);
    $(modal).find('.tg-IsActive').prop('checked', data.Data.IsActive);
}

function GetTagGroupToSave(modal, isUpdate) {
    var item = {
        Order: $(modal).find('.tg-Order').val(),
        Name: $(modal).find('.tg-Name').val(),
        IsActive: $(modal).find('.tg-IsActive').prop('checked'),
        TagSelectionType: $(modal).find('.tg-Select').val(),
    }
    if (isUpdate === true) {
        item.Id = $(modal).find('.hidtaggroupid').val();
    }
    return item;
}

function NewTagGroupModal() {
    NewEntityModal("Tag Group", ".newmodallink", ".taggroupmodal", 250, null, ".savetaggroup", GetTagGroupToSave, SystemSettingsLinks.NewTagGroup.Method, SystemSettingsLinks.NewTagGroup.Href, LoadTagGroupGrid);
}

function CreateNewModalLink(linkText, newEntityModalMethod, prependToClass) {

    var modallink = $('<a>').attr('href', '#').addClass('newmodallink').text(linkText).appendTo($('.contentcontainer'));
    $(prependToClass).before($(modallink));
//
//    $(modallink).unbind('click');
//
//    $(modallink).click(function (e) {
//
//        e.preventDefault();
//
        newEntityModalMethod();
//
//    });

}


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

    DisplayCustomFieldsGrid('contentcontainer', customfieldentity.CRM); // CRM = 19

    CreateNewCustomFieldModalLink(customfieldentity.CRM, 'New CRM Custom Field');

}

function LoadDonationClientCustomFields() {

    DisplayCustomFieldsGrid('contentcontainer', customfieldentity.Gifts); // Gifts = 9

    CreateNewCustomFieldModalLink(customfieldentity.Gifts, 'New Donations Custom Field');

}

function LoadGLClientCustomFields() {

    DisplayCustomFieldsGrid('contentcontainer', customfieldentity.GeneralLedger); // GeneralLedger = 1

    CreateNewCustomFieldModalLink(customfieldentity.GeneralLedger, 'New General Ledger Custom Field');
    
}

function RefreshCustomFieldsGrid() {

    $('.contentcontainer').html('');

    DisplayCustomFieldsGrid('contentcontainer', currentcustomfieldentity);

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
    $.each(customfieldtype, function (key, value) {

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
            Entity: currentcustomfieldentity,
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
            Entity: currentcustomfieldentity,
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

            CreateNewCustomFieldModalLink(currentcustomfieldentity, '');
            
        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error saving custom field.');
        }
    });

}
/* END CUSTOM FIELDS */






