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
    DBA: 'DBASettings',
    Demographics: 'DemographicSettings',
    Education: 'EducationSettings',
    Personal: 'PersonalSettings',
    Professional: 'ProfessionalSettings',
    Note: 'NoteSettings'
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

/* NOTE SYSTEM SETTINGS */

function LoadNoteSectionSettings() {

    LoadSectionSettings(SettingsCategories.Common, 'Note', 'sectionpreferences', SystemSettings.Note);

    var accordion = $('<div>').addClass('accordions');
    var noteCodes = $('<div>').addClass('noteCodecontainer');
    var noteCategories = $('<div>').addClass('noteCategorycontainer');
    var noteTopics = $('<div>').addClass('noteTopiccontainer');

    var header = $('<h1>').text('Note Code').appendTo($(accordion));
    $('<a>').attr('href', '#').addClass('newnoteCodemodallink modallink newbutton')
        .click(function (e) {
            e.preventDefault();

            modal = $('.noteCodemodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 250,
                resizable: false
            });

            $('.cancelmodal').click(function (e) {
                e.preventDefault();
                CloseModal(modal);
            });

            $('.submitnoteCode').unbind('click');

            $('.submitnoteCode').click(function () {

                var item = {
                    Code: $(modal).find('.noteCode-Code').val(),
                    Name: $(modal).find('.noteCode-Name').val(),
                    IsActive: $(modal).find('.noteCode-IsActive').prop('checked')
                }

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'notecodes',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    success: function () {

                        DisplaySuccessMessage('success', 'Note Code saved successfully.');

                        CloseModal(modal);

                        LoadNoteCodeSettingsGrid();
                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', 'An error occurred while saving the Note Code.')
                    }
                });
            });

        })
        .appendTo($(header));
    $(noteCodes).appendTo($(accordion));

    LoadNoteCodeSettingsGrid();

    header = $('<h1>').text('Note Category').appendTo($(accordion));
    $('<a>').attr('href', '#').addClass('newnoteCategorymodallink modallink newbutton')
        .click(function (e) {
            e.preventDefault();

            modal = $('.noteCategorymodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 250,
                resizable: false
            });

            $('.cancelmodal').click(function (e) {
                e.preventDefault();
                CloseModal(modal);
            });

            $('.submitnoteCategory').unbind('click');

            $('.submitnoteCategory').click(function () {
                var item = {
                    Label: $(modal).find('.noteCategory-Code').val(),
                    Name: $(modal).find('.noteCategory-Name').val(),
                    IsActive: $(modal).find('.noteCategory-IsActive').prop('checked')
                }

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'notecategories',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    success: function () {

                        DisplaySuccessMessage('success', 'Note Category saved successfully.');

                        CloseModal(modal);

                        LoadNoteCategorySettingsGrid();
                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', 'An error occurred while saving the Note Category.')
                    }
                });
            });

        })
        .appendTo($(header));
    $(noteCategories).appendTo($(accordion));

    LoadNoteCategorySettingsGrid();

    header = $('<h1>').text('Topic').appendTo($(accordion));
    $('<a>').attr('href', '#').addClass('newnoteTopicmodallink modallink newbutton')
        .click(function (e) {
            e.preventDefault();

            modal = $('.noteTopicmodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 250,
                resizable: false
            });

            $('.cancelmodal').click(function (e) {
                e.preventDefault();
                CloseModal(modal);
            });

            $('.submitnoteTopic').unbind('click');

            $('.submitnoteTopic').click(function () {

                var item = {
                    Code: $(modal).find('.noteTopic-Code').val(),
                    Name: $(modal).find('.noteTopic-Name').val(),
                    IsActive: $(modal).find('.noteTopic-IsActive').prop('checked')
                }

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'notetopics',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    success: function () {

                        DisplaySuccessMessage('success', 'Note Topic saved successfully.');

                        CloseModal(modal);

                        LoadNoteTopicSettingsGrid();
                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', 'An error occurred while saving the Note Topic.')
                    }
                });
            });

        })
        .appendTo($(header));
    $(noteTopics).appendTo($(accordion));

    LoadNoteTopicSettingsGrid();

    $(accordion).appendTo($('.contentcontainer'));

    LoadAccordions();

}

