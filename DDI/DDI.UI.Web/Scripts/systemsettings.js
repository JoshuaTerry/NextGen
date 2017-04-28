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

        $('.gridcontainer').html('');

        $('.systemsettings a').removeClass('selected');

        var functionToCall = $(this).attr('class') + 'SectionSettings';

        $(this).addClass('selected');

        $('.systemsettingsheader').text($(this).text());

        ExecuteFunction(functionToCall, window);

        InitRequiredLabels("educationLevelmodal")

    });

});

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

    $(container).appendTo($('.gridcontainer'));

}

function GetSetting(category, key, route, id, checkbox, label) {

    MakeServiceCall('GET', route + '/' + category + '/settings/' + key, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {

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

    MakeServiceCall(method, route, JSON.stringify(item), function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {
                DisplaySuccessMessage('Success', 'Setting saved successfully.');

                $(idContainer).val(data.Data.Id);
            }

        }

    }, null);

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
    LoadGrid('.noteCodecontainer', 'noteCodegrid', noteCodecolumns, 'notecodes', 'notecodes', null, 'noteCode-',
        '.noteCodemodal', '.noteCodemodal', 250, true, false, false, null);

    header = $('<h1>').text('Note Category').appendTo($(accordion));
    $(noteCategories).appendTo($(accordion));

    var noteCategorycolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Label', caption: 'Label' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];
    LoadGrid('.noteCategorycontainer', 'noteCategorygrid', noteCategorycolumns, 'notecategories?fields=all', 'notecategories', null, 'noteCategory-',
        '.noteCategorymodal', '.noteCategorymodal', 250, true, false, false, null);
    

    header = $('<h1>').text('Topic').appendTo($(accordion));
    $(noteTopics).appendTo($(accordion));

    var noteTopiccolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];
    LoadGrid('.noteTopiccontainer', 'noteTopicgrid', noteTopiccolumns, 'notetopics?fields=all', 'notetopics', null, 'noteTopic-',
        '.noteTopicmodal', '.noteTopicmodal', 250, true, false, false, null);

    $(accordion).appendTo($('.gridcontainer'));

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
    LoadGrid('.clergystatuscontainer', 'clergystatusgrid', statuscolumns, 'clergystatuses?fields=all', 'clergystatuses', null, 'cstat-',
        '.clergystatusmodal', '.clergystatusmodal', 250, true, false, false, null);

    header = $('<h1>').text('Clergy Type').appendTo($(accordion));
    $(types).appendTo($(accordion));

    var typecolumns = [
    { dataField: 'Id', width: '0px' },
    { dataField: 'Code', caption: 'Code' },
    { dataField: 'Name', caption: 'Description' },
    { dataField: 'IsActive', caption: 'Active' }
    ];
    LoadGrid('.clergytypecontainer', 'clergytypegrid', typecolumns, 'clergytypes?fields=all', 'clergytypes', null, 'ctype-',
        '.clergytypemodal', '.clergytypemodal', 250, true, false, false, null);

    $(accordion).appendTo($('.gridcontainer'));

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

            $('.canceltypemodal').click(function (e) {

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

                MakeServiceCall('POST', 'constituenttypes', item, function (data) {

                    if (data.Data) {
                        DisplaySuccessMessage('Success', 'Constituent Type saved successfully.');

                        CloseModal(modal);

                        LoadConstituentTypeSettingsGrid();
                    }

                }, null);

            });
        })
        .appendTo($(header));

    $(types).appendTo($(accordion));

    LoadConstituentTypeSettingsGrid();

    $(accordion).appendTo($('.gridcontainer'));

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
                MakeServiceCall('GET', 'DELETE' + $(modal).find('.consttype-Id').val() + '/tag/' + tag.Id, null, function (data) {

                    if (data.Data) {
                        DisplaySuccessMessage('Success', 'Tag was deleted successfully.');
                        CloseModal(modal);
                        EditConstituentType($(modal).find('.consttype-Id').val());
                    }

                }, null)
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

                tagmodal = $('.tagselectmodal').dialog({
                    closeOnEscape: false,
                    modal: true,
                    width: 450,
                    resizable: false
                });

                LoadAvailableTags(tagmodal);

                $('.cancelmodal').click(function (e) {

                    e.preventDefault();

                    CloseModal(tagmodal);
                    CloseModal(modal);
                    EditConstituentType(constTypeId);

                });

                $('.saveselectedtags').unbind('click');

                $('.saveselectedtags').click(function () {

                    var tagIds = [];

                    $('.tagselectgridcontainer').find('input').each(function (index, value) {

                        if ($(value).prop('checked')) {
                            tagIds.push($(value).val());
                        }
                    });

                    MakeServiceCall('POST', 'constituenttypes/' + constTypeId + '/constituenttypetags', JSON.stringify({ tags: tagIds }), function (data) {

                        if (data.Data) {
                            DisplaySuccessMessage('Success', 'Tags saved successfully.');
                            CloseModal(tagmodal);
                            EditConstituentType(constTypeId);
                        }

                    }, null);

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

    
    CustomLoadGrid('constituenttypesgrid', 'constituenttypescontainer', typecolumns, 'constituenttypes', null, EditConstituentType, DeleteConstituentType);

}


// CONSTITUENT TYPE SYSTEM SETTINGS 
function EditConstituentType(id) {

    LoadConstituentType(id);


    modal = $('.constituenttypemodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 400,
        resizable: false
    });

    $('.consttype-tagselect').show();

    $('.canceltypemodal').click(function (e) {

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

        MakeServiceCall('PATCH', 'constituenttypes/' + id, item, function (data) {

            if (data.Data) {
                DisplaySuccessMessage('Success', 'Constituent type saved successfully.');

                CloseModal(modal);

                LoadConstituentTypeSettingsGrid();
            }

        }, null);

    });

}

function DeleteConstituentType(id) {

    MakeServiceCall('DELETE', 'constituenttypes/' + id, null, function (data) {

        if (data.Data) {
            DisplaySuccessMessage('Success', 'Constituent Type deleted successfully.');

            LoadConstituentTypeSettingsGrid();
        }

    }, null);

}

function LoadConstituentType(id) {

    MakeServiceCall('GET', 'constituenttypes/' + id, null, function (data) {

        if (data.Data) {
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
        }

    }, null);

}
/* END CONSTITUENT TYPE SYSTEM SETTINGS */


