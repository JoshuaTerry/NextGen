using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Conversion.Statics;
using DDI.Data;
using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.GL;

namespace DDI.Conversion.GL
{    
    internal class SettingsLoader : ConversionBase
    {
        public enum ConversionMethod
        {
            Codes = 70001,
            BusinessUnits = 70002,
        }

        private string _glDirectory;        

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            _glDirectory = Path.Combine(baseDirectory, DirectoryName.GL);

            RunConversion(ConversionMethod.Codes, () => LoadLegacyCodes(InputFile.GL_FWCodes));
            RunConversion(ConversionMethod.BusinessUnits, () => LoadBusinessUnits(InputFile.GL_BusinessUnits));
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

                    /* THIS CODE CAN BE UNCOMMENTED & MODIFIED IF WE NEED TO LOAD ANY LEGACY CODE VALUES...
                     * 
                    switch (codeSet)
                    {
                        case EFT_FORMAT_SET:
                            context.CP_EFTFormats.AddOrUpdate(
                                prop => prop.Code,
                                new EFTFormat { Code = code, Name = description, IsActive = true });
                            break;
                    }
                    */
                }
            }
            context.SaveChanges();
        } // LoadLegacyCodes

        private void LoadBusinessUnits(string filename)
        {
            DomainContext context = new DomainContext();

            using (var importer = CreateFileImporter(_glDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
                {
                    string code = importer.GetString(0);
                    if (string.IsNullOrWhiteSpace(code))
                    {
                        continue;
                    }
                    
                    string name = importer.GetString(1);
                    BusinessUnitType type = importer.GetEnum<BusinessUnitType>(2);

                    context.GL_BusinessUnits.AddOrUpdate(p => p.Code,
                        new BusinessUnit()
                        {
                            Code = code,
                            Name = name,
                            BusinessUnitType = type
                        });
                    count++;
                }

                context.SaveChanges();
            }
        }



    }
}
