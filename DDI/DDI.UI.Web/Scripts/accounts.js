var fiscalYearId;
var ledgerId;
var accountId;

$(document).ready(function () {

    $('.editbusinessunit').addClass('disabled');

    accountId = sessionStorage.getItem('ACCOUNT_ID');

    if (accountId) {

        sessionStorage.removeItem('ACCOUNT_ID');

    }

    LoadAccountActivityAndBudgetTab(accountId);

    $('#activity-and-budget-tab').click(function (e) {

        e.preventDefault();

        LoadAccountActivityAndBudgetTab(accountId);

    });

    fiscalYearId = sessionStorage.getItem('FISCAL_YEAR_ID');

    if (fiscalYearId) {

        sessionStorage.removeItem('FISCAL_YEAR_ID');

        $('.tabscontainer').tabs("option", "active", 1);

    }

    $('.copyaccount').click(function (e) {

        e.preventDefault();

        CopyAccount();

    });

});

function LoadAccountActivityAndBudgetTab(id) {


    MakeServiceCall('GET', 'accounts/activity/' + id, null, function (dataActivity) {

        var columns = [
            { dataField: 'PeriodNumber', caption: 'Period Number', visible: false },
            { dataField: 'PeriodName', caption: 'Period Name' },

            { dataField: 'BeginningBalance', caption: 'Beginning Balance', format: 'currency', visible: false},
            { dataField: 'Debits', caption: 'Debits', format: 'currency' },
            { dataField: 'Credits', caption: 'Credits', format: 'currency' },
            { dataField: 'Activity', caption: 'Activity', format: 'currency' },
            { dataField: 'EndingBalance', caption: 'Ending Balance', format: 'currency' },

            { dataField: 'PriorBeginningBalance', caption: 'Prior Beginning Balance', format: 'currency', visible: false },
            { dataField: 'PriorDebits', caption: 'Prior Debits',  format: 'currency', visible: false },
            { dataField: 'PriorCredits', caption: 'Prior Credits',  format: 'currency', visible: false },
            { dataField: 'PriorActivity', caption: 'Prior Activity',  format: 'currency', visible: false },
            { dataField: 'PriorEndingBalance', caption: 'Prior Ending Balance',  format: 'currency', visible: false },

            { dataField: 'WorkingBudget', caption: dataActivity.Data.WorkingBudgetName, allowEditing: true, format: 'currency' },
            { dataField: 'FixedBudget', caption: dataActivity.Data.FixedBudgetName, allowEditing: true, format: 'currency'},
            { dataField: 'WhatIfBudget', caption: dataActivity.Data.WhatIfBudgetName, allowEditing: true, format: 'currency', visible: false },

            { dataField: 'WorkingBudgetVariance', caption: 'Working Budget Variance', allowEditing: true, format: 'currency' },
            { dataField: 'FixedBudgetVariance', caption: 'Fixed Budget Variance', allowEditing: true, format: 'currency', visible: false},
            { dataField: 'WhatIfBudgetVariance', caption: 'What-If Budget Variance', allowEditing: true, format: 'currency', visible: false}
        ];

        $('.activitytitle').empty();

        $('<label>').text(dataActivity.Data.AccountNumber + " " + dataActivity.Data.AccountName ).appendTo($('.activitytitle'));

        LoadGrid('.activitygridcontainer', 'activitygrid', columns, 'accounts/activity/' + id + '/detail', null, null, null, null, null, null, false, false, false, function (data) {

            $('.activitygrid').dxDataGrid({ columnChooser: { enabled: true, mode: 'select' } });

        });

    }, null);

}

//summary tab section

var segmentLevelArray = [];
var segmentLevels;
var segmentLevel;
var groupLevels;
var group1Data = [];
var saveGroupId;
var segmentData = [];
var accountSegmentItem = {Id: '', SegmentId: '', Segment: ''};
var accountSegmentArray = [];
var group1Id;
var group2Id;
var group3Id;
var group4Id;
var category = '0';
var editMode = '';
var activityTotal = 0;
var summaryContainer = '.accountsummarycontainer';

