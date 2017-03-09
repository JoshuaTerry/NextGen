using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

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
