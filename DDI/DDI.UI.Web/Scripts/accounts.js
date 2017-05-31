
$(document).ready(function () {

    LoadAccountActivityAndBudgetTab('05523233-784D-4D6E-920B-0019EFAF9912');

    $('#activity-and-budget-tab').click(function (e) {

        e.preventDefault();

        LoadAccountActivityAndBudgetTab('05523233-784D-4D6E-920B-0019EFAF9912');

    });

});


function LoadAccountActivityAndBudgetTab(id) {

    MakeServiceCall('GET', 'accounts/activity/' + id, null, function (data) {

    var columns = [
    
        { dataField: 'PeriodName', caption: 'Period' },
        { dataField: 'WorkingBudget', caption: data.Data.WorkingBudgetName, allowEditing: true },
        { dataField: 'FixedBudget', caption: data.Data.FixedBudgetName, allowEditing: true },
        { dataField: 'WhatIfBudget', caption: data.Data.WhatIfBudgetName, allowEditing: true },
        { dataField: 'Debits', caption: 'Debits' },
        { dataField: 'PriorCredits', caption: data.Data.PriorYearName },
        { dataField: 'Credits', caption: 'Credits' }
    ];

    LoadGrid('.activitygridcontainer', 'activitygrid', columns, 'accounts/activity/' + id + '/detail', null, null, null, null, null, null, false, false, false, function (data) {

        $('.activitytitle').empty();

        $('<label>').text(data.Data.AccountName).appendTo($('.activitytitle'));

    });

    }, null);

}