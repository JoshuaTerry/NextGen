<%@ Page Title="" Language="C#" MasterPageFile="~/TabbedContent.Master" AutoEventWireup="true" CodeBehind="AccountSummary.aspx.cs" Inherits="DDI.UI.Web.Pages.GL.AccountSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>Summary</h1>

    <div class="accountsegmentscontainer onecolumn">

    </div>
    
    <div class="twocolumn">
        <div>
            <label class="summaryprompts">GL Account: </label>
            <input type="text" class="AccountNumber readonly" readonly="read-only" />
            <a href="#" title="" class="editaccount editbutton"></a>
        </div>
        <div>
            <input type="checkbox" class="IsActive" /> Active
        </div>
    </div>

    <div>
        <label>Description: </label>
        <input type="text" class="Name fieldblock" />
    </div>

    <div class="twocolumn">
        <div class="accountgroup1">
            <label class="group1prompt summaryprompts"></label>
            <select class="group1dropdown" />
            <a href="#" title="" class="editgroup1 editbutton"></a>
        </div>

        <div>
            <label>Balances Normally: </label>
            <select class="IsNormallyDebit">
                <option value="0">Credit</option>
                <option value="1">Debit</option>
            </select>
        </div>
    </div>

    <div class="twocolumn">
        <div class="accountgroup2">
            <label class="group2prompt summaryprompts"></label>
            <select class="group2dropdown" />
            <a href="#" title="" class="editgroup2 editbutton"></a>
        </div>

        <div>
            <label>Beginning Balance: </label>
            <input type="text" class="BeginningBalance money justright" disabled="disabled"" />
        </div>
    </div>

    <div class="twocolumn">
        <div class="accountgroup3">
            <label class="group3prompt summaryprompts"></label>
            <select class="group3dropdown" />
            <a href="#" title="" class="editgroup3 editbutton"></a>
        </div>

        <div>
            <label>Activity: </label>
            <input type="text" class="Activity money justright" disabled="disabled"" />
        </div>
    </div>

    <div class="twocolumn">
        <div class="accountgroup4">
            <label class="group4prompt summaryprompts"></label>
            <select class="group4dropdown" />
            <a href="#" title="" class="editgroup4 editbutton"></a>
        </div>

        <div>
            <label>Activity: </label>
            <input type="text" class="EndingBalance money justright" disabled="disabled"" />
        </div>
    </div>

    <div class="twocolumn">
        <div class="closingaccountgroup">
            <label class="summaryprompts">Closing Account: </label>
            <input type="text" class="ClosingAccount"/>
            <a href="#" title="" class="browseclosingaccount editbutton"></a>
        </div>
        <div>
            <label> Accumulated Revenue</label>
        </div>
    </div>



</asp:Content>
