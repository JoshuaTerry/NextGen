using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using DDI.Business.Core;
using DDI.Business.CP;
using DDI.Conversion.GL;
using DDI.Conversion.Statics;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Enums.CP;
using DDI.Shared.Models.Client.CP;

namespace DDI.Conversion.CP
{
    internal class SettingsLoader : GLConversionBase
    {
        public enum ConversionMethod
        {
            Configuration = 40001,
            EFTFormats,
            BankAccounts,
            ReceiptTypes,
            ReceiptBatchTypes
        }

        private string _cpDirectory;
        private string _cpOutputDirectory;

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            Initialize(baseDirectory);
            MethodsToRun = conversionMethods;
            _cpDirectory = Path.Combine(baseDirectory, DirectoryName.CP);
            _cpOutputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.CP);

            RunConversion(ConversionMethod.Configuration, () => LoadConfiguration(InputFile.CP_Settings));
            RunConversion(ConversionMethod.EFTFormats, () => LoadEFTFormats(InputFile.CP_EFTFormat));
            RunConversion(ConversionMethod.BankAccounts, () => LoadBankAccounts(InputFile.CP_BankAccounts));
            RunConversion(ConversionMethod.ReceiptTypes, () => LoadReceiptTypes(InputFile.CP_ReceiptTypes));
            RunConversion(ConversionMethod.ReceiptBatchTypes, () => LoadReceiptBatchTypes(InputFile.CP_ReceiptBatchTypes));

        }

        private void LoadEFTFormats(string filename)
        {
            using (var context = new DomainContext())
            {
                using (var importer = CreateFileImporter(_cpDirectory, filename, typeof(ConversionMethod)))
                {
                    while (importer.GetNextRow())
                    {
                        string code = importer.GetString(0);
                        string description = importer.GetString(1);
                        bool isActive = importer.GetBool(2);

                        if (string.IsNullOrWhiteSpace(code))
                        {
                            continue;
                        }

                        context.CP_EFTFormats.AddOrUpdate(
                            prop => prop.Code,
                            new EFTFormat { Code = code, Name = description, IsActive = isActive });
                    }
                }
                context.SaveChanges();
            }
        }

        private void LoadReceiptTypes(string filename)
        {
            using (var context = new DomainContext())
            {
                using (var importer = CreateFileImporter(_cpDirectory, filename, typeof(ConversionMethod)))
                {
                    while (importer.GetNextRow())
                    {
                        string code = importer.GetString(0);

                        if (string.IsNullOrWhiteSpace(code))
                        {
                            continue;
                        }

                        string description = importer.GetString(1);
                        ReceiptCategory category = importer.GetEnum<ReceiptCategory>(2);
                        bool isActive = importer.GetBool(3);

                        context.CP_ReceiptTypes.AddOrUpdate(
                            prop => prop.Code,
                            new ReceiptType { Code = code, Name = description, Category = category, IsActive = isActive });
                    }
                }
                context.SaveChanges();
            }
        }

        private void LoadReceiptBatchTypes(string filename)
        {
            using (var context = new DomainContext())
            {
                var bankAccountIds = LoadLegacyIds(_cpOutputDirectory, OutputFile.CP_BankAccountIdMappingFile);

                using (var importer = CreateFileImporter(_cpDirectory, filename, typeof(ConversionMethod)))
                {
                    while (importer.GetNextRow())
                    {
                        string code = importer.GetString(0);

                        if (string.IsNullOrWhiteSpace(code))
                        {
                            continue;
                        }

                        string description = importer.GetString(1);
                        string acct = importer.GetString(2);
                        Guid? acctId = null;
                        if (!string.IsNullOrWhiteSpace(acct))
                        {
                            Guid id;
                            if (!bankAccountIds.TryGetValue(acct, out id))
                            {
                                importer.LogError($"Invalid bank account \"{acct}\".");
                                continue;
                            }
                            acctId = id;
                        }

                        bool isActive = importer.GetBool(3);

                        context.CP_ReceiptBatchTypes.AddOrUpdate(
                            prop => prop.Code,
                            new ReceiptBatchType { Code = code, Name = description, BankAccountId = acctId, IsActive = isActive });
                    }
                }
                context.SaveChanges();
            }
        }

        private void LoadConfiguration(string filename)
        {
            var bl = new ConfigurationLogic();
            IUnitOfWork uow = bl.UnitOfWork;

            using (var ifile = CreateFileImporter(_cpDirectory, filename, typeof(ConversionMethod)))
            {
                while (ifile.GetNextRow())
                {
                    CPConfiguration config = bl.GetConfiguration<CPConfiguration>();

                    config.DefaultBatchEntryMode = ifile.GetEnum<ReceiptBatchEntryMode>(0);
                    config.EnableMailroomBatches = ifile.GetBool(1);
                    config.EnableCashierBatches = ifile.GetBool(2);
                    config.EnableSlipPrinting = ifile.GetBool(3);
                    config.EnableTwoStageReceipting = ifile.GetBool(4);
                    config.CheckWrittenAmountFormat = ifile.GetString(5);

                    bl.SaveConfiguration(config);
                    break;
                }
            }
        }

        private void LoadBankAccounts(string filename)
        {
            LoadLedgerAccountIds();
            LoadBusinessUnitIds();

            DomainContext context = new DomainContext();

            var bankAccounts = LoadEntities(context.CP_BankAccounts);

            using (var importer = CreateFileImporter(_cpDirectory, filename, typeof(ConversionMethod)))
            {

                // Legacy ID file, to convert from OE id to SQL Id.
                FileExport<LegacyToID> legacyIdFile = new FileExport<LegacyToID>(Path.Combine(_cpOutputDirectory, OutputFile.CP_BankAccountIdMappingFile), false, true);

                while (importer.GetNextRow())
                {
                    string code = importer.GetString(0, 4);
                    string description = importer.GetString(1, 128);

                    if (string.IsNullOrWhiteSpace(code))
                    {
                        continue;
                    }

                    BankAccount ba = bankAccounts.FirstOrDefault(p => p.Code == code);
                    if (ba == null)
                    {
                        ba = new BankAccount();
                        context.CP_BankAccounts.Add(ba);
                    }

                    ba.Code = code;
                    ba.Name = description;
                    ba.BusinessUnitId = GetBusinessUnitId(importer, 2);
                    ba.CompanyName = importer.GetString(3, 30);
                    
                    int debitAccountKey = importer.GetInt(4);
                    int creditAccountKey = importer.GetInt(5);

                    if (debitAccountKey > 0)
                    {
                        Guid accountId;
                        if (!LedgerAccountIds.TryGetValue(debitAccountKey, out accountId))
                        {
                            importer.LogError($"Invalid debit GL account legacy key \"{debitAccountKey}\".");
                            continue;
                        }
                        ba.DebitAccountId = accountId;
                    }

                    if (creditAccountKey > 0)
                    {
                        Guid accountId;
                        if (!LedgerAccountIds.TryGetValue(creditAccountKey, out accountId))
                        {
                            importer.LogError($"Invalid credit GL account legacy key \"{creditAccountKey}\".");
                            continue;
                        }
                        ba.CreditAccountId = accountId;
                    }

                    ba.BankName = importer.GetString(6, 128);
                    ba.RoutingNumber = importer.GetString(7, 9);
                    ba.AccountNumber = importer.GetString(8, 128);
                    ba.BankAccountType = importer.GetEnum<EFTAccountType>(9);
                    ba.OriginNumber = importer.GetString(10, 10);
                    ba.OriginName = importer.GetString(11, 30);
                    ba.DestinationNumber = importer.GetString(12, 10);
                    ba.DestinationName = importer.GetString(13, 30);
                    ba.CompanyIdNumber = importer.GetString(14, 10);
                    ba.OriginatingFIDNumber = importer.GetString(15, 10);
                    ba.FileIdModifier = importer.GetString(16, 1);
                    ba.OffsetRoutingNumber = importer.GetString(17, 9);
                    ba.OffsetAccountNumber = importer.GetString(18, 30);
                    ba.OffsetDescription = importer.GetString(19, 30);
                    ba.FractionalFormat = importer.GetString(20, 30);
                    ba.GenerateBalancedACH = importer.GetBool(21);
                    ba.IsActive = importer.GetBool(22);
                    ba.AssignPrimaryKey();

                    context.CP_BankAccounts.AddOrUpdate(p => p.Id, ba);
                    legacyIdFile.AddRow(new LegacyToID(code, ba.Id));
                }

                context.SaveChanges();
                legacyIdFile.Dispose();
            }
        }

    }
}
