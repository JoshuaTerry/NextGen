<%@ Page Title="" Language="C#" MasterPageFile="~/TabbedContent.Master" AutoEventWireup="true" CodeBehind="JournalDetail.aspx.cs" Inherits="DDI.UI.Web.Pages.GL.JournalDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {
            JournalDetailLoad();
        });

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="journalbody maincontainer">

        <h1 class="journaltype"></h1>

        <br />

        <div class="editcontainer">
            <div>
                <div class="fieldblock">
                    <label class="journalstatus">Status: </label>
                    <label class="JournalStatus"></label>
                </div>
                <div class="editjournalbutton hidebuttons floatright">
                    <a href="#" class="editjournal editbutton">Edit</a>
                </div>
            </div>

            <div class="inlinecontainer">
                <span class="journalcolumn">
                    <span class="fieldblock leftprompt">
                        <label>Transaction Date:</label>
                    </span>
                    <span class="fieldblock leftinput">
                        <input type="text" class="editable TransactionDate date datepicker" />
                    </span>
                </span>
                <span class="journalcolumn fieldblock">
                    <label>Journal Memo:</label>
                </span>
                <span class="journalcolumn fieldblock">
                    <label>Journal Information:</label>
                </span>
            </div>

            <br />

            <div class="inlinecontainer">
                <span class="journalcolumn">
                    <span class="fieldblock leftprompt reverseondategroup">
                        <label>Reverse on this Date:</label>
                    </span>
                    <span class="fieldblock leftinput">
                        <input type="text" class="editable ReverseOnDate date datepicker" />
                    </span>
                </span>
                <span class="journalcolumn fieldblock">
                    <textarea rows="2" cols="60" maxlength="255" class="Comment editable"></textarea>
                </span>
                <span class="journalcolumn fieldblock">
                    <label>Created by: </label>
                    <label class="CreatedBy"></label>
                    <label>on: </label>
                    <label class="CreatedOn date"></label>
                </span>
            </div>

            <br />

            <div class="inlinecontainer">
                <span class="journalcolumn fieldblock"></span>
                <span class="journalcolumn fieldblock"></span>
                <span class="journalcolumn fieldblock">
                    <label>Last changed by: </label>
                    <label class="LastModifiedBy"></label>
                    <label>on: </label>
                    <label class="LastModifiedOn date"></label>
                </span>
            </div>

            <br />
            <br />

            <div>
                <a href="#" class="newjournallinemodallink">New Item</a>
                <div class="journallinegridcontainer">
                    <div class="journallinegrid"></div>
                </div>
            </div>

            <br />
            <br />

            <div class="savejournallinebuttons hidebuttons floatright">
                <input type="button" class="savejournalline" value="Save" />
                <a href="#" class="cancelsavejournalline">Cancel</a>
            </div>


        </div>

    </div>

    <div class="journallinemodal" title="Journal Line" style="display: none;">

        <div class="modalcontent">

            <div class="journallineaccountgroup">
                <label class="accountselectorlabel">GL Account: </label>
            </div>

            <div class="threecolumn">
                <div class="fieldblock">
                    <label>Amount: </label>
                    <input type="text" class="Amount editable money justright" />
                </div>

                <div class="fieldblock">
                    <input type="radio" name="debitcreditradio" value="Debit" />
                    Debit<br />
                    <input type="radio" name="debitcreditradio" value="Credit" />
                    Credit<br />
                </div>

                <div class="fieldblock">
                    <textarea rows="2" cols="70" maxlength="255" class="JournalLineComment"></textarea>
                </div>
            </div>

            <div class="threecolumn">
                <div class="fieldblock">
                    <label>Due: </label>
                    <select class="DoToMode editable">
                        <option value="0">None</option>
                        <option value="1">Due From</option>
                        <option value="2">Due To</option>
                    </select>
                    <label>Fund: </label>
                    <select class="SourceFundId">
                    </select>
                </div>
            </div>

            <div class="modalbuttons">
                <input type="button" class="savenewjournallinebutton" value="Save/New" />
                <input type="button" class="savejournallinebutton" value="Save" />
                <a href="#" class="canceljournallinemodal">Cancel</a>
            </div>

        </div>

    </div>
</asp:Content>
