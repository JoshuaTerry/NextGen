
var SAVE_ROUTE = 'constituents/';
var currentaddress = null;

$(document).ready(function () {

    $('#form1').validate();

    ApplySystemSettings();

    Resize();

    LoadDropDowns();

    $(window).resize(function () {
        Resize();
    });

    DisplayConstituentCustomFields();

    CreateEditControls();

    SetupEditControls();

    if (sessionStorage.getItem('constituentid')) {

        $('.hidconstituentid').val(sessionStorage.getItem('constituentid'));

    }

    GetConstituentData($('.hidconstituentid').val());

    LoadYears();

    $('.BirthMonth').change(function () { PopulateMonthDays(); });

    $('.BirthYear').change(function () { AmendMonthDays(); });

    $('.fileuploadlink').click(function (e) {
        e.preventDefault();

        UploadFiles();
    });

});

function UploadFiles(callback) {

    modal = $('.fileuploadmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 400,
        resizable: false
    });

    InitializeFileUploader(WEB_API_ADDRESS + 'filestorage/upload', callback);

}

function ApplySystemSettings() {
    
    GetSystemSettings(SettingsCategories.CRM, function (settings) {

        $.map(settings, function (item) {

            if (item.IsShown) {
                $('.' + item.Name + 'Section').show();
                $('.' + item.Name + 'SectionLabel').text(item.Value);
            }
            else {
                $('.' + item.Name + 'Section').hide();
            }


        });

    });

}

function LoadYears()
{
    var currentYear = new Date().getFullYear();
    $('.BirthYear').append('<option value=> </option>');
    for (x = currentYear; x >= currentYear - 100; x--) {
        $('.BirthYear').append('<option value=' + x + '>' + x + '</option>');
    }
}

function PopulateMonthDays()
{
    var arrayLookup = {
        '1': 31, '2': 29, '3': 31,
        '4': 30, '5': 31,
        '6': 30, '7': 31,
        '8': 31, '9': 30,
        '10': 31, '11': 30, '12': 31
    }
    var month = $('.BirthMonth').val();
    if (month != null)
    {
        var days = arrayLookup[month];
        $('.BirthDay').html('');
        $('.BirthDay').append('<option value=> </option>');
        for (x = 1; x <= days; x++) {
            $('.BirthDay').append('<option value=' + x + '>' + x + '</option>');
        }
    }
    AmendMonthDays();
}

function AmendMonthDays()
{
    var year = $('.BirthYear').val();
    var month = $('.BirthMonth').val();
    var day = $('.BirthDay').val();
    if (year != null && month == '2')
    {
        var option29 = $('.BirthDay option[value="29"]');

        if ((year % 4 == 0) && (year % 100 != 0) || (year % 400 == 0)) {
            if ($(option29).val() === undefined) {
                $('.BirthDay').append('<option value="29">29</option>');
            }
            else {
                return;
            }
        }
        else {
            if ($(option29) != null) {
                $(option29).remove();
            }
        }
    }
}

function Resize() {



}

function LoadDropDowns() {
    
    PopulateDropDown('.ConstituentStatusId', 'constituentstatuses', '', '');
    PopulateDropDown('.PrefixId', 'prefixes', '', '');
    PopulateDropDown('.GenderId', 'genders', '', '');
    PopulateDropDown('.ClergyTypeId', 'clergytypes', '', '');
    PopulateDropDown('.ClergyStatusId', 'clergystatuses', '', '');
    PopulateDropDown('.LanguageId', 'languages', '', '');
    PopulateDropDown('.EducationLevelId', 'educationlevels', '', '');
    PopulateDropDown('.MaritalStatusId', 'maritalstatuses', '', '');
    PopulateDropDown('.ProfessionId', 'professions', '', '');
    PopulateDropDown('.IncomeLevelId', 'incomelevels', '', '');
    PopulateDropDown('.RelationshipTypeId', 'relationshiptypes', '', '');
    EducationModalDropDowns();
}

function GetConstituentData(id) {
    route = 'constituents/';

    if (id.length <= 9) {
        route += 'number/'; // If id length is <= 9, assume the constituent ID is a constituent number.
    }
    
    $.ajax({
        url: WEB_API_ADDRESS + route + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            currentEntity = data.Data;

            DisplayConstituentData();
            
        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });
}

