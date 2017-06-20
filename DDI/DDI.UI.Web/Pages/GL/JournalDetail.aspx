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
                <span class="fieldblock">
                    <label>Status: </label>
                    <label class="StatusDescription"></label>
                </span>
                <span class="editjournalbutton hidebuttons floatright">
                    <a href="#" class="editjournal editbutton">Edit</a>
                </span>
            </div>

            <br />

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
                    <span class="fieldblock onetime">
                        <input type="checkbox" class="reversejournal editable" />
                        <label>Reverse this journal at a later date: </label>
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
                <span class="journalcolumn reverseondategroup invisible">
                    <span class="leftprompt">
                        <label>Reverse on:</label>
                    </span>
                    <span class="fieldblock leftinput">
                        <input type="text" class="ReverseOnDate editable date datepicker" />
                    </span>
                </span>
                <span class="journalcolumn fieldblock"></span>
                <span class="journalcolumn fieldblock">
                    <label>Last changed by: </label>
                    <label class="LastModifiedBy"></label>
                    <label>on: </label>
                    <label class="LastModifiedOn date"></label>
                </span>
            </div>

            <br />

            <div class="inlinecontainer">
                <span class="journalcolumn recurringgroup hidden">
                    <span class="leftprompt">
                        <label>Occurs: </label>
                    </span>
                    <span class="fieldblock leftinput">
                        <select class="IsNormallyDebit editable">
                            <option value="0">Monthly</option>
                            <option value="1">Daily</option>
                            <option value="2">Weekly</option>
                            <option value="3">Yearly</option>
                        </select>
                    </span>
                </span>
            </div>

            <br />

            <div class="inlinecontainer recurringgroup hidden">
                <span class="journalcolumn">
                    <span class="leftprompt">
                        <label>Expires: </label>
                    </span>
                </span>
            </div>

            <br />

            <div class="inlinecontainer recurringgroup hidden">
                <span class="journalcolumn">
                    <span class="leftprompt">
                        <input type="radio" name="expires" value="0" />
                        <label>Never</label>
                    </span>
                </span>
            </div>

            <br />

            <div class="inlinecontainer recurringgroup hidden">
                <span class="journalcolumn">
                    <span class="leftprompt">
                        <input type="radio" name="expires" value="1" />
                        <label>After date</label>
                    </span>
                    <span class="fieldblock leftinput">
                        <input type="text" class="editable ExpireDate date datepicker" />
                    </span>
                </span>
            </div>

            <br />

            <div class="inlinecontainer recurringgroup hidden">
                <span class="journalcolumn">
                    <span class="leftprompt">
                        <input type="radio" name="expires" value="2" />
                        <label>After count</label>
                    </span>
                    <span class="fieldblock leftinput">
                        <input type="text" class="editable ExpireCount number" />
                    </span>
                </span>
            </div>

            <br />

            <div class="inlinecontainer recurringgroup hidden">
                <span class="journalcolumn">
                    <span class="leftprompt">
                        <input type="radio" name="expires" value="2" />
                        <label>After amount</label>
                    </span>
                    <span class="fieldblock leftinput">
                        <input type="text" class="editable ExpireAmount money" />
                    </span>
                </span>
            </div>

            <br />
            <br />

            <div>
                <a href="#" class="newjournallinemodallink newmodallink">New Item</a>
                <div class="journallinegridcontainer">
                    <div class="journallinegrid"></div>
                </div>
            </div>

            <br />
            <br />

            <div class="savejournalbuttons hidebuttons floatright">
                <input type="button" class="savejournal" value="Save" />
                <a href="#" class="cancelsavejournal">Cancel</a>
            </div>


        </div>

    </div>

    <div class="journallinemodal" title="Journal Line" style="display: none;">

        <div class="modalcontent">

            <div class="journallineaccountgroup">
                <select class="SourceFundId">
                </select>
                <div>
                    <label class="accountselectorlabel">GL Account: </label>
                    <div class="journallineledgeraccountid"></div>
                </div>
            </div>

            <div class="twocolumn">
                <span class="fieldblock">
                    <label>Amount: </label>
                    <input type="text" class="Amount editable money justright" />
                </span>

                <span class="fieldblock">
                    <span>
                        <input type="radio" name="debitcreditradio" value="Debit" />
                        <label>Debit</label>
                    </span>
                    <span>
                        <input type="radio" name="debitcreditradio" value="Credit" />
                        <label>Credit</label>
                        <br />
                    </span>
                </span>

            </div>

            <br />

            <div class="fieldblock">
                <label>Memo:</label>
                <textarea rows="2" cols="70" maxlength="255" class="JournalLineComment"></textarea>
            </div>

            <br />

            <div class="twocolumn">
                <span class="fieldblock">
                    <label>Due: </label>
                    <select class="DoToMode editable">
                        <option value="0">None</option>
                        <option value="1">Due From</option>
                        <option value="2">Due To</option>
                    </select>
                    <label>Fund: </label>
                    <select class="SourceFundId">
                    </select>
                </span>
            </div>

            <br />

            <div class="modalbuttons">
                <input type="button" class="savenewjournalline" value="Save/New" />
                <input type="button" class="savejournalline" value="Save" />
                <a href="#" class="canceljournallinemodal">Cancel</a>
            </div>

        </div>

    </div>
</asp:Content>