function LoadContactInformationSectionSettings() {

    LoadSectionSettings(SettingsCategories.CRM, 'Contact Information', 'sectionpreferences', SystemSettings.ContactInformation);

    var accordion = $('<div>').addClass('accordions');
    var addresstypes = $('<div>').addClass('addresstypescontainer');
    var contactcategories = $('<div>').addClass('contactcategoriescontainer');
    var contacttypes = $('<div>').addClass('contacttypescontainer');


    var header = $('<h1>').text('Address Types').appendTo($(accordion));
    $('<a>').attr('href', '#').addClass('newaddresstypemodallink modallink newbutton')
        .click(function (e) {
            e.preventDefault();

            modal = $('.addresstypemodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 250,
                resizable: false
            });

            $('.cancelmodal').click(function (e) {

                e.preventDefault();

                CloseModal(modal);

            });

            $('.submitaddrtype').unbind('click');

            $('.submitaddrtype').click(function () {

                var item = {
                    Code: $(modal).find('.addrtype-Code').val(),
                    Name: $(modal).find('.addrtype-Name').val(),
                    IsActive: $(modal).find('.addrtype-IsActive').prop('checked')
                }

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'addresstypes',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    headers: GetApiHeaders(),
                    success: function () {

                        DisplaySuccessMessage('Success', 'Address Type saved successfully.');

                        CloseModal(modal);

                        LoadAddressTypeSettingsGrid();

                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', 'An error occurred saving the Address Type.');
                    }
                });

            });
        })
        .appendTo($(header));
    $(addresstypes).appendTo($(accordion));

    LoadAddressTypeSettingsGrid();

    header = $('<h1>').text('Contact Categories').appendTo($(accordion));
    $('<a>').attr('href', '#').addClass('newcontactcategorymodallink modallink newbutton')
        .click(function (e) {
            e.preventDefault();

            modal = $('.contactcategorymodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 250,
                resizable: false
            });

            PopulateDropDown('.contcat-DefaultContactTypeId', 'contacttypes', '', '');

            $('.cancelmodal').click(function (e) {

                e.preventDefault();

                CloseModal(modal);

            });

            $('.submitcontcat').unbind('click');

            $('.submitcontcat').click(function () {

                var item = {
                    Code: $(modal).find('.contcat-Code').val(),
                    Name: $(modal).find('.contcat-Name').val(),
                    SectionTitle: $(modal).find('.contcat-SectionTitle').val(),
                    TextBoxLabel: $(modal).find('.contcat-TextBoxLabel').val(),
                    DefaultContactTypeId: $(modal).find('.contcat-DefaultContactTypeId').val(),
                    IsActive: $(modal).find('.contcat-IsActive').prop('checked')
                }

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'contactcategory',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    headers: GetApiHeaders(),
                    success: function () {

                        DisplaySuccessMessage('Success', 'Contact Category saved successfully.');

                        CloseModal(modal);

                        LoadContactCategorySettingsGrid();

                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', 'An error occurred while saving the Contact Category.');
                    }
                });

            });
        })
        .appendTo($(header));
    $(contactcategories).appendTo($(accordion));

    LoadContactCategorySettingsGrid();

    header = $('<h1>').text('Contact Types').appendTo($(accordion));
    $('<a>').attr('href', '#').addClass('newcontacttypemodallink modallink newbutton')
        .click(function (e) {
            e.preventDefault();

            modal = $('.contacttypemodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 250,
                resizable: false
            });

            PopulateDropDown('.conttype-ContactCategoryId', 'contactcategory', '', '');

            $('.cancelmodal').click(function (e) {

                e.preventDefault();

                CloseModal(modal);

            });

            $('.submitconttype').unbind('click');

            $('.submitconttype').click(function () {

                var item = {
                    Code: $(modal).find('.conttype-Code').val(),
                    Name: $(modal).find('.conttype-Name').val(),
                    ContactCategoryId: $(modal).find('.conttype-ContactCategoryId').val(),
                    IsAlwaysShown: $(modal).find('.conttype-IsAlwaysShown').prop('checked'),
                    CanDelete: $(modal).find('.conttype-CanDelete').prop('checked'),
                    IsActive: $(modal).find('.conttype-IsActive').prop('checked')
                }

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'contacttypes',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    headers: GetApiHeaders(),
                    success: function () {

                        DisplaySuccessMessage('Success', 'Contact Type saved successfully.');

                        CloseModal(modal);

                        LoadContactTypeSettingsGrid();

                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', 'An error occurred saving the Contact Type.');
                    }
                });

            });
        })
        .appendTo($(header));
    $(contacttypes).appendTo($(accordion));

    LoadContactTypeSettingsGrid();

    $(accordion).appendTo($('.gridcontainer'));

    LoadAccordions();


}

function LoadAddressTypeSettingsGrid() {

    var addresstypecolumns = [
       { dataField: 'Id', width: '0px' },
       { dataField: 'Code', caption: 'Code' },
       { dataField: 'Name', caption: 'Description' },
       { dataField: 'IsActive', caption: 'Active' }
    ];

    CustomLoadGrid('addresstypesgrid', 'addresstypescontainer', addresstypecolumns, 'addresstypes?fields=all', null, EditAddressType, DeleteAddressType);

}

function LoadContactCategorySettingsGrid() {

    var contactcategorycolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'SectionTitle', caption: 'Section Title' },
        { dataField: 'TextBoxLabel', caption: 'Text Box Label' },
        { dataField: 'DefaultContactType.DisplayName', caption: 'Default Contact Type' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    CustomLoadGrid('contactcategoriesgrid', 'contactcategoriescontainer', contactcategorycolumns, 'contactcategory?fields=all', null, EditContactCategory, DeleteContactCategory);
}

function LoadContactTypeSettingsGrid() {

    var contacttypecolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'ContactCategory.DisplayName', caption: 'Contact Category' },
        { dataField: 'IsAlwaysShown', caption: 'Show Always' },
        { dataField: 'CanDelete', caption: 'Can Delete' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    CustomLoadGrid('contacttypesgrid', 'contacttypescontainer', contacttypecolumns, 'contacttypes?fields=all', null, EditContactType, DeleteContactType);

}

/* ADDRESS TYPE SYSTEM SETTINGS */
function EditAddressType(id) {

    LoadAddressType(id);

    modal = $('.addresstypemodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });


    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitaddrtype').unbind('click');

    $('.submitaddrtype').click(function () {

        var item = {
            Code: $(modal).find('.addrtype-Code').val(),
            Name: $(modal).find('.addrtype-Name').val(),
            IsActive: $(modal).find('.addrtype-IsActive').prop('checked')
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'addresstypes/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            headers: GetApiHeaders(),
            success: function () {

                DisplaySuccessMessage('Success', 'Address Type saved successfully.');

                CloseModal(modal);

                LoadAddressTypeSettingsGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred saving the Address Type.');
            }
        });

    });

}

function DeleteAddressType(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'addresstypes/' + id,
        method: 'DELETE',
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        headers: GetApiHeaders(),
        success: function () {

            DisplaySuccessMessage('Success', 'Address Type deleted successfully.');

            LoadAddressTypeSettingsGrid();

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred deleting the Address Type.');
        }

    });


}

function LoadAddressType(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'addresstypes/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        headers: GetApiHeaders(),
        success: function (data) {

            if (data && data.Data && data.IsSuccessful) {

                $(modal).find('.addrtype-Id').val(data.Data.Id);
                $(modal).find('.addrtype-Code').val(data.Data.Code);
                $(modal).find('.addrtype-Name').val(data.Data.Name);
                $(modal).find('.addrtype-IsActive').prop('checked', data.Data.IsActive);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error loading Address Type.');
        }
    });

}
/* END ADDRESS TYPE SYSTEM SETTINGS */


/* CONTACT CATEGORY SYSTEM SETTINGS */
function EditContactCategory(id) {

    LoadContactCategory(id);

    modal = $('.contactcategorymodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitcontcat').unbind('click');

    $('.submitcontcat').click(function () {

        var item = {
            Code: $(modal).find('.contcat-Code').val(),
            Name: $(modal).find('.contcat-Name').val(),
            SectionTitle: $(modal).find('.contcat-SectionTitle').val(),
            TextBoxLabel: $(modal).find('.contcat-TextBoxLabel').val(),
            DefaultContactTypeId: $(modal).find('.contcat-DefaultContactTypeId').val(),
            IsActive: $(modal).find('.contcat-IsActive').prop('checked')
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'contactcategory/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            headers: GetApiHeaders(),
            success: function () {

                DisplaySuccessMessage('Success', 'Contact Category saved successfully.');

                CloseModal(modal);

                LoadContactCategorySettingsGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred saving the Contact Category.');
            }
        });

    });

}

function DeleteContactCategory(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'contactcategory/' + id,
        method: 'DELETE',
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        headers: GetApiHeaders(),
        success: function () {

            DisplaySuccessMessage('Success', 'Contact Category deleted successfully.');

            LoadContactCategorySettingsGrid();

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred deleting the Contact Category.');
        }

    });


}

