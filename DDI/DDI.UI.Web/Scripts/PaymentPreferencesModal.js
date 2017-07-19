function NewPaymentPreferencesModal(linkbutton) {
    if (linkbutton.indexOf(".") === -1) {
        linkbutton = '.' + linkbutton;
    }

    $(linkbutton).click(function (e) {

        e.preventDefault();

        paymentPreferencesModal = $('.paymentpreferencesmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 500,
            resizable: false
        });

        $('.cancelpaymentpreferencesmodal').click(function (e) {

            e.preventDefault();

            CloseModal(paymentPreferencesModal);

        });

        $('.savepaymentpreferencesbutton').unbind('click');

        $('.savepaymentpreferencesbutton').click(function () {

            var item = GetpaymentpreferencesToSave();

            MakeServiceCall('POST', 'paymentmethods', item, function (data) {

                DisplaySuccessMessage('Success', 'Payment Preference saved successfully.');

                CloseModal(paymentPreferencesModal);

                LoadpaymentpreferencesGrid();

            }, function (xhr, status, err) {

                DisplayErrorMessage('Error', 'An error occurred during saving the Payment Preference.');

            });

        });

    });
}

function Editpaymentpreferences(id) {

    var paymentPreferencesModal = $('.paymentpreferencesmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 500,
        resizable: false
    });

    Loadpaymentpreferences(id);


    $('.cancelpaymentpreferencesmodal').click(function (e) {

        e.preventDefault();

        CloseModal(paymentPreferencesModal);

    });

    $('.savepaymentpreferencesbutton').unbind('click');

    $('.savepaymentpreferencesbutton').click(function () {

        var topicsavelist = GetNoteTopicsToSave();

        var item = GetpaymentpreferencesToSave();

        MakeServiceCall('PATCH', 'paymentmethods/' + id, item, function (data) {

            DisplaySuccessMessage('Success', 'Payment Preference saved successfully.');

            CloseModal(paymentPreferencesModal);

            LoadpaymentpreferencesGrid();

        }, function (xhr, status, err) {

            DisplayErrorMessage('Error', 'An error occurred during saving the Payment Preference.');
        });

    });
}

function Loadpaymentpreferences(id) {

    MakeServiceCall('GET', 'paymentmethods/' + id, null, function (data) {

        $('.Description').val(data.Data.Description),
        $('.BankName').val(data.Data.BankName),
        $('.RoutingNumber').val(data.Data.RoutingNumber),
        $('.BankAccount').val(data.Data.BankAccount),
        $('.AccountType').val(data.Data.AccountType),
        $('.EFTFormatId').val(data.Data.EFTFormatId),
        $('.Status').val(data.Data.Status)


    }, function (xhr, status, err) {
        DisplayErrorMessage('Error', 'An error occurred during loading the Payment Preference.');
    });

}


function GetpaymentpreferencesToSave() {

    var item = {

        Description: $(paymentPreferencesModal).find('.Description').val(),
        BankName: $(paymentPreferencesModal).find('.BankName').val(),
        RoutingNumber: $(paymentPreferencesModal).find('.RoutingNumber').val(),
        BankAccount: $(paymentPreferencesModal).find('.BankAccount').val(),
        AccountType: $(paymentPreferencesModal).find('.AccountType').val(),
        EFTFormatId: $(paymentPreferencesModal).find('.EFTFormatId').val(),
        Status: $(paymentPreferencesModal).find('.Status').val

    };

    return item;

}

