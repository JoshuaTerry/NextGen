<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Constituents.aspx.cs" Inherits="DDI.UI.Web.Constituents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="..\..\Scripts\region.js"></script>
    <script type="text/javascript" src="..\..\Scripts\constituents.js"></script>
    <link rel="stylesheet" href="..\..\CSS\constituents.css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <input id="hidconstituentid" runat="server" type="hidden" class="hidconstituentid" enableviewstate="true" />

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
                <li><a href="#tab-main" id="tab-main-link">Main</a></li>
                <li><a href="#tab-notes">Notes</a></li>
                <li><a href="#tab-relationships">Relationships</a></li>
                <li><a href="#tab-donor">Donor</a></li>
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

                    <%-- MAY BE INTRODUCED LATER --%>
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

                    <h1>Demographics</h1>
                    <div class="editcontainer">

                        <div class="threecolumn">
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
                    </div>

                    <h1>Doing Business As<a href="#" title="New" class="newdbamodallink newbutton"></a></h1>
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

                    <h1 class="organization">Organization</h1>
                    <div class="editcontainer organization">

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

                    <h1>Professional</h1>
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

                    <h1>Alternate ID <a href="#" class="newaltidmodal newbutton"></a></h1>
                    <div>
                        
                        <div class="alternateidgridcontainer"></div>

                        <div class="alternateidmodal" title="Alternate ID" style="display: none;">

                            <div class="modalcontent">

                                <div class="fieldblock">
                                    <label>Name</label>
                                    <input type="text"  class="ai-Name" />
                                </div>

                                <div class="modalbuttons">
                                    <input type="button" class="submitaltid" value="Save" />
                                    <a href ="#" class="cancelmodal">Cancel</a>
                                </div>

                            </div>

                        </div>
                    </div>

                    <h1>Contact Information</h1>
                    <div>

                        <div class="accordions">

                            <h1>Addresses<a href="#" title="New" class="newaddressmodallink newbutton"></a></h1>
                            <div>
                                <div class="constituentaddressgridcontainer"></div>
                            </div>

                           <%-- <h1>Phone Numbers</h1>
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
                            </div>--%>

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
                <input type="button" class="saveaddress" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="dbamodal" title="Doing Business As" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="hiddbaid" />

            <div class="fieldblock">
                <label>DBA Name:</label>
                <input type="text" class="DBAName" />
            </div>

            <div class="fieldblock">
                <label>From:</label>
                <input type="text" class="StartDate datepicker" />
            </div>

            <div class="fieldblock">
                <label>To:</label>
                <input type="text" class="EndDate datepicker" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="savedba" value="Save" />
                <a href="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>
                        
    <div class="phonenumbermodal" title="Phone Number" style="display: none;">

        <div class="modalcontent">
            
            <div class="fieldblock">
                <label>Type:</label>
                <select class="pn-PhoneNumberType ContactTypeId"></select>
            </div>

            <div class="fieldblock">
                <label>Phone:</label>
                <input type="text"  class="pn-Info" />
            </div>

            <div class="fieldblock">
                <label>Is Preferred:</label>
                <input type="checkbox"  class="pn-IsPreferred" />
            </div>

            <div class="fieldblock">
                <label>Comment:</label>
                <input type="text"  class="pn-Comment" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submitphonenumber" value="Save" />
                <a href ="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="emailmodal" title="Email" style="display: none;">

        <div class="modalcontent">
            
            <div class="fieldblock">
                <label>Type:</label>
                <select class="e-EmailType ContactTypeId"></select>
            </div>

            <div class="fieldblock">
                <label>Email:</label>
                <input type="text"  class="e-Info" />
            </div>

            <div class="fieldblock">
                <label>Is Preferred:</label>
                <input type="checkbox"  class="e-IsPreferred" />
            </div>

            <div class="fieldblock">
                <label>Comment:</label>
                <input type="text"  class="e-Comment" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submitemail" value="Save" />
                <a href ="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="websitesmodal" title="Web Sites" style="display: none;">

        <div class="modalcontent">
            
            <div class="fieldblock">
                <label>Type:</label>
                <select class="ws-WebSiteType ContactTypeId" ></select>
            </div>

            <div class="fieldblock">
                <label>Web Site:</label>
                <input type="text"  class="ws-Info" />
            </div>

            <div class="fieldblock">
                <label>Is Preferred:</label>
                <input type="checkbox"  class="ws-IsPreferred" />
            </div>

            <div class="fieldblock">
                <label>Comment:</label>
                <input type="text"  class="ws-Comment" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submitwebsites" value="Save" />
                <a href ="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="pocmodal" title="Point Of Contact" style="display: none;">

        <div class="modalcontent">
            
            <div class="fieldblock">
                <label>Type:</label>
                <select class="poc-PocType ContactTypeId"></select>
            </div>

            <div class="fieldblock">
                <label>Info:</label>
                <input type="text"  class="poc-Info" />
            </div>

            <div class="fieldblock">
                <label>Is Preferred:</label>
                <input type="checkbox"  class="poc-IsPreferred" />
            </div>

            <div class="fieldblock">
                <label>Comment:</label>
                <input type="text"  class="poc-Comment" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submitpoc" value="Save" />
                <a href ="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="socmedmodal" title="Social Media" style="display: none;">

        <div class="modalcontent">
            
            <div class="fieldblock">
                <label>Type:</label>
                <select class="sm-SocialMediaType ContactTypeId"></select>
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
            
            <div class="fieldblock">
                <label>Type:</label>
                <select class="o-OtherType ContactTypeId"></select>
            </div>

            <div class="fieldblock">
                <label>Info:</label>
                <input type="text"  class="o-Info" />
            </div>

            <div class="fieldblock">
                <label>Is Preferred:</label>
                <input type="checkbox"  class="o-IsPreferred" />
            </div>

            <div class="fieldblock">
                <label>Comment:</label>
                <input type="text"  class="o-Comment" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="submitother" value="Save" />
                <a href ="#" class="cancelmodal">Cancel</a>
            </div>

        </div>

    </div>
</asp:Content>
