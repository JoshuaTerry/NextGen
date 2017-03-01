using System;

namespace DDI.EFAudit.Filter
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
