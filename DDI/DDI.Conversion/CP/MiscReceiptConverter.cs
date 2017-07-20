using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using DDI.Conversion.Core;
using DDI.Conversion.GL;
using DDI.Conversion.Statics;
using DDI.Data;
using DDI.Shared.Enums.CP;
using DDI.Shared.Extensions;
using DDI.Shared.Models.Client.CP;
using DDI.Shared.Models.Client.GL;

namespace DDI.Conversion.CP
{
    internal class MiscReceiptConverter : GLConversionBase
    {
        private Dictionary<string, Guid> _miscReceiptIds;
        private Dictionary<int, Guid> _constituentIds;
        private string _cpOutputDirectory;
        private string _cpDirectory;
        private const string ENTITY_TYPE_MISC_RECEIPT = "MiscReceipt";

        public enum ConversionMethod
        {
            MiscReceipts = 41201,
            MiscReceiptLines,
            MiscReceiptTransactions,
            MiscReceiptApprovals,
            MiscReceiptNotes,
            MiscReceiptEntityNumbers,
            MiscReceiptFileStorage,
            MiscReceiptAttachments,
        }

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            Initialize(baseDirectory);
            _cpDirectory = Path.Combine(baseDirectory, DirectoryName.CP);
            _cpOutputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.CP);

