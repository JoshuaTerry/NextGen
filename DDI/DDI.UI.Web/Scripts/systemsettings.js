SettingsCategories = {
    CashProcessing: 'Cash Processing',
    Common: 'Common',
    CRM: 'CRM',
    Donations: 'Donations',
    GeneralLedger: 'General Ledger',
    Reports: 'Reports',
    CustomFields: 'Custom Fields'
}

var SystemSettings = {
    AlternateId: 'AlternateIdSettings',
    Clergy: 'ClergySettings',
    ContactInformation: 'ContactInformationSettings',
    DBA: 'DBASettings',
    Demographics: 'DemographicSettings',
    Education: 'EducationSettings',
    Organization: 'OrganizationSettings',
    Personal: 'PersonalSettings',
    Professional: 'ProfessionalSettings',
    Note: 'NoteSettings',
    PaymentPreferences: 'PaymentPreferencesSettings',
    Accounting: 'AccountingSettings',
    FundAccountingSettings: 'FundAccountingSettingsSettings'
}

$(document).ready(function () {

    $('.systemsettings a').click(function (e) {

        e.preventDefault();

        ClearNavMenu();

        $('.customfieldmodallink').remove();

        $('.gridcontainer').html('');

        $('.systemsettings a').removeClass('selected');

        var functionToCall = $(this).attr('class') + 'SectionSettings';

        $(this).addClass('selected');

        $('.systemsettingsheader').text($(this).text());

        ExecuteFunction(functionToCall, window);

    });

});

function ClearNavMenu() {
    $('.utilitymenu').find('li').each(function () {
        $(this).remove();
    })
    $('.utilitynav').hide();
}

function LoadSettingsGrid(grid, container, columns, route) {

    if (container.indexOf('.') != 0)
        container = '.' + container;

    var datagrid = $('<div>').addClass(grid);

    MakeServiceCall('GET', route, null, function (data) {

        if (data.Data) {
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
        }

    }, null);

}

function GetSystemSettings(category, callback) {

    MakeServiceCall('GET', 'sectionpreferences/' + category + '/settings', null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful && callback) {

                callback(data.Data);

            }
        }

    }, null);
}


/* SECTION SETTINGS */
function LoadSectionSettings(category, section, route, sectionKey) {

    var container = $('<div>').addClass('threecolumn');

    var activeSection = $('<div>').addClass('fieldblock');
    var checkbox = $('<input>').attr('type', 'checkbox').addClass('sectionAvailable').appendTo(activeSection);
    $('<span>').text('Activate ' + section + ' section of ' + category).appendTo(activeSection);
    $(activeSection).appendTo(container);


    var sectionLabel = $('<div>').addClass('fieldblock');
    $('<label>').text('Section Label: ').appendTo(sectionLabel);
    var label = $('<input>').attr('type', 'text').addClass('sectionLabel').appendTo(sectionLabel).attr('maxlength', '128');
    $(sectionLabel).appendTo(container);

    var id = $('<input>').attr('type', 'hidden').addClass('hidSettingId').appendTo(container);

    GetSetting(category, sectionKey, route, id, checkbox, label);

    var controlContainer = $('<div>').addClass('controlContainer');

    $('<input>').attr('type', 'button').addClass('saveEntity').val('Save')
        .click(function () {
            SaveSetting($('.hidSettingId'), route, category, sectionKey, $('.sectionLabel').val(), $('.sectionAvailable').prop('checked'));
        })
        .appendTo(controlContainer);

    $('<a>').addClass('cancel').text('Cancel').attr('href', '#')
        .click(function (e) {
            e.preventDefault();

            GetSetting(category, sectionKey, route, id, checkbox, label);
        })
        .appendTo(controlContainer);

    $(controlContainer).appendTo(container);

    $(container).appendTo($('.gridcontainer'));

}

function GetSetting(category, key, route, id, checkbox, label) {

    MakeServiceCall('GET', route + '/' + category + '/settings/' + key, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {

                currentEntity = data.Data;

                $(id).val(data.Data.Id);
                $(checkbox).prop('checked', data.Data.IsShown);
                $(label).val(data.Data.Value);

            }
        }

    }, null);

}

function SaveSetting(idContainer, route, categoryName, name, value, isShown) {

    var id = $(idContainer).val();
    var method = 'POST';
    var item = null;

    if (id) {
        method = 'PATCH';
        route = route + '/' + id;

        item = {
            Id: id,
            SectionName: categoryName,
            Name: name,
            Value: value,
            IsShown: isShown
        }
    }
    else {
        method = 'POST';

        item = {
            SectionName: categoryName,
            Name: name,
            Value: value,
            IsShown: isShown
        }
    }

    MakeServiceCall(method, route, item, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {
                DisplaySuccessMessage('Success', 'Setting saved successfully.');

                currentEntity = data.Data;

                $(idContainer).val(data.Data.Id);
            }

        }

    }, null);

}
/* END SECTION SETTINGS */

 
/* DONATIONS SETTINGS */
function LoadDonationSettingsSectionSettings() {}

function LoadDonorSettingsSectionSettings() {}

function LoadGLAccountAutoAssignSectionSettings() { }

function LoadDonationHomeScreenSectionSettings() {}
/* END DONATIONS SETTINGS */
 
/* REPORTS SETTINGS */
function LoadPageFootersSectionSettings() {}

function LoadPageHeadersSectionSettings() {}

function LoadReportFootersSectionSettings() {}

function LoadReportHeadersSectionSettings() {}
/* END REPORTS SETTINGS */




