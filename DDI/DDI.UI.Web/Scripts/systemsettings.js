﻿SettingsCategories = {
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
    DBA: 'DBASettings',
    Demographics: 'DemographicSettings',
    Education: 'EducationSettings',
    Organization: 'OrganizationSettings',
    Personal: 'PersonalSettings',
    Professional: 'ProfessionalSettings',
    Note: 'NoteSettings',
    PaymentPreferences: 'PaymentPreferencesSettings'

}

$(document).ready(function () {

    $('.systemsettings a').click(function (e) {

        e.preventDefault();

        $('.contentcontainer').html('');

        $('.systemsettings a').removeClass('selected');

        var functionToCall = $(this).attr('class') + 'SectionSettings';

        $(this).addClass('selected');

        ExecuteFunction(functionToCall, window);

    });

    // CreateNewCustomFieldModalLink(CustomFieldEntity.CRM, 'New CRM Custom Field');

});

function LoadSettingsGrid(grid, container, columns, route) {

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
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}

function GetSystemSettings(category, callback) {

    $.ajax({
        url: WEB_API_ADDRESS + 'sectionpreferences/' + category + '/settings',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            if (data.IsSuccessful && callback) {

                callback(data.Data);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}


/* SECTION SETTINGS */
function LoadSectionSettings(category, section, route, sectionKey) {

    route = route + '?fields=all';
    var container = $('<div>').addClass('twocolumn');

    var activeSection = $('<div>').addClass('fieldblock');
    var checkbox = $('<input>').attr('type', 'checkbox').addClass('sectionAvailable').appendTo(activeSection);
    $('<span>').text('Activate ' + section + ' of ' + category).appendTo(activeSection);
    $(activeSection).appendTo(container);

    var sectionLabel = $('<div>').addClass('fieldblock');
    $('<label>').text('Section Label: ').appendTo(sectionLabel);
    var label = $('<input>').attr('type', 'text').addClass('sectionLabel').appendTo(sectionLabel);
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

    $(container).appendTo($('.contentcontainer'));

}

function GetSetting(category, key, route, id, checkbox, label) {

    $.ajax({
        url: WEB_API_ADDRESS + route + '/' + category + '/settings/' + key,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            if (data.IsSuccessful) {

                $(id).val(data.Data.Id);
                $(checkbox).prop('checked', data.Data.IsShown);
                $(label).val(data.Data.Value);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

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

    $.ajax({
        url: WEB_API_ADDRESS + route,
        method: method,
        data: JSON.stringify(item),
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            if (data.IsSuccessful) {
                DisplaySuccessMessage('Success', 'Setting saved successfully.');

                $(idContainer).val(data.Data.Id);
            }


        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}
/* END SECTION SETTINGS */


/* CASH PROCESSING */
function LoadBankAccountsSectionSettings() {



}

function LoadBatchGroupsSectionSettings() {



}

function LoadGeneralSettingsSectionSettings() {



}

function LoadReceiptItemsSectionSettings() {



}
/* END CASH PROCESSING */


/* COMMON SETTINGS */
function LoadAlternateIDTypesSectionSettings() {



}

function LoadBusinessDateSectionSettings() {



}

function LoadCalendarDatesSectionSettings() {



}

function LoadDocumentTypesSectionSettings() {



}

function LoadCommonHomeSreenSectionSettings() {



}

function LoadMergeFormSystemSectionSettings() {



}

/* NOTE SYSTEM SETTINGS */

function LoadNoteSectionSettings() {

    var accordion = $('<div>').addClass('accordions');
    var noteCodes = $('<div>').addClass('noteCodecontainer');
    var noteCategories = $('<div>').addClass('noteCategorycontainer');
    var noteTopics = $('<div>').addClass('noteTopiccontainer');

    var header = $('<h1>').text('Note Code').appendTo($(accordion));
    $(noteCodes).appendTo($(accordion));
    
    var noteCodecolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];
    LoadGrid('.noteCodecontainer', 'noteCodegrid', noteCodecolumns, 'notecodes', null, 'noteCode-',
        '.noteCodemodal', '.noteCodemodal', 250, true, false, false, null);


    header = $('<h1>').text('Note Category').appendTo($(accordion));
    $(noteCategories).appendTo($(accordion));

    var noteCategorycolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Label', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];
    LoadGrid('.noteCategorycontainer', 'noteCategorygrid', noteCategorycolumns, 'notecategories', null, 'noteCategory-',
        '.noteCategorymodal', '.noteCategorymodal', 250, true, false, false, null);
    

    header = $('<h1>').text('Topic').appendTo($(accordion));
    $(noteTopics).appendTo($(accordion));

    var noteTopiccolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];
    LoadGrid('.noteTopiccontainer', 'noteTopicgrid', noteTopiccolumns, 'notetopics', null, 'noteTopic-', 
        '.noteTopicmodal', '.noteTopicmodal', 250, true, false, false, null);

    $(accordion).appendTo($('.contentcontainer'));

    LoadAccordions();

}




function LoadStatusCodesSectionSettings() {



}

function LoadTransactionCodesSectionSettings() {



}
/* END COMMON SETTINGS */


/* CRM SETTINGS */
function LoadAlternateIDSectionSettings() {

    LoadSectionSettings(SettingsCategories.CRM, 'Alternate ID', 'sectionpreferences', SystemSettings.AlternateId);

}

function LoadClergySectionSettings() {

    LoadSectionSettings(SettingsCategories.CRM, 'Clergy', 'sectionpreferences', SystemSettings.Clergy);

    var accordion = $('<div>').addClass('accordions');
    var status = $('<div>').addClass('clergystatuscontainer');
    var types = $('<div>').addClass('clergytypecontainer');

    var header = $('<h1>').text('Clergy Status').appendTo($(accordion));
    $(status).appendTo($(accordion));

    var statuscolumns = [
              { dataField: 'Id', width: '0px' },
              { dataField: 'Code', caption: 'Code' },
              { dataField: 'Name', caption: 'Description' },
              { dataField: 'IsActive', caption: 'Active' }
    ];
    LoadGrid('.clergystatuscontainer', 'clergystatusgrid', statuscolumns, 'clergystatuses', null, 'cstat-',
        '.clergystatusmodal', '.clergystatusmodal', 250, true, false, false, null);

    header = $('<h1>').text('Clergy Type').appendTo($(accordion));
    $(types).appendTo($(accordion));

    var typecolumns = [
    { dataField: 'Id', width: '0px' },
    { dataField: 'Code', caption: 'Code' },
    { dataField: 'Name', caption: 'Description' },
    { dataField: 'IsActive', caption: 'Active' }
    ];
    LoadGrid('.clergytypecontainer', 'clergytypegrid', typecolumns, 'clergytypes', null, 'ctype-',
        '.clergytypemodal', '.clergytypemodal', 250, true, false, false, null);

    $(accordion).appendTo($('.contentcontainer'));

    LoadAccordions();

}


function LoadConstituentTypesSectionSettings() {

    var accordion = $('<div>').addClass('accordions');
    var types = $('<div>').addClass('constituenttypescontainer');

    
    var header = $('<h1>').text('Constituent Types').appendTo($(accordion));
    $('<a>').attr('href', '#').addClass('newconstituenttypemodallink modallink newbutton')
        .click(function (e) {
            e.preventDefault();

            modal = $('.constituenttypemodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 400,
                resizable: false
            });

            $('.consttype-tagselect').hide();
            $('.tagSelectImage').hide();
            $('.tagSelect').hide();

            $('.cancelmodal').click(function (e) {

                e.preventDefault();

                CloseModal(modal);

            });

            $('.submitconsttype').unbind('click');

            $('.submitconsttype').click(function () {

                var item = {
                    Code: $(modal).find('.consttype-Code').val(),
                    Name: $(modal).find('.consttype-Name').val(),
                    Category: $(modal).find('.consttype-Category').val(),
                    NameFormat: $(modal).find('.consttype-NameFormat').val(),
                    SalutationFormal: $(modal).find('.consttype-SalutationFormal').val(),
                    SalutationInformal: $(modal).find('.consttype-SalutationInformal').val(),
                    IsActive: true,
                    IsRequired: true
                }

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'constituenttypes',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    success: function () {

                        DisplaySuccessMessage('Success', 'Constituent Type saved successfully.');

                        CloseModal(modal);

                        LoadConstituentTypeSettingsGrid();

                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', 'An error occurred while saving the Constituent Type.');
                    }
                });

            });
        })
        .appendTo($(header));

    $(types).appendTo($(accordion));

    LoadConstituentTypeSettingsGrid();

    $(accordion).appendTo($('.contentcontainer'));

    LoadAccordions();

}

function DisplayConstituentTypeTags(tags) {

    $(modal).find('.tagselect').empty();
    $(modal).find('.consttype-tagselect').empty();

    $(tags).each(function (i, tag) {

        var id = tag.Id;
        var name = tag.Name;

        var t = $('<div>').addClass('dx-tag-content').attr('id', id).appendTo($('.tagselect'));
        $('<span>').text(name).appendTo($(t));
        $('<div>').addClass('dx-tag-remove-button')
            .click(function () {
                $.ajax({
                    url: WEB_API_ADDRESS + 'constituenttypes/' + $(modal).find('.consttype-Id').val() + '/tag/' + tag.Id,
                    method: 'DELETE',
                    headers: GetApiHeaders(),
                    contentType: 'application/json; charset-utf-8',
                    crossDomain: true,
                    success: function () {

                        DisplaySuccessMessage('Success', 'Tag was deleted successfully.');
                        CloseModal(modal);
                        EditConstituentType($(modal).find('.consttype-Id').val());

                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', 'An error occurred deleting the tag.');
                        CloseModal(modal);
                        EditConstituentType($(modal).find('.consttype-Id').val());
                    }
                });
            })
            .appendTo($(t));

    });
    

}

function LoadConstituentTypeTagSelector(typeId, container) {

    $('.tagselect').each(function () {

        var img = $('.tagSelectImage');
        var constTypeId = typeId;
        
        if (img.length === 0) {
            img = $('<div>').addClass('tagSelectImage');
        }

            $(img).click(function () {

                modal = $('.tagselectmodal').dialog({
                    closeOnEscape: false,
                    modal: true,
                    width: 450,
                    resizable: false
                });

                LoadAvailableTags(modal);

                $('.saveselectedtags').unbind('click');

                $('.saveselectedtags').click(function () {

                    var tagIds = [];

                    $('.tagselectgridcontainer').find('input').each(function (index, value) {

                        if ($(value).prop('checked')) {
                            tagIds.push($(value).val());
                        }
                    });

                    $.ajax({
                        url: WEB_API_ADDRESS + 'constituenttypes/' + constTypeId + '/constituenttypetags',
                        method: 'POST',
                        headers: GetApiHeaders(),
                        data: JSON.stringify({ tags: tagIds }),
                        contentType: 'application/json; charset-utf-8',
                        dataType: 'json',
                        crossDomain: true,
                        success: function (data) {

                            // Display success
                            DisplaySuccessMessage('Success', 'Tags saved successfully.');
                            CloseModal(modal);
                            EditConstituentType(constTypeId);
                        },
                        error: function (xhr, status, err) {

                            DisplayErrorMessage('Error', 'An error occurred saving the tags.');
                            CloseModal(modal);
                            EditConstituentType(constTypeId);
                        }
                    });
                });
            });
            $(this).after($(img));
    });
}

function LoadConstituentTypeSettingsGrid() {

    var typecolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        {
            caption: 'Category', cellTemplate: function (container, options) {

                var category;

                switch (options.data.Category) {
                    case 0:
                        category = "Individual";
                        break;
                    case 1:
                        category = "Organization";
                        break;
                    case 2:
                        category = "Both";
                        break;
                }

                $('<label>').text(category).appendTo(container);
            }
        },
        {
            caption: 'Tags', cellTemplate: function (container, options) {

                var tags;

                if (options.data.Tags.length > 0) {
                    var label = $('<label>');

                    $.map(options.data.Tags, function (tag) {
                        $(label).text($(label).text() + tag.Code + ', ');
                    });

                    var codes = $(label).text();
                    $(label).text(codes.substr(0, codes.length - 2));
                    $(label).appendTo($(container));
                }
            }
        },
        { dataField: 'NameFormat', caption: 'Name Format' },
        { dataField: 'SalutationFormal', caption: 'Formal Salutation' },
        { dataField: 'SalutationInformal', caption: 'Informal Salutation' }
    ];

    
    LoadGrid('constituenttypesgrid', 'constituenttypescontainer', typecolumns, 'constituenttypes', null, EditConstituentType, DeleteConstituentType);

}


// CONSTITUENT TYPE SYSTEM SETTINGS 
function EditConstituentType(id) {

    LoadConstituentType(id);


    modal = $('.constituenttypemodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 400,
        height: 500,
        resizable: false
    });

    $('.consttype-tagselect').show();

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

        LoadConstituentTypeSettingsGrid()

    });

    $('.submitconsttype').unbind('click');

    $('.submitconsttype').click(function () {

        var item = {
            Code: $(modal).find('.consttype-Code').val(),
            Name: $(modal).find('.consttype-Name').val(),
            Category: $(modal).find('.consttype-Category').val(),
            NameFormat: $(modal).find('.consttype-NameFormat').val(),
            SalutationFormal: $(modal).find('.consttype-SalutationFormal').val(),
            SalutationInformal: $(modal).find('.consttype-SalutationInformal').val()
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'constituenttypes/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Constituent type saved successfully.');

                CloseModal(modal);

                LoadConstituentTypeSettingsGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred while saving the Constituent Type.');
            }
        });

    });

}

