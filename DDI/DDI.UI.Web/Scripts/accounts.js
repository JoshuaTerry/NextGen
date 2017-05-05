$(document).ready(function () {

    //LoadAccountActivityAndBudgetTab('05523233-784D-4D6E-920B-0019EFAF9912');

    $('#summary-tab').click(function () {
        //LoadSummaryTab('');    // test new
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

    // retrieve ledger settings
    MakeServiceCall('GET', 'ledgers/' + ledgerid, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {

                $('.accountgroup1').css('visibility', 'hidden');
                $('.accountgroup2').css('visibility', 'hidden');
                $('.accountgroup3').css('visibility', 'hidden');
                $('.accountgroup4').css('visibility', 'hidden');
                var levels = data.Data.AccountGroupLevels;
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

                if (AccountId === null) {
                    LoadGroupDropDown(1, '');
                }
                else {
                    $('.accountsegmentscontainer').hide();
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
                $('.AccountNumber').val(data.Data.AccountNumber);
                $('.Name').val(data.Data.Name);
                $('.IsActive').prop('checked', (data.Data.IsActive === 1 ? true : false));
                //if (levels > 0) {
                //    $('.group1dropdown').val(data.Data.Group1Id);
                //}
                //if (levels > 1) {
                //    $('.group2dropdown').val(data.Data.Group2Id);
                //}
                //if (levels > 2) {
                //    $('.group3dropdown').val(data.Data.Group3Id);
                //}
                //if (levels > 3) {
                //    $('.group4dropdown').val(data.Data.Group4Id);
                //}
                $('.IsNormallyDebit').val(data.Data.IsNormallyDebit);
                $('.BeginningBalance').val(data.Data.BeginningBalance);
                //$('.Activity').val(data.Data.Activity);
                //$('.EndingBalance').val(data.Data.EndingBalance);

                FormatFields();
                if (levels > 0) {
                    LoadGroupDropDown(1, null, data.Data.Group1Id)
                }
                if (levels > 1) {
                    LoadGroupDropDown(2, data.Data.Group1Id, data.Data.Group2Id)
                }
                if (levels > 2) {
                    LoadGroupDropDown(3, data.Data.Group2Id, data.Data.Group3Id)
                }
                if (levels > 3) {
                    LoadGroupDropDown(4, data.Data.Group3Id, data.Data.Group4Id)
                }
            }
        }

    }, null);

}

function LoadGroupDropDown(level, parentId, initialId) {
    var fiscalYearId = 'E20F3200-8E69-4DE2-9339-1EC57EC89597';    // testing
    if (parentId === '') {
        PopulateDropDown('.group' + level + 'dropdown', 'fiscalyears/' + fiscalYearId + '/AccountGroups', initialId);
    }
    else {
        PopulateDropDown('.group' + level + 'dropdown', 'AccountGroups/' + parentId + '/parent', initialId);
    }
}

function GroupChange(level) {
    var parentVal = $('.group' + level + 'dropdown').val();
    if (parentVal != null && parentVal != '') {
        PopulateDropDown('.group' + (level + 1) + 'dropdown', 'AccountGroups/' + parentVal + '/parent', '');
    }

}


