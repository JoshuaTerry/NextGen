/* GENERAL LEDGER SETTINGS */
function LoadAccountingSettingsSectionSettings() {

    $('.gridcontainer').empty();

    var acctsettingscontainer = $('<div>').addClass('accountingsettingscontainer onecolumn').appendTo($('.gridcontainer'));

    $('<input>').attr('type', 'hidden').addClass('hidLedgerId').appendTo(acctsettingscontainer);


    // Audit Link
    $('<a>').text('Audit History').attr('href', '#').attr('type', 'hidden').addClass('newauditmodal').prependTo($('.gridcontainer'));


    // Select the ledger
    CreateBasicFieldBlock('Ledger: ', '<select>', 'as-ledgerselect', acctsettingscontainer, true);

    PopulateDropDown('.as-ledgerselect', 'ledgers/businessunit/' + currentBusinessUnitId, '<Please Select>', null, '', function () {

        $('.hidLedgerId').val($('.as-ledgerselect').val());
        ShowAuditData($('.hidLedgerId').val());
        LoadAccountingSettings($('.hidLedgerId').val());


    }, function (element, data) {

        if (data.TotalResults > 1) {

            $('.as-ledgerselect').val(data.Data[0].Id);

        }

        $('.hidLedgerId').val($('.as-ledgerselect').val());
        ShowAuditData($('.hidLedgerId').val());
        LoadAccountingSettings($('.hidLedgerId').val());

    });

    // fiscal year
    CreateBasicFieldBlock('Fiscal Year: ', '<select>', 'as-fiscalyear', acctsettingscontainer, true);

    // transaction posted automatially
    CreateBasicFieldBlock('Post Transactions Automatically: ', '<input type="checkbox">', 'as-postedtransaction', acctsettingscontainer, false);

    // how many days in advance recurring journals will be processed
    CreateBasicFieldBlock('Number of days before recurring journals post:', '<input type="text">', 'as-daysinadvance', acctsettingscontainer, false, 3);

    // disable or enable approvals for journals
    CreateBasicFieldBlock('Enable Approvals:', '<input type="checkbox">', 'as-approval', acctsettingscontainer, false);

    CreateSaveAndCancelButtons('saveAccountingSettings', function () {


        var data = {

            Id: $('.as-ledgerselect').val(),
            DefaultFiscalYearId: $('.as-fiscalyear').val(),
            PostAutomatically: $('.as-postedtransaction').prop('checked'),
            PostDaysInAdvance: $('.as-daysinadvance').val(),
            ApproveJournals: $('.as-approval').prop('checked')
        };

        MakeServiceCall('PATCH', 'ledgers/' + $('.hidLedgerId').val(), data, function () {

            LoadAccountingSettings($('.hidLedgerId').val());
            DisplaySuccessMessage('Success', 'Accounting settings saved successfully.');

        }, null);


    }, 'cancel', function (e) {

        e.preventDefault();

        LoadAccountingSettings($('.hidLedgerId').val());

    },

        acctsettingscontainer);
}



function LoadAccountingSettings(id) {

    MakeServiceCall('GET', 'ledgers/' + id, null, function (data) {

        $('.hidLedgerId').val(data.Data.Id);
        $('.as-postedtransaction').prop('checked', data.Data.PostAutomatically);
        $('.as-daysinadvance').val(data.Data.PostDaysInAdvance);
        $('.as-approval').prop('checked', data.Data.ApproveJournals);

        PopulateDropDown('.as-fiscalyear', 'fiscalyears/ledger/' + $('.hidLedgerId').val(), '', '', data.Data.DefaultFiscalYearId, null);

    }, null);
}

function PickLedger() { }

function LoadBudgetSectionSettings() {

    $('.gridcontainer').empty();

    var container = $('<div>').addClass('budgetsettingscontainer onecolumn').css('width', '50%').appendTo($('.gridcontainer'));

    CreateBasicFieldBlock('Ledger: ', '<select>', 'budgetLedgerId', container, true);

    PopulateDropDown('.budgetLedgerId', 'ledgers/businessunit/' + currentBusinessUnitId, '', '', '', function () {
    }, function () {
        GetBudgetSetting();
    });

    $(container).find('.budgetLedgerId').change(function () { GetBudgetSetting(); });

    CreateBasicFieldBlock('Working Budget Name: ', '<input>', 'workingBudgetName', container, true, 128);

    CreateBasicFieldBlock('Fixed Budget Name: ', '<input>', 'fixedBudgetName', container, true, 128);

    CreateBasicFieldBlock('What If Budget Name: ', '<input>', 'whatifBudgetName', container, true, 128);

    var id = $('<input>').attr('type', 'hidden').addClass('hidLedgerId').appendTo(container);
    CreateSaveAndCancelButtons('saveBudgetSettings', function (e) {
        var data = {
            Id: $(id).val(),
            WorkingBudgetName: $('.workingBudgetName').val(),
            FixedBudgetName: $('.fixedBudgetName').val(),
            WhatIfBudgetName: $('.whatifBudgetName').val(),
        }

        MakeServiceCall('PATCH', 'ledgers/' + $(id).val(), data, function () {

            LoadAccountingSettings($('.hidLedgerId').val());
            DisplaySuccessMessage('Success', 'Accounting settings saved successfully.');

        }, null);

    }, 'cancel', function (e) {

        e.preventDefault();

        GetBudgetSetting();

    }, container);

}

function ValidBudgetSettingForm() {
    var validform = true;

    // required items
    if (ValidateForm('budgetsettingscontainer') === false) {
        return false;
    }

    return validform;
}

function GetBudgetSetting() {
    var ledgerid = $('.budgetLedgerId').val();

    MakeServiceCall('GET', 'ledgers/' + ledgerid, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {

                $('.hidLedgerId').val(data.Data.Id);
                $('.workingBudgetName').val(data.Data.WorkingBudgetName)
                $('.fixedBudgetName').val(data.Data.FixedBudgetName)
                $('.whatifBudgetName').val(data.Data.WhatIfBudgetName)

            }
        }

    }, null);
}