function LoadSummaryTab() {

    if (accountId != null && accountId != '') {
        RetrieveAccountSummaryData();
    }
    else {
        if (fiscalYearId) {
            RetrieveLedgerId();
        }
    }
}

function RetrieveAccountSummaryData() {
    MakeServiceCall('GET', 'accounts/' + accountId, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {
                fiscalYearId = data.Data.FiscalYearId;
                $('.AccountNumber').val(data.Data.AccountNumber);
                $('.Name').val(data.Data.Name);
                $('.IsActive').prop('checked', (data.Data.IsActive));
                $('.IsNormallyDebit').val((data.Data.IsNormallyDebit === true ? 1 : 0));
                $('.BeginningBalance').val(data.Data.BeginningBalance);

                category = data.Data.Category;

                group1Id = data.Data.Group1Id;
                group2Id = data.Data.Group2Id;
                group3Id = data.Data.Group3Id;
                group4Id = data.Data.Group4Id;

                $('.closingaccountgroup').find(".hidaccountid").val(data.Data.ClosingAccountId);
                if (data.Data.ClosingAccountId != null) {
                    $('.closingaccountgroup').find(".accountnumber").val(data.Data.ClosingAccount.AccountNumber);
                    $('.closingaccountgroup').find(".accountdescription").val(data.Data.ClosingAccount.Name);
                }

                accountSegmentArray = [];
                for (var i = 0; i < data.Data.AccountSegments.length; i++) {
                    accountSegmentItem = {Id: data.Data.AccountSegments[i].Id,
                        SegmentId: data.Data.AccountSegments[i].SegmentId,
                        Segment: data.Data.AccountSegments[i].Segment
                    }
                    accountSegmentArray.push(accountSegmentItem);
                }

                MakeServiceCall('GET', 'accounts/activity/' + accountId, null, function (data) {
                    if (data.Data) {
                        if (data.IsSuccessful) {
                            activityTotal = data.Data.ActivityTotal;
                            $('.Activity').val(data.Data.ActivityTotal);
                            $('.EndingBalance').val(data.Data.FinalEndingBalance);
                            RetrieveLedgerId();
                        }
                    }
                }, null);
            }

        }

    }, null);

}

function RetrieveLedgerId() {
    MakeServiceCall('GET', 'fiscalyears/' + fiscalYearId, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {
                ledgerId = data.Data.LedgerId;
                LoadSummaryTabContinued();
            }
        }

    }, null);

}

function LoadSummaryTabContinued() {
    GLAccountSelector($('.closingaccountgroup'), ledgerId, fiscalYearId);
    $('.accountselectorlabel').addClass("accountselectorprompt");
    $('.accountnumberlookup').removeAttr('style');
    $('.accountnumberlookup').attr("style", "width:30%");
    $('.closingaccountgroup').find('.fieldblock').each(function () {
        var oldDiv = $(this);
        var newSpan = $('<span></span>');
        oldDiv.before(newSpan);
        newSpan.append(oldDiv.contents());
        oldDiv.remove();
    });

    $('.accountnumberlookup').attr('disabled', true);
    $('.accountselectionsearch').css('visibility', 'hidden').addClass('accountsearch');

    $('.editaccount').click(function (e) {
        e.preventDefault();
        AccountEditMode();
    });

    $('.saveaccount').unbind('click');
    $('.saveaccount').click(function () {
        $('.saveaccountbuttons').hide();
        SaveAccount();
    });

    $('.cancelsaveaccount').click(function (e) {
        e.preventDefault();
        if (editMode === 'add') {
            ClearAccountFields();
            AccountAddMode();
        }
        else {
            $('.accountsegmentscontainer').hide();
            AccountDisplayMode();
        }
    });

    RetrieveLedgerSettings();

}

