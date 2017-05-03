<%@ Page Title="" Language="C#" MasterPageFile="~/TabbedContent.Master" AutoEventWireup="true" CodeBehind="AccountSummary.aspx.cs" Inherits="DDI.UI.Web.Pages.GL.AccountSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="..\..\CSS\accounts.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>Summary</h1>

    <div class="accountsegmentscontainer onecolumn">

    </div>
    
    <div>
        <div class="summaryprompts">
            <label>GL Account: </label>
        </div>
        <div class="summaryleftinput">
            <input type="text" class="AccountNumber readonly" readonly="read-only" />
            <a href="#" title="" class="editaccount editbuttonleft"></a>
        </div>
        <div class="summaryrightinput">
            <input type="checkbox" class="IsActive" /> Active
        </div>
    </div>

    <div>
        <div class="summaryprompts">
            <label>Description: </label>
        </div>
        <div class="summarysegmenttext">
            <input type="text" class="Name" />
        </div>
    </div>

    <div>
        <div class="accountgroup1">
            <div class="summaryprompts">
                <label class="group1prompt"></label>
            </div>
            <div class="summaryleftinput">
                <select class="group1dropdown" />
                <a href="#" title="" class="editgroup1 editbuttonleft"></a>
            </div>
        </div>

        <div class="summaryrightinput">
            <label>Balances Normally: </label>
            <select class="IsNormallyDebit">
                <option value="0">Credit</option>
                <option value="1">Debit</option>
            </select>
        </div>
    </div>

    <div>
        <div class="accountgroup2">
            <div class="summaryprompts">
                <label class="group2prompt"></label>
            </div>
            <div class="summaryleftinput">
                <select class="group2dropdown" />
                <a href="#" title="" class="editgroup2 editbuttonleft"></a>
            </div>
        </div>

        <div class="summaryrightinput">
            <label>Beginning Balance: </label>
            <input type="text" class="BeginningBalance money justright" disabled="disabled"" />
        </div>
    </div>

    <div">
        <div class="accountgroup3">
            <div class="summaryprompts">
                <label class="group3prompt"></label>
            </div>
            <div class="summaryleftinput">
            <select class="group3dropdown" />
            <a href="#" title="" class="editgroup3 editbuttonleft"></a>
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
            <a href="#" title="" class="editgroup4 editbuttonleft"></a>
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
            <a href="#" title="" class="browseclosingaccount editbuttonleft"></a>
        </div>
        <div>
            <label> Accumulated Revenue</label>
        </div>
    </div>



</asp:Content>
