using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.Security;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Audit
{
    public class PropertyChange : IPropertyChange<User>
    {
        public string ChangeType { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ObjectChangeId { get; set; }
        public ObjectChange ObjectChange { get; set; }
        [MaxLength(128)]
        public string PropertyName { get; set; }
        [NotMapped]
        public Type PropertyType { get; set; }
        [MaxLength(128)]
        public string PropertyTypeName { get; set; }
        [MaxLength(512)]
        public string OriginalDisplayName { get; set; }
        [MaxLength(512)]
        public string OriginalValue { get; set; }
        [MaxLength(512)]
        public string NewValue { get; set; }
        [MaxLength(512)]
        public string NewDisplayName { get; set; }
        [NotMapped]
        bool IPropertyChange<User>.IsForeignKey { get; set; }
        [NotMapped]
        bool IPropertyChange<User>.IsManyToMany { get; set; }
        IObjectChange<User> IPropertyChange<User>.ObjectChange
        {
            get { return ObjectChange; }
            set { ObjectChange = (ObjectChange)value; }
        }
        public override string ToString()
        {
            return string.Format("{0}:{1}", PropertyName, NewValue);
        }
    }
}
