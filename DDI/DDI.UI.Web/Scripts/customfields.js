
var route = 'customfields/';
var CustomFieldType = { 'Number': 0, 'TextBox': 1, 'TextArea': 2, 'DropDown': 3, 'Radio': 4, 'CheckBox': 5, 'Date': 6, 'DateTime': 7 };
var CustomFieldEntity = {
    'Accounting': 0, 'GeneralLedger': 1, 'AccountsPayable': 2, 'AccountsReceivable': 3, 'FixedAssets': 4,
    'Inventory': 5, 'CashProcessing': 6, 'CashDisbursements': 7, 'CashReceipting': 8, 'Gifts': 9,
    'NamedFunds': 10, 'CropEvents': 11, 'PlannedGiving': 12, 'Campaigns': 13, 'Investments': 14,
    'LineOfCredit': 15, 'Loans': 16, 'Portfolio': 17, 'Pools': 18, 'CRM': 19,
    'OfficeIntegration': 20, 'ProcessManagement': 21, 'ProjectManagement': 22, 'JobProcessing': 23, 'HealthPolicy': 24, 'SystemAdministration': 25 };
var currentCustomFieldEntity = 0;

$(document).ready(function () {



});

function DisplayCustomFieldsGrid(container, entity) {

    currentCustomFieldEntity = entity;

    if (container.indexOf('.') != 0)
        container = '.' + container;

    var datagrid = $('<div>').addClass('customfieldgrid');

    var columns = [
        { dataField: 'Id', width: "0px" },
        { dataField: 'FieldType', caption: 'Field Type' },
        { dataField: 'LabelText', caption: 'Label Text' },
        { dataField: 'MinValue', caption: 'Min Value' },
        { dataField: 'MaxValue', caption: 'Max Value' },
        { dataField: 'DisplayOrder', caption: 'Display Order' }
    ]

    $.ajax({
        url: WEB_API_ADDRESS + route + 'entity/' + entity,
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
                },
                onRowClick: function (info) {
                    EditCustomControl(info.values[0]);
                }
            });

            $(datagrid).appendTo($(container));

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error loading Custom Fields.');
        }
    });

}

function EditCustomControl(id) {



}

function DisplayCustomFields(container, entity) {

    if (container.indexOf('.') != 0)
        container = '.' + container;

    $.ajax({
        url: WEB_API_ADDRESS + route + 'entity/' + entity,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            if (data && data.IsSuccessful) {

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
        case CustomFieldType.Number:
            CreateNumberField(item);
            break;
        case CustomFieldType.TextBox:
            CreateTextField(item);
            break;
        case CustomFieldType.TextArea:
            CreateTextAreaField(item);
            break;
        case CustomFieldType.DropDown:
            CreateDropDownField(item);
            break;
        case CustomFieldType.Radio:
            CreateRadioField(item);
            break;
        case CustomFieldType.CheckBox:
            CreateCheckBoxField(item);
            break;
        case CustomFieldType.Date:
            CreateDateField(item);
            break;
        case CustomFieldType.DateTime:
            CreateDateTimeField(item);
            break;
    }


    return div;

}

function CreateNumberField(item) {

    var number = $('<input>').addClass('number');

    if (item.Answer) {
        $(number).val(item.Answer.Value);
    }

    return number;

}

function CreateTextField(item) {

    var text = $('<input>').attr('type', 'text');

    if (item.Answer) {
        $(text).val(item.Answer.Value);
    }

    return text;

}

function CreateTextAreaField(item) {

    var textarea = $('<textarea>');

    if (item.Answer) {
        $(textarea).val(item.Answer.Value);
    }

    return textarea;

}

function CreateDropDownField(item) {

    var dropdown = $('<select>');
    
    if (item.Options) {
        $.map(item.Options, function (o) {
            $('<option>').val(o.Id).text(o.DisplayName).appendTo($(dropdown));
        });
    }

    if (item.Answer) {
        $(dropdown).val(item.Answer.Value);
    }

    return dropdown;

}

function CreateRadioField(item) {

    var radio = $('<div>').addClass('radiobuttons');

    if (item.Options) {
        $.map(item.Options, function (o) {
            var rd = $('<div>').addClass('radiobutton');

            $('<label>').addClass('inline').text(o.DisplayName).appendTo($(rd));
            var i = $('<input>').attr('type', 'radio').attr('name', item.Id).val(o.Id).appendTo($(rd));

            if (item.Answer && item.Answer.Value == $(i).val()) {
                $(i).attr('checked', 'checked');
            }

            $(rd).appendTo($(radio));
        });
    }

    return radio;

}

function CreateCheckBoxField(item) {

    var checkbox = $('<input>').attr('type', 'checkbox');

    if (item.Answer && item.Answer.Value == '1') {
        $(checkbox).attr('checked', 'checked');
    }

    return checkbox;

}

function CreateDateField(item) {

    var date = $('<input>').attr('type', 'text').addClass('datepicker');

    if (item.Answer) {
        $(date).text(FormatJSONDate(item.Answer.Value));
    }

    return date;

}

function CreateDateTimeField(item) {

    var dt = $('<div>').addClass('datepair');
    var date = $('<input>').attr('type', 'text').addClass('date').appendTo($(dt));
    var time = $('<input>').attr('type', 'text').addClass('time').appendTo($(dt));

    if (item.Answer) {
        $(date).text(FormatJSONDate(item.Answer.Value));
        $(time).text(FormatJOSNTime(item.Answer.Value));
    }

    return dt;

}

