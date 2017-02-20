using DDI.EFAudit.Models;
using System;

namespace DDI.EFAudit.Logging
{
    internal interface IOven<TChangeSet, TPrincipal> where TChangeSet : IChangeSet<TPrincipal>
    {
        bool HasChangeSet { get; }
        TChangeSet Bake(DateTime timestamp, TPrincipal author);
    }
}
