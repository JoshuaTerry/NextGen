﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("RegionLevel")]
    public class RegionLevel : EntityBase
    {
        #region Public Properties 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(128)]
        public string Abbreviation { get; set; }

        [MaxLength(128)]
        public string Label { get; set; }

        public int Level { get; set; }

        public bool IsRequired { get; set; }

        public bool IsChildLevel { get; set; }


        public override string DisplayName
        {
            get
            {
                return Label;
            }
        }

        #endregion Public Properties

    }
}
