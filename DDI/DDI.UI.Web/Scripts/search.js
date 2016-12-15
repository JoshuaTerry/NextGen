
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
        if (e.which == 13) {
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

    //LoadConstituentTypes();

    LoadCountries();

    LoadStates();
}

function LoadConstituentTypes() {

    $.ajax({
        url: WEB_API_ADDRESS + 'constituenttypes',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $('.searchtype').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.searchtype'));

            $.map(data.Data, function (item) {

                option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.searchtype'));

            });

        },
        failure: function (response) {
            alert(response);
        }
    });

}

function LoadCountries() {

    $.ajax({
        url: WEB_API_ADDRESS + 'countries',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $('.searchcountry').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.searchcountry'));

            $.map(data.Data, function (item) {

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

            $('.searchstate').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.searchstate'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Description);
                $(option).appendTo($('.searchstate'));

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

    var parameters = GetSearchParameters();

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
                        { dataField: 'ConstituentNumber', caption: 'ID', alignment: 'center', width: '100px' },
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




