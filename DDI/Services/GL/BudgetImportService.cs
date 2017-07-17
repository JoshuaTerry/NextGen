using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.GL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace DDI.Services.GL
{
    public class BudgetImportService
    {
        private IUnitOfWork _uow = null;
        public BudgetImportService()
        {
            _uow = new UnitOfWorkEF();
        }

        public BudgetImportService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public IDataResponse ImportBudgets(Guid fileId, Guid fiscalYearId, MappableEntityField[] fields)
        {
            var response = new DataResponse();
            var importFile = GetImportFile(fileId);
            if (!fields.Any(f => f.PropertyName == "Account"))
                throw new Exception("No Account Mapping Specified.");

            char[] delimiters = { ',' };

            string[] columns = null;

            using (var stream = new MemoryStream(importFile.File.Data))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    if (importFile.ContainsHeaders)
                        columns = streamReader.ReadLine().Split(delimiters, StringSplitOptions.None);

                    var mappings = fields.Select(f => new { PropertyName = f.PropertyName, ColumnName = f.ColumnName, Ordinal = columns != null ? Array.IndexOf(columns, f.ColumnName) : Convert.ToInt32(f.ColumnName) }).ToList();

                    string line = string.Empty;
                    while ((line = streamReader.ReadLine()) != string.Empty)
                    {
                        string[] values = line.Split(delimiters, StringSplitOptions.None);

                        var propertyValues = mappings.Select(m => new { Property = m.PropertyName, Value = values[m.Ordinal] }).ToList();
                        var accountNumber = propertyValues.FirstOrDefault(p => p.Property == "Account");
                        var budgetName = propertyValues.FirstOrDefault(p => p.Property == "Budget");
                        var budget = GetAccountBudget(accountNumber.Value, fiscalYearId, budgetName.Value);

                        if (budget != null)
                        {
                            var remainingValues = propertyValues.Where(pv => pv.Property != "Account" && pv.Property != "Budget").ToList();
                            foreach (var item in propertyValues)
                            {
                                var budgetType = typeof(AccountBudget);
                                var property = budgetType.GetProperty(item.Property);
                                property.SetValue(budget, Convert.ChangeType(item.Value, property.PropertyType));
                            }
                            _uow.Update(budget);
                            _uow.SaveChanges();
                        }
                    }
                }
            }
            return response;
        }

        private AccountBudget GetAccountBudget(string accountNumber, Guid fiscalYearId, string budgetName)
        {
            AccountBudget budget = null;
            var account = GetAccount(accountNumber, fiscalYearId);

            (string fix, string working, string whatif) = GetBudgetNames(account);
            var budgets = GetAccountBudgets(account);

            if ((new string[] { fix, working, whatif }).Contains(budgetName))
            {
                int budgetType = -1;

                if (fix == budgetName)
                    budgetType = 0;
                else if (working == budgetName)
                    budgetType = 1;
                else if (whatif == budgetName)
                    budgetType = 2;

                if (budgetType != -1)
                {
                    budget = budgets.FirstOrDefault(b => (int)b.BudgetType == budgetType);
                }
            }
            return budget;
        }
        private (string, string, string) GetBudgetNames(Account account)
        {
            string fixedBudget = string.Empty;
            string workingBudget = string.Empty;
            string whatIfBudget = string.Empty;

            var ledgerAccount = _uow.GetEntities<LedgerAccount>().FirstOrDefault(la => la.AccountNumber == account.AccountNumber);
            if (ledgerAccount != null)
            {
                var ledger = _uow.GetEntities<Ledger>().FirstOrDefault(l => l.Id == ledgerAccount.LedgerId);
                if (ledger != null)
                {
                    fixedBudget = ledger.FixedBudgetName;
                    workingBudget = ledger.WorkingBudgetName;
                    whatIfBudget = ledger.WhatIfBudgetName;
                }
            }
            return (fixedBudget, workingBudget, whatIfBudget);
        }
        private List<AccountBudget> GetAccountBudgets(Account account)
        {
            return _uow.GetEntities<AccountBudget>().Where(ab => ab.AccountId == account.Id).ToList();

        }
        private Account GetAccount(string accountNumber, Guid fiscalYearId)
        {
            var includes = new Expression<Func<Account, object>>[] { a => a.Budgets };
            return _uow.GetEntities<Account>(includes).FirstOrDefault(a => a.AccountNumber == accountNumber && a.FiscalYearId == fiscalYearId);
        }
        private ImportFile GetImportFile(Guid id)
        {
            var includes = new Expression<Func<ImportFile, object>>[] { f => f.File, f => f.File.Data };
            var importFile = _uow.GetById<ImportFile>(id, includes);

            return importFile;
        }
    }
}

