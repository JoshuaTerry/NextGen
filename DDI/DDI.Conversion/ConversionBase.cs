using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Helpers;

namespace DDI.Conversion
{
    /// <summary>
    /// Base class for conversion classes, provides commonly used logic.
    /// </summary>
    internal abstract class ConversionBase
    {

        #region Public Abstract Methods
        
        /// <summary>
        /// Execute a set of conversion methods.
        /// </summary>
        public abstract void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods);

        #endregion


        #region Protected Properties

        /// <summary>
        /// Set of conversion methods to be run.  If empty, all conversion methods are run in order.
        /// </summary>
        protected IEnumerable<ConversionMethodArgs> MethodsToRun { get; set; }

        /// <summary>
        /// ConversionMethodArgs instance for the currently running method.
        /// </summary>
        protected ConversionMethodArgs MethodArgs { get; set; }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Load a ConversionMethodArgs instance into MethodArgs, then run a conversion method.
        /// </summary>
        /// <param name="method">An enum value that corresponds to the method to be run.</param>
        /// <param name="action">An Action that invokes the conversion method.</param>
        protected void RunConversion(object method, Action action)
        {
            var methodNum = (int)method;
            if (MethodsToRun == null)
            {
                MethodArgs = new ConversionMethodArgs(methodNum);
                action.Invoke();
            }
            else
            {
                var methodArgs = MethodsToRun.FirstOrDefault(p => p.MethodNum == methodNum);
                if (methodArgs != null && methodArgs.Skip == false)
                {
                    MethodArgs = methodArgs;
                    action.Invoke();

                }
            }
        }

        /// <summary>
        /// Create a FileImport instance.
        /// </summary>
        /// <param name="baseDirectory">Directory where input files are located.</param>
        /// <param name="defaultFilename">Default input filename, used if no filename was specified in the ConversionMethodArgs object.</param>
        /// <param name="enumType"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        protected FileImport CreateFileImporter(string baseDirectory, string defaultFilename, Type enumType, char delimiter = ',')
        {
            string filename = null;

            // Retrieve next filename from MethodArgs.
            if (MethodArgs.Filenames != null && MethodArgs.FileNum < MethodArgs.Filenames.Length)
            {
                filename = MethodArgs.Filenames[MethodArgs.FileNum++];
            }

            // If no filename provided via ConversionMethodArgs, use the default.
            if (string.IsNullOrWhiteSpace(filename))
            {
                filename = defaultFilename;
            }

            filename = Path.Combine(baseDirectory, filename);

            // The logname is based on the Enum's name.
            string logName = Enum.GetName(enumType, Enum.ToObject(enumType, MethodArgs.MethodNum));

            // Create the FileImport and return it.
            return new FileImport(filename, logName, delimiter);
        }

        /// <summary>
        /// Load a set of legacy Id's and EF Id's that were previously saved to a file.
        /// </summary>
        protected Dictionary<string,Guid> LoadLegacyIds(string baseDirectory, string filename) 
        {
            Dictionary<string, Guid> legacyIds = new Dictionary<string, Guid>();

            using (var importer = new FileImport(Path.Combine(baseDirectory, filename), string.Empty))
            {
                while (importer.GetNextRow())
                {
                    string legacyKey = importer.GetString(0);                    
                    if (!string.IsNullOrWhiteSpace(legacyKey))
                    {
                        legacyIds[legacyKey] = importer.GetGuid(1);
                    }
                }
            }
            return legacyIds;
        }

        /// <summary>
        /// Load a set of legacy Id's and EF Id's that were previously saved to a file.
        /// </summary>
        protected Dictionary<int, Guid> LoadIntLegacyIds(string baseDirectory, string filename) 
        {
            Dictionary<int, Guid> legacyIds = new Dictionary<int, Guid>();

            using (var importer = new FileImport(Path.Combine(baseDirectory, filename), string.Empty))
            {
                while (importer.GetNextRow())
                {
                    int legacyKey = importer.GetInt(0);
                    legacyIds[legacyKey] = importer.GetGuid(1);
                }
            }
            return legacyIds;
        }

        #endregion

        #region Internal Classes

        /// <summary>
        /// Class for exporting rows that map a legacy ID (string) to an entity ID (Guid)
        /// </summary>
        protected class LegacyToID
        {
            public string LegacyKey { get; set; }
            public Guid Id { get; set; }

            public LegacyToID(string legacyKey, Guid id)
            {
                LegacyKey = legacyKey;
                Id = id;
            }

            public LegacyToID(int legacyKey, Guid id)
            {
                LegacyKey = legacyKey.ToString();
                Id = id;
            }
        }

        /// <summary>
        /// Class for exporting rows that join a parent ID to a child ID
        /// </summary>
        protected class JoinRow
        {
            public Guid LeftId { get; set; }
            public Guid RightId { get; set; }

            public JoinRow(Guid leftId, Guid rightId)
            {
                LeftId = leftId;
                RightId = rightId;
            }

        }

        #endregion

    }
}
