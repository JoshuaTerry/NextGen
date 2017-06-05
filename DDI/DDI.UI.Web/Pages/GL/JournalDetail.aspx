<%@ Page Title="" Language="C#" MasterPageFile="~/TabbedContent.Master" AutoEventWireup="true" CodeBehind="JournalDetail.aspx.cs" Inherits="DDI.UI.Web.Pages.GL.JournalDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {
            JournalDetailLoad();
        });

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="journalbody">

        <h1 class="journaltype"></h1>

        <div class="threecolumn inlinecontainer">
            <span class="journalnumbergroup">
                <label class="journallabel">Journal #: </label>
                <label class="JournalNumber"></label>
            </span>
            <span class="journalstatusgroup">
                <label class="journalstatus">Status: </label>
                <label class="JournalStatus"></label>
            </span>
            <span class="editjournalbutton hidebuttons floatright">
                <a href="#" class="editaccount editbutton">Edit</a>
            </span>
        </div>

        <div class="threecolumn">
            <div class="fieldblock">
                <label>Transaction Date:</label>
                <input type="text" class="editable TransactionDate datepicker" />
            </div>
            <div class="fieldblock">
                <label>Journal Memo:</label>
                <textarea rows="2" cols="70" maxlength="255" class="Comment"></textarea>
            </div>
            <div class="fieldblock">
                <label>Journal Information:</label>
            </div>
        </div>

        <div class="threecolumn">
            <span class="fieldblock reverseondategroup">
                <label>Reverse on this Date:</label>
                <input type="text" class="editable ReverseOnDate datepicker" />
            </span>
            <span class="fieldblock">
            </span>
            <span class="fieldblock">
                <label>Created by: </label>
                <label class="CreatedBy"></label>
                <label>on: </label>
                <label class="CreatedOn date"></label>
            </span>
        </div>

        <div>
            <a href="#" class="newjournalentrymodallink">New Item</a>
            <div class="journalgridcontainer">
                <div class="journalgrid"></div>
            </div>
        </div>

    </div>

</asp:Content>
