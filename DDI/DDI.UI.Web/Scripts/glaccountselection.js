

function GLAccountSelector(container, ledgerId, fiscalYearId) {

    CreateGLAccountSelector(container)

    $('.accountnumberlookup').autocomplete({
        source: function (request, response) {
            MakeServiceCall('GET', 'accounts/lookup/' + request.term + "/" + ledgerId + "/" + fiscalYearId, null, function (result) {

                if (result.Data.length == 1) {
                    var item = {
                        label: result.Data[0].AccountNumber,
                        value: result.Data[0].Id
                    };
                    SelectAccountNumberLookup(item);
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
            SelectAccountNumberLookup(ui.item);
        }
    });

    
    $(".accountselectionsearch").click(function () {
        var grid = $('#gridContainer .dx-widget').length;
        if(grid == 0)
        {
            LoadGLAccounts('#gridContainer', ledgerId, fiscalYearId);
        }
        $("#gridContainer").show();
    });

    $(".accountnumber").focus();

}

function SelectAccountNumberLookup(item) {

    $(".accountdescription").html(item.description)
    $('.accountnumber').val(item.label);
    $('.hidaccountid').val(item.value);

}

function CreateGLAccountSelector(container)
{
    var glcontrol = $('<div>').addClass("inline-block");
        
    $('<input>').attr("type", "hidden").addClass("hidaccountid").appendTo($(container));
    $('<input>').attr("type", "text").attr("maxlength", "25").attr("style", "width:10%").addClass("accountnumber").addClass("accountnumberlookup").addClass("inline").appendTo($(glcontrol));
    $('<label>').addClass("accountdescription").addClass("inline").appendTo($(glcontrol));
    $('<span>').addClass("accountselectionsearch").addClass("inline").appendTo($(glcontrol));

    glcontrol.appendTo($(container));

    $('<div>').attr("Id", "gridContainer").addClass("accountselectiongrid").appendTo($(container));

}

function LoadGLAccounts(container, ledgerId, fiscalYearId) {
   
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
                    LoadGLAcountGrid(container, data.Data, columns);
                }

                , null)
              
        }
        , null
    )
}

function LoadGLAcountGrid(container, data, columns)
{
    //create grid
    $(container).dxDataGrid({
        columns: $.parseJSON(columns),
        dataSource: data,
        //cacheEnabled: true,
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
                $(".accountnumber").val(data.AccountNumber);
                $(".hidaccountid").val(data.Id);
                $(".accountdescription").text(data.Description)
                $(".accountnumber").focus();
                $(container).hide();
            }
        }
       
    });

}