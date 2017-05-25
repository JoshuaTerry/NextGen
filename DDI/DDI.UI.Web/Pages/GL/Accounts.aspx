<%@ Page Title="DDI - Accounts" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Accounts.aspx.cs" Inherits="DDI.UI.Web.Accounts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../../Scripts/glaccountselection.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {

            LoadAccountSelectorGrid();

        });

    </script>

    <link rel="stylesheet" href="../../CSS/accounts.css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="as-accounts">
        <input type="hidden" class="hidaccountid" />
        <div class="gridContainer"></div>
    </div>
   
</asp:Content>
