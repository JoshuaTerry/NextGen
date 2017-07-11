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
            EFTFormats = 40001,
        }

        private string _cpDirectory;

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            _cpDirectory = Path.Combine(baseDirectory, DirectoryName.CP);

            RunConversion(ConversionMethod.EFTFormats, () => LoadEFTFormats(InputFile.CP_EFTFormat));
        }

        private void LoadEFTFormats(string filename)
        {
            DomainContext context = new DomainContext();
            using (var importer = CreateFileImporter(_cpDirectory, filename, typeof(ConversionMethod)))
            {
                while (importer.GetNextRow())
                {
                    string code = importer.GetString(0);
                    string description = importer.GetString(1);
                    bool isActive = importer.GetBool(2);

                    if (string.IsNullOrWhiteSpace(code))
                    {
                        continue;
                    }

                    context.CP_EFTFormats.AddOrUpdate(
                        prop => prop.Code,
                        new EFTFormat { Code = code, Name = description, IsActive = isActive });
                }
            }
            context.SaveChanges();
        }


    }
}
