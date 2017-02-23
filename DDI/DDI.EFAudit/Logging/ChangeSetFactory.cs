using DDI.Shared.Models.Client.Audit;
using DDI.Shared.Models.Client.Core;
using System.Collections.Generic;

namespace DDI.EFAudit.Logging
{
    public class ChangeSetFactory : IChangeSetFactory<ChangeSet, DDIUser>
    {
        public ChangeSet ChangeSet()
        {
            var set = new ChangeSet();
            set.ObjectChanges = new List<ObjectChange>();
            return set;
        }

        public IObjectChange<DDIUser> ObjectChange()
        {
            var o = new ObjectChange();
            o.PropertyChanges = new List<PropertyChange>();
            return o;
        }

        public IPropertyChange<DDIUser> PropertyChange()
        {
            return new PropertyChange();
        }
    }
}
