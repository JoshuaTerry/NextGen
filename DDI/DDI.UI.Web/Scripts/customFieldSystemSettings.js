/* CUSTOM FIELDS */
var modalLeft = 0;
var options = [];

function LoadCRMClientCustomFieldsSectionSettings() {

    DisplayCustomFieldsGrid('gridcontainer', CustomFieldEntity.CRM); // CRM = 19

    CreateNewCustomFieldModalLink(CustomFieldEntity.CRM, 'New CRM Custom Field');
}

function LoadDonationClientCustomFieldsSectionSettings() {

    DisplayCustomFieldsGrid('gridcontainer', CustomFieldEntity.Gifts); // Gifts = 9

    CreateNewCustomFieldModalLink(CustomFieldEntity.Gifts, 'New Donations Custom Field');
}

function LoadGLClientCustomFieldsSectionSettings() {

    DisplayCustomFieldsGrid('gridcontainer', CustomFieldEntity.GeneralLedger); // GeneralLedger = 1

    CreateNewCustomFieldModalLink(CustomFieldEntity.GeneralLedger, 'New General Ledger Custom Field');
}

function RefreshCustomFieldsGrid() {

    $('.gridcontainer').html('');

    DisplayCustomFieldsGrid('gridcontainer', currentCustomFieldEntity);
}

function CreateNewCustomFieldModalLink(entity, title) {

    var modallink = $('<a>').attr('href', '#').addClass('customfieldmodallink').text('New Custom Field');
    $('.gridcontainer').before($(modallink));

    $(modallink).unbind('click');

    $(modallink).click(function (e) {

        e.preventDefault();

        CreateNewCustomFieldModal(entity, title);
    });
}

function CreateNewCustomFieldModal(entity, title) {

    var modal = $('.newcustomfieldmodal').dialog({
        closeOnEscape: false,
        modal: true,
        resizable: false,
        beforeClose: function (event, ui) {
            ClearModal(modal);
        }
    });

    $('.options').hide();

    modalLeft = parseInt($('.ui-dialog').css('left').replace('px', ''));

    var type = $(modal).find('.cftype');
    var save = $(modal).find('.submitcf');

    $('<option>').text('').val('').appendTo($(type));
    $.each(CustomFieldType, function (key, value) {
        $('<option>').text(key).val(value).appendTo($(type));
    });

    $('.addoption').click(function () {
        AddOption();
    });

    $(type).change(function () {

        CustomFieldTypeSelected($(this).val());
    });

    $(save).unbind('click');

    $(save).click(function () {
        SaveCustomField(modal);
    });

    $('.cancelmodal').click(function (e) {
        e.preventDefault();

        ClearModal(modal);

        $('.ui-dialog').css('width', '300px');
        $('.fieldproperties').attr('style', 'width: 100%');

        $(modal).dialog('close');
    });
}

function AddOption() {

    var code = $('.cfoptioncode').val();
    var desc = $('.cfoptiondesc').val();
    var order = $('.cfoptionorder').val();
    var option = {
        Code: code,
        Description: desc,
        SortOrder: order
    };

    options.push(option);

    var tr = $('<tr>');

    var tdcode = $('<td>').text(code).css('width', '28px').appendTo($(tr));
    var tddesc = $('<td>').text(desc).css('padding-left', '5px').css('width', '155px').appendTo($(tr));
    var tdorder = $('<td>').text(order).css('padding-left', '2px').css('width', '30px').appendTo($(tr));

    $(tr).appendTo($('.tempoptions'));

    $('.cfoptioncode').val('').focus();
    $('.cfoptiondesc').val('');
    $('.cfoptionorder').val('');
}

function ClearModal(modal) {

    $('.options').hide();
    options = [];
    $('.tempoptions').html('');

    $(modal).find('div.fieldblock input').not('.noclear').each(function () {
        $(this).val('');
    });

    $(modal).find('select').not('.noclear').each(function () {
        $(this).html('');
    });
}

function CustomFieldTypeSelected(selectedvalue) {

    if (selectedvalue) {
        if (selectedvalue == CustomFieldType.Number ||
            selectedvalue == CustomFieldType.Date ||
            selectedvalue == CustomFieldType.DateTime) {
            $('.minmaxvalues').show()
        }
        else {
            $('.minmaxvalues').hide()
        }

        if (selectedvalue && selectedvalue == CustomFieldType.Number) {
            $('.decimalplacecontainer').show();
        }
        else {
            $('.decimalplacecontainer').hide();
        }

        if (selectedvalue == CustomFieldType.Radio ||
            selectedvalue == CustomFieldType.DropDown) {

            var left = parseInt($('.ui-dialog').css('left').replace('px', ''));

            if (left >= modalLeft)
                left -= 150;

            $('.ui-dialog').stop().animate(
                {
                    width: '600px',
                    left: left
                },
                {
                    start: function () {
                        $('.fieldproperties').attr('style', '');
                    },
                    complete: function () {
                        $('.options').show();
                    }
                }
                , 500);


        }
        else {

            var left = parseInt($('.ui-dialog').css('left').replace('px', ''));

            if (left < modalLeft)
                left += 150;

            $('.ui-dialog').stop().animate(
                {
                    width: '300px',
                    left: left
                },
                {
                    start: function () {
                        $('.options').hide();
                        $('.fieldproperties').attr('style', 'width: 100%');
                    },
                    complete: function () {

                    }
                }
                , 500);
        }
    }
    else {

        $('.minmaxvalues').hide()
        $('.decimalplacecontainer').hide();
    }
}

function SaveCustomField(modal) {

    var id = $('.cfid').val();
    var method = '';

    if (id) {
        // Update
        var data = {
            Id: id,
            LabelText: $('.cflabel').val(),
            MinValue: $('.cfminvalue').val(),
            MaxValue: $('.cfmaxvalue').val(),
            DecimalPlaces: $('.cfdecimalplaces').val(),
            IsActive: true,
            IsRequired: $('.cfisrequired').prop('checked'),
            DisplayOrder: 1,
            FieldType: $('.cftype').val(),
            Entity: currentCustomFieldEntity,
            Options: []
        }

        method = 'PATCH';
    }
    else {
        // Insert

        var data = {
            LabelText: $('.cflabel').val(),
            MinValue: $('.cfminvalue').val(),
            MaxValue: $('.cfmaxvalue').val(),
            DecimalPlaces: $('.cfdecimalplaces').val(),
            IsActive: true,
            IsRequired: $('.cfisrequired').prop('checked'),
            DisplayOrder: 1,
            FieldType: $('.cftype').val(),
            Entity: currentCustomFieldEntity,
            Options: []
        }

        method = 'POST';
    }

    if (options && options.length > 0) {
        data.Options = options;
    }

    SendCustomField(method, 'customfields', data, modal);
}

function SendCustomField(method, route, data, modal) {

    MakeServiceCall(method, 'route', data, function (data) {

        if (data.Data) {
            DisplaySuccessMessage('Success', 'Custom field saved successfully.');

            CloseModal(modal);

            RefreshCustomFieldsGrid();

            CreateNewCustomFieldModalLink(currentCustomFieldEntity, '');
        }

    }, null);
}

/* END CUSTOM FIELDS */