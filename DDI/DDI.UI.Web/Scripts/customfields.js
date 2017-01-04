
var route = 'customfields/';
var customfieldtype = { 'Number': 0, 'TextBox': 1, 'TextArea': 2, 'DropDown': 3, 'Radio': 4, 'CheckBox': 5, 'Date': 6, 'DateTime': 7 };
var customfieldentity = {
    'Accounting': 0, 'GeneralLedger': 1, 'AccountsPayable': 2, 'AccountsReceivable': 3, 'FixedAssets': 4,
    'Inventory': 5, 'CashProcessing': 6, 'CashDisbursements': 7, 'CashReceipting': 8, 'Gifts': 9,
    'NamedFunds': 10, 'CropEvents': 11, 'PlannedGiving': 12, 'Campaigns': 13, 'Investments': 14,
    'LineOfCredit': 15, 'Loans': 16, 'Portfolio': 17, 'Pools': 18, 'CRM': 19,
    'OfficeIntegration': 20, 'ProcessManagement': 21, 'ProjectManagement': 22, 'JobProcessing': 23, 'HealthPolicy': 24, 'SystemAdministration': 25 };

$(document).ready(function () {



});

function CustomFieldsGrid(container, entity) {

    if (container.indexOf('.') != 0)
        container = '.' + container;

    var datagrid = $('<div>').addClass('customfieldgrid');

    var columns = [
        { dataField: 'FieldType', caption: 'Field Type' },
        { dataField: 'LabelText', caption: 'Label Text' },
        { dataField: 'MinValue', caption: 'Min Value' },
        { dataField: 'MaxValue', caption: 'Max Value' },
        { dataField: 'DisplayOrder', caption: 'Display Order' }
    ]

    $.ajax({
        url: WEB_API_ADDRESS + route,
        data: '{ entity: + ' + entity + ' }',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $(datagrid).dxDataGrid({
                dataSource: data.Data,
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
                    visible: true,
                    allowColumnDragging: true
                },
                filterRow: {
                    visible: true,
                    showOperationChooser: false
                }
            });

            $(datagrid).appendTo($(container));

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error loading Custom Fields.');
        }
    });

}

function DisplayCustFields(container, entity) {

    if (container.indexOf('.') != 0)
        container = '.' + container;

    $.ajax({
        url: WEB_API_ADDRESS + route,
        data: '{ entity: + ' + entity + ' }',
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            if (data.IsSuccessful) {

                $.map(data.Data, function (item) {

                    $(container).append(CreateCustomField(item));

                });

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error loading Custom Fields.');
        }
    });

}

function CreateCustomField(item) {

    var div = $('<div>').addClass('fieldblock');

    $('<label>').text(item.LabelText).appendTo($(div));

    switch (item.FieldType) {
        case customfieldtype.Number:

            break;
        case customfieldtype.TextBox:

            break;
        case customfieldtype.TextArea:

            break;
        case customfieldtype.DropDown:

            break;
        case customfieldtype.Radio:

            break;
        case customfieldtype.CheckBox:

            break;
        case customfieldtype.Date:

            break;
        case customfieldtype.DateTime:

            break;
    }


    return div;

}

