using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Conversion.Statics;
using DDI.Data;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.Security;
using DDI.Shared.Models;
using DDI.Business.Core;
using DDI.Shared;

namespace DDI.Conversion.Core
{    
    internal class SettingsLoader : ConversionBase
    {
        public enum ConversionMethod
        {
            Users = 990001,
            Codes,
            NoteCategories,
            Configuration,
        }

        // nacodes.record-cd sets - these are the ones that are being imported here.        
        private const int NOTE_CODE_SET = 17;
        private const int NOTE_TOPIC_SET = 43;
        private const int NOTE_CONTACT_METHOD_SET = 44;

        private string _crmDirectory, _ddiDirectory;

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            _crmDirectory = Path.Combine(baseDirectory, DirectoryName.CRM);
            _ddiDirectory = Path.Combine(baseDirectory, DirectoryName.DDI);

            RunConversion(ConversionMethod.Users, () => LoadUsers(InputFile.DDI_User));
            RunConversion(ConversionMethod.Codes, () => LoadLegacyCodes(InputFile.CRM_NACodes));
            RunConversion(ConversionMethod.NoteCategories, () => LoadNoteCategories(InputFile.CRM_NoteCategory));
            RunConversion(ConversionMethod.Configuration, () => LoadConfiguration(InputFile.DDI_Settings));

        }

        private void LoadLegacyCodes(string filename)
        {
            using (var context = new DomainContext())
            {
                int createdByField = 9;
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
                        IAuditableEntity entity;

                        switch (codeSet)
                        {
                            case NOTE_CODE_SET:
                                entity = new NoteCode { Code = code, Name = description, IsActive = isActive };
                                ImportCreatedModifiedInfo(entity, importer, createdByField);
                                context.NoteCodes.AddOrUpdate(p => p.Code, (NoteCode)entity);
                                break;
                            case NOTE_TOPIC_SET:
                                entity = new NoteTopic { Code = code, Name = description, IsActive = isActive };
                                ImportCreatedModifiedInfo(entity, importer, createdByField);
                                context.NoteTopics.AddOrUpdate(p => p.Code, (NoteTopic)entity);
                                break;
                            case NOTE_CONTACT_METHOD_SET:
                                entity = new NoteContactMethod { Code = code, Name = description, IsActive = isActive };
                                ImportCreatedModifiedInfo(entity, importer, createdByField);
                                context.NoteContactMethods.AddOrUpdate(prop => prop.Code, (NoteContactMethod)entity);
                                break;
                        }
                    }
                }
                context.SaveChanges();
            }
        }

        private void LoadNoteCategories(string filename)
        {
            using (var context = new DomainContext())
            {
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

                        ImportCreatedModifiedInfo(noteCategory, importer, 3);
                        context.NoteCategories.AddOrUpdate(p => p.Label, noteCategory);

                        count++;
                    }
                    context.SaveChanges();
                }
            }
        }

        private void LoadUsers(string filename)
        {
            using (var context = new DomainContext())
            {

                using (var importer = CreateFileImporter(_ddiDirectory, filename, typeof(ConversionMethod)))
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
                        string createdBy = importer.GetString(3);
                        DateTime? createdOn = importer.GetDateTime(4);
                        string modifiedBy = importer.GetString(5);
                        DateTime? modifiedOn = importer.GetDateTime(6);
                        DateTime? deletedOn = importer.GetDateTime(7);
                        string email = importer.GetString(8);
                        DateTime? lastLogin = importer.GetDateTime(9);

                        User user = new User()
                        {
                            UserName = userName,
                            Email = email,
                            FullName = fullName,
                            IsActive = isActive,
                            LastLogin = lastLogin,
                            CreatedBy = createdBy,
                            CreatedOn = createdOn,
                            LastModifiedBy = modifiedBy
                        };
                        context.Users.AddOrUpdate(p => p.UserName, user);

                        count++;
                    }
                    context.SaveChanges();
                }
            }
        }

        private void LoadConfiguration(string filename)
        {
            var bl = new ConfigurationLogic();

            IUnitOfWork uow = bl.UnitOfWork;

            using (var importer = CreateFileImporter(_ddiDirectory, filename, typeof(ConversionMethod)))
            {
                while (importer.GetNextRow())
                {
                    CoreConfiguration config = bl.GetConfiguration<CoreConfiguration>();

                    config.ClientName = importer.GetString(0);
                    config.ClientCode = importer.GetString(1);

                    bool useBusinessDate = importer.GetBool(3);
                    if (useBusinessDate)
                    {
                        config.BusinessDate = importer.GetDate(4);
                    }
                    else
                    {
                        config.BusinessDate = null;
                    }

                    config.BusinessUnitLabel = importer.GetString(6);

                    bl.SaveConfiguration(config);
                    break;
                }
            }
        }

    }
}
