using DDI.Shared.Models.Client.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Audit
{
    public class ObjectChange : IObjectChange<ApplicationUser> 
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
        public string EntityId { get; set; }
        public long ChangeSetId { get; set; }
        public ChangeSet ChangeSet { get; set; }
        public List<PropertyChange> PropertyChanges { get; set; }

        IEnumerable<IPropertyChange<ApplicationUser>> IObjectChange<ApplicationUser>.PropertyChanges
        {
            get { return PropertyChanges; }
        }
        public void Add(IPropertyChange<ApplicationUser> propertyChange)
        {
            PropertyChanges.Add((PropertyChange)propertyChange);
        }
        IChangeSet<ApplicationUser> IObjectChange<ApplicationUser>.ChangeSet
        {
            get { return ChangeSet; }
            set { ChangeSet = (ChangeSet)value; }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", TypeName, EntityId);
        }
    }
}
