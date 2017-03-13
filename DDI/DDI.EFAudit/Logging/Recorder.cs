using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using DDI.EFAudit.Helpers;
using DDI.Shared.Models.Client.Audit;
using DDI.EFAudit.Translation.Serializers;
using System.Data.Entity;
using DDI.Shared.Models;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace DDI.EFAudit.Logging
{
    public class Recorder<TChangeSet, TPrincipal> : IOven<TChangeSet, TPrincipal>
         where TChangeSet : IChangeSet<TPrincipal>
    {
        private TChangeSet _set;
        private IChangeSetFactory<TChangeSet, TPrincipal> _factory;
        private IDictionary<object, DeferredObjectChange<TPrincipal>> _deferredObjectChanges;
        private ISerializationManager _serializer;
        private DbContext _dbContext;

        public Recorder(IChangeSetFactory<TChangeSet, TPrincipal> factory, DbContext dbContext)
        {
            this._deferredObjectChanges = new Dictionary<object, DeferredObjectChange<TPrincipal>>(new ReferenceEqualityComparer());
            this._factory = factory;
            this._serializer = null;
            this._dbContext = dbContext;
        }

        public Recorder(IChangeSetFactory<TChangeSet, TPrincipal> factory, ISerializationManager serializer, DbContext dbContext)
            : this(factory, dbContext)
        {
            this._serializer = serializer;
        }

        public bool HasChangeSet { get { return _set != null; } }
                     
        public void Record(ObjectStateEntry entry, object entity, Func<string> deferredReference, string propertyName, Func<object> deferredValue)
        {
            EnsureChangeSetExists();

            var typeName = ObjectContext.GetObjectType(entity.GetType()).Name;
            var deferredObjectChange = CreateOrRetreiveDeferredObjectChange(_set, entry, entity, typeName, deferredReference);
            Record(entry, deferredObjectChange, propertyName, deferredValue, entity);
        }
        private void Record(ObjectStateEntry entry, DeferredObjectChange<TPrincipal> deferredObjectChange, string propertyName, Func<object> deferredValue, object entity)
        {
            var deferredValues = deferredObjectChange.FutureValues;
            var propertyChange = CreateOrRetrievePropertyChange(deferredObjectChange.ObjectChange, propertyName, entry, entity);
            if (deferredValue != null)
            {
                deferredValues.Store(propertyName, deferredValue);
            }
        }
        
        public TChangeSet Bake(DateTime timestamp, TPrincipal user)
        {
            _set.User = user;
            _set.Timestamp = timestamp;

            foreach (var deferredObjectChange in _deferredObjectChanges.Values)
            {
                deferredObjectChange.ProcessDeferredValues();
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
            result.EntityId = null;

            if (entity != null && entity is IEntity)
            {
                result.DisplayName = (entity as IEntity).DisplayName;
            }
            result.ChangeType = entry.State.ToString();
            result.ChangeSet = set;
            set.Add(result);

            deferredObjectChange = new DeferredObjectChange<TPrincipal>(result, deferredReference, _serializer, entity);
            _deferredObjectChanges.Add(entity, deferredObjectChange);

            return deferredObjectChange;
        }
        private IPropertyChange<TPrincipal> ProcessScalarChange(IPropertyChange<TPrincipal> change, ObjectStateEntry entry, object entity, string propertyName)
        {
            change.PropertyName = propertyName;

            if (entity.GetType().GetProperty(propertyName) != null)
            {
                change.PropertyType = entity.GetType().GetProperty(propertyName).PropertyType;
                change.PropertyTypeName = Nullable.GetUnderlyingType(change.PropertyType)?.Name ?? change.PropertyType.Name;
            }

            // Deletes for Complex entities will not have the propertyName in the OriginalValues
            if (entry.State != EntityState.Added && entry.State != EntityState.Deleted)
                change.OriginalValue = Convert.ToString(entry.OriginalValues[propertyName]);

            return change;
        }
        private IPropertyChange<TPrincipal> ProcessForeignKeyChange(IPropertyChange<TPrincipal> change, ObjectStateEntry entry, object entity, string propertyName, Dictionary<string, string> fkLookup)
        {
            if (entry.State != EntityState.Added)
            {
                change.PropertyType = entity.GetType().GetProperty(fkLookup[propertyName]).PropertyType;
                change.PropertyTypeName = change.PropertyType.Name;

                ObjectStateEntry ose = entry.ObjectStateManager.GetObjectStateEntry(entity);
                var oValues = ose.OriginalValues;
                var id = ose.OriginalValues[propertyName];
                if (id.GetType().Name != "DBNull")
                {                    
                    IEntity navEntity = (IEntity)_dbContext.Set(change.PropertyType).Find(id);
                    change.OriginalDisplayName = navEntity?.DisplayName;
                }
            }
            return change;
        }
        private IPropertyChange<TPrincipal> ProcessRelationshipChange(IPropertyChange<TPrincipal> change, ObjectStateEntry entry, object entity, string propertyName)
        {
            var ends = GetAssociationEnds(entry);
            var foreignEnd = GetOtherAssociationEnd(entry, ends[0]);
            var typeNameP1 = entity.GetType().GetProperty(propertyName).PropertyType.Name.Replace("`1", " ");
            var typeNameP2 = entity.GetType().GetProperty(propertyName).PropertyType.GetGenericArguments()[0].Name;

            change.PropertyTypeName = $"{typeNameP1}{typeNameP2}";
            change.IsManyToMany = foreignEnd.RelationshipMultiplicity == RelationshipMultiplicity.Many;

            if (entry.State == EntityState.Deleted)
            {
                IEntity child = (IEntity)entry.ObjectStateManager.GetObjectStateEntry(GetEndEntityKey(entry, foreignEnd)).Entity;
                change.OriginalDisplayName = child.DisplayName;
                change.OriginalValue = child.Id.ToString();
            }

            return change;
        }
        private IPropertyChange<TPrincipal> CreateOrRetrievePropertyChange(IObjectChange<TPrincipal> objectChange, string propertyName, ObjectStateEntry entry, object entity)
        {
            var result = _factory.PropertyChange();

            result.ChangeType = entry.State.ToString();
            result.ObjectChange = objectChange;
            result.PropertyName = propertyName;

            var fkNameLookup = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(ForeignKeyAttribute))).ToDictionary(p => p.GetCustomAttribute<ForeignKeyAttribute>().Name, p => p.Name);
            result.IsForeignKey = fkNameLookup.ContainsKey(propertyName);

            if (result.IsForeignKey)
            {
                result = ProcessForeignKeyChange(result, entry, entity, propertyName, fkNameLookup);
            }
            else if (entry.IsRelationship)
            {
                result = ProcessRelationshipChange(result, entry, entity, propertyName);
            }
            else
            {
                result = ProcessScalarChange(result, entry, entity, propertyName);
            }

            result.NewValue = null;
            objectChange.Add(result);

            return result;
        }

        private void EnsureChangeSetExists()
        {
            if (_set == null)
                _set = _factory.ChangeSet();
        }

        private AssociationEndMember GetOtherAssociationEnd(ObjectStateEntry entry, AssociationEndMember end)
        {
            AssociationEndMember[] ends = GetAssociationEnds(entry);
            if (ends[0] == end)
                return ends[1];
            else
                return ends[0];
        }

        private EntityKey GetEndEntityKey(ObjectStateEntry entry, AssociationEndMember end)
        {
            AssociationEndMember[] ends = GetAssociationEnds(entry);
            if (ends[0] == end)
                return UseableValues(entry)[0] as EntityKey;
            else
                return UseableValues(entry)[1] as EntityKey;
        }

        private AssociationEndMember[] GetAssociationEnds(ObjectStateEntry entry)
        {
            var fieldMetadata = UseableValues(entry).DataRecordInfo.FieldMetadata;
            return fieldMetadata.Select(m => m.FieldType as AssociationEndMember).ToArray();
        }

        private IExtendedDataRecord UseableValues(ObjectStateEntry entry)
        {
            return entry.State == EntityState.Deleted ? (IExtendedDataRecord)entry.OriginalValues : entry.CurrentValues;
        }
    }
}
