<%@ Page Title="DDI - Accounts" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Accounts.aspx.cs" Inherits="DDI.UI.Web.Accounts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../../Scripts/glaccountselection.js"></script>

    <script type="text/javascript">
        
        $(document).ready(function () {

            LoadFiscalYears();

            $('.newaccountlink').click(function (e) {

                e.preventDefault();

                sessionStorage.setItem('FISCAL_YEAR_ID', $('.as-fiscalyear').val());

                location.href = "/Pages/GL/AccountDetails.aspx";

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

    <link rel="stylesheet" href="../../CSS/accounts.css" />

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
   
</asp:Content>
