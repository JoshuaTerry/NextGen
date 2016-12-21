<%@ Page Title="DDI - Login" Language="C#" MasterPageFile="~/Blank.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DDI.UI.Web.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="Scripts\login.js"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="login">

        <div class="fieldblock">
            <label class="inline">Username</label>
            <input type="text" class="username" />
        </div>

        <div class="fieldblock">
            <label class="inline">Password</label>
            <input type="password" class="password" />
        </div>

        <div class="buttons">
            <input type="button" class="loginbutton" value="Login" />
        </div>

    </div>

</asp:Content>
