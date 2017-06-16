<%@ Page Title="DDI - Security" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Security.aspx.cs" Inherits="DDI.UI.Web.Security" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../../Scripts/security.js"></script>
    <link rel="stylesheet" href="../../CSS/security.css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="adminsecurity">

        <div class="securitynav">
            <ul>
                <li><a href="#" class="groupnav">Groups</a></li>
                <li><a href="#" class="usernav">Users</a></li>
            </ul>
        </div>

        <div class="contentcontainer groupscotainer" style="display: none;">

            <h1>Groups</h1>

            <div class="groupselectcontainer"></div>

            <div class="groupdetailscontainer">

                <input type="hidden" class="hidgroupid" />

                <div class="fieldblock">
                    <label>Name</label>
                    <input type="text" class="gp-Name" maxlength="128" />
                </div>

                <div class="accordions">

                    <h1>Roles</h1>
                    <div>
                        <div class="grouprolesgridcontainer"></div>
                    </div>

                    <h1>Users in Group</h1>
                    <div>
                        <div class="groupusersgridcontainer"></div>
                    </div>

                </div>

            </div>

        </div>

        <div class="contentcontainer userscontainer" style="display: none;">

            <h1>Users</h1>

            <div class="userselectcontainer"></div>

            <div class="userdetailscontainer">



            </div>

        </div>

    </div>

</asp:Content>
