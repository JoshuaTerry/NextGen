
var AUTH_TOKEN_KEY = "DDI_AUTH_TOKEN";
var auth_token = null;
var editing = false;
var lastActiveSection = null;
var currentEntity = null;
var modal = null;

$(document).ready(function () {

    $.support.cors = true;

    LoadDatePickers();

    LoadDatePair();

    LoadHeaderInfo();

    LoadTabs();

    LoadAccordions();

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

            SaveNewConstituent(modal);

        });

        $('.cancelmodal').click(function (e) {

            e.preventDefault();

            CloseModal(modal);

        });

        LoadNewConstituentModalDropDowns();

        AutoZip(modal);

    });

    $('.logout').click(function (e) {

        e.preventDefault();

        sessionStorage.removeItem(AUTH_TOKEN_KEY);
        auth_token = null;

        location.href = "/Login.aspx";
    });

});

function LoadHeaderInfo() {
    LoadBusinessDate();
    LoadBusinessUnit();
    LoadEnvironment();
}

function LoadBusinessDate() {

    if (sessionStorage.getItem('businessdate')) {
        $('.businessdate').text(sessionStorage.getItem('businessdate'));
    }
    else {

        $.ajax({
            url: WEB_API_ADDRESS + 'businessdate',
            method: 'GET',
            contentType: 'application/json; charset-utf-8',
            dataType: 'json',
            crossDomain: true,
            success: function (data) {
                
                var date = FormatJSONDate(data.Data);

                if (date) {
                    sessionStorage.setItem('businessdate', date);
                }

                $('.businessdate').text(date);
            },
            failure: function (response) {
                alert(response);
            }
        });
    }
}

function LoadBusinessUnit() {
    /* Will be implemented with Business Unit */
}

function LoadEnvironment() {
    if (sessionStorage.getItem('environment')) {
        $('.environment').text(sessionStorage.getItem('environment'));
    }
    else {
        $.ajax({
            url: WEB_API_ADDRESS + 'environment',
            method: 'GET',
            contentType: 'application/json; charset-utf-8',
            dataType: 'json',
            crossDomain: true,
            success: function (data) {

                if (data.Data.length > 0) {
                    sessionStorage.setItem('environment', data.Data);
                }

                $('.environment').text(data.Data);
            },
            failure: function (response) {
                alert(response);
            }
        });
    }
}
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

function SaveNewConstituent(modal) {

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

            CloseModal(modal);
            
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

function CloseModal(modal) {

    ClearFields('.modalcontent');
    
    $(modal).dialog('close');

}

function ClearFields(container) {

    $(container + ' div.fieldblock input').val('');

    $(container + ' div.fieldblock textarea').val('');

    $(container + ' div.fieldblock select').val(0);

}

function LoadTabs() {

    $('.tabscontainer').tabs();

}

function LoadAccordions() {

    $('.accordions').accordion({
        heightStyle: "content",
        collapsible: true,
        beforeActivate: function (event, ui) {
            if (event.originalEvent && $(event.originalEvent.target).is('.ui-accordion-header > .newbutton')) {
                return false;
            }
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

function LoadDatePair() {

    // if ($.timepicker) {
        //$('.datepair .time').timepicker({
        //    'showDuration': true,
        //    'timeFormat': 'g:ia'
        //});

        //$('.datepair .date').datepicker({
        //    'format': 'm/d/yyyy',
        //    'autoclose': true
        //});

        // $('.datepair').datepair();
    // }
    
}

function FormatJSONDate(jsonDate) {

    var date = '';

    if (jsonDate) {
        date = new Date(jsonDate).toDateString();
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

function AutoZip(container) {

    $(container).find('.autozip').blur(function () {

        GetAutoZipData(container);

    });

}

function GetAutoZipData(container) {

    var zip = $(container).find('.autozip').val();

    if (zip && zip.length > 0) {

        var fields = 'addressLine1=' +
                '&addressLine2=' +
                '&city=' +
                '&countryId=' +
                '&countyId=' +
                '&stateId=' +
                '&zip=' + $(container).find('.autozip').val();

        $.ajax({
            url: WEB_API_ADDRESS + 'addresses/zip/?' + fields,
            method: 'GET',
            contentType: 'application/json; charset-utf-8',
            dataType: 'json',
            crossDomain: true,
            success: function (data) {

                if (data && data.Data) {

                    $(container).find('.autozip').val(data.Data.PostalCode);

                    if (data.Data.State) {
                        $(container).find('.autocountry').val(data.Data.State.CountryId);
                        
                        PopulateDropDown('.autostate', 'states/?countryid=' + $(container).find('.autocountry').val(), '', '', data.Data.State.Id);

                        PopulateDropDown('.autocounty', 'counties/?stateid=' + data.Data.State.Id, '', '', data.Data.County.Id);
                    }

                    $(container).find('.autocity').val(data.Data.City);

                }
            
            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during loading address data.');
            }
        });

    }

}

function ExecuteFunction(functionName, context) {

    var args = [].slice.call(arguments).splice(2);
    var namespaces = functionName.split(".");
    var func = namespaces.pop();

    for (var i = 0; i < namespaces.length; i++) {
        context = context[namespaces[i]];
    }

    if (context[func])
        return context[func].apply(context, args);

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
        if ($('#form1').valid()) {
            var editcontainer = $(this).closest('.editcontainer');
            StopEdit(editcontainer);
            SaveEdit(editcontainer);
        } else {
            DisplayErrorMessage('Error', 'There are invalid fields. Please fix those and then try saving again.');
        }
    });

    $('.cancelbutton').click(function (e) {

        e.preventDefault();

        var editcontainer = $(this).closest('.editcontainer');

        StopEdit(editcontainer);
        $('#form1').validate().resetForm();
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
    $(editcontainer).find('.dx-tagbox').each(function() {
        $(this).dxTagBox({
            disabled: false
        });
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

    //SaveTagBoxes
    SaveTagBoxes(editcontainer);

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

function SaveTagBoxes(editcontainer) {

    $(editcontainer).find('.tagbox').each(function (item) {

        var idCollection = [];
        var route = $(this).attr('class').split(' ')[2];

        var foo = $(this).first().attr('class');

        $(this).find('.dx-tagbox').first().dxTagBox('instance').option('selectedItems').forEach(function (selectedItem) {

            idCollection.push(selectedItem.Id);

        });

        SaveChildCollection(idCollection, WEB_API_ADDRESS + 'constituents/' + currentEntity.Id + '/' + route);

    });

}

function SaveChildCollection(children, route) {
    $.ajax({
        url: route,
        method: 'POST',
        headers: GetApiHeaders(),
        data: JSON.stringify({ChildIds: children}),
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            // Display success
            DisplaySuccessMessage('Success', 'Constituent saved successfully.');

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during saving the constituent.');
        }
    });
}

function CancelEdit() {

    RefreshEntity();
}
//
// END EDITING

// DELETING
//

function DeleteEntity(url, method, confirmationMessage) {
    var okToDelete = confirm(confirmationMessage);
    if (okToDelete === true) {
        // delete the entity
        $.ajax({
            url: url,
            method: method,
            headers: GetApiHeaders(),
            contentType: 'application/json; charset-utf-8',
            crossDomain: true,
            success: function() {
                // Display success
                DisplaySuccessMessage('Success', 'The item was deleted.');
            },
            error: function(xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during delete. It was unsuccessful');
            }
        });
    };

}

//
// END DELETING

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