
$(document).ready(function () {

    $('.constituentlookup').autocomplete({
        source: function (request, response) {

            MakeServiceCall('GET', 'constituents/lookup/' + request.term, null, function (result) {
                var results = $.ui.autocomplete.filter($.map(result.Data, function (item) {
                    return {
                        label: item.ConstituentNumber + ": " + item.Name + ", " + item.PrimaryAddress,
                        value: item.Id
                    }
                }), request.term[0]);

                response(results);               
            }, null);

        },
        select: function (event, ui) {
            event.preventDefault();
            SelectConstituentLookup(ui.item);
        }
    });

});

function SelectConstituentLookup(item) {

    $('.rs-Constituent1Information').val(item.label);
    $('.rs-Constituent1Id').val(item.value);

}