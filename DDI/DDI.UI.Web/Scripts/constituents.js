
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

            $('.Prefix').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.Prefix'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Abbreviation);
                $(option).appendTo($('.Prefix'));

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

            $('.Gender').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.Gender'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.Gender'));

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

            $('.Denomination').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.Denomination'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.Denomination'));

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

            $('.Ethnicity').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.Ethnicity'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.Ethnicity'));

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

            $('.EducationLevel').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.EducationLevel'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.EducationLevel'));

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

            $('.MaritalStatus').html('');
            var option = $('<option>').val('').text('');
            $(option).appendTo($('.MaritalStatus'));

            $.map(data.Data, function (item) {

                var option = $('<option>').val(item.Id).text(item.Name);
                $(option).appendTo($('.MaritalStatus'));

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

    $.map(currentEntity, function (value, key) {

        if (typeof (value) == 'string')
            value = value.replace('"', '').replace('"', '');

        if (key != '$id') {

            var classname = '.' + key;

            $(classname).text(value);
            $(classname).val(value);

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

