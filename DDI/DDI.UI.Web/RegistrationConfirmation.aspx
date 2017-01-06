<%@ Page Title="DDI - Registration Confirmation" Language="C#" MasterPageFile="~/Blank.Master" AutoEventWireup="true" CodeBehind="RegistrationConfirmation.aspx.cs" Inherits="DDI.UI.Web.RegistrationConfirmation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="Scripts/registrationconfirmation.js"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="registrationconfirmation">
        
        <h1>Thank You</h1>

        <p>You're registration is complete.</p>

        <p>You will be automatically redirected within <span class="timer">15</span> seconds, or you can click <a href="/Login.aspx">here</a> to log in.</p>

    </div>

</asp:Content>
