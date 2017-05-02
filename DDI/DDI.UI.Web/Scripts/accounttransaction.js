$(document).ready(function () {

    LoadTransactionGrid('1DA29C60-2792-46C3-9ED6-C398061885F8');

});

function LoadTransactionGrid(accountId) {
    var columns = [
        { dataField: 'Id', width:0 },
        { dataField: 'TransactionNumber', caption: 'Trans #' },
        { dataField: 'TransactionDate', caption: 'Tran Date', dataType: 'date' },
        { dataField: 'Amount', caption: 'Amount', dataType: 'number', precision:2, format:'currency'},
        { dataField: 'Description', caption: 'Description' },
        { dataField: 'TransactionId', caption: 'Trans Id' },
        { dataField: 'SourceDescription', caption: 'Source Description' },
       
    ];

    var gridData = new DevExpress.data.CustomStore({
        key: 'Id',
        load: function (loadOptions) {
            var deferred = $.Deferred(),
                args = {};

            if (loadOptions.sort) {
                args.orderby = loadOptions.sort[0].selector;
                if (loadOptions.sort[0].desc)
                    args.orderby += " desc";
            }
            
            args.filter = loadOptions.filter || "";
            args.group = loadOptions.group || null;
            args.requireTotalCount = false;
            args.searchExpr = loadOptions.searchExpr || "";
            args.searchOperation = loadOptions.searchOperation || "";
            args.searchValue = loadOptions.skip || null;
            args.skip = loadOptions.skip || 0;
            args.take = loadOptions.take || 12;

            $.ajax({
                url: WEB_API_ADDRESS + 'posttransactions/accountId/' + accountId,
                data: args,
                success: function (result) {
                    deferred.resolve({data: result, totalCount: result.length });
                },
                error: function (result) {
                    deferred.reject("Data Loading Error");
                },
               
            });

            return deferred.promise();
        }
    });

    $('.gridcontainer').dxDataGrid({
        columns: columns,
        key: 'Id',
        remoteOperations: { paging: true, filtering: true, sorting: true, grouping: true, summary: false, groupPaging: true },
        dataSource: {
            store: gridData
        },
        groupPanel: {
            allowColumnDragging: true,
            visible: true
        },
       
        scrolling: {
            mode: "virtual"
        },
        
    });
}