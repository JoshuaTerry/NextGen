﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
    [Table("ConstituentType")]
    public class ConstituentType : BaseEntity, IEntity
    {
        #region Public Properties 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsRequired { get; set; }

        [MaxLength(128)]
        public string BaseType { get; set; }

        [MaxLength(128)]
        public string Code { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }
        #endregion Public Properties
    }
}
