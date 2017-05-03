$(document).ready(function () {

    LoadTransactionGrid('1DA29C60-2792-46C3-9ED6-C398061885F8');

});

function LoadTransactionGrid(accountId) {
    var columns = [
        { dataField: 'MonthYear', caption: 'Month/Year', groupIndex: 0 },
        { dataField: 'Id', width: '0px' },
        { dataField: 'TransactionNumber', caption: 'Trans #' },
        { dataField: 'TransactionDate', caption: 'Tran Date', dataType: 'date'},
        { dataField: 'Amount', caption: 'Amount', dataType: 'number', precision:2, format:'currency'},
        { dataField: 'Description', caption: 'Description' },
        { dataField: 'TransactionId', caption: 'Trans Id' },
        { dataField: 'SourceDescription', caption: 'Source Description' },
        {
            caption: 'View', cellTemplate: function (container, options) {
              

                $('<input>').attr("type", "button").attr("Id", "btnView").addClass("searchicon").appendTo(container);
            },
            allowGrouping: false
        }
       
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
            if (loadOptions.filter) {
                args.filter = JSON.stringify(loadOptions.filter);
            }
           
            //Getting group options
            if (loadOptions.group) {
                args.args = JSON.stringify(loadOptions.group);
            }

            if (loadOptions.searchValue) {
                args.searchValue = loadOptions.searchValue;
                args.searchOperation = loadOptions.searchOperation;
                args.searchExpr = loadOptions.searchExpr;
            }
            
            args.requireTotalCount = false;
            args.skip = loadOptions.skip || 0;
            args.take = loadOptions.take || 50;

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
        remoteOperations: { paging: true, filtering: true, sorting: true, grouping: false, groupPaging: true, summary: false, groupPaging: true },
        dataSource: {
            store: gridData
        },
        filterRow: {
            visible: true,
            showOperationChooser: false
        },
        paging: {
            pageSize: 50
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [25, 50, 100],
            showInfo: true
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
        sortByGroupSummaryInfo: [{
            summaryItem: "count"
        }],
        summary: {
            groupItems: [{
                column: "MonthYear",
                summaryType: "count",
                displayFormat: "count: {0}",
            }]
        }

        
    });
}