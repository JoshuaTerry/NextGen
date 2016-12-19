
var save_route = 'constituents/';

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

            $('.ConstituentStatusId').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.ConstituentStatusId'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.ConstituentStatusId'));

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

            $('.PrefixId').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.PrefixId'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Abbreviation);
                $(option).appendTo($('.PrefixId'));

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

            $('.GenderId').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.GenderId'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.GenderId'));

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

            $('.ClergyTypeId').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.ClergyTypeId'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Code + ' : ' + item.Name);
                $(option).appendTo($('.ClergyTypeId'));

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

            $('.ClergyStatusId').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.ClergyStatusId'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.ClergyStatusId'));

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

            $('.DenominationId').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.DenominationId'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.DenominationId'));

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

            $('.EthnicityId').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.EthnicityId'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.EthnicityId'));

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

            $('.LanguageId').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.LanguageId'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.LanguageId'));

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

            $('.EducationLevelId').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.EducationLevelId'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.EducationLevelId'));

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

            $('.MaritalStatusId').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.MaritalStatusId'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.MaritalStatusId'));

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

            $('.ProfessionId').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.ProfessionId'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.ProfessionId'));

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

            $('.IncomeLevelId').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.IncomeLevelId'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.IncomeLevelId'));

            });

        },
        failure: function (response) {
            alert(response);
        }
    });

}

function GetConstituentData(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'constituents/number/' + id,
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
                    $(classname).val(value);
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
            $(img).attr('src', 'Images/Male.png');
        } else {
            $(img).attr('src', 'Images/Female.png');
        }

        $(img).appendTo($('.constituentpic'));

    }
}

