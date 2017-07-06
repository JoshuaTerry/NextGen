using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DDI.Conversion;
using DDI.Conversion.Statics;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using System.IO.Compression;
using DDI.Shared.Extensions;

namespace DDI.Conversion.Core
{
    /// <summary>
    /// Load Attachments
    /// </summary>
    internal class AttachmentConverter
    {
        private Dictionary<int, Guid> _constituentIds = null;

        public void ConvertAttachments(Func<FileImport> attachmentImporter, string outputFileNameSuffix, Dictionary<string, Guid> entityIds, bool append)
        {
            string outputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.Core);
            string attachmentFilename = ConversionBase.CreateOutputFilename(OutputFile.Core_AttachmentFile, outputFileNameSuffix);
            string legacyIdFilename = ConversionBase.CreateOutputFilename(OutputFile.Core_AttachmentMappingFile, outputFileNameSuffix);
            string noteIdFilename = ConversionBase.CreateOutputFilename(OutputFile.Core_NoteIdMappingFile, outputFileNameSuffix);
            string fileStorageIdFilename = OutputFile.Core_FileStorageMappingFile;

            Dictionary<int,Guid> noteIds = ConversionBase.LoadIntLegacyIds(outputDirectory, noteIdFilename);
            Dictionary<int, Guid> fileStorageIds = ConversionBase.LoadIntLegacyIds(outputDirectory, fileStorageIdFilename);

            if (entityIds == null)
            {
                LoadConstituentIds();
            }

            using (var importer = attachmentImporter())
            {
                Guid id;
                Guid noteId;
                Guid parentId;

                var outputFile = new FileExport<Attachment>(Path.Combine(outputDirectory, attachmentFilename), append);
                var legacyIdFile = new FileExport<ConversionBase.LegacyToID>(Path.Combine(outputDirectory, legacyIdFilename), append, true);

                if (!append)
                {
                    outputFile.AddHeaderRow();
                }

                int count = 0;

                while (importer.GetNextRow())
                {
                    count++;

                    int legacyId = importer.GetInt(0);
                    if (legacyId == 0)
                    {
                        continue;
                    }

                    if (!fileStorageIds.TryGetValue(legacyId, out id))
                    {
                        importer.LogError($"Invalid attachment legacy Id {legacyId}.");
                        continue;
                    }

                    string entityType = importer.GetString(1);
                    string entityKey = importer.GetString(2);

                    if (entityIds != null)
                    {
                        if (!entityIds.TryGetValue(entityKey, out parentId))
                        {
                            importer.LogError($"{entityType} Attachment with legacy id {legacyId} has invalid parent entity key {entityKey}. ");
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
                        if (!_constituentIds.TryGetValue(pin, out parentId))
                        {
                            importer.LogError($"{entityType} Attachment with legacy id {legacyId} has invalid parent entity key {entityKey}. ");
                            continue;
                        }
                    }

                    var attachment = new Attachment();
                    attachment.EntityType = entityType;
                    attachment.ParentEntityId = parentId;
                    attachment.FileId = id;
                    attachment.Title = importer.GetString(4, 256);
                    attachment.CreatedBy = importer.GetString(5, 64);
                    attachment.CreatedOn = importer.GetDateTime(6);

                    attachment.AssignPrimaryKey();
                    legacyIdFile.AddRow(new ConversionBase.LegacyToID(legacyId, attachment.Id));
                    
                    // Linkage to note
                    legacyId = importer.GetInt(3);
                    if (legacyId > 0)
                    {
                        if (!noteIds.TryGetValue(legacyId, out noteId))
                        {
                            importer.LogError($"Invalid note legacy Id {legacyId}.");
                        }
                        attachment.NoteId = noteId;
                    }

                    outputFile.AddRow(attachment);

                }
                outputFile.Dispose();
                legacyIdFile.Dispose();
            }
        }

        public void ConvertFileStorage(Func<FileImport> fileStorageImporter, bool append)
        {
            string outputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.Core);
            var legacyIdFile = new FileExport<ConversionBase.LegacyToID>(Path.Combine(outputDirectory, OutputFile.Core_FileStorageMappingFile), append, true);
            int count = 0;
            
            IUnitOfWork uow = Factory.CreateUnitOfWork();
            using (var importer = fileStorageImporter())
            {
                string zipFileName = importer.FileName;
                int pos = zipFileName.LastIndexOf('.');
                if (pos >= 0)
                {
                    zipFileName = zipFileName.Substring(0, pos);
                }
                zipFileName += ".zip";

                using (ZipArchive zipFile = ZipFile.OpenRead(zipFileName))
                {
                    while (importer.GetNextRow())
                    {
                        int legacyId = importer.GetInt(0);
                        if (legacyId == 0)
                        {
                            continue;
                        }

                        FileStorage file = new FileStorage();
                        file.Name = importer.GetString(1, 256);
                        file.Extension = importer.GetString(2, 8);
                        file.Size = importer.GetInt64(3);
                        file.CreatedBy = importer.GetString(4, 64);
                        file.CreatedOn = importer.GetDateTime(5);

                        string entryName = $"{legacyId}.dat";
                        var entry = zipFile.GetEntry(entryName);
                        if (entry == null)
                        {
                            importer.LogError($"Zip file {zipFileName} has no entry for {entryName}.");
                            continue;
                        }

                        using (var stream = entry.Open())
                        {
                            file.Data = stream.ReadAllBytes();
                        }
                        file.AssignPrimaryKey();
                        uow.Insert(file);

                        legacyIdFile.AddRow(new ConversionBase.LegacyToID(legacyId, file.Id));

                        if (++count % 100 == 0)
                        {
                            legacyIdFile.Flush();
                            uow.SaveChanges();
                            uow.Dispose();
                            uow = Factory.CreateUnitOfWork();
                        }
                    }

                } // using zipfile
            } // using importer

            legacyIdFile.Flush();
            legacyIdFile.Dispose();
            uow.SaveChanges();
            uow.Dispose();
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
    }
}
