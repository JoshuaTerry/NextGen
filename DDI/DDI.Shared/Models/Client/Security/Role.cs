using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Client.Security
{
    [Table("Roles")]
    public class Role : IdentityRole<Guid, UserRole>, IEntity, IAuditableEntity
    {
          
        public Guid? CreatedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedOn { get; set; }
        [NotMapped]
        public string DisplayName => Name;
        public Guid? LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }
    }
}
//public class Role : EntityBase, IRole<Guid>
//{
//    [Key]
//    [DatabaseGenerated(DatabaseGeneratedOption.None)]
//    public override Guid Id { get; set; }

//    public string Name { get; set; }
//}



