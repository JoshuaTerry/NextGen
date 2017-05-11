

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
       
        if (grid == 0) {
            LoadGLAccounts(container, ledgerId, fiscalYearId);
            $(container).find('.gridContainer').show();
        }
        else{
            if ( ($(container).find('.gridContainer').css("display") == 'block')){
                $(container).find('.gridContainer').hide();
            }
            else {
                
               
               
                $(container).find('.gridContainer').show();
            }
        }

        //if ($(container).find(".hidaccountid").val().length > 0)
        //{
        //    var dataGrid = $(container).find('.gridContainer').dxDataGrid('instance')
        //    var key = $(container).find(".hidaccountid").val();
        //    dataGrid.selectRows(key);
        //}
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
    var glcontrol = $('<div>').addClass("inline-block");
        
    $('<input>').attr("type", "hidden").addClass("hidaccountid").appendTo($(container));
    $('<input>').attr("type", "text").attr("maxlength", "25").attr("style", "width:10%").addClass("accountnumber").addClass("accountnumberlookup").addClass("inline").appendTo($(glcontrol));
    $('<span>').addClass("accountselectionsearch").addClass("inline").appendTo($(glcontrol));
    $('<label>').addClass("accountdescription").addClass("inline").appendTo($(glcontrol));

    glcontrol.appendTo($(container));

    $('<div>').addClass("gridContainer").addClass("accountselectiongrid").appendTo($(container));

}

function LoadGLAccounts(container, ledgerId, fiscalYearId) {
   
    MakeServiceCall('GET', 'ledgers/' + ledgerId, null,
            
        function (data) {

            var numberOfAccountGroups = data.Data.AccountGroupLevels
            var columns = [];

            for(var i = 0; i < numberOfAccountGroups; i++)
            {
                columns.push({ dataField: "Level" + (i +1) , caption: "",  groupIndex:i });
            }
                   
            columns.push("AccountNumber");
            columns.push("Description");
            columns.push({ dataField: "Id"});
           
            MakeServiceCall('GET', 'accounts/fiscalyear/' + fiscalYearId, null,
                                
                function (data) {
                    LoadGLAccountGrid(container, data.Data, columns);
                }

                , null)
              
        }
        , null
    )
}

function LoadGLAccountGrid(container, data, columns)
{
    //create grid
    $(container).find(".gridContainer").dxDataGrid({
        columns: columns,
       
        dataSource:
            { 
                store: data,
                key: 'Id'
            },
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
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                $(container).find(".accountnumber").val(data.AccountNumber);
                $(container).find(".hidaccountid").val(data.Id);
                $(container).find(".accountdescription").text(data.Description)
                $(container).find(".accountnumber").focus();
                $(container).find(".gridContainer").hide();
            }
        },
        onContentReady: function () {
            if ($(container).find(".hidaccountid").val().length > 0) {
                var dataGrid = $(container).find('.gridContainer').dxDataGrid('instance')
                var key = $(container).find(".hidaccountid").val();
                dataGrid.selectRows(key);
            }
        }
    });

}

function LoadSelectedAccount(container, value)
{

    MakeServiceCall('GET', 'accounts/' + value, null, function (data) {
       
            $(container).find(".accountnumber").val(data.Data.AccountNumber);
            $(container).find(".hidaccountid").val(data.Data.Id);
            $(container).find(".accountdescription").text(data.Data.Name);
           
    }, null);

}