function DeleteConstituentType(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'constituenttypes/' + id,
        method: 'DELETE',
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function () {

            DisplaySuccessMessage('Success', 'Constituent Type deleted successfully.');

            LoadConstituentTypeSettingsGrid();

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred deleting the Constituent Type.');
        }

    });

}

function LoadConstituentType(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'constituenttypes/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            if (data && data.Data && data.IsSuccessful) {

                $(modal).find('.consttype-Id').val(data.Data.Id);
                $(modal).find('.consttype-Code').val(data.Data.Code);
                $(modal).find('.consttype-Name').val(data.Data.Name);
                $(modal).find('.consttype-Category').val(data.Data.Category);
                $(modal).find('.consttype-NameFormat').val(data.Data.NameFormat);
                $(modal).find('.consttype-SalutationFormal').val(data.Data.SalutationFormal);
                $(modal).find('.consttype-SalutationInformal').val(data.Data.SalutationInformal);

                LoadConstituentTypeTagSelector(data.Data.Id, $('.consttype-tagselect'));

                if (data.Data.Tags.length > 0) {
                    DisplayConstituentTypeTags($(data.Data.Tags));
                }
                else {
                    $(modal).find('.consttype-tagselect').empty();
                }
            }
        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error loading constituent type.');
        }
    });

}
/* END CONSTITUENT TYPE SYSTEM SETTINGS */



function LoadContactInformationSectionSettings() {



}



