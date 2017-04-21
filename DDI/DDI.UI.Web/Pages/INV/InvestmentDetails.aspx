<%@ Page Title="DDI - Investment Details" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="InvestmentDetails.aspx.cs" Inherits="DDI.UI.Web.InvestmentDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="..\..\Scripts\systemsettings.js"></script>
    <script type="text/javascript" src="..\..\Scripts\InvestmentDetails.js"></script>
    <link rel="stylesheet" href="..\..\CSS\globalstyles.css" />
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
                                    <input type="date" class="editable PurchaseDate" />
                                </div>

                                <div class="fieldblock">
                                    <label>Last Maintenance Date</label>
                                    <input type="date" class="editable LastMaintenanceDate" />
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
                        <div class="editcontainer">
                            <div class="threecolumn">

                                <div class="fieldblock">
                                    <label>Interest Frequency:</label>
                                </div>
                                <div>
                                    <select class="InterestFrequency editable">
                                        <option value="0">None</option>
                                        <option value="1">Monthly</option>
                                        <option value="2">Quarter</option>
                                        <option value="3">Semi-Annual</option>
                                        <option value="4">Annual</option>
                                        <option value="5">Maturity Only</option>
                                    </select>
                                </div>

                            </div>

                            <div class="threecolumn">

                                <div class="fieldblock">
                                    <label>Rate:</label>
                                </div>
                                <div>
                                    <input type="number" class="Rate justright editable" />%
                                </div>

                            </div>

                            <div class="threecolumn">

                                <div class="fieldblock">
                                    <label>Accrued Interest:</label>
                                </div>
                                <div>
                                    <input type="text" class="AccruedInterest money justright" disabled="disabled" />
                                </div>

                            </div>

                            <div class="threecolumn">

                                <div class="fieldblock">
                                    <label>Last Interest Calculation Dt:</label>
                                </div>
                                <div>
                                    <input type="text" class="LastInterestCalculatedDate date" disabled="disabled" />
                                </div>

                            </div>

                            <div class="threecolumn">

                                <div class="fieldblock">
                                    <label>Interest Paid YTD:</label>
                                </div>
                                <div>
                                    <input type="text" class="InterestPaidYTD money justright" disabled="disabled" />
                                </div>

                            </div>

                            <div class="threecolumn">

                                <div class="fieldblock">
                                    <label>Interest Withheld YTD:</label>
                                </div>
                                <div>
                                    <input type="text" class="InterestWithheldYTD money justright" disabled="disabled" />
                                </div>

                            </div>

                            <div class="threecolumn">

                                <div class="fieldblock">
                                    <label>Penalty Charged YTD:</label>
                                </div>
                                <div>
                                    <input type="text" class="PenaltyChargedYTD money justright" disabled="disabled" />
                                </div>

                            </div>

                            <div class="accordions">
                                <h1>Interest Payment</h1>
                                <div class="interestpaymentgridcontainer">
                                    <div class="interestpaymentgrid"></div>
                                </div>
                                <h1>IRS Information</h1>
                                <div class="interestIRSgridcontainer">
                                    <div class="interestIRSgrid"></div>
                                </div>
                            </div>

                        </div>

                        <h1>Linked Accounts</h1>
                            <div class="editcontainer">
                                <div class="accordions">

                                <h1>Linked Account Detail<a href="#" title="New" class="newlinkedaccountsmodallink newbutton"></a></h1>
                                <div class="linkedaccountsgridcontainer">
                                    <div class="linkedaccountsgrid"></div>
                                </div>
                            </div>

                        </div>

                        <h1>Maturity</h1>
                        <div class="editcontainer">

                            <div class="twocolumn">

                                <div class="fieldblock">
                                    <label>Maturity Date</label>
                                    <input type="text" class="CurrentMaturityDate editable datepicker" />
                                </div>

                                 <div class="fieldblock">
                                    <label>Original Maturity Date</label>
                                    <input type="text" class="OriginalMaturityDate editable datepicker" />
                                </div>

                            </div>

                            <div class="twocolumn">

                                <div class="fieldblock">
                                    <label>Maturity Method</label>
                                    <input type="text" class="MaturityMethod editable" />
                                </div>
                                
                                <div class="fieldblock">
                                    <label>Last Maturity Date</label>
                                    <input type="text" class="LastMaturityDate editable datepicker" />
                                </div>

                            </div>

                            <div class="twocolumn">

                                 <div class="fieldblock">
                                    <label>Renewal Investment Type</label>
                                    <input type="text" class="RenewalInvestmentType editable datepicker" />
                                </div>

                            </div>

                            <div class="twocolumn">

                                 <div class="fieldblock">
                                    <label>Maturity Response Date</label>
                                    <input type="text" class="MaturityResponseDate editable datepicker" />
                                </div>

                            </div>

                            <div class="twocolumn">

                                 <div class="fieldblock">
                                    <label>Number of Renewals</label>
                                    <input type="text" class="NumberOfRenewals editable datepicker" />
                                </div>

                            </div>


                        </div>

                        <h1>Payment Preferences</h1>
                        <div class="editcontainer">
                        </div>

                    </div>

            </div>

        </div>
    </div>



