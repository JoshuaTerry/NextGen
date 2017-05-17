

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

    CreateGLAccountSelector(container)

    $('.accountnumberlookup',container).autocomplete({
        source: function (request, response) {
            MakeServiceCall('GET', 'fiscalyears/' + fiscalYearId + '/accounts/lookup/' + request.term , null, function (result) {

                if (result.Data.length == 1) {
                    var item = {
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
                        }
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

    
    $(".accountselectionsearch",container).click(function () {
        var grid = $(' .gridContainer .dx-widget', container).length;
       
        if(grid == 0)
        {
            LoadGLAccounts(container, ledgerId, fiscalYearId, function (d) {

                if (d) {
                    $(container).find(".accountnumber").val(d.AccountNumber);
                    $(container).find(".hidaccountid").val(d.Id);
                    $(container).find(".accountdescription").text(d.Description)
                    $(container).find(".accountnumber").focus();
                    $(container).find(".gridContainer").hide();
                }

            });
        }
        $(container).find('.gridContainer').show();
    });

    $(".accountnumber:first").focus();

}

function SelectAccountNumberLookup(item, container) {

    $(container).find(".accountdescription").html(item.description)
    $(container).find(".accountnumber").val(item.label)
    $(container).find(".hidaccountid").val(item.value)
}

function CreateGLAccountSelector(container)
{
    var glcontrol = $('<span>').addClass("inline-block");
        
    $('<input>').attr("type", "hidden").addClass("hidaccountid").appendTo($(container));
    $('<input>').attr("type", "text").attr("maxlength", "25").attr("style", "width:10%").addClass("accountnumber").addClass("accountnumberlookup").addClass("inline").appendTo($(glcontrol));
    $('<label>').addClass("accountdescription").addClass("inline").appendTo($(glcontrol));
    $('<span>').addClass("accountselectionsearch").addClass("inline").appendTo($(glcontrol));

    glcontrol.appendTo($(container));

    $('<div>').addClass("gridContainer").addClass("accountselectiongrid").appendTo($(container));

}

function LoadGLAccounts(container, ledgerId, fiscalYearId, onSelect) {
   
    MakeServiceCall('GET', 'ledgers/' + ledgerId, null,
            
        function (data) {

            var numberOfAccountGroups = data.Data.AccountGroupLevels
            var columns = "[";

            for(var i = 0; i < numberOfAccountGroups; i++)
            {
                columns = columns + '{ "dataField": "Level' + (i +1) + '", "caption": "", "groupIndex": "' + i +'" },'
            }
                   
            columns = columns +  '"AccountNumber", "Description"]';

            MakeServiceCall('GET', 'accounts/fiscalyear/' + fiscalYearId, null,
                                
                function (data) {
                    LoadGLAccountGrid(container, data.Data, columns, onSelect);
                }

                , null)
              
        }
        , null
    )
}

function LoadGLAccountGrid(container, data, columns, onSelect)
{
    //create grid
    $(container).find(".gridContainer").dxDataGrid({
        columns: $.parseJSON(columns),
        dataSource: data,
        scrolling: {
            mode: "virtual"
        },
        grouping: {
            autoExpandAll: false,
        },
        selection: {
           mode: 'single' // 'multiple'
        },
        onSelectionChanged: function (selectedItems) {

            if (onSelect) {
                var data = selectedItems.selectedRowsData[0];
                onSelect(data);
            }
        }
       
    });

}