function LoadChartAccountsSectionSettings() {

    $('.gridcontainer').empty();

    var ledgerid;

    var container = $('<div>').addClass('chartsettingscontainer onecolumn');

    var selectledgergroup = $('<div>');
    $('<label>').text('Select Ledger: ').appendTo(selectledgergroup);
    var selectledgername = $('<select>').addClass('chartLedgerId').appendTo(selectledgergroup);
    $(selectledgergroup).append('<br />').append('<br />').appendTo(container);

    PopulateDropDown('.chartLedgerId', 'ledgers/businessunit/' + currentBusinessUnitId, '', '', '', function () {
        //update on change  (not working so added .change logic below
        //GetChartSetting();
    }, function () {
        //retrieve initial value on populate complete
        GetChartSetting();
    });

    $('.chartLedgerId').change(function () {
        GetChartSetting();
    });

    var capitalizeheadersgroup = $('<div>').addClass('fieldblock');
    var capitalizeheaderscheckbox = $('<input>').attr('type', 'checkbox').addClass('capitalizeheaders').appendTo(capitalizeheadersgroup);
    $('<span>').text('Capitalize account group descriptions').appendTo(capitalizeheadersgroup);
    $(capitalizeheadersgroup).append('<br />').appendTo(container);

    var grouplevelsgroup = $('<div>');
    $('<label>').text('Number of account groups: ').appendTo(grouplevelsgroup);
    var grouplevels = $('<select>').addClass('groupLevels').appendTo(grouplevelsgroup).change(function () {
        GroupLevelsChange();
    });
    grouplevels.append('<option value="1">1</option>');
    grouplevels.append('<option value="2">2</option>');
    grouplevels.append('<option value="3">3</option>');
    grouplevels.append('<option value="4">4</option>');
    grouplevels.appendTo(grouplevelsgroup);
    $(grouplevelsgroup).append('<br />').append('<br />').appendTo(container);

    var group1 = $('<div>').addClass('fieldblock AccountGroup1group');
    $('<label>').text('Account Group 1: ').appendTo(group1);
    $('<input>').attr({ type: 'text', maxLength: '40' }).addClass('accountGroup1Title').appendTo(group1);
    $(group1).appendTo(container);

    var group2 = $('<div>').addClass('fieldblock AccountGroup2group AccountGroup ag2 ag3 ag4');
    $('<label>').text('Account Group 2: ').appendTo(group2);
    $('<input>').attr({ type: 'text', maxLength: '40' }).addClass('accountGroup2Title').appendTo(group2);
    $(group2).hide().appendTo(container);

    var group3 = $('<div>').addClass('fieldblock AccountGroup3group AccountGroup ag3 ag4');
    $('<label>').text('Account Group 3: ').appendTo(group3);
    $('<input>').attr({ type: 'text', maxLength: '40' }).addClass('accountGroup3Title').appendTo(group3);
    $(group3).hide().appendTo(container);

    var group4 = $('<div>').addClass('fieldblock AccountGroup4group AccountGroup ag4');
    $('<label>').text('Account Group 4: ').appendTo(group4);
    $('<input>').attr({ type: 'text', maxLength: '40' }).addClass('accountGroup4Title').appendTo(group4);
    $(group4).hide().appendTo(container);

    var errorgroup = $('<div>').addClass('fieldblock');
    $('<label>').text('').addClass('validateerror chartsettingerror').append('<br />').appendTo(errorgroup);
    $(errorgroup).append('<br />').appendTo(container);

    var id = $('<input>').attr('type', 'hidden').addClass('hidLedgerId').appendTo(container);

    var controlContainer = $('<div>').addClass('controlContainer');

    $('<input>').attr('type', 'button').addClass('saveEntity').val('Save')
        .click(function () {
            if (ValidChartSettingForm() === true) {
                SaveChartSetting(id);
            }
        })
        .appendTo(controlContainer);

    $('<a>').addClass('cancel').text('Cancel').attr('href', '#')
        .click(function (e) {
            e.preventDefault();
            $('.chartsettingerror').text('');
            RemoveValidation('chartsettingscontainer')

            GetChartSetting();
        })
        .appendTo(controlContainer);

    $(controlContainer).appendTo(container);

    $(container).appendTo($('.gridcontainer'));
}

function SaveBudgetSetting(id) {

    var data = {
        Id: $(id).val(),
        WorkingBudgetName: $('.workingBudgetName').val(),
        FixedBudgetName: $('.fixedBudgetName').val(),
        WhatIfBudgetName: $('.whatifBudgetName').val(),
    }

    MakeServiceCall('PATCH', 'ledgers/' + $(id).val(), data, function (data) {

        if (data.Data) {
            DisplaySuccessMessage('Success', 'Budget Settings saved successfully.');
        }

    }, null);
}

function ValidChartSettingForm() {
    var validform = true;

    // required items
    if (ValidateForm('budgetsettingscontainer') === false) {
        return false;
    }

    return validform;
}

function GetChartSetting() {
    var ledgerid = $('.chartLedgerId').val();

    MakeServiceCall('GET', 'ledgers/' + ledgerid, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {

                $('.hidLedgerId').val(data.Data.Id);
                $('.capitalizeheaders').prop('checked', data.Data.CapitalizeHeaders);
                $('.groupLevels').val(data.Data.AccountGroupLevels);
                $('.accountGroup1Title').val(data.Data.AccountGroup1Title)
                $('.accountGroup2Title').val(data.Data.AccountGroup2Title)
                $('.accountGroup3Title').val(data.Data.AccountGroup3Title)
                $('.accountGroup4Title').val(data.Data.AccountGroup4Title)
                GroupLevelsChange();

            }
        }

    }, null);
}

function SaveChartSetting(id) {
    var data = {
        Id: $(id).val(),
        CapitalizeHeaders: $('.capitalizeheaders').prop('checked'),
        AccountGroupLevels: $('.groupLevels').val(),
        AccountGroup1Title: $('.accountGroup1Title').val(),
        AccountGroup2Title: $('.accountGroup2Title').val(),
        AccountGroup3Title: $('.accountGroup3Title').val(),
        AccountGroup4Title: $('.accountGroup4Title').val(),
    }

    MakeServiceCall('PATCH', 'ledgers/' + $(id).val(), data, function (data) {

        if (data.Data) {
            DisplaySuccessMessage('Success', 'Chart of Accounts Settings saved successfully.');
        }

    }, null);
}

