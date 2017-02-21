﻿
namespace DDI.Shared.Models.Client.Audit
{
    public interface IChangeSetFactory<TChangeSet, TPrincipal> where TChangeSet : IChangeSet<TPrincipal>
    {
        TChangeSet ChangeSet();
        IObjectChange<TPrincipal> ObjectChange();
        IPropertyChange<TPrincipal> PropertyChange();
    }
}
