﻿

function LoadRegions(container, prefix) {

    if (container.indexOf('.') != 0) {
        container = '.' + container;
    }

    if (prefix.indexOf('.') != 0) {
        prefix = '.' + prefix;
    }

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'regionlevels',
        data: item,
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

function LoadRegionDropDown(prefix, level, selectedvalue) {

    var route = 'regions/' + level;

    if (selectedvalue)
        route = route + '/' + selectedvalue;
    else
        route = route + '/null';

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + route,
        data: item,
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

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the region levels.');
        }
    });
}