function GroupLevelsChange() {
    var groupLevels = $('.groupLevels').val();

    $('.AccountGroup').hide();
    $('.ag' + groupLevels).show();


}
function LoadChartAccountsSettingsSectionSettings() { }

/// Entities/BusinessUnits Settings
function LoadEntitiesSectionSettings() {

    $('.gridcontainer').empty();

    var entityColumns = [

        { dataField: 'Code', caption: 'Code', sortOrder: 'asc', sortIndex: 0 },
        { dataField: 'Name', caption: 'Description' },
        {
            caption: 'Entity Type', cellTemplate: function (container, options) {
                var entity = 'None';

                switch (options.data.BusinessUnitType) {
                    case 0:
                        entity = "Organization";
                        break;
                    case 1:
                        entity = "Common";
                        break;
                    case 2:
                        entity = "Separate";
                        break;
                }

                $('<label>').text(entity).appendTo(container);
            }
        }
    ];

    LoadGrid('gridcontainer', 'bugridcontainer', entityColumns, 'businessunits/noorganization', 'businessunits', null, 'en-',
        '.entitymodal', '.entitymodal', 250, true, false, false, null);

}
/// End Entities/Business Untis Settings

function LoadFiscalYearSectionSettings() {

    $('.gridcontainer').empty();
    $('.fiscalyearcontainer').remove();
    PopulateFiscalYearNavMenu();

    var container = $('<div>');
    var selectledgergroup = $('<div style="margin-bottom: 20px;">');
    var selectledgername = $('<h2>').text('Ledger: ');

    $('<select>').addClass('LedgerId').appendTo(selectledgername);
    $(selectledgername).appendTo(selectledgergroup);
    $(selectledgergroup).appendTo(container);

    var gridgroup = $('<div>').addClass('twocolumn');

    var fycontainer = $('<fieldset style="display: none;">').addClass('fiscalyearcontainer');
    $('<legend>').text('Fiscal Years').appendTo($(fycontainer));
    $('.gridcontainer').append($(fycontainer));

    $('.fiscalperiodscontainer').remove();
    var fpcontainer = $('<fieldset style="display: none;">').addClass('fiscalperiodscontainer');
    $('<legend>').text('Fiscal Perods').appendTo($(fpcontainer));
    $('.gridcontainer').append($(fpcontainer));

    $(fycontainer).appendTo(gridgroup);
    $(fpcontainer).appendTo(gridgroup);

    $(gridgroup).appendTo(container);
    $(container).appendTo($('.gridcontainer'));

    PopulateDropDown('.LedgerId', 'ledgers/businessunit/' + currentBusinessUnitId, 'Please Select', '', $('.LedgerId').val(), function () {

        LoadFiscalYearGrid();
        $('.fy-LedgerId').val($('.LedgerId').val());

    }, function (element, data) {

        if (data.Data.length == 1) {
            $(element).val(data.Data[0].Id);
            $(element).change();
        }

    });
}

function LoadFiscalYearGrid() {

    var ledgerid = $('.LedgerId').val();

    var columns = [
        { dataField: 'Name', caption: 'Name', sortOrder: 'desc' },
        {
            caption: 'Status', cellTemplate: function (container, options) {

                var status;

                switch (options.data.Status) {
                    case 0:
                        status = "Empty";
                        break;
                    case 1:
                        status = "Open";
                        break;
                    case 2:
                        status = "Closed";
                        break;
                    case 3:
                        status = "Reopened";
                        break;
                    case 4:
                        status = "Locked";
                        break;
                }

                $('<label>').text(status).appendTo(container);
            }
        },
    ];

    LoadGrid('fiscalyearcontainer', 'fiscalyeargrid', columns, 'fiscalyears/ledger/' + ledgerid + '?fields=Id,Name,Status', 'fiscalyears', LoadFiscalPeriods, 'fy-', '.fiscalyearmodal', '.fiscalyearmodal', 250, true, false, false, function (data) {

        if (data.Data.length > 0) {
            $('.fiscalyearcontainer').show();
            $('.fiscalperiodscontainer').hide();
        }
        else {
            $('.fiscalyearcontainer').hide();
        }

    });
}

function LoadFiscalPeriods(info) {
    var fiscalYearId = "";
    if (!info) {
        var dataGrid = $('.fiscalyeargrid').dxDataGrid('instance');
        info = dataGrid.getSelectedRowsData();
        selectedRow = info[0];
        fiscalYearId = info[0].Id;
    } else {
        selectedRow = info.data;
        fiscalYearId = info.data.Id;
    }

    var columns = [
        { dataField: 'PeriodNumber', caption: '' },
        { dataField: 'StartDate', caption: 'Start Date', dataType: 'date' },
        { dataField: 'EndDate', caption: 'End Date', dataType: 'date' },
        {
            caption: 'Status', cellTemplate: function (container, options) {

                var status;

                switch (options.data.Status) {
                    case 0:
                        status = "Open";
                        break;
                    case 1:
                        status = "Closed";
                        break;
                    case 2:
                        status = "Reopened";
                        break;
                }

                $('<label>').text(status).appendTo(container);
            }
        }
    ]

    LoadGrid('fiscalperiodscontainer', 'fiscalperiodgrid', columns, 'fiscalperiods/fiscalyear/' + selectedRow.Id + '?fields=all', 'fiscalperiods', null, 'fp-', '.fiscalperiodmodal', '.fiscalperiodmodal', 250, true, false, false, function (data) {

        $('.fiscalperiodscontainer').show();
        $('.fp-FiscalYearId').val(fiscalYearId)

    });
}

