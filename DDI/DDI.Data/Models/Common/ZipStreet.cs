﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace DDI.Data.Models.Common
{
    [Table("ZipStreet")]
    public class ZipStreet : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }

        [MaxLength(8)]
        public string Prefix { get; set; }

        public string Street { get; set; }

        [MaxLength(8)]
        public string Suffix { get; set; }

        [MaxLength(8)]
        public string Suffix2 { get; set; }

        [MaxLength(8)]
        public string UrbanizationKey { get; set; }

        [Index]
        [MaxLength(8)]
        public string CityKey { get; set; }    

        public Guid? ZipId { get; set; }

        // Navigation Properties

        public Zip Zip { get; set; }

        public ICollection<ZipPlus4> ZipPlus4s { get; set; }

    }
}