function LoadContactCategory(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'contactcategory/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        headers: GetApiHeaders(),
        success: function (data) {

            if (data && data.Data && data.IsSuccessful) {

                $(modal).find('.contcat-Id').val(data.Data.Id);
                $(modal).find('.contcat-Code').val(data.Data.Code);
                $(modal).find('.contcat-Name').val(data.Data.Name);
                $(modal).find('.contcat-SectionTitle').val(data.Data.SectionTitle);
                $(modal).find('.contcat-TextBoxLabel').val(data.Data.TextBoxLabel);
                $(modal).find('.contcat-DefaultContactTypeId').val(data.Data.DefaultContactTypeId);
                $(modal).find('.contcat-IsActive').prop('checked', data.Data.IsActive);

                PopulateDropDown('.contcat-DefaultContactTypeId', 'contacttypes', '', '', data.Data.DefaultContactTypeId);


            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error loading Contact Category.');
        }
    });

}
/* END CONTACT CATEGORY SYSTEM SETTINGS */

/* CONTACT TYPE SYSTEM SETTINGS */
function EditContactType(id) {

    LoadContactType(id);

    modal = $('.contacttypemodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitconttype').unbind('click');

    $('.submitconttype').click(function () {

        var item = {
            Code: $(modal).find('.conttype-Code').val(),
            Name: $(modal).find('.conttype-Name').val(),
            ContactCategoryId: $(modal).find('.conttype-ContactCategoryId').val(),
            IsAlwaysShown: $(modal).find('.conttype-IsAlwaysShown').prop('checked'),
            CanDelete: $(modal).find('.conttype-CanDelete').prop('checked'),
            IsActive: $(modal).find('.conttype-IsActive').prop('checked')
        }

        $.ajax({
            method: 'PATCH',
            url: WEB_API_ADDRESS + 'contacttypes/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            headers: GetApiHeaders(),
            success: function () {

                DisplaySuccessMessage('Success', 'Contact Type saved successfully.');

                CloseModal(modal);

                LoadContactTypeSettingsGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred saving the Contact Type.');
            }
        });

    });

}

function DeleteContactType(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'contacttypes/' + id,
        method: 'DELETE',
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        headers: GetApiHeaders(),
        success: function () {

            DisplaySuccessMessage('Success', 'Contact Type deleted successfully.');

            LoadContactTypeSettingsGrid();

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred deleting the Contact Type.');
        }

    });


}

function LoadContactType(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'contacttypes/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        headers: GetApiHeaders(),
        success: function (data) {

            if (data && data.Data && data.IsSuccessful) {

                $(modal).find('.conttype-Id').val(data.Data.Id);
                $(modal).find('.conttype-Code').val(data.Data.Code);
                $(modal).find('.conttype-Name').val(data.Data.Name);
                $(modal).find('.conttype-ContactCategoryId').val(data.Data.ContactCategoryId);
                $(modal).find('.conttype-IsAlwaysShown').prop('checked', data.Data.IsAlwaysShown);
                $(modal).find('.conttype-CanDelete').prop('checked', data.Data.CanDelete);
                $(modal).find('.conttype-IsActive').prop('checked', data.Data.IsActive);

                PopulateDropDown('.conttype-ContactCategoryId', 'contactcategory', '', '', data.Data.ContactCategoryId);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error loading Contact Type.');
        }
    });

}
/* END CONTACT TYPE SYSTEM SETTINGS */

/* DEMOGRAPHICS SYSTEM SETTINGS */
function LoadDemographicsSectionSettings() {

    LoadSectionSettings(SettingsCategories.CRM, 'Demographics', 'sectionpreferences', SystemSettings.Demographics);

    var accordion = $('<div>').addClass('accordions');
    var denomination = $('<div>').addClass('denominationscontainer');
    var ethnicity = $('<div>').addClass('ethnicitiescontainer');
    var language = $('<div>').addClass('languagescontainer');


    var header = $('<h1>').text('Denominations').appendTo($(accordion));
    $(denomination).appendTo($(accordion));
    LoadDenominationSettingsGrid();

    header = $('<h1>').text('Ethnicities').appendTo($(accordion));
    $(ethnicity).appendTo($(accordion));
    LoadEthnicitySettingsGrid();

    header = $('<h1>').text('Languages').appendTo($(accordion));
    $(language).appendTo($(accordion));
    LoadLanguageSettingsGrid();

    $(accordion).appendTo($('.gridcontainer'));

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
    
    LoadGrid('.denominationscontainer', 'denominationsgrid', denominationcolumns, 'denominations?fields=all', 'denominations', null, 'den-',
        '.denominationmodal', '.denominationmodal', 250, true, false, false, null);

}

function LoadEthnicitySettingsGrid() {

    var ethnicitycolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Ethnicity' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('.ethnicitiescontainer', 'ethnicitiesgrid', ethnicitycolumns, 'ethnicities?fields=all', 'ethnicities', null, 'eth-',
        '.ethnicitymodal', '.ethnicitymodal', 250, true, false, false, null);

}

function LoadLanguageSettingsGrid() {

    var languagecolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Language' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('.languagescontainer', 'languagesgrid', languagecolumns, 'languages?fields=all', 'languages', null, 'lang-',
        '.languagemodal', '.languagemodal', 250, true, false, false, null);

}
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
    $(degrees).appendTo($(accordion));
    LoadDegreeSettingsGrid();
        
    header = $('<h1>').text('Education Level').appendTo($(accordion));
    $(educationlevels).appendTo($(accordion));
    LoadEducationLevelSettingsGrid();

    header = $('<h1>').text('Schools').appendTo($(accordion));
    $(schools).appendTo($(accordion));
    LoadSchoolsSettingsGrid();

    $(accordion).appendTo($('.gridcontainer'));

    LoadAccordions();
}

function LoadDegreeSettingsGrid() {

    var degreecolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('.degreecontainer', 'degreesgrid', degreecolumns, 'degrees?fields=all', 'degrees', null, 'deg-',
    '.degreemodal', '.degreemodal', 250, true, false, false, null);

}

function LoadEducationLevelSettingsGrid() {

    var educationLevelcolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('.educationlevelscontainer', 'educationlevelsgrid', educationLevelcolumns, 'educationlevels?fields=all', 'educationlevels', null, 'eduLev-',
    '.educationLevelmodal', '.educationLevelmodal', 250, true, false, false, null);

}

function LoadSchoolsSettingsGrid() {

    var schoolcolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('.schoolscontainer', 'schoolsgrid', schoolcolumns, 'schools?fields=all', 'schools', null, 'sch-',
    '.schoolmodal', '.schoolmodal', 250, true, false, false, null);

}
/* END EDUCATION SYSTEM SETTINGS */