function RetrieveLedgerSettings() {
    MakeServiceCall('GET', 'ledgers/' + ledgerId, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {

                //groups
                groupLevels = data.Data.AccountGroupLevels;
                var groupLevel;
                var groupPrompt;
                var groupId;
                var parentGroupId;

                for (var i = 1; i <= groupLevels; i++) {
                    $('.accountgroup' + i).css('visibility', 'visible');

                    switch (i) {
                        case 1:
                            $('.group1prompt').html(data.Data.AccountGroup1Title + ':');
                            if (group1Id != null) {
                                LoadGroupDropDown(1, '', group1Id)
                            }
                            break;
                        case 2:
                            $('.group2prompt').html(data.Data.AccountGroup2Title + ':');
                            if (group2Id != null) {
                                LoadGroupDropDown(2, group1Id, group2Id)
                            }
                            break;
                        case 3:
                            $('.group3prompt').html(data.Data.AccountGroup3Title + ':');
                            if (group3Id != null) {
                                LoadGroupDropDown(3, group2Id, group3Id)
                            }
                            break;
                        case 4:
                            $('.group4prompt').html(data.Data.AccountGroup41Title + ':');
                            if (group4Id != null) {
                                LoadGroupDropDown(4, group3Id, group4Id)
                            }
                            break;
                    }

                    $('.editgroup' + i).click(function (e) {
                        e.preventDefault();
                        var test = $(this).attr('class');
                        groupLevel = $(this).attr('class').substring(9, 10);
                        groupId = '.Group' + groupLevel + 'Id';
                        groupPrompt = '.group' + groupLevel + 'prompt';
                        if ($(groupId).val() === null || $(groupId).val() === '') {
                            DisplayErrorMessage('Error', $(groupPrompt).html() + ' Must be selected first');
                            return;
                        }
                        saveGroupId = $(groupId).val();               // save for initing drop down
                        if (groupLevel === '1') {
                            parentGroupId = null;
                        }
                        else {
                            parentGroupId = $('.Group' + (parseInt(groupLevel) - 1) + 'Id').val()
                        }
                        ConfirmGroupModal(groupLevel, $('.Group' + groupLevel + 'Id').val(), parentGroupId, $(groupPrompt).html());
                    })
                    $('.newgroup' + i).click(function (e) {
                        var parentGroupId;
                        e.preventDefault();
                        groupLevel = $(this).attr('class').substring(8, 9);
                        groupPrompt = '.group' + groupLevel + 'prompt';
                        if (groupLevel > '1') {
                            parentGroupId = '.Group' + (parseInt(groupLevel) - 1) + 'Id';
                            if ($(parentGroupId).val() === null || $(parentGroupId).val() === '') {
                                DisplayErrorMessage('Error', $(groupPrompt).html() + ' Must be selected first');
                                alert($('.group' + (groupLevel - 1) + 'prompt').html() + ' Must be selected first');
                                return;
                            }
                        }
                        groupId = '.Group' + groupLevel + 'Id';
                        saveGroupId = $(groupId).val();               // save for initing drop down

                        if (groupLevel === '1') {
                            parentGroupId = null;
                        }
                        else {
                            parentGroupId = $('.Group' + (parseInt(groupLevel) - 1) + 'Id').val()
                        }
                        NewGroupModal(groupLevel, parentGroupId, $(groupPrompt).html());
                    })
                }

                //segments
                segmentLevels = data.Data.NumberOfSegments;

                for (i = (segmentLevels + 1); i <= 10; i++) {
                    $('.segmentgroup' + i).hide();
                }

                for (i = 1; i <= segmentLevels; i++) {
                    $('.segmentgroup' + i).show();
                    $('.segment' + i + 'prompt').html(data.Data.SegmentLevels[i - 1].Name + ':');
                }
                RetrieveSegmentLevels()
            }
        }

    }, null);

}

