using DDI.Shared.Models.Client.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Audit
{
    public class ObjectChange : IObjectChange<DDIUser>, IReadOnlyEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(128)]
        public string TypeName { get; set; }
        [MaxLength(128)]
        public string DisplayName { get; set; }
        [MaxLength(64)]
        public string ChangeType { get; set; }
        [MaxLength(128)]
        public string ObjectReference { get; set; }
        public long ChangeSetId { get; set; }
        public ChangeSet ChangeSet { get; set; }
        public List<PropertyChange> PropertyChanges { get; set; }

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
