

function LoadAccountSelectorGrid() {

    // LedgerId 7BAFBB1E-A2DC-4D85-9542-229378F8DBC7
    // FY ID	1A67ED6F-0FD8-47CD-9476-DC09D94E5F28

    //GLAccountSelector(container, ledgerId, fiscalYearId)
    // FiscalYear.Id
    // FiscalYear.Ledger.Id

    // GLAccountSelector('.as-accounts', '7BAFBB1E-A2DC-4D85-9542-229378F8DBC7', '1A67ED6F-0FD8-47CD-9476-DC09D94E5F28');
    //MakeServiceCall(method, route, item, successCallback, errorCallback)

    var fiscalyearid = '39BCC96D-3473-44B6-A188-AE96226F3464';

    MakeServiceCall('GET', 'fiscalyears/' + fiscalyearid, null, function (data) {

        LoadGLAccounts('.as-accounts', data.Data.LedgerId, data.Data.Id, function (d) {

            if (d) {
                sessionStorage.setItem(ACCOUNT_ID, d.AccountNumber);
                location.href = "/Pages/GL/AccountDetails.aspx";
            }

        });

    }, null);
    
}

function GLAccountSelector(container, ledgerId, fiscalYearId) {

    CreateGLAccountSelector(container);

    $('.accountnumberlookup', container).change(function () {
        if ($(container).find(".hidaccountnumber").val() != $(container).find(".accountnumber").val())
        {
            LoadNewAccountNumber(container, $(container).find(".accountnumber").val(),fiscalYearId);
        }
    });

    $('.accountnumberlookup',container).autocomplete({
        source: function (request, response) {
            MakeServiceCall('GET', 'fiscalyears/' + fiscalYearId + '/accounts/lookup/' + request.term , null, function (result) {
                if (result.Data.length == 1) {
                    var item = {
                        description: result.Data[0].Description,
                        label: result.Data[0].AccountNumber,
                        value: result.Data[0].Id
                    };
                    SelectAccountNumberLookup(item, container);
                }
                else {

                    var results = $.ui.autocomplete.filter($.map(result.Data, function (item) {
                        return {
                            description: item.Description,
                            label: item.AccountNumber,
                            value: item.Id
                        };
                    }), request.term);

                    response(results);
                }
            }, null);
        },
        select: function (event, ui) {
            event.preventDefault();
            SelectAccountNumberLookup(ui.item, container);
        }
    });

    
    $(".accountselectionsearch", container).click(function () {

        var grid = $(' .gridContainer .dx-widget', container).length;

        if (grid == 0) {

            LoadGLAccounts(container, ledgerId, fiscalYearId, function (d) {
                if (d) {
                    $(container).find(".accountnumber").val(d.AccountNumber);
                    $(container).find(".hidaccountnumber").val(d.AccountNumber);
                    $(container).find(".hidaccountid").val(d.Id);
                    $(container).find(".accountdescription").text(d.Description);
                    $(container).find(".accountnumber").focus();
                    // $(container).find(".gridContainer").hide();
                }
            });

            $(container).find('.gridContainer').show();

        }
        else {

            if (($(container).find('.gridContainer').css("display") == 'block')){
                $(container).find('.gridContainer').hide();
            }
            else {
                $(container).attr("gridOpen", "");
                var dataGrid = $(container).find('.gridContainer').dxDataGrid("instance");
                dataGrid.repaint();
                $(container).find('.gridContainer').show();
            }

        }
    });

    $(".accountnumber:first").focus();
}

function SelectAccountNumberLookup(item, container) {
    $(container).find(".accountdescription").html(item.description);
    $(container).find(".accountnumber").val(item.label);
    $(container).find(".hidaccountid").val(item.value);
    $(container).find(".hidaccountnumber").val(item.label);
    $(container).find(".gridContainer").hide();
}

function CreateGLAccountSelector(container)
{
    var glcontrol = $('<div>').addClass("fieldblock");
    $('<input>').attr("type", "text").attr("maxlength", "25").attr("style", "width:45%").addClass("accountnumber").addClass("accountnumberlookup").appendTo($(glcontrol));
    $('<span>').addClass("accountselectionsearch").addClass("inline").appendTo($(glcontrol));
    $('<label>').addClass("accountdescription").addClass("inline").appendTo($(glcontrol));

    glcontrol.appendTo($(container));
   
    $('<div>').addClass("gridContainer").addClass("accountselectiongrid").appendTo($(container));
    $('<input>').attr("type", "hidden").addClass("hidaccountid").addClass("inline").appendTo($(container));
    $('<input>').attr("type", "hidden").addClass("hidaccountnumber").addClass("inline").appendTo($(container));
}

