﻿using DDI.Shared.Models.Client.Audit;
using System.Linq;

namespace DDI.EFAudit.Contexts
{
    public interface IHistoryContext
    {        
        bool ObjectHasReference(object model);       
        string GetReferenceForObject(object model);        
        string GetReferencePropertyForObject(object model);        
        object GetObjectByReference(System.Type type, string raw);
    }

    public interface IHistoryContext<TChangeSet, TPrincipal> : IHistoryContext where TChangeSet : IChangeSet<TPrincipal>
    {
        IQueryable<IChangeSet<TPrincipal>> ChangeSets { get; }
        IQueryable<IObjectChange<TPrincipal>> ObjectChanges { get; }
        IQueryable<IPropertyChange<TPrincipal>> PropertyChanges { get; }
    }
}
