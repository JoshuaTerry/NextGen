
var constituent = null;

$(document).ready(function () {

    Resize();

    LoadDropDowns();

    $(window).resize(function () {
        Resize();
    });

    if (sessionStorage.getItem('constituentnumber')) {
        GetConstituentData(parseInt(sessionStorage.getItem('constituentnumber')));

        sessionStorage.removeItem('constituentnumber');
    }

});

function Resize() {



}

function LoadDropDowns() {

    LoadConstituentStatuses();

    LoadPrefixes();

    LoadGenders();

    LoadClergyTypes();

    LoadClergyStatuses();

    LoadDenominations();

    LoadEthnicities();

    LoadLanguages();

    LoadEducationLevels();

    LoadMaritalStatuses();

    LoadProfessions();

    LoadIncomeLevels();

}

function LoadConstituentStatuses() {

    $.ajax({
        url: WEB_API_ADDRESS + 'constituentstatues',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $('.c-ConstituentStatusId').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.c-ConstituentStatusId'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.c-ConstituentStatusId'));

            });

        },
        failure: function (response) {
            alert(response);
        }
    });

}

function LoadPrefixes() {

    $.ajax({
        url: WEB_API_ADDRESS + 'prefixes',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $('.c-Prefix').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.c-Prefix'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Abbreviation);
                $(option).appendTo($('.c-Prefix'));

            });

        },
        failure: function (response) {
            alert(response);
        }
    });

}

function LoadGenders() {

    $.ajax({
        url: WEB_API_ADDRESS + 'genders',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $('.c-Gender').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.c-Gender'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.c-Gender'));

            });

        },
        failure: function (response) {
            alert(response);
        }
    });

}

function LoadClergyTypes() {

    $.ajax({
        url: WEB_API_ADDRESS + 'clergytypes',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $('.c-ClergyTypeId').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.c-ClergyTypeId'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Code + ' : ' + item.Name);
                $(option).appendTo($('.c-ClergyTypeId'));

            });

        },
        failure: function (response) {
            alert(response);
        }
    });

}

function LoadClergyStatuses() {

    $.ajax({
        url: WEB_API_ADDRESS + 'clergystatuses',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $('.c-ClergyStatusId').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.c-ClergyStatusId'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.c-ClergyStatusId'));

            });

        },
        failure: function (response) {
            alert(response);
        }
    });

}

function LoadDenominations() {

    $.ajax({
        url: WEB_API_ADDRESS + 'denominations',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $('.c-Denomination').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.c-Denomination'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.c-Denomination'));

            });

        },
        failure: function (response) {
            alert(response);
        }
    });

}

function LoadEthnicities() {

    $.ajax({
        url: WEB_API_ADDRESS + 'ethnicity',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $('.c-Ethnicity').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.c-Ethnicity'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.c-Ethnicity'));

            });

        },
        failure: function (response) {
            alert(response);
        }
    });

}

function LoadLanguages() {

    $.ajax({
        url: WEB_API_ADDRESS + 'languages',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $('.c-LanguageId').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.c-LanguageId'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.c-LanguageId'));

            });

        },
        failure: function (response) {
            alert(response);
        }
    });

}

function LoadEducationLevels() {

    $.ajax({
        url: WEB_API_ADDRESS + 'educationlevels',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $('.c-EducationLevel').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.c-EducationLevel'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.c-EducationLevel'));

            });

        },
        failure: function (response) {
            alert(response);
        }
    });

}

function LoadMaritalStatuses() {

    $.ajax({
        url: WEB_API_ADDRESS + 'professions',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $('.c-MaritalStatus').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.c-MaritalStatus'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.c-MaritalStatus'));

            });

        },
        failure: function (response) {
            alert(response);
        }
    });

}

function LoadProfessions() {

    $.ajax({
        url: WEB_API_ADDRESS + 'professions',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $('.c-ProfessionId').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.c-ProfessionId'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.c-ProfessionId'));

            });

        },
        failure: function (response) {
            alert(response);
        }
    });

}

function LoadIncomeLevels() {

    $.ajax({
        url: WEB_API_ADDRESS + 'incomelevels',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $('.c-IncomeLevelId').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.c-IncomeLevelId'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.c-IncomeLevelId'));

            });

        },
        failure: function (response) {
            alert(response);
        }
    });

}

function GetConstituentData(id) {

    $.ajax({
        //url: 'Constituents.aspx/GetConstituentById',
        url: WEB_API_ADDRESS + 'constituents/number/' + id,
        method: 'GET',
        // data: '{ constituentnumber: "' + id + '" }',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            constituent = data.Data;

            DisplayConstituentData();
            
        },
        failure: function (response) {
            alert(response);
        }
    });
}

function ResetEntity() {

    DisplayConstituentData();

}

function DisplayConstituentData() {

    $.map(constituent, function (value, key) {

        if (typeof (value) == 'string')
            value = value.replace('"', '').replace('"', '');

        if (key != '$id') {

            $('.c-' + key).text(value);
            $('.c-' + key).val(value);

            if (key.toLowerCase().indexOf('date') !== -1) {

                var date = FormatJSONDate(value);

                $('.c-' + key).text(date);
            }

        }
    });

    var img = $('<img>');

    if (constituent.IsMasculine) {
        $(img).attr('src', 'Images/Male.png');
    } else {
        $(img).attr('src', 'Images/Female.png');
    }

    $(img).appendTo($('.constituentpic'));

}

