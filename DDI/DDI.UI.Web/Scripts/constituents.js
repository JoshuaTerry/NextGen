
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
    PopulateDropDown('.AccountTypeId', 'paymentpreferences/accounttypes', '', '');
    // PopulateDropDown('.PreferredPaymentMethod', 'paymentpreferences/paymentmethods', '', '');
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

        LoadDenominationsTagBox();

        LoadDBAGrid();

        NewDBAModal();

        LoadEducationGrid();

        NewEducationModal();

        LoadEthnicitiesTagBox();

        LoadPaymentPreferencesTable();

        NewPaymentPreference();

        LoadContactInfo();

        NewAddressModal();

        LoadAlternateIDTable();

        NewAlternateIdModal();

        PopulateMonthDays();

        AmendMonthDays();

        $('.BirthDay').val(currentEntity.BirthDay);
	
	PopulateUserIdDropDown();
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

        modal = $('.dbamodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 250,
            resizable: false
        });

        $('.cancelmodal').click(function (e) {

            e.preventDefault();

            CloseModal();

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

                    CloseModal();

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

    modal = $('.dbamodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    LoadDBA(id);

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal();

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

                CloseModal();

                LoadDBAGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the Doing Business As.');
            }
        });

    });

}

function LoadDBA(id) {

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

        modal = $('.educationmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 600,
            resizable: false
        });

        EducationModalDropDowns();

        $('.cancelmodal').click(function (e) {

            e.preventDefault();

            CloseModal();

        });

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

                    CloseModal();

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

    modal = $('.educationmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 600,
        resizable: false
    });

    EducationModalDropDowns();

    LoadEducation(id);

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal();

    });

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
            success: function () {

                DisplaySuccessMessage('Success', 'Education saved successfully.');

                CloseModal();

                LoadEducationGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the education.');
            }
        })

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

    var columns = [
            { dataField: 'Id', width: '0px' },
            { dataField: 'Name', caption: 'Description' },
            { dataField: 'RoutingNumber', caption: 'Routing Number' },
            { dataField: 'BankAccount', caption: 'Account Number' },
            { dataField: 'AccountType', caption: 'Ch/S' },
            { dataField: '', caption: 'Notes' }
    ];

    LoadGrid('paymentpreferencesgrid',
        'paymentpreferencesgridcontainer',
        columns,
        Links.GetPaymentPreference.Href,
        null,
        EditPaymentPreference);
    
}

function NewPaymentPreference() {

    $('.newppmodallink').click(function (e) {

        e.preventDefault();

        modal = $('.paymentpreferencemodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 600,
            resizable: false
        });

    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal();

    });

    $('.savepaymentpreference').unbind('click');

    $('.savepaymentpreference').click(function () {

        var date = new Date();

        var item = {
            ConstituentId: $('.hidconstituentid').val(),
            Description: $(modal).find('.pp-Description').val(),
            BankName: $(modal).find('.pp-BankName').val(),
            RoutingNumber: $(modal).find('.pp-RoutingNumber').val(),
            BankAccount: $(modal).find('.pp-AccountNumber').val(),
            AccountType: $(modal).find('.pp-AccountType').val(),
            Status: $(modal).find('.pp-Status').val(),
            StatusDate: date.getMonth() + 1 + '/' + date.getDate() + '/' + date.getFullYear()
        };

        $.ajax({
            type: Links.NewPaymentPreference.Method,
            url: Links.NewPaymentPreference.Href,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Payment Method saved successfully.');

                CloseModal();

                LoadPaymentPreferencesTable();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the payment method.');
            }
        });

    });

}

function EditPaymentPreference(id) {

    modal = $('.paymentpreferencemodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 600,
        resizable: false
    });

    LoadPaymentPreference(id);

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal();

    });

    $('.savepaymentpreference').unbind('click');

    $('.savepaymentpreference').click(function () {

        var date = '';
        var prevStatus = $(modal).find('.pp-PreviousStatus').val();
        var selectedStatus = $(modal).find('.pp-Status').val();
        
        if (prevStatus != selectedStatus) {

            var newDate = new Date();

            date = newDate.getMonth() + 1 + '/' + newDate.getDate() + '/' + newDate.getFullYear();

        }

        var item = {
            ConstituentId: $('.hidconstituentid').val(),
            Description: $(modal).find('.pp-Description').val(),
            BankName: $(modal).find('.pp-BankName').val(),
            RoutingNumber: $(modal).find('.pp-RoutingNumber').val(),
            BankAccount: $(modal).find('.pp-AccountNumber').val(),
            AccountType: $(modal).find('.pp-AccountType').val(),
            Status: $(modal).find('.pp-Status').val(),
            StatusDate: date
        };

        $.ajax({
            type: 'PATCH',
            url: WEB_API_ADDRESS + 'paymentpreferences/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Payment Method saved successfully.');

                CloseModal();

                LoadPaymentPreferencesTable();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the payment method.');
            }
        });

    });

}

function LoadPaymentPreference(id) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'paymentpreferences/' + id,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            $(modal).find('.pp-Description').val(data.Data.Description);
            $(modal).find('.pp-BankName').val(data.Data.BankName);
            $(modal).find('.pp-RoutingNumber').val(data.Data.RoutingNumber);
            $(modal).find('.pp-AccountNumber').val(data.Data.BankAccount);
            $(modal).find('.pp-AccountType').val(data.Data.AccountType);
            $(modal).find('.pp-Status').val(data.Data.Status);
            $(modal).find('.pp-PreviousStatus').val(data.Data.Status);
            $(modal).find('.pp-StatusDate').val(FormatJSONDate(data.Data.StatusDate));

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the Payment Method.');
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

        modal = $('.alternateidmodal').dialog({
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

                    CloseModal();

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

        CloseModal();

    });

   

}

function EditAlternateId(id) {

    modal = $('.alternateidmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    LoadAlternateId(id);

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal();

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

                CloseModal();

                LoadAlternateIDTable();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the Alternate Id.');
            }
        });

    });

}

function LoadAlternateId(id) {

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

        modal = $('.addressmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 375,
            resizable: false
        });

        $('.cancelmodal').click(function (e) {

            e.preventDefault();

            CloseModal();

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

                    CloseModal();

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

    modal = $('.addressmodal').dialog({
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

        CloseModal();

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
/* End Contact Information Section */




