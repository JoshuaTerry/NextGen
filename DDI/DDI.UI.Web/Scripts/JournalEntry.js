
var fiscalYearId = 'B80B38CF-108C-4D9D-BE49-DFFD6502449C'      // testing
var ledgerId = '52822462-5041-46CB-9883-ECB1EF8F46F0'          // testing
var journalId = 'E2A35D00-2452-11E7-833C-00C0DD01025C'         // testing
var businessUnitId = 'D63D404A-1BDD-40E4-AC19-B9354BD11D16'    // testing
var editMode = ''
var journalContainer = '.journalbody'


$(document).ready(function () {

    

    $('#attachmentstab').click(function (e) {

        $(".attachments").empty();
        LoadAttachmentsTab();
    })

});

function JournalDetailLoad() {
    $('.editjournal').click(function (e) {
        e.preventDefault()
        JournalEditMode()
    })

    $('.savejournal').unbind('click')
    $('.savejournal').click(function () {
        $('.savejournalbuttons').hide()
        SaveJournal()
    })

    $('.reversejournal').unbind('click')
    $('.reversejournal').click(function () {
        if ($('.reversejournal').prop('checked') === true) {
            $('.reverseondategroup').css('visibility', 'visible')
        }
        else {
            $('.reverseondategroup').css('visibility', 'hidden')
            $('.ReverseOnDate').val('')
        }
    })

    $('.cancelsavejournal').click(function (e) {
        e.preventDefault()
        ClearJournalFields()
        if (editMode === 'add') {
            JournalAddMode()
        }
        else {
            LoadJournalData(journalId)
            JournalDisplayMode()
        }
    })

    JournalLoad()
    InitNewJournalLineModal()
   
}

function LoadAttachmentsTab() {
    container = $('.attachments');
    LoadAttachments(container, "journalentrygrid", currentEntity.Id, null, NoteEntity[1]);
}

function JournalDisplayMode() {
    editMode = 'display'
    $('.editjournalbutton').show()
    $('.savejournalbuttons').hide()
    $(journalContainer).find('.editable').each(function () {

        $(this).prop('disabled', true)

    })
    $('.newjournallinemodallink').hide()
    FormatFields()
}

function JournalAddMode() {
    editMode = 'add'
    MaskFields()
    $('.editjournalbutton').hide()
    $('.savejournalbuttons').show()
    $('.newjournallinemodallink').show()
}

function JournalEditMode() {
    editMode = 'edit'
    $('.editjournalbutton').hide()
    $('.savejournalbuttons').show()
    $('.newjournallinemodallink').show()

    MaskFields()

    $(journalContainer).find('.editable').each(function () {

        $(this).prop('disabled', false)

    })

}

function JournalLoad() {

    if (journalId === null || journalId === '') {
        JournalAddMode()
        editMode = 'add'
        $('.journaltypeselect').show()
        InitJournalImages()
    }
    else {
        LoadJournalData()
    }

}

function InitJournalImages() {
    $('.onetimejournaloption').click(function (e) {
        e.preventDefault()
        $('.reverseondategroup').show()
        ProcessJournal('One-Time Journal')
    })
    $('.recurringjournaloption').click(function (e) {
        e.preventDefault()
        $('.reverseondategroup').hide()
        ProcessJournal('Recurring Journal')
    })
    $('.journaltemplateoption').click(function (e) {
        e.preventDefault()
        $('.reverseondategroup').hide()
        ProcessJournal('Journal Template')
    })
}

function ProcessJournal(journalTypeDescr) {
    $('.journaltype').html(journalTypeDescr)
    $('.journaltypeselect').hide()
    $('.tabscontainer').show()
    $('.journalbody').show()
    $('.journallabel').hide()
    LoadDatePickers()
}

function LoadJournalData() {
    $('.tabscontainer').show()
    $('.journallabel').show()
    LoadDatePickers()
    MakeServiceCall('GET', 'journals/' + journalId, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {
                currentEntity = data.Data;
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
                JournalDisplayMode();
                LoadJournalLineGrid(data)
            }
        }

    }, null)

    }

function GetJournalType(data) {
    switch (data.Data.JournalType) {
        case 0:
            return 'One-Time Journal #' + data.Data.JournalNumber
            break
        case 1:
            return 'Recurring Journal' + data.Data.JournalNumber
            break
        case 2:
            return 'Journal Template' + data.Data.JournalNumber
            break
    }
}

function SaveJournal() {

    var action
    var route

    var fields = GetJournalFields()

    if (editMode === 'add') {
        var action = 'POST'
        var route = 'journals'
    }
    else {
        var action = 'PATCH'
        var route = 'journals/' + journalId
    }

    MakeServiceCall(action, route, fields, function (data) {

        if (data.Data) {
            DisplaySuccessMessage('Success', 'Journal saved successfully.')
            JournalDisplayMode()
        }

    },
        function () {
            if (editMode === 'add') {
                JournalAddMode()
            }
            else {
                JournalEditMode()
            }
        }
    )
}

