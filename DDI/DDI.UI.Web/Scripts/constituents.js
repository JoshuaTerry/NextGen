
var SAVE_ROUTE = 'constituents/';
var currentaddress = null;

$(document).ready(function () {

    Resize();

    LoadDropDowns();

    $(window).resize(function () {
        Resize();
    });

    if (sessionStorage.getItem('constituentid')) {

        $('.hidconstituentid').val(sessionStorage.getItem('constituentid'))

        // sessionStorage.removeItem('constituentnumber');

    }

    GetConstituentData($('.hidconstituentid').val());

    NewAddressModal();

});

function Resize() {



}

function LoadDropDowns() {
    
    PopulateDropDown('.ConstituentStatusId', 'constituentstatues', '', '');
    PopulateDropDown('.PrefixId', 'prefixes', '', '');
    PopulateDropDown('.GenderId', 'genders', '', '');
    PopulateDropDown('.ClergyTypeId', 'clergytypes', '', '');
    PopulateDropDown('.ClergyStatusId', 'clergystatuses', '', '');
    PopulateDropDown('.DenominationId', 'denominations', '', '');
    PopulateDropDown('.EthnicityId', 'ethnicity', '', '');
    PopulateDropDown('.LanguageId', 'languages', '', '');
    PopulateDropDown('.EducationLevelId', 'educationlevels', '', '');
    PopulateDropDown('.MaritalStatusId', 'maritalstatuses', '', '');
    PopulateDropDown('.ProfessionId', 'professions', '', '');
    PopulateDropDown('.IncomeLevelId', 'incomelevels', '', '');

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

        LoadDBATable();

        LoadEducationTable();

        LoadPaymentPreferencesTable();
    }
}

function DisplayConstituentType() {
    $('#tab-main-link').text(currentEntity.ConstituentType.DisplayName);
    if (currentEntity.ConstituentType.Category === 0) {
        $('.organization').hide();
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

                $('.CityStateZip').text(item.Address.City + ', ' + item.Address.State.DisplayName + item.Address.PostalCode);

            }

        });

    }

}

function LoadDBATable() {

    $('.doingbusinessastable').dxDataGrid({
        dataSource: currentEntity.DoingBusinessAs,
        columns: [
            { dataField: 'StartDate', caption: 'From', },
            { dataField: 'EndDate', caption: 'To' },
            { dataField: 'Name', caption: 'Name' }
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

function LoadEducationTable() {

    $('.educationleveltable').dxDataGrid({
        dataSource: currentEntity.Educations,
        columns: [
            { dataField: 'StartDate', caption: 'Start Date', },
            { dataField: 'EndDate', caption: 'End Date' },
            { dataField: 'School', caption: 'School' },
            { dataField: 'Degree', caption: 'Degree' },
            { dataField: 'Major', caption: 'Major' }
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

function NewAddressModal() {

    $('.newaddressmodallink').click(function (e) {

        e.preventDefault();

        modal = $('.addressmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 375,
            resizable: false
        });

    });

    PopulateAddressTypesInModal(null);

    PopulateCountriesInModal(null);

    LoadRegions();

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal();

    });

    $('.saveaddress').click(function () {

        var item = {
            ConstituentId: $('.hidconstituentid').val(),
            IsPrimary: $('.na-IsPreferred').prop('checked'),
            Comment: $('.na-Comment').val(),
            StartDate: $('.na-FromDate').val(),
            EndDate: $('.na-ToDate').val(),
            StartDay: 0,
            EndDay: 0,
            ResidentType: $('.na-Residency').val(),
            AddressTypeId: $('.na-AddressTypeId').val(),
            Address: {
                AddressLine1: $('.na-AddressLine1').val(),
                AddressLine2: $('.na-AddressLine2').val(),
                City: $('.na-City').val(),
                CountryId: $('.na-CountryId').val(),
                CountyId: $('.na-CountyId').val(),
                PostalCode: $('.na-PostalCode').val(),
                StateId: $('.na-StateId').val(),
                Region1Id: $('.na-Region1Id').val(),
                Region2Id: $('.na-Region2Id').val(),
                Region3Id: $('.na-Region3Id').val(),
                Region4Id: $('.na-Region4Id').val()
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

                CloseModal();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the address.');
            }
        });

    });

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

function LoadRegions() {

    GetRegionLevels();

}

function GetRegionLevels() {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'regionlevels',
        data: item,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            $.map(data.Data, function (item) {

                $('.region' + item.Level).show();
                $('.region' + item.Level + 'label').text(item.Label);
                
                if (!item.IsChildLevel) {
                    LoadRegionDropDown(item.Level);
                } else {
                    $('.na-Region' + (item.Level - 1) + 'Id').change(function () {

                        ClearElement('.na-Region' + item.Level + 'Id');

                        if ($('.na-Region' + (item.Level - 1) + 'Id option:selected').text().length > 0) {
                            LoadRegionDropDown((item.Level), $('.na-Region' + item.Level + 'Id').val());
                        }

                    });
                }

            });

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the region levels.');
        }
    });

}

function LoadRegionDropDown(level, selectedvalue) {

    var route = 'regions/' + level;

    if (selectedvalue)
        route = route + '/' + selectedvalue;
    else
        route = route + '/null';

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + route,
        data: item,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            var currentdropdown = $('.na-Region' + level + 'Id');
            ClearElement('.na-Region' + level + 'Id');
            AddDefaultOption($(currentdropdown), '', '');

            $.map(data.Data, function (item) {

                option = $('<option>').val(item.Id).text(item.DisplayName);
                $(option).appendTo($(currentdropdown));

            });

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the region levels.');
        }
    });
}

