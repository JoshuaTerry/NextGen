﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("DoingBusinessAs")]
	public class DoingBusinessAs : AuditableEntityBase
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }
        [ForeignKey("ConstituentId")]
        public Constituent Constituent { get; set; }

        public Guid? ConstituentId { get; set; }
        #endregion Public Properties
    }
}
