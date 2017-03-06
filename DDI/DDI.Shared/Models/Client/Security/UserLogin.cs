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
    [Table("UserLogins")]
    public class UserLogin : IdentityUserLogin<Guid>, IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedOn { get; set; }
        [NotMapped]
        public string DisplayName { get; set; } 
        public Guid? LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }
    }
     
}