function LoadGenderSectionSettings() {

    var columns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Gender' },
        { dataField: 'IsMasculine', caption: 'Masculine' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('.gridcontainer', 'gendergridcontainer', columns, 'genders?fields=all', 'genders', null, 'gen-',
    '.gendermodal', '.gendermodal', 250, true, false, false, null);
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

function LoadPrefixSectionSettings() {

    var prefixcolumns = [
       { dataField: 'Id', width: '0px' },
       { dataField: 'Code', caption: 'Code' },
       { dataField: 'Name', caption: 'Description' },
       { dataField: 'Salutation', caption: 'Salutation Prefix' },
       { dataField: 'LabelPrefix', caption: 'Label Prefix' },
       { dataField: 'LabelAbbreviation', caption: 'Label Prefix Short' }
    ];

    LoadGrid('.gridcontainer', 'prefixgrid', prefixcolumns, 'prefixes?fields=all', 'prefixes', null, 'prefix-',
        '.prefixmodal', '.prefixmodal', 250, true, false, false, null);
}

/* PROFESSIONAL SYSTEM SETTINGS */
function LoadProfessionalSectionSettings() {
    LoadSectionSettings(SettingsCategories.CRM, 'Professional', 'sectionpreferences', SystemSettings.Professional);

    var accordion = $('<div>').addClass('accordions');
    var incomeLevels = $('<div>').addClass('incomeLevelcontainer');
    var professions = $('<div>').addClass('professioncontainer');

    var header = $('<h1>').text('Income Level').appendTo($(accordion));
    $(incomeLevels).appendTo($(accordion));
    LoadIncomeLevelSettingsGrid();

    header = $('<h1>').text('Professions').appendTo($(accordion));
    $(professions).appendTo($(accordion));
    LoadProfessionSettingsGrid();

    $(accordion).appendTo($('.gridcontainer'));

    LoadAccordions();
}

function LoadIncomeLevelSettingsGrid() {

    var incomeLevelcolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('.incomeLevelcontainer', 'incomeLevelgrid', incomeLevelcolumns, 'incomelevels?fields=all', 'incomelevels', null, 'inc-',
        '.incomeLevelmodal', '.incomeLevelmodal', 250, true, false, false, null);

}

function LoadProfessionSettingsGrid() {

    var professioncolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('.professioncontainer', 'professiongrid', professioncolumns, 'professions?fields=all', 'professions', null, 'pro-',
        '.professionmodal', '.professionmodal', 250, true, false, false, null);
}
/* END PROFESSIONAL SYSTEM SETTINGS */

/* REGIONS SETTINGS */
function LoadRegionsSectionSettings() {

    $('.gridcontainer').empty();
    var lc = $('<div>').addClass('regionlevelcontainer');
    var rc = $('<div>').addClass('regioncontainer');
    var rgc = $('<div>').addClass('regiongridcontainer');
    $(lc).appendTo($('.gridcontainer'));
    $(rc).appendTo($('.gridcontainer'));
    $(rgc).appendTo($(rc));

    CreateRegionLevelSelector(lc);

}

function CreateRegionLevelSelector(container) {

    var ul = $('<ul>').addClass('regionlevellist').appendTo($(container));
    DisplayRegionLevels(ul);

}

function DisplayRegionLevels(container) {

    MakeServiceCall('GET', 'regionlevels/', null, function (data) {

        if (data.Data) {
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

                            MakeServiceCall('PATCH', 'regionlevels/' + id, item, function (data) {

                                if (data.Data) {
                                    DisplaySuccessMessage('Success', 'Region Level saved successfully.');

                                    CloseModal(modal);

                                    LoadRegionsSectionSettings();
                                }

                            }, null);

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

                                MakeServiceCall('POST', 'regionlevels', item, function (data) {

                                    if (data.Data) {
                                        DisplaySuccessMessage('Success', 'Region Level saved successfully.');

                                        CloseModal(modal);

                                        LoadRegionsSectionSettings();
                                    }

                                }, null);

                            });
                        });

                    $(li).appendTo($(container));
                }

            }
        }

    }, null);

    MakeServiceCall('GET', 'regionlevels/', null, function (data) {

        if (data.Data) {
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

                    $('<a>').attr('href', '#').addClass('deleteregionlevellink').click(function (e) {
                            e.preventDefault();
                        
                            ConfirmModal('Are you sure you want to delete this Region Level?', function () {
                          
                            DeleteRegionLevel(level.Id);

                        }, function () {
                           
                        });

                    }).appendTo($(li));

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

                            MakeServiceCall('PATCH', 'regionlevels/' + id, item, function (data) {

                                if (data.Data) {
                                    DisplaySuccessMessage('Success', 'Region Level saved successfully.');

                                    CloseModal(modal);

                                    LoadRegionsSectionSettings();
                                }

                            }, null);

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

                                MakeServiceCall('POST', 'regionlevels', item, function (data) {

                                    if (data.Data) {
                                        DisplaySuccessMessage('Success', 'Region Level saved successfully.');

                                        CloseModal(modal);

                                        LoadRegionsSectionSettings();
                                    }

                                }, null);

                            });
                        });

                    $(li).appendTo($(container));
                }

            }

        }

    }, null);

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

    CustomLoadGrid('regiongrid', 'regiongridcontainer', columns, route, null, EditRegion, DeleteRegion, function () {
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

        MakeServiceCall('POST', 'regions', item, function (data) {

            if (data.Data) {
                DisplaySuccessMessage('Success', 'Region saved successfully.');

                CloseModal(modal);

                DisplayRegions(data.Data.Level, data.Data.ParentRegionId);
            }

        }, null);

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

        MakeServiceCall('PATCH', 'regions/' + id, item, function (data) {

            if (data.Data) {
                DisplaySuccessMessage('Success', 'Region saved successfully.');

                CloseModal(modal);

                DisplayRegions(data.Data.Level, data.Data.ParentRegionId);

            }

        }, null);

    });

}

function DeleteRegion(id) {

    MakeServiceCall('DELETE', 'regions/' + id, null, function (data) {

        if (data.Data) {
            DisplaySuccessMessage('Success', 'Region deleted successfully.');

            CloseModal(modal);

            DisplayRegions($('.currentlevel').val(), $('.parentregions').val());
        }

    }, null);

}
function DeleteRegionLevel(id) {

 
    $.ajax({
        url: WEB_API_ADDRESS + 'regionlevels/' + id,
        method: 'DELETE',
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {
            DisplaySuccessMessage('Success', 'Region Level deleted successfully.');

            CloseModal(modal);

            LoadRegionsSectionSettings();
        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}


function LoadRegionLevel(id) {

    MakeServiceCall('GET', 'regionlevels/' + id, null, function (data) {

        if (data && data.Data && data.IsSuccessful) {

            $(modal).find('.rl-Level').val(data.Data.Level);
            $(modal).find('.rl-Label').val(data.Data.Label);
            $(modal).find('.rl-Abbreviation').val(data.Data.Abbreviation);
            $(modal).find('.rl-IsRequired').prop('checked', data.Data.IsRequired);
            $(modal).find('.rl-IsChildLevel').prop('checked', data.Data.IsChildLevel);

        }

    }, null);
}

function LoadRegion(id) {

    MakeServiceCall('GET', 'regions/' + id, null, function (data) {

        if (data && data.Data && data.IsSuccessful) {

            $(modal).find('.regionid').val(id);
            $(modal).find('.parentregionid').val(data.Data.ParentRegionId);
            $(modal).find('.currentlevel').val(data.Data.Level);

            $(modal).find('.reg-Code').val(data.Data.Code);
            $(modal).find('.reg-Name').val(data.Data.Name);
            $(modal).find('.reg-IsActive').prop('checked', data.Data.IsActive);

        }

    }, null);

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
                        DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
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
                        DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
                    }
                });

            });
        })
        .appendTo($(header));
    $(type).appendTo($(accordion));
    LoadRelationshipTypeSettingsGrid();

    $(accordion).appendTo($('.gridcontainer'));

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

    LoadGrid('.relationshipcategorycontainer', 'relationshipcategorygrid', relationshipcategorycolumns, 'relationshipcategories?fields=all', 'relationshipcategories', null, 'relcat-',
        '.relcatmodal', '.relcatmodal', 250, true, false, false, null);

}