function RetrieveSegmentLevels() {
    MakeServiceCall('GET', 'ledgers/' + ledgerId + '/segmentlevels', null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {

                segmentLevelArray = data.Data;

                var segmentLevel;
                var segmentPrompt;
                var segmentId;
                var parentSegmentId;

                for (var i = 1; i <= segmentLevels; i++) {

                    $('.editsegment' + i).click(function (e) {
                        e.preventDefault();
                        segmentLevel = $(this).attr('class').substring(11, 12);
                        segmentId = '.Segment' + segmentLevel + 'Id';
                        segmentPrompt = '.segment' + segmentLevel + 'prompt';
                        if ($(segmentId).val() === null || $(segmentId).val() === '') {
                            alert($(segmentPrompt).html() + ' Must be selected first');
                            return;
                        }
                        if (segmentLevel === '1') {
                            parentSegmentId = null;
                        }
                        else {
                            if (segmentLevelArray[segmentLevel - 1].IsLinked === false) {
                                parentSegmentId = null;
                            }
                            else {
                                parentSegmentId = $('.Segment' + (parseInt(segmentLevel) - 1) + 'Id').val()
                            }
                        }
                        ConfirmSegmentModal(segmentLevel, $('.Segment' + segmentLevel + 'Id').val(), parentSegmentId, $(segmentPrompt).html());
                    })
                    $('.newsegment' + i).click(function (e) {
                        e.preventDefault();
                        segmentLevel = $(this).attr('class').substring(10, 11);
                        segmentPrompt = '.segment' + segmentLevel + 'prompt';
                        if (segmentLevelArray[segmentLevel - 1].IsLinked === true) {
                            segmentId = '.Segment' + (segmentLevel - 1) + 'Id';
                            if (segmentLevel > 1) {
                                if ($(segmentId).val() === null || $(segmentId).val() === '') {
                                    alert($('.segment' + (segmentLevel - 1) + 'prompt').html() + ' Must be selected first');
                                    return;
                                }
                            }
                        }
                        if (segmentLevel === '1') {
                            parentSegmentId = null;
                        }
                        else {
                            if (segmentLevelArray[segmentLevel - 1].IsLinked === false) {
                                parentSegmentId = null;
                            }
                            else {
                                parentSegmentId = $('.Segment' + (parseInt(segmentLevel) - 1) + 'Id').val()
                            }
                        }
                        NewSegmentModal(segmentLevel, $(segmentId).val(), $(segmentPrompt).html());
                    })

                }
                if (accountId === null || accountId === '') {
                    LoadGroupDropDown(1, '', null);
                    InitSegmentDropDowns();
                    AccountAddMode();
                }
                else {
                    LoadSegmentDropDowns();
                    AccountDisplayMode();
                }
            }
        }

    }, null);

}

function AccountDisplayMode() {
    editMode = 'display';
    $('.accountsegmentscontainer').hide();
    $('.editaccountbutton').show();
    $('.saveaccountbuttons').hide();
    $(summaryContainer).find('.editable').each(function () {

        $(this).prop('disabled', true);

    });
    $('.accountnumberlookup').attr('disabled', true);
    $('.accountselectionsearch').css('visibility', 'hidden').addClass('accountsearch');
    FormatFields();
}

function AccountAddMode() {
    editMode = 'add';
    MaskFields();
    $('.editaccountbutton').hide();
    $('.saveaccountbuttons').show();
    $('.accountsegmentscontainer').show();
}

function AccountEditMode() {
    editMode = 'edit';
    if (activityTotal === 0) {
        $('.accountsegmentscontainer').show();
    }
    $('.editaccountbutton').hide();
    $('.saveaccountbuttons').show();

    MaskFields();

    $(summaryContainer).find('.editable').each(function () {

        $(this).prop('disabled', false);

    });

    if (category < 4) {
        $('.BeginningBalance').removeAttr("disabled");
        $('.accountnumberlookup').attr('disabled', true);
        $('.accountselectionsearch').css('visibility', 'hidden');
    }
    else {
        $('.BeginningBalance').attr('disabled', true);
        $('.accountnumberlookup').removeAttr("disabled");
        $('.accountselectionsearch').css('visibility', 'visible');
    }

}

// account group section

function LoadGroupDropDown(groupLevel, parentId, initialId) {
    if (parentId === '' || parentId === null) {
        PopulateDropDown('.Group' + groupLevel + 'Id', 'fiscalyears/' + fiscalYearId + '/AccountGroups', '', '', initialId, GroupChange, StoreGroupData);
    }
    else {
        PopulateDropDown('.Group' + groupLevel + 'Id', 'AccountGroups/' + parentId + '/parent', '', '', initialId, GroupChange, StoreGroupData);
    }
}

