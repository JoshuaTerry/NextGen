
var EntityMappingType = { 'Budget': 0 }
var currentwizpos = 0;

$(document).ready(function () {

    $('.import').click(function (e) {
        e.preventDefault();

        DisplayImportWizardModal();
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        $(modal).find('.stepsinner').css('left', 0);

        CloseModal(modal);

    });

    $('.nextstep').click(function (e) {

        $(modal).find('.stepsinner').animate({
            left: '-=618'
        }, 500);

    });

    $('.prevstep').click(function (e) {

        $(modal).find('.stepsinner').animate({
            left: '+=618'
        }, 500);

    });

    $('.newmapping').click(function (e) {

        e.preventDefault();

        if ($(this).text() === 'New') {
            $('.newmappingname').show();
            $('.mappings').hide();

            $(this).text('Cancel');
        }
        else {
            $('.newmappingname').hide();
            $('.mappings').show();

            $(this).text('New');
        }
        

    });

    GetFieldMappings();

});

function DisplayImportWizardModal() {

    modal = $('.importwizardmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 640,
        height: 480,
        resizable: false
    });

    InitializeFileUploader(WEB_API_ADDRESS + 'filestorage/upload', function (file) {

        if (file) {
            $('.hiddenimportfileid').val(file.Id);
        }

    });

}

function GetFieldMappings() {

    PopulateDropDown('.mappings', 'savedentitymapping', '', '', null, null, null);

}

function SaveFieldMapping() {

    var item = {
        'Name': $('.newmappingname').val(),
        'Description': '',
        'MappingFields': GetMappingFields()
    }

    MakeServiceCall('POST', 'savedentitymapping', item, function (data) {



    }, null);

}

function GetMappingFields() {

    var map = [];

    $.each('.mappingselection ul li', function (index) {

        var c = {
            ColumnName: $(this).text(),
            EntityMapping: {
                PropertyName: $('.importfilecolumns ul li')[index].text(),
                MappingType: EntityMappingType.Budget
            }
        }

        map.push(c);

    });

    map = '{' + map + '}';

    return map;
}

function PreviewImport() {



}

