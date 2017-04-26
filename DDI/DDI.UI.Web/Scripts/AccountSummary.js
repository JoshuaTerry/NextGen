$(document).ready(function () {


});


function LoadAccountSummaryTab() {

    var columns = [
    { dataField: 'Id', width: '0px', },
    { dataField: 'StartDate', caption: 'From', dataType: 'date' },
    { dataField: 'EndDate', caption: 'To', dataType: 'date' },
    { dataField: 'Name', caption: 'Name' }
    ];


    LoadGrid(); // May not be able to use LoadGrid function

}