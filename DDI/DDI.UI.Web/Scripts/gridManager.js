var GridManager = function () {

    loadGrid = function (container, gridClass, columns, getRoute, saveRoute, selected, prefix, editModalClass, newModalClass, modalWidth, showDelete, showFilter, showGroup, onComplete) {

        if ($.type(container) === "string" && container.indexOf('.') != 0) {
            container = '.' + container;
        }

        var newlink = null;

        if (newModalClass) {
            newlink = function () {
                newModalLink(container, saveRoute, prefix, newModalClass, modalWidth, refreshGrid);
            }
        }

        var refreshGrid = function () {
            loadGridData(container, gridClass, columns, getRoute, selected, newlink, showFilter, showGroup, onComplete);
        }

        columns.push({
            width: '100px',
            alignment: 'center',
            allowResizing: false,
            cellTemplate: function (container, options) {
                if (editModalClass) {
                    // Add button for edit

                    $('<a/>')
                        .addClass('actionbuttons')
                        .addClass('editbutton')
                        .attr('title', 'Edit')
                        .click(function (e) {
                            e.preventDefault();

                            editEntity(saveRoute, prefix, options.data.Id, editModalClass, modalWidth, refreshGrid);
                        })
                        .appendTo(container);
                }

                if (showDelete) {
                    // Add button for delete

                    $('<a/>')
                        .addClass('actionbuttons')
                        .addClass('deletebutton')
                        .attr('title', 'Delete')
                        .click(function (e) {
                            e.preventDefault();

                            ConfirmModal('Are you sure you want to delete this item?', function () {
                                deleteEntity(saveRoute, options.data.Id, refreshGrid);
                            }, null);
                        })
                        .appendTo(container);
                }
            }
        });

        loadGridData(container, gridClass, columns, getRoute, selected, newlink, showFilter, showGroup, onComplete);

    }

    loadGridData = function (container, grid, columns, getRoute, selected, newlink, showFilter, showGroup, onComplete) {

        $('.' + grid).remove();

        var datagrid = $('<div>').addClass(grid);

        if (typeof (showFilter) == 'undefined' || showFilter == null) {
            showFilter = false; // Hide the filter by default
        }

        if (typeof (showGroup) == 'undefined' || showGroup == null) {
            showGroup = false; // Hide the group by default
        }

        MakeServiceCall('GET', getRoute, null, function (data) {

            $(datagrid).dxDataGrid({
                dataSource: {
                    store: {
                        data: data.Data,
                        type: 'array',
                        key: 'Id'
                    }
                },
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
                allowColumnResizing: true,
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

            if (newlink) {
                // Add link for new modal
                newlink();
            }

            if (onComplete) {
                onComplete(data);
            }

        }, null);

    }

    customLoadGrid = function (grid, container, columns, route, selected, editMethod, deleteMethod, oncomplete) {

        if (container.indexOf('.') != 0)
            container = '.' + container;

        $.ajax({
            url: WEB_API_ADDRESS + route,
            method: 'GET',
            contentType: 'application/json; charset-utf-8',
            dataType: 'json',
            crossDomain: true,
            headers: GetApiHeaders(),
            success: function (data) {

                loadGridWithData(grid, container, columns, route, selected, editMethod, deleteMethod, data, oncomplete);

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
            }
        });

    }

    loadGridWithData = function (grid, container, columns, route, selected, editMethod, deleteMethod, data, oncomplete) {

        $(container).html('');

        var datagrid = $('<div>').addClass(grid);

        if (editMethod) {
            columns.push({
                width: '100px',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<a/>')
                        .addClass('editthing')
                        .addClass('actionbuttons')
                        .addClass('editbutton')
                        .attr('title', 'Edit')
                        .click(function (e) {
                            e.preventDefault();

                            editMethod(options.data.Id);
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
                        .addClass('actionbuttons')
                        .addClass('deletebutton')
                        .attr('title', 'Delete')
                        .click(function (e) {
                            e.preventDefault();

                            deleteMethod(options.data.Id);
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
            dataSource: {
                store: {
                    data: actualData,
                    type: 'array',
                    key: 'ID'
                }
            },
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
            allowColumnResizing: true,
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

        if (oncomplete) {
            oncomplete(data);
        }

    }

    newModalLink = function (container, route, prefix, modalClass, modalWidth, refreshGrid) {

        var modalLinkClass = prefix + 'newmodallink';

        $('.' + modalLinkClass).remove();

        var link = $('<a>')
            .attr('href', '#')
            .addClass('newmodallink')
            .addClass(modalLinkClass)
            .text('New Item')
            .click(function (e) {
                e.preventDefault();

                newEntityModal(route, prefix, modalClass, modalWidth, refreshGrid);

            });
        $(container).prepend($(link));

    }

    newEntityModal = function (route, prefix, modalClass, modalWidth, refreshGrid) {

        modal = $(modalClass).dialog({
            closeOnEscape: false,
            modal: true,
            width: modalWidth,
            resizable: false,
            beforeClose: function (event, ui) {
                if (previousEntity) {
                    currentEntity = previousEntity;
                }
            }
        });

        if (currentEntity && currentEntity.Id) {
            $(modal).find('.parentid').val(currentEntity.Id);
        }

        $(modal).find('select').each(function () {

            var classes = $(this).attr('class').split(' ');
            if (classes.length > 1) {
                var route = classes[1];

                PopulateDropDown($(this), route, '', '', null, null);
            }
        });

        $(modal).find('.cancelmodal').click(function (e) {
            e.preventDefault();
            CloseModal(modal);
        });

        $(modal).find('.savebutton').unbind('click');

        $(modal).find('.savebutton').click(function () {
            if (!ValidateFields(modal, function () {

                previousEntity = currentEntity;
                currentEntity = null;

                var item = getModalFieldsToSave(prefix);

                MakeServiceCall('POST', route, item, function () {
                    DisplaySuccessMessage("Success", "Save successful.");

                    CloseModal(modal);

                    if (refreshGrid) {
                        refreshGrid();
                    }
                }, null);
            })) {
                return;
            }
        });

    }

    editEntity = function (route, prefix, id, modalClass, modalWidth, refreshGrid) {

        modal = $(modalClass).dialog({
            closeOnEscape: false,
            modal: true,
            width: modalWidth,
            resizable: false,
            beforeClose: function (event, ui) {
                currentEntity = previousEntity;
            }
        });

        loadEntity(route, id, prefix);

        $(modal).find('.cancelmodal').click(function (e) {

            e.preventDefault();

            CloseModal(modal);

        });

        $(modal).find('.savebutton').unbind('click');

        $(modal).find('.savebutton').click(function () {

            if (ValidateForm($(modal).attr('class').split(" ")[0]) == false) {
                return;
            }

            var item = getModalFieldsToSave(prefix);

            MakeServiceCall('PATCH', route + '/' + id, item, function () {
                DisplaySuccessMessage("Save successful.");

                CloseModal(modal);

                if (refreshGrid) {
                    refreshGrid();
                }
            }, null);

        });

    }

    deleteEntity = function (route, id, refreshGrid) {

        MakeServiceCall('DELETE', route + '/' + id, null, function () {

            DisplaySuccessMessage('Success', 'Delete successful.');

            if (refreshGrid) {
                refreshGrid();
            }
        }, null);

    }

    getModalFieldsToSave = function (prefix) {

        var p = [];

        $(modal).find('input').not('input[type="button"]').each(function () {

            var property = $(this).attr('class').split(' ');

            if (property[0].indexOf(prefix) >= 0) {
                var propertyName = property[0].replace(prefix, '');
                var value = '';

                if ($(this).is(':checkbox')) {
                    value = $(this).prop('checked');
                }
                else {
                    value = $(this).val();
                }

                if (currentEntity) { // null if new
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
                else {
                    if (propertyName != 'id') {
                        p.push('"' + propertyName + '": "' + value + '"');
                    }
                }

            }

        });

        $(modal).find('select').each(function () {

            var property = $(this).attr('class').split(' ');

            if (property[0].indexOf(prefix) >= 0) {

                var propertyName = property[0].replace(prefix, '');
                var value = $(this).val();

                if (currentEntity) { // null if new
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
                else {
                    p.push('"' + propertyName + '": "' + value + '"');
                }


            }
        });

        p = '{' + p + '}';

        return p;

    }

    loadEntity = function (route, id, prefix) {

        if ($.type(prefix) === "string" && prefix.indexOf('.') != 0) {
            prefix = '.' + prefix;
        }

        MakeServiceCall('GET', route + '/' + id, null, function (data) {

            previousEntity = currentEntity;
            currentEntity = data.Data;

            for (var property in data.Data) {

                if ($(prefix + property).is('select')) {

                    var classes = $(prefix + property).attr('class').split(' ');

                    if (classes.length > 1) {
                        var route = classes[1];

                        PopulateDropDown($(prefix + property), route, '', '', data.Data[property], null);
                    }
                    else {
                        $(prefix + property).val(data.Data[property]);
                    }

                }
                else if ($(prefix + property).is('input[type="checkbox"]')) {
                    $(prefix + property).prop('checked', data.Data[property]);
                }
                else {
                    $(prefix + property).val(data.Data[property]);
                }


            }

        }, null);
    }

    return {
        LoadGrid: loadGrid,
        CustomLoadGrid: customLoadGrid
    };
}();