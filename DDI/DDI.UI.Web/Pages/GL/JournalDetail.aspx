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

        <div class="threecolumn">
            <div class="fieldblock">
                <label>Transaction Date:</label>
                <input type="text" class="editable TransactionDate datepicker" />
            </div>
            <div class="fieldblock">
                <label>Journal Memo:</label>
                <input class="Comment" type="text" />
            </div>
        </div>

        <div>
            <a href="#" class="newjournalentrymodallink">New Item</a>
            <div class="journalgridcontainer">
                <div class="journalgrid"></div>
            </div>
        </div>

    </div>

</asp:Content>
