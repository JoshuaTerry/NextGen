<%@ Page Title="DDI - Security" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Security.aspx.cs" Inherits="DDI.UI.Web.Security" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../../Scripts/security.js"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="adminsecurity">

        <div class="tabscontainer">

            <ul>
                <li><a href="#tab-groups">Groups</a></li>
                <li><a href="#tab-Users">Users</a></li>
            </ul>

            <!-- Groups Tab -->
            <div id="tab-groups">

                <a href="#" class="addnewgroup">New Group</a>

                <div class="groupsgridcontainer"></div>

                <div class="groupmembersgridcontainer"></div>

                <div class="securitysettingsgridcontianer"></div>

            </div>
            
            <!-- Users Tab -->
            <div id="tab-users">

                <a href="#" class="addnewuser">New User</a>

                <div class="usersgridcontainer"></div>

                <div class="userinfocontainer">



                </div>

                <div class="usergroupsgridcontainer"></div>

            </div>

        </div>

    </div>

</asp:Content>
