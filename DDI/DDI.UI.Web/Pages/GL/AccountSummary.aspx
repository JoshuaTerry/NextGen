<%@ Page Title="" Language="C#" MasterPageFile="~/TabbedContent.Master" AutoEventWireup="true" CodeBehind="AccountSummary.aspx.cs" Inherits="DDI.UI.Web.Pages.GL.AccountSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {
            //LoadSummaryTab('');    // test new
            LoadSummaryTab('BDF91701-0531-4787-91AA-1EEA7C65BF98');
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
                    <select class="segment1dropdown segmentselect" />
                    <a href="#" title="" class="editsegment1 editbuttoninline"></a>
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
                    <select class="segment2dropdown segmentselect" />
                    <a href="#" title="" class="editsegment2 editbuttoninline"></a>
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
                    <select class="segment3dropdown segmentselect" />
                    <a href="#" title="" class="editsegment3 editbuttoninline"></a>
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
                    <select class="segment4dropdown segmentselect" />
                    <a href="#" title="" class="editsegment4 editbuttoninline"></a>
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
                    <select class="segment5dropdown segmentselect" />
                    <a href="#" title="" class="editsegment5 editbuttoninline"></a>
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
                    <select class="segment6dropdown segmentselect" />
                    <a href="#" title="" class="editsegment6 editbuttoninline"></a>
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
                    <select class="segment7dropdown segmentselect" />
                    <a href="#" title="" class="editsegment7 editbuttoninline"></a>
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
                    <select class="segment8dropdown segmentselect" />
                    <a href="#" title="" class="editsegment8 editbuttoninline"></a>
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
                    <select class="segment9dropdown segmentselect" />
                    <a href="#" title="" class="editsegment9 editbuttoninline"></a>
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
                    <select class="segment10dropdown segmentselect" />
                    <a href="#" title="" class="editsegment10 editbuttoninline"></a>
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

    <div>
        <div class="closingaccountgroup">
            <div class="closingaccountcontainer fieldblock inline">
                <label class="inline">Closing Account: </label>
            </div>
        </div>
    </div>

    <div class="modalbuttons saveaccountbuttons hidebuttons">
        <input type="button" class="saveaccount" value="Save" />
        <a href="#" class="cancelsaveaccount">Cancel</a>
    </div>

    <div class="modalbuttons editaccountbutton hidebuttons">
        <a href="#" class="editaccount">Edit</a>
    </div>

    </div>




</asp:Content>



