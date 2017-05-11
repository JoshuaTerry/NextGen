$(document).ready(function () {

    //LoadAccountActivityAndBudgetTab('05523233-784D-4D6E-920B-0019EFAF9912');

    $('#summary-tab').click(function () {
        //LoadSummaryTab('');    // test new
        LoadSummaryTab('0A5110A1-BA39-4D6B-BF83-E9D319D691C3');    // test existing
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

var segmentLevelArray = [];
var segmentLevels;
var segmentLevel;
var groupLevels;
var accountId;
var segmentData = [];

function LoadSummaryTab(AccountId) {

    var fiscalYearId = 'E20F3200-8E69-4DE2-9339-1EC57EC89597';    // testing

    var ledgerId = '52822462-5041-46CB-9883-ECB1EF8F46F0'         // testing

    accountId = AccountId;
    var container = '.closingaccountcontainer';
    GLAccountSelector(container, ledgerId, fiscalYearId);

    // retrieve ledger settings
    MakeServiceCall('GET', 'ledgers/' + ledgerId, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {

                $('.accountgroup1').css('visibility', 'hidden');
                $('.accountgroup2').css('visibility', 'hidden');
                $('.accountgroup3').css('visibility', 'hidden');
                $('.accountgroup4').css('visibility', 'hidden');
                groupLevels = data.Data.AccountGroupLevels;
                if (groupLevels > 0) {
                    $('.accountgroup1').css('visibility', 'visible');
                    $('.group1prompt').html(data.Data.AccountGroup1Title);
                    $('.group1dropdown').change(function () {
                        GroupChange(1);
                    })
                }
                if (groupLevels > 1) {
                    $('.accountgroup2').css('visibility', 'visible');
                    $('.group2prompt').html(data.Data.AccountGroup2Title);
                    $('.group2dropdown').change(function () {
                        GroupChange(2);
                    })
                }
                if (groupLevels > 2) {
                    $('.accountgroup3').css('visibility', 'visible');
                    $('.group3prompt').html(data.Data.AccountGroup3Title);
                    $('.group3dropdown').change(function () {
                        GroupChange(3);
                    })
                }
                if (groupLevels > 3) {
                    $('.accountgroup4').css('visibility', 'visible');
                    $('.group4prompt').html(data.Data.AccountGroup4Title);
                }

                segmentLevels = data.Data.NumberOfSegments;

                for (i = (segmentLevels + 1) ; i <= 10; i++) {
                    $('.segmentgroup' + i).hide();
                }

                for (i = 1; i <= segmentLevels; i++) {
                    $('.segmentgroup' + i).show();
                    $('segment' + i + 'prompt').css('visibility', 'visible');
                    $('segment' + i + 'prompt').html(data.Data.SegmentLevels[i - 1].Abbreviation);
                    if (AccountId === null || AccountId === '') {
                        if (i = 1) {
                            LoadSegmentDropDown(1, '', null);
                        }
                    }
                }
                RetrieveSegmentLevels(ledgerId)

            }
        }

    }, null);

}

function RetrieveSegmentLevels(ledgerId) {
    MakeServiceCall('GET', 'ledgers/' + ledgerId + '/segmentlevels', null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {

                segmentLevelArray = data.Data;
                if (accountId === null || accountId === '') {
                    LoadGroupDropDown(1, '', null);
                }
                else {
                    $('.accountsegmentscontainer').hide();
                    RetrieveAccountSummaryData();
                }
            }
        }

    }, null);

}

