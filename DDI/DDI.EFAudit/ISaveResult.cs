﻿namespace DDI.EFAudit
{
    public interface ISaveResult<TChangeSet>
    { 
        int AffectedObjectCount { get; }
         
        TChangeSet ChangeSet { get; }
    }
}