// used to retrieve group 1's category later
function StoreGroupData(element, data) {
    var dropdownlevel = element.substring(6, 7);
    if (dropdownlevel === '1') {
        group1Data = data.Data;
    }
}

function GroupChange(element) {
    var level = element.substring(6, 7);
    var parentVal = $(element).val();

    if (level === '1') {
        var idx = $(element).prop('selectedIndex');
        if (idx > 0) {
            category = group1Data[idx - 1].Category;
            if (category < 4) {
                $('.BeginningBalance').removeAttr("disabled");
                $('.accountnumberlookup').attr('disabled', true);
                $('.accountselectionsearch').css('visibility', 'hidden');
            }
            else {
                $('.BeginningBalance').attr('disabled', true);
                $('.accountnumberlookup').removeAttr("disabled");
                $('.accountselectionsearch').css('visibility', 'visible');
            }
        }
    }

    //populate or clear subsequent group level dropdowns
    for (var i = (parseInt(level) + 1); i <= groupLevels; i++) {
        if (i === (parseInt(level) + 1)) {
            if (parentVal != null && parentVal != '') {
                PopulateDropDown('.Group' + i + 'Id', 'AccountGroups/' + parentVal + '/parent', '', null, null, GroupChange);
            }
            else {
                $('.Group' + i + 'Id').empty();
            }
        }
        else {
            $('.Group' + i + 'Id').empty();
        }
    }

}

// end group section

// segment section

// load non linked drop downs for adds
function InitSegmentDropDowns() {
    for (var i = 0; i < segmentLevels; i++) {
        if (segmentLevelArray[i].IsLinked === false) {
            LoadSegmentDropDown(i + 1, '', '');
        }
    }
}

// load all drop downs for edits
function LoadSegmentDropDowns() {

    for (var i = 0; i < segmentLevels; i++) {
        if (i < accountSegmentArray.length) {
            if (segmentLevelArray[i].IsLinked === false) {
                LoadSegmentDropDown((i + 1), '', accountSegmentArray[i].SegmentId);
            }
            else {
                if (accountSegmentArray[i - 1].Id === null) {
                    LoadSegmentDropDown((i + 1), '', accountSegmentArray[i].SegmentId);
                }
                else {
                    LoadSegmentDropDown((i + 1), accountSegmentArray[i - 1].SegmentId, accountSegmentArray[i].SegmentId);
                }
            }
            if (accountSegmentArray[i].Id != null) {
                $('.segment' + (i + 1) + 'code').html(accountSegmentArray[i].Segment.Code);
                $('.segment' + (i + 1) + 'name').html(accountSegmentArray[i].Segment.Name);
            }
        }
        else {
            if (segmentLevelArray[i].IsLinked === false) {
                LoadSegmentDropDown(i + 1, '', '');
            }
        }
    }
}

function LoadSegmentDropDown(segmentLevel, parentId, initialId) {
    if (parentId === '' || parentId === null) {
        PopulateDropDown('.Segment' + segmentLevel + 'Id', 'fiscalyears/' + fiscalYearId + '/segments/level/' + segmentLevel, '', null, initialId, SegmentChange, StoreSegmentData);
    }
    else {
        PopulateDropDown('.Segment' + segmentLevel + 'Id', 'fiscalyears/' + fiscalYearId + '/segments/' + parentId + '/level/' + segmentLevel, '', null, initialId, SegmentChange, StoreSegmentData);
    }
}

function StoreSegmentData(element, data) {
    var dropdownlevel = element.substring(8, 9);
    segmentData[parseInt(dropdownlevel) - 1] = data.Data;
}

