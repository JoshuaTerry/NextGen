<%@ Page Title="" Language="C#" MasterPageFile="~/TabbedContent.Master" AutoEventWireup="true" CodeBehind="Notes.aspx.cs" Inherits="DDI.UI.Web.Pages.Common.Notes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../../Scripts/Notes.js"></script>

    <script type="text/javascript">
        
        shownotealert = false;

    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="tab-notes">

        <div class="tabscontainer inner">
                <h1>Note Detail<a href="#" title="New" class="newnotesdetailmodallink newbutton"></a></h1>
                <div class="notedetailsgridcontainer"></div>
        </div>
    </div>
</asp:Content>
