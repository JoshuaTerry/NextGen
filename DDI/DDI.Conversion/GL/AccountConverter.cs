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
using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Client.Security;

namespace DDI.Conversion.GL
{    
    internal class AccountConverter : ConversionBase
    {
        public enum ConversionMethod
        {
            Segments = 70100,
        }

        private string _glDirectory;
        private string _outputDirectory;
        private Dictionary<int, Guid> _ledgerIds;

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            _glDirectory = Path.Combine(baseDirectory, DirectoryName.GL);
            _outputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.GL);
            _ledgerIds = new Dictionary<int, Guid>();

            // Make sure the IS Payload directory exists.
            Directory.CreateDirectory(_outputDirectory);

            RunConversion(ConversionMethod.Segments, () => ConvertSegments(InputFile.GL_Segments));
        }

        private void ConvertSegments(string filename)
        {
            DomainContext context = new DomainContext();

            LoadLedgerIds();
            var ledgers = LoadEntities(context.GL_Ledgers, nameof(Ledger.DefaultFiscalYear));

            using (var importer = CreateFileImporter(_glDirectory, filename, typeof(ConversionMethod)))
            {

                var outputFile = new FileExport<Segment>(Path.Combine(_outputDirectory, OutputFile.GL_SegmentFile), false);
                outputFile.AddHeaderRow();

                int count = 1;

                while (importer.GetNextRow())
                {
                    // Legacy company ID
                    string code = importer.GetString(0);
                    if (string.IsNullOrWhiteSpace(code))
                    {
                        continue;
                    }

                    int cid = 0;
                    Guid ledgerId;

                    if (!int.TryParse(code, out cid) || !_ledgerIds.TryGetValue(cid, out ledgerId))
                    {
                        importer.LogError($"Invalid legacy company ID \"{code}\"");
                        continue;
                    }

                    int level = importer.GetInt(1);

                    SegmentLevel slev = context.GL_SegmentLevels.FirstOrDefault(p => p.LedgerId == ledgerId && p.Level == level);
                    if (slev == null)
                    {
                        slev = new SegmentLevel();
                        context.GL_SegmentLevels.Add(slev);
                        slev.LedgerId = ledgerId;
                        slev.Level = level;
                    }

                    slev.Type = importer.GetEnum<SegmentType>(2);
                    slev.Format = importer.GetEnum<SegmentFormat>(3);
                    slev.Length = importer.GetInt(4);
                    slev.IsLinked = importer.GetBool(5);
                    slev.IsCommon = importer.GetBool(6);
                    slev.Name = importer.GetString(7, 40);
                    slev.Abbreviation = importer.GetString(8, 16);
                    slev.Separator = importer.GetString(9, 1);
                    slev.SortOrder = importer.GetInt(10);

                    count++;
                }

                context.SaveChanges();
            }
        }


        /// <summary>
        /// If necessary, load legacy ledger IDs into dictionary.
        /// </summary>
        private void LoadLedgerIds()
        {
            if (_ledgerIds.Count == 0)
            {
                _ledgerIds = LoadIntLegacyIds(_outputDirectory, OutputFile.LedgerIdMappingFile);
            }


        }
    }
}
