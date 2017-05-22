$(document).ready(function () {

    LoadTransactionGrid('1DA29C60-2792-46C3-9ED6-C398061885F8');

});

function LoadTransactionGrid(accountId) {
    
    var columns = [
        {
            dataField: 'MonthYear', caption: 'Month/Year',
            calculateSortValue: function (data) {
                var sortValue = data.TransactionDate.getMonth();
                return sortValue;
            }

        },
        { dataField: 'TransactionNumber', caption: 'Trans #' },
        { dataField: 'TransactionDate', caption: 'Tran Date', dataType: 'date', sortOrder:'asc'},
        { dataField: 'Amount', caption: 'Amount', dataType: 'number', precision: 2, format: 'currency', allowGrouping: false },
        { dataField: 'Description', caption: 'Description', allowGrouping: false },
        { dataField: 'TransactionId', caption: 'Trans Id' },
        { dataField: 'SourceDescription', caption: 'Source Description', allowGrouping: false },
        {
            caption: 'View', cellTemplate: function (container, options) {
                
                $('<input>').attr("type", "button").attr("Id", "btnView").addClass("searchicon").appendTo(container);
            },
            allowGrouping: false
        }       
    ];

    var url = WEB_API_ADDRESS + 'posttransactions/accountId/' + accountId;
    var gridData = DevExpress.data.AspNet.createStore({
        key: "Id",
        loadUrl: url,
    });


    $('.gridcontainer').dxDataGrid({
        columns: columns,
        sorting: {
            mode: "multiple"
        },
        remoteOperations: {
            grouping: true, groupPaging: true, paging: true,
            filtering: true, sorting: true, summary: true
        },
        dataSource: {
            store: gridData,
            group: [
                { selector: "MonthYear", groupInterval: 15, desc: false },
            ]
        },
        filterRow: {
            visible: true,
            showOperationChooser: false
        },
        paging: {
            pageSize: 50
        },
        groupPanel: {
            allowColumnDragging: true,
            visible: true
        },
        grouping: {
            autoExpandAll: false
        },
        scrolling: {
            mode: "virtual"
        },
        summary: {
            groupItems: [{
                column: "MonthYear",
                summaryType: "count",
                displayFormat: "count: {0}",
            }]
        }
     
    });
}