<%@ Page Title="DDI - Login" Language="C#" MasterPageFile="~/Blank.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DDI.UI.Web.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript" src="Scripts\jquery.validate.min.js"></script>
    <script type="text/javascript" src="Scripts\login.js"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="login">

        <div class="fieldblock">
            <label class="inline">Email</label>
            <input type="email" name="email" class="username" />
        </div>

        <div class="fieldblock">
            <label class="inline">Password</label>
            <input type="password" name="password" class="password" />
        </div>

        <div class="buttons">
            <input type="submit" class="loginbutton" value="Login" />
        </div>

        <div class="forgotpassword">
            <a href="#" class="forgotpasswordlink">Forgot Password?</a>
        </div>

    </div>

    <div class="forgotpasswordmodal" title="Forgot Password" style="display: none;">

        <div class="modalcontent">

            <div class="fieldblock">
                <label class="inline">Email</label>
                <input type="email" name="email" class="username" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submitforgotpassword" value="Submit" />
            </div>

        </div>

    </div>

</asp:Content>
