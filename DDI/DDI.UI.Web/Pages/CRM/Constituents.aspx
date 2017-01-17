﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Constituents.aspx.cs" Inherits="DDI.UI.Web.Constituents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="..\..\Scripts/constituents.js"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="constituentcontainer">

        <div class="constituentinfocontainer">
            <h1>Photo</h1>
            <div class="constituentpic"></div>
            
            <h1>Information</h1>
            <div class="constituentinformation">
                <div>
                    Status: <span class="editable ConstituentStatus">Active</span>
                </div>
                <div>
                    <label class="editable FormattedName"></label>
                    <label class="editable Address"></label>
                    <label class="editable CityStateZip"></label>
                </div>
                <div>
                    <span class="editable PhoneNumber"></span>
                </div>
            </div>

            <h1>Relationships</h1>
            <div class="editable Realtionships">

            </div>
        </div>

        <div class="tabscontainer">

            <ul>
                <li><a href="#tab-individual">Individual</a></li>
                <li><a href="#tab-notes">Notes</a></li>
                <li><a href="#tab-relationships">Relationships</a></li>
                <li><a href="#tab-donor">Donor</a></li>
            </ul>

            <!-- Individual Tab -->
            <div id="tab-individual" class="scrollable">

                <div class="constituenttopinfo editcontainer">

                    <div class="threecolumn">
                        
                        <div class="fieldblock">
                            <label>ID</label>
                            <input type="text" class="editable ConstituentNumber" maxlength="9" />
                        </div>

                        <div class="fieldblock">
                            <label class="editable FormattedName"></label>
                        </div>

                        <div class="fieldblock">
                            <label>Status</label>
                            <select class="editable ConstituentStatusId"></select>
                        </div>
                    </div>

                    <div class="fieldblock">
                        <label>Tags</label>
                        <input type="text" class="editable Tags" />
                    </div>

                    <div class="fieldlock">
                        Created: <span class="CreatedDate"></span> by <span class="CreatedBy"></span>
                    </div>
                    
                </div>

                <div class="accordions">
                    
                    <h1>Name</h1>
                    <div class="editcontainer">

                        <div class="inline">
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

                        <div class="fieldblock">
                            <label>Name 2</label>
                            <input type="text" class="editable Name2" />
                        </div>

                        <div class="fieldblock">
                            <label>Nickname</label>
                            <input type="text" class="editable NickName" />
                        </div>

                        <div class="fieldblock">
                            <label>Salutation</label>
                            <input type="text" class="editable Salutation" />
                        </div>

                        <div class="inline">
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

                    <%--<h1>Addional Data</h1>
                    <div class="editcontainer">

                        <div class="fieldblock">
                            <label class="inline">Is Conditional</label>
                            <input type="checkbox" class="editable IsConditional" />
                        </div>

                        <div class="fieldblock">
                            <label>Number of pets</label>
                            <input type="text" class="editable NumberOfPets" />
                        </div>

                        <div class="fieldblock">
                            <label>Income Prod Assets</label>
                            <input type="text" class="editable IncomeProdAssets" />
                        </div>

                    </div>--%>

                    <h1>Clergy Section</h1>
                    <div class="editcontainer">
                        
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

                    <h1>Demographics</h1>
                    <div class="editcontainer">

                        <div class="fieldblock">
                            <label>Denomination</label>
                            <select class="editable DenominationId"></select>
                        </div>

                        <div class="fieldblock">
                            <label>Ethnicity</label>
                            <select class="editable EthnicityId"></select>
                        </div>

                        <div class="fieldblock">
                            <label>Language</label>
                            <select class="editable LanguageId"></select>
                        </div>

                    </div>

                    <h1>Doing Business As (Not Implemented Yet)</h1>
                    <div>
                        <div class="doingbusinessastable"></div>
                    </div>

                    <h1>Education (Not Implemented Yet)</h1>
                    <div class="editcontainer">
                        <div class="fieldblock">
                            <label>Education Level</label>
                            <select class="editable EducationLevelId"></select>
                        </div>

                        <div class="educationleveltable"></div>
                    </div>

                    <h1>Payment Preferences (Not Implemented Yet)</h1>
                    <div class="editcontainer">

                        <div class="fieldblock">
                            <label>Method</label>
                            <select class="editable PaymentMethodId"></select>
                        </div>

                        <label>ETF Information</label>
                        <div class="paymentpreferencestable"></div>

                    </div>

                    <h1>Personal</h1>
                    <div class="editcontainer">

                        <div class="twocolumn">

                            <div>
                                <div class="fieldblock">
                                    <label>Birth Date</label>
                                    <input type="text" class="editable BirthDate datepicker" />
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
                                    <input type="number" class="editable AgeFrom" />
                                    to:
                                    <input type="number" class="editable AgeTo" />
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

                    <h1>Professional</h1>
                    <div class="editcontainer">

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

                        <div class="fieldblock">
                            <label>Employer</label>
                            <input type="text" class="editable Employer" />
                        </div>

                        <div class="fieldblock">
                            <label>Position</label>
                            <input type="text" class="editable Position" />
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

                    <h1>Alternate ID (Not Implemented Yet)</h1>
                    <div>



                    </div>

                    <h1>Contact Information (Not Implemented Yet)</h1>
                    <div>

                        <div class="accordions">

                            <h1>Addresses</h1>
                            <div>
                                <a href="#" class="newaddressmodallink">New</a>
                                <div class="constituentaddressgridcontainer"></div>
                            </div>

                            <h1>Phone Numbers</h1>
                            <div>
                                <div class="constituentphonegridcontainer"></div>
                            </div>

                            <h1>Emails</h1>
                            <div>
                                <div class="constituentemailgridcontainer"></div>
                            </div>

                            <h1>Web Sites</h1>
                            <div>
                                <div class="constituentwebsitegridcontainer"></div>
                            </div>

                            <h1>Point of Contact</h1>
                            <div>
                                <div class="constituentpocgridcontainer"></div>
                            </div>

                            <h1>Social Media</h1>
                            <div>
                                <div class="constituentsocmedgridcontainer"></div>
                            </div>

                            <h1>Other Contacts</h1>
                            <div>
                                <div class="constituentothergridcontainer"></div>
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

                        <table class="datagrid notedetailtable">
                            <thead>
                                <tr>
                                    <th>Created Date</th>
                                    <th>Created By</th>
                                    <th>Title</th>
                                    <th><img src="Images/Note_16.png" /></th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>

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

        </div>

        <div class="dashboard">

            <h1>Dashboard</h1>

        </div>

    </div>

    <div class="newaddressmodal" title="New Address" style="display: none;">

        <div class="modalcontent">

            <div class="fieldblock">
                <label>Addres Type</label>
                <select class="na-AddressType"></select>
            </div>

            <div class="fieldblock">
                <label>Street</label>
                <textarea class="na-street"></textarea>
            </div>

            <div class="fieldblock">
                <label>Country</label>
                <select class="na-Country"></select>
            </div>

            <div class="fieldblock">
                <label>Postal code</label>
                <input type="text" class="na-PostalCode" />
            </div>

            <div class="inline">
                <div class="fieldblock">
                    <label>City</label>
                    <input type="text" class="na-City" />
                </div>

                <div class="fieldblock">
                    <label>St</label>
                    <select class="na-State"></select>
                </div>
            </div>

            <div class="fieldblock">
                <label>County</label>
                <select class="na-County"></select>
            </div>

            <div class="fieldblock">
                <label>Region</label>
                <select class="na-Region1"></select>
            </div>

            <div class="fieldblock">
                <label>Region</label>
                <select class="na-Region2"></select>
            </div>

            <div class="fieldblock">
                <label>Region</label>
                <select class="na-Region3"></select>
            </div>

            <div class="fieldblock">
                <label>Region</label>
                <select class="na-Region4"></select>
            </div>

            <div class="fieldblock">
                <label>Phone</label>
                <input type="text" class="na-Phone" />
            </div>

            <fieldset>
                <legend>Address Options</legend>

                <div class="fieldblock">
                    <input type="checkbox" class="na-IsPreferred" />
                    <span>Is Preferred</span>
                </div>

                <div class="fieldblock">
                    <label>Residency</label>
                    <select class="na-Residency"></select>
                </div>

                <div class="fieldblock">
                    <label>Comment</label>
                    <input type="text" class="na-Comment" />
                </div>

                <div class="fieldblock range">
                    <label>Dates</label>
                    <input type="text" class="na-FromDate datepicker" />
                    to
                    <input type="text" class="na-ToDate datepicker" />
                </div>

            </fieldset>

            <div class="modalbuttons">
                <input type="button" class="savenewaddress" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

</asp:Content>