function SegmentChange(element) {
    var level = element.substring(8, 9);
    var segmentName;
    var segmentCode;
    var parentVal = $(element).val();
    var idx = $(element).prop('selectedIndex')

    if (idx > 0) {
        segmentName = segmentData[parseInt(level) - 1][idx - 1].Name;
        segmentCode = segmentData[parseInt(level) - 1][idx - 1].Code;
    }
    else {
        segmentName = '';
        segmentCode = '';
    }
    $('.segment' + level + 'code').html(segmentCode);
    $('.segment' + level + 'name').html(segmentName);

    BuildAccountNumber();

    //populate or clear subsequent segment level dropdowns
    for (var i = (parseInt(level) + 1); i <= segmentLevels; i++) {
        if (i === (parseInt(level) + 1)) {
            if (segmentLevelArray[i - 1].IsLinked === true && parentVal != null) {
                PopulateDropDown('.Segment' + i + 'Id', 'fiscalyears/' + fiscalYearId + '/segments/' + parentVal + '/level/' + (parseInt(level) + 1), '', null, null, SegmentChange, StoreSegmentData);
                $('.segment' + i + 'code').html('');
                $('.segment' + i + 'name').html('');
            }
        }
        else {
            if (segmentLevelArray[i - 1].IsLinked === true) {
                $('.Segment' + i + 'Id').empty();
                $('.segment' + i + 'code').html('');
                $('.segment' + i + 'name').html('');
            }
        }
    }

}

//account save stuff

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

function SaveAccount() {

    var action;
    var route;

    var fields = GetAccountFields();

    if (editMode === 'add') {
        var action = 'POST';
        var route = 'accounts'
    }
    else {
        var action = 'PATCH';
        var route = 'accounts/' + accountId;
    }

    MakeServiceCall(action, route, fields, function (data) {

        if (data.Data) {
            DisplaySuccessMessage('Success', 'Account saved successfully.');
            if ($('.Activity').val() === '' || $('.Activity').val() === null) {
                $('.Activity').val(0);
            }
            $('.EndingBalance').val(parseFloat($('.BeginningBalance').val()) + parseFloat($('.Activity').val()));
            sessionStorage.setItem('ACCOUNT_ID', data.Data.Id);
            accountId = data.Data.Id;
            AccountDisplayMode();
        }

    },
        function () {
            if (editMode === 'add') {
                AccountAddMode();
            }
            else {
                AccountEditMode();
            }
        }
    );
}

function GetAccountFields() {

    var p = [];
    var value;

    $(summaryContainer).find('.editable').each(function () {
        var property = $(this).attr('class').split(' ');
        var propertyName = property[0];
        switch (propertyName) {
            case 'IsNormallyDebit': {
                if ($(this).val() === '1')
                    value = 'true';
                else
                    value = 'false';
                break;
            }
            case 'IsActive': {
                if ($(this).prop('checked') === true)
                    value = 'true';
                else
                    value = 'false';
                break;
            }
            case 'BeginningBalance': {
                if ($(this).val() === null || $(this).val() === '') {
                    $(this).val(0);
                }
                value = $(this).val();
                break;
            }
            case 'ClosingAccountId': {
                if ($(this).val() === '') {
                    $(this).val(false);
                }
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
    p.push('"ClosingAccountId": "' + $(summaryContainer).find(".hidaccountid").val() + '"');

    // Segment information
    asSet = []; // Array of AccountSegment objects
    for (var level = 1; level <= segmentLevels; level++) {
        var segment = $('.Segment' + level + 'Id');
        var i = segment.prop('selectedIndex');
        if (i > 0) {
            value = segmentData[level - 1][i - 1].Id;
            if (value && value.trim().length > 0) {
                var as = [];                    // An AccountSegment object
                as.push('"SegmentId": "' + value + '"');
                asSet.push('{' + as + '}');     // Add the object to the array
            }
        }
    }
    if (asSet.length > 0) {
        p.push('"AccountSegments": [ ' + asSet + ']');  // Add the array of Account Segment objects to the Account object
    }

    p = '{' + p + '}';

    return p;

}

function ClearAccountFields() {

    $(summaryContainer + ' input[type="text"]').val('');
    $(summaryContainer + ' input[type="number"]').val('');
    $(summaryContainer + ' .segmentname').html('');
    $(summaryContainer + ' .segmentcode').html('');
    $(summaryContainer + ' select').val(0);
    $(summaryContainer + ' input:checkbox').prop('checked', false);
    $(summaryContainer).find(".accountdescription").html('');
    $(summaryContainer).find(".accountnumber").val('');
    $(summaryContainer).find(".hidaccountid").val('');
}


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

            if (editMode != 'display') {
                LoadGroupDropDown(groupLevel, parentId, data.Data.Id)
            }
            else {
                LoadGroupDropDown(groupLevel, parentId, saveGroupId)
            }

        });

    });

}

