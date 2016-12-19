using System;

namespace DDI.Business.Extensions
{
    public static class TypeExtensions
    {

        public static object DefaultValue(this Type self)
        {
            if (self.IsValueType)
            {
                return Activator.CreateInstance(self);
            }

            return null;
        }
    }
}