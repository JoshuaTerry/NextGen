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

namespace DDI.Conversion.Core
{    
    internal class SettingsLoader : ConversionBase
    {
        public enum ConversionMethod
        {
            Users = 990001,
            Codes = 990002,
            NoteCategories
        }

        // nacodes.record-cd sets - these are the ones that are being imported here.        
        private const int NOTE_CONTACT_METHOD_SET = 44;
        private const int NOTE_TOPIC_SET = 43;

        private string _crmDirectory, _ddiDirectory;

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            _crmDirectory = Path.Combine(baseDirectory, DirectoryName.CRM);
            _ddiDirectory = Path.Combine(baseDirectory, DirectoryName.DDI);

            RunConversion(ConversionMethod.Users, () => LoadUsers(InputFile.DDI_User));
            RunConversion(ConversionMethod.Codes, () => LoadLegacyCodes(InputFile.CRM_NACodes));
        }

        private void LoadLegacyCodes(string filename)
        {
            DomainContext context = new DomainContext();
            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
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
                    bool isActive = importer.GetBool(8);


                    switch (codeSet)
                    {
                        case NOTE_TOPIC_SET:
                            context.NoteTopics.AddOrUpdate(
                                prop => prop.Code,
                                new NoteTopic { Code = code, Name = description, IsActive = isActive });
                            break;
                        case NOTE_CONTACT_METHOD_SET:
                            context.NoteContactMethods.AddOrUpdate(
                                prop => prop.Code,
                                new NoteContactMethod { Code = code, Name = description, IsActive = isActive });
                            break;
                    }
                }
            }
            context.SaveChanges();
        }

        private void LoadNoteCategories(string filename)
        {
            DomainContext context = new DomainContext();
            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
                {
                    string screenLabel = importer.GetString(0);
                    string description = importer.GetString(1);

                    if (string.IsNullOrWhiteSpace(screenLabel))
                    {
                        continue;
                    }

                    var noteCategory = new NoteCategory()
                    {
                        Label = screenLabel,
                        Name = description
                    };

                    context.NoteCategories.AddOrUpdate(p => p.Label, noteCategory);

                    count++;
                }
                context.SaveChanges();
            }
        }

        private void LoadUsers(string filename)
        {

            DomainContext context = new DomainContext();
            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
                {
                    string userName = importer.GetString(0);

                    if (string.IsNullOrWhiteSpace(userName) || userName.StartsWith("#"))
                    {
                        continue;
                    }

                    string fullName = importer.GetString(1);
                    bool isActive = importer.GetBool(2);
                    DateTime? deletedOn = importer.GetDateTime(7);
                    string email = importer.GetString(8);
                    DateTime? lastLogin = importer.GetDateTime(9);


                    /*
                    var noteCategory = new NoteCategory()
                    {
                        Label = screenLabel,
                        Name = description
                    };

                    context.NoteCategories.AddOrUpdate(p => p.Label, noteCategory);
                    */

                    count++;
                }
                context.SaveChanges();
            }
        }    
    }
}
