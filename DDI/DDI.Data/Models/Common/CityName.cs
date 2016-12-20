﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace DDI.Data.Models.Common
{
    [Table("CityName")]
    public class CityName : BaseEntity
    {
        #region Public Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }

        public string Description { get; set; }

        public bool IsPreferred { get; set; }

        public Guid? CityId { get; set; }

        // Navigation Properties

        public City City { get; set; }
        
        #endregion

        #region Public Methods

        public override string DisplayName
        {
            get
            {
                return Description;
            }
        }

        #endregion

    }
}
