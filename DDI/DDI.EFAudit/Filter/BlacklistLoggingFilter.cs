using DDI.EFAudit.Contexts;
using DDI.Shared.Attributes.Logging;
using System.Collections.Generic;
using System.Linq;

namespace DDI.EFAudit.Filter
{

    public class BlacklistLoggingFilter : AttributeBasedLoggingFilter, ILoggingFilter
    {
        public BlacklistLoggingFilter(IAuditLogContext context) : base(context) { }

        protected override bool ShouldLogFromAttributes(IEnumerable<IFilterAttribute> filters)
        {
            return filters.All(f => f.ShouldLog());
        }

        public class Provider : ILoggingFilterProvider
        {
            public ILoggingFilter Get(IAuditLogContext context)
            {
                return new BlacklistLoggingFilter(context);
            }
        }
    }
}
