using DDI.Shared.Models.Client.Audit; 
using DDI.Shared.Models.Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Audit
{
    public class ObjectChange : IObjectChange<DDIUser>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [MaxLength(128)]
        public string TypeName { get; set; }
        [MaxLength(128)]
        public string DisplayName { get; set; }
        [MaxLength(64)]
        public string ChangeType { get; set; }
        [MaxLength(128)]
        public string ObjectReference { get; set; }
        public Guid ChangeSetId { get; set; }
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