function PopulateFiscalYearNavMenu() {
    var um = $('.utilitymenu')
    um.append('<li class="closefiscalyear"><a href="#">Close Fiscal Year</a></li>');
    um.append('<li class="reopenfiscalyear"><a href="#">Reopen Fiscal Year</a></li>');
    um.append('<li class="reclosefiscalyear"><a href="#">Reclose Fiscal Year</a></li>');
    um.append('<li class="newfiscalyear"><a href="#">Create New Fiscal Year</a></li>');

    var un = $('.utilitynav');
    un.show();
    un.unbind('click');
    un.click(function (e) {

        e.preventDefault();
        e.stopPropagation();

        toolbox = $(this).find('.utilitymenu');
        toolbox.toggle();

    });

    $('.closefiscalyear').unbind('click');
    $('.closefiscalyear').click(function (e) {

        e.preventDefault();
        UpdateFiscalYearModal('Close');

    });

    $('.reopenfiscalyear').unbind('click');
    $('.reopenfiscalyear').click(function (e) {

        e.preventDefault();
        UpdateFiscalYearModal('Reopen');

    });

    $('.reclosefiscalyear').unbind('click');
    $('.reclosefiscalyear').click(function (e) {

        e.preventDefault();
        UpdateFiscalYearModal('Reclose');

    });

    $('.newfiscalyear').unbind('click');
    $('.newfiscalyear').click(function (e) {

        e.preventDefault();
        NewFiscalYearModal();

    });

}

//close, reopen, reclose fiscal year

function UpdateFiscalYearModal(updateOption) {
    var fiscalYearId = '';
    var dataGrid = $('.fiscalyeargrid').dxDataGrid('instance');
    if (!dataGrid) {
        DisplayErrorMessage('Error', 'You must select a ledger first.');
        return;
    }
    info = dataGrid.getSelectedRowsData();
    if (info.length > 0) {
        selectedRow = info[0];
        fiscalYearId = selectedRow.Id;
    }

    $('.selectfiscalyearlabel').html('Fiscal Year to ' + updateOption + ":");
    $('.updatefiscalyearmodalbuttons').show();

    modal = $('.updatefiscalyearmodal').dialog({
        title: updateOption + ' Fiscal Year',
        closeOnEscape: false,
        modal: true,
        width: 500,
        resizable: false
    });

    $('.cancelupdatefiscalyearmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.okupdatefiscalyearmodalbutton').unbind('click');

    $('.okupdatefiscalyearmodalbutton').click(function () {

        UpdateFiscalYearValidate(modal, updateOption)

    });

    $('.updatefiscalyearnotivylabel').html('Please wait... Now loading fiscal years');
    PopulateDropDown('.uf-FiscalYear', 'fiscalyears/ledger/' + $('.LedgerId').val() + '?fields=Id,DisplayName', '', '', fiscalYearId, '', function () {
        UpdateFiscalYearDropDownComplete(modal, updateOption)
    })

}

function UpdateFiscalYearDropDownComplete(element, data) {
    $('.updatefiscalyearnotifylabel').html('');
}

function UpdateFiscalYearValidate(modal, updateOption) {
    if ($('.uf-FiscalYear').val() === '') {
        DisplayErrorMessage('Error', 'You must select a fiscal year first.');
        return;
    }
    UpdateFiscalYearUpdate(modal, updateOption)

}

function UpdateFiscalYearUpdate(modal, updateOption) {

    $('.updatefiscalyearnotifylabel').html('Please wait... Now updating fiscal year');
    $('.updatefiscalyearmodalbuttons').hide();

    var item = null;
    var status = 0;
    switch (updateOption) {
        case 'Close':
            status = 'closed'
            break;
        case 'Reopen':
            status = 'open'
            break;
        case 'Reclose':
            status = 'reclose'
            break;
    }
    item = {
        Id: $('.uf-FiscalYear').val(),
        Status: status
    }

    MakeServiceCall('PATCH', 'fiscalyears/' + $('.uf-FiscalYear').val(), item, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {
                DisplaySuccessMessage('Success', 'Fiscal year update successful.');
                CloseModal(modal);
                $('.fiscalperiodscontainer').hide();
                LoadFiscalYearGrid()
            }

        }

    }, function () {

        $('.updatefiscalyearmodalbuttons').show();
        $('.updatefiscalyearnotifylabel').html(updateOption + ' fiscal year failed');

    }

    );
}

//new fiscal year

function NewFiscalYearModal() {
    var fiscalYearId = '';
    var dataGrid = $('.fiscalyeargrid').dxDataGrid('instance');
    if (!dataGrid) {
        DisplayErrorMessage('Error', 'You must select a ledger first.');
        return;
    }
    info = dataGrid.getSelectedRowsData();
    if (info.length > 0) {
        selectedRow = info[0];
        fiscalYearId = selectedRow.Id;
    }
    InitRequiredLabels('newfiscalyearmodal')
    $('.newfiscalyearmodalbuttons').show();

    modal = $('.newfiscalyearmodal').dialog({
        NewOnEscape: false,
        modal: true,
        width: 500,
        resizable: false
    });

    $('.cancelnewfiscalyearmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.oknewfiscalyearmodalbutton').unbind('click');

    $('.oknewfiscalyearmodalbutton').click(function () {

        NewFiscalYearValidate(modal, option)

    });

    $('.newfiscalyearnotifylabel').html('Please wait... Now loading fiscal years');
    PopulateDropDown('.fn-FromFiscalYear', 'fiscalyears/ledger/' + $('.LedgerId').val() + '?fields=Id,DisplayName', '', '', fiscalYearId, '', function () {
        NewFiscalYearDropDownComplete(modal)
    })

}

function NewFiscalYearDropDownComplete(element, data) {
    $('.newfiscalyearnotifylabel').html('');
}

function NewFiscalYearValidate(modal) {

    if (ValidateForm('newfiscalyearmodal') === false) {
        return
    }

    NewFiscalYearUpdate(modal)

}

function NewFiscalYearUpdate(modal) {
    $('.newfiscalyearmodalbuttons').hide();
    $('.newfiscalyearnotifylabel').html('Please wait... Now creating new fiscal year');

    var item = null;
    item = {
        Name: $('.fn-NewFiscalYear').val(),
        StartDate: $('.fn-StartDate').val(),
        CopyInactiveAccounts: ($('.fn-CopyInactiveAccounts').prop('checked') === true ? 'true' : 'false')
    }

    MakeServiceCall('POST', 'fiscalyears/' + $('.fn-FromFiscalYear').val() + '/copy', item, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {
                DisplaySuccessMessage('Success', 'Fiscal year copy successful.');
                var dt1 = new Date();
                var time1 = dt1.getHours() + ":" + dt1.getMinutes() + ":" + dt1.getSeconds();
                var dt2 = new Date();
                var time2 = dt2.getHours() + ":" + dt2.getMinutes() + ":" + dt2.getSeconds();
                $('.newfiscalyearnotifylabel').html(time1 + ' - ' + time2);

                //CloseModal(modal);
                //LoadFiscalYearGrid()
            }

        }

    }, function () {

        $('.newfiscalyearmodalbuttons').show();
        $('.newfiscalyearnotifylabel').html('Creating new fiscal year failed');

    }

    );

}

