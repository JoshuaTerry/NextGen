using System;

namespace DDI.Shared
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

    public class Foo
    {
        public void Test()
        {
            

        }
    }
}