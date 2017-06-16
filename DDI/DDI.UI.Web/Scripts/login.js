$(document).ready(function () {

    $('#form1').validate({
        submitHandler: function (form) {
            Login();
        },
        rules: {
            email: {
                required: true
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
                headers: GetApiHeaders(),
                contentType: 'application/x-www-form-urlencoded',
                crossDomain: true,
                success: function () {

                    location.href = "/Login.aspx";

                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
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
        headers: GetApiHeaders(),
        crossDomain: true,
        success: function (data) {

            sessionStorage.setItem(AUTH_TOKEN_KEY, data.access_token);

            MakeServiceCall('GET', 'userbyname/' + data.userName + '/', null, function (data) {

                sessionStorage.setItem('CURRENT_USER_ID', data.Data.Id);

                LoadCurrentUser();

            }, null);
            // set user id

            var user = {};
            user.username = $('.username').val();
            user.token = data.access_token;

            $.ajax({
                type: 'POST',
                url: 'Login.aspx/AuthorizeUser',
                data: JSON.stringify(user),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function () {
                    location.href = "/Default.aspx";
                },
                error: function (error) {
                    var err = error;
                }
            });

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}

