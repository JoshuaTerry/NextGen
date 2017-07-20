/* JOURNALS INQUIRY */


$(document).ready(function () {

    // Resize();
 
    $('.clearsearch').click(function () {
        $('.searchcriteria div.fieldblock input').each(function () {
            $(this).val('');
        });

        $('.searchcriteria select').each(function () {
            $(this).val(0);
        });
    });

    $(document).keypress(function (e) {
        if (e.which === 13) {
            DoSearch();
        }
    });

    $('.dosearch').click(function () {
        DoSearch();
    });

    $('.addnewjournal').click(function (e) {

        e.preventDefault();

        AddNewJournal();
    });


});



function AddColumnHeaders() {

    var header = $('.searchresultstable thead');
    var columns = ['ID', 'Journal No.', 'JournalType', 'Tran Dt', 'Memo', 'Amount', 'Created By', 'Created Date', 'Status'];
    var tr = $('<tr>');

    $(columns).each(function () {
        $('<th>').text(this).appendTo($(tr));
    });

    $(tr).appendTo($(header));

}


function DoSearch() {

    var parameters = GetSearchParameters();

    MakeServiceCall('GET', 'journals/?' + parameters, null, function (data) {

        if (data.Data.length === 1) {

            DisplayJournal(data.Data[0].ID);

        }
        else {

            var columns = [
                { dataField: 'JournalNumber', caption: 'Journal No.' },
                {
                    caption: 'Type', cellTemplate: function (container, options) {

                        var JournalType;

                        switch (options.data.JournalType) {
                            case 0:
                                JournalType = "One-Time";
                                break;
                            case 1:
                                JournalType = "Recurring";
                                break;
                            case 2:
                                JournalType = "Template";
                                break;
                        }

                        $('<label>').text(JournalType).appendTo(container);
                    }
                },
                { dataField: 'TransactionDate', caption: 'Tran Dt', dataType: 'date', alignment: 'right' },
                { dataField: 'Comment', caption: 'Memo' },
                { dataField: 'Amount', caption: 'Amount', format: { type: 'currency', precision: 2 } },
                { dataField: 'CreatedBy', caption: 'Created By' },
                { dataField: 'CreatedOn', caption: 'Created On', dataType: 'date', alignment: 'right' },
                { dataField: 'Status', caption: 'Status' }
            ];
            
            LoadGridWithData('journalinquerygrid', '.gridcontainer', columns, '', DisplayJournal, null, null, data, null);

        }

    }, null);

}

function GetSearchParameters() {

    var p = '';

    $('.searchcriteria div.fieldblock input').each(function () {
        var property = $(this).attr('class').split(' ')[0].replace('search', '');
        var value = $(this).val();

        if (value) {
            p += property + '=' + encodeURIComponent(value) + '&';
        }
    });

    $('.searchcriteria div.fieldblock select').each(function () {
        var property = $(this).attr('class').split(' ')[0].replace('search', '');
        var value = $(this).val();

        if (value && value !== 'null') {
            p += property + '=' + encodeURIComponent(value) + '&';
        }
    });

    p += 'limit=100&' + 'BusinessUnitId=' + currentBusinessUnitId;
    p += '&fields=Id,JournalNumber,JournalType,TransactionDate,Comment,Amount,CreatedBy,CreatedOn,Status&';

    p = p.substring(0, p.length - 1);

    return p;

}

function LoadCreatedBy()
{

    PopulateDropDown('.searchCreatedBy', 'CreatedBy', '', '');

}

function AddNewJournal()
{

    modal = $('.newjournalmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 400,
        resizable: false
    });

    $('.journaltypeselection div').unbind('click');

    $('.journaltypeselection div').click(function () {

        sessionStorage.setItem('JOURNAL_TYPE', $(this).attr('id'));
        location.href = "JournalEntry.aspx";

    });

}

function DisplayJournal(id)
{
    sessionStorage.setItem('JOURNAL_ID', id.data.Id);
    location.href = "JournalEntry.aspx";

}

