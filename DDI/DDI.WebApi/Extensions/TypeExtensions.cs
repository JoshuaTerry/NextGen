using System;

namespace DDI.WebApi.Extensions
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