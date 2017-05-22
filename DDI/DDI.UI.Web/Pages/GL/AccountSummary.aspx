<%@ Page Title="" Language="C#" MasterPageFile="~/TabbedContent.Master" AutoEventWireup="true" CodeBehind="AccountSummary.aspx.cs" Inherits="DDI.UI.Web.Pages.GL.AccountSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {
            LoadSummaryTab('');    // test new
            //LoadSummaryTab('0A5110A1-BA39-4D6B-BF83-E9D319D691C3');
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
            <input type="text" class="AccountNumber readonly editable" readonly="read-only" />
        </span>
    </div>

    <br />
    <br />

    <div class="accountsegmentscontainer">
      <fieldset>
        <legend>Account Segments: </legend>

        <div class="inlinecontainer segmentgroup1">
            <span class="accountsegment1">
                <span class="summarysegmentprompt">
                    <label class="segment1prompt"></label>
                </span>
                <span class="summarysegmentinput">
                    <select class="Segment1Id segmentselect" />
                    <a href="#" title="" class="editsegment1 editbuttoninline"></a>
                    <a href="#" title="" class="newsegment1 newbuttoninline"></a>
                </span>
            </span>

            <span class="summarysegmentdisplay">
                <label class="segment1code segmentcode"></label>
                <label class="segment1name segmentname readonly"></label>
            </span>
            <br />
        </div>

        <div class="inlinecontainer segmentgroup2">
            <span class="accountsegment2">
                <span class="summarysegmentprompt">
                    <label class="segment2prompt"></label>
                </span>
                <span class="summarysegmentinput">
                    <select class="Segment2Id segmentselect" />
                    <a href="#" title="" class="editsegment2 editbuttoninline"></a>
                    <a href="#" title="" class="newsegment2 newbuttoninline"></a>
                </span>
            </span>

            <span class="summarysegmentdisplay">
                <label class="segment2code segmentcode"></label>
                <label class="segment2name segmentname readonly"></label>
            </span>
            <br />
        </div>

        <div class="inlinecontainer segmentgroup3">
            <span class="accountsegment3">
                <span class="summarysegmentprompt">
                    <label class="segment3prompt"></label>
                </span>
                <span class="summarysegmentinput">
                    <select class="Segment3Id segmentselect" />
                    <a href="#" title="" class="editsegment3 editbuttoninline"></a>
                    <a href="#" title="" class="newsegment3 newbuttoninline"></a>
                </span>
            </span>

            <span class="summarysegmentdisplay">
                <label class="segment3code segmentcode"></label>
                <label class="segment3name segmentname readonly"></label>
            </span>
            <br />
        </div>

        <div class="inlinecontainer segmentgroup4">
            <span class="accountsegment4">
                <span class="summarysegmentprompt">
                    <label class="segment4prompt"></label>
                </span>
                <span class="summarysegmentinput">
                    <select class="Segment4Id segmentselect" />
                    <a href="#" title="" class="editsegment4 editbuttoninline"></a>
                    <a href="#" title="" class="newsegment4 newbuttoninline"></a>
                </span>
            </span>

            <span class="summarysegmentdisplay">
                <label class="segment4code segmentcode"></label>
                <label class="segment4name segmentname readonly"></label>
            </span>
            <br />
        </div>

        <div class="inlinecontainer segmentgroup5">
            <span class="accountsegment5">
                <span class="summarysegmentprompt">
                    <label class="segment5prompt"></label>
                </span>
                <span class="summarysegmentinput">
                    <select class="Segment5Id segmentselect" />
                    <a href="#" title="" class="editsegment5 editbuttoninline"></a>
                    <a href="#" title="" class="newsegment5 newbuttoninline"></a>
                </span>
            </span>

            <span class="summarysegmentdisplay">
                <label class="segment5code segmentcode"></label>
                <label class="segment5name segmentname readonly"></label>
            </span>
            <br />
        </div>

        <div class="inlinecontainer segmentgroup6">
            <span class="accountsegment6">
                <span class="summarysegmentprompt">
                    <label class="segment6prompt"></label>
                </span>
                <span class="summarysegmentinput">
                    <select class="Segment6Id segmentselect" />
                    <a href="#" title="" class="editsegment6 editbuttoninline"></a>
                    <a href="#" title="" class="newsegment6 newbuttoninline"></a>
                </span>
            </span>

            <span class="summarysegmentdisplay">
                <label class="segment6code segmentcode"></label>
                <label class="segment5name segmentname readonly"></label>
            </span>
            <br />
        </div>

        <div class="inlinecontainer segmentgroup7">
            <span class="accountsegment7">
                <span class="summarysegmentprompt">
                    <label class="segment7prompt"></label>
                </span>
                <span class="summarysegmentinput">
                    <select class="Segment7Id segmentselect" />
                    <a href="#" title="" class="editsegment7 editbuttoninline"></a>
                    <a href="#" title="" class="newsegment7 newbuttoninline"></a>
                </span>
            </span>

            <span class="summarysegmentdisplay">
                <label class="segment7code segmentcode"></label>
                <label class="segment7name segmentname readonly"></label>
            </span>
            <br />
        </div>

        <div class="inlinecontainer segmentgroup8">
            <span class="accountsegment8">
                <span class="summarysegmentprompt">
                    <label class="segment8prompt"></label>
                </span>
                <span class="summarysegmentinput">
                    <select class="Segment8Id segmentselect" />
                    <a href="#" title="" class="editsegment8 editbuttoninline"></a>
                    <a href="#" title="" class="newsegment8 newbuttoninline"></a>
                </span>
            </span>

            <span class="summarysegmentdisplay">
                <label class="segment8code segmentcode"></label>
                <label class="segment8name segmentname readonly"></label>
            </span>
            <br />
        </div>

        <div class="inlinecontainer segmentgroup9">
            <span class="accountsegment9">
                <span class="summarysegmentprompt">
                    <label class="segment9prompt"></label>
                </span>
                <span class="summarysegmentinput">
                    <select class="Segment9Id segmentselect" />
                    <a href="#" title="" class="editsegment9 editbuttoninline"></a>
                    <a href="#" title="" class="newsegment9 newbuttoninline"></a>
                </span>
            </span>

            <span class="summarysegmentdisplay">
                <label class="segment9code segmentcode"></label>
                <label class="segment9name segmentname readonly"></label>
            </span>
            <br />
        </div>

        <div class="inlinecontainer segmentgroup10">
            <span class="accountsegment10">
                <span class="summarysegmentprompt">
                    <label class="segment10prompt"></label>
                </span>
                <span class="summarysegmentinput">
                    <select class="Segment10Id segmentselect" />
                    <a href="#" title="" class="editsegment10 editbuttoninline"></a>
                    <a href="#" title="" class="newsegment10 newbuttoninline"></a>
                </span>
            </span>

            <span class="summarysegmentdisplay">
                <label class="segment10code segmentcode"></label>
                <label class="segment10name segmentname readonly"></label>
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
            <a href="#" title="" class="newgroup1 newbuttoninline invisible"></a>
        </span>

        <span class="activeadjust">
            <span class="summaryrightprompts">
                <label >Active: </label>
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
                <a href="#" title="" class="editgroup1 editbuttoninline"></a>
                <a href="#" title="" class="newgroup1 newbuttoninline"></a>
            </span>
        </span>

        <span>
            <span class="summaryrightprompts">
                <label >Balances Normally: </label>
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
                <a href="#" title="" class="editgroup2 editbuttoninline"></a>
                <a href="#" title="" class="newgroup2 newbuttoninline"></a>
            </span>
        </span>

        <span>
            <span class="summaryrightprompts">
                <label>Beginning Balance: </label>
            </span>
            <span class="summaryrightinput">
                <input type="number" class="BeginningBalance editable money justright" disabled="disabled" />
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
                <a href="#" title="" class="editgroup3 editbuttoninline"></a>
                <a href="#" title="" class="newgroup3 newbuttoninline"></a>
            </span>
        </span>

        <span>
            <span class="summaryrightprompts">
                <label>Activity: </label>
            </span>
            <span class="summaryrightinput">
                <input type="text" class="Activity money justright" disabled="disabled"" />
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
                <a href="#" title="" class="editgroup4 editbuttoninline"></a>
                <a href="#" title="" class="newgroup4 newbuttoninline"></a>
            </span>
        </span>

        <span>
            <span class="summaryrightprompts">
                <label>Ending Balance: </label>
            </span>
            <span class="summaryrightinput">
                <input type="text" class="EndingBalance money justright" disabled="disabled"" />
            </span>
        </span>
    </div>

    <br />

    <div class="closingaccountgroup fieldblock inline inlinecontainer">
        <label class="inline accountselectorlabel">Closing Account: </label>
    </div>

    <div class="modalbuttons saveaccountbuttons hidebuttons">
        <input type="button" class="saveaccount" value="Save" />
        <a href="#" class="cancelsaveaccount">Cancel</a>
    </div>

    <div class="modalbuttons editaccountbutton hidebuttons">
        <a href="#" class="editaccount">Edit</a>
    </div>

    </div>



<div class="accountsegmentmodal" title="Account Segments" style="display: none;">

    <div class="modalcontent">

        <div class="fieldblock">
            <label>Segment</label>
            <label class="modalSegmentName readonly" ></label>
        </div>

        <div class="fieldblock">
            <label>Code</label>
            <input type="text" class="as-Code editable" />
        </div>

        <div class="fieldblock">
            <label>Name</label>
            <input type="text" class="as-Name editable" />
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
            <label class="modalGroupName readonly" ></label>
        </div>

        <div class="fieldblock">
            <label>Name</label>
            <input type="text" class="ag-Name editable" />
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



