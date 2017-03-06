
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

function UploadFiles() {

    var modal = $('.fileuploadmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 400,
        resizable: false
    });

    InitializeFileUploader(WEB_API_ADDRESS + 'filestorage/upload');

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

    if (id.length > 9) {
        route += 'id/'; // If length > 9, id is probably a GUID.
    }
    else {
        route += 'number/'; // Otherwise it's likely a constituent number.
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

        DisplayConstituentSideBar();

        DisplayConstituentType();

        GenerateContactInfoSection();

	    LoadDenominationsTagBox();

        LoadDBAGrid();

        NewDBAModal();

        LoadEducationGrid();

        NewEducationModal();

        LoadEthnicitiesTagBox();

        LoadPaymentPreferencesTable();

        NewPaymentPreference();

        LoadContactInfo();

        LoadAlternateIDTable();

        NewAlternateIdModal();

        PopulateMonthDays();

        AmendMonthDays();

        $('.BirthDay').val(currentEntity.BirthDay);
	
        PopulateUserIdDropDown();

        LoadRelationshipsData();

        NewRelationshipModal();

        NewAddressModal();
    }
}

function DisplayConstituentSideBar() {

    $('.FormattedName').text(currentEntity.FormattedName);

    GetConstituentPrimaryAddress();

    GetConstituentPreferredContactInfo();

}

function DisplayConstituentType() {
    $('#tab-main-link').text(currentEntity.ConstituentType.DisplayName);
    if (currentEntity.ConstituentType.Category === 0) {
        $('.organizationConstituent').hide();
        $('.individualConstituent').show();
        $('.organizationSection').hide();
        $('.dbaSection').hide();
    } else {
        $('.organizationConstituent').show();
        $('.individualConstituent').hide();
        $('.personalSection').hide();
        $('.clergySection').hide();
        $('.educationSection').hide();
        $('.professionalSection').hide();
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
            DisplayErrorMessage('Error', 'An error occurred during loading the primary address.');
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
            DisplayErrorMessage('Error', 'An error occurred during loading the preferred contact info.');
        }
    });

}


/* Demograpics Section */
function LoadDenominationsTagBox() {
    LoadTagBoxes('tagBoxDenominations', 'tagDenominationsContainer', 'denominations', '/constituents/' + currentEntity.Id + '/denominations');
}

function LoadEthnicitiesTagBox() {
    LoadTagBoxes('tagBoxEthnicities', 'tagEthnicitiesContainer', 'ethnicities', '/constituents/' + currentEntity.Id + '/ethnicities');
}
/* End Demographics Section */


/* Relationships Tab */
function LoadRelationshipsData() {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'constituents/' + currentEntity.Id + '/relationships',
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

    var formattedData = $('<ul>').addClass('relationshipQuickViewData');

    if (data.Data) {

        $.map(data.Data, function (item) {

            if (item.RelationshipType.RelationshipCategory.IsShownInQuickView === true) {
                $('<li>').text(item.DisplayName).appendTo($(formattedData));
            }

        });

    }

    $(formattedData).appendTo($('.relationshipsQuickView'));

}

function LoadRelationshipsTab(data) {

    var columns = [
        { dataField: 'Id', width: '0px' },
        { dataField: 'RelationshipType.RelationshipCategory.DisplayName', caption: 'Category', groupIndex: 0 },
        { dataField: 'DisplayName', caption: 'Relationship' },
    ];

    LoadGridWithData('relationshipsgrid',
        'relationshipstable',
        columns,
        'relationships',
        null,
        EditRelationship,
        null,
        data);

}

function EditRelationship(id) {

    EditEntity('.relationshipmodal', '.saverelationship', 250, LoadRelationshipData, LoadRelationshipsData, GetRelationshipToSave, 'Relationship', 'relationships', id);

}

function NewRelationshipModal() {

    NewEntityModal('.newrelationshipmodal', '.relationshipmodal', '.saverelationship', 250, PrePopulateNewRelationshipModal, LoadRelationshipsData, GetRelationshipToSave, 'Replationship', 'relationships');

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
/* End Relationships Tab */


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
        'doingbusinessas',
        null,
        EditDBA,
        null);

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
                type: 'POST',
                url: WEB_API_ADDRESS + 'doingbusinessas',
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
        'educations',
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
                type: 'POST',
                url: WEB_API_ADDRESS + 'educations',
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

    LoadGrid('paymentpreferencesgrid',
        'paymentpreferencesgridcontainer',
        columns,
        'paymentmethods/constituents/' + currentEntity.Id,
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
            type: 'POST',
            url: WEB_API_ADDRESS + 'paymentmethods',
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

    var modal = $('.paymentpreferencemodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 600,
        resizable: false
    });

    LoadPaymentPreference(id, modal);

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

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
            url: WEB_API_ADDRESS + 'paymentmethods/' + id,
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

