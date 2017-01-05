
var count = 15;
var counter = null;

$(document).ready(function () {

    var params = GetQueryString();

    if (params['email'] && params['code']) {
        
        ConfirmEmail(params['email'], params['code']);

        counter = setInterval(CountDown, 1000);

    }
    else {

        location.href = '/Login.aspx';
    }

});

function CountDown() {

    count = count - 1;

    if (count <= 0) {
        clearInterval(counter);
        
        location.href = '/Login.aspx';
    }

    $('.timer').text(count);
}

function ConfirmEmail(email, code) {

    var model = {
        Email: email,
        Code: code
    }

    $.ajax({
        type: 'POST',
        url: WEB_API_ADDRESS + 'ConfirmEmail',
        data: model,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function () {

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during email confirmation.');
        }
    });

}
