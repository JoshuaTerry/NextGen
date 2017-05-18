$(document).ready(function () {

    //LoadAccountActivityAndBudgetTab('05523233-784D-4D6E-920B-0019EFAF9912');

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


//summary tab section

var segmentLevelArray = [];
var segmentLevels;
var segmentLevel;
var groupLevels;
var group1Data = [];
var accountId;
var segmentData = [];
var category;
var fiscalYearId = 'E20F3200-8E69-4DE2-9339-1EC57EC89597';    // testing
var ledgerId = '52822462-5041-46CB-9883-ECB1EF8F46F0'         // testing

function LoadSummaryTab(AccountId) {

    accountId = AccountId;
    GLAccountSelector($('.closingaccountcontainer'), ledgerId, fiscalYearId);
    $('accountnumberlookup').removeClass('inline').addClass('summaryrightinput');
    $('accountdescription').removeClass('inline').addClass("summaryleftinput");
    $('accountselectionsearch').removeClass('inline').addClass("summaryrightcheckbox");

    $('.closingaccountgroup').attr('disabled', true);
    $('.accountselectionsearch').css('visibility', 'hidden');

    var container = '.accountsummarycontainer';

    if (AccountId === '' || AccountId === null) {
        $('.saveaccountbuttons').show();
        $('.saveaccount').unbind('click');
        $('.saveaccount').click(function () {
            SaveNewAccount(container);
        });

        $('.cancelsaveaccount').click(function (e) {
            e.preventDefault();
            ClearAccountFields(container);
        });

    }
    else {

        $('.editaccountbutton').show();
        $('.saveaccount').unbind('click');
        $('.saveaccount').click(function () {
            SaveOldAccount(container);
        });

        $('.editaccount').click(function (e) {
            $('.accountsegmentscontainer').show();
            LoadSegmentDropDowns();
            e.preventDefault();
        });

        $('.cancelsaveaccount').click(function (e) {
            e.preventDefault();
            ClearAccountFields(container);
            RetrieveAccountSummaryData();
        });

        CreateEditControls();

        SetupEditControls();
    }

    // retrieve ledger settings
    MakeServiceCall('GET', 'ledgers/' + ledgerId, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {

                groupLevels = data.Data.AccountGroupLevels;
                for (i = 1; i <= groupLevels; i++) {
                    $('.accountgroup' + i).css('visibility', 'visible');
                    switch (i) {
                        case 1:
                            $('.group1prompt').html(data.Data.AccountGroup1Title + ':');
                            break;
                        case 2:
                            $('.group2prompt').html(data.Data.AccountGroup2Title + ':');
                            break;
                        case 3:
                            $('.group3prompt').html(data.Data.AccountGroup3Title + ':');
                            break;
                        case 4:
                            $('.group4prompt').html(data.Data.AccountGroup41Title + ':');
                            break;
                    }

                    if (i < 4) {
                        $('.Group' + i + 'Id').change(function () {
                            GroupChange(i);
                        })
                    }
                    $('.editgroup' + i).click(function (e) {
                        e.preventDefault();
                        EditGroup(i);
                    })
                    $('.newgroup1' + i).click(function (e) {
                        e.preventDefault();
                        NewGroup(i);
                    })
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

function EditGroup(level) {
    var prevLevel = level - 1;
    if ($('.Group' + level + 'Id').val() === null || $('.Group' + level + 'Id').val() === '') {
        alert($('.group' + level + 'prompt').html() + ' must be selected first');
        return;
    }
    if (level > 1) {
        if ($('.Group' + prevLevel + 'Id').val() === null || $('.Group' + prevLevel + 'Id').val() === '') {
            alert($('.group' + prevLevel + 'prompt').html() + ' must be selected first');
            return;
        }
    }
    EditGroupModal(level, $('.Group' + level + 'Id').val(), $('.Group' + prevLevel + 'Id').val(), $('.group' + level + 'prompt').html());
}

function NewGroup(level) {
    var prevLevel = level - 1;
    if (level > 1) {
        if ($('.Group' + prevLevel + 'Id').val() === null || $('.Group' + prevLevel + 'Id').val() === '') {
            alert($('.group' + prevLevel + 'prompt').html() + ' must be selected first');
            return;
        }
        NewGroupModal(level, $('.Group' + prevLevel + 'Id').val(), $('.group' + level + 'prompt').html());
    }
    else {
        NewGroupModal(level, null, $('.group' + level + 'prompt').html());
    }
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

                category = data.Data.Category;
                if (category < 4) {
                    $('.BeginningBalance').removeAttr("disabled");
                    $('.closingaccountgroup').attr('disabled', true);
                    $('.accountselectionsearch').css('visibility', 'hidden');
                }
                else {
                    $('.BeginningBalance').attr('disabled', true);
                    $('.closingaccountgroup').removeAttr("disabled");
                    $('.accountselectionsearch').css('visibility', 'visible');
                }

                if (groupLevels > 0 && data.Data.Group1Id != null) {
                    LoadGroupDropDown(1, '', data.Data.Group1Id)
                }
                if (groupLevels > 1 && data.Data.Group2Id != null) {
                    LoadGroupDropDown(2, data.Data.Group1Id, data.Data.Group2Id)
                }
                if (groupLevels > 2 && data.Data.Group3Id != null) {
                    LoadGroupDropDown(3, data.Data.Group2Id, data.Data.Group3Id)
                }
                if (groupLevels > 3 && data.Data.Group4Id != null) {
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
                    else {
                        var test = 1;
                    }
                }, null);
            }

        }

    }, null);

}

// account group section

function LoadGroupDropDown(groupLevel, parentId, initialId) {
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
    if (groupLevel === 1) {
        var i = element.prop('selectedIndex');
        if (i > 0) {
            category = group1Data[i - 1].Category;
            if (category < 4) {
                $('.BeginningBalance').removeAttr("disabled");
                $('.closingaccountgroup').attr('disabled', true);
                $('.accountselectionsearch').css('visibility', 'hidden');
            }
            else {
                $('.BeginningBalance').attr('disabled', true);
                $('.closingaccountgroup').removeAttr("disabled");
                $('.accountselectionsearch').css('visibility', 'visible');
            }
        }
    }

    if (parentVal != null && parentVal != '') {
        PopulateDropDown('.Group' + (groupLevel + 1) + 'Id', 'AccountGroups/' + parentVal + '/parent', '');
    }
}

// end group section

// segment section

function LoadSegmentDropDowns() {
    for (i = 0; segments; i++) {
        if (data.Data.AccountSegments[i].SegmentId != null) {
            if (i === 0 || data.Data.AccountSegments[i].IsLinked === 0) {
                LoadSegmentDropDown(i + 1, '', data.Data.AccountSegments[i].SegmentId);
            }
            else {
                LoadSegmentDropDown(i + 1, data.Data.AccountSegments[i - 1].SegmentId, data.Data.AccountSegments[i].SegmentId);
            }
        }
    }
}

function LoadSegmentDropDown(segmentLevel, parentId, initialId) {
    if (parentId === '' || parentId === null) {
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
    var fields = GetNewAccountFields(container);

    // Save the Account
    MakeServiceCall('POST', 'accounts', fields, function (data) {

        if (data.Data) {
            DisplaySuccessMessage('Success', 'Account saved successfully.');
            ClearAccountFields(container);
        }

    }, null);

}

function GetNewAccountFields(container) {

    var p = [];
    var value;

    $(container).find('.editable').each(function () {
        var property = $(this).attr('class').split(' ');
        var propertyName = property[0];
        switch (propertyName) {
            case 'IsNormallyDebit': {
                if ($(this).val() === '1')
                    value = true;
                else
                    value = false;
                break;
            }
            case 'IsActive': {
                if ($(this).prop('checked') === true)
                    value = true;
                else
                    value = false;
                break;
            }
            case 'BeginningBalance': {
                if ($(this).val() === null || $(this).val() === '')
                    value = 0;
                else
                    value = $(this).val();
                break;
            }
            default:
                value = $(this).val();
        }

        if (value)
            p.push('"' + propertyName + '": "' + value + '"');
    });

    // add non-screen items
    p.push('"Category": "' + category + '"');
    p.push('"FiscalYearId": "' + fiscalYearId + '"');
    p.push('"ClosingAccountId": "' + $(container).find(".hidaccountid").val() + '"');

    // Segment information
    asSet = []; // Array of AccountSegment objects
    for (var level = 1; level <= segmentLevels; level++) {
        var segment = $('.segment' + level + 'dropdown');
        var i = segment.prop('selectedIndex');
        value = segmentData[level - 1][i - 1].Id;
        if (value && value.trim().length > 0) {
            var as = [];  // An AccountSegment object
            as.push("Level", level);
            as.push("SegmentId", value);
            asSet.push('{' + as + '}');  // Add the object to the array
        }
    }
    if (asSet.length > 0) {
        p.push('"AccountSegment": [ ' + asSet + ']');  // Add the array of Account Segment objects to the Account object
    }

    p = '{' + p + '}';

    return p;

}

function ClearAccountFields(container) {

    $(container + ' input[type="text"]').val('');
    $(container + ' input[type="number"]').val('');
    $(container + ' .segmentname').html('');
    $(container + ' .segmentcode').html('');
    $(container + ' select').val(0);
    $(container + ' input:checkbox').prop('checked', false);
    $(container).find(".accountdescription").html('');
    $(container).find(".accountnumber").val('');
    $(container).find(".hidaccountid").val('');
}

function SaveOldAccount(container) {

    // Get the fields
    var fields = GetNewAccountFields(container);

    // Save the Account
    MakeServiceCall('PATCH', 'accounts', fields, function (data) {

        if (data.Data) {
            DisplaySuccessMessage('Success', 'Account saved successfully.');
            ClearAccountFields(container);
        }

    }, null);

}

//end summary tab section


//group modal section

function NewGroupModal(groupLevel, parentId, groupName) {
    $('.modalGroupName').html(groupName);
    $('.ag-Category').val(0);
    $('.ag_Name').val('');
    if (groupLevel != 1) {
        $('.group1only').hide();
    }
    else {
        $('.group1only').show();
    }

    modal = $('.accountgroupmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 500,
        resizable: false
    });

    $('.cancelaccountgroupmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.saveaccountgroupbutton').unbind('click');

    $('.saveaccountgroupbutton').click(function () {

        var item = GetAccountGroupItemsToSave(modal, parentId);

        MakeServiceCall('POST', 'AccountGroups', item, function (data) {

            DisplaySuccessMessage('Success', 'Account Group saved successfully.');

            CloseModal(modal);

            LoadGroupDropDown(groupLevel, parentId, '')

        }, function (xhr, status, err) {

            DisplayErrorMessage('Error', 'An error occurred during saving the Account Group.');

        });

    });

}

function EditGroupModal(groupLevel, groupId, parentId, groupName) {

    $('.modalGroupName').html(groupName);
    if (groupLevel != 1) {
        $('.ag-Category').val(0);
        $('.group1only').hide();
    }
    else {
        $('.group1only').show();
    }

    var modal = $('.accountgroupmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 500,
        resizable: false
    });

    LoadAccountGroups(groupId);
    
    $('.cancelaccountgroupmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.saveaccountgroupbutton').unbind('click');

    $('.saveaccountgroupbutton').click(function () {

        var item = GetAccountGroupItemsToSave(modal, parentId);

        MakeServiceCall('PATCH', 'AccountGroups/' + groupId, item, function (data) {

            DisplaySuccessMessage('Success', 'Account Group saved successfully.');

            CloseModal(modal);

            LoadGroupDropDown(groupLevel, parentId, '')

        }, function (xhr, status, err) {

            DisplayErrorMessage('Error', 'An error occurred during saving the Account Group.');
        });

    });
}

function LoadAccountGroups(id) {

    MakeServiceCall('GET', 'AccountGroups/' + id, null, function (data) {

        $('.ag-Name').val(data.Data.Name),
        $('.ag-Category').val(data.Data.Category)

    }, function (xhr, status, err) {
        DisplayErrorMessage('Error', 'An error occurred during loading the Account Group.');
    });

}


function GetAccountGroupItemsToSave(modal, parentId) {

    var rawitem = {

        Name: $(modal).find('.ag-Name').val(),
        Category: $(modal).find('.ag-Category').val(),
        FiscalYearId: fiscalYearId,
        ParentGroupId: parentId

    };

    var item = JSON.stringify(rawitem);

    return item;

}

//end segments modal section

