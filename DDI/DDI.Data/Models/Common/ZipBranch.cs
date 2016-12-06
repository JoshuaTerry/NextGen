using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace DDI.Data.Models.Common
{
    [Table("ZipBranch")]
    public class ZipBranch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Description { get; set; }

        [MaxLength(8)]
        public string USPSKey { get; set; }

        [MaxLength(2)]
        public string FacilityCode { get; set; }

        public bool IsPreferred { get; set; }

        public Guid? ZipId { get; set; }

        // Navigation Properties

        public virtual Zip Zip { get; set; }

    }
}
