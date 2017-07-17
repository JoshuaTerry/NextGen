using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Conversion.Core;
using DDI.Conversion.Statics;
using DDI.Data;
using DDI.Shared.Enums.Core;
using DDI.Shared.Enums.GL;
using DDI.Shared.Extensions;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Client.Security;

namespace DDI.Conversion.GL
{    
    internal class JournalConverter : GLConversionBase
    {
        private const string ENTITY_TYPE_JOURNAL = "Journal";
        private Dictionary<string, Guid> _journalIds, _fundIds;

        public enum ConversionMethod
        {
            Journals = 70400,
            JournalLines,
            JournalTransactions,
            JournalApprovals,
            JournalNotes,
            JournalEntityNumbers,
            JournalFileStorage,
            JournalAttachments,
        }

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            Initialize(baseDirectory);
            
            RunConversion(ConversionMethod.Journals, () => ConvertJournals(InputFile.GL_Journals, false));
            RunConversion(ConversionMethod.JournalLines, () => ConvertJournalLines(InputFile.GL_JournalLines, false));
            RunConversion(ConversionMethod.JournalTransactions, () => ConvertTransactions(InputFile.GL_JournalTransactions, InputFile.GL_JournalEntityTransactions, false));
            RunConversion(ConversionMethod.JournalApprovals, () => ConvertApprovals(InputFile.GL_JournalApprovals, false));
            RunConversion(ConversionMethod.JournalNotes, () => ConvertNotes(InputFile.GL_MemoJournals, false));
            RunConversion(ConversionMethod.JournalEntityNumbers, () => ConvertEntityNumbers(InputFile.GL_JournalEntityNumbers));
            RunConversion(ConversionMethod.JournalFileStorage, () => ConvertFileStorage(InputFile.GL_FileStorage, true));
            RunConversion(ConversionMethod.JournalAttachments, () => ConvertAttachments(InputFile.GL_Attachment, false));

        }

        private void ConvertTransactions(string transactionFilename, string entityTransactionFilename, bool append)
        {
            LoadFiscalYearIds();
            LoadLedgerAccountYearIds();
            LoadJournalIds();
            _journalIds.AddRange(LoadLegacyIds(GLOutputDirectory, OutputFile.GL_JournalLineMappingFile));

            var tranConverter = new TransactionConverter(FiscalYearIds, LedgerAccountYearIds);
            tranConverter.ConvertTransactions(() => CreateFileImporter(GLDirectory, transactionFilename, typeof(ConversionMethod)),
                                               () => CreateFileImporter(GLDirectory, entityTransactionFilename, typeof(ConversionMethod)),
                                               ENTITY_TYPE_JOURNAL, _journalIds, append);
        }

        private void ConvertApprovals(string approvalFilename, bool append)
        {
            LoadJournalIds();
            var tranConverter = new ApprovalConverter();
            tranConverter.ConvertApprovals(() => CreateFileImporter(GLDirectory, approvalFilename, typeof(ConversionMethod)), ENTITY_TYPE_JOURNAL, _journalIds, append);
        }

        private void ConvertEntityNumbers(string filename)
        {
            LoadBusinessUnitIds();
            LoadFiscalYearIds();

            var converter = new EntityNumberConverter(BusinessUnitIds, FiscalYearIds);
            converter.ConvertEntityNumbers(() => CreateFileImporter(GLDirectory, filename, typeof(ConversionMethod)));
        }

        private void ConvertNotes(string filename, bool append)
        {
            LoadJournalIds();
            var noteConverter = new NoteConverter();
            noteConverter.ConvertNotes(() => CreateFileImporter(GLDirectory, filename, typeof(ConversionMethod)), ENTITY_TYPE_JOURNAL, _journalIds, false);
        }

        private void ConvertFileStorage(string filename, bool append)
        {
            var attachmentConverter = new AttachmentConverter();

            attachmentConverter.ConvertFileStorage(() => CreateFileImporter(GLDirectory, filename, typeof(ConversionMethod)), append);
        }

        private void ConvertAttachments(string filename, bool append)
        {
            var attachmentConverter = new AttachmentConverter();
            LoadJournalIds();
            attachmentConverter.ConvertAttachments(() => CreateFileImporter(GLDirectory, filename, typeof(ConversionMethod)),
                ENTITY_TYPE_JOURNAL, _journalIds, append);
        }


