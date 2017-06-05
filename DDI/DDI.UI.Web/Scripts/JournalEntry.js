//$(document).ready(function () {

var fiscalYearId = 'B80B38CF-108C-4D9D-BE49-DFFD6502449C'      // testing
var ledgerId = '52822462-5041-46CB-9883-ECB1EF8F46F0'          // testing
var journalId = 'E2A35D00-2452-11E7-833C-00C0DD01025C'         // testing
var businessUnitId = 'D63D404A-1BDD-40E4-AC19-B9354BD11D16'    // testing
var editMode = ''
var journalContainer = 'journalbody'

$(document).ready(function () {
    //if (sessionStorage.getItem('journalid')) {
    //    $('.hidjournalid').val(sessionStorage.getItem('journalid'))
    //}
    //else {
    //    $('.hidjournalid').val('')
    //}
    $('.hidjournalid').val(journalId)       // testing

    $('.editjournal').click(function (e) {
        e.preventDefault()
        JournalEditMode()
    })

    $('.savejournal').unbind('click')
    $('.savejournal').click(function () {
        $('.savejournalbuttons').hide()
        SaveJournal()
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

})

function JournalDisplayMode() {
    editMode = 'display'
    $('.editjournalbutton').show()
    $('.savejournalbuttons').hide()
    $(summaryContainer).find('.editable').each(function () {

        $(this).prop('disabled', true)

    })
    FormatFields()
}

function JournalAddMode() {
    editMode = 'add'
    MaskFields()
    $('.editjournalbutton').hide()
    $('.savejournalbuttons').show()
    $('.journalsegmentscontainer').show()
}

function JournalEditMode() {
    editMode = 'edit'
    if (activityTotal === 0) {
        $('.journalsegmentscontainer').show()
    }
    $('.editjournalbutton').hide()
    $('.savejournalbuttons').show()

    MaskFields()

    $(summaryContainer).find('.editable').each(function () {

        $(this).prop('disabled', false)

    })

}

function JournalDetailLoad() {
    journalId = $('.hidjournalid').val()

    if (journalId === '') {
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
    $('.generaljournaloption').click(function (e) {
        e.preventDefault()
        $('.reverseondategroup').show()
        ProcessJournal('General Journal')
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

function ProcessJournal(journalType) {
    $('.journaltype').html(journalType)
    $('.journaltypeselect').hide()
    $('.tabscontainer').show()
    $('.journalbody').show()
    $('.journallabel').hide()
    //LoadDatePickers()
}

function LoadJournalData() {
    $('.tabscontainer').show()
    $('.journallabel').show()
    //LoadDatePickers()
    MakeServiceCall('GET', 'journals/' + journalId, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {
                $('.journaltype').html(function(data){
                    GetJournalType()
                })
                $('.JournalNumber').html(data.Data.JournalNumber)
                $('.TransactionDate').html(data.Data.TransactionsDate)
                $('.Comment').val(data.Data.Comment)
                $('.ReverseOnDate').html(data.Data.ReverseOnDate)
                $('.journaltype').html(data.Data.journalType)
                $('.CreatedBy').html(data.Data.CreatedBy)
                $('.CreatedOn').html(data.Data.CreatedOn)
                AccountDisplayMode();
            }
        }

    }, null)

}

function GetJournalType(data) {
    switch (data.Data.JournalType) {
        case 0:
            return 'General Journal'
            break
        case 1:
            return 'Recurring Journal'
            break
        case 2:
            return 'Journal Template'
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
    $(journalContainer + ' .segmentname').html('')
    $(journalContainer + ' .segmentcode').html('')
    $(journalContainer + ' select').val(0)
    $(journalContainer + ' input:checkbox').prop('checked', false)
    $(journalContainer).find(".journaldescription").html('')
    $(journalContainer).find(".journalnumber").val('')
    $(journalContainer).find(".hidjournalid").val('')
}


//group modal section

function NewJournalLineModal(groupLevel, parentId, groupName) {
    $('.modalGroupName').html(groupName)
    $('.ag-Category').val(0)
    $('.ag_Name').val('')
    if (groupLevel != 1) {
        $('.group1only').hide()
    }
    else {
        $('.group1only').show()
    }

    modal = $('.journalJournalLineModal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 500,
        resizable: false
    })

    $('.canceljournalJournalLineModal').click(function (e) {

        e.preventDefault()

        CloseModal(modal)

    })

    $('.savejournalgroupbutton').unbind('click')

    $('.savejournalgroupbutton').click(function () {

        var item = GetJournalGroupItemsToSave(modal, parentId)

        MakeServiceCall('POST', 'JournalGroups', item, function (data) {

            DisplaySuccessMessage('Success', 'Journal Group saved successfully.')

            CloseModal(modal)

            if (editMode != 'display') {
                LoadGroupDropDown(groupLevel, parentId, data.Data.Id)
            }
            else {
                LoadGroupDropDown(groupLevel, parentId, saveGroupId)
            }

        })

    })

}

function EditJournalLineModal(groupLevel, groupId, parentId, groupName) {
    $('.modalGroupName').html(groupName)
    if (groupLevel != 1) {
        $('.ag-Category').val(0)
        $('.group1only').hide()
    }
    else {
        $('.group1only').show()
    }

    var modal = $('.journalJournalLineModal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 500,
        resizable: false
    })

    LoadJournalGroups(groupId)

    $('.canceljournalgroupmodal').click(function (e) {

        e.preventDefault()

        CloseModal(modal)

    })

    $('.savejournalgroupbutton').unbind('click')

    $('.savejournalgroupbutton').click(function () {

        var item = GetJournalGroupItemsToSave(modal, parentId)

        MakeServiceCall('PATCH', 'JournalGroups/' + groupId, item, function (data) {

            DisplaySuccessMessage('Success', 'Journal Group saved successfully.')

            CloseModal(modal)

            if (editMode != 'display') {
                LoadGroupDropDown(groupLevel, parentId, data.Data.Id)
            }

        })

    })
}



