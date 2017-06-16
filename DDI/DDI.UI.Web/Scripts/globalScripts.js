var AUTH_TOKEN_KEY = "DDI_AUTH_TOKEN";
var ACCOUNT_ID = "ACCOUNT_ID";
var auth_token = null;
var editing = false;
var lastActiveSection = null;
var currentEntity = null;
var previousEntity = null;
var modal = null;
var currentUser = null;
var currentBusinessUnit = null;
var currentBusinessUnitId = null;
var toolbox = null;
var newContactInformationFields = null;

$(document).ready(function () {

    $.support.cors = true;

    LoadDefaultAuthToken();

    LoadCurrentUser();

    LoadCurrentBusinessUnit();

    LoadDatePickers();

    LoadDatePair();

    LoadHeaderInfo();

    LoadTabs();

    LoadAccordions();

    NewConstituentModal();

    BusinessUnitModal();

    $('.logout').click(function (e) {

        e.preventDefault();

        $.ajax({
            type: 'POST',
            url: 'Login.aspx/Logout',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function () {
                sessionStorage.removeItem(AUTH_TOKEN_KEY);
                auth_token = null;

                location.href = "/Login.aspx";
            },
            error: function (error) {
                var err = error;
            }
        });

        
    });

    if ($('.utilitymenu li').length > 0) {

        $('.utilitynav').show();

        $('.utilitynav').click(function (e) {

            e.preventDefault();
            e.stopPropagation();

            toolbox = $(this).find('.utilitymenu');
            $(toolbox).toggle();

            //toolbox = $(this).next('.utilitymenu');
            //$(toolbox).toggle();

            // $('.utilitymenu').toggle();

        });
    }
    else {
        $('.utilitynav').hide();
    }
    
    $(document).click(function (e) {

        if (toolbox) {
            toolbox.hide();
            toolbox = null;
        }

        
    });

});

function LoadDefaultAuthToken() {

    var token = sessionStorage.getItem(AUTH_TOKEN_KEY);

    if (!token) {

        $.ajax({
            type: 'POST',
            url: 'Login.aspx/GetAuthToken',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data.d) {
                    sessionStorage.setItem(AUTH_TOKEN_KEY, data.d);

                    location.href = "/Default.aspx";
                }
            },
            error: function (error) {
                var err = error;
            }
        });

    }

}

function LoadCurrentUser() {

    var dummyUserId = 'D3BFB26C-4603-E711-80E5-005056B7555A';

    MakeServiceCall('GET', 'users/' + dummyUserId, null, function (data) {

        currentUser = data.Data;

        currentBusinessUnit = data.Data.DefaultBusinessUnit;
        currentBusinessUnitId = data.Data.DefaultBusinessUnit.Id;

        sessionStorage.setItem('CURRENT_BUSINESS_UNIT', currentBusinessUnitId);

        $('.editbusinessunit').text(currentBusinessUnit.DisplayName);

    });

}

function LoadCurrentBusinessUnit() {

    currentBusinessUnitId = sessionStorage.getItem('CURRENT_BUSINESS_UNIT');
    
    if (currentBusinessUnitId == null) {
        
        LoadCurrentUser();

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

        if (newContactInformationFields == null) {
            CreateContactInformationFields('contactInformationContainer');
        }

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

        AutoZip(modal, '.nca-');

    });

}

function CreateContactInformationFields(containerClass) {
    if ($.type(containerClass) === "string" && containerClass.indexOf('.') != 0)
        containerClass = '.' + containerClass;

    MakeServiceCall('GET', 'contacttypes/newconstituent', null, function (data) {
        if (data && data.IsSuccessful) {

            var column = 0;
            var container = null;
            newContactInformationFields = [];

            $.map(data.Data, function (item) {
                if (column == 0) {
                    container = $('<div>').addClass('fourcolumn');
                    $(containerClass).append(container);
                }
                container.append(CreateContactInformationField(item));
                column = ++column % 4;
            });
        }
    });
}

function CreateContactInformationField(item) {
    var div = $('<div>').addClass('fieldblock');
    $('<label>').text(item.Name).appendTo($(div));
    var text = $('<input>').attr('type', 'text').addClass('editable');
    $(div).append(text);

    newContactInformationFields.push( { TextBox: text, Id: item.Id, Category: item.ContactCategory.Code } );

    return div;
}

