using DDI.Business.ModuleInfo;
using DDI.Shared.Enums;
using System.Collections.Generic;

namespace DDI.Business
{

    public static class ModuleInfoCreator
    {

        #region Fields

        private static Dictionary<ModuleType, ModuleInformation> _moduleDictionary;
        private static List<ModuleInformation> _modules;

        #endregion

        #region Public Properties 

       
        public static ICollection<ModuleInformation> ModuleInfoCollection
        {
            get
            {
                Initialize();
                return _modules;
            }
        }
        #endregion

        #region Public Methods

        
        public static ModuleInformation GetModuleInfo(ModuleType moduleType)
        {
            Initialize();
            ModuleInformation moduleInfo = null;
            _moduleDictionary.TryGetValue(moduleType, out moduleInfo);
            return moduleInfo;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate list of modules only once, and only when first requested
        /// </summary>
        private static void Initialize()
        {
            if (_modules == null)
            {

                _moduleDictionary = new Dictionary<ModuleType, ModuleInformation>();
                _modules = new List<ModuleInformation>();
                // Populate the list of modules and the module dictionary.
                var accounting = new ModuleInformation
                {
                    Code = "ACCT",
                    Name = "Connect-Accounting",
                    IsRequired = true,
                    ModuleType = ModuleType.Accounting,
                    ParentModuleType = ModuleType.None,
                };
                _moduleDictionary.Add(ModuleType.Accounting, accounting);
                _modules.Add(accounting);

                var accountsPayable = new ModuleInformation
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

                var accountsReceivable = new ModuleInformation
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

                var campaigns = new ModuleInformation
                {
                    Code = "FRCM",
                    Name = "Campaigns and Appeals",
                    ModuleType = ModuleType.Campaigns,
                    ParentModuleType = ModuleType.FundRaising,

                };
                _moduleDictionary.Add(ModuleType.Campaigns, campaigns);
                _modules.Add(campaigns);

                var cashDisbursements = new ModuleInformation
                {
                    Code = "CD",
                    Name = "Cash Disbursements",
                    ModuleType = ModuleType.CashDisbursements,
                    ParentModuleType = ModuleType.CashProcessing
                };
                _moduleDictionary.Add(ModuleType.CashDisbursements, cashDisbursements);
                _modules.Add(cashDisbursements);

                var cashProcessing = new ModuleInformation
                {
                    Code = "CP",
                    Name = "Cash Processing",
                    ModuleType = ModuleType.CashProcessing,
                    ParentModuleType = ModuleType.None,

                };
                _moduleDictionary.Add(ModuleType.CashProcessing, cashProcessing);
                _modules.Add(cashProcessing);

                var cashReceipting = new ModuleInformation
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

                var crm = new ModuleInformation
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

                var cropEvents = new ModuleInformation
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

                var donations = new ModuleInformation
                {
                    Code = "FRDG",
                    Name = "Donations",
                    HasCustomFields = true,
                    ModuleType = ModuleType.Donations,
                    ParentModuleType = ModuleType.FundRaising,

                };
                _moduleDictionary.Add(ModuleType.Donations, donations);
                _modules.Add(donations);

                var extensionFund = new ModuleInformation
                {
                    Code = "CEF",
                    Name = "Connect-CEF",
                    ModuleType = ModuleType.ExtensionFund,
                    ParentModuleType = ModuleType.None,

                };
                _moduleDictionary.Add(ModuleType.ExtensionFund, extensionFund);
                _modules.Add(extensionFund);

                var fixedAssets = new ModuleInformation
                {
                    Code = "FA",
                    Name = "Fixed Assets",
                    ModuleType = ModuleType.FixedAssets,
                    ParentModuleType = ModuleType.Accounting

                };
                _moduleDictionary.Add(ModuleType.FixedAssets, fixedAssets);
                _modules.Add(fixedAssets);

                var fundRaising = new ModuleInformation
                {
                    Code = "FR",
                    Name = "Fund Raising",
                    ModuleType = ModuleType.FundRaising,
                    ParentModuleType = ModuleType.None,

                };
                _moduleDictionary.Add(ModuleType.FundRaising, fundRaising);
                _modules.Add(fundRaising);

                var generalLedger = new ModuleInformation
                {
                    Code = "GL",
                    Name = "General Ledger",
                    IsRequired = true,
                    HasCustomFields = true,
                    ModuleType = ModuleType.GeneralLedger,
                    ParentModuleType = ModuleType.Accounting
                };
                _moduleDictionary.Add(ModuleType.GeneralLedger, generalLedger);
                _modules.Add(generalLedger);

                var healthPolicy = new ModuleInformation
                {
                    Code = "HP",
                    Name = "Health Policy",
                    CanDisburse = true,
                    CheckStubInvoiceLabel = "Policy #",
                    ModuleType = ModuleType.HealthPolicy,
                    ParentModuleType = ModuleType.None
                };
                _moduleDictionary.Add(ModuleType.HealthPolicy, healthPolicy);
                _modules.Add(healthPolicy);

                var inventory = new ModuleInformation
                {
                    Code = "IV",
                    Name = "Inventory",
                    ModuleType = ModuleType.Inventory,
                    ParentModuleType = ModuleType.Inventory,

                };
                _moduleDictionary.Add(ModuleType.Inventory, inventory);
                _modules.Add(inventory);

                var investments = new ModuleInformation
                {
                    Code = "EFIN",
                    Name = "Investments",
                    CanDisburse = true,
                    CheckStubInvoiceLabel = "Invest #",

                };
                _moduleDictionary.Add(ModuleType.Investments, investments);
                _modules.Add(investments);

                var jobProcessing = new ModuleInformation
                {
                    Code = "JP",
                    Name = "Job Processing",
                    ModuleType = ModuleType.JobProcessing,
                    ParentModuleType = ModuleType.None,

                };
                _moduleDictionary.Add(ModuleType.JobProcessing, jobProcessing);
                _modules.Add(jobProcessing);

                var lineOfCredit = new ModuleInformation
                {
                    Code = "EFLC",
                    Name = "Line of Credit",
                    CanDisburse = true,
                    CheckStubInvoiceLabel = "L of C #",
                    ModuleType = ModuleType.LineOfCredit,
                    ParentModuleType = ModuleType.ExtensionFund,

                };
                _moduleDictionary.Add(ModuleType.LineOfCredit, lineOfCredit);
                _modules.Add(lineOfCredit);

                var loans = new ModuleInformation
                {
                    Code = "EFLN",
                    Name = "Loans",
                    CanDisburse = true,
                    CheckStubInvoiceLabel = "Loan #",
                    ModuleType = ModuleType.Loans,
                    ParentModuleType = ModuleType.ExtensionFund
                };
                _moduleDictionary.Add(ModuleType.Loans, loans);
                _modules.Add(loans);

                var namedFunds = new ModuleInformation
                {
                    Code = "FRNF",
                    Name = "Named Funds",
                    ModuleType = ModuleType.NamedFunds,
                    ParentModuleType = ModuleType.FundRaising,

                };
                _moduleDictionary.Add(ModuleType.NamedFunds, namedFunds);
                _modules.Add(namedFunds);

                var plannedGiving = new ModuleInformation
                {
                    Code = "FRPG",
                    Name = "Planned Giving",
                    ModuleType = ModuleType.PlannedGiving,
                    ParentModuleType = ModuleType.FundRaising,

                };
                _moduleDictionary.Add(ModuleType.PlannedGiving, plannedGiving);
                _modules.Add(plannedGiving);

                var pools = new ModuleInformation
                {
                    Code = "EFPO",
                    Name = "Pools",
                    ModuleType = ModuleType.Pools,
                    ParentModuleType = ModuleType.ExtensionFund
                };
                _moduleDictionary.Add(ModuleType.Pools, pools);
                _modules.Add(pools);

                var portfolio = new ModuleInformation
                {
                    Code = "EFPF",
                    Name = "Portfolio",
                    CanDisburse = true,
                    CheckStubInvoiceLabel = "Port. #",
                    ModuleType = ModuleType.Portfolio,
                    ParentModuleType = ModuleType.ExtensionFund
                };
                _moduleDictionary.Add(ModuleType.Portfolio, portfolio);
                _modules.Add(portfolio);

                var processManagement = new ModuleInformation
                {
                    Code = "PM",
                    Name = "Process Management",
                    ModuleType = ModuleType.ProcessManagement,
                    ParentModuleType = ModuleType.None,

                };
                _moduleDictionary.Add(ModuleType.ProcessManagement, processManagement);
                _modules.Add(processManagement);

                var projects = new ModuleInformation
                {
                    Code = "PR",
                    Name = "Project Management",
                    ModuleType = ModuleType.ProjectManagement,
                    ParentModuleType = ModuleType.None
                };
                _moduleDictionary.Add(ModuleType.ProjectManagement, projects);
                _modules.Add(projects);

                var systemAdministration = new ModuleInformation
                {
                    Code = "DDI",
                    Name = "System Administration",
                    IsRequired = true,
                    ModuleType = ModuleType.SystemAdministration,
                    ParentModuleType = ModuleType.None
                };
                _moduleDictionary.Add(ModuleType.SystemAdministration, systemAdministration);
                _modules.Add(systemAdministration);

                // Link up modules
                accounting.ChildModules.Add(accountsPayable);
                accounting.ChildModules.Add(accountsReceivable);
                accounting.ChildModules.Add(fixedAssets);
                accounting.ChildModules.Add(generalLedger);
                accounting.ChildModules.Add(inventory);

                cashProcessing.ChildModules.Add(cashDisbursements);
                cashProcessing.ChildModules.Add(cashReceipting);

                extensionFund.ChildModules.Add(investments);
                extensionFund.ChildModules.Add(lineOfCredit);
                extensionFund.ChildModules.Add(loans);
                extensionFund.ChildModules.Add(pools);
                extensionFund.ChildModules.Add(portfolio);

                fundRaising.ChildModules.Add(campaigns);
                fundRaising.ChildModules.Add(cropEvents);
                fundRaising.ChildModules.Add(donations);
                fundRaising.ChildModules.Add(namedFunds);
                fundRaising.ChildModules.Add(plannedGiving);

                accountsPayable.ParentModule = accounting;
                accountsReceivable.ParentModule = accounting;
                fixedAssets.ParentModule = accounting;
                generalLedger.ParentModule = accounting;
                inventory.ParentModule = accounting;

                cashDisbursements.ParentModule = cashProcessing;
                cashReceipting.ParentModule = cashProcessing;

                investments.ParentModule = extensionFund;
                lineOfCredit.ParentModule = extensionFund;
                loans.ParentModule = extensionFund;
                pools.ParentModule = extensionFund;
                portfolio.ParentModule = extensionFund;

                campaigns.ParentModule = fundRaising;
                cropEvents.ParentModule = fundRaising;
                donations.ParentModule = fundRaising;
                namedFunds.ParentModule = fundRaising;
                plannedGiving.ParentModule = fundRaising;
            }
        }
    }

    #endregion
}
