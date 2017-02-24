using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Client.Core
{
    public class DDIUser : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        [MaxLength(256)]
        public string FirstName { get; set; }
        [MaxLength(256)]
        public string MiddleName { get; set; }
        [MaxLength(256)]
        public string LastName { get; set; }
        [MaxLength(256)]
        public string EmailAddress { get; set; }
        public Guid? UserLoginId { get; set; }
        public UserLogin UserLogin { get; set; }
        public override Guid? CreatedBy { get; set; } 
        public override DateTime? CreatedOn { get; set; } 
        public override Guid? LastModifiedBy { get; set; } 
        public override DateTime? LastModifiedOn { get; set; }
        public override string DisplayName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}
