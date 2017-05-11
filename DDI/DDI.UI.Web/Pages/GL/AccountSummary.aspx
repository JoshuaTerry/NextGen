﻿<%@ Page Title="" Language="C#" MasterPageFile="~/TabbedContent.Master" AutoEventWireup="true" CodeBehind="AccountSummary.aspx.cs" Inherits="DDI.UI.Web.Pages.GL.AccountSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="..\..\CSS\accounts.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>Summary</h1>

    <br />

    <div class="accountsegmentscontainer">
      <fieldset>
        <legend>Account Segments:</legend>

        <div class="inlinecontainer segmentgroup1">
            <span class="accountsegment1">
                <span class="summarysegmentprompt">
                    <label class="segment1prompt"></label>
                </span>
                <span class="summarysegmentinput">
                    <select class="segment1dropdown" />
                    <a href="#" title="" class="editsegment1 editbuttoninline"></a>
                </span>
            </span>

            <span class="summarysegmenttext">
                <label class="segment1name"></label>
            </span>
        </div>

        <div class="inlinecontainer segmentgroup2">
            <span class="accountsegment2">
                <span class="summarysegmentprompt">
                    <label class="segment2prompt"></label>
                </span>
                <span class="summarysegmentinput">
                    <select class="segment2dropdown" />
                    <a href="#" title="" class="editsegment2 editbuttoninline"></a>
                </span>
            </span>

            <span class="summarysegmenttext">
                <label class="segment2name"></label>
            </span>
        </div>

        <div class="inlinecontainer segmentgroup3">
            <span class="accountsegment3">
                <span class="summarysegmentprompt">
                    <label class="segment3prompt"></label>
                </span>
                <span class="summarysegmentinput">
                    <select class="segment3dropdown" />
                    <a href="#" title="" class="editsegment3 editbuttoninline"></a>
                </span>
            </span>

            <span class="summarysegmenttext">
                <label class="segment3name"></label>
            </span>
        </div>

        <div class="inlinecontainer segmentgroup4">
            <span class="accountsegment4">
                <span class="summarysegmentprompt">
                    <label class="segment4prompt"></label>
                </span>
                <span class="summarysegmentinput">
                    <select class="segment4dropdown" />
                    <a href="#" title="" class="editsegment4 editbuttoninline"></a>
                </span>
            </span>

            <span class="summarysegmenttext">
                <label class="segment4name"></label>
            </span>
        </div>

        <div class="inlinecontainer segmentgroup5">
            <span class="accountsegment5">
                <span class="summarysegmentprompt">
                    <label class="segment5prompt"></label>
                </span>
                <span class="summarysegmentinput">
                    <select class="segment5dropdown" />
                    <a href="#" title="" class="editsegment5 editbuttoninline"></a>
                </span>
            </span>

            <span class="summarysegmenttext">
                <label class="segment5name"></label>
            </span>
        </div>

        <div class="inlinecontainer segmentgroup6">
            <span class="accountsegment6">
                <span class="summarysegmentprompt">
                    <label class="segment6prompt"></label>
                </span>
                <span class="summarysegmentinput">
                    <select class="segment6dropdown" />
                    <a href="#" title="" class="editsegment6 editbuttoninline"></a>
                </span>
            </span>

            <span class="summarysegmenttext">
                <label class="segment6name"></label>
            </span>
        </div>

        <div class="inlinecontainer segmentgroup7">
            <span class="accountsegment7">
                <span class="summarysegmentprompt">
                    <label class="segment7prompt"></label>
                </span>
                <span class="summarysegmentinput">
                    <select class="segment7dropdown" />
                    <a href="#" title="" class="editsegment7 editbuttoninline"></a>
                </span>
            </span>

            <span class="summarysegmenttext">
                <label class="segment7name"></label>
            </span>
        </div>

        <div class="inlinecontainer segmentgroup8">
            <span class="accountsegment8">
                <span class="summarysegmentprompt">
                    <label class="segment8prompt"></label>
                </span>
                <span class="summarysegmentinput">
                    <select class="segment8dropdown" />
                    <a href="#" title="" class="editsegment8 editbuttoninline"></a>
                </span>
            </span>

            <span class="summarysegmenttext">
                <label class="segment8name"></label>
            </span>
        </div>

        <div class="inlinecontainer segmentgroup9">
            <span class="accountsegment9">
                <span class="summarysegmentprompt">
                    <label class="segment9prompt"></label>
                </span>
                <span class="summarysegmentinput">
                    <select class="segment9dropdown" />
                    <a href="#" title="" class="editsegment9 editbuttoninline"></a>
                </span>
            </span>

            <span class="summarysegmenttext">
                <label class="segment9name"></label>
            </span>
        </div>

        <div class="inlinecontainer segmentgroup10">
            <span class="accountsegment10">
                <span class="summarysegmentprompt">
                    <label class="segment10prompt"></label>
                </span>
                <span class="summarysegmentinput">
                    <select class="segment10dropdown" />
                    <a href="#" title="" class="editsegment10 editbuttoninline"></a>
                </span>
            </span>

            <span class="summarysegmenttext">
                <label class="segment10name"></label>
            </span>
        </div>
      </fieldset>

    </div>

    <div class="inlinecontainer">
        <span class="summaryleftprompts">
            <label>GL Account: </label>
        </span>
        <span class="summaryleftinput">
            <input type="text" class="AccountNumber readonly" readonly="read-only" />
            <a href="#" title="" class="editaccount editbuttoninline"></a>
        </span>

        <span>
            <span class="summaryrightprompts">
                <label >Active: </label>
            </span>
            <span class="summaryrightcheckbox">
                <input type="checkbox" class="IsActive" />
            </span>
        </span>
    </div>

    <br />

    <div class="inlinecontainer">
        <span class="summaryleftprompts">
            <label>Description: </label>
        </span>
        <span class="summaryleftinputwide">
            <input type="text" class="Name" />
        </span>
    </div>

    <br />

    <div class="inlinecontainer">
        <span class="accountgroup1">
            <span class="summaryleftprompts">
                <label class="group1prompt"></label>
            </span>
            <span class="summaryleftinput">
                <select class="group1dropdown" />
                <a href="#" title="" class="editgroup1 editbuttoninline"></a>
            </span>
        </span>

        <span>
            <span class="summaryrightprompts">
                <label >Balances Normally: </label>
            </span>
            <span class="summaryrightinput">
                <select class="IsNormallyDebit">
                    <option value="0">Credit</option>
                    <option value="1">Debit</option>
                </select>
            </span>
        </span>
    </div>

    <br />

    <div class="inlinecontainer">
        <span class="accountgroup2">
            <span class="summaryleftprompts">
                <label class="group2prompt"></label>
            </span>
            <span class="summaryleftinput">
                <select class="group2dropdown" />
                <a href="#" title="" class="editgroup2 editbuttoninline"></a>
            </span>
        </span>

        <span>
            <span class="summaryrightprompts">
                <label>Beginning Balance: </label>
            </span>
            <span class="summaryrightinput">
                <input type="text" class="BeginningBalance money justright" disabled="disabled"" />
            </span>
        </span>
    </div>

    <br />

    <div class="inlinecontainer">
        <span class="accountgroup3">
            <span class="summaryleftprompts">
                <label class="group3prompt"></label>
            </span>
            <span class="summaryleftinput">
                <select class="group3dropdown" />
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
        <span class="accountgroup4">
            <span class="summaryleftprompts">
                <label class="group4prompt"></label>
            </span>
            <span class="summaryleftinput">
                <select class="group4dropdown" />
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

    <div class="inlinecontainer">
        <div class="closingaccountgroup">
            <div class="summaryleftprompts">
                <label>Closing Account: </label>
            </div>
            <div class="closingaccountcontainer">
            </div>
            <div class="summaryrightinput">
                <label> (Accumulated Revenue)</label>
            </div>
        </div>
    </div>



</asp:Content>
