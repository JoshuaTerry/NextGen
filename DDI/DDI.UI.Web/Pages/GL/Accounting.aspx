<%@ Page Title="DDI - Accounting" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Accounting.aspx.cs" Inherits="DDI.UI.Web.Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../../Scripts/glaccountselection.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {

            $('.dostuff').click(function () {

                DoStuff();

            });

        });

        function DoStuff() {

            var fund = '';
            var container = $('<div>').addClass('fundsettingscontainer onecolumn');

            /* FISCAL YEAR */
            $('.contentcontainer').empty();
            var container = $('<div>').appendTo('.contentcontainer');
            var header = $('<div>');
            $('<label>').text('Setting for ').appendTo(header);
            var fundnamedisplay = $('<label>').addClass('FundLedgerId').appendTo(header);
            $('<hr>').addClass('').appendTo(header);
            $(header).append('<br />').appendTo(container);

            /* FUND */
            var selectfiscalyeargroup = $('<div>').addClass('twocolumn');
            var selectfiscalyearname = $('<label>').text('Fiscal Year: ');
            $('<select>').addClass('selectfiscalyear').appendTo(selectfiscalyearname);
            $(selectfiscalyearname).appendTo(selectfiscalyeargroup);
            $(selectfiscalyeargroup).append('<br />').append('<br />').appendTo(container);

            var selectfundgroup = $('<div>');
            var selectfundname = $('<label>').text('Fund: ');
            $('<select>').addClass('selectfund').appendTo(selectfundname);
            $(selectfundname).appendTo(selectfundgroup);
            $(selectfundgroup).appendTo(container);

            var fiscalyearid = '';

            MakeServiceCall('GET', 'ledgers/businessunit/' + currentBusinessUnit.Id + '?fields=all', null, function (data) {
                var ledger = data.Data[0];
                $('.FundLedgerId').text(ledger.Code);

                fiscalyearid = ledger.DefaultFiscalYearId;
                PopulateDropDown('.selectfiscalyear', 'fiscalyears/ledger/' + ledger.Id + '?fields=all', '', '', ledger.DefaultFiscalYearId, null, function () {
                    PopulateFundBusinessFromFiscalYear(fiscalyearid, ledger);

                    $('.selectfiscalyear').unbind('change');
                    $('.selectfiscalyear').change(function (e) {

                        fiscalyear = $('.selectfiscalyear').val();
                        PopulateFundBusinessFromFiscalYear(fiscalyear, ledger);
                        PopulateFundDueFromFund($('.selectfund').val());
                        $('.accountnumber').val("");

                    });

                    $('.selectfund').unbind('change');

                    $('.selectfund').change(function (e) {

                        e.preventDefault();
                        var fundid = $('.selectfund').val();
                        LoadFundGLAccountSelector($('.selectfiscalyear').val(), $('.FundLedgerId').val(), $('.selectfund').val())
                        PopulateFundDueFromFund(fundid);

                    });

                });



            }, null);

        }

        function PopulateFundBusinessFromFiscalYear(fiscalyearid, ledger) {
            
            PopulateFundFromFiscalYear(fiscalyearid, ledger);

            var businessduecolumns = [
                { dataField: 'Id', width: '0px' },
                { dataField: 'OffsettingBusinessUnit.Code', caption: 'Business Unit' },
                { dataField: 'FromLedgerAccount.AccountNumber', caption: 'Due From Account' },
                { dataField: 'FromLedgerAccount.Name', caption: 'Description' },
                { dataField: 'ToLedgerAccount.AccountNumber', caption: 'Due To Account' },
                { dataField: 'ToLedgerAccount.Name', caption: 'Description' }
            ];

            LoadGrid('.businessunitduecontainer', 'businessunitduegrid', businessduecolumns, 'fiscalyears/' + fiscalyearid + '/businessunitfromto', '', null, '',
                '', '.businessunitduemodal', 250, true, false, false, null);
        }

        function PopulateFundFromFiscalYear(fiscalyearid, ledger, fundid) {
            
            PopulateDropDown('.selectfund', 'fund/' + fiscalyearid + '/fiscalyear', '', '', '', function () {

                fundid = $('.selectfund').val();
                LoadFundGLAccountSelector(fiscalyearid, ledger, fundid);
                PopulateFundDueFromFund(fundid);

                $('.selectfund').unbind('change');
                $('.selectfund').change(function (e) {
                    LoadFundGLAccountSelector(fiscalyearid, ledger, fundid);
                    PopulateFundDueFromFund(fundid);

                });

            }, null);

            LoadFundGLAccountSelector(fiscalyearid, ledger, fundid);

        }

        function LoadFundGLAccountSelector(fiscalyearid, ledger, fundid) {

            if (($('.selectfundbalanceaccount').children().length <= 0)) {

                GLAccountSelector($('.selectfundbalanceaccount'), ledger.Id, fiscalyearid);
                GLAccountSelector($('.selectclosingrevenueaccount'), ledger.Id, fiscalyearid);
                GLAccountSelector($('.selectclosingexpenseaccount'), ledger.Id, fiscalyearid);

            }
            else {

                MakeServiceCall('GET', 'fund/' + fundid, null, function (data) {

                    var fund = data.Data;

                    LoadSelectedAccount($('.selectfundbalanceaccount'), fund.FundBalanceAccountId);
                    LoadSelectedAccount($('.selectclosingrevenueaccount'), fund.ClosingRevenueAccountId);
                    LoadSelectedAccount($('.selectclosingexpenseaccount'), fund.ClosingExpenseAccountId);
                });

            }
        }

        /* POPULATING FUND DUE FROM FUND */
        function PopulateFundDueFromFund(fundid) {

            var fundduecolumns = [
                { dataField: 'Id', width: '0px' },
                { dataField: 'DisplayName', caption: 'Fund' },
                { dataField: 'FromLedgerAccount.AccountNumber', caption: 'Due From Account' },
                { dataField: 'FromLedgerAccount.Name', caption: 'Description' },
                { dataField: 'ToLedgerAccount.AccountNumber', caption: 'Due To Account' },
                { dataField: 'ToLedgerAccount.Name', caption: 'Description' }
            ];
            LoadGrid('.fundduecontainer', 'fundduegrid', fundduecolumns, 'funds/' + fundid + '/fundfromto', '', null, '',
                '', '.fundduemodal', 250, true, false, false, null);
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <input type="button" class="dostuff" value="Do Stuff" />

    <div class="contentcontainer"></div>

    <div class="testas">
        <div class="selectfundbalanceaccount"></div>
        <div class="selectclosingrevenueaccount"></div>
        <div class="selectclosingexpenseaccount"></div>
    </div>

    <div class="businessunitduecontainer"></div>

</asp:Content>