function LoadGLAccounts(container, ledgerId, fiscalYearId, onSelect) {
   
    MakeServiceCall('GET', 'ledgers/' + ledgerId, null,
        function (data) {
            var numberOfAccountGroups = data.Data.AccountGroupLevels;
            var columns = [];

            for (var i = 0; i < numberOfAccountGroups; i++) {
                columns.push({
                    dataField: "Level" + (i + 1),
                    caption: "",
                    groupIndex: i,
                    groupCellTemplate: function (groupCell, info) {
                        var groupheader = info.value;
                        if (groupheader == undefined)
                        {
                            groupheader = "Accounts"
                        }
                        $('<label>').html(groupheader).appendTo(groupCell);
                    },
                });
            }

            columns.push({ dataField: 'AccountNumber', caption: 'Account Number', sortOrder: 'asc', sortIndex: 0});
            columns.push("Description");

            LoadGLAccountGrid(container, fiscalYearId, columns, onSelect);
        }
        , null
    );
}

function LoadGLAccountGrid(container, fiscalYearId, columns, onSelect)
{
    var url = WEB_API_ADDRESS + 'accounts/fiscalyear/' + fiscalYearId;
    var gridData = DevExpress.data.AspNet.createStore({
        key: "Id",
        loadUrl: url,
        onBeforeSend: function (operation, ajaxSettings) {
            ajaxSettings.headers = GetApiHeaders();
            ajaxSettings.crossDomain = true;
        }
    });
    //create grid
    $(container).find(".gridContainer").dxDataGrid({
        columns: columns,
        dataSource: {
            store: gridData,
            type: "array"
        },
        scrolling: {
            mode: "virtual"
        },
        grouping: {
            autoExpandAll: false
        },
        selection: {
           mode: 'single' // 'multiple'
        },
        onSelectionChanged: function (selectedItems) {
            if (onSelect) {
                var data = selectedItems.selectedRowsData[0];
                // $(container).find('.gridContainer').hide();
                onSelect(data);
            }
        },
        onContentReady: function () {
            if (($(container).find(".hidaccountid").val().length > 0) && ($(container).attr("gridOpen") != "true")) {

                var dataGrid = $(container).find('.gridContainer').dxDataGrid('instance');
                var keyId = $(container).find(".hidaccountid").val();

                dataGrid.selectRows(keyId);

                var groupedColumns = dataGrid.getVisibleColumns().filter(function(column) {
                    return column.groupIndex > -1;
                }).sort(function(c1, c2) {
                    return c1.groupIndex - c2.groupIndex;
                }).map(function(column) {
                    return column.dataField;
                });

                dataGrid.byKey(keyId).done(function (row) {

                    var keys = groupedColumns.map(function(col) {
                        return row[col];
                    });

                    var groupToExpand = [];

                    keys.forEach(function(key) {
                        groupToExpand.push(key);
                        dataGrid.expandRow(groupToExpand);
                    });

                });

                $(container).attr("gridopen", true)
            }

            $(container).find('.gridContainer').show();
        }
    });
}

function LoadSelectedAccount(container, value)
{
    MakeServiceCall('GET', 'accounts/' + value, null, function (data) {
            $(container).find(".accountnumber").val(data.Data.AccountNumber);
            $(container).find(".hidaccountid").val(data.Data.Id);
            $(container).find(".hidaccountnumber").val(data.Data.AccountNumber);
            $(container).find(".accountdescription").text(data.Data.Name);          
    }, null);
}

function LoadNewAccountNumber(container, accountNumber,fiscalYearId)
{
    MakeServiceCall('GET', 'accounts/fiscalyear/' + fiscalYearId + '/accountnumber/' + accountNumber, null, function (data) {
        if (data.Data.length > 0)
        {
            $.map(data.Data, function (item) {
                $(container).find(".accountnumber").val(item.AccountNumber);
                $(container).find(".hidaccountid").val(item.Id);
                $(container).find(".hidaccountnumber").val(item.AccountNumber);
                $(container).find(".accountdescription").text(item.Description);
                $(container).find(".accountnumber").focus();
                $(container).find(".gridContainer").hide();
            });
        }
        else {
            DisplayErrorMessage('Error', "Account Number was not found. Please enter another number.");
        }
    },
    function (xhr, status, err) {
        DisplayErrorMessage('Error', err);
    });
}

