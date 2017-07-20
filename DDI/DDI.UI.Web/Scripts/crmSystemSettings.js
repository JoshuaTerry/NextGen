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

        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];
    LoadGrid('.clergystatuscontainer', 'clergystatusgrid', statuscolumns, 'clergystatuses?fields=all', 'clergystatuses', null, 'cstat-',
        '.clergystatusmodal', '.clergystatusmodal', 250, true, false, false, null);

    header = $('<h1>').text('Clergy Type').appendTo($(accordion));
    $(types).appendTo($(accordion));

    var typecolumns = [

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
    $('<a>').attr('href', '#').addClass('newconstituenttypemodallink modallink newbutton').text('New Item')
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

                ValidateFields(modal, function () {
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

                    }, function (xhr, status, err) {

                        DisplayErrorMessage('Error', 'An error occurred while saving the Constituent Type.');

                    });
                });

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

            LoadAvailableTags(tagmodal, false);

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

                MakeServiceCall('POST', 'constituenttypes/' + constTypeId + '/constituenttypetags', { tags: tagIds }, function (data) {

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


    CustomLoadGrid('constituenttypesgrid', 'constituenttypescontainer', typecolumns, 'constituenttypes?fields=all', '', EditConstituentType, DeleteConstituentType, null);

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

        ValidateFields(modal, function () {
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

    });

}

function DeleteConstituentType(id) {

    MakeServiceCall('DELETE', 'constituenttypes/' + id, null, function (data) {

        DisplaySuccessMessage('Success', 'Constituent Type deleted successfully.');

        LoadConstituentTypeSettingsGrid();

    },

        function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred deleting the Constituent Type.');
        }
    );
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
    $('<a>').attr('href', '#').addClass('newaddresstypemodallink modallink newbutton').text('New Item')
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
    $('<a>').attr('href', '#').addClass('newcontactcategorymodallink modallink newbutton').text('New Item')
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
    $('<a>').attr('href', '#').addClass('newcontacttypemodallink modallink newbutton').text('New Item')
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

        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    CustomLoadGrid('addresstypesgrid', 'addresstypescontainer', addresstypecolumns, 'addresstypes?fields=all', null, EditAddressType, DeleteAddressType);

}

function LoadContactCategorySettingsGrid() {

    var contactcategorycolumns = [

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
            DisplayErrorMessage('Error', xhr.responseJSON.Message);
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
            DisplayErrorMessage('Error', xhr.responseJSON.Message);
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

        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Ethnicity' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('.ethnicitiescontainer', 'ethnicitiesgrid', ethnicitycolumns, 'ethnicities?fields=all', 'ethnicities', null, 'eth-',
        '.ethnicitymodal', '.ethnicitymodal', 250, true, false, false, null);

}

function LoadLanguageSettingsGrid() {

    var languagecolumns = [

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

    InitRequiredLabels("educationLevelmodal")
}

function LoadDegreeSettingsGrid() {

    var degreecolumns = [

        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('.degreecontainer', 'degreesgrid', degreecolumns, 'degrees?fields=all', 'degrees', null, 'deg-',
        '.degreemodal', '.degreemodal', 250, true, false, false, null);

}

function LoadEducationLevelSettingsGrid() {

    var educationLevelcolumns = [

        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('.educationlevelscontainer', 'educationlevelsgrid', educationLevelcolumns, 'educationlevels?fields=all', 'educationlevels', null, 'eduLev-',
        '.educationLevelmodal', '.educationLevelmodal', 250, true, false, false, null);

}

function LoadSchoolsSettingsGrid() {

    var schoolcolumns = [

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

        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    LoadGrid('.incomeLevelcontainer', 'incomeLevelgrid', incomeLevelcolumns, 'incomelevels?fields=all', 'incomelevels', null, 'inc-',
        '.incomeLevelmodal', '.incomeLevelmodal', 250, true, false, false, null);

}

function LoadProfessionSettingsGrid() {

    var professioncolumns = [

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

    route = route + "?fields=Id,Code,Name,IsActive";

    var columns = [

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

function EditRegion(id) {

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
    $('<a>').attr('href', '#').addClass('newrelationshipcategorymodallink modallink newbutton').text('New Item')
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
    $('<a>').attr('href', '#').addClass('newrelationshiptypesmodallink modallink newbutton').text('New Item')
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
        { dataField: 'Order', caption: 'Order' },
        { dataField: 'Code', caption: "Code" },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];

    CustomLoadGrid('tagsgrid',
        'tagscontainer',
        columns,
        'taggroups/' + selectedRow.Id + '/tags?fields=Id,Order,Code,Name,IsActive',
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