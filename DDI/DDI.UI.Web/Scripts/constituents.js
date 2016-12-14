
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

            $.map(data.d, function (item) {

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

            $.map(data.d, function (item) {

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

            $.map(data.d, function (item) {

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

            $.map(data.d, function (item) {

                var option = $('<option>').val(item.Id).text(item.Code);
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

            $.map(data.d, function (item) {

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

            $.map(data.d, function (item) {

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

            $.map(data.d, function (item) {

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

            $.map(data.d, function (item) {

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

            $.map(data.d, function (item) {

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

            $.map(data.d, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.c-ProfessionId'));

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

            $.map(data.d, function (item) {

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

            $.map(data.d, function (item) {

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

            $.map(data.Data, function (value, key) {

                $('.c-' + key).text(value);
                $('.c-' + key).val(value);

                if (key.toLowerCase().indexOf('date') !== -1) {

                    var date = FormatJSONDate(value);

                    $('.c-' + key).text(date);
                    $('.c-' + key).val(date);
                }
            });

            var img = $('<img>');

            if (data.Data.IsMasculine) {
                $(img).attr('src', 'Images/Male.png');
            } else {
                $(img).attr('src', 'Images/Female.png');
            }

            $(img).appendTo($('.constituentpic'));
        },
        failure: function (response) {
            alert(response);
        }
    });
}

