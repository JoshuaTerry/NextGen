﻿
function AddDefaultOption(e, text, val) {

    var option = $('<option>').val('null').text('');
    $(option).appendTo($(e));

}

function MakeServiceCall(e, method, selectedValue) {

    $.ajax({
        url: WEB_API_ADDRESS + method,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $.map(data.Data, function (item) {

                option = $('<option>').val(item.Id).text(item.DisplayName);
                $(option).appendTo($(e));

            });

            if (selectedValue) {
                $(e).val(selectedValue);
            }

        },
        failure: function (response) {
            alert(response);
        }
    });

}

function PopulateDropDown(e, method, selectedValue) {

    ClearElement(e);
    
    MakeServiceCall(e, method, selectedValue);

}

function PopulateDropDown(e, method, defaultText, defaultValue, selectedValue, callback) {

    ClearElement(e);
    AddDefaultOption(e, defaultText, defaultValue);

    MakeServiceCall(e, method, selectedValue);

    if (callback) {

        $(e).unbind('change');

        $(e).change(function () {
            callback();
        });

    }

}

function LoadTagBoxes(tagBox, container, routeForAllOptions, routeForSelectedOptions) {
    if (container.indexOf('.') != 0)
        container = '.' + container;

    $(container).html('');

    var tagBoxControl = $('<div>').addClass(tagBox);
    var selectedItems = [];

    $.ajax({
        url: routeForSelectedOptions,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {
            data.Data.forEach(function (item) {
                selectedItems.push(item);
            });
        },
        failure: function (response) {
            alert(response);
        }
    });

    $.ajax({
        url: WEB_API_ADDRESS + routeForAllOptions,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {
            $(tagBoxControl).dxTagBox({
                items: data.Data,
                value: selectedItems,
                displayExpr: 'DisplayName',
                valueExpr: 'Id',
                showClearButton: true,
                disabled: true
            });

            $(tagBoxControl).appendTo(container);
        },
        failure: function(response) {
            alert(response);
        }
    });
}

function LoadGrid(grid, container, columns, route, selected, edit) {

    if (container.indexOf('.') != 0)
        container = '.' + container;

    $(container).html('');

    var datagrid = $('<div>').addClass(grid);

    if (edit) {
        columns.push({
            width: '100px',
            alignment: 'center',
            cellTemplate: function (container, options) {
                $('<a/>').addClass('editthing')
                    .text('Edit')
                    .click(function (e) {
                        e.preventDefault();

                        edit($(this).parent().parent().find('td').first().text())
                    })
                    .appendTo(container);
            }
        });
    }

    $.ajax({
        url: route,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            var actualData = data;

            if (data.Data) {
                actualData = data.Data;
            }

            $(datagrid).dxDataGrid({
                dataSource: actualData,
                columns: columns,
                paging: {
                    pageSize: 25
                },
                pager: {
                    showNavigationButtons: true,
                    showPageSizeSelector: true,
                    showInfo: true,
                    allowedPageSizes: [15, 25, 50, 100]
                },
                groupPanel: {
                    visible: false,
                    allowColumnDragging: true
                },
                filterRow: {
                    visible: true,
                    showOperationChooser: false
                },
                onRowClick: function (info) {

                    if (selected) {
                        selected(info);
                    }

                }
            });

            $(datagrid).appendTo($(container));

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error loading grid.');
        }
    });

}