/* DEMOGRAPHICS SYSTEM SETTINGS */
function LoadDemographicsSectionSettings() {

    LoadSectionSettings(SettingsCategories.CRM, 'Demographics', 'sectionpreferences', SystemSettings.Demographics);

    var accordion = $('<div>').addClass('accordions');
    var denomination = $('<div>').addClass('denominationscontainer');
    var ethnicity = $('<div>').addClass('ethnicitiescontainer');
    var language = $('<div>').addClass('languagescontainer');


    var header = $('<h1>').text('Denominations').appendTo($(accordion));
    $('<a>').attr('href', '#').addClass('newdenominationmodallink modallink newbutton')
        .click(function (e) {
            e.preventDefault();

            modal = $('.denominationmodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 250,
                resizable: false
            });

            $('.cancelmodal').click(function (e) {

                e.preventDefault();

                CloseModal(modal);

            });

            $('.submitden').unbind('click');

            $('.submitden').click(function () {

                var item = {
                    Code: $(modal).find('.den-Code').val(),
                    Name: $(modal).find('.den-Name').val(),
                    Religion: $(modal).find('.den-Religion').val(),
                    Affiliation: $(modal).find('.den-Affiliation').val(),
                    IsActive: $(modal).find('.den-IsActive').prop('checked')
                }

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'denominations',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    success: function () {

                        DisplaySuccessMessage('Success', 'Denomination saved successfully.');

                        CloseModal(modal);

                        LoadDenominationSettingsGrid();

                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
                    }
                });

            });
        })
        .appendTo($(header));
    $(denomination).appendTo($(accordion));

    LoadDenominationSettingsGrid();

    header = $('<h1>').text('Ethnicities').appendTo($(accordion));
    $('<a>').attr('href', '#').addClass('newEthnicitiesmodallink modallink newbutton')
        .click(function (e) {
            e.preventDefault();

            modal = $('.ethnicitymodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 250,
                resizable: false
            });

            $('.cancelmodal').click(function (e) {

                e.preventDefault();

                CloseModal(modal);

            });

            $('.submiteth').unbind('click');

            $('.submiteth').click(function () {

                var item = {
                    Code: $(modal).find('.eth-Code').val(),
                    Name: $(modal).find('.eth-Name').val(),
                    IsActive: $(modal).find('.eth-IsActive').prop('checked')
                }

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'ethnicities',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    success: function () {

                        DisplaySuccessMessage('Success', 'Ethnicity saved successfully.');

                        CloseModal(modal);

                        LoadEthnicitySettingsGrid();

                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
                    }
                });

            });
        })
        .appendTo($(header));
    $(ethnicity).appendTo($(accordion));

    LoadEthnicitySettingsGrid();

    header = $('<h1>').text('Languages').appendTo($(accordion));
    $('<a>').attr('href', '#').addClass('newLanguagesmodallink modallink newbutton')
        .click(function (e) {
            e.preventDefault();

            modal = $('.languagemodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 250,
                resizable: false
            });

            $('.cancelmodal').click(function (e) {

                e.preventDefault();

                CloseModal(modal);

            });

            $('.submitlang').unbind('click');

            $('.submitlang').click(function () {

                var item = {
                    Code: $(modal).find('.lang-Code').val(),
                    Name: $(modal).find('.lang-Name').val(),
                    IsActive: $(modal).find('.lang-IsActive').prop('checked')
                }

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'languages',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    success: function () {

                        DisplaySuccessMessage('Success', 'Language saved successfully.');

                        CloseModal(modal);

                        LoadLanguageSettingsGrid();

                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
                    }
                });

            });
        })
        .appendTo($(header));
    $(language).appendTo($(accordion));

    LoadLanguageSettingsGrid();

    $(accordion).appendTo($('.contentcontainer'));

    LoadAccordions();

}

function LoadDenominationSettingsGrid() {
        
    var denominationcolumns = [
       { dataField: 'Id', width: '0px' },
       { dataField: 'Code', caption: 'Code' },
       { dataField: 'Name', caption: 'Denomination' },
       {
           caption: 'Religion', cellTemplate: function (container, options) {
               var religion = 'None';

               switch (options.data.Religion) {
                   case 1:
                       religion = "Catholic";
                       break;
                   case 2:
                       religion = "Protestant";
                       break;
                   case 3:
                       religion = "Orthodox";
                       break;
                   case 4:
                       religion = "Jewish";
                       break;
                   case 5:
                       religion = "Islam";
                       break;
                   case 6:
                       religion = "Hindu";
                       break;
                   case 7:
                       religion = "Buddhist";
                       break;
                   case 8:
                       religion = "Taoist";
                       break;
                   case 9:
                       religion = "Shinto";
                       break;
                   case 10:
                       religion = "Sikh";
                       break;
                   case 11:
                       religion = "Bahai";
                       break;
               }

               $('<label>').text(religion).appendTo(container);
           }
       },
       {
           caption: 'Affiliation', cellTemplate: function (container, options) {
               var affiliation = 'None';

               switch (options.data.Affiliation) {
                   case 1:
                       affiliation = "Affiliated";
                       break;
                   case 2:
                       affiliation = "Unaffiliated";
                       break;
               }

               $('<label>').text(affiliation).appendTo(container);
           }
       },
       { dataField: 'IsActive', caption: 'Active' }
    ];
   
    LoadGrid('denominationsgrid', 'denominationscontainer', denominationcolumns, 'denominations', null, EditDenomination, DeleteDenomination);

}

function LoadEthnicitySettingsGrid() {

    var ethnicitycolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Ethnicity' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('ethnicitiesgrid', 'ethnicitiescontainer', ethnicitycolumns, 'ethnicities', null, EditEthnicity, DeleteEthnicity);
}


function LoadLanguageSettingsGrid() {

    var languagecolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Language' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('languagesgrid', 'languagescontainer', languagecolumns, 'languages', null, EditLanguage, DeleteLanguage);

}

/* DENOMINATION SYSTEM SETTINGS */

function EditDenomination(id) {

    LoadDenomination(id);

    modal = $('.denominationmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitden').unbind('click');

    $('.submitden').click(function () {

        var item = {
            Code: $(modal).find('.den-Code').val(),
            Name: $(modal).find('.den-Name').val(),
            Religion: $(modal).find('.den-Religion').val(),
            Affiliation: $(modal).find('.den-Affiliation').val(),
            IsActive: $(modal).find('.den-IsActive').prop('checked')
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'denominations/' + $(modal).find('.den-Id').val(),
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Denomination saved successfully.');

                CloseModal(modal);

                LoadDenominationSettingsGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
            }
        });

    });

}

function DeleteDenomination(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'denominations/' + id,
        method: 'DELETE',
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function () {

            DisplaySuccessMessage('Success', 'Denomination deleted successfully.');

            LoadDenominationSettingsGrid();

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred deleting the Denomination.');
        }

    });


}

function LoadDenomination(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'denominations/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            if (data && data.Data && data.IsSuccessful) {

                $(modal).find('.den-Id').val(data.Data.Id);
                $(modal).find('.den-Code').val(data.Data.Code);
                $(modal).find('.den-Name').val(data.Data.Name);
                $(modal).find('.den-Religion').val(data.Data.Religion);
                $(modal).find('.den-Affiliation').val(data.Data.Affiliation);
                $(modal).find('.den-IsActive').prop('checked', data.Data.IsActive);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}
/* END DENOMINATION SYSTEM SETTINGS */

/* ETHNICITY SYSTEM SETTINGS */
function EditEthnicity(id) {

    LoadEthnicity(id);

    modal = $('.ethnicitymodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submiteth').unbind('click');

    $('.submiteth').click(function () {

        var item = {
            Code: $(modal).find('.eth-Code').val(),
            Name: $(modal).find('.eth-Name').val(),
            IsActive: $(modal).find('.eth-IsActive').prop('checked')
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'ethnicities/' + $(modal).find('.eth-Id').val(),
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Ethnicity saved successfully.');

                CloseModal(modal);

                LoadEthnicitySettingsGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
            }
        });

    });

}

function DeleteEthnicity(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'ethnicities/' + id,
        method: 'DELETE',
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function () {

            DisplaySuccessMessage('Success', 'Ethnicity deleted successfully.');

            LoadEthnicitySettingsGrid();

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred deleting the Ethnicity.');
        }

    });


}

function LoadEthnicity(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'ethnicities/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            if (data && data.Data && data.IsSuccessful) {

                $(modal).find('.eth-Id').val(data.Data.Id);
                $(modal).find('.eth-Code').val(data.Data.Code);
                $(modal).find('.eth-Name').val(data.Data.Name);
                $(modal).find('.eth-IsActive').prop('checked', data.Data.IsActive);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}
/* END ETHNICITY SYSTEM SETTINGS */

/* LANGUAGE SYSTEM SETTINGS */
function EditLanguage(id) {

    LoadLanguage(id);

    modal = $('.languagemodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitlang').unbind('click');

    $('.submitlang').click(function () {

        var item = {
            Code: $(modal).find('.lang-Code').val(),
            Name: $(modal).find('.lang-Name').val(),
            IsActive: $(modal).find('.lang-IsActive').prop('checked')
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'languages/' + $(modal).find('.lang-Id').val(),
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Language saved successfully.');

                CloseModal(modal);

                LoadLanguageSettingsGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
            }
        });

    });

}

function DeleteLanguage(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'languages/' + id,
        method: 'DELETE',
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function () {

            DisplaySuccessMessage('Success', 'Language deleted successfully.');

            LoadLanguageSettingsGrid();

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred deleting the Language.');
        }

    });


}

function LoadLanguage(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'languages/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            if (data && data.Data && data.IsSuccessful) {

                $(modal).find('.lang-Id').val(data.Data.Id);
                $(modal).find('.lang-Code').val(data.Data.Code);
                $(modal).find('.lang-Name').val(data.Data.Name);
                $(modal).find('.lang-IsActive').prop('checked', data.Data.IsActive);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}
/* END LANGUAGE SYSTEM SETTINGS */

/* END DEMOGRAPHICS SYSTEM SETTINGS */