        private void ConvertJournals(string filename, bool append)
        {

            using (var context = new DomainContext())
            {

                context.GL_Ledgers.Load();
                var ledgers = context.GL_Ledgers.Local;

                LoadLedgerIds();
                LoadFiscalYearIds();

                var outputFile = new FileExport<Journal>(Path.Combine(GLOutputDirectory, OutputFile.GL_JournalFile), append);
                var legacyIdFile = new FileExport<LegacyToID>(Path.Combine(GLOutputDirectory, OutputFile.GL_JournalMappingFile), append, true);

                var journalIds = new Dictionary<string, Guid>();  // For mapping journal legacy keys to Guids.

                if (!append)
                {
                    outputFile.AddHeaderRow();
                }

                using (var importer = CreateFileImporter(GLDirectory, filename, typeof(ConversionMethod)))
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

                        Journal journal = new Journal();
                        journal.BusinessUnitId = ledger.BusinessUnitId;
                        journal.FiscalYearId = fiscalYearId;
                        journal.JournalType = importer.GetEnum<JournalType>(2);
                        journal.JournalNumber = importer.GetInt(3);
                        journal.Comment = importer.GetString(5, 255);
                        journal.Amount = importer.GetDecimal(6);
                        journal.TransactionDate = importer.GetDate(7);
                        journal.ReverseOnDate = importer.GetDate(8);
                        journal.IsReversed = importer.GetBool(9);
                        journal.DeletionDate = importer.GetDate(10);
                        journal.RecurringType = importer.GetEnum<RecurringType>(11);
                        journal.RecurringDay = importer.GetEnum<RecurringDay>(12);
                        journal.PreviousDate = importer.GetDate(13);
                        journal.IsExpired = importer.GetBool(14);
                        journal.ExpireAmount = importer.GetDecimal(15);
                        journal.ExpireAmountTotal = importer.GetDecimal(16);
                        journal.ExpireDate = importer.GetDate(17);
                        journal.ExpireCount = importer.GetInt(18);
                        journal.ExpireCountTotal = importer.GetInt(19);

                        string parentJournalKey = importer.GetString(20);
                        if (!string.IsNullOrWhiteSpace(parentJournalKey))
                        {
                            // Get existing Id for parent journal legacy key.  If there isn't one, create a new Id for it.
                            if (!journalIds.TryGetValue(parentJournalKey, out id))
                            {
                                id = GuidHelper.NewGuid();
                                journalIds[parentJournalKey] = id;
                            }

                            journal.ParentJournalId = id;
                        }

                        ImportCreatedModifiedInfo(journal, importer, 21);

                        // Look for an existing Id for the journal legacy key, which may have been assigned above for a child journal.
                        if (!journalIds.TryGetValue(legacyKey, out id))
                        {
                            id = GuidHelper.NewGuid();
                            journalIds[legacyKey] = id;
                        }

                        journal.Id = id;

                        outputFile.AddRow(journal);
                        legacyIdFile.AddRow(new LegacyToID(legacyKey, journal.Id));

                        count++;
                    }
                }

                legacyIdFile.Dispose();
                outputFile.Dispose();
            }
        }

        private void ConvertJournalLines(string filename, bool append)
        {
            using (var context = new DomainContext())
            {

                context.GL_Ledgers.Load();
                var ledgers = context.GL_Ledgers.Local;

                LoadBusinessUnitIds();
                LoadJournalIds();
                LoadLedgerAccountIds();
                LoadFundIds();

                var outputFile = new FileExport<JournalLine>(Path.Combine(GLOutputDirectory, OutputFile.GL_JournalLineFile), append);
                var legacyIdFile = new FileExport<LegacyToID>(Path.Combine(GLOutputDirectory, OutputFile.GL_JournalLineMappingFile), append, true);

                if (!append)
                {
                    outputFile.AddHeaderRow();
                }

                using (var importer = CreateFileImporter(GLDirectory, filename, typeof(ConversionMethod)))
                {
                    int count = 1;

                    while (importer.GetNextRow())
                    {
                        Guid journalId;
                        Guid id;

                        string journalKey = importer.GetString(0);
                        if (!_journalIds.TryGetValue(journalKey, out journalId))
                        {
                            importer.LogError($"Invalid journal key \"{journalKey}\".");
                            continue;
                        }

                        JournalLine line = new JournalLine();
                        line.JournalId = journalId;
                        line.LineNumber = importer.GetInt(1);
                        line.Comment = importer.GetString(2, 255);
                        line.TransactionDate = importer.GetDate(3);
                        line.DeletedOn = importer.GetDate(4);
                        line.Amount = importer.GetDecimal(5);
                        line.Percent = importer.GetDecimal(6);
                        line.DueToMode = importer.GetEnum<DueToMode>(7);

                        // GL account
                        int accountKey = importer.GetInt(14);
                        if (!LedgerAccountIds.TryGetValue(accountKey, out id))
                        {
                            importer.LogError($"Invalid account key {accountKey}.");
                        }
                        else
                        {
                            line.LedgerAccountId = id;
                        }

                        // Source business unit
                        int unitKey = importer.GetInt(8);
                        if (unitKey > 0)
                        {
                            if (!BusinessUnitIds.TryGetValue(unitKey, out id))
                            {
                                importer.LogError($"Invalid source business unit {unitKey}.");
                            }
                            else
                            {
                                line.SourceBusinessUnitId = id;
                            }
                        }

                        // Source fund
                        string fundKey = importer.GetString(9);
                        if (!string.IsNullOrWhiteSpace(fundKey))
                        {
                            if (!_fundIds.TryGetValue(fundKey, out id))
                            {
                                importer.LogError($"Invalid source fund key \"{fundKey}\".");
                            }
                            else
                            {
                                line.SourceFundId = id;
                            }
                        }

                        ImportCreatedModifiedInfo(line, importer, 10);

                        line.AssignPrimaryKey();

                        outputFile.AddRow(line);
                        legacyIdFile.AddRow(new LegacyToID($"{journalKey},{line.LineNumber}", line.Id));

                        count++;
                    }
                }

                legacyIdFile.Dispose();
                outputFile.Dispose();
            }
        }

        private void LoadJournalIds()
        {
            if (_journalIds == null)
            {
                _journalIds = LoadLegacyIds(GLOutputDirectory, OutputFile.GL_JournalMappingFile);                     
            }
        }

        private Dictionary<string, Guid> GetJournalIds()
        {
            LoadJournalIds();
            return _journalIds;
        }

        private void LoadFundIds()
        {
            if (_fundIds == null)
            {
                _fundIds = LoadLegacyIds(GLOutputDirectory, OutputFile.GL_FundIdMappingFile);
            }
        }


    }
}
