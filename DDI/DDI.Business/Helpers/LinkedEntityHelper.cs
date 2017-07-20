using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DDI.Shared.Attributes.Models;

namespace DDI.Business.Helpers
{
    public static class LinkedEntityHelper 
    {
        #region Private Fields
        private static Dictionary<string, Type> _entityTypes = null;        
        private static Dictionary<Type, string> _displaynames = new Dictionary<Type, string>();
        #endregion

        #region Constructors 

        #endregion

        #region Public Methods
        
        public static string GetEntityTypeName(Type type)
        {
            return type.Name;
        }

        public static string GetEntityTypeName<T>() where T : EntityBase
        {
            return GetEntityTypeName(typeof(T));
        }

        /// <summary>
        /// Get the displayable name for an entity.
        /// </summary>
        public static string GetEntityDisplayName(IEntity entity)
        {
            return (GetEntityDisplayName(entity.GetType()) + " " + entity.DisplayName).Trim();
        }

        /// <summary>
        /// Get the displayable name for an entity type.
        /// </summary>
        public static string GetEntityDisplayName(Type type)
        {
            string name;

            if (!_displaynames.TryGetValue(type, out name))
            {
                // First look for the EntityName attribute on the type.
                var attribute = type.GetCustomAttribute<EntityNameAttribute>();
                if (attribute != null)
                {
                    name = attribute.Name;
                }

                if (string.IsNullOrWhiteSpace(name))
                {
                    // No entity name, so convert the type name to separate words.
                    name = StringHelper.ToSeparateWords(type.Name);
                }

                _displaynames[type] = name;
            }

            return name;
        }


        public static EntityBase GetParentEntity(ILinkedEntityBase childEntity, IUnitOfWork unitOfWork)
        {
            if (childEntity.ParentEntityId == null)
            {
                return null;
            }
            
            Type parentType;
            if (_EntityTypes.TryGetValue(childEntity.EntityType, out parentType))
            {
                var query = unitOfWork.GetEntities(parentType) as IQueryable<EntityBase>;
                return query?.FirstOrDefault(p => p.Id == childEntity.ParentEntityId);
            }

            return null;
        }

        public static void SetParentEntity(ILinkedEntityBase childEntity, EntityBase parentEntity)
        {
            if (parentEntity == null)
            {
                childEntity.ParentEntityId = null;
                childEntity.EntityType = null;
            }
            else
            {
                childEntity.ParentEntityId = parentEntity.Id;
                childEntity.EntityType = GetEntityTypeName(parentEntity.GetType());
            }
        }

        #endregion

        #region Private Methods

        private static Dictionary<string, Type> _EntityTypes
        {
            get
            {
                if (_entityTypes == null)
                {
                    LoadEntityTypeDict();
                }
                return _entityTypes;
            }
        }

        private static void LoadEntityTypeDict()
        {
            _entityTypes = new Dictionary<string, Type>();

            foreach (Type type in ReflectionHelper.GetDerivedTypes<EntityBase>(typeof(EntityBase).Assembly))
            {
                _entityTypes.Add(GetEntityTypeName(type), type);
            }
        }

        #endregion  

    }
}