/* GENERAL LEDGER FUND SETTINGS */
/* Fund Accounting Section Settings */
function LoadFundAccountingSectionSettings() {

    var fund = '';
    $('.gridcontainer').empty();

    ///* ACCOUNT/REVENUE/EXPENSE ACCORDION */
    var container = $('<div>').addClass('fundsettingscontainer onecolumn').appendTo('.gridcontainer');
    var id = $('<input>').attr('type', 'hidden').addClass('hidLedgerId').appendTo(container);
    var fiscalYearid = $('<input>').attr('type', 'hidden').addClass('hidFiscalId').appendTo(container);
    CreateBasicFieldBlock('Ledger: ', '<select>', 'fundLedgerId', container, true);
    CreateBasicFieldBlock('Fiscal Year: ', '<select>', 'selectfiscalyear', container, true);
    CreateBasicFieldBlock('Fund: ', '<select>', 'selectfund', container, true);
    PopulateDropDown('.fundLedgerId', 'ledgers/businessunit/' + currentBusinessUnitId, 'Please Select', '', null, function () {
        $('.hidLedgerId').val($('.fundLedgerId').val());
        PopulateFiscalYears();

    }, null);

    $('.selectfiscalyear').parent().hide();
    $('.selectfund').parent().hide();

    var accordions = $('<div>').addClass('accordions');
    var accountrevenuegroup = $('<div>').addClass('accountrevenuecontainer');
    var header = $('<h1>').text('GL Account Settings ').appendTo($(accordions));

    /* FUND BALANCE ACCOUNT */
    var selectfundbalanceaccountgroup = $('<div>');
    CreateBasicFieldBlock('Fund Balance Account: ', '<div>', 'selectfundbalanceaccount', selectfundbalanceaccountgroup, true);
    $(selectfundbalanceaccountgroup).appendTo(accountrevenuegroup);
    $(accountrevenuegroup).append('<br />').appendTo(accordions);

    /* CLOSING REVENUE ACCOUNT */
    var selectclosingrevenueaccountgroup = $('<div>');
    CreateBasicFieldBlock('Closing Revenue Account: ', '<div>', 'selectclosingrevenueaccount', selectclosingrevenueaccountgroup, true);
    $(selectclosingrevenueaccountgroup).appendTo(accountrevenuegroup);
    $(accountrevenuegroup).append('<br />').appendTo(accordions);

    /* CLOSING EXPENSE ACCOUNT */
    var selectclosingexpenseaccountgroup = $('<div>');
    CreateBasicFieldBlock('Closing Expense Account: ', '<div>', 'selectclosingexpenseaccount', selectclosingexpenseaccountgroup, true);
    $(selectclosingexpenseaccountgroup).appendTo(accountrevenuegroup);
    $(accountrevenuegroup).append('<br />').appendTo(accordions);

    /* SAVE & CANCEL */
    var errorgroup = $('<div>').addClass('fieldblock');
    $('<label>').text('').addClass('validateerror fundsettingerror').append('<br />').appendTo(errorgroup);
    $(errorgroup).append('<br />').appendTo(accountrevenuegroup).appendTo(container);

    CreateSaveAndCancelButtons('SaveFundSetting', function (e) {

        fundid = $('.selectfund').val();
        var item = null;
        fiscalyearid = $('.selectfiscalyear').val();
        var item = {
            FundBalanceAccountId: $('.selectfundbalanceaccount> .hidaccountid').val(),
            ClosingRevenueAccountId: $('.selectclosingrevenueaccount > .hidaccountid').val(),
            ClosingExpenseAccountId: $('.selectclosingexpenseaccount > .hidaccountid').val(),
        };

        MakeServiceCall('PATCH', 'fund/' + $('.selectfund').val(), item, function () {
            LoadFundGLAccountSelector($('.selectfiscalyear').val(), $('.fundLedgerId').val(), $('.selectfund').val())
            DisplaySuccessMessage('Success', 'Setting saved successfully.');
            LoadFundGLAccountSelector($('.selectfiscalyear').val(), $('.fundLedgerId').val(), $('.selectfund').val())
        }, null);
    },

        'cancel', function (e) {
            e.preventDefault();
            LoadFundGLAccountSelector($('.selectfiscalyear').val(), $('.fundLedgerId').val(), $('.selectfund').val())
        },

        accountrevenuegroup);


    /* BUSINESS UNIT & FUND DUE ACCORDION */
    var businessunitdue = $('<div>').addClass('businessunitduecontainer');
    var funddue = $('<div>').addClass('fundduecontainer');

    var header = $('<h1>').text('Business Unit Due From/Due to Accounts ').appendTo($(accordions));
    $(businessunitdue).appendTo($(accordions));

    var header = $('<h1>').text('Fund Due From/Due to Accounts ').appendTo($(accordions));
    $(funddue).appendTo($(accordions));

    $(accordions).appendTo($('.gridcontainer'));

    LoadAccordions();

    $(accordions).hide();
    $('.accordion-buttons').hide();

}