function LoadNoteCodeSettingsGrid() {
    var noteCodecolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];
    LoadGrid('noteCodegrid', 'noteCodecontainer', noteCodecolumns, 'notecodes', null, EditNoteCode);

}

function EditNoteCode(id) {
    LoadNoteCode(id);
    modal = $('.noteCodemodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitnoteCode').unbind('click');

    $('.submitnoteCode').click(function () {

        var item = {
            Code: $(modal).find('.noteCode-Code').val(),
            Name: $(modal).find('.noteCode-Name').val(),
            IsActive: $(modal).find('.noteCode-IsActive').prop('checked')
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'notecodes/' + $(modal).find('.noteCodeId').val(),
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Note Code saved successfully.');

                CloseModal(modal);

                LoadNoteCodeSettingsGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the Note Code.');
            }
        });

    });

}

function LoadNoteCode(id) {
    $.ajax({
        url: WEB_API_ADDRESS + 'notecodes/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {
            if (data && data.Data && data.IsSuccessful) {
                $(modal).find('.noteCodeId').val(data.Data.Id);
                $(modal).find('.noteCode-Code').val(data.Data.Code);
                $(modal).find('.noteCode-Name').val(data.Data.Name);
                $(modal).find('.noteCode-IsActive').prop('checked', data.Data.IsActive);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error loading Note Code.');
        }
    });

}

function LoadNoteCategorySettingsGrid() {
    var noteCategorycolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Label', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];
    LoadGrid('noteCategorygrid', 'noteCategorycontainer', noteCategorycolumns, 'notecategories', null, EditNoteCategory);

}

function EditNoteCategory(id) {
    LoadNoteCategory(id);
    modal = $('.noteCategorymodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitnoteCategory').unbind('click');

    $('.submitnoteCategory').click(function () {
        var item = {
            Label: $(modal).find('.noteCategory-Code').val(),
            Name: $(modal).find('.noteCategory-Name').val(),
            IsActive: $(modal).find('.noteCategory-IsActive').prop('checked')
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'notecategories/' + $(modal).find('.noteCategoryId').val(),
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Note Category saved successfully.');

                CloseModal(modal);

                LoadNoteCategorySettingsGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the Note Category.');
            }
        });

    });

}

function LoadNoteCategory(id) {
    $.ajax({
        url: WEB_API_ADDRESS + 'notecategories/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {
            if (data && data.Data && data.IsSuccessful) {
                $(modal).find('.noteCategoryId').val(data.Data.Id);
                $(modal).find('.noteCategory-Code').val(data.Data.Label);
                $(modal).find('.noteCategory-Name').val(data.Data.Name);
                $(modal).find('.noteCategory-IsActive').prop('checked', data.Data.IsActive);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error loading Note Category.');
        }
    });

}

function LoadNoteTopicSettingsGrid() {
    var noteTopiccolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];
    LoadGrid('noteTopicgrid', 'noteTopiccontainer', noteTopiccolumns, 'notetopics', null, EditNoteTopic);

}

function EditNoteTopic(id) {
    LoadNoteTopic(id);
    modal = $('.noteTopicmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitnoteTopic').unbind('click');

    $('.submitnoteTopic').click(function () {

        var item = {
            Code: $(modal).find('.noteTopic-Code').val(),
            Name: $(modal).find('.noteTopic-Name').val(),
            IsActive: $(modal).find('.noteTopic-IsActive').prop('checked')
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'notetopics/' + $(modal).find('.noteTopicId').val(),
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Topic saved successfully.');

                CloseModal(modal);

                LoadNoteTopicSettingsGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the Topic.');
            }
        });

    });

}

function LoadNoteTopic(id) {
    $.ajax({
        url: WEB_API_ADDRESS + 'notetopics/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {
            if (data && data.Data && data.IsSuccessful) {
                $(modal).find('.noteTopicId').val(data.Data.Id);
                $(modal).find('.noteTopic-Code').val(data.Data.Code);
                $(modal).find('.noteTopic-Name').val(data.Data.Name);
                $(modal).find('.noteTopic-IsActive').prop('checked', data.Data.IsActive);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error loading Note Topic.');
        }
    });

}

