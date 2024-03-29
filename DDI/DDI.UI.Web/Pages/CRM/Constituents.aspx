﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Constituents.aspx.cs" Inherits="DDI.UI.Web.Constituents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript" src="..\..\Scripts\region.js"></script>
    <script type="text/javascript" src="..\..\Scripts\systemsettings.js"></script>
    <script type="text/javascript" src="..\..\Scripts\constituents.js"></script>
    <link rel="stylesheet" href="..\..\CSS\constituents.css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <input id="hidconstituentid" runat="server" type="hidden" class="hidconstituentid" enableviewstate="true" />

    <div class="constituentcontainer">

        <div class="constituentinfocontainer">
            <h1>Photo</h1>
            <div class="constituentpic">
                <img />
                <div class="changeconstituentpic" style="height: 0px; bottom: 0px; display: none;">Change Profile Pic</div>
            </div>
            <div>
                <a href="#" class="newauditmodal">Audit History</a>
            </div>
            <h1>Information</h1>
            <div class="constituentinformation">
                <div>
                    Status: <span class="editable ConstituentStatus">Active</span>
                </div>
                <div>
                    <label class="editable FormattedName"></label>
                    <label class="editable Address"></label>
                    <label class="editable CityStateZip"></label>
                    <label class="editable ContactInfo"></label>
                </div>
                <div>
                    <span class="editable PhoneNumber"></span>
                </div>
            </div>
            
            <h1>Relationships</h1>
            <div class="editable relationshipsQuickView">

            </div>
        </div>

        <div class="tabscontainer">

            <ul>
                <li><a href="#tab-main" id="tab-main-link">Main</a></li>
                <li id="tab-notes-main"><a href="#tab-notes">Notes</a></li>
                <li><a href="#tab-relationships">Relationships</a></li>
                <li><a href="#tab-donor">Donor</a></li>
                <li id="tab-investments-main"><a href="#tab-investments">Investments</a></li>
            </ul>

            <!-- Individual Tab -->
            <div id="tab-main" class="scrollable">

                <div class="constituenttopinfo editcontainer">

                    <div class="threecolumn">
                        
                        <div class="fieldblock">
                            <label>ID</label>
                            <input type="text" class="editable ConstituentNumber" maxlength="9" />
                        </div>

                        <div class="fieldblock">
                            <label>Name</label>
                            <label class="editable FormattedName"></label>
                        </div>

                        <div class="fieldblock">
                            <label>Status</label>
                            <select class="editable ConstituentStatusId"></select>
                        </div>
                    </div>

                    <div class="fieldblock">
                        <label>Tags</label>
                        <div class="tagselect constituenttagselect disabled"></div>
                    </div>

                    <div class="fieldlock">
                        Created: <span class="CreatedDate"></span> by <span class="CreatedBy"></span>
                    </div>
                    
                    <a href="#" class="fileuploadlink">Upload Files...(for testing purposes)</a>

                </div>

                <div class="accordions">
                    
                    <h1>Name</h1>
                    <div class="editcontainer">
                        <div class="individualConstituent">
                            <div class="fivecolumn">
                                <div class="fieldblock">
                                    <label>Prefix</label>
                                    <select class="editable PrefixId"></select>
                                </div>

                                <div class="fieldblock">
                                    <label>First</label>
                                    <input type="text" class="editable FirstName" />
                                </div>

                                <div class="fieldblock">
                                    <label>Middle</label>
                                    <input type="text" class="editable MiddleName" />
                                </div>

                                <div class="fieldblock">
                                    <label>Last</label>
                                    <input type="text" class="editable LastName" />
                                </div>

                                <div class="fieldblock">
                                    <label>Suffix</label>
                                    <input type="text" class="editable Suffix" />
                                </div>
                            </div>

                            <div class="threecolumn">
                                <div class="fieldblock">
                                    <label>Name 2</label>
                                    <input type="text" class="editable Name2" />
                                </div>

                                <div class="fieldblock">
                                    <label>Nickname</label>
                                <input type="text" class="editable Nickname" />
                                </div>

                                <div class="fieldblock">
                                    <label>Salutation</label>
                                    <input type="text" class="editable Salutation" />
                                </div>
                            </div>

                            <div class="threecolumn">
                                <div class="fieldblock">
                                    <label>Tax ID</label>
                                    <input type="text" class="editable TaxId" />
                                </div>
                                <div class="fieldblock">
                                    <label>Gender</label>
                                    <select class="editable GenderId"></select>
                                </div>
                                <div class="fieldblock">
                                    <label>Source</label>
                                    <input type="text" class="editable Source" />
                                </div>
                            </div>
                        </div>
                        <div class="organizationConstituent" style="display: none;">

                            <div class="threecolumn">
                                <div class="fieldblock">
                                    <label>Name</label>
                                    <input type="text" class="editable Name" />
                                </div>

                                <div class="fieldblock">
                                    <label>Name 2</label>
                                    <input type="text" class="editable Name2" />
                                </div>

                                <div class="fieldblock">
                                    <label>Salutation</label>
                                    <input type="text" class="editable Salutation" />
                                </div>
                            </div>

                            <div class="twocolumn">
                                <div class="fieldblock">
                                    <label>Tax ID</label>
                                    <input type="text" name="taxIdOrganization" class="editable TaxId" pattern="^\d{2}-?\d{7}$" title="Must be in format: 12-3456789" />
                                </div>

                                <div class="fieldblock">
                                    <label>Source</label>
                                    <input type="text" class="editable Source" />
                                </div>
                            </div>

                        </div>
                    </div>

                    <h1>Additional Data</h1>
                    <div class="editcontainer customFieldContainer"></div>

                    <h1 class="ClergySettingsSection"><span class="ClergySettingsSectionLabel">Clergy Section</span></h1>
                    <div class="editcontainer">
                        
                        <div class="fourcolumn">
                            <div class="fieldblock">
                                <label>Clergy Type</label>
                                <select class="editable ClergyTypeId"></select>
                            </div>

                            <div class="fieldblock">
                                <label>Clergy Status</label>
                                <select class="editable ClergyStatusId"></select>
                            </div>

                            <div class="fieldblock">
                                <label>Ordination Date</label>
                                <input type="text" class="editable OrdinationDate datepicker" />
                            </div>

                            <div class="fieldblock">
                                <label>Place of Ordination</label>
                                <input type="text" class="editable PlaceOfOrdination" />
                            </div>
                        </div>

                    </div>

                   <h1 class="DemographicSettingsSection"><span class="DemographicSettingsSectionLabel">Demographics</span></h1>
                    
                    <div class="editcontainer">
                        <div class="fieldblock">
                            <label>Denomination</label>
                            <div class="editable tagbox denominations"></div>
                        </div>

                        <div class="fieldblock">
                            <label>Ethnicity</label>
                            <div class="editable tagbox ethnicities"></div>
                        </div>

                        <div class="fieldblock">
                            <label>Language</label>
                            <select class="editable LanguageId"></select>
                        </div>
                    </div>

                    <h1 class ="DBASettingsSection"><span class="DBASettingsSectionLabel">Doing Business As</span></h1>
                    <div>
                        <div class="doingbusinessastable"></div>
                    </div>

                    <h1 class="EducationSettingsSection"><span class="EducationSettingsSectionLabel">Education</span></h1> 

                    <div class="editcontainer">

                        <div class="fieldblock">
                            <label>Education Level</label>
                            <select class="editable EducationLevelId"></select>
                        </div>

                        <div class="educationgridcontainer"></div>

                    </div>

                   <h1 class="OrganizationSettingsSection"><span class="OrganizationSettingsSectionLabel">Organization</span></h1>
                    <div class="editcontainer">

                        <div class="inline">
                            <div class="fieldblock">
                                <label>Membership</label>
                                <input type="text" class="editable MembershipCount" />
                            </div>

                            <div class="fieldblock">
                                <label>Year Established</label>
                                <input type="text" class="editable YearEstablished" />
                            </div>
                        </div>

                        <div class="fieldblock">
                            <label>Business</label>
                            <input type="text" class="editable Business" />
                        </div>

                        <div class="twocolumn">
                            <div class="fieldblock">
                                <label>Tax Exempt</label>
                                <input type="checkbox" class="editable IsTaxExempt" />
                            </div>

                            <div class="fieldblock">
                                <label>Verify Date</label>
                                <input type="text" class="editable TaxExemptVerifyDate datepicker" />
                            </div>
                        </div>

                        <div class="fieldblock">
                            <label>IRS Letter Received</label>
                            <input type="checkbox" class="editable IsIRSLetterReceived" />
                        </div>

                    </div>

                    <h1 class ="PaymentPreferencesSettingsSection">
                        <span class="PaymentPreferencesSettingsSectionLabel">Payment Preferences</span></h1>
                    <div class="editcontainer">

                        <div class="fieldblock">
                            <label>Method</label>
                            <select class="editable PreferredPaymentMethod">
                                <option value="0">None</option>
                                <option value="1">Check</option>
                                <option value="2">ACH Transfer</option>
                                <option value="3">Wire Transfer</option>
                                <option value="4">SWIFT Transfer</option>
                            </select>
                        </div>

                        <label>EFT Information</label>
                        <div class="paymentpreferencesgridcontainer"></div>

                    </div>

                    <h1 class ="PersonalSettingsSection"><span class="PersonalSettingsSectionLabel">Personal</span></h1>
                    <div class="editcontainer">

                        <div class="twocolumn">

                            <div>                                
                                <div class="fieldblock">
                                    <label>Birth Date</label>
                                    <div class="threecolumn">
                                        <div style="width: 50px;">
                                        <select class="editable BirthMonth">
                                        <option></option>
                                        <option value="1">01</option>
                                        <option value="2">02</option>
                                        <option value="3">03</option>
                                        <option value="4">04</option>
                                        <option value="5">05</option>
                                        <option value="6">06</option>
                                        <option value="7">07</option>
                                        <option value="8">08</option>
                                        <option value="9">09</option>
                                        <option value="10">10</option>
                                        <option value="12">11</option>
                                        <option value="12">12</option>
                                    </select>
                                        </div>
                                        <div style="width: 50px;">
                                    <select class="editable BirthDay"></select>
                                            </div>
                                        <div style="width: 100px;">
                                    <select class="editable BirthYear"></select>
                                            </div>
                                    </div>
                                </div>

                                <div class="fieldblock">
                                    <label>Deceased</label>
                                    <input type="text" class="editable DeceasedDate datepicker" />
                                </div>

                                <div class="fieldblock">
                                    <label>Marriage Date</label>
                                    <input type="text" class="editable MarriageDate datepicker" />
                                </div>

                                <div class="fieldblock">
                                    <label>Divorce Date</label>
                                    <input type="text" class="editable DivorceDate datepicker" />
                                </div>
                            </div>

                            <div>
                                <div class="fieldblock range">
                                    <label>Age</label>
                                    <input type="number" class="editable BirthYearFrom" />
                                    to:
                                    <input type="number" class="editable BirthYearTo" />
                                </div>

                                <div class="fieldblock">
                                    <label>Marital Status</label>
                                    <select class="editable MaritalStatusId"></select>
                                </div>

                                <div class="fieldblock">
                                    <label>Prospect Date</label>
                                    <input type="text" class="editable ProspectDate datepicker" />
                                </div>
                            </div>

                        </div>
                        
                    </div>

                    <h1 class="ProfessionalSettingsSection"><span class="ProfessionalSettingsSectionLabel">Professional</span></h1>

                    <div class="editcontainer">

                        <div class="threecolumn">
                            <div class="fieldblock">
                                <label>Profession</label>
                                <select class="editable ProfessionId"></select>
                            </div>

                            <div class="fieldblock">
                                <label>Income Level</label>
                                <select class="editable IncomeLevelId"></select>
                            </div>

                            <div class="fieldblock">
                                <label>First Employed</label>
                                <input type="text" class="editable FirstEmploymentDate datepicker" />
                            </div>
                        </div>
                        
                        <div class="twocolumn">
                            <div class="fieldblock">
                                <label>Employer</label>
                                <input type="text" class="editable Employer" />
                            </div>

                            <div class="fieldblock">
                                <label>Position</label>
                                <input type="text" class="editable Position" />
                            </div>
                        </div>
                            
                        <div class="twocolumn">
                            <div class="fieldblock">
                                <label>Start</label>
                                <input type="text" class="editable EmploymentStartDate datepicker" />
                            </div>

                            <div class="fieldblock">
                                <label>End</label>
                                <input type="text" class="editable EmploymentEndDate datepicker" />
                            </div>
                        </div>

                        <div class="twocolumn">
                            <div class="fieldblock">
                                <label>Employee</label>
                                <input type="checkbox" class="editable IsEmployee" />
                            </div>

                            <div class="fieldblock">
                                <label>User ID</label>
                                <select class="editable UserId"></select>
                            </div>
                        </div>
                        

                    </div>

                    <h1 class="AlternateIdSettingsSection"><span class="AlternateIdSettingsSectionLabel">Alternate ID</span></h1>
                    <div>
                        
                        <div class="alternateidgridcontainer"></div>

                        <div class="alternateidmodal" title="Alternate ID" style="display: none;">

                            <div class="modalcontent">

                                <div class="fieldblock">
                                    <label>Name</label>
                                    <input type="text"  class="ai-Name" />
                                </div>

                                 <input type="hidden" class="ai-ConstituentId parentid" />

                                <div class="modalbuttons">
                                    <input type="button" class="savebutton" value="Save" />
                                    <a href ="#" class="cancelmodal">Cancel</a>
                                </div>

                            </div>

                        </div>
                    </div>

                    <h1 class="ContactInformationSettingsSection"><span class="ContactInformationSettingsSectionLabel">Contact Information</span></h1>
                    <div>

                        <div class="accordions contactinfocontainer">

                            <h1>Addresses<a href="#" title="New" class="newaddressmodallink newbutton"></a></h1>
                            <div>
                                <div class="constituentaddressgridcontainer"></div>
                            </div>

                        </div>

                    </div>

                </div>

            </div>

             <!-- Notes Tab -->
            <div id="tab-notes" class="scrollable">

                <div class="tabscontainer inner">

                    <ul> 
                        <li><a href="#tab-notedetail">Note Detail</a></li>
                        <li><a href="#tab-attachments">Attachments</a></li>
                    </ul>

                    <div id="tab-notedetail">

                        <h1>Note Detail<a href="#" title="New" class="newnotesdetailmodallink newbutton"></a></h1>
                        <div class="notedetailsgridcontainer"></div>
                        
                    </div>

                    <div id="tab-attachments">

                        <table class="datagrid attachmentstable">
                            <thead>
                                <tr>
                                    <th>Description</th>
                                    <th>File Name</th>
                                    <th>Private</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>

                    </div>
                    
                </div>

            </div>
            
            <!-- Relationships Tab -->
            <div id="tab-relationships" class="scrollable">
                <h1>Relationships</h1>
                <div class="relationshipstable"></div>
            </div>
            
            <!-- Donor Tab -->
            <div id="tab-donor" class="scrollable">

                <div class="accordions">

                    <h1>Donation History</h1>
                    <div>
                        <table class="donationhistorytable datagrid" border="0">
                            <thead>
                                <tr>
                                    <th>Donation #</th>
                                    <th>Date</th>
                                    <th>Memo</th>
                                    <th>Amount</th>
                                    <th>Created By</th>
                                    <th>Status</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>

                    <h1>Donor Statistics</h1>
                    <div>

                        <div class="fieldblock">
                            <label>Donor Segment: </label>
                            <input type="text" class="donorsegment" />
                        </div>

                        <div class="accordions">

                            <h1>Donation Level Statistics</h1>
                            <div>
                                <table class="datagrid donationlevelstattable">
                                    <thead>
                                        <tr>
                                            <th>Cateogory</th>
                                            <th>Lifetime Amount</th>
                                            <th>Largest Amount</th>
                                            <th>First Amount</th>
                                            <th>First Date</th>
                                            <th>Lifetime Count</th>
                                            <th>Largest Date</th>
                                            <th>Recent Amount</th>
                                            <th>Recent Date</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>

                            <h1>Donation Line Level Statistics</h1>
                            <div>
                                <table class="datagrid donationlinelevelstattable">
                                    <thead>
                                        <tr>
                                            <th>Description</th>
                                            <th>Lifetime Amount</th>
                                            <th>Largest Amount</th>
                                            <th>First Amount</th>
                                            <th>First Date</th>
                                            <th>Lifetime Count</th>
                                            <th>Largest Date</th>
                                            <th>Recent Amount</th>
                                            <th>Recent Date</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>

                        </div>

                    </div>

                    <h1>Scheduled Donation Settings</h1>
                    <div>
                        <table class="donationsettingstable datagrid" border="0">
                            <thead>
                                <tr>
                                    <th>Active</th>
                                    <th>Next Date</th>
                                    <th>Frequency</th>
                                    <th>Amount</th>
                                    <th>Saved Bank Info</th>
                                    <th>Donation Template</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>

                </div>

            </div>

            <!-- Investment Tab -->
            <div id="tab-investments" class="scrollable">

                <div class="investmenttopinfo">

                        <div class="fieldblock">
                            <h2>Investor Summary</h2>
                        </div>
                        <br />

                        <div class="fivecolumn" >
                            <div>
                                <label>Status: </label>
                            </div>
                            <div>
                                <input type="text" class="InvestorStatus" disabled="disabled"/>
                            </div>
                        </div>

                        <div class="fivecolumn" >
                            <div>
                                <label>Since: </label>
                            </div>
                            <div>
                                <input type="text" class="InvestorStartDate date" disabled="disabled"/>
                            </div>
                        </div>

                        <div class="fivecolumn">
                            <div>
                                <label>Primary Owner Investment: </label>
                            </div>
                            <div>
                                <input type="text" class="PrimaryInvestorTotal money justright" disabled="disabled"/>
                            </div>
                        </div>

                        <div class="fivecolumn">
                            <div>
                                <label>Joint Owner Investment: </label>
                            </div>
                            <div>
                                <input type="text" class="JointInvestorTotal money justright" disabled="disabled"/>
                            </div>
                        </div>

                        <div class="fivecolumn">
                            <div>
                                <label>Total Investment Balance: </label>
                            </div>
                            <div>
                                <input type="text" class="InvestorTotal money justright" disabled="disabled"/>
                            </div>
                        </div>

                    </div>

                <hr />
                
                <div class="investmentstable scrollable"></div>

        </div>

        <div class="dashboard" style="display: none;">

            <h1>Dashboard</h1>

        </div>

    </div>

    </div>

    <div class="addressmodal" title="Address" style="display: none;">

        <div class="modalcontent">
            
            <input type="hidden" class="hidconstituentaddressid" />
            <input type="hidden" class="hidaddressid" />

            <div class="fieldblock">
                <label>Address Type</label>
                <select class="na-AddressTypeId"></select>
            </div>

            <div class="fieldblock">
                <label>Address Line 1</label>
                <input type="text" class="na-AddressLine1 autoaddress1" />
            </div>

            <div class="fieldblock">
                <label>Address Line 2</label>
                <input type="text" class="na-AddressLine2 autoaddress2" />
            </div>

            <div class="fieldblock">
                <label>Postal code</label>
                <input type="text" class="na-PostalCode autozip" />
            </div>

            <div class="fieldblock">
                <label>Country</label>
                <select class="na-CountryId autocountry"></select>
            </div>

            <div class="inline">
                <div class="fieldblock">
                    <label style="width: 100px;">City</label>
                    <input type="text" class="na-City autocity" style="width: 85px;" />
                </div>

                <div class="fieldblock">
                    <label style="width: 25px;">St</label>
                    <select class="na-StateId autostate" style="width: 106px;"></select>
                </div>
            </div>

            <div class="fieldblock">
                <label>County</label>
                <select class="na-CountyId autocounty"></select>
            </div>

            <div class="regionscontainer"></div>

            <fieldset>
                <legend>Address Options</legend>

                <div class="fieldblock">
                    <label>Is Preferred</label>
                    <input type="checkbox" class="na-IsPreferred" />
                </div>

                <div class="fieldblock">
                    <label>Residency</label>
                    <select class="na-ResidentType">
                        <option value="0">Primary</option>
                        <option value="1">Secondary</option>
                        <option value="2">Separate</option>
                    </select>
                </div>

                <div class="fieldblock">
                    <label>Comment</label>
                    <input type="text" class="na-Comment" />
                </div>

                <div class="fieldblock range">
                    <label>Dates</label>
                    <input type="text" class="na-StartDate datepicker" />
                    <span>to</span>
                    <input type="text" class="na-EndDate datepicker" />
                </div>

            </fieldset>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="dbamodal" title="Doing Business As" style="display: none;">

        <div class="modalcontent">
            
            <input type="hidden" class="dba-ConstituentId parentid" />

            <div class="fieldblock">
                <label>DBA Name:</label>
                <input type="text" class="dba-Name" />
            </div>

            <div class="twocolumn">

                <div class="fieldblock">
                    <label>From:</label>
                    <input type="text" class="dba-StartDate datepicker" />

                </div>


                <div class="fieldblock">
                    <label>To:</label>
                    <input type="text" class="dba-EndDate datepicker" />

                </div>

                <div class="modalbuttons">
                    <input type="button" class="savebutton" value="Save" />
                    <a href="#" class="cancelmodal">Cancel</a>
                </div>

            </div>

        </div>

    </div>
	
	<div class="paymentpreferencemodal" title="Payment Preference" style="display: none;">

        <div class="modalcontent">

            <div class="fieldblock">
                <label>Description</label>
                <input type="text" class="pp-Description" />
            </div>

            <div class="fieldblock">
                <label>Bank Name</label>
                <input type="text" class="pp-BankName" />
            </div>

            <div class="twocolumn">

                <div class="fieldblock">
                    <label>Routing Number</label>
                    <input type="text" class="pp-RoutingNumber" />
                </div>

                <div class="fieldblock">
                    <label>Account Number</label>
                    <input type="text" class="pp-BankAccount" />
                </div>

            </div>

            <div class="twocolumn">

                <div class="fieldblock">
                    <label>Account Type</label>
                    <select class="pp-AccountType">
                        <option value=""></option>
                        <option value="0">Checking Account</option>
                        <option value="1">Savings Account</option>
                    </select>
                </div>

                <div class="fieldblock">
                    <label>EFT Format</label>
                    <select class="pp-EFTFormatId eftformats"></select>
                </div>

            </div>

            <div class="twocolumn">

                <div class="fieldblock">
                    <label>Status</label>
                    <select class="pp-Status">
                        <option value=""></option>
                        <option value="0">Active</option>
                        <option value="1">Inactive</option>
                        <option value="2">Pre-note Required</option>
                        <option value="3">Pre-Note Sent</option>
                        <option value="4">Expired</option>
                        <option value="5">Not Valid</option>
                    </select>
                </div>

            </div>
            
            <input type="hidden" class="pp-StatusDate" />
            <input type="hidden" class="pp-PreviousStatus" />
            <input type="hidden" class="pp-ConstituentId parentid" />

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>
                        
    <div class="phonemodal" title="Phone Number" style="display: none;">

        <div class="modalcontent">
            
            <input type="hidden" class="phone-ConstituentId parentid" />

            <div class="fieldblock">
                <label>Type:</label>
                <select class="phone-ContactTypeId"></select>
            </div>

            <div class="fieldblock">
                <label>Phone:</label>
                <input type="text"  class="phone-Info" />
            </div>

            <div class="fieldblock">
                <label>Is Preferred:</label>
                <input type="checkbox"  class="phone-IsPreferred" />
            </div>

            <div class="fieldblock">
                <label>Comment:</label>
                <input type="text"  class="phone-Comment" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href ="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="emailmodal" title="Email" style="display: none;">

        <div class="modalcontent">
            
            <input type="hidden" class="email-ConstituentId parentid" />

            <div class="fieldblock">
                <label>Type:</label>
                <select class="email-ContactTypeId"></select>
            </div>

            <div class="fieldblock">
                <label>Email:</label>
                <input type="text"  class="email-Info" />
            </div>

            <div class="fieldblock">
                <label>Is Preferred:</label>
                <input type="checkbox"  class="email-IsPreferred" />
            </div>

            <div class="fieldblock">
                <label>Comment:</label>
                <input type="text"  class="email-Comment" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href ="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="webmodal" title="Web Sites" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="web-ConstituentId parentid" />
            
            <div class="fieldblock">
                <label>Type:</label>
                <select class="web-ContactTypeId" ></select>
            </div>

            <div class="fieldblock">
                <label>Web Site:</label>
                <input type="text"  class="web-Info" />
            </div>

            <div class="fieldblock">
                <label>Is Preferred:</label>
                <input type="checkbox"  class="web-IsPreferred" />
            </div>

            <div class="fieldblock">
                <label>Comment:</label>
                <input type="text"  class="web-Comment" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href ="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="personmodal" title="Point Of Contact" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="person-ConstituentId parentid" />
            
            <div class="fieldblock">
                <label>Type:</label>
                <select class="person-ContactTypeId"></select>
            </div>

            <div class="fieldblock">
                <label>Info:</label>
                <input type="text"  class="person-Info" />
            </div>

            <div class="fieldblock">
                <label>Is Preferred:</label>
                <input type="checkbox"  class="person-IsPreferred" />
            </div>

            <div class="fieldblock">
                <label>Comment:</label>
                <input type="text"  class="person-Comment" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href ="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="socmedmodal" title="Social Media" style="display: none;">

        <div class="modalcontent">
            
            <div class="fieldblock">
                <label>Type:</label>
                <select class="sm-ContactTypeId"></select>
            </div>

            <div class="fieldblock">
                <label>URL:</label>
                <input type="text"  class="sm-Info" />
            </div>

            <div class="fieldblock">
                <label>Is Preferred:</label>
                <input type="checkbox"  class="sm-IsPreferred" />
            </div>

            <div class="fieldblock">
                <label>Comment:</label>
                <input type="text"  class="sm-Comment" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submitsocmed" value="Save" />
                <a href ="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="othermodal" title="Other Contacts" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="other-ConstituentId parentid" />
            
            <div class="fieldblock">
                <label>Type:</label>
                <select class="other-ContactTypeId"></select>
            </div>

            <div class="fieldblock">
                <label>Info:</label>
                <input type="text"  class="other-Info" />
            </div>

            <div class="fieldblock">
                <label>Is Preferred:</label>
                <input type="checkbox"  class="other-IsPreferred" />
            </div>

            <div class="fieldblock">
                <label>Comment:</label>
                <input type="text"  class="other-Comment" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href ="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>
	
	<div class="relationshipmodal" title="Relationship" style="display: none;">

        <div class="modalcontent">
            <input type="hidden" class="rs-IsSwapped" />
            <input type="hidden" class="rs-Constituent1Id" />
            <input type="hidden" class="rs-Constituent2Id" />

            <div class="fieldblock">
                <label></label>
                <input type="text" class="rs-Constituent1Name constituentlookup" />
            </div>

            <div class="fieldblock">
                <label>is the</label>
                <select class="rs-RelationshipTypeId"></select>
            </div>

            <div class="fieldblock">
                <label>of</label>
                <input type="text" class="rs-Constituent2Name" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>
	
	<div class="educationmodal" title="Education" style="display: none;">

        <div class="modalcontent">
            
            <div class="fieldblock">
                <label>Major</label>
                <input type="text" class="ed-Major" />
            </div>

            <div class="twocolumn">

                <div class="fieldblock">
                    <label>Start Date</label>
                    <input type="text" class="ed-StartDate datepicker" />
                </div>

                <div class="fieldblock">
                    <label>End Date</label>
                    <input type="text" class="ed-EndDate datepicker" />
                </div>

            </div>

            <div class="twocolumn">

                <div class="fieldblock">
                    <label>School</label>
                    <select class="ed-SchoolId schools"></select>
                </div>

                <div class="fieldblock">
                    <label>Degree</label>
                    <select class="ed-DegreeId degrees"></select>
                </div>

            </div>
            
             <input type="hidden" class="ed-ConstituentId parentid" />

            <div class="modalbuttons">
                <input type="button" class="savebutton" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="notesdetailmodal" title="Notes Detail" style="display: none;">

        <div class="modalcontent">
