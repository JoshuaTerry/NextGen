using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Common
{
    [Table("ZipBranch")]
    public class ZipBranch : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public string Description { get; set; }

        [Index]
        [MaxLength(8)]
        public string USPSKey { get; set; }

        [MaxLength(2)]
        public string FacilityCode { get; set; }

        public bool IsPreferred { get; set; }

        public Guid? ZipId { get; set; }

        // Navigation Properties

        public Zip Zip { get; set; }
        
    }
}
