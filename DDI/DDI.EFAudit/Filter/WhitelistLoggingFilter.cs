using System.Collections.Generic;
using System.Linq;
using DDI.EFAudit.Contexts;
using DDI.Shared.Attributes.Logging;

namespace DDI.EFAudit.Filter
{     
    public class WhitelistLoggingFilter : AttributeBasedLoggingFilter, ILoggingFilter
    {
        public WhitelistLoggingFilter(IAuditLogContext context) : base(context) { }

        protected override bool ShouldLogFromAttributes(IEnumerable<IFilterAttribute> filters)
        {
            return filters.Any() && filters.All(f => f.ShouldLog());
        }

        public class Provider : ILoggingFilterProvider
        {
            public ILoggingFilter Get(IAuditLogContext context)
            {
                return new WhitelistLoggingFilter(context);
            }
        }
    }
}
