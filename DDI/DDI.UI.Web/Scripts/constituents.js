
var SAVE_ROUTE = 'constituents/';
var currentaddress = null;

$(document).ready(function () {
    $('#form1').validate();

    Resize();

    LoadDropDowns();

    $(window).resize(function () {
        Resize();
    });

    if (sessionStorage.getItem('constituentid')) {

        $('.hidconstituentid').val(sessionStorage.getItem('constituentid'))

    }

    GetConstituentData($('.hidconstituentid').val());
    LoadYears();

    $('.BirthMonth').change(function () { PopulateMonthDays(); });    
    $('.BirthYear').change(function () { AmendMonthDays(); });

});

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

    $.ajax({
        url: WEB_API_ADDRESS + 'constituents/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            currentEntity = data.Data;

            LoadLinks(currentEntity);

            DisplayConstituentData();
            
        },
        failure: function (response) {
            alert(response);
        }
    });
}

function RefreshEntity() {

    DisplayConstituentData();

}

function DisplayConstituentData() {

    if (currentEntity) {

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

        $('.constituentpic').html('');
        var img = $('<img>');

        if (currentEntity.IsMasculine) {
            $(img).attr('src', '../../Images/Male.png');
        } else {
            $(img).attr('src', '../../Images/Female.png');
        }

        $(img).appendTo($('.constituentpic'));

        DisplayConstituentType();

        DisplayConstituentPrimaryAddress();

        GenerateContactInfoSection();

	    LoadDenominationsTagBox();

        LoadDBAGrid();

        NewDBAModal();

        LoadEducationGrid();

        NewEducationModal();

        LoadEthnicitiesTagBox();

        LoadPaymentPreferencesTable();

        LoadContactInfo();

        LoadAlternateIDTable();

        NewAlternateIdModal();

        PopulateMonthDays();

        AmendMonthDays();

        $('.BirthDay').val(currentEntity.BirthDay);
	
        PopulateUserIdDropDown();

        LoadRelationshipsGrid();

        NewRelationshipModal();

        NewAddressModal();
    }
}

function DisplayConstituentType() {
    $('#tab-main-link').text(currentEntity.ConstituentType.DisplayName);
    if (currentEntity.ConstituentType.Category === 0) {
        $('.organizationConstituent').hide();
        $('.individualConstituent').show();
    } else {
        $('.organizationConstituent').show();
        $('.individualConstituent').hide();
    }
}

function DisplayConstituentPrimaryAddress() {

    if (currentEntity.ConstituentAddresses) {

        $.map(currentEntity.ConstituentAddresses, function (item) {

            if (item.IsPrimary) {

                $('.Address').text(item.Address.AddressLine1);

                if (item.Address.AddressLine2 && item.Address.AddressLine2.length > 0) {
                    $('.address').after($('<div>').addClass('address2').text(item.Address.AddressLine2));
                }

             //   $('.CityStateZip').text(item.Address.City + ', ' + item.Address.State.DisplayName + item.Address.PostalCode);

            }

        });

    }

}

/* Demograpics Section */

function LoadDenominationsTagBox() {
    LoadTagBoxes('tagBoxDenominations', 'tagDenominationsContainer', 'denominations');
}

function LoadEthnicitiesTagBox() {
    LoadTagBoxes('tagBoxEthnicities', 'tagEthnicitiesContainer', 'ethnicities');
}

/* End Demographics Section */

function LoadRelationshipsData() {
    $.ajax({
        type: Links.GetRelationship.method,
        url: Links.GetRelationship.Href,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {
            LoadRelationshipsQuickView(data);
            LoadRelationshipsTab(data);
        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during the loading of the Relationships.');
        }
    });
}

function LoadRelationshipsQuickView(data) {
    var formattedData = "<ul>";
    if (data.Data && Array.isArray(data.Data)) {
        data.Data.forEach(function (eachItem) {
            if (eachItem.RelationshipType.RelationshipCategory.IsShownInQuickView === true) {
                formattedData = formattedData + "<li>" + eachItem.DisplayName + "</li>";
            };
        });
    }
    formattedData = formattedData + "</ul>";
    $('.relationshipsQuickView').html(formattedData);
}

function LoadRelationshipsTab(data) {
    var columns = [
        { dataField: 'RelationshipType.RelationshipCategory.DisplayName', caption: 'Category', groupIndex: 0 },
        { dataField: 'DisplayName', caption: 'Relationship' },
    ];
    LoadGridFromHateoas("relationshipsgrid",
        "relationshipstable",
        columns,
        Links.GetRelationship.Href,
        null,
        EditRelationship,
        DeleteEntity,
        "Delete that ",
        data);

}