function PopulateFiscalYears() {
    var ledgerid = $('.hidLedgerId').val();

    MakeServiceCall('GET', 'ledgers/' + ledgerid, null, function (data) {
        if (data.Data) {
            if (data.IsSuccessful) {
                $('.selectfiscalyear').parent().show();
                $('.hidLedgerId').val(data.Data.Id);
                PopulateDropDown('.selectfiscalyear', 'fiscalyears/ledger/' + $('.hidLedgerId').val(), 'Please Select', '', data.Data.DefaultFiscalYearId, function () {
                    $('.hidFiscalId').val($('.selectfiscalyear').val());
                    var fiscalId = $('.hidFiscalId').val();
                    var ledgerId = $('.hidLedgerId').val();
                    if ($('.selectfiscalyear').val().length > 0) {
                        PopulateFundFromFiscalYear(fiscalId, ledgerId);
                        PopulateFundBusinessFromFiscalYear(($('.selectfiscalyear').val(), $('.hidLedgerId').val()));
                    }
                },
                    function () {
                        $('.hidFiscalId').val($('.selectfiscalyear').val());
                        var fiscalId = $('.hidFiscalId').val();
                        var ledgerId = $('.hidLedgerId').val();
                        $('.gridcontainer .accordions').show();
                        $('.gridcontainer .accordion-buttons').show();
                        LoadFundGLAccountSelector($('.selectfiscalyear').val(), ledgerid, $('.selectfund').val());
                        PopulateFundBusinessFromFiscalYear(($('.selectfiscalyear').val(), $('.hidLedgerId').val()));
                        PopulateFundFromFiscalYear(fiscalId, ledgerId);
                    });
            }
        }
    }, null);
}

/* POPULATING FUND FROM FISCAL YEAR GRID */
function PopulateFundFromFiscalYear(fiscalyear, ledgerid, fundid) {

    PopulateDropDown('.selectfund', 'fund/' + fiscalyear + '/fiscalyear', 'Please Select', '', '',
        function () {

            LoadFundGLAccountSelector($('.selectfiscalyear').val(), $('.hidLedgerId').val(), $('.selectfund').val());
            PopulateFundBusinessFromFiscalYear(($('.selectfiscalyear').val(), $('.hidLedgerId').val()));
            PopulateFundDueFromFund($('.selectfund').val());

        },
        function (e, data) {
            if (data.Data.length <= 1) {
                $('.selectfund').parent().hide();
                $('.fundduecontainer').hide();
                $('.fundduecontainer').parent().find("#" + $('.fundduecontainer').attr("aria-labelledby")).hide()
                LoadFundGLAccountSelector($('.selectfiscalyear').val(), $('.hidLedgerId').val(), $('.selectfund').val());
            }
            else {
                $('.selectfund').parent().show();
                $('.fundduecontainer').parent().find("#" + $('.fundduecontainer').attr("aria-labelledby")).show()
            };
            if (data.Data.length > 0) {
                $('.selectfund option:eq(1)').attr('selected', 'selected');
                LoadFundGLAccountSelector($('.selectfiscalyear').val(), $('.hidLedgerId').val(), $('.selectfund').val());
                PopulateFundDueFromFund($('.selectfund').val());
            };
        }
    );
}
/*POPULATE FUND ACCOUNT SELECTOR*/

function LoadFundGLAccountSelector(fiscalyearid, ledgerid, fundid) {

    $('.selectfundbalanceaccount').empty();
    $('.selectclosingrevenueaccount').empty();
    $('.selectclosingexpenseaccount').empty();

    GLAccountSelector($('.selectfundbalanceaccount'), ledgerid, fiscalyearid);
    GLAccountSelector($('.selectclosingrevenueaccount'), ledgerid, fiscalyearid);
    GLAccountSelector($('.selectclosingexpenseaccount'), ledgerid, fiscalyearid);
    if (fundid != undefined && fundid.length > 0) {
        MakeServiceCall('GET', 'fund/' + fundid, null, function (data) {
            var fund = data.Data;
            LoadSelectedAccount($('.selectfundbalanceaccount'), fund.FundBalanceAccountId);
            LoadSelectedAccount($('.selectclosingrevenueaccount'), fund.ClosingRevenueAccountId);
            LoadSelectedAccount($('.selectclosingexpenseaccount'), fund.ClosingExpenseAccountId);
        });
    }
    else {
        LoadSelectedAccount($('.selectfundbalanceaccount'), "");
        LoadSelectedAccount($('.selectclosingrevenueaccount'), "");
        LoadSelectedAccount($('.selectclosingexpenseaccount'), "");
    }
}

/* POPULATING FUND DUE FROM FUND */
function PopulateFundDueFromFund(fundid) {

    var fundduecolumns = [
        { dataField: 'DisplayName', caption: 'Fund' },
        { dataField: 'FromLedgerAccount.AccountNumber', caption: 'Due From Account' },
        { dataField: 'FromLedgerAccount.Name', caption: 'Description' },
        { dataField: 'ToLedgerAccount.AccountNumber', caption: 'Due To Account' },
        { dataField: 'ToLedgerAccount.Name', caption: 'Description' },
        {
            width: '100px',
            alignment: 'center',
            allowResizing: false,
            cellTemplate: function (container, options) {

                $('<a/>')
                    .addClass('actionbuttons')
                    .addClass('editbutton')
                    .attr('title', 'Edit')
                    .click(function (e) {
                        e.preventDefault();

                        EditFundDue(options.data);
                    })
                    .appendTo(container);
            }
        }
    ];

    if (!(fundid == '' || fundid == null)) {
        CustomLoadGrid('fundduegrid', '.fundduecontainer', fundduecolumns, 'funds/' + fundid + '/fundfromto', null, null, null, null);
    }
}

/* POPULATING BUSINESS UNIT */
function PopulateFundBusinessFromFiscalYear() {

    var businessduecolumns = [
        { dataField: 'DisplayName', caption: 'Business Unit' },
        { dataField: 'FromLedgerAccount.AccountNumber', caption: 'Due From Account' },
        { dataField: 'FromLedgerAccount.Name', caption: 'Description' },
        { dataField: 'ToLedgerAccount.AccountNumber', caption: 'Due To Account' },
        { dataField: 'ToLedgerAccount.Name', caption: 'Description' },
        {
            width: '100px',
            alignment: 'center',
            allowResizing: false,
            cellTemplate: function (container, options) {

                $('<a/>')
                    .addClass('actionbuttons')
                    .addClass('editbutton')
                    .attr('title', 'Edit')
                    .click(function (e) {
                        e.preventDefault();

                        EditBusinessUnit(options.data);
                    })
                    .appendTo(container);
            }
        }
    ];

    CustomLoadGrid('businessunitduegrid', '.businessunitduecontainer', businessduecolumns, 'fiscalyears/' + $('.selectfiscalyear').val() + '/businessunitfromto', null, null, null, null);
}

