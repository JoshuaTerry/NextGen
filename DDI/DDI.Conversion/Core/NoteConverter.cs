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

        private const string ENTITY_CONSTITUENTS = "CRM.Constituent";

        private Dictionary<int, Guid> _constituentIds;
        private string _crmDirectory;
        private string _outputDirectory;

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            _crmDirectory = Path.Combine(baseDirectory, DirectoryName.CRM);
            _outputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.Core);
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
                _constituentIds = LoadIntLegacyIds(_outputDirectory, OutputFile.ConstituentIdMappingFile);
            }
        }

        private void ConvertNotes(string filename, EntityType entityType, string entityTypeName, bool append)
        {
            DomainContext context = new DomainContext();

            switch(entityType)
            {
                case EntityType.Constituent:
                    LoadConstituentIds();
                    break;
                default:
                    return;
            }

            // Load other tables
            context.NoteCategories.Load();
            context.NoteContactMethods.Load();
            context.NoteTopics.Load();
            context.NoteCodes.Load();

            var noteCategories = context.NoteCategories.Local;
            var noteContactMethods = context.NoteContactMethods.Local;
            var noteTopics = context.NoteTopics.Local;
            var noteCodes = context.NoteCodes.Local;

            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                var outputFile = new FileExport<Note>(Path.Combine(_outputDirectory, OutputFile.Core_NoteFile), append);
                if (!append)
                {
                    outputFile.AddHeaderRow();
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
                    string text = importer.GetString(5);
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
                    Guid parentEntity;

                    switch (entityType)
                    {
                        case EntityType.Constituent:
                            {
                                int constituentNum;
                                if (int.TryParse(recordId, out constituentNum) &&
                                    _constituentIds.TryGetValue(constituentNum, out parentEntity))
                                {
                                    note.ParentEntityId = parentEntity;
                                }
                                break;
                            }                            
                    }

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
                            //note.NotecodeId = noteCode.Id;
                        }
                    }

                    // Contact Method
                    if (!string.IsNullOrWhiteSpace(contactMethodCode))
                    {
                        NoteContactMethod method = noteContactMethods.FirstOrDefault(p => p.Code == contactMethodCode);
                        if (method == null)
                        {
                            importer.LogError($"Invalid note cpntact method {contactMethodCode} for note legacy Id {legacyId}.");
                        }
                        else
                        {
                            note.ContactMethodId = method.Id;
                        }
                    }

                }
            }
        }
    }
}
