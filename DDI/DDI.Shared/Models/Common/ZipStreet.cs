using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Common
{
    [Table("ZipStreet")]
    public class ZipStreet : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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

        public override string DisplayName => Street;

    }
}