function LoadRelationshipTypeSettingsGrid() {

    var relationshiptypecolumns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'ReciprocalTypeMale.DisplayName', caption: 'Male Reciprocal' },
        { dataField: 'ReciprocalTypeFemale.DisplayName', caption: 'Female Reciprocal' },
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
        { dataField: 'RelationshipCategory.DisplayName', caption: 'Relationship Category' },
        { dataField: 'IsSpouse', caption: 'Spouse' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('.relationshiptypecontainer', 'relationshiptypegrid', relationshiptypecolumns, 'relationshiptypes?fields=all', 'relationshiptypes', null, 'reltype-',
        '.reltypemodal', '.reltypemodal', 250, true, false, false, null);
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
                DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
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
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
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

    CustomLoadGrid('taggroupsgrid',
        'gridcontainer',
        columns,
        'taggroups?fields=all',
        TagGroupSelected,
        EditTagGroup,
        null,
        function () {
            CreateNewModalLink("New Tag Group", NewTagGroupModal, '.taggroupsgrid', '.gridcontainer', 'newtaggroupmodal')
        });

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
        { dataField: 'IsActive', caption: 'Active' }
    ];

    CustomLoadGrid('tagsgrid',
        'tagscontainer',
        columns,
        'taggroups/' + selectedRow.Id + '/tags',
        TagGroupSelected,
        EditTag,
        null,
        function () {
            CreateNewModalLink("New Tag", NewTagModal, '.tagsgrid', '.tagscontainer', 'newtagmodal')
        });

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

    var modallink = $('<a>').attr('href', '#').addClass('newmodallink').addClass(modalLinkClass).text(linkText);

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

    $('.contentcontainer').empty();

    var acctsettingscontainer = $('<div>').addClass('accountingsettingscontainer onecolumn').appendTo($('.contentcontainer'));

    $('<input>').attr('type', 'hidden').addClass('hidLedgerId').appendTo(acctsettingscontainer);

    // Select the ledger
    CreateBasicFieldBlock('Ledger: ', '<select>', 'as-ledgerselect', acctsettingscontainer); 

    PopulateDropDown('.as-ledgerselect', 'ledgers/businessunit/' + currentBusinessUnit.Id, '', '', '', function () {
        
        $('.hidLedgerId').val($('.as-ledgerselect').val());
        LoadAccountingSettings($('.hidLedgerId').val());

    }, null);

    // fiscal year
    CreateBasicFieldBlock('Fiscal Year: ', '<select>', 'as-fiscalyear', acctsettingscontainer);

    // transaction posted automatially
    CreateBasicFieldBlock('Post transactions automatically when saved or approved: ', '<input type="checkbox">', 'as-postedtransaction', acctsettingscontainer);

    // how many days in advane recurring journals will be processed
    CreateBasicFieldBlock('Number of days before recurring journals post:', '<input type="text">', 'as-daysinadvance', acctsettingscontainer);

    // disable or enable approvals for journals
    CreateBasicFieldBlock('Enable Approvals:', '<input type="checkbox">', 'as-approval', acctsettingscontainer);

    // if approval is enabled: load users and select who can approve
    $('.as-approval').change(function () {

        if (this.checked) {

            CreateBasicFieldBlock('Approved Users:', '<select>', 'as-approvedusers', $('.as-approval').parent());
            // PopulateDropDown(); // users with permissions to post approvals

        } else {

            $('.as-approvedusers').parent().remove();

        }
        
    });

    CreateSaveAndCancelButtons('saveAccountingSettings', function (e) {

        e.preventDefault();

        var data = {

            Id: $('.as-ledgerselect').val(),
            DefaultFiscalYearId: $('.as-fiscalyear').val(),
            PostAutomatically: $('.as-postedtransaction').prop('checked'),
            PostDaysInAdvance: $('.as-daysinadvance').val(), 
            ApproveJournals: $('.as-approval').prop('checked')

        };

        MakeServiceCall('PATCH', 'ledgers/' + $('.hidLedgerId').val(), JSON.stringify(data), function () {

            LoadAccountingSettings($('.hidLedgerId').val());
            DisplaySuccessMessage('Success', 'Accounting settings saved successfully.');

        }, null); 


    },'cancel', function (e) {

        e.preventDefault();

        LoadAccountingSettings($('.hidLedgerId').val());

    },

    acctsettingscontainer);
}

function LoadAccountingSettings(id) {

    MakeServiceCall('GET', 'ledgers/' + id, null, function (data) { 

        $('.hidLedgerId').val(data.Data.Id);
        $('.as-postedtransaction').prop('checked', data.Data.PostAutomatically);
        $('.as-daysinadvance').val(data.Data.PostDaysInAdvance); 
        $('.as-approval').prop('checked', data.Data.ApproveJournals);

        PopulateDropDown('.as-fiscalyear', 'fiscalyears/ledger/' + $('.hidLedgerId').val(), '', '', data.Data.DefaultFiscalYearId, null);

        if (data.Data.ApproveJournals && !($('.as-approvedusers').length > 0)) {

            CreateBasicFieldBlock('Approved Users:', '<select>', 'as-approvedusers', $('.as-approval').parent());
            // PopulateDropDown(); // users with permissions to post approvals

        } 

    }, null);

}

function PickLedger() {



}

function LoadBudgetSectionSettings() {

    $('.contentcontainer').empty();

    var businessunitid = currentBusinessUnit.Id;
    var ledgerid;

    var container = $('<div>').addClass('budgetsettingscontainer onecolumn');

    var ledgernamegroup = $('<div>');
    $('<label>').text('Ledger: ').appendTo(ledgernamegroup);
    var ledgernamedisplay = $('<label>').addClass('ledgernamedisplay').appendTo(ledgernamegroup);
    $('<hr>').addClass('').appendTo(ledgernamegroup);
    $(ledgernamegroup).append('<br />').appendTo(container);

    var headinggroup = $('<div>');
    $('<label>').text('Settings for Organizational Ledger').addClass('pageheading').appendTo(headinggroup);
    $(headinggroup).append('<br />').append('<br />').appendTo(container);

    var selectledgergroup = $('<div>'); 
    $('<label>').text('Select Ledger: ').appendTo(selectledgergroup);
    var selectledgername = $('<select>').addClass('budgetLedgerId').appendTo(selectledgergroup);
    $(selectledgergroup).append('<br />').append('<br />').appendTo(container);

    PopulateDropDown('.budgetLedgerId', 'ledgers/businessunit/' + businessunitid, '', '', '', function () {
        //update on change  (not working so added .change logic below
        //GetBudgetSetting();
    }, function () {
        //retrieve initial value on populate complete
        GetBudgetSetting();
    });

    selectledgername.change(function () {
        GetBudgetSetting();
    });

    var workingbudgetgroup = $('<div>').addClass('fieldblock');
    $('<label>').text('Name of working budget: ').appendTo(workingbudgetgroup);
    var workingbudgetname = $('<input>').attr({ type: 'text', maxLength: '40' }).addClass('workingBudgetName required').appendTo(workingbudgetgroup);
    $(workingbudgetgroup).appendTo(container);

    var fixedbudgetgroup = $('<div>').addClass('fieldblock');
    $('<label>').text('Name of fixed budget: ').appendTo(fixedbudgetgroup);
    var fixedbudgetname = $('<input>').attr({ type: 'text', maxLength: '40' }).addClass('fixedBudgetName required').appendTo(fixedbudgetgroup);
    $(fixedbudgetgroup).appendTo(container);

    var whatifbudgetgroup = $('<div>').addClass('fieldblock');
    $('<label>').text('Name of "what if" budget: ').appendTo(whatifbudgetgroup);
    var whatifbudgetname = $('<input>').attr({ type: 'text', maxLength: '40' }).addClass('whatifBudgetName required').appendTo(whatifbudgetgroup);
    $(whatifbudgetgroup).append('<br />').appendTo(container);

    var errorgroup = $('<div>').addClass('fieldblock');
    $('<label>').text('').addClass('validateerror budgetsettingerror').append('<br />').appendTo(errorgroup);
    $(errorgroup).append('<br />').appendTo(container);

    var id = $('<input>').attr('type', 'hidden').addClass('hidLedgerId').appendTo(container);

    var controlContainer = $('<div>').addClass('controlContainer');

    $('<input>').attr('type', 'button').addClass('saveEntity').val('Save')
        .click(function () {
            if (ValidBudgetSettingForm() === true) {
                SaveBudgetSetting(id);
            }
        })
        .appendTo(controlContainer);

    $('<a>').addClass('cancel').text('Cancel').attr('href', '#')
        .click(function (e) {
            e.preventDefault();
            $('.budgetsettingerror').text('');
            RemoveValidation('budgetsettingscontainer')

            GetBudgetSetting();
        })
        .appendTo(controlContainer);

    $(controlContainer).appendTo(container);

    $(container).appendTo($('.contentcontainer'));

    InitRequiredLabels('budgetsettingscontainer')

}

function ValidBudgetSettingForm() {
    var validform = true;

    // required items
    if (ValidateForm('budgetsettingscontainer') === false) {
        return false;
    }

    return validform;
}

