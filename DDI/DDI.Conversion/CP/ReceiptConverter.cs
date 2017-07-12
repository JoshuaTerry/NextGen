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
        }

        private string _cpDirectory;
        private string _cpOutputDirectory;

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            _cpDirectory = Path.Combine(baseDirectory, DirectoryName.CP);
            _cpOutputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.CP);
 
            RunConversion(ConversionMethod.ReceiptBatches, () => ConvertReceiptBatches(InputFile.CP_ReceiptBatches, false));
        }

        private void ConvertReceiptBatches(string filename, bool append)
        {
            DomainContext context = new DomainContext();
            char[] commaDelimiter = { ',' };

            LoadBusinessUnitIds();

            // Load the EFT formats
            var batchTypes = LoadEntities(context.CP_ReceiptBatchTypes);
            var bankAccounts = LoadLegacyIds(_cpOutputDirectory, OutputFile.CP_BankAccountIdMappingFile);
            /*
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

                    int constituentNum;
                    string constituentNumText = importer.GetString(0);
                
                    if (!int.TryParse(constituentNumText, out constituentNum))
                    {
                        // non-blank invalid constituent number is probably a header row and can be skipped.
                        continue;
                    }

                    Guid? constituentId = null;

                    // Not all EFTInfo rows have a constituent number (PIN).  If PIN > 0, grab the constituent.
                    if (constituentNum > 0)
                    {
                        constituentId = _constituentIds.GetValueOrDefault(constituentNum);
                        if (constituentId == null || constituentId.Value == Guid.Empty)
                        {
                            importer.LogError($"Invalid constituent number {constituentNum}.");
                            continue;
                        }
                    }

                    string description = importer.GetString(1, 128);
                    string bankName = importer.GetString(2, 128);
                    string bankAccount = importer.GetString(3, 64);
                    string routingNumber = importer.GetString(4, 64);
                    string tranCode = importer.GetString(5);
                    string formatCode = importer.GetString(6);
                    string legacyId = importer.GetString(7);
                    string statusCode = importer.GetString(8);

                    var eftInfo = new PaymentMethod();
                    eftInfo.Category = PaymentMethodCategory.EFT;
                    eftInfo.BankAccount = bankAccount;
                    eftInfo.BankName = bankName;
                    eftInfo.RoutingNumber = routingNumber;
                    eftInfo.Description = description;
                    
                    if (tranCode == "22")
                    {
                        eftInfo.AccountType = EFTAccountType.Checking;
                    }
                    else if (tranCode == "32")
                    {
                        eftInfo.AccountType = EFTAccountType.Savings;
                    }
                    else
                    {
                        importer.LogError($"Invalid tran code \"{tranCode}\".");
                    }

                    EFTFormat format = null;
                    if (!string.IsNullOrWhiteSpace(formatCode))
                    {
                        format = formats.FirstOrDefault(p => p.Code == formatCode);
                        if (format == null)
                        {
                            importer.LogError($"Invalid EFT format code \"{format}\".");
                        }
                    }

                    eftInfo.EFTFormatId = format?.Id;
                    
                    switch(statusCode)
                    {
                        case "I": eftInfo.Status = PaymentMethodStatus.Inactive; break;
                        case "P": eftInfo.Status = PaymentMethodStatus.PrenoteRequired; break;
                        case "S": eftInfo.Status = PaymentMethodStatus.PrenoteSent; break;
                        default:
                            eftInfo.Status = PaymentMethodStatus.Active; break;
                    }
                                        
                    eftInfo.AssignPrimaryKey();

                    outputFile.AddRow(eftInfo);

                    legacyIdFile.AddRow(new LegacyToID(legacyId, eftInfo.Id));

                    if (count % 1000 == 0)
                    {
                        joinOutputFile.Flush();
                        outputFile.Flush();
                        importer.LogDebug($"{count} Loaded");
                    }
                }

                joinOutputFile.Dispose();
                outputFile.Dispose();
                legacyIdFile.Dispose();
            }

            context.Dispose();
            */
        }

    }



}
