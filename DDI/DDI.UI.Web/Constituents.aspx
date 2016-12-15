<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Constituents.aspx.cs" Inherits="DDI.UI.Web.Constituents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="Scripts/constituents.js"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="constituentcontainer">

        <div class="constituentinfocontainer">
            <h1>Photo</h1>
            <div class="constituentpic"></div>

            <h1>Information</h1>
            <div class="constituentinformation">
                <div>
                    Status: <span class="c-ConstituentStatus">Active</span>
                </div>
                <div>
                    <label class="c-FormattedName"></label>
                    <label class="c-Address"></label>
                    <label class="c-CityStateZip"></label>
                </div>
                <div>
                    <span class="c-PhoneNumber"></span>
                </div>
            </div>

            <h1>Relationships</h1>
            <div class="c-Realtionships">

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

                <div class="threecolumn">
                    <div class="fieldblock">
                        <label>ID:</label>
                        <input type="text" class="c-ConstituentNum" />
                    </div>

                    <div class="fieldblock">
                        <label class="c-FormattedName"></label>
                    </div>

                    <div class="fieldblock">
                        <label>Status</label>
                        <select class="c-ConstituentStatusId"></select>
                    </div>
                </div>

                <div class="fieldblock">
                    <label>Tags</label>
                    <input type="text" class="c-Tags" />
                </div>

                <div class="fieldlock">
                    Created: <span class="c-CreatedDate"></span> by <span class="c-CreatedBy"></span>
                </div>

                <div class="accordions">
                    
                    <h1>Name</h1>
                    <div>

                        <div class="inline">
                            <div class="fieldblock">
                                <label>Prefix</label>
                                <select class="c-Prefix"></select>
                            </div>

                            <div class="fieldblock">
                                <label>First:</label>
                                <input type="text" class="c-FirstName" />
                            </div>

                            <div class="fieldblock">
                                <label>Middle:</label>
                                <input type="text" class="c-MiddleName" />
                            </div>

                            <div class="fieldblock">
                                <label>Last:</label>
                                <input type="text" class="c-LastName" />
                            </div>

                            <div class="fieldblock">
                                <label>Suffix:</label>
                                <input type="text" class="c-Suffix" />
                            </div>
                        </div>

                        <div class="fieldblock">
                            <label>Name 2:</label>
                            <input type="text" class="c-Name2" />
                        </div>

                        <div class="fieldblock">
                            <label>Nickname</label>
                            <input type="text" class="c-NickName" />
                        </div>

                        <div class="fieldblock">
                            <label>Salutation:</label>
                            <input type="text" class="c-Salutation" />
                        </div>

                        <div class="inline">
                            <div class="fieldblock">
                                <label>Tax ID:</label>
                                <input type="text" class="c-TaxId" />
                            </div>
                            <div class="fieldblock">
                                <label>Gender:</label>
                                <select class="c-Gender"></select>
                            </div>
                            <div class="fieldblock">
                                <label>Source:</label>
                                <input type="text" class="c-Source" />
                            </div>
                        </div>

                    </div>

                    <h1>Addional Data</h1>
                    <div>

                        <div class="fieldblock">
                            <label class="inline">Is Conditional</label>
                            <input type="checkbox" class="c-IsConditional" />
                        </div>

                        <div class="fieldblock">
                            <label>Number of pets</label>
                            <input type="text" class="c-NumberOfPets" />
                        </div>

                        <div class="fieldblock">
                            <label>Income Prod Assets</label>
                            <input type="text" class="c-IncomeProdAssets" />
                        </div>

                    </div>

                    <h1>Clergy Section</h1>
                    <div>
                        
                        <div class="fieldblock">
                            <label>Clergy Type:</label>
                            <select class="c-ClergyTypeId"></select>
                        </div>

                        <div class="fieldblock">
                            <label>Clergy Status:</label>
                            <select class="c-ClergyStatusId"></select>
                        </div>

                        <div class="fieldblock">
                            <label>Ordination Date:</label>
                            <input type="text" class="c-OrdinationDate datepicker" />
                        </div>

                        <div class="fieldblock">
                            <label>Place of Ordination:</label>
                            <input type="text" class="c-PlaceOfOrdination"></input>
                        </div>

                    </div>

                    <h1>Demographics</h1>
                    <div>

                        <div class="fieldblock">
                            <label>Denomination:</label>
                            <select class="c-Denomination"></select>
                        </div>

                        <div class="fieldblock">
                            <label>Ethnicity:</label>
                            <select class="c-Ethnicity"></select>
                        </div>

                        <div class="fieldblock">
                            <label>Language:</label>
                            <select class="c-LanguageId"></select>
                        </div>

                    </div>

                    <h1>Doing Business As</h1>
                    <div>
                        <table class="datagrid doingbusinessastable">
                            <thead>
                                <tr>
                                    <th>From</th>
                                    <th>To</th>
                                    <th>DBA Name</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>

                    <h1>Education</h1>
                    <div>
                        <div class="fieldblock">
                            <label>Education Level</label>
                            <select class="c-EducationLevel" />
                        </div>

                        <table class="datagrid educationleveltable" border="0">
                            <thead>
                                <tr>
                                    <th>Start Date</th>
                                    <th>End Date</th>
                                    <th>School</th>
                                    <th>Degree</th>
                                    <th>Major</th>
                                </tr>
                            </thead>
                        </table>
                    </div>

                    <h1>Payment Preferences</h1>
                    <div>

                        <div class="fieldblock">
                            <label>Method:</label>
                            <select class="paymentmethod"></select>
                        </div>

                        <table class="c-PaymentPreferences">
                            <thead>
                                <tr>
                                    <th>Description</th>
                                    <th>ABA Number</th>
                                    <th>Account Number</th>
                                    <th>Ch/S</th>
                                    <th><img src="Images/Note_16.png" /></th>
                                </tr>
                            </thead>
                        </table>

                    </div>

                    <h1>Personal</h1>
                    <div class="twocolumn">

                        <div>
                            <div class="fieldblock">
                                <label>Birth Date:</label>
                                <input type="text" class="c-BirthDate datepicker" />
                            </div>

                            <div class="fieldblock">
                                <label>Deceased:</label>
                                <input type="text" class="c-DeceasedDate datepicker" />
                            </div>

                            <div class="fieldblock">
                                <label>Marriage Date:</label>
                                <input type="text" class="c-MarriageDate datepicker" />
                            </div>

                            <div class="fieldblock">
                                <label>Divorce Date:</label>
                                <input type="text" class="c-DivorceDate datepicker" />
                            </div>
                        </div>

                        <div>
                            <div class="fieldblock range">
                                <label>Age:</label>
                                <input type="number" class="c-AgeFrom" />
                                to:
                                <input type="number" class="c-AgeTo" />
                            </div>

                            <div class="fieldblock">
                                <label>Marital Status:</label>
                                <select class="c-MaritalStatus"></select>
                            </div>

                            <div class="fieldblock">
                                <label>Prospect Date:</label>
                                <input type="text" class="c-ProspectDate datepicker" />
                            </div>
                        </div>

                    </div>

                    <h1>Professional</h1>
                    <div>

                        <div class="fieldblock">
                            <label>Profession:</label>
                            <select class="c-ProfessionId"></select>
                        </div>

                        <div class="fieldblock">
                            <label>Income Level:</label>
                            <select class="c-IncomeLevelId"></select>
                        </div>

                        <div class="fieldblock">
                            <label>First Employed:</label>
                            <input type="text" class="c-FirstEmploymentDate datepicker" />
                        </div>

                        <div class="fieldblock">
                            <label>Employer:</label>
                            <input type="text" class="c-Employer" />
                        </div>

                        <div class="fieldblock">
                            <label>Position:</label>
                            <input type="text" class="c-Position" />
                        </div>

                        <div class="twocolumn">
                            <div class="fieldblock">
                                <label>Start:</label>
                                <input type="text" class="c-EmploymentStartDate datepicker" />
                            </div>

                            <div class="fieldblock">
                                <label>End:</label>
                                <input type="text" class="c-EmploymentEndDate datepicker" />
                            </div>
                        </div>

                        <div class="twocolumn">
                            <div class="fieldblock">
                                <label>Employee</label>
                                <input type="checkbox" class="c-IsEmployee" />
                            </div>

                            <div class="fieldblock">
                                <label>User ID:</label>
                                <select class="c-UserId"></select>
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

</asp:Content>
