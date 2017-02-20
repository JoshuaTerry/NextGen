using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using DDI.EFAudit.Helpers;
using DDI.EFAudit.Models;
using DDI.EFAudit.Translation.Serializers;
using System.Data.Entity;

namespace DDI.EFAudit.Logging
{
    public class Recorder<TChangeSet, TPrincipal> : IOven<TChangeSet, TPrincipal>
        where TChangeSet : IChangeSet<TPrincipal>
    {
        private TChangeSet set;
        private IChangeSetFactory<TChangeSet, TPrincipal> factory;
        private IDictionary<object, DeferredObjectChange<TPrincipal>> deferredObjectChanges;
        private ISerializationManager serializer;

        public Recorder(IChangeSetFactory<TChangeSet, TPrincipal> factory)
        {
            this.deferredObjectChanges = new Dictionary<object, DeferredObjectChange<TPrincipal>>(new ReferenceEqualityComparer());
            this.factory = factory;
            this.serializer = null;
        }

        public Recorder(IChangeSetFactory<TChangeSet, TPrincipal> factory, ISerializationManager serializer)
            : this(factory)
        {
            this.serializer = serializer;
        }

        public bool HasChangeSet { get { return set != null; } }

        public void Record(object entity, Func<string> deferredReference, string propertyName, Func<object> deferredValue, ObjectStateEntry entry)
        {
            EnsureChangeSetExists();

            var typeName = ObjectContext.GetObjectType(entity.GetType()).Name;
            var deferredObjectChange = CreateOrRetreiveDeferredObjectChange(set, entity, typeName, deferredReference, entry);
            Record(deferredObjectChange, propertyName, deferredValue, entry);
        }
        private void Record(DeferredObjectChange<TPrincipal> deferredObjectChange, string propertyName, Func<object> deferredValue, ObjectStateEntry entry)
        {
            var deferredValues = deferredObjectChange.FutureValues;
            var propertyChange = CreateOrRetrievePropertChange(deferredObjectChange.ObjectChange, propertyName, entry);
            if (deferredValue != null)
            {
                deferredValues.Store(propertyName, deferredValue);
            }
        }

        /// <summary>
        /// Values for Many-to-Many can't be calculated while commit changes are unfinished.
        /// So instead you have to just record the deferred values until after the db changes 
        /// are saved.  Then before you commit the ChangeSet you have to "Bake" in those deferred 
        /// values into the PropertyChange.  
        /// 
        /// The timing for these types of changes was difficult to figure out.
        /// </summary>
        public TChangeSet Bake(DateTime timestamp, TPrincipal author)
        {
            set.Author = author;
            set.Timestamp = timestamp;

            foreach (var deferredObjectChange in deferredObjectChanges.Values)
            {
                deferredObjectChange.Bake();
            }

            return set;
        }

        private DeferredObjectChange<TPrincipal> CreateOrRetreiveDeferredObjectChange(TChangeSet set, object entity, string typeName, Func<string> deferredReference, ObjectStateEntry entry)
        {
            var deferredObjectChange = deferredObjectChanges.SingleOrDefault(doc => ReferenceEquals(doc.Key, entity)).Value;
            if (deferredObjectChange != null)
                return deferredObjectChange;

            var result = factory.ObjectChange();
            result.TypeName = typeName;
            result.ObjectReference = null;

            if (entity != null && entity is IEntityBase)
            {
                result.DisplayName = (entity as IEntityBase).DisplayName;
            }
            
            result.ChangeSet = set;
            set.Add(result);

            deferredObjectChange = new DeferredObjectChange<TPrincipal>(result, deferredReference, serializer);
            deferredObjectChanges.Add(entity, deferredObjectChange);

            return deferredObjectChange;
        }
        private IPropertyChange<TPrincipal> CreateOrRetrievePropertChange(IObjectChange<TPrincipal> objectChange, string propertyName, ObjectStateEntry entry)
        {
            var result = objectChange.PropertyChanges.SingleOrDefault(pc => pc.PropertyName == propertyName);

            if (result == null)
            {
                result = factory.PropertyChange();
                result.ChangeType = entry.State.ToString();
                result.ObjectChange = objectChange;
                result.PropertyName = propertyName;

                // Although Deleted Entities have an original value, the value is already in the EntityId
                if (entry.State != EntityState.Added && entry.State != EntityState.Deleted)
                    result.OriginalValue = Convert.ToString(entry.OriginalValues[propertyName]);

                result.Value = null;
                objectChange.Add(result);
            }

            return result;
        }
        private void EnsureChangeSetExists()
        {
            if (set == null)
                set = factory.ChangeSet();
        }
    }
}
