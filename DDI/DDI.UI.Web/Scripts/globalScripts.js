var AUTH_TOKEN_KEY = "DDI_AUTH_TOKEN";
var auth_token = null;
var editing = false;
var lastActiveSection = null;
var currentEntity = null;
var previousEntity = null;
var modal = null;
var currentUser = null;
var currentBusinessUnit = null;

$(document).ready(function () {

    $.support.cors = true;

    // LoadDefaultAuthToken();

    LoadDatePickers();

    LoadDatePair();

    LoadHeaderInfo();

    LoadTabs();

    LoadAccordions();

    NewConstituentModal();

    BusinessUnitModal();

    $('.logout').click(function (e) {

        e.preventDefault();

        sessionStorage.removeItem(AUTH_TOKEN_KEY);
        auth_token = null;

        location.href = "/Login.aspx";
    });

});

function LoadDefaultAuthToken() {

    var token = sessionStorage.getItem(AUTH_TOKEN_KEY);

    if (!token) {

        $.ajax({
            type: 'POST',
            url: 'Default.aspx/GetAuthToken',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                sessionStorage.setItem(AUTH_TOKEN_KEY, data.d);
            },
            error: function (error) {
                var err = error;
            }
        });

    }

}

/* NEW CONSTITUENT */
function NewConstituentModal() {

    $('.addconstituent').click(function (e) {

        e.preventDefault();

        modal = $('.addconstituentmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 925,
            height: 625,
            resizable: false,
            beforeClose: function (event, ui) {
                $('.constituenttypeselect').show();
                $('.constituentdetails').hide();
            }
        });

        SetupConstituentTypeSelector();

        // Save Constituent
        $('.savenewconstituent').unbind('click');

        $('.savenewconstituent').click(function () {
            SaveNewConstituent(modal, false);
        });

        // Save & New Constituent
        $('.saveandnewconstituent').unbind('click');

        $('.saveandnewconstituent').click(function () {
            SaveNewConstituent(modal, true);
        });

        $('.cancelmodal').click(function (e) {

            e.preventDefault();

            CloseModal(modal);

        });

        LoadNewConstituentModalDropDowns();

        AutoZip(modal, '.nc-');

    });

}

function SetupConstituentTypeSelector() {

    var container = $('.constituenttypeselect');
    var details = $('.constituentdetails');

    $(container).empty();

    $.ajax({
        url: WEB_API_ADDRESS + 'constituenttypes',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        headers: GetApiHeaders(),
        success: function (data) {

            if (data && data.Data && data.IsSuccessful) {
                
                $.map(data.Data, function (item) {

                    var option = $('<div>')
                    .attr('id', item.Id)
                    .click(function (e) {

                        // Might be implemented later...  Issue with getting the inner container to sit correctly in the modal when opened. - MR 3/13/2017
                        // $('.constituenttypeinner').stop().animate({ left: '-895px' }, 250);

                        $(container).hide('fast');
                        $(details).show('fast');

                        ConstituentTypeLayout(item.Name);

                        SetupNewConstituent(item.Id);

                    });

                    var label = $('<label>').text(item.Name);
                    var img = $('<img>').attr('src', GetConstituentTypeImage(item.Name)).attr('alt', item.Name);

                    $(img).appendTo($(option));
                    $(label).appendTo($(option));

                    $(option).appendTo($(container));

                });

            }

        },
        failure: function (response) {
            
        }
    });

}

function GetConstituentTypeImage(name) {
    
    var path = 'default';

    switch (name) {
        case 'Church':
            path = '../../Images/church.png';
            break;
        case 'Family':
            path = '../../Images/family.png';
            break;
        case 'Individual':
            path = '../../Images/male.png';
            break;
        case 'Organization':
            path = '../../Images/organization.png';
            break;
        default:
            path = '../../Images/male.png';
            break;
    }

    return path;

}

