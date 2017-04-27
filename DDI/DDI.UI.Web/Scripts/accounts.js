$(document).ready(function () {

    $('#summarytab').click(function () {

        LoadAccountSummaryTab('05523233-784D-4D6E-920B-0019EFAF9912');

    });

});


function LoadAccountSummaryTab(id) {
    

    var columns = [

    { dataField: 'Id', width: '0px', },
    { dataField: 'Detail.PeriodName', caption: 'Period' },
    { dataField: 'Detail.WorkingBudget', caption: 'Working Budget', dataType: 'date' },
    { dataField: 'Detail.Debits', caption: 'Prior Year Name' },
    { dataField: 'Detail.PriorCredits', caption: 'Prior credits', allowEditing: true },
    { dataField: 'Detail.Credits', caption: 'Credits', allowEditing: true },
    { dataField: 'Detail.WhatIfBudget', caption: 'What If Budget Name', allowEditing: true }

    ];

    //function CustomLoadGrid(grid, container, columns, route, selected, editMethod, deleteMethod, oncomplete) {

    CustomLoadGrid('summarygrid', '.summarygridcontainer', columns, 'accounts/activity/' + id, null, null, null, function (data) {

        $('.summarytitle').empty();

        $('<label>').text(data.Data.AccountName).appendTo($('.summarytitle'));
        
    }); 

}