function RefreshEntity() {

    DisplayConstituentData();

}

function DisplayConstituentData() {

    if (currentEntity) {

        var id = currentEntity.Id;
        sessionStorage.setItem('constituentid', id);
        $('.hidconstituentid').val(id);

        $.map(currentEntity, function (value, key) {

            if (typeof (value) == 'string')
                value = value.replace('"', '').replace('"', '');

            if (key != '$id') {

                var classname = '.' + key;

                if ($(classname).is('input')) {
                    if ($(classname).is(':checkbox')) {
                        $(classname).prop('checked', value);
                    }
                    else {
                        $(classname).val(value);
                    }
                }

                if ($(classname).is('select')) {
                    $(classname).val(value);
                }
            
                if (key.toLowerCase().indexOf('date') !== -1) {

                    var date = FormatJSONDate(value);

                    $(classname).text(date);
                }

            }
        });

        DisplayConstituentPicture();

        DisplayConstituentSideBar();

        DisplayConstituentType();

        GenerateContactInfoSection();

        LoadDenominationsTagBox();

        LoadDBAGrid();

        LoadEducationGrid();

        LoadEthnicitiesTagBox();

        LoadPaymentPreferencesTable();

        LoadContactInfo();

        LoadAlternateIDTable();

        LoadRelationshipsTab();

        PopulateMonthDays();

        AmendMonthDays();

        $('.BirthDay').val(currentEntity.BirthDay);
    
        PopulateUserIdDropDown();

        LoadRelationshipsQuickView();

      

        NewAddressModal();

        DisplaySelectedTags($('.constituenttagselect'));

		ShowAuditData(currentEntity.Id);
    }
}