/* END NOTE SYSTEM SETTINGS */

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
    $('<a>').attr('href', '#').addClass('newclergystatusmodallink modallink newbutton')
        .click(function (e) {
            e.preventDefault();

            modal = $('.clergystatusmodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 250,
                resizable: false
            });

            $('.cancelmodal').click(function (e) {
                e.preventDefault();
                CloseModal(modal);
            });

            $('.submitcstat').unbind('click');

            $('.submitcstat').click(function () {
                var item = {
                    Code: $(modal).find('.cstat-Code').val(),
                    Name: $(modal).find('.cstat-Name').val(),
                    IsActive: $(modal).find('.cstat-IsActive').prop('checked')
                }

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'clergystatuses',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    success: function () {

                        DisplaySuccessMessage('success', 'Clergy Status saved successfully.');

                        CloseModal(modal);

                        LoadClergyStatusSettingsGrid();
                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', 'An error occurred while saving the Clergy Status.')
                    }
                });
            });

        })
        .appendTo($(header));
    $(status).appendTo($(accordion));

    LoadClergyStatusSettingsGrid();

    header = $('<h1>').text('Clergy Type').appendTo($(accordion));
    $('<a>').attr('href', '#').addClass('newclergytypemodallink modallink newbutton')
        .click(function (e) {
            e.preventDefault();

            modal = $('.clergytypemodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 250,
                resizable: false
            });

            $('.cancelmodal').click(function (e) {
                e.preventDefault();
                CloseModal(modal);
            });

            $('.submitctype').unbind('click');

            $('.submitctype').click(function () {
                var item = {
                    Code: $(modal).find('.ctype-Code').val(),
                    Name: $(modal).find('.ctype-Name').val(),
                    IsActive: $(modal).find('.ctype-IsActive').prop('checked')
                }

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'clergytypes',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    success: function () {

                        DisplaySuccessMessage('success', 'Clergy Type saved successfully.');

                        CloseModal(modal);

                        LoadClergyTypeSettingsGrid();
                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', 'An error occurred while saving the Clergy Type.')
                    }
                });
            });

        })
        .appendTo($(header));
    $(types).appendTo($(accordion));

    LoadClergyTypeSettingsGrid();

    $(accordion).appendTo($('.contentcontainer'));

    LoadAccordions();

}

function LoadClergyStatusSettingsGrid() {
    var statuscolumns = [
          { dataField: 'Id', width: '0px' },
          { dataField: 'Code', caption: 'Code' },
          { dataField: 'Name', caption: 'Description' },
          { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('clergystatusgrid', 'clergystatuscontainer', statuscolumns, 'clergystatuses', null, EditClergyStatus, DeleteClergyStatus);
}

function LoadClergyTypeSettingsGrid() {
    var typecolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('clergytypegrid', 'clergytypecontainer', typecolumns, 'clergytypes', null, EditClergyType, DeleteClergyType);
}

// CLERGY STATUS SYSTEM SETTINGS
function EditClergyStatus(id) {

    LoadClergyStatus(id);

    modal = $('.clergystatusmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    $('.cancelmodal').click(function (e) {
        e.preventDefault();
        CloseModal(modal);
    })

    $('.submitcstat').unbind('click');

    $('.submitcstat').click(function () {

        var item = {
            Code: $(modal).find('.cstat-Code').val(),
            Name: $(modal).find('.cstat-Name').val(),
            IsActive: $(modal).find('.cstat-IsActive').prop('checked')
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'clergystatuses/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Clergy Status saved successfully.');

                CloseModal(modal);

                LoadClergyStatusSettingsGrid();

            },
            error: function (xhr, status, err) {

                DisplayErrorMessage('Error', 'An error occurred while saving Clergy Status.');

            }
        });
    });
}

function DeleteClergyStatus(id) {

}