function SetupConstituentTypeSelector() {

    var container = $('.constituenttypeselect');
    var details = $('.constituentdetails');

    $(container).empty();

    $.ajax({
        url: WEB_API_ADDRESS + 'constituenttypes?fields=Id,Name,Category',
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

                        ConstituentTypeLayout(item.Category);

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

function ConstituentTypeLayout(category) {

    switch (category) {
        case 0:
            IndividualLayout();
            break;
        case 1:
            NonindividualLayout();
            break;
        default:
            NonindividualLayout();
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

    MakeServiceCall('GET', 'configurations?module=CRM', null, function (data) {
        $('.nca-AddressType').val(data.Data.DefaultAddressType.Id);
        $('.nca-Country').val(data.Data.DefaultCountryId);
        
    }, null);

}

function SaveNewConstituent(modal, addnew) {

    // Get the fields
    var fields = GetNewFields();

    // Save the Constituent

    MakeServiceCall('POST', 'constituents', fields, function (data) {

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
                location.href = "Pages/CRM/Constituents.aspx";
            }

        }

    }, null);
  
}

function PushField(fields, propertyName, propertyValue) {
    if (propertyValue) {
        propertyValue = propertyValue.trim();
        if (propertyValue.length > 0) {
            fields.push('"' + propertyName + '": ' + JSON.stringify(propertyValue));
        }
    }
}

function GetNewFields() {

    var p = [];

    $(modal).find('.modalcontent div.fieldblock input').each(function () {
        var property = $(this).attr('class').split(' ');
        if (property[0].startsWith('nc-')) {
            var propertyName = property[0].replace('nc-', '');
            var value = $(this).val();

            PushField(p, propertyName, value);
        }
    });

    $(modal).find('.modalcontent div.fieldblock select').each(function () {
        var property = $(this).attr('class').split(' ');
        if (property[0].startsWith('nc-')) {
            var propertyName = property[0].replace('nc-', '');
            var value = $(this).val();

            PushField(p, propertyName, value);
        }
    });


    // Address
    var addr = [];
    PushField(addr, "AddressLine1", $('.nca-AddressLine1').val());
    PushField(addr, "AddressLine2", $('.nca-AddressLine2').val());
    PushField(addr, "City", $('.nca-City').val());

    var caddr = [];
    PushField(caddr, 'AddressTypeId', $('.nca-AddressType').val());

    var caddrSet = [];

    if (addr.length > 0 && caddr.length > 0) {
        // As long as there's an address line or city, continue with the rest of the address.
        PushField(addr, "CountryId", $('.nca-Country').val());
        PushField(addr, "StateId", $('.nca-State').val());
        PushField(addr, "PostalCode", $('.nca-PostalCode').val());
        PushField(addr, "CountyId", $('.nca-County').val());
        PushField(addr, "Region1Id", $('.nca-Region1Id').val());
        PushField(addr, "Region2Id", $('.nca-Region2Id').val());
        PushField(addr, "Region3Id", $('.nca-Region3Id').val());
        PushField(addr, "Region4Id", $('.nca-Region4Id').val());

        PushField(caddr, "IsPrimary", "true");
        PushField(caddr, "ResidentType", "0");

        caddr.push('"Address": {' + addr + '}');
        caddrSet.push('{' + caddr + '}');
    }

    // Contact information
    primaries = [];
    ciSet = [];
    for (var idx = 0; idx < newContactInformationFields.length; idx++) {
        var entry = newContactInformationFields[idx];
        var value = entry.TextBox.val();
        if (value && value.trim().length > 0) {
            var ci = [];
            PushField(ci, "Info", value.trim());
            PushField(ci, "ContactTypeId", entry.Id);

            // Make sure one of each category is flagged primary
            if (primaries.indexOf(entry.Category) < 0) {
                PushField(ci, "IsPreferred", "true");
                primaries.push(entry.Category);
            }

            ciSet.push('{' + ci + '}');
        }
    }

    if (caddrSet.length > 0) {
        p.push('"ConstituentAddresses": [ ' + caddrSet + ']');
    }
    if (ciSet.length > 0) {
        p.push('"ContactInfo": [ ' + ciSet + ']');
    }

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

    PopulateDropDown('.nca-Country', 'countries', '', '');
    PopulateDropDown('.nca-AddressType', 'addresstypes', '', '');

    $('.nca-Country').change(function () {

        PopulateDropDown('.nca-State', 'states/?countryid=' + $('.nca-Country').val(), '', '');

    });

    LoadRegions($(modal).find('.regionscontainer'), 'nca-');

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

        $(this).parent().next('.accordions').find('.ui-accordion-content').hide('fast');
        $(this).parent().next('.accordions').find('.ui-accordion-header').removeClass('ui-state-active');
    });

    $('.accordion-expandall').click(function (e) {
        e.preventDefault();

        $(this).parent().next('.accordions').find('.ui-accordion-content').show('fast');
        $(this).parent().next('.accordions').find('.ui-accordion-header').addClass('ui-state-active');

    });

}

