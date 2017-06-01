<%@ Page  Title="DDI - GL Accounts" Language="C#"  MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="GLAccountSelection.aspx.cs" Inherits="DDI.UI.Web.Pages.Common.GLAccountSelection" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="..\..\Scripts\glaccountselection.js"></script>
    <script type="text/javascript">

         $(document).ready(function () {

            var container = $(".accountcontrol");
            var ledgerId = '6A86AB9F-FA51-4EDB-8D0E-892CB84D50C1'
            var fiscalYearId = '18888509-AA22-42E6-85DD-0D29240E72EC'

            GLAccountSelector(container, ledgerId, fiscalYearId);
            //LoadSelectedAccount($('.accountcontrol'), 'f8ea9e78-4931-4a5d-b547-fef72bce2511');
  
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="accountcontrol fieldblock inline ">
         <label class="">GL Account Number: </label>
    </div>
</asp:Content>
