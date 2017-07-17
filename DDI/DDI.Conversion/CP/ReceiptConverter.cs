using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Conversion.Statics;
using DDI.Data;
using DDI.Shared.Extensions;
using DDI.Shared.Models.Client.CP;
using DDI.Shared.Enums.CP;
using DDI.Conversion.GL;
using DDI.Conversion.Core;

namespace DDI.Conversion.CP
{

    /// <summary>
    /// OpenEdge to SSIS Data Conversion for the cash receipts.
    /// </summary>
    internal class ReceiptConverter : GLConversionBase
    {

        public enum ConversionMethod
        {
            ReceiptBatches = 41101,
            Receipts,
            ReceiptTransactions,
            ReceiptEntityNumbers,
            ReceiptFileStorage,
            ReceiptAttachments,

        }

        private string _cpDirectory;
        private string _cpOutputDirectory;
        private Dictionary<string, Guid> _receiptIds;

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            Initialize(baseDirectory);
            MethodsToRun = conversionMethods;
            _cpDirectory = Path.Combine(baseDirectory, DirectoryName.CP);
            _cpOutputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.CP);
 
            RunConversion(ConversionMethod.ReceiptBatches, () => ConvertReceiptBatches(InputFile.CP_ReceiptBatches, false));
            RunConversion(ConversionMethod.Receipts, () => ConvertReceipts(InputFile.CP_Receipts, false));
            RunConversion(ConversionMethod.ReceiptTransactions, () => ConvertTransactions(InputFile.CP_ReceiptTransactions, InputFile.CP_ReceiptEntityTransactions, false));
            RunConversion(ConversionMethod.ReceiptEntityNumbers, () => ConvertEntityNumbers(InputFile.CP_ReceiptEntityNumbers));
            RunConversion(ConversionMethod.ReceiptFileStorage, () => ConvertFileStorage(InputFile.CP_ReceiptFileStorage, true));
            RunConversion(ConversionMethod.ReceiptAttachments, () => ConvertAttachments(InputFile.CP_ReceiptAttachments, false));
        }

        private void ConvertReceiptBatches(string filename, bool append)
        {
            DomainContext context = new DomainContext();

            LoadBusinessUnitIds();

            var batchTypes = LoadEntities(context.CP_ReceiptBatchTypes);
            var bankAccounts = LoadLegacyIds(_cpOutputDirectory, OutputFile.CP_BankAccountIdMappingFile);
            var users = LoadEntities(context.Users);

            using (var importer = CreateFileImporter(_cpDirectory, filename, typeof(ConversionMethod)))
            {                
                var outputFile = new FileExport<ReceiptBatch>(Path.Combine(_cpOutputDirectory, OutputFile.CP_ReceiptBatchFile), append);

                // Legacy ID file, to convert from OE id to SQL Id.
                FileExport<LegacyToID> legacyIdFile = new FileExport<LegacyToID>(Path.Combine(_cpOutputDirectory, OutputFile.CP_ReceiptBatchIdMappingFile), append, true);
                
                if (!append)
                {
                    outputFile.AddHeaderRow();
                }

                int count = 0;

                while (importer.GetNextRow())
                {
                    count++;

                    var batch = new ReceiptBatch();
                    batch.BusinessUnitId = GetBusinessUnitId(importer, 0);
                    batch.BatchNumber = importer.GetInt(1);
                    batch.Name = importer.GetString(2, 256);
                    batch.EntryMode = importer.GetEnum<ReceiptBatchEntryMode>(4);
                    batch.Status = importer.GetEnum<ReceiptBatchStatus>(6);
                    batch.DistributionMode = importer.GetEnum<ReceiptBatchDistributionMode>(7);
                    batch.EffectiveDate = importer.GetDate(8);
                    batch.TransactionDate = importer.GetDate(9);
                    batch.CreatedOn = importer.GetDateTime(10);
                    batch.CreatedBy = importer.GetString(11, 64);

                    string acct = importer.GetString(3);
                    if (!string.IsNullOrWhiteSpace(acct))
                    {
                        Guid id;
                        if (bankAccounts.TryGetValue(acct, out id))
                        {
                            batch.BankAccountId = id;
                        }
                        else
                        {
                            importer.LogError($"Invalid bank account {acct}.");
                        }
                    }

                    string batchType = importer.GetString(5);
                    if (!string.IsNullOrWhiteSpace(batchType))
                    {
                        batch.BatchTypeId = batchTypes.FirstOrDefault(p => p.Code == batchType)?.Id;
                        if (batch.BatchTypeId == null)
                        {
                            importer.LogError($"Invalid cash receipt batch type code {batchType}.");
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(batch.CreatedBy))
                    {
                        batch.EnteredById = GetUserByName(users, batch.CreatedBy)?.Id;
                    }

                    string inUseBy = importer.GetString(12);
                    if (!string.IsNullOrWhiteSpace(inUseBy))
                    {
                        batch.InUseById = GetUserByName(users, inUseBy)?.Id;
                    }

                    batch.AssignPrimaryKey();

                    outputFile.AddRow(batch);

                    legacyIdFile.AddRow(new LegacyToID(batch.BatchNumber, batch.Id));
                }

                outputFile.Dispose();
                legacyIdFile.Dispose();
            }

            context.Dispose();
            
        }


        private void ConvertReceipts(string filename, bool append)
        {
            DomainContext context = new DomainContext();

            var batches = LoadLegacyIds(_cpOutputDirectory, OutputFile.CP_ReceiptBatchIdMappingFile);
            var receiptTypes = LoadEntities(context.CP_ReceiptTypes);

            using (var importer = CreateFileImporter(_cpDirectory, filename, typeof(ConversionMethod)))
            {
                var outputFile = new FileExport<Receipt>(Path.Combine(_cpOutputDirectory, OutputFile.CP_ReceiptFile), append);

                // Legacy ID file, to convert from OE id to SQL Id.
                FileExport<LegacyToID> legacyIdFile = new FileExport<LegacyToID>(Path.Combine(_cpOutputDirectory, OutputFile.CP_ReceiptMappingFile), append, true);

                if (!append)
                {
                    outputFile.AddHeaderRow();
                }

                int count = 0;

                while (importer.GetNextRow())
                {
                    count++;

                    string legacyKey = importer.GetString(0);
                    if (string.IsNullOrWhiteSpace(legacyKey))
                    {
                        continue;
                    }

                    int batchNumber = importer.GetInt(1);
                    Guid batchId;
                    if (!batches.TryGetValue(batchNumber.ToString(), out batchId))
                    {
                        importer.LogError($"Receipt batch {batchNumber} is invalid.");
                        continue;
                    }

                    var receipt = new Receipt();
                    receipt.ReceiptNumber = importer.GetInt(2);
                    receipt.ReceiptBatchId = batchId;
                    receipt.Amount = importer.GetDecimal(3);
                    receipt.Reference = importer.GetString(4, 128);
                    receipt.IsProcessed = importer.GetBool(5);
                    receipt.IsReversed = importer.GetBool(6);
                    receipt.AccountNumber = importer.GetString(8, 64);
                    receipt.RoutingNumber = importer.GetString(9, 64);
                    receipt.CheckNumber = importer.GetString(10, 30);
                    receipt.TransactionDate = importer.GetDate(11);
                    ImportCreatedModifiedInfo(receipt, importer, 12);

                    string receiptType = importer.GetString(7);
                    if (!string.IsNullOrWhiteSpace(receiptType))
                    {
                        receipt.ReceiptTypeId = receiptTypes.FirstOrDefault(p => p.Code == receiptType)?.Id;
                        if (receipt.ReceiptTypeId == null)
                        {
                            importer.LogError($"Invalid cash receipt type \"{receiptType}\".");
                        }
                    }

                    receipt.AssignPrimaryKey();

                    outputFile.AddRow(receipt);

                    legacyIdFile.AddRow(new LegacyToID(legacyKey, receipt.Id));
                }

                outputFile.Dispose();
                legacyIdFile.Dispose();
            }

            context.Dispose();

        }

        private void ConvertTransactions(string transactionFilename, string entityTransactionFilename, bool append)
        {
            LoadFiscalYearIds();
            LoadLedgerAccountYearIds();
            LoadReceiptIds();

            var tranConverter = new TransactionConverter(FiscalYearIds, LedgerAccountYearIds);
            tranConverter.ConvertTransactions(() => CreateFileImporter(_cpDirectory, transactionFilename, typeof(ConversionMethod)),
                                               () => CreateFileImporter(_cpDirectory, entityTransactionFilename, typeof(ConversionMethod)),
                                               "Receipt", _receiptIds, append);
        }

        private void ConvertEntityNumbers(string filename)
        {
            LoadBusinessUnitIds();
            LoadFiscalYearIds();

            var converter = new EntityNumberConverter(BusinessUnitIds, FiscalYearIds);
            converter.ConvertEntityNumbers(() => CreateFileImporter(_cpDirectory, filename, typeof(ConversionMethod)));
        }

        private void ConvertFileStorage(string filename, bool append)
        {
            var attachmentConverter = new AttachmentConverter();

            attachmentConverter.ConvertFileStorage(() => CreateFileImporter(_cpDirectory, filename, typeof(ConversionMethod)), append);
        }

        private void ConvertAttachments(string filename, bool append)
        {
            var attachmentConverter = new AttachmentConverter();
            LoadReceiptIds();
            attachmentConverter.ConvertAttachments(() => CreateFileImporter(_cpDirectory, filename, typeof(ConversionMethod)),
                "Receipt", _receiptIds, append);
        }

        private void LoadReceiptIds()
        {
            if (_receiptIds == null)
            {
                _receiptIds = LoadLegacyIds(_cpOutputDirectory, OutputFile.CP_ReceiptMappingFile);
            }
        }

    }



}
