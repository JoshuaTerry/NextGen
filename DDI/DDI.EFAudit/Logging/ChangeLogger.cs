using DDI.EFAudit.Contexts;
using DDI.EFAudit.Filter;
using DDI.EFAudit.Logging.ValuePairs;
using DDI.Shared.Models.Client.Audit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using DDI.EFAudit.History;
using DDI.EFAudit.Translation.Serializers;

namespace DDI.EFAudit.Logging
{
    internal class ChangeLogger<TChangeSet, TPrincipal>
        where TChangeSet : IChangeSet<TPrincipal>
    {
        private IAuditLogContext<TChangeSet, TPrincipal> _context;
        private IChangeSetFactory<TChangeSet, TPrincipal> _factory;
        private Recorder<TChangeSet, TPrincipal> _recorder;
        private ILoggingFilter _filter;

        public ChangeLogger(IAuditLogContext<TChangeSet, TPrincipal> context, 
            IChangeSetFactory<TChangeSet, TPrincipal> factory,
            ILoggingFilter filter, ISerializationManager serializer)
        {
            this._context = context;
            this._factory = factory;
            this._recorder = new Recorder<TChangeSet, TPrincipal>(factory, serializer);
            this._filter = filter;
        }

        // This is where you can get State (Add, Edit, Delete)
        public IOven<TChangeSet, TPrincipal> Log(ObjectStateManager objectStateManager)
        {  
            var entries = objectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified | EntityState.Deleted);

            foreach (var entry in entries)
            {
                Process(entry);
            }

            return _recorder;
        }

        private void Process(ObjectStateEntry entry)
        { 
            if (entry.IsRelationship)
            {
                LogRelationshipChange(entry);
            }
            else
            {
                 LogScalarChanges(entry);
            }
        }

        private void LogScalarChanges(ObjectStateEntry entry)
        {
            // If this class shouldn't be logged, give up at this point
            if (!_filter.ShouldLog(entry.Entity.GetType()))
                return; 
           
            foreach (string propertyName in GetChangedProperties(entry))
            {
                if (_filter.ShouldLog(entry.Entity.GetType(), propertyName))
                {
                    // We can have multiple changes for the same property if its a complex type
                    var valuePairs = ValuePairSource.Get(entry, propertyName).Where(p => p.HasChanged);
                    var entity = entry.Entity;

                    foreach (var valuePair in valuePairs)
                    {
                        var pair = valuePair;                        
                        _recorder.Record(entry, entry.Entity,
                            () => _context.GetReferenceForObject(entity),
                            valuePair.PropertyName,
                            pair.NewValue);
                    }
                }
            }
        }

        /// <summary>
        /// This is where Navigation Properties will get updated
        /// </summary> 
        private void LogRelationshipChange(ObjectStateEntry entry)
        {         
             
            if (entry.State == EntityState.Added || entry.State == EntityState.Deleted)
            {
                // Each relationship has two ends. Log both directions.
                var ends = GetAssociationEnds(entry);

                foreach (var localEnd in ends)
                {
                    var foreignEnd = GetOtherAssociationEnd(entry, localEnd);
                    LogForeignKeyChange(entry, localEnd, foreignEnd);
                }
            }
        }

        private void LogForeignKeyChange(ObjectStateEntry entry, AssociationEndMember localEnd, AssociationEndMember foreignEnd)
        {  
            // These "keys" represent in-memory unique references to the objects at the ends of these associations.
            // This will give you the Entity Type and the Key you need
            var key = GetEndEntityKey(entry, localEnd);

            // Get the object identified by the local key
            object entity = _context.GetObjectByKey(key);
            if (!_filter.ShouldLog(entity.GetType()))
                return;

            // The property on the "local" object that navigates to the "foreign" object
            var property = GetProperty(entry, localEnd, foreignEnd, key);
             
            if (property == null || !_filter.ShouldLog(property))
                return;

            // Generate the change
            var value = GetForeignValue(entry, entity, foreignEnd, property.Name);

            _recorder.Record(entry, entity, () => _context.GetReferenceForObject(entity), property.Name, value);
        }

