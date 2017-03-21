using DDI.EFAudit.Translation.Serializers;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.Audit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace DDI.EFAudit.Logging
{
    public class DeferredObjectChange<TPrincipal>
    {
        private readonly IObjectChange<TPrincipal> _objectChange;
        private readonly DeferredValue _futureReference;
        private readonly DeferredValueMap _futureValues;
        private readonly ISerializationManager _serializer;
        private readonly DbContext _dbContext;
        private readonly object _entity;

        public DeferredObjectChange(IObjectChange<TPrincipal> objectChange, Func<string> deferredReference, ISerializationManager serializer, object entity, DbContext context)
        {
            this._objectChange = objectChange;
            this._futureReference = new DeferredValue(deferredReference);
            this._futureValues = new DeferredValueMap(objectChange);
            this._serializer = serializer;
            this._entity = entity;
            this._dbContext = context;
        }
         
        public void ProcessDeferredValues()
        {
            _objectChange.EntityId = (string)_futureReference.CalculateAndRetrieve();

            var deferredValues = _futureValues.CalculateAndRetrieve();
            foreach (KeyValuePair<string, object> kv in deferredValues)
            {
                var propertyChanges = _objectChange.PropertyChanges.Where(pc => pc.PropertyName == kv.Key).ToList();
                int i = -1;
                foreach (var change in propertyChanges)
                {
                    i++;
                    if (kv.Value != null)
                    {
                        var values = kv.Value.ToString().Split(',');
                        if (values.Length > i)
                            SetValue(change, values[i]);
                    }
                }
            }
        }
        private void SetValue(IPropertyChange<TPrincipal> propertyChange, object value)
        {
            if (propertyChange.IsForeignKey)
            {
                var fkNameLookup = _entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(ForeignKeyAttribute))).ToDictionary(p => p.GetCustomAttribute<ForeignKeyAttribute>().Name, p => p.Name);
                var reference = _dbContext.Entry(_entity).Reference(fkNameLookup[propertyChange.PropertyName]);
                if (reference != null)
                {
                    reference.Load();
                }

                var obj = _entity.GetType().GetProperty(fkNameLookup[propertyChange.PropertyName]).GetValue(_entity);
                if (obj != null && obj.GetType().GetInterfaces().Contains(typeof(IEntity)))
                {
                    propertyChange.NewDisplayName = (obj as IEntity).DisplayName;
                }
            }
            else if (propertyChange.IsManyToMany)
            {
                var collection = (IEnumerable)_entity.GetType().GetProperty(propertyChange.PropertyName).GetValue(_entity);
                var item = collection.Cast<IEntity>().FirstOrDefault(i => i.Id == Guid.Parse(Convert.ToString(value)));
                if (item != null)
                    propertyChange.NewDisplayName = (item as IEntity).DisplayName;
            }

            string valueAsString = ValueToString(value);
            propertyChange.NewValue = valueAsString;

            if (string.IsNullOrEmpty(propertyChange.NewDisplayName) && !string.IsNullOrEmpty(propertyChange.NewValue))
                propertyChange.NewDisplayName = propertyChange.NewValue;
        }
        private string ValueToString(object value)
        {
            if (value == null)
            {
                return null;
            }
            else if (_serializer != null)
            {
                return _serializer.Serialize(value);
            }
            else
            {
                return value.ToString();
            }
        }

        public IObjectChange<TPrincipal> ObjectChange
        {
            get { return _objectChange; }
        }
        public DeferredValue FutureReference
        {
            get { return _futureReference; }
        }
        public DeferredValueMap FutureValues
        {
            get { return _futureValues; }
        }
    }
}
