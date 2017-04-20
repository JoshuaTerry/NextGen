<%@ Page Title="DDI - Accounts" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Accounts.aspx.cs" Inherits="DDI.UI.Web.Accounts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../../../Scripts/glaccountselection.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {

            // LedgerId 7BAFBB1E-A2DC-4D85-9542-229378F8DBC7
            // FY ID	1A67ED6F-0FD8-47CD-9476-DC09D94E5F28

            //GLAccountSelector(container, ledgerId, fiscalYearId)

            GLAccountSelector('.as-accounts', '7BAFBB1E-A2DC-4D85-9542-229378F8DBC7', '1A67ED6F-0FD8-47CD-9476-DC09D94E5F28');

        });

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="as-accounts"></div>

</asp:Content>
