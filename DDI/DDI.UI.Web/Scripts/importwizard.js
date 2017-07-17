var EntityMappingType = { Budget: 0};
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

    

});

function DisplayImportWizardModal(type) {

    SetupCustomStep();
    GetFieldMappings(type);
    GetEntityMapping(type);


    modal = $('.importwizardmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 640,
        height: 480,
        resizable: false
    });

    InitializeImportWizardFileUploader(WEB_API_ADDRESS + 'filestorage/upload', function (file) {

        if (file) {
            $('.hiddenimportfileid').val(file.Id);
        }
    });

    
}

function SetupCustomStep() {

    var content = $('.customstep').find('.customstepcontent').html();

    if (content.length > 0) {
        // Custom Content Loaded
        $('.customstep').show();

        $('.step1').find('.modalbuttons').find('.prevstep').show();
    }
    else {
        // Nothing Loaded
        $('.customstep').hide();

        $('.step1').find('.modalbuttons').find('.prevstep').hide();
    }

}

function InitializeImportWizardFileUploader(url, callback) {

    var filecounter = 0;
    var totalfiles = 0;

    $('#file_upload2').fileupload({
        url: url,
        headers: GetApiHeaders(),
        sequentialUploads: true,
        dropZone: $('.importwizardfileuploadcontainer'),
        disableImageResize: true,
        done: function (e, data) {

            var responseFile = null;
            if (data._response.result.Data) {
                responseFile = data._response.result.Data;
            }

            $.each(data.files, function (index, file) {
                filecounter++;

                $('.filemessage2').text('File upload in progress, please do not close this window...').show();
                $('.totalfiles2').text(filecounter + ' upload completed').show();

                var progress = Math.floor(filecounter / totalfiles * 100);
                $('.progress-bar2').css('width', progress + '%');

                if (totalfiles == filecounter) {
                    $('.filemessage2').text('File upload complete.');
                    filecounter = 0;
                    totalfiles = 0;
                }

                if (callback) {
                    callback(responseFile);
                }
            });
        }
    }).bind('fileuploadadd', function (e, data) {
        totalfiles++;

        $('.filemessage2').text('');
        $('.totalfiles2').text('');
    });

}

function GetEntityMapping(type) {
    //function MakeServiceCall(method, route, item, successCallback, errorCallback)
    MakeServiceCall('GET', 'entitymappings/' + type, null, function (data) {
        // for each row of data, add a dropdown with a label tied to the row and dropdown options matching the column headers of the spreadsheet
        // or create a row in a table that allows selection of mapped column
    }, null);
}

function GetFieldMappings(type) {

    PopulateDropDown('.mappings', 'savedentitymapping/type/' + type, '', '', null, DisplayMapping, null);

    // Populate the mappingcolumns amd importfilecolumns
}

function DisplayMapping () {
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

    function GetMappingFields(type) {

        var map = [];

        $.each('.mappingselection ul li', function (index) {

            var c = {
                ColumnName: $(this).text(),
                EntityMapping: {
                    PropertyName: $('.importfilecolumns ul li')[index].text(),
                    MappingType: type
                }
            }

            map.push(c);

        });

        map = '{' + map + '}';

        return map;
    }

    function PreviewImport() {



    }

