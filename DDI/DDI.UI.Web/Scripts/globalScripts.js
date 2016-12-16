var WEB_API_ADDRESS = 'http://192.168.10.107:8080/api/v1/';   // DEV
// var WEB_API_ADDRESS = 'http://devapi.ddi.org';   // TEST
// var WEB_API_ADDRESS = '';   // PROD

var editing = false;
var currentEntity = null;

$(document).ready(function () {

    $.support.cors = true;

    LoadDatePickers();

    LoadTabs();

    LoadAccordions();

    CreateEditControls();

    SetupEditControls();

});

function LoadTabs() {

    $('.tabscontainer').tabs();

}

function LoadAccordions() {

    $('.accordions').accordion({
        heightStyle: "content",
        collapsible: true
    });

    /*

    TODO: Accordion expand/collapse all
    
    <a id="collapseAll">Collapse All</a>
    <a id="expandAll">Expand All</a>


    $("#collapseAll").click(function(){
        $(".ui-accordion-content").hide()
    });


    $("#expandAll").click(function(){
        $(".ui-accordion-content").show()
    });

    */

}

function LoadDatePickers() {

    $('.datepicker').datepicker();

}

function FormatJSONDate(jsonDate) {

    var date = '';

    if (jsonDate) {
        date = new Date(parseInt(jsonDate.substr(6)));
        date.getMonth() + 1 + '/' + date.getDate() + '/' + date.getFullYear();
    }
    
    return date;
}

function CreateEditControls() {

    $('.editcontainer').each(function () {

        $(this).prepend(
            $('<div>').addClass('editcontrols').append(
                $('<div>').addClass('editmode-inactive').append(
                    $('<a>').attr('href', '#').addClass('editbutton').text('Edit')
                )
            ).append(
                $('<div>').addClass('editmode-active').append(
                    $('<a>').attr('href', '#').addClass('savebutton').text('Save')
                ).append(
                    $('<a>').attr('href', '#').addClass('cancelbutton').text('Cancel')
                )
            )
        );

    });

}

function SetupEditControls() {

    $('.editable').prop('disabled', true);

    $('.editbutton').click(function (e) {

        e.preventDefault();

        if (!editing) { // No other Edit in progress, good to go

            StartEdit($(this).closest('.editcontainer'));

        }
        else { // Another Edit already in progress

            if (confirm('Another Edit is in progress.\r\n\r\nAre you sure you would like to continue and lose any unsaved information?')) {
                // OK

                // Cancel other edit
                StopEdit($('.editcontainer.active'));

                // Start new edit
                StartEdit($(this).closest('.editcontainer'));
            }
            else {
                // Do nothing
            }

        }
                
    });

    $('.savebutton').click(function (e) {

        e.preventDefault();

        var editcontainer = $(this).closest('.editcontainer');

        StopEdit($(editcontainer));

        SaveEdit($(editcontainer));

    });

    $('.cancelbutton').click(function (e) {

        e.preventDefault();

        StopEdit($(this).closest('.editcontainer'));

        CancelEdit();

    });

}

function StartEdit(editcontainer) {

    editing = true;

    $(editcontainer).find('.editmode-active').show();
    $(editcontainer).find('.editmode-inactive').hide();
    $(editcontainer).addClass('active');

    $(editcontainer).find('.editable').each(function () {

        $(this).prop('disabled', false);

    });

}

function StopEdit(editcontainer) {

    editing = false;

    $(editcontainer).find('.editmode-active').hide();
    $(editcontainer).find('.editmode-inactive').show();
    $(editcontainer).removeClass('active');

    $(editcontainer).find('.editable').each(function () {

        $(this).prop('disabled', true);

    });

}

function SaveEdit(editcontainer) {

    // Get just the fields that have been edited
    var fields = GetEditedFields(editcontainer);

    // Save the entity
    $.ajax({
        url: WEB_API_ADDRESS + 'constituent/' + currentEntity.Id,
        method: 'PATCH',
        data: fields,
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            // Display success
            alert('BOOM... saved!');

            // Display updated entity data
            currentEntity = data.Data;

            RefreshEntity();
        },
        failure: function (response) {
            alert(response);
        }
    });

}

function GetEditedFields(editcontainer) {

    var p = [];

    $(editcontainer).find('input.editable').each(function () {
        var property = $(this).attr('class').replace('editable ', '');
        var value = $(this).val();

        if (value) {
            for (var key in currentEntity) {
                if (key == property && currentEntity[key] != value) {
                    p.push('"' + property + '": "' + value + '"');
                }
            }
        }

    });

    $(editcontainer).find('select').each(function () {
        var property = $(this).attr('class').replace('', '');
        var value = $(this).val();

        if (value) {
            for (var key in currentEntity) {
                if (key == property && currentEntity[key] != value) {
                    p.push('"' + property + '": "' + value + '"');
                }
            }
        }
    });

    p = '{' + p + '}';

    return p;

}

function CancelEdit() {

    RefreshEntity();

}