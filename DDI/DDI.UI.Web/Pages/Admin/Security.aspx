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
                <div class="newlinkcontainer"><a href="#" class="newgroupmodal newlink">New Group</a></div>
                <div class="groupstable"></div>
            </div>

            <div class="groupmodal" title="Groups" style="display: none;">

                <input type="hidden" class="hidgroupid" />

                <div class="modalcontent">

                    <div class="fieldblock">
                        <label>Name:</label>
                        <input type="text" class="gp-Name required" maxlength="128" />
                    </div>

                    <div class="rolesmodal" style="display: none;">

                        <div class="fieldblock">
                            <label>Roles:</label>
                            <div class="gp-rolesdropdowncontainer"></div>
                        </div>
                    
                        <div class="modalbuttons roleaddbuttons">
                            <input type="button" class="saverolesbutton" value="Add" />
                            <a href="#" class="cancelrolesmodal">Cancel</a>
                        </div>

                    </div>

                    <div class="rolesgriditems" style="display: none;">

                        <div class="rolesgridcontainer"></div>

                        <input type="button" class="addrolesbutton" value="Add Roles" />
                    </div>

                    <div class="modalbuttons">
                        <input type="button" class="savegroupbutton" value="Save" />
                        <a href="#" class="cancelgroupmodal">Cancel</a>
                    </div>

                </div>

            </div>

            

            <!-- Users Tab -->
            <div id="tab-users">
                <div class="newlinkcontainer"><a href="#" class="newusermodal newlink">New User</a></div>
                <div class="usersgridcontainer">
                    <div class="usergrid"></div>
                </div>
            </div>
        </div>
    </div>

    <div class="newgroupmodal " title="New Group" style="display: none;">
        <div class="modalcontent">
        </div>
    </div>

    <div class="usermodal" title="User Maintenance" style="display: none;">
        <div class="modalcontent">

            <div class="user-editonly1" style="display: none;">
                <div class="fieldblock">
                    <label>Full Name</label>
                    <input type="text" class="user-FullName" maxlength="256" />
                </div>

                <div class="fieldblock">
                    <label>User Name</label>
                    <input type="text" class="user-UserName" maxlength="256" />
                </div>
            </div>

            <div class="fieldblock">
                <label>Email</label>
                <input type="email" name="email required" class="user-Email" />
            </div>

            <div class="user-editonly2" style="display: none;">
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
                    <select class="user-GroupId"></select>
                </div>

                <div class="fieldblock">
                    <div class="fieldblock">
                        <input type="hidden" class="rs-Constituent1Id" />
                        <label>Constituent</label>
                        <input type="text" class="rs-Constituent1Information constituentlookup" />
                    </div>
                </div>

                <div class="fieldblock">
                    <label>Active</label>
                    <input type="checkbox" class="user-IsActive" />
                </div>

            </div>

            
            <div class="modalbuttons">
                <input type="button" class="submituser" value="Save" />
                <a href ="#" class="cancelusermodal">Cancel</a>
            </div>

        </div>

    </div>

</asp:Content>