            RunConversion(ConversionMethod.MiscReceipts, () => ConvertMiscReceipts(InputFile.CP_MiscReceipts, false));
            RunConversion(ConversionMethod.MiscReceiptLines, () => ConvertMiscReceiptLines(InputFile.CP_MiscReceiptLines, false));
            RunConversion(ConversionMethod.MiscReceiptTransactions, () => ConvertTransactions(InputFile.CP_MiscReceiptTransactions, InputFile.CP_MiscReceiptEntityTransactions, false));
            RunConversion(ConversionMethod.MiscReceiptApprovals, () => ConvertApprovals(InputFile.CP_MiscReceiptApprovals, false));
            RunConversion(ConversionMethod.MiscReceiptNotes, () => ConvertNotes(InputFile.CP_MemoMiscReceipts, false));
            RunConversion(ConversionMethod.MiscReceiptEntityNumbers, () => ConvertEntityNumbers(InputFile.CP_MiscReceiptEntityNumbers));
            RunConversion(ConversionMethod.MiscReceiptFileStorage, () => ConvertFileStorage(InputFile.CP_MiscReceiptFileStorage, true));
            RunConversion(ConversionMethod.MiscReceiptAttachments, () => ConvertAttachments(InputFile.CP_MiscReceiptAttachments, false));

        }

        private void ConvertMiscReceipts(string filename, bool append)
        {
            using (var context = new DomainContext())
            {
                context.GL_Ledgers.Load();
                var ledgers = context.GL_Ledgers.Local;

                LoadLedgerIds();
                LoadFiscalYearIds();
                LoadLedgerAccountIds();
                LoadConstituentIds();

                var outputFile = new FileExport<MiscReceipt>(Path.Combine(_cpOutputDirectory, OutputFile.CP_MiscReceiptFile), append);
                var legacyIdFile = new FileExport<LegacyToID>(Path.Combine(_cpOutputDirectory, OutputFile.CP_MiscReceiptMappingFile), append, true);

                if (!append)
                {
                    outputFile.AddHeaderRow();
                }

                using (var importer = CreateFileImporter(_cpDirectory, filename, typeof(ConversionMethod)))
                {
                    int count = 1;

                    while (importer.GetNextRow())
                    {
                        Guid? ledgerId;
                        Guid id;

                        Guid? fiscalYearId = GetFiscalYearId(importer, 0, true, out ledgerId);

                        if (ledgerId == null)
                        {
                            continue;
                        }

                        Ledger ledger = ledgers.FirstOrDefault(p => p.Id == ledgerId);
                        if (ledger == null)
                        {
                            continue;
                        }

                        string legacyKey = importer.GetString(4);

                        MiscReceipt mrcpt = new MiscReceipt();
                        mrcpt.BusinessUnitId = ledger.BusinessUnitId;
                        mrcpt.FiscalYearId = fiscalYearId;
                        mrcpt.MiscReceiptType = importer.GetEnum<MiscReceiptType>(2);
                        mrcpt.MiscReceiptNumber = importer.GetInt(3);
                        mrcpt.Comment = importer.GetString(5, 255);
                        mrcpt.Amount = importer.GetDecimal(6);
                        mrcpt.TransactionDate = importer.GetDate(7);
                        mrcpt.IsReversed = importer.GetBool(8);
                        mrcpt.DeletionDate = importer.GetDate(9);

                        // GL account
                        int accountKey = importer.GetInt(10);
                        if (accountKey > 0)
                        {
                            if (!LedgerAccountIds.TryGetValue(accountKey, out id))
                            {
                                importer.LogError($"Invalid account key {accountKey}.");
                            }
                            else
                            {
                                mrcpt.DebitLedgerAccountId = id;
                            }
                        }

                        // Constituent

                        int constituentNum = importer.GetInt(11);
                        if (constituentNum > 0)
                        {
                            if (_constituentIds.TryGetValue(constituentNum, out id))
                            {
                                mrcpt.ConstituentId = id;
                            }
                            else
                            {
                                importer.LogError($"Invalid constituent PIN {constituentNum} for misc. receipt legacy Id {legacyKey}.");
                            }
                        }

                        ImportCreatedModifiedInfo(mrcpt, importer, 12);
                        mrcpt.AssignPrimaryKey();

                        outputFile.AddRow(mrcpt);
                        legacyIdFile.AddRow(new LegacyToID(legacyKey, mrcpt.Id));

                        count++;
                    }
                }

                legacyIdFile.Dispose();
                outputFile.Dispose();
            }
        }

        private void ConvertMiscReceiptLines(string filename, bool append)
        {
            using (var context = new DomainContext())
            {

                context.GL_Ledgers.Load();
                var ledgers = context.GL_Ledgers.Local;

                LoadMiscReceiptIds();
                LoadLedgerAccountIds();

                var outputFile = new FileExport<MiscReceiptLine>(Path.Combine(_cpOutputDirectory, OutputFile.CP_MiscReceiptLineFile), append);
                var legacyIdFile = new FileExport<LegacyToID>(Path.Combine(_cpOutputDirectory, OutputFile.CP_MiscReceiptLineMappingFile), append, true);

                if (!append)
                {
                    outputFile.AddHeaderRow();
                }

                using (var importer = CreateFileImporter(_cpDirectory, filename, typeof(ConversionMethod)))
                {
                    int count = 1;

                    while (importer.GetNextRow())
                    {
                        Guid miscReceiptId;
                        Guid id;

                        string miscReceiptKey = importer.GetString(0);
                        if (!_miscReceiptIds.TryGetValue(miscReceiptKey, out miscReceiptId))
                        {
                            importer.LogError($"Invalid misc. receipt key \"{miscReceiptKey}\".");
                            continue;
                        }

                        MiscReceiptLine line = new MiscReceiptLine();
                        line.MiscReceiptId = miscReceiptId;
                        line.LineNumber = importer.GetInt(1);
                        line.Comment = importer.GetString(6, 255);
                        line.TransactionDate = importer.GetDate(2);
                        line.DeletedOn = importer.GetDate(3);
                        line.Amount = importer.GetDecimal(4);

                        // GL account
                        int accountKey = importer.GetInt(5);
                        if (!LedgerAccountIds.TryGetValue(accountKey, out id))
                        {
                            importer.LogError($"Invalid account key {accountKey}.");
                        }
                        else
                        {
                            line.LedgerAccountId = id;
                        }
                                                
                        ImportCreatedModifiedInfo(line, importer, 7);

                        line.AssignPrimaryKey();

                        outputFile.AddRow(line);
                        legacyIdFile.AddRow(new LegacyToID($"{miscReceiptKey},{line.LineNumber}", line.Id));

                        count++;
                    }
                }

                legacyIdFile.Dispose();
                outputFile.Dispose();
            }
        }

        private void ConvertTransactions(string transactionFilename, string entityTransactionFilename, bool append)
        {
            LoadFiscalYearIds();
            LoadLedgerAccountYearIds();
            LoadMiscReceiptIds();
            _miscReceiptIds.AddRange(LoadLegacyIds(_cpOutputDirectory, OutputFile.CP_MiscReceiptLineMappingFile));

            var tranConverter = new TransactionConverter(FiscalYearIds, LedgerAccountYearIds);
            tranConverter.ConvertTransactions(() => CreateFileImporter(_cpDirectory, transactionFilename, typeof(ConversionMethod)),
                                               () => CreateFileImporter(_cpDirectory, entityTransactionFilename, typeof(ConversionMethod)),
                                               ENTITY_TYPE_MISC_RECEIPT, _miscReceiptIds, append);
        }

        private void ConvertApprovals(string approvalFilename, bool append)
        {
            LoadMiscReceiptIds();
            var tranConverter = new ApprovalConverter();
            tranConverter.ConvertApprovals(() => CreateFileImporter(_cpDirectory, approvalFilename, typeof(ConversionMethod)), ENTITY_TYPE_MISC_RECEIPT, _miscReceiptIds, append);
        }

        private void ConvertEntityNumbers(string filename)
        {
            LoadBusinessUnitIds();
            LoadFiscalYearIds();

            var converter = new EntityNumberConverter(BusinessUnitIds, FiscalYearIds);
            converter.ConvertEntityNumbers(() => CreateFileImporter(_cpDirectory, filename, typeof(ConversionMethod)));
        }

        private void ConvertNotes(string filename, bool append)
        {
            LoadMiscReceiptIds();
            var noteConverter = new NoteConverter();
            noteConverter.ConvertNotes(() => CreateFileImporter(_cpDirectory, filename, typeof(ConversionMethod)), ENTITY_TYPE_MISC_RECEIPT, _miscReceiptIds, false);
        }

        private void ConvertFileStorage(string filename, bool append)
        {
            var attachmentConverter = new AttachmentConverter();

            attachmentConverter.ConvertFileStorage(() => CreateFileImporter(_cpDirectory, filename, typeof(ConversionMethod)), append);
        }

        private void ConvertAttachments(string filename, bool append)
        {
            var attachmentConverter = new AttachmentConverter();
            LoadMiscReceiptIds();
            attachmentConverter.ConvertAttachments(() => CreateFileImporter(_cpDirectory, filename, typeof(ConversionMethod)),
                ENTITY_TYPE_MISC_RECEIPT, _miscReceiptIds, append);
        }


        private void LoadMiscReceiptIds()
        {
            if (_miscReceiptIds == null)
            {
                _miscReceiptIds = LoadLegacyIds(_cpOutputDirectory, OutputFile.CP_MiscReceiptMappingFile);                     
            }
        }

        private Dictionary<string, Guid> GetMiscReceiptIds()
        {
            LoadMiscReceiptIds();
            return _miscReceiptIds;
        }


        private void LoadConstituentIds()
        {
            string crmOutputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.CRM);
            _constituentIds = new Dictionary<int, Guid>();

            using (var importer = new FileImport(Path.Combine(crmOutputDirectory, OutputFile.CRM_ConstituentIdMappingFile), string.Empty))
            {
                while (importer.GetNextRow())
                {
                    int legacyKey = importer.GetInt(0);
                    _constituentIds[legacyKey] = importer.GetGuid(1);
                }
            }
        }

    }
}