function DisplayConstituentPicture() {

    var img = $('.constituentpic img');

    $.ajax({
        url: WEB_API_ADDRESS + 'constituentpicture/' + currentEntity.Id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            if (data.Data && data.Data[0]) {

                GetFile(data.Data[0].FileId, function (item) {

                    $(img).attr('src', 'data:image/jpg;base64,' + item.Data).appendTo($('.constituentpic'));

                });

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

    $('.constituentpic').on('mouseenter', function () {
        $('.changeconstituentpic').stop().show().animate({ height: '50px', bottom: '0px', opacity: '.9' }, 100);
    }).on('mouseleave', function () {
        $('.changeconstituentpic').stop().animate({ height: '0px', bottom: '0px', opacity: '0' }, 100);
    });

    $('.constituentpic').unbind('click');
    $('.constituentpic').click(function () {

        UploadFiles(function (file) {

            var data = {
                ConstituentId: currentEntity.Id,
                FileId: file.Id
            }

            $.ajax({
                url: WEB_API_ADDRESS + 'constituentpicture',
                method: 'POST',
                data: JSON.stringify(data),
                contentType: 'application/json; charset-utf-8',
                dataType: 'json',
                crossDomain: true,
                success: function (data) {

                    DisplayConstituentPicture();

                    CloseModal(modal);

                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
                }
            });

        });

    });

}

function DisplayConstituentSideBar() {

    $('.FormattedName').text(currentEntity.FormattedName);

    GetConstituentPrimaryAddress();

    GetConstituentPreferredContactInfo();

}

function DisplayConstituentType() {

    $('#tab-main-link').text(currentEntity.ConstituentType.DisplayName);

    LoadTagSelector(currentEntity.Category, $('.constituenttagselect'));

    if (currentEntity.ConstituentType.Category === 0) {
        $('.organizationConstituent').hide();
        $('.individualConstituent').show();
        $('.organizationSection').hide();
        $('.DBASettingsSection').hide();
    } else {
        $('.organizationConstituent').show();
        $('.individualConstituent').hide();
        $('.PersonalSettingsSection').hide();
        $('.ClergySettingsSection').hide();
        $('.EducationSettingsSection').hide();
        $('.ProfessionalSettingsSection').hide();
    }

}

function GetConstituentPrimaryAddress() {
    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + SAVE_ROUTE + currentEntity.Id + '/constituentaddresses/' ,
        contentType: 'application/json',
        crossDomain: true,
        success: function (data) {

            currentaddress = data.Data;

            for (i = 0; i < currentaddress.length; i++) { 

                if (currentaddress[i].IsPrimary) {

                    $('.Address').text(currentaddress[i].Address.AddressLine1);

                    if (currentaddress[i].Address.AddressLine2 != null && currentaddress[i].Address.AddressLine2.length > 0) {

                        $('.Address').append(currentaddress[i].Address.AddressLine2);

                    }

                    $('.CityStateZip').text(currentaddress[i].Address.City + ', ' + currentaddress[i].Address.State + ', ' + currentaddress[i].Address.PostalCode);

                }
            }
        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });
}

function GetConstituentPreferredContactInfo() {
    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'constituents/' + currentEntity.Id,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            currentcontactinfo = data.Data.ContactInfo;

            var preferredContactInfos = ''

            for (i = 0; i < currentcontactinfo.length; i++) {

                if (currentcontactinfo[i].IsPreferred) {

                    preferredContactInfos += currentcontactinfo[i].Info + ' ';

                }

                $('.ContactInfo').text(preferredContactInfos);

            }
        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}


/* Demograpics Section */
function LoadDenominationsTagBox() {
    LoadTagBoxes('tagBoxDenominations', 'denominations', 'denominations', '/constituents/' + currentEntity.Id + '/denominations');
}

function LoadEthnicitiesTagBox() {
    LoadTagBoxes('tagBoxEthnicities', 'ethnicities', 'ethnicities', '/constituents/' + currentEntity.Id + '/ethnicities');
}
/* End Demographics Section */

/* Doing Business As Section */
function LoadDBAGrid() {

    var columns = [
        { dataField: 'Id', width: '0px', },
        { dataField: 'StartDate', caption: 'From', dataType: 'date' },
        { dataField: 'EndDate', caption: 'To', dataType: 'date' },
        { dataField: 'Name', caption: 'Name' }
    ];

    LoadGrid('.doingbusinessastable', 'dbagrid', columns, 'constituents/' + currentEntity.Id + '/doingbusinessas', 'doingbusinessas'
        , null, 'dba-', '.dbamodal', '.dbamodal', 250, false, true, false, null);

}

/* End Doing Business As Section */


/* Education Section */
function LoadEducationGrid() {

    var columns = [
            { dataField: 'Id', width: '0px' },
            { dataField: 'StartDate', caption: 'Start Date', dataType: 'date' },
            { dataField: 'EndDate', caption: 'End Date', dataType: 'date' },
            { dataField: 'School.DisplayName', caption: 'School' },
            { dataField: 'Degree.DisplayName', caption: 'Degree' },
            { dataField: 'Major', caption: 'Major' }
    ];

    LoadGrid('.educationgridcontainer', 'educationgrid', columns, 'constituents/' + currentEntity.Id + '/educations', 'educations'
        , null, 'ed-', '.educationmodal', '.educationmodal', 250, false, true, false, null);

}

function EducationModalDropDowns() {

    PopulateDropDown('.ed-Degree', 'degrees', '', '', null);
    PopulateDropDown('.ed-School', 'schools', '', '', null);

}

/* End Education Section */
 
/* Payment Preference Section */
function LoadPaymentPreferencesTable() {

    var columns = [
            { dataField: 'Id', width: '0px' },
            { dataField: 'Description', caption: 'Description' },
            { dataField: 'RoutingNumber', caption: 'Routing Number' },
            { dataField: 'BankAccount', caption: 'Account Number' },
            {
                caption: 'Ch/S', cellTemplate: function (container, options) {
                    var type = 'Ch';

                    if (options.data.AccountType == '1') {
                        type = 'S';
                    }

                    $('<label>').text(type).appendTo(container);
                }
            }
    ];

    LoadGrid('.paymentpreferencesgridcontainer', 'paymentpreferencesgrid', columns, 'paymentmethods/constituents/' + currentEntity.Id, 'paymentmethods'
        , null, 'pp-', '.paymentpreferencemodal', '.paymentpreferencemodal', 250, false, true, false, null);
}

/* End Payment Preference Section */


/* Additional Information (Custom Fields) */
function DisplayConstituentCustomFields() {

    DisplayCustomFields('customFieldContainer', CustomFieldEntity.CRM, function () {
        $('.editable').prop('disabled', true);
        LoadDatePickers();
        LoadDatePair();
    });

}
/* End Additional Information (Custom Fields) */

/* Professional Section */
function PopulateUserIdDropDown() {
    $('.IsEmployee').change(function () {

        if (this.checked) {

            PopulateDropDown('.UserId', 'userid', '', '');
            // populate dropdown

        } else {

            $('.UserId option').remove();
            // clear dropdown
        }
    });
}

/* End Professional Section */

/* Audit Section */
function ShowAuditData(id) {

    $('.newauditmodal').click(function (e) {

        e.preventDefault();
    var modal = $('.auditmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 800,
        height: 600,
        resizable: true
    });

    LoadAuditTable(id, modal);
    });
    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.queryAudit').unbind('click');

    $('.queryAudit').click(function () {

        $.ajax({
            type: 'GET',
            url: WEB_API_ADDRESS + 'audit/flat/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function (data) {

                LoadAuditTable();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
            }
        });

    });

}

