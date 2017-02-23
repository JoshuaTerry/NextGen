
using DDI.Shared.Models.Client.Audit;

namespace DDI.EFAudit.Logging
{
    public interface IChangeSetFactory<TChangeSet, TPrincipal> where TChangeSet : IChangeSet<TPrincipal>
    {
        TChangeSet ChangeSet();
        IObjectChange<TPrincipal> ObjectChange();
        IPropertyChange<TPrincipal> PropertyChange();
    }
}
