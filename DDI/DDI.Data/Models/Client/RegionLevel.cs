﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
	[Table("RegionLevel")]
	public class RegionLevel : BaseEntity, IEntity
    {
		#region Public Properties 
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

        [MaxLength(128)]
        public string Abbreviation { get; set; }

        [MaxLength(128)]
        public string Label { get; set; }

        public int Level { get; set; }

        public bool IsRequired { get; set; }

        public bool IsChildLevel { get; set; }


        #endregion Public Properties
    }
}
