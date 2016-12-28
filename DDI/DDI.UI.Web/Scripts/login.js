$(document).ready(function () {

    $('#form1').validate({
        submitHandler: function (form) {
            location.href = "Default.aspx";
        },
        rules: {
            email: {
                required: true,
                email: true
            },
            password: {
                required: true
            }
        }
    });

    $('.forgotpasswordlink').click(function (e) {

        e.preventDefault();

        $('.forgotpasswordmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 400,
            height: 150,
            resizable: false
        });

    });

});

