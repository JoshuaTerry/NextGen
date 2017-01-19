
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

                option = $('<option>').val(item.Id).text(item.DisplayName); //item.Name || item.Description
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

function PopulateDropDown(e, method, defaultText, defaultValue, selectedValue) {

    ClearElement(e);
    AddDefaultOption(e, defaultText, defaultValue);

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