function LoadFundSettings(fundid) {

    MakeServiceCall('GET', 'fund/' + fundid, null, function (data) {
        $('.selectfund').val(data.Data.fundid);
        $('.selectfundbalanceaccount').val(data.Data.FundBalanceAccountId);
        $('.selectclosingrevenueaccount').val(data.Data.ClosingRevenueAccountId);
        $('.selectclosingexpenseaccount').val(data.Data.ClosingExpenseAccountId);
    }, null);
}
function EditBusinessUnit(buFromToInfo) {

    if (buFromToInfo.Id != "00000000-0000-0000-0000-000000000000") {

        MakeServiceCall('GET', 'businessunitfromtos/' + buFromToInfo.Id, null, function (data) {
            modal = $('.businessunitduemodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 900,
                resizable: false,
                beforeClose: function (e) {
                    $('.bus-FromLedgerAccount').empty();
                    $('.bus-ToLedgerAccount').empty();
                }
            });

            GLAccountSelector($(modal).find('.bus-FromLedgerAccount'), $('.fundLedgerId').val(), $('.selectfiscalyear').val());
            GLAccountSelector($(modal).find('.bus-ToLedgerAccount'), $('.fundLedgerId').val(), $('.selectfiscalyear').val());

            LoadSelectedAccount($(modal).find('.bus-FromLedgerAccount'), data.Data.FromAccountId);
            LoadSelectedAccount($(modal).find('.bus-ToLedgerAccount'), data.Data.ToAccountId);

            $('.cancelbusinessunitduedetailsmodal').unbind('click');
            $('.cancelbusinessunitduedetailsmodal').click(function (e) {

                e.preventDefault();
                CloseModal(modal);

                $('.bus-FromLedgerAccount').empty();
                $('.bus-ToLedgerAccount').empty();
            });

            $('.Savebusinessunitduedetails').unbind('click');

            $('.Savebusinessunitduedetails').click(function () {
                var item = {
                    FromAccountId: $(modal).find('.bus-FromLedgerAccount > .hidaccountid').val(),
                    ToAccountId: $(modal).find('.bus-ToLedgerAccount > .hidaccountid').val()
                }

                MakeServiceCall('PATCH', 'businessunitfromtos/' + buFromToInfo.Id, item, function (data) {
                    DisplaySuccessMessage('Success', 'Business Unit saved successfully.');
                    CloseModal(modal);
                    PopulateFundBusinessFromFiscalYear($('.selectfiscalyear').val(), $('.fundLedgerId').val());

                    $('.bus-FromLedgerAccount').empty();
                    $('.bus-ToLedgerAccount').empty();
                }, function (xhr, status, err) {
                    DisplayErrorMessage('Error', 'An error occurred during saving the Business Due.');
                });
            });
        }, null);
    }
    else {
        modal = $('.businessunitduemodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 900,
            resizable: false,
            beforeClose: function (e) {
                $('.bus-FromLedgerAccount').empty();
                $('.bus-ToLedgerAccount').empty();
            }
        });

        GLAccountSelector($(modal).find('.bus-FromLedgerAccount'), $('.fundLedgerId').val(), $('.selectfiscalyear').val());
        GLAccountSelector($(modal).find('.bus-ToLedgerAccount'), $('.fundLedgerId').val(), $('.selectfiscalyear').val());

        $('.cancelbusinessunitduedetailsmodal').unbind('click');
        $('.cancelbusinessunitduedetailsmodal').click(function (e) {
            e.preventDefault();
            CloseModal(modal);

            $('.bus-FromLedgerAccount').empty();
            $('.bus-ToLedgerAccount').empty();
        });

        $('.Savebusinessunitduedetails').unbind('click');
        $('.Savebusinessunitduedetails').click(function () {

            var item = {
                BusinessUnitId: buFromToInfo.BusinessUnitId,
                FiscalYearId: buFromToInfo.FiscalYearId,
                OffsettingBusinessUnitId: buFromToInfo.OffsettingBusinessUnitId,
                FromAccountId: $(modal).find('.bus-FromLedgerAccount > .hidaccountid').val(),
                ToAccountId: $(modal).find('.bus-ToLedgerAccount > .hidaccountid').val()
            }

            MakeServiceCall('POST', 'businessunitfromtos/', item, function (data) {
                DisplaySuccessMessage('Success', 'Business Unit saved successfully.');
                CloseModal(modal);
                PopulateFundBusinessFromFiscalYear($('.selectfiscalyear').val(), $('.fundLedgerId').val());

                $('.bus-FromLedgerAccount').empty();
                $('.bus-ToLedgerAccount').empty();
            }, function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the Business Unit From To.');
            });
        });
    }
}