function LoadDBASectionSettings() {

    LoadSectionSettings(SettingsCategories.CRM, 'Doing Business As', 'sectionpreferences', SystemSettings.DBA);

}

/* EDUCATION SYSTEM SETTINGS */
function LoadEducationSectionSettings() {
    LoadSectionSettings(SettingsCategories.CRM, 'Education', 'sectionpreferences', SystemSettings.Education);

    var accordion = $('<div>').addClass('accordions');
    var degrees = $('<div>').addClass('degreecontainer');
    var educationlevels = $('<div>').addClass('educationlevelscontainer');
    var schools = $('<div>').addClass('schoolscontainer');

    var header = $('<h1>').text('Degrees').appendTo($(accordion));
    $('<a>').attr('href', '#').addClass('newdegreemodallink modallink newbutton')
        .click(function (e) {
            e.preventDefault();

            modal = $('.degreemodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 250,
                resizable: false
            });

            $('.cancelmodal').click(function (e) {
                e.preventDefault();
                CloseModal(modal);
            });

            $('.submitdeg').unbind('click');

            $('.submitdeg').click(function () {

                var item = {
                    Code: $(modal).find('.deg-Code').val(),
                    Name: $(modal).find('.deg-Name').val(),
                    IsActive: $(modal).find('.deg-IsActive').prop('checked')
                }

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'degrees',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    success: function () {

                        DisplaySuccessMessage('success', 'Degree saved successfully.');

                        CloseModal(modal);

                        LoadDegreeSettingsGrid();
                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
                    }
                });
            });

        })
        .appendTo($(header));
    $(degrees).appendTo($(accordion));

    LoadDegreeSettingsGrid();
        
    header = $('<h1>').text('Education Level').appendTo($(accordion));
    $('<a>').attr('href', '#').addClass('neweducationlevelsmodallink modallink newbutton')
        .click(function (e) {
            e.preventDefault();

            modal = $('.educationLevelmodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 250,
                resizable: false
            });

            $('.cancelmodal').click(function (e) {
                e.preventDefault();
                CloseModal(modal);
            });

            $('.submiteduLev').unbind('click');

            $('.submiteduLev').click(function () {

                var item = {
                    Code: $(modal).find('.eduLev-Code').val(),
                    Name: $(modal).find('.eduLev-Name').val(),
                    IsActive: $(modal).find('.eduLev-IsActive').prop('checked')
                }

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'educationlevels',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    success: function () {

                        DisplaySuccessMessage('success', 'Education Level saved successfully.');

                        CloseModal(modal);

                        LoadEducationLevelSettingsGrid();
                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
                    }
                });
            });

        })
        .appendTo($(header));
    $(educationlevels).appendTo($(accordion));

    LoadEducationLevelSettingsGrid();

    header = $('<h1>').text('Schools').appendTo($(accordion));
    $('<a>').attr('href', '#').addClass('newschoolgridmodallink modallink newbutton')
        .click(function (e) {
            e.preventDefault();

            modal = $('.schoolmodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 250,
                resizable: false
            });

            $('.cancelmodal').click(function (e) {
                e.preventDefault();
                CloseModal(modal);
            });

            $('.submitsch').unbind('click');

            $('.submitsch').click(function () {

                var item = {
                    Code: $(modal).find('.sch-Code').val(),
                    Name: $(modal).find('.sch-Name').val(),
                    IsActive: $(modal).find('.sch-IsActive').prop('checked')
                }

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'schools',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    success: function () {

                        DisplaySuccessMessage('success', 'School saved successfully.');

                        CloseModal(modal);

                        LoadSchoolsSettingsGrid();
                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
                    }
                });
            });

        })
        .appendTo($(header));
    $(schools).appendTo($(accordion));

    LoadSchoolsSettingsGrid();

    $(accordion).appendTo($('.contentcontainer'));

    LoadAccordions();
}

function LoadDegreeSettingsGrid() {

    var degreecolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('degreesgrid', 'degreecontainer', degreecolumns, 'degrees', null, EditDegree);

}

function LoadEducationLevelSettingsGrid() {

    var educationLevelcolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('educationlevelsgrid', 'educationlevelscontainer', educationLevelcolumns, 'educationlevels', null, EditEducationLevel);


}

function LoadSchoolsSettingsGrid() {

    var schoolcolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('schoolsgrid', 'schoolscontainer', schoolcolumns, 'schools?fields=all', null, EditSchool);


}

function EditDegree(id) {

    LoadDegree(id);

    modal = $('.degreemodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitdeg').unbind('click');

    $('.submitdeg').click(function () {

        var item = {
            Code: $(modal).find('.deg-Code').val(),
            Name: $(modal).find('.deg-Name').val(),
            IsActive: $(modal).find('.deg-IsActive').prop('checked')
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'degrees/' + $(modal).find('.degreeId').val(),
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Degree saved successfully.');

                CloseModal(modal);

                LoadDegreeSettingsGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
            }
        });

    });

}

function LoadDegree(id) {
    $.ajax({
        url: WEB_API_ADDRESS + 'degrees/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {
            if (data && data.Data && data.IsSuccessful) {
                $(modal).find('.degreeId').val(data.Data.Id);
                $(modal).find('.deg-Code').val(data.Data.Code);
                $(modal).find('.deg-Name').val(data.Data.Name);
                $(modal).find('.deg-IsActive').prop('checked', data.Data.IsActive);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}

function EditEducationLevel(id) {
    LoadEducationLevel(id);

    modal = $('.educationLevelmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submiteduLev').unbind('click');

    $('.submiteduLev').click(function () {

        var item = {
            Code: $(modal).find('.eduLev-Code').val(),
            Name: $(modal).find('.eduLev-Name').val(),
            IsActive: $(modal).find('.eduLev-IsActive').prop('checked')
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'educationlevels/' + $(modal).find('.educationLevelId').val(),
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Education Level saved successfully.');

                CloseModal(modal);

                LoadEducationLevelSettingsGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
            }
        });

    });

}

function LoadEducationLevel(id) {
    $.ajax({
        url: WEB_API_ADDRESS + 'educationlevels/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {
            if (data && data.Data && data.IsSuccessful) {
                $(modal).find('.educationLevelId').val(data.Data.Id);
                $(modal).find('.eduLev-Code').val(data.Data.Code);
                $(modal).find('.eduLev-Name').val(data.Data.Name);
                $(modal).find('.eduLev-IsActive').prop('checked', data.Data.IsActive);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}

function EditSchool(id) {
    LoadSchool(id);

    modal = $('.schoolmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitsch').unbind('click');

    $('.submitsch').click(function () {

        var item = {
            Code: $(modal).find('.sch-Code').val(),
            Name: $(modal).find('.sch-Name').val(),
            IsActive: $(modal).find('.sch-IsActive').prop('checked')
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'schools/' + $(modal).find('.schoolId').val(),
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'School saved successfully.');

                CloseModal(modal);

                LoadSchoolsSettingsGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
            }
        });

    });

}

function LoadSchool(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'schools/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            if (data && data.Data && data.IsSuccessful) {

                $(modal).find('.schoolId').val(data.Data.Id);
                $(modal).find('.sch-Code').val(data.Data.Code);
                $(modal).find('.sch-Name').val(data.Data.Name);
                $(modal).find('.sch-IsActive').prop('checked', data.Data.IsActive);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}

/* END EDUCATION SYSTEM SETTINGS */



function LoadGenderSectionSettings() {

    var columns = [
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Gender' }
    ];

    LoadGrid('gendergridcontainer', 'contentcontainer', columns, 'genders');
}

function LoadHubSearchSectionSettings() {



}

function LoadOrganizationSectionSettings() {

    LoadSectionSettings(SettingsCategories.CRM, 'Organization', 'sectionpreferences', SystemSettings.Organization);

}

function LoadPaymentPreferencesSectionSettings() {

    LoadSectionSettings(SettingsCategories.CRM, 'Payment Preferences', 'sectionpreferences', SystemSettings.PaymentPreferences);

}

function LoadPersonalSectionSettings() {

    LoadSectionSettings(SettingsCategories.CRM, 'Personal', 'sectionpreferences', SystemSettings.Personal);

}

/* PREFIX SYSTEM SETTINGS */
function LoadPrefixSectionSettings() {

    var prefixcolumns = [
       { dataField: 'Id', width: '0px' },
       { dataField: 'Code', caption: 'Code' },
       { dataField: 'Name', caption: 'Description' },
       { dataField: 'Salutation', caption: 'Salutation Prefix' },
       { dataField: 'LabelPrefix', caption: 'Label Prefix' },
       { dataField: 'LabelAbbreviation', caption: 'Label Prefix Short' }
    ];

    // LoadGrid(container, gridClass, columns, route, selected, prefix, editModalClass, newModalClass, modalWidth, 
    // showDelete, showFilter, showGroup, onComplete)
    LoadGrid('.contentcontainer', 'prefixgrid', prefixcolumns, 'prefixes', null, 'prefix-', '.prefixmodal', '.prefixmodal',
              250, true, false, false, null);
}

function LoadPrefixSectionSettings2() {

    var accordion = $('<div>').addClass('accordions');
    var prefix = $('<div>').addClass('prefixescontainer');

    var header = $('<h1>').text('Prefixes').appendTo($(accordion));
    $('<a>').attr('href', '#').addClass('newprefixmodallink modallink newbutton')
        .click(function (e) {
            e.preventDefault();

            modal = $('.prefixmodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 250,
                resizable: false
            });

            $('.cancelmodal').click(function (e) {

                e.preventDefault();

                CloseModal(modal);

            });

            $('.cancelmodal').click(function (e) {

                e.preventDefault();

                CloseModal(modal);

            });

            $('.submitprefix').unbind('click');

            $('.submitprefix').click(function () {

                var item = {
                    Code: $(modal).find('.prefix-Code').val(),
                    Name: $(modal).find('.prefix-Name').val(),
                    Salutation: $(modal).find('.prefix-Salutation').val(),
                    LabelPrefix: $(modal).find('.prefix-LabelPrefix').val(),
                    LabelAbbreviation: $(modal).find('.prefix-LabelAbbreviation').val()
                }

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'prefixes',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    success: function () {

                        DisplaySuccessMessage('Success', 'Prefix saved successfully.');

                        CloseModal(modal);

                        LoadPrefixSettingsGrid();

                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
                    }
                });

            });
        })
        .appendTo($(header));

    $(prefix).appendTo($(accordion));

    LoadPrefixSettingsGrid();

    $(accordion).appendTo($('.contentcontainer'));

    LoadAccordions();


}

