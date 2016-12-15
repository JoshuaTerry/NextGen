var WEB_API_ADDRESS = 'http://192.168.10.107:8080/api/v1/';   // DEV
// var WEB_API_ADDRESS = 'http://devapi.ddi.org';   // TEST
// var WEB_API_ADDRESS = '';   // PROD

$(document).ready(function () {

    $.support.cors = true;

    LoadDatePickers();

    LoadTabs();

    LoadAccordions();

});

function LoadTabs() {

    $('.tabscontainer').tabs();

}

function LoadAccordions() {

    $('.accordions').accordion({
        heightStyle: "content",
        collapsible: true
    });

}

function LoadDatePickers() {

    $('.datepicker').datepicker();

}

function FormatJSONDate(jsonDate) {

    var date = '';

    if (jsonDate) {
        date = new Date(parseInt(jsonDate.substr(6)));
        date.getMonth() + 1 + '/' + date.getDate() + '/' + date.getFullYear();
    }
    
    return date;
}