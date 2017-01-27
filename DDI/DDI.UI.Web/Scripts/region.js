

function LoadRegions(container, prefix) {

    GetRegionLevels(container, prefix);

}

function GetRegionLevels(container, prefix) {

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
                    LoadRegionDropDown(item.Level);
                } else {
                    $('.na-Region' + (item.Level - 1) + 'Id').change(function () {

                        ClearElement('.na-Region' + item.Level + 'Id');

                        if ($('.na-Region' + (item.Level - 1) + 'Id option:selected').text().length > 0) {
                            LoadRegionDropDown((item.Level), $('.na-Region' + item.Level + 'Id').val());
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

    var field = $('<div>').addClass('.fieldblock').addClass('region' + index);
    var label = $('<label>').addClass('region' + index + 'label').appendTo($(field));
    var ddl = $('<select>').addClass(prefix + 'Region' + index + 'Id').appendTo($(field));

    $(field).appendTo($(container));

    return field;

}

function LoadRegionDropDown(level, selectedvalue) {

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

            var currentdropdown = $('.na-Region' + level + 'Id');
            ClearElement('.na-Region' + level + 'Id');
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