function LoadRelationshipsGrid() {

    LoadRelationshipsData();
}

function EditRelationship(getUrl, patchUrl) {
    EditEntity(getUrl, patchUrl, "Relationship", ".relationshipmodal", ".saverelationship", 250, LoadRelationship, LoadRelationshipsGrid, GetRelationshipToSave);
}

function NewRelationshipModal() {
    NewEntityModal("Relationship", ".newrelationshipmodal", ".relationshipmodal", 250, PrePopulateNewRelationshipModal, ".saverelationship", GetRelationshipToSave, Links.NewRelationship.Method, Links.NewRelationship.Href, LoadRelationshipsGrid);
}

function LoadRelationship(url, modal) {
    LoadEntity(url, modal, "GET", LoadRelationshipData, "Relationship");
}

function GetRelationshipToSave(modal, isUpdate) {
    var item = {
        Constituent1Id: $('.FormattedName1').val(),
        Constituent2Id: $(modal).find('.FormattedName2').val(),
        RelationshipTypeId: $(modal).find('.RelationshipTypeId').val(),
    }
    if (isUpdate === true) {
        item.Id = $(modal).find('.hidrelationshipid').val();
    }
    return item;
}

function PrePopulateNewRelationshipModal(modal) {
    $(modal).find('.FormattedName1').val($('.hidconstituentid').val());
}

function LoadRelationshipData(data, modal) {
    $(modal).find('.hidrelationshipid').val(data.Data.Id);
    $(modal).find('.FormattedName1').val(data.Data.Constituent1Id);
    $(modal).find('.FormattedName2').val(data.Data.Constituent2Id);
    $(modal).find('.RelationshipTypeId').val(data.Data.RelationshipTypeId);
}

/* Doing Business As Section */
function LoadDBAGrid() {

    var columns = [
        { dataField: 'Id', width: '0px', },
        { dataField: 'StartDate', caption: 'From', dataType: 'date' },
        { dataField: 'EndDate', caption: 'To', dataType: 'date' },
        { dataField: 'Name', caption: 'Name' }
    ];

    LoadGrid('dbagrid',
        'doingbusinessastable',
        columns,
        Links.GetDoingBusinessAs.Href,
        null,
        EditDBA);

}

function NewDBAModal() {

    $('.newdbamodallink').click(function (e) {

        e.preventDefault();

        var modal = $('.dbamodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 250,
            resizable: false
        });

        $('.cancelmodal').click(function (e) {

            e.preventDefault();

            CloseModal(modal);

        });

        $('.savedba').unbind('click');

        $('.savedba').click(function () {

            var item = {
                ConstituentId: $('.hidconstituentid').val(),
                Name: $(modal).find('.DBAName').val(),
                StartDate: $(modal).find('.StartDate').val(),
                EndDate: $(modal).find('.EndDate').val()
            }

            $.ajax({
                type: Links.NewDoingBusinessAs.Method,
                url: Links.NewDoingBusinessAs.Href,
                data: item,
                contentType: 'application/x-www-form-urlencoded',
                crossDomain: true,
                success: function () {

                    DisplaySuccessMessage('Success', 'Doing Business As saved successfully.');

                    CloseModal(modal);

                    LoadDBAGrid();

                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', 'An error occurred during saving the Doing Business As.');
                }
            });

        });

    });

}

function EditDBA(id) {

    var modal = $('.dbamodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    LoadDBA(id, modal);

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.savedba').unbind('click');

    $('.savedba').click(function () {

        var item = {
            Id: $(modal).find('.hiddbaid').val(),
            ConstituentId: $('.hidconstituentid').val(),
            Name: $(modal).find('.DBAName').val(),
            StartDate: $(modal).find('.StartDate').val(),
            EndDate: $(modal).find('.EndDate').val()
        }

        $.ajax({
            type: 'PATCH',
            url: WEB_API_ADDRESS + 'doingbusinessas/' + $(modal).find('.hiddbaid').val(),
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Doing Business As saved successfully.');

                CloseModal(modal);

                LoadDBAGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the Doing Business As.');
            }
        });

    });

}

function LoadDBA(id, modal) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'doingbusinessas/' + id,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            $(modal).find('.hiddbaid').val(data.Data.Id);
            $(modal).find('.StartDate').val(data.Data.StartDate);
            $(modal).find('.EndDate').val(data.Data.EndDate);
            $(modal).find('.DBAName').val(data.Data.Name);

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the Doing Business As.');
        }
    });

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

    LoadGrid('educationgrid',
        'educationgridcontainer',
        columns,
        Links.GetEducation.Href,
        null,
        EditEducationModal);

}

