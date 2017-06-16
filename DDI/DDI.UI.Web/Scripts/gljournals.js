/* JOURNALS INQUIRY */


$(document).ready(function () {

    Resize();

    //PopulateDropDowns();
    

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

    $(window).resize(function () {
        Resize();
    });

});



function Resize() {

    var windowHeight = $(window).height();
    var header = $('header').height();
    var adjustedHeight = (windowHeight - header) - 90;

    $('.searchcriteria div.scrollable').height(adjustedHeight);

    $('.searchresults div.scrollable').height(adjustedHeight + 30);
}

//function PopulateDropDowns() {

//    LoadCreatedBy();
//}

//function LoadCreatedBy() {

//    PopulateDropDown('.searchCreatedby', 'CreatedBy', '', '');

//    $('.searchCreatedby').change(function () {

//        PopulateDropDown('.searchCreatedby', 'Journals/?CreatedBy=' + $('.searchCreatedby').val(), '', '');

//    });

//}

//function LoadCreatedBy() {

//    $.ajax({
//        url: WEB_API_ADDRESS + 'journals/?' + 'fields=CreatedBy&',
//        method: 'GET',
//        contentType: 'application/json; charset-utf-8',
//        dataType: 'json',
//        headers: GetApiHeaders(),
//        crossDomain: true,
//        success: function (data) {
            

//            $('.gridcontainer').dxDataGrid({
//                dataSource: data.Data,
//                columns: [
//                    { dataField: 'CreatedBy', caption: 'Created By' },
            

       
    

//}}
//    }

function AddColumnHeaders() {

    var header = $('.searchresultstable thead');
    var columns = ['ID', 'Journal#', 'JournalType', 'Tran Dt', 'Memo', 'Amount', 'Created By', 'Created Date', 'Status'];
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
                    columns: [
                        { dataField: 'ID', caption: 'ID', alignment: 'right', width: '100px' },
                        { dataField: 'JournalNumber', caption: 'Journal#' },
                        { dataField: 'JournalType', caption: 'Type' },
                        { dataField: 'TransactionDate', caption: 'Tran Dt' },
                        { dataField: 'Comment', caption: 'Memo' },
                        { dataField: 'Amount', caption: 'Amount' },
                        { dataField: 'CreatedBy', caption: 'Created By' },
                        { dataField: 'CreatedOn', caption: 'Year' },

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
                    }
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

    p += 'limit=100&';
    p += 'fields=JournalNumber,JournalType,TransactionDate,Comment,Amount,CreatedBy,CreatedOn,&';

    p = p.substring(0, p.length - 1);

    return p;

}

function DisplayJournals(id) {

    sessionStorage.setItem("ID", id);
    location.href = "../Admin/SystemSettings.aspx";

}

function addnewjournal() {

   // sessionStorage.setItem("ID", id);
    location.href = "../Admin/SystemSettings.aspx";

}

function EditBusinessUnit(bufromtoid) {

    MakeServiceCall('GET', 'businessunitfromtos/' + bufromtoid, null, function (data) {
        modal = $('.businessunitduemodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 400,
            resizable: false,
            beforeClose: function (e) {
                $('.bus-FromLedgerAccount').empty();
                $('.bus-ToLedgerAccount').empty();
            }

        });

        $('.businessunitduemodal').show();


        GLAccountSelector($('.bus-FromLedgerAccount'), $('.FundLedgerId').val(), $('.selectfiscalyear').val());
        GLAccountSelector($('.bus-ToLedgerAccount'), $('.FundLedgerId').val(), $('.selectfiscalyear').val());


        LoadSelectedAccount($('.bus-FromLedgerAccount'), data.Data.FromAccountId);
        LoadSelectedAccount($('.bus-ToLedgerAccount'), data.Data.ToAccountId);

        //$('.bus-FromLedgerAccount').val(data.Data.FromLedgerAccount.AccountNumber);

        //$('.bus-ToLedgerAccount').val(data.Data.ToLedgerAccount.AccountNumber);


        $('.cancelbusinessunitduedetailsmodal').click(function (e) {

            e.preventDefault();

            CloseModal(modal);

            PopulateFundBusinessFromFiscalYear($('.selectfiscalyear').val(), $('.FundLedgerId').val());

            $('.bus-FromLedgerAccount').empty();
            $('.bus-ToLedgerAccount').empty();

        });

        $('.Savebusinessunitduedetails').unbind('click');

        $('.Savebusinessunitduedetails').click(function () {

            var item = {
                FromAccountId: $(modal).find('.bus-FromLedgerAccount > .hidaccountid').val(),
                ToAccountId: $(modal).find('.bus-ToLedgerAccount > .hidaccountid').val()

            }

            MakeServiceCall('PATCH', 'businessunitfromtos/' + bufromtoid, JSON.stringify(item), function (data) {

                DisplaySuccessMessage('Success', 'Business Unit saved successfully.');
                CloseModal(modal);
                PopulateFundBusinessFromFiscalYear($('.selectfiscalyear').val(), $('.FundLedgerId').val());
                // PopulateFundFromFiscalYear(fiscalyear, $('.FundLedgerId').val(), fundid);
                $('.bus-FromLedgerAccount').empty();
                $('.bus-ToLedgerAccount').empty();

            }, function (xhr, status, err) {

                DisplayErrorMessage('Error', 'An error occurred during saving the Business Due.');

            });



        });



    }, null);


}

function DisplayJournal(id) {

    sessionStorage.setItem("id", id);
    location.href = "/Admin/SystemSettings.aspx";

}


function LoadjournalsaccountSettings() {

    var accordion = $('<div>').addClass('accordions');
    var journals = $('<div>').addClass('journalsinquirycontainer');
    

    var header = $('<h1>').text('').appendTo($(accordion));
    $(journals).appendTo($(accordion));
    PopulatejournalsGrid();

    $(accordion).appendTo($('.gridcontainer'));

    LoadAccordions();

    function PopulatejournalsGrid() {
        
        var journalsinquirycolumns = [
         //   { dataField: 'Appr', width: '0px' },
            { dataField: 'Id', width: '0px' },
            { dataField: 'JournalNumber', caption: 'Journal#' },
            { dataField: 'JournalType', caption: 'Type' },
            { dataField: 'TransactionDate', caption: 'Tran Dt' },
            { dataField: 'Comment', caption: 'Memo' },
            { dataField: 'Amount', caption: 'Amount' },
            { dataField: 'CreatedBy', caption: 'Created By' },
            { dataField: 'CreatedOn', caption: 'Year' },
            { dataField: 'ToLedgerAccount.Name', caption: 'Status' },
         
        ];

                CustomLoadGrid('journalsinquiry', '.journalsinquirycontainer', journalsinquirycolumns, 'journals?fields=all', null, EditBusinessUnit, null, null);
        //function CustomLoadGrid(grid, container, columns, route, selected, editMethod, deleteMethod, oncomplete) {

       // CustomLoadGrid('businessunitduegrid', '.businessunitduecontainer', businessduecolumns, 'fiscalyears/' + fiscalyearid + '/businessunitfromto', null, EditBusinessUnit, null, null);
        //LoadGrid('.businessunitduecontainer', 'businessunitduegrid', businessduecolumns, 'fiscalyears/' + fiscalyearid + '/businessunitfromto', 'businessunitfromtos', null, 'bus-',
        //'.businessunitduemodal', '', 250, false, false, false, null
        //
    }

}