
var AUTH_TOKEN_KEY = "DDI_AUTH_TOKEN";
var auth_token = null;
var editing = false;
var lastActiveSection = null;
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
            width: 900,
            height: 625,
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

        AutoZip();

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

    LoadRegions('regionscontainer', 'nc-');

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
        var property = $(this).attr('class').split(' ');
        var propertyName = property[0].replace('nc-', '');
        var value = $(this).val();

        if (value && value.length > 0)
            p.push('"' + propertyName + '": "' + value + '"');
    });

    $(modal).find('.modalcontent div.fieldblock select').each(function () {
        var property = $(this).attr('class').split(' ');
        var propertyName = property[0].replace('nc-', '');
        var value = $(this).val();

        if (value && value.length > 0)
            p.push('"' + propertyName + '": "' + value + '"');
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
        collapsible: true,
        beforeActivate: function (event, ui) {
            var newbutton = $(event.originalEvent.target).is('.ui-accordion-header > .newbutton');
            if (newbutton)
                return false;
        }
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

function ClearElement(e) {

    if ($(e).is('select')
        || $(e).is('div')) {

        $(e).html('');

    }

    if ($(e).is('input')) {

        if ($(e).is(':checkbox')) {
            $(e).prop('checked', false);
        }
        else {
            $(e).val('');
        }

    }
    
    if ($(e).is('textarea')) {

        $(e).text('');

    }
}

function AutoZip() {

    $('.autozip').blur(function () {

        GetAutoZipData();

    });

    $('.autocity').blur(function () {

        GetAutoZipData();

    });

    $('.autostate').blur(function () {

        GetAutoZipData();

    });

}

function GetAutoZipData() {

    var zip = $('.autozip').val();

    if (zip && zip.length > 0) {

        var fields = 'addressLine1=' + $('.autoaddress1').val() +
                '&addressLine2=' + $('.autoaddress2').val() +
                '&city=' + $('.autocity').val() +
                '&countryId=' + $('.autocountry').val() +
                '&countyId=' + $('.autocounty').val() +
                '&stateId=' + $('.autostate').val() +
                '&zip=' + $('.autozip').val();

        $.ajax({
            url: WEB_API_ADDRESS + 'locations/?' + fields,
            method: 'GET',
            contentType: 'application/json; charset-utf-8',
            dataType: 'json',
            crossDomain: true,
            success: function (data) {

                if (data && data.Data) {

                    $('.autozip').val(data.Data.PostalCode);

                    if (data.Data.State) {
                        $('.autocountry').val(data.Data.State.CountryId);

                        PopulateDropDown('.autostate', 'states/?countryid=' + $('.autocountry').val(), '', '', data.Data.State.Id);

                        PopulateDropDown('.autocounty', 'counties/?stateid=' + stateid, '', '');
                    }

                    $('.autocity').val(data.Data.City);

                }
            
            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during loading address data.');
            }
        });

    }

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

        var editcontainer = $(this).closest('.editcontainer');


        if (!editing) { // No other Edit in progress, good to go

            StartEdit(editcontainer);
        }
        else { // Another Edit already in progress

            if (confirm('Another Edit is in progress.\r\n\r\nAre you sure you would like to continue and lose any unsaved information?')) {
                // OK

                // Cancel other edit
                StopEdit($('.editcontainer.active'));

                // Start new edit
                StartEdit(editcontainer);
            }
            else {
                // Cancel

                // Return to previous edit
                $('.accordions').accordion('option', 'active', lastActiveSection);
                
            }

        }
                
    });

    $('.savebutton').click(function (e) {

        e.preventDefault();

        var editcontainer = $(this).closest('.editcontainer');

        StopEdit(editcontainer);

        SaveEdit(editcontainer);

    });

    $('.cancelbutton').click(function (e) {

        e.preventDefault();

        var editcontainer = $(this).closest('.editcontainer');

        StopEdit(editcontainer);

        CancelEdit();

    });

}

function StartEdit(editcontainer) {
   
    editing = true;
    // Get the index of the section that was previously edited
    lastActiveSection = $('.accordions').accordion('option', 'active'); 

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

        var property = $(this).attr('class').replace('editable ', '').split(' ');
        var propertyName = property[0]
        var value = '';

        if ($(this).is(':checkbox')) {
            value = $(this).prop('checked');
        }
        else {
            value = $(this).val();
        }

        for (var key in currentEntity) {
            if (key == propertyName && currentEntity[key] != value) {
                if (value == 'null') {
                    p.push('"' + propertyName + '": ' + null);
                }
                else {
                    p.push('"' + propertyName + '": "' + value + '"');
                }
                
            }
        }

    });

    $(editcontainer).find('select').each(function () {
        var property = $(this).attr('class').replace('editable ', '').split(' ');
        var propertyName = property[0]
        var value = $(this).val();

        for (var key in currentEntity) {
            if (key == propertyName && currentEntity[key] != value) {
                if (value == 'null') {
                    p.push('"' + propertyName + '": ' + null);
                }
                else {
                    p.push('"' + propertyName + '": "' + value + '"');
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