using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Core
{
    [Table("AspNetUsers")]
    public class UserLogin : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        [MaxLength(256)]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnable { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public DDIUser DDIUser { get; set; }
        public Guid DDIUserId { get; set; }
        [MaxLength(256)]
        public string UserName { get; set; }
        [NotMapped]
        public override Guid? CreatedBy { get; set; }
        [NotMapped]
        public override DateTime? CreatedOn { get; set; }
        [NotMapped]
        public override Guid? LastModifiedBy { get; set; }
        [NotMapped]
        public override DateTime? LastModifiedOn { get; set; }
        public override string DisplayName
        {
            get
            {
                return UserName;
            }
        }
    }
}
