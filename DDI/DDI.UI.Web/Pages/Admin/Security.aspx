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
                
                <div class="newlinkcontainer">
                    <a href="#" class="newlink addnewgroup">New Group</a>
                </div>

                <div class="groupsgridcontainer"></div>

                <div class="groupmembersgridcontainer"></div>

                <div class="securitysettingsgridcontainer"></div>

            </div>
            
            <!-- Users Tab -->
            <div id="tab-users">

                <div class="newlinkcontainer">
                    <a href="#" class="newlink addnewuser">New User</a>
                </div>

                <div class="usersgridcontainer"></div>

                <fieldset>
                    <legend>User Information</legend>

                    <div class="userinfocontainer">

                        <div class="twocolumn">

                            <div class="fieldblock">
                                <label>User ID</label>
                                <input type="text" class="userid" />
                            </div>

                            <div class="fieldblock">
                                <label>Name</label>
                                <input type="text" class="username" />
                            </div>

                        </div>
                        
                        <div class="twocolumn">

                            <div class="fieldblock">
                                <label>Email</label>
                                <input type="text" class="useremail" />
                            </div>

                            <div class="fieldblock">
                                <label>Status</label>
                                <input type="checkbox" class="userstatus" />
                            </div>

                        </div>
                        
                    </div>
                    
                </fieldset>

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

            <div class="fieldblock">
                <label>Email</label>
                <input type="email" name="email" class="newusername" />
            </div>

            <div class="fieldblock">
                <label>Password</label>
                <input type="password" name="password" class="newpassword" />
            </div>

            <div class="fieldblock">
                <label>Confirm Password</label>
                <input type="password" name="confirmpassword" class="newconfirmpassword" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submitnewuser" value="Submit" />
            </div>

        </div>

    </div>

</asp:Content>
