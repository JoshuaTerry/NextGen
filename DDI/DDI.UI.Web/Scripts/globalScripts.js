var WEB_API_ADDRESS = 'http://192.168.10.107:8080/api/v1/';   // DEV
// var WEB_API_ADDRESS = 'http://devapi.ddi.org';   // TEST
// var WEB_API_ADDRESS = '';   // PROD

var AUTH_TOKEN_KEY = "DDI_AUTH_TOKEN";
var auth_token = null;
var editing = false;
var currentEntity = null;
var modal = null;

$(document).ready(function () {

    $.support.cors = true;

    LoadDatePickers();

    LoadTabs();

    LoadAccordions();

    CreateEditControls();

    SetupEditControls();

    $('.addconstituent').click(function (e) {

        e.preventDefault();

        modal = $('.addconstituentmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 800,
            height: 640,
            resizable: false
        });

        $('.savenewconstituent').click(function () {

            SaveNewConstituent();

        });

        $('.cancelmodal').click(function (e) {

            e.preventDefault();

            CloseModal();

        });

        LoadNewConstituentModalDropDowns();

    });

    $('.logout').click(function (e) {

        e.preventDefault();

        sessionStorage.removeItem(AUTH_TOKEN_KEY);
        auth_token = null;

        location.href = "/Login.aspx";
    });

});

function LoadNewConstituentModalDropDowns() {

    PopulateDropDown('.nc-PrefixId', 'prefixes', '', '');
    PopulateDropDown('.nc-GenderId', 'genders', '', '');

    PopulateDropDown('.nc-Country', 'countries', '', '');
    PopulateDropDown('.nc-AddressType', 'addresstypes', '', '');

    $('.nc-Country').change(function () {

        PopulateDropDown('.nc-State', 'states/?countryid=' + $('.nc-Country').val(), '', '');

    });

}

function GetApiHeaders() {

    var token = sessionStorage.getItem(AUTH_TOKEN_KEY);
    var headers = {};

    if (token) {
        headers.Authorization = 'Bearer ' + token;
    }

    return headers;

}

function GetQueryString() {

    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');

    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }

    return vars;
}

function SaveNewConstituent() {

    // Get the fields
    var fields = GetNewFields();

    // Save the Constituent
    $.ajax({
        url: WEB_API_ADDRESS + 'constituents',
        method: 'POST',
        headers: GetApiHeaders(),
        data: fields,
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            // Display success
            DisplaySuccessMessage('Success', 'Constituent saved successfully.');
            
            ClearFields();

            CloseModal();

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during saving the constituent.');
        }
    });

}

function GetNewFields() {

    var p = [];

    $(modal).find('.modalcontent div.fieldblock input').each(function () {
        var property = $(this).attr('class');
        property = property.replace('nc-', '');
        var value = $(this).val();

        if (value && value.length > 0)
            p.push('"' + property + '": "' + value + '"');
    });

    $(modal).find('.modalcontent div.fieldblock select').each(function () {
        var property = $(this).attr('class');
        property = property.replace('nc-', '');
        var value = $(this).val();

        if (value && value.length > 0)
            p.push('"' + property + '": "' + value + '"');
    });

    p = '{' + p + '}';

    return p;

}

function CloseModal() {

    ClearFields();
    
    $(modal).dialog('close');

}

function ClearFields() {

    $('div.fieldblock input').val('');

    $('div.fieldblock select').val(0);

}

function LoadTabs() {

    $('.tabscontainer').tabs();

}

function LoadAccordions() {

    $('.accordions').accordion({
        heightStyle: "content",
        collapsible: true
    });

    $('.accordions').each(function () {

        if (!$(this).hasClass('nocontrols')) {
            $('<div>').addClass('accordion-buttons').append(
                $('<a>').attr('href', '#').text('Collapse All').addClass('accordion-collapseall')
            ).append(
                $('<a>').attr('href', '#').text('Expand All').addClass('accordion-expandall')

            ).insertBefore(this);
        }
        
    });
    

    $('.accordion-collapseall').click(function (e) {
        e.preventDefault();

        $('.accordions').accordion({
            active: false
        });

        $(".ui-accordion-content").hide('fast');
        $('.ui-accordion-header').removeClass('ui-state-active');
    });

    $('.accordion-expandall').click(function (e) {
        e.preventDefault();

        $(".ui-accordion-content").show('fast');
        $('.ui-accordion-header').addClass('ui-state-active');
        
    });

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


// EDITING
//
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
        url: WEB_API_ADDRESS + SAVE_ROUTE + currentEntity.Id,
        method: 'PATCH',
        headers: GetApiHeaders(),
        data: fields,
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            // Display success
            DisplaySuccessMessage('Success', 'Constituent saved successfully.');

            // Display updated entity data
            currentEntity = data.Data;

            RefreshEntity();
        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during saving the constituent.');
        }
    });

}

function GetEditedFields(editcontainer) {

    var p = [];

    $(editcontainer).find('input.editable').each(function () {
        var property = $(this).attr('class').replace('editable ', '');
        var value = $(this).val();

        for (var key in currentEntity) {
            if (key == property && currentEntity[key] != value) {
                if (value == 'null') {
                    p.push('"' + property + '": ' + null);
                }
                else {
                    p.push('"' + property + '": "' + value + '"');
                }
                
            }
        }

    });

    $(editcontainer).find('select').each(function () {
        var property = $(this).attr('class').replace('editable ', '');
        var value = $(this).val();

        for (var key in currentEntity) {
            if (key == property && currentEntity[key] != value) {
                if (value == 'null') {
                    p.push('"' + property + '": ' + null);
                }
                else {
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
//
// END EDITING


// MESSAGING
//
function DisplayInfoMessage(heading, text) {
    DisplayMessage(heading, text, 'info');
}

function DisplayErrorMessage(heading, text) {
    DisplayMessage(heading, text, 'error');
}

function DisplayWarningMessage(heading, text) {
    DisplayMessage(heading, text, 'warning');
}

function DisplaySuccessMessage(heading, text) {
    DisplayMessage(heading, text, 'success');
}

function DisplayMessage(heading, text, icon) {

    $.toast({
        heading: heading,
        text: text,
        icon: icon,
        showHideTransition: 'slide',
        position: 'top-right'
    });

}
//
// END MESSAGING