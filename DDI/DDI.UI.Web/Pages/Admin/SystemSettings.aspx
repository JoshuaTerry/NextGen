<%@ Page Title="DDI - System Settings" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SystemSettings.aspx.cs" Inherits="DDI.UI.Web.SystemSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="..\..\Scripts\systemsettings.js"></script>
    <link rel="stylesheet" href="..\..\CSS\systemsettings.css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="systemsettings">

        <div class="accordions nocontrols left-accordions">

            <h1>Cash Processing</h1>
            <div>
                <ul>
                    <li><a href="#" class="">Bank Accounts</a></li>
                    <li><a href="#" class="">Batch Groups</a></li>
                    <li><a href="#" class="">General Settings</a></li>
                    <li><a href="#" class="">Receipt Item Types</a></li>
                </ul>
            </div>

            <h1>Common</h1>
            <div>
                <ul>
                    <li><a href="#" class="">Alternate ID Types</a></li>
                    <li><a href="#" class="">Business Date</a></li>
                    <li><a href="#" class="">Calendar Dates</a></li>
                    <li><a href="#" class="">Document Types</a></li>
                    <li><a href="#" class="">Home Screen</a></li>
                    <li><a href="#" class="">Merge Form System</a></li>
                    <li><a href="#" class="">Notes Settings</a></li>
                    <li><a href="#" class="">Status Codes</a></li>
                    <li><a href="#" class="">Transaction Codes</a></li>
                </ul>
            </div>

            <h1>CRM</h1>
            <div>
                <ul>
                    <li><a href="#" class="">Alternate ID</a></li>
                    <li><a href="#" class="">Clergy</a></li>
                    <li><a href="#" class="">Client Custom Fields</a></li>
                    <li><a href="#" class="">Constituent Types</a></li>
                    <li><a href="#" class="">Contact Information</a></li>
                    <li><a href="#" class="">Demographics</a></li>
                    <li><a href="#" class="">Doing Business As</a></li>
                    <li><a href="#" class="">Education</a></li>
                    <li><a href="#" class="">Gender</a></li>
                    <li><a href="#" class="">Hub Search</a></li>
                    <li><a href="#" class="">Organization</a></li>
                    <li><a href="#" class="">Payment Preferences</a></li>
                    <li><a href="#" class="">Personal</a></li>
                    <li><a href="#" class="">Prefix</a></li>
                    <li><a href="#" class="">Professional</a></li>
                    <li><a href="#" class="">Regions</a></li>
                    <li><a href="#" class="">Relationship</a></li>
                    <li><a href="#" class="">Tags</a></li>
                </ul>
            </div>

            <h1>Donations</h1>
            <div>
                <ul>
                    <li><a href="#" class="">Client Custom Fields</a></li>
                    <li><a href="#" class="">Donation Settings</a></li>
                    <li><a href="#" class="">Donor Settings</a></li>
                    <li><a href="#" class="">GL Account Auto Assing</a></li>
                    <li><a href="#" class="">Home Screen</a></li>
                </ul>
            </div>

            <h1>General Ledger</h1>
            <div>
                <ul>
                    <li><a href="#" class="">Accounting Settings</a></li>
                    <li><a href="#" class="">Budget Settings</a></li>
                    <li><a href="#" class="">Chart of Accounts Settings</a></li>
                    <li><a href="#" class="">Client Custom Fields</a></li>
                    <li><a href="#" class="">Entities</a></li>
                    <li><a href="#" class="">Fiscal Year Settings</a></li>
                    <li><a href="#" class="">Fund Accounting Settings</a></li>
                    <li><a href="#" class="">GL Format Settings</a></li>
                    <li><a href="#" class="">Journal Settings</a></li>
                    <li><a href="#" class="">Utilities</a></li>
                </ul>
            </div>

            <h1>Reports</h1>
            <div>
                <ul>
                    <li><a href="#" class="">Page Footers</a></li>
                    <li><a href="#" class="">Page Headers</a></li>
                    <li><a href="#" class="">Report Footers</a></li>
                    <li><a href="#" class="">Report Headers</a></li>
                </ul>
            </div>

        </div>

    </div>

</asp:Content>
