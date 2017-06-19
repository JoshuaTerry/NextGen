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

        <div class="contentcontainer groupscontainer" style="display: none;">

            <h1>Groups</h1>
            <hr />
            <div class="groupselectcontainer"></div>

            <div class="groupdetailscontainer" style="display: none;">

                <input type="hidden" class="hidgroupid" />

                <div class="fieldblock">
                    <label>Name</label>
                    <input type="text" class="gp-Name" maxlength="128" />
                </div>

                <div class="buttons">
                    <input type="button" value="Save" class="savegroup" />
                    <a href="cancel" class="cancelgroup">Cancel</a>
                </div>

                <br /><br />

                <div class="newlinkcontainer">
                    <a href="#" class="addroles">Manage Roles</a>
                </div>
                <div class="accordions nocontrols">

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

            <div class="rolesmodal" title="Add Roles..." style="display: none;">

                <div class="modalcontent">

                    <div class="roleselection"></div>

                    <div class="modalbuttons">
                        <input type="button" class="saveroles" value="Save" />
                        <a href="#" class="cancelmodal">Cancel</a>
                    </div>

                </div>

            </div>

        </div>

        <div class="contentcontainer userscontainer" style="display: none;">

            <h1>Users</h1>
            <hr />

            <div class="userselectcontainer"></div>

            <div class="userdetailscontainer">



            </div>

            <div class="newusermodal" title="New User..." style="display: none;">
                
                <div class="modalcontent">



                </div>

            </div>

        </div>

    </div>

</asp:Content>
