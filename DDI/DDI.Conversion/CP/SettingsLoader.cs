using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Core;
using DDI.Business.CRM;
using DDI.Conversion.Statics;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Enums.Common;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Common;
using DDI.Shared.Extensions;
using DDI.Shared.Models.Client.CP;

namespace DDI.Conversion.CP
{    
    internal class SettingsLoader : ConversionBase
    {
        public enum ConversionMethod
        {
            Codes = 40001,
        }

        // nacodes.record-cd sets - these are the ones that are being imported here.
        private const int EFT_FORMAT_SET = 102;

        private string _glDirectory;
        private string _cpDirectory;

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            _glDirectory = Path.Combine(baseDirectory, DirectoryName.GL);
            _cpDirectory = Path.Combine(baseDirectory, DirectoryName.CP);

            RunConversion(ConversionMethod.Codes, () => LoadLegacyCodes(InputFile.GL_FWCodes));
        }

        private void LoadLegacyCodes(string filename)
        {
            DomainContext context = new DomainContext();
            using (var importer = CreateFileImporter(_glDirectory, filename, typeof(ConversionMethod)))
            {
                while (importer.GetNextRow())
                {
                    int codeSet = importer.GetInt(0);
                    string code = importer.GetString(1);
                    string description = importer.GetString(2);
                    int int1 = importer.GetInt(3);
                    int int2 = importer.GetInt(4);
                    string text1 = importer.GetString(5);
                    string text2 = importer.GetString(6);

                    switch (codeSet)
                    {
                        case EFT_FORMAT_SET:
                            context.CP_EFTFormats.AddOrUpdate(
                                prop => prop.Code,
                                new EFTFormat { Code = code, Name = description, IsActive = true });
                            break;
                    }
                }
            }
            context.SaveChanges();
        }


    }
}
