<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AccountDetails.aspx.cs" Inherits="DDI.UI.Web.Pages.GL.AccountDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="..\..\Scripts\accounts.js"></script>
    <script type="text/javascript" src="..\..\Scripts\glaccountselection.js"></script>
    <link rel="stylesheet" href="../../../CSS/accounts.css" />

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="UtilityMenuContainer" runat="server">

    <li><a href="#" class="copyaccount">Copy Account</a></li>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="accounts">
        <div class="tabscontainer">

            <ul>
                <li id="activity-and-budget-tab"><a href="ActivityAndBudget.aspx">Activity and Budget</a></li>
                <li id="summary-tab"><a href="AccountSummary.aspx">Summary</a></li>
                <li><a href="AccountTransactions.aspx">Transations</a></li>
            </ul>

            <div id="tab-activity">

            </div>

        </div>
    </div>

</asp:Content>
