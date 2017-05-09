$(document).ready(function () {

    //LoadAccountActivityAndBudgetTab('05523233-784D-4D6E-920B-0019EFAF9912');

    $('#summary-tab').click(function () {
        LoadSummaryTab('');    // test new
        //LoadSummaryTab('0A5110A1-BA39-4D6B-BF83-E9D319D691C3');    // test existing
    });

    $('#activity-and-budget-tab').click(function (e) {

        e.preventDefault();

        LoadAccountActivityAndBudgetTab('05523233-784D-4D6E-920B-0019EFAF9912');

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

    var ledgerId = '52822462-5041-46CB-9883-ECB1EF8F46F0'         // testing
    var container = $('.closingaccountgroup .closingaccountcontainer');
    GLAccountSelector(container, ledgerId, fiscalYearId);

    // retrieve ledger settings
    MakeServiceCall('GET', 'ledgers/' + ledgerId, null, function (data) {

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

                var segments = data.Data.NumberOfSegments;

                for (i = (segments + 1); i <= 10; i++) {
                    $('.segmentgroup' + i).hide();
                }

                for (i = 1; i <= segments; i++){
                    $('.segmentgroup' + i).show();
                    $('segment' + i + 'prompt').css('visibility', 'visible');
                    $('segment' + i + 'prompt').html(data.Data.SegmentLevels[i - 1].Abbreviation);
                    if (AccountId === null || AccountId === '') {
                        LoadSegmentDropDown(i, '', null);
                    }
                }

                if (AccountId === null || AccountId === '') {
                    LoadGroupDropDown(1, '', null);
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
                $('.IsActive').prop('checked', (data.Data.IsActive));  
                $('.IsNormallyDebit').val((data.Data.IsNormallyDebit === true ? 1 : 0));
                $('.BeginningBalance').val(data.Data.BeginningBalance);

                if (levels > 0) {
                    LoadGroupDropDown(1, '', data.Data.Group1Id)
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
                MakeServiceCall('GET', 'accounts/activity/' + AccountId, null, function (data) {
                    if (data.Data) {
                        if (data.IsSuccessful) {
                            $('.Activity').val(data.Data.ActivityTotal);
                            $('.EndingBalance').val(data.Data.FinalEndingBalance);

                            FormatFields();
                        }
                    }
                }, null);
            }

        }

    }, null);

}

function LoadGroupDropDown(level, parentId, initialId) {
    var fiscalYearId = 'E20F3200-8E69-4DE2-9339-1EC57EC89597';    // testing
    if (parentId === '') {
        PopulateDropDown('.group' + level + 'dropdown', 'fiscalyears/' + fiscalYearId + '/AccountGroups', '', '', initialId);
    }
    else {
        PopulateDropDown('.group' + level + 'dropdown', 'AccountGroups/' + parentId + '/parent', '', '', initialId);
    }
}

function GroupChange(level) {
    var parentVal = $('.group' + level + 'dropdown').val();
    if (parentVal != null && parentVal != '') {
        PopulateDropDown('.group' + (level + 1) + 'dropdown', 'AccountGroups/' + parentVal + '/parent', '');
    }

}

function LoadSegmentDropDown(level, parentId, initialId) {
    var fiscalYearId = 'E20F3200-8E69-4DE2-9339-1EC57EC89597';    // testing
    if (parentId === '') {
        PopulateDropDownData('.segment' + level + 'dropdown', 'fiscalyears/' + fiscalYearId + '/segments/level/' + level, '', null, null, SegmentChange(level, data), '');
    }
    else {
        PopulateDropDownData('.segment' + level + 'dropdown', 'Accountsegments/' + parentId + '/parent', '', '', initialId);
    }
}

function SegmentChange(level, data) {
    var parentVal = $('.segment' + level + 'dropdown').val();
    if (parentVal != null && parentVal != '') {
        PopulateDropDown('.segment' + (level + 1) + 'dropdown', 'Accountsegments/' + parentVal + '/parent', '');
    }

}