function LoadPaymentPreference(id, modal) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'paymentmethods/' + id,
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
       'alternateids',
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

        $('.cancelmodal').click(function (e) {

            e.preventDefault();

            CloseModal(modal);

        });

        $('.submitaltid').unbind('click');

        $('.submitaltid').click(function () {

            var item = {
                ConstituentId: $('.hidconstituentid').val(),
                Name: $(modal).find('.ai-Name').val()
            }

            $.ajax({
                type: 'POST',
                url: WEB_API_ADDRESS + 'alternateids',
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
        'constituentaddresses',
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

        $.map(data.Data, function (category) {

            // most of our accordions use h1, but for some reason accordions.refresh() only works with h3.
            var header = $('<h3>').text(category.SectionTitle).appendTo($('.contactinfocontainer'));
            $('<a>', { 
                title: 'New', 
                class: 'new' + category.Name.toLowerCase() + 'modallink' + ' newbutton', 
                href: '#'
            }).appendTo($(header));

            $('<div>').attr('id', category.Id).addClass('constituent' + category.Name + 'gridcontainer').appendTo($('.contactinfocontainer'));

            LoadContactCategoryGrid(category.Id, category.TextBoxLabel, category.Name);

        });
        
        ContactInfoAddModals();

        $('.accordions').accordion('refresh');
        // LoadAccordions will not work here

    });

}

function ContactInfoAddModals() {

    NewPhoneModal();

    NewEmailModal();

    NewWebModal();

    NewPersonModal();

    NewOtherModal();
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

function LoadContactCategoryGrid(categoryid, displayText, name, idField) {

    var columns = [
        { dataField:  'Id', width: '0px' }, 
        { dataField: 'IsPreferred', caption: 'Is Preferred' },
        { dataField: 'ContactType.Name', caption: 'Type' }, 
        { dataField: 'Info', caption: displayText },
        { dataField: 'Comment', caption: 'Comment' }
    ];

    LoadGrid('constituent' + name + 'grid',
        'constituent' + name + 'gridcontainer',
        columns,
        'contactinfo/' + categoryid + '/' + currentEntity.Id,
        null,
        function (id) { ExecuteFunction('Edit' + name, window, id) }); 
}

// Phone # Subsection
function NewPhoneModal() {

    $('.newphonemodallink').click(function (e) {

        e.preventDefault();

        var categoryId = $('.constituentPhonegridcontainer').attr('id');

        PopulateDropDown('.pn-PhoneNumberType', 'contacttypes/' + categoryId, '', '');

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
                success: function (data) {

                    DisplaySuccessMessage('Success', 'Phone Number saved successfully.');

                    CloseModal(modal);

                    var categoryid = $('.constituentPhonegridcontainer').attr('id');

                    LoadContactCategoryGrid(categoryid, 'Phone', 'Phone');

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

function EditPhone(id) {

    var modal = $('.phonenumbermodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    LoadPhone(id, modal);

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
            success: function (data) { 

                DisplaySuccessMessage('Success', 'Phone Number saved successfully.');

                CloseModal(modal);

                var category = data.Data.ContactType.ContactCategory;

                LoadContactCategoryGrid(category.Id, category.TextBoxLabel, category.Name);

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the Phone Number.');
            }
        });

    });

}

function LoadPhone(id, modal) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'contactinfo/' + id, 
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            PopulateDropDown('.pn-PhoneNumberType', 'contacttypes/' + $('.constituentPhonegridcontainer').attr('id'), '', '', data.Data.ContactTypeId);

            $(modal).find('.pn-Info').val(data.Data.Info);
            $(modal).find('.pn-PhoneNumberType').val(data.Data.ContactTypeId);
            $(modal).find('.pn-IsPreferred').prop('checked', data.Data.IsPreferred);
            $(modal).find('.pn-Comment').val(data.Data.Comment);

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the Phone Number.');
        }
    });

}
// End Phone # Subsection

// Emails Subsection
function NewEmailModal() {

    $('.newemailmodallink').click(function (e) {

        e.preventDefault();

        var categoryId = $('.constituentEmailgridcontainer').attr('id');

        PopulateDropDown('.e-EmailType', 'contacttypes/' + categoryId, '', '');

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

                    var categoryid = $('.constituentEmailgridcontainer').attr('id');

                    LoadContactCategoryGrid(categoryid, 'Email', 'Email');


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
            success: function (data) {

                DisplaySuccessMessage('Success', 'Email saved successfully.');

                CloseModal(modal);

                var category = data.Data.ContactType.ContactCategory;

                LoadContactCategoryGrid(category.Id, category.TextBoxLabel, category.Name);

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

            PopulateDropDown('.e-EmailType', 'contacttypes/' + $('.constituentEmailgridcontainer').attr('id'), '', '', data.Data.ContactTypeId);

            $(modal).find('.e-Info').val(data.Data.Info);
            $(modal).find('.e-EmailType').val(data.Data.ContactTypeId);
            $(modal).find('.e-IsPreferred').prop('checked', data.Data.IsPreferred);
            $(modal).find('.e-Comment').val(data.Data.Comment);


        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the Email.');
        }
    });

}
// End Emails Subsection