function GetBudgetSetting() {
    var ledgerid = $('.budgetLedgerId').val();

    MakeServiceCall('GET', 'ledgers/' + ledgerid, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {

                $('.hidLedgerId').val(data.Data.Id);
                $('.ledgernamedisplay').text(data.Data.Name)
                $('.workingBudgetName').val(data.Data.WorkingBudgetName)
                $('.fixedBudgetName').val(data.Data.FixedBudgetName)
                $('.whatifBudgetName').val(data.Data.WhatIfBudgetName)

            }
        }

    }, null);
}

function LoadChartAccountsSectionSettings() {

    $('.contentcontainer').empty();

    var ledgerid;

    var container = $('<div>').addClass('chartsettingscontainer onecolumn');

    var ledgernamegroup = $('<div>').addClass('');
    $('<label>').text('Ledger: ').appendTo(ledgernamegroup);
    ledgernamedisplay = $('<label>').addClass('ledgernamedisplay').appendTo(ledgernamegroup);
    $('<hr>').appendTo(ledgernamegroup);
    $(ledgernamegroup).append('<br />').appendTo(container);

    var headinggroup = $('<div>');
    $('<label>').text('Settings for Organizational Ledger').addClass('pageheading').appendTo(headinggroup);
    $(headinggroup).append('<br />').append('<br />').appendTo(container);

    var selectledgergroup = $('<div>');
    $('<label>').text('Select Ledger: ').appendTo(selectledgergroup);
    var selectledgername = $('<select>').addClass('chartLedgerId').appendTo(selectledgergroup);
    $(selectledgergroup).append('<br />').append('<br />').appendTo(container);

    PopulateDropDown('.chartLedgerId', 'ledgers/businessunit/' + currentBusinessUnit.Id, '', '', '', function () {
        //update on change  (not working so added .change logic below
        //GetChartSetting();
    }, function () {
        //retrieve initial value on populate complete
        GetChartSetting();
    });

    selectledgername.change(function () {
        GetChartSetting();
    });

    var capitalizeheadersgroup = $('<div>').addClass('fieldblock');
    var capitalizeheaderscheckbox = $('<input>').attr('type', 'checkbox').addClass('capitalizeheaders').appendTo(capitalizeheadersgroup);
    $('<span>').text('Capitalize account group descriptions').appendTo(capitalizeheadersgroup);
    $(capitalizeheadersgroup).append('<br />').appendTo(container);

    var grouplevelsgroup = $('<div>');
    $('<label>').text('Number of account groups: ').appendTo(grouplevelsgroup);
    var grouplevels = $('<select>').addClass('groupLevels').appendTo(grouplevelsgroup).change(function () {
        GroupLevelsChange();
    });
    grouplevels.append('<option value="1">1</option>');
    grouplevels.append('<option value="2">2</option>');
    grouplevels.append('<option value="3">3</option>');
    grouplevels.append('<option value="4">4</option>');
    grouplevels.appendTo(grouplevelsgroup);
    $(grouplevelsgroup).append('<br />').append('<br />').appendTo(container);

    var group1 = $('<div>').addClass('fieldblock AccountGroup1group');
    $('<label>').text('Account Group 1: ').appendTo(group1);
    $('<input>').attr({ type: 'text', maxLength: '40' }).addClass('accountGroup1Title').appendTo(group1);
    $(group1).appendTo(container);

    var group2 = $('<div>').addClass('fieldblock AccountGroup2group');
    $('<label>').text('Account Group 2: ').appendTo(group2);
    $('<input>').attr({ type: 'text', maxLength: '40' }).addClass('accountGroup2Title').appendTo(group2);
    $(group2).hide().appendTo(container);

    var group3 = $('<div>').addClass('fieldblock AccountGroup3group');
    $('<label>').text('Account Group 3: ').appendTo(group3);
    $('<input>').attr({ type: 'text', maxLength: '40' }).addClass('accountGroup3Title').appendTo(group3);
    $(group3).hide().appendTo(container);

    var group4 = $('<div>').addClass('fieldblock AccountGroup4group');
    $('<label>').text('Account Group 4: ').appendTo(group4);
    $('<input>').attr({ type: 'text', maxLength: '40' }).addClass('accountGroup4Title').appendTo(group4);
    $(group4).hide().appendTo(container);

    var errorgroup = $('<div>').addClass('fieldblock');
    $('<label>').text('').addClass('validateerror chartsettingerror').append('<br />').appendTo(errorgroup);
    $(errorgroup).append('<br />').appendTo(container);

    var id = $('<input>').attr('type', 'hidden').addClass('hidLedgerId').appendTo(container);

    var controlContainer = $('<div>').addClass('controlContainer');

    $('<input>').attr('type', 'button').addClass('saveEntity').val('Save')
        .click(function () {
            if (ValidChartSettingForm() === true) {
                SaveChartSetting(id);
            }
        })
        .appendTo(controlContainer);

    $('<a>').addClass('cancel').text('Cancel').attr('href', '#')
        .click(function (e) {
            e.preventDefault();
            $('.chartsettingerror').text('');
            RemoveValidation('chartsettingscontainer')

            GetChartSetting();
        })
        .appendTo(controlContainer);

    $(controlContainer).appendTo(container);

    $(container).appendTo($('.contentcontainer'));

    InitRequiredLabels("chartsettingscontainer")
}

function SaveBudgetSetting(id) {

    var data = {
        Id: $(id).val(),
        WorkingBudgetName: $('.workingBudgetName').val(),
        FixedBudgetName: $('.fixedBudgetName').val(),
        WhatIfBudgetName: $('.whatifBudgetName').val(),
    }

    MakeServiceCall('PATCH', 'ledgers/' + $(id).val(), JSON.stringify(data), function (data) {

        if (data.Data) {
            DisplaySuccessMessage('Success', 'Budget Settings saved successfully.');
        }

    }, null);
}


function ValidChartSettingForm() {
    var validform = true;

    // required items
    if (ValidateForm('budgetsettingscontainer') === false) {
        return false;
    }

    return validform;
}

function GetChartSetting() {

    var ledgerid = $('.chartLedgerId').val();

    MakeServiceCall('GET', 'ledgers/' + ledgerid, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {

                $('.hidLedgerId').val(data.Data.Id);
                $('.ledgernamedisplay').text(data.Data.Name)
                $('.capitalizeheaders').prop('checked', data.Data.CapitalizeHeaders);
                $('.groupLevels').val(data.Data.AccountGroupLevels);
                $('.accountGroup1Title').val(data.Data.AccountGroup1Title)
                $('.accountGroup2Title').val(data.Data.AccountGroup2Title)
                $('.accountGroup3Title').val(data.Data.AccountGroup3Title)
                $('.accountGroup4Title').val(data.Data.AccountGroup4Title)
                GroupLevelsChange();

            }
        }

    }, null);
}

function SaveChartSetting(id) {

    var data = {
        Id: $(id).val(),
        CapitalizeHeaders: $('.capitalizeheaders').prop('checked'),
        AccountGroupLevels: $('.groupLevels').val(),
        AccountGroup1Title: $('.accountGroup1Title').val(),
        AccountGroup2Title: $('.accountGroup2Title').val(),
        AccountGroup3Title: $('.accountGroup3Title').val(),
        AccountGroup4Title: $('.accountGroup4Title').val(),
    }

    MakeServiceCall('PATCH', 'ledgers/' + $(id).val(), JSON.stringify(data), function (data) {

        if (data.Data) {
            DisplaySuccessMessage('Success', 'Chart of Accounts Settings saved successfully.');
        }

    }, null);
}

function GroupLevelsChange() {
    var groupLevels = $('.groupLevels').val();
    switch (groupLevels) {
        case '1':
            $('.AccountGroup2group').hide();
            $('.AccountGroup3group').hide();
            $('.AccountGroup4group').hide();
            break;
        case '2':
            $('.AccountGroup2group').show();
            $('.AccountGroup3group').hide();
            $('.AccountGroup4group').hide();
            break;
        case '3':
            $('.AccountGroup2group').show();
            $('.AccountGroup3group').show();
            $('.AccountGroup4group').hide();
            break;
        case '4':
            $('.AccountGroup2group').show();
            $('.AccountGroup3group').show();
            $('.AccountGroup4group').show();
            break;
    }
}


