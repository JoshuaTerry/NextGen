using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DDI.Data.Enums;

namespace DDI.Data.Models.Client
{
    [Table("ConstituentAddress")]
    public class ConstituentAddress : BaseEntity
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }

        public Guid? AddressId { get; set; }

        public Guid? ConstituentId { get; set; }

        public bool IsPrimary { get; set; }

        [MaxLength(255)]
        public string Comment { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public int StartDay { get; set; }

        public int EndDay { get; set; }

        public ResidentType ResidentType { get; set; }

        public Guid? AddressTypeId { get; set; }
        
        [MaxLength(128)]
        [Index]
        public string DuplicateKey { get; set; }

        // Navigation Properties
        public Address Address { get; set; }

        public Constituent Constituent { get; set; }

        public AddressType AddressType { get; set; }

        #endregion Public Properties
    }
}
