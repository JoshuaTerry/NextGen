using DDI.EFAudit.Exceptions;
using System;

namespace DDI.EFAudit.Logging
{
    public class DeferredValue
    {
        private Func<object> _future;

        public DeferredValue(Func<object> future)
        {
            this._future = future;
        }

        public object CalculateAndRetrieve()
        {
            try
            {
                return _future();
            }
            catch (Exception e)
            {
                throw new ErrorInDeferredCalculation(e);
            }
        }
    }
}
