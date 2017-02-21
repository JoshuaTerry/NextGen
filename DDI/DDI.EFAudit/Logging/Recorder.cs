using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using DDI.EFAudit.Helpers;
using DDI.Shared.Models.Client.Audit;
using DDI.EFAudit.Translation.Serializers;
using System.Data.Entity; 

namespace DDI.EFAudit.Logging
{
    public class Recorder<TChangeSet, TPrincipal> : IOven<TChangeSet, TPrincipal>
         where TChangeSet : IChangeSet<TPrincipal>
    {
        private TChangeSet _set;
        private IChangeSetFactory<TChangeSet, TPrincipal> _factory;
        private IDictionary<object, DeferredObjectChange<TPrincipal>> _deferredObjectChanges;
        private ISerializationManager _serializer;

        public Recorder(IChangeSetFactory<TChangeSet, TPrincipal> factory)
        {
            this._deferredObjectChanges = new Dictionary<object, DeferredObjectChange<TPrincipal>>(new ReferenceEqualityComparer());
            this._factory = factory;
            this._serializer = null;
        }

        public Recorder(IChangeSetFactory<TChangeSet, TPrincipal> factory, ISerializationManager serializer)
            : this(factory)
        {
            this._serializer = serializer;
        }

        public bool HasChangeSet { get { return _set != null; } }

        // You have to pass both the ObjectStateEntry entry AND object entity.  For scalar changes the entity is available off of 
        // entry.Entity, but for non-scalar changes the entry.Entity is null and is instead retreived by the caller using a GetById        
        public void Record(ObjectStateEntry entry, object entity, Func<string> deferredReference, string propertyName, Func<object> deferredValue)
        {
            EnsureChangeSetExists();

            var typeName = ObjectContext.GetObjectType(entity.GetType()).Name;
            var deferredObjectChange = CreateOrRetreiveDeferredObjectChange(_set, entry, entity, typeName, deferredReference);

            if (entry.State != EntityState.Deleted)
                Record(entry, deferredObjectChange, propertyName, deferredValue);
        }
        private void Record(ObjectStateEntry entry, DeferredObjectChange<TPrincipal> deferredObjectChange, string propertyName, Func<object> deferredValue)
        {
            var deferredValues = deferredObjectChange.FutureValues;
            var propertyChange = CreateOrRetrievePropertyChange(deferredObjectChange.ObjectChange, propertyName, entry);
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
        public TChangeSet Bake(DateTime timestamp, TPrincipal user)
        {
            _set.User = user;
            _set.Timestamp = timestamp;

            foreach (var deferredObjectChange in _deferredObjectChanges.Values)
            {
                deferredObjectChange.Bake();
            }

            return _set;
        }

        private DeferredObjectChange<TPrincipal> CreateOrRetreiveDeferredObjectChange(TChangeSet set, ObjectStateEntry entry, object entity, string typeName, Func<string> deferredReference)
        {
            var deferredObjectChange = _deferredObjectChanges.FirstOrDefault(doc => ReferenceEquals(doc.Key, entity)).Value;
            if (deferredObjectChange != null)
                return deferredObjectChange;

            var result = _factory.ObjectChange();
            result.TypeName = typeName;
            result.ObjectReference = null;

            if (entity != null && entity is IEntityBase)
            {
                result.DisplayName = (entity as IEntityBase).DisplayName;
            }
            result.ChangeType = entry.State.ToString();
            result.ChangeSet = set;
            set.Add(result);

            deferredObjectChange = new DeferredObjectChange<TPrincipal>(result, deferredReference, _serializer);
            _deferredObjectChanges.Add(entity, deferredObjectChange);

            return deferredObjectChange;
        }
        private IPropertyChange<TPrincipal> CreateOrRetrievePropertyChange(IObjectChange<TPrincipal> objectChange, string propertyName, ObjectStateEntry entry)
        {
            var result = objectChange.PropertyChanges.FirstOrDefault(pc => pc.PropertyName == propertyName);

            if (result == null)
            {
                result = _factory.PropertyChange();
                result.ChangeType = entry.State.ToString();
                result.ObjectChange = objectChange;
                result.PropertyName = propertyName;
                // Deletes for Complex entities will not have the propertyName in the OriginalValues

                if (entry.State != EntityState.Added && entry.State != EntityState.Deleted)
                    result.OriginalValue = Convert.ToString(entry.OriginalValues[propertyName]);

                result.Value = null;
                objectChange.Add(result);
            }

            return result;
        }
        private void EnsureChangeSetExists()
        {
            if (_set == null)
                _set = _factory.ChangeSet();
        }
    }
}
