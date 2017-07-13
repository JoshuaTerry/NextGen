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

namespace DDI.Conversion.CP
{

    /// <summary>
    /// OpenEdge to SSIS Data Conversion for the cash receipts.
    /// </summary>
    internal class ReceiptConverter : GLConversionBase
    {

        public enum ConversionMethod
        {
            ReceiptBatches = 42001,
            Receipts,
            ReceiptTransactions,
            ReceiptEntityNumbers,
            ReceiptFileStorage,
            ReceiptAttachments,

        }

        private string _cpDirectory;
        private string _cpOutputDirectory;

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            Initialize(baseDirectory);
            MethodsToRun = conversionMethods;
            _cpDirectory = Path.Combine(baseDirectory, DirectoryName.CP);
            _cpOutputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.CP);
 
            RunConversion(ConversionMethod.ReceiptBatches, () => ConvertReceiptBatches(InputFile.CP_ReceiptBatches, false));
            RunConversion(ConversionMethod.Receipts, () => ConvertReceipts(InputFile.CP_Receipts, false));
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

                    int batchNumber = importer.GetInt(2);
                    Guid batchId;
                    if (!batches.TryGetValue(batchNumber.ToString(), out batchId))
                    {
                        importer.LogError($"Receipt batch {batchNumber} is invalid.");
                        continue;
                    }

                    var receipt = new Receipt();
                    receipt.ReceiptNumber = importer.GetInt(3);
                    receipt.ReceiptBatchId = batchId;
                    receipt.Amount = importer.GetDecimal(4);
                    receipt.Reference = importer.GetString(5, 128);
                    receipt.IsProcessed = importer.GetBool(6);
                    receipt.IsReversed = importer.GetBool(7);
                    receipt.AccountNumber = importer.GetString(9, 64);
                    receipt.RoutingNumber = importer.GetString(10, 64);
                    receipt.CheckNumber = importer.GetString(11, 30);
                    receipt.TransactionDate = importer.GetDate(12);
                    ImportCreatedModifiedInfo(receipt, importer, 13);

                    string receiptType = importer.GetString(8);
                    if (!string.IsNullOrWhiteSpace(receiptType))
                    {
                        receipt.ReceiptType = receiptTypes.FirstOrDefault(p => p.Code == receiptType);
                        if (receipt.ReceiptType == null)
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

    }



}