function LoadAuditTable() {
    var id = currentEntity.Id;
    var start = $(modal).find('.na-StartDate').val();
    var end = $(modal).find('.na-EndDate').val();
    var route = 'audit/flat/' + id;
    var columns = [
            { dataField: 'ChangeSetId', groupIndex: 0, sortOrder: 'desc', sortIndex: 0  },
            { dataField: 'Timestamp', caption: 'Date', dataType: 'date', width: '10%' },
            { dataField: 'User' }, 
            { dataField: 'ChangeType', width: '100px' },  
            { dataField: 'Property' },
            { dataField: 'OldValue', caption: 'Old Value' },
            { dataField: 'OldDisplayName', caption: 'Old Display Name' },
            { dataField: 'NewValue', caption: 'New Value' },
            { dataField: 'NewDisplayName', caption: 'New Display Name' }
    ];

    LoadAuditGrid('auditgrid',
       'auditgridcontainer',
       columns,
       route,
       false);
     
}
/* End Audit Section */

/* Alternate Id Section */
function LoadAlternateIDTable() {

    var columns = [
            { dataField: 'Id', width:'0px' },
            { dataField: 'Name', caption: 'Name' }
    ];

    LoadGrid('.alternateidgridcontainer', 'altidgrid', columns, 'constituents/' + currentEntity.Id + '/alternateids', 'alternateids'
        , null, 'ai-', '.alternateidmodal', '.alternateidmodal', 250, false, true, false, null);
}

/* End Alternate Id Section */

/* Contact Information Section */
function LoadContactInfo() {

    LoadAddressesGrid();

}

function LoadAddressesGrid() {

    var columns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'IsPrimary', caption: 'Is Primary' },
        { dataField: 'AddressType.DisplayName', caption: 'Type' },
        { dataField: 'Address.AddressLine1', caption: 'Address' }
    ];

    LoadGrid('constituentaddressgrid',
        'constituentaddressgridcontainer',
        columns,
        'constituents/' + currentEntity.Id + '/constituentaddresses',
        null,
        EditAddressModal);
    
    // LoadGrid(container, gridClass, columns, route, selected, prefix, editModalClass, newModalClass, modalWidth, 
    // showDelete, showFilter, showGroup, onComplete)
    //LoadGrid('.constituentaddressgridcontainer', 'constituentaddressgrid', columns, 'constituents/' + currentEntity.Id + '/constituentaddresses'
    //    , null, 'na-', '.addressmodal', '.addressmodal', 250, false, true, false, null);


}

