﻿<%@ Page Title="DDI - System Settings" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SystemSettings.aspx.cs" Inherits="DDI.UI.Web.SystemSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <link rel="stylesheet" href="../../CSS/systemsettings.css" />
    
    <script type="text/javascript" src="..\..\Scripts\systemsettings.js"></script>
    <script type="text/javascript" src="..\..\Scripts\customfields.js"></script>

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
                    <li><a href="#" class="LoadAccounting">Accounting Settings</a></li>
                    <li><a href="#" class="LoadBudget">Budget Settings</a></li>
                    <li><a href="#" class="LoadChartAccounts">Chart of Accounts Settings</a></li>
                    <li><a href="#" class="LoadEntities">Entities</a></li>
                    <li><a href="#" class="LoadFiscalYear">Fiscal Year Settings</a></li>
                    <li><a href="#" class="LoadFundAccounting">Fund Accounting Settings</a></li>
                    <li><a href="#" class="LoadGLFormat">GL Format Settings</a></li>
                    <li><a href="#" class="LoadJournal">Journal Settings</a></li>
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

    <div class="newcustomfieldmodal" title="New Custom Field" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="cfid" />

            <div class="twocolumn">

                <div class="fieldproperties" style="width: 100%;">
                    <div class="fieldblock">
                        <label>Label Text</label>
                        <input type="text" class="cflabel" />
                    </div>

                    <div class="fieldblock">
                        <label>Type</label>
                        <select class="cftype"></select>
                    </div>

                    <div class="fieldblock">
                        <label>Display Order</label>
                        <input type="text" class="cforder" />
                    </div>

                    <div class="fieldblock">
                        <label class="inline">Is Required</label>
                        <input type="checkbox" class="cfisrequired" />
                    </div>

                    <div class="fieldblock decimalplacecontainer" style="display: none;">
                        <label>Decimal Places</label>
                        <input type="number" class="cfdecimalplaces" />
                    </div>

                    <div class="minmaxvalues" style="display: none;">

                        <div class="fieldblock">
                            <label>Min Value</label>
                            <input type="text" class="cfminvalue" />
                        </div>

                        <div class="fieldblock">
                            <label>Max Value</label>
                            <input type="text" class="cfmaxvalue" />
                        </div>
                            
                    </div>
                </div>
                
                <div class="options">

                    <fieldset>
                        <legend>Custom Field Options</legend>

                        <div class="inline">

                            <div class="fieldblock">
                                <label>Code</label>
                                <input type="text" class="cfoptioncode" maxlength="4" />
                            </div>

                            <div class="fieldblock">
                                <label>Description</label>
                                <input type="text" class="cfoptiondesc" />
                            </div>

                            <div class="fieldblock">
                                <label>Order</label>
                                <input type="number" class="cfoptionorder" />
                            </div>

                            <div class="fieldblock">
                                <label>&nbsp;</label>
                                <input type="button" class="addoption noclear" value="Add" />
                            </div>

                        </div>

                        <table class="tempoptions"></table>

                    </fieldset>

                </div>

            </div>

            <div class="modalbuttons">
                <input type="button" class="submitcf" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="denominationmodal" title="Denomination" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="den-Id" />

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="den-Code" />
            </div>

            <div class="fieldblock">
                <label>Name</label>
                <input type="text" class="den-Name" />
            </div>

            <div class="fieldblock">
                <label>Religion</label>
                <select class="den-Religion">
                    <option value="0">None</option>
                    <option value="1">Catholic</option>
                    <option value="2">Protestant</option>
                    <option value="3">Orthodox</option>
                    <option value="4">Jewish</option>
                    <option value="5">Islam</option>
                    <option value="6">Hindu</option>
                    <option value="7">Buddhist</option>
                    <option value="8">Taoist</option>
                    <option value="9">Shinto</option>
                    <option value="10">Sikh</option>
                    <option value="11">Bahai</option>
                </select>
            </div>

            <div class="fieldblock">
                <label>Affiliation</label>
                <select class="den-Affiliation">
                    <option value="0">None</option>
                    <option value="1">Affiliated</option>
                    <option value="2">Unaffiliated</option>
                    <option value="3">Other</option>
                </select>
            </div>


            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="den-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submitden" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="ethnicitymodal" title="Ethnicity" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="eth-Id" />

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="eth-Code" />
            </div>

            <div class="fieldblock">
                <label>Name</label>
                <input type="text" class="eth-Name" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="eth-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submiteth" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>
 <div class="languagemodal" title="Language" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="lang-Id" />

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="lang-Code" />
            </div>

            <div class="fieldblock">
                <label>Name</label>
                <input type="text" class="lang-Name" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="lang-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submitlang" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>
 <div class="languagemodal" title="Language" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="lang-Id" />

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="lang-Code" />
            </div>

            <div class="fieldblock">
                <label>Name</label>
                <input type="text" class="lang-Name" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="lang-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submitlang" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>
    <div class="degreemodal" title="Degree" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="degreeId" />

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="deg-Code" />
            </div>

            <div class="fieldblock">
                <label>Name</label>
                <input type="text" class="deg-Name" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="deg-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submitdeg" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>
        <div class="educationLevelmodal" title="Education Level" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="educationLevelId" />

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="eduLev-Code" />
            </div>

            <div class="fieldblock">
                <label>Name</label>
                <input type="text" class="eduLev-Name" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="eduLev-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submiteduLev" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>
       <div class="schoolmodal" title="Schools" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="schoolId" />

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="sch-Code" />
            </div>

            <div class="fieldblock">
                <label>Name</label>
                <input type="text" class="sch-Name" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="sch-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submitsch" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>
</asp:Content>