function EducationModalDropDowns() {

    PopulateDropDown('.ed-Degree', 'degrees', '', '', null);
    PopulateDropDown('.ed-School', 'schools', '', '', null);

}

function NewEducationModal() {

    $('.neweducationmodallink').click(function (e) {

        e.preventDefault();

        var modal = $('.educationmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 600,
            resizable: false
        });

        EducationModalDropDowns();

        $('.cancelmodal').click(function (e) {

            e.preventDefault();

            CloseModal(modal);

        });

        $('.saveeducation').unbind('click');
        $('.saveeducation').click(function () {

            var item = {
                ConstituentId: $('.hidconstituentid').val(),
                Major: $('.ed-Major').val(),
                StartDate: $('.ed-StartDate').val(),
                EndDate: $('.ed-EndDate').val(),
                SchoolId: $('.ed-School').val(),
                DegreeId: $('.ed-Degree').val()
            }

            $.ajax({
                type: Links.NewEducation.Method,
                url: Links.NewEducation.Href,
                data: item,
                contentType: 'application/x-www-form-urlencoded',
                crossDomain: true,
                success: function () {

                    DisplaySuccessMessage('Success', 'Education saved successfully.');

                    CloseModal(modal);

                    LoadEducationGrid();

                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', 'An error occurred during saving the education.');
                }
            });

        });

    });

}

function EditEducationModal(id) {

    var modal = $('.educationmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 600,
        resizable: false
    });

    LoadEducation(id);

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.saveeducation').unbind('click');
    $('.saveeducation').click(function () {

        var item = {
            ConstituentId: $('.hidconstituentid').val(),
            Major: $('.ed-Major').val(),
            StartDate: $('.ed-StartDate').val(),
            EndDate: $('.ed-EndDate').val(),
            SchoolId: $('.ed-School').val(),
            DegreeId: $('.ed-Degree').val()
        }

        $.ajax({
            type: 'PATCH',
            url: WEB_API_ADDRESS + 'educations/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function() {

                DisplaySuccessMessage('Success', 'Education saved successfully.');

                CloseModal(modal);

                LoadEducationGrid();

            },
            error: function(xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the education.');
            }
        });

    });

}

function LoadEducation(id) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'educations/' + id,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            $('.ed-Major').val(data.Data.Major),
            $('.ed-StartDate').val(data.Data.StartDate),
            $('.ed-EndDate').val(data.Data.EndDate),
            $('.ed-School').val(data.Data.SchoolId),
            $('.ed-Degree').val(data.Data.DegreeId)

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the address.');
        }
    });

}
/* End Education Section */

/* Payment Preference Section */
function LoadPaymentPreferencesTable() {

    $('.paymentpreferencestable').dxDataGrid({
        dataSource: currentEntity.PaymentPreferences,
        columns: [
            { dataField: 'Name', caption: 'Description', },
            { dataField: 'ABANumber', caption: 'ABA Number' },
            { dataField: 'AccountNumber', caption: 'Account Number' },
            { dataField: '', caption: 'Ch/S' },
            { dataField: '', caption: 'Notes' }
        ],
        paging: {
            pageSize: 15
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
        },
        onRowClick: function (info) {
            DisplayConstituent(info.values[0]);
        }
    });

}
/* End Payment Preference Section */

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


/* Alternate Id Section */

function LoadAlternateIDTable() {

    var columns = [
            { dataField: 'Id', width:'0px' },
            { dataField: 'Name', caption: 'Name' }
    ];

    LoadGrid('altidgrid',
       'alternateidgridcontainer',
       columns,
       Links.GetAlternateId.Href,
       null,
       EditAlternateId);
}

function NewAlternateIdModal() {

    $('.newaltidmodal').click(function (e) {

        e.preventDefault();

        var modal = $('.alternateidmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 250,
            resizable: false
        });
        $('.submitaltid').unbind('click');

        $('.submitaltid').click(function () {

            var item = {
                ConstituentId: $('.hidconstituentid').val(),
                Name: $(modal).find('.ai-Name').val()
            }

            $.ajax({
                type: Links.NewAlternateId.Method,
                url: Links.NewAlternateId.Href,
                data: item,
                contentType: 'application/x-www-form-urlencoded',
                crossDomain: true,
                success: function () {

                    DisplaySuccessMessage('Success', 'Alternate Id saved successfully.');

                    CloseModal(modal);

                    LoadAlternateIDTable();

                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', 'An error occurred during saving the Alternate Id');
                }
            });

        });
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

   

}

function EditAlternateId(id) {

    var modal = $('.alternateidmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    LoadAlternateId(id, modal);

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitaltid').unbind('click');

    $('.submitaltid').click(function () {

        var item = {
            Id: id,
            ConstituentId: $('.hidconstituentid').val(),
            Name: $(modal).find('.ai-Name').val()
        }

        $.ajax({
            type: 'PATCH',
            url: WEB_API_ADDRESS + 'alternateids/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Alternate Id saved successfully.');

                CloseModal(modal);

                LoadAlternateIDTable();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the Alternate Id.');
            }
        });

    });

}

