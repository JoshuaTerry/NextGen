

function LoadRegions(container, prefix) {

    if ($.type(container) === "string" && container.indexOf('.') != 0) {
        container = '.' + container;
    }

    if (prefix.indexOf('.') != 0) {
        prefix = '.' + prefix;
    }

    $(container).html('');

    MakeServiceCall('GET', 'regionlevels', item, function (data) {

        if (data.Data) {
            $.map(data.Data, function (item) {

                var currentRegion = CreateRegionControl(container, prefix, item.Level);

                $(currentRegion).show();
                $('.region' + item.Level + 'label').text(item.Label);

                if (!item.IsChildLevel) {
                    LoadRegionDropDown(container, prefix, item.Level);
                } else {
                    $(prefix + 'Region' + (item.Level - 1) + 'Id').change(function () {

                        ClearElement(prefix + 'Region' + item.Level + 'Id');

                        if ($(prefix + 'Region' + (item.Level - 1) + 'Id option:selected').text().length > 0) {
                            LoadRegionDropDown(container, prefix, (item.Level), $(prefix + 'Region' + item.Level + 'Id').val());
                        }

                    });
                }

            });
        }

    }, null);

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

function LoadAllRegionDropDowns(container, prefix, address) {

    if (address) {

        if (address.Region1) {
            LoadRegionDropDown(container, prefix, 1, null, address.Region1.Id);
        }

        if (address.Region2 && address.Region2.Id && address.Region2.ParentRegionId) {
            LoadRegionDropDown(container, prefix, 2, address.Region2.ParentRegionId, address.Region2.Id);
        }
    
        if (address.Region3 && address.Region3.Id && address.Region3.ParentRegionId) {
            LoadRegionDropDown(container, prefix, 3, address.Region3.ParentRegionId, address.Region3.Id);
        }
    
        if (address.Region4 && address.Region4.Id && address.Region4.ParentRegionId) {
            LoadRegionDropDown(container, prefix, 4, address.Region4.ParentRegionId, address.Region4.Id);
        }

    }

}

function LoadRegionDropDown(container, prefix, level, parentid, selectedvalue) {

    var route = 'regionlevels/' + level + '/regions/';

    if (parentid) {
        route = route + parentid;
    }

    MakeServiceCall('GET', route, item, function (data) {

        if (data.Data) {
            var currentdropdown = $(container).find(prefix + 'Region' + level + 'Id');
            ClearElement(currentdropdown);
            AddDefaultOption($(currentdropdown), '', '');

            $.map(data.Data, function (item) {

                option = $('<option>').val(item.Id).text(item.DisplayName);
                $(option).appendTo($(currentdropdown));

            });

            if (selectedvalue) {

                $(currentdropdown).val(selectedvalue);

            }
        }

    }, null);
}
