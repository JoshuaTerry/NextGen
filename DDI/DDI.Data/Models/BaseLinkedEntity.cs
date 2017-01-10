using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks; 
using DDI.Shared.Models;

namespace DDI.Data.Models
{
    /// <summary>
    /// Base class for entities like notes or custom data that can be linked to many other entity types.
    /// </summary>
    public abstract class BaseLinkedEntity : EntityBase
    {

        #region Fields

        private EntityBase _parentEntity = null;
        private bool _ignoreChanges = false;
        private Type _thisGenericType = null;

        #endregion

        #region Properties

        /// <summary>
        /// ID of linked parent entity.
        /// </summary>
        [Index]
        public Guid? ParentEntityId { get; set; }

        /// <summary>
        /// Entity type string value.
        /// </summary>
        [Index]
        public string EntityType { get; set; }

        /// <summary>
        /// Linked parent entity.  The value is not valid until loaded vai LoadParentEntity(context).
        /// </summary>
        [NotMapped]
        public EntityBase ParentEntity
        {
            get
            {
                return _parentEntity;
            }
            set
            {
                SetParentEntityValue(value, true);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load the parent entity property
        /// </summary>
        public void LoadParentEntity(DbContext context)
        {
            Type parentType;
            if (EntityBase.EntityTypeDict.TryGetValue(this.EntityType, out parentType))
            {
                var query = context.Set(parentType) as IQueryable<EntityBase>;
                _parentEntity = query?.FirstOrDefault(p => p.Id == ParentEntityId);
            }
        }

        /// <summary>
        /// Get the generic type of a LinkedEntityCollection for this specific entity's type.
        /// </summary>
        private void GetGenericType()
        {
            _thisGenericType = typeof(LinkedEntityCollection<>).MakeGenericType(this.GetType());
        }

        /// <summary>
        /// Set the value of the ParentEntity property to the specified entity.
        /// </summary>
        /// <param name="newValue"></param>
        internal void SetParentEntityValue(EntityBase newValue, bool updateParentCollection)
        {
            // Avoid recursion
            if (_ignoreChanges)
            {
                return;
            }

            if (updateParentCollection)
            {
                if (_thisGenericType == null)
                {
                    GetGenericType();
                }

                _ignoreChanges = true;

                if ((newValue != null || _parentEntity != null) && !object.ReferenceEquals(_parentEntity, newValue))
                {
                    // value is being changed.
                    if (_parentEntity != null)
                    {
                        // Try to find a property in the current parent entity that is a LinkedEntityCollection for this entity's type.
                        PropertyInfo prop = _parentEntity.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance)
                                .FirstOrDefault(p => p.PropertyType == _thisGenericType);
                        // Updating the ParentEntity where ParentEntity is not null...
                        if (prop != null)
                        {
                            // Try to remove this entity from the parent entity's LinkedEntityCollection.
                            var ecCollection = prop.GetValue(_parentEntity);
                            if (ecCollection != null)
                            {
                                ((IList)ecCollection).Remove(this);
                            }
                        }
                    }

                    if (newValue != null)
                    {
                        // Try to find a property in the new parent entity that is a LinkedEntityCollection for this entity's type.
                        PropertyInfo prop = newValue.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance)
                            .FirstOrDefault(p => p.PropertyType == _thisGenericType);

                        // Updating the ParentEntity to a new entity...
                        if (prop != null)
                        {
                            // Try to add this entity to the parent entity's LinkedEntityCollection.
                            var ecCollection = prop.GetValue(newValue);
                            if (ecCollection != null)
                            {
                                ((IList)ecCollection).Add(this);
                            }
                        }
                    }
                }
            }

            _parentEntity = newValue;

            // Update the ParentEntityID and EntityType properties.
            if (newValue == null)
            {
                ParentEntityId = null;
                EntityType = null;
            }
            else
            {
                ParentEntityId = newValue.Id;
                EntityType = newValue.GetEntityType();
            }

            _ignoreChanges = false;
        }

        #endregion


    }
}