function LoadAlternateId(id, modal) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'alternateids/' + id,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            $(modal).find('.ai-Name').val(data.Data.Name);

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the Alternate Id.');
        }
    });

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
        Links.GetConstituentAddress.Href,
        null,
        EditAddressModal);

}

function NewAddressModal() {

    $('.newaddressmodallink').click(function (e) {

        e.preventDefault();

        var modal = $('.addressmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 375,
            resizable: false
        });

        $('.cancelmodal').click(function (e) {

            e.preventDefault();

            CloseModal(modal);

        });

        $('.saveaddress').unbind('click');

        $('.saveaddress').click(function () {

            var item = {
                ConstituentId: $('.hidconstituentid').val(),
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
                type: Links.NewConstituentAddress.Method,
                url: Links.NewConstituentAddress.Href,
                data: item,
                contentType: 'application/x-www-form-urlencoded',
                crossDomain: true,
                success: function () {

                    DisplaySuccessMessage('Success', 'Address saved successfully.');

                    CloseModal(modal);

                    LoadAddressesGrid();

                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', 'An error occurred during saving the address.');
                }
            });

        });

        AutoZip(modal);

    });

    PopulateAddressTypesInModal(null);

    PopulateCountriesInModal(null);

    LoadRegions('regionscontainer', 'na-');

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

    var modal = $('.addressmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 375,
        height: 560,
        resizable: false
    });

    PopulateAddressTypesInModal(null);

    PopulateCountriesInModal(null);

    AutoZip(modal);

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

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the address.');
            }
        })

    });

}

function GetEditedAddressFields() {

    var item = {
        Id: $(modal).find('.hidconstituentaddressid').val(),
        ConstituentId: $('.hidconstituentid').val(),
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

            LoadRegionDropDown('.na-', 1, null, data.Data.Address.Region1Id);
            LoadRegionDropDown('.na-', 2, data.Data.Address.Region1Id, data.Data.Address.Region2Id);

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the address.');
        }
    });

}

function GenerateContactInfoSection() {
   
    // Grab all the contact categories, then dynamically create the accordions
    LoadCategories(function(data) {

        var categoryData = data.Data;

        categoryData.forEach(function (category) {

            $("h1:contains('Addresses')").next('div').after($('<h3>').text(category.SectionTitle)); 
            // most of our accordions use h1, but for some reason accordions.refresh() only works with h3.

        });

        ContactInfoNewModalButtonsAndDivs();

        $('.accordions').accordion('refresh');

        ContactInfoLoadAndAddModals();

    });

}

function ContactInfoLoadAndAddModals() {

    LoadPhoneNumbersTable();

    NewPhoneNumberModal();

    LoadEmailTable();

    NewEmailModal();

    LoadWebsiteTable();

    NewWebsiteModal();

    LoadPointOfContactTable();

    NewPointOfContactModal();

    LoadSocialMediaTable();

    NewSocialMediaModal();

    LoadOtherContactsTable();

    NewOtherContactsModal();
}

function ContactInfoNewModalButtonsAndDivs() {

    var modalClassNames = ['website', 'socmed', 'phone', 'poc', 'other', 'email'];

    var classNamesIndex = 0;

    $('h1:contains("Contact Information") ~ div div.accordions h3').each(function () {
        // most of our accordions use h1, but for some reason accordion.refresh only works with h3.

        $(this).append($('<a>', { title: 'New', class: 'new' + modalClassNames[classNamesIndex] + 'modallink'+ ' newbutton', href: '#' }));

        $(this).after($('<div>', { class: 'constituent' + modalClassNames[classNamesIndex] + 'gridcontainer' }));

        classNamesIndex++;

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
            DisplayErrorMessage('Error', 'An error occurred during loading the Contact Categories.');
        }
    });
}

// Phone # Subsection
function LoadPhoneNumbersTable() {

    var columns = [ 
        { dataField: 'Id', width: '0px' },
        { dataField: 'IsPreferred', caption: 'Is Preferred' },
        { dataField: 'ContactType', caption: 'Type' },
        { dataField: 'Info', caption: 'Phone #' },
        { dataField: 'Comment', caption: 'Comment' }
    ];

    LoadGrid('constituentphonegrid',
       'constituentphonegridcontainer',
       columns,
       'http://localhost:49490/api/v1/contactinfo/A7FDB5B3-6ED2-4D3B-8EB8-B551809DA5B1/' + currentEntity.Id,
       null,
       EditPhoneNumber);
}

