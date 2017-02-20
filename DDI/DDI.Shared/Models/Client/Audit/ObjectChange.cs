using DDI.EFAudit.Models;
using DDI.Shared.Models.Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Client.Audit
{
    public class ObjectChange : IObjectChange<DDIUser>
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string DisplayName { get; set; }
        public string ObjectReference { get; set; }
        public virtual ChangeSet ChangeSet { get; set; }
        public virtual List<PropertyChange> PropertyChanges { get; set; }

        IEnumerable<IPropertyChange<DDIUser>> IObjectChange<DDIUser>.PropertyChanges
        {
            get { return PropertyChanges; }
        }
        void IObjectChange<DDIUser>.Add(IPropertyChange<DDIUser> propertyChange)
        {
            PropertyChanges.Add((PropertyChange)propertyChange);
        }
        IChangeSet<DDIUser> IObjectChange<DDIUser>.ChangeSet
        {
            get { return ChangeSet; }
            set { ChangeSet = (ChangeSet)value; }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", TypeName, ObjectReference);
        }
    }
}