<%--            <input type="hidden" class="hidrelationshipid" />--%>
            <input type="hidden" class="nd-hidparententityid" />
            <input type="hidden" class="hidentitytype" />

            <div class="fieldblock">
                <label>Title</label>
                <input type="text" class="nd-Title" />
            </div>

            <div class="fieldblock">
                <label>Description</label>
                <textarea rows ="4" cols ="86" class="nd-Description"></textarea>
            </div>

            <div class="twocolumn">

                <div class="fieldblock">
                    <label>Alert Start Date</label>
                    <input type="text" class="nd-AlertStartDate datepicker" />
                </div>

                <div class="fieldblock">
                    <label>Alert End Date</label>
                    <input type="text" class="nd-AlertEndDate datepicker" />
                </div>

            </div>

            <div class="fieldblock">
                <label>Note Topics</label>
                <div class="nd-Topics noteTopicSelect"></div> 
                <div class="noteTopicSelectImage" style="display: inline-block;"></div>
                <div class="tagdropdowncontainer" style="display:none;">
                    <div class="tagdropdowndivcontainer"></div>
                    <div class="modalbuttons">
                        <input type="button" class="savenotetopics" value="Save" />
                        <a href="#" class="cancelnotetopics">Cancel</a>
                    </div>
                </div>
            </div>

            <div class="twocolumn">

                <div class="fieldblock">
                    <label>Note Code</label>
                    <select class="nd-NoteCode"></select>
                </div>

                <div class="fieldblock">
                    <label>Note Category</label>
                    <select class="nd-Category Id"></select>
                </div>

            </div>

             <div class="twocolumn">

                <div class="fieldblock">
                    <label>Contact Date</label>
                    <input type="text" class="nd-ContactDate datepicker" />
                </div>

                 <div class="fieldblock">
                    <label>Primary Contact</label>
                    <select class="nd-PrimaryContact"></select>
                </div>

            </div>

            <div class="twocolumn">

                <div class="fieldblock">
                    <label>Person Responsible</label>
                    <select class="nd-PersonResponsible"></select>
                </div>

                <div class="fieldblock">
                    <label>Contact Method</label>
                    <select class="nd-ContactMethod"></select>
                </div>

            </div>

            <div class="twocolumn">

                <div class="fieldblock">
                    <label>Created By</label>
                    <label class="nd-CreatedBy"></label>
                </div>

                <div class="fieldblock">
                    <label>Updated By</label>
                    <label class="nd-UpdatedBy"></label>
                </div>

            </div>

             <div class="twocolumn">

                <div class="fieldblock">
                    <label>On</label>
                    <label class="nd-CreatedOn"></label>
                </div>

                <div class="fieldblock">
                    <label>On</label>
                    <label class="nd-UpdatedOn"></label>
                </div>

            </div>

            <div class="modalbuttons">
                <input type="button" class="savenotedetails" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="notealertmodal" title="Note Alerts" style="display: none;">

        <div class="modalcontent">

            <div class="notealertgridcontainer"></div>

            <div class="modalbuttons">
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>
</asp:Content>
