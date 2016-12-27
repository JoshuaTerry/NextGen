

function ClearElement(e) {

    $(e).html('');

}

function AddDefaultOption(e, text, val) {

    var option = $('<option>').val('null').text('');
    $(option).appendTo($(e));

}

function MakeServiceCall(e, method) {

    $.ajax({
        url: WEB_API_ADDRESS + method,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $.map(data.Data, function (item) {

                option = $('<option>').val(item.Id).text(item.DisplayName); //item.Name || item.Description
                $(option).appendTo($(e));

            });

        },
        failure: function (response) {
            alert(response);
        }
    });

}

function PopulateDropDown(e, method, defaultText, defaultValue) {

    ClearElement(e);
    AddDefaultOption(e, defaultText, defaultValue);

    MakeServiceCall(e, method);

}

function PopulateDropDown(e, method, defaultText, defaultValue, callback) {

    ClearElement(e);
    AddDefaultOption(e, defaultText, defaultValue);

    MakeServiceCall(e, method);

    if (callback) {

        $(e).unbind('change');

        $(e).change(function () {
            callback();
        });

    }

}


