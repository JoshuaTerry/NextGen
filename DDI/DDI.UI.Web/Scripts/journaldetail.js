
var fiscalYearId = 'B80B38CF-108C-4D9D-BE49-DFFD6502449C'      // testing
var ledgerId = '52822462-5041-46CB-9883-ECB1EF8F46F0'          // testing
var journalId = 'E2A35D00-2452-11E7-833C-00C0DD01025C'         // testing
var businessUnitId = 'D63D404A-1BDD-40E4-AC19-B9354BD11D16'    // testing
var editMode = ''
var journalContainer = '.journalbody'

// 0: 
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

});

function LoadJournalDetail() {

    MakeServiceCall('GET', 'journals/' + journalId, null, function (data) {

        if (data.Data && data.IsSuccessful) {

            $('.journaltype').html(data.Data.JournalDescription)
            $('.StatusDescription').html(data.Data.StatusDescription)
            $('.TransactionDate').val(data.Data.TransactionDate)
            $('.Comment').val(data.Data.Comment)

            switch (data.Data.JournalType) {
                case 0:
                    if (data.Data.ReverseOnDate === null) {
                        $('.reverseondategroup').css('visibility', 'hidden')
                    }
                    else {
                        $('.reverseondategroup').css('visibility', 'visible')
                        $('.ReverseOnDate').val(data.Data.ReverseOnDate)
                    }
                    break

                case 1:
                    $('.recurringgroup').show()
                    break
                case 2:
                    break
            }

            $('.CreatedBy').html(data.Data.CreatedBy)
            $('.CreatedOn').html(data.Data.CreatedOn)
            $('.LastChangedBy').html(data.Data.LastChangedBy)
            $('.LastChangedOn').html(data.Data.LastChangedOn)

            LoadJournalLineGrid(data)
        }

    }, null)

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
                return [GetDueToMode(data.DueToMode)];
            }
        },
        { dataField: 'SourceFund', caption: 'Fund' },
        { dataField: 'SourceBusinessUnit', caption: '' },
    ];

    $('.journallinegridcontainer').show()

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

function RefreshEntity() {



}


