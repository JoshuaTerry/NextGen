using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Business.CRM.ModuleInfo.Base;

namespace DDI.Business.CRM
{
    public static class ModuleInfoCreator
    {

        private static Dictionary<ModuleType, ModuleInfoConcrete> _moduleDictionary;
        private static List<ModuleInfoConcrete> _modules;


        public static ModuleInfoConcrete GetModuleInfo(ModuleType moduleType)
        {
            Initialize();

        }

        private static void Initialize()
        {
            _moduleDictionary = new Dictionary<ModuleType, ModuleInfoConcrete>();
            _modules = new List<ModuleInfoConcrete>();

            var accounting = new ModuleInfoConcrete
            {
                Code = "ACCT",
                Name = "Connect-Accounting",
                IsRequired = true,
                ModuleType = ModuleType.Accounting,
                ParentModuleType = ModuleType.None
            };
            _moduleDictionary.Add(ModuleType.Accounting, accounting);
            _modules.Add(accounting);

            var accountsPayable = new ModuleInfoConcrete
            {
                Code = "AP",
                Name = "Accounts Payable",
                HasCustomFields = true,
                CanDisburse = true,
                CheckStubInvoiceLabel = "Invoice #",
                ModuleType = ModuleType.AccountsPayable,
                ParentModuleType = ModuleType.Accounting
            };
            _moduleDictionary.Add(ModuleType.AccountsPayable, accountsPayable);
            _modules.Add(accountsPayable);

            var accountsReceivable = new ModuleInfoConcrete
            {
                Code = "AR",
                Name = "Accounts Receivable",
                HasCustomFields = true,
                CanDisburse = true,
                CheckStubInvoiceLabel = "Refund #",
                ModuleType = ModuleType.AccountsReceivable,
                ParentModuleType = ModuleType.Accounting
            };
            _moduleDictionary.Add(ModuleType.AccountsReceivable, accountsReceivable);
            _modules.Add(accountsReceivable);

            var campaigns = new ModuleInfoConcrete {
                Code = "FRCM",
                Name = "Campaigns and Appeals",
                ModuleType = ModuleType.Campaigns,
                ParentModuleType = ModuleType.FundRaising,

            };
            _moduleDictionary.Add(ModuleType.Campaigns, campaigns);
            _modules.Add(campaigns);

            var cashDisbursements = new ModuleInfoConcrete
            {
                Code = "CD",
                Name = "Cash Disbursements",
                ModuleType = ModuleType.CashDisbursements,
                ParentModuleType = ModuleType.CashProcessing
            };
            _moduleDictionary.Add(ModuleType.CashDisbursements, cashDisbursements);
            _modules.Add(cashDisbursements);

            var cashProcessing = new ModuleInfoConcrete
            {
                Code = "CP",
                Name = "Cash Processing",
                ModuleType = ModuleType.CashProcessing,
                ParentModuleType = ModuleType.None,

            };
            _moduleDictionary.Add(ModuleType.CashProcessing, cashProcessing);
            _modules.Add(cashProcessing);

            var cashReceipting = new ModuleInfoConcrete
            {
                Code = "CR",
                Name = "Cash Receipting",
                CanDisburse = true,
                CheckStubInvoiceLabel = "Credit #",
                HasCustomFields = true,
                ModuleType = ModuleType.CashReceipting,
                ParentModuleType = ModuleType.CashProcessing
            };
            _moduleDictionary.Add(ModuleType.CashReceipting, cashReceipting);
            _modules.Add(cashReceipting);

            var crm = new ModuleInfoConcrete
            {
                Code = "CRM",
                Name = "Connect-CRM",
                IsRequired = true,
                HasCustomFields = true,
                ModuleType = ModuleType.CRM,
                ParentModuleType = ModuleType.None
            };
            _moduleDictionary.Add(ModuleType.CRM, crm);
            _modules.Add(crm);

            var cropEvents = new ModuleInfoConcrete
            {
                Code = "FREV",
                Name = "CROP Events and Programs",
                CanDisburse = true,
                CheckStubInvoiceLabel = "Event #",
                ModuleType = ModuleType.CropEvents,
                ParentModuleType = ModuleType.FundRaising
            };
            _moduleDictionary.Add(ModuleType.CropEvents, cropEvents);
            _modules.Add(cropEvents);

            var donations = new ModuleInfoConcrete
            {
                Code = "FRDG",
                Name = "Donations",
                HasCustomFields = true,
                ModuleType = ModuleType.Gifts,
                ParentModuleType = ModuleType.FundRaising,

            };
            _moduleDictionary.Add(ModuleType.Gifts, donations);

            var extensionFund = new ModuleInfoConcrete
            {
                Code = "CEF",
                Name = "Connect-CEF",
                ModuleType = ModuleType.ExtensionFund,
                ParentModuleType = ModuleType.None,

            };
            _moduleDictionary.Add(ModuleType.ExtensionFund, extensionFund);

            var fixedAssets = new ModuleInfoConcrete
            {
                Code = "FA",
                Name = "Fixed Assets",
                ModuleType = ModuleType.FixedAssets,
                ParentModuleType = ModuleType.Accounting

            };
            _moduleDictionary.Add(ModuleType.FixedAssets, fixedAssets);

            var fundRaising = new ModuleInfoConcrete
            {
                Code = "FR",
                Name = "Fund Raising",
                ModuleType = ModuleType.FundRaising,
                ParentModuleType = ModuleType.None,

            };
            _moduleDictionary.Add(ModuleType.FundRaising, fundRaising);

            var generalLedger = new ModuleInfoConcrete
            {
                Code = "GL",
                Name = "General Ledger",
                IsRequired = true,
                HasCustomFields = true,
                ModuleType = ModuleType.GeneralLedger,
                ParentModuleType = ModuleType.Accounting
            };
            _moduleDictionary.Add(ModuleType.GeneralLedger, generalLedger);

            var healthPolicy = new ModuleInfoConcrete
            {
                Code = "HP",
                Name = "Health Policy",
                CanDisburse = true,
                CheckStubInvoiceLabel = "Policy #",
                ModuleType = ModuleType.HealthPolicy,
                ParentModuleType = ModuleType.None
            };
            _moduleDictionary.Add(ModuleType.HealthPolicy, healthPolicy);

            var inventory = new ModuleInfoConcrete
            {
                Code = "IV",
                Name = "Inventory",
                ModuleType = ModuleType.Inventory,
                ParentModuleType = ModuleType.Inventory,

            };
            _moduleDictionary.Add(ModuleType.Inventory, inventory);

            var investments = new ModuleInfoConcrete
            {
                Code = "EFIN",
                Name = "Investments",
                CanDisburse = true,
                CheckStubInvoiceLabel = "Invest #",

            };
            _moduleDictionary.Add(ModuleType.Investments, investments);

            var jobProcessing = new ModuleInfoConcrete
            {
                Code = "JP",
                Name = "Job Processing",
                ModuleType = ModuleType.JobProcessing,
                ParentModuleType = ModuleType.None,

            };
            _moduleDictionary.Add(ModuleType.JobProcessing, jobProcessing);

            var lineOfCredit = new ModuleInfoConcrete
            {
                Code = "EFLC",
                Name = "Line of Credit",
                CanDisburse = true,
                CheckStubInvoiceLabel = "L of C #",
                ModuleType = ModuleType.LineOfCredit,
                ParentModuleType = ModuleType.ExtensionFund,

            };
            _moduleDictionary.Add(ModuleType.LineOfCredit, lineOfCredit);

            var loans = new ModuleInfoConcrete
            {
                Code = "EFLN",
                Name = "Loans",
                CanDisburse = true,
                CheckStubInvoiceLabel = "Loan #",
                ModuleType = ModuleType.Loans,
                ParentModuleType = ModuleType.ExtensionFund
            };
            _moduleDictionary.Add(ModuleType.Loans, loans);

            var namedFunds = new ModuleInfoConcrete
            {
                Code = "FRNF",
                Name = "Named Funds",
                ModuleType = ModuleType.NamedFunds,
                ParentModuleType = ModuleType.FundRaising,

            };
            _moduleDictionary.Add(ModuleType.NamedFunds, namedFunds);

            var plannedGiving = new ModuleInfoConcrete
            {
                Code = "FRPG",
                Name = "Planned Giving",
                ModuleType = ModuleType.PlannedGiving,
                ParentModuleType = ModuleType.FundRaising,

            };
            _moduleDictionary.Add(ModuleType.PlannedGiving, plannedGiving);

            var pools = new ModuleInfoConcrete
            {
                Code = "EFPO",
                Name = "Pools",
                ModuleType = ModuleType.Pools,
                ParentModuleType = ModuleType.ExtensionFund
            };
            _moduleDictionary.Add(ModuleType.Pools, pools);

            var portfolio = new ModuleInfoConcrete
            {
                Code = "EFPF",
                Name = "Portfolio",
                CanDisburse = true,
                CheckStubInvoiceLabel = "Port. #",
                ModuleType = ModuleType.Portfolio,
                ParentModuleType = ModuleType.ExtensionFund
            };
            _moduleDictionary.Add(ModuleType.Portfolio, portfolio);

            var processManagement = new ModuleInfoConcrete
            {
                Code = "PM",
                Name = "Process Management",
                ModuleType = ModuleType.ProcessManagement,
                ParentModuleType = ModuleType.None,

            };
            _moduleDictionary.Add(ModuleType.ProcessManagement, processManagement);

            var projects = new ModuleInfoConcrete
            {
                Code = "PR",
                Name = "Project Management",
                ModuleType = ModuleType.ProjectManagement,
                ParentModuleType = ModuleType.None
            };
            _moduleDictionary.Add(ModuleType.ProjectManagement, projects);

            var systemAdministration = new ModuleInfoConcrete
            {
                Code = "DDI",
                Name = "System Administration",
                IsRequired = true,
                ModuleType = ModuleType.SystemAdministration,
                ParentModuleType = ModuleType.None
            };
            _moduleDictionary.Add(ModuleType.SystemAdministration, systemAdministration);

        }
    }
}
