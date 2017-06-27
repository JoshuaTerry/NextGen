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
        [TransactionType("Investment transaction (Converted)", ModuleType = ModuleType.Investments)]
        INConverted = 9991,
        [TransactionType("Investment month end transaction (Converted)", ModuleType = ModuleType.Investments)]
        IEConverted = 9992,
        [TransactionType("Loan transaction (Converted)", ModuleType = ModuleType.Loans)]
        LNConverted = 9993,
        [TransactionType("Loan month end transaction (Converted)", ModuleType = ModuleType.Loans)]
        LEConverted = 9994,
        [TransactionType("Portfolio transaction (Converted)", ModuleType = ModuleType.Portfolio)]
        PFConverted = 9995,
        [TransactionType("Portfolio month end transaction (Converted)", ModuleType = ModuleType.Portfolio)]
        PEConverted = 9996,
        [TransactionType("Line of Credit transaction (Converted)", ModuleType = ModuleType.LineOfCredit)]
        LCConverted = 9997,

        // Accounts Payable: 10000 - 19999
        [TransactionType("Accounts Payable transaction (Converted)", ModuleType = ModuleType.AccountsPayable)]
        APConverted = 10000,

        // Accounts Receivable: 20000 - 29999
        [TransactionType("Accounts Receivable transaction (Converted)", ModuleType = ModuleType.AccountsReceivable)]
        ARConverted = 20000,

        // Cash Disbursements: 30000 - 39999
        [TransactionType("Check transaction (Converted)", ModuleType = ModuleType.CashDisbursements)]
        CheckConverted = 30000,
        [TransactionType("EFT transaction (Converted)", ModuleType = ModuleType.CashDisbursements)]
        EFTConverted = 30001,

        // Cash Receipts: 40000 - 49999
        [TransactionType("Cash Receipt transaction (Converted)", ModuleType = ModuleType.CashReceipting)]
        CRConverted = 40000,

        // Fixed Assets: 50000 - 59999
        [TransactionType("Fixed Assets transaction (Converted)", ModuleType = ModuleType.FixedAssets)]
        FAConvered = 50000,

        // Inventory: 60000 - 69999
        [TransactionType("Inventory transaction (Converted)", ModuleType = ModuleType.Inventory)]
        IVConverted = 60000,

        // Journals:  70000 - 79999
        [TransactionType("Journal entry transaction (Converted)", ModuleType = ModuleType.GeneralLedger)]
        GLJVConverted = 70000,
        [TransactionType("Prior period journal entry transaction (Converted)", ModuleType = ModuleType.GeneralLedger)]
        GLJPConverted = 70001,
        [TransactionType("Miscellaneous revenue transaction (Converted)", ModuleType = ModuleType.GeneralLedger)]
        GLMRConverted = 70002,
        Journal = 70010,
        JournalReversal = 70013,
        JournalCancel = 70012,
        JournalPriorPeriod = 70020,
        JournalPriorPeriodCancel = 70022,

        

        // Donations:  100000 - 109999
        [TransactionType("Donation transaction (Converted)", ModuleType = ModuleType.Donations)]
        FRDGConverted = 100000,
        [TransactionType("Event receipt transaction (Converted)", ModuleType = ModuleType.Donations)]
        FRERConverted = 100001,
        [TransactionType("Miscellaneous revenue transaction (Converted)", ModuleType = ModuleType.Donations)]
        FRMRConverted = 100002,
        [TransactionType("Named fund transaction (Converted)", ModuleType = ModuleType.NamedFunds)]
        FRNFConverted = 100003,

        // Campaigns/Appeals:  101000 - 101999

        // Planned Giving:  102000 - 102999

        // Named Funds:  103000 - 103999

        // Events/Programs:  104000 - 104999

        // Health Policy:  190000 - 199999
        [TransactionType("Health care transaction (Converted)", ModuleType = ModuleType.HealthPolicy)]
        HPConverted = 190000,

        // CRM:  200000 - 209999



    }
}
