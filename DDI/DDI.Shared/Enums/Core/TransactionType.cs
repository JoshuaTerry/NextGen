using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Attributes.Transactions;

namespace DDI.Shared.Enums.Core
{
    public enum TransactionType
    {
        None = 0,

        // Investments: 1 - 199

        // Loans: 200-499

        // Portfolio: 600 - 699

        // Line of Credit: 800 - 899

        // Extension Fund (Other): 900 - 9999
        [Description("Investment transaction (Converted)")]
        [TransactionType("Investment transaction (Converted)", ModuleType = ModuleType.Investments)]
        INConverted = 9991,
        [Description("Investment month end transaction (Converted)")]
        [TransactionType("Investment month end transaction (Converted)", ModuleType = ModuleType.Investments)]
        IEConverted = 9992,
        [Description("Loan transaction (Converted)")]
        [TransactionType("Loan transaction (Converted)", ModuleType = ModuleType.Loans)]
        LNConverted = 9993,
        [Description("Loan month end transaction (Converted)")]
        [TransactionType("Loan month end transaction (Converted)", ModuleType = ModuleType.Loans)]
        LEConverted = 9994,
        [Description("Portfolio transaction (Converted)")]
        [TransactionType("Portfolio transaction (Converted)", ModuleType = ModuleType.Portfolio)]
        PFConverted = 9995,
        [Description("Portfolio month end transaction (Converted)")]
        [TransactionType("Portfolio month end transaction (Converted)", ModuleType = ModuleType.Portfolio)]
        PEConverted = 9996,
        [Description("Line of Credit transaction (Converted)")]
        [TransactionType("Line of Credit transaction (Converted)", ModuleType = ModuleType.LineOfCredit)]
        LCConverted = 9997,

        // Accounts Payable: 10000 - 19999
        [Description("Accounts Payable transaction (Converted)")]
        [TransactionType("Accounts Payable transaction (Converted)", ModuleType = ModuleType.AccountsPayable)]
        APConverted = 10000,

        // Accounts Receivable: 20000 - 29999
        [Description("Accounts Receivable transaction (Converted)")]
        [TransactionType("Accounts Receivable transaction (Converted)", ModuleType = ModuleType.AccountsReceivable)]
        ARConverted = 20000,

        // Cash Disbursements: 30000 - 39999
        [Description("Check transaction (Converted)")]
        [TransactionType("Check transaction (Converted)", ModuleType = ModuleType.CashDisbursements)]
        CheckConverted = 30000,
        [Description("EFT transaction (Converted)")]
        [TransactionType("EFT transaction (Converted)", ModuleType = ModuleType.CashDisbursements)]
        EFTConverted = 30001,

        // Cash Receipts: 40000 - 49999
        [Description("Cash Receipt transaction (Converted)")]
        [TransactionType("Cash Receipt transaction (Converted)", ModuleType = ModuleType.CashReceipting)]
        CRConverted = 40000,

        // Fixed Assets: 50000 - 59999
        [Description("Fixed Assets transaction (Converted)")]
        [TransactionType("Fixed Assets transaction (Converted)", ModuleType = ModuleType.FixedAssets)]
        FAConvered = 50000,

        // Inventory: 60000 - 69999
        [Description("Inventory transaction (Converted)")]
        [TransactionType("Inventory transaction (Converted)", ModuleType = ModuleType.Inventory)]
        IVConverted = 60000,

        // Journals:  70000 - 79999
        [Description("Journal entry transaction (Converted)")]
        [TransactionType("Journal entry transaction (Converted)", ModuleType = ModuleType.GeneralLedger)]
        GLJVConverted = 70000,
        [Description("Prior period journal entry transaction (Converted)")]
        [TransactionType("Prior period journal entry transaction (Converted)", ModuleType = ModuleType.GeneralLedger)]
        GLJPConverted = 70001,
        [Description("Miscellaneous revenue transaction (Converted)")]
        [TransactionType("Miscellaneous revenue transaction (Converted)", ModuleType = ModuleType.GeneralLedger)]
        GLMRConverted = 70002,
        [Description("Journal")]
        Journal = 70010,
        [Description("Journal Reversal")]
        JournalReversal = 70013,
        [Description("Journal Cancel")]
        JournalCancel = 70012,
        [Description("Journal Prior Period")]
        JournalPriorPeriod = 70020,
        [Description("Journal Prior Period Cancel")]
        JournalPriorPeriodCancel = 70022,



        // Donations:  100000 - 109999
        [Description("Donation transaction (Converted)")]
        [TransactionType("Donation transaction (Converted)", ModuleType = ModuleType.Donations)]
        FRDGConverted = 100000,
        [Description("Event receipt transaction (Converted)")]
        [TransactionType("Event receipt transaction (Converted)", ModuleType = ModuleType.Donations)]
        FRERConverted = 100001,
        [Description("Miscellaneous revenue transaction (Converted)")]
        [TransactionType("Miscellaneous revenue transaction (Converted)", ModuleType = ModuleType.Donations)]
        FRMRConverted = 100002,
        [Description("Named fund transaction (Converted)")]
        [TransactionType("Named fund transaction (Converted)", ModuleType = ModuleType.NamedFunds)]
        FRNFConverted = 100003,

        // Campaigns/Appeals:  101000 - 101999

        // Planned Giving:  102000 - 102999

        // Named Funds:  103000 - 103999

        // Events/Programs:  104000 - 104999

        // Health Policy:  190000 - 199999
        [Description("Health care transaction (Converted)")]
        [TransactionType("Health care transaction (Converted)", ModuleType = ModuleType.HealthPolicy)]
        HPConverted = 190000,

        // CRM:  200000 - 209999

        // Special:  9999XX

        [Description("G/L closing balance")]
        [TransactionType("G/L closing balance", ModuleType = ModuleType.GeneralLedger)]
        ClosingBalance = 999998,
        [Description("G/L beginning balance")]
        [TransactionType("G/L beginning balance", ModuleType = ModuleType.GeneralLedger)]
        BeginningBalance = 999999,

    }
}
