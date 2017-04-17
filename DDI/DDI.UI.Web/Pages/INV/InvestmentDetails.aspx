<%@ Page Title="DDI - Investment Details" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="InvestmentDetails.aspx.cs" Inherits="DDI.UI.Web.InvestmentDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="..\..\Scripts\systemsettings.js"></script>
    <script type="text/javascript" src="..\..\Scripts\InvestmentDetails.js"></script>
    <link rel="stylesheet" href="..\..\CSS\constituents.css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <input id="hidinvestmentid" runat="server" type="hidden" class="hidinvestmentid" enableviewstate="true" />

    <div class="constituentcontainer">
        <%--Named for css styles. How do we want to do CSS? Separate page or just use constituents.css?--%>
        <div class="constituentinfocontainer"> 
            <%--Do we want to keep this here, or have a separate page for this container? --%>
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
            </ul>


            <div id="tab-main" class="scrollable">

                <div class="investmenttopinfo editcontainer"> 

                    <div class="twocolumn">
                        
                        <div class="fieldblock">
                            <label>ID</label>
                            <input type="text" class="editable Id" />
                        </div>

                        <div class="fieldblock">
                            <label>Status</label>
                            <input type="text" class="editable InvestmentStatus" />
                        </div>

                    </div>

                    <div class="twocolumn">
                        
                        <div class="fieldblock">
                            <label>Investment Number</label>
                            <input type="text" class="editable InvestmentNumber"  />
                        </div>

                        <div class="fieldblock">
                            <label>Balance</label>
                            <input type="text" class="editable Balance" />
                        </div>

                    </div>

                    <div class="twocolumn">
                        
                        <div class="fieldblock">
                            <label>Investment Type</label>
                            <input type="text" class="editable InvestmentType" />
                        </div>

                        <div class="fieldblock">
                            <label>Rate</label>
                            <input type="text" class="editable Rate" />
                        </div>

                    </div>

                    <div class="twocolumn">
                        
                        <div class="fieldblock">
                            <label>Description</label>
                            <input type="text" class="editable InvestmentDescription"  />
                        </div>

                        <div class="fieldblock">
                            <label>Maturity Date</label>
                            <input type="text" class="editable CurrentMaturityDate" />
                        </div>

                    </div>

                    <div class="twocolumn">
                        
                        <div class="fieldblock">
                            <label>Ownership Type</label>
                            <input type="text" class="editable InvestmentOwnershipType" />
                        </div>

                        <div class="fieldblock">
                            <label>CUSIP</label>
                            <input type="text" class="editable CUSIP" />
                        </div>

                    </div>

                </div>

                    <div class="accordions">

                        <h1>Attributes</h1>
                        <div class="editcontainer">

                            <div class="twocolumn">

                                <div class="fieldblock">
                                    <label>Purchase Date</label>
                                    <input type="text" class="editable PurchaseDate" />
                                </div>

                                <div class="fieldblock">
                                    <label>Last Maintenance Date</label>
                                    <input type="text" class="editable LastMaintenanceDate" />
                                </div>

                            </div>

                            <div class="twocolumn">

                                <div class="fieldblock">
                                    <label>Issuance Method</label>
                                    <input type="text" class="editable IssuanceMethod" />
                                </div>

                                <div class="fieldblock">
                                    <label>Last Transaction Date</label>
                                    <input type="text" class="editable LastTransactionDate" />
                                </div>

                            </div>

                            <div class="accordions">
                            <h1>Step-Up</h1>
                                <div class="editcontainer">
                                <%--Not attached to doing this as a nested accordion, just one idea--%>

                                    <div class="fieldblock">
                                        <label>Step-up Eligible</label>
                                        <input type="checkbox" class="editable StepUpEligible" />
                                    </div>

                                    <div class="fieldblock">
                                        <label>Step-up Date</label>
                                        <input type="text" class="editable StepUpDate" />
                                    </div>

                                     <div class="modalbuttons">
                                        <input type="button" class="processstepup editable" value="Process Step-up" />
                                    </div>

                                </div>
                            </div>
                        </div>

                        <h1>Deposits and Withdrawals</h1>
                        <div class="editcontainer">

                            <div class="dwgridcontainer"></div>

                        </div>

                        <h1>Interest</h1>
                        <div class="editcontainer"></div>

                        <h1>Linked Accounts</h1>
                        <div class="editcontainer"></div>

                        <h1>Maturity</h1>
                        <div class="editcontainer"></div>

                        <h1>Payment Preferences</h1>
                        <div class="editcontainer"></div>

                    </div>

            </div>

        </div>

  </div>


<%--    Modal section     --%>

</asp:Content>