function LoadClergyStatus(id) {
    $.ajax({
        url: WEB_API_ADDRESS + 'clergystatuses/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            if (data && data.Data && data.IsSuccessful) {

                $(modal).find('.cstat-Id').val(data.Data.Id);
                $(modal).find('.cstat-Code').val(data.Data.Code);
                $(modal).find('.cstat-Name').val(data.Data.Name);
                $(modal).find('.cstat-IsActive').prop('checked', data.Data.IsActive);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred loading Clergy Status.');
        }

    });
}
// END CLERGY STATUS SYSTEM SETTINGS

// CLERGY TYPE SYSTEM SETTINGS
function EditClergyType(id) {

    LoadClergyType(id);

    modal = $('.clergytypemodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    $('.cancelmodal').click(function (e) {
        e.preventDefault();
        CloseModal(modal);
    })

    $('.submitctype').unbind('click');

    $('.submitctype').click(function () {

        var item = {
            Code: $(modal).find('.ctype-Code').val(),
            Name: $(modal).find('.ctype-Name').val(),
            IsActive: $(modal).find('.ctype-IsActive').prop('checked')
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'clergytypes/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Clergy Type saved successfully.');

                CloseModal(modal);

                LoadClergyTypeSettingsGrid();

            },
            error: function (xhr, status, err) {

                DisplayErrorMessage('Error', 'An error occurred while saving Clergy Type.');

            }
        });
    });
}

function DeleteClergyType(id) {

}

function LoadClergyType(id) {
    $.ajax({
        url: WEB_API_ADDRESS + 'clergytypes/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            if (data && data.Data && data.IsSuccessful) {

                $(modal).find('.ctype-Id').val(data.Data.Id);
                $(modal).find('.ctype-Code').val(data.Data.Code);
                $(modal).find('.ctype-Name').val(data.Data.Name);
                $(modal).find('.ctype-IsActive').prop('checked', data.Data.IsActive);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred loading Clergy Type.');
        }

    });
}
// END CLERGY TYPE SYSTEM SETTINGS

function LoadConstituentTypesSectionSettings() {



}

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
                        DisplayErrorMessage('Error', 'An error occurred while saving the Degree.')
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
                        DisplayErrorMessage('Error', 'An error occurred while saving the Education Level.')
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
                        DisplayErrorMessage('Error', 'An error occurred while saving the School.')
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
                DisplayErrorMessage('Error', 'An error occurred during saving the Degree.');
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
            DisplayErrorMessage('Error', 'An error occurred when loading the degree.');
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
                DisplayErrorMessage('Error', 'An error occurred during saving the Education Level.');
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
            DisplayErrorMessage('Error', 'An error occurred when loading the education level.');
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
                DisplayErrorMessage('Error', 'An error occurred during saving the School.');
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
            DisplayErrorMessage('Error', 'An error occurred when loading the school.');
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



}

function LoadPaymentPreferencesSectionSettings() {



}

function LoadPersonalSectionSettings() {

    LoadSectionSettings(SettingsCategories.CRM, 'Personal', 'sectionpreferences', SystemSettings.Personal);

}

/* PREFIX SYSTEM SETTINGS */
function LoadPrefixSectionSettings() {

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
                        DisplayErrorMessage('Error', 'An error occurred while saving the Prefix.');
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
                DisplayErrorMessage('Error', 'An error occurred while saving the Prefix.');
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
            DisplayErrorMessage('Error', 'An error occurred deleting the Prefix.');
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
            DisplayErrorMessage('Error', 'An error loading prefix.');
        }
    });

}
/* END PREFIX SYSTEM SETTINGS */

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
                        DisplayErrorMessage('Error', 'An error occurred while saving the Income Level.')
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
                        DisplayErrorMessage('Error', 'An error occurred while saving the Profession.')
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
                DisplayErrorMessage('Error', 'An error occurred during saving the Income Level.');
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
            DisplayErrorMessage('Error', 'An error loading Income Level.');
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
                DisplayErrorMessage('Error', 'An error occurred during saving the Profession.');
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
            DisplayErrorMessage('Error', 'An error loading Profession.');
        }
    });

}




/* END PROFESSIONAL SYSTEM SETTINGS */

function LoadRegionsSectionSettings() {



}

function LoadRelationshipSectionSettings() {



}

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
            DisplayErrorMessage('Error', 'An error saving custom field.');
        }
    });

}
/* END CUSTOM FIELDS */






