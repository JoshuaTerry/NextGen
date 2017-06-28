<%@ Page Title="" Language="C#" MasterPageFile="~/TabbedContent.Master" AutoEventWireup="true" CodeBehind="JournalDetail.aspx.cs" Inherits="DDI.UI.Web.Pages.GL.JournalDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {
            JournalDetailLoad();
        });

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="journalbody">

        <h1 class="journaltype"></h1>

        <div class="editcontrols">
            <a href="#" class="editjournal editbutton">Edit</a>
        </div>

        <div class="editcontainer">
            <div class="fieldblock">
                <label class="inline">Status: </label>
                <label class="StatusDescription inline"></label>
            </div>

            <div class="threecolumn">

                <div class="fieldblock">
                    <h3>Transaction Date:</h3>
                    <input type="text" class="TransactionDate editable datepicker" />

                    <div>
                        <input id="ReverseJournal" type="checkbox" class="reversejournal editable" />
                        <label for="ReverseJournal" class="inline">Reverse this journal at a later date: </label>
                    </div>

                    <div>
                        <label>Reverse on:</label>
                        <input type="text" class="ReverseOnDate editable date datepicker" />
                    </div>
                </div>

                <div class="fieldblock">
                    <h3>Journal Memo:</h3>
                    <textarea rows="2" cols="60" maxlength="255" class="Comment editable"></textarea>
                </div>

                <div class="fieldblock">
                    <h3>Journal Information:</h3>
                    <div>
                        <label class="inline bold">Created by: </label>
                        <label class="CreatedBy inline"></label>
                        <label class="inline">on: </label>
                        <label class="CreatedOn date inline"></label>
                    </div>

                    <div>
                        <label class="inline bold">Last changed by: </label>
                        <label class="LastModifiedBy inline"></label>
                        <label class="inline">on: </label>
                        <label class="LastModifiedOn date inline"></label>
                    </div>

                    <div>
                        <label class="inline bold">Status:</label>
                        <label class="Status inline"></label>
                    </div>
                </div>
                
            </div>

            <div class="fieldblock">
                

                
            </div>

            <div class="fieldblock">
                <label>Occurs: </label>
                <select class="IsNormallyDebit editable">
                    <option value="0">Monthly</option>
                    <option value="1">Daily</option>
                    <option value="2">Weekly</option>
                    <option value="3">Yearly</option>
                </select>
            </div>

            <div class="expirationselection">
                <div class="fieldblock">
                    <label>Expires: </label>
                </div>

                <div class="fieldblock">
                    <input id="ExpiresNever" type="radio" name="expires" value="0" />
                    <label for="ExpiresNever" class="inline">Never</label>
                </div>

                <div class="fieldblock">
                    <input id="ExpiresAfterDate" type="radio" name="expires" value="1" />
                    <label for="ExpiresAfterDate" class="inline">After date</label>
                    <input type="text" class="ExpireDate editable datepicker inline" />
                </div>

                <div class="fieldblock">
                    <input id="ExpiresAfterCount" type="radio" name="expires" value="2" />
                    <label for="ExpiresAfterCount" class="inline">After count</label>
                    <input type="text" class="ExpireCount editable number inline" />
                </div>

                <div class="fieldblock">
                    <input id="ExpiresAfterAmount" type="radio" name="expires" value="3" />
                    <label for="ExpiresAfterAmount" class="inline">After amount</label>
                    <input type="text" class="ExpireAmount editable money inline" />
                </div>
            </div>
            
            <div>
                <a href="#" class="newjournallinemodallink newmodallink">New Item</a>
                <div class="journallinegridcontainer">
                    <div class="journallinegrid"></div>
                </div>
            </div>

            <div class="savejournalbuttons hidebuttons floatright">
                <input type="button" class="savejournal" value="Save" />
                <a href="#" class="cancelsavejournal">Cancel</a>
            </div>


        </div>

    </div>

    <div class="journallinemodal" title="Journal Line" style="display: none;">

        <div class="modalcontent">

            <div class="journallineaccountgroup">
                <select class="SourceFundId"></select>
                <label class="accountselectorlabel">GL Account: </label>
                <div class="journallineledgeraccountid"></div>
            </div>

            <div class="twocolumn">
                <div class="fieldblock">
                    <label>Amount: </label>
                    <input type="text" class="Amount editable money justright" />
                </div>

                <div class="fieldblock">
                    <input type="radio" name="debitcreditradio" value="Debit" />
                    <label>Debit</label>
                </div>
                <div class="fieldblock">
                    <input type="radio" name="debitcreditradio" value="Credit" />
                    <label>Credit</label>
                </div>

            </div>

            <div class="fieldblock">
                <label>Memo:</label>
                <textarea rows="2" cols="70" maxlength="255" class="JournalLineComment"></textarea>
            </div>

            <div class="twocolumn">

                <div class="fieldblock">
                    <label>Due: </label>
                    <select class="DoToMode editable">
                        <option value="0">None</option>
                        <option value="1">Due From</option>
                        <option value="2">Due To</option>
                    </select>
                </div>

                <div class="fieldblock">
                    <label>Fund: </label>
                    <select class="SourceFundId"></select>
                </div>

            </div>

            <div class="modalbuttons">
                <input type="button" class="savenewjournalline" value="Save/New" />
                <input type="button" class="savejournalline" value="Save" />
                <a href="#" class="canceljournallinemodal">Cancel</a>
            </div>

        </div>

    </div>

</asp:Content>
