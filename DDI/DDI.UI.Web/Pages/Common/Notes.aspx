<%@ Page Title="" Language="C#" MasterPageFile="~/TabbedContent.Master" AutoEventWireup="true" CodeBehind="Notes.aspx.cs" Inherits="DDI.UI.Web.Pages.Common.Notes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../../Scripts/Notes.js"></script>

    <script type="text/javascript">
        
        shownotealert = false;

    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="tab-notes">
        <h1>Note Detail</h1>
        <div class="notedetailsgridcontainer">
            <a href="#" title="New" class="newnotesdetailmodallink newbutton"></a>
        </div>
    </div>

</asp:Content>
