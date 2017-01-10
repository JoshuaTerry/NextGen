using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DDI.Data.Attributes;
using DDI.Data.Extensions;

namespace DDI.Data.Models
{
    /// <summary>
    /// Base class for all entity model classes.
    /// </summary>
    public abstract class BaseEntity : IEntity
    {
        #region Fields

        private string _entityType = null;
        private static Dictionary<string, Type> _entityTypeDict = null;

        #endregion

        #region Properties 

        public abstract Guid Id { get; set; }

        public virtual string DisplayName => string.Empty;

        /// <summary>
        /// Dictionary relating entity type string values to underlying entity types.
        /// </summary>
        protected static Dictionary<string, Type> EntityTypeDict
        {
            get
            {
                if (_entityTypeDict == null)
                {
                    LoadEntityTypeDict();
                }
                return _entityTypeDict;
            }
        }
        #endregion

        #region Constructors 

        #endregion

        #region Methods

        public override string ToString()
        {
            return DisplayName;
        }

        /// <summary>
        /// Load the entity type dictionary.
        /// </summary>
        private static void LoadEntityTypeDict()
        {
            _entityTypeDict = new Dictionary<string, Type>();

            Assembly assm = typeof(BaseEntity).Assembly;

            foreach (var type in assm.GetTypes().Where(p => p.IsSubclassOf(typeof(BaseEntity))))
            {
                string entityType = type.GetCustomAttribute<EntityTypeAttribute>()?.EntityTypeName;
                if (!string.IsNullOrEmpty(entityType))
                {
                    _entityTypeDict.Add(entityType, type);
                }
            }
        }

        /// <summary>
        /// Return the entity type string value.
        /// </summary>
        public string GetEntityType()
        {
            if (_entityType == null)
            {
                MemberInfo info = this.GetType();
                _entityType = info.GetCustomAttribute<EntityTypeAttribute>()?.EntityTypeName ?? string.Empty;
            }
            return _entityType;
        }


        /// <summary>
        /// Ensure the entity's primary key has been assigned.
        /// </summary>
        public void AssignPrimaryKey()
        {
            if (this.Id == default(Guid))
                this.Id = Guid.NewGuid();
        }

        #endregion

        public dynamic ToPartialObject(string fields, bool shouldAddLinks)
        {
            if (string.IsNullOrWhiteSpace(fields) || !shouldAddLinks)
            {
                return this;
            }
            var listOfFields = fields?.Split(',').ToList() ?? new List<string>();
            var result = this.ToDynamic(listOfFields);
            if (shouldAddLinks)
            {
                result = AddHATEAOSLinks(result);
            }
            return result;
        }

        internal virtual dynamic AddHATEAOSLinks(IDictionary<string, object> entity)
        {
            return this.ToDynamic(null);
        }
    }


}
