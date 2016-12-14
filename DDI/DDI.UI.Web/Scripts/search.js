
$(document).ready(function () {

    Resize();

    PopulateDropDowns();

    // AddColumnHeaders();
    
    $('.clearsearch').click(function () {
        $('.searchcriteria div.fieldblock input').each(function () {
            $(this).val('');
        });

        $('.searchcriteria select').each(function () {
            $(this).val(0);
        });
    });

    $(document).keypress(function (e) {
        if (e.which == 13) {
            DoSearch2();
        }
    });

    $('.dosearch').click(function () {
        DoSearch2();
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

    // LoadCountries();

    // LoadStates();
}

function LoadCountries() {

    $.ajax({
        url: WEB_API_ADDRESS + 'countries',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $.map(data.d, function (item) {

                var option = $('<option>').val(item.Id).text(item.CountryCode);
                $(option).appendTo($('.searchcountry'));

            });

        },
        failure: function (response) {
            alert(response);
        }
    });

}

function LoadStates() {

    $.ajax({
        url: WEB_API_ADDRESS + 'states',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $.map(data.d, function (item) {

                var option = $('<option>').val(item.Id).text(item.CountryCode);
                $(option).appendTo($('.searchcountry'));

            });

        },
        failure: function (response) {
            alert(response);
        }
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

    var data = JSON.stringify({ properties: GetSearchParameters() });

    $.ajax({
        url: 'Search.aspx/PerformSearch',
        method: 'POST',
        data: data,
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        success: function (data) {

            if (data.d.length == 1) {
                DisplayConstituent(data.d[0].ConstituentNum);
            }
            else {

                $('.gridcontainer').dxDataGrid({
                    dataSource: data.d,
                    columns: [
                        { dataField: 'ConstituentNum', caption: 'ID', alignment: 'center', width: '75px' },
                        { dataField: 'FormattedName', caption: 'Name' },
                        { dataField: 'FullAddress', caption: 'Primary Address' },
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

function DoSearch2() {

    var parameters = GetSearchParameters2();

    /*

    limit: pagesize
    offset: pagenumber
    orderby: <string> not setup yet

    */

    $.ajax({
        url: WEB_API_ADDRESS + 'constituents/?' + parameters,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        success: function (data) {

            if (data.Data.length == 1) {
                DisplayConstituent(data.Data[0].ConstituentNum);
            }
            else {

                $('.gridcontainer').dxDataGrid({
                    dataSource: data.Data,
                    columns: [
                        { dataField: 'ConstituentNum', caption: 'ID', alignment: 'center', width: '100px' },
                        { dataField: 'FormattedName', caption: 'Name' },
                        { dataField: 'FullAddress', caption: 'Primary Address' },
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

    var p = [];

    $('.searchcriteria div.fieldblock input').each(function () {
        var property = $(this).attr('class').replace('search', '');
        var value = $(this).val();

        if (value)
            p.push('"' + property + '": "' + value + '"');
    });

    $('.searchcriteria div.fieldblock select').each(function () {
        var property = $(this).attr('class').replace('search', '');
        var value = $(this).val();

        if (value)
            p.push('"' + property + '": "' + value + '"');
    });

    p = '{' + p + '}';

    return p;

}

function GetSearchParameters2() {

    var p = '';

    $('.searchcriteria div.fieldblock input').each(function () {
        var property = $(this).attr('class').replace('search', '');
        var value = $(this).val();

        if (value) {
            p += property + '=' + value + '&';
        }
    });

    $('.searchcriteria div.fieldblock select').each(function () {
        var property = $(this).attr('class').replace('search', '');
        var value = $(this).val();

        if (value) {
            p += property + '=' + value + '&';
        }
    });

    p = p.substring(0, p.length - 1);

    return p;

}

function DisplayConstituent(id) {

    sessionStorage.setItem("constituentnumber", id);
    location.href = "Constituents.aspx";

}




