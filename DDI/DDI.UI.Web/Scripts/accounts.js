$(document).ready(function () {

    //LoadAccountActivityAndBudgetTab('05523233-784D-4D6E-920B-0019EFAF9912');

    //$('#summary-tab').click(function () {
    //    LoadSummaryTab('');    // test new
    //    //LoadSummaryTab('0A5110A1-BA39-4D6B-BF83-E9D319D691C3');    // test existing
    //});

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
var group1Data = [];
var accountId;
var segmentData = [];

function LoadSummaryTab(AccountId) {

    var fiscalYearId = 'E20F3200-8E69-4DE2-9339-1EC57EC89597';    // testing

    var ledgerId = '52822462-5041-46CB-9883-ECB1EF8F46F0'         // testing

    accountId = AccountId;
    GLAccountSelector($('.closingaccountcontainer'), ledgerId, fiscalYearId);

    var container = '.accountsummarycontainer';

    if (AccountId === '' || AccountId === null) {
        $('.newaccountbuttons').show();
        $('.savenewaccount').unbind('click');
        $('.savenewaccount').click(function () {
            SaveNewAccount(container);
        });

        $('.cancelnewaccount').click(function (e) {
            e.preventDefault();
            ClearAccountFields(container);
        });


    }
    else {

        CreateEditControls();

        SetupEditControls();
    }

    // retrieve ledger settings
    MakeServiceCall('GET', 'ledgers/' + ledgerId, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {

                groupLevels = data.Data.AccountGroupLevels;
                if (groupLevels > 0) {
                    $('.accountgroup1').css('visibility', 'visible');
                    $('.group1prompt').html(data.Data.AccountGroup1Title + ':');
                    $('.Group1Id').change(function () {
                        GroupChange(1);
                    })
                }
                if (groupLevels > 1) {
                    $('.accountgroup2').css('visibility', 'visible');
                    $('.group2prompt').html(data.Data.AccountGroup2Title + ':');
                    $('.Group2Id').change(function () {
                        GroupChange(2);
                    })
                }
                if (groupLevels > 2) {
                    $('.accountgroup3').css('visibility', 'visible');
                    $('.group3prompt').html(data.Data.AccountGroup3Title + ':');
                    $('.Group3Id').change(function () {
                        GroupChange(3);
                    })
                }
                if (groupLevels > 3) {
                    $('.accountgroup4').css('visibility', 'visible');
                    $('.group4prompt').html(data.Data.AccountGroup4Title + ':');
                }

                segmentLevels = data.Data.NumberOfSegments;

                for (i = (segmentLevels + 1) ; i <= 10; i++) {
                    $('.segmentgroup' + i).hide();
                }

                for (i = 1; i <= segmentLevels; i++) {
                    $('.segmentgroup' + i).show();
                    $('.segment' + i + 'prompt').html(data.Data.SegmentLevels[i - 1].Name + ':');
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
                    $('.accountsegmentscontainer').show();
                    LoadGroupDropDown(1, '', null);
                    LoadSegmentDropDown(1, '', null);
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
        PopulateDropDownData('.Group' + groupLevel + 'Id', 'fiscalyears/' + fiscalYearId + '/AccountGroups', '', '', initialId, '', StoreGroupData);
    }
    else {
        PopulateDropDownData('.Group' + groupLevel + 'Id', 'AccountGroups/' + parentId + '/parent', '', '', initialId, '', StoreGroupData);
    }
}

// used to retrieve group 1's category later
function StoreGroupData(element, data) {
    var dropdownlevel = element.substring(6, 7);
    if (dropdownlevel === '1') {
        group1Data = data.Data;
    }
}

function GroupChange(groupLevel) {
    var element = $('.Group' + groupLevel + 'Id');
    var parentVal = element.val();
    if (parentVal != null && parentVal != '') {
        PopulateDropDown('.Group' + (groupLevel + 1) + 'Id', 'AccountGroups/' + parentVal + '/parent', '');
    }
    if (groupLevel === '1') {
        var i = element.prop('selectedIndex');
        if (group1Data[i - 1].Category.contains('123')) {
            $('.BeginningBalance').attr('enabled', true);
            $('.closingaccountgroup').attr('visibility', 'hidden');
        }
        else {
            $('.BeginningBalance').attr('enabled', false);
            $('.closingaccountgroup').attr('visibility', 'visible');
        }

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
    segmentData[parseInt(dropdownlevel) - 1] = data.Data;
}

function SegmentChange(element) {
    var fiscalYearId = 'E20F3200-8E69-4DE2-9339-1EC57EC89597';    // testing
    var dropdownlevel = element.substring(8, 9);
    var segmentName;
    var segmentCode;
    var segmentIsLinked;
    var segmentVal = $(element).val();
    var i = $(element).prop('selectedIndex')
    segmentName = segmentData[parseInt(dropdownlevel) - 1][i - 1].Name;
    segmentCode = segmentData[parseInt(dropdownlevel) - 1][i - 1].Code;
    if (dropdownlevel < segmentLevels) {
        segmentIsLinked = segmentLevelArray[dropdownlevel].IsLinked;
    }
    $('.segment' + dropdownlevel + 'code').html(segmentCode);
    $('.segment' + dropdownlevel + 'name').html(segmentName);

    BuildAccountNumber();

    //populate next segment level dropdown 
    if (dropdownlevel < $('.hidSegmentLevels').val) {
        if (segmentIsLinked === true && segmentVal != null) {
            PopulateDropDownData('.segment' + (parseInt(dropdownlevel) + 1) + 'dropdown', 'fiscalyears/' + fiscalYearId + '/segments/' + segmentVal + '/level/' + (parseInt(dropdownlevel) + 1), '', null, null, SegmentChange, StoreSegmentData);
        }
        else {
            PopulateDropDownData('.segment' + (parseInt(dropdownlevel) + 1) + 'dropdown', 'fiscalyears/' + fiscalYearId + '/segments/level/' + (parseInt(dropdownlevel) + 1), '', null, null, SegmentChange, StoreSegmentData);
        }

    }

}

function BuildAccountNumber() {
    var accountNumber;
    var segment;

    for (i = 1; i <= segmentLevels; i++) {
        segment = $('.segment' + i + 'code').html();
        if (segment != '') {
            if (i === 1) {
                accountNumber = segment;
            }
            else {
                accountNumber = accountNumber + '-' + segment;
            }
        }
    }

    $('.AccountNumber').val(accountNumber);
}

function SaveNewAccount(container) {

    // Get the fields
    var fields = GetNewAccountFields();

    // Save the Account
    MakeServiceCall('POST', 'accounts', fields, function (data) {

        if (data.Data) {
            // Display success
            DisplaySuccessMessage('Success', 'Account saved successfully.');

            ClearAccountFields(container);

        }

    }, null);

}

function GetNewAccountFields(container) {

    var p = [];

    $(modal).find('.accountsummarycontainer editable div.fieldblock input').each(function () {
        var property = $(this).attr('class').split(' ');
        var propertyName = property[0];
        var value = $(this).val();

        if (value && value.length > 0)
            p.push('"' + propertyName + '": "' + value + '"');
    });

    $(modal).find('.accountsummarycontainer editable select').each(function () {
        var property = $(this).attr('class').split(' ');
        var propertyName = property[0];
        var value = $(this).val();

        if (value && value.length > 0)
            p.push('"' + propertyName + '": "' + value + '"');
    });

    $(modal).find('.accountsummarycontainer editable checkbox').each(function () {
        var property = $(this).attr('class').split(' ');
        var propertyName = property[0];
        var value = $(this).val();

        if (value && value.length > 0)
            p.push('"' + propertyName + '": "' + value + '"');
    });

    p = '{' + p + '}';

    return p;

}

function ClearAccountFields(container) {

    $(container + ' input').val('');

    $(container + ' .segmentname').html('');

    $(container + ' .segmentcode').html('');

    $(container + ' select').val(0);

    $(container + ' input:checkbox').attr('checked', false);

}