function NewPhoneNumberModal() {

    $('.newphonemodallink').click(function (e) {

        e.preventDefault();

        PopulateDropDown('.pn-PhoneNumberType', 'contacttypes/A7FDB5B3-6ED2-4D3B-8EB8-B551809DA5B1', '', '');

         modal = $('.phonenumbermodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 250,
            resizable: false
        });

        $('.submitphonenumber').unbind('click');

        $('.submitphonenumber').click(function () {

            var item = {
                ConstituentId: $('.hidconstituentid').val(),
                ContactTypeId: $(modal).find('.pn-PhoneNumberType').val(),
                Info: $(modal).find('.pn-Info').val(),
                IsPreferred: $(modal).find('.pn-IsPreferred').prop('checked'), 
                Comment: $(modal).find('.pn-Comment').val()
            }

            $.ajax({
                type: 'POST',
                url: WEB_API_ADDRESS + 'contactinfo/', 
                data: item,
                contentType: 'application/x-www-form-urlencoded',
                crossDomain: true,
                success: function () {

                    DisplaySuccessMessage('Success', 'Phone Number saved successfully.');

                    CloseModal();

                    LoadPhoneNumbersTable();

                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', 'An error occurred during saving the Phone Number');
                }
            });

        });
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });



}

function EditPhoneNumber(id) {

    var modal = $('.phonenumbermodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    LoadPhoneNumber(id, modal);

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitphonenumber').unbind('click');

    $('.submitphonenumber').click(function () {

        var item = {

            ConstituentId: $('.hidconstituentid').val(),
            ContactTypeId: $(modal).find('.pn-PhoneNumberType').val(),
            Info: $(modal).find('.pn-Info').val(),
            IsPreferred: $(modal).find('.pn-IsPreferred').prop('checked'),
            Comment: $(modal).find('.pn-Comment').val()

        }

        $.ajax({
            type: 'PATCH',
            url: WEB_API_ADDRESS + 'contactinfo/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Phone Number saved successfully.');

                CloseModal(modal);

                LoadPhoneNumbersTable();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the Phone Number.');
            }
        });

    });

}

function LoadPhoneNumber(id, modal) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'contactinfo/' + id, 
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            $(modal).find('.pn-Info').val(data.Data.Info);
            $(modal).find('.pn-IsPreferred').prop('checked', data.Data.IsPreferred);
            $(modal).find('.pn-Comment').val(data.Data.Comment);

            PopulateDropDown('.pn-PhoneNumberType', 'contacttypes/A7FDB5B3-6ED2-4D3B-8EB8-B551809DA5B1');

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the Phone Number.');
        }
    });

}
// End Phone # Subsection

// Emails Subsection
function LoadEmailTable() {

    var columns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'IsPreferred', caption: 'Is Preferred' },
        { dataField: 'ContactType', caption: 'Type' },
        { dataField: 'Info', caption: 'Email' },
        { dataField: 'Comment', caption: 'Comment' }
    ];

    LoadGrid('constituentemailgrid',
        'constituentemailgridcontainer',
        columns,
        'http://localhost:49490/api/v1/contactinfo/A4BBF374-4C47-45D7-AF2E-92C81F3BADFA/' + currentEntity.Id,
        null,
        EditEmail);
}

function NewEmailModal() {

    $('.newemailmodallink').click(function (e) {

        e.preventDefault();

        PopulateDropDown('.e-EmailType', 'contacttypes/A4BBF374-4C47-45D7-AF2E-92C81F3BADFA/', '', '');

        modal = $('.emailmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 250,
            resizable: false
        });
        $('.submitemail').unbind('click');

        $('.submitemail').click(function () {

            var item = {
                ConstituentId: $('.hidconstituentid').val(),
                ContactTypeId: $(modal).find('.e-EmailType').val(),
                Info: $(modal).find('.e-Info').val(),
                IsPreferred: $(modal).find('.e-IsPreferred').prop('checked'),
                Comment: $(modal).find('.e-Comment').val()
            }

            $.ajax({
                type: 'POST',
                url: WEB_API_ADDRESS + 'contactinfo/',
                data: item,
                contentType: 'application/x-www-form-urlencoded',
                crossDomain: true,
                success: function () {

                    DisplaySuccessMessage('Success', 'Email saved successfully.');

                    CloseModal(modal);

                    LoadEmailTable();

                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', 'An error occurred during saving the Email');
                }
            });

        });
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

}

function EditEmail(id) {

   var modal = $('.emailmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    LoadEmail(id, modal);

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitemail').unbind('click');

    $('.submitemail').click(function () {

        var item = {

            ConstituentId: $('.hidconstituentid').val(),
            ContactTypeId: $(modal).find('.e-EmailType').val(),
            Info: $(modal).find('.e-Info').val(),
            IsPreferred: $(modal).find('.e-IsPreferred').prop('checked'),
            Comment: $(modal).find('.e-Comment').val()

        }

        $.ajax({
            type: 'PATCH',
            url: WEB_API_ADDRESS + 'contactinfo/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Email saved successfully.');

                CloseModal(modal);

                LoadEmailTable();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the Email.');
            }
        });

    });

}

