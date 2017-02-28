using DDI.Shared.Models.Client.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Audit
{
    public class ChangeSet : IChangeSet<ApplicationUser> 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }        
        public List<ObjectChange> ObjectChanges { get; set; }

        IEnumerable<IObjectChange<ApplicationUser>> IChangeSet<ApplicationUser>.ObjectChanges
        {
            get { return ObjectChanges; }
        }
         
        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; }

        void IChangeSet<ApplicationUser>.Add(IObjectChange<ApplicationUser> objectChange)
        {
            ObjectChanges.Add((ObjectChange)objectChange);
        }

        public override string ToString()
        {
            return string.Format("By {0} on {1}, with {2} ObjectChanges", User, Timestamp, ObjectChanges.Count);
        }
    }
}
