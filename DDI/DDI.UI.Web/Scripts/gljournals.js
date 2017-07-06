/* JOURNALS INQUIRY */


$(document).ready(function () {

    //Resize();
    
 
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

    $('.addnewjournal').click(function () {
        addnewjournal();
    });

    //$(window).resize(function () {
    //    Resize();
    //});

});



//function Resize() {

//    var windowHeight = $(window).height();
//    var header = $('header').height();
//    var adjustedHeight = (windowHeight - header) - 90;

//    $('.searchcriteria div.scrollable').height(adjustedHeight);

//    $('.searchresults div.scrollable').height(adjustedHeight + 30);
//}


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


    $.ajax({
        url: WEB_API_ADDRESS + 'journals/?' + parameters,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        headers: GetApiHeaders(),
        crossDomain: true,
        success: function (data) {

            if (data.Data.length === 1) {
                DisplayJournals(data.Data[0].ID);
            }
            else {

                $('.gridcontainer').dxDataGrid({
                    dataSource: data.Data,
                     key: "Id",
                    type:"array",
                    columns: [
                       // { dataField: 'ID', caption: 'ID', alignment: 'right', width: '100px' },
                        { dataField: 'JournalNumber', caption: 'Journal No.' },
                       // { dataField: 'JournalType', caption: 'Type' },
                       {
                           caption: 'Type', cellTemplate: function (container, options) {

                               var JournalType;

                               switch (options.data.JournalType) {
                                   case 0:
                                       JournalType = "Normal";
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
                       
                        { dataField: 'TransactionDate', caption: 'Tran Dt', dataType: 'date' },
                        { dataField: 'Comment', caption: 'Memo' },
                        { dataField: 'Amount', caption: 'Amount' },
                        { dataField: 'CreatedBy', caption: 'Created By'},
                        { dataField: 'CreatedOn', caption: 'Created On', dataType: 'date' },
                        { dataField: 'Status', caption: 'Status'}
                    ],
                    paging: {
                        pageSize: 15
                    },
                    pager: {
                        showNavigationButtons: true,
                        showPageSizeSelector: true,
                        showInfo: true,
                        allowedPageSizes: [15, 25, 50, 100]
                    },
                    groupPanel: {
                        visible: true,
                        allowColumnDragging: true
                    },
                    filterRow: {
                        visible: true,
                        showOperationChooser: false
                    },

                    onRowClick: function (info) {
                     
                       DisplayJournals(info.values[0]);
                    },
                    columnAutoWidth: true,
                });

            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

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
    p += '&fields=JournalNumber,JournalType,TransactionDate,Comment,Amount,CreatedBy,CreatedOn,Status&';

    p = p.substring(0, p.length - 1);

    return p;

}

function LoadCreatedBy() {

    PopulateDropDown('.searchCreatedBy', 'CreatedBy', '', '');
   
}


function DisplayJournals(id) {

    sessionStorage.setItem("ID", id);
    location.href = "../Admin/SystemSettings.aspx";

}

function addnewjournal() {

   // sessionStorage.setItem("ID", id);
    location.href = "../Admin/SystemSettings.aspx";

}



function DisplayJournal(id) {

    sessionStorage.setItem("id", id);
    location.href = "/Admin/SystemSettings.aspx";

}