function LoadEmail(id, modal) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'contactinfo/' + id,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            $(modal).find('.e-Info').val(data.Data.Info);
            $(modal).find('.e-IsPreferred').prop('checked', data.Data.IsPreferred);
            $(modal).find('.e-Comment').val(data.Data.Comment);

            PopulateDropDown('.e-EmailType', 'contacttypes/A4BBF374-4C47-45D7-AF2E-92C81F3BADFA', '', '');

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the Phone Number.');
        }
    });

}
// End Emails Subsection

// Websites Subsection
function LoadWebsiteTable() {

    var columns = [ // double check these against models
        { dataField: 'Id', width: '0px' },
        { dataField: 'IsPreferred', caption: 'Is Preferred' },
        { dataField: 'ContactType', caption: 'Type' },
        { dataField: 'Info', caption: 'URL:' },
        { dataField: 'Comment', caption: 'Comment' }
    ];

    LoadGrid('constituentwebsitegrid',
        'constituentwebsitegridcontainer',
        columns,
        'http://localhost:49490/api/v1/contactinfo/D6595D53-2A76-4E8A-A1F1-61E766564349/' + currentEntity.Id,
        null,
        EditWebsite);
}

function NewWebsiteModal() {

    $('.newwebsitemodallink').click(function (e) {

        e.preventDefault();

        PopulateDropDown('.ws-WebSiteType', 'contacttypes/D6595D53-2A76-4E8A-A1F1-61E766564349', '', '');

        modal = $('.websitemodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 250,
            resizable: false
        });

        $('.submitwebsite').unbind('click');

        $('.submitwebsite').click(function () {

            var item = {
                ConstituentId: $('.hidconstituentid').val(),
                ContactTypeId: $(modal).find('.ws-WebSiteType').val(),
                Info: $(modal).find('.ws-Info').val(),
                IsPreferred: $(modal).find('.ws-IsPreferred').prop('checked'),
                Comment: $(modal).find('.ws-Comment').val()
            }

            $.ajax({
                type: 'POST',
                url: WEB_API_ADDRESS + 'contactinfo/',
                data: item,
                contentType: 'application/x-www-form-urlencoded',
                crossDomain: true,
                success: function () {

                    DisplaySuccessMessage('Success', 'Web Site saved successfully.');

                    CloseModal(modal);

                    LoadWebsiteTable();

                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', 'An error occurred during saving the Web Site');
                }
            });

        });
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });



}

function EditWebsite(id) {

    var modal = $('.websitemodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    LoadWebsite(id, modal);

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitwebsite').unbind('click');

    $('.submitwebsite').click(function () {

        var item = {

            ConstituentId: $('.hidconstituentid').val(),
            ContactTypeId: $(modal).find('.ws-WebSiteType').val(),
            Info: $(modal).find('.ws-Info').val(),
            IsPreferred: $(modal).find('.ws-IsPreferred').prop('checked'),
            Comment: $(modal).find('.ws-Comment').val()

        }

        $.ajax({
            type: 'PATCH',
            url: WEB_API_ADDRESS + 'contactinfo/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Web Site saved successfully.');

                CloseModal(modal);

                LoadWebsiteTable();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the Web Site.');
            }
        });

    });

}

function LoadWebsite(id, modal) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'contactinfo/' + id,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            $(modal).find('.ws-Info').val(data.Data.Info);
            $(modal).find('.ws-IsPreferred').prop('checked', data.Data.IsPreferred);
            $(modal).find('.ws-Comment').val(data.Data.Comment);

            PopulateDropDown('.ws-WebSiteType', 'contacttypes/D6595D53-2A76-4E8A-A1F1-61E766564349', '', '');

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the Web Site.');
        }
    });

}
// End Websites Subsection

// Point Of Contact Subsection
function LoadPointOfContactTable() {

    var columns = [ // double check these against models
        { dataField: 'Id', width: '0px' },
        { dataField: 'IsPreferred', caption: 'Is Preferred' },
        { dataField: 'ContactType', caption: 'Type' },
        { dataField: 'Info', caption: 'Info' },
        { dataField: 'Comment', caption: 'Comment' }
    ];

    LoadGrid('constituentpocgrid',
        'constituentpocgridcontainer',
        columns,
        'http://localhost:49490/api/v1/contactinfo/0A613EF8-A432-459D-9158-04E6D2C11F7B/' + currentEntity.Id,
        null,
        EditPointOfContact);
}

