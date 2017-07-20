
var route = 'customfields/';
var CustomFieldType = { 'Number': 0, 'TextBox': 1, 'TextArea': 2, 'DropDown': 3, 'Radio': 4, 'CheckBox': 5, 'Date': 6, 'DateTime': 7 };
var CustomFieldEntity = {
    'Accounting': 0, 'GeneralLedger': 1, 'AccountsPayable': 2, 'AccountsReceivable': 3, 'FixedAssets': 4,
    'Inventory': 5, 'CashProcessing': 6, 'CashDisbursements': 7, 'CashReceipting': 8, 'Gifts': 9,
    'NamedFunds': 10, 'CropEvents': 11, 'PlannedGiving': 12, 'Campaigns': 13, 'Investments': 14,
    'LineOfCredit': 15, 'Loans': 16, 'Portfolio': 17, 'Pools': 18, 'CRM': 19,
    'OfficeIntegration': 20, 'ProcessManagement': 21, 'ProjectManagement': 22, 'JobProcessing': 23, 'HealthPolicy': 24, 'SystemAdministration': 25
};
var currentCustomFieldEntity = 0;
var currentCustomFields = [];

$(document).ready(function () {



});

function DisplayCustomFieldsGrid(container, entity) {

    currentCustomFieldEntity = entity;

    if ($.type(container) === "string" && container.indexOf('.') != 0)
        container = '.' + container;

    var datagrid = $('<div>').addClass('customfieldgrid');

    var columns = [
        { dataField: 'FieldType', caption: 'Field Type' },
        { dataField: 'LabelText', caption: 'Label Text' },
        { dataField: 'MinValue', caption: 'Min Value' },
        { dataField: 'MaxValue', caption: 'Max Value' },
        { dataField: 'DisplayOrder', caption: 'Display Order' }
    ]

    MakeServiceCall('GET', route + 'entity/' + entity, null, function (data) {

        if (data.Data) {
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
                    EditCustomControl(info.data.Id);
                }
            });

            $(datagrid).appendTo($(container));

        }

    }, null);

}

function EditCustomControl(id) {



}

function DisplayCustomFields(container, entity, callback) {

    if ($.type(container) === "string" && container.indexOf('.') != 0)
        container = '.' + container;
    // url: WEB_API_ADDRESS + route + 'entity/' + entity + '/constituent/' + currentEntity.Id
    MakeServiceCall('GET', 'customfields/entity/' + entity + '/constituent/' + currentEntity.Id, null, function (data) {

        if (data && data.IsSuccessful) {

            $.map(data.Data, function (item) {

                currentCustomFields[item.Id] = item;
                $(container).append(CreateCustomField(item));

            });

            if (callback) {
                callback();
            }
        }

    }, null);

}

function CreateCustomField(item) {

    var div = $('<div>').attr('id', item.Id).addClass('fieldblock customField');

    $('<label>').text(item.LabelText).appendTo($(div));

    switch (item.FieldType) {
        case CustomFieldType.Number:
            $(div).append(CreateNumberField(item));
            break;
        case CustomFieldType.TextBox:
            $(div).append(CreateTextField(item));
            break;
        case CustomFieldType.TextArea:
            $(div).append(CreateTextAreaField(item));
            break;
        case CustomFieldType.DropDown:
            $(div).append(CreateDropDownField(item));
            break;
        case CustomFieldType.Radio:
            $(div).append(CreateRadioField(item));
            break;
        case CustomFieldType.CheckBox:
            $(div).append(CreateCheckBoxField(item));
            break;
        case CustomFieldType.Date:
            $(div).append(CreateDateField(item));
            break;
        case CustomFieldType.DateTime:
            $(div).append(CreateDateTimeField(item));
            break;
    }

    return div;

}

function CreateNumberField(item) {

    var number = $('<input>').addClass('number editable');

    if (item.Data[0]) {
        $(number).val(item.Data[0].Value);
    }

    return number;

}

function CreateTextField(item) {

    var text = $('<input>').attr('type', 'text').addClass('editable');

    if (item.Data[0]) {
        $(text).val(item.Data[0].Value);
    }

    return text;

}

function CreateTextAreaField(item) {

    var textarea = $('<textarea>').addClass('editable');

    if (item.Data[0]) {
        $(textarea).val(item.Data[0].Value);
    }

    return textarea;

}

