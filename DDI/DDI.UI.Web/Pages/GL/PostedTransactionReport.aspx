<%@ Page Title="DDI - Posted Transaction Report" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="PostedTransactionReport.aspx.cs" Inherits="DDI.UI.Web.PostedTransactionReport" %>

<%@ Register Assembly="DevExpress.XtraReports.v16.2.Web, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxDocumentViewer runat="server" Width="100%" Height="1100px">
        <settingsremotesource reporttypename=" DDI.WebApi.Reports.GL.PostedTransactionReport" serveruri="http://localhost:49490/DXReportService.svc" />
    </dx:ASPxDocumentViewer>


</asp:Content>