<%--    Modal section     --%>

    <div class="interestpaymentmodal" title="Interest Payment" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="interestPayout-Id" />
            <input type="hidden" class="hidconstituentlookupid" />

            <div class="twocolumn">
                <div>
                    <label>Priority: </label>
                    <input type="number" class="Priority" />
                </div>

                <div>
                    <label>Method: </label>
                    <select class="interest-Category">
                        <option value="0">Compound</option>
                        <option value="1">ACH</option>
                        <option value="2">Check</option>
                        <option value="3">Investment Deposit</option>
                        <option value="4">Wire</option>
                    </select>
                </div>
            </div>

            <br />

            <div class="twocolumn">
                <div>
                    <input type="radio" name="interestpctamt" value="Percent"/>Percent
                    <input type="radio" name="interestpctamt" value="Amount"/>Amount
                </div>
                <div>
                    <input type="number" class="interest-AmtPct" />
                </div>
            </div>

            <br />

            <div class="interestOptionACHWire twocolumn">
                <div>
                <label>Payment Preference: </label>
                    <select class="interest-PaymentPreference">
                        <option value=""></option>
                        <option value="Default">Default</option>
                    </select>
                </div>
            </div>

            <br />

<%--            <div class="twocolumn">
                <label></label>
                <input type="text" class="rs-FormattedName2 constituentlookup" />
            </div>
--%>
            <div class="fieldblock interestOptionCheck twocolumn">
                <div>
                    <label>Id:</label>
                    <input type="text" class="interest-CheckId" />
                </div>
                <div>
                    <label>Name:</label>
                    <input type="text" class="interest-CheckName" />
                </div>
            </div>

            <br />

            <div class="fieldblock interestOptionDonation">
                <fieldset>
                    <legend>Dontation Template</legend>
                </fieldset>            
            </div>

        </div>

    </div>

    <div class="linkedaccountsmodal" title="Linked Accounts" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="interest-Id" />
            <input type="hidden" class="hidconstituentlookupid" />

            <div class="twocolumn">
                <div>
                    <label>Type: </label>
                    <select class="linkedAccountType">
                        <option value="0">Loan Support</option>
                        <option value="1">Pool</option>
                        <option value="2">Down Payment</option>
                        <option value="3">Grant</option>
                    </select>
                </div>
            </div>

            <br />

            <div class="twocolumn">
                <div>
                    <input type="checkbox" class="editable LinkedAccountsLoanInd" />
                    <label>Loan</label>
                </div>
                <div>
                    <label>Loan Number:</label>
                    <input type="text" class="rs-LoanName loanlookup" />
                </div>
            </div>

            <br />

            <div class="interestOptionACHWire twocolumn">
                <div>
                    <input type="checkbox" class="editable LinkedAccountsLoanCollateral" />
                    <label>Collateral on Loan</label>
                </div>
                <div class="loanCollateralOption">
                    <fieldset>
                        <span>
                            <input type="radio" name="loancollateral" value="Percent"/>Percent
                            <input type="radio" name="loancollateral" value="Amount"/>Amount
                        </span>
                    </fieldset>            
                </div>
            </div>

            <br />

            <div class="interestBlockLinkedInvestments">
                <input type="checkbox" class="editable LoanBlockLinkedInvestments" />
                <label>Block Investments from linking to other loans:</label>
            </div>

        </div>

    </div>

</asp:Content>
