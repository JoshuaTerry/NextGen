
$(document).ready(function () {

    $('.constituentlookup').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: WEB_API_ADDRESS + 'constituents/lookup/' + request.term,
                type: 'GET',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                crossDomain: true,
                success: function (result) {

                    if (result.Data.length == 1) {
                        var item = {
                            label: result.Data[0].Name,
                            value: result.Data[0].Id
                        };
                        SelectConstituentLookup(item);
                    }
                    else {

                        var results = $.ui.autocomplete.filter($.map(result.Data, function (item) {
                            return {
                                label: item.Name,
                                value: item.Id
                            }
                        }), request.term);

                        response(results);

                    }
                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
                }
            });
        },
        select: function (event, ui) {
            event.preventDefault();
            SelectConstituentLookup(ui.item);
        }
    });

});

function SelectConstituentLookup(item) {

    $('.FormattedName2').val(item.label);
    $('.hidconstituentlookupid').val(item.value);

}