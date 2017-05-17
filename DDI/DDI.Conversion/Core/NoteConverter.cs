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
    internal class NoteConverter
    {        
        private char[] _commaDelimiter = { ',' };
        private Dictionary<int, Guid> _constituentIds;

        public void ConvertNotes(Func<FileImport> noteImporter, string outputFileNameSuffix, Dictionary<string, Guid> entityIds, bool append)
        {
            LoadConstituentIds();

            using (var context = new DomainContext())
            {
                // For future note conversions, will need to load legacy Ids for parent entities here, based on entityType.

                context.NoteCategories.Load();
                context.NoteContactMethods.Load();
                context.NoteTopics.Load();
                context.NoteCodes.Load();
                context.Users.Load();

                // Load other tables
                var noteCategories = context.NoteCategories.Local;
                var noteContactMethods = context.NoteContactMethods.Local;
                var noteTopics = context.NoteTopics.Local;
                var noteCodes = context.NoteCodes.Local;
                var users = context.Users.Local;

                string _outputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.Core);
                string noteFilename = CreateOutputFilename(OutputFile.Core_NoteFile, outputFileNameSuffix);
                string legacyIdFilename = CreateOutputFilename(OutputFile.Core_NoteIdMappingFile, outputFileNameSuffix);
                string topicFilename = CreateOutputFilename(OutputFile.Core_NoteTopicFile, outputFileNameSuffix);

                using (var importer = noteImporter())
                {
                    Guid id;

                    var outputFile = new FileExport<Note>(Path.Combine(_outputDirectory, noteFilename), append);
                    var legacyIdFile = new FileExport<ConversionBase.LegacyToID>(Path.Combine(_outputDirectory, legacyIdFilename), append, true);
                    var topicFile = new FileExport<ConversionBase.JoinRow>(Path.Combine(_outputDirectory, topicFilename), append); // Join table created by EF

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

                        string entityType = importer.GetString(1);
                        string entityKey = importer.GetString(2);
                        int sequenceNum = importer.GetInt(3);
                        string title = importer.GetString(4);
                        string text = importer.GetString(5).Replace(@"\#", "#");
                        DateTime? alertStart = importer.GetDate(6);
                        DateTime? alertEnd = importer.GetDate(7);
                        DateTime? contactDt = importer.GetDate(8);
                        string categoryCode = importer.GetString(9);
                        string noteCodeCode = importer.GetString(10);
                        string userResponsible = importer.GetString(11);
                        int primaryContactNum = importer.GetInt(12);
                        string contactMethodCode = importer.GetString(13);
                        string topicCodes = importer.GetString(14);
                        DateTime? deleteDt = importer.GetDate(15);

                        if (entityIds != null)
                        {
                            if (!entityIds.TryGetValue(entityKey, out id))
                            {
                                importer.LogError($"{entityType} Note with legacy id {legacyId} has invalid parent entity key {entityKey}. ");
                                continue;
                            }
                        }
                        else
                        {
                            // If no entity Ids were provided, assume Entity key is a constituent PIN
                            int pin;
                            if (!int.TryParse(entityKey, out pin))
                            {
                                continue;
                            }
                            if (!_constituentIds.TryGetValue(pin, out id))
                            {
                                importer.LogError($"{entityType} Note with legacy id {legacyId} has invalid parent entity key {entityKey}. ");
                                continue;
                            }
                        }

                        Note note = new Note();
                        note.EntityType = entityType;
                        note.ParentEntityId = id;

                        note.AssignPrimaryKey();
                        legacyIdFile.AddRow(new ConversionBase.LegacyToID(legacyId, note.Id));

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
                                    topicFile.AddRow(new ConversionBase.JoinRow(topic.Id, note.Id));
                                }
                            }

                        }

                        outputFile.AddRow(note);

                    }

                    outputFile.Dispose();
                    topicFile.Dispose();
                    legacyIdFile.Dispose();
                }
            }
        }

        private void LoadConstituentIds()
        {
            string crmOutputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.CRM);
            _constituentIds = new Dictionary<int, Guid>();

            using (var importer = new FileImport(Path.Combine(crmOutputDirectory, OutputFile.CRM_ConstituentIdMappingFile), string.Empty))
            {
                while (importer.GetNextRow())
                {
                    int legacyKey = importer.GetInt(0);
                    _constituentIds[legacyKey] = importer.GetGuid(1);
                }
            }
        }

        private string CreateOutputFilename(string filename, string suffix)
        {
            if (!string.IsNullOrWhiteSpace(suffix))
            {
                string extension = Path.GetExtension(filename);
                filename = filename.Replace(extension, "_" + suffix) + extension;
            }

            return filename;
        }


    }
}
