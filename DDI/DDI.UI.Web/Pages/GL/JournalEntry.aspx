<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="JournalEntry.aspx.cs" Inherits="DDI.UI.Web.Pages.GL.JournalEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../Scripts/JournalEntry.js"></script>
    <script type="text/javascript" src="../../Scripts/glaccountselection.js"></script>
    <link rel="stylesheet" href="../../../CSS/journals.css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="journaltypeselect" style="display: none">
        <div class="threecolumn">
            <img src="../../Images/OneTimeJournal.gif" alt="One-Time Journal" class="onetimejournaloption" />
            <img src="../../Images/RecurringJournal.gif" alt="Recurring Journal" class="recurringjournaloption" />
            <img src="../../Images/TemplateJournal.gif" alt="Journal Template" class="journaltemplateoption" />
        </div>
    </div>

    <div class="tabscontainer journaltabs">

        <ul>
            <li><a href="JournalDetail.aspx">Journal</a></li>
            <li><a href="JournalNotes.aspx">Notes</a></li>
            <li><a href="JournalCustom.aspx">Awesome Custom Stuff</a></li>
        </ul>

    </div>

    <input type="hidden" class="hidjournalid" />




</asp:Content>
