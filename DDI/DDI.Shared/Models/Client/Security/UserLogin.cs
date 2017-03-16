﻿using Microsoft.AspNet.Identity.EntityFramework;
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
    public class UserLogin : IdentityUserLogin<Guid>, IEntity, IAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }        
        public DateTime? CreatedOn { get; set; }
        [NotMapped]
        public string DisplayName { get; set; } 
        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public void AssignPrimaryKey() { }

    }

}