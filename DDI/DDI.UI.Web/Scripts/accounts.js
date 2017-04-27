$(document).ready(function () {

    //$('#activity-and-budget-tab').click(function () {

        LoadAccountActivityAndBudgetTab('05523233-784D-4D6E-920B-0019EFAF9912');

    //});

});


function LoadAccountActivityAndBudgetTab(id) {
    

    var columns = [

    { dataField: 'Id'},
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
    //function LoadGrid(container, gridClass, columns, getRoute, saveRoute, selected, prefix, editModalClass, newModalClass, modalWidth, showDelete, showFilter, showGroup, onComplete) {

    //LoadGrid( '.summarygridcontainer', 'summarygrid', columns, 'accounts/activity/' + id, null, null, null, null, null, null, false, false, false,  function (data) {

    //        $('.summarytitle').empty();

    //        $('<label>').text(data.Data.AccountName).appendTo($('.summarytitle'));
        
    //    }); 
}