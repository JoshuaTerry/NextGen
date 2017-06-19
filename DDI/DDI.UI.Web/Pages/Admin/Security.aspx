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

                <h1>Group Details</h1>
                <hr />

                <div class="fieldblock required">
                    <label>Name</label>
                    <input type="text" class="gp-Name" maxlength="128" />
                </div>

                <div class="buttons">
                    <input type="button" value="Save" class="savegroup" />
                    <a href="cancel" class="cancelgroup">Cancel</a>
                </div>

                <br /><br />

                <div class="accordions nocontrols">

                    <h1>Roles</h1>
                    <div>
                        <div class="newlinkcontainer">
                            <a href="#" class="addroles">Manage Roles</a>
                        </div>
                        <div class="grouprolesgridcontainer"></div>
                    </div>

                    <h1>Users in Group</h1>
                    <div>
                        <div class="groupusersgridcontainer"></div>
                    </div>

                </div>

            </div>

            <div class="rolesmodal" title="Manage Roles..." style="display: none;">

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

                <input type="hidden" class="hiduserid" />

                <h1>User Details</h1>
                <hr />

                <div class="twocolumn">

                    <div>

                        <div class="fieldblock required">
                            <label>User Name</label>
                            <input type="text" class="user-UserName" maxlength="256" />
                        </div>

                        <div class="fieldblock">
                            <label>Full Name</label>
                            <input type="text" class="user-FullName" maxlength="256" />
                        </div>

                        <div class="fieldblock">
                            <label>E-mail</label>
                            <input type="text" class="user-Email email" />
                        </div>

                        <div class="fieldblock">
                            <label>Phone Number</label>
                            <input type="text" class="user-PhoneNumber phone" />
                        </div>

                    </div>

                    <div>

                        <div class="fieldblock">
                            <label>Is Active</label>
                            <input type="checkbox" class="user-IsActive" />
                        </div>

                        <div class="fieldblock">
                            <label>Constituent</label>
                            <input type="text" class="rs-Constituent1Information constituentlookup" />
                            <input type="hidden" class="rs-Constituent1Id" />
                        </div>

                        <div class="fieldblock">
                            <label>Business Unit</label>
                            <select class="user-DefaultBusinessUnitId"></select>
                        </div>

                    </div>

                </div>

                <div class="buttons">
                    <input type="button" value="Save" class="saveuser" />
                    <a href="cancel" class="canceluser">Cancel</a>
                </div>

                <br /><br />

                <div class="twocolumn">

                    <div>
                        <h2>Groups</h2>
                        <hr />
                        <div class="newlinkcontainer">
                            <a href="#" class="managegroups">Manage Groups</a>
                        </div>
                        <div class="usergroupsgridcontainer"></div>
                    </div>

                    <div>
                        <h2>Business Units</h2>
                        <hr />
                        <div class="newlinkcontainer">
                            <a href="#" class="managebu">Manage Business Units</a>
                        </div>
                        <div class="userbugridcontainer"></div>
                    </div>

                </div>

            </div>

            <div class="usergroupsmodal" title="Manage Groups..." style="display: none;">
                
                <div class="modalcontent">

                    <div class="usergroupsselection"></div>
                    
                    <div class="modalbuttons">
                        <input type="button" class="saveusergroups" value="Save" />
                        <a href="#" class="cancelmodal">Cancel</a>
                    </div>

                </div>

            </div>

            <div class="userbusinessunitsmodal" title="Manage Business Units..." style="display: none;">

                <div class="modalcontent">

                    <div class="userbuselection"></div>

                    <div class="modalbuttons">
                        <input type="button" class="saveuserbu" value="Save" />
                        <a href="#" class="cancelmodal">Cancel</a>
                    </div>

                </div>

            </div>

        </div>

    </div>

</asp:Content>