function LoadDatePickers() {

    $('.datepicker').datepicker({ 'dateFormat': 'm/d/yy' });

}

function LoadDatePair() {

    
    //$('.datepair .time').timepicker({
    //    'showDuration': true,
    //    'timeFormat': 'g:ia'
    //});

    //$('.datepair .date').datepicker({
    //    'format': 'm/d/yyyy',
    //    'autoclose': true
    //});

    // $('.datepair').datepair();
    

}

function FormatJSONDate(jsonDate) {

    var date = '';

    if (jsonDate) {
        var dt = new Date(jsonDate);
        date = (dt.getMonth() + 1) + '/' + dt.getDate() + '/' + dt.getFullYear();
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

                    // PopulateDropDown(element, route, defaultText, defaultValue, selectedValue, changecallback, completecallback)

                    PopulateDropDown('.autostate', 'states/?countryid=' + $(container).find('.autocountry').val(), '', '', data.Data.State.Id);

                    PopulateDropDown('.autocounty', 'counties/?stateid=' + data.Data.State.Id, '', '', data.Data.County.Id);
                }

                $(container).find('.autocity').val(data.Data.City);

                LoadAllRegionDropDowns(modal, prefix, data.Data);

            }

        }, null);
    }

}

function LoadTagSelector(container) {

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

                LoadAvailableTags(modal, true);

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

function LoadAvailableTags(container, isCategorySpecific) {

    var route = 'taggroups/tags';

    if (isCategorySpecific && currentEntity && currentEntity.ConstituentType) {
        route += '?category=' + currentEntity.ConstituentType.Category;
    }

    MakeServiceCall('GET', route, null, function (data) {

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

    if ($(editcontainer).hasClass('customFieldContainer')) {

        SaveCustomFields(editcontainer);
    }
    else {
        var fields = GetEditedFields(editcontainer);

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

        //SaveTagBoxes
        SaveTagBoxes(editcontainer);
    }
    
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
    DisplayMessage(heading, text, 'info', false);
}

function DisplayErrorMessage(heading, text) {
    DisplayMessage(heading, text, 'error', true);
}

function DisplayWarningMessage(heading, text) {
    DisplayMessage(heading, text, 'warning', true);
}

function DisplaySuccessMessage(heading, text) {
    DisplayMessage(heading, text, 'success', false);
}

function DisplayMessage(heading, text, icon, sticky) {

    var options = {
        heading: heading,
        text: text,
        icon: icon,
        showHideTransition: 'slide',
        allowToastClose: true,
        position: { left: 'auto', right: 100, top: 35, bottom: 'auto' }, 
    };
    if (sticky) {
        options.hideAfter = false;
    }

    $.toast(options);

}
//
// END MESSAGING



/* Note Alerts Modal */
function GetNoteAlerts(showalertsflag) {

    if (showalertsflag) {

        MakeServiceCall('GET', 'entity/' + currentEntity.Id + '/notes/alert', null, function (data) {

            if (data.Data.length > 0) {

                SetupNoteAlertModal();

                LoadNoteAlertGrid(data.Data);

                $('.notealertmodal').show();

            }

        },

            function (xhr, status, err) {

            });
    }
}

function LoadNoteAlertGrid(data) {

    var columns = [
        { dataField: 'AlertStartDate', caption: 'Alert Date Start', dataType: 'date' },
        { dataField: 'AlertEndDate', caption: 'Alert Date End', dataType: 'date' },
        { dataField: 'Title', caption: 'Title' }
    ];

    LoadGridWithData('notealertgrid', '.notealertgridcontainer', columns, null, null, EditNoteDetails, null, data, null);
}

function EditNoteDetails(id) {

    var modal = $('.notesdetailmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 500,
        resizable: false
    });

    $('.editnoteinfo').show();

    LoadNoteDetails(id);

    $('.noteTopicSelectImage').unbind('click');

    $('.noteTopicSelectImage').click(function (e) {

        SetupNoteTopicsMultiselectModal();

        $(modal).find('.tagdropdowncontainer').show();

        $('.savenotetopics').unbind('click');

        $('.savenotetopics').click(function (e) {

            var newsavednotetopics = GetCheckedNoteTopics();

            $.map(newsavednotetopics, function (topicid) {

                MakeServiceCall('GET', 'notetopics/' + topicid, null, function (data) {

                    var topic = data.Data;

                    StyleAndSetupIndividualTags(topic, function () {

                        var removeid = '#' + data.Data.Id;

                        $('.noteTopicSelect > div').remove(removeid);

                        MakeServiceCall('DELETE', 'notes/' + id + '/notetopics/' + topic.Id, null, null,

                            function (xhr, status, err) {
                                DisplayErrorMessage('Error', 'An error occurred during saving the note topics.');
                            });

                    }, function (xhr, status, err) {
                        DisplayErrorMessage('Error', 'An error occurred during saving the note topics.');
                    });

                });

            });

            $(modal).find('.tagdropdowncontainer').hide();

            ClearNoteTopicSelectModal();

            newsavednotetopics = null;

        });

        $('.cancelnotetopics').click(function (e) {

            e.preventDefault();

            $(modal).find('.tagdropdowncontainer').hide();

            ClearNoteTopicSelectModal();

        });


    });

    $('.cancelnotesmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

        ClearNoteTopicTagBox(modal);

        $('.editnoteinfo').hide();

        $('.nd-CreatedBy').text('');
        $('.nd-UpdatedBy').text('');
        $('.nd-CreatedOn').text('');
        $('.nd-UpdatedOn').text('');

    });

    $('.savenotedetails').unbind('click');

    $('.savenotedetails').click(function () {

        var topicsavelist = GetNoteTopicsToSave();

        var item = GetNoteDetailsToSave(modal);

        MakeServiceCall('PATCH', 'notes/' + id, item, function (data) {

            MakeServiceCall('POST', 'notes/' + data.Data.Id + '/notetopics/', JSON.stringify(topicsavelist), function (data) {

                DisplaySuccessMessage('Success', 'Note topics saved successfully.');

            }, function (xhr, status, err) {

                DisplayErrorMessage('Error', 'An error occurred during saving the Note topics.');

            });

            DisplaySuccessMessage('Success', 'Note Details saved successfully.');

            CloseModal(modal);

            $('.editnoteinfo').hide();

            $('.nd-CreatedBy').text('');
            $('.nd-UpdatedBy').text('');
            $('.nd-CreatedOn').text('');
            $('.nd-UpdatedOn').text('');

            ClearNoteTopicTagBox(modal);

            LoadNoteDetailsGrid();

        }, function (xhr, status, err) {

            DisplayErrorMessage('Error', 'An error occurred during saving the Note Details.');
        });

    });
}

function SetupNoteAlertModal() {

    var modal = $('.notealertmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 500,
        resizable: false
    });

    $('.cancelnotealertmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

}
/* End Note Alerts Modal */



// BUSINESS UNIT
//

function GetCurrentBusinessUnit() {

    currentBusinessUnitId = sessionStorage.getItem('CURRENT_BUSINESS_UNIT');

    if (currentBusinessUnitId == null) {

        BusinessUnitModal();

    }
}

function BusinessUnitModal() {
    
    $('.editbusinessunit').click(function (e) {

        LoadBusinessUnitDropDown(currentBusinessUnitId);

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
                
                if (value.Id === $('.bu-currentbu').val()) {

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

function LoadBusinessUnitDropDown(currentBusinessUnitId) {

    PopulateDropDown('.bu-currentbu', 'users/' + currentUser.Id + '/businessunit', '', '', currentBusinessUnitId);
    
}

//
// END BUSINESS UNIT

// FORM VALIDATION
//
function InitRequiredLabels(formClassName) {
    formClassName.replace(".", "");
    $('.' + formClassName).find('.required').each(function (index, el) {
        var labelElement = $(this).prev();
        if (labelElement[0].innerHTML.indexOf("*") === -1){
            labelElement[0].innerHTML = labelElement[0].innerHTML + " *";
        }
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

// DYNAMIC MARKUP
//
function CreateBasicFieldBlock(labelText, controlType, controlClass, appendContainer) {

    var fieldblock = $('<div>').addClass('fieldblock');
    $('<label>').text(labelText).appendTo(fieldblock);
    $(controlType).addClass(controlClass).appendTo(fieldblock);
    $(fieldblock).appendTo(appendContainer);

}

function CreateSaveAndCancelButtons(saveClass, saveFunction, cancelClass, cancelFunction, appendContainer) {

    var buttons = $('<div>').addClass('dynamicbuttons').appendTo(appendContainer);

    if (saveFunction) {

        $('<input>').attr('type', 'button').addClass(saveClass).val('Save').click(saveFunction).appendTo(buttons);

    }
    
    if (cancelFunction) {

        $('<a>').addClass(cancelClass).text('Cancel').attr('href', '#').click(cancelFunction).appendTo(buttons);

    }

}
//
// END DYNAMIC MARKUP

function MaskFields() {
    $('.date').each(function () {
        if ($(this).val() != null && $(this).val() != '') {
            $(this).val($.datepicker.formatDate('mm/dd/yy', new Date($(this).val())));
        }
    })
    $('.date').mask('00/00/0000');

    $('.time').mask('00:00:00');
    $('.datetime').mask('00/00/0000 00:00:00');
    $('.money').mask("#0.00", { reverse: true });
    $('.money').each(function () {
        $(this).val($(this).val().replace('$', '').replace(',', ''));
    })
    $('.moneynegative').maskMoney({ prefix: '$', allowNegative: true, thousands: ',', decimal: '.', affixesStay: false });
    $('.phone').mask('(000) 000-0000');
    $('.ip_address').mask('0ZZ.0ZZ.0ZZ.0ZZ', {
        translation: {
            'Z': {
                pattern: /[0-9]/, optional: true
            }
        }
    });
}

function FormatFields() {
    $(".decimal").each(function () {
        if ($(this).val() != null && $(this).val() != '') {
            $(this).val(parseFloat($(this).val(), 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
        }
    });
    $(".money").each(function () {
        if ($(this).val() != null && $(this).val() != '') {
            $(this).val('$' + parseFloat($(this).val(), 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
        }
    });
    $("input.date").each(function () {
        if ($(this).val() != null && $(this).val() != '') {
            $(this).val($.datepicker.formatDate('D M dd, yy', new Date($(this).val())));
        }
    });
    $("label.date").each(function () {
        if ($(this).html() != null && $(this).html() != '') {
            $(this).html($.datepicker.formatDate('D M dd, yy', new Date($(this).html())));
        }
    });
    $(".percent").each(function () {
        if ($(this).val() != null && $(this).val() != '') {
            $(this).val($(this).val() + '%');
        }
    });
}

// Take a string like "Approved on 2017-06-09T13:37:09.1111520Z" and return "Approved on 6/9/2017, 9:37:09 AM"
// DateTime values should be in ToString("O") format.
function FormatDateTimeStrings(str) { 
    var rexp = /\d{4}-\d\d-\d\dT\d\d:\d\d:\d\d(.\d{0,9})?(Z|[+-]\d\d:\d\d)?/;
    do {
        var result = rexp.exec(str);
        if (result) {
            var dt = new Date(result[0]);
            if (dt.getMilliseconds() == 0 && dt.getSeconds() == 0 && dt.getMinutes() == 0 && dt.getHours() == 0) {
                str = str.replace(rexp, dt.toLocaleDateString());
            }
            else {
                str = str.replace(rexp, dt.toLocaleString());
            }
        }
    } while (result);    

    return str;
}