function LoadEntitiesSectionSettings() {


}
function LoadChartAccountsSettingsSectionSettings() {



}

/// Entities/BusinessUnits Settings
function LoadEntitiesSectionSettings() {

    $('.contentcontainer').empty();

    var entityColumns = [
      { dataField: 'Id', width: '0px' },
      { dataField: 'Code', caption: 'Code' },
      { dataField: 'Name', caption: 'Description' },
      {
          caption: 'Entity Type', cellTemplate: function (container, options) {
              var entity = 'None';

              switch (options.data.BusinessUnitType) {
                  case 0:
                      entity = "Organization";
                      break;
                  case 1:
                      entity = "Common";
                      break;
                  case 2:
                      entity = "Separate";
                      break;
              }

              $('<label>').text(entity).appendTo(container);
          }
      },
      {
          caption: '', cellTemplate: function (container, options) {
              

          }
      }
    ];

    LoadGrid('.contentcontainer', 'gridcontainer', entityColumns, 'businessunits', 'businessunits', null, 'en-',
        '.entitymodal', '.entitymodal', 250, true, false, false, null);

}
/// End Entities/Business Untis Settings

function LoadFiscalYearSectionSettings() {

    $('.contentcontainer').empty();
    $('.fiscalyearcontainer').remove();
    
    var container = $('<div>'); 
    var selectledgergroup = $('<div>').addClass('twocolumn');   
    var selectledgername = $('<h1>').text('Fiscal Year Settings for Ledger: ');
    $('<select>').addClass('LedgerId').appendTo(selectledgername);
    $(selectledgername).appendTo(selectledgergroup);
    $(selectledgergroup).appendTo(container);

    var gridgroup = $('<div>').addClass('twocolumn');

    var fycontainer = $('<div>').addClass('fiscalyearcontainer');
    $('.gridcontainer').append($(fycontainer));
    $('<h2>').text('Fiscal Years').appendTo($(fycontainer));

    $('.fiscalperiodscontainer').remove();
    var fpcontainer = $('<div>').addClass('fiscalperiodscontainer');
    $('.gridcontainer').append($(fpcontainer));
    $('<h2>').text('Fiscal Periods').appendTo($(fpcontainer));

    $(fycontainer).appendTo(gridgroup);
    $(fpcontainer).appendTo(gridgroup);

    $(gridgroup).appendTo(container);
    $(container).appendTo($('.contentcontainer'));

    PopulateDropDown('.LedgerId', 'ledgers/businessunit/' + currentBusinessUnit.Id, '', '', $('.LedgerId').val(), function () {

        var ledgerid = $('.LedgerId').val();

        var columns = [
            { dataField: 'Id', width: "0px" },
            { dataField: 'Name', caption: 'Name' },
            { caption: 'Status', cellTemplate: function (container, options) {

                    var status;

                    switch (options.data.Status) {
                        case 0:
                            status = "Empty";
                            break;
                        case 1:
                            status = "Open";
                            break;
                        case 2:
                            status = "Closed";
                            break;
                        case 3:
                            status = "Reopened";
                            break;
                        case 4:
                            status = "Locked";
                            break;
                    }

                    $('<label>').text(status).appendTo(container);
                }
            },
        ];

        LoadGrid('fiscalyearcontainer', 'fiscalyeargrid', columns, 'fiscalyears/ledger/' + ledgerid + '?fields=all', 'fiscalyears', LoadFiscalPeriods, 'fy-', '.fiscalyearmodal', '.fiscalyearmodal', 250, true, false, false, null);
        
    });
}

function LoadFiscalPeriods(info) {

    if (!info) {
        var dataGrid = $('.taggroupsgrid').dxDataGrid('instance');
        info = dataGrid.getSelectedRowsData();
        selectedRow = info[0];
    } else {
        selectedRow = info.data;
    }

    /*
    <option value="0">Open</option>
    <option value="1">Closed</option>
    <option value="2">Reopened</option>
    */

    var columns = [
        { dataField: 'Id', width: "0px" },
        { dataField: 'PeriodNumber', caption: '' },
        { dataField: 'StartDate', caption: 'Start Date', dataType: 'date' },
        { dataField: 'EndDate', caption: 'End Date', dataType: 'date' },
        {
            caption: 'Status', cellTemplate: function (container, options) {

                var status;

                switch (options.data.Status) {
                    case 0:
                        status = "Open";
                        break;
                    case 1:
                        status = "Closed";
                        break;
                    case 2:
                        status = "Reopened";
                        break;
                }

                $('<label>').text(status).appendTo(container);
            }
        }
    ]

    LoadGrid('fiscalperiodscontainer', 'fiscalperiodgrid', columns, 'fiscalperiods/fiscalyear/' + selectedRow.Id + '?fields=all', 'fiscalperiods', null, 'fp-', '.fiscalperiodmodal', '.fiscalperiodmodal', 250, true, false, false, null);

}

function LoadFundAccountingSectionSettings() {

  $('.contentcontainer').empty();
  var container = $('<div>').appendTo('.contentcontainer');

    var header = $('<h1>').text('Settings for DCEF ');
    $(header).appendTo(container);

    var selectfiscalyeargroup = $('<div>').addClass('twocolumn');
    var selectfiscalyearname = $('<label>').text('Fiscal Year: ');
      $('<select>').addClass('selectfiscalyear').appendTo(selectfiscalyearname);
    $(selectfiscalyearname).appendTo(selectfiscalyeargroup);
    $(selectfiscalyeargroup).appendTo(container);

    var selectfundgroup = $('<div>').addClass('twocolumn');
    var selectfundname = $('<label>').text('Fund: ');
    $('<select>').addClass('selectfund').appendTo(selectfundname);
    $(selectfundname).appendTo(selectfundgroup);
    $(selectfundgroup).appendTo(container);
   
    var selectfundbalanceaccountgroup = $('<div>').addClass('threecolumn');
    var selectfundbalanceaccountname = $('<label>').text('Fund balance account: ');
    $('<select>').addClass('selectfund').appendTo(selectfundbalanceaccountname);
    var selectaccumlatedrevenuename = $('<label>').text('Accumlated Revenue ');
    $(selectfundbalanceaccountname).appendTo(selectfundbalanceaccountgroup);
    $(selectaccumlatedrevenuename).appendTo(selectfundbalanceaccountgroup);
    $(selectfundbalanceaccountgroup).appendTo(container);

    var selectclosingrevenueaccountgroup = $('<div>').addClass('twocolumn');
    var selectclosingrevenueaccountname = $('<label>').text('Closing Revenue Account: ');
    $('<select>').addClass('selectfund').appendTo(selectclosingrevenueaccountname);
    var selectaccumlatedrevenuename = $('<label>').text('Accumlated Revenue ');
    $(selectclosingrevenueaccountname).appendTo(selectclosingrevenueaccountgroup);
    $(selectaccumlatedrevenuename).appendTo(selectclosingrevenueaccountgroup);
    $(selectclosingrevenueaccountgroup).appendTo(container);

    var selectclosingexpenseaccountgroup = $('<div>').addClass('twocolumn');
    var selectclosingexpenseaccountname = $('<label>').text('Closing Expense Account: ');
    $('<select>').addClass('selectfund').appendTo(selectclosingexpenseaccountname);
    var selectaccumlatedrevenuename = $('<label>').text('Accumlated Revenue ');
    $(selectclosingexpenseaccountname).appendTo(selectclosingexpenseaccountgroup);
    $(selectaccumlatedrevenuename).appendTo(selectclosingexpenseaccountgroup);
    $(selectclosingexpenseaccountgroup).appendTo(container);
    
    MakeServiceCall('GET', 'ledgers/businessunit/' + currentBusinessUnit.Id, null, function (data) {
        var ledger = data.Data[0];
        if (data && data.Data) {

            
            PopulateDropDown('.selectfiscalyear', 'fiscalyears/ledger/' + ledger.Id, '', '', ledger.DefaultFiscalYearId, null, null)
             //PopulateDropDown('.selectfund', 'ledgers/' + ledger.Id, '', '', ledger.selectfundname, null, null)


        }

    }, null);
    $(container).appendTo($('.contentcontainer'));

    LoadSectionSettings(SettingsCategories.GeneralLedger, 'Fund Accounting Settings', 'sectionpreferences', SystemSettings.FundAccountingSettings);

    var accordion = $('<div>').addClass('accordions');
    var businessunitdue = $('<div>').addClass('businessunitduecontainer');
   // var funddue = $('<div>').addClass('fundduefrom/duetoaccounts');

    var header = $('<h1>').text('Business Unit Due From/Due to Accounts ').appendTo($(accordion));
    $(status).appendTo($(accordion));

    var businessduecolumns = [
              { dataField: 'Id', width: '0px' },
              { dataField: 'Name', caption: 'Fund' },
              { dataField: 'Name', caption: 'Due From Account' },
              { dataField: 'Name', caption: 'Description' },
           //   { dataField: 'Name', caption: 'Due To Account' },
            //  { dataField: 'Name', caption: 'Description' }
    ];
    LoadGrid('.businessunitduecontainer', 'businessunitduecontainergrid', businessduecolumns, 'businessunitdues', 'businessunitdues', null, 'businessdue-',
        '.businessunitduemodal', '.businessunitduemodal', 250, true, false, false, null);
    
    
    $(accordion).appendTo($('.gridcontainer'));

    LoadAccordions();

  


    // a change here for demo purposes, can be removed later.
    // something here
}

