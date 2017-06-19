<%@ Page Title="" Language="C#" MasterPageFile="~/TabbedContent.Master" AutoEventWireup="true" CodeBehind="AccountSummary.aspx.cs" Inherits="DDI.UI.Web.Pages.GL.AccountSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {
            LoadSummaryTab();
        });

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>Summary</h1>

    <br />

    <div class="accountsummarycontainer">
        <div class="inlinecontainer">
            <span class="summaryleftprompts">
                <label>GL Account: </label>
            </span>
            <span class="summaryleftinput">
                <input type="text" class="AccountNumber readonly editable" readonly="read-only" maxlength="64" />
            </span>

            <span class="editaccountbutton floatright">
                <a href="#" class="editaccount editbutton">Edit</a>
            </span>
        </div>

        <br />
        <br />

        <div class="accountsegmentscontainer">
            <fieldset>
                <legend>Account Segments: </legend>

                <div class="inlinecontainer segmentgroup segmentgroup1" style="display: none">
                    <span class="accountsegment1">
                        <span class="summarysegmentprompt">
                            <label class="segment1prompt"></label>
                        </span>
                        <span class="summarysegmentinput">
                            <select class="Segment1Id segmentselect" />
                            <a href="#" title="" class="editsegment1 hidebuttons editbuttoninline"></a>
                            <a href="#" title="" class="newsegment1 hidebuttons newbuttoninline"></a>
                        </span>
                    </span>

                    <span class="summarysegmentdisplay">
                        <label class="segment1code segmentcode"></label>
                    </span>
                    <br />
                </div>

                <div class="inlinecontainer segmentgroup segmentgroup2" style="display: none">
                    <span class="accountsegment2">
                        <span class="summarysegmentprompt">
                            <label class="segment2prompt"></label>
                        </span>
                        <span class="summarysegmentinput">
                            <select class="Segment2Id segmentselect" />
                            <a href="#" title="" class="editsegment2 hidebuttons editbuttoninline"></a>
                            <a href="#" title="" class="newsegment2 hidebuttons newbuttoninline"></a>
                        </span>
                    </span>

                    <span class="summarysegmentdisplay">
                        <label class="segment2code segmentcode"></label>
                    </span>
                    <br />
                </div>

                <div class="inlinecontainer segmentgroup segmentgroup3" style="display: none">
                    <span class="accountsegment3">
                        <span class="summarysegmentprompt">
                            <label class="segment3prompt"></label>
                        </span>
                        <span class="summarysegmentinput">
                            <select class="Segment3Id segmentselect" />
                            <a href="#" title="" class="editsegment3 hidebuttons editbuttoninline"></a>
                            <a href="#" title="" class="newsegment3 hidebuttons newbuttoninline"></a>
                        </span>
                    </span>

                    <span class="summarysegmentdisplay">
                        <label class="segment3code segmentcode"></label>
                    </span>
                    <br />
                </div>

                <div class="inlinecontainer segmentgroup segmentgroup4" style="display: none">
                    <span class="accountsegment4">
                        <span class="summarysegmentprompt">
                            <label class="segment4prompt"></label>
                        </span>
                        <span class="summarysegmentinput">
                            <select class="Segment4Id segmentselect" />
                            <a href="#" title="" class="editsegment4 hidebuttons editbuttoninline"></a>
                            <a href="#" title="" class="newsegment4 hidebuttons newbuttoninline"></a>
                        </span>
                    </span>

                    <span class="summarysegmentdisplay">
                        <label class="segment4code segmentcode"></label>
                    </span>
                    <br />
                </div>

                <div class="inlinecontainer segmentgroup segmentgroup5" style="display: none">
                    <span class="accountsegment5">
                        <span class="summarysegmentprompt">
                            <label class="segment5prompt"></label>
                        </span>
                        <span class="summarysegmentinput">
                            <select class="Segment5Id segmentselect" />
                            <a href="#" title="" class="editsegment5 hidebuttons editbuttoninline"></a>
                            <a href="#" title="" class="newsegment5 hidebuttons newbuttoninline"></a>
                        </span>
                    </span>

                    <span class="summarysegmentdisplay">
                        <label class="segment5code segmentcode"></label>
                    </span>
                    <br />
                </div>

                <div class="inlinecontainer segmentgroup segmentgroup6" style="display: none">
                    <span class="accountsegment6">
                        <span class="summarysegmentprompt">
                            <label class="segment6prompt"></label>
                        </span>
                        <span class="summarysegmentinput">
                            <select class="Segment6Id segmentselect" />
                            <a href="#" title="" class="editsegment6 hidebuttons editbuttoninline"></a>
                            <a href="#" title="" class="newsegment6 hidebuttons newbuttoninline"></a>
                        </span>
                    </span>

                    <span class="summarysegmentdisplay">
                        <label class="segment6code segmentcode"></label>
                    </span>
                    <br />
                </div>

                <div class="inlinecontainer segmentgroup segmentgroup7" style="display: none">
                    <span class="accountsegment7">
                        <span class="summarysegmentprompt">
                            <label class="segment7prompt"></label>
                        </span>
                        <span class="summarysegmentinput">
                            <select class="Segment7Id segmentselect" />
                            <a href="#" title="" class="editsegment7 hidebuttons editbuttoninline"></a>
                            <a href="#" title="" class="newsegment7 hidebuttons newbuttoninline"></a>
                        </span>
                    </span>

                    <span class="summarysegmentdisplay">
                        <label class="segment7code segmentcode"></label>
                    </span>
                    <br />
                </div>

                <div class="inlinecontainer segmentgroup segmentgroup8" style="display: none">
                    <span class="accountsegment8">
                        <span class="summarysegmentprompt">
                            <label class="segment8prompt"></label>
                        </span>
                        <span class="summarysegmentinput">
                            <select class="Segment8Id segmentselect" />
                            <a href="#" title="" class="editsegment8 hidebuttons editbuttoninline"></a>
                            <a href="#" title="" class="newsegment8 hidebuttons newbuttoninline"></a>
                        </span>
                    </span>

                    <span class="summarysegmentdisplay">
                        <label class="segment8code segmentcode"></label>
                    </span>
                    <br />
                </div>

                <div class="inlinecontainer segmentgroup segmentgroup9" style="display: none">
                    <span class="accountsegment9">
                        <span class="summarysegmentprompt">
                            <label class="segment9prompt"></label>
                        </span>
                        <span class="summarysegmentinput">
                            <select class="Segment9Id segmentselect" />
                            <a href="#" title="" class="editsegment9 hidebuttons editbuttoninline"></a>
                            <a href="#" title="" class="newsegment9 hidebuttons newbuttoninline"></a>
                        </span>
                    </span>

                    <span class="summarysegmentdisplay">
                        <label class="segment9code segmentcode"></label>
                    </span>
                    <br />
                </div>

                <div class="inlinecontainer segmentgroup segmentgroup10" style="display: none">
                    <span class="accountsegment10">
                        <span class="summarysegmentprompt">
                            <label class="segment10prompt"></label>
                        </span>
                        <span class="summarysegmentinput">
                            <select class="Segment10Id segmentselect" />
                            <a href="#" title="" class="editsegment10 hidebuttons editbuttoninline"></a>
                            <a href="#" title="" class="newsegment10 hidebuttons newbuttoninline"></a>
                        </span>
                    </span>

                    <span class="summarysegmentdisplay">
                        <label class="segment10code segmentcode"></label>
                    </span>
                </div>
            </fieldset>

        </div>

        <div class="inlinecontainer">
            <span class="summaryleftprompts">
                <label>Description: </label>
            </span>
            <span class="summaryleftinput">
                <input type="text" class="Name editable" />
                <a href="#" title="" class="editbuttoninline invisible"></a>
                <a href="#" title="" class="newbuttoninline invisible"></a>
            </span>

            <span class="activeadjust">
                <span class="summaryrightprompts">
                    <label>Active: </label>
                </span>
                <span class="summaryrightcheckbox">
                    <input type="checkbox" class="IsActive editable activeadjust" />
                </span>
            </span>
        </div>

        <br />

        <div class="inlinecontainer">
            <span class="accountgroup1 accountgroup">
                <span class="summaryleftprompts">
                    <label class="group1prompt"></label>
                </span>
                <span class="summaryleftinput">
                    <select class="Group1Id editable" />
                    <a href="#" title="" class="editgroup1 hidebuttons editbuttoninline"></a>
                    <a href="#" title="" class="newgroup1 hidebuttons newbuttoninline"></a>
                </span>
            </span>

            <span>
                <span class="summaryrightprompts">
                    <label>Balances Normally: </label>
                </span>
                <span class="summaryrightinput">
                    <select class="IsNormallyDebit editable">
                        <option value="0">Credit</option>
                        <option value="1">Debit</option>
                    </select>
                </span>
            </span>
        </div>

        <br />

        <div class="inlinecontainer">
            <span class="accountgroup2 accountgroup">
                <span class="summaryleftprompts">
                    <label class="group2prompt"></label>
                </span>
                <span class="summaryleftinput">
                    <select class="Group2Id editable" />
                    <a href="#" title="" class="editgroup2 hidebuttons editbuttoninline"></a>
                    <a href="#" title="" class="newgroup2 hidebuttons newbuttoninline"></a>
                </span>
            </span>

            <span>
                <span class="summaryrightprompts">
                    <label>Beginning Balance: </label>
                </span>
                <span class="summaryrightinput">
                    <input type="text" class="BeginningBalance editable money justright" disabled="disabled" />
                </span>
            </span>
        </div>

        <br />

        <div class="inlinecontainer">
            <span class="accountgroup3 accountgroup">
                <span class="summaryleftprompts">
                    <label class="group3prompt"></label>
                </span>
                <span class="summaryleftinput">
                    <select class="Group3Id editable" />
                    <a href="#" title="" class="editgroup3 hidebuttons editbuttoninline"></a>
                    <a href="#" title="" class="newgroup3 hidebuttons newbuttoninline"></a>
                </span>
            </span>

            <span>
                <span class="summaryrightprompts">
                    <label>Activity: </label>
                </span>
                <span class="summaryrightinput">
                    <input type="text" class="Activity money justright" disabled="disabled" />
                </span>
            </span>
        </div>

        <br />

        <div class="inlinecontainer">
            <span class="accountgroup4 accountgroup">
                <span class="summaryleftprompts">
                    <label class="group4prompt"></label>
                </span>
                <span class="summaryleftinput">
                    <select class="Group4Id editable" />
                    <a href="#" title="" class="editgroup4 hidebuttons editbuttoninline"></a>
                    <a href="#" title="" class="newgroup4 hidebuttons newbuttoninline"></a>
                </span>
            </span>

            <span>
                <span class="summaryrightprompts">
                    <label>Ending Balance: </label>
                </span>
                <span class="summaryrightinput">
                    <input type="text" class="EndingBalance money justright" disabled="disabled" />
                </span>
            </span>
        </div>

        <br />

        <div class="closingaccountgroup inline inlinecontainer">
            <label class="inline accountselectorlabel">Closing Account: </label>
        </div>


        <div class="saveaccountbuttons floatright">
            <input type="button" class="saveaccount" value="Save" />
            <a href="#" class="cancelsaveaccount">Cancel</a>
        </div>

    </div>



    <div class="accountsegmentmodal" title="Account Segments" style="display: none;">

        <div class="modalcontent">

            <div class="fieldblock">
                <label>Segment</label>
                <label class="modalSegmentName readonly"></label>
            </div>

            <div class="fieldblock">
                <label>Code</label>
                <input type="text" class="as-Code editable" maxlength="30" />
            </div>

            <div class="fieldblock">
                <label>Name</label>
                <input type="text" class="as-Name editable" maxlength="128" />
            </div>

            <div class="modalbuttons">
                <input type="button" class="saveaccountsegmentbutton" value="Save" />
                <a href="#" class="cancelaccountsegmentmodal">Cancel</a>
            </div>

        </div>

    </div>

    <div class="accountgroupmodal" title="Account Groups" style="display: none;">

        <div class="modalcontent">

            <div class="fieldblock">
                <label>Group</label>
                <label class="modalGroupName readonly"></label>
            </div>

            <div class="fieldblock">
                <label>Name</label>
                <input type="text" class="ag-Name editable" maxlength="128" />
            </div>

            <div class="fieldblock group1only">
                <label>Account Category</label>
                <select class="ag-Category editable">
                    <option value="0">None</option>
                    <option value="1">Asset</option>
                    <option value="2">Liability</option>
                    <option value="3">Fund</option>
                    <option value="4">Revenue</option>
                    <option value="5">Expense</option>
                </select>
            </div>

            <div class="modalbuttons">
                <input type="button" class="saveaccountgroupbutton" value="Save" />
                <a href="#" class="cancelaccountgroupmodal">Cancel</a>
            </div>

        </div>

    </div>


</asp:Content>



