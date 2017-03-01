using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Shared.Models.Client.Audit;
using DDI.EFAudit.Translation.Serializers;

namespace DDI.EFAudit.Logging
{
    public class DeferredObjectChange<TPrincipal>
    {
        private readonly IObjectChange<TPrincipal> _objectChange;
        private readonly DeferredValue _futureReference;
        private readonly DeferredValueMap _futureValues;
        private readonly ISerializationManager _serializer;

        public DeferredObjectChange(IObjectChange<TPrincipal> objectChange, Func<string> deferredReference, ISerializationManager serializer)
        {
            this._objectChange = objectChange;
            this._futureReference = new DeferredValue(deferredReference);
            this._futureValues = new DeferredValueMap(objectChange);
            this._serializer = serializer;
        }

        // "Bake" is a term used for setting up for values that aren't finished yet, like an Identity Field
        public void Bake()
        {
            _objectChange.EntityId = (string)_futureReference.CalculateAndRetrieve();

            var bakedValues = _futureValues.CalculateAndRetrieve();
            foreach (KeyValuePair<string, object> kv in bakedValues)
            {
                var propertyChange = _objectChange.PropertyChanges.SingleOrDefault(pc => pc.PropertyName == kv.Key);
                setValue(propertyChange, kv.Value);
            }
        }
        private void setValue(IPropertyChange<TPrincipal> propertyChange, object value)
        {
            string valueAsString = valueToString(value);
            propertyChange.Value = valueAsString; 
        }
        private string valueToString(object value)
        {
            if (value == null)
                return null;
            else if (_serializer != null)
                return _serializer.Serialize(value);
            else
                return value.ToString();
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
