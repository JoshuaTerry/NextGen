using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.GL;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public IDataResponse<object> ImportBudgets(Guid fileId, Guid fiscalYearId, MappableEntityField[] fields)
        {
            var response = new DataResponse<object>();
            var successfulImports = new List<string>();
            var failedImports = new List<string>();

            var importFile = GetImportFile(fileId);

            if (!fields.Any(f => f.PropertyName == "AccountNumber"))
                throw new Exception(messages.NoAccountMapping);

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

                    while (!string.IsNullOrEmpty((line = streamReader.ReadLine())))
                    {
                        string[] values = line.Split(delimiters, StringSplitOptions.None);

                        try
                        {
                            var propertyValues = mappings.Select(m => new { Property = m.PropertyName, Value = values[m.Ordinal] }).ToList();
                            var accountNumber = propertyValues.FirstOrDefault(p => p.Property == "AccountNumber");
                            var budgetName = propertyValues.FirstOrDefault(p => p.Property == "Budget");
                            var budget = GetAccountBudget(accountNumber.Value, fiscalYearId, budgetName.Value);

                            if (budget != null)
                            {
                                var remainingValues = propertyValues.Where(pv => pv.Property != "AccountNumber" && pv.Property != "Budget").ToList();
                                var amounts = budget.Budget;

                                foreach (var item in propertyValues)
                                {
                                    var budgetType = typeof(PeriodAmountList);
                                    var property = budgetType.GetProperty(item.Property);
                                    if (property != null)
                                    {
                                        property.SetValue(amounts, Convert.ChangeType(item.Value, property.PropertyType, CultureInfo.InvariantCulture));
                                    }
                                }
                                _uow.Update(budget);
                                _uow.SaveChanges();
                                successfulImports.Add(line);
                            }
                        }
                        catch (Exception ex)
                        {
                            response.ErrorMessages.Add(ex.Message);
                            failedImports.Add(line);
                        }
                    }
                }
            }

            response.Data = new { Imported = successfulImports, Failed = failedImports };

            return response;
        }

        internal AccountBudget GetAccountBudget(string accountNumber, Guid fiscalYearId, string budgetName)
        {
            AccountBudget budget = null;
            var account = GetAccount(accountNumber, fiscalYearId);
            if (account == null)
                throw new NullReferenceException(messages.InvalidAccountNumber + accountNumber);

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

                budget = budgets.FirstOrDefault(b => (int)b.BudgetType == budgetType);
            }
            else
            {
                throw new NullReferenceException($"The Budget: {budgetName} could not be found.");
            }

            return budget;
        }
        internal (string, string, string) GetBudgetNames(Account account)
        {
            if (account == null)
                throw new Exception(messages.InvalidAccountBudget);

            var ledgerAccount = _uow.GetEntities<LedgerAccount>().FirstOrDefault(la => la.AccountNumber == account.AccountNumber);
            if (ledgerAccount == null)
                throw new NullReferenceException($"No LedgerAccount was found for AccountNumber: {account.AccountNumber}.");

            var ledger = _uow.GetEntities<Ledger>().FirstOrDefault(l => l.Id == ledgerAccount.LedgerId);
            if (ledger == null)
                throw new NullReferenceException($"No Ledger was found for AccountNumber: {account.AccountNumber}.");

            return (ledger.FixedBudgetName, ledger.WorkingBudgetName, ledger.WhatIfBudgetName);
        }
        internal List<AccountBudget> GetAccountBudgets(Account account)
        {
            var includes = new Expression<Func<AccountBudget, object>>[] { a => a.Budget };
            return _uow.GetEntities<AccountBudget>().Where(ab => ab.AccountId == account.Id).ToList();
        }
        internal Account GetAccount(string accountNumber, Guid fiscalYearId)
        {
            var includes = new Expression<Func<Account, object>>[] { a => a.Budgets };
            return _uow.GetEntities<Account>(includes).FirstOrDefault(a => a.AccountNumber == accountNumber && a.FiscalYearId == fiscalYearId);
        }
        internal ImportFile GetImportFile(Guid id)
        {
            var includes = new Expression<Func<ImportFile, object>>[] { f => f.File, f => f.File.Data };
            var importFile = _uow.GetById<ImportFile>(id, includes);

            return importFile;
        }
    }
}

