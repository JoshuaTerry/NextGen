using DDI.EFAudit.Exceptions;
using System;

namespace DDI.EFAudit.Logging
{
    public class DeferredValue
    {
        private Func<object> future;

        public DeferredValue(Func<object> future)
        {
            this.future = future;
        }

        public object CalculateAndRetrieve()
        {
            try
            {
                return future();
            }
            catch (Exception e)
            {
                throw new ErrorInDeferredCalculation(e);
            }
        }
    }
}
