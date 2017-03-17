﻿
function AddDefaultOption(e, text, val) {

    var option = $('<option>').val('').text('');
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
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
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
    if ($.type(container) === "string" && container.indexOf('.') != 0)
        container = '.' + container;

    $(container).html('');

    var selectedItems = [];

    $.ajax({
        url: WEB_API_ADDRESS + routeForSelectedOptions,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {
            data.Data.forEach(function (item) {
                selectedItems.push(item.Id);
            });
            DisplayTagBox(routeForAllOptions, tagBox, container, selectedItems);
        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}

function DisplayTagBox(routeForAllOptions, tagBox, container, selectedItems) {

    var tagBoxControl = $('<div>').addClass(tagBox);

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
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    }); 
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

function EditEntity(modalClass, saveButtonClass, modalWidth, loadEntityMethod, loadEntityGrid, getEntityToSave, entityName, route, id) {

    var modal = $(modalClass).dialog({
        closeOnEscape: false,
        modal: true,
        width: modalWidth,
        resizable: false
    });
    
    LoadEntity(route, id, modal, loadEntityMethod, entityName);

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $(saveButtonClass).unbind('click');

    $(saveButtonClass).click(function () {

        var item = getEntityToSave(modal, true);

        $.ajax({
            type: 'PATCH',
            url: WEB_API_ADDRESS + route + '/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage("Success", entityName + " saved successfully.");

                CloseModal(modal);

                loadEntityGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
            }
        });

    });

}

function NewEntityModal(newModalLink, modalClass, saveButtonClass, modalWidth, prePopulateNewModal, loadEntityGrid, getEntityToSave, entityName, route) {

    $(newModalLink).click(function (e) {

        e.preventDefault();

        var modal = $(modalClass).dialog({
            closeOnEscape: false,
            modal: true,
            width: modalWidth,
            resizable: false
        });

        if (prePopulateNewModal) {
            prePopulateNewModal(modal);
        }

        $('.cancelmodal')
            .click(function (e) {
                e.preventDefault();
                CloseModal(modal);
            });

        $(saveButtonClass).unbind('click');

        $(saveButtonClass).click(function () {

            var item = getEntityToSave(modal, false);

            $.ajax({
                type: 'POST',
                url: WEB_API_ADDRESS + route,
                data: item,
                contentType: 'application/x-www-form-urlencoded',
                crossDomain: true,
                success: function () {

                    DisplaySuccessMessage("Success", entityName + " saved successfully.");

                    CloseModal(modal);

                    loadEntityGrid();

                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
                }
            });

        });
    });

}

function LoadEntity(route, id, modal, loadEntityData, entityName) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + route + '/' + id,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {
            loadEntityData(data, modal);
        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });
}