function NewAddressModal() {

    $('.newaddressmodallink').click(function (e) {

        e.preventDefault();

        modal = $('.addressmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 375,
            resizable: false
        });

        LoadRegions($(modal).find('.regionscontainer'), 'na-');

        $('.cancelmodal').click(function (e) {

            e.preventDefault();

            CloseModal(modal);

        });

        $('.saveaddress').unbind('click');

        $('.saveaddress').click(function () {

            var item = {
                ConstituentId: currentEntity.Id,
                IsPrimary: $(modal).find('.na-IsPreferred').prop('checked'),
                Comment: $(modal).find('.na-Comment').val(),
                StartDate: $(modal).find('.na-StartDate').val(),
                EndDate: $(modal).find('.na-EndDate').val(),
                StartDay: 0,
                EndDay: 0,
                ResidentType: $(modal).find('.na-ResidentType').val(),
                AddressTypeId: $(modal).find('.na-AddressTypeId').val(),
                Address: {
                    AddressLine1: $(modal).find('.na-AddressLine1').val(),
                    AddressLine2: $(modal).find('.na-AddressLine2').val(),
                    City: $(modal).find('.na-City').val(),
                    CountryId: $(modal).find('.na-CountryId').val(),
                    CountyId: $(modal).find('.na-CountyId').val(),
                    PostalCode: $(modal).find('.na-PostalCode').val(),
                    StateId: $(modal).find('.na-StateId').val(),
                    Region1Id: $(modal).find('.na-Region1Id').val(),
                    Region2Id: $(modal).find('.na-Region2Id').val(),
                    Region3Id: $(modal).find('.na-Region3Id').val(),
                    Region4Id: $(modal).find('.na-Region4Id').val()
                }
            }

            $.ajax({
                type: 'POST',
                url: WEB_API_ADDRESS + 'constituentaddresses',
                data: item,
                contentType: 'application/x-www-form-urlencoded',
                crossDomain: true,
                success: function () {

                    DisplaySuccessMessage('Success', 'Address saved successfully.');

                    CloseModal(modal);

                    LoadAddressesGrid();

                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
                }
            });

        });

        AutoZip(modal, '.na-');

    });

    PopulateAddressTypesInModal(null);

    PopulateCountriesInModal(null);

}

function PopulateAddressTypesInModal(selectedValue) {

    PopulateDropDown('.na-AddressTypeId', 'addresstypes', '', '', selectedValue);

}

function PopulateCountriesInModal(selectedValue) {

    PopulateDropDown('.na-CountryId', 'countries', '', '', selectedValue, function () {
        PopulateStatesInModal(null);
    });

}

function PopulateStatesInModal(selectedValue) {

    ClearElement('.na-StateId');
    ClearElement('.na-CountyId');

    var countryid = $('.na-CountryId').val();

    PopulateDropDown('.na-StateId', 'states/?countryid=' + countryid, '', '', selectedValue, function () {
        PopulateCountiesInModal(null)
    });

}

function PopulateCountiesInModal(selectedValue) {

    var stateid = $('.na-StateId').val();

    PopulateDropDown('.na-CountyId', 'counties/?stateid=' + stateid, '', '', selectedValue);

}

function EditAddressModal(id) {

    modal = $('.addressmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 375,
        height: 560,
        resizable: false
    });

    PopulateAddressTypesInModal(null);

    PopulateCountriesInModal(null);

    LoadRegions($(modal).find('.regionscontainer'), 'na-');

    AutoZip(modal, '.na-');

    LoadAddress(id);

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.saveaddress').unbind('click');

    $('.saveaddress').click(function () {

        // Get the changed fields from currentaddress and put into new array.
        var fields = GetEditedAddressFields();

        $.ajax({
            type: 'PATCH',
            url: WEB_API_ADDRESS + 'constituentaddresses/' + id,
            data: fields,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Address saved successfully.');

                CloseModal(modal);

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
            }
        })

    });

}

