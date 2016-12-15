using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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