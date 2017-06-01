<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="JournalEntry.aspx.cs" Inherits="DDI.UI.Web.Pages.GL.JournalEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>General Journal Entry</h1>

    <div class="threecolumn">
        <div class="fieldblock">
            <label>Transaction Date:</label>
            <input type="text" class="editable TransactionDate datepicker" />
        </div>
        <div class="fieldblock">
            <label>Journal Memo:</label>
        </div>
    </div>

    <div>
        <a href="#" class="newjournalentrymodallink">New Item</a>
        <div class="journalgridcontainer">
            <div class="journalgrid"></div>
        </div>
    </div>



</asp:Content>