function NewPointOfContactModal() {

    $('.newpocmodallink').click(function (e) {

        e.preventDefault();

        PopulateDropDown('.poc-PocType', 'contacttypes/0A613EF8-A432-459D-9158-04E6D2C11F7B', '', '');

        modal = $('.pocmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 250,
            resizable: false
        });
        $('.submitpoc').unbind('click');

        $('.submitpoc').click(function () {

            var item = {
                ConstituentId: $('.hidconstituentid').val(),
                ContactTypeId: $(modal).find('.poc-PocType').val(),
                Info: $(modal).find('.poc-Info').val(),
                IsPreferred: $(modal).find('.poc-IsPreferred').prop('checked'),
                Comment: $(modal).find('.poc-Comment').val()
            }

            $.ajax({
                type: 'POST',
                url: WEB_API_ADDRESS + 'contactinfo/',
                data: item,
                contentType: 'application/x-www-form-urlencoded',
                crossDomain: true,
                success: function () {

                    DisplaySuccessMessage('Success', 'Point of Contact saved successfully.');

                    CloseModal(modal);

                    LoadPointOfContactTable();

                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', 'An error occurred during saving the Point of Contact');
                }
            });

        });
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });



}

function EditPointOfContact(id) {

    var modal = $('.pocmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    LoadPointOfContact(id, modal);

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitpoc').unbind('click');

    $('.submitpoc').click(function () {

        var item = {

            ConstituentId: $('.hidconstituentid').val(),
            ContactTypeId: $(modal).find('.poc-PocType').val(),
            Info: $(modal).find('.poc-Info').val(),
            IsPreferred: $(modal).find('.poc-IsPreferred').prop('checked'),
            Comment: $(modal).find('.poc-Comment').val()

        }

        $.ajax({
            type: 'PATCH',
            url: WEB_API_ADDRESS + 'contactinfo/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Point of Contact saved successfully.');

                CloseModal(modal);

                LoadPointOfContactTable();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the Point of Contact.');
            }
        });

    });

}

function LoadPointOfContact(id, modal) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'contactinfo/' + id,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            $(modal).find('.poc-Info').val(data.Data.Info);
            $(modal).find('.poc-IsPreferred').prop('checked', data.Data.IsPreferred);
            $(modal).find('.poc-Comment').val(data.Data.Comment);

            PopulateDropDown('.poc-PocType', 'contacttypes/0A613EF8-A432-459D-9158-04E6D2C11F7B', '', '');

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the Point of Contact.');
        }
    });

}
// End Point Of Contact Subsection

// Social Media Subsection
function LoadSocialMediaTable() {

    var columns = [ // double check these against models
        { dataField: 'Id', width: '0px' },
        { dataField: 'IsPreferred', caption: 'Is Preferred' },
        { dataField: 'ContactType', caption: 'Type' },
        { dataField: 'Info', caption: 'URL' },
        { dataField: 'Comment', caption: 'Comment' }
    ];

    LoadGrid('constituentsocmedgrid',
        'constituentsocmedgridcontainer',
        columns,
       'http://localhost:49490/api/v1/contactinfo/26193F4C-A8A5-4730-AB93-2BD84CA121A4/' + currentEntity.Id,
        null,
        EditPhoneNumber);
}

function NewSocialMediaModal() {

    $('.newsocmedmodallink').click(function (e) {

        e.preventDefault();

        PopulateDropDown('.sm-SocialMediaType', 'contacttypes/26193F4C-A8A5-4730-AB93-2BD84CA121A4', '', '');

        modal = $('.socmedmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 250,
            resizable: false
        });
        $('.submitsocmed').unbind('click');

        $('.submitsocmed').click(function () {

            var item = {
                ConstituentId: $('.hidconstituentid').val(),
                ContactTypeId: $(modal).find('.sm-SocialMediaType').val(),
                Info: $(modal).find('.sm-Info').val(),
                IsPreferred: $(modal).find('.sm-IsPreferred').prop('checked'),
                Comment: $(modal).find('.sm-Comment').val()
            }

            $.ajax({
                type: 'POST',
                url: WEB_API_ADDRESS + 'contactinfo/',
                data: item,
                contentType: 'application/x-www-form-urlencoded',
                crossDomain: true,
                success: function () {

                    DisplaySuccessMessage('Success', 'Social Media saved successfully.');

                    CloseModal();

                    LoadSocialMediaTable();

                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', 'An error occurred during saving the Social Media');
                }
            });

        });
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal();

    });



}

