using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.Security;

namespace DDI.Conversion
{
    /// <summary>
    /// Base class for conversion classes, provides commonly used logic.
    /// </summary>
    internal abstract class ConversionBase
    {
        #region Private Fields
        private IRepository<User> _userRepository = null;

        #endregion 

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

        protected IRepository<User> UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new CachedRepository<User>();
                }
                return _userRepository;
            }
        }

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

        /// <summary>
        /// Load all entities and return the local collection.
        /// </summary>
        protected ObservableCollection<T> LoadEntities<T>(DbSet<T> entities, params string[] paths) where T : class
        {
            IQueryable<T> query = entities;
            foreach (string path in paths)
            {
                query = query.Include(path);
            }
            query.Load();
            return entities.Local;
        }

        /// <summary>
        /// Import CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn values.
        /// </summary>
        /// <param name="entity">Entity being converted.</param>
        /// <param name="importer">FileImport object.</param>
        /// <param name="createdByColumn">Column number for CreatedBy</param>
        /// <param name="createdOnColumn">Column number for CreatedOn</param>
        /// <param name="modifiedByColumn">Column number for LastModifiedBy</param>
        /// <param name="modifiedOnColumn">Column number for LastModifiedOn</param>
        protected void ImportCreatedModifiedInfo(IAuditableEntity entity, FileImport importer, int createdByColumn, int createdOnColumn = -1, int modifiedByColumn = -1, int modifiedOnColumn = -1)
        {
            if (createdOnColumn < 0)
            {
                createdOnColumn = createdByColumn + 1;
                modifiedByColumn = createdByColumn + 2;
                modifiedOnColumn = createdByColumn + 3;
            }

            string createdBy = importer.GetString(createdByColumn);
            string modifiedBy = importer.GetString(modifiedByColumn);

            entity.CreatedOn = importer.GetDateTime(createdOnColumn);
            entity.LastModifiedOn = importer.GetDateTime(modifiedOnColumn);            
            if (!string.IsNullOrWhiteSpace(createdBy))
            {
                entity.CreatedBy = UserRepository.Entities.Where(p => p.UserName == createdBy).Select(p => p.Id).FirstOrDefault();
            }
            if (!string.IsNullOrWhiteSpace(modifiedBy))
            {
                entity.CreatedBy = UserRepository.Entities.Where(p => p.UserName == modifiedBy).Select(p => p.Id).FirstOrDefault();
            }
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
