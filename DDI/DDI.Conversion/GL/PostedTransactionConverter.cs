using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Conversion.Statics;
using DDI.Data;
using DDI.Shared.Enums.Core;
using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Client.Security;

namespace DDI.Conversion.GL
{    
    internal class PostedTransactionConverter : GLConversionBase
    {
        public enum ConversionMethod
        {
            PostedTransactions = 70300,
        }

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            Initialize(baseDirectory);
            
            RunConversion(ConversionMethod.PostedTransactions, () => ConvertPostedTransactions(InputFile.GL_PostedTransactions, false));
        }


        private void ConvertPostedTransactions(string filename, bool append)
        {
            DomainContext context = new DomainContext();

            LoadFiscalYearIds();
            LoadLedgerAccountYearIds();
            
            var outputFile = new FileExport<PostedTransaction>(Path.Combine(OutputDirectory, OutputFile.GL_PostedTransactionFile), append);
            var legacyIdFile = new FileExport<LegacyToID>(Path.Combine(OutputDirectory, OutputFile.GL_PostedTransactionMappingFile), append, true);

            if (!append)
            {
                outputFile.AddHeaderRow();
            }

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

                    int legacyKey = importer.GetInt(13);
                    string legacyAccountKey = importer.GetString(2);

                    Guid accountId;
                    if (!LedgerAccountYearIds.TryGetValue(legacyAccountKey, out accountId))
                    {
                        importer.LogError($"Invalid account legacy key \"{legacyAccountKey}\".");
                        continue;
                    }

                    PostedTransaction transaction = new PostedTransaction();
                    transaction.FiscalYearId = fiscalYearId;
                    transaction.LedgerAccountYearId = accountId;
                    transaction.PeriodNumber = importer.GetInt(3);
                    transaction.PostedTransactionType = importer.GetEnum<PostedTransactionType>(4);
                    transaction.TransactionDate = importer.GetDate(5);
                    transaction.Amount = importer.GetDecimal(6);
                    transaction.TransactionNumber = importer.GetInt64(8);
                    transaction.LineNumber = importer.GetInt(9);
                    transaction.Description = importer.GetString(10);
                    transaction.TransactionType = importer.GetEnum<TransactionType>(11);
                    transaction.IsAdjustment = importer.GetBool(12);

                    transaction.AssignPrimaryKey();

                    outputFile.AddRow(transaction);
                    legacyIdFile.AddRow(new LegacyToID(legacyKey, transaction.Id));

                    count++;                    
                }
            }

            legacyIdFile.Dispose();
            outputFile.Dispose();
        }

    }
}
