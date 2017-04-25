$(document).ready(function () {


});

function LoadTransactionGrid(accountId) {
    var columns = {

    };

    var gridSource = DevExpress.data.AspNet.createStore({
        key: "",
        loadUrl: "api/Orders",
    });

    $('.gridContainer').dxDataGrid({
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