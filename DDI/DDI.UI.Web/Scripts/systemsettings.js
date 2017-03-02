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
    Demographics: 'DemographicSettings'
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

    // CreateNewCustomFieldModalLink(customfieldentity.CRM, 'New CRM Custom Field');

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
            DisplayErrorMessage('Error', 'An error loading Grid.');
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
            DisplayErrorMessage('Error', 'An error getting the system settings.');
        }
    });

}


/* SECTION SETTINGS */
function LoadSectionSettings(category, section, route, sectionKey) {

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
            DisplayErrorMessage('Error', 'An error getting the section label.');
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
            DisplayErrorMessage('Error', 'An error getting the section label.');
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

function LoadNotesSettingsSectionSettings() {



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
                width: 250,
                resizable: false
            });

            $('.cancelmodal').click(function (e) {

                e.preventDefault();

                CloseModal();

            });

            $('.submitconsttype').unbind('click');

            $('.submitconsttype').click(function () {

                var item = {
                    Code: $(modal).find('.consttype-Code').val(),
                    Name: $(modal).find('.consttype-Name').val(),
                    Category: $(modal).find('.consttype-Category').val(),
                    //Tags: $(modal).find('.consttype-Tags').val(),
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

                        CloseModal();

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
        { dataField: 'Tags', caption: 'Default Tags' },
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
        width: 250,
        resizable: false
    });

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
            //Tags: $(modal).find('.consttype-Tags').val(),
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
                $(modal).find('.consttype-Tags').val(data.Data.Tags);
                $(modal).find('.consttype-NameFormat').val(data.Data.NameFormat);
                $(modal).find('.consttype-SalutationFormal').val(data.Data.SalutationFormal);
                $(modal).find('.consttype-SalutationInformal').val(data.Data.SalutationInformal);

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

                CloseModal();

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

                        CloseModal();

                        LoadDenominationSettingsGrid();

                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', 'An error occurred during saving the Denomination.');
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

                CloseModal();

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

                        CloseModal();

                        LoadEthnicitySettingsGrid();

                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', 'An error occurred during saving the Ethnicity.');
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

                CloseModal();

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

                        CloseModal();

                        LoadLanguageSettingsGrid();

                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', 'An error occurred during saving the language.');
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
                DisplayErrorMessage('Error', 'An error occurred during saving the Denomination.');
            }
        });

    });

}

function DeleteDenomination(id) {



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
            DisplayErrorMessage('Error', 'An error loading denomination.');
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
                DisplayErrorMessage('Error', 'An error occurred during saving the Ethnicity.');
            }
        });

    });

}

function DeleteEthnicity(id) {



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
            DisplayErrorMessage('Error', 'An error loading ethnicity.');
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
                DisplayErrorMessage('Error', 'An error occurred during saving the language.');
            }
        });

    });

}

function DeleteLanguage(id) {



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
            DisplayErrorMessage('Error', 'An error loading language.');
        }
    });

}
/* END LANGUAGE SYSTEM SETTINGS */

/* END DEMOGRAPHICS SYSTEM SETTINGS */




function LoadDBASectionSettings() {



}

function LoadEducationSectionSettings() {

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



}

function LoadPaymentPreferencesSectionSettings() {



}

function LoadPersonalSectionSettings() {



}

function LoadPrefixSectionSettings() {



}

function LoadProfessionalSectionSettings() {



}

function LoadRegionsSectionSettings() {



}

function LoadRelationshipSectionSettings() {



}

function LoadTagsSectionSettings() {



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

    DisplayCustomFieldsGrid('contentcontainer', customfieldentity.CRM); // CRM = 19

    CreateNewCustomFieldModalLink(customfieldentity.CRM, 'New CRM Custom Field');

}

function LoadDonationClientCustomFieldsSectionSettings() {

    DisplayCustomFieldsGrid('contentcontainer', customfieldentity.Gifts); // Gifts = 9

    CreateNewCustomFieldModalLink(customfieldentity.Gifts, 'New Donations Custom Field');

}

function LoadGLClientCustomFieldsSectionSettings() {

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