// Websites Subsection
function NewWebModal() {

    $('.newwebmodallink').click(function (e) {

        e.preventDefault();

        PopulateDropDown('.ws-WebSiteType', 'contacttypes/' + $('.constituentWebgridcontainer').attr('id'), '', '');

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
                success: function (data) {

                    DisplaySuccessMessage('Success', 'Web Site saved successfully.');

                    CloseModal(modal);

                    LoadContactCategoryGrid($('.constituentWebgridcontainer').attr('id'), 'URL', 'Web');

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

function EditWeb(id) {

    var modal = $('.websitemodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    LoadWeb(id, modal);

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
            success: function (data) {

                DisplaySuccessMessage('Success', 'Web Site saved successfully.');

                CloseModal(modal);

                var category = data.Data.ContactType.ContactCategory;

                LoadContactCategoryGrid(category.Id, category.TextBoxLabel, category.Name);

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the Web Site.');
            }
        });

    });

}

function LoadWeb(id, modal) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'contactinfo/' + id,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            PopulateDropDown('.ws-WebSiteType', 'contacttypes/' + $('.constituentWebgridcontainer').attr('id'), '', '', data.Data.ContactTypeId);

            $(modal).find('.ws-Info').val(data.Data.Info);
            $(modal).find('.ws-WebSiteType').val(data.Data.ContactTypeId);
            $(modal).find('.ws-IsPreferred').prop('checked', data.Data.IsPreferred);
            $(modal).find('.ws-Comment').val(data.Data.Comment);

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the Web Site.');
        }
    });

}
// End Websites Subsection

// Point Of Contact Subsection
function NewPersonModal() {

    $('.newpersonmodallink').click(function (e) {

        e.preventDefault();

        PopulateDropDown('.poc-PocType', 'contacttypes/' + $('.constituentPersongridcontainer').attr('id'), '', '');

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
                success: function (data) {

                    DisplaySuccessMessage('Success', 'Point of Contact saved successfully.');

                    CloseModal(modal);

                    LoadContactCategoryGrid($('.constituentPersongridcontainer').attr('id'), 'Name', 'Person');

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

function EditPerson(id) {

    var modal = $('.pocmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    LoadPerson(id, modal);

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
            success: function (data) {

                DisplaySuccessMessage('Success', 'Point of Contact saved successfully.');

                CloseModal(modal);

                var category = data.Data.ContactType.ContactCategory;

                LoadContactCategoryGrid(category.Id, category.TextBoxLabel, category.Name);

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the Point of Contact.');
            }
        });

    });

}

function LoadPerson(id, modal) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'contactinfo/' + id,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            PopulateDropDown('.poc-PocType', 'contacttypes/' + $('.constituentPersongridcontainer').attr('id'), '', '', data.Data.ContactTypeId);

            $(modal).find('.poc-Info').val(data.Data.Info);
            $(modal).find('.poc-PocType').val(data.Data.ContactTypeId)
            $(modal).find('.poc-IsPreferred').prop('checked', data.Data.IsPreferred);
            $(modal).find('.poc-Comment').val(data.Data.Comment);

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the Point of Contact.');
        }
    });

}
// End Point Of Contact Subsection

// Other Contacts Subsection
function NewOtherModal() {

    $('.newothermodallink').click(function (e) {

        PopulateDropDown('.o-OtherType', 'contacttypes/' + $('.constituentOthergridcontainer').attr('id'), '', '');

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
                success: function (data) {

                    DisplaySuccessMessage('Success', 'Other Contact saved successfully.');

                    CloseModal(modal);

                    LoadContactCategoryGrid($('.constituentOthergridcontainer').attr('id'), 'Info', 'Other');

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

function EditOther(id) {

    var modal = $('.othermodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 250,
        resizable: false
    });

    LoadOther(id, modal);

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
            success: function (data) {

                DisplaySuccessMessage('Success', 'Other Contact saved successfully.');

                CloseModal(modal);

                LoadContactCategoryGrid(data.Data.Id, 'Info', 'Other');

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the Other Contacts.');
            }
        });

    });

}

function LoadOther(id, modal) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'contactinfo/' + id,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            PopulateDropDown('.o-OtherType', 'contacttypes/' + $('.constituentOthergridcontainer').attr('id'), '', '', data.Data.ContactTypeId);

            $(modal).find('.o-Info').val(data.Data.Info);
            $(modal).find('o-OtherType').val(data.Data.ContactTypeId);
            $(modal).find('.o-IsPreferred').prop('checked', data.Data.IsPreferred);
            $(modal).find('.o-Comment').val(data.Data.Comment);

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the Other Contact.');
        }
    });

}
// End Other Contacts Subsection

/* End Contact Information Section */
