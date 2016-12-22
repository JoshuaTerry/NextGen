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

});

