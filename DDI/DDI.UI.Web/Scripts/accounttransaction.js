var _periodDescription = [];

function GetPeriodDescription(periodNumber)
{
    for (var i = 0; i < _periodDescription.length; i++)
    {
        if (_periodDescription[i].PeriodNumber == periodNumber)
        {
            return _periodDescription[i].MonthYear;
        }
    }
}

$(document).ready(function () {

    LoadTransactionGrid(accountId);

});

function LoadTransactionGrid(accountId) {

    MakeServiceCall("GET", "fiscalperiods/glaccount/" + accountId, null,
        function (data) {
            getMonthName = function (v) {
                var month = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
                return month[v];
            };

            var periods = data.Data;
            if (data != undefined)
            {
                $.map(data.Data, function (item) {
                    var fiscalPeriodInfo = {
                        PeriodNumber: item.PeriodNumber,
                        MonthYear: item.IsAdjustmentPeriod ? "Adjustments" : getMonthName(new Date(item.StartDate).getMonth()) + ' ' + new Date(item.StartDate).getFullYear()
                    };
                    _periodDescription.push(fiscalPeriodInfo);
                });
            }

        }, null
    );

    var columns = [
        {
            dataField: 'PeriodNumber', caption: 'Period',
            cellTemplate: function (container, options) {
                var periodDescription = GetPeriodDescription(options.value);
                $('<label>').text(periodDescription).appendTo(container);
            },
            groupIndex: 0,
            groupCellTemplate: function (groupCell, info) {
                var periodDescription = GetPeriodDescription(info.value);
                var groupheader = periodDescription + ' (count: ' + info.summaryItems[0].value + ')';
                $('<div>').html(groupheader).appendTo(groupCell);
            },
            sortOrder: 'asc',
            sortIndex: 0
        },
        {
            dataField: 'TransactionNumber', caption: 'Trans #',
            groupCellTemplate: function (groupCell, info) {
                var groupheader = info.value + ' (count: ' + info.summaryItems[0].value + ')';
                $('<div>').html(groupheader).appendTo(groupCell);
            }
        },
        {
            dataField: 'TransactionDate', caption: 'Tran Date', dataType: 'date',
            sortOrder: 'asc', sortIndex: 1,
            groupCellTemplate: function (groupCell, info) {
                var groupheader = info.value.format('MM/dd/yyyy') + ' (count: ' + info.summaryItems[0].value + ')';
                $('<div>').html(groupheader).appendTo(groupCell);
            }
        },
        { dataField: 'Amount', caption: 'Amount', dataType: 'number', precision: 2, format: 'currency', allowGrouping: false },
        { dataField: 'Description', caption: 'Description', allowGrouping: false },
        { dataField: 'TransactionId', caption: 'Trans Id', allowGrouping: false  },
        { dataField: 'SourceDescription', caption: 'Source Description', allowGrouping: false },
        {
            caption: 'View', cellTemplate: function (container, options) {
               $('<input>').attr("type", "button").attr("Id", "btnView").addClass("searchicon").appendTo(container);
            },
            allowGrouping: false
        }       
    ];

    var url = WEB_API_ADDRESS + 'postedtransactions/accountId/' + accountId;
    var gridData = DevExpress.data.AspNet.createStore({
        key: "Id",
        loadUrl: url,
        onBeforeSend: function (operation, ajaxSettings) {
            ajaxSettings.headers = GetApiHeaders();
            ajaxSettings.crossDomain = true;
        }
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
            type: "array"
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
                column: "PeriodNumber",
                summaryType: "count",
                displayFormat: "count: {0}"
            }]
        }
     
    });
}