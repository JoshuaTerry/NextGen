﻿<%@ Page Title="DDI - System Settings" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SystemSettings.aspx.cs" Inherits="DDI.UI.Web.SystemSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="..\..\Scripts\systemsettings.js"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="systemsettings">

        <div class="accordions nocontrols left-accordions">

            <h1>Cash Processing</h1>
            <div>
                <ul>
                    <li><a href="#" class="LoadBankAccounts">Bank Accounts</a></li>
                    <li><a href="#" class="LoadBatchGroups">Batch Groups</a></li>
                    <li><a href="#" class="LoadGeneralSettings">General Settings</a></li>
                    <li><a href="#" class="LoadReceiptItems">Receipt Item Types</a></li>
                </ul>
            </div>

            <h1>Common</h1>
            <div>
                <ul>
                    <li><a href="#" class="LoadAlternateIDTypes">Alternate ID Types</a></li>
                    <li><a href="#" class="LoadBusinessDate">Business Date</a></li>
                    <li><a href="#" class="LoadCalendarDates">Calendar Dates</a></li>
                    <li><a href="#" class="LoadDocumentTypes">Document Types</a></li>
                    <li><a href="#" class="LoadCommonHomeSreen">Home Screen</a></li>
                    <li><a href="#" class="LoadMergeFormSystem">Merge Form System</a></li>
                    <li><a href="#" class="LoadNotesSettings">Notes Settings</a></li>
                    <li><a href="#" class="LoadStatusCodes">Status Codes</a></li>
                    <li><a href="#" class="LoadTransactionCodes">Transaction Codes</a></li>
                </ul>
            </div>

            <h1>CRM</h1>
            <div>
                <ul>
                    <li><a href="#" class="LoadAlternateID">Alternate ID</a></li>
                    <li><a href="#" class="LoadClergy">Clergy</a></li>
                    <li><a href="#" class="LoadConstituentTypes">Constituent Types</a></li>
                    <li><a href="#" class="LoadContactInformation">Contact Information</a></li>
                    <li><a href="#" class="LoadDemographics">Demographics</a></li>
                    <li><a href="#" class="LoadDBA">Doing Business As</a></li>
                    <li><a href="#" class="LoadEducation">Education</a></li>
                    <li><a href="#" class="LoadGender">Gender</a></li>
                    <li><a href="#" class="LoadHubSearch">Hub Search</a></li>
                    <li><a href="#" class="LoadOrganization">Organization</a></li>
                    <li><a href="#" class="LoadPaymentPreferences">Payment Preferences</a></li>
                    <li><a href="#" class="LoadPersonal">Personal</a></li>
                    <li><a href="#" class="LoadPrefix">Prefix</a></li>
                    <li><a href="#" class="LoadProfessional">Professional</a></li>
                    <li><a href="#" class="LoadRegions">Regions</a></li>
                    <li><a href="#" class="LoadRelationship">Relationship</a></li>
                    <li><a href="#" class="LoadTags">Tags</a></li>
                </ul>
            </div>

            <h1>Donations</h1>
            <div>
                <ul>
                    <li><a href="#" class="LoadDonationSettings">Donation Settings</a></li>
                    <li><a href="#" class="LoadDonorSettings">Donor Settings</a></li>
                    <li><a href="#" class="LoadGLAccountAutoAssign">GL Account Auto Assign</a></li>
                    <li><a href="#" class="LoadDonationHomeScreen">Home Screen</a></li>
                </ul>
            </div>

            <h1>General Ledger</h1>
            <div>
                <ul>
                    <li><a href="#" class="LoadAccountingSettings">Accounting Settings</a></li>
                    <li><a href="#" class="LoadBudgetSettings">Budget Settings</a></li>
                    <li><a href="#" class="LoadChartAccountsSettings">Chart of Accounts Settings</a></li>
                    <li><a href="#" class="LoadEntities">Entities</a></li>
                    <li><a href="#" class="LoadFiscalYearSettings">Fiscal Year Settings</a></li>
                    <li><a href="#" class="LoadFundAccountingSettings">Fund Accounting Settings</a></li>
                    <li><a href="#" class="LoadGLFormatSettings">GL Format Settings</a></li>
                    <li><a href="#" class="LoadJournalSettings">Journal Settings</a></li>
                    <li><a href="#" class="LoadUtilities">Utilities</a></li>
                </ul>
            </div>

            <h1>Reports</h1>
            <div>
                <ul>
                    <li><a href="#" class="LoadPageFooters">Page Footers</a></li>
                    <li><a href="#" class="LoadPageHeaders">Page Headers</a></li>
                    <li><a href="#" class="LoadReportFooters">Report Footers</a></li>
                    <li><a href="#" class="LoadReportHeaders">Report Headers</a></li>
                </ul>
            </div>

            <h1>Custom Fields</h1>
            <div>
                <ul>
                    <li><a href="#" class="LoadCRMClientCustomFields">Client Custom Fields</a></li>
                    <li><a href="#" class="LoadDonationClientCustomFields">Donation Custom Fields</a></li>
                    <li><a href="#" class="LoadGLClientCustomFields">GL Format Custom Fields</a></li>
                </ul>
            </div>
        </div>

        <div class="contentcontainer">
            <div class="gridcontainer"></div>
        </div>
        

    </div>

</asp:Content>
