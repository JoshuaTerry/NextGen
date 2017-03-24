using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Security
{
    [Table("UserClaims")]
    public class UserClaim : IdentityUserClaim<Guid>, IEntity, IAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(64)]
        public string CreatedBy { get; set; }        
        public DateTime? CreatedOn { get; set; }
        [NotMapped]
        public string DisplayName { get; set; }
        [MaxLength(64)]
        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public void AssignPrimaryKey() { }

    }

}