function RetrieveAccountSummaryData() {
    MakeServiceCall('GET', 'accounts/' + accountId, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {
                $('.AccountNumber').val(data.Data.AccountNumber);
                $('.Name').val(data.Data.Name);
                $('.IsActive').prop('checked', (data.Data.IsActive));  
                $('.IsNormallyDebit').val((data.Data.IsNormallyDebit === true ? 1 : 0));
                $('.BeginningBalance').val(data.Data.BeginningBalance);

                if (groupLevels > 0) {
                    LoadGroupDropDown(1, '', data.Data.Group1Id)
                }
                if (groupLevels > 1) {
                    LoadGroupDropDown(2, data.Data.Group1Id, data.Data.Group2Id)
                }
                if (groupLevels > 2) {
                    LoadGroupDropDown(3, data.Data.Group2Id, data.Data.Group3Id)
                }
                if (groupLevels > 3) {
                    LoadGroupDropDown(4, data.Data.Group3Id, data.Data.Group4Id)
                }
                MakeServiceCall('GET', 'accounts/activity/' + accountId, null, function (data) {
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

function LoadGroupDropDown(groupLevel, parentId, initialId) {
    var fiscalYearId = 'E20F3200-8E69-4DE2-9339-1EC57EC89597';    // testing
    if (parentId === '' || parentId === null) {
        PopulateDropDown('.group' + groupLevel + 'dropdown', 'fiscalyears/' + fiscalYearId + '/AccountGroups', '', '', initialId);
    }
    else {
        PopulateDropDown('.group' + groupLevel + 'dropdown', 'AccountGroups/' + parentId + '/parent', '', '', initialId);
    }
}

function GroupChange(groupLevel) {
    var parentVal = $('.group' + groupLevel + 'dropdown').val();
    if (parentVal != null && parentVal != '') {
        PopulateDropDown('.group' + (groupLevel + 1) + 'dropdown', 'AccountGroups/' + parentVal + '/parent', '');
    }
}

function LoadSegmentDropDown(segmentLevel, parentId, initialId) {
    var fiscalYearId = 'E20F3200-8E69-4DE2-9339-1EC57EC89597';    // testing
    if (parentId === '') {
        PopulateDropDownData('.segment' + segmentLevel + 'dropdown', 'fiscalyears/' + fiscalYearId + '/segments/level/' + segmentLevel, '', null, null, SegmentChange, StoreSegmentData);
    }
    else {
        PopulateDropDownData('.segment' + segmentLevel + 'dropdown', 'fiscalyears/' + fiscalYearId + '/segments/' + parentId + '/level/' + segmentLevel, '', null, null, SegmentChange, StoreSegmentData);
    }
}

function StoreSegmentData(element, data) {
    var dropdownlevel = element.substring(8, 9);
    segmentData[dropdownlevel] = data.Data;
}

function SegmentChange(element) {
    var dropdownlevel = element.substring(8, 9);
    var segmentName;
    var segmentIsLinked;
    var segmentVal = $(element).val();
    var i = $(element).prop('selectedIndex')
    //for (i = 0; i <= segmentData.length - 1 ; i++) {
    //    if (segmentVal === segmentData[dropdownlevel - 1][i].Id) {
    //        segmentName = segmentData[dropdownlevel - 1][i].Name;
    //        if (dropdownlevel < segmentLevels) {
    //            segmentIsLinked = segmentLevelArray[dropdownlevel].IsLinked;
    //        }
    //    }
    //}
    segmentName = segmentData[dropdownlevel - 1][i].Name;
    if (dropdownlevel < segmentLevels) {
        segmentIsLinked = segmentLevelArray[dropdownlevel].IsLinked;
    }

    $('.segment' + dropdownlevel + 'name') = segmentName;

    //populate next segment level dropdown 
    if (dropdownlevel < $('.hidSegmentLevels').val) {
        if (segmentIsLinked === 1 && segmentVal != null) {
            PopulateDropDownData('.segment' + (dropdownlevel + 1) + 'dropdown', 'fiscalyears/' + fiscalYearId + '/segments/level/' + (dropdownlevel + 1), '', null, null, SegmentChange, StoreSegmentData);
        }
        else {
            PopulateDropDownData('.segment' + (dropdownlevel + 1) + 'dropdown', 'fiscalyears/' + fiscalYearId + '/segments/' + segmentVal + '/level/' + (dropdownlevel + 1), '', null, null, SegmentChange, StoreSegmentData);
        }

    }

}



