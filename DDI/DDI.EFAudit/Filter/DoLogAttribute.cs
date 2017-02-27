using System;

namespace DDI.EFAudit.Filter
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