function ConfirmGroupModal(groupLevel, groupId, parentId, groupName) {

    ConfirmModal('Warning: Updating this group may impact other accounts within this group.  Are you sure you want to continue?', function () {
        EditGroupModal(groupLevel, groupId, parentId, groupName);
    }, function () { return; });

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

            if (editMode != 'display') {
                LoadGroupDropDown(groupLevel, parentId, data.Data.Id)
            }

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

//end group modal section

// segment modal section

function NewSegmentModal(segmentLevel, parentId, segmentName) {
    $('.modalSegmentName').html(segmentName);
    $('.ag-Code').val('');
    $('.ag_Name').val('');

    modal = $('.accountsegmentmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 500,
        resizable: false
    });

    $('.cancelaccountsegmentmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.saveaccountsegmentbutton').unbind('click');

    $('.saveaccountsegmentbutton').click(function () {

        var item = GetAccountSegmentItemsToSave(modal, parentId, segmentLevel);

        MakeServiceCall('POST', 'Segments', item, function (data) {

            DisplaySuccessMessage('Success', 'Account Segment saved successfully.');

            CloseModal(modal);

            if (editMode != 'display') {
                LoadSegmentDropDown(segmentLevel, parentId, data.Data.Id);
            }

        });

    });

}

function ConfirmSegmentModal(segmentLevel, segmentId, parentId, segmentName) {

    ConfirmModal('Warning: Updating this segment may impact other accounts within this segment.  Are you sure you want to continue?', function () {
        EditSegmentModal(segmentLevel, segmentId, parentId, segmentName);
    }, function () { return; });

}

function EditSegmentModal(segmentLevel, segmentId, parentId, segmentName) {

    $('.modalSegmentName').html(segmentName);

    var modal = $('.accountsegmentmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 500,
        resizable: false
    });

    LoadAccountSegments(segmentId);

    $('.cancelaccountsegmentmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.saveaccountsegmentbutton').unbind('click');

    $('.saveaccountsegmentbutton').click(function () {

        var item = GetAccountSegmentItemsToSave(modal, parentId, segmentLevel);

        MakeServiceCall('PATCH', 'Segments/' + segmentId, item, function (data) {

            DisplaySuccessMessage('Success', 'Account Segment saved successfully.');

            CloseModal(modal);

            if (editMode != 'display') {
                LoadSegmentDropDown(segmentLevel, parentId, data.Data.Id);
            }

        });

    });
}

function LoadAccountSegments(id) {

    MakeServiceCall('GET', 'Segments/' + id, null, function (data) {

        $('.as-Code').val(data.Data.Code),
            $('.as-Name').val(data.Data.Name)

    }, function (xhr, status, err) {
        DisplayErrorMessage('Error', 'An error occurred during loading the Account Segment.');
    });

}

function GetAccountSegmentItemsToSave(modal, parentId, level) {

    var rawitem = {

        Name: $(modal).find('.as-Name').val(),
        Code: $(modal).find('.as-Code').val(),
        Level: level,
        FiscalYearId: fiscalYearId,
        ParentSegmentId: parentId

    };

    var item = JSON.stringify(rawitem);

    return item;

}

//end segments modal section

// COPY ACCOUNT

function ValidateAccountNumber() {

    var bar = '';

    MakeServiceCall('GET', 'accounts/fiscalyear/' + fiscalYearId + '/accountnumber/' + $(modal).find('.ac-AccountNumber').val(), null, function (data) {

        var foo = data.Data;

    }, null);

}

function CopyAccount() {

    // Clear the account Id and all other information except
    // FiscalYear, four account groups, the Category, IsNormallyDebit, AccountNumber, ClosingAccount, Name, and the AccountSegment collection.  Also set IsActive to true.  

    LoadSummaryTab(accountId);

    $('.IsActive').prop('checked', true);

    AccountAddMode();
}

// END COPY ACCOUNT

