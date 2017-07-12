using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Conversion.Statics;
using DDI.Data;
using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Client.Security;

namespace DDI.Conversion.GL
{    
    /// <summary>
    /// Common logic for GL conversion classes.
    /// </summary>
    internal abstract class GLConversionBase : ConversionBase
    {
        private string _glDirectory;
        protected string GLDirectory => _glDirectory;

        private string _outputDirectory;
        protected string OutputDirectory => _outputDirectory;

        private Dictionary<int, Guid> _ledgerIds;
        protected Dictionary<int, Guid> LedgerIds => _ledgerIds;

        private Dictionary<int, Guid> _businessUnitIds;
        protected Dictionary<int, Guid> BusinessUnitIds => _businessUnitIds;

        private Dictionary<string, Guid> _fiscalYearIds;
        protected Dictionary<string, Guid> FiscalYearIds => _fiscalYearIds;

        private Dictionary<int, Guid> _ledgerAccountIds;
        protected Dictionary<int, Guid> LedgerAccountIds => _ledgerAccountIds;

        private Dictionary<string, Guid> _ledgerAccountYearIds;
        protected Dictionary<string, Guid> LedgerAccountYearIds => _ledgerAccountYearIds;

        private Dictionary<string, Guid> _segmentIds;
        protected Dictionary<string, Guid> SegmentIds => _segmentIds;

        protected void Initialize(string baseDirectory)
        {
            _glDirectory = Path.Combine(baseDirectory, DirectoryName.GL);
            _outputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.GL);

            // Make sure the IS Payload directory exists.
            Directory.CreateDirectory(_outputDirectory);
        }

        /// <summary>
        /// Load legacy business unit IDs into a dictionary.
        /// </summary>
        protected void LoadBusinessUnitIds()
        {
            if (_businessUnitIds == null)
            {
                _businessUnitIds = LoadIntLegacyIds(_outputDirectory, OutputFile.GL_BusinessUnitIdMappingFile);
            }
        }

        /// <summary>
        /// Load legacy ledger IDs into a dictionary.
        /// </summary>
        protected void LoadLedgerIds()
        {
            if (_ledgerIds == null)
            {
                _ledgerIds = LoadIntLegacyIds(_outputDirectory, OutputFile.GL_LedgerIdMappingFile);
            }
        }

        /// <summary>
        /// Load legacy fiscal year IDs into a dictionary.
        /// </summary>
        protected void LoadFiscalYearIds()
        {
            if (_fiscalYearIds == null)
            {
                _fiscalYearIds = LoadLegacyIds(_outputDirectory, OutputFile.GL_FiscalYearIdMappingFile);
            }
        }


        /// <summary>
        /// Load legacy ledger account IDs into a dictionary.
        /// </summary>
        protected void LoadLedgerAccountIds()
        {
            if (_ledgerAccountIds == null)
            {
                _ledgerAccountIds = LoadIntLegacyIds(_outputDirectory, OutputFile.GL_LedgerAccountIdMappingFile);
            }
        }

        protected void LoadLedgerAccountYearIds()
        {
            if (_ledgerAccountYearIds == null)
            {
                _ledgerAccountYearIds = LoadLegacyIds(OutputDirectory, OutputFile.GL_LedgerAccountYearIdMappingFile);
            }
        }

        /// <summary>
        /// Load legacy segment IDs into a dictionary.
        /// </summary>
        protected void LoadSegmentIds()
        {
            if (_segmentIds == null)
            {
                _segmentIds = LoadLegacyIds(_outputDirectory, OutputFile.GL_SegmentIdMappingFile);
            }
        }

        protected Guid? GetFiscalYearId(FileImport importer, int column)
        {
            Guid? ledgerId;
            return GetFiscalYearId(importer, column, false, out ledgerId);
        }

        protected Guid? GetFiscalYearId(FileImport importer, int column, bool ignoreNullYear, out Guid? ledgerId)
        {
            if (_ledgerIds != null)
            {
                ledgerId = GetLedgerId(importer, column);
                if (ledgerId == null)
                {
                    return null;
                }
            }
            else
            {
                ledgerId = null;
            }

            string code = importer.GetString(column);
            string yearName = importer.GetString(column + 1);

            if (ignoreNullYear && (string.IsNullOrWhiteSpace(yearName) || yearName == "0"))
            {
                return null;
            }

            string legacyKey = $"{code},{yearName}";
            Guid fiscalYearId;
            if (_fiscalYearIds.TryGetValue(legacyKey, out fiscalYearId))
            {
                return fiscalYearId;
            }

            importer.LogError($"Invalid year \"{yearName}\" for cid \"{code}\".");
            return null;
        }

        protected Guid? GetLedgerId(FileImport importer, int column)
        {
            Guid? ledgerId = null;

            // Legacy company ID
            string code = importer.GetString(column);
            if (string.IsNullOrWhiteSpace(code))
            {
                return null;
            }

            if (_ledgerIds != null)
            {
                int cid = 0;
                Guid id;
                if (!int.TryParse(code, out cid) || !_ledgerIds.TryGetValue(cid, out id))
                {
                    importer.LogError($"Invalid legacy company ID \"{code}\"");
                    return null;
                }

                ledgerId = id;
            }

            return ledgerId;
        }

        protected Guid? GetLedgerAccountId(FileImport importer, int column)
        {
            Guid? accountId = null;

            // Legacy account key
            int legacyKey = importer.GetInt(column);
            if (legacyKey == 0)
            {
                return null;
            }

            if (_ledgerAccountIds != null)
            {
                Guid id;
                if (!_ledgerAccountIds.TryGetValue(legacyKey, out id))
                {
                    importer.LogError($"Invalid legacy GL account ID \"{legacyKey}\"");
                    return null;
                }

                accountId = id;
            }

            return accountId;
        }

    }
}
