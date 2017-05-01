<%@ Page Title="" Language="C#" MasterPageFile="~/TabbedContent.Master" AutoEventWireup="true" CodeBehind="AccountSummary.aspx.cs" Inherits="DDI.UI.Web.Pages.GL.AccountSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>Summary</h1>

    <div class="accountsegmentscontainer onecolumn">

    </div>
    
    <div class="twocolumn">
        <div class="glaccount">
            <input type="text" class="GLAccount readonly" readonly="read-only" />
        </div>
        <div class="justright">
            <input type="checkbox" class="Active" /> Active
        </div>
    </div>

    <div class="fieldblock">
        <input type="text" class="Description" />
    </div>

    <div class="twocolumn">
        <div class="accountcategory1">
            <label class="category1prompt"></label>
            <select class="category1dropdown" />
            <a href="#" title="" class="editcategory1 editbutton"></a>
            <a href="#" title="" class="newcategory1 editbutton"></a>
        </div>

        <div>
            <label>Balances Normally: </label>
            <select class="NormalBalace">
                <option value="0">Credit</option>
                <option value="1">Debit</option>
            </select>
        </div>
    </div>

    <div class="twocolumn">
        <div class="accountcategory2">
            <label class="category2prompt"></label>
            <select class="category2dropdown" />
            <a href="#" title="" class="editcategory2 editbutton"></a>
            <a href="#" title="" class="newcategory2 editbutton"></a>
        </div>

        <div>
            <label>Beginning Balance: </label>
            <select class="NormalBalace">
                <option value="0">Credit</option>
                <option value="1">Debit</option>
            </select>
        </div>
    </div>




</asp:Content>
