using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Conversion.Statics;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Enums.Core;

namespace DDI.Conversion.Core
{
    internal class TransactionConverter
    {

        private char[] _commaDelimiter = { ',' };
        private Dictionary<string, Guid> _fiscalYearIds;
        private Dictionary<string, Guid> _ledgerAccountYearIds;


        public TransactionConverter(Dictionary<string, Guid> fiscalYearIds, Dictionary<string, Guid> ledgerAccountYearIds)
        {
            _fiscalYearIds = fiscalYearIds;
            _ledgerAccountYearIds = ledgerAccountYearIds;
        }

        public void ConvertTransactions(Func<FileImport> transactionImporter, Func<FileImport> entityTransactionImporter, string outputFileNameSuffix,
            Dictionary<string, Guid> entityids, bool append)
        {
            Guid id;
            var postedtranXref = new Dictionary<Guid, string[]>();
            var postedTranKeys = new Dictionary<int, Guid?>();
            var tranKeys = new Dictionary<string, Guid>();

            string _outputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.Core);
            string _glPayloadDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.GL);

            // Make sure the IS Payload directory exists.
            Directory.CreateDirectory(_outputDirectory);

            // Create the output filename for transactions.
            string outputFilename = ConversionBase.CreateOutputFilename(OutputFile.Core_TransactionFile, outputFileNameSuffix);


            // Import the transactions.

            using (var importer = transactionImporter())
            {
                var outputFile = new FileExport<Transaction>(Path.Combine(_outputDirectory, outputFilename), append);
                if (!append)
                {
                    outputFile.AddHeaderRow();
                }


                while (importer.GetNextRow())
                {

                    string code = importer.GetString(0);
                    string yearName = importer.GetString(1);
                    string legacyKey = $"{code},{yearName}";

                    Guid fiscalYearId;
                    if (!_fiscalYearIds.TryGetValue(legacyKey, out fiscalYearId))
                    {
                        importer.LogError($"Invalid year \"{yearName}\" for cid \"{code}\".");
                        continue;
                    }

                    Transaction tran = new Transaction();
                    tran.FiscalYearId = fiscalYearId;
                    tran.TransactionNumber = importer.GetInt64(2);
                    tran.LineNumber = importer.GetInt(3);
                    tran.TransactionType = importer.GetEnum<TransactionType>(4);
                    tran.TransactionDate = importer.GetDate(5);
                    tran.PostDate = importer.GetDateTime(6);
                    tran.Amount = importer.GetDecimal(7);
                    tran.Status = importer.GetEnum<TransactionStatus>(10);
                    tran.IsAdjustment = importer.GetBool(11);
                    tran.Description = importer.GetString(12, 255);
                    tran.CreatedBy = importer.GetString(13);
                    tran.CreatedOn = importer.GetDateTime(14);

                    // Debit account
                    string accountKey = importer.GetString(8);
                    if (!string.IsNullOrWhiteSpace(accountKey))
                    {
                        if (!_ledgerAccountYearIds.TryGetValue(accountKey, out id))
                        {
                            importer.LogError($"Invalid debit account key \"{accountKey}\".");
                        }
                        else
                        {
                            tran.DebitAccountId = id;
                        }
                    }

                    // Credit account
                    accountKey = importer.GetString(9);
                    if (!string.IsNullOrWhiteSpace(accountKey))
                    {
                        if (!_ledgerAccountYearIds.TryGetValue(accountKey, out id))
                        {
                            importer.LogError($"Invalid credit account key \"{accountKey}\".");
                        }
                        else
                        {
                            tran.CreditAccountId = id;
                        }
                    }

                    tran.AssignPrimaryKey();

                    tranKeys.Add($"{tran.TransactionNumber},{tran.LineNumber}", tran.Id);

                    // Get the posted transaction keys and store in dictionary for later.
                    string keysText = importer.GetString(15);
                    if (!string.IsNullOrWhiteSpace(keysText))
                    {
                        string[] keys = keysText.Split(_commaDelimiter);
                        if (keys.Length > 0)
                        {
                            postedtranXref.Add(tran.Id, keys);
                            foreach (string key in keys)
                            {
                                int intkey;
                                if (!int.TryParse(key, out intkey))
                                {
                                    importer.LogError($"Invalid posted transaction key \"{key}\".");
                                }
                                else
                                {
                                    postedTranKeys.Add(intkey, null);
                                }
                            }
                        }
                    }
                    
                    outputFile.AddRow(tran);
                }

                outputFile.Dispose();
            }