function GetEditedAddressFields() {

    var item = {
        Id: $(modal).find('.hidconstituentaddressid').val(),
        ConstituentId: currentEntity.Id,
        IsPrimary: $(modal).find('.na-IsPreferred').prop('checked'),
        Comment: $(modal).find('.na-Comment').val(),
        StartDate: $(modal).find('.na-StartDate').val(),
        EndDate: $(modal).find('.na-EndDate').val(),
        StartDay: 0,
        EndDay: 0,
        ResidentType: $(modal).find('.na-ResidentType').val(),
        AddressTypeId: $(modal).find('.na-AddressTypeId').val(),
        Address: {
            Id: $(modal).find('.hidaddressid').val(),
            AddressLine1: $(modal).find('.na-AddressLine1').val(),
            AddressLine2: $(modal).find('.na-AddressLine2').val(),
            City: $(modal).find('.na-City').val(),
            CountryId: $(modal).find('.na-CountryId').val(),
            CountyId: $(modal).find('.na-CountyId').val(),
            PostalCode: $(modal).find('.na-PostalCode').val(),
            StateId: $(modal).find('.na-StateId').val(),
            Region1Id: $(modal).find('.na-Region1Id').val(),
            Region2Id: $(modal).find('.na-Region2Id').val(),
            Region3Id: $(modal).find('.na-Region3Id').val(),
            Region4Id: $(modal).find('.na-Region4Id').val()
        }
    }

    return item;

}

function LoadAddress(id) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'constituentaddresses/' + id,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            currentaddress = data.Data;

            $('.hidconstituentaddressid').val(data.Data.Id);
            $('.hidaddressid').val(data.Data.Address.Id);

            $('.na-isIsPreferred').prop('checked', data.Data.Address.IsPreferred);
            $('.na-Comment').val(data.Data.Comment);
            $('.na-StartDate').val(data.Data.StartDate);
            $('.na-EndDate').val(data.Data.EndDate);
            $('.na-ResidentType').val(data.Data.ResidentType);

            $('.na-AddressLine1').val(data.Data.Address.AddressLine1);
            $('.na-AddressLine2').val(data.Data.Address.AddressLine2);
            $('.na-City').val(data.Data.Address.City);

            $('.na-PostalCode').val(data.Data.Address.PostalCode);

            PopulateAddressTypesInModal(data.Data.AddressTypeId);
            PopulateCountriesInModal(data.Data.Address.CountryId);

            PopulateDropDown('.na-StateId', 'states/?countryid=' + data.Data.Address.CountryId, '', '', data.Data.Address.StateId, function () {
                PopulateCountiesInModal(null)
            });

            PopulateCountiesInModal(data.Data.Address.CountyId);

            LoadAllRegionDropDowns(modal, '.na-', data.Data.Address);

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}

function GenerateContactInfoSection() {
   
    // Grab all the contact categories, then dynamically create the accordions
    LoadCategories(function(data) {

        $.map(data.Data, function (category) {

            // remove previous elements
            $('.' + category.DisplayName + 'header').remove();
            $('.constituent' + category.DisplayName + 'gridcontainer').remove();

            // most of our accordions use h1, but for some reason accordions.refresh() only works with h3.
            var header = $('<h3>').text(category.SectionTitle).addClass(category.DisplayName + 'header').appendTo($('.contactinfocontainer'));

            $('<div>').attr('id', category.Id).addClass('constituent' + category.DisplayName + 'gridcontainer').appendTo($('.contactinfocontainer'));

            LoadContactCategoryGrid(category.Id, category.TextBoxLabel, category.DisplayName);

        });
        
        $('.accordions').accordion('refresh');
        // LoadAccordions will not work here

    });

}

function LoadCategories(CategoryTitles) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'contactcategory',
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            CategoryTitles(data);

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });
}

function LoadContactCategoryGrid(categoryid, displayText, name, idField) {

    var columns = [
        { dataField:  'Id', width: '0px' }, 
        { dataField: 'IsPreferred', caption: 'Is Preferred' },
        { dataField: 'ContactType.Name', caption: 'Type' }, 
        { dataField: 'Info', caption: displayText },
        { dataField: 'Comment', caption: 'Comment' }
    ];

    PopulateDropDown($('.'+ name.toLowerCase() + "-ContactTypeId"), 'contacttypes/'+categoryid, null)
   
    LoadGrid( 'constituent' + name + 'gridcontainer', 'constituent' + name + 'grid', columns,  'contactinfo/' + categoryid + '/' + currentEntity.Id, 'contactinfo'
        , null, name.toLowerCase() + '-', '.' + name.toLowerCase() + 'modal', '.' + name.toLowerCase() + 'modal', 250, false, true, false, null);

   
}


/* End Contact Information Section */
/* Relationships Tab */

