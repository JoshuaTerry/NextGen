using System.Collections.Generic;

namespace DDI.EFAudit.Models
{
    public interface IObjectChange<TPrincipal>
    {
        IChangeSet<TPrincipal> ChangeSet { get; set; }
        IEnumerable<IPropertyChange<TPrincipal>> PropertyChanges { get; }
        void Add(IPropertyChange<TPrincipal> propertyChange);
        string DisplayName { get; set; }
        string TypeName { get; set; }
        string ObjectReference { get; set; }        
    }
}
