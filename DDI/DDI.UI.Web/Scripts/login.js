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

            sessionStorage.setItem('CURRENT_USER_NAME', data.userName);
            // set userName

            LoadCurrentUser();
            
            var user = {
                username: $('.username').val(),
                token: data.access_token
            };

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

