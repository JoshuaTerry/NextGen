using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DDI.Shared.Models.Client.Security
{
    [Table("Roles")]
    public class Role : IdentityRole<Guid, UserRole>, IEntity, IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("RoleId")]
        public new Guid Id { get; set; }
        [MaxLength(64)]
        public string CreatedBy { get; set; }        
        public DateTime? CreatedOn { get; set; }
        [MaxLength(64)]
        public string Module { get; set; }
        [NotMapped]
        public string DisplayName => Module + " " + Name ;
        [MaxLength(64)]
        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }
        [InverseProperty(nameof(Group.Roles))]
        public ICollection<Group> Groups { get; set; }
        public void AssignPrimaryKey() { }
    }
} 


