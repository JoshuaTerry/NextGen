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
                <h1 class ="GroupsSectionLabel">Groups</h1>
                    <div>
                        <div class="groupstable"></div>
                    </div>

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
                <input type="text" class="dba-Name" />
            </div>

            <div class="threecolumn">

                <div class="fieldblock">
                    <label>Assigned Roles</label>
                    <select class="gp-Roles" size="5"> 
                    </select>

                </div>
                 
                <div>
                    <input type="button" value="Add" /> <br />
                    <input type="button" value="Remove" /> <br />
                </div>

                <div class="fieldblock">
                    <label>Available Roles</label>
                    <select class="AllRoles" size="5"> 
                    </select>
                </div>

                <div class="modalbuttons">
                    <input type="button" class="savebutton" value="Save" />
                    <a href="#" class="cancelmodal">Cancel</a>
                </div>

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

    <div class="usermodal" title="User Maintenance" style="display: none;">
        <div class="modalcontent">

            <div class="fieldblock">
                <label>Full Name</label>
                <input type="text" class="user-FullName" />
            </div>

            <div class="fieldblock">
                <label>User Name</label>
                <input type="text" class="user-UserName" />
            </div>
        
            <div class="fieldblock">
                <label>Email</label>
                <input type="email" name="email" class="user-Email" />
            </div>

            <div class="fieldblock">
                <label>Default Business Unit</label>
                <select class="user-DefaultBusinessUnitId"></select>
            </div>

            <div class="fieldblock">
                <label>Business Units</label>
                <div class="tagselect user-BusinessUnits"></div>
            </div>

            <div class="fieldblock">
                <label>Group</label>
                <select class="user-Groups"></select>
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="user-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submituser" value="Save" />
                <a href ="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

</asp:Content>
