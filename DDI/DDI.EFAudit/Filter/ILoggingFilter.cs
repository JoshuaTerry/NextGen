using System;
using System.Data.Entity.Core.Metadata.Edm;

namespace DDI.EFAudit.Filter
{
    /// <summary>
    /// A logging filter determines whether a given class or any child properties should be logged 
    /// </summary>
    public interface ILoggingFilter
    {       
        bool ShouldLog(Type type);   
        bool ShouldLog(NavigationProperty property);
        bool ShouldLog(Type type, string propertyName);
    }
}