function EditAddressModal(id) {

    modal = $('.addressmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 375,
        height: 560,
        resizable: false
    });

    LoadAddress(id);

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal();

    });

    $('.saveaddress').click(function () {

        // Get the changed fields from currentaddress and put into new array.
        var fields = GetEditedAddressFields();
        
        $.ajax({
            type: 'PATCH',
            url: WEB_API_ADDRESS + 'constituentaddresses',
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
        ConstituentId: $('.hidconstituentid').val(),
        IsPrimary: $('.na-IsPreferred').prop('checked'),
        Comment: $('.na-Comment').val(),
        StartDate: $('.na-FromDate').val(),
        EndDate: $('.na-ToDate').val(),
        StartDay: 0,
        EndDay: 0,
        ResidentType: $('.na-Residency').val(),
        AddressTypeId: $('.na-AddressTypeId').val(),
        Address: {
            AddressLine1: $('.na-AddressLine1').val(),
            AddressLine2: $('.na-AddressLine2').val(),
            City: $('.na-City').val(),
            CountryId: $('.na-CountryId').val(),
            CountyId: $('.na-CountyId').val(),
            PostalCode: $('.na-PostalCode').val(),
            StateId: $('.na-StateId').val(),
            Region1Id: $('.na-Region1Id').val(),
            Region2Id: $('.na-Region2Id').val(),
            Region3Id: $('.na-Region3Id').val(),
            Region4Id: $('.na-Region4Id').val()
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

            currentaddress = data;

            $('.hidconstituentid').val(id);
            $('.na-isIsPreferred').prop('checked', data.Address.IsPreferred);
            $('.na-Comment').val(data.Address.Comment);
            $('.na-StartDate').val(data.StartDate);
            $('.na-EndDate').val(data.EndDate);
            $('.na-ResidentType').val(data.ResidentType);

            $('.na-AddressLine1').val(data.Address.AddressLine1);
            $('.na-AddressLine2').val(data.Address.AddressLine2);
            $('.na-City').val(data.Address.City);
            
            $('.na-PostalCode').val(data.Address.PostalCode);

            PopulateAddressTypesInModal(data.AddressTypeId);
            PopulateCountiesInModal(data.address.CountryId);
            PopulateStatesInModal(data.address.StateId);
            PopulateCountiesInModal(data.address.CountyId);

            LoadRegions();

            LoadRegion1(data.Address.Region1Id);
            LoadRegion2(data.Address.Region2Id);
            LoadRegion3(data.Address.Region3Id);
            LoadRegion4(data.Address.Region4Id);
            
        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the address.');
        }
    });

}