function LoadPrefixSettingsGrid() {

    var prefixcolumns = [
       { dataField: 'Id', width: '0px' },
       { dataField: 'Code', caption: 'Code' },
       { dataField: 'Name', caption: 'Description' },
       { dataField: 'Salutation', caption: 'Salutation Prefix' },
       { dataField: 'LabelPrefix', caption: 'Label Prefix' },
       { dataField: 'LabelAbbreviation', caption: 'Label Prefix Short' }
    ];

    LoadGrid('prefixesgrid', 'prefixescontainer', prefixcolumns, 'prefixes', null, EditPrefix, DeletePrefix);

}

function EditPrefix(id) {

    LoadPrefix(id);

    modal = $('.prefixmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitprefix').unbind('click');

    $('.submitprefix').click(function () {

        var item = {
            Code: $(modal).find('.prefix-Code').val(),
            Name: $(modal).find('.prefix-Name').val(),
            Salutation: $(modal).find('.prefix-Salutation').val(),
            LabelPrefix: $(modal).find('.prefix-LabelPrefix').val(),
            LabelAbbreviation: $(modal).find('.prefix-LabelAbbreviation').val()
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'prefixes/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Prefix saved successfully.');

                CloseModal(modal);

                LoadPrefixSettingsGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
            }
        });

    });

}

function DeletePrefix(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'prefixes/' + id,
        method: 'DELETE',
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function () {

            DisplaySuccessMessage('Success', 'Prefix deleted successfully.');

            LoadPrefixSettingsGrid();

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }

    });
    

}

function LoadPrefix(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'prefixes/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            if (data && data.Data && data.IsSuccessful) {

                $(modal).find('.prefix-Id').val(data.Data.Id);
                $(modal).find('.prefix-Code').val(data.Data.Code);
                $(modal).find('.prefix-Name').val(data.Data.Name);
                $(modal).find('.prefix-Salutation').val(data.Data.Salutation);
                $(modal).find('.prefix-LabelPrefix').val(data.Data.LabelPrefix);
                $(modal).find('.prefix-LabelAbbreviation').val(data.Data.LabelAbbreviation);

            }
        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}
/* END PREFIX SYSTEM SETTINGS */

/* new system settings section */

/* end new system settings section */

/* PROFESSIONAL SYSTEM SETTINGS */
function LoadProfessionalSectionSettings() {
    LoadSectionSettings(SettingsCategories.CRM, 'Professional', 'sectionpreferences', SystemSettings.Professional);

    var accordion = $('<div>').addClass('accordions');
    var incomeLevels = $('<div>').addClass('incomeLevelcontainer');
    var professions = $('<div>').addClass('professioncontainer');

    var header = $('<h1>').text('Income Level').appendTo($(accordion));
    $('<a>').attr('href', '#').addClass('newincomeLevelmodallink modallink newbutton')
        .click(function (e) {
            e.preventDefault();

            modal = $('.incomeLevelmodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 250,
                resizable: false
            });

            $('.cancelmodal').click(function (e) {
                e.preventDefault();
                CloseModal(modal);
            });

            $('.submitinc').unbind('click');

            $('.submitinc').click(function () {

                var item = {
                    Code: $(modal).find('.inc-Code').val(),
                    Name: $(modal).find('.inc-Name').val(),
                    IsActive: $(modal).find('.inc-IsActive').prop('checked')
                }

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'incomelevels',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    success: function () {

                        DisplaySuccessMessage('success', 'Income Levels saved successfully.');

                        CloseModal(modal);

                        LoadIncomeLevelSettingsGrid();
                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
                    }
                });
            });

        })
        .appendTo($(header));
    $(incomeLevels).appendTo($(accordion));

    LoadIncomeLevelSettingsGrid();

    header = $('<h1>').text('Professions').appendTo($(accordion));
    $('<a>').attr('href', '#').addClass('newprofessionsmodallink modallink newbutton')
        .click(function (e) {
            e.preventDefault();

            modal = $('.professionmodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 250,
                resizable: false
            });

            $('.cancelmodal').click(function (e) {
                e.preventDefault();
                CloseModal(modal);
            });

            $('.submitpro').unbind('click');

            $('.submitpro').click(function () {

                var item = {
                    Code: $(modal).find('.pro-Code').val(),
                    Name: $(modal).find('.pro-Name').val(),
                    IsActive: $(modal).find('.pro-IsActive').prop('checked')
                }

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'professions',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    success: function () {

                        DisplaySuccessMessage('success', 'Profession saved successfully.');

                        CloseModal(modal);

                        LoadProfessionSettingsGrid();
                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
                    }
                });
            });

        })
        .appendTo($(header));
    $(professions).appendTo($(accordion));

    LoadProfessionSettingsGrid();

    $(accordion).appendTo($('.contentcontainer'));

    LoadAccordions();
}

function LoadIncomeLevelSettingsGrid() {

    var incomeLevelcolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('incomeLevelgrid', 'incomeLevelcontainer', incomeLevelcolumns, 'incomelevels', null, EditIncomeLevel);

}

function LoadProfessionSettingsGrid() {

    var professioncolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('professiongrid', 'professioncontainer', professioncolumns, 'professions', null, Editprofession);

}

function EditIncomeLevel(id) {

    LoadIncomeLevel(id);

    modal = $('.incomeLevelmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitinc').unbind('click');

    $('.submitinc').click(function () {

        var item = {
            Code: $(modal).find('.inc-Code').val(),
            Name: $(modal).find('.inc-Name').val(),
            IsActive: $(modal).find('.inc-IsActive').prop('checked')
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'incomelevels/' + $(modal).find('.incomeLevelId').val(),
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Income Level saved successfully.');

                CloseModal(modal);

                LoadIncomeLevelSettingsGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
            }
        });

    });

}

