//$(document).ready(function () {

var fiscalYearId = 'B80B38CF-108C-4D9D-BE49-DFFD6502449C';      // testing
var ledgerId = '52822462-5041-46CB-9883-ECB1EF8F46F0';          // testing
var journalId = 'E2A35D00-2452-11E7-833C-00C0DD01025C';         // testing
var businessUnitId = 'D63D404A-1BDD-40E4-AC19-B9354BD11D16';    // testing

$(document).ready(function () {
    if (sessionStorage.getItem('journalid')) {
        $('.hidjournalid').val(sessionStorage.getItem('journalid'))
    }
    else {
        $('.hidjournalid').val('')
    }
});

function JournalDetailLoad() {
    journalId = $('.hidjournalid').val();

    if (journalId === '') {
        $('.journaltypeselect').show()
        InitJournalImages();
    }
    else {
        LoadJournalData(journalId)
    }

}

function InitJournalImages() {
    $('.generaljournaloption').click(function (e) {
        e.preventDefault();
        ProcessJournal('General Journal');
    })
    $('.recurringjournaloption').click(function (e) {
        e.preventDefault();
        ProcessJournal('Recurring Journal');
    })
    $('.journaltemplateoption').click(function (e) {
        e.preventDefault();
        ProcessJournal('Journal Template');
    })
}

function ProcessJournal(journalType) {
    $('.journaltype').html(journalType)
    $('.journaltypeselect').hide()
    $('.tabscontainer').show()
    $('.journalbody').show()
}

function LoadJournalData(journalId) {
    $('.tabscontainer').show()
    MakeServiceCall('GET', 'journals/' + journalId, null, function (data) {

        if (data.Data) {
            if (data.IsSuccessful) {
                $('.Comment').val(data.Data.Comment);
                $('.journaltype').html(data.Data.journalType)
            }
        }

    }, null);

}



