﻿<%@ Page Title="DDI - System Settings" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SystemSettings.aspx.cs" Inherits="DDI.UI.Web.SystemSettings" %>

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
                    <li><a href="#" class="LoadNote">Notes Settings</a></li>
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
                    <li><a href="#" class="LoadTagGroup">Tags</a></li>
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
                    <li><a href="#" class="LoadBudget">Budget Settings</a></li>
                    <li><a href="#" class="LoadChartAccounts">Chart of Accounts Settings</a></li>
                    <li><a href="#" class="LoadEntities">Business Unit Settings</a></li>
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
                    <li><a href="#" class="LoadCRMClientCustomFields">CRM Custom Fields</a></li>
                    <li><a href="#" class="LoadDonationClientCustomFields">Donation Custom Fields</a></li>
                    <li><a href="#" class="LoadGLClientCustomFields">GL Format Custom Fields</a></li>
                </ul>
            </div>
        </div>
        
        <div class="contentcontainer">
            <h1 class="systemsettingsheader"></h1>
            <div class="gridcontainer"></div>
        </div>

    </div>

    <div class="clergystatusmodal" title="Clergy Status" style="display: none;">

        <div class="modalcontent">

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="cstat-Code" />
            </div>

            <div class="fieldblock">
                <label>Description</label>
                <input type="text" class="cstat-Name" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="cstat-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>
    </div>

    <div class="clergytypemodal" title="Clergy Type" style="display: none;">

        <div class="modalcontent">

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="ctype-Code" />
            </div>

            <div class="fieldblock">
                <label>Description</label>
                <input type="text" class="ctype-Name" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="ctype-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>
    </div>

    <div class="constituenttypemodal" title="Constituent Type" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="consttype-Id" />
            <input type="hidden" class="consttype-IsActive" />
            <input type="hidden" class="consttype-IsRequired" />

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="consttype-Code" />
            </div>

            <div class="fieldblock">
                <label>Description</label>
                <input type="text" class="consttype-Name" />
            </div>

            <div class="fieldblock">
                <label>Category</label>
                <select class="consttype-Category">
                    <option value="0">Individual</option>
                    <option value="1">Organization</option>
                    <option value="2">Both</option>
                </select>
            </div>

            <div class="fieldblock">
                <label>Name Format</label>
                <input type="text" class="consttype-NameFormat" />
            </div>

            <div class="fieldblock">
                <label>Formal Salutation</label>
                <input type="text" class="consttype-SalutationFormal" />
            </div>

            <div class="fieldblock">
                <label>Informal Salutation</label>
                <input type="text" class="consttype-SalutationInformal" />
            </div>

            <div class="fieldblock">
                <label>Tags</label>
                <div class="tagselect consttype-tagselect"></div>
            </div>

            <div class="modalbuttons">
                <input type="button" class="submitconsttype" value="Save" />
                <a href="#" class="canceltypemodal">Cancel</a>
            </div>


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
    
    <div class="degreemodal" title="Degree" style="display: none;">

        <div class="modalcontent">

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="deg-Code" />
            </div>

            <div class="fieldblock">
                <label>Description</label>
                <input type="text" class="deg-Name" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="deg-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>
    
    <div class="denominationmodal" title="Denomination" style="display: none;">

        <div class="modalcontent">

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
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="educationLevelmodal" title="Education Level" style="display: none;">

        <div class="modalcontent">

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="eduLev-Code required" />
            </div>

            <div class="fieldblock">
                <label>Description</label>
                <input type="text" class="eduLev-Name required" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="eduLev-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="ethnicitymodal" title="Ethnicity" style="display: none;">

        <div class="modalcontent">

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
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="gendermodal" title="Gender" style="display: none;">

        <div class="modalcontent">

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="gen-Code" />
            </div>

            <div class="fieldblock">
                <label>Name</label>
                <input type="text" class="gen-Name" />
            </div>

            <div class="fieldblock">
                <label>Masculine</label>
                <input type="checkbox" class="gen-IsMasculine" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="gen-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="incomeLevelmodal" title="Income Level" style="display: none;">

        <div class="modalcontent">

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="inc-Code" />
            </div>

            <div class="fieldblock">
                <label>Description</label>
                <input type="text" class="inc-Name" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="inc-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="languagemodal" title="Language" style="display: none;">

        <div class="modalcontent">

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
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="noteCategorymodal" title="Note Category" style="display: none;">

        <div class="modalcontent">

            <div class="fieldblock">
                <label>Label</label>
                <input type="text" class="noteCategory-Label" />
            </div>

            <div class="fieldblock">
                <label>Description</label>
                <input type="text" class="noteCategory-Name" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="noteCategory-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="noteCodemodal" title="Note Code" style="display: none;">

        <div class="modalcontent">

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="noteCode-Code" />
            </div>

            <div class="fieldblock">
                <label>Description</label>
                <input type="text" class="noteCode-Name" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="noteCode-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>
    
    <div class="noteTopicmodal" title="Note Topic" style="display: none;">

        <div class="modalcontent">

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="noteTopic-Code" />
            </div>

            <div class="fieldblock">
                <label>Description</label>
                <input type="text" class="noteTopic-Name" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="noteTopic-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>
	
    <div class="prefixmodal" title="Prefix" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="prefix-id" />

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="prefix-Code" />
            </div>

            <div class="fieldblock">
                <label>Description</label>
                <input type="text" class="prefix-Name" />
            </div>

            <div class="fieldblock">
                <label>Salutation Prefix</label>
                <input type="text" class="prefix-Salutation" />
            </div>

            <div class="fieldblock">
                <label>Label Prefix</label>
                <input type="text" class="prefix-LabelPrefix" />
            </div>

            <div class="fieldblock">
                <label>Label Prefix Short</label>
                <input type="text" class="prefix-LabelAbbreviation" />
            </div>
            
            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="professionmodal" title="Professions" style="display: none;">

        <div class="modalcontent">

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="pro-Code" />
            </div>

            <div class="fieldblock">
                <label>Description</label>
                <input type="text" class="pro-Name" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="pro-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

	<div class="regionlevelmodal" title="Region Level" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="regionlevelid" />

            <div class="fieldblock">
                <label>Level</label>
                <input type="text" class="number rl-Level" />
            </div>

            <div class="fieldblock">
                <label>Label</label>
                <input type="text" class="rl-Label" />
            </div>

            <div class="fieldblock">
                <label>Abbreviation</label>
                <input type="text" class="rl-Abbreviation" />
            </div>

            <div class="fieldblock">
                <label>Required</label>
                <input type="checkbox" class="rl-IsRequired" />
            </div>

            <div class="fieldblock">
                <label>Child Level</label>
                <input type="checkbox" class="rl-IsChildLevel" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submitregionlevel" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="regionmodal" title="Region" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="regionid" />
            <input type="hidden" class="parentregionid" />
            <input type="hidden" class="currentlevel" />

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="reg-Code" />
            </div>

            <div class="fieldblock">
                <label>Name</label>
                <input type="text" class="reg-Name" />
            </div>

            <div class="fieldblock">
                <label>Is Active</label>
                <input type="checkbox" class="reg-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submitregion" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="relcatmodal" title="Relationship Category" style="display: none;">

        <div class="modalcontent">

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="relcat-Code" />
            </div>

            <div class="fieldblock">
                <label>Description</label>
                <input type="text" class="relcat-Name" />
            </div>

            <div class="fieldblock">
                <label>Shown In Quick View</label>
                <input type="checkbox" class="relcat-IsShownInQuickView" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="relcat-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="reltypemodal" title="Relationship Type" style="display: none;">

        <div class="modalcontent">

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="reltype-Code" />
            </div>

            <div class="fieldblock">
                <label>Description</label>
                <input type="text" class="reltype-Name" />
            </div>

            <div class="fieldblock">
                <label>Male Reciprocal</label>
                <select class="reltype-ReciprocalTypeMaleId relationshiptypes"></select>
            </div>

            <div class="fieldblock">
                <label>Female Reciprocal</label>
                <select class="reltype-ReciprocalTypeFemaleId relationshiptypes"></select>
            </div>

            <div class="fieldblock">
                <label>Constituent Category</label>
                <select class="reltype-ConstituentCategory">
                    <option value="0">Individual</option>
                    <option value="1">Organization</option>
                    <option value="2">Both</option>
                </select>
            </div>

            <div class="fieldblock">
                <label>Relationship Category</label>
                <select class="reltype-RelationshipCategoryId relationshipcategories"></select>
            </div>

            <div class="fieldblock">
                <label>Spouse</label>
                <input type="checkbox" class="reltype-IsSpouse" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="reltype-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="schoolmodal" title="Schools" style="display: none;">

        <div class="modalcontent">

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="sch-Code" />
            </div>

            <div class="fieldblock">
                <label>Description</label>
                <input type="text" class="sch-Name" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="sch-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
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


            <div class="modalbuttons">
                <input type="button" class="savetag" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>
 <div class="addresstypemodal" title="Address Types" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="addrtype-Id" />

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="addrtype-Code" />
            </div>

            <div class="fieldblock">
                <label>Name</label>
                <input type="text" class="addrtype-Name" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="addrtype-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submitaddrtype" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="contactcategorymodal" title="Contact Categories" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="contcat-Id" />

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="contcat-Code" />
            </div>

            <div class="fieldblock">
                <label>Name</label>
                <input type="text" class="contcat-Name" />
            </div>

            <div class="fieldblock">
                <label>Section Title</label>
                <input type="text" class="contcat-SectionTitle" />
            </div>

            <div class="fieldblock">
                <label>Text Box Label</label>
                <input type="text" class="contcat-TextBoxLabel" />
            </div>

            <div class="fieldblock">
                <label>Default Contact Type</label>
                <select class="contcat-DefaultContactTypeId" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="contcat-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submitcontcat" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="contacttypemodal" title="Contact Types" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="conttype-Id" />

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="conttype-Code" />
            </div>

            <div class="fieldblock">
                <label>Name</label>
                <input type="text" class="conttype-Name" />
            </div>

            <div class="fieldblock">
                <label>Contact Category</label>
                <select class="conttype-ContactCategoryId" />
            </div>

            <div class="fieldblock">
                <label>Always Show</label>
                <input type="checkbox" class="conttype-IsAlwaysShown" />
            </div>

            <div class="fieldblock">
                <label>Can Delete</label>
                <input type="checkbox" class="conttype-CanDelete" />
            </div>

            <div class="fieldblock">
                <label>Active</label>
                <input type="checkbox" class="conttype-IsActive" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submitconttype" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="fiscalyearmodal" title="Fiscal Year" style="display: none;">
        <div class="modalcontent">

            <input type="hidden" class="fy-LedgerId" />

            <div class="fieldblock">
                <label>Name</label>
                <input type="text" class="fy-Name" />
            </div>

            <div class="fieldblock">
                <label>Number Of Periods</label>
                <select class="fy-NumberOfPeriods">
                    <option value="1">1</option>
                    <option value="2">2</option>
                    <option value="3">3</option>
                    <option value="4">4</option>
                    <option value="6">6</option>
                    <option value="12">12</option>
                </select>
            </div>

            <div class="fieldblock">
                <label>Use final accounting period for year end adjustments only</label>
                <input type="checkbox" class="fy-HasAdjustmentPeriod" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>
    </div>

    <div class="fiscalperiodmodal" title="Fiscal Period" style="display: none;">
        <div class="modalcontent">

            <input type="hidden" class="fp-Id" />
            <input type="hidden" class="fp-FiscalYearId" />

            <div class="fieldblock">
                <label>Period Number</label>
                <input type="text" maxlength="2" class="fp-PeriodNumber number" />
            </div>

            <div class="fieldblock">
                <label>Start Date</label>
                <input type="text" class="fp-StartDate datepicker" />
            </div>

            <div class="fieldblock">
                <label>End Date</label>
                <input type="text" class="fp-EndDate datepicker" />
            </div>

            <div class="fieldblock">
                <label>Status</label>
                <select class="fp-Status">
                    <option value="0">Open</option>
                    <option value="1">Closed</option>
                    <option value="2">Reopened</option>
                </select>
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>
    </div>

    <div class="glformatmodal" title="GL Format" style="display: none;">

        <div class="modalcontent">


            <input type="hidden" class="glformat-LedgerId parentid" />

            <div class="fieldblock">
                <label>Level</label>
                <input type="text" class="glformat-Level" />
            </div>

            <div class="fieldblock">
                <label>Type</label>
                <select class="glformat-Type">  
                   <option value="0">None</option>
                   <option value="1">Fund</option>
                   <option value="2">Account</option>
                </select>
            </div>

            <div class="fieldblock">
                <label>Format</label>
                <select class="glformat-Format">  
                   <option value="0">Both</option>
                   <option value="1">Numberic</option>
                   <option value="2">Alpha</option>
                </select>
            </div>

            <div class="fieldblock">
                <label>Length</label>
                <input type="text" class="glformat-Length" />
            </div>

            <div class="fieldblock">
                <label>Linked</label>
                <input type="checkbox" class="glformat-IsLinked" />
            </div>

            <div class="fieldblock">
                <label>Common</label>
                <input type="checkbox" class="glformat-IsCommon" />
            </div>

            <div class="fieldblock">
                <label>Name</label>
                <input type="text" class="glformat-Name" />
            </div>
            
            <div class="fieldblock">
                <label>Abbreviation</label>
                <input type="text" class="glformat-Abbreviation" />
            </div>
            
            <div class="fieldblock">
                <label>Separator</label>
                <select class="glformat-Separator">  
                   <option value="">None</option>
                   <option value=" ">(Space)</option>
                   <option value="-">-</option>
                   <option value=".">.</option>
                   <option value=",">,</option>
                   <option value="/">/</option>
                   <option value="(">(</option>
                   <option value=")">)</option>
                   <option value="[">[</option>
                   <option value="]">]</option>
                </select>
            </div>

            <div class="fieldblock">
                <label>Sort Order</label>
                <select class="glformat-SortOrder">  
                   <option value="0">Ascending</option>
                   <option value="1">Descending</option>
                </select>
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>


        </div>

    </div>
    <div class="entitymodal" title="Business Units" style="display: none;">

        <div class="modalcontent">

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="en-Code" />
            </div>

            <div class="fieldblock">
                <label>Name</label>
                <input type="text" class="en-Name" />
            </div>

            <div class="fieldblock">
                <label>Entity Type</label>
                <select class="en-BusinessUnitType">
                        <option value="1">Common</option>
                        <option value="2">Separate</option>
                </select>
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>
        </div>
    </div>

</asp:Content>
