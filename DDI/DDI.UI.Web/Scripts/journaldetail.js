
var DueTo = { 0: 'None', 1: 'Due From', 2: 'Due To' }

var fiscalYearId = 'B80B38CF-108C-4D9D-BE49-DFFD6502449C'      // testing
var ledgerId = '52822462-5041-46CB-9883-ECB1EF8F46F0'          // testing
var journalId = 'E2A35D00-2452-11E7-833C-00C0DD01025C'         // testing
var businessUnitId = 'D63D404A-1BDD-40E4-AC19-B9354BD11D16'    // testing
var editMode = ''
var journalContainer = '.journalbody'

// 0: One-Time, 1: Recurring, 2: Template
var journalType = 0;

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

    if (journalId) {
        LoadJournalDetail();
    }

});

function LoadJournalDetail() {

    MakeServiceCall('GET', 'journals/' + journalId, null, function (data) {

        if (data.Data && data.IsSuccessful) {

            currentEntity = data.Data;

            $('.journaltype').html(data.Data.JournalDescription);
            $('.StatusDescription').html(data.Data.StatusDescription);
            $('.TransactionDate').val(data.Data.TransactionDate);
            $('.Comment').val(data.Data.Comment);

            switch (data.Data.JournalType) {
                case 0:
                    if (data.Data.ReverseOnDate === null) {
                        $('.reverseondatecontainer').hide();
                    }
                    else {
                        $('.reverseondatecontainer').show();
                        $('.ReverseOnDate').val(data.Data.ReverseOnDate);
                    }
                    break;
                case 1:
                    $('.expirationselection').show()
                    PopulateDropDown('.RecurringType', 'journals/recurringtypes', null, null, data.Data.RecurringType, null, null);
                    break;
                case 2:
                    break;
            }

            $('.CreatedBy').html(data.Data.CreatedBy);
            $('.CreatedOn').html(data.Data.CreatedOn);
            $('.LastChangedBy').html(data.Data.LastChangedBy);
            $('.LastChangedOn').html(data.Data.LastChangedOn);

            LoadJournalLineGrid(data);
        }

    }, null)

}

function LoadJournalLineGrid(data) {

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
        { dataField: 'SourceFund', caption: 'Fund' },
        { dataField: 'SourceBusinessUnit', caption: '' },
    ];

    $('.journallinegridcontainer').show()

    //LoadGrid('journallinegridcontainer', 'journallinegrid', columns, 'getroute', 'saveroute', null, null, 'journallinemodal', 'journallinemodal', 750, true, false, false, function (data) {
    //    $('.journallinegrid').dxDataGrid({
    //        summary: {
    //            totalItems: [
    //                { column: 'debit', summaryType: 'sum', valueFormat: 'currency', precision: 2, displayFormat: '{0}' },
    //                { column: 'credit', summaryType: 'sum', valueFormat: 'currency', precision: 2, displayFormat: '{0}' }
    //            ]
    //        }
    //    });
    //});

    LoadGridWithData('journallinegrid', '.journallinegridcontainer', columns, '', '', EditJournalLineModal, DeleteJournalLine, data.Data.JournalLines, function (data) {
        $('.journallinegrid').dxDataGrid({
            summary: {
                totalItems: [
                    { column: 'debit', summaryType: 'sum', valueFormat: 'currency', precision: 2, displayFormat: '{0}'},
                    { column: 'credit', summaryType: 'sum', valueFormat: 'currency', precision: 2, displayFormat: '{0}'}
                ]
            }
        });
    });

}

function EditJournalLineModal(id) {

    if (editMode === 'display') {
        return
    }

    var modal = $('.journallinemodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 900,
        resizable: false,
        beforeClose: function (e) {
            $('.journallineledgeraccountid').empty();
        }
    });

    LoadJournalLine(id);

    $('.canceljournallinemodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.savejournalline').unbind('click');

    $('.savejournalline').click(function () {

        var topicsavelist = GetNoteTopicsToSave();

        var item = GetJournalLineToSave();

        MakeServiceCall('PATCH', 'JournalLine/' + id, item, function (data) {

            DisplaySuccessMessage('Success', 'Linked Account saved successfully.');

            CloseModal(modal);

            LoadJournalLineGrid();

        }, function (xhr, status, err) {

            DisplayErrorMessage('Error', 'An error occurred during saving the Linked Account.');
        });

    });

    $('.savenewjournalline').unbind('click');

    $('.savenewjournalline').click(function () {

        var topicsavelist = GetNoteTopicsToSave();

        var item = GetJournalLineToSave();

        MakeServiceCall('PATCH', 'JournalLine/' + id, item, function (data) {

            DisplaySuccessMessage('Success', 'Journal Line saved successfully.');

            CloseModal(modal);

            LoadJournalLineGrid();

        }, function (xhr, status, err) {

            DisplayErrorMessage('Error', 'An error occurred during saving the Journal Line.');
        });

    });
}

function LoadJournalLine(id) {

    MakeServiceCall('GET', 'JournalLine/' + id, null, function (data) {

        $('.LinkedAccountType').val(data.Data.LinkedAccountType),
            $('.LinkedAccountInd').val((data.Data.LinkedAccountNumber > 0) ? 1 : 0),
            $('.LinkedAccountNumber').val(data.Data.LinkedAccountNumber),
            $('.CollateralTypePercent').prop("checked", (data.Data.CollateralType === 0) ? true : false),
            $('.CollateralTypeAmount').prop("checked", (data.Data.CollateralType === 1) ? true : false),
            $('.CollateralType').val(data.Data.CollateralType),
            $('.Collateral').val(data.Data.Collateral),
            $('.BlockOtherLoanLinks').val(data.Data.BlockOtherLoanLinks)


    }, function (xhr, status, err) {
        DisplayErrorMessage('Error', 'An error occurred during loading the Linked Account.');
    });

}

function DeleteJournalLine(id) {

    if (editMode === 'display') {
        return
    }
    
    ConfirmModal('Are you sure you want to delete this Journal Line?', function () {
        
        MakeServiceCall('DELETE', 'JournalLines/' + id, null, function (data) {

            if (data.Data) {
                DisplaySuccessMessage('Success', 'Journal Line deleted successfully.');

                CloseModal(modal);

                DisplayJournalLines($('.currentlevel').val(), $('.parentJournalLines').val());
            }

        }, null);

    }, function () {
        // no

    });
    
}

function RefreshEntity() {

    // load the grid when and event is fired on the modal.

}


