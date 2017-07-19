
var SAVE_ROUTE = 'journals/';
var DueTo = { 0: 'None', 1: 'Due From', 2: 'Due To' }

//var fiscalYearId = 'B80B38CF-108C-4D9D-BE49-DFFD6502449C'      // testing
//var ledgerId = '52822462-5041-46CB-9883-ECB1EF8F46F0'          // testing
//var journalId = 'E2A35D00-2452-11E7-833C-00C0DD01025C'         // testing
var fiscalYearId = null;
var ledgerId = null;
var journalId = null;
var editMode = ''
var nextLineNumber = 1;

// 0: One-Time, 1: Recurring, 2: Template
var journalType = null;

$(document).ready(function () {

    CreateEditControls();

    SetupEditControls();

    $('.reversejournal').click(function () {

        if ($('.reversejournal').prop('checked') == true) {
            $('.reverseondatecontainer').show();
        }
        else {
            $('.reverseondatecontainer').hide();
        }

    });

    if (sessionStorage.getItem('JOURNAL_ID')) {
        $('.hidjournalid').val(sessionStorage.getItem('JOURNAL_ID'));
        sessionStorage.removeItem('JOURNAL_ID');
    }

    journalId = $('.hidjournalid').val();

    if (journalId && journalId.length > 0) {
        LoadJournalDetail();
    }
    else {
        editing = false;
        $('.journalbody .editbutton').click();
    }

    if (sessionStorage.getItem('JOURNAL_TYPE')) {
        $('.hidjournaltype').val(sessionStorage.getItem('JOURNAL_TYPE'));
        sessionStorage.removeItem('JOURNAL_TYPE');
    }

    journalType = $('.hidjournaltype').val();

    if (journalType && journalType.lenght > 0) {
        journalType = parseInt(journalType);
        SetupJournalTypeDisplay(null);
    }

    $('.newjournallinemodallink').click(function (e) {

        e.preventDefault();

        JournalLineModal(null, true);

    });

});

function LoadJournalDetail() {

    MakeServiceCall('GET', 'journals/' + journalId, null, function (data) {

        if (data.Data && data.IsSuccessful) {

            currentEntity = data.Data;

            fiscalYearId = currentEntity.FiscalYearId;
            ledgerId = (currentEntity.FiscalYear) ? currentEntity.FiscalYear.LedgerId : null;
            journalType = data.Data.JournalType;

            $('.journaltype').html(data.Data.JournalDescription);
            $('.StatusDescription').html(FormatDateTimeStrings(data.Data.StatusDescription));            
            $('.TransactionDate').val(FormatJSONDate(data.Data.TransactionDate));
            $('.Comment').val(data.Data.Comment);
            $('.CreatedBy').html(data.Data.CreatedBy);
            $('.CreatedOn').html(FormatDateTime(data.Data.CreatedOn));
            $('.LastChangedBy').html(data.Data.LastChangedBy);
            $('.LastChangedOn').html(FormatDateTime(data.Data.LastChangedOn));

            $('.newjournallinemodallink').show();

            SetupJournalTypeDisplay(data);

            LoadJournalLineGrid(data);

            LoadDatePickers();
        }

    }, null)

}

function SetupJournalTypeDisplay(data) {

    var currentRecurringType = null;
    
    if (data) {
        currentRecurringType = data.Data.RecurringType;
    }

    PopulateDropDown('.RecurringType', 'journals/recurringtypes', null, null, currentRecurringType, null, null);

    switch (journalType) {
        case 0:

            if (data) {
                if (data.Data.ReverseOnDate === null) {
                    $('.reverseondatecontainer').hide();
                }
                else {
                    $('.reverseondatecontainer').show();
                    $('.reversejournal').prop('checked', true);
                    $('.ReverseOnDate').val(FormatJSONDate(data.Data.ReverseOnDate));
                }
            }
            
            $('.RecurringType').val('0');

            break;
        case 1:

            $('.expirationselection').show()
            
            break;
        case 2:
            break;
    }

}

