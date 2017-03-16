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
using DDI.Shared.Enums;
using DDI.Shared.Models;
using DDI.Business.Helpers;
using DDI.Shared.Models.Client.Security;

namespace DDI.Conversion.Core
{
    internal class NoteConverter : ConversionBase
    {
        public enum ConversionMethod
        {
            Notes_CRM = 991001
        }

        private enum EntityType
        {
            Constituent
        }

        private Dictionary<int, Guid> _constituentIds;
        private string _crmDirectory;
        private string _outputDirectory;
        private string _crmOutputDirectory;
        private char[] _commaDelimiter = { ',' };

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            _crmDirectory = Path.Combine(baseDirectory, DirectoryName.CRM);

            _outputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.Core);
            _crmOutputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.CRM);
            _constituentIds = new Dictionary<int, Guid>();

            // Make sure the IS Payload directory exists.
            Directory.CreateDirectory(_outputDirectory);

            RunConversion(ConversionMethod.Notes_CRM, () => ConvertNotes(InputFile.CRM_MemoConstituent, EntityType.Constituent, LinkedEntityHelper.GetEntityTypeName<Constituent>(), false));
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

        private void ConvertNotes(string filename, EntityType entityType, string entityTypeName, bool append)
        {
            DomainContext context = new DomainContext();
            LoadConstituentIds();

            // For future note conversions, will need to load legacy Ids for parent entities here, based on entityType.

            // Load other tables
            context.NoteCategories.Load();
            context.NoteContactMethods.Load();
            context.NoteTopics.Load();
            context.NoteCodes.Load();
            context.Users.Load();

            var noteCategories = context.NoteCategories.Local;
            var noteContactMethods = context.NoteContactMethods.Local;
            var noteTopics = context.NoteTopics.Local;
            var noteCodes = context.NoteCodes.Local;
            var users = context.Users.Local;

            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                var outputFile = new FileExport<Note>(Path.Combine(_outputDirectory, OutputFile.Core_NoteFile), append);
                var legacyIdFile = new FileExport<LegacyToID>(Path.Combine(_outputDirectory, OutputFile.NoteIdMappingFile), append, true);
                var topicFile = new FileExport<JoinRow>(Path.Combine(_outputDirectory, OutputFile.Core_NoteTopicFile), append); // Join table created by EF
                topicFile.SetColumnNames("NoteTopic_Id", "Note_Id");

                if (!append)
                {
                    outputFile.AddHeaderRow();
                    topicFile.AddHeaderRow();
                }

                int count = 0;

                while (importer.GetNextRow())
                {
                    count++;

                    string legacyId = importer.GetString(0);

                    if (string.IsNullOrWhiteSpace(legacyId) || legacyId.StartsWith("#"))
                    {
                        continue;
                    }

                    string recordCode = importer.GetString(1);
                    string recordId = importer.GetString(2);
                    int sequenceNum = importer.GetInt(3);
                    string title = importer.GetString(4);
                    string text = importer.GetString(5).Replace(@"\#", "#");
                    DateTime? alertStart = importer.GetDateTime(6);
                    DateTime? alertEnd = importer.GetDateTime(7);
                    DateTime? contactDt = importer.GetDateTime(8);
                    string categoryCode = importer.GetString(9);
                    string noteCodeCode = importer.GetString(10);
                    string userResponsible = importer.GetString(11);
                    int primaryContactNum = importer.GetInt(12);
                    string contactMethodCode = importer.GetString(13);
                    string topicCodes = importer.GetString(14);
                    DateTime? deleteDt = importer.GetDateTime(15);
                    
                    Note note = new Note();
                    note.EntityType = entityTypeName;

                    // ParentEntityId
                    note.ParentEntityId = GetParentEntityId(entityType, recordCode, recordId);

                    if (note.ParentEntityId == null)
                    {
                        importer.LogError($"Note with legacy id {legacyId} could not be linked to parent entity using record code {recordCode}, record id {recordId}. ");
                        continue;
                    }

                    // At this point note will be saved despite additional errors.
                    note.AssignPrimaryKey();
                    legacyIdFile.AddRow(new LegacyToID(legacyId, note.Id));

                    note.Title = title;
                    note.Text = text;
                    note.AlertStartDate = alertStart;
                    note.AlertEndDate = alertEnd;
                    note.ContactDate = contactDt;
                    
                    // Note Category
                    if (!string.IsNullOrWhiteSpace(categoryCode))
                    {
                        NoteCategory category = noteCategories.FirstOrDefault(p => p.Label == categoryCode);
                        if (category == null)
                        {
                            importer.LogError($"Invalid note category label {categoryCode} for note legacy Id {legacyId}.");
                        }
                        else
                        {
                            note.CategoryId = category.Id;
                        }
                    }

                    // Note Code
                    if (!string.IsNullOrWhiteSpace(noteCodeCode))
                    {
                        NoteCode noteCode = noteCodes.FirstOrDefault(p => p.Code == noteCodeCode);
                        if (noteCode == null)
                        {
                            importer.LogError($"Invalid note code {noteCodeCode} for note legacy Id {legacyId}.");
                        }
                        else
                        {
                            note.NoteCodeId = noteCode.Id;
                        }
                    }

                    // Contact Method
                    if (!string.IsNullOrWhiteSpace(contactMethodCode))
                    {
                        NoteContactMethod method = noteContactMethods.FirstOrDefault(p => p.Code == contactMethodCode);
                        if (method == null)
                        {
                            importer.LogError($"Invalid note contact method {contactMethodCode} for note legacy Id {legacyId}.");
                        }
                        else
                        {
                            note.ContactMethodId = method.Id;
                        }
                    }

                    // User Responsible
                    if (!string.IsNullOrWhiteSpace(userResponsible))
                    {
                        User user = users.FirstOrDefault(p => p.UserName == userResponsible);
                        if (user == null)
                        {
                            importer.LogError($"Invalid note user responsible {userResponsible} for note legacy Id {legacyId}.");
                        }
                        else
                        {
                            note.UserResponsibleId = user.Id;
                        }
                    }

                    // Primary Contact
                    if (primaryContactNum > 0)
                    {
                        Guid id;
                        if (_constituentIds.TryGetValue(primaryContactNum, out id))
                        {
                            note.PrimaryContactId = id;
                        }
                        else
                        {
                            importer.LogError($"Invalid primary contact PIN {primaryContactNum} for note legacy Id {legacyId}.");
                        }
                    }

                    // Topics
                    if (!string.IsNullOrWhiteSpace(topicCodes))
                    {
                        string[] codeList = topicCodes.Split(_commaDelimiter, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string code in codeList)
                        {
                            NoteTopic topic = noteTopics.FirstOrDefault(p => p.Code == code);
                            if (topic == null)
                            {
                                importer.LogError($"Invalid note topic {code} for note legacy Id {legacyId}.");
                            }
                            else
                            {
                                topicFile.AddRow(new JoinRow(topic.Id, note.Id));
                            }
                        }

                    }

                    outputFile.AddRow(note);

                    if (count % 1000 == 0)
                    {
                        importer.LogDebug($"{count} Loaded");

                        outputFile.Flush();
                        topicFile.Flush();
                        legacyIdFile.Flush();
                    }

                }

                outputFile.Dispose();
                topicFile.Dispose();
                legacyIdFile.Dispose();
            }
        }

        private Guid? GetParentEntityId(EntityType entityType, string recordCode, string recordId)
        {
            Guid parentEntity;
            Guid? result = null;
            recordCode = recordCode.ToUpper();

            switch (entityType)
            {
                case EntityType.Constituent:
                    {
                        if (recordCode == "NA/NA" || recordCode == "NA/AC")
                        {
                            int constituentNum;
                            if (int.TryParse(recordId, out constituentNum) &&
                                _constituentIds.TryGetValue(constituentNum, out parentEntity))
                            {
                                result = parentEntity;
                            }
                        }
                        break;
                    }
            }

            return result;
        }

    }
}
