using System.ComponentModel;

namespace DDI.Shared.Enums.Common
{
    public enum CustomFieldEntity
    {
        [Description("Accounting")]
        Accounting = 0,

        [Description("General Ledger")]
        GeneralLedger = 1,

        [Description("Accounts Payable")]
        AccountsPayable = 2,

        [Description("Accounts Receivable")]
        AccountsReceivable = 3,

        [Description("Fixed Assets")]
        FixedAssets = 4,

        [Description("Inventory")]
        Inventory = 5,

        [Description("Cash Processing")]
        CashProcessing = 6,

        [Description("Cash Disbursements")]
        CashDisbursements = 7,

        [Description("Cash Receipting")]
        CashReceipting = 8,

        [Description("Gifts")]
        Gifts = 9,

        [Description("Named Funds")]
        NamedFunds = 10,

        [Description("Crop Events")]
        CropEvents = 11,

        [Description("Planned Giving")]
        PlannedGiving = 12,

        [Description("Campaigns")]
        Campaigns = 13,

        [Description("Investments")]
        Investments = 14,

        [Description("Line Of Credit")]
        LineOfCredit = 15,

        [Description("Loans")]
        Loans = 16,

        [Description("Portfolio")]
        Portfolio = 17,

        [Description("Pools")]
        Pools = 18,

        [Description("CRM")]
        CRM = 19,

        [Description("Office Integration")]
        OfficeIntegration = 20,

        [Description("Process Management")]
        ProcessManagement = 21,

        [Description("Project Management")]
        ProjectManagement = 22,

        [Description("Job Processing")]
        JobProcessing = 23,

        [Description("Health Policy")]
        HealthPolicy = 24,

        [Description("System Administration")]
        SystemAdministration = 25
    }
}
