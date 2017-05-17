using System;

namespace DDI.Shared.Attributes.Logging
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class DoNotLogAttribute : Attribute, IFilterAttribute
    {
        public bool ShouldLog()
        {
            return false;
        }
    }
}
