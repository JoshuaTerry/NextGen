
var count = 15;
var counter = null;

$(document).ready(function () {

    var params = GetQueryString();

    if (params['email'] && params['code']) {
        
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
