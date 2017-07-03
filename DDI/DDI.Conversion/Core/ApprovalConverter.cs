using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Conversion.Statics;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.Security;
using System.Text.RegularExpressions;
using DDI.Data;
using System.Data.Entity;

namespace DDI.Conversion.Core
{
    internal class ApprovalConverter
    {
        public void ConvertApprovals(Func<FileImport> approvalImporter, string outputFileNameSuffix,
            Dictionary<string, Guid> entityids, bool append)
        {

            using (var context = new DomainContext())
            {

                Guid id;

                context.Users.Load();
                var users = context.Users.Local;

                string _outputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.Core);

                // Make sure the IS Payload directory exists.
                Directory.CreateDirectory(_outputDirectory);

                // Create the output filename
                string outputFilename = ConversionBase.CreateOutputFilename(OutputFile.Core_EntityApprovalFile, outputFileNameSuffix);

                // Import the approvals.

                using (var importer = approvalImporter())
                {
                    var outputFile = new FileExport<EntityApproval>(Path.Combine(_outputDirectory, outputFilename), append);
                    if (!append)
                    {
                        outputFile.AddHeaderRow();
                    }


                    while (importer.GetNextRow())
                    {
                        string entityType = importer.GetString(0);

                        string key = importer.GetString(1);
                        if (string.IsNullOrWhiteSpace(entityType) || string.IsNullOrWhiteSpace(key))
                        {
                            continue;
                        }

                        if (!entityids.TryGetValue(key, out id))
                        {
                            importer.LogError($"Invalid parent entity key \"{key}\".");
                            continue;
                        }

                        var approval = new EntityApproval();
                        approval.EntityType = entityType;
                        approval.ParentEntityId = id;
                        approval.ApprovedOn = importer.GetDateTime(2);
                        approval.ApprovedById = GetUserByName(users, importer.GetString(3))?.Id;
                        approval.AssignPrimaryKey();

                        outputFile.AddRow(approval);
                    }

                    outputFile.Dispose();
                }
            }
        }

        private User GetUserByName(IEnumerable<User> users, string userName)
        {
            return users.FirstOrDefault(p => Regex.IsMatch(p.UserName, $"^{userName}(@.*)?$", RegexOptions.IgnoreCase));
        }

    }
}
