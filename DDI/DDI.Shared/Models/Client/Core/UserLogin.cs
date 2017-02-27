using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [MaxLength(256)]
        public string UserName { get; set; }

        [MaxLength(256)]
        public string FirstName { get; set; }
        [MaxLength(256)]
        public string MiddleName { get; set; }
        [MaxLength(256)]
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public override Guid? CreatedBy { get; set; }
        public override DateTime? CreatedOn { get; set; }
        public override Guid? LastModifiedBy { get; set; }
        public override DateTime? LastModifiedOn { get; set; }
        public override string DisplayName
        {
            get
            {
                return string.IsNullOrEmpty(UserName) ? $"{FirstName} {LastName}" : UserName;
            }
        }
    }
}
