﻿

function LoadRegions(container, prefix) {

    if (container.indexOf('.') != 0) {
        container = '.' + container;
    }

    if (prefix.indexOf('.') != 0) {
        prefix = '.' + prefix;
    }

    $(container).html('');

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'regionlevels',
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            $.map(data.Data, function (item) {

                var currentRegion = CreateRegionControl(container, prefix, item.Level);

                $(currentRegion).show();
                $('.region' + item.Level + 'label').text(item.Label);

                if (!item.IsChildLevel) {
                    LoadRegionDropDown(prefix, item.Level);
                } else {
                    $(prefix + 'Region' + (item.Level - 1) + 'Id').change(function () {

                        ClearElement(prefix + 'Region' + item.Level + 'Id');

                        if ($(prefix + 'Region' + (item.Level - 1) + 'Id option:selected').text().length > 0) {
                            LoadRegionDropDown(prefix, (item.Level), $(prefix + 'Region' + item.Level + 'Id').val());
                        }

                    });
                }

            });

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the region levels.');
        }
    });

}

function CreateRegionControl(container, prefix, index) {

    if (prefix.indexOf('.') == 0) {
        prefix = prefix.replace('.', '');
    }

    var field = $('<div>').addClass('fieldblock').addClass('region' + index);
    var label = $('<label>').addClass('region' + index + 'label').appendTo($(field));
    var ddl = $('<select>').addClass(prefix + 'Region' + index + 'Id').appendTo($(field));

    $(field).appendTo($(container));

    return field;

}

function LoadAllRegionDropDowns(prefix, address) {

    if (address) {

        if (address.Region1) {
            LoadRegionDropDown(prefix, 1, null, address.Region1.Id);
        }

        if (address.Region2 && address.Region2.Id && address.Region2.ParentRegionId) {
            LoadRegionDropDown(prefix, 2, address.Region2.ParentRegionId, address.Region2.Id);
        }
    
        if (address.Region3 && address.Region3.Id && address.Region3.ParentRegionId) {
            LoadRegionDropDown(prefix, 3, address.Region3.ParentRegionId, address.Region3.Id);
        }
    
        if (address.Region4 && address.Region4.Id && address.Region4.ParentRegionId) {
            LoadRegionDropDown(prefix, 4, address.Region4.ParentRegionId, address.Region4.Id);
        }

    }

}

function LoadRegionDropDown(prefix, level, parentid, selectedvalue) {

    var route = 'regionlevels/' + level + '/regions/';

    if (parentid) {
        route = route + parentid;
    }

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + route,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            var currentdropdown = $(prefix + 'Region' + level + 'Id');
            ClearElement(prefix + 'Region' + level + 'Id');
            AddDefaultOption($(currentdropdown), '', '');

            $.map(data.Data, function (item) {

                option = $('<option>').val(item.Id).text(item.DisplayName);
                $(option).appendTo($(currentdropdown));

            });

            if (selectedvalue) {

                $(currentdropdown).val(selectedvalue);

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the region levels.');
        }
    });
}
