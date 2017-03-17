
$(document).ready(function () {

    $('.constituentlookup').autocomplete({
        source: function (request, response) {

            MakeServiceCall('GET', 'constituents/lookup/' + request.term, function (result) {
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

                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
                }
            }, null);

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