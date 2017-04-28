$(document).ready(function () {

    //$('#activity-and-budget-tab').click(function () {

        LoadAccountActivityAndBudgetTab('05523233-784D-4D6E-920B-0019EFAF9912');

    //});

});


function LoadAccountActivityAndBudgetTab(id) {
    

    var columns = [

        // going to need...
        // bring in the accountsummary
        // get the details from that - extra route or just get it off the details\
        // the column captions will come from that - I think

   // { dataField: 'Id'},
    { dataField: 'Detail.PeriodName', caption: 'Period' },
    { dataField: 'Detail.WorkingBudget', caption: 'Working Budget', dataType: 'date' },
    { dataField: 'Detail.Debits', caption: 'Prior Year Name' },
    { dataField: 'Detail.PriorCredits', caption: 'Prior credits', allowEditing: true },
    { dataField: 'Detail.Credits', caption: 'Credits', allowEditing: true },
    { dataField: 'Detail.WhatIfBudget', caption: 'What If Budget Name', allowEditing: true },
    {
        summary: {
            totalItems: [{
                column: 'Debits',
                summaryType: 'sum',
                alignment: 'left'
            }, {
                column: 'Credits',
                summaryType: 'sum',
                alignment: 'left',
            }]
        }
    }

    ];

    //function CustomLoadGrid(grid, container, columns, route, selected, editMethod, deleteMethod, oncomplete) {

    //CustomLoadGrid('activitygrid', '.activitygridcontainer', columns, 'accounts/activity/' + id, null, null, null, function (data) {

    //    $('.activitytitle').empty();

    //    $('<label>').text(data.Data.AccountName).appendTo($('.activitytitle'));
        
    //}); 
    //function LoadGrid(container, gridClass, columns, getRoute, saveRoute, selected, prefix, editModalClass, newModalClass, modalWidth, showDelete, showFilter, showGroup, onComplete) {

    LoadGrid('.activitygridcontainer', 'activitygrid', columns, 'accounts/activity/' + id, null, null, null, null, null, null, false, false, false, function (data) {

            $('.activitytitle').empty();

            $('<label>').text(data.Data.AccountName).appendTo($('.activitytitle'));
        
    });

    var summaryoptions =
               {
                   totalItems: [{
                       column: 'Debits',
                       summaryType: 'sum',
                       alignment: 'right'
                   }, {
                       column: 'Credits',
                       summaryType: 'sum',
                       alignment: 'left',
                       showInColumn: 'OrderDate',
                   }]
               }

    $('.activitygridcontainer').dxDataGrid('instance').option(summary, summaryoptions);
}