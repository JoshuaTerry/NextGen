using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DDI.Shared.Models.Client.Security
{
    [Table("Roles")]
    public class Role : IdentityRole<Guid, UserRole>, IEntity, IAuditableEntity
    {

        [MaxLength(64)]
        public string CreatedBy { get; set; }        
        public DateTime? CreatedOn { get; set; }
        [NotMapped]
        public string DisplayName => Name;
        [MaxLength(64)]
        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public void AssignPrimaryKey() { }
    }
}
//public class Role : EntityBase, IRole<Guid>
//{
//    [Key]
//    [DatabaseGenerated(DatabaseGeneratedOption.None)]
//    public override Guid Id { get; set; }

//    public string Name { get; set; }
//}



