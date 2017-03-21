
function AddDefaultOption(e, text, val) {

    var option = $('<option>').val('').text('');
    $(option).appendTo($(e));

}

function GetApiHeaders() {

    var token = sessionStorage.getItem(AUTH_TOKEN_KEY);
    var headers = {};

    if (token) {
        headers.Authorization = 'Bearer ' + token;
    }

    return headers;

}

function MakeServiceCall(method, route, item, successCallback, errorCallback) {

    $.ajax({
        url: WEB_API_ADDRESS + route,
        method: method,
        contentType: 'application/json; charset-utf-8',
        data: item,
        dataType: 'json',
        headers: GetApiHeaders(),
        crossDomain: true,
        success: function (data) {

            if (data && data.IsSuccessful) {
                if (successCallback) {
                    successCallback(data);
                }
            }
            else {
                DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
            if (errorCallback) {
                errorCallback();
            }
        }
    });

}


/* POPULATE DROPDOWN CONTROLS */
function AddDefaultOption(element, text, val) {

    var option = $('<option>').val('').text('');
    $(option).appendTo($(element));

}

function PopulateDropDown(element, route, selectedValue) {

    ClearElement(element);
    
    MakeServiceCall('GET', route, function (data) {
        if (data.Data) {

            $.map(data.Data, function (item) {

                option = $('<option>').val(item.Id).text(item.DisplayName);
                $(option).appendTo($(element));

            });

            if (selectedValue) {
                $(element).val(selectedValue);
            }
        }
    }, null);

}

function PopulateDropDown(element, route, defaultText, defaultValue, selectedValue, callback) {

    ClearElement(element);

    AddDefaultOption(element, defaultText, defaultValue);

    MakeServiceCall('GET', route, function (data) {
        if (data.Data) {

            $.map(data.Data, function (item) {

                option = $('<option>').val(item.Id).text(item.DisplayName);
                $(option).appendTo($(element));

            });

            if (selectedValue) {
                $(element).val(selectedValue);
            }

        }
    }, null);

    if (callback) {

        $(element).unbind('change');

        $(element).change(function () {
            callback();
        });

    }

}
/* END POPULATE DROPDOWN CONTROLS */



/* POPULATE TAGBOX CONTROLS */
function LoadTagBoxes(tagBox, container, routeForAllOptions, routeForSelectedOptions) {
    if ($.type(container) === "string" && container.indexOf('.') != 0)
        container = '.' + container;

    $(container).html('');

    var selectedItems = [];

    MakeServiceCall('GET', routeForSelectedOptions, null, function (data) {
        data.Data.forEach(function (item) {
            selectedItems.push(item.Id);
        });
        DisplayTagBox(routeForAllOptions, tagBox, container, selectedItems);
    }, null);

}

function DisplayTagBox(routeForAllOptions, tagBox, container, selectedItems) {

    var tagBoxControl = $('<div>').addClass(tagBox);

    MakeServiceCall('GET', routeForAllOptions, null, function (data) {
        $(tagBoxControl).dxTagBox({
            items: data.Data,
            value: selectedItems,
            displayExpr: 'DisplayName',
            valueExpr: 'Id',
            showClearButton: true,
            disabled: true
        });

        $(tagBoxControl).appendTo(container);
    }, null);

}
/* END POPULATE TAGBOX CONTROLS */



/* DATAGRID FUNCTIONALITY */

function LoadGrid(container, gridClass, columns, route, selected, modalClass, prefix, editModalClass, newModalClass, modalWidth, showDelete, showFilter, showGroup, onComplete) {

    if ($.type(container) === "string" && container.indexOf('.') != 0) {
        container = '.' + containr;
    }

    var datagrid = $('<div>').addClass(grid);

    if (typeof (showFilter) == 'undefined' || showFilter == null) {
        showFilter = false; // Hide the filter by default
    }

    if (typeof (showGroup) == 'undefined' || showGroup == null) {
        showGroup = false; // Hide the group by default
    }

    if (newModalClass) {
        // Add link for new modal
    }

    if (editModalClass) {
        // Add column for edit
        columns.push({
            width: '100px',
            alignment: 'center',
            cellTemplate: function(container, options) {
                $('<a/>')
                    .addClass('editLink')
                    .text('Edit')
                    .click(function(e) {
                        e.preventDefault();

                        var id = $(this).parent().parent().find('td:not(:empty):first').text();
                        EditEntity(route, prefix, id, editModal, modalWidth);
                    })
                    .appendTo(container);
            }
        });
    }

    if (showDelete) {
        // add column for delete
        columns.push({
            width: '100px',
            alignment: 'center',
            cellTemplate: function (container, options) {
                $('<a/>')
                    .addClass('deleteLink')
                    .text('Delete')
                    .click(function (e) {
                        e.preventDefault();

                        var id = $(this).parent().parent().find('td:not(:empty):first').text();
                        // ConfirmModal(message, yes, no)
                        ConfirmModal('Are you sure you want to delete this item?', function () {
                            DeleteEntity(route, id);
                        }, null);
                    })
                    .appendTo(container);
            }
        });
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
            visible: showGroup,
            allowColumnDragging: true
        },
        filterRow: {
            visible: showFilter,
            showOperationChooser: false
        },
        selection: {
            mode: 'single',
            allowSelectAll: false
        },
        onRowClick: function (info) {

            if (selected) {
                selected(info);
            }

        }
    });

    $(datagrid).appendTo($(container));

}

