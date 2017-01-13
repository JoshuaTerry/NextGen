using DDI.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DDI.Shared.Models
{
    /// <summary>
    /// Base class for all entity model classes.
    /// </summary>
    public abstract class EntityBase : IEntity
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

            Assembly assm = typeof(EntityBase).Assembly;

            foreach (var type in assm.GetTypes().Where(p => p.IsSubclassOf(typeof(EntityBase))))
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


    }


}
