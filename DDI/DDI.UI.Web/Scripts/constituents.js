
var SAVE_ROUTE = 'constituents/';

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

    PopulateDropDown('.ConstituentStatusId', 'constituentstatues', '', '');
}

function LoadPrefixes() {

    PopulateDropDown('.PrefixId', 'prefixes', '', '');
}

function LoadGenders() {

    PopulateDropDown('.GenderId', 'genders', '', '');

}

function LoadClergyTypes() {

    PopulateDropDown('.ClergyTypeId', 'clergytypes', '', '');

}

function LoadClergyStatuses() {

    PopulateDropDown('.ClergyStatusId', 'clergystatuses', '', '');

}

function LoadDenominations() {

    PopulateDropDown('.DenominationId', 'denominations', '', '');

}

function LoadEthnicities() {

    PopulateDropDown('.EthnicityId', 'ethnicity', '', '');

}

function LoadLanguages() {

    PopulateDropDown('.LanguageId', 'languages', '', '');

}

function LoadEducationLevels() {

    PopulateDropDown('.EducationLevelId', 'educationlevels', '', '');

}

function LoadMaritalStatuses() {

    PopulateDropDown('.MaritalStatusId', 'maritalstatuses', '', '');

}

function LoadProfessions() {

    PopulateDropDown('.ProfessionId', 'professions', '', '');

}

function LoadIncomeLevels() {

    PopulateDropDown('.IncomeLevelId', 'incomelevels', '', '');

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

