using System;
using System.Collections.Generic;
using DDI.EFAudit.Exceptions;

namespace DDI.EFAudit.Logging
{
    public class DeferredValueMap
    {
        private Dictionary<string, DeferredValue> _map;
        private object _container;

        public DeferredValueMap(object container = null)
        {
            this._map = new Dictionary<string, DeferredValue>();
            this._container = container;
        }

        public void Store(string key, Func<object> futureValue)
        {
            _map[key] = new DeferredValue(futureValue);
        }
        public IDictionary<string, object> CalculateAndRetrieve()
        {
            var result = new Dictionary<string, object>();
            foreach (var kv in _map)
            {
                try
                {
                    result[kv.Key] = kv.Value.CalculateAndRetrieve();
                }
                catch (Exception e)
                {
                    throw new ErrorInDeferredCalculation(_container, kv.Key, e);
                }
            }

            return result;
        }
    }
}
