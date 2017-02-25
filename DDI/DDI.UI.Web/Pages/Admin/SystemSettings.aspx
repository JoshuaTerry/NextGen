<%@ Page Title="DDI - System Settings" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SystemSettings.aspx.cs" Inherits="DDI.UI.Web.SystemSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <link rel="stylesheet" href="../../CSS/systemsettings.css" />
    
    <script type="text/javascript" src="..\..\Scripts\systemsettings.js"></script>
    <script type="text/javascript" src="..\..\Scripts\customfields.js"></script>
    <script type="text/javascript" src="..\..\Scripts\data.js"></script>

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
                    <li><a href="#" class="LoadTagGroupGrid">Tags</a></li>
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

    <div class="taggroupmodal" title="Tag Group" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="hidtaggroupid" />

            <div class="fieldblock">
                <label>Order</label>
                <input type="text" class="tg-Order" />
            </div>

            <div class="fieldblock">
                <label>Description</label>
                <input type="text" class="tg-Name" />
            </div>

            <div class="fieldblock">
                <label>Multi/Single Select</label>
                <select class="tg-Select">  
                   <option value="0">Single</option>
                   <option value="1">Multiple</option>
                </select>
            </div>

            <div class="fieldblock">
                <label>Is Active</label>
                <input type="checkbox" class="tg-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savetaggroup" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="tagmodal" title="Tag" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="hidtagid" />
            <input type="hidden" class="hidtagparentgroupid" />

            <div class="fieldblock">
                <label>Order</label>
                <input type="text" class="t-Order" />
            </div>

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="t-Code" />
            </div>

            <div class="fieldblock">
                <label>Description</label>
                <input type="text" class="t-Name" />
            </div>

            <div class="fieldblock">
                <label>Is Active</label>
                <input type="checkbox" class="t-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savetag" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

</asp:Content>