function ConstituentTypeLayout(name) {

    switch (name) {
        case 'Individual':
            IndividualLayout();
            break;
        case 'Church':
            NonindividualLayout();
            break;
        case 'Family':
            NonindividualLayout();
            break;
        case 'Organization':
            NonindividualLayout();
            break;
        default:
            IndividualLayout();
            break;
    }

}

function IndividualLayout() {

    $('.IndividualContainer').show();
    $('.OrganizationContainer').hide();

    $('.nc-TaxID').mask('000-00-0000');
}

function NonindividualLayout() {

    $('.IndividualContainer').hide();
    $('.OrganizationContainer').show();

    $('.nc-TaxID').mask('00-0000000');
}

function SetupNewConstituent(constituenttypeid) {

    $.ajax({
        url: WEB_API_ADDRESS + 'constituents/new/' + constituenttypeid,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        headers: GetApiHeaders(),
        success: function (data) {

            if (data && data.Data && data.IsSuccessful) {

                $('.nc-ConstituentNumber').val(data.Data.ConstituentNumber);
                $('.nc-ConstituentTypeId').val(data.Data.ConstituentType.Id);
            }

        },
        failure: function (response) {

        }
    })

}

function SaveNewConstituent(modal, addnew) {

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

            if (data && data.IsSuccessful) {

                // Display success
                DisplaySuccessMessage('Success', 'Constituent saved successfully.');
                
                if (addnew) {
                    ClearFields('.modalcontent');

                    $(modal).find('.constituenttypeselect').show('fast');
                    $(modal).find('.constituentdetails').hide('fast');
                }
                else {
                    CloseModal(modal);

                    currentEntity = data.Data;

                    sessionStorage.setItem("constituentid", data.Data.ConstituentNumber);
                    location.href = "Constituents.aspx";
                }

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
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
/* NEW CONSTITUENT */

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

        MakeServiceCall('GET', 'businessdate', null, function (data) {

            if (data.Data) {
                var date = FormatJSONDate(data.Data);

                if (date) {
                    sessionStorage.setItem('businessdate', date);
                }

                $('.businessdate').text(date);
            }

        }, null);

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
        MakeServiceCall('GET', 'environment', null, function (data) {

            if (data.Data) {
                if (data.Data.length > 0) {
                    sessionStorage.setItem('environment', data.Data);
                }

                $('.environment').text(data.Data);
            }

        }, null);
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

    LoadRegions($(modal).find('.regionscontainer'), 'nc-');

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
    MakeServiceCall('POST', 'constituents', fields, function (data) {

        if (data.Data) {
            // Display success
            DisplaySuccessMessage('Success', 'Constituent saved successfully.');

            ClearFields();

            CloseModal(modal);
        }

    }, null);

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

    RemoveValidation($(modal).attr('class').split(" ")[0]);

    $(modal).dialog('close');

}

function ConfirmModal(message, yes, no) {

    modal = $('.confirmmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 450,
        resizable: false
    });

    $(modal).find('.confirmmessage').html(message);

    $('.confirmyes').unbind('click');

    if (yes) {

        $('.confirmyes').click(function () {
            CloseModal(modal);
            yes();
        });

    }

    $('.confirmno').unbind('click');

    if (no) {

        $('.confirmno').click(function () {
            CloseModal(modal);
            no();
        });

    }

}

