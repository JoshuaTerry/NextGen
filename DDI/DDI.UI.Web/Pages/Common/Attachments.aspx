<%@ Page Title="" Language="C#" MasterPageFile="~/TabbedContent.Master" AutoEventWireup="true" CodeBehind="Attachments.aspx.cs" Inherits="DDI.UI.Web.Pages.Common.Attachments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../Scripts/fileupload/fileupload.js"></script>
    <script type="text/javascript" src="../../Scripts/attachments.js"></script>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   <div id="tab-attachments" class="scrollable">
        <h1>Attachments</h1>
        <div class="attachmentscontainer"></div>
    </div>
</asp:Content>
