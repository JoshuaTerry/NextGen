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
            <button></button>
        </div>
    </div>



</asp:Content>
