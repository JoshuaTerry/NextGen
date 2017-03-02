
$(document).ready(function () {

    Resize();

    PopulateDropDowns();
    
    $('.clearsearch').click(function () {
        $('.searchcriteria div.fieldblock input').each(function () {
            $(this).val('');
        });

        $('.searchcriteria select').each(function () {
            $(this).val(0);
        });
    });

    $(document).keypress(function (e) {
        if (e.which === 13) {
            DoSearch();
        }
    });

    $('.dosearch').click(function () {
        DoSearch();
    });

    $(window).resize(function () {
        Resize();
    });

});

function Resize() {

    var windowHeight = $(window).height();
    var header = $('header').height();
    var adjustedHeight = (windowHeight - header) - 55;

    $('.searchcriteria div.scrollable').height(adjustedHeight);

    $('.searchresults div.scrollable').height(adjustedHeight + 30);
}

function PopulateDropDowns() {

    LoadConstituentTypes();

    LoadCountries();
}

function LoadConstituentTypes() {

    PopulateDropDown('.searchtype', 'constituenttypes', '', '');

}

function LoadCountries() {

    PopulateDropDown('.searchcountry', 'countries', '', '');

    $('.searchcountry').change(function () {

        PopulateDropDown('.searchstate', 'states/?countryid=' + $('.searchcountry').val(), '', '');

    });

}

function AddColumnHeaders() {

    var header = $('.searchresultstable thead');
    var columns = ['ID', 'Name', 'Primary Address', 'Contact Information'];
    var tr = $('<tr>');

    $(columns).each(function () {
        $('<th>').text(this).appendTo($(tr));
    });

    $(tr).appendTo($(header));

}

function DoSearch() {

    var parameters = GetSearchParameters();

    $.ajax({
        url: WEB_API_ADDRESS + 'constituents/?' + parameters,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            if (data.Data.length === 1) {
                DisplayConstituent(data.Data[0].ConstituentNumber);
            }
            else {

                $('.gridcontainer').dxDataGrid({
                    dataSource: data.Data,
                    columns: [
                        { dataField: 'Id', width: '0px' },
                        { dataField: 'ConstituentNumber', caption: 'ID', alignment: 'right', width: '100px' },
                        { dataField: 'FormattedName', caption: 'Name' },
                        { dataField: 'PrimaryAddress', caption: 'Primary Address' },
                        'Contact Information'
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

        },
        failure: function (response) {
            alert(response);
        }
    });

}

function GetSearchParameters() {

    var p = '';

    $('.searchcriteria div.fieldblock input').each(function () {
        var property = $(this).attr('class').split(' ')[0].replace('search', '');
        var value = $(this).val();

        if (value) {
            p += property + '=' + encodeURIComponent(value) + '&';
        }
    });

    $('.searchcriteria div.fieldblock select').each(function () {
        var property = $(this).attr('class').split(' ')[0].replace('search', '');
        var value = $(this).val();

        if (value && value !== 'null') {
            p += property + '=' + encodeURIComponent(value) + '&';
        }
    });

    p += 'limit=100&';
    p += 'fields=ConstituentNumber,FormattedName,PrimaryAddress&';

    p = p.substring(0, p.length - 1);

    return p;

}

function DisplayConstituent(id) {

    sessionStorage.setItem("constituentid", id);
    location.href = "Constituents.aspx";

}




