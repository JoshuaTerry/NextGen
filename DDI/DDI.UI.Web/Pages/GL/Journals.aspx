<%@ Page Title="DDI - Journals" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Journals.aspx.cs" Inherits="DDI.UI.Web.Journals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../../Scripts/gljournals.js"></script>
    <script type="text/javascript" src="../../Scripts/data.js"></script>
      <link rel="stylesheet" href="../../CSS/Journals.css" />
    
    

   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="searchcriteria">
        
        <div class="scrollable">
            <fieldset>
                <legend>Quick Search</legend>

                <div class="fieldblock">
                    <input type="text" class="searchQueryString" />
                </div>

                                       

               <%-- <div class="accordions nocontrols">--%>

                      <div>
                        <div class="fieldblock">
                            <label>JournalType</label>
                             <select class="searchjournaltype">
                                    <option ></option>
                                    <option value="0">Normal</option>
                                    <option value="1">Recurring</option>
                                    <option value="2">Template</option>
                                   
                             </select>
                        </div>

                        <div class="fieldblock">
                            <label>Journal#</label>
                            <input type="text" class="searchJournalNumber" />
                        </div>

                        <div class="fieldblock">
                            <label>Transaction Date From</label>
                            <input type="date" class="transactionDateFrom datepicker" />
                        </div>

                        <div class="fieldblock">
                            <label>Transaction Date To</label>
                            <input type="date" class="transactionDateTo datepicker" />
                        </div>

                        <div class="fieldblock">
                            <label>through</label>
                             <input type="text" class="through datepicker" />
                        </div>

                        <div class="fieldblock">
                            <label>Created By</label>
                            <select class="searchCreatedBy"></select>
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
                                   <option ></option>
                                   <option value="1">Active</option>
                                    <option value="2">Expired</option>
                                    <option value="4">Approved</option>
                                    <option value="5">Unapproved</option>
                                    <option value="6">Posted</option>
                                    <option value="7">Unposted</option>
                                     <option value="8">Deleted</option>
                          </select>
                        </div>

                        <div class="fieldblock">
                            <label>Journal Memo</label>
                              <input type="text" class="searchComment" />
                        </div>

                        <div class="fieldblock">
                            <label>Line Item Memo</label>
                            <input type="text" class="searchlineItemComment" />
                        </div>


                             
                  

                </div>
                

            </fieldset>
        </div>

        <div class="buttons">
            <input type="button" class="clearsearch" value="Clear" />
            <input type="button" class="dosearch" value="Search" />
        </div>

        </div>
    
    
    <div class="searchresults">
        <div class="buttons">
            <input type="button" class="addnewjournal" value="Add New Journal" />
        </div>
        <div class="scrollable">
            <div class="gridcontainer"></div>
        </div>
    </div>
      
  


</asp:Content>
