using DDI.Shared.Models.Client.Audit;
using System;
using System.Collections.Generic;

namespace DDI.EFAudit.History
{
    public interface IChange<TValue, TPrincipal>
    {
        DateTime Timestamp { get; }
        TPrincipal User { get; }
        TValue Value { get; }
        IObjectChange<TPrincipal> ObjectChange { get; }        
        IEnumerable<Exception> Errors { get; }
        bool ProblemsRetrievingData { get; }
    }
}
