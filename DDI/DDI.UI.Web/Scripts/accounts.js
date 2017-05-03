$(document).ready(function () {

    //LoadAccountActivityAndBudgetTab('05523233-784D-4D6E-920B-0019EFAF9912');

    $('#summary-tab').click(function () {
        LoadSummaryTab('');    // test new
        //LoadSummaryTab('0A5110A1-BA39-4D6B-BF83-E9D319D691C3');    // test existing
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

    // retrieve ledger settings
    MakeServiceCall('GET', 'ledgers/' + ledgerid, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {

                $('.accountgroup1').css('visibility', 'hidden');
                $('.accountgroup2').css('visibility', 'hidden');
                $('.accountgroup3').css('visibility', 'hidden');
                $('.accountgroup4').css('visibility', 'hidden');
                levels = data.Data.AccountGroupLevels;
                if (levels > 0){
                    $('.accountgroup1').css('visibility', 'visible');
                    $('.group1prompt').html(data.Data.AccountGroup1Title);
                    $('.group1dropdown').change(function () {
                        GroupChange(1);
                    })
                }
                if (levels > 1){
                    $('.accountgroup2').css('visibility', 'visible');
                    $('.group2prompt').html(data.Data.AccountGroup2Title);
                    $('.group2dropdown').change(function () {
                        GroupChange(2);
                    })
                }
                if (levels > 2){
                    $('.accountgroup3').css('visibility', 'visible');
                    $('.group3prompt').html(data.Data.AccountGroup3Title);
                    $('.group3dropdown').change(function () {
                        GroupChange(3);
                    })
                }
                if (levels > 3){
                    $('.accountgroup4').css('visibility', 'visible');
                    $('.group4prompt').html(data.Data.AccountGroup4Title);
                }

                if (addMode = true) {
                    LoadGroupDropDown(1, '');
                }
                else {
                    RetrieveAccountSummaryData(AccountId, levels);
                }

            }
        }

    }, null);

}

function RetrieveAccountSummaryData(AccountId, levels) {
    MakeServiceCall('GET', 'accounts/' + AccountId, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {
                $('.AccountNumber').val(data.data.AccountNumber);
                $('.Name').val(data.data.Name);
                $('.IsActive').prop('checked', (data.Data.IsActive === 1 ? true : false));
                if (levels > 0) {
                    $('.group1dropdown').val(data.data.Group1Id);
                }
                if (levels > 1) {
                    $('.group2dropdown').val(data.data.Group2Id);
                }
                if (levels > 2) {
                    $('.group3dropdown').val(data.data.Group3Id);
                }
                if (levels > 3) {
                    $('.group4dropdown').val(data.data.Group4Id);
                }
                $('.IsNormallyDebit').val(data.data.IsNormallyDebit);
                $('.BeginningBalance').val(data.data.BeginningBalance);
                //$('.Activity').val(data.data.Activity);
                //$('.EndingBalance').val(data.data.EndingBalance);

                FormatFields();
                if (levels > 0) {
                    LoadGroupDropDown(1, data.data.Group1Id)
                }
                if (levels > 1) {
                    LoadGroupDropDown(2, data.data.Group2Id)
                }
                if (levels > 2) {
                    LoadGroupDropDown(3, data.data.Group3Id)
                }
                if (levels > 3) {
                    LoadGroupDropDown(4, data.data.Group4Id)
                }
            }
        }

    }, null);

}

function LoadGroupDropDown(level, parentId) {
    var fiscalYearId = 'E20F3200-8E69-4DE2-9339-1EC57EC89597';    // testing
    if (parentId === '') {
        PopulateDropDown('.group' + level + 'dropdown', 'fiscalyears/' + fiscalYearId + '/AccountGroups', '');
    }
    else {
        PopulateDropDown('.group' + level + 'dropdown', 'AccountGroups/' + parentId + '/parent', '');
    }
}

function GroupChange(level) {
    var parentVal = $('.group' + level + 'dropdown').val();
    if (parentVal != null && parentVal != '') {
        PopulateDropDown('.group' + (level + 1) + 'dropdown', 'AccountGroups/' + parentVal + '/parent', '');
    }

}