        private Func<object> GetForeignValue(ObjectStateEntry entry, object entity, AssociationEndMember foreignEnd, string propertyName)
        {
            if (foreignEnd.RelationshipMultiplicity == RelationshipMultiplicity.Many)
            {
                return ManyToManyValue(entity, propertyName);
            }
            else
            {
                if (entry.State == EntityState.Added)
                    return () =>
                    {
                        // Get the key that identifies the the object we are making or breaking a relationship with
                        var foreignKey = GetEndEntityKey(entry, foreignEnd);
                        return GetKeyReference(foreignKey);
                    };
                else
                    return null;
            }
        }

        private Func<object> ManyToManyValue(object entity, string propertyName)
        {
            return () =>
            {
                // In this case the key id represents an object being added to or removed from a set.
                // We use reflection to get the current contents of the set (so after the change we are logging).
                if (entity == null)
                {
                    throw new InvalidOperationException("Attempted to log change to null object of type " + entity.GetType().Name);
                }

                var property = entity.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (property == null)
                {
                    throw new InvalidOperationException(string.Format("Unable to find a property with name '{0}' on type '{1}'", propertyName, entity.GetType()));
                }

                var value = property.GetValue(entity, null);
                if (value == null)
                {
                    throw new InvalidOperationException(string.Format("Many-to-many set '{0}.{1}' was null", entity.GetType().Name, property.Name));
                }

                IEnumerable<string> current = ((IEnumerable<object>)value)
                    .Select(e => _context.GetReferenceForObject(e))
                    .Distinct()
                    .OrderBy(reference => reference);

                return ToIdList(current);
            };
        }
        private string ToIdList(IEnumerable<string> references)
        {
            return string.Format("{0}", string.Join(",", references));
        }

        private string GetKeyReference(EntityKey key)
        {
            var entity = _context.GetObjectByKey(key);
            return _context.GetReferenceForObject(entity);
        }

        private IEnumerable<string> GetChangedProperties(ObjectStateEntry entry)
        {
            var values = UseableValues(entry);
            for (int i = 0; i < values.FieldCount; i++)
                yield return values.GetName(i);
        }

        /// <summary>
        /// Gets either CurrentValues or OriginalValues depending on which the entry.
        /// Added will have no OriginalValues, and Deleted only have OriginalValues).
        /// </summary>
        private IExtendedDataRecord UseableValues(ObjectStateEntry entry)
        {
            return entry.State == EntityState.Deleted ? (IExtendedDataRecord)entry.OriginalValues : entry.CurrentValues;
        }

        private AssociationEndMember[] GetAssociationEnds(ObjectStateEntry entry)
        {
            var fieldMetadata = UseableValues(entry).DataRecordInfo.FieldMetadata;
            return fieldMetadata.Select(m => m.FieldType as AssociationEndMember).ToArray();
        }

        /// <summary>
        /// Given one end of an association, fetches the other end
        /// </summary>
        private AssociationEndMember GetOtherAssociationEnd(ObjectStateEntry entry, AssociationEndMember end)
        {
            AssociationEndMember[] ends = GetAssociationEnds(entry);
            if (ends[0] == end)
                return ends[1];
            else
                return ends[0];
        }

        /// <summary>
        /// Gets the EntityKey associated with this end of the association
        /// </summary>
        private EntityKey GetEndEntityKey(ObjectStateEntry entry, AssociationEndMember end)
        {
            AssociationEndMember[] ends = GetAssociationEnds(entry);
            if (ends[0] == end)
                return UseableValues(entry)[0] as EntityKey;
            else
                return UseableValues(entry)[1] as EntityKey;
        }

        /// <summary>  
        /// Gets the NavigationProperty that that will let you navigate to the entity 
        /// at the local and foreign ends.
        /// </summary>
        private NavigationProperty GetProperty(ObjectStateEntry entry, AssociationEndMember localEnd, AssociationEndMember foreignEnd, EntityKey key)
        {
            var relationshipType = entry.EntitySet.ElementType;
            var entitySet = key.GetEntitySet(entry.ObjectStateManager.MetadataWorkspace);

            return entitySet.ElementType.NavigationProperties
                            .Where(p => p.RelationshipType == relationshipType
                                     && p.FromEndMember == localEnd
                                     && p.ToEndMember == foreignEnd).SingleOrDefault();
        }
    }
}