<%@ Page Title="DDI - Accounts" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Accounts.aspx.cs" Inherits="DDI.UI.Web.Accounts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../../Scripts/glaccountselection.js"></script>

    <script type="text/javascript">
        
        $(document).ready(function () {

            LoadFiscalYears();

            $('.newaccountlink').click(function (e) {

                e.preventDefault();

                sessionStorage.setItem('FISCAL_YEAR_ID', $('.as-fiscalyear').val());
                sessionStorage.removeItem('ACCOUNT_ID');

                location.href = "/Pages/GL/AccountDetails.aspx";

            });

            $('.mergeaccounts').click(function (e) {

                e.preventDefault();

                modal = $('.mergeaccountmodal').dialog({
                    closeOnEscape: false,
                    modal: true,
                    width: 500,
                    resizable: false
                });

                // Load the account selectors
                var fiscalYearId = $('.as-fiscalyear').val();

                MakeServiceCall('GET', 'ledgers/businessunit/' + currentBusinessUnitId + '?fields=all', null, function (data) {

                    var ledgerId = data.Data[0].Id;

                    $('.ma-selectaccount1').html('');
                    $('.ma-selectaccount2').html('');

                    GLAccountSelector($('.ma-selectaccount1'), ledgerId, fiscalYearId);
                    GLAccountSelector($('.ma-selectaccount2'), ledgerId, fiscalYearId);

                });
                
                $('.cancelmodal').click(function (e) {

                    e.preventDefault();

                    CloseModal(modal);

                });

                $('.savebutton').unbind('click');

                $('.savebutton').click(function () {

                    var acctId1 = $('.ma-selectaccount1').find('.hidaccountid').val();
                    var acctId2 = $('.ma-selectaccount2').find('.hidaccountid').val();

                    MakeServiceCall('POST', 'accounts/' + acctId1 + '/merge/' + acctId2, null, function (data) {

                        CloseModal(modal);

                        DisplaySuccessMessage('Success', 'Accounts merged successfully.');

                        LoadAccountSelectorGrid($('.as-fiscalyear').val());

                    }, null);

                });

            });

        });

        function LoadFiscalYears() {

            MakeServiceCall('GET', 'businessunits/' + currentBusinessUnitId + '/currentfiscalyear', null, function (data) {
                
                PopulateDropDown('.as-fiscalyear', 'businessunits/' + currentBusinessUnitId + '/fiscalyears?fields=Id,DisplayName', '', '', data.Data.Id,
                    function () { // change
                        LoadAccountSelectorGrid($('.as-fiscalyear').val());
                    },
                    function () { // complete
                        LoadAccountSelectorGrid($('.as-fiscalyear').val());
                    });

            }, null);
            
        }

    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="UtilityMenuContainer" runat="server">

    <li><a href="#" class="mergeaccounts">Merge Accounts</a></li>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="as-accounts">
        <input type="hidden" class="hidaccountid" />

        <div class="fiscalyearselectioncontainr">
            Fiscal Year <select class="as-fiscalyear"></select>
        </div>

        <h1>Accounts</h1>
        
        <div class="newaccountlinkcontainer">
            <a href="#" class="newaccountlink">New Account</a>
        </div>

        <div class="gridContainer"></div>
    </div>

    <div class="mergeaccountmodal" style="display: none;">

        <div class="modalcontent">
            
            <div class="fieldblock">
                <label>Account 1</label>
                <div class="ma-selectaccount1"></div>
            </div>

            <div class="fieldblock">
                <label>Account 2</label>
                <div class="ma-selectaccount2"></div>
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href ="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>
   
</asp:Content>