            // Create the output filename for transaction xrefs.
            outputFilename = ConversionBase.CreateOutputFilename(OutputFile.Core_TransactionXrefFile, outputFileNameSuffix);

            // Read the posted transaction ID mapping file and populate postedTranKeys dictionary
            // which will map legacy ids to PostedTransaction.Id
            using (var importer = new FileImport(Path.Combine(_glPayloadDirectory, OutputFile.GL_PostedTransactionMappingFile), "TransactionConverter"))
            {
                while (importer.GetNextRow())
                {
                    int key = importer.GetInt(0);
                    if (key == 0)
                    {
                        continue;
                    }

                    id = importer.GetGuid(1);
                    if (postedTranKeys.ContainsKey(key))
                    {
                        postedTranKeys[key] = id;
                    }
                }

                // Process postedTranXref dictionary to generate the xref file.
                using (var outputFile = new FileExport<ConversionBase.JoinRow>(Path.Combine(_outputDirectory, outputFilename), append))
                {
                    if (!append)
                    {
                        outputFile.AddHeaderRow();
                    }
                    foreach (var entry in postedtranXref)
                    {
                        foreach (var key in entry.Value)
                        {
                            int intkey;
                            if (int.TryParse(key, out intkey))
                            {
                                Guid? keyId;
                                if (!postedTranKeys.TryGetValue(intkey, out keyId) || keyId == null)
                                {
                                    importer.LogError($"Invalid posted transaction key \"{key}\".");
                                }
                                else
                                {
                                    outputFile.AddRow(new ConversionBase.JoinRow(keyId.Value, entry.Key));
                                }
                            }
                        }
                    }
                } // using outputfile
            } // using importer


            // Create the output filename for entity transactions.
            outputFilename = ConversionBase.CreateOutputFilename(OutputFile.Core_EntityTransactionFile, outputFileNameSuffix);

            // Import the entity transactions

            using (var importer = entityTransactionImporter())
            {
                var outputFile = new FileExport<EntityTransaction>(Path.Combine(_outputDirectory, outputFilename), append);
                if (!append)
                {
                    outputFile.AddHeaderRow();
                }


                while (importer.GetNextRow())
                {
                    Guid entityId, tranId;

                    string tranKey = importer.GetString(5);
                    if (string.IsNullOrWhiteSpace(tranKey))
                    {
                        continue;
                    }

                    if (!tranKeys.TryGetValue(tranKey, out tranId))
                    {
                        importer.LogError($"Invalid transaction key \"{tranKey}\".");
                        continue;
                    }

                    string entityKey = importer.GetString(1);
                    if (!entityids.TryGetValue(entityKey, out entityId))
                    {
                        importer.LogError($"Invalid parent entity key \"{entityKey}\".");
                        continue;
                    }

                    EntityTransaction tran = new EntityTransaction();
                    tran.TransactionId = tranId;
                    tran.EntityType = importer.GetString(0, 128);
                    tran.Relationship = importer.GetEnum<EntityTransactionRelationship>(2);
                    tran.Category = importer.GetEnum<EntityTransactionCategory>(3);
                    tran.AmountType = importer.GetEnum<TransactionAmountType>(4);
                    tran.ParentEntityId = entityId;
                    tran.AssignPrimaryKey();

                    outputFile.AddRow(tran);
                }

                outputFile.Dispose();
            }
            
        }

    }
}
