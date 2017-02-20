using DDI.EFAudit.Models;
using DDI.Shared.Models.Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Client.Audit
{
    public class PropertyChange : IPropertyChange<DDIUser>
    {
        public string ChangeType { get; set; }
        public int Id { get; set; }
        public virtual ObjectChange ObjectChange { get; set; }
        public string PropertyName { get; set; }
        public string OriginalValue { get; set; }
        public string Value { get; set; }
        IObjectChange<DDIUser> IPropertyChange<DDIUser>.ObjectChange
        {
            get { return ObjectChange; }
            set { ObjectChange = (ObjectChange)value; }
        }
        public override string ToString()
        {
            return string.Format("{0}:{1}", PropertyName, Value);
        }
    }
}