function LoadIncomeLevel(id) {
    $.ajax({
        url: WEB_API_ADDRESS + 'incomelevels/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {
            if (data && data.Data && data.IsSuccessful) {
                $(modal).find('.incomeLevelId').val(data.Data.Id);
                $(modal).find('.inc-Code').val(data.Data.Code);
                $(modal).find('.inc-Name').val(data.Data.Name);
                $(modal).find('.inc-IsActive').prop('checked', data.Data.IsActive);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}

function Editprofession(id) {

    LoadProfession(id);

    modal = $('.professionmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitpro').unbind('click');

    $('.submitpro').click(function () {

        var item = {
            Code: $(modal).find('.pro-Code').val(),
            Name: $(modal).find('.pro-Name').val(),
            IsActive: $(modal).find('.pro-IsActive').prop('checked')
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'professions/' + $(modal).find('.professionId').val(),
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Profession saved successfully.');

                CloseModal(modal);

                LoadProfessionSettingsGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
            }
        });

    });

}

function LoadProfession(id) {
    $.ajax({
        url: WEB_API_ADDRESS + 'professions/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {
            if (data && data.Data && data.IsSuccessful) {
                $(modal).find('.professionId').val(data.Data.Id);
                $(modal).find('.pro-Code').val(data.Data.Code);
                $(modal).find('.pro-Name').val(data.Data.Name);
                $(modal).find('.pro-IsActive').prop('checked', data.Data.IsActive);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}




/* END PROFESSIONAL SYSTEM SETTINGS */

/* REGIONS SETTINGS */
function LoadRegionsSectionSettings() {

    $('.contentcontainer').empty();
    var lc = $('<div>').addClass('regionlevelcontainer');
    var rc = $('<div>').addClass('regioncontainer');
    var rgc = $('<div>').addClass('regiongridcontainer');
    $(lc).appendTo($('.contentcontainer'));
    $(rc).appendTo($('.contentcontainer'));
    $(rgc).appendTo($(rc));

    CreateRegionLevelSelector(lc);

}

function CreateRegionLevelSelector(container) {

    var ul = $('<ul>').addClass('regionlevellist').appendTo($(container));
    DisplayRegionLevels(ul);

}

function DisplayRegionLevels(container) {

    $.ajax({
        url: WEB_API_ADDRESS + 'regionlevels/',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            if (data && data.Data && data.IsSuccessful) {

                $.map(data.Data, function (level) {
                    var li = $('<li>').attr('id', level.Id).click(function () {
                        
                        if ($('.parentregions').length) {
                            $('.parentregions').remove();
                        }

                        $('.regiongridcontainer').empty();

                        $('.regionlevellist li').removeClass('selected');
                        $(this).addClass('selected');

                        if (level.IsChildLevel) {
                            var parents = $('<select>').addClass('parentregions');
                            $('.regioncontainer').prepend($(parents));

                            PopulateDropDown('.parentregions', 'regions/regionlevels/' + (level.Level - 1), '', '', null, function () {
                                DisplayRegions(level.Level, $('.parentregions').val());
                            });
                        }
                        else {
                            DisplayRegions(level.Level, null);
                        }
                    });

                    $('<a>').attr('href', '#').addClass('editregionlevellink').click(function (e) {
                        e.preventDefault();

                        $('.regionlevelid').val(level.Id);

                        modal = $('.regionlevelmodal').dialog({
                            closeOnEscape: false,
                            modal: true,
                            width: 250,
                            resizable: false
                        });

                        LoadRegionLevel(level.Id);

                        $('.cancelmodal').click(function (e) {

                            e.preventDefault();

                            CloseModal(modal);

                        });

                        $('.submitregionlevel').unbind('click');

                        $('.submitregionlevel').click(function () {

                            var id = $('.regionlevelid').val();

                            var item = {
                                Level: $(modal).find('.rl-Level').val(),
                                Label: $(modal).find('.rl-Label').val(),
                                Abbreviation: $(modal).find('.rl-Abbreviation').val(),
                                IsRequired: $(modal).find('.rl-IsRequired').prop('checked'),
                                IsChildLevel: $(modal).find('.rl-IsChildLevel').prop('checked'),
                            }

                            $.ajax({
                                type: 'PATCH',
                                url: WEB_API_ADDRESS + 'regionlevels/' + id,
                                data: item,
                                contentType: 'application/x-www-form-urlencoded',
                                crossDomain: true,
                                success: function () {

                                    DisplaySuccessMessage('Success', 'Region Level saved successfully.');

                                    CloseModal(modal);

                                    LoadRegionsSectionSettings();

                                },
                                error: function (xhr, status, err) {
                                    DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
                                }
                            });

                        });
                    }).appendTo($(li));
                    $('<span>').text(level.DisplayName).appendTo($(li));
                    $(li).appendTo($(container));
                });

                // Add Region Level button
                if (data.Data.length < 4) {

                    var li = $('<li>')
                        .attr('href', '#')
                        .text('+ Add Region Level')
                        .addClass('newregionlevellink')
                        .click(function (e) {
                            e.preventDefault();

                            modal = $('.regionlevelmodal').dialog({
                                closeOnEscape: false,
                                modal: true,
                                width: 250,
                                resizable: false
                            });

                            $(modal).find('.rl-Level').val(data.Data.length + 1);

                            $('.cancelmodal').click(function (e) {

                                e.preventDefault();

                                CloseModal(modal);

                            });

                            $('.submitregionlevel').unbind('click');

                            $('.submitregionlevel').click(function () {

                                var item = {
                                    Level: $(modal).find('.rl-Level').val(),
                                    Label: $(modal).find('.rl-Label').val(),
                                    Abbreviation: $(modal).find('.rl-Abbreviation').val(),
                                    IsRequired: $(modal).find('.rl-IsRequired').prop('checked'),
                                    IsChildLevel: $(modal).find('.rl-IsChildLevel').prop('checked')
                                }

                                $.ajax({
                                    type: 'POST',
                                    url: WEB_API_ADDRESS + 'regionlevels',
                                    data: item,
                                    contentType: 'application/x-www-form-urlencoded',
                                    crossDomain: true,
                                    success: function () {

                                        DisplaySuccessMessage('Success', 'Region Level saved successfully.');

                                        CloseModal(modal);

                                        LoadRegionsSectionSettings();

                                    },
                                    error: function (xhr, status, err) {
                                        DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
                                    }
                                });

                            });
                        });

                    $(li).appendTo($(container));
                }

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}

function DisplayRegions(level, parentid) {

    $('.currentlevel').val(level);

    var route = 'regionlevels/' + level + '/regions/';

    if (parentid) {
        route = route + parentid;
    }

    var columns = [
       { dataField: 'Id', width: '0px' },
       { dataField: 'Code', caption: 'Code' },
       { dataField: 'Name', caption: 'Name' },
       { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('regiongrid', 'regiongridcontainer', columns, route, null, EditRegion, DeleteRegion, function () {

        // Add the new link...
        var link = $('<a>')
            .attr('href', '#')
            .addClass('newmodallink')
            .text('New Region')
            .click(function (e) {
                e.preventDefault();

                NewRegion();

            });
        $('.regiongridcontainer').prepend($(link));

    });

}

function NewRegion() {

    modal = $('.regionmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitregion').unbind('click');

    $('.submitregion').click(function () {

        var item = {
            Level: $('.currentlevel').val(),
            ParentRegionId: $('.parentregions').val(),
            Code: $(modal).find('.reg-Code').val(),
            Name: $(modal).find('.reg-Name').val(),
            IsActive: $(modal).find('.reg-IsActive').prop('checked')
        }

        $.ajax({
            method: 'POST',
            url: WEB_API_ADDRESS + 'regions',
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function (data) {

                DisplaySuccessMessage('Success', 'Region saved successfully.');

                CloseModal(modal);

                DisplayRegions(data.Data.Level, data.Data.ParentRegionId);

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
            }
        });

    });

}

function EditRegion(id){

    LoadRegion(id);

    modal = $('.regionmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitregion').unbind('click');

    $('.submitregion').click(function () {

        var item = {
            ParentRegionId: $(modal).find('.parentregionid').val(),
            Code: $(modal).find('.reg-Code').val(),
            Name: $(modal).find('.reg-Name').val(),
            IsActive: $(modal).find('.reg-IsActive').prop('checked')
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'regions/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function (data) {

                DisplaySuccessMessage('Success', 'Region saved successfully.');

                CloseModal(modal);

                DisplayRegions(data.Data.Level, data.Data.ParentRegionId);

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
            }
        });

    });

}

function DeleteRegion(id) {

    $.ajax({
        method: 'DELETE',
        url: WEB_API_ADDRESS + 'regions/' + id,
        data: item,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function () {

            DisplaySuccessMessage('Success', 'Region deleted successfully.');

            CloseModal(modal);

            DisplayRegions($('.currentlevel').val(), $('.parentregions').val());

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}

function LoadRegionLevel(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'regionlevels/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {
            if (data && data.Data && data.IsSuccessful) {

                $(modal).find('.rl-Level').val(data.Data.Level);
                $(modal).find('.rl-Label').val(data.Data.Label);
                $(modal).find('.rl-Abbreviation').val(data.Data.Abbreviation);
                $(modal).find('.rl-IsRequired').prop('checked', data.Data.IsRequired);
                $(modal).find('.rl-IsChildLevel').prop('checked', data.Data.IsChildLevel);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error loading Region Level.');
        }
    });

}

function LoadRegion(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'regions/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {
            if (data && data.Data && data.IsSuccessful) {

                $(modal).find('.regionid').val(id);
                $(modal).find('.parentregionid').val(data.Data.ParentRegionId);
                $(modal).find('.currentlevel').val(data.Data.Level);

                $(modal).find('.reg-Code').val(data.Data.Code);
                $(modal).find('.reg-Name').val(data.Data.Name);
                $(modal).find('.reg-IsActive').prop('checked', data.Data.IsActive);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error loading Region Level.');
        }
    });

}

/* REGIONS SETTINGS */

function LoadRelationshipSectionSettings() {

    var accordion = $('<div>').addClass('accordions');
    var category = $('<div>').addClass('relationshipcategorycontainer');
    var type = $('<div>').addClass('relationshiptypecontainer');
    
    var header = $('<h1>').text('Relationship Categories').appendTo($(accordion));
    $('<a>').attr('href', '#').addClass('newrelationshipcategorymodallink modallink newbutton')
        .click(function (e) {
            e.preventDefault();

            modal = $('.relcatmodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 250,
                resizable: false
            });

            $('.cancelmodal').click(function (e) {

                e.preventDefault();

                CloseModal(modal);

            });

            $('.submitrelcat').unbind('click');

            $('.submitrelcat').click(function () {

                var item = {
                    Code: $(modal).find('.relcat-Code').val(),
                    Name: $(modal).find('.relcat-Name').val(),
                    IsActive: $(modal).find('.relcat-IsActive').prop('checked'),
                    IsShownInQuickView: $(modal).find('.relcat-IsShownInQuickView').prop('checked')
                }

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'relationshipcategories',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    success: function () {

                        DisplaySuccessMessage('Success', 'Relationship Category saved successfully.');

                        CloseModal(modal);

                        LoadRelationshipCategorySettingsGrid();

                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', 'An error occurred while saving the relationship category.');
                    }
                });

            });
        })
        .appendTo($(header));
    $(category).appendTo($(accordion));

    LoadRelationshipCategorySettingsGrid();

    header = $('<h1>').text('Relationship Types').appendTo($(accordion));
    $('<a>').attr('href', '#').addClass('newrelationshiptypesmodallink modallink newbutton')
        .click(function (e) {
            e.preventDefault();

            modal = $('.reltypemodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 250,
                resizable: false
            });

            PopulateDropDown('.reltype-ReciprocalTypeMaleId', 'relationshiptypes', '', '');
            PopulateDropDown('.reltype-ReciprocalTypeFemaleId', 'relationshiptypes', '', '');
            PopulateDropDown('.reltype-RelationshipCategoryId', 'relationshipcategories', '', '');

            $('.cancelmodal').click(function (e) {

                e.preventDefault();

                CloseModal(modal);

            });

            $('.submitreltype').unbind('click');

            $('.submitreltype').click(function () {

                var item = {
                    Code: $(modal).find('.reltype-Code').val(),
                    Name: $(modal).find('.reltype-Name').val(),
                    ReciprocalTypeMaleId: $(modal).find('.reltype-ReciprocalTypeMaleId').val(),
                    ReciprocalTypeFemaleId: $(modal).find('.reltype-ReciprocalTypeFemaleId').val(),
                    ConstituentCategory: $(modal).find('.reltype-ConstituentCategory').val(),
                    RelationshipCategoryId: $(modal).find('.reltype-RelationshipCategoryId').val(),
                    IsSpouse: $(modal).find('.reltype-IsSpouse').prop('checked'),
                    IsActive: $(modal).find('.reltype-IsActive').prop('checked')
                }

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'relationshiptypes',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    success: function () {

                        DisplaySuccessMessage('Success', 'Relationship type saved successfully.');

                        CloseModal(modal);

                        LoadRelationshipTypeSettingsGrid();

                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', 'An error occurred while saving the relationship type.');
                    }
                });

            });
        })
        .appendTo($(header));
    $(type).appendTo($(accordion));

    LoadRelationshipTypeSettingsGrid();

    $(accordion).appendTo($('.contentcontainer'));

    LoadAccordions();


}

