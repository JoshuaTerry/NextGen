using DDI.EFAudit.Contexts;

namespace DDI.EFAudit.Filter
{
    public interface ILoggingFilterProvider
    {
        ILoggingFilter Get(IAuditLogContext context);
    }
}
