
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

function LoadTagBoxes(tagBox, container, route) {
    if (container.indexOf('.') != 0)
        container = '.' + container;

    $(container).html('');

    var tagBoxControl = $('<div>').addClass(tagBox);

    $.ajax({
        url: WEB_API_ADDRESS + route,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {
            $(tagBoxControl).dxTagBox({
                dataSource: data.Data,
                displayExpr: 'DisplayName',
                valueExpr: 'Id',
                showClearButton: true,
            });

            $(tagBoxControl).appendTo(container);
        },
        failure: function(response) {
            alert(response);
        }
    });


}

function LoadGrid(grid, container, columns, route, selected, edit, data) {
    if (edit) {
        columns.push({
            width: '100px',
            alignment: 'center',
            cellTemplate: function(container, options) {
                $('<a/>')
                    .addClass('editthing')
                    .text('Edit')
                    .click(function(e) {
                        e.preventDefault();

                        edit($(this).parent().parent().find('td').first().text());
                    })
                    .appendTo(container);
            }
        });
    }
    LoadGridFromHateoas(grid, container, columns, route, selected, null, null, null, null, null);
}

function LoadGridFromHateoas(grid, container, columns, route, selected, hateoasRouteNameBase, editMethod, deleteMethod, deleteMessage, data) {

    if (container.indexOf('.') != 0)
        container = '.' + container;

    $(container).html('');

    var datagrid = $('<div>').addClass(grid);

    if (editMethod) {
        columns.push({
            width: '100px',
            alignment: 'center',
            cellTemplate: function (container, options) {
                $('<a/>')
                    .addClass('editthing')
                    .text('Edit')
                    .click(function (e) {
                        e.preventDefault();

                        editMethod(options.data.FormattedLinks["Self"].Href, options.data.FormattedLinks["Update" + hateoasRouteNameBase].Href);
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
                    .text('Remove')
                    .click(function (e) {
                        e.preventDefault();

                        deleteMethod(options.data.FormattedLinks["Delete" + hateoasRouteNameBase].Href, options.data.FormattedLinks["Delete" + hateoasRouteNameBase].Method, deleteMessage + options.data.DisplayName + "?");
                    })
                    .appendTo(container);
            }
        });
    }
    if (data) {
        LoadGridData(data, datagrid, columns, container, selected);
    } else {
        $.ajax({
            url: route,
            method: 'GET',
            contentType: 'application/json; charset-utf-8',
            dataType: 'json',
            crossDomain: true,
            success: function (data) {
                LoadGridData(data, datagrid, columns, container, selected);
            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error loading grid.');
            }
        });
    }
}

function LoadGridData(data, datagrid, columns, container, selected) {

    var actualData = data;

    if (data.Data) {
        actualData = data.Data;
    }

    if (actualData && Array.isArray(actualData)) {
        actualData.forEach(function(eachItem) {
            var links = [];
            if (eachItem.Links) {
                $.map(eachItem.Links,
                    function(link) {

                        links[link.Relationship] = {
                            Href: link.Href,
                            Method: link.Method
                        };

                    });
                eachItem.FormattedLinks = links;
            };
        });
    }

    $(datagrid)
        .dxDataGrid({
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
            onRowClick: function(info) {

                if (selected) {
                    selected(info);
                }

            }
        });

    $(datagrid).appendTo($(container));

}

function EditEntity(getUrl, patchUrl, entityType, modalClassName, saveButtonClassName, modalWidth, loadEntityMethod, loadEntityGridMethod, getEntityToSave) {

    var modal = $(modalClassName).dialog({
        closeOnEscape: false,
        modal: true,
        width: modalWidth,
        resizable: false
    });

    loadEntityMethod(getUrl, modal);

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $(saveButtonClassName).unbind('click');

    $(saveButtonClassName).click(function () {

        var item = getEntityToSave(modal, true);

        $.ajax({
            type: 'PATCH',
            url: patchUrl,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage("Success", entityType + " saved successfully.");

                CloseModal(modal);

                loadEntityGridMethod();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage("Error", "An error occurred during the saving of the " + entityType + ".");
            }
        });

    });

}

function NewEntityModal(entityName, newEntityModalClassName, entityModalClassName, entityModalWidth, prePopulateNewModal, saveEntityClassName, getEntityToSave, ajaxMethod, ajaxUrl, loadEntityGrid) {

    $(newEntityModalClassName)
        .click(function (e) {

            e.preventDefault();

            var modal = $(entityModalClassName)
                .dialog({
                    closeOnEscape: false,
                    modal: true,
                    width: entityModalWidth,
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

            $(saveEntityClassName).unbind('click');

            $(saveEntityClassName)
                .click(function () {

                    var item = getEntityToSave(modal, false);

                    $.ajax({
                        type: ajaxMethod,
                        url: ajaxUrl,
                        data: item,
                        contentType: 'application/x-www-form-urlencoded',
                        crossDomain: true,
                        success: function () {

                            DisplaySuccessMessage("Success", entityName + " saved successfully.");

                            CloseModal(modal);

                            loadEntityGrid();

                        },
                        error: function (xhr, status, err) {
                            DisplayErrorMessage("Error", "An error occurred during the saving of the " + entityName + ".");
                        }
                    });

                });
        });
}

function LoadEntity(url, modal, ajaxMethod, loadEntityData, entityName) {

    $.ajax({
        type: ajaxMethod,
        url: url,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {
            loadEntityData(data, modal);
        },
        error: function (xhr, status, err) {
            DisplayErrorMessage("Error", "An error occurred during the loading of the " + entityName + ".");
        }
    });
}