function LoadJournalLineGrid() {

    var columns = [
        { dataField: 'LineNumber', caption: 'Line #', sortOrder: 'asc', sortIndex: 0},
        { dataField: 'LedgerAccount.AccountNumber', caption: 'GL Account', alignment: 'left' },
        { dataField: 'LedgerAccount.Name', caption: 'Description' },
        {
            name: 'debit', dataField: 'Amount', caption: 'Debit', dataType: 'number', format: 'currency', precision: 2, alignment: 'right', calculateCellValue: function (data) {
                return (data.Amount >= 0 ? data.Amount : 0);
            }
        },
        {
            name: 'credit', dataField: 'Amount', caption: 'Credit', dataType: 'number', format: 'currency', precision: 2, alignment: 'right', calculateCellValue: function (data) {
                return (data.Amount < 0 ? (data.Amount * -1): 0);
            }
        },
        {
            dataField: 'DueToMode', caption: 'Due', calculateCellValue: function (data) {
                return DueTo[data.DueToMode];
            }
        },
        { dataField: 'SourceFund.DisplayName', caption: 'Fund' },
        { dataField: 'SourceBusinessUnit.DisplayName', caption: '' },
    ];

    $('.journallinegridcontainer').show()

    CustomLoadGrid('journallinegrid', '.journallinegridcontainer', columns, 'journalline/journal/' + currentEntity.Id, null, JournalLineModal, DeleteJournalLine, function (data) {
        $('.journallinegrid').dxDataGrid({
            summary: {
                totalItems: [
                    { column: 'debit', summaryType: 'sum', valueFormat: 'currency', precision: 2, displayFormat: '{0}' },
                    { column: 'credit', summaryType: 'sum', valueFormat: 'currency', precision: 2, displayFormat: '{0}' }
                ]
            }
        });

        nextLineNumber = GetMaxLineNumber(data.Data) + 1;
    });

}

function GetMaxLineNumber(arr) {

    var max;

    for (var i = 0; i < arr.length; i++) {

        if (!max || parseInt(arr[i]['LineNumber']) > parseInt(max)) {
            max = arr[i]['LineNumber'];
        }
        
    }

    return max;
}

function JournalLineModal(id, isNew) {
    
    var modal = $('.journallinemodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 900,
        resizable: false,
        beforeClose: function (e) {
            $('.journallineledgeraccountid').empty();
        }
    });

    MaskFields();

    $(modal).find('.journallineledgeraccountid').empty();
    GLAccountSelector($(modal).find('.journallineledgeraccountid'), ledgerId, fiscalYearId);
    
    if (!isNew) {
        LoadJournalLine(id);
    }
    else {
        PopulateDropDown($(modal).find('.SourceFundId'), 'fund/' + fiscalYearId + '/fiscalyear', null, null, null, null, null);
        PopulateDropDown($(modal).find('.DueToMode'), 'journals/duetomodes', null, null, null, null, null);
    }
    
    $('.canceljournallinemodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.savejournalline').unbind('click');

    $('.savejournalline').click(function () {

        var item = GetJournalLineToSave();
        var method = 'POST';
        var route = 'journalline';

        if (id) {
            method = 'PATCH';
            route = 'journalline/' + id;
        }
        else {
            item.LineNumber = nextLineNumber;
        }

        MakeServiceCall(method, route, item, function (data) {

            DisplaySuccessMessage('Success', 'Journal Line saved successfully.');

            CloseModal(modal);

            LoadJournalLineGrid();

        }, function (xhr, status, err) {

            DisplayErrorMessage('Error', 'An error occurred during saving the Journal Line.');
        });

    });
}

function GetJournalLineToSave() {

    var amount = $('.Amount').val();

    if ($('.creditoption').prop('checked')) {

        if (amount.length > 0 && amount.substring(0, 1) != '-') {
            amount = '-' + amount;
        }

    }

    var rawitem = {

        JournalId: currentEntity.Id,
        SourceBusinessUnitId: currentBusinessUnitId,
        LedgerAccountId: $('.hidledgeraccountid').val(),
        Amount: amount,
        Comment: $('.JournalLineComment').text(),
        DueToMode: $('.DueToMode').val(),
        SourceFundId: $('.SourceFundId').val()

    };

    return rawitem;

}

function LoadJournalLine(id) {

    MakeServiceCall('GET', 'journalline/' + id, null, function (data) {

        $('.hidjournallineid').val(id);

        $('.hidledgeraccountid').val(data.Data.LedgerAccountId);
        $('.hidaccountnumber').val(data.Data.LedgerAccount.AccountNumber);
        $('.accountnumber').val(data.Data.LedgerAccount.AccountNumber);

        $('.Amount').val(data.Data.Amount);
        $('.JournalLineComment').text(data.Data.Comment);

        PopulateDropDown($('.DueToMode'), 'journals/duetomodes', null, null, data.Data.DueToMode, null, null);
        PopulateDropDown($('.SourceFundId'), 'fund/' + fiscalYearId + '/fiscalyear', null, null, data.Data.SourceFundId, null, null);

        if (data.Data.Amount && data.Data.Amount < 0) {
            $('.creditoption').prop('checked', true);
            $('.debitoption').prop('checked', false);
        } else {
            $('.creditoption').prop('checked', false);
            $('.debitoption').prop('checked', true);
        }
        
    }, function (xhr, status, err) {

        DisplayErrorMessage('Error', 'An error occurred during loading the Journal Line.');

    });

}

function DeleteJournalLine(id) {
    
    MakeServiceCall('DELETE', 'journalline/' + id, null, function (data) {
        
        DisplaySuccessMessage('Success', 'Journal Line deleted successfully.');
            
        CloseModal(modal);

        LoadJournalLineGrid();

    }, null);
    
}

function RefreshEntity() {

    LoadJournalDetail();

}

