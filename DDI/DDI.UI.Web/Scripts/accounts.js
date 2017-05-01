$(document).ready(function () {

    //LoadAccountActivityAndBudgetTab('05523233-784D-4D6E-920B-0019EFAF9912');

    $('#summary-tab').click(function () {
        LoadSummaryTab('');    // test new
        LoadSummaryTab('0A5110A1-BA39-4D6B-BF83-E9D319D691C3');    // test existing
    });

});


function LoadAccountActivityAndBudgetTab(id) {

    MakeServiceCall('GET', 'accounts/activity/' + id, null, function (data) {

    var columns = [

        { dataField: 'Id', width: '0px' },
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

function LoadSummaryTab(AccountId) {

    var fiscalYearId = 'E20F3200-8E69-4DE2-9339-1EC57EC89597';    // testing

    var ledgerid = '52822462-5041-46CB-9883-ECB1EF8F46F0'         // testing

    var group1Id;
    var group2Id;
    var group3Id;
    var group4Id;
    var levels = 0;

    MakeServiceCall('GET', 'ledgers/' + ledgerid, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {

                $('.accountgroup1').hide;
                $('.accountgroup2').hide;
                $('.accountgroup3').hide;
                $('.accountgroup4').hide;
                levels = data.Data.AccountGroupLevels;
                if (levels > 0){
                    $('.accountgroup1').show;
                    $('.group1prompt').val(data.Data.AccountGroup1Title);
                }
                if (levels > 1){
                    $('.accountgroup2').show;
                    $('.group2prompt').val(data.Data.AccountGroup2Title);
                }
                if (levels > 2){
                    $('.accountgroup3').show;
                    $('.group3prompt').val(data.Data.AccountGroup3Title);
                }
                if (levels > 3){
                    $('.accountgroup4').show;
                    $('.group4prompt').val(data.Data.AccountGroup4Title);
                }

            }
        }

    }, null);

    if (AccountId === '' || AccountId === null) {
        LoadGroup1DropDown(levels, null)

    }


    MakeServiceCall('GET', 'accounts/' + AccountId, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {


            }
        }

    }, null);

}


function LoadGroup1DropDown(levels, id) {
    PopulateDropDown('.group1dropdown', 'accountgroup', '', null, id, function () {
        LoadGroup2DropDown(levels, $('.group1dropdown').val())
    });
}

function LoadGroup2DropDown(levels, id) {
    if (levels > 1) {
        PopulateDropDown('.group2dropdown', 'accountgroup', '', null, id, function () {
            LoadGroup3DropDown(levels, null)
        });
    }
}