function EditSocialMedia(id) {

    modal = $('.socmedmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    LoadPhoneNumber(id);

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal();

    });

    $('.submitsocmed').unbind('click');

    $('.submitsocmed').click(function () {

        var item = {

            ConstituentId: $('.hidconstituentid').val(),
            ContactType: $(modal).find('.sm-PhoneNumberType').val(),
            Info: $(modal).find('.sm-Info').val(),
            IsPreferred: $(modal).find('.sm-IsPreferred').prop('checked'),
            Comment: $(modal).find('.sm-Comment').val()

        }

        $.ajax({
            type: 'PATCH',
            url: WEB_API_ADDRESS + 'contactinfo/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Social Media saved successfully.');

                CloseModal();

                LoadSocialMediaTable();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the Phone Number.');
            }
        });

    });

}

function LoadSocialMedia(id) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'contactinfo/' + id,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            $(modal).find('.pn-Info').val(data.Data.Info);
            $(modal).find('.pn-IsPreferred').prop('checked', data.Data.IsPreferred);
            $(modal).find('.pn-Comment').val(data.Data.Comment);

            PopulateDropDown('.sm-SocialMediaType', 'contacttypes/26193F4C-A8A5-4730-AB93-2BD84CA121A4', '', '');

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the Phone Number.');
        }
    });

}
// End Social Media Subsection

// Other Contacts Subsection
function LoadOtherContactsTable() {

    var columns = [ // double check these against models
        { dataField: 'Id', width: '0px' },
        { dataField: 'IsPreferred', caption: 'Is Preferred' },
        { dataField: 'ContactType', caption: 'Type' },
        { dataField: 'Info', caption: 'Phone #' },
        { dataField: 'Comment', caption: 'Comment' }
    ];

    LoadGrid('constituentothergrid',
        'constituentothergridcontainer',
        columns,
        'http://localhost:49490/api/v1/contactinfo/DD0417C2-66EC-4F2F-8A43-AF1D90AC5791/' + currentEntity.Id,
        null,
        EditOtherContacts);
}

function NewOtherContactsModal() {

    $('.newothermodallink').click(function (e) {

        PopulateDropDown('.o-OtherType', 'contacttypes/DD0417C2-66EC-4F2F-8A43-AF1D90AC5791', '', '');

        e.preventDefault();

        modal = $('.othermodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 250,
            resizable: false
        });
        $('.submitother').unbind('click');

        $('.submitother').click(function () {

            var item = {
                ConstituentId: $('.hidconstituentid').val(),
                ContactTypeId: $(modal).find('.o-OtherType').val(),
                Info: $(modal).find('.o-Info').val(),
                IsPreferred: $(modal).find('.o-IsPreferred').prop('checked'),
                Comment: $(modal).find('.o-Comment').val()
            }

            $.ajax({
                type: 'POST',
                url: WEB_API_ADDRESS + 'contactinfo/',
                data: item,
                contentType: 'application/x-www-form-urlencoded',
                crossDomain: true,
                success: function () {

                    DisplaySuccessMessage('Success', 'Other Contact saved successfully.');

                    CloseModal(modal);

                    LoadOtherContactsTable();

                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', 'An error occurred during saving the Other Contact');
                }
            });

        });
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });



}

function EditOtherContacts(id) {

    var modal = $('.othermodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    LoadOtherContacts(id, modal);

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.submitother').unbind('click');

    $('.submitother').click(function () {

        var item = {

            ConstituentId: $('.hidconstituentid').val(),
            ContactTypeId: $(modal).find('.o-OtherType').val(),
            Info: $(modal).find('.o-Info').val(),
            IsPreferred: $(modal).find('.o-IsPreferred').prop('checked'),
            Comment: $(modal).find('.o-Comment').val()

        }

        $.ajax({
            type: 'PATCH',
            url: WEB_API_ADDRESS + 'contactinfo/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Other Contact saved successfully.');

                CloseModal(modal);

                LoadOtherContactsTable();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the Other Contacts.');
            }
        });

    });

}

function LoadOtherContacts(id, modal) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'contactinfo/' + id,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            $(modal).find('.o-Info').val(data.Data.Info);
            $(modal).find('.o-IsPreferred').prop('checked', data.Data.IsPreferred);
            $(modal).find('.o-Comment').val(data.Data.Comment);

            PopulateDropDown('.o-OtherType', 'contacttypes/DD0417C2-66EC-4F2F-8A43-AF1D90AC5791', '', '');

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the Other Contact.');
        }
    });

}
// End Other Contacts Subsection


/* End Contact Information Section */