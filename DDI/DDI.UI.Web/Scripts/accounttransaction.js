$(document).ready(function () {

    LoadTransactionGrid('A959855A-F8DD-4705-ACAA-0C17C37D84B8');

});

function LoadTransactionGrid(accountId) {
    var columns = [
        { dataField: 'Id', width: '0px', },
        { dataField: 'TransactionNumber', caption: 'Trans #' },
        { dataField: 'TransactionDate', caption: 'Tran Date', dataType: 'date' },
        { dataField: 'Amount', caption: 'Amount', dataType: 'decimal'},
        { dataField: 'Description', caption: 'Description' },
        { dataField: 'TransactionId', caption: 'Trans Id' },
        { dataField: 'SourceDescription', caption: 'Source Description' },
        {
            caption: 'View',
            width: 100,
            allowFiltering: false,
            allowSorting: false,
            cellTemplate: function (container, options) {
                
            }
        }
    ];

    var gridSource = DevExpress.data.AspNet.createStore({
        load: function (loadOptions) {
            var d = $.Deferred();
            $.getJSON(WEB_API_ADDRESS + '/posttransactions/accountId/' + accountId, {
                // Passing settings to the server

                filter: loadOptions.filter ? JSON.stringify(loadOptions.filter) : "", // Pass if filtering is remote
                sort: loadOptions.sort ? JSON.stringify(loadOptions.sort) : "",       // Pass if sorting is remote
                // Pass if paging is remote
                skip: loadOptions.skip,     // The number of records to skip
                take: loadOptions.take,     // The number of records to take
                requireTotalCount: loadOptions.requireTotalCount,   // A flag telling the server whether
                // the total count of records (totalCount) is required
                group: loadOptions.group ? JSON.stringify(loadOptions.group) : "", // Pass if grouping is remote
                totalSummary: loadOptions.totalSummary, // Pass if summary is calculated remotely
                groupSummary: loadOptions.groupSummary  // Pass if grouping is remote and summary is calculated remotely
            }).done(function (result) {
                // You can process the received data here
                d.resolve(result.data, {
                    totalCount: result.totalCount, // The count of received records; needed if paging is enabled
                    summary: result.summary        // Needed if summary is calculated remotely
                });
            });
            return d.promise();
        }
    });

    $('.gridcontainer').dxDataGrid({
        columns:columns,
        remoteOperations: { paging: true, filtering: true, sorting: true, grouping: true, summary: true, groupPaging: true },
        dataSource: gridSource ,
        groupPanel: {
            allowColumnDragging: true,
            visible: true
        },
        grouping: {
            autoExpandAll: false,
        },
        filterRow: { visible: true },
        scrolling: {
            mode: "virtual"
        },
        
    });
}