function LoadRelationshipsQuickView() {
    var quickviewdata;

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'constituents/' + currentEntity.Id + '/relationships',
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {
            quickviewdata = data;
        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

    var formattedData = $('<ul>').addClass('relationshipQuickViewData');

    if (quickviewdata) {

        $.map(quickviewdata, function (item) {

            if (item.RelationshipType.RelationshipCategory.IsShownInQuickView === true) {

                var li = $('<li>');

                var rowText = item.RelationshipType.Name + ': ';
                var link = $('<a>').attr('href', '#').text(item.Constituent1.FormattedName).click(function (e) {
                    e.preventDefault();
                    RelationshipLinkClicked(item.Constituent1.Id)
                });

                $(li).html(rowText);
                $(link).appendTo($(li));

                $(li).appendTo($(formattedData));
                
            }

        });

    }

    $('.relationshipsQuickView').empty();

    $(formattedData).appendTo($('.relationshipsQuickView'));

}

function RelationshipLinkClicked(id) {

    sessionStorage.setItem('constituentid', id);

    if (sessionStorage.getItem('constituentid')) {
        $('.hidconstituentid').val(sessionStorage.getItem('constituentid'));
    }

    GetConstituentData($('.hidconstituentid').val());

}

function LoadRelationshipsTab() {

    var columns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'RelationshipType.RelationshipCategory.Name', caption: 'Category', groupIndex: 0 },
        { dataField: 'RelationshipType.Name', caption: 'Relationship', width: '30%' },
        { dataField: 'Constituent1.ConstituentNumber', caption: 'ID', width: '20%' },
        { dataField: 'Constituent1.FormattedName', caption: 'Name', width: '50%' }
    ];
   
    LoadGrid('.relationshipstable', 'relationshipsgrid', columns,  'constituents/' + currentEntity.Id + '/relationships', 'relationsships', null, 
        'rs-', '.relationshipmodal', '.relationshipmodal', 250, false, false, false, null);
}


function LoadRelationshipsGrid() {

    LoadRelationshipsData();
}

function EditRelationship(id) {
    var constituentId = $('.hidconstituentid').val();
    EditEntity('.relationshipmodal', '.saverelationship', 250, LoadRelationshipData, LoadRelationshipsData, GetRelationshipToSave, 'Relationship', 'constituents/' + constituentId + '/relationships', id);
}
/*
function EditRelationship(getUrl, patchUrl) {
    EditEntity(getUrl, patchUrl, "Relationship", ".relationshipmodal", ".saverelationship", 250, LoadRelationship, LoadRelationshipsGrid, GetRelationshipToSave);
}
*/

function NewRelationshipModal() {
    NewEntityModal('.newrelationshipmodal', '.relationshipmodal', '.saverelationship', 250, PrePopulateNewRelationshipModal, LoadRelationshipsData, GetRelationshipToSave, 'Relationship', 'relationships');
}

/*function LoadRelationship(url, modal) {
    LoadEntity(url, modal, "GET", LoadRelationshipData, "Relationship");
}*/

function GetRelationshipToSave(modal, isUpdate) {

    var item = {
        Constituent1Id: currentEntity.Id,
        Constituent2Id: $(modal).find('.hidconstituentlookupid').val(),
        RelationshipTypeId: $(modal).find('.RelationshipTypeId').val(),
    }

    if (isUpdate === true) {
        item.Id = $(modal).find('.hidrelationshipid').val();
        item.IsSwapped = $(modal).find('.hidrelationshipisswapped').val();
    }

    return item;
}

function PrePopulateNewRelationshipModal(modal) {
    $(modal).find('.FormattedName1').val(currentEntity.FormattedName);
}

function LoadRelationshipData(data, modal) {

    $(modal).find('.hidrelationshipid').val(data.Data.Id);
    $(modal).find('.hidrelationshipisswapped').val(data.Data.IsSwapped);

    $(modal).find('.hidconstituentlookupid').val(data.Data.Constituent2.Id);
    $(modal).find('.FormattedName1').val(data.Data.Constituent1.FormattedName);
    $(modal).find('.FormattedName2').val(data.Data.Constituent2.FormattedName);
    $(modal).find('.RelationshipTypeId').val(data.Data.RelationshipType.Id);

}
/* End Relationships Tab */




