using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Client.Security
{
    public class Group : IEntity, IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }
        [NotMapped]
        public string DisplayName => Name;
        public ICollection<User> Users { get; set; }
        public ICollection<Role> Roles { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }

        public void AssignPrimaryKey()
        {
        }
    }
}