function RefreshGrid() {

}

function LoadAuditGrid(grid, container, columns, route, showFilterRow) {

    if (showFilterRow == '' || showFilterRow === undefined)
        showFilterRow = false;

    if ($.type(container) === "string" && container.indexOf('.') != 0)
        container = '.' + container;

    $.ajax({
        url: WEB_API_ADDRESS + route,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            LoadAuditGridWithData(grid, container, columns, route, showFilterRow, data);

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error loading grid.');
        }
    });
}

function LoadAuditGridWithData(grid, container, columns, route, showFilterRow, data) {

    $(container).html('');

    var datagrid = $('<div>').addClass(grid);
    
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
            visible: showFilterRow,
            showOperationChooser: false
        },
        selection: {
            mode: 'single',
            allowSelectAll: false
        }
    });

    $(datagrid).appendTo($(container));
}

function LoadGrid(grid, container, columns, route, selected, editMethod, deleteMethod, oncomplete) {

    if (container.indexOf('.') != 0)
        container = '.' + container;

    $.ajax({
        url: WEB_API_ADDRESS + route,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            LoadGridWithData(grid, container, columns, route, selected, editMethod, deleteMethod, data);

            if (oncomplete) {
                oncomplete();
            }
                        
        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });
}

function LoadGridWithData(grid, container, columns, route, selected, editMethod, deleteMethod, data) {
    
    $(container).html('');

    var datagrid = $('<div>').addClass(grid);

    if (editMethod) {
        columns.push({
            width: '100px',
            alignment: 'center',
            cellTemplate: function(container, options) {
                $('<a/>')
                    .addClass('editthing')
                    .text('Edit')
                    .click(function(e) {
                        e.preventDefault();

                        editMethod($(this).parent().parent().find('td:not(:empty):first').text());
                    })
                    .appendTo(container);
            }
        });
    }

    if (deleteMethod) {
        columns.push({
            width: '100px',
            alignment: 'center',
            cellTemplate: function (container, options) {
                $('<a/>')
                    .addClass('editthing')
                    .text('Delete')
                    .click(function (e) {
                        e.preventDefault();

                        deleteMethod($(this).parent().parent().find('td:not(:empty):first').text());
                    })
                    .appendTo(container);
            }
        });
    }

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
            selection: {
                mode: 'single',
                allowSelectAll: false
        },
        onRowClick: function (info) {

            if (selected) {
                selected(info);
            }

        }
    });

    $(container).append($(datagrid));
}

function NewModalLink() {



}

function NewEntityModal(route, prefix, newModalLink, modalClass, modalWidth) {

    $(newModalLink).click(function (e) {

        e.preventDefault();

        var modal = $(modalClass).dialog({
            closeOnEscape: false,
            modal: true,
            width: modalWidth,
            resizable: false
        });

        $(modal).find('.cancelmodal').click(function (e) {
            e.preventDefault();
            CloseModal(modal);
        });

        $(modal).find('.savebutton').unbind('click');

        $(modal).find('.savebutton').click(function () {

            var item = GetModalFieldsToSave(prefix);

            MakeServiceCall('POST', route, item, function () {
                DisplaySuccessMessage("Success", "Save successful.");

                CloseModal(modal);
                RefreshGrid();
            }, null);

        });
    });

}

function EditEntity(route, prefix, id, modalClass, modalWidth) {

    var modal = $(modalClass).dialog({
        closeOnEscape: false,
        modal: true,
        width: modalWidth,
        resizable: false
    });
    
    LoadEntity(route, id, prefix);

    $(modal).find('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $(modal).find('.savebutton').unbind('click');

    $(modal).find('.savebutton').click(function () {

        var item = GetModalFieldsToSave(prefix);
        
        MakeServiceCall('PATCH', route + '/' + id, item, function () {
            DisplaySuccessMessage("Save successful.");

            CloseModal(modal);
            RefreshGrid();
        }, null);

    });

}

function DeleteEntity(route, id) {

    MakeServiceCall('DELETE', route + '/' + id, null, function () {

        DisplaySuccessMessage('Success', 'Delete successful.');
        RefreshGrid();

    }, null);

}

function GetModalFieldsToSave(prefix) {

    var p = [];

    $(modal).find('input').not('input[type="button"]').each(function () {

        var property = $(this).attr('class').split(' ');

        if (property[0].indexOf(prefix) > 0) {
            var propertyName = property[0].replace(prefix, '');
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
        }
        
    });

    $(modal).find('select').each(function () {

        var property = $(this).attr('class').split(' ');

        if (property[0].indexOf(prefix) > 0) {

            var propertyName = property[0].replace(prefix, '');
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

        }
    });

    p = '{' + p + '}';

    return p;

}

function LoadEntity(route, id, prefix) {

    MakeServiceCall('GET', route + '/' + id, item, function (data) {

        for (var property in data.Data) {
            $(prefix + property).val(data.Data[property]);
        }

    }, null);
}
/* END DATAGRID FUNCTIONALITY */