function LoadRelationshipCategorySettingsGrid() {


    var relationshipcategorycolumns = [
       { dataField: 'Id', width: '0px' },
       { dataField: 'Code', caption: 'Code' },
       { dataField: 'Name', caption: 'Denomination' },
       { dataField: 'IsShownInQuickView', caption: 'Show in Quick View' },
       { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('relationshipcategorygrid', 'relationshipcategorycontainer', relationshipcategorycolumns, 'relationshipcategories', null, EditRelationshipCategory, DeleteRelationshipCategory);

}

function LoadRelationshipTypeSettingsGrid() {

    var relationshiptypecolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'ReciprocalTypeMale.DisplayName', caption: 'Male Reciprocal'},
        { dataField: 'ReciprocalTypeFemale.DisplayName', caption: 'Female Reciprocal'},
        {
            caption: 'Constituent Category', cellTemplate: function (container, options) {
                var category = 'Individual';

                switch (options.data.ConstituentCategory) {
                    case 1:
                        category = 'Organization';
                        break;
                    case 2:
                        category = 'Both';
                        break;
                }

                $('<label>').text(category).appendTo(container);
            }
        },
        { dataField: 'RelationshipCategory.DisplayName', caption: 'Relationship Category'},
        { dataField: 'IsSpouse', caption: 'Spouse'},
        { dataField: 'IsActive', caption: 'Active'}
    ];

    LoadGrid('relationshiptypegrid', 'relationshiptypecontainer', relationshiptypecolumns, 'relationshiptypes?fields=all', null, EditRelationshipType, DeleteRelationshipType);
}

/* RELATIONSHIP CATEGORY SYSTEM SETTINGS */
function EditRelationshipCategory(id) {

    LoadRelationshipCategory(id);

    modal = $('.relcatmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitrelcat').unbind('click');

    $('.submitrelcat').click(function () {

        var item = {
            Code: $(modal).find('.relcat-Code').val(),
            Name: $(modal).find('.relcat-Name').val(),
            IsShownInQuickView: $(modal).find('.relcat-IsShownInQuickView').prop('checked'),
            IsActive: $(modal).find('.relcat-IsActive').prop('checked')
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'relationshipcategories/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Relationship Category saved successfully.');

                CloseModal(modal);

                LoadRelationshipCategorySettingsGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred while saving the relationship category.');
            }
        });

    });

}

function DeleteRelationshipCategory(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'relationshipcategories/' + id,
        method: 'DELETE',
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function () {

            DisplaySuccessMessage('Success', 'Relationship Category deleted successfully.');

            LoadRelationshipCategorySettingsGrid();

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred deleting the Relationship Category.');
        }

    });


}

function LoadRelationshipCategory(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'relationshipcategories/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            if (data && data.Data && data.IsSuccessful) {

                $(modal).find('.relcat-Id').val(data.Data.Id);
                $(modal).find('.relcat-Code').val(data.Data.Code);
                $(modal).find('.relcat-Name').val(data.Data.Name);
                $(modal).find('.relcat-IsShownInQuickView').prop('checked', data.Data.IsShownInQuickView);
                $(modal).find('.relcat-IsActive').prop('checked', data.Data.IsActive);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error loading relationship category.');
        }
    });

}
/* END RELATIONSHIP CATEGORY SYSTEM SETTINGS */

/* RELATIONSHIP TYPE SYSTEM SETTINGS */
function EditRelationshipType(id) {

    LoadRelationshipType(id);

    modal = $('.reltypemodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    
    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitreltype').unbind('click');

    $('.submitreltype').click(function () {

        var item = {
            Code: $(modal).find('.reltype-Code').val(),
            Name: $(modal).find('.reltype-Name').val(),
            ReciprocalTypeMaleId: $(modal).find('.reltype-ReciprocalTypeMaleId').val(),
            ReciprocalTypeFemaleId: $(modal).find('.reltype-ReciprocalTypeFemaleId').val(),
            ConstituentCategory: $(modal).find('.reltype-ConstituentCategory').val(),
            RelationshipCategoryId: $(modal).find('.reltype-RelationshipCategoryId').val(),
            IsSpouse: $(modal).find('.reltype-IsSpouse').prop('checked'),
            IsActive: $(modal).find('.reltype-IsActive').prop('checked')
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'relationshiptypes/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Relationship Type saved successfully.');

                CloseModal(modal);

                LoadRelationshipTypeSettingsGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred while saving the relationship type.');
            }
        });

    });

}