function CreateDropDownField(item) {

    var dropdown = $('<select>').addClass('editable');

    if (item.Options) {
        AddDefaultOption(dropdown, '', '');

        $.map(item.Options, function (o) {
            $('<option>').val(o.Id).text(o.DisplayName).appendTo($(dropdown));
        });
    }

    if (item.Data[0]) {
        $(dropdown).val(item.Data[0].Value);
    }

    return dropdown;

}

function CreateRadioField(item) {

    var radio = $('<div>').addClass('radiobuttons editable');

    if (item.Options) {
        $.map(item.Options, function (o) {
            var rd = $('<div>').addClass('radiobutton');

            var i = $('<input>').attr('type', 'radio').attr('name', item.Id).val(o.Id).appendTo($(rd));
            $('<label>').addClass('inline').text(o.DisplayName).appendTo($(rd));

            if (item.Data[0] && item.Data[0].Value == $(i).val()) {
                $(i).attr('checked', 'checked');
            }

            $(rd).appendTo($(radio));
        });
    }

    return radio;

}

function CreateCheckBoxField(item) {

    var checkbox = $('<input>').attr('type', 'checkbox').addClass('editable');

    if (item.Data[0] && item.Data[0].Value == 'true') {
        $(checkbox).attr('checked', 'checked');
    }

    return checkbox;

}

function CreateDateField(item) {

    var date = $('<input>').attr('type', 'text').addClass('datepicker editable');

    if (item.Data[0]) {
        $(date).val(item.Data[0].Value);
    }

    return date;

}

function CreateDateTimeField(item) {

    var dt = $('<div>').addClass('datepair editable');
    var date = $('<input>').attr('type', 'text').addClass('date').appendTo($(dt));
    var time = $('<input>').attr('type', 'text').addClass('time').appendTo($(dt));

    // Need to verify that the data is coming back appropriatly, then how to format it
    // in a way that it goes into the text box....

    if (item.Data[0] && item.Data[0].Value) {

        var data = item.Data[0].Value.split(' ');

        if (data[0]) {

            $(date).val(data[0]);

            if (data[1]) {
                $(time).val(data[1]);
            }
        }
    }

    return dt;

}

function SaveCustomFields(container) {

    $(container).find('div.fieldblock').each(function () {

        var customFieldId = $(this).attr('id');
        var cf = currentCustomFields[customFieldId];
        var data = null;
        var cfDataRoute = 'customfielddata';
        var method = '';

        var value = GetCustomFieldValue(this, cf);

        if (cf.Data && cf.Data[0] && cf.Data[0].Id) {
            // Update
            if (value != cf.Data[0].Value)
            {
                data = {
                    Value: value
                }
                method = 'PATCH';
                cfDataRoute = cfDataRoute + '/' + cf.Data[0].Id;
            }
        }
        else {
            if (value) {
                // Insert
                data = {
                    CustomFieldId: customFieldId,
                    EntityType: cf.Entity,
                    ParentEntityId: currentEntity.Id,
                    Value: value
                }
                method = 'POST';
            }
        }

        if (data) {
            $.ajax({
                url: WEB_API_ADDRESS + cfDataRoute,
                method: method,
                data: data,
                headers: GetApiHeaders(),
                contentType: 'application/json; charset-utf-8',
                crossDomain: true,
                success: function () {

                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', 'An error occurred during saving a custom field.');
                }
            });
        }

    });

}

function GetCustomFieldValue(container, customField) {

    var value = '';

    switch (customField.FieldType) {
        case CustomFieldType.Number:
            value = $(container).find('input').val();
            break;
        case CustomFieldType.TextBox:
            value = $(container).find('input').val();
            break;
        case CustomFieldType.TextArea:
            value = $(container).find('input').text();
            break;
        case CustomFieldType.DropDown:
            value = $(container).find('select').val();
            break;
        case CustomFieldType.Radio:
            value = $('input[name=' + customField.Id + ']:checked').val();
            break;
        case CustomFieldType.CheckBox:
            value = $(container).find('input').prop('checked');
            break;
        case CustomFieldType.Date:
            value = $(container).find('input').val();
            break;
        case CustomFieldType.DateTime:
            var d = $(container).find('input.date').val();
            var t = $(container).find('input.time').val();
            value = d + ' ' + t;
            break;
    }

return value;

}

