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

                <div class="securitysettingsgridcontainer"></div>

            </div>
            
            <!-- Users Tab -->
            <div id="tab-users">

                <a href="#" class="addnewuser">New User</a>

                <div class="usersgridcontainer"></div>

                <div class="userinfocontainer">

                    <div class="fieldblock">
                        <label>User ID</label>
                        <input type="text" class="userid" />
                    </div>

                    <div class="fieldblock">
                        <label>Name</label>
                        <input type="text" class="username" />
                    </div>

                    <div class="fieldblock">
                        <label>Email</label>
                        <input type="text" class="useremail" />
                    </div>

                    <div class="fieldblock">
                        <label>Status</label>
                        <input type="checkbox" class="userstatus" />
                    </div>

                </div>

                <div class="usergroupsgridcontainer"></div>

            </div>

        </div>

    </div>

    <div class="newgroupmodal" title="New Group" style="display: none;">

        <div class="modalcontent">



        </div>

    </div>

    <div class="newusermodal" title="New User" style="display: none;">

        <div class="modalcontent">



        </div>

    </div>

</asp:Content>
