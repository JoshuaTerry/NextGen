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

    $('.newuser').click(function (e) {

        e.preventDefault();

        $('.newusermodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 400,
            height: 225,
            resizable: false
        });

        $('.submitnewuser').click(function () {

            var model = {
                Email: $('.newusername').val(),
                Password: $('.newpassword').val(),
                ConfirmPassword: $('.newconfirmpassword').val()
            }

            $.ajax({
                type: 'POST',
                url: WEB_API_ADDRESS + 'Register',
                data: model,
                contentType: 'application/x-www-form-urlencoded',
                crossDomain: true,
                success: function () {

                    location.href = "/Login.aspx";

                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', 'An error occurred during registration.');
                }
            });

        });

    });

});

function Login() {

    var loginData = 'grant_type=password&username=' + $('.username').val() + '&password=' + $('.password').val();

    $.ajax({
        type: 'POST',
        url: WEB_API_ADDRESS + 'Login',
        data: loginData,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            sessionStorage.setItem(AUTH_TOKEN_KEY, data.access_token);

            location.href = "/Default.aspx";

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during logging in.');
        }
    });

}