function GetJournalFields() {

    var p = []
    var value

    $(summaryContainer).find('.editable').each(function () {
        var property = $(this).attr('class').split(' ')
        var propertyName = property[0]
        switch (propertyName) {
            //case 'IsNormallyDebit': {
            //    if ($(this).val() === '1')
            //        value = 'true'
            //    else
            //        value = 'false'
            //    break
            //}
            //case 'IsActive': {
            //    if ($(this).prop('checked') === true)
            //        value = 'true'
            //    else
            //        value = 'false'
            //    break
            //}
            //case 'BeginningBalance': {
            //    if ($(this).val() === null || $(this).val() === '') {
            //        $(this).val(0)
            //    }
            //    value = $(this).val()
            //    break
            //}
            //case 'ClosingJournalId': {
            //    if ($(this).val() === '') {
            //        $(this).val(false)
            //    }
            //}
            default:
                value = $(this).val()
        }

        if (value)
            p.push('"' + propertyName + '": "' + value + '"')
    })

    // add non-screen items
    p.push('"FiscalYearId": "' + fiscalYearId + '"')
    p.push('"BusinessUnitId": "' + businessUnitId + '"')

    // Line Item information
    jlSet = [] // Array of Journal Line objects

    var journalentry = { LedgerAccountId: '', LineNumber: '', Comment: '', Amount: 0, Percent: 0, DueToMode: 0, SourceBusinessUnitId: '', SourceFundId: ''}
    dxdatagrid.rows.each(function (item) {
        var je = new journalentry();
        je.id = item.id;
        je.description = item.description;
        jlSet.push(je)
    })

    if (jlSet.length > 0) {
        p.push('"JournalLines": [ ' + jlSet + ']')  // Add the array of Journal line objects to the Journal object
    }

    p = '{' + p + '}'

    return p

}

function ClearJournalFields() {

    $(journalContainer + ' input[type="text"]').val('')
    $(journalContainer + ' input[type="number"]').val('')
    $(journalContainer + ' textarea').val('')
    $(journalContainer + ' select').val(0)
    $(journalContainer + ' input:checkbox').prop('checked', false)
    $(journalContainer).find(".StatusDescription").html('')
    $(journalContainer).find(".CreatedBy").html('')
    $(journalContainer).find(".CreatedOn").html('')
    $('.journallinegridcontainer').hide()
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

function GetDebitCredit(amount, type) {
    if (type === 'Debit') {
        return (amount >= 0 ? amount : 0);
    }
    if (type === 'Credit') {
        return (amount < 0 ? amount : 0);
    }
}

function GetDueToMode(dueToMode) {
    switch (dueToMode) {
        case 0:
            return 'None'
            break;
        case 1:
            return 'Due From'
            break;
        case 2:
            return 'Due To'
            break;
        default:
            return 'Undefined'
    }
}


// modal section

function InitNewJournalLineModal() {

    $('.newjournallinemodallink').click(function (e) {

        GLAccountSelector($('.journallineaccountgroup'), ledgerId, fiscalYearId);

        e.preventDefault();

        modal = $('.journallinemodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 1000,
            resizable: false,
            beforeClose: function (e) {
                $('.journallineledgeraccountid').empty();
            }
        });

        $('.canceljournallinemodal').click(function (e) {

            e.preventDefault();

            CloseModal(modal);

        });

        $('.savejournalline').unbind('click');

        $('.savejournalline').click(function () {

            var item = GetJournalLineToSave();

            MakeServiceCall('POST', 'JournalLine', item, function (data) {

                DisplaySuccessMessage('Success', 'Linked Account saved successfully.');

                CloseModal(modal);

                LoadJournalLineGrid();

            }, function (xhr, status, err) {

                DisplayErrorMessage('Error', 'An error occurred during saving the Linked Account.');

            });

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

function DeleteJournalLine(id) {

    if (editMode === 'display') {
        return
    }

    MakeServiceCall('DELETE', 'JournalLines/' + id, null, function (data) {

        if (data.Data) {
            DisplaySuccessMessage('Success', 'Journal Line deleted successfully.');

            CloseModal(modal);

            DisplayJournalLines($('.currentlevel').val(), $('.parentJournalLines').val());
        }

    }, null);

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

function GetJournalLineToSave() {

    var rawitem = {

        LinkedAccountType: $(modal).find('.LinkedAccountType').val(),
        //LinkedAccountInd: ($.isNumeric($(modal).find('.LinkedAccountNumber').val()) ? 1 : 0),    // when the item is available
        LinkedAccountNumber: $(modal).find('.LinkedAccountNumber').val(),
        CollateralType: ($('.CollateralTypePercent').prop("checked" === true) ? 0 : 1),
        Collateral: $(modal).find('.Collateral').val(),
        BlockOtherLoanLinks: $(modal).find('.BlockOtherLoanLinks').val()

    };

    var item = JSON.stringify(rawitem);

    return item;

}

