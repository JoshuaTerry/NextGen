<%@ Page Title="DDI - Journals" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Journals.aspx.cs" Inherits="DDI.UI.Web.Journals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../../Scripts/gljournals.js"></script>
      <link rel="stylesheet" href="../../CSS/Journals.css" />
    
    

   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="searchcriteria">
        
        <div class="scrollable">
            <fieldset>
                <legend>Quick Search</legend>

                <div class="fieldblock">
                    <input type="text" class="searchquicksearch" />
                </div>

                                       

               <%-- <div class="accordions nocontrols">--%>

                      <div>
                        <div class="fieldblock">
                            <label>Type</label>
                             <select class="searchtype">
                                    <option ></option>
                                    <option value="0">Normal</option>
                                    <option value="1">Recurring</option>
                                    <option value="2">Template</option>
                                   
                             </select>
                        </div>

                        <div class="fieldblock">
                            <label>Journal#</label>
                            <input type="text" class="searchname" />
                        </div>

                        <div class="fieldblock">
                            <label>Transaction Date</label>
                            <input type="text" class="TransactionDate datepicker" />
                        </div>

                        <div class="fieldblock">
                            <label>through</label>
                             <input type="text" class="through datepicker" />
                        </div>

                        <div class="fieldblock">
                            <label>Created By</label>
                            <select class="searchcreated"></select>
                        </div>

                        <div class="fieldblock">
                            <label>Creation Date</label>
                              <input type="text" class="creationdate datepicker" />
                        </div>

                        <div class="fieldblock">
                            <label>Status</label>
                          <select class="searchstatus">
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
                              <input type="text" class="searchjournalmemo" />
                        </div>

                        <div class="fieldblock">
                            <label>Line Item Memo</label>
                            <input type="text" class="searchLineItemMemo" />
                        </div>


                             
                  

                </div>
                  <%--  </div>--%>
                

            </fieldset>
        </div>

        <div class="buttons">
            <input type="button" class="clearsearch" value="Clear" />
            <input type="button" class="dosearch" value="Search" />
        </div>

    

<%--     <script type="text/javascript">

        $(document).ready(function () {

            LoadjournalsaccountSettings();

        });

    </script>--%>
         </div>

    
    <div class="searchresults">
        <div class="gridcontainer scrollable"></div>
    </div>
      
  


</asp:Content>