function DeleteRelationshipType(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'relationshiptypes/' + id,
        method: 'DELETE',
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function () {

            DisplaySuccessMessage('Success', 'Relationship Type deleted successfully.');

            LoadRelationshipTypeSettingsGrid();

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred deleting the Relationship Type.');
        }

    });

}

function LoadRelationshipType(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'relationshiptypes/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            if (data && data.Data && data.IsSuccessful) {

                $(modal).find('.reltype-Id').val(data.Data.Id);
                $(modal).find('.reltype-Code').val(data.Data.Code);
                $(modal).find('.reltype-Name').val(data.Data.Name);
                $(modal).find('.reltype-ConstituentCategory').val(data.Data.ConstituentCategory);
                $(modal).find('.reltype-IsSpouse').prop('checked', data.Data.IsSpouse);
                $(modal).find('.reltype-IsActive').prop('checked', data.Data.IsActive);

                PopulateDropDown('.reltype-ReciprocalTypeMaleId', 'relationshiptypes', '', '', data.Data.ReciprocalTypeMaleId);
                PopulateDropDown('.reltype-ReciprocalTypeFemaleId', 'relationshiptypes', '', '', data.Data.ReciprocalTypeFemaleId);
                PopulateDropDown('.reltype-RelationshipCategoryId', 'relationshipcategories', '', '', data.Data.RelationshipCategoryId);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error loading relationship type.');
        }
    });

}
/* END RELATIONSHIP TYPE SYSTEM SETTINGS */

function LoadTagGroupSectionSettings() {

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

    LoadGrid('taggroupsgrid',
        'contentcontainer',
        columns,
        'taggroups',
        TagGroupSelected,
        EditTagGroup,
        DeleteEntity);

    CreateNewModalLink("New Tag Group", NewTagGroupModal, '.taggroupsgrid', '.contentcontainer', 'newtaggroupmodal');
}

function TagGroupSelected(info) {

    var taggrid;
    var selectedRow;
    var tagscontainers = $(".tagscontainer");

    if (tagscontainers.length === 0) {
        taggrid = $("<div>").addClass("tagscontainer");
    } else {
        taggrid = tagscontainers[0];
    }

    $(taggrid).insertAfter($('.taggroupsgrid'));

    if (!info) {
        var dataGrid = $('.taggroupsgrid').dxDataGrid('instance');
        info = dataGrid.getSelectedRowsData();
        selectedRow = info[0];
    } else {
        selectedRow = info.data;
    }

    var columns = [
        { dataField: 'Id', width: "0px" },
        { dataField: 'Order', caption: 'Order' },
        { dataField: 'Code', caption: "Code" },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' },
    ];

    LoadGrid('tagsgrid',
        'tagscontainer',
        columns,
        'taggroups/' + selectedRow.Id + '/tags',
        TagGroupSelected,
        EditTag,
        DeleteEntity);

    CreateNewModalLink("New Tag", NewTagModal, '.tagsgrid', '.tagscontainer', 'newtagmodal');

}

function EditTagGroup(id) {

    EditEntity('.taggroupmodal', '.savetaggroup', 250, LoadTagGroupData, LoadTagGroupSectionSettings, GetTagGroupToSave, 'Tag Group', 'taggroups', id);

}

function EditTag(id) {

    EditEntity('.tagmodal', '.savetag', 250, LoadTagData, TagGroupSelected, GetTagToSave, 'Tag', 'tags', id);

}

function LoadTagGroupData(data, modal) {

    if (data.Data) {
        $(modal).find('.hidtaggroupid').val(data.Data.Id);
        $(modal).find('.tg-Order').val(data.Data.Order);
        $(modal).find('.tg-Name').val(data.Data.Name);
        $(modal).find('.tg-Select').val(data.Data.TagSelectionType);
        $(modal).find('.tg-IsActive').prop('checked', data.Data.IsActive);
    }
    
}

function LoadTagData(data, modal) {

    if (data.Data) {
        $(modal).find('.hidtagid').val(data.Data.Id);
        $(modal).find('.hidtagparentgroupid').val(data.Data.TagGroupId);
        $(modal).find('.t-Order').val(data.Data.Order);
        $(modal).find('.t-Name').val(data.Data.Name);
        $(modal).find('.t-Code').val(data.Data.Code);
        $(modal).find('.t-IsActive').prop('checked', data.Data.IsActive);
    }
    
}

function GetTagToSave(modal, isUpdate) {
    var dataGrid = $('.taggroupsgrid').dxDataGrid('instance');
    var info = dataGrid.getSelectedRowsData();

    var item = {
        TagGroupId: info[0].Id,
        Order: $(modal).find('.t-Order').val(),
        Code: $(modal).find('.t-Code').val(),
        Name: $(modal).find('.t-Name').val(),
        IsActive: $(modal).find('.t-IsActive').prop('checked'),
    }
    if (isUpdate === true) {
        item.Id = $(modal).find('.hidtagid').val();
    }
    return item;
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

function NewTagGroupModal(modalLinkClass) {

    NewEntityModal(modalLinkClass, '.taggroupmodal', '.savetaggroup', 250, LoadTagGroupData, LoadTagGroupSectionSettings, GetTagGroupToSave, 'Tag Group', 'taggroups');

}

function NewTagModal(modalLinkClass) {

    NewEntityModal(modalLinkClass, '.tagmodal', '.savetag', 250, LoadTagData, TagGroupSelected, GetTagToSave, 'Tag', 'tags');

}

function CreateNewModalLink(linkText, newEntityModalMethod, prependToClass, addToContainer, modalLinkClass) {

    var modallink = $('<a>').attr('href', '#').addClass('newmodallink').addClass(modalLinkClass).text(linkText).appendTo($(addToContainer));

    $(prependToClass).before($(modallink));

    if (modalLinkClass.indexOf('.') != 0)
        modalLinkClass = '.' + modalLinkClass;

    newEntityModalMethod(modalLinkClass);

}
/* END CRM SETTINGS */


/* DONATIONS SETTINGS */
function LoadDonationSettingsSectionSettings() {



}

function LoadDonorSettingsSectionSettings() {



}

function LoadGLAccountAutoAssignSectionSettings() {



}

function LoadDonationHomeScreenSectionSettings() {



}
/* END DONATIONS SETTINGS */


/* GENERAL LEDGER SETTINGS */
function LoadAccountingSettingsSectionSettings() {



}

function LoadBudgetSettingsSectionSettings() {



}

function LoadChartAccountsSettingsSectionSettings() {



}

function LoadEntitiesSectionSettings() {



}

function LoadFiscalYearSectionSettings() {



}

function LoadFundAccountingSectionSettings() {



}

function LoadGLFormatSectionSettings() {



}

function LoadJournalSectionSettings() {



}

function LoadUtilitiesSectionSettings() {



}
/* END GENERAL LEDGER SETTINGS */


/* REPORTS SETTINGS */
function LoadPageFootersSectionSettings() {



}

function LoadPageHeadersSectionSettings() {



}

function LoadReportFootersSectionSettings() {



}

function LoadReportHeadersSectionSettings() {



}
/* END REPORTS SETTINGS */


/* CUSTOM FIELDS */
var modalLeft = 0;
var options = [];

function LoadCRMClientCustomFieldsSectionSettings() {

    DisplayCustomFieldsGrid('contentcontainer', CustomFieldEntity.CRM); // CRM = 19

    CreateNewCustomFieldModalLink(CustomFieldEntity.CRM, 'New CRM Custom Field');

}

function LoadDonationClientCustomFieldsSectionSettings() {

    DisplayCustomFieldsGrid('contentcontainer', CustomFieldEntity.Gifts); // Gifts = 9

    CreateNewCustomFieldModalLink(CustomFieldEntity.Gifts, 'New Donations Custom Field');

}

function LoadGLClientCustomFieldsSectionSettings() {

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
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}
/* END CUSTOM FIELDS */

