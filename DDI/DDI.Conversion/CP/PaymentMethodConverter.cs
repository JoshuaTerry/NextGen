using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.CRM;
using DDI.Conversion.Statics;
using DDI.Data;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Common;
using DDI.Shared.Extensions;
using DDI.Shared.Models.Client.CP;

namespace DDI.Conversion.CP
{

    /// <summary>
    /// OpenEdge to SSIS Data Conversion for the payment methods and linkage to constituents.
    /// </summary>
    internal class PaymentMethodConverter : ConversionBase
    {

        public enum ConversionMethod
        {
            PaymentMethods = 41001,
        }

        private string _cpDirectory;
        private string _crmOutputDirectory;
        private string _cpOutputDirectory;

        private Dictionary<int, Guid> _constituentIds;

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            _cpDirectory = Path.Combine(baseDirectory, DirectoryName.CP);
            _crmOutputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.CRM);
            _cpOutputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.CP);
            _constituentIds = new Dictionary<int, Guid>();
 
            // Make sure the IS Payload directory exists.
            Directory.CreateDirectory(_crmOutputDirectory);

            RunConversion(ConversionMethod.PaymentMethods, () => ConvertPaymentMethods(InputFile.CP_EFTInfo, false));
        }

        /// <summary>
        /// If necessary, load legacy constituent IDs into dictionary.
        /// </summary>
        private void LoadConstituentIds()
        {
            if (_constituentIds.Count == 0)
            {
                _constituentIds = LoadIntLegacyIds(_crmOutputDirectory, OutputFile.ConstituentIdMappingFile);
            }
        }

        private void ConvertPaymentMethods(string filename, bool append)
        {
            DomainContext context = new DomainContext();
            char[] commaDelimiter = { ',' };
            string eftPaymentMethodDiscriminator = typeof(EFTPaymentMethod).Name; // Value that EF would store in PaymentMethod.Discriminator

            // Load the constituent Ids
            LoadConstituentIds();

            // Load the EFT formats
            var formats = LoadEntities(context.EFTFormats);

            using (var importer = CreateFileImporter(_cpDirectory, filename, typeof(ConversionMethod)))
            {
                // This is a join table for PaymentMethodBaseConstituents, created by EF.
                var joinOutputFile = new FileExport<JoinRow>(Path.Combine(_cpOutputDirectory, OutputFile.CP_PaymentMethodConstituentFile), append);
                joinOutputFile.SetColumnNames("PaymentMethodBase_Id", "Constituent_Id");

                // The actual payment method file.
                var outputFile = new FileExport<EFTPaymentMethodWithDiscriminator>(Path.Combine(_cpOutputDirectory, OutputFile.CP_PaymentMethodFile), append);

                // Legacy ID file, to convert from OE id to SQL Id.
                FileExport<LegacyToID> legacyIdFile = new FileExport<LegacyToID>(Path.Combine(_cpOutputDirectory, OutputFile.PaymentMethodIdMappingFile), append, true);
                
                if (!append)
                {
                    joinOutputFile.AddHeaderRow();
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

                    var eftInfo = new EFTPaymentMethodWithDiscriminator();
                    eftInfo.BankAccount = bankAccount;
                    eftInfo.BankName = bankName;
                    eftInfo.RoutingNumber = routingNumber;
                    eftInfo.Description = description;
                    eftInfo.Discriminator = eftPaymentMethodDiscriminator;
                    
                    if (tranCode == "22")
                    {
                        eftInfo.AccountType = Shared.Enums.CP.EFTAccountType.Checking;
                    }
                    else if (tranCode == "32")
                    {
                        eftInfo.AccountType = Shared.Enums.CP.EFTAccountType.Savings;
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
                        case "I": eftInfo.Status = Shared.Enums.CP.PaymentMethodStatus.Inactive; break;
                        case "P": eftInfo.Status = Shared.Enums.CP.PaymentMethodStatus.PrenoteRequired; break;
                        case "S": eftInfo.Status = Shared.Enums.CP.PaymentMethodStatus.PrenoteSent; break;
                        default:
                            eftInfo.Status = Shared.Enums.CP.PaymentMethodStatus.Active; break;
                    }
                                        
                    eftInfo.AssignPrimaryKey();

                    if (constituentId != null)
                    {
                        joinOutputFile.AddRow(new JoinRow(eftInfo.Id, constituentId.Value));
                    }

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
            }
        }

        /// <summary>
        /// Internal class for EFTPaymentMethod with a "Discriminator" column added.
        /// </summary>
        private class EFTPaymentMethodWithDiscriminator : EFTPaymentMethod
        {
            public string Discriminator { get; set; }
        }

    }



}
