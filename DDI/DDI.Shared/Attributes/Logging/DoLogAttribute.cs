using System;

namespace DDI.Shared.Attributes.Logging
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class DoLogAttribute : Attribute, IFilterAttribute
    {
        public bool ShouldLog()
        {
            return true;
        }
    }
}
