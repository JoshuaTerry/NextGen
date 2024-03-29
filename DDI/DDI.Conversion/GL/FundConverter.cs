﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Conversion.Statics;
using DDI.Data;
using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Client.Security;

namespace DDI.Conversion.GL
{    
    internal class FundConverter : GLConversionBase
    {
        private Dictionary<string, Guid> _fundIds;

        public enum ConversionMethod
        {
            Funds = 70200,
            FundFromTos = 70201,
            BusinessUnitFromTos = 70202
        }

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            Initialize(baseDirectory);
            
            RunConversion(ConversionMethod.Funds, () => ConvertFunds(InputFile.GL_Funds));
            RunConversion(ConversionMethod.FundFromTos, () => ConvertFundFromTos(InputFile.GL_FundFromTos));
            RunConversion(ConversionMethod.BusinessUnitFromTos, () => ConvertBusinessUnitFromTos(InputFile.GL_BusinessUnitFromTos));
        }


        private void ConvertFunds(string filename)
        {
            DomainContext context = new DomainContext();

            LoadFiscalYearIds();
            LoadLedgerAccountIds();
            LoadSegmentIds();

            var outputFile = new FileExport<Fund>(Path.Combine(OutputDirectory, OutputFile.GL_FundFile), false);
            var legacyIdFile = new FileExport<LegacyToID>(Path.Combine(OutputDirectory, OutputFile.GL_FundIdMappingFile), false, true);

            outputFile.AddHeaderRow();

            using (var importer = CreateFileImporter(GLDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
                {
                    Guid? fiscalYearId = GetFiscalYearId(importer, 0);

                    if (fiscalYearId == null)
                    {
                        continue;
                    }

                    string fundLegacyKey = importer.GetString(2);
                    string fundSegmentKey = importer.GetString(4);

                    Guid fundSegmentId;
                    if (!SegmentIds.TryGetValue(fundSegmentKey, out fundSegmentId))
                    {
                        importer.LogError($"Invalid fund segment legacy key \"{fundSegmentKey}\".");
                        continue;
                    }

                    Fund fund = new Fund();
                    fund.FiscalYearId = fiscalYearId;
                    fund.FundSegmentId = fundSegmentId;

                    int fundBalKey = importer.GetInt(5);
                    int closeRevKey = importer.GetInt(6);
                    int closeExpKey = importer.GetInt(7);

                    if (fundBalKey > 0)
                    {
                        Guid accountId;
                        if (!LedgerAccountIds.TryGetValue(fundBalKey, out accountId))
                        {
                            importer.LogError($"Invalid fund balance ledger account legacy key \"{fundBalKey}\".");
                            continue;
                        }
                        fund.FundBalanceAccountId = accountId;
                    }

                    if (closeRevKey > 0)
                    {
                        Guid accountId;
                        if (!LedgerAccountIds.TryGetValue(closeRevKey, out accountId))
                        {
                            importer.LogError($"Invalid closing revenue ledger account legacy key \"{closeRevKey}\".");
                            continue;
                        }
                        fund.ClosingRevenueAccountId = accountId;
                    }

                    if (closeExpKey > 0)
                    {
                        Guid accountId;
                        if (!LedgerAccountIds.TryGetValue(closeExpKey, out accountId))
                        {
                            importer.LogError($"Invalid closing expense ledger account legacy key \"{closeExpKey}\".");
                            continue;
                        }
                        fund.ClosingExpenseAccountId = accountId;
                    }
                                        
                    fund.AssignPrimaryKey();

                    outputFile.AddRow(fund);
                    legacyIdFile.AddRow(new LegacyToID(fundLegacyKey, fund.Id));

                    count++;
                }
            }

            legacyIdFile.Dispose();
            outputFile.Dispose();
        }

        private void ConvertFundFromTos(string filename)
        {
            DomainContext context = new DomainContext();

            LoadFiscalYearIds();
            LoadLedgerAccountIds();
            LoadFundIds();

            var outputFile = new FileExport<FundFromTo>(Path.Combine(OutputDirectory, OutputFile.GL_FundFromToFile), false);

            outputFile.AddHeaderRow();

            using (var importer = CreateFileImporter(GLDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
                {
                    Guid? fiscalYearId = GetFiscalYearId(importer, 0);

                    if (fiscalYearId == null)
                    {
                        continue;
                    }

                    string fundLegacyKey = importer.GetString(3);
                    Guid fundId;
                    if (!_fundIds.TryGetValue(fundLegacyKey, out fundId))
                    {
                        importer.LogError($"Invalid fund legacy key \"{fundLegacyKey}\".");
                        continue;
                    }

                    FundFromTo fromTo = new FundFromTo();
                    fromTo.FiscalYearId = fiscalYearId;
                    fromTo.FundId = fundId;

                    string offsettingFundLegacyKey = importer.GetString(5);
                    if (!string.IsNullOrWhiteSpace(offsettingFundLegacyKey))
                    {
                        Guid offsettingFundId;
                        if (!_fundIds.TryGetValue(offsettingFundLegacyKey, out offsettingFundId))
                        {
                            importer.LogError($"Invalid offsetting fund legacy key \"{offsettingFundLegacyKey}\".");
                            continue;
                        }
                        fromTo.OffsettingFundId = offsettingFundId;
                    }

                    int fromAccountKey = importer.GetInt(6);
                    if (fromAccountKey > 0)
                    {
                        Guid accountId;
                        if (!LedgerAccountIds.TryGetValue(fromAccountKey, out accountId))
                        {
                            importer.LogError($"Invalid 'due from' ledger account legacy key \"{fromAccountKey}\".");
                            continue;
                        }
                        fromTo.FromAccountId = accountId;
                    }

                    int toAccountKey = importer.GetInt(7);
                    if (toAccountKey > 0)
                    {
                        Guid accountId;
                        if (!LedgerAccountIds.TryGetValue(toAccountKey, out accountId))
                        {
                            importer.LogError($"Invalid 'due to' ledger account legacy key \"{toAccountKey}\".");
                            continue;
                        }
                        fromTo.ToAccountId = accountId;
                    }

                    fromTo.AssignPrimaryKey();

                    outputFile.AddRow(fromTo);

                    count++;
                }
            }

            outputFile.Dispose();
        }

        private void ConvertBusinessUnitFromTos(string filename)
        {
            DomainContext context = new DomainContext();

            LoadLedgerIds();
            LoadFiscalYearIds();
            LoadLedgerAccountIds();

            var ledgers = LoadEntities(context.GL_Ledgers, nameof(Ledger.BusinessUnit));

            var outputFile = new FileExport<BusinessUnitFromTo>(Path.Combine(OutputDirectory, OutputFile.GL_BusinessUnitFromToFile), false);

            outputFile.AddHeaderRow();

            using (var importer = CreateFileImporter(GLDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
                {
                    Guid? fiscalYearId = GetFiscalYearId(importer, 0);

                    if (fiscalYearId == null)
                    {
                        continue;
                    }

                    Guid? businessUnitId = GetBusinessUnitId(importer, 0, ledgers);
                    if (businessUnitId == null)
                    { 
                        importer.LogError($"Invalid or null business unit for BusinessUnitFromTo.");
                        continue;
                    }

                    BusinessUnitFromTo fromTo = new BusinessUnitFromTo();
                    fromTo.FiscalYearId = fiscalYearId;
                    fromTo.BusinessUnitId = businessUnitId;
                    fromTo.OffsettingBusinessUnitId = GetBusinessUnitId(importer, 2, ledgers);

                    int fromAccountKey = importer.GetInt(3);
                    if (fromAccountKey > 0)
                    {
                        Guid accountId;
                        if (!LedgerAccountIds.TryGetValue(fromAccountKey, out accountId))
                        {
                            importer.LogError($"Invalid 'due from' ledger account legacy key \"{fromAccountKey}\".");
                            continue;
                        }
                        fromTo.FromAccountId = accountId;
                    }

                    int toAccountKey = importer.GetInt(4);
                    if (toAccountKey > 0)
                    {
                        Guid accountId;
                        if (!LedgerAccountIds.TryGetValue(toAccountKey, out accountId))
                        {
                            importer.LogError($"Invalid 'due to' ledger account legacy key \"{toAccountKey}\".");
                            continue;
                        }
                        fromTo.ToAccountId = accountId;
                    }

                    fromTo.AssignPrimaryKey();

                    outputFile.AddRow(fromTo);

                    count++;
                }
            }

            outputFile.Dispose();
        }

        /// <summary>
        /// Import a legacy company Id and lookup the corresponding BusinessUnit Id.
        /// </summary>
        private Guid? GetBusinessUnitId(FileImport importer, int column, IList<Ledger> ledgers)
        {
            Guid? businessUnitId = null;
            int cid = importer.GetInt(column);
            if (cid > 0)
            {
                Guid ledgerId;
                if (!LedgerIds.TryGetValue(cid, out ledgerId))
                {
                    importer.LogError($"Invalid company Id {cid}.");
                    return null;
                }

                businessUnitId = ledgers.FirstOrDefault(p => p.Id == ledgerId)?.BusinessUnit?.Id;
            }

            return businessUnitId;
        }

        /// <summary>
        /// Load legacy fiscal year IDs into a dictionary.
        /// </summary>
        private void LoadFundIds()
        {
            if (_fundIds == null)
            {
                _fundIds = LoadLegacyIds(OutputDirectory, OutputFile.GL_FundIdMappingFile);
            }
        }

    }
}
