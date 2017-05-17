using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DDI.Business.Helpers
{
    public static class LinkedEntityHelper 
    {
        #region Private Fields
        private static Dictionary<string, Type> _entityTypes = null;
        private static string _namespacePrefix;
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