function EditFundDue(fundDueInfo) {

    if (fundDueInfo.Id != "00000000-0000-0000-0000-000000000000") {
        MakeServiceCall('GET', 'fundfromtos/' + fundDueInfo.Id, null, function (data) {
            modal = $('.fundduemodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 900,
                resizable: false,
                beforeClose: function (e) {
                    $('.fn-DueFromAccount').empty();
                    $('.fn-DueToAccount').empty();
                }
            });

            GLAccountSelector($('.fn-DueFromAccount'), $('.fundLedgerId').val(), $('.selectfiscalyear').val());
            GLAccountSelector($('.fn-DueToAccount'), $('.fundLedgerId').val(), $('.selectfiscalyear').val());

            LoadSelectedAccount($('.fn-DueFromAccount'), data.Data.FromAccountId);
            LoadSelectedAccount($('.fn-DueToAccount'), data.Data.ToAccountId);

            $('.cancelfundduemodal').unbind('click');
            $('.cancelfundduemodal').click(function (e) {
                e.preventDefault();
                CloseModal(modal);

                $('.fn-DueFromAccount').empty();
                $('.fn-DueToAccount').empty();
            });

            $('.Savefundduedetails').unbind('click');

            $('.Savefundduedetails').click(function () {
                var item = {
                    FromAccountId: $(modal).find('.fn-DueFromAccount > .hidaccountid').val(),
                    ToAccountId: $(modal).find('.fn-DueToAccount > .hidaccountid').val()
                }

                MakeServiceCall('PATCH', 'fundfromtos/' + fundDueInfo.Id, item, function (data) {
                    DisplaySuccessMessage('Success', 'Fund due saved successfully.');
                    CloseModal(modal);
                    PopulateFundDueFromFund($('.selectfund').val());

                    $('.fn-DueFromAccount').empty();
                    $('.fn-DueToAccount').empty();
                }, function (xhr, status, err) {
                    DisplayErrorMessage('Error', 'An error occurred during saving the Fund due.');
                });
            });
        }, null);
    }
    else {
        modal = $('.fundduemodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 900,
            resizable: false,
            beforeClose: function (e) {
                $('.fn-DueFromAccount').empty();
                $('.fn-DueToAccount').empty();
            }
        });

        GLAccountSelector($('.fn-DueFromAccount'), $('.fundLedgerId').val(), $('.selectfiscalyear').val());
        GLAccountSelector($('.fn-DueToAccount'), $('.fundLedgerId').val(), $('.selectfiscalyear').val());

        $('.cancelfundduemodal').unbind('click');
        $('.cancelfundduemodal').click(function (e) {
            e.preventDefault();
            CloseModal(modal);

            $('.fn-DueFromAccount').empty();
            $('.fn-DueToAccount').empty();
        });

        $('.Savefundduedetails').unbind('click');

        $('.Savefundduedetails').click(function () {
            var item = {
                FundId: fundDueInfo.FundId,
                FiscalYearId: fundDueInfo.FiscalYearId,
                OffsettingFundId: fundDueInfo.OffsettingFundId,
                FromAccountId: $(modal).find('.fn-DueFromAccount > .hidaccountid').val(),
                ToAccountId: $(modal).find('.fn-DueToAccount > .hidaccountid').val()
            }

            MakeServiceCall('POST', 'fundfromtos/', item, function (data) {
                DisplaySuccessMessage('Success', 'Fund due saved successfully.');
                CloseModal(modal);
                PopulateFundDueFromFund($('.selectfund').val());

                $('.fn-DueFromAccount').empty();
                $('.fn-DueToAccount').empty();
            }, function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the Fund due.');
            });
        });
    }
}
/*End Fund Accounting Section Settings */

function LoadGLFormatSectionSettings() {

    $('.gridcontainer').empty();

    var container = $('<div>').addClass('onecolumn');

    var glaccountformat = '';
    CreateBasicFieldBlock('Ledger: ', '<select>', 'glf-ledgerselect', container, true);

    var glformat = $('<div>').addClass('glformatcontainer').css('display', 'block');
    $(glformat).appendTo($(container));
    $(container).appendTo($('.gridcontainer'));

    PopulateDropDown('.glf-ledgerselect', 'ledgers/businessunit/' + currentBusinessUnitId, '', '', $('.LedgerId').val(), function () {

        var ledgerId = $('.glf-ledgerselect').val();
        var canDeleteSegmentLevels = false;
        var editModalClass = '';

        var modalLinkClass = 'glformat-newmodallink';
        $('.' + modalLinkClass).remove();

        $('.glformat-LedgerId').val(ledgerId);

        MakeServiceCall('GET', 'ledgers/' + ledgerId, null, function (data) {

            if (data && data.Data && data.IsSuccessful) {

                glaccountformat = data.Data.DisplayFormat;

                if (data.Data.HasLedgerAccounts === false) {

                    canDeleteSegmentLevels = true;
                    editModalClass = '.glformatmodal';
                    NewModalLink('.glformatcontainer', 'segmentlevels', 'glformat-', editModalClass, 250, '');
                }
                else {
                    canDeleteSegmentLevels = false;
                    editModalClass = '';
                }

                var glformatcolumns = [

                    { dataField: 'Level', caption: 'Level' },
                    {
                        caption: 'Type', cellTemplate: function (container, options) {
                            var type = "None";

                            switch (options.data.Type) {
                                case 1:
                                    type = "Fund";
                                    break;
                                case 2:
                                    type = "Account";
                                    break;
                            }

                            $('<label>').text(type).appendTo(container);
                        }
                    },
                    {
                        caption: 'Format', cellTemplate: function (container, options) {
                            var format;

                            switch (options.data.Format) {
                                case 0:
                                    format = "Both";
                                    break;
                                case 1:
                                    format = "Numeric";
                                    break;
                                case 2:
                                    format = "Alpha";
                                    break;
                            }

                            $('<label>').text(format).appendTo(container);
                        }
                    },
                    { dataField: 'Length', caption: 'Length' },
                    { dataField: 'IsLinked', caption: 'Linked' },
                    { dataField: 'IsCommon', caption: 'Common' },
                    { dataField: 'Name', caption: 'Name' },
                    { dataField: 'Abbreviation', caption: 'Abbreviation' },
                    {
                        caption: 'Separator', cellTemplate: function (container, options) {
                            var separator = 'None';
                            options.data.Separator.replace(' ', '(Space)');

                            if (options.data.Separator == '') {
                                options.data.Separator = 'None';
                            }

                            $('<label>').text(options.data.Separator).appendTo(container);
                        }
                    },
                    {
                        caption: 'Sort Order', cellTemplate: function (container, options) {
                            var order = 'None';

                            switch (options.data.SortOrder) {
                                case 0:
                                    order = "Ascending";
                                    break;
                                case 1:
                                    order = "Unaffiliated";
                                    break;
                            }

                            $('<label>').text(order).appendTo(container);
                        }
                    }
                ];

                LoadGrid('.glformatcontainer', 'glformatgrid', glformatcolumns, 'segmentlevels/ledger/' + ledgerId, 'segmentlevels', null, 'glformat-',
                    editModalClass, editModalClass, 250, canDeleteSegmentLevels, false, false, function () {

                        MakeServiceCall('GET', 'ledgers/' + ledgerId, null, function (data) {

                            if (data && data.Data && data.IsSuccessful) {

                                glaccountformat = data.Data.DisplayFormat;

                            }
                            else {
                                glaccountformat = '';
                            }
                            $('.AccountFormat').remove();
                            $('<span>').addClass('AccountFormat').text('Example: ' + glaccountformat).appendTo($('.glformatcontainer'));
                        }, null);
                    });
            }
        }, null);
    });
}

function LoadJournalSectionSettings() {

    $('.gridcontainer').empty();
}

function LoadUtilitiesSectionSettings() {

    $('.gridcontainer').empty();
}