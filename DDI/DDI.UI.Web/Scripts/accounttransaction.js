$(document).ready(function () {

    LoadTransactionGrid('1DA29C60-2792-46C3-9ED6-C398061885F8');

});

function LoadTransactionGrid(accountId) {
    
    var columns = [
        { dataField: 'Id', visible: false },
        { dataField: 'MonthYear', caption: 'Month/Year'},
        { dataField: 'TransactionNumber', caption: 'Trans #' },
        { dataField: 'TransactionDate', caption: 'Tran Date', dataType: 'date'},
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

    var gridData = new DevExpress.data.CustomStore({
        
        key: 'Id',
        load: function (loadOptions) {
            debugger;
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
                args.group = JSON.stringify(loadOptions.group);
            }
            //else {
            //    var group = [];
            //    var groupcolumn = {
            //        selector :"MonthYear",
            //        groupInterval : null,
            //        desc : false,
            //    }
            //    group.push(groupcolumn);
            //    args.group = (JSON.stringify(group));
            //}

            if (loadOptions.remotegrouping) {
                args.remotegrouping = loadOptions.remotegrouping;
            }
            if (loadOptions.searchValue) {
                args.searchValue = loadOptions.searchValue;
                args.searchOperation = loadOptions.searchOperation;
                args.searchExpr = loadOptions.searchExpr;
            }
            
            args.requireTotalCount = false;
            args.skip = loadOptions.skip || 0;
            args.take = loadOptions.take || 100;
           
            $.ajax({
                url: WEB_API_ADDRESS + 'posttransactions/accountId/' + accountId,
                data: args,
                success: function (result) {
                    deferred.resolve({data: result, totalCount: result.length });
                },
                error: function (result) {
                    debugger;
                    deferred.reject("Data Loading Error");
                },
               
            });

            return deferred.promise();
        },
        group: [
            { selector: "MonthYear", groupInterval: 100, desc: false },
        ]

        

    });

    $('.gridcontainer').dxDataGrid({
        columns: columns,
        key: 'Id',
        remoteOperations: {  grouping: true, groupPaging: true, paging: true, filtering: true, sorting: true, summary: false },
        dataSource: {
            store: gridData,
            group: "MonthYear"
        },
        filterRow: {
            visible: true,
            showOperationChooser: false
        },
        paging: {
            enabled: true,
            pageSize: 50
        },
        pager: {
            visible: true,
            showPageSizeSelector: true,
            allowedPageSizes: [50, 100, 200],
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
       
        summary: {
            groupItems: [{
                column: "MonthYear",
                summaryType: "count",
                displayFormat: "count: {0}",
            }]
        }
     
    });
}