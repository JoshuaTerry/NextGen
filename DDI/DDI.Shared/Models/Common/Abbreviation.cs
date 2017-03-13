using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;
using System.Text;

namespace DDI.Shared.Models.Common
{
    [Table("Abbreviation")]
    public class Abbreviation : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(32)]
        public string Word { get; set; }

        [MaxLength(32)]
        public string USPSAbbreviation { get; set; }

        [MaxLength(32)]
        public string AddressWord { get; set; }

        [MaxLength(32)]
        public string NameWord { get; set; }

        public bool IsSuffix { get; set; }

        public bool IsCaps { get; set; }

        public bool IsSecondary { get; set; }

        public int Priority { get; set; }


        [NotMapped]
        public override string CreatedBy
        {
            get
            {
                return base.CreatedBy;
            }

            set
            {
                base.CreatedBy = value;
            }
        }

        [NotMapped]
        public override DateTime? CreatedOn
        {
            get
            {
                return base.CreatedOn;
            }

            set
            {
                base.CreatedOn = value;
            }
        }

        [NotMapped]
        public override string LastModifiedBy
        {
            get
            {
                return base.LastModifiedBy;
            }

            set
            {
                base.LastModifiedBy = value;
            }
        }

        [NotMapped]
        public override DateTime? LastModifiedOn
        {
            get
            {
                return base.LastModifiedOn;
            }

            set
            {
                base.LastModifiedOn = value;
            }
        }
    }
}
