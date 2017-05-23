<%@ Page Title="DDI - Security" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Security.aspx.cs" Inherits="DDI.UI.Web.Security" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../../Scripts/security.js"></script>
    <link rel="stylesheet" href="../../CSS/security.css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="adminsecurity">

        <div class="tabscontainer">

            <ul>
                <li><a href="#tab-groups">Groups</a></li>
                <li><a href="#tab-users">Users</a></li>
            </ul>

            <!-- Groups Tab -->
            <div id="tab-groups">
                <h1 class="GroupsSectionLabel">Groups</h1>
                <a href="#" class="newgroupmodal">New Group</a>
                <div class="groupstable"></div>

                <%--  <div class="newlinkcontainer">
                    <a href="#" class="newlink addnewgroup">New Group</a>
                </div>

                <div class="groupsgridcontainer"></div>

                <div class="groupmembersgridcontainer"></div>

                <div class="securitysettingsgridcontainer"></div>--%>
            </div>

            <div class="groupmodal" title="Groups" style="display: none;">

                <div class="modalcontent">

                    <div class="fieldblock">
                        <label>Name:</label>
                        <input type="text" class="gp-Name" />
                    </div>

                    <div class="fieldblock">
                        <label>Roles:</label>
                        <div class="gp-rolesdropdowncontainer"></div>
                    </div>

                    <div class="modalbuttons">
                        <input type="button" class="savebutton" value="Save" />
                        <a href="#" class="cancelgroupmodal">Cancel</a>
                    </div>
                </div>

            </div>

            <!-- Users Tab -->
            <div id="tab-users">
                <div class="usersgridcontainer">
                    <div class="usergrid"></div>
                </div>
            </div>
        </div>
    </div>

    <div class="newgroupmodal" title="New Group" style="display: none;">
        <div class="modalcontent">
        </div>
    </div>

    <div class="usermodal" title="New User" style="display: none;">
        <div class="modalcontent">

            <div class="fieldblock">
                <label>Email</label>
                <input type="email" name="email" class="user-Email" />
            </div>

            <div class="fieldblock">
                <label>Password</label>
                <input type="password" name="password" class="user-Password" />
            </div>

            <div class="fieldblock">
                <label>Confirm Password</label>
                <input type="password" name="confirmpassword" class="user-ConfirmPassword" />
            </div>

            <div class="fieldblock">
                <label>Default Business Unit:</label>
                <select class="user-DefaultBusinessUnitId"></select>
            </div>

            <div class="fieldblock">
                <div class="businessUnits">
                </div>
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

</asp:Content>