function LoadGLFormatSectionSettings() {

    $('.contentcontainer').empty();

    var container = $('<div>'); 

    var glaccountformat = '';
    
    var selectledgergroup = $('<div>').addClass('twocolumn');   
    var selectledgername = $('<h1>').text('GL Format for Ledger: ');
    $('<select>').addClass('LedgerId').appendTo(selectledgername);
    $(selectledgername).appendTo(selectledgergroup);
    $(selectledgergroup).appendTo(container);

    var glformat = $('<div>').addClass('glformatcontainer');
    $(glformat).appendTo($(container));

    
    $(container).appendTo($('.contentcontainer'));

    

    PopulateDropDown('.LedgerId', 'ledgers/businessunit/' + currentBusinessUnit.Id, '', '', $('.LedgerId').val(), function () {

        var ledgerId = $('.LedgerId').val();
        var canDeleteSegmentLevels = false;
        var editModalClass = '';
        
        var modalLinkClass = 'glformat-newmodallink';
        $('.' + modalLinkClass).remove();

        $('.glformat-LedgerId').val(ledgerId);

        MakeServiceCall('GET', 'ledgers/' + ledgerId, null, function (data) {

            if (data && data.Data && data.IsSuccessful) {

                glaccountformat = data.Data.DisplayFormat;

                if (data.Data.LedgerAccounts === null || data.Data.LedgerAccounts.length === 0) {

                    canDeleteSegmentLevels = true;
                    editModalClass = '.glformatmodal';

                    NewModalLink('.glformatcontainer', 'segmentlevels', 'glformat-', editModalClass, 250, '');

                }
                else {
                    canDeleteSegmentLevels = false;
                    editModalClass = '';
                }

                var glformatcolumns = [
            { dataField: 'Id', width: '0px' },
            { dataField: 'Level', caption: 'Level' },
            {
                caption: 'Type', cellTemplate: function (container, options) {
                    var type = "None";

                    switch (options.data.Type) {
                        case 1:
                            type = "Fund";
                            break;
                        case 2:
                            type = "Account";
                            break;
                    }

                    $('<label>').text(type).appendTo(container);
                }
            },
            {
                caption: 'Format', cellTemplate: function (container, options) {
                    var format;

                    switch (options.data.Format) {
                        case 0:
                            format = "Both";
                            break;
                        case 1:
                            format = "Numeric";
                            break;
                        case 2:
                            format = "Alpha";
                            break;
                    }

                    $('<label>').text(format).appendTo(container);
                }
            },
            { dataField: 'Length', caption: 'Length' },
            { dataField: 'IsLinked', caption: 'Linked' },
            { dataField: 'IsCommon', caption: 'Common' },
            { dataField: 'Name', caption: 'Name' },
            { dataField: 'Abbreviation', caption: 'Abbreviation' },
            {
                caption: 'Separator', cellTemplate: function (container, options) {
                    var separator = 'None';

                    switch (options.data.Separator) {
                        case " ":
                            separator = "(Space)";
                            break;
                        case "-":
                            separator = "-";
                            break;
                        case ".":
                            separator = ".";
                            break;
                        case ",":
                            separator = ",";
                            break;
                        case "/":
                            separator = "/";
                            break;
                        case "(":
                            separator = "(";
                            break;
                        case ")":
                            separator = ")";
                            break;
                        case "[":
                            separator = "[";
                            break;
                        case "]":
                            separator = "]";
                            break;
                    }

                    $('<label>').text(separator).appendTo(container);
                }
            },
            {
                caption: 'Sort Order', cellTemplate: function (container, options) {
                    var order = 'None';

                    switch (options.data.SortOrder) {
                        case 0:
                            order = "Ascending";
                            break;
                        case 1:
                            order = "Unaffiliated";
                            break;
                    }

                    $('<label>').text(order).appendTo(container);
                }
            }
                ];


                LoadGrid('.glformatcontainer', 'glformatgrid', glformatcolumns, 'segmentlevels/ledger/' + ledgerId, 'segmentlevels', null, 'glformat-',
                    editModalClass, editModalClass, 250, canDeleteSegmentLevels, false, false, function () {

                        $('.AccountFormat').remove();
                        $('<span>').addClass('AccountFormat').text('Example3: ' + glaccountformat).appendTo($('.glformatcontainer'));

                    });
            }
            


        }, null);
        
    });

    


    
    
    
}

function LoadJournalSectionSettings() {

    $('.contentcontainer').empty();


}

function LoadUtilitiesSectionSettings() {

    $('.contentcontainer').empty();


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

    DisplayCustomFieldsGrid('gridcontainer', CustomFieldEntity.CRM); // CRM = 19

    CreateNewCustomFieldModalLink(CustomFieldEntity.CRM, 'New CRM Custom Field');

}

function LoadDonationClientCustomFieldsSectionSettings() {

    DisplayCustomFieldsGrid('gridcontainer', CustomFieldEntity.Gifts); // Gifts = 9

    CreateNewCustomFieldModalLink(CustomFieldEntity.Gifts, 'New Donations Custom Field');

}

function LoadGLClientCustomFieldsSectionSettings() {

    DisplayCustomFieldsGrid('gridcontainer', CustomFieldEntity.GeneralLedger); // GeneralLedger = 1

    CreateNewCustomFieldModalLink(CustomFieldEntity.GeneralLedger, 'New General Ledger Custom Field');
    
}

function RefreshCustomFieldsGrid() {

    $('.gridcontainer').html('');

    DisplayCustomFieldsGrid('gridcontainer', currentCustomFieldEntity);

}

function CreateNewCustomFieldModalLink(entity, title) {

    var modallink = $('<a>').attr('href', '#').addClass('customfieldmodallink').text('New Custom Field').appendTo($('.gridcontainer'));
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

    SendCustomField(method, 'customfields', data, modal);

}

function SendCustomField(method, route, data, modal) {

    MakeServiceCall(method, 'route', JSON.stringify(data), function (data) {

        if (data.Data) {
            DisplaySuccessMessage('Success', 'Custom field saved successfully.');

            CloseModal(modal);

            RefreshCustomFieldsGrid();

            CreateNewCustomFieldModalLink(currentCustomFieldEntity, '');

        }

    }, null);

}
/* END CUSTOM FIELDS */

