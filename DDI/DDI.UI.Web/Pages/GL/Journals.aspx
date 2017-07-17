<%@ Page Title="DDI - Journals" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Journals.aspx.cs" Inherits="DDI.UI.Web.Journals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../../Scripts/gljournals.js"></script>
    <script type="text/javascript" src="../../Scripts/data.js"></script>
    <link rel="stylesheet" href="../../CSS/Journals.css" />

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="UtilityMenuContainer" runat="server">

    <li><a href="#" class="addnewjournal">New Journal</a></li>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="searchcriteria">

        <div>
            <fieldset>
                <div>
                    <div class="fieldblock">
                        <label>Journal Type</label>
                        <select class="searchjournaltype">
                            <option value="">(All)</option>
                            <option value="0">Normal</option>
                            <option value="1">Recurring</option>
                            <option value="2">Template</option>
                        </select>
                    </div>
                   
                    <div class="fieldblock">
                        <label>Journal No.</label>
                        <input type="text" class="searchJournalNumber" maxlength="5" />
                    </div>

                    <div class="fieldblock">
                        <label>Transaction Date From</label>
                        <input type="text" class="transactionDateFrom datepicker" />
                    </div>

                    <div class="fieldblock">
                        <label>Transaction Date To</label>
                        <input type="text" class="transactionDateTo datepicker" />
                    </div>

                    <div class="fieldblock">
                        <label>Creation Date From</label>
                        <input type="text" class="CreatedOnFrom datepicker" />
                    </div>

                    <div class="fieldblock">
                        <label>Creation Date To</label>
                        <input type="text" class="CreatedOnTo datepicker" />
                    </div>

                    <div class="fieldblock">
                        <label>Status</label>
                        <select class="searchjournalStatus">
                            <option value="">(All)</option>
                            <option value="Active">Active</option>
                            <option value="Expired">Expired</option>
                            <option value="Approved">Approved</option>
                            <option value="Unapproved">Unapproved</option>
                            <option value="Posted">Posted</option>
                            <option value="Reversed">Reversed</option>
                            <option value="Unposted">Unposted</option>
                            <option value="Deleted">Deleted</option>

                        </select>
                    </div>

                    <div class="fieldblock">
                        <label>Journal Memo</label>
                        <input type="text" class="searchComment" maxlength="128" />
                    </div>

                    <div class="fieldblock">
                        <label>Line Item Memo</label>
                        <input type="text" class="searchlineItemComment" maxlength="128" />
                    </div>

                </div>

            </fieldset>
        </div>

        <div class="buttons">
            <input type="button" class="clearsearch" value="Clear" />
            <input type="button" class="dosearch" value="Search" />
        </div>

    </div>


    <div class="searchresults" style="height: 670px; width: 83%">
        <div class="scrollable">
            <div class="gridcontainer"></div>
        </div>
    </div>

    <div class="newjournalmodal" title="New Journal" style="display: none;">

        <div class="modalcontent">

            <div class="journaltypeselection">

                <div class="general" id="0">
                    <label>General Journal Entry</label>
                    <img src="../../Images/OneTimeJournal.gif" />
                </div>

                <div class="recurring" id="1">
                    <label>Recurring Journal Entry</label>
                    <img src="../../Images/RecurringJournal.gif" />
                </div>

                <div class="priorperiod" id="3">
                    <label>Prior Period Journal Entry</label>
                    <img src="../../Images/PriorPeriod.gif" />
                </div>

                <div class="template" id="2">
                    <label>Journal Template</label>
                    <img src="../../Images/TemplateJournal.gif" />
                </div>

            </div>

        </div>
        
    </div>

</asp:Content>
