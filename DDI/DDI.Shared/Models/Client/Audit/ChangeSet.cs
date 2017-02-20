using DDI.EFAudit.Models;
using DDI.Shared.Models.Client.Core;
using System;
using System.Collections.Generic;

namespace DDI.Shared.Models.Client.Audit
{
    public class ChangeSet : IChangeSet<DDIUser>
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public DDIUser Author { get; set; }
        public virtual List<ObjectChange> ObjectChanges { get; set; }

        IEnumerable<IObjectChange<DDIUser>> IChangeSet<DDIUser>.ObjectChanges
        {
            get { return ObjectChanges; }
        }

        void IChangeSet<DDIUser>.Add(IObjectChange<DDIUser> objectChange)
        {
            ObjectChanges.Add((ObjectChange)objectChange);
        }

        public override string ToString()
        {
            return string.Format("By {0} on {1}, with {2} ObjectChanges", Author, Timestamp, ObjectChanges.Count);
        }
    }
}
