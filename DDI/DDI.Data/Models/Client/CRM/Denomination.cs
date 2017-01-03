﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DDI.Data.Enums.CRM;

namespace DDI.Data.Models.Client.CRM
{
    [Table("Denomination")]
    public class Denomination : BaseEntity
    {
        #region Public Properties       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(128)]
        public string Code { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public bool IsActive { get; set; }
       
        public Religion Religion { get; set; }

        public Affiliation Affiliation { get; set; }

        public ICollection<Constituent> Constituents { get; set; }
        #endregion Public Properties

        #region Public Methods

        public override string DisplayName
        {
            get
            {
                return Code + ": " + Name;
            }
        }

        #endregion

    }
}
