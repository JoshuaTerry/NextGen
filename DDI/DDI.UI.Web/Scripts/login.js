$(document).ready(function () {

    $('#form1').validate({
        submitHandler: function (form) {
            Login();
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

function Login() {

    var loginData = {
        grant_type: 'password',
        username: $('.username').val(),
        password: $('.password').val()
    }

    $.ajax({
        type: 'POST',
        url: WEB_API_ADDRESS + 'login',
        data: loginData,
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            sessionStorage.setItem(AUTH_TOKEN_KEY, data.access_token);

            location.href = "/Default.aspx";

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during logging in.');
        }
    })

    

}

