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
                            <input type="text" class="editable ConstituentNumber" maxlength="9" />
                        </div>

                        <div class="fieldblock">
                            <label>Status</label>
                            <input type="text" class="editable FormattedName" />
                        </div>

                    </div>

                    <div class="twocolumn">
                        
                        <div class="fieldblock">
                            <label>Investment Number</label>
                            <input type="text" class="editable ConstituentNumber" maxlength="9" />
                        </div>

                        <div class="fieldblock">
                            <label>Balance</label>
                            <input type="text" class="editable FormattedName" />
                        </div>

                    </div>

                    <div class="twocolumn">
                        
                        <div class="fieldblock">
                            <label>Investment Type</label>
                            <input type="text" class="editable ConstituentNumber" maxlength="9" />
                        </div>

                        <div class="fieldblock">
                            <label>Rate</label>
                            <input type="text" class="editable FormattedName" />
                        </div>

                    </div>

                    <div class="twocolumn">
                        
                        <div class="fieldblock">
                            <label>Description</label>
                            <input type="text" class="editable ConstituentNumber" maxlength="9" />
                        </div>

                        <div class="fieldblock">
                            <label>Maturity Date</label>
                            <input type="text" class="editable FormattedName" />
                        </div>

                    </div>

                    <div class="twocolumn">
                        
                        <div class="fieldblock">
                            <label>Ownership Type</label>
                            <input type="text" class="editable ConstituentNumber" maxlength="9" />
                        </div>

                        <div class="fieldblock">
                            <label>CUSIP</label>
                            <input type="text" class="editable FormattedName" />
                        </div>

                    </div>

                </div>

                    <div class="accordions">

                        <h1>Attributes</h1>
                        <div class="editcontainer">

                            <div class="twocolumn">

                                <div class="fieldblock">
                                    <label>Purchase Date</label>
                                    <input type="text" class="editable" />
                                </div>

                                <div class="fieldblock">
                                    <label>Last Maintenance Date</label>
                                    <input type="text" class="editable" />
                                </div>

                            </div>

                            <div class="twocolumn">

                                <div class="fieldblock">
                                    <label>Issuance Method</label>
                                    <input type="text" class="editable" />
                                </div>

                                <div class="fieldblock">
                                    <label>Last Transaction Date</label>
                                    <input type="text" class="editable" />
                                </div>

                            </div>

                            <div class="accordions">
                            <h1>Process Step-up</h1>
                                <div class="editcontainer">
                                <%--Not attached to doing this as a nested accordion, just one idea--%>

                                    <div class="fieldblock">
                                        <label>Step-up Eligible</label>
                                        <input type="checkbox" class="editable" />
                                    </div>

                                    <div class="fieldblock">
                                        <label>Step-up Date</label>
                                        <input type="text" class="editable" />
                                    </div>

                                     <div class="modalbuttons">
                                        <input type="button" class="processstepup editable" value="Process Step-up" />
                                    </div>

                                </div>
                            </div>
                        </div>

                        <h1>Automatic Transactions</h1>
                        <div class="editcontainer"></div>

                        <h1>Interest</h1>
                        <div class="editcontainer">
                            <div class="threecolumn">

                                <div class="fieldblock">
                                    <label>Interest Frequency:</label>
                                </div>
                                <div>
                                    <select class="editable InterestFrequency">
                                        <option value=""></option>
                                        <option value="Monthly">Monthly</option>
                                        <option value="Quarter">Quarter</option>
                                        <option value="Semi-Annual">Semi-Annual</option>
                                        <option value="Annual">Annual</option>
                                        <option value="Maturity Only">Maturity Only</option>
                                        <option value="None">None</option>
                                    </select>
                                </div>

                            </div>

                            <div class="threecolumn">

                                <div class="fieldblock">
                                    <label>Rate:</label>
                                </div>
                                <div>
                                    <input type="number" class="InterestRate editable" />
                                </div>

                            </div>

                            <div class="threecolumn">

                                <div class="fieldblock">
                                    <label>Accrued Interest:</label>
                                </div>
                                <div>
                                    <input type="text" class="InterestAccrued readonly" readonly="read-only" />
                                </div>

                            </div>

                            <div class="threecolumn">

                                <div class="fieldblock">
                                    <label>Last Interest Calculation Dt:</label>
                                </div>
                                <div>
                                    <input type="text" class="InterestLastCalcDate readonly"  readonly="read-only" />
                                </div>

                            </div>

                            <div class="threecolumn">

                                <div class="fieldblock">
                                    <label>Interest Paid YTD:</label>
                                </div>
                                <div>
                                    <input type="text" class="InterestPaidYTD readonly"  readonly="read-only" />
                                </div>

                            </div>

                            <div class="threecolumn">

                                <div class="fieldblock">
                                    <label>Interest Withheld YTD:</label>
                                </div>
                                <div>
                                    <input type="text" class="InterestWithheldYTD readonly"  readonly="read-only" />
                                </div>

                            </div>

                            <div class="threecolumn">

                                <div class="fieldblock">
                                    <label>Penalty Charged YTD:</label>
                                </div>
                                <div>
                                    <input type="text" class="InterestPenaltyYTD readonly"  readonly="read-only" />
                                </div>

                            </div>

                            <div class="accordions">
                            <h1>Interest Payment</h1>
                                <div class="interestpaymentgridcontainer"></div>
                            <h1>IRS Information</h1>
                                <div class="interestIRSgridcontainer"></div>
                            </div>

                            <div>
                                <div class="interestpaymenttable"></div>
                            </div>
                        </div>

                        <h1>Linked Accounts</h1>
                            <div class="editcontainer">
                                <div class="accordions">

                                <h1>Linked Account Detail<a href="#" title="New" class="newlinkedaccountsmodallink newbutton"></a></h1>
                                <div class="linkedaccountsgridcontainer"></div>
                            </div>

                        </div>

                        <h1>Maturity</h1>
                        <div class="editcontainer"></div>

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

            <input type="hidden" class="interest-Id" />
            <input type="hidden" class="hidconstituentlookupid" />

            <div class="fieldblock fourcolumn">
                <label>Priority</label>
                <input type="number" class="interest-Priority" />

                <label>Method</label>
                <select class="interest-Category">
                    <option value=""></option>
                    <option value="ACH">ACH</option>
                    <option value="Check">Check</option>
                    <option value="Compound">Compound</option>
                    <option value="Donation">Donation</option>
                    <option value="Investment Deposit">Investment Deposit</option>
                    <option value="Wire">Wire</option>
                </select>
            </div>

            <div class="fieldblock fourcolumn">
                <input type="radio" name="gender" value="Percent"/>Percent<br />
                <input type="radio" name="gender" value="Amount"/>Amount
                <input type="number" class="interest-AmtPct" />
            </div>

            <div class="fieldblock">
                <label></label>
                <input type="text" class="rs-FormattedName2 constituentlookup" />
            </div>

            <div class="fieldblock interestOptionACHWire fourcolumn">
                <label>Payment Preference:</label>
                <select class="interest-PaymentPreference">
                    <option value=""></option>
                    <option value="Default">Default</option>
                </select>
            </div>

            <div class="fieldblock interestOptionCheck fourcolumn">
                <label>Id:</label>
                <input type="text" class="interest-CheckId" />
                <label>Name:</label>
                <input type="text" class="interest-CheckName" />
            </div>

            <div class="fieldblock interestOptionDonation">
                <fieldset>
                    <legend>Dontation Template</legend>
                </fieldset>            
            </div>

            <div class="fieldblock interestOptionCompound">
            </div>

        </div>

    </div>

    <div class="linkedaccountsmodal" title="Linked Accounts" style="display: none;">

        <div class="modalcontent">

            <input type="hidden" class="interest-Id" />
            <input type="hidden" class="hidconstituentlookupid" />

            <div class="fieldblock twocolumn">

                <label>Method</label>
                <select class="linkedAccounts-Type">
                    <option value=""></option>
                    <option value="Down Payment">Down Payment</option>
                    <option value="Grant">Grant</option>
                    <option value="Loan Support">Loan Support</option>
                    <option value="Pool">Pool</option>
                </select>
            </div>

            <div class="fieldblock fourcolumn">
                <label>Loan</label>
                <input type="checkbox" class="editable LinkedAccountsLoan" />
                <label>Loan Number:</label>
                <input type="text" class="rs-LoanName loanlookup" />
            </div>

            <div class="fieldblock interestOptionACHWire fourcolumn">
                <label>Collateral on Loan</label>
                <input type="checkbox" class="editable LinkedAccountsLoanCollateral" />
                <div class="fieldblock loanCollateralOption">
                    <fieldset>
                        <input type="radio" name="loancollateral" value="Percent"/>Percent<br />
                        <input type="radio" name="loancollateral" value="Amount"/>Amount
                    </fieldset>            
                </div>
            </div>

            <div>
                <label>Block Investments from linking to other loans:</label>
                <input type="checkbox" class="editable LoanBlockLinkedInvestments" />
            </div>

        </div>

    </div>

</asp:Content>