function ClearFields(container) {

    $(container + ' div.fieldblock input').val('');

    $(container + ' div.fieldblock textarea').val('');

    $(container + ' div.fieldblock select').val(0);

    $(container + ' div.fieldblock input:checkbox').removeAttr('checked');

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

function AutoZip(container, prefix) {

    $(container).find('.autozip').blur(function () {

        GetAutoZipData(container, prefix);

    });

}

function GetAutoZipData(container, prefix) {

    var zip = $(container).find('.autozip').val();

    if (zip && zip.length > 0) {

        var fields = 'addressLine1=' + $(container).find('.autoaddress1').val() +
                '&addressLine2=' + $(container).find('.autoaddress2').val() +
                '&city=' +
                '&countryId=' +
                '&countyId=' +
                '&stateId=' +
                '&zip=' + $(container).find('.autozip').val();

        MakeServiceCall('GET', 'addresses/zip/?' + fields, null, function (data) {

            if (data && data.Data) {

                $(container).find('.autozip').val(data.Data.PostalCode);

                if (data.Data.State) {
                    $(container).find('.autocountry').val(data.Data.State.CountryId);

                    PopulateDropDown('.autostate', 'states/?countryid=' + $(container).find('.autocountry').val(), '', '', data.Data.State.Id);

                    PopulateDropDown('.autocounty', 'counties/?stateid=' + data.Data.State.Id, '', '', data.Data.County.Id);
                }

                $(container).find('.autocity').val(data.Data.City);

                LoadAllRegionDropDowns(modal, prefix, data.Data);

            }

        }, null);
    }

}

function LoadTagSelector(type, container) {

    $('.tagselect').each(function () {

        var img = $('.tagSelectImage');

        if (img.length === 0) {
            img = $('<div>').addClass('tagSelectImage');

            $(img).click(function () {

                modal = $('.tagselectmodal').dialog({
                    closeOnEscape: false,
                    modal: true,
                    width: 450,
                    resizable: false
                });

                LoadAvailableTags(modal);

                $('.saveselectedtags').unbind('click');

                $('.saveselectedtags').click(function () {

                    var tagIds = [];

                    $('.tagselectgridcontainer').find('input').each(function (index, value) {

                        if ($(value).prop('checked')) {
                            tagIds.push($(value).val());
                        }

                    });

                    MakeServiceCall('POST', path1 + '/' + currentEntity.Id + '/' + path2, JSON.stringify({ tags: tagIds }), function (data) {

                        if (data.Data) {

                            DisplaySuccessMessage('Success', 'Tags saved successfully.');

                            CloseModal(modal);

                            currentEntity = data.Data;

                            DisplaySelectedTags(container);

                        }

                    }, null);

                });

            });

            $(this).after($(img));
        }

        $(img).hide();

    });

}

function LoadAvailableTags(container) {

    MakeServiceCall('GET', 'taggroups', null, function (data) {

        if (data.Data) {

            $(container).find('.tagselectgridcontainer').html('');

            $.map(data.Data, function (group) {

                var header = $('<div>').addClass('tagSelectorHeader').text(group.DisplayName);
                var tagsContainer = $('<div>').addClass('tagSelectorContainer');

                if (group.Tags) {

                    switch (group.TagSelectionType) {

                        case 0:
                            CreateMultiSelectTags(group.Tags, tagsContainer);
                            break;
                        case 1:
                            CreateSingleSelectTags(group.Tags, group.Id, tagsContainer);
                            break;
                        default:
                            break;
                    }

                }

                            $(header).appendTo($(container).find('.tagselectgridcontainer'));
                            $(tagsContainer).appendTo($(container).find('.tagselectgridcontainer'));

            });

            RefreshTags();

        }

    }, null);

}

function RefreshTags() {

    if (currentEntity && currentEntity.Tags) {

        $.map(currentEntity.Tags, function (item) {

            $('input[value=' + item.Id + ']').prop('checked', true);

        });

    };

}

function DisplaySelectedTags(container) {

    if (currentEntity && currentEntity.Tags) {

        $(container).html('');

        $.map(currentEntity.Tags, function (tag) {

            var t = $('<div>').addClass('dx-tag-content').attr('id', tag.Id).appendTo($('.tagselect'));
            $('<span>').text(tag.DisplayName).appendTo($(t));
            $('<div>').addClass('dx-tag-remove-button')
                .click(function () {
                    MakeServiceCall('DELETE', 'constituents/' + currentEntity.Id + '/tag/' + tag.Id, null, function (data) {

                        if (data.Data) {

                            currentEntity = data.Data;

                            DisplaySelectedTags(container);
                        }

                    }, null);
                })
                .appendTo($(t));

        });

        if (!editing) {
            $(container).find('.dx-tag-remove-button').hide();
        }

    }
}

function DisplaySelectedTagsConstituentType() {

    if (currentEntity && currentEntity.Tags) {

        $('.tagselect').html('');

        $.map(currentEntity.Tags, function (tag) {

            var t = $('<div>').addClass('dx-tag-content').attr('id', tag.Id).appendTo($('.tagselect'));
            $('<span>').text(tag.DisplayName).appendTo($(t));
            $('<div>').addClass('dx-tag-remove-button')
                .click(function () {
                    MakeServiceCall('DELETE', 'constituenttypes/' + currentEntity.Id + '/tag/' + tag.Id, null, function (data) {

                        if (data.Data) {

                            currentEntity = data.Data;

                            DisplaySelectedTagsConstituentType();
                        }

                    }, null);
                })
                .appendTo($(t));

        });

    }
}

function CreateSingleSelectTags(tags, groupId, container) {

    var ul = $('<ul>');

    $.map(tags, function (tag) {

        var li = $('<li>');

        $('<input>').attr('type', 'radio').attr('name', groupId).val(tag.Id).appendTo($(li));
        $('<span>').text(tag.DisplayName).appendTo($(li));

        $(li).appendTo($(ul));

    });

    $(ul).appendTo($(container));

}

function CreateMultiSelectTags(tags, container) {

    var ul = $('<ul>');

    $.map(tags, function (tag) {

        var li = $('<li>');

        $('<input>').attr('type', 'checkbox').val(tag.Id).appendTo($(li));
        $('<span>').text(tag.DisplayName).appendTo($(li));

        $(li).appendTo($(ul));
    });

    $(ul).appendTo($(container));
}

function GetFile(id, callback) {

    MakeServiceCall('GET', 'filestorage/' + id, null, function (data) {

        if (data.Data && callback) {
            callback(data.Data);
        }

    }, null);

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

            ConfirmModal('Another Edit is in progress.<br /><br />Are you sure you would like to continue and lose any unsaved information?', function () {
                // Cancel other edit
                StopEdit($('.editcontainer.active'));

                // Start new edit
                StartEdit(editcontainer);
            }, function () {
                // Return to previous edit
                $('.accordions').accordion('option', 'active', lastActiveSection);
            });

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

    $(editcontainer).find('.dx-tagbox').each(function () {
        $(this).dxTagBox({
            disabled: false
        });
    });

    $(editcontainer).find('.tagselect').each(function () {
        $(this).removeClass('disabled');
        $(this).find('.dx-tag-remove-button').show();
        $('.tagSelectImage').show();
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

    $(editcontainer).find('.tagselect').each(function () {
        $(this).addClass('disabled');
        $(this).find('.dx-tag-remove-button').show();
        $('.tagSelectImage').hide();
    });

}

function SaveEdit(editcontainer) {

    // Get just the fields that have been edited
    var fields = GetEditedFields(editcontainer);

    //SaveTagBoxes
    SaveTagBoxes(editcontainer);

    // Save the entity
    MakeServiceCall('PATCH', SAVE_ROUTE + currentEntity.Id, fields, function (data) {

        if (data.Data) {
            // Display success
            DisplaySuccessMessage('Success', 'Constituent saved successfully.');

            // Display updated entity data
            currentEntity = data.Data;

            RefreshEntity();
        }

    }, null);

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
    MakeServiceCall('POST', SAVE_ROUTE + currentEntity.Id, JSON.stringify({ ChildIds: children }), function (data) {

        if (data.Data) {
            // Display success
            DisplaySuccessMessage('Success', 'Constituent saved successfully.');

        }

    }, null);
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
        MakeServiceCall(method, url, null, function (data) {

            if (data.Data) {
                // Display success
                DisplaySuccessMessage('Success', 'This item was deleted.');

            }

        }, null);
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

// FORM VALIDATION
//
function InitRequiredLabels(formClassName) {
    formClassName.replace(".", "");
    $('.' + formClassName).find('.required').each(function (index, el) {
        var labelElement = $(this).prev();
        labelElement[0].innerHTML = labelElement[0].innerHTML + " *";
    });
}

function ValidateForm(formClassName) {
    var validform = true;
    formClassName.replace(".", "");
    // required items
    $('.' + formClassName).find('.required').each(function (index, el) {
        var errorId = "errlbl" + $(this).attr('class').split(" ")[0];
        $("#" + errorId).remove();
        if ($(this).val() === "") {
            $(this).parent().append('<label class="validateerror" id="' + errorId + '">Required</label>');
            validform = false;
        }
    });
    return validform;
}

function RemoveValidation(formClassName) {
    formClassName.replace(".", "");
    $('.' + formClassName).find('.required').each(function (index, el) {
        var errorId = "errlbl" + $(this).attr('class').split(" ")[0];
        $("#" + errorId).remove();
    });
}
//
// END FORM VALIDATION



// END MESSAGING

// BUSINESS UNIT
//

function BusinessUnitModal() {

    // DummyUser('D3BFB26C-4603-E711-80E5-005056B7555A');
    // Uncomment this to use currentBusinessUnit
    // Add the guid of the user you want to use

    $('.editbusinessunit').click(function (e) {

        LoadBusinessUnitDropDown(currentBusinessUnit);

        e.preventDefault();

        var modal = $('.changebusinessunitmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 250,
            resizable: false,
        });

        $('.savebusinessunit').unbind('click');

        $('.savebusinessunit').click(function (e) {

             $.each(currentUser.BusinessUnits, function(index, value) {
                
                if(value.Id === $('.bu-currentbu').val()) {

                    currentBusinessUnit = value;
                }
                
             }); 

            $('.editbusinessunit').text(currentBusinessUnit.DisplayName); 

            CloseModal(modal);

        });

        $('.cancelmodal').click(function (e) {

            e.preventDefault();

            CloseModal(modal);

        });

    });

}

function LoadBusinessUnitDropDown(currentBusinessUnit) {

    if (currentBusinessUnit != null) {

        PopulateDropDown('.bu-currentbu', 'users/' + currentUser.Id + '/businessunit', '', '', currentBusinessUnit.Id);

        $('.editbusinessunit').text(currentBusinessUnit.DisplayName);

    } else {
        
        $('.editbusinessunit').text('BU');

    }

   

}

function DummyUser(dummyUserId) {
    //THIS FUNCTION FOR TESTING/DEMONSTRATION PURPOSES ONLY

    MakeServiceCall('GET', 'users/' + dummyUserId, null, function (data) {
        
        currentUser = data.Data;

        currentBusinessUnit = data.Data.DefaultBusinessUnit;

        LoadBusinessUnitDropDown(currentBusinessUnit);

    });
}

//
// END BUSINESS UNIT

// FORM VALIDATION
//
function InitRequiredLabels(formClassName) {
    formClassName.replace(".", "");
    $('.' + formClassName).find('.required').each(function (index, el) {
        var labelElement = $(this).prev();
        labelElement[0].innerHTML = labelElement[0].innerHTML + " *";
    });
}

function ValidateForm(formClassName) {
    var validform = true;
    formClassName.replace(".", "");
    // required items
    $('.' + formClassName).find('.required').each(function (index, el) {
        var errorId = "errlbl" + $(this).attr('class').split(" ")[0];
        $("#" + errorId).remove();
        if ($(this).val() === "") {
            $(this).parent().append('<label class="validateerror" id="' + errorId + '">Required</label>');
            validform = false;
        }
    });
    return validform;
}

function RemoveValidation(formClassName) {
    formClassName.replace(".", "");
    $('.' + formClassName).find('.required').each(function (index, el) {
        var errorId = "errlbl" + $(this).attr('class').split(" ")[0];
        $("#" + errorId).remove();
    });
}
//
// END FORM VALIDATION



