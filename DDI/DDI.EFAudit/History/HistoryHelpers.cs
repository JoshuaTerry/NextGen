using DDI.EFAudit.Exceptions;
using System;

namespace DDI.EFAudit.History
{
    public static class HistoryHelpers
    {
        public static object Instantiate(Type type)
        {
            if (type == null)
                return null;

            try
            {
                var instance = Activator.CreateInstance(type);
                return instance;
            }
            catch (Exception ex)
            {
                throw new UnableToInstantiateObjectException(type, ex);
            }
        }
        public static T Instantiate<T>()
        {
            return (T)Instantiate(typeof(T));
        }
    }
}
