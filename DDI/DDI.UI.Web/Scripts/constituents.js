
var SAVE_ROUTE = 'constituents/';

$(document).ready(function () {

    Resize();

    LoadDropDowns();

    $(window).resize(function () {
        Resize();
    });

    if (sessionStorage.getItem('constituentnumber')) {

        $('.hidconstituentid').val(sessionStorage.getItem('constituentnumber'))

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

        DisplayConstituentPrimaryAddress();

        LoadDBATable();

        LoadEducationTable();

        LoadPaymentPreferencesTable();
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

        modal = $('.newaddressmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 375,
            height: 560,
            resizable: false
        });

    });

    LoadNewAddressModalDropDowns();

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal();

    });

    $('.savenewaddress').click(function () {

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
                AddressLine2: '',
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

                // DisplayContactInfo();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the address.');
            }
        })

    });

}

function LoadNewAddressModalDropDowns() {

    // PopulateDropDown('.ConstituentStatusId', 'constituentstatues', '', '');
    PopulateDropDown('.na-AddressTypeId', 'addresstypes', '', '');

    PopulateDropDown('.na-CountryId', 'countries', '', '', function () {
        PopulateStatesInModal();
    });
    
    LoadRegions();

}

function PopulateStatesInModal() {

    ClearElement('.na-StateId');
    ClearElement('.na-CountyId');

    var countryid = $('.na-CountryId').val();

    PopulateDropDown('.na-StateId', 'states/?countryid=' + countryid, '', '', function () {
        PopulateCountiesInModal()
    });

}

function PopulateCountiesInModal() {

    var stateid = $('.na-StateId').val();

    PopulateDropDown('.na-CountyId', 'counties/?stateid=' + stateid, '', '');

}

function LoadRegions() {

    GetRegionLevels();

}

function GetRegionLevels() {



}

function LoadRegion1() {

    $('.region1').show();

    PopulateDropDown('.na-Region1', 'regions', '', ''); // needs to load region2

}

function LoadRegion2() {

    $('.region2').show();

}

function LoadRegion3() {

    $('.region3').show();

}

function LoadRegion4() {

    $('.region4').show();

}





